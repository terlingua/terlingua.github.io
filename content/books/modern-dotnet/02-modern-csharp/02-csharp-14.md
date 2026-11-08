---
title: What's new in C# 14
questions:
  - id: field
    prompt: "In C# 14, what does the `field` keyword refer to inside a property accessor?"
    options:
      - text: The first field declared in the class
      - text: A static field shared by all instances
      - text: A database column mapped by EF Core
      - id: backing
        text: The compiler-generated backing field of that property
        correct: true
    explanation: "`field` lets you add logic to one accessor without declaring an explicit backing field."
  - id: extension-blocks
    prompt: What do C# 14 extension blocks (`extension(T receiver) { ... }`) add over classic extension methods?
    options:
      - text: Nothing; they are a new syntax for the same thing
      - text: The ability to add fields to existing types
      - id: properties
        text: The ability to declare extension properties and static extension members as well as methods
        correct: true
    explanation: Extension members can be instance or static properties and methods. They still cannot add state (fields) to a type.
  - id: null-cond-assign
    prompt: "What does `customer?.Name = \"Ada\";` do in C# 14?"
    options:
      - text: Throws if `customer` is null
      - id: conditional
        text: Assigns only when `customer` is not null; the right-hand side is not evaluated otherwise
        correct: true
      - text: Creates a new customer if it is null
    explanation: Null-conditional assignment evaluates the right-hand side only if the receiver is non-null.
  - id: other-features
    prompt: Which of these are C# 14 features?
    type: multiple
    options:
      - text: "`nameof` with unbound generic types, such as `nameof(List<>)`"
        correct: true
      - text: Implicit conversions between arrays, `Span<T>` and `ReadOnlySpan<T>`
        correct: true
      - text: Partial constructors and partial events
        correct: true
      - text: Discriminated union types with a `union` keyword
    explanation: Union types were not part of C# 14.
---
C# 14 ships with .NET 10. These features remove boilerplate that C# developers have written for years.

## Learning objectives

- Use the `field` keyword
- Declare extension properties and static extension members
- Apply null-conditional assignment and other C# 14 conveniences

## The `field` keyword

```csharp
public sealed class Customer
{
    public string Name
    {
        get;
        set => field = value?.Trim() ?? throw new ArgumentNullException(nameof(value));
    }
}
```

Before C# 14 you needed a private `_name` field just to add the trimming logic.

## Extension members

```csharp
public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public bool IsEmpty => !source.Any();

        public IEnumerable<T> WhereNotNull() => source.Where(x => x is not null);
    }

    extension<T>(IEnumerable<T>)
    {
        public static IEnumerable<T> Empty => [];
    }
}

var names = new[] { "Ada", "Grace" };
Console.WriteLine(names.IsEmpty);                 // extension property
var none = IEnumerable<string>.Empty;             // static extension member
```

Classic `this`-parameter extension methods still work and are source- and binary-compatible.

## Null-conditional assignment

```csharp
order?.ShippedAt = time.GetUtcNow();
settings?.Retries += 1;
```

## Smaller improvements

```csharp
Console.WriteLine(nameof(Dictionary<,>));         // "Dictionary"

ReadOnlySpan<int> span = new[] { 1, 2, 3 };        // implicit span conversion

TryParse<int> parse = (text, out result) => int.TryParse(text, out result); // modifiers without types
delegate bool TryParse<T>(string text, out T result);
```

- **Partial constructors and events** — useful for source generators
- **User-defined compound assignment operators** — types can define `operator +=` to update in place
  instead of allocating a new instance

## Recent history at a glance

| Version | .NET | Highlights |
|---------|------|------------|
| C# 11 | 7 | `required`, raw string literals, list patterns, generic math |
| C# 12 | 8 | Primary constructors, collection expressions, `using` alias for any type |
| C# 13 | 9 | `params` collections, `System.Threading.Lock`, partial properties |
| C# 14 | 10 | Extension members, `field`, null-conditional assignment |
