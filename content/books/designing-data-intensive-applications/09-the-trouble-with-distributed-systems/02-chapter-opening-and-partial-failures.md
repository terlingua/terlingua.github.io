---
title: "Faults and Partial Failures"
questions:
  - id: one-in-million
    prompt: "Why does the chapter say one-in-a-million chances matter?"
    options:
      - id: od1ffd5
        text: "In a large enough system, one-in-a-million events happen every day"
        correct: true
      - id: o84a052
        text: "Because regulators require it"
      - id: o31acbd
        text: "Because tests run a million times"
      - id: ofa413c
        text: "They don't matter"
    explanation: "“It doesn’t matter whether there is only a one-in-a-million chance; in a large enough system, one-in-a-million events happen every day.”"
  - id: crash-preferred
    prompt: "Why do computers prefer to crash on an internal fault rather than continue?"
    options:
      - id: oc3be18
        text: "Wrong results are difficult and confusing to deal with"
        correct: true
      - id: o25661d
        text: "It speeds up reboots"
      - id: o1fc1cb
        text: "Operating systems cannot recover"
      - id: od259eb
        text: "Crashes are cheaper"
    explanation: "“If an internal fault occurs, we prefer a computer to crash completely rather than returning a wrong result, because wrong results are difficult and confusing to deal with.”"
  - id: partial-failure
    prompt: "What is a partial failure and what makes it hard?"
    options:
      - id: o9c01d2
        text: "Some parts break unpredictably while others work, and outcomes are nondeterministic — you may not know if something succeeded"
        correct: true
      - id: oa047a8
        text: "A failure of half the disks"
      - id: obee467
        text: "A failure only in tests"
      - id: o9a3908
        text: "A failure that always repeats"
    explanation: "“there may well be some parts of the system that are broken in an unpredictable way, even though other parts of the system are working fine. This is known as a partial failure. The difficulty is that partial failures are nondeterministic”."
  - id: reliable-from-unreliable
    prompt: "What opportunity does tolerating partial failures create?"
    options:
      - id: ob47bb9
        text: "Avoiding all testing"
      - id: o000a3e
        text: "Running without replication"
      - id: o4beabc
        text: "Eliminating the network"
      - id: o4dd3eb
        text: "Building a system more reliable than any single node, e.g., rolling upgrades without downtime"
        correct: true
    explanation: "“Fault tolerance therefore allows us to make distributed systems more reliable than single-node systems; we can build a reliable system from unreliable components.”"
---
On one computer things usually work or don't. In a distributed system, parts fail unpredictably while
others keep running — partial failure.

## Learning objectives

- Explain why single computers present an idealized, deterministic model
- Define partial failure and why it is nondeterministic
- Explain why fault tolerance lets us build reliable systems from unreliable parts

## In a nutshell

- In a large system, one-in-a-million events happen every day.
- Computers prefer crashing to returning wrong results.
- Partial failures may leave you not knowing whether something succeeded.
- "Suspicion, pessimism, and paranoia pay off."

## Connections to today

- **Cloud incident reports** routinely describe partial failures — one zone's control plane degraded while
  data planes kept working.
- **Kubernetes liveness and readiness probes** encode the chapter's distinction between "up" and "useful".
