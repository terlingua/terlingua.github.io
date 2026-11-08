---
title: Immutability and encapsulation
questions:
  - id: with
    prompt: "Given `public record Point(int X, int Y);` and `var p = new Point(1, 2);`, what does `p with { Y = 5 }` do?"
    options:
      - text: Changes `p.Y` to 5
      - text: Fails to compile because positional records are immutable
      - id: copy
        text: Creates a new `Point(1, 5)` and leaves `p` unchanged
        correct: true
    explanation: A `with` expression creates a modified copy. Positional record properties are `init`-only.
  - id: value-object
    prompt: Which are characteristics of a value object such as `Money` or `EmailAddress`?
    type: multiple
    options:
      - text: Equality is based on its values rather than identity
        correct: true
      - text: It is immutable
        correct: true
      - text: It validates itself on construction so invalid instances cannot exist
        correct: true
      - text: It has a database identity column
    explanation: Value objects are defined by their values, are immutable and are always valid.
  - id: expose-list
    prompt: "An `Order` exposes `public List<OrderLine> Lines { get; } = new();`. What is the problem?"
    options:
      - text: Lists cannot be used as properties
      - text: There is no problem because the property has no setter
      - id: mutate
        text: Callers can add or remove lines directly, bypassing any rules in `Order`
        correct: true
    explanation: A getter-only property still returns a mutable list. Expose `IReadOnlyList<OrderLine>` and add lines through a method.
---
## Learning objectives

- Use records and `init` for immutable data
- Model value objects that are always valid
- Protect collections inside aggregates

## Why immutability

Immutable objects are thread-safe, easy to reason about and impossible to corrupt after
construction. In C#, records make them cheap to write:

```csharp
public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public Money Add(Money other) =>
        other.Currency == Currency
            ? this with { Amount = Amount + other.Amount }
            : throw new CurrencyMismatchException(Currency, other.Currency);
}
```

Records provide value-based equality, so `new Money(5, "EUR") == new Money(5, "EUR")` is `true`.

## Always-valid value objects

```csharp
public sealed record EmailAddress
{
    public string Value { get; }

    private EmailAddress(string value) => Value = value;

    public static EmailAddress Parse(string input)
    {
        var trimmed = input?.Trim() ?? throw new ArgumentNullException(nameof(input));
        if (!MailAddress.TryCreate(trimmed, out _)) throw new FormatException($"'{input}' is not a valid e-mail address");
        return new EmailAddress(trimmed.ToLowerInvariant());
    }

    public override string ToString() => Value;
}
```

Once you hold an `EmailAddress`, you never need to validate it again. This replaces "primitive
obsession" — passing raw `string`s everywhere and validating them repeatedly.

## Encapsulating collections

```csharp
public sealed class Order
{
    private readonly List<OrderLine> _lines = [];
    public IReadOnlyList<OrderLine> Lines => _lines;

    public void AddLine(Product product, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        if (_lines.Count >= 100) throw new OrderTooLargeException();
        _lines.Add(new OrderLine(product.Sku, product.Price, quantity));
    }
}
```

The order controls every change to its lines, so its rules always hold.
