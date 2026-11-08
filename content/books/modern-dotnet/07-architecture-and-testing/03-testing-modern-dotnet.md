---
title: Testing modern .NET applications
questions:
  - id: mtp
    prompt: What is Microsoft.Testing.Platform (MTP)?
    options:
      - text: A cloud service for running tests
      - id: platform
        text: A lightweight test platform where test projects are executables, an alternative to VSTest
        correct: true
      - text: A mocking library
      - text: A code coverage file format
    explanation: MTP test projects can run with `dotnet run`, `dotnet test` (when opted in through `global.json`) or directly as executables.
  - id: waf
    prompt: What does `WebApplicationFactory<Program>` provide?
    options:
      - id: in-memory
        text: An in-memory test server running your real app pipeline, with the ability to override services
        correct: true
      - text: A browser automation driver
      - text: A fake database
    explanation: It boots the app with its real middleware and DI, served by an in-memory `TestServer`, and gives you an `HttpClient` to call it.
  - id: pyramid
    prompt: Which statements about the test pyramid are true?
    type: multiple
    options:
      - text: Unit tests are the fastest and most numerous
        correct: true
      - text: Integration tests check that components work together with real infrastructure
        correct: true
      - text: End-to-end UI tests are slow and should be fewer
        correct: true
      - text: Only end-to-end tests provide any value
    explanation: Most tests should be fast unit tests, supported by fewer integration tests and a thin layer of end-to-end tests.
  - id: testcontainers
    prompt: Why use Testcontainers for integration tests?
    options:
      - text: It replaces unit tests
      - id: real
        text: It starts real dependencies such as SQL Server or PostgreSQL in Docker containers for the duration of the tests
        correct: true
      - text: It mocks the database in memory
    explanation: Testing against the real engine catches SQL dialect, constraint and transaction issues that in-memory fakes miss.
---
## Learning objectives

- Write unit tests with xUnit v3
- Run integration tests against the real pipeline with `WebApplicationFactory`
- Use real databases in tests with Testcontainers

## Unit tests with xUnit v3

```csharp
public class PricingTests
{
    [Theory]
    [InlineData(50, 0)]
    [InlineData(100, 10)]
    public void Volume_discount_applies_from_100(decimal subtotal, decimal expectedDiscount)
    {
        var pricing = new VolumeDiscountPolicy(threshold: 100m, rate: 0.10m);
        Assert.Equal(expectedDiscount, pricing.Discount(subtotal));
    }
}
```

xUnit v3 test projects are executables. With MTP enabled in `global.json`:

```json
{ "test": { "runner": "Microsoft.Testing.Platform" } }
```

```bash
dotnet test
dotnet test --filter-class Shop.Tests.PricingTests
```

## Integration tests

```csharp
public class OrderApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Unknown_order_returns_404()
    {
        var client = factory.WithWebHostBuilder(b => b.ConfigureServices(services =>
        {
            services.RemoveAll<IOrderRepository>();
            services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        })).CreateClient();

        var response = await client.GetAsync("/api/orders/999", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
```

`WebApplicationFactory` needs a `public` `Program` class. In .NET 10 the Web SDK generates
`public partial class Program` for top-level programs; on older versions, declare it yourself.

## Real databases with Testcontainers

```csharp
public sealed class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest").Build();
    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync() => await _container.StartAsync();
    public async ValueTask DisposeAsync() => await _container.DisposeAsync();
}
```

xUnit v3's `IAsyncLifetime` uses `ValueTask`. Share one container per test class or collection to
keep runs fast.

## What to test where

| Level | Tests | Example |
|-------|-------|---------|
| Unit | Domain rules, pure functions, handlers with fakes | Discount calculation |
| Integration | Data access, HTTP pipeline, serialization | Repository against real SQL |
| End-to-end | Critical user journeys | Checkout with Playwright |
