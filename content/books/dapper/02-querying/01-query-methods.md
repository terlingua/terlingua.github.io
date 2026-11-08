---
title: Choosing the right query method
questions:
  - id: single-vs-first
    prompt: A query returns two rows. Which method throws an exception?
    options:
      - text: "`QueryFirst`"
      - id: single
        text: "`QuerySingle`"
        correct: true
      - text: "`QueryFirstOrDefault`"
      - text: "`Query`"
    explanation: "`QuerySingle` and `QuerySingleOrDefault` require at most one row (exactly one for `QuerySingle`). The `First` variants take the first row and ignore the rest."
  - id: no-rows
    prompt: A query by primary key returns no rows. Which method returns `null` (for a reference type) without throwing?
    options:
      - text: "`QuerySingle`"
      - id: single-or-default
        text: "`QuerySingleOrDefault`"
        correct: true
      - text: "`QueryFirst`"
    explanation: The `OrDefault` variants return `default(T)` when there are no rows.
  - id: execute
    prompt: Which statements about `Execute` and `ExecuteScalar` are true?
    type: multiple
    options:
      - text: "`Execute` returns the number of rows affected"
        correct: true
      - text: "`ExecuteScalar<T>` returns the first column of the first row"
        correct: true
      - text: "`Execute` is used for INSERT, UPDATE and DELETE statements"
        correct: true
      - text: "`ExecuteScalar<T>` returns every row as a list"
    explanation: Use `Query` methods for result sets, `Execute` for commands and `ExecuteScalar` for single values.
---
## Learning objectives

- Pick the query method that matches the expected number of rows
- Use `Execute` and `ExecuteScalar` appropriately

## The family

| Method | 0 rows | 1 row | 2+ rows |
|--------|--------|-------|---------|
| `Query<T>` | Empty | One item | All items |
| `QueryFirst<T>` | Throws | Item | First item |
| `QueryFirstOrDefault<T>` | `default` | Item | First item |
| `QuerySingle<T>` | Throws | Item | Throws |
| `QuerySingleOrDefault<T>` | `default` | Item | Throws |

All have `Async` versions. Choose the one that **expresses your expectation** — the exceptions are
a feature, catching data bugs early.

```csharp
// Exactly one row expected; zero or many is a bug
var order = await db.QuerySingleAsync<Order>("SELECT * FROM Orders WHERE Id = @id", new { id });

// Optional lookup
var user = await db.QuerySingleOrDefaultAsync<User>(
    "SELECT * FROM Users WHERE Email = @email", new { email });

// Newest item, possibly none
var latest = await db.QueryFirstOrDefaultAsync<Order>(
    "SELECT TOP (1) * FROM Orders WHERE CustomerId = @customerId ORDER BY CreatedAt DESC",
    new { customerId });
```

When using `First` variants, limit rows in SQL (`TOP (1)`, `LIMIT 1`) so the database does not
send rows you will discard.

## Commands and scalars

```csharp
int affected = await db.ExecuteAsync(
    "UPDATE Products SET Price = Price * 1.05 WHERE Category = @category", new { category });

int newId = await db.ExecuteScalarAsync<int>(
    """
    INSERT INTO Products (Name, Category, Price)
    OUTPUT INSERTED.Id
    VALUES (@Name, @Category, @Price)
    """,
    product);
```

Raw string literals (`"""`) keep multi-line SQL readable. Passing `product` as the parameter object
maps its `Name`, `Category` and `Price` properties to the matching `@` parameters.

## Avoid SELECT * in production

The snippets above use `SELECT *` for brevity. In real code, list columns explicitly: the query
documents its shape, the database does less work, and adding a column to the table cannot break
constructor mapping.
