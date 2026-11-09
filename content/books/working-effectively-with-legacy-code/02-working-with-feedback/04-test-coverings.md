---
title: "Test Coverings and the Legacy Code Dilemma"
questions:
  - id: change-points
    prompt: "In the invoice example, what are `getResponseText` and `getValue` called?"
    options:
      - id: change-points
        text: "Change points"
        correct: true
      - id: o53f4fe
        text: "Test points"
      - id: oeae2d1
        text: "Pinch points"
      - id: o4fc4a9
        text: "Seams"
    explanation: "“We want to make changes to the getResponseText method of InvoiceUpdateResponder and the getValue method of Invoice. Those methods are our change points.”"
  - id: obstacles
    prompt: "What makes `InvoiceUpdateResponder` hard to construct in a test?"
    type: multiple
    options:
      - id: servlet
        text: "Its constructor needs an `InvoiceUpdateServlet`"
        correct: true
      - id: ob4b970
        text: "It has no public constructor"
      - id: oa991d9
        text: "It is declared final"
      - id: db
        text: "Its constructor takes a real live `DBConnection`"
        correct: true
    explanation: "“It accepts a DBConnection, a real connection to a live database.” and “The constructor for InvoiceUpdateResponder needs an InvoiceUpdateServlet as an argument.”"
  - id: dilemma
    prompt: "State the Legacy Code Dilemma."
    options:
      - id: oa15db8
        text: "Old code cannot be tested at all"
      - id: dilemma-opt
        text: "When we change code we should have tests in place; to put tests in place we often have to change code"
        correct: true
      - id: o0159f1
        text: "Tests slow down delivery so they must be skipped"
      - id: o332b63
        text: "Refactoring is impossible without a rewrite"
    explanation: "“When we change code, we should have tests in place. To put tests in place, we often have to change code.”"
  - id: techniques-used
    prompt: "Which two catalog techniques break the dependencies in Figure 2.2?"
    type: multiple
    options:
      - id: primitivize
        text: "Primitivize Parameter — pass the invoice IDs instead of the servlet"
        correct: true
      - id: o761525
        text: "Subclass and Override Method"
      - id: extract-interface
        text: "Extract Interface — introduce `IDBConnection`"
        correct: true
      - id: o2af5f0
        text: "Link Substitution"
    explanation: "“These refactorings are named Primitivize Parameter (385) and Extract Interface (362), respectively.”"
  - id: conservative
    prompt: "Why should those initial dependency-breaking refactorings be done \"very conservatively\"?"
    options:
      - id: oca4f11
        text: "They are illegal without tests"
      - id: oc8afd7
        text: "To keep the diff small for reviewers only"
      - id: oabe239
        text: "Because conservative code runs faster"
      - id: no-tests-yet
        text: "They are made before tests exist, so any error goes undetected; errors are usually a big deal"
        correct: true
    explanation: "“The trick is to do these initial refactorings very conservatively.” and “When errors are a big deal, and they usually are, it pays to be conservative.”"
  - id: scar
    prompt: "You added a constructor parameter that production code doesn't strictly need, just to get a test in. How does the chapter frame this?"
    options:
      - id: od0faaa
        text: "As a violation of SOLID that disqualifies the test"
      - id: ocf7559
        text: "As a design failure that must be reverted"
      - id: scar-opt
        text: "As a surgical scar — acceptable now, and it can be healed once surrounding code is covered"
        correct: true
    explanation: "“There might be a scar left in your code after your work, but everything beneath it can get better.” and “If later you can cover code around the point where you broke the dependencies, you can heal that scar, too.”"
  - id: dependency-critical
    prompt: "All of the obstacles in the invoice example are what kind of problem?"
    options:
      - id: dependency
        text: "Dependency problems"
        correct: true
      - id: oafed7d
        text: "Naming problems"
      - id: o0c6719
        text: "Concurrency problems"
      - id: o52fb5a
        text: "Performance problems"
    explanation: "“All of these problems are dependency problems.” and “Dependency is one of the most critical problems in software development.”"
---
## Learning objectives

- Identify change points in a design
- State the Legacy Code Dilemma
- Explain why the first dependency-breaking steps must be conservative

## In a nutshell

You want tests before changing code, but getting tests in place usually requires changing code.
The way out is to make small, careful, low-risk structural changes (catalogued in Part III) that
open the code to testing, accept a temporary scar, and heal it later.

## Connections to today

```csharp
public interface IInvoiceSource
{
    IReadOnlyList<Invoice> GetInvoices(Criteria criteria);
}

public sealed class InvoiceUpdateResponder(IInvoiceSource source, IReadOnlyList<int> invoiceIds)
{
}
```

- C# primary constructors and DI containers make the "after" shape of Figure 2.2 the default in
  new code. Legacy code often predates them — the same moves still apply.
- IDE refactorings such as **Extract Interface** in Visual Studio and Rider do the mechanical part
  safely; the judgment about *which* dependency to cut is still yours.
