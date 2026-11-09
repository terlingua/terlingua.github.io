---
title: "A Huge Sheet of Text"
questions:
  - id: sheet-view
    prompt: "What is wrong with viewing a program as \"a huge sheet of text\" when you try to get it under test?"
    options:
      - id: doesnt-cut-it
        text: "It hides the places where behaviour can be replaced; pulling classes out for testing demands a different view"
        correct: true
      - id: o933e76
        text: "Nothing — careful text editing is the safest approach"
      - id: o2b4e8e
        text: "It makes programs too modular"
    explanation: "“Pulling classes out of existing projects for testing really changes your idea of what \"good\" is with regard to design.” and “The idea of a program as a sheet of text just doesn’t cut it anymore.”"
  - id: reuse
    prompt: "What does Feathers observe about reuse of \"independent\" pieces?"
    options:
      - id: od49d0e
        text: "Reuse is easy once code is object-oriented"
      - id: subtle
        text: "Reuse is tough; pieces that look independent often depend on each other in subtle ways"
        correct: true
      - id: od5fec7
        text: "Small pieces are reused independently all the time"
    explanation: "“how often are small pieces reused independently? Not very often. Reuse is tough. Even when pieces of software look independent, they often depend upon each other in subtle ways.”"
  - id: good-design
    prompt: "Your system has a \"good\" layered design, yet extracting one class for testing takes days. Is that surprising per this section?"
    options:
      - id: oe9c4d8
        text: "Yes — it proves the design is actually bad"
      - id: o26e4ec
        text: "Yes — good designs are always easy to test"
      - id: not-surprising
        text: "No — you often have a lot of work to do regardless of how \"good\" the design is"
        correct: true
    explanation: "“Interestingly enough, you often have a lot of work to do, regardless of how \"good\" the design is.”"
  - id: testable-paths
    prompt: "What are the two ways to end up with an easily testable program, and which has had little success in the field?"
    options:
      - id: two-ways
        text: "Writing tests as you develop it, or designing for testability; the latter has not been very successful"
        correct: true
      - id: o1b38c3
        text: "Using mocks or using integration tests; mocks have failed"
      - id: o313f09
        text: "Static typing and code review; code review has failed"
    explanation: "“the only ways to end up with an easily testable program are to write tests as you develop it or spend a bit of time trying to \"design for testability.\" There is a lot of hope for the former approach, but if much of the code in the field is evidence, the latter hasn’t been very successful.”"
---
## Learning objectives

- Explain why the listing view of code is inadequate for testing work
- Recognise that "good" design and testable design are not the same thing

## In a nutshell

Most people edit programs as if they were documents: change a line, hope the meaning holds.
Testing legacy code forces a different view — a program as a set of places where you could swap
behaviour without editing. That view is the seam model.

## Connections to today

Designing for testability "up front" still tends to fail without tests driving it. Teams that
claim "clean architecture" often discover static helpers, `new` calls to infrastructure and
sealed SDK types the moment they try to write their first unit test.
