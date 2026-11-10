---
title: Where and how to use cancellation tokens
questions:
  - id: param-position
    prompt: By .NET convention, where does a `CancellationToken` parameter go in a method signature?
    options:
      - text: First
      - text: Anywhere, as long as it is named `token`
      - id: last
        text: Last
        correct: true
      - text: It should be a property on the class instead
    explanation: The Framework Design Guidelines and analyzer CA1068 expect the token last, which keeps overloads and optional parameters consistent.
  - id: default-or-not
    prompt: Should a public async method's token parameter have a default value (`CancellationToken ct = default`)?
    options:
      - text: Never; defaults are forbidden for structs
      - text: Always; required tokens are an anti-pattern
      - text: Only in .NET Framework
      - id: depends
        text: In reusable libraries, commonly yes for convenience; in application code, making it required forces every caller to think about which token to pass
        correct: true
    explanation: Both are defensible. Many teams make it required in internal application code so a forgotten token is a compile error, and rely on analyzer CA2016 to flag tokens not forwarded.
  - id: aspnet-binding
    prompt: In an ASP.NET Core minimal API, how do you get a token that is cancelled when the client disconnects?
    options:
      - id: parameter
        text: Add a `CancellationToken` parameter to the handler; it is bound to `HttpContext.RequestAborted`
        correct: true
      - text: "`new CancellationTokenSource().Token`"
      - text: Read it from `appsettings.json`
      - text: "`CancellationToken.None`"
    explanation: The same works for MVC actions, SignalR hub methods and gRPC methods.
  - id: ca2016
    prompt: What does analyzer CA2016 report?
    options:
      - text: A token parameter is not last
      - text: A `CancellationTokenSource` is not disposed
      - id: forward
        text: A method has a `CancellationToken` available but calls an API overload that accepts one without passing it
        correct: true
      - text: An `async void` method
    explanation: Turn it on as a warning (and warnings as errors). It catches the most common cancellation bug — forgetting to pass the token along.
  - id: httpclient-timeout
    prompt: What happens when an `HttpClient` call exceeds `HttpClient.Timeout` (100 seconds by default)?
    options:
      - text: It returns a 408 response
      - id: tce-timeout
        text: It throws `TaskCanceledException` whose `InnerException` is a `TimeoutException` (since .NET 5)
        correct: true
      - text: It retries automatically
      - text: It blocks forever
    explanation: Timeouts and caller cancellation both surface as cancellation exceptions; check the inner exception or your own token to tell them apart. Resilience handlers (`AddStandardResilienceHandler`) add per-attempt and total timeouts.
  - id: dapper-token
    prompt: How do you pass a cancellation token to a Dapper query?
    options:
      - text: Dapper does not support cancellation
      - text: Pass it as an anonymous object property named `ct`
      - id: commanddefinition
        text: Wrap the SQL, parameters and token in a `CommandDefinition` and pass it to `QueryAsync`/`ExecuteAsync`
        correct: true
      - text: Set a static `SqlMapper.CancellationToken`
    explanation: The `QueryAsync(string sql, object param)` overloads have no token parameter; `CommandDefinition` carries one through to `DbCommand.ExecuteReaderAsync`.
  - id: xunit-token
    prompt: In xUnit v3, which token should an async test pass to the code under test?
    options:
      - text: "`CancellationToken.None`"
      - id: testcontext
        text: "`TestContext.Current.CancellationToken`, which is cancelled when the test run is cancelled or the test times out"
        correct: true
      - text: A new `CancellationTokenSource` per test
      - text: Tests cannot use tokens
    explanation: xUnit v3's analyzer (xUnit1051) suggests it. It makes test runs stop promptly when you cancel them in the IDE or CI.
  - id: shutdown
    prompt: Which token tells a hosted service that the application is stopping?
    type: multiple
    options:
      - id: stopping-token
        text: The `stoppingToken` passed to `BackgroundService.ExecuteAsync`
        correct: true
      - id: lifetime
        text: "`IHostApplicationLifetime.ApplicationStopping`"
        correct: true
      - text: "`HttpContext.RequestAborted`"
      - text: "`CancellationToken.None`"
    explanation: Request tokens belong to requests. Background work should use the host's shutdown signals.
---
## Learning objectives

- Design method signatures with tokens
- Get the right token at each framework boundary
- Pass tokens to HttpClient, Dapper, EF Core, channels and tests

## A field map of where tokens come from

| Boundary | Token |
|---|---|
| Minimal API / MVC / SignalR / gRPC | Handler parameter → `HttpContext.RequestAborted` |
| `BackgroundService` | `stoppingToken` |
| Any hosted code | `IHostApplicationLifetime.ApplicationStopping` |
| Console app | Your own `CancellationTokenSource` wired to `Console.CancelKeyPress` |
| Azure Functions (isolated) | `CancellationToken` parameter on the function |
| xUnit v3 test | `TestContext.Current.CancellationToken` |
| A timeout you impose | Linked source + `CancelAfter` |

## Signatures

```csharp
public interface ICustomerDirectory
{
    Task<Customer?> FindAsync(CustomerId id, CancellationToken ct);
    IAsyncEnumerable<Customer> SearchAsync(string text, CancellationToken ct);
}
```

## End-to-end flow in a request

```csharp
app.MapGet("/customers/{id:guid}/summary", async (Guid id, CustomerSummaryQuery query, CancellationToken ct) =>
    await query.ExecuteAsync(new CustomerId(id), ct) is { } summary
        ? Results.Ok(summary)
        : Results.NotFound());

public sealed class CustomerSummaryQuery(DbDataSource db, IHttpClientFactory httpFactory)
{
    public async Task<CustomerSummary?> ExecuteAsync(CustomerId id, CancellationToken ct)
    {
        await using var connection = await db.OpenConnectionAsync(ct);
        var customer = await connection.QuerySingleOrDefaultAsync<CustomerRow>(new CommandDefinition(
            "select Id, Name, Email from dbo.Customers where Id = @id",
            new { id = id.Value },
            cancellationToken: ct));
        if (customer is null) return null;

        var http = httpFactory.CreateClient("loyalty");
        var points = await http.GetFromJsonAsync<LoyaltyPoints>($"points/{id}", ct);

        return new CustomerSummary(customer.Name, customer.Email, points?.Balance ?? 0);
    }
}
```

If the user closes the page mid-request, the SQL command is cancelled (SqlClient sends an
attention signal to the server), the HTTP call is cancelled, and no further work is done.

## EF Core and channels

```csharp
var open = await db.Orders.Where(o => o.Status == OrderStatus.Open).ToListAsync(ct);
await db.SaveChangesAsync(ct);
await channel.Writer.WriteAsync(item, ct);
```

## Tests with controllable time

```csharp
[Fact]
public async Task Quote_times_out_after_three_seconds()
{
    var time = new FakeTimeProvider();
    var carrier = new NeverRespondingCarrier();
    var sut = new QuoteService(carrier, time);

    var pending = sut.GetQuoteAsync(SampleShipment, TestContext.Current.CancellationToken);
    time.Advance(TimeSpan.FromSeconds(3));

    await Assert.ThrowsAsync<TimeoutException>(() => pending);
}
```

For this to work, `QuoteService` must create its timeout source with the provider:
`new CancellationTokenSource(TimeSpan.FromSeconds(3), time)`, linked to the caller's token via
`ct.Register` or a linked source.

## A decision checklist

1. Does the caller have a reason to stop waiting? Accept a token.
2. Do I call anything that accepts a token? Pass it (CA2016).
3. Is there a point of no return? Switch deliberately to `CancellationToken.None` or a shutdown-only token.
4. Do I need my own deadline? Linked source + `CancelAfter`, disposed with `using`.
5. Is my loop CPU-bound? `ThrowIfCancellationRequested` periodically.
6. Am I at a boundary? Treat `OperationCanceledException` from the caller's token as a normal outcome.

## Sources

- Microsoft Learn, CA2016 — <https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2016>
- Microsoft Learn, CA1068 — <https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1068>
- Microsoft Learn, `HttpClient.Timeout` — <https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.timeout>
- Microsoft Learn, "Build resilient HTTP apps" — <https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience>
- xUnit.net, "What's new in v3" — <https://xunit.net/docs/getting-started/v3/whats-new>
