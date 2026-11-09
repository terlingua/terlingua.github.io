---
title: "Subclass and Override Method"
questions:
  - id: core
    prompt: "What is the central idea of Subclass and Override Method?"
    options:
      - id: inheritance
        text: "Use inheritance in a test to nullify behaviour you don't care about or access behaviour you do"
        correct: true
      - id: o3cbaa5
        text: "Copy the class into the test project"
      - id: o7c232d
        text: "Replace the class at link time"
    explanation: "“The central idea of Subclass and Override Method is that you can use inheritance in the context of a test to nullify behavior that you don’t care about or get access to behavior that you do care about.”"
  - id: relation
    prompt: "How does it relate to the rest of the catalog?"
    options:
      - id: o627df6
        text: "It's unrelated"
      - id: oa04cda
        text: "It's a last resort"
      - id: core-variant
        text: "It's a core technique; many other techniques are variations on it"
        correct: true
    explanation: "“Subclass and Override Method is a core technique for breaking dependencies in object-oriented programs. In fact, many of the other dependency-breaking techniques in this chapter are variations on it.”"
  - id: minimal
    prompt: "What production change was needed for `TestingMessageForwarder`?"
    options:
      - id: o53a30a
        text: "Rewriting the constructor"
      - id: protected
        text: "Only changing `createForwardMessage` from private to protected"
        correct: true
      - id: o776ef7
        text: "Adding an interface"
    explanation: "“We were able to get separation with minimal modification of the production code. All we did was change the scope of a method from private to protected.”"
  - id: careful
    prompt: "What caution applies when overriding to return an almost empty `FakeMessage`?"
    options:
      - id: o936388
        text: "You must never return fakes"
      - id: what-testing
        text: "That only makes sense if the test doesn't care about the nulled-out behaviour, e.g. just message routing rather than content"
        correct: true
      - id: o6358f4
        text: "Fake messages are always fine"
    explanation: "“I can return an empty message without a subject, from address, and so on, but that would make sense only if I was, say, testing the fact that I can get a message from one place in the software to another and don’t care what the actual content and addressing are.”"
  - id: paper-view
    prompt: "What is Feathers' \"paper view\"?"
    options:
      - id: odbda7f
        text: "Printing the code"
      - id: od51a2a
        text: "A UML diagram"
      - id: translucent
        text: "Imagining a translucent sheet over the class where any extractable snippet could be replaced during testing"
        correct: true
    explanation: "“It is as if I placed a piece of translucent paper on top of the one with the code. The new sheet can have a different piece of code for the snippet that I want to replace.”"
  - id: existing
    prompt: "When actually applying the technique, which methods does Feathers try to override?"
    options:
      - id: o559a25
        text: "Constructors"
      - id: existing-methods
        text: "Methods that already exist — extracting new ones without tests can be risky"
        correct: true
      - id: obe270b
        text: "Newly extracted ones only"
    explanation: "“when I start to use Subclass and Override Method, I try to override methods that already exist. After all, the goal is to get tests in place, and extracting methods without tests in place can be risky at times.”"
  - id: languages
    prompt: "Which statements about making methods overridable match the book?"
    type: multiple
    options:
      - id: csharp-protected
        text: "In Java and C#, at least protected visibility is required"
        correct: true
      - id: cpp-private
        text: "In C++ methods can remain private and still be overridden"
        correct: true
      - id: cpp-virtual
        text: "In C++ the methods must be made virtual"
        correct: true
      - id: java-final
        text: "In Java they must be non-final"
        correct: true
    explanation: "“In C++, the methods have to be made virtual if they aren’t already. In Java, the methods need to be made non-final.” “In Java and C#, methods must at least have protected visibility to be overridden in subclasses. In C++, methods can remain private and still be overridden in subclasses.”"
  - id: smallest
    prompt: "What does step 1 ask you to find?"
    options:
      - id: offb009
        text: "All private fields"
      - id: smallest-set
        text: "The smallest set of methods you can override to achieve your separation or sensing goals"
        correct: true
      - id: o241114
        text: "Every method in the class"
    explanation: "“Try to find the smallest set of methods that you can override to achieve your goals.”"
---
## Learning objectives

- Apply Subclass and Override Method with minimal production change

## Connections to today

C#: mark the method `protected virtual`; in tests, `class TestingMessageForwarder : MessageForwarder
{ protected override Message CreateForwardMessage(...) => new FakeMessage(...); }`. Mocking
libraries like Moq can override `virtual` members of concrete classes, which is the same seam.
