---
title: LSP in the .NET libraries
questions:
  - id: readonly-collection
    prompt: "`ReadOnlyCollection<T>` implements `ICollection<T>`. What happens when you call `Add` on it through the `ICollection<T>` interface?"
    options:
      - text: The item is added silently
      - id: throws
        text: It throws `NotSupportedException`
        correct: true
      - text: It does nothing and returns false
      - text: It fails to compile
    explanation: "The interface promises `Add`, but the implementation refuses it. Callers must check `IsReadOnly` — a widely cited LSP smell in the BCL."
  - id: stream
    prompt: How does `System.IO.Stream` reduce surprises for callers when not every stream supports every operation?
    options:
      - text: It forbids subclasses from throwing any exception
      - text: It provides separate abstract classes for every combination of capabilities
      - id: capability
        text: It exposes capability properties such as `CanRead`, `CanWrite` and `CanSeek` as part of its contract
        correct: true
    explanation: Because the capability checks are part of the documented contract, a caller who honours them is not surprised. Splitting into smaller interfaces would be the ISP-style alternative.
  - id: fixes
    prompt: Which are good ways to fix a subtype that cannot support an inherited operation?
    type: multiple
    options:
      - text: Split the base interface so the subtype only implements what it supports
        correct: true
      - text: Replace inheritance with composition
        correct: true
      - text: Override the method to throw `NotImplementedException` and hope nobody calls it
    explanation: Throwing from an inherited member pushes the problem onto every caller. Narrower interfaces or composition remove it at the source.
---
## Learning objectives

- Recognise LSP trade-offs in the base class library
- Distinguish "refused bequest" from legitimate capability checks
- Fix violations with narrower interfaces or composition

## A famous compromise

```csharp
ICollection<int> numbers = new List<int> { 1, 2, 3 }.AsReadOnly();
numbers.Add(4); // NotSupportedException
```

`ICollection<T>` has an `IsReadOnly` property, so technically the contract allows refusal. In
practice, many callers never check it, which is why later versions of .NET added
`IReadOnlyCollection<T>` and `IReadOnlyList<T>`. Prefer those in your own APIs:

```csharp
public IReadOnlyList<OrderLine> Lines => _lines;
```

## Capability properties

`Stream` exposes `CanRead`, `CanWrite`, `CanSeek` and `CanTimeout`. A network stream throws
`NotSupportedException` from `Seek`, but only after declaring `CanSeek == false`. Code that
honours the capability flags keeps working with any stream, so the contract holds.

## Refused bequest

When a subclass inherits a member it does not want and throws from it, that is a **refused
bequest**:

```csharp
public class Bird { public virtual void Fly() { } }
public class Penguin : Bird
{
    public override void Fly() => throw new NotSupportedException();
}
```

## Fixing it

```csharp
public abstract class Bird { public abstract void Eat(); }
public interface IFlyer { void Fly(); }

public sealed class Sparrow : Bird, IFlyer { public override void Eat() { } public void Fly() { } }
public sealed class Penguin : Bird { public override void Eat() { } }
```

Code that needs flight asks for `IFlyer`. The compiler now prevents the bug that the exception
used to report at runtime — and that is LSP and ISP working together.
