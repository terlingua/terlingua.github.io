---
title: Role interfaces in practice
questions:
  - id: cqrs
    prompt: A repository interface has 15 members, but a reporting page only reads data. Which ISP-friendly refactoring helps most?
    options:
      - text: Add five more methods so the interface is complete
      - id: split-read
        text: Introduce a small read-only interface that the reporting page depends on
        correct: true
      - text: Make every method `virtual`
    explanation: Splitting reads from writes gives each client exactly what it needs, a light form of command–query separation.
  - id: bcl
    prompt: Which .NET interfaces are examples of small, role-based design?
    type: multiple
    options:
      - text: "`IDisposable`"
        correct: true
      - text: "`IComparable<T>`"
        correct: true
      - text: "`IAsyncEnumerable<T>`"
        correct: true
    explanation: Each describes one capability, which is why so many unrelated types implement them.
  - id: too-small
    prompt: What is the risk of taking ISP to the extreme with one interface per method everywhere?
    options:
      - text: The code will no longer compile
      - text: Interfaces with one method cannot be mocked
      - id: fragmentation
        text: Related operations become fragmented and constructors need many tiny dependencies
        correct: true
    explanation: Interfaces should match how clients actually use them. Operations that are always used together belong together.
---
## Learning objectives

- Split repositories and services by how they are used
- Recognise role interfaces in the base class library
- Avoid over-fragmentation

## Read and write roles

```csharp
public interface IOrderReader
{
    Task<OrderDto?> FindAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<OrderSummary>> ListForCustomerAsync(int customerId, CancellationToken ct);
}

public interface IOrderWriter
{
    Task<int> AddAsync(Order order, CancellationToken ct);
    Task CancelAsync(int id, CancellationToken ct);
}

public sealed class SqlOrderStore : IOrderReader, IOrderWriter { /* ... */ }
```

The reporting page asks for `IOrderReader`. It cannot accidentally write, and its tests only need
a fake with two methods.

## Interfaces in the base class library

The BCL is full of tiny role interfaces: `IDisposable` (one method), `IAsyncDisposable`,
`IComparable<T>`, `IEquatable<T>`, `IEnumerable<T>`. Their smallness is exactly why so many types
can implement them and so much generic code can rely on them.

## Interfaces shaped for consumers

```csharp
public sealed class InvoiceEmailer(IOrderReader orders, IEmailSender email)
{
    public async Task SendAsync(int orderId, CancellationToken ct)
    {
        var order = await orders.FindAsync(orderId, ct) ?? throw new OrderNotFoundException(orderId);
        await email.SendAsync(order.CustomerEmail, "Your invoice", InvoiceTemplate.Render(order), ct);
    }
}
```

## Balance

Do not create an interface per method by reflex. Group members by the clients that use them
together. A good test: could you describe the interface's role in a short noun phrase —
"order reader", "email sender"? If so, it is probably the right size.
