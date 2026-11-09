---
title: "Foundations Check — Before Chapter 12"
questions:
  - id: bounded
    prompt: "What makes a dataset \"unbounded\"?"
    options:
      - id: o250972
        text: "It is stored in RAM"
      - id: o16ca33
        text: "It keeps arriving over time and is never complete"
        correct: true
      - id: o9c4864
        text: "It has no schema"
      - id: o52cdf5
        text: "It is larger than one disk"
    explanation: "Batch assumes a known, finite input; streams never end."
  - id: idempotent-ops
    prompt: "Which operation is idempotent?"
    options:
      - id: oaf6a56
        text: "Appending to a list"
      - id: oce210d
        text: "Setting a key to a specific value"
        correct: true
      - id: o842055
        text: "Incrementing a counter"
      - id: oe9c144
        text: "Sending an email"
    explanation: "Repeating `SET x = 5` has the same effect as doing it once; increments and appends do not."
  - id: disk-calc
    prompt: "A 20 TB disk written at 250 MB/s fills in roughly how long?"
    options:
      - id: o9acc50
        text: "About 2 hours"
      - id: o38805c
        text: "About 22 minutes"
      - id: o60feef
        text: "About 1 week"
      - id: oea760d
        text: "About 22 hours"
        correct: true
    explanation: "20,000,000 MB ÷ 250 MB/s = 80,000 s ≈ 22 hours — the chapter's back-of-the-envelope figure for log retention."
  - id: pubsub
    prompt: "In publish/subscribe, how do producers and consumers relate?"
    options:
      - id: o44d181
        text: "Producers wait for each consumer to finish"
      - id: od804d8
        text: "Each producer connects directly to each consumer"
      - id: obd29e2
        text: "Producers publish to a topic without knowing which consumers will receive the messages"
        correct: true
      - id: o3068f8
        text: "Consumers must poll producers"
    explanation: "Pub/sub decouples senders from receivers via named topics."
  - id: derivative
    prompt: "If a bank balance is the running sum of transactions, what are the transactions relative to the balance?"
    options:
      - id: o3ce844
        text: "A snapshot"
      - id: od13815
        text: "Its changes over time — like a derivative of the balance"
        correct: true
      - id: of0b81e
        text: "Its integral"
      - id: o13240b
        text: "Unrelated data"
    explanation: "The chapter uses this calculus analogy: state integrates the event stream; the stream differentiates state."
  - id: at-least-once
    prompt: "A consumer may crash after processing but before acknowledging. Under at-least-once delivery, what happens?"
    options:
      - id: o30d77c
        text: "The consumer is banned"
      - id: o4d9353
        text: "The message is redelivered and may be processed twice"
        correct: true
      - id: o6fcea9
        text: "The message is lost"
      - id: o77685b
        text: "The broker deletes it"
    explanation: "At-least-once avoids loss by accepting possible duplicates; exactly-once effects need idempotence or atomic commit."
---
Chapter 12 assumes familiarity with queues, pub/sub, and the time and ordering ideas from Chapters 6
and 9. These questions are not drawn from the book's text.

## Learning objectives

- Recall queue versus topic semantics
- Recall idempotence and offsets
- Reason about rates and capacities
