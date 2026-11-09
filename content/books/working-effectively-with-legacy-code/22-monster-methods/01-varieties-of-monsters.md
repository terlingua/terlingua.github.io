---
title: "Varieties of Monsters"
questions:
  - id: definition
    prompt: "What makes a method a \"monster method\" rather than just a long one?"
    options:
      - id: o9c2a55
        text: "It has no comments"
      - id: obe2985
        text: "It has more than 100 lines"
      - id: uncomfortable
        text: "It's so long and complex that you don't feel comfortable touching it"
        correct: true
    explanation: "“A monster method is a method that is so long and so complex that you really don’t feel comfortable touching it.”"
  - id: bulleted
    prompt: "What is a bulleted method?"
    options:
      - id: o96b12c
        text: "A method full of `switch` statements"
      - id: chunks
        text: "A method with nearly no indentation — a sequence of code chunks like a bulleted list"
        correct: true
      - id: o34643e
        text: "A method with bullet-point comments"
    explanation: "“A bulleted method is a method with nearly no indentation. It is just a sequence of code chunks that reminds you of a bulleted list.”"
  - id: deceptive
    prompt: "Why is the spacing between sections of a bulleted method \"a little bit deceptive\"?"
    options:
      - id: oa07fdb
        text: "The spaces are tabs"
      - id: o48fd88
        text: "The sections run in parallel"
      - id: temps
        text: "Temporary variables declared in one section are often used in the next, so you can't simply cut each section out"
        correct: true
    explanation: "“The space between the sections is a little bit deceptive because often temporary variables are declared in one section and used in the next.”"
  - id: snarled
    prompt: "What is a snarled method, and how do you know you have a real one?"
    options:
      - id: o08638c
        text: "A method that calls itself recursively"
      - id: vertigo
        text: "A method dominated by a large indented section; if you feel vertigo trying to line up the blocks, it's a real snarl"
        correct: true
      - id: o61f077
        text: "A method with many parameters"
    explanation: "“A snarled method is a method dominated by a single large, indented section.” and “The best way to know whether you have a real snarl is to try to line up the blocks in a long method. If you start to feel vertigo, you’ve run into a snarled method.”"
  - id: platypus
    prompt: "Are bulleted and snarled methods distinct types in practice?"
    options:
      - id: mixtures
        text: "No — like platypuses, real methods are mixtures; many snarls hide bulleted sections deep in nesting"
        correct: true
      - id: oa7c108
        text: "Yes — they never overlap"
    explanation: "“Methods out in the field are kind of like platypuses—they look like mixtures of several types.” and “Many snarls have long bulleted sections hidden deep in their nesting”."
  - id: tool-difference
    prompt: "How does having a safe Extract Method tool change the approach?"
    options:
      - id: ocd42b3
        text: "You no longer need any tests ever"
      - id: no-tests-needed
        text: "You don't need tests to verify extractions; without it you must be more conservative and are bounded by the tests you can get in place"
        correct: true
      - id: o4ce069
        text: "It makes no difference"
    explanation: "“If a tool can extract methods for you safely, you don’t need tests to verify your extractions.” “When you don’t have extract method support” — “your work is bounded by the tests you can get in place.”"
---
## Learning objectives

- Distinguish bulleted and snarled monsters
- Explain why tool support changes your strategy

## Connections to today

Cognitive-complexity metrics (SonarQube, Roslyn analyzers) flag snarls; plain line counts flag
bulleted methods. Sticky scroll and brace guides help with the "line up the blocks" test.
