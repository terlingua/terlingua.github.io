---
title: Taming null
questions:
  - id: nrt
    prompt: What do nullable reference types (`<Nullable>enable</Nullable>`) do?
    options:
      - text: They make the runtime throw when a null reference is assigned
      - text: They replace all reference types with structs
      - text: They remove null from the language
      - id: static
        text: They let the compiler warn about possible null dereferences based on annotations like `string?`
        correct: true
    explanation: Nullable reference types are a compile-time analysis. At runtime, references can still be null.
  - id: bang
    prompt: What does the null-forgiving operator `!` (as in `name!.Length`) do?
    options:
      - text: Throws if `name` is null
      - id: suppress
        text: Suppresses the compiler's nullable warning without any runtime check
        correct: true
      - text: Converts null to an empty string
    explanation: It tells the compiler "trust me". Overusing it reintroduces the bugs the feature is meant to prevent.
  - id: techniques
    prompt: Which techniques reduce null-related bugs?
    type: multiple
    options:
      - text: Return empty collections instead of null
        correct: true
      - text: Use `required` members so objects cannot be created half-initialised
        correct: true
      - text: Use the null-coalescing operators `??` and `??=` for defaults
        correct: true
      - text: Return null from every method that might fail, without documenting it
    explanation: Empty collections, required members and explicit defaults remove ambiguity. Undocumented nulls create surprises.
---
Tony Hoare called the null reference his "billion-dollar mistake". Modern C# gives you tools to
contain it.

## Learning objectives

- Enable and respond to nullable reference type warnings
- Avoid returning null where better options exist
- Use the null operators idiomatically

## Nullable reference types

```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
</PropertyGroup>
```

```csharp
public Customer? Find(int id) => _customers.GetValueOrDefault(id);

var customer = repository.Find(42);
Console.WriteLine(customer.Name);   // warning CS8602: possible null dereference
if (customer is not null) Console.WriteLine(customer.Name); // fine
```

The `?` in the signature is documentation the compiler enforces.

## Prefer empty over null

```csharp
public IReadOnlyList<Order> OrdersFor(int customerId) =>
    _orders.TryGetValue(customerId, out var orders) ? orders : [];
```

Callers can iterate without checking. The `[]` collection expression creates an empty list.

## Required members

```csharp
public sealed class CreateUserRequest
{
    public required string Email { get; init; }
    public string? DisplayName { get; init; }
}

var request = new CreateUserRequest { Email = "ada@example.com" }; // omitting Email is a compile error
```

## Null operators

```csharp
var city = order.Customer?.Address?.City ?? "Unknown";
_cache ??= new Dictionary<string, string>();
```

C# 14 also allows the null-conditional operator on the left of an assignment:

```csharp
customer?.LastSeen = DateTimeOffset.UtcNow; // assigns only when customer is not null
```

Avoid the `!` operator except where you have knowledge the compiler cannot have, and prefer
fixing the annotation instead.
