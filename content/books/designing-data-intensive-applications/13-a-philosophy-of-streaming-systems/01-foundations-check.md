---
title: "Foundations Check — Before Chapter 13"
questions:
  - id: eager-lazy
    prompt: "What is the difference between eager and lazy evaluation?"
    options:
      - id: oc6955f
        text: "Lazy never computes values"
      - id: o9b39f9
        text: "Eager computes values as soon as they're defined; lazy computes them only when needed"
        correct: true
      - id: o2779e9
        text: "They are synonyms"
      - id: oc17a53
        text: "Eager is always faster"
    explanation: "The chapter compares the write path to eager evaluation and the read path to lazy evaluation."
  - id: post-retry
    prompt: "Why is resubmitting an HTML form with POST risky?"
    options:
      - id: oc0f13f
        text: "Browsers forbid resubmission"
      - id: oa933cb
        text: "POST requests are cached"
      - id: ofbae5e
        text: "POST is encrypted"
      - id: o9bba87
        text: "POST is not idempotent, so a resubmission may perform the action twice"
        correct: true
    explanation: "Browsers warn before resubmitting POST data because servers may treat it as a new action."
  - id: prg
    prompt: "What does the Post/Redirect/Get pattern do?"
    options:
      - id: oed691d
        text: "After a POST, the server redirects to a GET page so refreshing doesn't resubmit the form"
        correct: true
      - id: oc3d509
        text: "It converts POST into PUT"
      - id: o99b4d4
        text: "It retries failed POSTs"
      - id: o1e5b40
        text: "It caches POST responses"
    explanation: "It avoids accidental resubmission on refresh, though it cannot help if the original POST times out."
  - id: hash
    prompt: "What property of a cryptographic hash makes it useful for detecting tampering?"
    options:
      - id: o69acad
        text: "Hashes are reversible"
      - id: oa9f601
        text: "Any change to the input changes the hash unpredictably, and finding collisions is infeasible"
        correct: true
      - id: oa6f244
        text: "Hashes compress data losslessly"
      - id: o14cf81
        text: "Hashes are random each time"
    explanation: "Comparing hashes detects corruption or tampering cheaply."
  - id: merkle
    prompt: "What does a Merkle tree let you prove efficiently?"
    options:
      - id: o1c15a0
        text: "That a database is linearizable"
      - id: oa2a022
        text: "That a record is included in a dataset, using a logarithmic number of hashes"
        correct: true
      - id: o1e428f
        text: "That a dataset is sorted"
      - id: o759ddd
        text: "That a network is reliable"
    explanation: "Each internal node hashes its children; an inclusion proof is the path of sibling hashes to the root."
  - id: websocket
    prompt: "What do WebSockets and server-sent events enable?"
    options:
      - id: oa5a4a5
        text: "Servers pushing messages to browsers over a long-lived connection"
        correct: true
      - id: ob60a5e
        text: "Faster DNS lookups"
      - id: of4727e
        text: "Offline storage only"
      - id: o34db6d
        text: "Encrypting cookies"
    explanation: "They break the strict request/response pattern, letting servers notify clients of changes."
---
Chapter 13 pulls together ideas from the whole book. These questions are not drawn from the book's
text; they check prerequisites from networking, web development and cryptography.

## Learning objectives

- Recall eager versus lazy evaluation
- Recall HTTP POST retries and the Post/Redirect/Get pattern
- Recall hashes and Merkle trees
