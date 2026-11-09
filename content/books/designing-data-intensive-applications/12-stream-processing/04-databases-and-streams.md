---
title: "Databases and Streams"
questions:
  - id: replication-log-stream
    prompt: "How is a replication log related to streams?"
    options:
      - id: o712a6a
        text: "It is a cache"
      - id: o02e0ba
        text: "It is unrelated"
      - id: o689514
        text: "It is a batch file"
      - id: o2e185d
        text: "It is a stream of write events that followers apply to stay in sync"
        correct: true
    explanation: "“a replication log ... is a stream of database write events, produced by the leader as it processes transactions. The followers apply that stream of writes to their own copy of the database”."
  - id: dual-write-race
    prompt: "Two clients set X to A and B; each writes to the database then the search index, interleaved. What can happen?"
    options:
      - id: o38b79e
        text: "The second write fails"
      - id: o8c2152
        text: "Both converge to B"
      - id: o1e1574
        text: "The database ends at B and the index at A, permanently inconsistent with no error"
        correct: true
      - id: ob4dd6a
        text: "Both converge to A"
    explanation: "“The two systems are now permanently inconsistent with each other, even though no error occurred.”"
  - id: dual-write-fail
    prompt: "What other problem do dual writes have besides races?"
    options:
      - id: o663b8b
        text: "They are too fast"
      - id: o923c83
        text: "One write may succeed while the other fails, which is an atomic commit problem"
        correct: true
      - id: oac9332
        text: "They need a schema registry"
      - id: o27a034
        text: "They cause dirty reads only"
    explanation: "“Another problem with dual writes is that one of the writes may fail while the other succeeds ... Ensuring that they either both succeed or both fail is a case of the atomic commit problem”."
  - id: cdc-def
    prompt: "What is change data capture?"
    options:
      - id: o01eeaa
        text: "A type of index"
      - id: o325368
        text: "Observing all changes written to a database and extracting them so they can be replicated to other systems"
        correct: true
      - id: oe7c5fe
        text: "Capturing screenshots of data"
      - id: o39f6b4
        text: "Copying a database nightly"
    explanation: "CDC “is the process of observing all data changes written to a database and extracting them in a form in which they can be replicated to other systems.”"
  - id: cdc-fix
    prompt: "How does CDC fix the dual-write race?"
    options:
      - id: obcf653
        text: "It locks the search index"
      - id: o5874df
        text: "It uses LWW"
      - id: o2191ca
        text: "It writes to the index first"
      - id: o61369d
        text: "The database decides the order and downstream systems apply changes in that same order"
        correct: true
    explanation: "“the database decides on the order in which to execute them and writes them to its replication log in that order. The search index picks them up and applies them in the same order.”"
  - id: cdc-leader
    prompt: "In replication terms, what does CDC do to your systems?"
    options:
      - id: of5113d
        text: "Makes one database the leader and turns the others into followers"
        correct: true
      - id: oe39747
        text: "Makes all systems leaders"
      - id: of9f362
        text: "Turns the database into a follower of the index"
      - id: of1ed04
        text: "Removes the leader"
    explanation: "“Essentially, CDC makes one database the leader (the one from which the changes are captured) and turns the others into followers.”"
  - id: cdc-async
    prompt: "What is the trade-off of CDC usually being asynchronous?"
    options:
      - id: o937001
        text: "Writes become synchronous"
      - id: o4f43b3
        text: "Slow consumers don't hurt the source much, but replication-lag issues apply"
        correct: true
      - id: o86edbc
        text: "It provides linearizability"
      - id: o498c45
        text: "It loses ordering"
    explanation: "“adding a slow consumer does not affect the system of record too much, but it has the downside that all the issues of replication lag apply”."
  - id: snapshot-offset
    prompt: "Why must a CDC initial snapshot correspond to a known log position?"
    options:
      - id: o1d3569
        text: "To sort it"
      - id: o072d61
        text: "To compress it"
      - id: o90771c
        text: "So you know where to start applying changes after loading the snapshot"
        correct: true
      - id: ocb3c61
        text: "To encrypt the snapshot"
    explanation: "“The snapshot of the database must correspond to a known position or offset in the change log, so that you know at which point to start applying changes after the snapshot has been processed.”"
  - id: compaction
    prompt: "How does log compaction let you rebuild a derived system without a new snapshot?"
    options:
      - id: o522237
        text: "It deletes all old messages"
      - id: o5ae0fe
        text: "It keeps the latest value per key, so reading from offset 0 yields a full copy of current contents"
        correct: true
      - id: o417cfb
        text: "It sorts messages by time"
      - id: oe5a000
        text: "It merges topics"
    explanation: "“you can start a new consumer from offset 0 of the log-compacted topic ... The log is guaranteed to contain the most recent value for every key in the database ... you can use it to obtain a full copy of the database contents without having to take another snapshot”."
  - id: compaction-space
    prompt: "What does the space of a compacted log depend on?"
    options:
      - id: o4a4c1d
        text: "The retention time"
      - id: o3523f3
        text: "The total number of writes"
      - id: o144f31
        text: "The current contents of the database, not the number of writes ever made"
        correct: true
      - id: o83986e
        text: "The number of consumers"
    explanation: "“The disk space required for such a compacted log depends only on the current contents of the database, not the number of writes that have ever occurred”."
  - id: cassandra-cdc
    prompt: "Why is CDC hard for quorum-based databases like Cassandra?"
    options:
      - id: o0bf77a
        text: "Cassandra has no logs"
      - id: oddbc9a
        text: "There is no single source of truth to subscribe to, so consumers must merge per-node raw logs"
        correct: true
      - id: o7ab443
        text: "Quorums are linearizable"
      - id: o9c0bc6
        text: "Cassandra forbids reads"
    explanation: "“CDC support for quorum writes is challenging because there’s no single source of truth to subscribe to ... Cassandra sidesteps this issue by exposing raw log segments for each node”."
  - id: cdc-vs-es
    prompt: "How does CDC differ from event sourcing?"
    options:
      - id: ob7736a
        text: "CDC extracts low-level changes from a mutable database; event sourcing records application-level immutable events by design"
        correct: true
      - id: o981dcc
        text: "CDC requires an event store"
      - id: o07ae00
        text: "They are the same"
      - id: oafa51d
        text: "Event sourcing parses replication logs"
    explanation: "“In CDC, the application uses the database in a mutable way ... In event sourcing, the application logic is explicitly built on the basis of immutable events”. CDC “can be added to an existing database with minimal changes”."
  - id: cdc-schema
    prompt: "What new risk does CDC introduce in a microservices architecture?"
    options:
      - id: o88cd7c
        text: "It disables transactions"
      - id: oe8764c
        text: "The service's internal database schema becomes a public API, so a dropped column can break consumers"
        correct: true
      - id: oa2930f
        text: "It requires shared databases"
      - id: o28652c
        text: "It removes the need for APIs"
    explanation: "“CDC systems typically use the upstream database’s schema when replicating its data, which turns these schemas into public APIs ... Removing a column in the database table will break downstream consumers”."
  - id: outbox
    prompt: "Why isn't the outbox pattern subject to the dual-write problems?"
    options:
      - id: oad6db7
        text: "Both writes are in the same database and can be in a single transaction"
        correct: true
      - id: o1e803e
        text: "It writes only to Kafka"
      - id: o41ce3a
        text: "It avoids writing twice"
      - id: o11885e
        text: "It uses 2PC across systems"
    explanation: "Outboxes are dual writes but “avoid the challenges ... by keeping both writes in the same system (the database). This design allows both writes to appear in a single transaction.”"
  - id: es-compaction
    prompt: "Why can't event-sourced logs be compacted like CDC logs?"
    options:
      - id: o74f62e
        text: "Events lack keys"
      - id: oe0ef72
        text: "Event stores are too small"
      - id: od61df4
        text: "Events express intent, and later events don't override earlier ones, so the full history is needed"
        correct: true
      - id: o7249ec
        text: "Compaction is illegal"
    explanation: "“with event sourcing, events are modeled at a higher level ... later events typically do not override prior events, so you need the full history of events to reconstruct the final state.”"
  - id: duality
    prompt: "What relationship does the chapter draw between state and event streams?"
    options:
      - id: o940bc5
        text: "State is the derivative of events"
      - id: o514b29
        text: "They are unrelated"
      - id: o52b269
        text: "State is the integral of the event stream over time; the change stream is the derivative of state"
        correct: true
      - id: offbe2d
        text: "They contradict each other"
    explanation: "“the application state is what you get when you integrate an event stream over time, and a change stream is what you get when you differentiate the state by time”."
  - id: gray-reuter
    prompt: "What did Gray and Reuter say about the log and the database?"
    options:
      - id: oa4fe56
        text: "Logs are only for crash recovery"
      - id: o246b33
        text: "The database is the source of truth"
      - id: od6838f
        text: "The log contains all the information; the database is kept only for retrieval performance"
        correct: true
      - id: o535f5d
        text: "Logs should be deleted daily"
    explanation: "“There is no fundamental need to keep a database at all; the log contains all the information there is. The only reason for storing the database (i.e., the current end-of-the-log) is performance of retrieval operations.”"
  - id: accounting
    prompt: "How do accountants correct a mistaken ledger entry?"
    options:
      - id: o80d5c8
        text: "Delete the ledger"
      - id: o76a44b
        text: "Erase it"
      - id: o4480e0
        text: "Overwrite it"
      - id: o121c69
        text: "Add a compensating transaction; the original stays for audit"
        correct: true
    explanation: "“accountants don’t erase or change the incorrect transaction in the ledger. Instead, they add another transaction that compensates for the mistake”."
  - id: immutable-info
    prompt: "What extra information does an immutable event log keep compared with current state?"
    options:
      - id: ob8a7aa
        text: "Actions later undone, such as adding then removing a cart item, useful for analytics"
        correct: true
      - id: o82f629
        text: "Only deletions"
      - id: oc922fa
        text: "Only timestamps"
      - id: o7e7086
        text: "Nothing extra"
    explanation: "“a customer may add an item to their cart and then remove it again ... it may be useful to know for analytics purposes that the customer was considering a particular item ... This information is recorded in an event log, but it would be lost in a database”."
  - id: write-read-fallacy
    prompt: "What fallacy does deriving views from an event log expose?"
    options:
      - id: o6549cc
        text: "That caches are unnecessary"
      - id: o653129
        text: "That logs are slow"
      - id: o015dea
        text: "That SQL is required"
      - id: oe489a7
        text: "That data must be written in the same form as it will be queried"
        correct: true
    explanation: "“The traditional approach to database and schema design is based on the fallacy that data must be written in the same form as it will be queried.”"
  - id: cqrs-downside
    prompt: "What is the biggest downside of CQRS with asynchronous view consumers?"
    options:
      - id: o5b248e
        text: "Views can't be rebuilt"
      - id: o44bbac
        text: "The log loses events"
      - id: obffd40
        text: "Writes are slower"
      - id: oa64ba4
        text: "A user may write and then not see their write in the derived view yet"
        correct: true
    explanation: "“The biggest downside of CQRS is that the consumers of the event log are usually asynchronous, so a user could make a write to the log, then read from a derived view and find that their write has not yet been reflected”."
  - id: es-concurrency
    prompt: "How can event sourcing simplify concurrency control?"
    options:
      - id: o4594a4
        text: "A self-contained event is one atomic append, and a single-threaded consumer per shard needs no write concurrency control"
        correct: true
      - id: o3f495e
        text: "It uses LWW"
      - id: o3226ab
        text: "It requires 2PL"
      - id: oa839da
        text: "It eliminates all concurrency"
    explanation: "“The user action then requires only a single write in one place—namely, appending the event to the log—which is easy to make atomic” and “a straightforward single-threaded log consumer needs no concurrency control for writes.”"
  - id: immut-limits
    prompt: "Which are limits of keeping immutable history forever?"
    type: multiple
    options:
      - id: o234bfb
        text: "Crypto-shredding moves the problem to mutable key storage"
        correct: true
      - id: obc3c4e
        text: "Immutable data cannot be queried"
      - id: o353c49
        text: "High-churn datasets can grow prohibitively large"
        correct: true
      - id: o2a136e
        text: "Legal deletion requires rewriting history (excision, shunning)"
        correct: true
    explanation: "The chapter discusses churn, GDPR-style deletion requiring that “you actually want to rewrite history”, and crypto-shredding where “the actual data is still immutable, but your key storage is mutable.”"
  - id: deletion-reality
    prompt: "How does the chapter characterize deletion in practice?"
    options:
      - id: o92353e
        text: "Always complete and verifiable"
      - id: o934692
        text: "Only possible with 2PC"
      - id: o1638e8
        text: "Impossible, so don't try"
      - id: ob586a2
        text: "\"Making it harder to retrieve the data\" rather than truly impossible"
        correct: true
    explanation: "“Overall, deletion is more a matter of “making it harder to retrieve the data” than actually “making it impossible to retrieve the data.””"
---
Every database write is an event. Change data capture turns a database into a stream source; event
sourcing makes the stream the source of truth. State and logs are two sides of the same coin.

## Learning objectives

- Explain the race and failure problems of dual writes
- Describe CDC, initial snapshots, log compaction and the outbox pattern
- Contrast CDC with event sourcing
- Explain the state/stream duality and the benefits and limits of immutability

## In a nutshell

- Dual writes: interleaving makes systems permanently inconsistent; partial failure too.
- CDC makes the database the leader and derived systems followers, preserving order.
- Snapshots must align with a log position; log compaction keeps the latest value per key.
- CDC exposes schemas as public APIs; outbox tables decouple them.
- State = integral of events; changelog = derivative of state.
- Immutability aids auditing, recovery and multiple views; deletion needs excision or crypto-shredding.

## Connections to today

- **Debezium** — the chapter's main example — plus managed CDC (AWS DMS, Google Datastream, Azure
  Data Factory CDC) are standard ingestion paths into lakehouses.
- **Transactional outbox** libraries in .NET (MassTransit, Wolverine) implement the outbox pattern.
- **Postgres logical replication slots** must be monitored — a stuck consumer causes WAL growth.
