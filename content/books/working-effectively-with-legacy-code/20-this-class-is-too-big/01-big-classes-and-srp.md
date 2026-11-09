---
title: "Big Classes and the Single Responsibility Principle"
questions:
  - id: problems
    prompt: "Which problems with big classes does Chapter 20 describe?"
    type: multiple
    options:
      - id: oceecaa
        text: "They compile faster"
      - id: testing
        text: "They are a pain to test because they hide too much"
        correct: true
      - id: confusion
        text: "Confusion — hard to know what to change and what it affects"
        correct: true
      - id: scheduling
        text: "Task scheduling — many reasons to change cause concurrent programmers to thrash"
        correct: true
    explanation: "“The first is confusion.” “Another problem is task scheduling.” “particularly because of the third problem: Big classes are a pain to test.”"
  - id: over-encapsulation
    prompt: "How can too much encapsulation lead to Edit and Pray?"
    options:
      - id: oeb471e
        text: "Encapsulation always prevents bugs"
      - id: ob9f473
        text: "Encapsulation slows the CPU"
      - id: rot
        text: "When classes hide too much, there's no easy way to sense effects of change, so the inside rots and people fall back to Edit and Pray"
        correct: true
    explanation: "“when we encapsulate too much, the stuff inside rots and festers. There isn’t any easy way to sense the effects of change, so people fall back on Edit and Pray (9) programming.”"
  - id: not-worse
    prompt: "What are the key tactics for working in a big class without making it worse?"
    options:
      - id: oe098f9
        text: "Add code inline to existing methods"
      - id: o6fc2c9
        text: "Make all methods public"
      - id: sprouts
        text: "Sprout Class and Sprout Method"
        correct: true
    explanation: "“The key tactics we can use here are Sprout Class (63) and Sprout Method (59).” Sprout Method helps subtly: “you are identifying and naming another thing that the class does”."
  - id: srp
    prompt: "How does the book state the Single Responsibility Principle?"
    options:
      - id: o6c61bb
        text: "Every class should have one method"
      - id: o2f87b9
        text: "Every class should have one field"
      - id: srp-def
        text: "Every class should have a single purpose in the system, and only one reason to change"
        correct: true
    explanation: "“Every class should have a single responsibility: It should have a single purpose in the system, and there should be only one reason to change it.”"
  - id: naive
    prompt: "Does SRP mean every class should have only a single method?"
    options:
      - id: main-purpose
        text: "No — responsibility comes into focus when we talk about main purpose"
        correct: true
      - id: o63425e
        text: "Only for public methods"
      - id: o711894
        text: "Yes — one method per class"
    explanation: "“If we look at it in a very naïve way, we might say, \"Oh, that means that every class should have only a single method, right?\"” — “But what we mean by a responsibility really comes into focus when we talk about main purpose.”"
  - id: ruleparser
    prompt: "Which responsibilities does `RuleParser` have?"
    type: multiple
    options:
      - id: evaluation
        text: "Expression evaluation"
        correct: true
      - id: parsing
        text: "Parsing"
        correct: true
      - id: variables
        text: "Variable management"
        correct: true
      - id: tokenization
        text: "Term tokenization"
        correct: true
      - id: oda0b47
        text: "Persistence"
    explanation: "“To summarize, it seems that Parser has the following responsibilities”: parsing, expression evaluation, term tokenization and variable management."
  - id: castle
    prompt: "Why does Feathers call the four-class `RuleParser` redesign \"a castle in the sky\"?"
    options:
      - id: hypothetical
        text: "Unless you're rewriting that part, it's hypothetical — the real task is incremental movement toward focused responsibilities"
        correct: true
      - id: o88752f
        text: "It uses too many patterns"
      - id: oa87460
        text: "It is impossible to implement"
    explanation: "“Unless we are choosing to rewrite this part of the system, our little multiclass design is a castle in the sky.” — “the key is to identify the different responsibilities and then figure out a way to incrementally move toward more focused responsibilities.”"
---
## Learning objectives

- Explain the costs of big classes
- Apply SRP by main purpose, not method count

## Connections to today

Merge conflicts on the same 5,000-line `OrderService.cs` are the "task scheduling" problem in
git form. Partial classes hide size but don't reduce responsibilities.
