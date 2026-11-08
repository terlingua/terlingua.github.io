---
title: Comments that earn their place
questions:
  - id: good-comment
    prompt: Which comment is most valuable?
    options:
      - id: why
        text: "`// The vendor API rejects batches larger than 500 items (see ticket OPS-412)`"
        correct: true
      - text: "`// increment i` above `i++;`"
      - text: "`// changed by Bob on 3 May` above a method"
      - text: A block of commented-out code kept just in case
    explanation: Good comments explain *why* — constraints and decisions the code cannot express. Version control already records who changed what.
  - id: commented-out
    prompt: What should you do with commented-out code?
    options:
      - text: Keep it so others can restore it
      - text: Move it to a `#region` named Old
      - id: delete
        text: Delete it; version control keeps the history
        correct: true
    explanation: Commented-out code rots, confuses readers and is never trusted. Git remembers it for you.
  - id: xml-docs
    prompt: Which statements about XML documentation comments (`///`) are true?
    type: multiple
    options:
      - text: They are valuable on public APIs consumed by other teams or packages
        correct: true
      - text: The compiler can emit them to an XML file used by IntelliSense
        correct: true
      - text: They should restate the method name on every private helper
    explanation: Document public contracts. Restating obvious names on private code is noise.
---
## Learning objectives

- Distinguish helpful comments from noise
- Replace explanatory comments with better code
- Use XML documentation where it adds value

## Comments are a failure to express — usually

```csharp
// Check to see if the employee is eligible for full benefits
if ((employee.Flags & HourlyFlag) != 0 && employee.Age > 65) { }
```

```csharp
if (employee.IsEligibleForFullBenefits()) { }
```

The extracted method says the same thing, and unlike a comment, it cannot drift out of date
with the code.

## Comments worth writing

| Kind | Example |
|------|---------|
| Intent or reasoning | `// Use insertion sort: n is always < 10 and this is a hot path` |
| External constraints | `// Regulators require amounts rounded half away from zero` |
| Warnings | `// Not thread-safe: callers must hold _gate` |
| Public API documentation | `/// <summary>` on library members |
| TODOs tracked elsewhere | `// TODO(OPS-981): remove after v2 migration` |

## Comments to delete

- Restating code: `// return the total`
- Journal comments: `// 2024-01-02 fixed bug`
- Commented-out code
- Closing-brace markers: `} // end if`

## XML documentation

```csharp
/// <summary>Calculates the shipping cost for an order.</summary>
/// <param name="order">The order; must contain at least one line.</param>
/// <returns>The cost in the order's currency.</returns>
/// <exception cref="ArgumentException">The order has no lines.</exception>
public Money ShippingCost(Order order) { /* ... */ }
```

Enable `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in library projects so the
compiler warns about missing or malformed docs on public members.
