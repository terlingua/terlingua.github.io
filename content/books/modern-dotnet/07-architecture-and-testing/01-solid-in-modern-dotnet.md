---
title: SOLID with modern .NET building blocks
questions:
  - id: srp-tool
    prompt: Which .NET feature most directly supports the Dependency Inversion Principle?
    options:
      - text: Extension methods
      - text: Records
      - id: di
        text: Constructor injection with the built-in DI container and a composition root in `Program.cs`
        correct: true
      - text: The `dynamic` keyword
    explanation: Business classes depend on abstractions passed through constructors; only the composition root knows the concrete types.
  - id: ocp-tool
    prompt: Registering several `IShippingMethod` implementations and injecting `IEnumerable<IShippingMethod>` supports which principle?
    options:
      - text: Liskov Substitution — because the list is ordered
      - id: ocp
        text: Open/Closed — new implementations are added without modifying the consumer
        correct: true
      - text: Single Responsibility — because the container creates the objects
    explanation: The consumer is closed to modification yet the system is open to new shipping methods.
  - id: mapping
    prompt: Which pairings of .NET feature and principle are accurate?
    type: multiple
    options:
      - text: "`TimeProvider` — depend on an abstraction for a volatile dependency (DIP)"
        correct: true
      - text: "Decorating an `IHttpClientFactory` client with `DelegatingHandler`s — extend behaviour without editing the client (OCP)"
        correct: true
      - text: "`IReadOnlyList<T>` return types — give clients only the capability they need (ISP)"
        correct: true
      - text: "`static` service classes with global state — easier substitution (LSP)"
    explanation: Static global state prevents substitution entirely; it is the opposite of LSP-friendly design.
---
The SOLID book on this site covers each principle in depth. This section maps them onto the
everyday building blocks of modern .NET.

## Learning objectives

- Recognise SOLID in the features you already use
- Apply each principle with idiomatic .NET constructs

## The map

| Principle | Modern .NET building block |
|-----------|----------------------------|
| SRP | Feature folders; small handlers; `BackgroundService` per job; options classes per concern |
| OCP | `IEnumerable<T>` injection; middleware; `DelegatingHandler`; keyed services |
| LSP | Contract tests across implementations; `IReadOnly*` interfaces instead of throwing `NotSupportedException` |
| ISP | Small interfaces (`IOrderReader`, `IOrderWriter`); minimal API handlers asking only for what they use |
| DIP | Constructor injection; abstractions owned by the application layer; `TimeProvider`, `IHttpClientFactory` |

## DIP and OCP together

```csharp
public interface IPaymentProvider
{
    string Name { get; }
    Task<PaymentResult> ChargeAsync(PaymentRequest request, CancellationToken ct);
}

builder.Services.AddKeyedScoped<IPaymentProvider, StripeProvider>("stripe");
builder.Services.AddKeyedScoped<IPaymentProvider, AdyenProvider>("adyen");

app.MapPost("/pay/{provider}", async (string provider, PaymentRequest request,
    IServiceProvider services, CancellationToken ct) =>
{
    var payments = services.GetKeyedService<IPaymentProvider>(provider);
    return payments is null
        ? Results.NotFound()
        : Results.Ok(await payments.ChargeAsync(request, ct));
});
```

Resolving by key from `IServiceProvider` at the edge (an endpoint) is legitimate — the endpoint *is*
part of the composition layer. Business classes still receive their dependencies through constructors.

## Cross-cutting concerns as decorators

```csharp
public sealed class TimingHandler(ILogger<TimingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var start = Stopwatch.GetTimestamp();
        var response = await base.SendAsync(request, ct);
        logger.LogInformation("{Method} {Uri} took {Elapsed}", request.Method, request.RequestUri, Stopwatch.GetElapsedTime(start));
        return response;
    }
}

builder.Services.AddTransient<TimingHandler>();
builder.Services.AddHttpClient<GitHubClient>().AddHttpMessageHandler<TimingHandler>();
```

The `GitHubClient` never changes; behaviour is layered around it.
