---
title: "Learning from Effect Analysis"
questions:
  - id: basic-goodness
    prompt: "What has happened when, in a familiar code base, you feel you don't need to look for certain effects?"
    options:
      - id: o0c884c
        text: "The code has no effects"
      - id: goodness
        text: "You've found some \"basic goodness\" — implicit rules that let you stop being paranoid"
        correct: true
      - id: od5146f
        text: "You've become complacent"
    explanation: "“When you feel that way, you’ve found some \"basic goodness\" in your code base. In the best code, there aren’t many \"gotchas.\"”"
  - id: stupid-test
    prompt: "How does Feathers suggest discovering a code base's rules?"
    options:
      - id: of96682
        text: "Ask the architect"
      - id: o4a6ced
        text: "Read the style guide"
      - id: would-be-stupid
        text: "Think of an effect you've never seen there and say, \"But, no, that would be stupid\""
        correct: true
    explanation: "“The best way to find these rules is to think of a way that a piece of software could have an effect on another, a way that you’ve never seen in the code base. Then say to yourself, \"But, no, that would be stupid.\"”"
  - id: rules-nature
    prompt: "What are these rules usually like?"
    options:
      - id: contextual
        text: "Contextual — e.g. \"nobody modifies a list after passing it to a constructor\""
        correct: true
      - id: obe46b5
        text: "Grand style statements such as \"never use protected variables\""
      - id: o998b3c
        text: "Enforced by the compiler"
    explanation: "“The \"rules\" for a code base aren’t necessarily grand statements of programming style” — “Instead, they are often contextual things.” The `CppClass` list is “an ideal candidate for a \"but that would be stupid\" rule.”"
  - id: narrow
    prompt: "What generally happens as we narrow effects in a program?"
    options:
      - id: o0b2470
        text: "Testing becomes impossible"
      - id: easier
        text: "Programming gets easier because we need to know less to understand a piece of code"
        correct: true
      - id: o187da0
        text: "Programs get slower"
    explanation: "“In general, programming gets easier as we narrow effects in a program. We need to know less to understand a piece of code.” In OO languages, “restricting effects can make testing much easier, and there aren’t any hurdles to doing it.”"
  - id: bad-code
    prompt: "How do \"rules\" look in bad code?"
    options:
      - id: unknown-exceptions
        text: "People don't know what they are, or they're littered with exceptions"
        correct: true
      - id: ocd750f
        text: "They are written down but ignored"
      - id: of21a86
        text: "There are too many of them"
    explanation: "“In bad code, people don’t know what the \"rules\" are, or the \"rules\" are littered with exceptions.”"
---
## Learning objectives

- Discover and articulate implicit code-base rules
- Explain how narrowing effects makes code easier

## Connections to today

Turn "that would be stupid" rules into enforced ones: analyzers (for example banning
`DateTime.Now`), architecture tests (NetArchTest, ArchUnitNET), and immutable types make the rule
mechanical rather than tribal.
