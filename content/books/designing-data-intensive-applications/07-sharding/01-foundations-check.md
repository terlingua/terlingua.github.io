---
title: "Foundations Check — Before Chapter 7"
questions:
  - id: mod-move
    prompt: "Keys are placed with `hash % N`. N changes from 3 to 4. Roughly what fraction of keys stay put?"
    options:
      - id: o21e665
        text: "About a quarter"
        correct: true
      - id: oc61828
        text: "About three quarters"
      - id: o9023e8
        text: "All of them"
      - id: o3e5a36
        text: "None of them"
    explanation: "A key stays put only if `h % 3 == h % 4`, which holds for about 3 of every 12 consecutive hash values — roughly 25%. So most keys move, which is why the chapter rejects mod-N."
  - id: hash-uniform
    prompt: "What property makes a hash function useful for sharding?"
    options:
      - id: o9b6478
        text: "It preserves the order of keys"
      - id: ob06d13
        text: "It produces short outputs"
      - id: o022b6c
        text: "It is cryptographically secure"
      - id: o4bb6a8
        text: "It spreads similar inputs uniformly across its output range, deterministically"
        correct: true
    explanation: "Uniformity and determinism matter; cryptographic strength does not. Order preservation is exactly what hashing gives up."
  - id: power-of-two
    prompt: "A 16-bit hash produces values in what range?"
    options:
      - id: o9d813e
        text: "0 to 32,767"
      - id: oa31a39
        text: "0 to 65,535"
        correct: true
      - id: o3871b7
        text: "−32,768 to 32,767"
      - id: od7fa1a
        text: "0 to 16"
    explanation: "2^16 = 65,536 values, from 0 to 65,535 when unsigned."
  - id: divisors
    prompt: "Why might you choose 720 shards rather than 512?"
    options:
      - id: ode7779
        text: "720 is a power of two"
      - id: o4f6e6b
        text: "720 is prime"
      - id: ocb53bd
        text: "Databases require multiples of 10"
      - id: o1b9c01
        text: "720 is divisible by many numbers (1–6, 8, 9, 10, 12…), so shards spread evenly over many cluster sizes"
        correct: true
    explanation: "720 = 2⁴ × 3² × 5 has 30 divisors; 512 = 2⁹ has only 10. More divisors means more node counts that split shards evenly."
  - id: zipf
    prompt: "Popularity of keys in many real systems follows a heavy-tailed distribution. What does that imply?"
    options:
      - id: o9d45d0
        text: "Requests are normally distributed"
      - id: o1d4fff
        text: "All keys receive roughly equal requests"
      - id: ofca1dd
        text: "Hashing removes request skew"
      - id: ofd1faa
        text: "A small number of keys receive a large share of requests"
        correct: true
    explanation: "Heavy-tailed (e.g., Zipf-like) access means hot keys exist even when data is spread evenly — hashing spreads keys, not load per key."
---
Chapter 7 assumes hashing, modular arithmetic and basic probability. These questions are not drawn from
the book's text.

## Learning objectives

- Reason about hash distributions and modulo
- Estimate how many keys move when a mapping changes
