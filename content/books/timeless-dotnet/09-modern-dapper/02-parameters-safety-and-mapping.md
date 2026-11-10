---
title: Parameters, safety and mapping
questions:
  - id: injection
    prompt: "Which line is vulnerable to SQL injection?"
    options:
      - text: "`connection.QueryAsync<User>(\"select * from Users where Name = @name\", new { name })`"
      - text: "`connection.QueryAsync<User>(new CommandDefinition(\"select * from Users where Name = @name\", new { name }, cancellationToken: ct))`"
      - text: None of them
      - id: interpolated
        text: "`connection.QueryAsync<User>($\"select * from Users where Name = '{name}'\")`"
        correct: true
    explanation: Dapper parameterizes values you pass as parameters. It cannot protect values you paste into the SQL string yourself. An interpolated string passed to Dapper's `string sql` parameter is just a string.
  - id: list-expansion
    prompt: "What does Dapper do with `where Id in @ids` when `ids` is an `int[]` of three values?"
    options:
      - text: Sends the array as a comma-separated string
      - text: Throws; arrays are not supported
      - text: Uses only the first element
      - id: expands
        text: Expands it to `in (@ids1, @ids2, @ids3)` with three parameters
        correct: true
    explanation: Convenient for small lists. For hundreds or thousands of values, use a table-valued parameter (SQL Server), `= any(@ids)` (PostgreSQL), or a JSON parameter, to avoid parameter limits and plan-cache pollution.
  - id: dbstring
    prompt: Why would you pass a `DbString { Value = code, IsAnsi = true, Length = 10 }` instead of a plain `string` to SQL Server?
    type: multiple
    options:
      - id: varchar
        text: To send `varchar` instead of `nvarchar` when the column is `varchar`, avoiding an implicit conversion that can prevent index seeks
        correct: true
      - id: length
        text: To give the parameter a fixed length so query plans are not cached per distinct string length
        correct: true
      - text: To encrypt the value
      - text: Because plain strings are not parameterized
    explanation: An `nvarchar` parameter compared to a `varchar` column forces conversion of the column, which can turn a seek into a scan on large tables.
  - id: dynamic-columns
    prompt: A user can choose the sort column for a grid. How should the column name get into the SQL?
    options:
      - id: allowlist
        text: Map the user's choice through an allow-list (`switch`) to a known column name constant, and never interpolate raw input
        correct: true
      - text: As a Dapper parameter `order by @column`
      - text: Interpolate it after `.Replace("'", "''")`
      - text: Use a stored procedure, which makes injection impossible
    explanation: Identifiers cannot be parameters — `order by @column` sorts by a constant. Escaping is fragile. An allow-list is safe and simple. Stored procedures that build dynamic SQL from input are just as injectable.
  - id: multi-mapping
    prompt: "What does `splitOn` do in multi-mapping, as in `QueryAsync<Order, Customer, Order>(sql, map, splitOn: \"CustomerId\")`?"
    options:
      - text: Splits the SQL into two batches
      - text: Splits results across two threads
      - text: Names the foreign key constraint
      - id: boundary
        text: Tells Dapper at which column the second object's columns begin
        correct: true
    explanation: The default split column is `Id`. Use explicit column aliases to make boundaries obvious.
  - id: select-star
    prompt: Why does Dapper.AOT's analyzer warn about `select *` (DAP219)?
    options:
      - text: "`select *` is slower to parse"
      - text: Dapper cannot map `select *`
      - text: It is a SQL Server syntax error
      - id: explicit-columns
        text: Explicit columns make the result contract visible, avoid fetching unneeded data, and stop schema changes from silently changing results
        correct: true
    explanation: A column added to the table later — perhaps a large blob — is suddenly fetched by every query. Name what you need.
  - id: tvp
    prompt: When is a table-valued parameter (TVP) the right tool with SQL Server?
    options:
      - text: Passing a single integer
      - text: Returning results
      - text: Never; TVPs are deprecated
      - id: sets
        text: Passing a set of rows (thousands of ids, or rows to upsert) into one statement or procedure
        correct: true
    explanation: TVPs make set-based operations from .NET efficient. Dapper supports them via `DataTable.AsTableValuedParameter("dbo.IdList")` or a custom `SqlMapper.ICustomQueryParameter`.
---
## Learning objectives

- Parameterize every value, and handle identifiers safely
- Pass lists, ANSI strings and table-valued parameters correctly
- Map joins and multiple result sets

## The one rule

Values go in parameters. Always. Not "unless it is an int", not "unless it came from our own
dropdown".

```csharp
var users = await connection.QueryAsync<UserRow>(new CommandDefinition(
    "select Id, Name, Email from dbo.Users where TenantId = @tenantId and Name like @prefix",
    new { tenantId, prefix = name + "%" },
    cancellationToken: ct));
```

## Identifiers: allow-list, never interpolate input

```csharp
public static string SortColumn(string requested) => requested switch
{
    "name" => "Name",
    "created" => "CreatedAt",
    "total" => "LifetimeTotal",
    _ => "Name",
};

var sql = $"""
    select Id, Name, CreatedAt, LifetimeTotal
    from dbo.Customers
    order by {SortColumn(sort)} {(descending ? "desc" : "asc")}
    offset @skip rows fetch next @take rows only
    """;
```

The interpolation inserts only constants chosen by your code.

## Lists

```csharp
var orders = await connection.QueryAsync<OrderRow>(new CommandDefinition(
    "select Id, Status, Total from dbo.Orders where Id in @ids",
    new { ids },
    cancellationToken: ct));
```

For large sets on SQL Server, a TVP:

```sql
create type dbo.IdList as table (Id int primary key);
```

```csharp
var table = new DataTable();
table.Columns.Add("Id", typeof(int));
foreach (var id in ids) table.Rows.Add(id);

var rows = await connection.QueryAsync<OrderRow>(new CommandDefinition(
    "select o.Id, o.Status, o.Total from dbo.Orders o join @ids i on i.Id = o.Id",
    new { ids = table.AsTableValuedParameter("dbo.IdList") },
    cancellationToken: ct));
```

On PostgreSQL with Npgsql, arrays are native: `where id = any(@ids)`.

## ANSI strings and fixed lengths

```csharp
var product = await connection.QuerySingleOrDefaultAsync<ProductRow>(new CommandDefinition(
    "select Sku, Name, Price from dbo.Products where Sku = @sku",
    new { sku = new DbString { Value = sku, IsAnsi = true, Length = 20, IsFixedLength = false } },
    cancellationToken: ct));
```

Real-world scenario: a `varchar(20)` SKU column with 40 million rows. Passing a plain `string`
sent `nvarchar(4000)`; the implicit conversion caused an index scan, 900 ms per lookup.
`DbString` with `IsAnsi = true` restored the seek: 2 ms.

## Joins with multi-mapping

```csharp
const string sql = """
    select o.Id, o.PlacedAt, o.Total,
           c.Id as CustomerId, c.Name, c.Email
    from dbo.Orders o
    join dbo.Customers c on c.Id = o.CustomerId
    where o.PlacedAt >= @from
    """;

var orders = await connection.QueryAsync<OrderView, CustomerView, OrderView>(
    new CommandDefinition(sql, new { from }, cancellationToken: ct),
    (order, customer) => order with { Customer = customer },
    splitOn: "CustomerId");
```

## Several result sets in one round trip

```csharp
using var grid = await connection.QueryMultipleAsync(new CommandDefinition("""
    select Id, Name from dbo.Customers where Id = @id;
    select Id, PlacedAt, Total from dbo.Orders where CustomerId = @id order by PlacedAt desc;
    """, new { id }, cancellationToken: ct));

var customer = await grid.ReadSingleAsync<CustomerView>();
var orders = (await grid.ReadAsync<OrderSummaryRow>()).AsList();
```

## Sources

- Dapper README (parameters, list support, multi-mapping) — <https://github.com/DapperLib/Dapper>
- Dapper.AOT rule DAP219 — <https://aot.dapperlib.dev/rules/DAP219>
- OWASP, "SQL Injection Prevention Cheat Sheet" — <https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html>
- Microsoft Learn, "Table-valued parameters" — <https://learn.microsoft.com/en-us/sql/relational-databases/tables/use-table-valued-parameters-database-engine>
