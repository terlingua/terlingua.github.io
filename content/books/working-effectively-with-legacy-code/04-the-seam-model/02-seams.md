---
title: "Seams and Enabling Points"
questions:
  - id: seam-def
    prompt: "What is a seam?"
    options:
      - id: o5915d3
        text: "A place where code was merged from two branches"
      - id: oe04ccc
        text: "An interface with exactly one implementation"
      - id: def
        text: "A place where you can alter behaviour in your program without editing in that place"
        correct: true
      - id: o0e84e2
        text: "A boundary between two microservices"
    explanation: "“A seam is a place where you can alter behavior in your program without editing in that place.”"
  - id: enabling-def
    prompt: "What is an enabling point?"
    options:
      - id: decision
        text: "A place where you can make the decision to use one behaviour or another"
        correct: true
      - id: o8e844f
        text: "The point where tests start to pass"
      - id: o68f107
        text: "The first line of a method"
    explanation: "“Every seam has an enabling point, a place where you can make the decision to use one behavior or another.”"
  - id: why-not-edit
    prompt: "Why not simply delete the `PostReceiveError` line while testing?"
    options:
      - id: o23ff55
        text: "Deleting lines is never allowed"
      - id: same-source
        text: "The call is still needed in production; source code should be the same in production and test"
        correct: true
      - id: o93d9d6
        text: "The compiler forbids it"
    explanation: "The constraint is “How do we do that and still allow the call to PostReceiveError in production?” and “We can’t really go to that place and change the code just to test it. The source code should be the same in both production and test.”"
  - id: virtual-wrapper
    prompt: "To create an object seam at `PostReceiveError`, Feathers first adds a virtual member method that delegates to the global function. What does this step do to production behaviour?"
    options:
      - id: o8e20ac
        text: "It changes error codes"
      - id: o2e4720
        text: "It disables the error posting"
      - id: preserves
        text: "It preserves behaviour — a little indirection, but the same global function is called"
        correct: true
    explanation: "“That change should preserve behavior.” and “We have a little indirection there, but we end up calling the same global function.”"
  - id: why-seams
    prompt: "What does the seam view buy you in legacy code?"
    type: multiple
    options:
      - id: see-opportunities
        text: "It helps you see testing opportunities already in the code base"
        correct: true
      - id: od85a0b
        text: "It removes the need for any later refactoring"
      - id: sense
        text: "You can run other code at the seam to sense conditions"
        correct: true
      - id: exclude
        text: "Replacing behaviour at seams lets you selectively exclude dependencies in tests"
        correct: true
    explanation: "“The seam view of software helps us see the opportunities that are already in the code base. If we can replace behavior at seams, we can selectively exclude dependencies in our tests. We can also run other code where those dependencies were if we want to sense conditions”."
  - id: explore-seams
    prompt: "How does Feathers suggest discovering what seam types a language offers?"
    options:
      - id: build-steps
        text: "Look at each step that turns program text into running code; each step exposes different seams"
        correct: true
      - id: o06a794
        text: "Read the language specification's chapter on interfaces"
      - id: o669408
        text: "Use a mocking framework's documentation"
    explanation: "“The best way to explore them is to look at all of the steps involved in turning the text of a program into running code on a machine. Each identifiable step exposes different kinds of seams.”"
---
## Learning objectives

- Define *seam* and *enabling point* precisely
- Explain why the source must stay identical between test and production

## In a nutshell

A seam is a location in code whose behaviour can be swapped from somewhere else — the enabling
point. The skill is to spot seams that already exist, or create one with a tiny behaviour-
preserving step, then flip the enabling point in tests.

## Connections to today

| Seam in .NET | Where it lives | Enabling point |
|--------------|----------------|----------------|
| Interface or virtual call | The call site | Constructor argument, DI registration, subclass |
| Delegate / `Func<T>` parameter | The call site | Whatever passes the delegate |
| `TimeProvider`, `IFileSystem`-style abstractions | The call site | DI container setup |
| Conditional compilation (`#if`) | The source text | Build symbols in the `.csproj` |
| Assembly or package substitution | The reference | Project or build configuration |
