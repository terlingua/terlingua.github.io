---
title: Testing the data layer
questions:
  - id: mock-dapper
    prompt: Why is mocking Dapper's extension methods a poor testing strategy?
    options:
      - text: Extension methods run too fast to measure
      - text: Dapper forbids testing
      - id: no-value
        text: Extension methods are static and hard to mock, and a mock would not verify that your SQL actually works
        correct: true
      - text: Mocks cannot return lists
    explanation: The risk in a Dapper data layer is the SQL itself. Test it against a real database engine.
  - id: what-to-test
    prompt: Which approach gives the most confidence in a Dapper query class?
    options:
      - id: real-db
        text: Integration tests that run the real SQL against the same database engine used in production, for example via Testcontainers
        correct: true
      - text: Unit tests with an in-memory list
      - text: Manual testing in production
    explanation: Real-engine tests catch syntax, type, mapping and constraint problems.
  - id: isolation
    prompt: Which techniques keep database integration tests isolated and repeatable?
    type: multiple
    options:
      - text: Wrap each test in a transaction that is rolled back at the end
        correct: true
      - text: Reset the database to a known state between tests, for example with Respawn
        correct: true
      - text: Apply the same migrations to the test database that production uses
        correct: true
      - text: Share mutable rows between tests and rely on execution order
    explanation: Each test must start from a known state regardless of what ran before it.
---
## Learning objectives

- Test SQL against a real database engine
- Keep integration tests isolated and fast
- Test consumers of the data layer with fakes

## Two kinds of tests

| What | How |
|------|-----|
| Query/command classes (the SQL) | Integration tests against a real engine |
| Services that use `IOrderQueries` | Unit tests with a fake implementation |

Thanks to the interfaces from the previous section, business logic never needs a database to be tested.

## A database fixture

```csharp
public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sql = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public IDbConnectionFactory Connections { get; private set; } = default!;

    public async ValueTask InitializeAsync()
    {
        await _sql.StartAsync();
        var connectionString = _sql.GetConnectionString();
        await SchemaMigrator.ApplyAsync(connectionString);   // e.g. DbUp, EF migrations bundle, or SQL scripts
        Connections = new TestConnectionFactory(connectionString);
    }

    public async ValueTask DisposeAsync() => await _sql.DisposeAsync();
}
```

## A query test

```csharp
public sealed class SqlOrderQueriesTests(DatabaseFixture db) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task ListRecent_returns_newest_first_and_respects_take()
    {
        var ct = TestContext.Current.CancellationToken;
        var customerId = await TestData.CustomerWithOrdersAsync(db.Connections,
            placedAt: [new DateTime(2026, 1, 1), new DateTime(2026, 3, 1), new DateTime(2026, 2, 1)], ct);

        var orders = await new SqlOrderQueries(db.Connections).ListRecentAsync(customerId, take: 2, ct);

        Assert.Equal(new[] { new DateTime(2026, 3, 1), new DateTime(2026, 2, 1) }, orders.Select(o => o.PlacedAt));
    }
}
```

Each test creates its own customer, so tests do not interfere with each other even when they share
a database.

## Fakes for consumers

```csharp
public sealed class FakeOrderQueries(params OrderSummary[] orders) : IOrderQueries
{
    public Task<IReadOnlyList<OrderSummary>> ListRecentAsync(int customerId, int take, CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<OrderSummary>>(orders.Take(take).ToList());

    public Task<OrderDetails?> GetDetailsAsync(int orderId, CancellationToken ct) =>
        Task.FromResult<OrderDetails?>(null);
}
```
