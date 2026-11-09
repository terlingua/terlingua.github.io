---
title: "The Four Common Problems"
questions:
  - id: four
    prompt: "Which are the four most common problems when instantiating a class in a test harness?"
    type: multiple
    options:
      - id: side-effects
        text: "The constructor has bad side effects"
        correct: true
      - id: build
        text: "The test harness won't easily build with the class in it"
        correct: true
      - id: o59662e
        text: "The class has too many public methods"
      - id: sense-ctor
        text: "Significant work happens in the constructor and we need to sense it"
        correct: true
      - id: create
        text: "Objects of the class can't be created easily"
        correct: true
    explanation: "“1. Objects of the class can’t be created easily. 2. The test harness won’t easily build with the class in it. 3. The constructor we need to use has bad side effects. 4. Significant work happens in the constructor, and we need to sense it.”"
  - id: classify-ctor
    prompt: "A constructor opens a socket to a payment gateway. Which of the four problems is this primarily?"
    options:
      - id: o8f7574
        text: "The harness won't build"
      - id: side
        text: "The constructor has bad side effects"
        correct: true
      - id: o17b035
        text: "Work in the constructor needs to be sensed"
    explanation: "Opening a real network connection during construction is a side effect you don't want in a test — problem 3: “The constructor we need to use has bad side effects.”"
  - id: why-read
    prompt: "What does Feathers say is the value of reading all the cases in this chapter?"
    options:
      - id: oedf12e
        text: "The cases are only relevant to Java"
      - id: oe1c0f3
        text: "Each case has exactly one correct solution to memorise"
      - id: arsenal
        text: "You become familiar with the arsenal of dependency-breaking techniques and learn to trade them off"
        correct: true
    explanation: "“There is more than one way to tackle each of these problems. However, reading through these examples is a great way of becoming familiar with the arsenal of dependency breaking techniques and learning how to trade them off”."
---
## Learning objectives

- Classify an instantiation problem into one of the four categories
- Use the rest of the chapter as a case library

## Case map

| Case | Core problem | First techniques |
|------|--------------|------------------|
| Irritating Parameter | Hard-to-build argument | Extract Interface, Pass Null, Subclass and Override |
| Hidden Dependency | Constructor creates a resource | Parameterize Constructor |
| Construction Blob | Many nested creations | Extract and Override Factory Method, Supersede Instance Variable |
| Irritating Global | Singleton/global use | Introduce Static Setter |
| Horrible Includes | C++ header web | Fakes header, separate test program |
| Onion Parameter | Objects needing objects | Pass Null, Extract Interface on nearest dependency |
| Aliased Parameter | Interface can't fit a class-typed field | Subclass and Override Method |
