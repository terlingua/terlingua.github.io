---
title: "Taking Advantage of Object Orientation"
questions:
  - id: properties
    prompt: "Which properties of object seams does Chapter 19 list?"
    type: multiple
    options:
      - id: flexible
        text: "They provide flexibility useful for later extension"
        correct: true
      - id: o15939c
        text: "They require no code changes"
      - id: notice
        text: "They are easy to notice in the code"
        correct: true
      - id: smaller
        text: "They can break code into smaller, more understandable pieces"
        correct: true
    explanation: "“They are easy to notice in the code.” “They can be used to break code down into smaller, more understandable pieces.” “They provide more flexibility. Seams that you introduce for testing might be useful when you have to extend your software.”"
  - id: first-step
    prompt: "When your language allows a move to OO, what's usually the first step?"
    options:
      - id: oaea476
        text: "Add interfaces everywhere"
      - id: encapsulate
        text: "Encapsulate Global References to get the pieces you're changing under test"
        correct: true
      - id: o52a85b
        text: "Rewrite in a new language"
    explanation: "“The first step is usually to use Encapsulate Global References (339) to get the pieces you are changing under test.”"
  - id: compile-cpp
    prompt: "Before wrapping `ksr_notify` in a class, what must happen?"
    options:
      - id: ofec866
        text: "Convert all structs to classes"
      - id: cpp
        text: "Compile the code as C++ rather than C — all at once or piece by piece"
        correct: true
      - id: o5ec7ce
        text: "Delete `ksr_notify`"
    explanation: "“The first step is to compile under C++ rather than under C. This can be either a small or a large change, depending on how we handle it.”"
  - id: preserve
    prompt: "Why does `ResultNotifier::ksr_notify` keep the same name and signature as the C function?"
    options:
      - id: preserve-sig
        text: "Preserve Signatures minimises the chance of errors"
        correct: true
      - id: o1bf2db
        text: "C++ requires identical names"
      - id: o0e668a
        text: "For performance"
    explanation: "“Notice that we’re not changing the name of the function or its signature. We’re using Preserve Signatures (312) so that we minimize any chance of errors.”"
  - id: compiler-guides
    prompt: "After moving `ksr_notify`'s declaration into the class, how do you find the call sites to change?"
    options:
      - id: o368917
        text: "Run the tests"
      - id: oc4a3e6
        text: "Search for the string manually only"
      - id: compile-errors
        text: "Recompile and let the errors show where — the function is no longer declared at global scope"
        correct: true
    explanation: "“Now we can recompile and let the errors tell us where we have to change things. Because we’ve put the declaration of ksr_notify in a class, the compiler doesn’t see a declaration of it at global scope any longer.”"
  - id: next-steps
    prompt: "After calls go through `globalResultNotifier`, how do you swap in a test notifier?"
    options:
      - id: oa4920b
        text: "Reassign the global in every test"
      - id: o5a0995
        text: "Use a link seam"
      - id: scanner
        text: "Encapsulate `scan_packets` in a `Scanner` class and Parameterize Constructor to take a `ResultNotifier`, defaulting to the global"
        correct: true
    explanation: "“put scan_packets in another class that we can call Scanner.” “Now we can apply Parameterize Constructor (379) and change the class so that it uses a ResultNotifier that we supply”."
  - id: quality
    prompt: "How does Feathers rate the resulting design?"
    options:
      - id: wedge
        text: "Not great OO design, but safe, mechanical and good enough as a wedge to break dependencies"
        correct: true
      - id: o489c9a
        text: "An ideal object-oriented design"
      - id: odf1747
        text: "Too risky to use"
    explanation: "“These changes are pretty safe and pretty mechanical. They aren’t great examples of object-oriented design, but they are good enough to use as a wedge to break dependencies and allow us to test as we move forward.”"
---
## Learning objectives

- Migrate a procedural dependency into an object seam step by step

## Connections to today

The same moves apply to VB6 → VB.NET or to large static C# classes: wrap the static call in an
instance method on a small class, make a global instance, route calls through it, then inject it.
