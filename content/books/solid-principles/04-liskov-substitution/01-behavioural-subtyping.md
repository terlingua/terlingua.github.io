---
title: Behavioural subtyping
questions:
  - id: definition
    prompt: What does the Liskov Substitution Principle require?
    options:
      - text: Every subclass must override every virtual method
      - text: Subclasses must not add new methods
      - id: substitutable
        text: Objects of a subtype must be usable wherever the base type is expected without breaking the program's correctness
        correct: true
      - text: A base class must know about all of its subclasses
    explanation: LSP is about behaviour. Code written against the base type must keep working when handed any subtype.
  - id: contracts
    prompt: According to design-by-contract rules for subtypes, which are correct?
    type: multiple
    options:
      - text: A subtype must not strengthen preconditions
        correct: true
      - text: A subtype must not weaken postconditions
        correct: true
      - text: A subtype must preserve the invariants of the base type
        correct: true
      - text: A subtype may throw new kinds of exceptions that callers of the base type do not expect
    explanation: Subtypes may demand less and promise more, never the reverse. Unexpected exceptions break callers.
  - id: square
    prompt: "Why is `Square : Rectangle` (with settable `Width` and `Height`) a classic LSP violation?"
    options:
      - text: Squares have fewer sides than rectangles
      - id: setter
        text: Code that sets width and height independently, then checks the area, breaks for a square
        correct: true
      - text: C# does not allow a class to inherit from a class with properties
    explanation: A mutable square must keep width equal to height, which violates the rectangle's implicit promise that the two vary independently.
  - id: year
    prompt: When did Barbara Liskov introduce the idea in a keynote titled "Data Abstraction and Hierarchy"?
    options:
      - text: "1972"
      - id: y1987
        text: "1987"
        correct: true
      - text: "2000"
    explanation: The 1987 OOPSLA keynote introduced it; Liskov and Jeannette Wing formalised it in 1994.
---
## Learning objectives

- State LSP and its contract rules
- Spot "is-a" relationships that are not behaviourally substitutable
- Use tests to verify substitutability

## The principle

Barbara Liskov introduced the idea in 1987. Informally: **if code works with a base type, it must
keep working with any subtype.** "Is-a" in the real world is not enough — the subtype must keep
the base type's *behavioural* promises.

## The rectangle and the square

```csharp
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : Rectangle
{
    public override int Width  { set { base.Width = value; base.Height = value; } }
    public override int Height { set { base.Width = value; base.Height = value; } }
}

void Resize(Rectangle r)
{
    r.Width = 5;
    r.Height = 4;
    Debug.Assert(r.Area == 20);
}
```

`Resize(new Square())` produces an area of 16. Geometrically a square *is* a rectangle; as a
**mutable** object it does not honour the rectangle's contract.

The fix is to not model it with inheritance — for example, immutable records with a shared
`IShape` interface:

```csharp
public interface IShape { double Area { get; } }
public sealed record Rect(double Width, double Height) : IShape { public double Area => Width * Height; }
public sealed record Sq(double Side) : IShape { public double Area => Side * Side; }
```

## The contract rules

| Rule | Meaning |
|------|---------|
| Preconditions cannot be strengthened | A subtype cannot demand more from callers |
| Postconditions cannot be weakened | A subtype cannot promise less |
| Invariants must be preserved | What is always true of the base stays true |
| No new unexpected exceptions | Callers should not need to catch surprises |
| History constraint | A subtype cannot allow state changes the base forbids |

## Testing substitutability

Write the base type's contract as an abstract test class and run it against every implementation:

```csharp
public abstract class ShapeContractTests
{
    protected abstract IShape Create();

    [Fact]
    public void Area_is_never_negative() => Assert.True(Create().Area >= 0);
}

public sealed class RectTests : ShapeContractTests { protected override IShape Create() => new Rect(2, 3); }
```
