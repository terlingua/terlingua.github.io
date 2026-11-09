---
title: "Architecture Awareness"
questions:
  - id: hack-points
    prompt: "In sprawling long-lived systems, where do people tend to add new features?"
    options:
      - id: o3ec097
        text: "Where the architecture intends"
      - id: hacks
        text: "At the \"hack points\" — the recent hacks they know best"
        correct: true
      - id: o8e4627
        text: "In new microservices"
    explanation: "“When they add new features, they go to the \"hack points\" because those are the areas that they know best.”"
  - id: obstacles
    prompt: "What gets in the way of a team's awareness of its architecture?"
    type: multiple
    options:
      - id: reactive
        text: "The team is so reactive, fighting emergencies, that it loses sight of the big picture"
        correct: true
      - id: long-time
        text: "The system is so complex it takes a long time to get the big picture"
        correct: true
      - id: o51f16c
        text: "The team uses too many design patterns"
      - id: no-picture
        text: "The system is so complex there is no big picture"
        correct: true
    explanation: "The three bullets: “takes a long time to get the big picture”, “there is no big picture”, and “The team is in a very reactive mode, dealing with emergency after emergency”."
  - id: architect
    prompt: "What is the strong caveat on using an architect to preserve the big picture?"
    options:
      - id: oee8dd7
        text: "There must be at least two architects"
      - id: in-team
        text: "The architect must be out in the team working with members day to day, or the code diverges"
        correct: true
      - id: o4dcb94
        text: "The architect must not write code"
    explanation: "“An architect has to be out in the team, working with the members day to day, or else the code diverges from the big picture.”"
  - id: everyone
    prompt: "What is \"the brutal truth\" about architecture?"
    options:
      - id: oebaedc
        text: "It must be documented in UML"
      - id: too-important
        text: "It's too important to leave to a few people — everyone touching the code should know it and have a stake in it"
        correct: true
      - id: oe4226b
        text: "It doesn't matter in legacy systems"
    explanation: "“The brutal truth is that architecture is too important to be left exclusively to a few people.” — “the key way to keep an architecture intact is to make sure that everyone on the team knows what it is and has a stake in it.”"
  - id: most-important
    prompt: "Why practice big-picture techniques often?"
    options:
      - id: ob9de99
        text: "To produce documentation for auditors"
      - id: oa94b96
        text: "To speed up builds"
      - id: keep-alive
        text: "They keep architectural concerns alive in the team; it's hard to pay attention to something you don't think about often"
        correct: true
    explanation: "“If you practice them often on a team, they will help keep architectural concerns alive in the team—and that’s perhaps the most important thing you can do to preserve architecture. It is hard to pay attention to something that you don’t think about often.”"
---
## Learning objectives

- Explain why architecture degrades when teams aren't aware of it
- Argue for shared ownership of architecture

## Connections to today

Architecture Decision Records (ADRs), C4 diagrams and "architecture katas" are modern tools for the
same goal — but only if the whole team reads and argues about them.
