---
title: Pitfalls and balance
questions:
  - id: yagni
    prompt: You are writing the first and only implementation of a report exporter, and there is no known need for another. What is the most pragmatic choice?
    options:
      - text: Create an interface, an abstract base class, a factory and a plugin loader up front
      - text: Put the exporter code directly in the controller
      - id: simple
        text: Write the concrete class cleanly and extract an abstraction when a second need appears
        correct: true
    explanation: YAGNI — "you aren't gonna need it". Clean concrete code is easy to refactor later; speculative abstractions cost something every day.
  - id: header-interfaces
    prompt: What is a "header interface"?
    options:
      - id: mirror
        text: An interface that mirrors every public member of a single class, created by reflex rather than from client needs
        correct: true
      - text: An interface that only declares properties
      - text: An interface declared at the top of a file
    explanation: Header interfaces add indirection without providing a real abstraction. They often violate ISP because they are shaped around the implementation.
  - id: when-apply
    prompt: Which situations justify investing in SOLID-style abstractions?
    type: multiple
    options:
      - text: The code talks to volatile infrastructure such as databases, queues or third-party APIs
        correct: true
      - text: There are already two or three variations of the same behaviour
        correct: true
      - text: You need to unit test business rules without real infrastructure
        correct: true
      - text: A one-off script that will be deleted next week
    explanation: Abstractions pay for themselves where change, variation or testing pressure exists.
---
## Learning objectives

- Recognise over-engineering driven by SOLID
- Apply YAGNI and the rule of three
- Decide when an abstraction pays for itself

## Over-engineering

The most common failure with SOLID is not ignoring it — it is applying it everywhere:

```csharp
public interface IStringFormatterFactoryProvider
{
    IStringFormatterFactory GetFactory(FormatterKind kind);
}
```

If there is one formatter and no plan for another, every reader now navigates three files to
discover that a string is upper-cased.

## Useful counterweights

| Heuristic | Meaning |
|-----------|---------|
| YAGNI | Do not build for needs you do not have yet |
| Rule of three | Extract an abstraction on the third repetition, not the first |
| KISS | The simplest design that works is usually the right first design |
| Keep things that change together, together | Cohesion beats fragmentation |

## Header interfaces

Creating `IFoo` for every `Foo`, with identical members, is a ritual, not a design. Abstractions
should come from **clients** (ISP) and from **volatility** (DIP). Modern mocking libraries and
in-memory fakes mean you rarely need an interface purely for testing a stable, pure class.

## A practical checklist

- Does this code depend on something volatile? → invert the dependency
- Do I already have variations? → introduce a strategy or polymorphism
- Would a change requested by one group break another? → split responsibilities
- Are implementations throwing for members they cannot support? → segregate interfaces
- None of the above? → write clear, concrete code and move on

> SOLID is a means to an end: code that is easy to understand, test and change.
