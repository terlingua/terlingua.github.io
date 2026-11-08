---
title: Setup and first queries
questions:
  - id: package
    prompt: Which NuGet packages do you need to use Dapper with SQL Server?
    options:
      - text: Only `Dapper`
      - text: "`Dapper.SqlServer` only"
      - text: "`EntityFrameworkCore.Dapper`"
      - id: both
        text: "`Dapper` and an ADO.NET provider such as `Microsoft.Data.SqlClient`"
        correct: true
    explanation: Dapper is provider-agnostic; the provider package supplies the connection type.
  - id: open
    prompt: You pass a closed `SqlConnection` to `QueryAsync`. What happens?
    options:
      - text: An exception is thrown because the connection is closed
      - id: auto
        text: Dapper opens the connection, runs the query and closes it again
        correct: true
      - text: Dapper creates a second connection
    explanation: Dapper restores the connection's original state. If you open it yourself, it stays open.
  - id: dispose
    prompt: Why wrap connections in `await using`?
    options:
      - text: It deletes the database
      - text: Dapper requires it to compile
      - id: pool
        text: Disposing returns the underlying physical connection to the connection pool
        correct: true
    explanation: Connections are pooled. Disposing promptly prevents pool exhaustion under load.
---
## Learning objectives

- Install Dapper and a provider
- Run queries and map results to classes and records
- Manage connection lifetime correctly

## Install

```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.SqlClient   # or Npgsql, MySqlConnector, Microsoft.Data.Sqlite
```

`System.Data.SqlClient` is deprecated; use `Microsoft.Data.SqlClient` for SQL Server.

## A sample table

```sql
CREATE TABLE Products (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
```

## First queries

```csharp
using Dapper;
using Microsoft.Data.SqlClient;

public sealed class Product
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Category { get; init; } = "";
    public decimal Price { get; init; }
}

await using var connection = new SqlConnection(connectionString);

IEnumerable<Product> books = await connection.QueryAsync<Product>(
    "SELECT Id, Name, Category, Price FROM Products WHERE Category = @Category ORDER BY Name",
    new { Category = "Books" });

int count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Products");
```

`@Category` in the SQL matches the `Category` property of the anonymous object. Values are sent as
real parameters, never concatenated into the SQL text.

## Mapping to records

```csharp
public sealed record ProductSummary(int Id, string Name, decimal Price);

var summaries = await connection.QueryAsync<ProductSummary>(
    "SELECT Id, Name, Price FROM Products");
```

For constructor mapping, column names must match the parameter names (case-insensitively) and the
column types must be compatible with the parameter types — for example, a `BIGINT` column will not
map to an `int` parameter. Explicit casts or aliases in SQL fix mismatches.

## Async and cancellation

Use the `...Async` methods in server applications. To pass a cancellation token, use a
`CommandDefinition`:

```csharp
var command = new CommandDefinition(
    "SELECT Id, Name, Price FROM Products WHERE Price > @Min",
    new { Min = 10m },
    cancellationToken: ct);

var expensive = await connection.QueryAsync<ProductSummary>(command);
```
