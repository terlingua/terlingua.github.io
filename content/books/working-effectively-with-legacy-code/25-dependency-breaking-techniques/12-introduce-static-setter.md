---
title: "Introduce Static Setter"
questions:
  - id: spooky
    prompt: "Why does Feathers dislike global mutable data in particular?"
    options:
      - id: o42c191
        text: "It can't be serialised"
      - id: o9e04a9
        text: "It's slow"
      - id: setup
        text: "It's the most apparent hurdle to test harnesses — you must set each global into the right state — \"spooky action at a distance\""
        correct: true
    explanation: "“When I visit teams, it is usually the most apparent hurdle to getting portions of their system into test harnesses.” “Quantum physicists didn’t discover \"spooky action at a distance\"; in software, we’ve had it for years.”"
  - id: queasy
    prompt: "How does Feathers justify removing access protection to add a static setter?"
    options:
      - id: o9c9af1
        text: "Access protection is useless"
      - id: odcf2df
        text: "It's only temporary and auto-reverts"
      - id: purpose
        text: "Access protection exists to prevent errors; tests also prevent errors, and here we need the stronger tool"
        correct: true
    explanation: "“remember that the purpose of access protection is to prevent errors. We are putting in tests to prevent errors also. It just turns out that, in this case, we need the stronger tool.”"
  - id: setter-delete
    prompt: "In the C++ `setTestingInstance`, why does the setter delete the old instance?"
    options:
      - id: od65b07
        text: "It's required by the compiler"
      - id: destroy
        text: "The setter should destroy the singleton instance properly before setting the new one"
        correct: true
      - id: odd3f1c
        text: "To speed up the test"
    explanation: "The code shows `delete _instance; _instance = newInstance;`, and step 2 says “Make sure that the setter destroys the singleton instance properly before setting the new object.”"
  - id: shortcuts
    prompt: "Why not just add a boolean flag or conditional compilation to choose a dispatcher?"
    options:
      - id: invasive
        text: "They're invasive and get unwieldy across an application; Feathers prefers keeping production and test code separate"
        correct: true
      - id: oad2a7c
        text: "Flags don't work in C++"
      - id: of01a4f
        text: "They're slower"
    explanation: "“These techniques can work well, but they are invasive and can get unwieldy if you use them throughout an application. In general, I like to keep separation between production and test code.”"
  - id: global-factory
    prompt: "How can you make a global factory like `RouterFactory.makeRouter()` substitutable?"
    options:
      - id: o5eb1bd
        text: "Make `makeRouter` abstract"
      - id: delegate-factory
        text: "Have the static factory delegate to a replaceable `RouterServer` object with a static setter"
        correct: true
      - id: oeb67c4
        text: "Delete the factory"
    explanation: "“often you can do it by having the factory delegate to another factory.” The factory gets a `server` field and `setServer`."
  - id: shared-state
    prompt: "What must you remember about any static setter pattern in tests?"
    options:
      - id: o45e7c5
        text: "Static state is reset by the runtime"
      - id: all-tests
        text: "You're modifying state visible to all tests — reset in setUp/tearDown when leftover state could mislead"
        correct: true
      - id: od2f8b9
        text: "Each test gets its own copy automatically"
    explanation: "“in any of these static setter patterns, you are modifying state that is available to all tests. You can use the tearDown method in xUnit testing frameworks to put things back into some known state”."
  - id: better-state
    prompt: "What is the ultimate \"better state\" for a singleton?"
    options:
      - id: o77efbb
        text: "Two singletons"
      - id: o80802b
        text: "An interface-typed singleton"
      - id: normal-class
        text: "Reduce global references until it can become a normal class passed to those who need it"
        correct: true
    explanation: "“The ultimate \"better state\" is to reduce global references to the singleton to the point that it can just become a normal class.” He describes passing objects “to whoever needed it” in an embedded system."
---
## Learning objectives

- Replace a singleton instance in tests safely
- Manage shared static state across tests

## Connections to today

xUnit.net runs test classes in parallel by default. Tests that set a static instance must share a
`[Collection]` that disables parallelisation, or use `AsyncLocal<T>`-scoped overrides.
