---
title: Common code smells
questions:
  - id: feature-envy
    prompt: "A method in `InvoicePrinter` reads ten properties of `Customer` to compute a discount. Which smell is this?"
    options:
      - text: Long parameter list
      - text: Shotgun surgery
      - text: Dead code
      - id: envy
        text: Feature envy
        correct: true
    explanation: The method is more interested in another class's data than its own. Move the logic to `Customer`.
  - id: shotgun
    prompt: Every time a new currency is supported, you edit eight files in different folders. Which smell is this?
    options:
      - text: Primitive obsession
      - text: Lazy class
      - id: shotgun
        text: Shotgun surgery
        correct: true
    explanation: One conceptual change requires many scattered edits, a sign that the knowledge is not cohesive.
  - id: smell-facts
    prompt: Which statements about code smells are true?
    type: multiple
    options:
      - text: A smell is a hint that something may be wrong, not proof of a bug
        correct: true
      - text: The term was popularised by Kent Beck and Martin Fowler in "Refactoring"
        correct: true
      - text: Duplicated code is one of the most common smells
        correct: true
      - text: Every smell must be fixed immediately, regardless of context
    explanation: Smells guide attention. Whether to act depends on cost, risk and how often that code changes.
---
## Learning objectives

- Recognise the most common code smells
- Connect each smell to a refactoring

A **code smell** is a surface symptom that often points to a deeper design problem. The catalogue
was popularised by Kent Beck and Martin Fowler in *Refactoring*.

## A field guide

| Smell | Symptom | Typical refactoring |
|-------|---------|---------------------|
| Duplicated code | Same logic in several places | Extract method/class |
| Long method | You scroll to read it | Extract method |
| Large class | Many fields, many responsibilities | Extract class |
| Long parameter list | Many arguments, often primitives | Introduce parameter object |
| Primitive obsession | `string email`, `decimal amount` everywhere | Introduce value object |
| Feature envy | Method uses another class's data more than its own | Move method |
| Shotgun surgery | One change, many files | Move related code together |
| Divergent change | One class changes for many reasons | Extract class (SRP) |
| Switch statements | Same switch duplicated | Replace conditional with polymorphism |
| Speculative generality | Abstractions "for later" | Inline class, remove parameter |
| Dead code | Unused members | Delete |

## Example: primitive obsession

```csharp
public void Transfer(string fromIban, string toIban, decimal amount, string currency) { }
```

```csharp
public void Transfer(Iban from, Iban to, Money amount) { }
```

The second signature cannot be called with arguments in the wrong order, and validation lives in
`Iban` and `Money` once.

## Let tools help

Roslyn analyzers flag many smells automatically — unused members (IDE0051), unused parameters
(IDE0060), and complexity rules such as CA1502 (excessive cyclomatic complexity) when enabled.
Treat analyzer warnings as conversation starters in review, not as noise.
