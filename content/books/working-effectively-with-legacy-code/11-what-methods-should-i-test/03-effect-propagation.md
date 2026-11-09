---
title: "Effect Propagation"
questions:
  - id: three-ways
    prompt: "What are the three basic ways effects propagate?"
    type: multiple
    options:
      - id: params
        text: "Modification of objects passed as parameters that are used later"
        correct: true
      - id: oc04da4
        text: "Comments in the code"
      - id: returns
        text: "Return values used by a caller"
        correct: true
      - id: globals
        text: "Modification of static or global data used later"
        correct: true
    explanation: "“1. Return values that are used by a caller 2. Modification of objects passed as parameters that are used later 3. Modification of static or global data that is used later”"
  - id: handles
    prompt: "In Java and C#, why can a method change the state of an object passed to it?"
    options:
      - id: obcec3c
        text: "Objects are copied deeply"
      - id: references
        text: "References are passed by value — the method gets a handle and can mutate the object through it"
        correct: true
      - id: o905f4e
        text: "Only static methods can mutate parameters"
    explanation: "“The default in many cases is to pass references to objects by value. This is the default in Java and C#. Objects aren’t passed to methods; instead, \"handles\" to objects are passed. As a result, any method can change the state of objects through the handle they were passed.”"
  - id: sneakiest
    prompt: "What is the sneakiest way code affects other code?"
    options:
      - id: od5f138
        text: "Exceptions"
      - id: global
        text: "Global or static data — e.g. `View.getCurrentDisplay().addText(...)` inside `Element.addText`"
        correct: true
      - id: o06bfc6
        text: "Return values"
    explanation: "“The sneakiest way that a piece of code can affect other code is through global or static data.” and “Looking at the signatures of the methods on Element isn’t going to help us find the effect that elements have on views.”"
  - id: information-hiding
    prompt: "Complete Feathers' remark — \"Information hiding is great, unless...\""
    options:
      - id: o3b4c28
        text: "…it slows the program down"
      - id: need-to-know
        text: "…it is information that we need to know"
        correct: true
      - id: o6ff1a4
        text: "…it is in C++"
    explanation: "“Information hiding is great, unless it is information that we need to know.”"
  - id: heuristic
    prompt: "Which steps belong to Feathers' heuristic for finding effects?"
    type: multiple
    options:
      - id: hierarchy
        text: "Look at superclasses and subclasses that might use these"
        correct: true
      - id: returns-callers
        text: "If the method has a return value, look at its callers"
        correct: true
      - id: o23a694
        text: "Count the lines of code"
      - id: modified-values
        text: "If it modifies values, look at methods that use them, and the methods that use those"
        correct: true
      - id: globals-h
        text: "Look for globals and static data modified in the methods"
        correct: true
    explanation: "The heuristic includes “If the method has a return value, look at its callers.” “See if the method modifies any values. If it does, look at the methods that use those values, and the methods that use those methods.” “look for superclasses and subclasses” and “Look for global variables and static data”. Step 5 also checks parameters."
  - id: aspects
    prompt: "Which additional propagation mechanism does the book mention for some languages?"
    options:
      - id: aspect
        text: "Aspects in aspect-oriented languages, which affect behaviour of code elsewhere"
        correct: true
      - id: o662bd5
        text: "Annotations in C++"
      - id: o7e46e9
        text: "Macros in Java"
    explanation: "“in aspect-oriented languages, programmers can write constructs called aspects that affect the behavior of code in other areas of the system.”"
---
## Learning objectives

- Name the three channels of effect propagation
- Apply the six-step effect-finding heuristic

## Connections to today

- Aspect-like mechanisms in .NET — middleware, filters, interceptors, source generators, EF Core
  change tracking, `INotifyPropertyChanged` — are modern "sneaky" channels that a method signature
  won't reveal.
- Static `ActivitySource`, `Meter`, `ILogger` and ambient `AsyncLocal<T>` values are global
  channels. They're usually harmless, but a test that asserts on them must know they exist.
