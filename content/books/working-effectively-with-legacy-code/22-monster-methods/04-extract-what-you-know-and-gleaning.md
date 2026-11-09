---
title: "Extract What You Know, Gleaning Dependencies, Method Objects"
questions:
  - id: little
    prompt: "How small is \"little\" in Extract What You Know?"
    options:
      - id: two-five
        text: "Two or three lines — five at most — a chunk you can easily name"
        correct: true
      - id: o475d6f
        text: "One screen of code"
      - id: o48bf7b
        text: "One whole section"
    explanation: "“When I say \"little,\" I mean two or three lines—five, at most, a chunk of code that you can easily name.”"
  - id: coupling-count
    prompt: "What is the coupling count of an extraction?"
    options:
      - id: ocfe1bb
        text: "The number of classes it depends on"
      - id: o742928
        text: "The number of lines"
      - id: in-out
        text: "The number of values that pass into and out of the extracted method (instance variables don't count)"
        correct: true
    explanation: "“The coupling count is the number of values that pass into and out of the method you are extracting.” “Accesses of instance variables don’t count”."
  - id: max-count
    prompt: "Extracting `max(a, b)` from `process(int a, int b, int c)` has what coupling count?"
    options:
      - id: o6526b8
        text: "4 — including `c`"
      - id: oc81e72
        text: "2"
      - id: three
        text: "3 — two in, one out"
        correct: true
    explanation: "“The coupling count of the method is 3: two variables in and one variable out.”"
  - id: printed-max
    prompt: "As printed, the \"before\" code is `if (a > b) maximum = a; maximum = b;`. What does it actually compute, and why does that matter when extracting?"
    options:
      - id: o6d7f1f
        text: "The maximum of a and b"
      - id: always-b
        text: "`maximum` always ends up as `b` (no `else`), so an extraction to `max(a,b)` would change behaviour — exactly why you characterize before extracting"
        correct: true
      - id: o43a858
        text: "It doesn't compile"
    explanation: "The listing reads “if (a > b) maximum = a; maximum = b;”. Without an `else`, the second assignment always wins. Whether a typo or not, a test pinning current behaviour would reveal that `max(a,b)` isn't a pure refactoring here."
  - id: zero-count
    prompt: "Why are 0-count extractions the safest, and what else do they offer?"
    options:
      - id: o02a584
        text: "They run faster"
      - id: od4e543
        text: "They can be deleted later"
      - id: commands
        text: "No values pass in or out, and naming these \"commands\" often yields insight into what chunks do"
        correct: true
    explanation: "“If extractions with a low coupling count are safer, then extractions with a count of 0 must be the safest of all—and they are.” “when you attempt to name chunks of code like this, you often end up getting more insight”."
  - id: key-danger
    prompt: "What is the key danger in a method extraction, and how do you reduce it?"
    options:
      - id: odebb9b
        text: "Name clashes — use long names"
      - id: type-conversion
        text: "A type conversion error — prefer low coupling counts and check where each passed variable is declared"
        correct: true
      - id: oe39efb
        text: "Too many methods — avoid extraction"
    explanation: "“The key danger in a method extraction is a type conversion error.” “we should look back and find where each variable that is passed is declared, to make sure that we get the method signature right.”"
  - id: gleaning
    prompt: "What is Gleaning Dependencies?"
    options:
      - id: protect-critical
        text: "Write tests for the critical logic you must preserve, then extract secondary code (like display) that the tests don't cover"
        correct: true
      - id: o2f3e81
        text: "Deleting unused imports"
      - id: o98d812
        text: "Collecting all dependencies into one class"
    explanation: "“You write tests for the logic that you need to preserve. Afterward, you extract things that the tests do not cover. When you do this, you can at least have confidence that you are preserving the important behavior.”"
  - id: gleaning-why
    prompt: "Why is gleaning acceptable even though it feels like a cop-out?"
    options:
      - id: o5e5392
        text: "Display code never has bugs"
      - id: unequal
        text: "Not all behaviours are equal — errors in display code show up fast, errors in add logic may hide for a long time"
        correct: true
      - id: o2fc460
        text: "Tests are optional"
    explanation: "“If we make a mistake with the display code, we’ll see it pretty quickly. An error in the add logic, though, is something that might take quite a while to find.” “But not all behaviors are equal in an application.”"
  - id: method-object
    prompt: "Why choose Break Out Method Object over turning locals into instance variables?"
    options:
      - id: o2b096b
        text: "Locals can't be read by tests"
      - id: production-vars
        text: "Instance variables shared only by the monster and its extractions are confusing; a method object makes those variables legitimately part of a class you can sense through, with tests you can keep"
        correct: true
      - id: oe6899d
        text: "Method objects are faster"
    explanation: "Turning locals into fields “can be confusing.” With a method object, “We can turn the temporary variables in the method into instance variables and sense through them” and “unlike introducing a sensing variable, the variables that you are using are needed for production. This allows you to build up tests that you can keep.”"
---
## Learning objectives

- Choose low-coupling extractions and compute coupling counts
- Protect critical behaviour with Gleaning Dependencies
- Know when to break out a method object

## Connections to today

C#'s "Extract Method" dialog shows parameters and return values — your coupling count at a glance.
Local functions can be a stepping stone: extract to a local function first, then promote it.
