---
title: "Lean on the Compiler and Pair Programming"
questions:
  - id: definition
    prompt: "What does Lean on the Compiler involve?"
    type: multiple
    options:
      - id: navigate
        text: "Navigating to those errors and making changes"
        correct: true
      - id: ocea6ae
        text: "Disabling compiler warnings"
      - id: alter
        text: "Altering a declaration to cause compile errors"
        correct: true
    explanation: "“Lean on the Compiler involves two steps: 1. Altering a declaration to cause compile errors 2. Navigating to those errors and making changes.”"
  - id: exchange
    prompt: "How is Lean on the Compiler used in the `Exchange` example?"
    options:
      - id: wrap-globals
        text: "Globals are wrapped in an `Exchange` class; compile errors reveal every use, which is changed to `exchange.domestic_exchange_rate`"
        correct: true
      - id: o286a0b
        text: "The globals are deleted"
      - id: oc33a5f
        text: "A macro renames them"
    explanation: "“Now I compile to find all of the places where the compiler can’t find domestic_exchange_rate and foreign_exchange_rate, and I change them so that they are accessed off the exchange object.”"
  - id: type-change
    prompt: "What other use besides structural changes does the chapter mention?"
    options:
      - id: oc9b509
        text: "Finding performance bottlenecks"
      - id: obdb4ad
        text: "Generating documentation"
      - id: interface
        text: "Changing a variable's type from a class to an interface and letting errors show which methods the interface needs"
        correct: true
    explanation: "“One common case is changing the type of a variable declaration from a class to an interface, and using the errors to determine which methods need to be on the interface.”"
  - id: getx
    prompt: "You comment out `getX()` and the build succeeds. Can you conclude it's unused?"
    options:
      - id: not-necessarily
        text: "Not necessarily — if a superclass has a concrete `getX()`, callers silently use that one"
        correct: true
      - id: o64c98c
        text: "Only in C++"
      - id: o697afd
        text: "Yes, definitely"
    explanation: "“Does this mean that getX() is an unused method? Not necessarily. If getX() is declared as a concrete method in a superclass, commenting out getX in our current class will just cause the one in the superclass to be used.”"
  - id: slow-builds
    prompt: "When might Lean on the Compiler be impractical?"
    options:
      - id: o7c1f70
        text: "In statically typed languages"
      - id: odce746
        text: "When the code has tests"
      - id: slow
        text: "When builds take a long time — searching may be more practical"
        correct: true
    explanation: "“Leaning on the compiler isn’t always practical. If your builds take a long time, it might be more practical to search for the places where you need to make changes.”"
  - id: false-confidence
    prompt: "What's the main caution?"
    options:
      - id: know-limits
        text: "Know what the compiler will and won't find, so you aren't lulled into false confidence"
        correct: true
      - id: o8b2596
        text: "Never trust the compiler"
      - id: of05db8
        text: "Use it only for C++"
    explanation: "“It’s just very important to know what the compiler is going to find and what it isn’t so that we aren’t lulled into false confidence.”"
  - id: pairing
    prompt: "What does Feathers insist on when using the dependency-breaking techniques?"
    options:
      - id: o7337d6
        text: "Working alone for focus"
      - id: pair
        text: "Pair programming — working in legacy code is surgery, and doctors never operate alone"
        correct: true
      - id: o99fcdf
        text: "Code review after merge"
    explanation: "“In particular, I insist that you pair when you use the dependency-breaking techniques I’ve described in this book.” and “working in legacy code is surgery, and doctors never operate alone.”"
  - id: dynamic
    prompt: "In C#, which kinds of usage will Lean on the Compiler NOT find when you rename or remove a member?"
    type: multiple
    options:
      - id: od80198
        text: "Direct strongly typed method calls"
      - id: reflection
        text: "Reflection and string-based lookups (e.g. `GetMethod(\"Process\")`)"
        correct: true
      - id: serialization
        text: "Serializer or configuration bindings by member name"
        correct: true
      - id: dynamic
        text: "Calls through `dynamic`"
        correct: true
    explanation: "Feathers' principle — “you have to know what its limits are; if you don’t, you can end up making some serious mistakes.” — applies to every mechanism that binds by name at run time rather than through the type system."
---
## Learning objectives

- Use compile errors as a to-do list
- Know the blind spots: inheritance, shadowing and run-time binding

## Connections to today

Nullable reference types and `required` members turn more mistakes into compile errors, widening
what you can lean on. Analyzers with "treat warnings as errors" extend it further.
