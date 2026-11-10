---
title: A testing strategy that ages well
questions:
  - id: behavior
    prompt: Which kind of test survives refactoring best?
    options:
      - text: Tests that verify which private methods were called, in what order
      - text: Tests that assert on log message text for every line
      - id: behavior
        text: Tests that verify observable behavior through a public interface — inputs, outputs and visible side effects
        correct: true
      - text: Tests that use reflection to set private fields
    explanation: Tests coupled to implementation details break on every refactoring, so teams stop refactoring. Behavior-focused tests are the ones that protect change.
  - id: dont-mock-db
    prompt: Why is mocking `IDbConnection` or `DbContext` a weak way to test data access?
    options:
      - text: Mocking libraries cannot mock interfaces
      - text: It is too fast
      - text: It requires Docker
      - id: sql-untested
        text: The SQL, mappings, constraints and transactions — the things most likely to be wrong — are never executed
        correct: true
    explanation: Test data access against a real engine. Testcontainers makes that routine; the EF Core in-memory provider is not a relational database and is discouraged for this purpose.
  - id: webappfactory
    prompt: What does `WebApplicationFactory<TProgram>` (Microsoft.AspNetCore.Mvc.Testing) give you?
    options:
      - text: A browser automation tool
      - text: A load-testing framework
      - text: A mock HTTP handler only
      - id: in-memory-host
        text: An in-memory test server running your real `Program` pipeline, plus an `HttpClient` to call it, with the ability to replace services
        correct: true
    explanation: It tests routing, model binding, validation, filters, serialization and DI wiring together — the seams unit tests skip.
  - id: time
    prompt: How should code that depends on the current time or timers be tested?
    options:
      - text: With `Thread.Sleep` and generous margins
      - text: By running tests only at midnight
      - text: By changing the machine's clock
      - id: fake-time
        text: Inject `TimeProvider` and use `FakeTimeProvider` to set and advance time deterministically
        correct: true
    explanation: Sleep-based tests are slow and flaky. Fake time is instant and exact.
  - id: what-to-fake
    prompt: Which collaborators are usually worth faking in tests?
    type: multiple
    options:
      - id: third-party
        text: Third-party HTTP APIs you do not control (payment gateways, carriers)
        correct: true
      - id: email
        text: Outbound email and SMS
        correct: true
      - text: Your own pure domain classes
      - id: clock
        text: The clock
        correct: true
    explanation: Fake what is slow, non-deterministic, costly or outside your control. Use real instances of your own in-process code.
  - id: assertion-libs
    prompt: In 2025 Fluent Assertions v8 moved to a commercial licence for commercial use. What is the most timeless takeaway?
    options:
      - text: Never use any third-party library in tests
      - text: Licences do not apply to test code
      - id: weigh-deps
        text: Test dependencies are dependencies — weigh licence and maintenance risk; xUnit's built-in `Assert` (plus a few helpers) covers most needs, and free alternatives exist
        correct: true
      - text: Pin to v7 forever without reviewing
    explanation: Staying on the Apache-licensed v7 line is legitimate; so is switching. The lesson is to make it a decision, not an accident.
  - id: flaky
    prompt: A test fails 1 in 50 runs. What should you do?
    options:
      - text: Add an automatic retry and move on
      - id: investigate
        text: Treat it as a bug — usually shared state, time, ordering or concurrency — and fix the cause; quarantine it visibly if you cannot fix it immediately
        correct: true
      - text: Delete it
      - text: Mark it `Explicit` forever
    explanation: Retries hide real race conditions that also exist in production. Flakiness erodes trust in the whole suite.
---
## Learning objectives

- Test behavior, not implementation
- Use real dependencies where they matter and fakes where they help
- Write API and data integration tests with modern tools

## The shape of a healthy suite

- **Many fast tests** of pure logic: pricing, validation, state transitions, mapping with logic.
- **A solid layer of integration tests** of the application through its real HTTP pipeline and a
  real database in a container.
- **A few end-to-end tests** of critical journeys.

The exact ratio matters less than the principle: test each risk at the cheapest level that can
actually detect it.

## API tests with WebApplicationFactory

```csharp
public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _sql = new MsSqlBuilder().Build();
    public FakeTimeProvider Time { get; } = new(new DateTimeOffset(2026, 11, 10, 9, 0, 0, TimeSpan.Zero));

    public async ValueTask InitializeAsync() => await _sql.StartAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Shop", _sql.GetConnectionString());
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(Time);
            services.RemoveAll<IPaymentGateway>();
            services.AddSingleton<IPaymentGateway, FakePaymentGateway>();
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await _sql.DisposeAsync();
        await base.DisposeAsync();
    }
}

public sealed class CheckoutTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Checkout_returns_201_and_order_location()
    {
        var ct = TestContext.Current.CancellationToken;
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/checkout", SampleCheckout.Valid, ct);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.StartsWith("/orders/", response.Headers.Location?.OriginalString);
    }
}
```

`Program` needs to be visible to the test project; with top-level statements add
`public partial class Program;` at the end of `Program.cs`.

## Faking an HTTP API without a mocking library

```csharp
public sealed class StubHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) =>
        Task.FromResult(respond(request));
}

var http = new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.TooManyRequests)))
{
    BaseAddress = new Uri("https://carrier.test/"),
};
```

## Hand-written fakes age well

```csharp
public sealed class FakePaymentGateway : IPaymentGateway
{
    public List<Charge> Charges { get; } = [];

    public Task<ChargeResult> ChargeAsync(Charge charge, CancellationToken ct)
    {
        Charges.Add(charge);
        return Task.FromResult<ChargeResult>(new ChargeResult.Approved($"auth-{Charges.Count}"));
    }
}
```

A fake with state is often clearer than a mock configured with five `Setup` calls, has no
dependency, and keeps working when mocking libraries change.

## Sources

- Microsoft Learn, "Integration tests in ASP.NET Core" — <https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests>
- Microsoft Learn, "Testing EF Core applications — choosing a strategy" — <https://learn.microsoft.com/en-us/ef/core/testing/choosing-a-testing-strategy>
- Microsoft Learn, `FakeTimeProvider` — <https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.time.testing.faketimeprovider>
- Testcontainers for .NET — <https://dotnet.testcontainers.org/>
- InfoQ, "Fluent Assertions v8 licence change" — <https://www.infoq.com/news/2025/01/fluent-assertions-v8-license>
- Vladimir Khorikov, *Unit Testing Principles, Practices, and Patterns* (Manning, 2020)
