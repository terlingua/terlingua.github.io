---
title: Meaningful names
questions:
  - id: best-name
    prompt: Which variable name best reveals intent?
    options:
      - text: "`d`"
      - text: "`days`"
      - text: "`data`"
      - id: elapsed
        text: "`elapsedTimeInDays`"
        correct: true
    explanation: A good name answers why it exists, what it does and how it is used. Units are part of the meaning.
  - id: conventions
    prompt: Which follow standard .NET naming conventions?
    type: multiple
    options:
      - text: "`PascalCase` for types, methods and public properties"
        correct: true
      - text: "`camelCase` for parameters and local variables"
        correct: true
      - text: Interfaces prefixed with `I`, such as `IOrderRepository`
        correct: true
      - text: Hungarian notation such as `strName` and `iCount`
    explanation: .NET conventions use PascalCase, camelCase and the `I` prefix for interfaces. Hungarian notation is discouraged because the type system already tells you the type.
  - id: booleans
    prompt: Which is the clearest name for a boolean property?
    options:
      - text: "`ActiveFlag`"
      - id: is-active
        text: "`IsActive`"
        correct: true
      - text: "`Status`"
    explanation: Prefixes such as `Is`, `Has`, `Can` and `Should` make booleans read like questions.
  - id: async-suffix
    prompt: What convention does .NET use for methods that return an awaitable `Task`?
    options:
      - text: Prefix the name with `Await`
      - id: suffix
        text: Suffix the name with `Async`, as in `SaveAsync`
        correct: true
      - text: No convention exists
    explanation: The `Async` suffix tells callers they need to await the result.
---
Names are the first and most frequent documentation a reader sees.

## Learning objectives

- Choose names that reveal intent
- Apply .NET naming conventions consistently
- Avoid disinformation and encodings

## Reveal intent

```csharp
// Before
int d; // elapsed time in days
if (u.s == 2) { }

// After
int elapsedTimeInDays;
if (user.Status == AccountStatus.Suspended) { }
```

If a name needs a comment, the name is not finished.

## .NET conventions

| Element | Convention | Example |
|---------|------------|---------|
| Types, methods, properties | PascalCase | `OrderService`, `PlaceOrder`, `TotalPrice` |
| Parameters, locals | camelCase | `orderId`, `lineCount` |
| Private fields | `_camelCase` (common team convention) | `_orders` |
| Interfaces | `I` + PascalCase | `IClock` |
| Async methods | `Async` suffix | `LoadAsync` |
| Booleans | `Is`/`Has`/`Can` | `IsExpired`, `HasChildren` |

## Avoid disinformation

- Do not call something `accountList` if it is a `HashSet`
- Avoid names that differ in tiny ways: `XmlControllerForHandlingStrings` vs `XmlControllerForStorageOfStrings`
- Avoid noise words: `ProductInfo`, `ProductData` and `Product` usually mean the same thing

## Use domain language

Use the words your business experts use: `Invoice`, `Shipment`, `Policyholder`. Where a technical
concept is clearer, use the pattern name: `OrderFactory`, `PriceCalculator`, `CustomerRepository`.

## Scope and length

A short name is fine in a tiny scope (`i` in a three-line loop, `x` in `x => x.IsActive`). The
larger the scope, the more descriptive the name must be. Methods should be verbs (`Calculate`,
`Send`); classes should be nouns (`Invoice`, `Calculator`).
