---
title: Refactoring safely
questions:
  - id: definition
    prompt: How did Martin Fowler define refactoring?
    options:
      - text: Rewriting a system from scratch in a new language
      - text: Adding new features while cleaning up code
      - text: Any change that makes code shorter
      - id: behaviour
        text: Changing the internal structure of code without changing its observable behaviour
        correct: true
    explanation: Refactoring preserves behaviour. Adding features is a separate activity — do not mix them in one step.
  - id: boy-scout
    prompt: What does the Boy Scout Rule say?
    options:
      - text: Never touch code you did not write
      - text: Refactor the whole module before every feature
      - id: cleaner
        text: Leave the code a little cleaner than you found it
        correct: true
    explanation: Small, continuous improvements prevent decay without large, risky rewrites.
  - id: safe-steps
    prompt: Which practices make refactoring safe?
    type: multiple
    options:
      - text: Take small steps and run tests after each one
        correct: true
      - text: Use automated IDE refactorings such as Rename and Extract Method
        correct: true
      - text: Commit refactorings separately from behaviour changes
        correct: true
      - text: Refactor and add a feature in the same large commit to save time
    explanation: Small, tested, separately committed steps keep reviews simple and make it easy to find a mistake.
---
## Learning objectives

- Define refactoring precisely
- Use small, tool-assisted steps
- Make continuous improvement part of daily work

## What refactoring is

> Refactoring: a change made to the internal structure of software to make it easier to understand
> and cheaper to modify without changing its observable behaviour. — Martin Fowler

Kent Beck's "two hats": at any moment you are either adding functionality **or** refactoring.
Switch hats deliberately, never wear both.

## A worked example: replace conditional with polymorphism

```csharp
public decimal Pay(Employee e) => e.Type switch
{
    EmployeeType.Salaried => e.MonthlySalary,
    EmployeeType.Hourly => e.HoursWorked * e.HourlyRate,
    EmployeeType.Commissioned => e.BaseSalary + e.Sales * e.CommissionRate,
    _ => throw new ArgumentOutOfRangeException(nameof(e))
};
```

Steps, running tests after each:

1. Create `abstract class Employee { public abstract decimal Pay(); }`
2. Create one subclass and move its branch into `Pay()`
3. Repeat for each branch
4. Replace the switch with `e.Pay()` and delete the type code

```csharp
public sealed class HourlyEmployee(decimal hours, decimal rate) : Employee
{
    public override decimal Pay() => hours * rate;
}
```

## Use the tools

Visual Studio, Rider and VS Code (C# Dev Kit) provide automated Rename, Extract Method,
Extract Interface, Introduce Parameter and Inline. Automated refactorings are behaviour-preserving
by construction and much faster than manual edits.

## The Boy Scout Rule

Leave the campground cleaner than you found it. Rename one unclear variable, extract one
function, delete one dead method each time you touch a file. Over months this compounds into a
codebase that improves instead of decays — without anyone scheduling a "refactoring sprint".

## Legacy code

Michael Feathers defines legacy code as **code without tests**. To change it safely: find a seam,
write characterisation tests that capture current behaviour, then refactor under their protection.
