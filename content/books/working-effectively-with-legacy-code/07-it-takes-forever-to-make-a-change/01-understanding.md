---
title: "Understanding"
questions:
  - id: key-difference
    prompt: "What is the key difference between a well-maintained system and a legacy system when you need to make a change?"
    options:
      - id: o5ee011
        text: "Legacy systems are always larger"
      - id: off54b3
        text: "Well-maintained systems need no learning time"
      - id: easy-after
        text: "In both you may need time to figure things out, but in a well-maintained system the change is then usually easy and you feel more comfortable"
        correct: true
    explanation: "“In a well-maintained system, it might take a while to figure out how to make a change, but once you do, the change is usually easy and you feel much more comfortable with the system. In a legacy system, it can take a long time to figure out what to do, and the change is difficult also.”"
  - id: narrow
    prompt: "What does Feathers say you may feel after making a change in a legacy system?"
    options:
      - id: o6184e0
        text: "That you now understand the whole system"
      - id: o9e1ecd
        text: "That the system is easier than you thought"
      - id: narrow-understanding
        text: "That you haven't learned much beyond the narrow understanding the change required"
        correct: true
    explanation: "“You might also feel like you haven’t learned much beyond the narrow understanding you had to acquire to make the change.”"
  - id: remedy
    prompt: "What kind of system enables faster work, according to this section?"
    options:
      - id: small-pieces
        text: "Systems broken into small, well-named, understandable pieces"
        correct: true
      - id: od04db7
        text: "Systems with extensive comments"
      - id: o95f805
        text: "Systems written in interpreted languages"
    explanation: "“Systems that are broken up into small, well-named, understandable pieces enable faster work.” Chapters 16 and 17 offer help with understanding."
  - id: unavoidable
    prompt: "Is growing time-to-understand entirely avoidable?"
    options:
      - id: obd436e
        text: "Yes, with enough documentation"
      - id: partly
        text: "No — part of it is unavoidable when you are unfamiliar with the context"
        correct: true
      - id: o1e193a
        text: "Yes, with code generation"
    explanation: "“Part of this is unavoidable.” and “it is going to take a while to figure out how to make a change if we are unfamiliar with the context.”"
---
## Learning objectives

- Distinguish learning cost from change cost
- Connect understandability to small, well-named pieces

## In a nutshell

All systems have a learning cost. Legacy systems add a change cost on top, and the learning does
not accumulate into wider understanding.

## Connections to today

Code search, "go to implementation", AI summarisation and architecture diagrams generated from
code all reduce learning cost. None of them reduce change cost if the code itself is tangled.
