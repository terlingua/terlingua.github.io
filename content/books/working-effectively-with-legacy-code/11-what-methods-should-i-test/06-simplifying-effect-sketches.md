---
title: "Simplifying Effect Sketches, Effects and Encapsulation"
questions:
  - id: best-endpoint
    prompt: "In the original `CppClass` sketch, which method is the best single test point and why?"
    options:
      - id: o40060b
        text: "`getDeclarationCount`"
      - id: get-interface
        text: "`getInterface`, because it exercises declarations more — though `getDeclaration` and `getDeclarationCount` would go uncovered"
        correct: true
      - id: ocf8243
        text: "`getName`"
    explanation: "“The best one to use is getInterface because it exercises declarations a bit more.” — “but it would be a shame that getDeclaration and getDeclarationCount wouldn’t be covered.”"
  - id: reuse-effect
    prompt: "What changes when `getInterface` calls `getDeclaration` internally?"
    options:
      - id: of80a4a
        text: "Nothing"
      - id: o3634d6
        text: "Tests get slower"
      - id: covered
        text: "Testing `getInterface` now also exercises `getDeclaration` — the sketch has fewer endpoints"
        correct: true
    explanation: "“We end up exercising getDeclaration whenever we test getInterface.” and “When we remove tiny pieces of duplication, we often end up getting effect sketches with a smaller set of endpoints.”"
  - id: encapsulation-why
    prompt: "Many dependency-breaking techniques break encapsulation. What is Feathers' position?"
    options:
      - id: ofa9529
        text: "Encapsulation is obsolete"
      - id: bias-coverage
        text: "Encapsulation is a tool for understanding; when it conflicts with test coverage, bias toward coverage, which can yield more encapsulation later"
        correct: true
      - id: of22072
        text: "Encapsulation must never be broken"
    explanation: "“Encapsulation and test coverage aren’t always at odds, but when they are, I bias toward test coverage. Often it can help me get more encapsulation later.” and “Encapsulation isn’t an end in itself; it is a tool for understanding.”"
  - id: tests-reason
    prompt: "How can breaking encapsulation make reasoning *easier*?"
    options:
      - id: of893f4
        text: "By removing private fields"
      - id: o0cf9d2
        text: "It never does"
      - id: explanatory-tests
        text: "If you end up with good explanatory tests, you can reason with them and write new tests for any question"
        correct: true
    explanation: "“Breaking encapsulation can make reasoning about our code harder, but it can make it easier if we end up with good explanatory tests afterward.”"
  - id: param-ctor-path
    prompt: "Why does Parameterize Constructor add a path to effect reasoning?"
    options:
      - id: o9df419
        text: "It makes the class static"
      - id: o5e0638
        text: "It doesn't"
      - id: one-more
        text: "The new parameter is one more way state can enter the object"
        correct: true
    explanation: "“if we add another parameter to a constructor to break a dependency as we do in the Parameterize Constructor (379) refactoring, we have one more path to follow when we are reasoning about effects.”"
---
## Learning objectives

- Reduce sketch endpoints by removing duplication
- Weigh encapsulation against test coverage

## Connections to today

Reusing a public method internally (instead of duplicating its logic) is a cheap way to make a
single high-level test cover more. Snapshot tests on a rich output like `getInterface` exploit the
same effect.
