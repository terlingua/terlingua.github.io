---
title: Abstractions and polymorphism in C#
questions:
  - id: interface-vs-abstract
    prompt: Which statement about C# interfaces and abstract classes is correct?
    options:
      - text: A class can inherit from several abstract classes
      - text: Interfaces cannot declare methods with a body in modern C#
      - id: multi-interface
        text: A class can implement many interfaces but inherit from only one class
        correct: true
      - text: Abstract classes cannot contain implemented methods
    explanation: C# has single class inheritance and multiple interface implementation. Since C# 8, interfaces may also contain default implementations.
  - id: polymorphism
    prompt: What does polymorphism let a caller do?
    options:
      - id: same-call
        text: Use one call site that behaves differently depending on the runtime type of the object
        correct: true
      - text: Call private methods of another class
      - text: Avoid declaring types for variables
    explanation: The caller depends only on the abstraction; the concrete type decides what actually happens.
  - id: good-abstraction
    prompt: Which are signs of a good abstraction?
    type: multiple
    options:
      - text: It is named after what callers need, not after how it is implemented
        correct: true
      - text: It exposes implementation details such as SQL connection strings
      - text: Several different implementations could reasonably satisfy it
        correct: true
      - text: It is small enough to be understood quickly
        correct: true
    explanation: Good abstractions are expressed in the caller's language, hide details and are small enough to implement in more than one way.
---
Every SOLID principle relies on two language features: **abstractions** (interfaces and abstract
classes) and **polymorphism** (calling through an abstraction and letting the runtime type decide).

## Learning objectives

- Choose between an interface and an abstract class
- Use polymorphism to remove type-checking conditionals
- Recognise what makes an abstraction useful

## Interfaces and abstract classes

```csharp
public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

public abstract class Shape
{
    public abstract double Area();
    public override string ToString() => $"{GetType().Name} with area {Area():F2}";
}

public sealed class Circle(double radius) : Shape
{
    public override double Area() => Math.PI * radius * radius;
}
```

| Use an interface when… | Use an abstract class when… |
|------------------------|-----------------------------|
| You describe a capability (`IComparable`, `IDisposable`) | You share real implementation between related types |
| Unrelated types may implement it | Types form a genuine "is-a" family |
| You want a type to have several roles | You need constructors or protected state |

Since C# 8, interfaces may carry **default interface methods**, but they are best kept for
evolving published interfaces, not as a substitute for base classes.

## Polymorphism replaces conditionals

```csharp
double TotalArea(IEnumerable<Shape> shapes) => shapes.Sum(s => s.Area());
```

Compare that with a version that switches on a `Kind` property: every new shape forces a change
to every switch. Polymorphism moves the varying behaviour into the type that owns it.

## What makes a good abstraction

- It is named after the caller's need (`IOrderRepository`, `IClock`), not the technology (`ISqlHelper`)
- It is small — callers can understand it at a glance
- More than one implementation is plausible (including a test double)

> .NET 8 introduced `TimeProvider`, a built-in abstraction for time. Prefer it over hand-rolled
> `IClock` interfaces in new code.
