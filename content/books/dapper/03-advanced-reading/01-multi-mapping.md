---
title: Multi-mapping related objects
questions:
  - id: splaton
    prompt: What does the `splitOn` argument tell Dapper in a multi-mapping query?
    options:
      - text: Which table to join
      - text: How many rows to return
      - text: Which separator character to use for CSV output
      - id: boundary
        text: The column name where the next object's columns begin in each row
        correct: true
    explanation: Dapper reads columns left to right and starts a new object at each `splitOn` column. The default is `Id`.
  - id: one-to-many
    prompt: When multi-mapping a one-to-many join (orders with lines), why do you typically use a dictionary?
    options:
      - text: Dapper requires a dictionary parameter
      - text: To sort the results
      - id: dedupe
        text: Each order appears in several rows, so you look up the existing order and add each line to it
        correct: true
    explanation: The join repeats parent columns for every child row; the dictionary de-duplicates parents.
  - id: limits
    prompt: Which statements about multi-mapping are true?
    type: multiple
    options:
      - text: The mapping function receives one object per type in the generic argument list
        correct: true
      - text: Dapper supports mapping up to seven input types in one call
        correct: true
      - text: "Columns must be ordered so each object's columns are contiguous"
        correct: true
      - text: Dapper automatically generates the JOIN SQL
    explanation: You write the JOIN; Dapper splits each row into objects and lets you stitch them together.
---
## Learning objectives

- Map one row to several objects with `splitOn`
- Build one-to-many graphs without duplicates
- Know when to use a second query instead

## One-to-one

```csharp
public sealed class Order
{
    public int Id { get; set; }
    public DateTime PlacedAt { get; set; }
    public Customer Customer { get; set; } = default!;
    public List<OrderLine> Lines { get; } = [];
}

const string sql = """
    SELECT o.Id, o.PlacedAt,
           c.Id, c.FullName, c.Email
    FROM Orders o
    JOIN Customers c ON c.Id = o.CustomerId
    WHERE o.PlacedAt >= @since
    """;

var orders = await db.QueryAsync<Order, Customer, Order>(
    sql,
    (order, customer) => { order.Customer = customer; return order; },
    new { since },
    splitOn: "Id");
```

The second `Id` column starts the `Customer`. If the child's key had a different name, such as
`CustomerId`, you would pass `splitOn: "CustomerId"`; for several splits, a comma-separated list.

## One-to-many

```csharp
const string sql = """
    SELECT o.Id, o.PlacedAt,
           l.Id, l.Sku, l.Quantity, l.UnitPrice
    FROM Orders o
    LEFT JOIN OrderLines l ON l.OrderId = o.Id
    WHERE o.CustomerId = @customerId
    ORDER BY o.Id
    """;

var lookup = new Dictionary<int, Order>();

await db.QueryAsync<Order, OrderLine, Order>(
    sql,
    (order, line) =>
    {
        if (!lookup.TryGetValue(order.Id, out var existing))
        {
            existing = order;
            lookup.Add(order.Id, existing);
        }
        if (line is not null) existing.Lines.Add(line);
        return existing;
    },
    new { customerId },
    splitOn: "Id");

var result = lookup.Values.ToList();
```

With a `LEFT JOIN`, orders without lines produce a row where all line columns are `NULL`. Dapper
passes `null` for that object when the split column is null, hence the null check.

## When to use two queries instead

Joins multiply rows: 100 orders × 20 lines sends 2,000 rows with order columns repeated. For wide
parents or many children, two queries (or `QueryMultiple`, next section) transfer less data.
