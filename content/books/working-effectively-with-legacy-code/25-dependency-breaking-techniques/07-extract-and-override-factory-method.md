---
title: "Extract and Override Factory Method"
questions:
  - id: problem
    prompt: "What problem does Extract and Override Factory Method address?"
    options:
      - id: o6983c6
        text: "Too many factory classes"
      - id: ctor-creation
        text: "Hard-coded object creation in constructors that you need to avoid or replace with a sensing object under test"
        correct: true
      - id: o21e1ba
        text: "Slow constructors in production"
    explanation: "“Object creation in constructors can be vexing when you want to get a class under test.” and “Hard-coded initialization work in constructors can be very hard to work around in testing.”"
  - id: cpp
    prompt: "Why can't you use it in C++?"
    options:
      - id: od0415f
        text: "C++ has no constructors"
      - id: no-virtual
        text: "C++ doesn't let virtual calls from a base constructor resolve to derived-class overrides"
        correct: true
      - id: oe67708
        text: "C++ lacks inheritance"
    explanation: "“you can’t do it in C++. C++ does not allow virtual function calls to resolve to functions in derived classes.” Alternatives: “Supersede Instance Variable and Extract and Override Getter (352)”."
  - id: steps
    prompt: "What are the three steps?"
    options:
      - id: o84ac1d
        text: "Add a parameter → inject → delete constructor"
      - id: o547fb1
        text: "Make a static factory → call it everywhere → mock it"
      - id: three
        text: "Identify a creation in a constructor → extract all creation work into a factory method → override it in a testing subclass"
        correct: true
    explanation: "“1. Identify an object creation in a constructor. 2. Extract all of the work involved in the creation into a factory method. 3. Create a testing subclass and override the factory method”."
  - id: csharp-risk
    prompt: "In C#, overrides called from a base constructor DO run (like Java). What hazard from Chapter 9 should you remember?"
    options:
      - id: oab0c08
        text: "The override is never called"
      - id: uninitialized
        text: "The override runs before the derived class's constructor body, so it may see uninitialized derived fields"
        correct: true
      - id: odbf1ee
        text: "The base constructor is skipped"
    explanation: "Chapter 9 warns: “Until the constructor of the base class is completely finished, there is a chance that an overridden function that it calls can access an uninitialized variable.” Keep test overrides simple — return a prebuilt fake."
---
## Learning objectives

- Move constructor-time creation into an overridable factory method

## Connections to today

C# allows this but analyzers warn (CA2214). Prefer Parameterize Constructor when feasible; use the
factory method when many callers construct the class and you can't change them yet.
