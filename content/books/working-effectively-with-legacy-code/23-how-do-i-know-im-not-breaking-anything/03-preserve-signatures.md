---
title: "Preserve Signatures"
questions:
  - id: why
    prompt: "Why preserve signatures when breaking dependencies without tests?"
    options:
      - id: o429b30
        text: "It improves performance"
      - id: copy-paste
        text: "Without signature changes you can cut/copy/paste whole signatures, minimising the chance of errors"
        correct: true
      - id: o85c949
        text: "Signatures can't be changed in Java"
    explanation: "“When you avoid changing signatures at all, you can cut/copy and paste entire method signatures from place to place and minimize any chances of errors.”"
  - id: good-intentions
    prompt: "What went wrong when Feathers created `OrderBatch` and `CompensationTarget` while extracting?"
    options:
      - id: o3f5ce1
        text: "Nothing; it worked well"
      - id: mistakes
        text: "Trying to improve design during dependency breaking led to foolish mistakes that, without tests, were found much later"
        correct: true
      - id: o1842fd
        text: "The classes were too small"
    explanation: "“I wanted to make the design better as I was breaking dependencies, but it didn’t work out very well. I ended up making foolish mistakes, and with no tests to catch them, often they were found far later than they needed to be.”"
  - id: steps
    prompt: "What is the core of the paste-based signature procedure?"
    options:
      - id: o89577b
        text: "Retype each parameter carefully"
      - id: paste-delete-types
        text: "Copy the argument list, paste it into the new declaration and into the call, then delete the types in the call"
        correct: true
      - id: o423909
        text: "Use an IDE wizard only"
    explanation: "The steps: “I copied the entire argument list into my cut/copy paste buffer” — “I pasted the buffer into the new method declaration” — “I pasted the buffer into the call” — “Finally, I deleted the types, leaving the names of the arguments”."
  - id: lingering
    prompt: "Once these moves are automatic, what other risk can you concentrate on?"
    options:
      - id: o9446a9
        text: "Performance"
      - id: hiding
        text: "Whether the new method hides a base-class method with the same signature"
        correct: true
      - id: o12e6d0
        text: "Naming conventions"
    explanation: "“You can concentrate on some of the other lingering issues that can cause errors when you break dependencies. For instance, is your new method hiding a method with the same name signature in a base class?”"
  - id: method-object-use
    prompt: "Besides new method declarations, where else is Preserve Signatures used?"
    options:
      - id: od44be8
        text: "Renaming classes"
      - id: occ554b
        text: "Writing tests"
      - id: method-object
        text: "Creating instance variables for all of a method's arguments in Break Out Method Object"
        correct: true
    explanation: "“You can also use it to create a set of instance methods for all of the arguments to a method when you are doing the Break out Method Object refactoring.”"
---
## Learning objectives

- Keep signatures identical during untested dependency breaking
- Defer design improvements until tests exist

## Connections to today

IDE "Extract Method" preserves parameter types for you; Preserve Signatures matters most when you
must edit by hand or when tool support is weak (large Razor files, generated code, VB6 migrations).
