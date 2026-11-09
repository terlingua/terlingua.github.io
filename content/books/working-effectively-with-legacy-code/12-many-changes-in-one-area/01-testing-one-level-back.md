---
title: "Testing One Level Back"
questions:
  - id: problem
    prompt: "What is a \"closely scattered change\", and why is it infuriating?"
    options:
      - id: o2ba45b
        text: "A change spread across many repositories"
      - id: several-classes
        text: "A feature that requires modifying three or four closely related classes, each of which would take hours to get under test"
        correct: true
      - id: o939232
        text: "A change that only affects comments"
    explanation: "“You need to add a new feature to a system, and you find that you have to modify three or four closely related classes. Each of them would take a couple of hours to get under test.”"
  - id: one-level-back
    prompt: "What does testing \"one level back\" mean?"
    options:
      - id: ocda507
        text: "Testing the previous release"
      - id: one-place
        text: "Finding a place to write tests for several changes at once — a public method over several private ones, or one object over its collaborators"
        correct: true
      - id: oe3ecf4
        text: "Testing only the database layer"
    explanation: "“Often it pays to test \"one level back,\" to find a place where we can write tests for several changes at once.”"
  - id: cover
    prompt: "Besides testing the change itself, what do covering tests at a higher level provide?"
    options:
      - id: refactor-cover
        text: "Cover for more refactoring — structure below the tests can change radically while they pin behaviour"
        correct: true
      - id: o457ad3
        text: "Automatic documentation"
      - id: oadc946
        text: "Better performance"
    explanation: "“we also give ourselves some \"cover\" for more refactoring in the area. The structure of code below the tests can change radically as long as the tests pin down their behavior.”"
  - id: not-substitute
    prompt: "What is the proper role of higher-level tests relative to unit tests?"
    options:
      - id: oad3d6b
        text: "They replace unit tests"
      - id: o7c0156
        text: "They are only for QA"
      - id: first-step
        text: "A first step toward getting unit tests in place, not a substitute"
        correct: true
    explanation: "“While higher-level tests are an important tool, they shouldn’t be a substitute for unit tests. Instead, they should be a first step toward getting unit tests in place.”"
  - id: fine-grained-fear
    prompt: "Some prefer higher-level tests because many fine-grained tests make interface changes painful. What does Feathers observe?"
    options:
      - id: o1e5bc8
        text: "That fear is fully justified"
      - id: easier
        text: "Changes are often easier than expected — change the tests, then the code, in small safe increments"
        correct: true
      - id: o0a9979
        text: "Fine-grained tests should be avoided"
    explanation: "“In fact, changes are often easier than you would expect because you can make changes to the tests and then make changes to the code, moving the structure along in small safe increments.”"
---
## Learning objectives

- Recognise when to cover several changes from one place
- Position higher-level tests as scaffolding

## Connections to today

An in-process API test with `WebApplicationFactory` is often the "one level back" for a
controller, service and repository changed together. Plan to replace much of it with focused unit
tests once seams exist.
