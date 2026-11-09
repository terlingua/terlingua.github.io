---
title: "Class Naming Conventions"
questions:
  - id: suffix
    prompt: "Which test class naming convention does Feathers prefer, and why?"
    options:
      - id: o4553e0
        text: "A separate `Tests` namespace only"
      - id: oe7d4a0
        text: "The `Test` prefix, because tests group together"
      - id: test-suffix
        text: "The `Test` suffix (e.g. `DBEngineTest`), because alphabetical listings put each class next to its test"
        correct: true
    explanation: "“Personally, I like the Test suffix convention. If you have an IDE that lists classes alphabetically, each class lines up next to its test class”."
  - id: fake-prefix
    prompt: "What convention does he use for fakes?"
    options:
      - id: oe921d4
        text: "Suffix `Mock`"
      - id: fake
        text: "Prefix `Fake` — grouping them together, away from main classes"
        correct: true
      - id: o783a88
        text: "Prefix `Stub`"
    explanation: "“The convention I use for those is to use the prefix Fake. This groups all of them together alphabetically in a browser but somewhat away from the main classes in the package.”"
  - id: testing-subclass
    prompt: "What is a testing subclass and how is it named?"
    options:
      - id: o7425a2
        text: "A production subclass used only in staging"
      - id: o2023fe
        text: "Any test class, suffixed `Tests`"
      - id: testing-prefix
        text: "A subclass written to separate dependencies (Subclass and Override Method), prefixed `Testing`"
        correct: true
    explanation: "“It’s the subclass that you write when you use Subclass and Override Method (401). The naming convention that I use for testing subclasses is the name of the class prefixed by the word Testing.”"
  - id: ergonomics
    prompt: "What is the key thing to remember about these conventions?"
    options:
      - id: oafa548
        text: "Consistency with Java standards"
      - id: navigate
        text: "Ergonomics — how easy it is to navigate between classes and their tests"
        correct: true
      - id: o0c6222
        text: "They must never vary"
    explanation: "“The key thing to remember is that ergonomics is important. It’s important to consider how easy it will be to navigate back and forth between your classes and your tests.”"
  - id: listing
    prompt: "In the sample listing, where does `TestingCheckingAccount` appear relative to `CheckingAccount`?"
    options:
      - id: o659764
        text: "In a different directory"
      - id: grouped
        text: "Grouped with the other `Testing*` classes at the end, away from the production class"
        correct: true
      - id: o5d1637
        text: "Immediately after `CheckingAccount`"
    explanation: "“Notice that each production class is next to its test class. The fakes group together and the testing subclasses group together.”"
---
## Learning objectives

- Apply naming conventions for tests, fakes and testing subclasses

## Connections to today

.NET convention is a separate `MyApp.Tests` project with `ClassNameTests`. Solution Explorer
sorts files per project, so the side-by-side benefit comes from mirroring folder structure.
Fakes are often grouped in a `Fakes/` folder — the same idea as the `Fake` prefix.
