---
title: "Link Substitution"
questions:
  - id: what
    prompt: "What is Link Substitution?"
    options:
      - id: dummy-lib
        text: "Building a dummy library with functions of the same signatures and adjusting the build to link it instead of production code"
        correct: true
      - id: ocf15b2
        text: "Swapping DLLs at run time with reflection"
      - id: o609f18
        text: "Replacing hyperlinks in documentation"
    explanation: "“you can Link Substitution to replace one function with another. To do this, create a dummy library that has functions with the same signatures as the functions that you want to fake.”"
  - id: sensing
    prompt: "How does the `account_deposit` fake support sensing?"
    options:
      - id: od6cf0e
        text: "It prints to stdout"
      - id: o91ec99
        text: "It throws an exception"
      - id: record
        text: "It records each call (type and argument) in a global list that tests can inspect"
        correct: true
    explanation: "“we create a global list of calls to record each time that this function (or any other one we are faking) is called. In a test, we could check the list after we exercise a set of objects”."
  - id: best-targets
    prompt: "Which libraries are best to fake this way?"
    options:
      - id: o3b609b
        text: "Math libraries with complex return values"
      - id: data-sinks
        text: "Mostly pure data sinks — you call them but rarely care about return values, like graphics libraries"
        correct: true
      - id: ofd154b
        text: "Database query libraries"
    explanation: "“The best libraries to fake are those that are mostly pure data sinks: You call functions in them, but you don’t often care about the return values. For example, graphics libraries are particularly useful to fake with Link Substitution.”"
  - id: cpp-classes
    prompt: "Why might Link Substitution be hard with C++ classes?"
    options:
      - id: mangling
        text: "Name mangling by C++ compilers would make it rather difficult"
        correct: true
      - id: ob44f49
        text: "C++ classes have no methods"
      - id: oe37049
        text: "C++ can't link libraries"
    explanation: "“I’ve never tried to use Link Substitution with C++ classes, but I suppose that it is possible. I’m sure that the mangled names that C++ compilers produce would make it rather difficult”."
  - id: java
    prompt: "How can Link Substitution be done in Java?"
    options:
      - id: classpath
        text: "Create classes with the same names and methods and change the classpath"
        correct: true
      - id: o66038c
        text: "It can't"
      - id: o728ede
        text: "Through annotations"
    explanation: "“Link Substitution can also be used in Java. Create classes with the same names and methods, and change your classpath so that calls resolve to them”."
---
## Learning objectives

- Substitute functions at link time and sense calls through them

## Steps

1. Identify the functions or classes to fake.
2. Produce alternative definitions.
3. Adjust the build to include them instead of production versions.

## Connections to today

In .NET, swapping a referenced assembly in a test project is possible but rare; strong naming,
NuGet and DI make object seams far easier. Link-time tricks survive in native interop: a fake
native DLL with the same exports for P/Invoke tests.
