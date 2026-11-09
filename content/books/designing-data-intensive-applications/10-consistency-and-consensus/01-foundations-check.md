---
title: "Foundations Check — Before Chapter 10"
questions:
  - id: fetch-add
    prompt: "What does an atomic fetch-and-add on a counter return?"
    options:
      - id: ob94e4f
        text: "The previous value, while atomically incrementing the counter"
        correct: true
      - id: ocdb28c
        text: "A random value"
      - id: ofc3cb9
        text: "The new value only, non-atomically"
      - id: ofbbb48
        text: "Nothing"
    explanation: "Fetch-and-add atomically adds and returns the old value — the core of a single-node ID generator."
  - id: memory-barrier
    prompt: "Why might a thread on one core not immediately see another core's write to memory?"
    options:
      - id: o9a2970
        text: "RAM is too slow to write"
      - id: oc9ec99
        text: "Writes are always visible immediately"
      - id: o07685a
        text: "Per-core caches and store buffers delay visibility unless a memory barrier is used"
        correct: true
      - id: ob1de72
        text: "The OS hides writes from other threads"
    explanation: "Modern CPUs relax memory ordering for performance; fences force ordering."
  - id: uuid-bits
    prompt: "How many bits is a UUID?"
    options:
      - id: o73ee63
        text: "256"
      - id: o6d727f
        text: "64"
      - id: o785c92
        text: "128"
        correct: true
      - id: o734730
        text: "32"
    explanation: "UUIDs are 128-bit values, twice the size of a 64-bit integer key."
  - id: uuid-v4
    prompt: "Why is UUIDv4 ordering useless for knowing which record is newer?"
    options:
      - id: o850205
        text: "It is sequential"
      - id: ob52ffd
        text: "Its bits are random"
        correct: true
      - id: of3f6d9
        text: "It encodes a timestamp"
      - id: o285274
        text: "It encodes the MAC address"
    explanation: "Version 4 UUIDs are random; version 7 adds a timestamp prefix."
  - id: impossibility
    prompt: "What does an impossibility result in distributed computing tell you?"
    options:
      - id: oc2c7a4
        text: "No algorithm can guarantee certain properties under certain assumptions"
        correct: true
      - id: o0661a7
        text: "A problem has been solved"
      - id: of22f88
        text: "Nobody has tried yet"
      - id: o40e525
        text: "An algorithm is slow"
    explanation: "Results like FLP define boundaries; practical systems escape them by changing assumptions (timeouts, randomness)."
  - id: register
    prompt: "In distributed systems theory, what is a \"register\"?"
    options:
      - id: o041f9d
        text: "A single object that can be read and written, like one key in a key-value store"
        correct: true
      - id: o2cd0fa
        text: "A CPU register"
      - id: oa8d36e
        text: "A cash register"
      - id: o461e9c
        text: "A log of operations"
    explanation: "The chapter defines linearizability over registers — individual objects."
---
Chapter 10 builds on Chapters 6, 8 and 9, plus a few CPU and theory ideas. These questions are not drawn
from the book's text.

## Learning objectives

- Recall fetch-and-add and memory ordering basics
- Recall what an impossibility result is
- Recall UUID basics
