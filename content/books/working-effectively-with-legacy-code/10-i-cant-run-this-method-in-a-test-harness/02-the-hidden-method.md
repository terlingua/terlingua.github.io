---
title: "The Case of the Hidden Method"
questions:
  - id: first-question
    prompt: "You need to change a private method. What should you ask first?"
    options:
      - id: public
        text: "Can I test it through a public method?"
        correct: true
      - id: oe1decd
        text: "Can I use reflection?"
      - id: o1df98a
        text: "Can I delete it?"
    explanation: "“The first question to ask is whether we can test through a public method. If we can, it is a worthwhile thing to do.”"
  - id: benefits
    prompt: "What are the benefits of testing through public methods?"
    type: multiple
    options:
      - id: o833677
        text: "It always gives better error localization"
      - id: as-used
        text: "You test the method as it is actually used"
        correct: true
      - id: not-too-general
        text: "Little danger of making the method too general"
        correct: true
    explanation: "“If we test through public methods, we are guaranteed to be testing the method as it is used in the code.” and “If we test a private method through the public methods that use it, there isn’t much danger of making it too general.”"
  - id: direct-answer
    prompt: "What is Feathers' \"very direct answer\" to \"how do I test a private method?\""
    options:
      - id: od913d4
        text: "Use reflection"
      - id: make-public
        text: "Make it public; if that bothers you, the class is probably doing too much"
        correct: true
      - id: ob06695
        text: "Copy it into the test"
    explanation: "“If we need to test a private method, we should make it public. If making it public bothers us, in most cases, it means that our class is doing too much and we ought to fix it.”"
  - id: reasons
    prompt: "Making a private method public would let clients corrupt the results of other methods. What remedy does Feathers suggest?"
    options:
      - id: o27c862
        text: "Keep it private and skip tests"
      - id: new-class
        text: "Move the private methods to a new class where they are public, and have the original class use an internal instance"
        correct: true
      - id: o7c85a2
        text: "Add a comment saying \"do not call\""
    explanation: "“The second reason is a bit more serious, but fortunately there is a remedy: The private methods can be moved to a new class. They can be public on that class and our class can create an internal instance of it.”"
  - id: testable-design
    prompt: "Which maxim does Feathers state here?"
    options:
      - id: of197a7
        text: "Encapsulation trumps testability"
      - id: o1c0845
        text: "Private methods never need tests"
      - id: maxim
        text: "Good design is testable, and design that isn't testable is bad"
        correct: true
    explanation: "“The fact remains: Good design is testable, and design that isn’t testable is bad.”"
  - id: ccaimage
    prompt: "For `CCAImage::setSnapRegion`, why not simply make it public, and what is done instead?"
    options:
      - id: oaa9671
        text: "It's fine to make it public"
      - id: od913d4
        text: "Use reflection"
      - id: protected-subclass
        text: "Production callers outside `snap()` could break camera tracking; make it protected and expose it via a testing subclass"
        correct: true
    explanation: "“If someone starts to call setSnapRegion in production code outside of the snap() method, it could cause serious trouble with the camera’s tracking system.” Then: “The first step is to change setSnapRegion from private to protected.” and subclass to get access."
  - id: fair-trade
    prompt: "How does Feathers justify loosening encapsulation to protected?"
    options:
      - id: o84ba4b
        text: "It's only temporary and will be reverted automatically"
      - id: o7e1f8c
        text: "Encapsulation doesn't matter in C++"
      - id: trade
        text: "Getting tests in place is a fair trade; considering subclass calls when reasoning is relatively minor"
        correct: true
    explanation: "“For me, getting the tests in place is a fair trade. Yes, this change does let us violate encapsulation.” — “but that is relatively minor.”"
  - id: reflection
    prompt: "Why doesn't Feathers like keeping tests that use reflection to access private members?"
    options:
      - id: of60987
        text: "Reflection isn't available in Java"
      - id: hides-pain
        text: "The subterfuge prevents the team from noticing how bad the code is getting, delaying the bill"
        correct: true
      - id: o9b9dda
        text: "Reflection is too slow"
    explanation: "“That sort of subterfuge really prevents a team from noticing just how bad the code is getting.” — “unless we deal with the root causes, overly responsible classes and tangled dependencies, we are just delaying the bill.”"
  - id: root-cause
    prompt: "What is the real reason `CCAImage` is hard to test?"
    options:
      - id: o89dba8
        text: "It is written in C++"
      - id: o10c33f
        text: "It uses a camera"
      - id: responsibilities
        text: "It has too many responsibilities"
        correct: true
    explanation: "“The real reason we can’t test the image class well is that it has too many responsibilities.”"
---
## Learning objectives

- Choose between testing through public methods and exposing a private one
- Use protected access plus a testing subclass when public access is dangerous
- Explain why reflection-based tests can hide design problems

## Connections to today

- C#'s `internal` + `[InternalsVisibleTo("MyApp.Tests")]` is a middle ground the book doesn't
  cover: visible to tests, invisible to other assemblies.
- `UnsafeAccessor` (.NET 8+) and reflection reach private members. Feathers' sidebar applies
  word for word: handy for breaking in, but don't let such tests linger.
