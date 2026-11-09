---
title: "Chapter Opening and Data Integration"
questions:
  - id: opinionated
    prompt: "How does Chapter 13 differ in style from earlier chapters?"
    options:
      - id: o9cddf7
        text: "It is more opinionated, deep-diving into one philosophy rather than comparing approaches"
        correct: true
      - id: oe3c60b
        text: "It focuses on SQL syntax"
      - id: o1acd62
        text: "It has no examples"
      - id: o87a273
        text: "It is purely theoretical"
    explanation: "“This chapter is more opinionated than previous chapters, presenting a deep dive into one particular philosophy rather than comparing multiple approaches.”"
  - id: general-purpose
    prompt: "What does the chapter say about \"general-purpose\" databases?"
    options:
      - id: oc6d818
        text: "Every piece of software is designed for a particular usage pattern"
        correct: true
      - id: o5927a7
        text: "They don't exist"
      - id: o539857
        text: "They are always the best choice"
      - id: o133398
        text: "They are suitable for all workloads"
    explanation: "“Every piece of software—even a so-called “general-purpose” database—is designed for a particular usage pattern.”"
  - id: search-sor
    prompt: "Why do many applications combine an OLTP database with a search index?"
    options:
      - id: o627a18
        text: "Databases cannot store text"
      - id: o97edc1
        text: "Sophisticated search needs specialist tools, while search indexes are poor systems of record"
        correct: true
      - id: o233cd5
        text: "Search indexes are faster for writes"
      - id: o323d53
        text: "Regulations require two systems"
    explanation: "“more sophisticated search facilities require specialist information-retrieval tools. Conversely, search indexes are generally not very suitable as a durable system of record.”"
  - id: cdc-only
    prompt: "If CDC is the only way to update a search index, what can you say about the index?"
    options:
      - id: o21c824
        text: "It is always stale"
      - id: o76457f
        text: "It may have conflicting writes"
      - id: o57b798
        text: "It is entirely derived from the system of record and consistent with it, barring bugs"
        correct: true
      - id: od375e5
        text: "It is the system of record"
    explanation: "“If CDC is the only way of updating the index, you can be confident that the index is entirely derived from the system of record and therefore consistent with it (barring bugs in the software).”"
  - id: total-order-principle
    prompt: "What principle matters more than choosing CDC versus event sourcing?"
    options:
      - id: oef1854
        text: "Using JSON"
      - id: o172851
        text: "Using LWW"
      - id: o1b3986
        text: "Using a single region"
      - id: off19fe
        text: "Deciding on a total order of writes"
        correct: true
    explanation: "“Whether you use CDC or an event sourcing log is less important than the principle of deciding on a total order.”"
  - id: derived-vs-2pc
    prompt: "How do log-based derived data and distributed transactions differ in achieving correctness?"
    options:
      - id: ocdc595
        text: "They are identical"
      - id: o44b97c
        text: "Logs use 2PC internally"
      - id: o3e6348
        text: "Transactions use idempotence"
      - id: o6a56cd
        text: "Transactions use atomic commit; logs use deterministic retry and idempotence — and logs are usually asynchronous"
        correct: true
    explanation: "“Distributed transactions use an atomic commit protocol ... while log-based systems achieve correctness through deterministic retry and idempotence.” Derived systems “do not by default guarantee that reads are up-to-date.”"
  - id: most-promising
    prompt: "What does the chapter consider the most promising approach for integrating different data systems today?"
    options:
      - id: o706475
        text: "XA distributed transactions"
      - id: od01b51
        text: "Manual reconciliation"
      - id: oc9935a
        text: "Log-based derived data"
        correct: true
      - id: o64fb4b
        text: "Shared databases"
    explanation: "“In the absence of widespread support for a good distributed transaction protocol, log-based derived data is the most promising approach for integrating different data systems.”"
  - id: total-order-limits
    prompt: "Which situations make a total order of events hard or undefined?"
    type: multiple
    options:
      - id: o4dcab1
        text: "Offline-capable clients updating local state immediately"
        correct: true
      - id: o98a1c9
        text: "A single-node database with one writer"
      - id: o3858b9
        text: "Separate leaders in multiple regions"
        correct: true
      - id: o2d9505
        text: "Throughput beyond one node requiring a sharded log"
        correct: true
      - id: o1b64fe
        text: "Independent microservices with no shared state"
        correct: true
    explanation: "The chapter lists all four limitations; total order broadcast “is equivalent to consensus”, and most algorithms assume one node can process the whole stream."
  - id: unfriend
    prompt: "A user unfriends someone, then posts a message. Why might the ex-friend still be notified?"
    options:
      - id: oca37c0
        text: "LWW picked the wrong event"
      - id: oe32a7d
        text: "The message was sent before unfriending"
      - id: oac6028
        text: "Friendship status and messages are stored separately, so the causal order can be lost and the notifier may see the message first"
        correct: true
      - id: o8b21d9
        text: "Notifications ignore privacy"
    explanation: "“in a system that stores friendship status in one place and messages in another, that ordering dependency between the unfriend event and the message-send event may be lost.”"
  - id: causality-tools
    prompt: "Which starting points does the chapter suggest for capturing causality without total order?"
    type: multiple
    options:
      - id: o523a7e
        text: "Logical timestamps"
        correct: true
      - id: odcc1eb
        text: "Logging the state the user saw and referencing its event ID"
        correct: true
      - id: o8a6cec
        text: "Wall-clock timestamps alone"
      - id: ofa52f8
        text: "Conflict resolution algorithms (for state, not side effects)"
        correct: true
    explanation: "The chapter suggests logical timestamps, recording “the state of the system that the user saw before making a decision” with a unique ID, and conflict resolution algorithms, which “do not help if actions have external side effects”."
  - id: async-robust
    prompt: "Why does the chapter favour asynchronous maintenance of derived data?"
    options:
      - id: o3eba0c
        text: "It is always faster to read"
      - id: o79e7c2
        text: "It avoids logs"
      - id: ob0543a
        text: "Asynchrony contains faults locally, whereas distributed transactions abort if any participant fails and amplify failures"
        correct: true
      - id: o007592
        text: "It guarantees read-your-writes"
    explanation: "“asynchrony is what makes systems based on event logs robust. It allows a fault in one part of the system to be contained locally, whereas distributed transactions abort if any one participant fails, so they tend to amplify failures”."
  - id: reprocess
    prompt: "What does reprocessing historical data enable that simple schema evolution doesn't?"
    options:
      - id: of44f22
        text: "Restructuring a dataset into a completely different model"
        correct: true
      - id: o0c9457
        text: "Reading old data"
      - id: o9c8f1d
        text: "Encrypting data"
      - id: o5ed840
        text: "Adding an optional field"
    explanation: "“Without reprocessing, schema evolution is limited to simple changes like adding a new optional field ... with reprocessing it is possible to restructure a dataset into a completely different model”."
  - id: railway
    prompt: "What does the railway gauge story illustrate?"
    options:
      - id: o70a30a
        text: "Migrations should be done overnight"
      - id: o2a7a05
        text: "Standards should never change"
      - id: o57e0a4
        text: "Running old and new versions side by side (a third rail) enables gradual, reversible migration"
        correct: true
      - id: o94fe32
        text: "Trains are like databases"
    explanation: "Tracks were converted “to dual gauge or mixed gauge by adding a third rail ... allowing the old and new versions to exist side by side, makes it possible to change the gauge gradually”."
  - id: gradual-migration
    prompt: "Why is migrating via two derived views safer than a sudden switch?"
    options:
      - id: o6341c9
        text: "It is cheaper"
      - id: o393e2d
        text: "It needs no extra storage"
      - id: ofc47ee
        text: "Each stage is reversible, so you always have a working system to go back to"
        correct: true
      - id: o6df700
        text: "It avoids testing"
    explanation: "“The beauty of such a gradual migration is that every stage of the process is easily reversible if something goes wrong; you always have a working system to go back to.”"
  - id: unify
    prompt: "Which features does unifying batch and stream processing require?"
    type: multiple
    options:
      - id: obcb112
        text: "Exactly-once semantics for stream processors"
        correct: true
      - id: o536e22
        text: "Windowing by event time, not processing time"
        correct: true
      - id: obc424d
        text: "Separate code bases for batch and stream"
      - id: oe67a50
        text: "Replaying historical events through the same engine"
        correct: true
    explanation: "The chapter lists these three features; the older lambda architecture “had a number of problems” and “has fallen out of use”."
---
No single tool fits all uses, so applications compose tools. The chapter argues for deriving data
through ordered event logs rather than distributed transactions, and for using batch and stream together.

## Learning objectives

- Explain why tool composition is inevitable
- Reason about which systems are sources and which are derived
- Compare log-based derived data with distributed transactions
- Explain the limits of total ordering and capturing causality
- Explain how reprocessing enables evolution and what unifying batch and stream requires

## In a nutshell

- Decide on a total order of writes (CDC or event log) and derive everything else from it.
- Derived data vs distributed transactions: idempotent retry vs atomic commit; derived data is usually asynchronous.
- Total order breaks down with sharding, multiple regions, microservices and offline clients.
- Asynchrony contains faults; distributed transactions amplify them.
- Reprocessing lets you run old and new views side by side, like dual-gauge railways.
- Kappa-style unification needs replay, exactly-once, and event-time windows.

## Connections to today

- **Zero-ETL integrations** in cloud platforms are vendor-managed derived-data pipelines.
- **Blue/green data migrations** with shadow reads are the chapter's gradual evolution.
- **Apache Beam** — named in the chapter — remains the canonical unified batch/stream API.
