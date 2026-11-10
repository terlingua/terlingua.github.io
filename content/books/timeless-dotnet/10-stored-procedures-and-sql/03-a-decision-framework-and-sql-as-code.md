---
title: A decision framework, and SQL as code
questions:
  - id: first-question
    prompt: When deciding between a procedure and inline parameterized SQL, which question should come first?
    options:
      - id: who-owns
        text: Who owns this database, and how many applications use it?
        correct: true
      - text: Which is faster?
      - text: Which one the senior developer prefers
      - text: Which one has fewer lines
    explanation: Ownership and integration shape almost every other trade-off. Performance is usually equivalent.
  - id: versioning
    prompt: How should stored procedures and schema changes be managed in a long-lived system?
    type: multiple
    options:
      - id: source-control
        text: In source control, next to the application code
        correct: true
      - id: migrations
        text: Deployed by an automated migration tool (DbUp, SSDT dacpacs, Flyway, EF Core migrations) in CI/CD
        correct: true
      - id: reviewed
        text: Reviewed and tested like any other code
        correct: true
      - text: Edited directly in production through SSMS when needed
    explanation: A procedure edited by hand in production is code without history, review or tests. That, more than procedures themselves, is why they earned a bad reputation.
  - id: expand-contract
    prompt: An application must change a procedure's parameters. Old and new application versions run side by side during a rolling deployment. What pattern avoids breaking the old version?
    options:
      - text: Alter the procedure in place before deploying
      - id: expand-contract
        text: "Expand/contract — add a new procedure (or optional parameters) first, deploy apps that use it, then remove the old one later"
        correct: true
      - text: Stop all servers during deployment
      - text: Use `nolock` during deployment
    explanation: Any shared contract — API, schema or procedure — must be evolved compatibly when old and new code coexist.
  - id: sql-location
    prompt: Where should inline SQL live in a .NET codebase?
    options:
      - text: Concatenated across several methods for reuse
      - id: near-use
        text: Close to the code that uses it, as raw string literal constants (or `.sql` embedded resources), with explicit column lists
        correct: true
      - text: In `appsettings.json` so it can be changed without a deploy
      - text: In a database table, loaded at runtime
    explanation: SQL is code. Keep it readable, searchable, reviewable and tested — the same standard as C#.
  - id: dynamic-in-proc
    prompt: "A search procedure builds `@sql = N'select ... where 1=1' + case when @name is not null then N' and Name like ''%' + @name + '%''' end` and runs `exec(@sql)`. What is wrong?"
    options:
      - text: Nothing, procedures are always safe
      - id: injectable
        text: It concatenates input into SQL, so it is injectable; dynamic SQL in procedures must use `sp_executesql` with parameters
        correct: true
      - text: "`where 1=1` is invalid T-SQL"
      - text: "`exec` is deprecated"
    explanation: The same rule applies in every language — build the *shape* dynamically if you must, but pass *values* as parameters.
  - id: test-procs
    prompt: How can you test stored procedures?
    type: multiple
    options:
      - id: integration
        text: Integration tests from .NET against a real engine (Testcontainers), calling the procedure and asserting on results
        correct: true
      - id: tsqlt
        text: T-SQL unit testing frameworks such as tSQLt
        correct: true
      - text: Mock `IDbConnection` in C# and assert that `ExecuteAsync` was called
      - id: query-store
        text: Monitor them in production with Query Store for regressions — as a complement, not a substitute
        correct: true
    explanation: Mocking the connection tests that you called a method, not that the SQL is correct.
---
## Learning objectives

- Apply a short decision framework for procedures versus inline SQL
- Manage SQL in source control with automated, compatible deployments
- Test database code

## The framework

```text
1. Does more than one application or language need this exact logic against a shared database?
      yes → procedure is a strong candidate
2. Is there a hard least-privilege requirement (EXECUTE-only access)?
      yes → procedure
3. Is it set-based work over large data that would otherwise cross the network?
      yes → server-side SQL: a procedure or a single parameterized batch, decided by ownership
4. Is it In-Memory OLTP needing native compilation?
      yes → natively compiled procedure
5. Otherwise (single owner app, business logic, dynamic queries, CRUD)
      → parameterized inline SQL (Dapper) or an ORM (EF Core)
```

Write the reason down in an architecture decision record. The reason is what future developers
need to tell a deliberate choice from a vestige.

## SQL as code

```text
db/
  migrations/
    0001_create_orders.sql
    0002_add_order_status_index.sql
    0003_create_upsert_prices_proc.sql
```

```csharp
var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(typeof(Migrations).Assembly)
    .WithTransactionPerScript()
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful) throw result.Error;
```

The same scripts run in CI against a container, in staging, and in production.

## Dynamic SQL, safely, in T-SQL

```sql
create or alter procedure dbo.SearchCustomers
    @name nvarchar(100) = null,
    @city nvarchar(100) = null
as
begin
    set nocount on;
    declare @sql nvarchar(max) = N'select Id, Name, City from dbo.Customers where 1 = 1';
    if @name is not null set @sql += N' and Name like @name + N''%''';
    if @city is not null set @sql += N' and City = @city';
    exec sp_executesql @sql, N'@name nvarchar(100), @city nvarchar(100)', @name = @name, @city = @city;
end
```

And the equivalent in C#, which many teams find easier to maintain:

```csharp
var sql = new StringBuilder("select Id, Name, City from dbo.Customers where 1 = 1");
var parameters = new DynamicParameters();
if (name is not null) { sql.Append(" and Name like @name"); parameters.Add("name", name + "%"); }
if (city is not null) { sql.Append(" and City = @city"); parameters.Add("city", city); }

var rows = await connection.QueryAsync<CustomerRow>(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));
```

Each distinct filter combination produces one cached plan — a small, bounded number.

## Testing a procedure from .NET

```csharp
[Fact]
public async Task UpsertPrices_updates_changed_and_inserts_new()
{
    var ct = TestContext.Current.CancellationToken;
    await using var connection = await _db.DataSource.OpenConnectionAsync(ct);
    await connection.ExecuteAsync("insert dbo.Prices (Sku, Price, UpdatedAt) values ('A', 10, sysutcdatetime())");

    var table = PriceTable(("A", 12m), ("B", 5m));
    await connection.ExecuteAsync(new CommandDefinition("dbo.UpsertPrices",
        new { prices = table.AsTableValuedParameter("dbo.PriceList") },
        commandType: CommandType.StoredProcedure, cancellationToken: ct));

    var prices = (await connection.QueryAsync<(string Sku, decimal Price)>("select Sku, Price from dbo.Prices order by Sku")).ToList();
    Assert.Equal([("A", 12m), ("B", 5m)], prices);
}
```

## Sources

- DbUp — <https://dbup.github.io/>
- Microsoft Learn, "SQL Server Data Tools (SSDT)" — <https://learn.microsoft.com/en-us/sql/ssdt/sql-server-data-tools>
- tSQLt — <https://tsqlt.org/>
- Martin Fowler, "Parallel Change" (expand/contract) — <https://martinfowler.com/bliki/ParallelChange.html>
- Microsoft Learn, "Architecture decision records" — <https://learn.microsoft.com/en-us/azure/well-architected/architect-role/architecture-decision-record>
