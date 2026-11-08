---
title: The composition root and DI containers
questions:
  - id: composition-root
    prompt: Where should object graphs be composed in an ASP.NET Core application?
    options:
      - text: Inside each domain class, using `new`
      - text: In a static `ServiceLocator` called from anywhere
      - id: program
        text: In a single place near the entry point, such as `Program.cs`
        correct: true
    explanation: The composition root is the one place that knows about concrete implementations. Everywhere else depends on abstractions.
  - id: service-locator
    prompt: Why is calling `serviceProvider.GetService<T>()` from deep inside business classes considered an anti-pattern?
    options:
      - text: It is slower than reflection
      - id: hidden
        text: It hides dependencies, so a class's constructor no longer tells you what it needs
        correct: true
      - text: It is not supported in .NET 10
    explanation: Service location makes dependencies implicit, harder to test and easy to get wrong at runtime.
  - id: lifetimes
    prompt: Which statements about Microsoft.Extensions.DependencyInjection lifetimes are true?
    type: multiple
    options:
      - text: A singleton is created once for the root container
        correct: true
      - text: In ASP.NET Core, a scoped service is created once per HTTP request
        correct: true
      - text: A transient service is created every time it is resolved
        correct: true
      - text: Injecting a scoped service into a singleton is always safe
    explanation: A singleton holding a scoped service keeps it alive beyond its scope — a captive dependency. Scope validation in Development detects this.
---
## Learning objectives

- Compose object graphs in a single composition root
- Avoid the service locator anti-pattern
- Pick correct lifetimes and avoid captive dependencies

## The composition root

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();
app.MapPost("/checkout", (CheckoutRequest req, CheckoutService checkout, CancellationToken ct) =>
    checkout.CheckoutAsync(req.Cart, req.Card, ct));
app.Run();
```

Only `Program.cs` knows that `IPaymentGateway` means Stripe. Swapping providers is a one-line change.

## Lifetimes

| Lifetime | Created | Typical use |
|----------|---------|-------------|
| Singleton | Once | Stateless services, caches, `TimeProvider` |
| Scoped | Once per scope (per request in ASP.NET Core) | `DbContext`, unit of work |
| Transient | Every resolution | Lightweight stateless helpers |

A **captive dependency** happens when a longer-lived service holds a shorter-lived one, such as a
singleton holding a `DbContext`. ASP.NET Core enables scope validation in the Development
environment, which throws when a scoped service is resolved from the root provider.

## Service locator

```csharp
public sealed class CheckoutService(IServiceProvider services)
{
    public Task CheckoutAsync(Cart cart) =>
        services.GetRequiredService<IPaymentGateway>().ChargeAsync(/* ... */);
}
```

The constructor lies about what the class needs. Ask for `IPaymentGateway` directly instead.
Resolving from `IServiceProvider` is acceptable only in infrastructure code such as factories and
middleware, where creating scopes is the actual job.
