---
title: Open for extension, closed for modification
questions:
  - id: meaning
    prompt: What does "closed for modification" mean in practice?
    options:
      - text: The source file is marked read-only in source control
      - text: The class must be declared `sealed`
      - text: The code can never be changed, even to fix bugs
      - id: no-edit
        text: Adding a new variant of behaviour should not require editing existing, tested code
        correct: true
    explanation: Bugs still get fixed. OCP is about extending behaviour by adding new code rather than repeatedly editing a central switch.
  - id: smell
    prompt: Which code shape most often signals an OCP violation?
    options:
      - text: A class with a primary constructor
      - text: An interface with a single method
      - id: switch
        text: A `switch` on a type code that is duplicated across several methods
        correct: true
    explanation: Every new type code forces edits to every copy of the switch. Polymorphism or a lookup of strategies removes that.
  - id: meyer
    prompt: Who first formulated the open/closed principle?
    options:
      - text: Barbara Liskov, in a 1987 keynote
      - id: meyer
        text: Bertrand Meyer, in Object-Oriented Software Construction (1988)
        correct: true
      - text: Martin Fowler, in Refactoring (1999)
    explanation: Meyer described it using inheritance; Robert C. Martin later reframed it around abstractions and polymorphism.
  - id: mechanisms
    prompt: Which C# mechanisms can make code open for extension?
    type: multiple
    options:
      - text: Interfaces with multiple implementations
        correct: true
      - text: Delegates such as `Func<T, TResult>` passed in by the caller
        correct: true
      - text: Registering implementations in a dependency injection container
        correct: true
      - text: Copying the class and editing the copy
    explanation: Interfaces, delegates and DI registrations all let you add behaviour without touching existing code. Copy-and-edit duplicates code instead.
---
## Learning objectives

- Explain the open/closed principle and its history
- Replace type-code switches with polymorphism
- Choose the right extension point instead of making everything pluggable

## The problem

```csharp
public decimal Shipping(Order order) => order.Method switch
{
    "standard" => 4.99m,
    "express"  => 14.99m,
    "pickup"   => 0m,
    _ => throw new ArgumentOutOfRangeException(nameof(order))
};
```

On its own this is fine. It becomes a problem when the same `switch` appears in `EstimatedDays`,
`Label` and `CanShipTo`. Adding "drone" means finding and editing every copy.

## Making it open

```csharp
public interface IShippingMethod
{
    string Code { get; }
    decimal Cost(Order order);
    int EstimatedDays(Address destination);
}

public sealed class ExpressShipping : IShippingMethod
{
    public string Code => "express";
    public decimal Cost(Order order) => 14.99m;
    public int EstimatedDays(Address destination) => 1;
}

public sealed class ShippingCalculator(IEnumerable<IShippingMethod> methods)
{
    private readonly Dictionary<string, IShippingMethod> _byCode =
        methods.ToDictionary(m => m.Code, StringComparer.OrdinalIgnoreCase);

    public decimal Cost(Order order) =>
        _byCode.TryGetValue(order.Method, out var method)
            ? method.Cost(order)
            : throw new UnknownShippingMethodException(order.Method);
}
```

Adding drone delivery is now a new class plus one registration:

```csharp
builder.Services.AddSingleton<IShippingMethod, DroneShipping>();
```

`ShippingCalculator` is **closed**: it never changes. The system is **open**: it gains behaviour.

## Strategic closure

No design is closed against every change. Predict which axis is likely to vary — new shipping
methods, new report formats — and make *that* axis open. Wait for the second or third real
variation before introducing the abstraction; until then, a simple `switch` is easier to read.
