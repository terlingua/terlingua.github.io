---
title: Repositories, query objects and CQRS
questions:
  - id: isp
    prompt: A read-only dashboard needs three queries. Which design follows the Interface Segregation Principle?
    options:
      - text: Inject a generic `IRepository<T>` with 20 CRUD methods
      - text: Inject `IDbConnection` into the Razor page and write SQL there
      - id: query-service
        text: Inject a small `IDashboardQueries` interface with exactly the three methods it needs
        correct: true
      - text: Use a static class with all SQL for the application
    explanation: Client-shaped interfaces keep consumers decoupled from operations they never use.
  - id: generic-repo
    prompt: Why is a generic `IRepository<T>` with methods like `GetAll()` often a poor fit for Dapper?
    options:
      - text: Dapper cannot work with generics
      - id: leaky
        text: Dapper's strength is purpose-built SQL per use case; a generic repository hides that and encourages over-fetching
        correct: true
      - text: Generic interfaces are slower at runtime
    explanation: Specific query methods return exactly the shape each screen or API needs.
  - id: sql-location
    prompt: Which are reasonable places to keep SQL in a Dapper codebase?
    type: multiple
    options:
      - text: As `const` raw string literals next to the method that executes them
        correct: true
      - text: In `.sql` files embedded as resources, loaded once at startup
        correct: true
      - text: In stored procedures owned and versioned with the database schema
        correct: true
      - text: Concatenated from user input at runtime
    explanation: Keep SQL close to its use and under version control; never build it from untrusted input.
---
## Learning objectives

- Design query and command interfaces around use cases
- Apply SOLID to a Dapper data layer
- Keep SQL organised and testable

## Use-case-shaped interfaces

```csharp
public interface IOrderQueries
{
    Task<OrderDetails?> GetDetailsAsync(int orderId, CancellationToken ct);
    Task<IReadOnlyList<OrderSummary>> ListRecentAsync(int customerId, int take, CancellationToken ct);
}

public interface IOrderCommands
{
    Task<int> PlaceAsync(PlaceOrder command, CancellationToken ct);
    Task CancelAsync(int orderId, CancellationToken ct);
}
```

## An implementation

```csharp
public sealed class SqlOrderQueries(IDbConnectionFactory connections) : IOrderQueries
{
    private const string RecentSql = """
        SELECT TOP (@take) o.Id, o.PlacedAt, o.Total, o.Status
        FROM Orders o
        WHERE o.CustomerId = @customerId
        ORDER BY o.PlacedAt DESC
        """;

    public async Task<IReadOnlyList<OrderSummary>> ListRecentAsync(int customerId, int take, CancellationToken ct)
    {
        await using var db = await connections.OpenAsync(ct);
        var rows = await db.QueryAsync<OrderSummary>(new CommandDefinition(RecentSql, new { customerId, take }, cancellationToken: ct));
        return rows.AsList();
    }

    public async Task<OrderDetails?> GetDetailsAsync(int orderId, CancellationToken ct)
    {
        await using var db = await connections.OpenAsync(ct);
        using var multi = await db.QueryMultipleAsync(new CommandDefinition(DetailsSql, new { orderId }, cancellationToken: ct));
        var order = await multi.ReadSingleOrDefaultAsync<OrderDetails>();
        if (order is not null) order.Lines = (await multi.ReadAsync<OrderLine>()).AsList();
        return order;
    }

    private const string DetailsSql = """
        SELECT Id, CustomerId, PlacedAt, Total, Status FROM Orders WHERE Id = @orderId;
        SELECT Sku, Quantity, UnitPrice FROM OrderLines WHERE OrderId = @orderId;
        """;
}
```

## SOLID applied

| Principle | In this design |
|-----------|----------------|
| SRP | Queries and commands live in separate classes |
| OCP | New use cases add methods or classes; existing SQL is untouched |
| LSP | Any `IOrderQueries` (SQL, cached decorator, fake) behaves the same to callers |
| ISP | Consumers depend on `IOrderQueries` or `IOrderCommands`, not both |
| DIP | Application code depends on the interfaces; only the composition root knows about SQL |

## A caching decorator

```csharp
public sealed class CachedOrderQueries(IOrderQueries inner, HybridCache cache) : IOrderQueries
{
    public async Task<OrderDetails?> GetDetailsAsync(int orderId, CancellationToken ct) =>
        await cache.GetOrCreateAsync($"order:{orderId}", async t => await inner.GetDetailsAsync(orderId, t), cancellationToken: ct);

    public Task<IReadOnlyList<OrderSummary>> ListRecentAsync(int customerId, int take, CancellationToken ct) =>
        inner.ListRecentAsync(customerId, take, ct);
}
```
