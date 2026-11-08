---
title: Refactoring toward SRP
questions:
  - id: first-step
    prompt: Before splitting a large class with mixed responsibilities, what should you have in place?
    options:
      - text: A new database schema
      - text: A microservice for each responsibility
      - id: tests
        text: Tests that pin down its current behaviour
        correct: true
      - text: A dependency injection container
    explanation: Characterisation tests let you refactor with confidence that behaviour has not changed.
  - id: facade
    prompt: After extracting responsibilities, existing callers still call `OrderProcessor.Process`. What is the role of `OrderProcessor` now?
    options:
      - text: It is dead code that should be deleted
      - id: coordinator
        text: A thin coordinator that delegates to focused collaborators
        correct: true
      - text: A violation of SRP because it calls several classes
    explanation: Orchestrating focused collaborators is itself a single responsibility — the workflow.
  - id: over-split
    prompt: Which are risks of splitting responsibilities too aggressively?
    type: multiple
    options:
      - text: Logic that always changes together ends up scattered across many files
        correct: true
      - text: Readers must jump through many tiny classes to understand one behaviour
        correct: true
      - text: The compiler refuses to build classes with fewer than three methods
    explanation: Over-splitting lowers cohesion. Keep together what changes together.
---
Knowing the principle is easy; applying it to a 900-line class is the real skill.

## Learning objectives

- Refactor a mixed-responsibility class in small, safe steps
- Keep an orchestrating class thin
- Recognise when splitting has gone too far

## The starting point

```csharp
public class OrderProcessor
{
    public async Task ProcessAsync(Order order)
    {
        if (order.Lines.Count == 0) throw new InvalidOperationException("Empty order");
        var total = order.Lines.Sum(l => l.Price * l.Quantity);
        if (order.Customer.IsVip) total *= 0.9m;

        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO Orders ...", new { order.Id, total });

        using var smtp = new SmtpClient("mail.example.com");
        await smtp.SendMailAsync("shop@example.com", order.Customer.Email, "Thanks", $"Total {total}");
    }
}
```

Validation, pricing, persistence and notification all live together.

## Step 1 — pin the behaviour

Write characterisation tests around `ProcessAsync` (with a test database or by first extracting
seams). Refactoring without tests is just editing.

## Step 2 — extract one responsibility at a time

```csharp
public interface IPricingPolicy { decimal Total(Order order); }
public interface IOrderRepository { Task AddAsync(Order order, decimal total, CancellationToken ct); }
public interface IOrderNotifier { Task OrderPlacedAsync(Order order, decimal total, CancellationToken ct); }

public sealed class OrderProcessor(
    IPricingPolicy pricing,
    IOrderRepository orders,
    IOrderNotifier notifier)
{
    public async Task ProcessAsync(Order order, CancellationToken ct = default)
    {
        OrderValidator.EnsureValid(order);
        var total = pricing.Total(order);
        await orders.AddAsync(order, total, ct);
        await notifier.OrderPlacedAsync(order, total, ct);
    }
}
```

`OrderProcessor` still exists, but its only job is the **workflow**. Each collaborator can be
tested and replaced on its own.

## Knowing when to stop

Stop splitting when the pieces would always change together. A `PricingPolicy` that contains the
VIP discount, the volume discount and rounding rules is cohesive: all of it belongs to the people
who own pricing.

> Rule of thumb: separate code that changes for different reasons; keep together code that
> changes for the same reason. Both halves of the rule matter.
