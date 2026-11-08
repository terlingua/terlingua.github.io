---
title: Inserts, updates and batches
questions:
  - id: enumerable
    prompt: "What does `connection.ExecuteAsync(\"INSERT INTO Tags (Name) VALUES (@Name)\", tags)` do when `tags` is a list of 1,000 objects?"
    options:
      - text: Sends one multi-row INSERT statement
      - text: Throws because a list is not a valid parameter
      - text: Inserts only the first item
      - id: per-item
        text: Executes the command once per item — 1,000 executions
        correct: true
    explanation: Dapper iterates the sequence and executes the command for each element. It is convenient, but it is not a bulk insert.
  - id: bulk
    prompt: For loading 500,000 rows into SQL Server as quickly as possible, which approach is best?
    options:
      - text: "`ExecuteAsync` with a list parameter"
      - id: sqlbulkcopy
        text: "`SqlBulkCopy` (or a provider-specific bulk API such as Npgsql's binary COPY)"
        correct: true
      - text: One `ExecuteAsync` call per row inside a loop with a new connection each time
    explanation: Bulk APIs stream rows using the database's bulk-load protocol and are orders of magnitude faster.
  - id: identity
    prompt: Which are valid ways to get the generated key after an insert on SQL Server?
    type: multiple
    options:
      - text: "`OUTPUT INSERTED.Id` with `ExecuteScalarAsync<int>`"
        correct: true
      - text: "`SELECT CAST(SCOPE_IDENTITY() AS int)` in the same batch with `ExecuteScalarAsync<int>`"
        correct: true
      - text: "`SELECT MAX(Id) FROM Products` in a separate query"
    explanation: "`MAX(Id)` is wrong under concurrency — another insert can happen in between."
---
## Learning objectives

- Insert and update rows and read generated keys
- Understand how list parameters execute
- Choose bulk APIs for large loads

## Insert and get the key

```csharp
const string insert = """
    INSERT INTO Products (Name, Category, Price)
    OUTPUT INSERTED.Id
    VALUES (@Name, @Category, @Price);
    """;

var id = await db.ExecuteScalarAsync<int>(insert, new { product.Name, product.Category, product.Price });
```

PostgreSQL uses `RETURNING id`; SQLite and others have their own equivalents.

## Update with optimistic concurrency

```csharp
const string update = """
    UPDATE Products
    SET Price = @Price
    WHERE Id = @Id AND RowVersion = @RowVersion;
    """;

var affected = await db.ExecuteAsync(update, new { product.Id, product.Price, product.RowVersion });
if (affected == 0) throw new ConcurrencyException(product.Id);
```

`Execute` returns rows affected, which makes optimistic concurrency checks simple.

## List parameters

```csharp
var tags = new[] { new { Name = "dotnet" }, new { Name = "sql" } };
await db.ExecuteAsync("INSERT INTO Tags (Name) VALUES (@Name)", tags);
```

This runs the INSERT once per element. For a handful of rows inside a transaction it is perfectly
fine; for thousands, use a bulk API.

## Bulk loading

```csharp
using var bulk = new SqlBulkCopy(connection) { DestinationTableName = "dbo.Products", BatchSize = 5_000 };
bulk.ColumnMappings.Add(nameof(Product.Name), "Name");
bulk.ColumnMappings.Add(nameof(Product.Category), "Category");
bulk.ColumnMappings.Add(nameof(Product.Price), "Price");
await bulk.WriteToServerAsync(productsDataTable, ct);
```

`SqlBulkCopy` lives in `Microsoft.Data.SqlClient`. Dapper is not involved — use the right tool.

## Upserts

```sql
-- PostgreSQL
INSERT INTO product_stock (sku, quantity) VALUES (@Sku, @Quantity)
ON CONFLICT (sku) DO UPDATE SET quantity = EXCLUDED.quantity;
```

On SQL Server, `MERGE` exists but has well-documented concurrency pitfalls; an `UPDATE` followed by
an `INSERT` when no row was affected, inside a transaction with appropriate locking hints, is a
common alternative.
