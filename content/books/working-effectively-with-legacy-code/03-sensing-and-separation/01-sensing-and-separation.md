---
title: "Sensing and Separation"
questions:
  - id: definitions
    prompt: "Match the two reasons to break dependencies with their meanings."
    options:
      - id: ob24f6e
        text: "Sensing — log more; Separation — use more threads"
      - id: oc7cc0c
        text: "Sensing — measure performance; Separation — split into microservices"
      - id: o2d2ac2
        text: "Sensing — get code into a harness; Separation — read computed values"
      - id: right
        text: "Sensing — we can't access values our code computes; Separation — we can't even get the code into a harness to run"
        correct: true
    explanation: "“1. Sensing—We break dependencies to sense when we can’t access values our code computes. 2. Separation—We break dependencies to separate when we can’t even get a piece of code into a test harness to run.”"
  - id: network-bridge
    prompt: "Which problems does `NetworkBridge` illustrate?"
    type: multiple
    options:
      - id: separate
        text: "Separation — we can't run it apart from the rest of the application and hardware"
        correct: true
      - id: o4feedb
        text: "Neither — sniffing packets solves it cheaply"
      - id: sense
        text: "Sensing — we can't tell what it does to the endpoints or hardware"
        correct: true
    explanation: "“This example illustrates both the sensing and separation problems. We can’t sense the effect of our calls to methods on this class, and we can’t run it separately from the rest of the application.” Packet sniffing etc. “could work, but they are an awful lot of work.”"
  - id: tougher
    prompt: "Which is tougher, sensing or separation?"
    options:
      - id: no-answer
        text: "There is no clear answer; usually we need both — but there are many separation techniques and one dominant sensing technique"
        correct: true
      - id: o56dde4
        text: "Separation, always"
      - id: occdb09
        text: "Sensing, always"
    explanation: "“Which problem is tougher? Sensing or separation? There is no clear answer. Typically, we need them both” and “There are many ways to separate software” … “but there is one dominant technique for sensing.”"
  - id: impersonate
    prompt: "When a class affects another class whose interface does not let you observe the effect, what does Chapter 3 say is the only choice?"
    options:
      - id: od1b0a3
        text: "Add logging to production code"
      - id: oc6680d
        text: "Make every field public"
      - id: obd182e
        text: "Test only through the UI"
      - id: impersonate-opt
        text: "Impersonate the other class so the effects can be sensed directly"
        correct: true
    explanation: "“Sometimes we can sense those effects through the interface of the other class. At other times, we can’t. The only choice we have is to impersonate the other class so that we can sense the effects directly.”"
  - id: classify
    prompt: "A `ReportService` emails a PDF through a real SMTP client inside a method you can already call in a test. You need to check the email body. Is this mainly sensing or separation?"
    options:
      - id: o162b37
        text: "Separation — the class can't be instantiated"
      - id: o9a0b44
        text: "Neither — just read the SMTP server logs"
      - id: sensing
        text: "Sensing — the code runs, but its effect (the email) is not observable in the test"
        correct: true
    explanation: "The code already runs, so the problem is that “we can’t access values our code computes” — a sensing problem. (Sending real mail would also be a reason to separate, but the stated need is to observe the body.)"
  - id: cpp-link
    prompt: "In which language does Chapter 3 single out link time as making rapid turnaround nearly impossible without breaking dependencies?"
    options:
      - id: oa3efe6
        text: "Smalltalk"
      - id: od52387
        text: "Java"
      - id: od7efa1
        text: "C#"
      - id: cpp
        text: "C++"
        correct: true
    explanation: "“In others, most notably C++, link time alone can make rapid turnaround nearly impossible if you don’t break dependencies.”"
---
## Learning objectives

- Define sensing and separation
- Classify a testing obstacle as sensing, separation or both

## In a nutshell

Two different pains drive dependency-breaking. **Separation**: the code cannot run in a harness
(hardware, databases, giant constructors). **Sensing**: it runs, but you cannot see what it did.
Separation has a whole catalog of techniques (Part III). Sensing has one dominant technique: fake
collaborators.

## Connections to today

- `HttpClient` calls, message-bus publishes and cloud SDK calls are classic sensing problems:
  the effect leaves the process. In .NET you can sense them with a fake `HttpMessageHandler`, an
  in-memory bus, or an interface over the SDK.
- Static `DateTime.Now` is a separation *and* sensing problem; .NET 8's `TimeProvider` with
  `FakeTimeProvider` is a ready-made seam for it.
