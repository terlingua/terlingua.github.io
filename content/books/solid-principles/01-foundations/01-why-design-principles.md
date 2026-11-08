---
title: Why design principles matter
questions:
  - id: cost-of-change
    prompt: What is the main problem that design principles such as SOLID try to address?
    options:
      - id: change
        text: Keeping the cost of changing software low as it grows
        correct: true
      - text: Making programs run faster on modern hardware
      - text: Reducing the number of classes in a codebase
      - text: Replacing the need for automated tests
    explanation: Most of a system's lifetime cost is change. SOLID is about structuring code so that changes stay local, predictable and cheap.
  - id: coupling-cohesion
    prompt: Which combination usually indicates a healthy design?
    options:
      - id: low-high
        text: Low coupling and high cohesion
        correct: true
      - text: High coupling and high cohesion
      - text: High coupling and low cohesion
      - text: Low coupling and low cohesion
    explanation: Modules should depend on each other as little as possible (low coupling) while the code inside each module should be strongly related (high cohesion).
  - id: rigidity-symptoms
    prompt: Which of these are symptoms of a rotting design?
    type: multiple
    options:
      - text: A small change forces edits in many unrelated places
        correct: true
      - text: Fixing a bug in one module breaks behaviour in another
        correct: true
      - text: A useful module cannot be reused because it drags in too many dependencies
        correct: true
      - text: The codebase has a comprehensive automated test suite
    explanation: Rigidity, fragility and immobility are the classic symptoms described by Robert C. Martin. A good test suite is a sign of health, not rot.
  - id: solid-origin
    prompt: Who coined the acronym "SOLID" for the principles popularised by Robert C. Martin?
    options:
      - text: Barbara Liskov
      - text: Bertrand Meyer
      - id: feathers
        text: Michael Feathers
        correct: true
      - text: Kent Beck
    explanation: Robert C. Martin described the principles around 2000; Michael Feathers later noticed the letters spelled SOLID.
---
Software rarely fails because it cannot be written. It fails because, after a few years, it can
no longer be **changed** safely. Design principles are a vocabulary for keeping change cheap.

## Learning objectives

- Explain coupling and cohesion and why they matter
- Recognise the symptoms of a design that resists change
- Name the five SOLID principles and their origins

## Coupling and cohesion

**Coupling** measures how much one module knows about another. **Cohesion** measures how
closely the responsibilities inside one module belong together.

```csharp
public class ReportService
{
    public string Build(Order order)
    {
        var connection = new SqlConnection("Server=prod;...");
        var smtp = new SmtpClient("mail.example.com");
        // query the database, format HTML, send e-mail, write a log file...
        return "done";
    }
}
```

This class is tightly coupled to SQL Server, SMTP and the file system, and it has low cohesion:
data access, formatting and delivery are unrelated jobs. Any change in any of those areas touches
this one class, and none of it can be tested without real infrastructure.

## Symptoms of rot

Robert C. Martin described four smells of a decaying design:

| Symptom | What it feels like |
|---------|--------------------|
| Rigidity | Every change cascades into many other changes |
| Fragility | Changes break things in places that seem unrelated |
| Immobility | Useful parts cannot be reused elsewhere |
| Viscosity | Doing the right thing is harder than hacking around it |

## The five principles

| Letter | Principle | One-line idea |
|--------|-----------|---------------|
| S | Single Responsibility | A module should have one reason to change |
| O | Open/Closed | Extend behaviour without modifying existing code |
| L | Liskov Substitution | Subtypes must be usable wherever their base type is expected |
| I | Interface Segregation | Clients should not depend on methods they do not use |
| D | Dependency Inversion | Depend on abstractions, not on concrete details |

The principles were collected by Robert C. Martin around 2000; Michael Feathers noticed the
acronym. Liskov substitution comes from Barbara Liskov (1987) and open/closed from Bertrand Meyer (1988).

> Principles are heuristics, not laws. Each chapter ends with the cost of applying the principle,
> because over-applying SOLID creates its own kind of rigidity.
