---
title: "The Legacy Code Change Algorithm"
questions:
  - id: order
    prompt: "What is the correct order of the legacy code change algorithm?"
    options:
      - id: o27043d
        text: "Find test points → Write tests → Identify change points → Make changes → Break dependencies"
      - id: o6ee2fe
        text: "Write tests → Identify change points → Break dependencies → Find test points → Make changes"
      - id: o79f4a6
        text: "Break dependencies → Identify change points → Write tests → Find test points → Refactor"
      - id: correct-order
        text: "Identify change points → Find test points → Break dependencies → Write tests → Make changes and refactor"
        correct: true
    explanation: "“1. Identify change points. 2. Find test points. 3. Break dependencies. 4. Write tests. 5. Make changes and refactor.”"
  - id: goal
    prompt: "What should you be able to point to at the end of each programming episode?"
    options:
      - id: o14f1d0
        text: "A refactored module with no behaviour change"
      - id: oe2100c
        text: "An updated architecture diagram"
      - id: feature-and-tests
        text: "Code that provides new value and the tests for it"
        correct: true
    explanation: "“At the end of each programming episode, we should be able to point not only to code that provides some new feature, but also its tests.”"
  - id: islands
    prompt: "What does the \"islands rising out of the ocean\" image describe?"
    options:
      - id: tested-areas
        text: "Tested areas growing over time until you work in continents of covered code"
        correct: true
      - id: oea1eb6
        text: "Microservices split from a monolith"
      - id: o5818c9
        text: "Isolated teams owning separate modules"
    explanation: "“Over time, tested areas of the code base surface like islands rising out of the ocean.” and “Eventually, you’ll be able to work in continents of test-covered code.”"
  - id: two-manifestations
    prompt: "In which two ways does the dependency problem usually show up?"
    type: multiple
    options:
      - id: oe45bf8
        text: "Difficulty deploying to production"
      - id: run-methods
        text: "Difficulty running methods in test harnesses"
        correct: true
      - id: ocf8908
        text: "Difficulty naming classes"
      - id: instantiate
        text: "Difficulty instantiating objects in test harnesses"
        correct: true
    explanation: "“The two ways this problem manifests itself are difficulty instantiating objects in test harnesses and difficulty running methods in test harnesses.” Chapters 9 and 10 cover each."
  - id: map-chapters
    prompt: "You can break dependencies, but the test build takes too long. Which chapter does the algorithm overview send you to?"
    options:
      - id: ch7
        text: "Chapter 7, It Takes Forever to Make a Change"
        correct: true
      - id: o39e1df
        text: "Chapter 16"
      - id: o995d92
        text: "Chapter 22"
      - id: o8f3cfe
        text: "Chapter 13"
    explanation: "“If you find that you can break dependencies, but it takes too long to build your tests, take a look at Chapter 7, It Takes Forever to Make a Change.”"
  - id: legacy-tests-differ
    prompt: "Which chapter explains how the tests you write for legacy code differ from tests for new code?"
    options:
      - id: ch13
        text: "Chapter 13, I Need to Make a Change but I Don't Know What Tests to Write"
        correct: true
      - id: odae410
        text: "Chapter 5, Tools"
      - id: o25053d
        text: "Chapter 18, My Test Code Is in the Way"
    explanation: "“I find that the tests I write in legacy code are somewhat different from the tests I write for new code. Take a look at Chapter 13”."
  - id: baby-steps
    prompt: "How does Feathers characterise the structural improvements in Chapters 20–22?"
    options:
      - id: baby
        text: "Baby steps that make the design a few steps more maintainable, where \"better\" is context-dependent"
        correct: true
      - id: o013123
        text: "Optional cosmetics"
      - id: of1ec78
        text: "Complete redesigns toward ideal patterns"
    explanation: "“Remember that the things I describe in these chapters are \"baby steps.\"” and “These chapters show you how to make design better, where \"better\" is context dependent and often simply a few steps more maintainable than the design was before.”"
  - id: tdd-features
    prompt: "What technique does Feathers advocate for adding features in legacy code at step 5?"
    options:
      - id: o31fbcd
        text: "Pair debugging"
      - id: tdd
        text: "Test-driven development"
        correct: true
      - id: ob2e772
        text: "Big design up front"
    explanation: "“I advocate using test-driven development (TDD) to add features in legacy code.”"
---
## Learning objectives

- Recite and apply the five steps
- Map each step to the chapters that help with it
- Explain the "islands of tests" growth model

## In a nutshell

1. **Identify change points** — Chapters 16, 17
2. **Find test points** — Chapters 11, 12
3. **Break dependencies** — Chapters 23, 9, 10, 22, 7 and the Part III catalog
4. **Write tests** — Chapter 13
5. **Make changes and refactor** — Chapters 8, 20, 21, 22

## Connections to today

- Treat the algorithm as a PR checklist: does this PR identify the change point, add tests at a
  sensible test point, and keep dependency-breaking edits separate and conservative?
- When an AI assistant proposes a change to legacy code, ask it to follow the same order: first
  characterize, then change. Reviewers can verify each step independently.
