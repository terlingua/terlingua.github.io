---
title: Objects, data structures and the Law of Demeter
questions:
  - id: demeter
    prompt: "Which line violates the Law of Demeter?"
    options:
      - id: train
        text: "`order.GetCustomer().GetAddress().GetCountry().TaxRate`"
        correct: true
      - text: "`order.Total()`"
      - text: "`customer.PlaceOrder(cart)`"
      - text: "`items.Where(i => i.IsActive).ToList()`"
    explanation: Reaching through several objects couples the caller to the internal structure of each. Fluent LINQ chains return new objects of the same abstraction and are not considered violations.
  - id: tell-dont-ask
    prompt: What does "tell, don't ask" encourage?
    options:
      - text: Avoiding all getters in every class
      - text: Logging every method call
      - id: tell
        text: Telling an object to perform behaviour rather than pulling its data out and deciding for it
        correct: true
    explanation: Behaviour should live next to the data it needs, which keeps invariants inside the object.
  - id: anemic
    prompt: Which statements describe an anemic domain model?
    type: multiple
    options:
      - text: Entities are bags of public getters and setters with no behaviour
        correct: true
      - text: Business rules are scattered across service classes
        correct: true
      - text: Invariants such as "total cannot be negative" are enforced inside the entity
    explanation: In an anemic model, rules live outside the objects, so nothing prevents invalid state.
---
## Learning objectives

- Distinguish objects (behaviour, hidden data) from data structures (exposed data, no behaviour)
- Apply the Law of Demeter and "tell, don't ask"
- Recognise and fix anemic models

## Two different things

| Objects | Data structures |
|---------|-----------------|
| Hide data, expose behaviour | Expose data, have little behaviour |
| Easy to add new types | Easy to add new functions |
| `Account.Withdraw(amount)` | `record AccountDto(int Id, decimal Balance)` |

Both are useful. DTOs crossing a network boundary *should* be plain data. Domain objects that guard
business rules should hide their state.

## The Law of Demeter

A method should only talk to its immediate friends: its own fields, its parameters, objects it
creates, and its own methods.

```csharp
// Train wreck
var rate = order.GetCustomer().GetAddress().GetCountry().TaxRate;

// Ask the object that knows
var taxRate = order.TaxRate();
```

## Tell, don't ask

```csharp
// Asking — the rule lives outside the object
if (account.Balance >= amount)
{
    account.Balance -= amount;
}

// Telling — the object protects its own invariant
account.Withdraw(amount);

public sealed class Account
{
    public decimal Balance { get; private set; }

    public void Withdraw(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (amount > Balance) throw new InsufficientFundsException(Balance, amount);
        Balance -= amount;
    }
}
```

With a private setter, the only way to reduce the balance is through `Withdraw`, so the rule
cannot be bypassed.
