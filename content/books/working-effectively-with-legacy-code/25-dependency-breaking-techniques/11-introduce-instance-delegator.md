---
title: "Introduce Instance Delegator"
questions:
  - id: static-cling
    prompt: "What is \"static cling\"?"
    options:
      - id: o579cc9
        text: "Static imports"
      - id: hard-static
        text: "Static methods that contain something difficult to depend on in a test"
        correct: true
      - id: of127c3
        text: "Static fields that never get garbage collected"
    explanation: "“you won’t run into any trouble with them unless they contain something that is difficult to depend on in a test. (The technical term for this is static cling).”"
  - id: technique
    prompt: "What does Introduce Instance Delegator do?"
    options:
      - id: instance-method
        text: "Adds an instance method that delegates to the static, so callers can use an object (and an object seam) instead"
        correct: true
      - id: oe4eb72
        text: "Creates a singleton"
      - id: o6a8313
        text: "Converts all statics to instances immediately"
    explanation: "“We can add an instance method to the class like this and have it delegate to the static method” — then callers use `services.updateBalance(id,sum)` on a passed-in object."
  - id: requirement
    prompt: "What extra condition must hold for callers to use the instance method?"
    options:
      - id: ofc5940
        text: "The class must be sealed"
      - id: o777a52
        text: "The static must be removed first"
      - id: external-creation
        text: "You must find a way to create the `BankingServices` object externally and supply it (e.g. Parameterize Method)"
        correct: true
    explanation: "“Notice that we can pull this off only if we can find some way to externally create the BankingServices object that we are using.”"
  - id: utility
    prompt: "When are utility classes a fair solution, according to Feathers?"
    options:
      - id: o68eec4
        text: "Always"
      - id: o6e7b34
        text: "Never"
      - id: special-case
        text: "In special cases like Java's `Math`, where primitives lack methods — in nearly all other cases plain classes with instance data work"
        correct: true
    explanation: "“At the time of this writing, Java does not support math methods on primitive types, so the utility class is a fair solution, but it is also a special case. In nearly all cases, you can use plain old classes with instance data and methods to do your work.”"
  - id: endgame
    prompt: "What is the eventual endgame once all calls go through the delegators?"
    options:
      - id: o5ff5d1
        text: "Keep both forever"
      - id: o4c9b1e
        text: "Delete the instance methods"
      - id: move-bodies
        text: "Move the static bodies into the instance methods and delete the statics"
        correct: true
    explanation: "“Over time, you might get to the point that every call to the utility class comes through the delegating methods. At that time, you can move the bodies of the static methods into the instance methods and delete the static methods.”"
---
## Learning objectives

- Add object seams to static utility classes incrementally

## Connections to today

`File.ReadAllText` and `DateTime.UtcNow` are classic static cling. .NET now offers instance
abstractions (`TimeProvider`; `System.IO.Abstractions` as a popular package) that play the role of
instance delegators.
