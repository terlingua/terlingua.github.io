---
title: DI anti-patterns and vestiges
questions:
  - id: service-locator
    prompt: A class takes `IServiceProvider` in its constructor and calls `GetRequiredService<T>()` inside its methods. What is this called?
    options:
      - text: Constructor injection
      - text: The decorator pattern
      - id: locator
        text: The service locator anti-pattern — the class's real dependencies are hidden and only fail at runtime
        correct: true
      - text: Pure DI
    explanation: Legitimate exceptions exist at infrastructure boundaries (factories, middleware, hosted services creating scopes). In business classes, list the dependencies in the constructor.
  - id: over-injection
    prompt: A controller constructor has 14 parameters. What is the most useful interpretation?
    options:
      - text: DI is failing; switch containers
      - id: srp-smell
        text: A design smell — the class probably has too many responsibilities and should be split (for example into per-endpoint handlers)
        correct: true
      - text: Use property injection to hide them
      - text: Use a static service locator instead
    explanation: DI makes the cost of a class's dependencies visible. That visibility is a feature, not a problem to suppress.
  - id: third-party-container
    prompt: Which statement about third-party containers (Autofac, Lamar, etc.) in 2026 is most accurate?
    options:
      - text: They are required for any serious ASP.NET Core application
      - text: They are no longer compatible with .NET
      - id: optional
        text: The built-in container covers most applications; a third-party container is justified by a specific need such as convention-based modules, interception or child containers
        correct: true
      - text: They are faster in every scenario
    explanation: Before ASP.NET Core (2016), .NET had no standard container, so every project picked one. That habit outlived its reason in many codebases.
  - id: interface-per-class
    prompt: Is an interface required to register a class in DI?
    options:
      - text: Yes; the container only resolves interfaces
      - text: Only for singletons
      - id: no-interface
        text: No — `services.AddScoped<InvoiceSender>()` registers a concrete class, which is fine when there is no substitution need
        correct: true
      - text: Only for scoped services
    explanation: Introduce an interface when you have (or clearly will have) multiple implementations or need a test seam at an I/O boundary.
  - id: static-mediator
    prompt: Which risks come with routing every call through an in-process mediator library ("send a request object, a handler is found by reflection")?
    type: multiple
    options:
      - id: navigation
        text: "\"Go to definition\" lands on the mediator, not the handler, which slows navigation"
        correct: true
      - id: hidden-coupling
        text: Dependencies between features become implicit
        correct: true
      - id: licensing
        text: A dependency whose licence or maintenance model may change (MediatR introduced a commercial edition in 2025)
        correct: true
      - text: The C# compiler cannot compile mediator code
    explanation: Mediators can be useful for cross-cutting pipelines. Often, though, injecting a handler class directly gives the same structure with plain method calls.
  - id: async-factory
    prompt: A service needs an asynchronously loaded configuration (for example a key from a vault) before it can be used. Which approach fits DI best?
    options:
      - text: Call `.Result` on the load inside the registration lambda
      - id: startup-load
        text: Load it during startup (await before `app.Run()`, or in a hosted service / options with async validation) and register the result, or expose an explicit `InitializeAsync`
        correct: true
      - text: Make the constructor `async`
      - text: Use `async void` in the constructor
    explanation: DI resolution is synchronous. Do the async work at a point that can await, then hand the container a ready object.
---
## Learning objectives

- Recognize the service locator, captive dependencies and constructor over-injection
- Decide when an interface and when a third-party container are justified
- Handle async initialization without blocking

## Service locator, disguised

```csharp
public sealed class OrderService(IServiceProvider provider)
{
    public async Task PlaceAsync(Order order, CancellationToken ct)
    {
        var stock = provider.GetRequiredService<IStockService>();
        var payments = provider.GetRequiredService<IPayments>();
        await stock.ReserveAsync(order, ct);
        await payments.ChargeAsync(order, ct);
    }
}
```

Nothing about the constructor tells you what this class needs; tests must build a provider; a
missing registration fails only when this path runs. List the dependencies:

```csharp
public sealed class OrderService(IStockService stock, IPayments payments)
{
    public async Task PlaceAsync(Order order, CancellationToken ct)
    {
        await stock.ReserveAsync(order, ct);
        await payments.ChargeAsync(order, ct);
    }
}
```

## Over-injection as a design signal

```csharp
public sealed class OrdersController(
    IOrderStore store, IPricing pricing, ITax tax, IStock stock, IPayments payments,
    IShipping shipping, IEmail email, IAudit audit, IFraud fraud, ILoyalty loyalty,
    IFeatureFlags flags, ILogger<OrdersController> logger, TimeProvider time, IMapper mapper)
    : ControllerBase
{
}
```

Each endpoint uses three or four of these. Splitting into per-use-case handlers (vertical
slices) gives each class a small, honest constructor:

```csharp
app.MapPost("/orders/{id}/ship", async (Guid id, ShipOrder handler, CancellationToken ct) =>
    await handler.HandleAsync(new OrderId(id), ct));

public sealed class ShipOrder(IOrderStore store, IShipping shipping, IEmail email, TimeProvider time)
{
    public async Task<IResult> HandleAsync(OrderId id, CancellationToken ct)
    {
        var order = await store.GetAsync(id, ct);
        if (order is null) return Results.NotFound();
        var label = await shipping.CreateLabelAsync(order, ct);
        order.MarkShipped(label.TrackingNumber, time.GetUtcNow());
        await store.SaveAsync(order, ct);
        await email.SendShippedAsync(order, CancellationToken.None);
        return Results.NoContent();
    }
}
```

## Async initialization done right

```csharp
var builder = WebApplication.CreateBuilder(args);

var signingKey = await KeyVaultLoader.LoadSigningKeyAsync(builder.Configuration, CancellationToken.None);
builder.Services.AddSingleton(signingKey);

var app = builder.Build();
app.Run();
```

## Vestige check: why did we use Autofac?

Ask the historical question. Many 2014-era projects chose Autofac because ASP.NET MVC 5 had no
container. If the answers today are "module scanning we could replace with `AddXyz` methods" and
"nobody remembers", the dependency is vestigial. If the answer is "we rely on child lifetime
scopes per tenant", it is not — keep it.

## Sources

- Mark Seemann, "Service Locator is an Anti-Pattern" — <https://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/>
- Microsoft Learn, "Dependency injection guidelines — anti-patterns" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#recommendations>
- Jimmy Bogard, "AutoMapper and MediatR Commercial Editions Launch Today" — <https://www.jimmybogard.com/automapper-and-mediatr-commercial-editions-launch-today/>
