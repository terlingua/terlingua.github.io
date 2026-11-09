---
title: "Definition Completion"
questions:
  - id: what
    prompt: "What language capability does Definition Completion exploit?"
    options:
      - id: oaea1e4
        text: "Reflection"
      - id: o672c1b
        text: "Virtual dispatch"
      - id: declare-define
        text: "Declaring a function or method in one place (header) and defining it elsewhere — so tests can supply their own definitions"
        correct: true
    explanation: "“In some languages, we can declare a type in one place and define it in another.” — “We include the header containing this class declaration in the test file and provide alternate definitions for the methods before our tests.”"
  - id: separate-exe
    prompt: "Why are you \"pretty much obligated\" to build a separate test executable?"
    options:
      - id: o0e668a
        text: "For performance"
      - id: o8f9d6d
        text: "Tests need their own main"
      - id: clash
        text: "The completed definitions would clash with the real ones at link time"
        correct: true
    explanation: "“we are pretty much obligated to create a separate executable for the tests that use the completed definitions. If we don’t, they will clash with the real definitions at link time.”"
  - id: downsides
    prompt: "What other downsides does it have?"
    type: multiple
    options:
      - id: two-sets
        text: "Two sets of definitions to maintain — a big burden"
        correct: true
      - id: of2dea8
        text: "It only works in Java"
      - id: debugger
        text: "It can confuse debuggers if the environment isn't set up correctly"
        correct: true
    explanation: "“we now have two different sets of definitions for the methods of a class, one in a test source file and another in a production source file. This can be a big maintenance burden. It can also confuse debuggers”."
  - id: recommendation
    prompt: "What is Feathers' recommendation on using it?"
    options:
      - id: o563b95
        text: "Use it routinely"
      - id: worst
        text: "Only in the worst dependency situations, just to break initial dependencies, then get the class under test quickly and remove the duplicates"
        correct: true
      - id: oec0830
        text: "Never use it"
    explanation: "“I don’t recommend using Definition Completion except in the worst dependency situations. Even then, I recommend doing it just to break initial dependencies.”"
  - id: step-verify
    prompt: "Which verification steps appear in the procedure?"
    type: multiple
    options:
      - id: oaea80a
        text: "Verify the class has no virtual methods"
      - id: not-in-build
        text: "Verify that the class's source files are not part of the test build"
        correct: true
      - id: source-not-header
        text: "Verify that method definitions are in a source file, not a header"
        correct: true
    explanation: "“2. Verify that the method definitions are in a source file, not a header.” and “4. Verify that the source files for the class are not part of the build.”"
---
## Learning objectives

- Recognise Definition Completion as a C/C++ link-level technique of last resort

## Connections to today

C# `partial` methods let one part declare and another implement, but both parts compile into the
same assembly — they are not a test seam. In .NET, this technique has no direct analogue.
