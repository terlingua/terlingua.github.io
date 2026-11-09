---
title: "The Procedural Dilemma and An Easy Case"
questions:
  - id: why-hard
    prompt: "Why are procedural languages especially challenging in legacy work?"
    options:
      - id: oda5f51
        text: "They can't be compiled on modern machines"
      - id: oa8f85f
        text: "They don't support functions"
      - id: few-seams
        text: "They often lack the seams that OO and many functional languages have, so few techniques introduce unit tests"
        correct: true
    explanation: "“the number of things you can do to introduce unit tests in procedural languages is pretty small.” and “Procedural languages often just don’t have the seams that OO (and many functional) programming languages do.”"
  - id: strategy
    prompt: "What is the standard strategy for procedural legacy code?"
    options:
      - id: o0d5a82
        text: "Rewrite in an OO language before testing"
      - id: ob30977
        text: "Test each function in isolation first"
      - id: big-chunk
        text: "Get a large chunk under test first — find a pinch point, use the link seam (and the preprocessing seam if available)"
        correct: true
    explanation: "“often the best strategy is to try to get a large chunk of the code under test before doing anything else” — “it pays to look for a pinch point (180) and then use the link seam (36) to break dependencies”."
  - id: easy
    prompt: "Why is `set_writetime` from Linux easy to test?"
    options:
      - id: of1a647
        text: "It has no parameters"
      - id: inputs-outputs
        text: "You can set `jiffies`, create a `buffer_head`, call the function and check its fields — no hard-to-handle calls"
        correct: true
      - id: o82b942
        text: "It is already tested in the kernel"
    explanation: "“To test this function, we can set the value of the jiffies variable, create a buffer_head, pass it into the function, and then check its values after the call.”"
  - id: typical
    prompt: "What usually makes procedural functions hard to test?"
    options:
      - id: io
        text: "Chains of calls end in something hard — I/O or a vendor library — whose effect only something outside the program can see"
        correct: true
      - id: o8389b6
        text: "Use of structs"
      - id: ob024a1
        text: "Too many comments"
    explanation: "“Sometimes a function calls a function that calls another function. Then it calls something hard to deal with: a function that actually does I/O someplace or comes from some vendor’s library.”"
---
## Learning objectives

- Explain why procedural code has fewer seams
- Recognise functions that are already testable

## Connections to today

Static utility classes in C# behave like procedural modules: no object seam, only "link-like"
options (wrapping, `Func<>` parameters, or runtime detours). Pure static functions, though, are
the easiest code of all to test — like `set_writetime`.
