---
title: Strategies, decorators and pipelines
questions:
  - id: decorator
    prompt: You need to add caching to an existing `IProductRepository` without editing it. Which pattern fits best?
    options:
      - text: Singleton
      - text: Abstract factory
      - id: decorator
        text: Decorator
        correct: true
      - text: Visitor
    explanation: A decorator implements the same interface, wraps the original and adds behaviour before or after delegating.
  - id: pipeline
    prompt: Which ASP.NET Core feature is an example of the open/closed principle?
    options:
      - id: middleware
        text: The middleware pipeline, where new behaviour is added with `app.Use...` calls
        correct: true
      - text: Hard-coding authentication inside every controller action
      - text: The `web.config` file
    explanation: New middleware extends request processing without modifying the framework or existing middleware.
  - id: strategy-facts
    prompt: Which statements about the strategy pattern are true?
    type: multiple
    options:
      - text: It encapsulates interchangeable algorithms behind a common interface
        correct: true
      - text: In C#, a simple strategy can be a delegate such as `Func<Order, decimal>`
        correct: true
      - text: It requires inheritance from a common abstract base class
    explanation: An interface or a delegate is enough; no base class is needed.
---
## Learning objectives

- Apply the strategy and decorator patterns to achieve OCP
- Recognise pipeline-style extensibility in .NET
- Use delegates when an interface would be overkill

## Strategy

A strategy is an interchangeable algorithm. In modern C#, a delegate is often the lightest strategy:

```csharp
public sealed class PriceCalculator(Func<Order, decimal> discount)
{
    public decimal Total(Order order) => order.Subtotal - discount(order);
}

var blackFriday = new PriceCalculator(o => o.Subtotal * 0.25m);
var none = new PriceCalculator(_ => 0m);
```

Use an interface when the strategy has several members, needs dependencies, or deserves a name.

## Decorator

A decorator wraps an implementation of the same interface:

```csharp
public sealed class CachedProductRepository(IProductRepository inner, IMemoryCache cache)
    : IProductRepository
{
    public async Task<Product?> GetAsync(int id, CancellationToken ct) =>
        await cache.GetOrCreateAsync($"product:{id}", _ => inner.GetAsync(id, ct));
}
```

The original repository is unchanged, and caching can be removed by changing one registration.
Cross-cutting concerns — caching, logging, retries, metrics — are natural decorators.

## Pipelines

ASP.NET Core's middleware is OCP at framework scale:

```csharp
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    await next(context);
});
```

Each `Use` adds behaviour around the next step. The same idea appears in `HttpClient`
`DelegatingHandler`s and in MediatR-style pipeline behaviours.

> Patterns are tools for OCP, not goals. If only one implementation will ever exist, the
> abstraction adds indirection without paying for itself.
