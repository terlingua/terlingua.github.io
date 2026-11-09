---
title: "Naked CRC"
questions:
  - id: crc
    prompt: "What does CRC stand for, and what goes on a card?"
    options:
      - id: class-resp-collab
        text: "Class, Responsibility, and Collaborations — the class name, its responsibilities and its collaborators"
        correct: true
      - id: o6751cc
        text: "Cyclic Redundancy Check — a checksum"
      - id: o68cdf1
        text: "Code Review Checklist — a list of review items"
    explanation: "“CRC stands for Class, Responsibility, and Collaborations. You mark up each card with a class name, its responsibilities, and a list of its collaborators”."
  - id: uml-confusion
    prompt: "What mistake did many people make after UML consolidated the notations?"
    options:
      - id: o40dc07
        text: "They stopped designing altogether"
      - id: o113b3f
        text: "They used CRC cards exclusively"
      - id: notation-method
        text: "They thought the notation was a method — draw plenty of diagrams, then write code"
        correct: true
    explanation: "“People started to think that the notation was a method, that UML was a way of developing systems: Draw plenty of diagrams, and then write code afterward.”"
  - id: naked
    prompt: "What makes Naked CRC \"naked\"?"
    options:
      - id: blank
        text: "You don't write on the cards — you use blank cards, position and motion to show objects interacting"
        correct: true
      - id: oa96bb1
        text: "It's done without a facilitator"
      - id: obefb57
        text: "There are no collaborators"
    explanation: "“dubbed Naked CRC because it is just like CRC, except that you don’t write on the cards.” “The key is that you are able to use motion and position to show how parts of the system interact.”"
  - id: guidelines
    prompt: "What are the two guidelines of Naked CRC?"
    type: multiple
    options:
      - id: oa4cb6a
        text: "Use one colour per layer"
      - id: overlap
        text: "Overlap cards to show a collection of them"
        correct: true
      - id: instances
        text: "Cards represent instances, not classes"
        correct: true
      - id: oc5e07b
        text: "Each card must list responsibilities"
    explanation: "“1. Cards represent instances, not classes. 2. Overlap cards to show a collection of them.”"
  - id: why-instances
    prompt: "Why do cards represent instances rather than classes?"
    options:
      - id: interactions
        text: "The technique conveys the typical objects in the system and how they interact at run time — e.g. many server sessions each with two connections"
        correct: true
      - id: o7d3a1b
        text: "Classes are too hard to draw"
      - id: o6308a6
        text: "Instances are always fewer than classes"
    explanation: "The describer moves cards “to convey the typical objects in the system and how they interact.” In the voting example, “We can have many sessions on the server side” — each shown as its own set of cards."
---
## Learning objectives

- Run a Naked CRC session to explain run-time interactions

## Connections to today

Sequence diagrams capture the same instance-level interactions, but Naked CRC is live and
physical. In remote teams, sticky notes on a shared whiteboard with someone narrating is the
closest equivalent.
