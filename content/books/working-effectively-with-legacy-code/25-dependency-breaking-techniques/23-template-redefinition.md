---
title: "Template Redefinition"
questions:
  - id: what
    prompt: "What does Template Redefinition do?"
    options:
      - id: oc07f34
        text: "Replaces methods at run time"
      - id: o7a085d
        text: "Redefines macros"
      - id: generic
        text: "Turns a class into a template/generic parameterized by the dependency type, so tests instantiate it with a fake type"
        correct: true
    explanation: "“if a language supplies generics and a way of aliasing types, you can break dependencies using a technique called Template Redefinition.” Tests use `AsyncReceptionPortImpl<FakeSocket>`."
  - id: typedef
    prompt: "What's \"the sweetest thing\" about the technique?"
    options:
      - id: o291c4f
        text: "It needs no compilation"
      - id: typedef-opt
        text: "A typedef keeps the original name, so references throughout the code base don't change"
        correct: true
      - id: o5afa89
        text: "It removes all headers"
    explanation: "“The sweetest thing about this technique is the fact that we can use a typedef to avoid having to change references all through our code base.”"
  - id: disadvantage
    prompt: "What is the primary disadvantage in C++?"
    options:
      - id: ob522e1
        text: "It is slower at run time"
      - id: headers
        text: "Implementation code moves into headers, increasing dependencies and forcing users to recompile when it changes"
        correct: true
      - id: o9c5d76
        text: "Templates can't be tested"
    explanation: "“Code that was in implementation files moves to headers when you templatize it. This can increase the dependencies in systems. Users of the template then are forced to recompile whenever the template code is changed.”"
  - id: when
    prompt: "When is Template Redefinition especially useful?"
    options:
      - id: oea8bd2
        text: "For C code"
      - id: already-templated
        text: "When the dependencies are already in templatized code, where Extract Interface is awkward"
        correct: true
      - id: of1d87b
        text: "For all new code"
    explanation: "“Template Redefinition can be useful when the dependencies that you want to break are already in templatized code.” In `CollaborationManager`, “we can’t easily use Extract Interface (362) on it because of the way that we are using templates here.”"
  - id: printed-bug
    prompt: "In the printed `Run()` loop, the condition reads `if (n = m_segmentSize - 1)`. What does that do?"
    options:
      - id: assignment
        text: "It assigns rather than compares, so `n` is reset each iteration and the condition is usually true — a test with a fake socket would expose it"
        correct: true
      - id: o33f5d5
        text: "It's a correct comparison"
      - id: o0a1fbd
        text: "It fails to compile in C++"
    explanation: "The listing shows “if (n = m_segmentSize - 1)” in both versions. In C++ that is an assignment that compiles. Being able to run `AsyncReceptionPortImpl<FakeSocket>` in a harness is precisely what lets you discover behaviour like this."
  - id: no-typedef
    prompt: "In languages with generics but no type aliasing, what do you do?"
    options:
      - id: lean
        text: "Lean on the Compiler to update references"
        correct: true
      - id: o5c5720
        text: "The technique is impossible"
      - id: od913d4
        text: "Use reflection"
    explanation: "“In languages that have generics but no type-aliasing mechanism such as typedef, you will have to Lean on the Compiler.”"
---
## Learning objectives

- Parameterize a class over its dependency type to swap in fakes

## Connections to today

C# generics can do this (`AsyncReceptionPort<TSocket> where TSocket : ISocket`), and `using
AsyncReceptionPort = AsyncReceptionPortImpl<CSocket>;` aliases (global using aliases since C# 10)
play the typedef role. Interfaces are usually simpler; generics shine for allocation-free hot paths.
