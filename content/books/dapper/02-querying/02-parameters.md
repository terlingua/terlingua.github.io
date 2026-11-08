---
title: Parameters
questions:
  - id: in-list
    prompt: "How do you query products whose IDs are in a C# list `ids` with Dapper on SQL Server?"
    options:
      - text: "`WHERE Id IN (\" + string.Join(\",\", ids) + \")\"`"
      - text: It is not possible with Dapper
      - text: "`WHERE Id IN (@Ids)` with `new { Ids = string.Join(\",\", ids) }`"
      - id: expansion
        text: "`WHERE Id IN @Ids` with `new { Ids = ids }`"
        correct: true
    explanation: Dapper expands an enumerable parameter into `(@Ids1, @Ids2, ...)` automatically. Do not add your own parentheses.
  - id: output
    prompt: How do you read an output parameter from a stored procedure?
    options:
      - text: Output parameters are not supported
      - id: dynamic
        text: "Add it to a `DynamicParameters` with `direction: ParameterDirection.Output`, execute, then call `Get<T>(\"name\")`"
        correct: true
      - text: Read it from the returned rows
    explanation: "`DynamicParameters` supports input, output, input-output and return-value parameters."
  - id: dbstring
    prompt: Why use `DbString { Value = code, IsAnsi = true, Length = 10 }` for a `VARCHAR(10)` column on SQL Server?
    options:
      - id: ansi
        text: By default .NET strings are sent as `NVARCHAR`, which can cause implicit conversions that prevent index seeks on `VARCHAR` columns
        correct: true
      - text: To encrypt the value
      - text: Because Dapper cannot send strings otherwise
    explanation: Matching the parameter type to the column type keeps queries sargable and plans stable.
  - id: injection
    prompt: Which practices prevent SQL injection when using Dapper?
    type: multiple
    options:
      - text: Pass all values as parameters
        correct: true
      - text: When a column or sort direction must vary, choose it from a fixed allow-list in code
        correct: true
      - text: Use string interpolation to insert user input into the SQL text
      - text: Never concatenate untrusted input into SQL
        correct: true
    explanation: Parameters cannot be used for identifiers like column names, so map user choices to known-safe SQL fragments.
---
## Learning objectives

- Pass parameters with anonymous objects and `DynamicParameters`
- Use list expansion, output parameters and `DbString`
- Keep dynamic SQL safe

## Anonymous objects

```csharp
var results = await db.QueryAsync<Product>(
    "SELECT Id, Name, Price FROM Products WHERE Category = @category AND Price BETWEEN @min AND @max",
    new { category, min = 10m, max = 50m });
```

## Lists

```csharp
int[] ids = [3, 7, 42];
var products = await db.QueryAsync<Product>(
    "SELECT Id, Name, Price FROM Products WHERE Id IN @ids", new { ids });
// Sent as: WHERE Id IN (@ids1, @ids2, @ids3)
```

On PostgreSQL with Npgsql, arrays are native, so the idiomatic form is `WHERE id = ANY(@ids)`.
For very large lists on SQL Server, use a table-valued parameter instead (SQL Server allows at most
2,100 parameters per command).

## DynamicParameters

```csharp
var p = new DynamicParameters();
p.Add("@Name", product.Name);
p.Add("@Price", product.Price, DbType.Decimal, precision: 10, scale: 2);
p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

await db.ExecuteAsync("dbo.CreateProduct", p, commandType: CommandType.StoredProcedure);
int newId = p.Get<int>("@NewId");
```

`DynamicParameters` is also handy when the set of parameters is built up conditionally.

## Matching string types

```csharp
var order = await db.QuerySingleOrDefaultAsync<Order>(
    "SELECT Id, Code, Total FROM Orders WHERE Code = @code",
    new { code = new DbString { Value = code, IsAnsi = true, Length = 10, IsFixedLength = false } });
```

## Dynamic SQL, safely

```csharp
private static readonly Dictionary<string, string> SortColumns = new(StringComparer.OrdinalIgnoreCase)
{
    ["name"] = "Name",
    ["price"] = "Price",
    ["newest"] = "CreatedAt"
};

public Task<IEnumerable<Product>> ListAsync(string sort, bool descending)
{
    var column = SortColumns.GetValueOrDefault(sort, "Name");
    var direction = descending ? "DESC" : "ASC";
    return _db.QueryAsync<Product>($"SELECT Id, Name, Price FROM Products ORDER BY {column} {direction}");
}
```

The interpolated values come only from constants in code, never from the user. Everything the user
supplies goes through parameters.
