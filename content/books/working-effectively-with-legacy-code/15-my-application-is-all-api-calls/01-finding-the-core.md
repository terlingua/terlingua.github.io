---
title: "Why API-Heavy Code Is Hard, and Finding the Core"
questions:
  - id: temptation
    prompt: "What is the immediate temptation with code that is \"just calling a library\"?"
    options:
      - id: no-tests
        text: "To say it doesn't really need tests because it's simple"
        correct: true
      - id: o775437
        text: "To rewrite the library"
      - id: o3fdd3a
        text: "To over-test it"
    explanation: "“The immediate temptation is to say that we don’t really need tests. After all, we aren’t really doing anything significant; we’re just calling a method here and there”."
  - id: harder
    prompt: "Why are systems littered with library calls harder than home-grown systems?"
    type: multiple
    options:
      - id: ocd64ae
        text: "Libraries are always buggy"
      - id: no-hints
        text: "All you can see are the API calls, so there are no hints at a design"
        correct: true
      - id: not-owned
        text: "You don't own the API, so you can't rename or add methods to make it clearer"
        correct: true
    explanation: "“The first reason is that it is often hard to see how to make the structure better because all you can see are the API calls.” “The second reason that API-intensive systems are difficult is that we don’t own the API.”"
  - id: first-step
    prompt: "What is the first step toward better structure for `MailingListServer`?"
    options:
      - id: o1aaa04
        text: "Wrap every JavaMail class"
      - id: o7b6a71
        text: "Write integration tests against a real mail server"
      - id: core
        text: "Identify the computational core — what the code is really doing for us — e.g. by writing a brief description"
        correct: true
    explanation: "“The first step is to identify the computational core of code: What is this chunk of code really doing for us? It might help to try to write a brief description of what it does”."
  - id: real-work
    prompt: "Beyond input and output, what \"real work\" does the mailing list code do?"
    type: multiple
    options:
      - id: of2ece5
        text: "Encrypts every message"
      - id: thread
        text: "Runs a thread that sleeps and wakes periodically to check for mail"
        correct: true
      - id: new-messages
        text: "Builds new messages from incoming ones, setting fields and altering the subject line"
        correct: true
    explanation: "“We’re running a thread in the code. It sleeps and then wakes up periodically to check for mail. In addition, we aren’t just sending out the incoming mail messages again; we’re making new messages based on the incoming one.”"
  - id: api-tied
    prompt: "Of the four responsibilities, which is least tied to the mail API?"
    options:
      - id: sleep
        text: "Sleeping and waking periodically to check for mail"
        correct: true
      - id: o0ab41b
        text: "Sending a message"
      - id: o03d9a1
        text: "Making new messages for each incoming message"
      - id: o86fe86
        text: "Receiving incoming messages"
    explanation: "“Responsibility 4 doesn’t really have anything to do with mail; it just requires a thread that is set to wake up at certain intervals.” Responsibilities 1 and 2 “are definitely tied to the mail API.”"
  - id: forwarder
    prompt: "In the improved design, why is `MessageForwarder` the most valuable class to test, and how?"
    options:
      - id: o29ea8e
        text: "It is the only class with no dependencies at all"
      - id: o6c4321
        text: "It sends real mail during tests"
      - id: fake-sender
        text: "It holds the primary mailing-list logic; a `FakeMailSender` implementing `MailService` lets you test it without sending mail"
        correct: true
    explanation: "“The primary logic of the mailing list is in the MessageForwarder class, and we can get it under test.” “That’s easily achievable if we make a FakeMailSender class that implements the MailService interface.”"
  - id: one-big-object
    prompt: "What mental trick does Feathers suggest for a system that is nothing but API calls?"
    options:
      - id: big-object
        text: "Imagine it's one big object and apply the Chapter 20 responsibility-separation heuristics"
        correct: true
      - id: o4c1be0
        text: "Pretend the API doesn't exist"
      - id: o089922
        text: "Imagine it's a microservice"
    explanation: "“it helps to imagine that it is just one big object and then apply the responsibility-separation heuristics in Chapter 20”."
---
## Learning objectives

- Describe why API-dominated code resists structure
- Identify the computational core and separate responsibilities

## Connections to today

A typical Azure Function or AWS Lambda handler is "all API calls": trigger binding, SDK reads,
HTTP calls, a queue write. The computational core — the decision logic — is often ten lines that
can be pulled into a plain class and tested without the cloud.
