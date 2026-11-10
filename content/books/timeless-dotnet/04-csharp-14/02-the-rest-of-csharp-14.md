---
title: field, null-conditional assignment, spans and the rest
questions:
  - id: field-keyword
    prompt: "What does `field` refer to in `public string Name { get; set => field = value.Trim(); }`?"
    options:
      - id: backing
        text: The compiler-generated backing field of the property
        correct: true
      - text: A new public field named `field`
      - text: The `value` parameter
      - text: A field on the base class
    explanation: The `field` contextual keyword lets you add logic to one accessor without declaring a backing field by hand. If you already have a member named `field`, use `@field` or `this.field` to disambiguate.
  - id: null-cond-assign
    prompt: "What does `customer?.LastSeen = now;` do in C# 14?"
    options:
      - text: Throws if `customer` is null
      - text: Assigns null to `customer`
      - text: It does not compile in any C# version
      - id: assign-if-not-null
        text: Assigns `now` to `LastSeen` only if `customer` is not null; otherwise does nothing, and the right side is not evaluated
        correct: true
    explanation: Null-conditional member access can now appear on the left of assignments (and compound assignments like `+=`). Increment and decrement (`++`, `--`) are not allowed.
  - id: spans
    prompt: What do C# 14's first-class span conversions provide?
    options:
      - text: Spans can now be stored in fields of classes
      - id: implicit
        text: Implicit conversions between arrays, `Span<T>` and `ReadOnlySpan<T>`, so span-based overloads and extension methods apply naturally
        correct: true
      - text: Spans become reference types
      - text: Automatic parallelization of span loops
    explanation: Library authors can now offer a single `ReadOnlySpan<T>` overload, and calls with arrays or strings bind to it, including as extension receivers.
  - id: nameof-unbound
    prompt: What does `nameof(List<>)` produce in C# 14?
    options:
      - text: "`\"List<T>\"`"
      - id: list
        text: "`\"List\"`"
        correct: true
      - text: A compile error, as in earlier versions
      - text: "`\"System.Collections.Generic.List`1\"`"
    explanation: Unbound generic types are now allowed in `nameof`. Previously you had to write `nameof(List<int>)` with an arbitrary type argument.
  - id: lambda-modifiers
    prompt: "Which lambda is newly legal in C# 14 given `delegate bool TryParse<T>(string text, out T result);`?"
    options:
      - text: "`TryParse<int> parse = async (text, out result) => ...;`"
      - id: modifiers-no-types
        text: "`TryParse<int> parse = (text, out result) => int.TryParse(text, out result);`"
        correct: true
      - text: "`TryParse<int> parse = text => true;`"
      - text: None — lambdas still require explicit types for `out` parameters
    explanation: C# 14 allows parameter modifiers such as `out`, `ref`, `in` and `scoped` on lambda parameters without spelling out their types.
  - id: partial-ctors
    prompt: Who benefits most from partial constructors and partial events?
    options:
      - text: Developers writing simple console apps
      - id: generators
        text: Source generators, which can implement a constructor or event that the user declared
        correct: true
      - text: Developers targeting .NET Framework
      - text: Nobody — they are deprecated
    explanation: Partial members let a declaration in your code be implemented in generated code — the pattern behind high-performance logging, regex and interop generators.
  - id: compound-ops
    prompt: What is the benefit of user-defined compound assignment operators such as `public void operator +=(Vector other)`?
    options:
      - text: They are required for all structs
      - text: They replace `IEquatable<T>`
      - id: in-place
        text: The type can update itself in place instead of allocating a new instance for `a = a + b`
        correct: true
      - text: They make operators virtual
    explanation: For large mutable numeric types, matrices or buffers, in-place updates avoid copying and allocation.
---
## Learning objectives

- Use the `field` keyword for validation and normalization
- Use null-conditional assignment
- Know what span conversions, unbound `nameof`, lambda modifiers, partial constructors/events and
  compound operators are for

## The field keyword

Before C# 14, adding one line of logic to an auto-property meant writing the backing field:

```csharp
private string _email = "";
public string Email
{
    get => _email;
    set => _email = value.Trim().ToLowerInvariant();
}
```

Now:

```csharp
public string Email
{
    get;
    set => field = value.Trim().ToLowerInvariant();
}
```

Validation in `init` accessors works the same way, which also protects records against invalid
`with` expressions:

```csharp
public sealed record Percentage
{
    public required decimal Value
    {
        get;
        init => field = value is >= 0m and <= 100m
            ? value
            : throw new ArgumentOutOfRangeException(nameof(Value));
    }
}
```

## Null-conditional assignment

```csharp
if (session is not null)
{
    session.LastActivity = time.GetUtcNow();
}
```

becomes

```csharp
session?.LastActivity = time.GetUtcNow();
```

Use it where "nothing to update" is legitimately normal. If `session` being null is a bug, an
explicit check that throws is more honest.

## First-class spans

```csharp
public static class Checksums
{
    public static int Sum(ReadOnlySpan<byte> data)
    {
        var total = 0;
        foreach (var b in data) total += b;
        return total;
    }
}

byte[] payload = File.ReadAllBytes("message.bin");
var sum = Checksums.Sum(payload);
```

One overload serves arrays, `Span<byte>`, slices and stack memory alike.

## Unbound generic `nameof`

```csharp
logger.LogDebug("Resolving {Type}", nameof(IRepository<>));
```

## Lambda parameter modifiers

```csharp
TryParse<decimal> parse = (text, out result) => decimal.TryParse(text, CultureInfo.InvariantCulture, out result);
```

## Partial constructors and events

```csharp
public partial class Telemetry
{
    public partial Telemetry(ILogger logger);
    public partial event EventHandler<Measurement>? Measured;
}
```

A source generator supplies the implementing declarations.

## User-defined compound assignment

```csharp
public sealed class Histogram
{
    private readonly long[] _buckets = new long[64];

    public void operator +=(int sample) => _buckets[Math.Clamp(sample, 0, 63)]++;
}

histogram += latencyMs;
```

## Sources

- Microsoft Learn, "What's new in C# 14" — <https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14>
- Microsoft Learn, "field keyword" — <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/field>
- dotnet/csharplang proposals — <https://github.com/dotnet/csharplang/tree/main/proposals>
