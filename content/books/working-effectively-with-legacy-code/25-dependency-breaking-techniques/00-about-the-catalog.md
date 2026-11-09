---
title: "About the Catalog"
questions:
  - id: special
    prompt: "How do the refactorings in Chapter 25 differ from most refactorings written up in the industry?"
    options:
      - id: without-tests
        text: "They preserve behaviour but are intended to be done without tests, to get tests in place"
        correct: true
      - id: ofc53aa
        text: "They intentionally change behaviour"
      - id: ob1eb25
        text: "They require automated tools"
    explanation: "“Technically, these techniques are refactorings—each of them preserves behavior. But unlike most refactorings written up in the industry so far, these refactorings are intended to be done without tests, to get tests in place.”"
  - id: safe
    prompt: "Are the techniques completely safe if you follow the steps?"
    options:
      - id: small-chance
        text: "The chance of mistakes is small, but not zero — exercise care and use the Chapter 23 tips"
        correct: true
      - id: oe188b7
        text: "Yes, completely safe"
      - id: o3cc206
        text: "No, they're usually unsafe"
    explanation: "“In most cases, if you follow the steps carefully, the chance of mistakes is small. This doesn’t mean that they are completely safe.”"
  - id: design
    prompt: "Do these techniques immediately improve design?"
    options:
      - id: oa05c8b
        text: "Yes, always"
      - id: flinch
        text: "No — some will make you flinch; they get code under test so later test-supported refactoring can clean the design"
        correct: true
      - id: o9196a9
        text: "They make design permanently worse"
    explanation: "“These techniques do not immediately make your design better. In fact, if you have good design sense, some of these techniques will make you flinch.” — “At that point, you can use test-supported refactorings to make the design cleaner.”"
  - id: fowler
    prompt: "Some techniques also appear in Fowler's Refactoring. What's different here?"
    options:
      - id: tailored
        text: "Different steps, tailored to be used safely without tests"
        correct: true
      - id: of80a4a
        text: "Nothing"
      - id: o896966
        text: "They're written in C# instead"
    explanation: "“I’ve included them here with different steps. They’ve been tailored so that they can be used safely without tests.”"
---
## Learning objectives

- Explain the purpose and limits of the catalog

## Technique map

| Group | Techniques |
|-------|-----------|
| Parameters | Adapt Parameter, Primitivize Parameter, Parameterize Constructor, Parameterize Method |
| Method-level isolation | Break Out Method Object, Expose Static Method, Extract and Override Call |
| Creation | Extract and Override Factory Method, Extract and Override Getter, Supersede Instance Variable |
| Interfaces | Extract Interface, Extract Implementer |
| Globals and statics | Encapsulate Global References, Introduce Static Setter, Introduce Instance Delegator, Replace Global Reference with Getter |
| Inheritance | Subclass and Override Method, Pull Up Feature, Push Down Dependency |
| Build-level (C/C++) | Definition Completion, Link Substitution, Replace Function with Function Pointer, Template Redefinition, Text Redefinition |
