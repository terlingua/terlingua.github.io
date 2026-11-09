---
title: "Chapter Opening — Eventual Versus Strong Consistency"
questions:
  - id: philosophies
    prompt: "What distinguishes the eventual and strong consistency philosophies?"
    options:
      - id: oe456e9
        text: "Eventual uses leaders; strong is leaderless"
      - id: o6a0785
        text: "They are identical"
      - id: o5ad078
        text: "Eventual is faster always; strong is slower always"
      - id: o359af7
        text: "Eventual exposes replication to the app, which handles conflicts; strong makes the system behave like a single node"
        correct: true
    explanation: "Eventual consistency: “the fact that a system is replicated is made visible to the application”. Strong consistency: “the system should behave as if it were a single node.”"
  - id: strong-cost
    prompt: "What are the disadvantages of strong consistency?"
    type: multiple
    options:
      - id: oe707b5
        text: "Some faults that eventual consistency tolerates cause outages"
        correct: true
      - id: o5ea0e4
        text: "It makes application code harder"
      - id: od7bebf
        text: "It requires multi-leader replication"
      - id: o21841f
        text: "A performance cost"
        correct: true
    explanation: "“The disadvantage is that stronger consistency has a performance cost, and some kinds of fault that an eventually consistent system can tolerate cause outages in strongly consistent systems.”"
  - id: offline
    prompt: "Your app lets users edit while offline. Which consistency is inevitable?"
    options:
      - id: o50ac77
        text: "Eventual consistency"
        correct: true
      - id: of3401a
        text: "Linearizability"
      - id: o8ad016
        text: "Strict serializability"
      - id: o1008a3
        text: "Strong consistency"
    explanation: "“If your app allows users to make changes to data while offline, eventual consistency is inevitable”."
  - id: hard
    prompt: "Why does the chapter warn about these topics?"
    options:
      - id: ocadbdc
        text: "They only apply to blockchains"
      - id: oeed19a
        text: "They require GPUs"
      - id: o601c38
        text: "Systems that work without faults can fall apart with an unlucky combination of faults or message orderings"
        correct: true
      - id: o1aa19b
        text: "They are mathematically trivial"
    explanation: "“It’s very easy to build systems that behave fine when there are no faults but completely fall apart when faced with an unlucky combination of faults or message orderings that their designers hadn’t considered.”"
---
Two philosophies: expose replication to the application (eventual consistency) or hide it (strong
consistency). This chapter makes "strong" precise and shows how to achieve it with consensus.

## Learning objectives

- Contrast eventual and strong consistency philosophies and when each fits

## In a nutshell

- Offline editing forces eventual consistency.
- Fast, reliable links make strong consistency affordable.
- Strong consistency = linearizability; fault-tolerant linearizability needs consensus.

## Connections to today

- **Global SQL databases** let teams choose per-region strong reads versus faster stale reads — the
  philosophical choice made per query.
