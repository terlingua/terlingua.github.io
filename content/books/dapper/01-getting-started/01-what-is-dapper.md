---
title: What Dapper is and when to use it
questions:
  - id: what
    prompt: What is Dapper?
    options:
      - id: micro-orm
        text: A micro-ORM that adds extension methods to ADO.NET connections for executing SQL and mapping results to objects
        correct: true
      - text: A full ORM with change tracking and migrations
      - text: A database engine
      - text: A replacement for ADO.NET providers such as Microsoft.Data.SqlClient
    explanation: Dapper sits on top of any ADO.NET provider. You write the SQL; Dapper handles parameters and mapping.
  - id: origin
    prompt: Where did Dapper originate?
    options:
      - text: It is part of Entity Framework Core
      - text: It was released by Oracle
      - id: stack-overflow
        text: It was built by the Stack Overflow team to make their data access fast
        correct: true
    explanation: Dapper was created at Stack Overflow and is open source under the Apache 2.0 licence.
  - id: when
    prompt: In which situations is Dapper a particularly good fit?
    type: multiple
    options:
      - text: Read-heavy endpoints where you want precise control over SQL
        correct: true
      - text: Existing databases with complex queries, views or stored procedures
        correct: true
      - text: The read side of a CQRS design, alongside EF Core for writes
        correct: true
      - text: When you want the library to generate and apply schema migrations
    explanation: Dapper has no migrations or change tracking; pair it with a migration tool or EF Core when you need those.
---
## Learning objectives

- Describe what Dapper does and does not do
- Compare Dapper with raw ADO.NET and EF Core
- Decide when Dapper is the right tool

## The problem Dapper solves

Raw ADO.NET is fast but verbose:

```csharp
await using var command = new SqlCommand("SELECT Id, Name, Price FROM Products WHERE Id = @Id", connection);
command.Parameters.AddWithValue("@Id", id);
await using var reader = await command.ExecuteReaderAsync(ct);
Product? product = null;
if (await reader.ReadAsync(ct))
{
    product = new Product
    {
        Id = reader.GetInt32(0),
        Name = reader.GetString(1),
        Price = reader.GetDecimal(2)
    };
}
```

With Dapper:

```csharp
var product = await connection.QuerySingleOrDefaultAsync<Product>(
    "SELECT Id, Name, Price FROM Products WHERE Id = @Id", new { Id = id });
```

Same SQL, same provider, same performance characteristics — without the boilerplate.

## Dapper versus EF Core

| | Dapper | EF Core |
|-|--------|---------|
| You write | SQL | LINQ (SQL generated) |
| Change tracking | No | Yes |
| Migrations | No | Yes |
| Relationships | Manual (multi-mapping) | Navigation properties |
| Overhead | Very low | Low to moderate |
| Learning curve | SQL knowledge | EF concepts + SQL knowledge |

They are not rivals. Many teams use EF Core for writes and aggregates, and Dapper for hot read
paths and reporting queries.

## What Dapper does

- Opens and closes connections when needed
- Converts anonymous objects to command parameters
- Maps result columns to properties or constructor parameters by name
- Caches mapping information so repeated queries are fast

## What it does not do

No change tracking, no identity map, no lazy loading, no LINQ, no schema migrations. You own the
SQL — which is precisely the point.
