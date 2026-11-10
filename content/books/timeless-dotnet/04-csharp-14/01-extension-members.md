---
title: Extension members
questions:
  - id: what-new
    prompt: What do C# 14 extension members add beyond classic `this`-parameter extension methods?
    type: multiple
    options:
      - id: props
        text: Extension properties
        correct: true
      - id: static
        text: Static extension members that appear to belong to the type itself, such as `Money.Zero`
        correct: true
      - id: block
        text: An `extension(Receiver r) { ... }` block that groups members for one receiver
        correct: true
      - text: The ability to add instance fields to an existing type
    explanation: Extensions still cannot add state. They add members that are resolved at compile time, so no storage can be attached to the receiver.
  - id: compat
    prompt: Are existing `this`-parameter extension methods compatible with C# 14?
    options:
      - text: No, they must be rewritten as extension blocks
      - text: Only in .NET Framework
      - id: compatible
        text: Yes — classic extension methods keep working, and the new syntax can generate source- and binary-compatible methods
        correct: true
      - text: Only if marked `[Obsolete]`
    explanation: The language team designed the feature so that libraries can move to the new syntax without breaking callers.
  - id: receiver-name
    prompt: "In `extension(Order order) { public bool IsLarge => order.Total > 1000m; }`, what is `order`?"
    options:
      - text: A field on the static class
      - text: A new instance created for each call
      - text: A static property
      - id: receiver
        text: The receiver parameter — the instance the property is called on
        correct: true
    explanation: The receiver is declared once on the block and is in scope for every instance member in it.
  - id: state
    prompt: A teammate wants an extension property `public string Nickname { get; set; }` on `Customer` that stores a value. What is the problem?
    options:
      - text: Extension properties must be `static`
      - text: Extension properties cannot have getters
      - id: no-storage
        text: Extension members cannot add storage to the receiver; there is nowhere for the value to live
        correct: true
      - text: There is no problem
    explanation: Extension properties are computed. Attaching data needs a real member, a wrapper, or (for advanced cases) `ConditionalWeakTable`.
  - id: when
    prompt: When is an extension member the right tool?
    type: multiple
    options:
      - id: not-yours
        text: Adding convenience members to types you do not own (BCL, third-party, generated code)
        correct: true
      - id: mapping
        text: Explicit mapping between layers, such as `order.ToDto()`
        correct: true
      - id: interface-helpers
        text: Adding helpers to an interface without forcing every implementation to write them
        correct: true
      - text: Hiding a dependency on a database inside a "pure" extension property
    explanation: Extensions are syntactic sugar for static methods. Hiding I/O behind something that looks like a property violates the reader's expectations badly.
---
## Learning objectives

- Write extension blocks with properties, methods and static members
- Know what extensions cannot do
- Use extensions for mapping and fluent APIs

## From extension methods to extension members

Classic extension methods (C# 3, 2007) only allowed methods:

```csharp
public static class OrderExtensions
{
    public static bool IsLarge(this Order order) => order.Total > 1000m;
}
```

C# 14 introduces extension blocks:

```csharp
public static class OrderExtensions
{
    extension(Order order)
    {
        public bool IsLarge => order.Total > 1000m;

        public bool IsOverdue(TimeProvider time) =>
            order.DueDate < DateOnly.FromDateTime(time.GetUtcNow().UtcDateTime) && !order.IsPaid;

        public OrderSummary ToSummary() => new(order.Id, order.CustomerName, order.Total);
    }

    extension(Order)
    {
        public static Order Empty(CustomerId customer) => new(OrderId.New(), customer, []);
    }
}
```

Callers write `order.IsLarge`, `order.ToSummary()` and `Order.Empty(customerId)`.

## Extending interfaces and sequences

```csharp
public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public bool IsEmpty => !source.Any();

        public IEnumerable<T[]> InBatchesOf(int size) => source.Chunk(size);
    }
}

if (pendingEmails.IsEmpty) return;
foreach (var batch in pendingEmails.InBatchesOf(100))
{
    await sender.SendBatchAsync(batch, ct);
}
```

## Real-world scenario: a value-object API you do not own

Your company's shared library exposes a `Money` type you cannot change. You need
`Money.Zero(currency)` and `money.IsZero` everywhere:

```csharp
public static class MoneyExtensions
{
    extension(Money)
    {
        public static Money Zero(string currency) => new(0m, currency);
    }

    extension(Money money)
    {
        public bool IsZero => money.Amount == 0m;
    }
}
```

## Limits worth remembering

- No instance state: extensions cannot add fields
- Resolution is static: no virtual dispatch on the receiver's runtime type
- A real member on the type always wins over an extension with the same signature
- Discoverability depends on `using` directives — keep extensions in predictable namespaces

## Sources

- Microsoft Learn, "What's new in C# 14" — <https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14>
- Microsoft Learn, "Extension members" — <https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods>
