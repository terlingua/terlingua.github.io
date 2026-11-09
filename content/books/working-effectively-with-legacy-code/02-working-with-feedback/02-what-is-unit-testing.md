---
title: "What Is Unit Testing?"
questions:
  - id: units
    prompt: "What are the \"units\" usually tested in procedural versus object-oriented code?"
    options:
      - id: odd8d6e
        text: "Files versus namespaces"
      - id: occea5a
        text: "Lines versus statements"
      - id: fn-class
        text: "Functions in procedural code, classes in object-oriented code"
        correct: true
      - id: oa86730
        text: "Modules versus packages"
    explanation: "“In procedural code, the units are often functions. In objectoriented code, the units are classes.”"
  - id: large-test-problems
    prompt: "Which problems with large tests does the chapter list?"
    type: multiple
    options:
      - id: oe033b8
        text: "They never find integration bugs"
      - id: loc
        text: "Error localization"
        correct: true
      - id: cov
        text: "Coverage — hard to connect code to the values that exercise it"
        correct: true
      - id: time
        text: "Execution time"
        correct: true
    explanation: "The three bullets are “Error localization”, “Execution time” and “Coverage”. Feathers grants that large tests matter: “they are important, I won’t deny that”."
  - id: two-qualities
    prompt: "Feathers resolves \"is this really a unit test?\" debates with two qualities. Which?"
    type: multiple
    options:
      - id: od39dd7
        text: "It has exactly one assertion"
      - id: oe28029
        text: "It uses no other production class"
      - id: fast
        text: "It runs fast"
        correct: true
      - id: localize
        text: "It helps localize problems"
        correct: true
    explanation: "“Here are qualities of good unit tests: 1. They run fast. 2. They help us localize problems.” and “I go back to the two qualities: Does the test run fast? Can it help us localize errors quickly?”"
  - id: tenth-second
    prompt: "A test takes 100 ms. With ~30,000 tests, why does Feathers call it slow?"
    options:
      - id: o8a9d06
        text: "Because 100 ms exceeds the xUnit default timeout"
      - id: hour
        text: "Running them all would take close to an hour, too long for feedback"
        correct: true
      - id: o82a8ca
        text: "It is not slow; 100 ms is fine"
      - id: oc0c734
        text: "Because it must be hitting a database"
    explanation: "“A unit test that takes 1/10th of a second to run is a slow unit test.” For 30,000 tests at that speed: “Close to an hour. That is a long time to wait for feedback.”"
  - id: not-unit
    prompt: "Which tests would Feathers say are *not* unit tests?"
    type: multiple
    options:
      - id: fs
        text: "One that writes a temporary file to disk"
        correct: true
      - id: o740151
        text: "One that constructs two in-memory production classes together and runs in 2 ms"
      - id: http
        text: "One that calls a real HTTP endpoint"
        correct: true
      - id: config
        text: "One that needs you to edit appsettings.json on your machine first"
        correct: true
      - id: db
        text: "One that inserts a row into a local SQL Server container"
        correct: true
    explanation: "“A test is not a unit test if: 1. It talks to a database. 2. It communicates across a network. 3. It touches the file system. 4. You have to do special things to your environment (such as editing configuration files) to run it.” Using another production class is not itself disqualifying — he goes back to speed and localization."
  - id: not-bad
    prompt: "Does Feathers say database or file-system tests are bad?"
    options:
      - id: obd4989
        text: "Yes, unless they run in under one second"
      - id: separate
        text: "No — they are often worth writing, but must be kept separate so a fast suite remains"
        correct: true
      - id: oba1ad2
        text: "Yes, they should be deleted"
    explanation: "“Tests that do these things aren’t bad. Often they are worth writing, and you generally will write them in unit test harnesses. However, it is important to be able to separate them from true unit tests”."
  - id: growing-test
    prompt: "Why does a test that exercises a class together with several collaborators tend to get worse over time?"
    options:
      - id: o634ea0
        text: "Mocking libraries stop working"
      - id: never-easier
        text: "If the class was never made separately instantiable, it only gets harder as code is added, and people put it off"
        correct: true
      - id: o9bda4a
        text: "The test framework degrades"
      - id: o4caf6e
        text: "Collaborators are deleted over time"
    explanation: "“If you haven’t taken the time to make a class separately instantiable in a test harness, how easy will it be when you add more code? It never gets easier. People put it off.”"
  - id: harness-term
    prompt: "How does the book use the term \"test harness\"?"
    options:
      - id: generic
        text: "Generic term for the test code that exercises software plus the code needed to run it"
        correct: true
      - id: o6819ac
        text: "Only the xUnit framework itself"
      - id: o6f3986
        text: "A hardware rig for embedded testing"
    explanation: "“I use the term test harness as a generic term for the testing code that we write to exercise some piece of software and the code that is needed to run it.”"
---
## Learning objectives

- Define a unit test by isolation, speed and error localization
- Apply Feathers' four rules for what is *not* a unit test
- Calculate why 100 ms per test is too slow at scale

## In a nutshell

Unit tests are valued for two practical qualities — speed and localization — not for purity. Large
tests are still useful, but they localize poorly, run slowly and make it hard to tie new code to
inputs. Tests that hit databases, networks, files or special configuration are worth having, but
keep them in a separate suite.

## Connections to today

```csharp
[Fact]
[Trait("Category", "Integration")]
public async Task Saves_invoice_to_database() { }
```

- **Traits and filters.** Tagging slow tests and running `dotnet test --filter-trait Category=Integration`
  separately is exactly the separation Feathers asks for.
- **Testcontainers** make database tests easy to write. They are valuable integration tests — and
  by this definition, still not unit tests.
- **Arithmetic still applies.** Modern machines are faster, but suites are bigger. 30,000 tests at
  10 ms each is still five minutes.
