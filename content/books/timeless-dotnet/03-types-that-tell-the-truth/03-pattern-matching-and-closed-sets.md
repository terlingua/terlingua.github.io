---
title: Pattern matching and closed sets of alternatives
questions:
  - id: switch-expr
    prompt: What is the main advantage of a `switch` *expression* over a chain of `if`/`else if` for mapping inputs to outputs?
    options:
      - text: It runs on a different thread
      - id: expression
        text: It is an expression that must produce a value for every input, and the compiler warns when cases are not exhaustive
        correct: true
      - text: It is required for async code
      - text: It avoids allocations in all cases
    explanation: Exhaustiveness checking turns "forgot a case" from a runtime bug into a compiler warning (CS8509), which you should treat as an error.
  - id: property-patterns
    prompt: "What does `order is { Status: OrderStatus.Paid, Total: > 1000m }` check?"
    options:
      - text: Only that `order` is not null
      - text: It assigns `Paid` to `Status`
      - id: both
        text: That `order` is not null, its `Status` is `Paid`, and its `Total` is greater than 1000
        correct: true
      - text: It does not compile; patterns cannot use relational operators
    explanation: Property patterns test nested members; relational patterns (`>`, `<=`) and logical patterns (`and`, `or`, `not`) compose them.
  - id: closed-today
    prompt: Before C# 15, what is the common idiom to model a closed set of alternatives such as `PaymentResult` = Approved | Declined | RequiresAction?
    options:
      - text: An `enum` with a `Dictionary<string, object>` payload
      - id: abstract-record
        text: An abstract base record with a private constructor and nested sealed derived records
        correct: true
      - text: A class with nullable fields for every possible case
      - text: Throwing a different exception type for each case
    explanation: A private constructor prevents derivation outside the type, so the set really is closed. The compiler still cannot prove exhaustiveness, so a discard arm is needed; C# 15's `closed` and `union` remove that limitation.
  - id: list-patterns
    prompt: "What does `args is [\"--verbose\", .. var rest]` match?"
    options:
      - text: Any array containing `--verbose` anywhere
      - id: first
        text: A sequence whose first element is `--verbose`, capturing the remaining elements in `rest`
        correct: true
      - text: Only an array of exactly one element
      - text: A string that starts with `--verbose`
    explanation: List patterns (C# 11) match sequences by shape. `..` is a slice pattern.
  - id: illegal-states
    prompt: "Which design follows \"make illegal states unrepresentable\"?"
    options:
      - text: "`class Shipment { bool IsShipped; DateTime? ShippedAt; string? TrackingNumber; }`"
      - text: "`class Shipment { string State; }` with comments explaining allowed values"
      - text: "`Dictionary<string, object?>` named `ShipmentData`"
      - id: cases
        text: "`abstract record Shipment` with `Pending()` and `Shipped(DateTimeOffset At, string TrackingNumber)` cases"
        correct: true
    explanation: With booleans and nullables, `IsShipped = true` with no tracking number is representable — so someday it will exist in production. With cases, it cannot be constructed.
  - id: enum-trap
    prompt: Why is a `switch` over an `enum` never fully exhaustive in C#?
    options:
      - text: Enums are reference types
      - text: Enums cannot be used in switch expressions
      - id: any-int
        text: Any integer value of the underlying type can be cast to the enum, such as `(OrderStatus)42`
        correct: true
      - text: It is exhaustive; there is no trap
    explanation: Even with every named member covered, the compiler asks for a discard. Throw `UnreachableException` (or `ArgumentOutOfRangeException`) in that arm.
---
## Learning objectives

- Use switch expressions, property, relational and list patterns
- Model closed sets of alternatives with records today
- Apply "make illegal states unrepresentable"

## Patterns in practice

```csharp
public static decimal ShippingCost(Order order) => order switch
{
    { Total: >= 100m } => 0m,
    { Destination.Country: "US", Weight: <= 1.0m } => 4.99m,
    { Destination.Country: "US" } => 9.99m,
    { IsExpress: true } => 29.99m,
    _ => 19.99m,
};
```

Patterns are evaluated top to bottom; the compiler reports arms that can never match.

## Closed alternatives, today

```csharp
public abstract record PaymentResult
{
    private PaymentResult() { }

    public sealed record Approved(string AuthorizationCode) : PaymentResult;
    public sealed record Declined(string Reason) : PaymentResult;
    public sealed record RequiresAction(Uri RedirectTo) : PaymentResult;
}

public static IResult ToHttp(PaymentResult result) => result switch
{
    PaymentResult.Approved a => Results.Ok(new { a.AuthorizationCode }),
    PaymentResult.Declined d => Results.UnprocessableEntity(new { d.Reason }),
    PaymentResult.RequiresAction r => Results.Accepted(r.RedirectTo.ToString()),
    _ => throw new UnreachableException(),
};
```

The nested types can call the private constructor; nobody else can derive. In C# 15 the same
idea becomes `closed` or `union` and the compiler checks exhaustiveness (Chapter 13).

## Real-world scenario: replacing boolean soup

```csharp
public sealed class Subscription
{
    public bool IsTrial { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? TrialEnds { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? RenewsOn { get; set; }
}
```

Five fields allow 2 × 2 × 2 × 2 × 2 = 32 combinations of null/non-null and true/false, of which
perhaps four are meaningful. A support ticket reading "customer is both cancelled and renewing"
is the inevitable result.

```csharp
public abstract record SubscriptionState
{
    private SubscriptionState() { }
    public sealed record Trial(DateOnly EndsOn) : SubscriptionState;
    public sealed record Active(DateOnly RenewsOn) : SubscriptionState;
    public sealed record Cancelled(DateOnly CancelledOn, DateOnly AccessUntil) : SubscriptionState;
}
```

## Sources

- Microsoft Learn, "Pattern matching" — <https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching>
- Microsoft Learn, "Patterns (C# reference)" — <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns>
- Yaron Minsky, "Effective ML" (origin of "make illegal states unrepresentable") — <https://blog.janestreet.com/effective-ml-revisited/>
