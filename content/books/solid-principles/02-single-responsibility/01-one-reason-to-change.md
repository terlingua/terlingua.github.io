---
title: One reason to change
questions:
  - id: definition
    prompt: In Robert C. Martin's refined wording, a module should be responsible to how many actors?
    options:
      - text: None
      - text: As many as needed, as long as the class is short
      - id: one
        text: One
        correct: true
      - text: Two — the developer and the user
    explanation: "\"A module should be responsible to one, and only one, actor.\" An actor is a group of stakeholders who would request changes for the same reason."
  - id: misconception
    prompt: Which statement about SRP is a misconception?
    options:
      - id: one-method
        text: Every class should have exactly one method
        correct: true
      - text: Code that changes for different reasons should be separated
      - text: Code that changes for the same reason should be kept together
    explanation: SRP is about reasons to change, not about counting methods. A cohesive class can have many methods serving one actor.
  - id: actors
    prompt: "An `Employee` class has `CalculatePay()` (used by finance), `ReportHours()` (used by operations) and `Save()` (owned by the DBA team). Which statements are true?"
    type: multiple
    options:
      - text: The class serves three different actors
        correct: true
      - text: A change requested by finance could accidentally break the operations report
        correct: true
      - text: The class satisfies SRP because each method is short
      - text: Splitting by actor reduces the chance of accidental coupling
        correct: true
    explanation: This is Martin's own example. Three actors means three reasons to change living in one class.
---
The **Single Responsibility Principle** is the most quoted and most misunderstood SOLID principle.

## Learning objectives

- State SRP in terms of actors and reasons to change
- Identify mixed responsibilities in a class
- Avoid the "one method per class" misreading

## The definition

The original wording was *"a class should have only one reason to change."* Martin later sharpened
it: **a module should be responsible to one, and only one, actor.** An actor is a group of people
— finance, operations, a compliance team — who request changes for the same reason.

## A class with three masters

```csharp
public class Employee
{
    public decimal CalculatePay() { /* rules owned by finance */ return 0; }
    public string ReportHours()   { /* format owned by operations */ return ""; }
    public void Save()            { /* schema owned by the DBA team */ }
}
```

Suppose `CalculatePay` and `ReportHours` share a private helper `RegularHours()`. Finance asks
for a change to how overtime is counted, a developer updates the helper, and the operations
report silently changes too. Nobody asked for that.

## Separating by actor

```csharp
public sealed record EmployeeData(int Id, string Name, IReadOnlyList<TimeEntry> Entries);

public sealed class PayCalculator      { public decimal Calculate(EmployeeData e) => 0; }
public sealed class HoursReporter      { public string Report(EmployeeData e) => ""; }
public sealed class EmployeeRepository { public Task SaveAsync(EmployeeData e) => Task.CompletedTask; }
```

Each class now changes for one reason. The shared data is a plain record with no behaviour that
could couple the actors together.

## Finding responsibilities

Useful questions when reviewing a class:

- Who would ask for this code to change? If the answer is several unrelated groups, split.
- Does the class name need "And" or "Manager" to describe it?
- Do its tests need very different kinds of setup (a database *and* an SMTP server)?

> SRP applies at every level: functions, classes, modules and services. The same reasoning that
> splits a class also tells you where to draw microservice boundaries.
