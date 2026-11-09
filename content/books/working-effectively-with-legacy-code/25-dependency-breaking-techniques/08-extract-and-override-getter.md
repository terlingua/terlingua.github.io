---
title: "Extract and Override Getter"
questions:
  - id: purpose
    prompt: "What gap does Extract and Override Getter fill?"
    options:
      - id: cpp-gap
        text: "C++ constructors can't call derived overrides, so instead of a factory you introduce a lazy getter used everywhere and override that"
        correct: true
      - id: o9d8480
        text: "Java lacks getters"
      - id: o7d9448
        text: "It replaces all constructors"
    explanation: "“The big \"hole\" in its applicability is C++.” “The gist of this refactoring is to introduce a getter for the instance variable that you want to replace with a fake object. You then refactor to use the getter every place in the class.”"
  - id: lazy
    prompt: "What is a lazy getter?"
    options:
      - id: o5dcebf
        text: "A getter that never returns"
      - id: oeb5fda
        text: "A getter that returns a copy"
      - id: first-call
        text: "A getter that creates the object on its first call when the field is null"
        correct: true
    explanation: "“A lazy getter is a method that looks like a normal getter to all of its callers. The key difference is that lazy getters create the object they are supposed to return the first time they are called.”"
  - id: downside
    prompt: "What is a downside of this technique?"
    options:
      - id: ob3ab11
        text: "It can't be overridden"
      - id: before-init
        text: "Someone may use the variable directly before it's initialised — make sure all code uses the getter"
        correct: true
      - id: o641ed0
        text: "It slows every call dramatically"
    explanation: "“One downside of Extract and Override Getter is that there is a chance that someone will use the variable before it is initialized. For this reason, it’s good to make sure that all of the code in the class is using the getter.”"
  - id: when
    prompt: "When is it better than Extract and Override Call?"
    options:
      - id: many-methods
        text: "When many methods on the same object are problematic"
        correct: true
      - id: o6e7b34
        text: "Never"
      - id: o70f828
        text: "When only one call is problematic"
    explanation: "“When there is just a single method on an object that is problematic, it is far easier to use Extract and Override Call (348). But, Extract and Override Getter is a better choice when there are many problematic methods on the same object.”"
  - id: lifetime
    prompt: "What special care applies in C++?"
    options:
      - id: ofc5174
        text: "Use `mutable`"
      - id: o9b19e4
        text: "Avoid templates"
      - id: delete
        text: "Object lifetime — delete the testing instance consistently with how code deletes the production instance"
        correct: true
    explanation: "“When you use Extract and Override Getter, you have to be very conscious of object lifetime issues, particularly in a non-garbage-collected language such as C++.”"
  - id: null-init
    prompt: "In step 3, what must happen to the field in all constructors?"
    options:
      - id: oa85c46
        text: "Initialise it to null so the lazy getter will create it on first use"
        correct: true
      - id: o3df56c
        text: "Create the real object"
      - id: o843b31
        text: "Make it static"
    explanation: "“3. Replace all uses of the object with calls to the getter, and initialize the reference that holds the object to null in all constructors.”"
---
## Learning objectives

- Replace direct field use with an overridable lazy getter

## Connections to today

`Lazy<T>` and the C# 14 `field` keyword make lazy properties concise, but a `virtual` property is
still what lets a testing subclass substitute the object.
