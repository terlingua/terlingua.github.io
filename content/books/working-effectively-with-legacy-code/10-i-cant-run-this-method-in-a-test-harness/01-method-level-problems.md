---
title: "Method-Level Problems"
questions:
  - id: problems
    prompt: "Which problems does Chapter 10 list for writing tests for a method?"
    type: multiple
    options:
      - id: o8958c7
        text: "The method has too many comments"
      - id: sense
        text: "We need to sense through an object the method uses"
        correct: true
      - id: side
        text: "It has bad side effects (a database, a cruise missile…)"
        correct: true
      - id: params
        text: "It is hard to construct the parameters needed to call it"
        correct: true
      - id: access
        text: "The method isn't accessible to the test (private or similar)"
        correct: true
    explanation: "The four bullets: “The method might not be accessible to the test.” “It might be hard to call the method because it is hard to construct the parameters we need to call it.” “The method might have bad side effects” and “We might need to sense through some object that the method uses.”"
  - id: without-instance
    prompt: "Which techniques can let you test a method without instantiating its class at all?"
    type: multiple
    options:
      - id: expose-static
        text: "Expose Static Method, if the method uses little instance data"
        correct: true
      - id: method-object
        text: "Break Out Method Object, if the method is long and hard to deal with"
        correct: true
      - id: o71d945
        text: "Pass Null"
    explanation: "“If the method doesn’t use much instance data, we can use Expose Static Method (345) to get access to the code. If the method is pretty long and difficult to deal with, we can use Break Out Method Object (330) to move the code to a class that we can instantiate more easily.”"
  - id: second-part
    prompt: "According to the chapter opening, what is \"the second part of the battle\"?"
    options:
      - id: o2a5936
        text: "Getting code review approval"
      - id: methods
        text: "Writing tests for the methods you need to change, after instantiation"
        correct: true
      - id: ob1ffe3
        text: "Deploying the change"
    explanation: "“Instantiating a class is often only the first part of the battle. The second part is writing tests for the methods we need to change.”"
---
## Learning objectives

- Classify a method-level testing obstacle
- Know when to skip instantiation entirely

## Case map

| Case | Obstacle | Techniques |
|------|----------|------------|
| Hidden Method | Private access | Test through public; make public; move to new class; make protected + testing subclass |
| "Helpful" Language Feature | Sealed/final library types | Adapt Parameter, Skin and Wrap the API |
| Undetectable Side Effect | No observable result | Extract Method, Command/Query Separation, Subclass and Override |
