---
title: Records — value semantics on purpose
questions:
  - id: equality
    prompt: "Given `public record Money(decimal Amount, string Currency);`, what does `new Money(10m, \"EUR\") == new Money(10m, \"EUR\")` return?"
    options:
      - text: "`false`, because they are different objects"
      - text: It does not compile
      - id: true-value
        text: "`true`, because records compare by the values of their members"
        correct: true
      - text: It throws `InvalidOperationException`
    explanation: Records generate `Equals`, `GetHashCode`, `==` and `!=` that compare member values. `record` is a reference type (`record class`), so `ReferenceEquals` would still be `false`.
  - id: with
    prompt: "What does `var discounted = price with { Amount = price.Amount * 0.9m };` do?"
    options:
      - text: Mutates `price` in place
      - id: copy
        text: Creates a shallow copy of `price` with `Amount` replaced; `price` is unchanged
        correct: true
      - text: Creates a deep copy of every nested object
      - text: Only works on structs
    explanation: "Non-destructive mutation. It is a *shallow* copy: reference-type members (like a `List<T>`) are shared between the original and the copy."
  - id: list-equality
    prompt: "`public record Basket(string Owner, List<string> Items);` — two baskets with the same owner and lists containing the same strings, but different list instances. Are they equal?"
    options:
      - text: Yes, records compare collections element by element
      - id: no-reference
        text: No — `List<string>` uses reference equality, so the generated `Equals` sees two different lists
        correct: true
      - text: Only if the lists are sorted
      - text: It depends on the culture
    explanation: Record equality delegates to each member's `Equals`. Collections in records are a common trap; use an immutable collection with value semantics you write yourself, or override equality, or keep collections out of value-object records.
  - id: ef-entities
    prompt: Why are records usually a poor fit for EF Core entities?
    type: multiple
    options:
      - id: identity
        text: Entities have identity (two rows with equal data are still different rows); records define equality by value
        correct: true
      - id: tracking
        text: EF Core's change tracker relies on reference identity and mutation, while records encourage replacing instances with `with`
        correct: true
      - text: EF Core cannot read records at all
      - id: hashcode
        text: Mutable members participating in `GetHashCode` can break `HashSet`/`Dictionary` lookups as the entity changes
        correct: true
    explanation: Records are excellent for DTOs, messages, value objects and query results. Entities are about identity over time — a different concept.
  - id: positional
    prompt: For a *positional* `record class`, what does the compiler generate for each positional parameter?
    options:
      - text: A public field
      - text: A private readonly field only
      - text: A public `set` property
      - id: init-prop
        text: A public `init`-only property, plus a constructor and a `Deconstruct` method
        correct: true
    explanation: "`record struct` positional parameters generate *mutable* (`get; set;`) properties unless you declare `readonly record struct`."
  - id: tostring
    prompt: What does a record's generated `ToString()` produce, and what is a real-world risk?
    options:
      - text: Only the type name, like a class
      - text: JSON
      - text: An empty string
      - id: members-leak
        text: "Something like `User { Email = a@b.com, PasswordHash = ... }` — every public member, which can leak secrets into logs"
        correct: true
    explanation: The printing is great for debugging and tests, and dangerous for records holding secrets or personal data. Override `PrintMembers` or keep sensitive values out of records that might be logged.
  - id: when-records
    prompt: Which are good uses of records?
    type: multiple
    options:
      - id: dto
        text: API request and response DTOs
        correct: true
      - id: messages
        text: Commands, events and messages
        correct: true
      - id: value-objects
        text: Small value objects such as `Money`, `DateRange` or `EmailAddress`
        correct: true
      - text: Long-lived services registered in DI
    explanation: Services have behavior and dependencies, not values. Records shine where the *data* is the identity.
  - id: sealed-record
    prompt: Why might you declare `public sealed record OrderPlaced(...)`?
    options:
      - text: Sealed records cannot be serialized
      - text: To make the record a value type
      - text: Because records cannot have methods otherwise
      - id: equality-contract
        text: To prevent derived records, which simplifies the equality contract (`EqualityContract`) and states intent
        correct: true
    explanation: Record inheritance works but complicates equality (a derived record is never equal to its base). Most DTOs and messages should be sealed.
---
## Learning objectives

- Explain what the compiler generates for a record
- Use `with` and positional records correctly
- Avoid the collection, entity and logging traps

## What a record is

A record is a class (or struct) for which the compiler writes the boring parts: value-based
equality, `GetHashCode`, `ToString`, a copy constructor for `with`, and — for positional
records — properties, a constructor and `Deconstruct`.

```csharp
public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public Money Add(Money other) =>
        other.Currency == Currency
            ? this with { Amount = Amount + other.Amount }
            : throw new InvalidOperationException($"Cannot add {other.Currency} to {Currency}");
}
```

## The four flavors

| Declaration | Kind | Positional properties | Equality |
|---|---|---|---|
| `record` / `record class` | reference | `init` | value |
| `readonly record struct` | value | `init` | value |
| `record struct` | value | `get; set;` | value |
| `class` | reference | — | reference |

## Validation in records

Positional records are concise, but validation needs a little more:

```csharp
public sealed record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (!value.Contains('@')) throw new ArgumentException("Not an email address", nameof(value));
        Value = value.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;
}
```

Note that `with` bypasses the constructor and copies fields directly. If invariants matter,
validate in property `init` accessors (C# 14's `field` keyword makes this concise — see Chapter 4),
or avoid exposing `with`-able properties.

## The collection trap

```csharp
public sealed record Basket(string Owner, IReadOnlyList<string> Items);

var a = new Basket("ada", ["tea", "milk"]);
var b = new Basket("ada", ["tea", "milk"]);
Console.WriteLine(a == b);
```

This prints `False`. One fix is to implement equality for the collection member:

```csharp
public sealed record Basket(string Owner, IReadOnlyList<string> Items)
{
    public bool Equals(Basket? other) =>
        other is not null && Owner == other.Owner && Items.SequenceEqual(other.Items);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Owner);
        foreach (var item in Items) hash.Add(item);
        return hash.ToHashCode();
    }
}
```

## The logging trap

```csharp
public sealed record LoginRequest(string Email, string Password);

logger.LogInformation("Login attempt {Request}", request);
```

The log now contains the password. Real incident reports contain this exact bug. Mitigate by
logging specific fields, or by overriding the printer:

```csharp
public sealed record LoginRequest(string Email, string Password)
{
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"Email = {Email}, Password = ***");
        return true;
    }
}
```

## Real-world scenario: idempotent message handling

Records make "have I seen this message?" trivial when the message *is* its data:

```csharp
public sealed record InvoicePaid(Guid InvoiceId, decimal Amount, DateOnly PaidOn);

private readonly HashSet<InvoicePaid> _processed = [];

public void Handle(InvoicePaid message)
{
    if (!_processed.Add(message)) return;
    Apply(message);
}
```

## Sources

- Microsoft Learn, "Records (C# reference)" — <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record>
- Microsoft Learn, "Use record types" tutorial — <https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/records>
