---
title: "Conversation Scrutiny"
questions:
  - id: locking
    prompt: "In the multithreading story, what did Feathers suggest when a programmer started writing the locking policy inline?"
    options:
      - id: o355a8a
        text: "Add comments explaining the counts"
      - id: class
        text: "Create a `LockingPolicy` class with descriptive method names instead of bumping counts in an array"
        correct: true
      - id: ocbb3c0
        text: "Remove multithreading"
    explanation: "“Why don’t we create a class called LockingPolicy and maintain the counts in there? We can use method names that really describe what we are trying to do”."
  - id: mesmerizing
    prompt: "Why did an experienced team miss the `LockingPolicy` abstraction?"
    options:
      - id: o3c6b0a
        text: "They lacked training"
      - id: procedural
        text: "Large chunks of procedural code are mesmerising — they seem to beg for more"
        correct: true
      - id: ob99ed2
        text: "The language didn't support classes"
    explanation: "“The terrible thing is that the team wasn’t inexperienced.” — “there is something mesmerizing about large chunks of procedural code: They seem to beg for more.”"
  - id: overlap
    prompt: "If there's little overlap between concepts in design conversations and concepts in code, what's usually the reason?"
    type: multiple
    options:
      - id: not-adapted
        text: "The code hasn't been allowed to adapt to the team's understanding"
        correct: true
      - id: o760f18
        text: "The language is too old"
      - id: understand-differently
        text: "The team needs to understand it differently"
        correct: true
    explanation: "“The answer is usually a mixture of two things: The code hasn’t been allowed to adapt to the team’s understanding, or the team needs to understand it differently.”"
  - id: design-over
    prompt: "What does Feathers call one of the worst mistakes a team can make?"
    options:
      - id: of1143d
        text: "Writing tests first"
      - id: over
        text: "Feeling that design is over at some point in development"
        correct: true
      - id: o5d6c22
        text: "Using too many classes"
    explanation: "“One of the worst mistakes a team can make is it to feel that design is over at some point in development.” — “There is no surer way to make a legacy system worse.”"
  - id: action
    prompt: "In a design discussion, people keep saying \"the pricing rules\", but the code has only `OrderService.CalculateTotal` with 400 lines. What does this section suggest?"
    options:
      - id: put-in-code
        text: "Put that understanding in the code — consider a `PricingRules` abstraction"
        correct: true
      - id: o3dc6e4
        text: "Rename the meeting topic to match the code"
      - id: o31188c
        text: "Ignore the conversation"
    explanation: "“When people talk about design, they are trying to make other people understand them. Put some of that understanding in the code.”"
---
## Learning objectives

- Listen for domain concepts missing from code
- Keep design alive throughout development

## Connections to today

This is the seed of Domain-Driven Design's "ubiquitous language" (Evans, 2003). Meeting notes and
PR discussions are rich sources of missing abstractions.
