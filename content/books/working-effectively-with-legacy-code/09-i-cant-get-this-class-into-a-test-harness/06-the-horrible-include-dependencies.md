---
title: "The Case of the Horrible Include Dependencies"
questions:
  - id: cpp-vs-java
    prompt: "Why are C++ include dependencies worse for build times than Java/C# imports?"
    options:
      - id: textual
        text: "Headers are textually included and reparsed every time, and transitive includes can pull in tens of thousands of lines"
        correct: true
      - id: o9f3a95
        text: "Java compiles everything at once"
      - id: oaba134
        text: "C++ compilers are written badly"
    explanation: "“the declaration of the class (in another file) is textually included in the file that needs to use it.” “The compiler has to reparse the declaration” and “it’s not uncommon to find small files that end up transitively including tens of thousands of lines of code.”"
  - id: same-directory
    prompt: "Why put `SchedulerTests` in the same directory as `Scheduler`?"
    options:
      - id: ob2dc3e
        text: "C++ requires it"
      - id: o6a1cd5
        text: "It speeds up linking"
      - id: paths
        text: "With the preprocessor and inconsistent include paths, it is often just easier"
        correct: true
    explanation: "“In the presence of the preprocessor, it is often just easier. If the project doesn’t use paths to include files in consistent ways, we could have a lot of work to do if we try to create the tests in other directories.”"
  - id: add-includes
    prompt: "What is the best tack for supplying includes?"
    options:
      - id: o575c78
        text: "Copy all `#include`s from the source file"
      - id: o62550f
        text: "Include a precompiled header of everything"
      - id: one-at-a-time
        text: "Add them one at a time and decide whether each dependency is really needed"
        correct: true
    explanation: "“We could just copy over all of the #include directives from the Scheduler class source file, but the fact is, we might not need them all. The best tack to take is to add them one at a time”."
  - id: separate-program
    prompt: "Why does providing an alternative `SchedulerDisplay::displayEntry` require a separate test program?"
    options:
      - id: o819162
        text: "Display code can't be linked with tests"
      - id: of11bb1
        text: "The test framework needs its own executable anyway"
      - id: odr
        text: "There can be only one definition of each method in a program"
        correct: true
    explanation: "“We can have only one definition for each method in SchedulerDisplay in a program, so we need to have a separate program for our scheduler tests.”"
  - id: downsides
    prompt: "What are the serious downsides of this technique?"
    type: multiple
    options:
      - id: not-cleaner
        text: "You aren't breaking dependencies at the language level, so the code isn't getting cleaner"
        correct: true
      - id: o7ed015
        text: "It only works on Windows"
      - id: maintain
        text: "Duplicate fake definitions must be maintained as long as the tests exist"
        correct: true
      - id: program
        text: "You must create a separate program"
        correct: true
    explanation: "“We have to create that separate program, and we really aren’t breaking dependencies at the language level, so we aren’t making the code cleaner” and “those duplicate definitions … have to be maintained as long as we keep this set of tests in place.”"
  - id: when
    prompt: "When does Feathers reserve this technique?"
    options:
      - id: oda6a06
        text: "For every new C++ class"
      - id: o6e7b34
        text: "Never"
      - id: huge
        text: "For very huge classes with severe dependency problems — it can act as a testing point for lots of refactoring"
        correct: true
    explanation: "“I reserve this technique for cases in which I have a very huge class with very severe dependency problems.” “It can act as a testing point for a lot of refactoring.”"
---
## Learning objectives

- Explain why C++ headers make classes hard to isolate
- Use a fakes header and separate test program, knowing the costs

## Connections to today

- Modern C++ modules (C++20) and tools such as include-what-you-use address the textual include
  problem directly.
- In .NET, the analogue is a test project that must reference half the solution. Splitting
  projects (Chapter 7) is the language-level fix the include trick lacks.
