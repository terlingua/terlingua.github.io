---
title: "A Heuristic for Writing Characterization Tests"
questions:
  - id: step1
    prompt: "What is step 1 of the heuristic?"
    options:
      - id: o7b48b3
        text: "Make the change, then test"
      - id: o574ae9
        text: "Write tests for the whole system"
      - id: area
        text: "Write tests for the area where you'll make changes — as many as you need to understand it"
        correct: true
    explanation: "“1. Write tests for the area where you will make your changes. Write as many cases as you feel you need to understand the behavior of the code.”"
  - id: step2
    prompt: "What comes after understanding the area?"
    options:
      - id: specific
        text: "Look at the specific things you'll change and write tests for those"
        correct: true
      - id: o3637ca
        text: "Write performance tests"
      - id: ob75105
        text: "Delete the exploratory tests"
    explanation: "“2. After doing this, take a look at the specific things you are going to change, and attempt to write tests for those.”"
  - id: step3
    prompt: "When extracting or moving functionality, what should step 3 tests verify?"
    type: multiple
    options:
      - id: exercise-move
        text: "That you're exercising the code you will move and it's connected properly"
        correct: true
      - id: conversions
        text: "That conversions are exercised"
        correct: true
      - id: o363539
        text: "That the code is faster after moving"
      - id: existence
        text: "Existence and connection of the behaviours, case by case"
        correct: true
    explanation: "“write tests that verify the existence and connection of those behaviors on a case-bycase basis. Verify that you are exercising the code that you are going to move and that it is connected properly. Exercise conversions.”"
  - id: scenario
    prompt: "You'll move a discount calculation from `OrderService` to `Customer`. You have 15 characterization tests, all with whole-dollar prices. What does the heuristic suggest adding?"
    options:
      - id: targeted
        text: "Tests that go through the discount path with inputs that make every numeric conversion matter (e.g. fractional cents)"
        correct: true
      - id: occaf15
        text: "Nothing; 15 tests is enough"
      - id: od2e3bc
        text: "More whole-dollar tests for coverage"
    explanation: "Step 3: “Verify that you are exercising the code that you are going to move and that it is connected properly. Exercise conversions.”"
---
## Learning objectives

- Apply the three-step heuristic to a planned change

## Checklist

1. Understand the area.
2. Pin the specific behaviours you'll touch.
3. For moves and extractions: existence, connection, conversions.
