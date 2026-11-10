---
title: Dapper in the .NET 10 era
questions:
  - id: what-dapper
    prompt: What is Dapper?
    options:
      - text: A full ORM with change tracking and migrations
      - text: A SQL Server-only stored procedure generator
      - text: A replacement for ADO.NET providers
      - id: micro-orm
        text: A micro-ORM — extension methods on `DbConnection` that run your SQL and map results to objects
        correct: true
    explanation: Dapper sits on top of any ADO.NET provider (SqlClient, Npgsql, MySqlConnector, Microsoft.Data.Sqlite…). You write the SQL; Dapper handles parameters and materialization.
  - id: sqlclient
    prompt: Which SQL Server provider should new .NET 10 code use?
    options:
      - id: microsoft
        text: "`Microsoft.Data.SqlClient`"
        correct: true
      - text: "`System.Data.SqlClient`"
      - text: "`System.Data.OleDb`"
      - text: "`System.Data.Odbc` only"
    explanation: "`System.Data.SqlClient` is deprecated and receives no new features. `Microsoft.Data.SqlClient` is the actively developed provider (note its default `Encrypt=true` since v4)."
  - id: commanddefinition
    prompt: Why is `CommandDefinition` the preferred way to call Dapper's async methods in modern code?
    type: multiple
    options:
      - id: token
        text: It carries a `CancellationToken`, which the simple overloads do not accept
        correct: true
      - id: tx-timeout
        text: It bundles transaction, command timeout, command type and flags in one value
        correct: true
      - text: It is the only way to pass parameters
      - text: It disables SQL injection protection for speed
    explanation: Parameters can still be passed in the simple overloads; cancellation cannot.
  - id: buffered
    prompt: What does Dapper's default *buffered* mode mean for `QueryAsync<T>`?
    options:
      - text: Results are cached across calls
      - text: Rows are streamed lazily while you iterate
      - id: all-rows
        text: All rows are read into a `List<T>` before the method returns, and the reader is closed
        correct: true
      - text: Only the first 1,000 rows are returned
    explanation: Buffering is the right default for typical result sets — the connection is freed quickly. For very large results, stream instead.
  - id: unbuffered
    prompt: Which Dapper method streams rows asynchronously as `IAsyncEnumerable<T>`?
    options:
      - text: "`QueryFirstAsync<T>`"
      - id: qua
        text: "`QueryUnbufferedAsync<T>`"
        correct: true
      - text: "`ExecuteScalarAsync<T>`"
      - text: "`QueryMultipleAsync`"
    explanation: Use it with `await foreach` for exports or large reports. The connection stays busy until you finish enumerating.
  - id: dateonly
    prompt: What is the state of `DateOnly`/`TimeOnly` support in recent Dapper releases?
    options:
      - text: They have never been supported
      - text: They require Dapper.AOT
      - text: They only work with PostgreSQL
      - id: reenabled
        text: Support was added, temporarily disabled because of defects, and re-enabled in Dapper 2.1.86 (2025)
        correct: true
    explanation: Check release notes when relying on newer type support; on older versions a `TypeHandler` was the workaround.
  - id: single-variants
    prompt: Which method should you use when a query must return exactly one row, and more than one would be a bug?
    options:
      - text: "`QueryFirstOrDefaultAsync`"
      - text: "`QueryAsync` and take `.First()`"
      - text: "`ExecuteAsync`"
      - id: single
        text: "`QuerySingleAsync` (or `QuerySingleOrDefaultAsync` if zero rows is acceptable)"
        correct: true
    explanation: "`Single` variants throw on duplicates, turning a silent data bug into a visible one. `First` variants ignore extra rows."
---
## Learning objectives

- Set up Dapper with a modern provider and `DbDataSource`
- Use `CommandDefinition` with cancellation
- Choose buffered, unbuffered and single-row query methods

## Why Dapper is still a good fit

Dapper has been stable since 2011 because it does one thing: map SQL to objects with almost no
overhead. In 2025 it gained .NET 10 packaging, full strong-name signing, trusted publishing to
NuGet and re-enabled `DateOnly`/`TimeOnly`. It is a deliberately small dependency — exactly the
kind that ages well.

Use Dapper when you want the SQL to be visible and owned: reporting queries, performance-critical
reads, CQRS read models, or teams fluent in SQL. Use EF Core when you want change tracking,
migrations and LINQ for a rich domain. Many systems use both.

## Setup

```xml
<PackageReference Include="Dapper" />
<PackageReference Include="Microsoft.Data.SqlClient" />
```

```csharp
builder.Services.AddSingleton<DbDataSource>(_ =>
    SqlClientFactory.Instance.CreateDataSource(builder.Configuration.GetConnectionString("Shop")!));
```

`DbDataSource` (.NET 7) is the modern, provider-agnostic way to hand out connections. Npgsql's
`NpgsqlDataSource` adds connection-level configuration (type mappings, logging) on top.

## Queries with cancellation

```csharp
public sealed record ProductRow(string Sku, string Name, decimal Price, DateOnly AvailableFrom);

public sealed class ProductQueries(DbDataSource db)
{
    public async Task<IReadOnlyList<ProductRow>> SearchAsync(string term, int take, CancellationToken ct)
    {
        const string sql = """
            select top (@take) Sku, Name, Price, AvailableFrom
            from dbo.Products
            where Name like @pattern
            order by Name
            """;

        await using var connection = await db.OpenConnectionAsync(ct);
        var rows = await connection.QueryAsync<ProductRow>(new CommandDefinition(
            sql,
            new { take, pattern = $"%{term}%" },
            cancellationToken: ct));
        return rows.AsList();
    }

    public async Task<ProductRow?> FindAsync(string sku, CancellationToken ct)
    {
        await using var connection = await db.OpenConnectionAsync(ct);
        return await connection.QuerySingleOrDefaultAsync<ProductRow>(new CommandDefinition(
            "select Sku, Name, Price, AvailableFrom from dbo.Products where Sku = @sku",
            new { sku },
            cancellationToken: ct));
    }
}
```

Notes:

- Raw string literals (`"""`) keep SQL readable with no escaping.
- `like @pattern` is parameterized; only the *value* includes `%`.
- `AsList()` avoids a copy when Dapper already returned a `List<T>`.
- Positional records map by constructor parameter names.

## Streaming a large export

```csharp
await using var connection = await db.OpenConnectionAsync(ct);
var rows = connection.QueryUnbufferedAsync<OrderExportRow>(
    "select OrderId, PlacedAt, Total, CustomerEmail from dbo.Orders where PlacedAt >= @from",
    new { from });

await using var writer = new StreamWriter(output);
await foreach (var row in rows.WithCancellation(ct))
{
    await writer.WriteLineAsync($"{row.OrderId},{row.PlacedAt:O},{row.Total},{row.CustomerEmail}");
}
```

Memory stays flat regardless of row count. (`QueryUnbufferedAsync` takes no token directly; pass
it to the enumeration with `WithCancellation`.)

## Writes

```csharp
var affected = await connection.ExecuteAsync(new CommandDefinition(
    "update dbo.Orders set Status = @status, UpdatedAt = sysutcdatetime() where Id = @id and RowVersion = @rowVersion",
    new { id, status = "Shipped", rowVersion },
    cancellationToken: ct));

if (affected == 0) throw new DBConcurrencyException($"Order {id} was modified by someone else");
```

## Sources

- Dapper on GitHub — <https://github.com/DapperLib/Dapper>
- Dapper releases — <https://github.com/DapperLib/Dapper/releases>
- Microsoft Learn, `DbDataSource` — <https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdatasource>
- Microsoft Learn, "Introduction to Microsoft.Data.SqlClient" — <https://learn.microsoft.com/en-us/sql/connect/ado-net/introduction-microsoft-data-sqlclient-namespace>
