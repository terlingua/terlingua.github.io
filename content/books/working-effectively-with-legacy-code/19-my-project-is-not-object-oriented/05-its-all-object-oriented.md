---
title: "It's All Object Oriented"
questions:
  - id: one-object
    prompt: "What does Feathers mean by \"all procedural programs are object oriented\"?"
    options:
      - id: o9b0e4d
        text: "All C code is secretly C++"
      - id: o2a455c
        text: "Procedural code uses polymorphism implicitly"
      - id: single
        text: "They're effectively one big object — wrapping all functions in a `program` class changes nothing"
        correct: true
    explanation: "“all procedural programs are object oriented; it’s just a shame that many contain only one object.” and “The old C system was, in reality, just one big object.”"
  - id: behavior
    prompt: "Does wrapping all functions in `class program` and adding a new `main` change behaviour?"
    options:
      - id: ob4066a
        text: "Yes, everything becomes virtual"
      - id: o07f5fc
        text: "Yes, memory layout changes behaviour"
      - id: unchanged
        text: "Not really — it's mechanical and keeps meaning and behaviour the same"
        correct: true
    explanation: "“Does that change the behavior of the system? Not really. That change was just a mechanical process, and it kept the meaning and behavior of the program exactly the same.”"
  - id: recommendation
    prompt: "If your procedural language has an OO successor, what does Feathers recommend and why?"
    options:
      - id: oc821f8
        text: "Use only preprocessing seams"
      - id: o5c0ed0
        text: "Stay procedural; OO adds complexity"
      - id: move
        text: "Move toward it — object seams improve design, while link and preprocessing seams only help get code under test"
        correct: true
    explanation: "“If the procedural language you are using has an object-oriented successor, I recommend moving toward it. Object seams (40) are good for far more than getting tests in place. Link and preprocessing seams are great for getting code under test, but they really don’t do much to improve design beyond that.”"
  - id: depth
    prompt: "How deep is the object orientation being advocated at this stage?"
    options:
      - id: o37b1bc
        text: "Inheritance hierarchies for every concept"
      - id: shallow
        text: "Not deep — just using objects enough to break up the program for testing"
        correct: true
      - id: oa1d135
        text: "Full domain modelling up front"
    explanation: "“This isn’t deep object-orientation; it’s just using objects enough to break up the program for testing.”"
---
## Learning objectives

- Reframe a procedural program as one large object to subdivide

## Connections to today

Top-level statements in C# and minimal API `Program.cs` files that grow to 2,000 lines are modern
"one big object" programs. Extract classes from them the same way.
