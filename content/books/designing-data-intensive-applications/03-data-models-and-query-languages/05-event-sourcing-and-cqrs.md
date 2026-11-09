---
title: "Event Sourcing and CQRS"
questions:
  - id: es-def
    prompt: "What is event sourcing?"
    options:
      - id: oc06a84
        text: "Using events as the source of truth and expressing every state change as an event"
        correct: true
      - id: o9385c4
        text: "Sourcing data from external event APIs"
      - id: o3cd3a8
        text: "Logging HTTP requests for debugging"
      - id: o9a4ba8
        text: "Storing only the latest state in a table"
    explanation: "“The idea of using events as the source of truth and expressing every state change as an event is known as event sourcing.”"
  - id: cqrs-def
    prompt: "What is CQRS?"
    options:
      - id: o61f8d2
        text: "A replication protocol"
      - id: oe2b8af
        text: "Querying with SQL and writing with REST"
      - id: o9fb31f
        text: "Encrypting queries and responses"
      - id: o7901f4
        text: "Maintaining separate read-optimized representations derived from the write-optimized one"
        correct: true
    explanation: "“The principle of maintaining separate read-optimized representations and deriving them from the write-optimized representation is called command query responsibility segregation (CQRS).”"
  - id: command-vs-event
    prompt: "A user tries to book seats when none remain. What is added to the log?"
    options:
      - id: oe05555
        text: "A \"seats booked\" event that views later reject"
      - id: odd4fd2
        text: "The raw HTTP request"
      - id: o5033f2
        text: "A \"seats booked\" event marked as failed"
      - id: o242bc4
        text: "Nothing — the command is invalid, so it does not become an event"
        correct: true
    explanation: "“When a request from a user comes in, it is called a command, and it first needs to be validated. Once the command has been executed and has been determined to be valid ... it becomes a fact” and “a consumer of the event log that builds a materialized view is not allowed to reject an event.”"
  - id: past-tense
    prompt: "Why name events in the past tense?"
    options:
      - id: o28c6a1
        text: "An event records the fact that something happened, which remains true even if later changed"
        correct: true
      - id: o5fea2b
        text: "It makes events smaller"
      - id: o907741
        text: "It is a convention required by Kafka"
      - id: o2e6d4d
        text: "Future-tense names are reserved for commands only"
    explanation: "“it is recommended that you name your events in the past tense (e.g., “the seats were booked”), because an event is a record of the fact that something has happened.”"
  - id: fact-table-diff
    prompt: "How does an event log differ from a star-schema fact table?"
    type: multiple
    options:
      - id: o592cf7
        text: "Event order matters, while a fact table is unordered"
        correct: true
      - id: oe66a5c
        text: "Events cannot have timestamps"
      - id: o7c54b1
        text: "Fact tables are mutable while events are immutable"
      - id: o91793b
        text: "Events can have many types with different properties"
        correct: true
    explanation: "“rows in a fact table all have the same set of columns, whereas in event sourcing there may be many event types, each with different properties. In addition, a fact table is an unordered collection, while in event sourcing the order of events is important”."
  - id: rebuild-view
    prompt: "A bug in view-maintenance code corrupted a materialized view. What does event sourcing let you do?"
    options:
      - id: o91cdf0
        text: "Nothing; views are the source of truth"
      - id: o7b7be0
        text: "Restore only from a nightly backup"
      - id: o49d2be
        text: "Delete the view and recompute it from the event log with fixed code"
        correct: true
      - id: obe899b
        text: "Edit the events to match the view"
    explanation: "“You should always be able to delete the materialized views and recompute them by processing the same events in the same order, using the same code. If there was a bug in the view maintenance code, you can just delete the view and recompute it with the new code.”"
  - id: es-benefits
    prompt: "Which are benefits of event sourcing and CQRS listed in the chapter?"
    type: multiple
    options:
      - id: o2746e1
        text: "Events can be edited in place to fix errors"
      - id: o85f99d
        text: "Multiple views optimized for different queries"
        correct: true
      - id: oeff2f7
        text: "Events communicate intent better than row changes"
        correct: true
      - id: o5a5a4e
        text: "Mistakes can be reversed with a subsequent event"
        correct: true
      - id: o7d1f9b
        text: "The log can serve as an audit log"
        correct: true
    explanation: "The chapter lists clearer intent, reproducible views, multiple optimized views, easy evolution, “If an event was written in error, you can write a subsequent deletion event to reverse it”, and “The event log can also serve as an audit log”."
  - id: irreversible
    prompt: "How does event sourcing relate to the evolvability principle from Chapter 2?"
    options:
      - id: o35620d
        text: "It guarantees backward compatibility"
      - id: o60d995
        text: "It removes the need for schemas"
      - id: o64c95a
        text: "It reduces the number of irreversible actions in the system"
        correct: true
      - id: oc9aa62
        text: "It makes every action irreversible"
    explanation: "“Event sourcing can therefore reduce the number of irreversible actions in the system, making it easier to change”."
  - id: burst
    prompt: "Why can event logs absorb write bursts?"
    options:
      - id: o5b2315
        text: "Views are updated synchronously"
      - id: o0254be
        text: "Logs drop events during bursts"
      - id: oa06a12
        text: "Sequential appends give high write throughput, and view builders can catch up at their own pace"
        correct: true
      - id: ocbed4a
        text: "Logs are stored only in memory"
    explanation: "“Event logs can typically handle higher write throughput than databases because of their sequential access patterns. If you have a temporary burst of events, the log can absorb it, and downstream systems ... can catch up at their own pace”."
  - id: exchange-rate
    prompt: "A view converts event prices into euros by calling a live exchange-rate API when processing each event. What goes wrong when the view is rebuilt later?"
    options:
      - id: o24153d
        text: "Nothing; the API is idempotent"
      - id: occ170f
        text: "The rebuild fails because APIs cannot be called"
      - id: o77ae18
        text: "The result differs because rates have changed; store the rate in the event or use a deterministic historical lookup"
        correct: true
      - id: o076adf
        text: "The events are deleted"
    explanation: "“Since the exchange rate may fluctuate, it would be problematic to fetch the exchange rate from an external source when processing the event, since you would get a different result if you recomputed the materialized view on another date.”"
  - id: crypto-shredding
    prompt: "How can you honour a GDPR deletion request when immutable events contain personal data for many users?"
    options:
      - id: o0d1157
        text: "Delete the whole shared log"
      - id: o212680
        text: "Store personal data outside the event, or encrypt it with a key you later delete (crypto-shredding)"
        correct: true
      - id: ofafc26
        text: "Edit the events in place"
      - id: o400fee
        text: "Ignore the request because events are immutable"
    explanation: "“You can try storing the personal data outside of the actual event, or encrypting it with a key that you can later choose to delete (a technique known as crypto-shredding), but that also makes it harder to recompute derived state when needed.”"
  - id: side-effects
    prompt: "Why must replaying events be handled carefully when views send emails?"
    options:
      - id: o792972
        text: "Email servers reject old events"
      - id: od8bd2f
        text: "Emails cannot be sent from event handlers"
      - id: o7d27fa
        text: "Rebuilding a view could resend confirmation emails"
        correct: true
      - id: o04e252
        text: "Replays are always skipped"
    explanation: "“Reprocessing events requires care if there are externally visible side effects—for example, you probably don’t want to resend confirmation emails every time you rebuild a materialized view.”"
  - id: ordering-req
    prompt: "What is the one important requirement on the event storage system?"
    options:
      - id: o9d0bc6
        text: "Events must be stored in a relational database"
      - id: o76fe2b
        text: "All materialized views must process events in exactly the same order as in the log"
        correct: true
      - id: ofdbce5
        text: "Views must be stored with the events"
      - id: o67eb5f
        text: "Events must be smaller than 1 KB"
    explanation: "“The only important requirement is that the event storage system must guarantee that all materialized views process the events in exactly the same order as they appear in the log.”"
  - id: memory-view
    prompt: "Is it acceptable to keep a materialized view only in memory?"
    options:
      - id: o469be9
        text: "No, views must always be persisted"
      - id: o06aa94
        text: "Only for analytics"
      - id: o9bc052
        text: "Yes, if it is acceptable to recompute it from the log whenever the service restarts"
        correct: true
      - id: o9c1408
        text: "Only if the log is also in memory"
    explanation: "“You can even keep a view only in memory and avoid persisting it, as long as it’s OK to recompute the view from the event log whenever the service restarts.”"
---
Write data once in the form best for writing — an append-only log of immutable events — and derive
read-optimized views from it.

## Learning objectives

- Define event sourcing, CQRS, commands and events
- List the benefits and the pitfalls (external data, deletion, side effects)
- Explain the ordering requirement on the event log

## In a nutshell

- Commands are validated; valid ones become past-tense events appended to the log.
- Materialized views (projections, read models) are rebuilt reproducibly from the log.
- Benefits: intent, reproducibility, many views, easy evolution, reversible mistakes, auditability, burst absorption.
- Pitfalls: nondeterministic external lookups, GDPR deletion (crypto-shredding), replaying side effects.
- All views must process events in the same order.

## Connections to today

- **Kafka plus stream processors** (Flink, Kafka Streams) are common event-sourcing backbones.
- **Agent memory and audit trails.** LLM agent platforms often log every tool call as an immutable event so
  runs can be replayed and audited.
- **Marten** gives .NET teams event sourcing on PostgreSQL — one of the systems named in the chapter.
