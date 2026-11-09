---
title: "Higher-Level Testing"
questions:
  - id: purpose
    prompt: "What role does Chapter 2 give to higher-level tests?"
    options:
      - id: pin-down
        text: "They cover scenarios and interactions and can pin down behaviour for a set of classes at a time"
        correct: true
      - id: oecb105
        text: "They are unnecessary once coverage is high"
      - id: ode0f6f
        text: "They are only for user acceptance"
      - id: o65d3a5
        text: "They replace unit tests in legacy code"
    explanation: "“there is a place for higher-level tests, tests that cover scenarios and interactions in an application. Higher-level tests can be used to pin down behavior for a set of classes at a time.”"
  - id: stepping-stone
    prompt: "How can higher-level tests make unit testing easier?"
    options:
      - id: o81e9a7
        text: "They generate unit tests automatically"
      - id: easier
        text: "Once a group of classes is pinned down, writing tests for the individual classes often becomes easier"
        correct: true
      - id: o33e595
        text: "They remove dependencies from the code"
    explanation: "“When you are able to do that, often you can write tests for the individual classes more easily.”"
  - id: scenario
    prompt: "You must restructure five tightly coupled classes that nobody can instantiate alone. Which first step is most consistent with this section?"
    options:
      - id: o2e3da7
        text: "Write unit tests with every collaborator mocked before touching anything"
      - id: cover-group
        text: "Write a test that pins the behaviour of the five together, then break them apart and add class-level tests"
        correct: true
      - id: o26547b
        text: "Refactor all five, then write unit tests"
      - id: obfc788
        text: "Skip tests; the classes are too coupled"
    explanation: "Higher-level tests “pin down behavior for a set of classes at a time” and make class-level tests easier afterwards. Chapter 12 develops this idea as interception and pinch points."
---
## Learning objectives

- Place higher-level tests alongside unit tests rather than in competition with them
- Use group-level tests as scaffolding for finer-grained tests

## In a nutshell

Higher-level tests are a safety net for a cluster of classes. They are often the easiest net to
hang first and they make it safer to split the cluster apart.

## Connections to today

- `WebApplicationFactory<TEntryPoint>` in ASP.NET Core lets you run an API in-process and pin a
  whole request pipeline — a modern higher-level test that can still run quickly if external
  services are replaced.
- Contract tests between services play a similar role at system scale: pin the interaction first,
  then refactor either side.
