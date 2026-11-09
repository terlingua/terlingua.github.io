---
title: "Interception Points"
questions:
  - id: definition
    prompt: "What is an interception point?"
    options:
      - id: o3ac775
        text: "A breakpoint in the debugger"
      - id: detect
        text: "A point in the program where you can detect the effects of a particular change"
        correct: true
      - id: o40117d
        text: "A method that intercepts exceptions"
    explanation: "“An interception point is simply a point in your program where you can detect the effects of a particular change.”"
  - id: start
    prompt: "How do you start finding interception points?"
    options:
      - id: o9f15e5
        text: "Start at `main()`"
      - id: trace
        text: "Identify change points and trace effects outward from them"
        correct: true
      - id: o74864d
        text: "Search for public methods"
    explanation: "“The best way to start is to identify the places where you need to make changes and start tracing effects outward from those change points.”"
  - id: shipping-pricer
    prompt: "Why isn't the `shippingPricer` field a good interception point for the Invoice change?"
    type: multiple
    options:
      - id: private
        text: "It is private, so tests can't access it"
        correct: true
      - id: narrow
        text: "Even if accessible, it can't show that `getValue` hasn't changed badly"
        correct: true
      - id: o9e2ba9
        text: "It is created in a static method"
    explanation: "“it is a private variable in the Invoice class, so we don’t have access to it. Even if it were accessible to tests, shippingPricer is a pretty narrow interception point.” — “we can’t use it to make sure that getValue doesn’t change in a bad way.”"
  - id: best-choice
    prompt: "Between `BillingStatement.makeStatement` and `Invoice.getValue`, which interception point does Feathers prefer for this change?"
    options:
      - id: getvalue
        text: "`Invoice.getValue` — closer to the change and possibly less work"
        correct: true
      - id: o72e608
        text: "Neither"
      - id: o0b1e7e
        text: "`BillingStatement.makeStatement` — always test at the highest level"
    explanation: "“But better than that, we can write tests that exercise getValue on Invoice and check there. It might even be less work.”"
  - id: why-close
    prompt: "Why pick interception points close to change points?"
    type: multiple
    options:
      - id: o5da463
        text: "Close points run on faster hardware"
      - id: safety
        text: "Safety — each intermediate step is a step in a logical argument that could be wrong"
        correct: true
      - id: setup
        text: "Distant points are often harder to set up tests for"
        correct: true
    explanation: "“The first reason is safety. Every step between a change point and an interception point is like a step in a logical argument.” and “Another reason why more distant interception points are worse is that it is often harder to set up tests at them.”"
  - id: verify-argument
    prompt: "If unsure that a distant test covers a change, what fallback does Feathers describe?"
    options:
      - id: mutate
        text: "Write the test, then alter the code at the change point a little and see if the test fails"
        correct: true
      - id: oa34fee
        text: "Ask a colleague"
      - id: o9b48e0
        text: "Increase logging"
    explanation: "“Sometimes the only way you can be sure is by writing tests at the interception point and then going back to the change point to alter the code a little bit and see if the test fails.”"
  - id: higher-benefits
    prompt: "When several classes change (Invoice, Item, BillingStatement), what are the twofold benefits of a higher-level interception point?"
    type: multiple
    options:
      - id: o860a5c
        text: "Faster test execution"
      - id: bigger-vise
        text: "A bigger chunk held in the vise, giving more cover for refactoring"
        correct: true
      - id: less-breaking
        text: "Less dependency breaking"
        correct: true
    explanation: "“The benefits of doing this are twofold: We could have less dependency breaking to do, and we’re also holding a bigger chunk in the vise.”"
  - id: empty-expected
    prompt: "The starter test asserts `assertEquals(\"\", statement.makeStatement())`. Why an empty string?"
    options:
      - id: o511d5d
        text: "It's a bug in the test"
      - id: discover
        text: "It's a placeholder to discover what the code actually produces, then the test is updated to that value"
        correct: true
      - id: o31c4b2
        text: "Statements are always empty"
    explanation: "“We can find out what BillingStatement creates for an invoice of one item and change the test to use that value.” This is the characterization technique formalised in Chapter 13."
---
## Learning objectives

- Find interception points by tracing from change points
- Prefer close interception points and justify exceptions
- Use the "break it and see" check when the argument is long

## Connections to today

The "alter the code a little bit and see if the test fails" check is manual mutation testing.
Stryker.NET automates it across a code base and reports surviving mutants — places where tests
don't actually intercept effects.
