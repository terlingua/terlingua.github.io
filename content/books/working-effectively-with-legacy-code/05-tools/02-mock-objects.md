---
title: "Mock Objects"
questions:
  - id: why-mocks
    prompt: "According to Chapter 5, why do we need mock objects when breaking dependencies?"
    options:
      - id: supply-values
        text: "If we remove other code, something must stand in its place and supply the right values so we can exercise our code thoroughly"
        correct: true
      - id: o7c40f6
        text: "To avoid writing assertions"
      - id: oa41864
        text: "To make production code faster"
    explanation: "“If we remove the other code, we need to have something in its place that supplies the right values when we are testing so that we can exercise our piece of code thoroughly. In object-oriented code, these are often called mock objects.”"
  - id: availability
    prompt: "Which statement about mock libraries is consistent with the book?"
    options:
      - id: free
        text: "Several mock object libraries are freely available"
        correct: true
      - id: of762bd
        text: "Mocks are built into every language"
      - id: o9c6517
        text: "Mock libraries are commercial and expensive"
    explanation: "“Several mock object libraries are freely available.” Chapter 3 adds that “simple fake objects suffice in most situations.”"
  - id: supply-vs-sense
    prompt: "A stand-in returns canned exchange rates so a pricing method can run. Which purpose from Chapters 3 and 5 does it mainly serve?"
    options:
      - id: o7af3e0
        text: "Speeding up the build"
      - id: supply
        text: "Supplying the right values so the code can be exercised"
        correct: true
      - id: o50d8b0
        text: "Sensing effects on the collaborator"
    explanation: "This matches Chapter 5's description: “something in its place that supplies the right values when we are testing”. A fake that *records* calls, like `FakeDisplay`, is primarily for sensing."
---
## Learning objectives

- Explain the role of stand-ins when dependencies are removed
- Distinguish value-supplying stand-ins from effect-sensing ones

## Connections to today

| Need | Typical .NET choice |
|------|---------------------|
| Return canned values | Hand-written stub, `Substitute.For<T>().Returns(...)` |
| Record calls for assertions | Hand-written spy, `Received()`/`Verify()` |
| Replace time | `FakeTimeProvider` |
| Replace HTTP | Fake `HttpMessageHandler` |

Prefer state checks on a simple fake over deep interaction verification; over-specified mocks
make refactoring harder, which defeats the point in legacy work.
