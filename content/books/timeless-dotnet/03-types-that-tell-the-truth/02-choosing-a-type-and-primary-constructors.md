---
title: Choosing a type, and primary constructors
questions:
  - id: pc-readonly
    prompt: "In `public sealed class OrderService(IOrderStore store) { ... }`, is the captured parameter `store` read-only?"
    options:
      - id: mutable
        text: No — in a class or struct primary constructor the parameter is a mutable captured variable; it can be reassigned inside the class
        correct: true
      - text: Yes, primary constructor parameters are always `readonly` fields
      - text: Only if the class is sealed
      - text: It is a public property
    explanation: This surprises many developers. If immutability matters, assign it to an explicit `private readonly` field, or rely on the analyzer warning when you reassign it.
  - id: pc-vs-record
    prompt: How do primary constructors on a *class* differ from positional parameters on a *record*?
    options:
      - text: There is no difference
      - text: Classes generate `Deconstruct`; records do not
      - id: no-properties
        text: A class's primary constructor parameters do not become public properties; a record's positional parameters do
        correct: true
      - text: Records cannot have primary constructors
    explanation: For classes, primary constructor parameters are simply in scope for the body — ideal for dependency injection. For records, they define the public shape of the data.
  - id: struct-when
    prompt: When is a `struct` (or `readonly record struct`) a better choice than a class?
    type: multiple
    options:
      - id: small
        text: The type is small (a few fields) and represents a single value
        correct: true
      - id: immutable
        text: It is logically immutable
        correct: true
      - id: many-instances
        text: Large numbers are created in hot paths and heap allocations matter
        correct: true
      - text: It needs inheritance
    explanation: Structs avoid heap allocation but are copied on assignment. Large or mutable structs cause subtle bugs and extra copying.
  - id: default-struct
    prompt: What is a real hazard of value types such as `public readonly record struct OrderId(Guid Value);`?
    options:
      - text: They cannot be used as dictionary keys
      - id: default
        text: "`default(OrderId)` exists and bypasses any constructor validation, so `Guid.Empty` can sneak in"
        correct: true
      - text: They are always slower than classes
      - text: They cannot implement interfaces
    explanation: Every struct has a default value. Arrays, uninitialized fields and `default` all produce it. Design strongly-typed ids so that the default is detectable or harmless.
  - id: required
    prompt: What does the `required` modifier do?
    options:
      - text: Throws at runtime if the property is null
      - id: object-initializer
        text: Makes the compiler insist that callers set the member in an object initializer (or a constructor marked `[SetsRequiredMembers]`)
        correct: true
      - text: Marks a parameter as non-optional in ASP.NET Core
      - text: Makes the property `readonly`
    explanation: "`required` is a compile-time contract for construction. `System.Text.Json` also honors it, failing deserialization when the property is missing from the JSON."
  - id: strong-ids
    prompt: Which bug is prevented by strongly-typed ids such as `CustomerId` and `OrderId` instead of plain `Guid`s?
    options:
      - text: SQL injection
      - id: swap
        text: Accidentally passing an order id where a customer id is expected, as in `GetOrders(orderId)`
        correct: true
      - text: Deadlocks
      - text: Null reference exceptions
    explanation: The compiler can only catch mixed-up arguments if they have different types — a cheap, timeless technique ("primitive obsession" is the smell it cures).
---
## Learning objectives

- Choose between `class`, `struct`, `record` and `record struct` with reasons
- Use primary constructors for services and know their mutability caveat
- Use `required`, `init` and strongly-typed identifiers

## A decision table

| You are modeling… | Prefer |
|---|---|
| A service with dependencies and behavior | `sealed class` with a primary constructor |
| A DTO, message, or query result | `sealed record` |
| A small immutable value (id, coordinate, money in a hot path) | `readonly record struct` |
| An entity with identity and a lifecycle | `class` |
| A closed set of alternatives | `abstract record` with derived records (today), `union` (C# 15) |

## Primary constructors for services

```csharp
public sealed class InvoiceSender(
    IInvoiceRepository invoices,
    IEmailGateway email,
    ILogger<InvoiceSender> logger)
{
    public async Task SendAsync(InvoiceId id, CancellationToken ct)
    {
        var invoice = await invoices.GetAsync(id, ct)
            ?? throw new InvalidOperationException($"Invoice {id} not found");
        await email.SendAsync(invoice.CustomerEmail, invoice.ToEmail(), ct);
        logger.LogInformation("Sent invoice {InvoiceId}", id);
    }
}
```

This removes the vestigial ceremony of `private readonly` fields assigned in a constructor —
seven lines per dependency in 2015-era code.

### The caveat, made concrete

```csharp
public sealed class Counter(int start)
{
    public int Next() => ++start;
}
```

This compiles: `start` is mutable captured state. For services that is rarely a problem, but if
you want an enforced invariant, be explicit:

```csharp
public sealed class PriceCalculator(TaxTable taxes)
{
    private readonly TaxTable _taxes = taxes;
}
```

## Strongly-typed ids

```csharp
public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();
}

public readonly record struct OrderId(Guid Value)
{
    public static OrderId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();
}

public Task<IReadOnlyList<Order>> GetOrdersAsync(CustomerId customer, CancellationToken ct);
```

`Guid.CreateVersion7()` (.NET 9) produces time-ordered GUIDs, which index far better than random
v4 GUIDs in clustered indexes — an old database problem with a new, built-in answer.

## `required` and `init`

```csharp
public sealed class CreateCustomerRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; init; }
}

var request = new CreateCustomerRequest { Name = "Ada" };
```

The last line fails to compile: `Email` is required. This replaces the vestigial pattern of a
constructor with nine parameters, or a mutable object half-filled by mistake.

## Sources

- Microsoft Learn, "Primary constructors" — <https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/primary-constructors>
- Microsoft Learn, "required modifier" — <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required>
- Microsoft Learn, "Choosing between class and struct" — <https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct>
- Microsoft Learn, `Guid.CreateVersion7` — <https://learn.microsoft.com/en-us/dotnet/api/system.guid.createversion7>
