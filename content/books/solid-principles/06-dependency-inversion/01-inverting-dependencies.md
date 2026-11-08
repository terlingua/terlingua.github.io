---
title: Inverting dependencies
questions:
  - id: definition
    prompt: Which pair of statements makes up the Dependency Inversion Principle?
    options:
      - text: Classes should create their own dependencies; dependencies should be static.
      - id: dip
        text: High-level modules should not depend on low-level modules; both should depend on abstractions. Abstractions should not depend on details; details should depend on abstractions.
        correct: true
      - text: Every class must be registered in a container; every constructor must take an interface.
    explanation: This is Robert C. Martin's two-part formulation.
  - id: ownership
    prompt: In a design that follows DIP, which layer should own the `IOrderRepository` interface?
    options:
      - text: The infrastructure layer that contains the SQL implementation
      - text: A shared "Interfaces" project referenced by everything
      - id: application
        text: The domain or application layer that uses it
        correct: true
    explanation: The inversion is about source-code dependency direction. The policy layer defines the abstraction; the detail implements it and depends on the policy layer.
  - id: not-same
    prompt: Which statements are true?
    type: multiple
    options:
      - text: Dependency injection is a technique that helps implement dependency inversion
        correct: true
      - text: You can follow DIP without using a DI container
        correct: true
      - text: Using a DI container automatically means you follow DIP
      - text: "`new`-ing up a stable type like `List<T>` violates DIP"
    explanation: A container is optional plumbing. DIP is about the direction of dependencies on volatile details; stable framework types are fine to depend on directly.
---
## Learning objectives

- State the dependency inversion principle
- Explain which layer owns an abstraction
- Distinguish DIP, dependency injection and DI containers

## The problem

```csharp
public sealed class CheckoutService
{
    private readonly SqlOrderRepository _orders = new("Server=prod;...");
    private readonly StripeGateway _payments = new("sk_live_...");
}
```

Checkout — the business policy — depends on SQL Server and Stripe. To test it you need both; to
change payment providers you must edit the business logic.

## The principle

> High-level modules should not depend on low-level modules. Both should depend on abstractions.
> Abstractions should not depend on details. Details should depend on abstractions.

```csharp
namespace Shop.Application;

public interface IOrderRepository { Task AddAsync(Order order, CancellationToken ct); }
public interface IPaymentGateway { Task<PaymentResult> ChargeAsync(Money amount, CardToken card, CancellationToken ct); }

public sealed class CheckoutService(IOrderRepository orders, IPaymentGateway payments)
{
    public async Task CheckoutAsync(Cart cart, CardToken card, CancellationToken ct)
    {
        var result = await payments.ChargeAsync(cart.Total, card, ct);
        if (!result.Succeeded) throw new PaymentDeclinedException(result.Reason);
        await orders.AddAsync(Order.From(cart, result.TransactionId), ct);
    }
}
```

```csharp
namespace Shop.Infrastructure;

public sealed class StripePaymentGateway(StripeClient client) : Application.IPaymentGateway { /* ... */ }
```

## Why "inversion"

Without DIP, source-code dependencies point the same way as the flow of control: policy → detail.
With DIP, the interface lives *with the policy*, so the infrastructure project references the
application project — the source-code arrow is **inverted** against the flow of control.

## DIP, DI and containers

| Term | What it is |
|------|------------|
| Dependency inversion | A design principle about dependency *direction* |
| Dependency injection | A technique: pass dependencies in, usually through the constructor |
| DI container | A library that builds object graphs (e.g. `Microsoft.Extensions.DependencyInjection`) |

You can follow DIP with plain constructor calls in `Main`. A container is a convenience.

> Do not abstract everything. `List<T>`, `string` and `DateOnly` are stable; depend on them
> directly. Invert dependencies on things that are **volatile**: databases, networks, clocks,
> file systems, third-party services.
