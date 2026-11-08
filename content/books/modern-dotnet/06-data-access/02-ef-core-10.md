---
title: What's new in EF Core 10
questions:
  - id: left-join
    prompt: How do you express a left outer join in an EF Core 10 LINQ query?
    options:
      - text: It is impossible in LINQ; raw SQL is required
      - id: leftjoin
        text: With the new `LeftJoin` operator
        correct: true
      - text: "With `Join(..., outer: true)`"
      - text: With `Include` only
    explanation: .NET 10 adds `LeftJoin` and `RightJoin`, and EF Core 10 translates them, replacing the `GroupJoin`/`SelectMany`/`DefaultIfEmpty` pattern.
  - id: named-filters
    prompt: What do named query filters in EF Core 10 allow?
    options:
      - text: Filtering by table name
      - text: Running filters on the client
      - id: multiple
        text: Defining several global filters on one entity and disabling them selectively by name
        correct: true
    explanation: "Before EF Core 10, an entity could have only one filter, and `IgnoreQueryFilters()` disabled it entirely."
  - id: features
    prompt: Which statements about EF Core 10 are true?
    type: multiple
    options:
      - text: It requires .NET 10
        correct: true
      - text: "`ExecuteUpdateAsync` accepts a regular lambda, making conditional setters easier to write"
        correct: true
      - text: It supports vector search on SQL Server and Azure SQL with a vector data type
        correct: true
      - text: It removes support for migrations
    explanation: EF Core 10 is an LTS release aligned with .NET 10 and continues to support migrations.
---
## Learning objectives

- Use `LeftJoin` and `RightJoin` in queries
- Combine multiple named query filters
- Write conditional bulk updates

EF Core 10 is an LTS release that requires .NET 10.

## LeftJoin and RightJoin

```csharp
var report = await db.Customers
    .LeftJoin(db.Orders,
        customer => customer.Id,
        order => order.CustomerId,
        (customer, order) => new { customer.Name, OrderId = (int?)order.Id })
    .ToListAsync(ct);
```

## Named query filters

```csharp
model.Entity<Order>()
    .HasQueryFilter("SoftDelete", o => !o.IsDeleted)
    .HasQueryFilter("Tenant", o => o.TenantId == _tenant.Id);

var includingDeleted = await db.Orders
    .IgnoreQueryFilters(["SoftDelete"])   // the tenant filter still applies
    .ToListAsync(ct);
```

Previously, ignoring the soft-delete filter also removed tenant isolation — a dangerous trap.

## Conditional bulk updates

```csharp
await db.Products
    .Where(p => p.CategoryId == categoryId)
    .ExecuteUpdateAsync(setters =>
    {
        setters.SetProperty(p => p.UpdatedAt, now);
        if (newPrice is not null)
        {
            setters.SetProperty(p => p.Price, newPrice.Value);
        }
    }, ct);
```

Before EF Core 10 the argument had to be an expression tree, so conditional setters required
building expressions by hand.

## Other highlights

- **Complex types** improvements, including mapping complex types to JSON columns and optional complex types
- **Vector search** for SQL Server 2025 and Azure SQL (`SqlVector<float>` and `EF.Functions.VectorDistance`)
- Better translation of parameterised collections (`Contains` over a list)

> Upgrade tip: read the "breaking changes" page for each EF Core version before upgrading a
> production app; most are small but some change query translation.
