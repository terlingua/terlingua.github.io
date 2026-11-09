---
title: "Unbundling Databases"
questions:
  - id: unix-vs-rel
    prompt: "How did Unix and relational databases differ in philosophy?"
    options:
      - id: o9f292d
        text: "Both chose high-level abstractions"
      - id: o5dfabd
        text: "Both chose low-level abstractions"
      - id: ob603af
        text: "Unix gave a low-level, thin hardware abstraction; relational databases gave a high-level abstraction hiding storage, concurrency and recovery"
        correct: true
      - id: o738789
        text: "Unix hid concurrency; databases exposed it"
    explanation: "“Unix viewed its purpose as presenting programmers with a logical but fairly low-level hardware abstraction, whereas relational databases wanted to give application programmers a high-level abstraction”."
  - id: nosql-unix
    prompt: "How does the chapter interpret the NoSQL movement?"
    options:
      - id: o9d6ee9
        text: "As a cloud pricing model"
      - id: obe9b28
        text: "As a new query language"
      - id: od91449
        text: "As applying a Unix-like approach of low-level abstractions to distributed OLTP storage"
        correct: true
      - id: o9cf46a
        text: "As a relational revival"
    explanation: "“the NoSQL movement could be interpreted as wanting to apply a Unix-esque approach of low-level abstractions to the domain of distributed OLTP data storage.”"
  - id: create-index
    prompt: "What is running CREATE INDEX similar to?"
    options:
      - id: ob11168
        text: "Running a backup"
      - id: o5a0011
        text: "Starting a transaction"
      - id: oa891cf
        text: "Dropping a table"
      - id: oc5d7b3
        text: "Setting up a new follower and bootstrapping CDC: snapshot, backlog, then keep up"
        correct: true
    explanation: "“This process is remarkably similar to setting up a new follower replica ..., and also very similar to bootstrapping CDC in a streaming system”."
  - id: meta-database
    prompt: "In the \"meta-database of everything\" view, what are batch and stream processors like?"
    options:
      - id: o34eda0
        text: "Network switches"
      - id: o8ed611
        text: "Query parsers"
      - id: o6b61ee
        text: "Elaborate implementations of triggers, stored procedures and materialized view maintenance"
        correct: true
      - id: o40a90e
        text: "Storage engines"
    explanation: "“batch and stream processors are like elaborate implementations of triggers, stored procedures, and materialized view maintenance algorithms. The derived data systems they maintain are like different index types.”"
  - id: federated-vs-unbundled
    prompt: "What distinguishes federated databases from unbundled databases?"
    options:
      - id: offe21c
        text: "Unbundling requires 2PC"
      - id: o1b80d1
        text: "Federation unifies writes; unbundling unifies reads"
      - id: o7292dc
        text: "They are identical"
      - id: o7fe32e
        text: "Federation unifies reads across systems; unbundling unifies writes via CDC and logs"
        correct: true
    explanation: "Federated databases are about “unifying reads” while “Unbundled databases (unifying writes)” make it “easier to reliably plug together storage systems (e.g., through CDC and event logs)”."
  - id: unbundled-unix
    prompt: "Which tradition does unbundling follow?"
    options:
      - id: o1531d3
        text: "The Unix tradition of small tools composed through a uniform low-level interface"
        correct: true
      - id: o7ccc2c
        text: "The relational tradition of one integrated system"
      - id: ob4fa7b
        text: "The mainframe tradition"
      - id: odc7c31
        text: "The blockchain tradition"
    explanation: "“The unbundled approach follows the Unix tradition of small tools that do one thing well, that communicate through a uniform low-level API (pipes), and that can be composed using a higher-level language (the shell).”"
  - id: harder-problem
    prompt: "Which is the harder engineering problem: federated reads or synchronized writes?"
    options:
      - id: o043cdb
        text: "They are equally hard"
      - id: o7986ad
        text: "Federated read-only querying"
      - id: ofede01
        text: "Keeping writes to several storage systems in sync"
        correct: true
      - id: o09f6ab
        text: "Neither is hard"
    explanation: "“Federated read-only querying requires mapping one data model into another ... ultimately quite a manageable problem. Keeping the writes to several storage systems in sync is the harder engineering problem”."
  - id: loose-coupling
    prompt: "What two kinds of loose coupling does log-based integration provide?"
    type: multiple
    options:
      - id: o6514fc
        text: "Human level: teams develop and maintain components independently"
        correct: true
      - id: o3dd1b8
        text: "Legal level: no need for compliance"
      - id: oe9fa35
        text: "Hardware level: no need for disks"
      - id: o083b53
        text: "System level: a slow or failed consumer is buffered and contained"
        correct: true
    explanation: "“At a system level, asynchronous event streams make the system as a whole more robust ... At a human level, unbundling data systems allows software components and services to be developed, improved, and maintained independently”."
  - id: breadth
    prompt: "What is the goal of unbundling, and when should you not bother?"
    options:
      - id: o9cb2d7
        text: "Breadth across many workloads; if one product does everything you need, just use it"
        correct: true
      - id: o2e688a
        text: "Reducing the number of teams"
      - id: o3112eb
        text: "Beating each database on its own workload; always unbundle"
      - id: o2ca357
        text: "Avoiding databases entirely"
    explanation: "“The goal of unbundling is not to compete with individual databases on performance ... It’s about breadth, not depth. Thus, if a single technology does everything you need, you’re most likely best off simply using that product”."
  - id: spreadsheet
    prompt: "What feature of spreadsheets does the chapter want data systems to have?"
    options:
      - id: o81bc4a
        text: "Single-user editing"
      - id: o705bd3
        text: "Cell formatting"
      - id: ob37b32
        text: "Manual refresh buttons"
      - id: oab3adc
        text: "Automatic recalculation of dependent values when inputs change"
        correct: true
    explanation: "“whenever any input to the formula changes, the result of the formula is automatically recalculated. This is exactly what we want at a data system level.”"
  - id: derivation-functions
    prompt: "Which are examples of derivation functions?"
    type: multiple
    options:
      - id: o3a8bc9
        text: "Building a search index with language processing"
        correct: true
      - id: ob50e02
        text: "Training an ML model from data"
        correct: true
      - id: of5d4c1
        text: "Building a secondary index by sorting indexed values"
        correct: true
      - id: o687e45
        text: "Populating a UI-specific cache"
        correct: true
      - id: o8bda6b
        text: "Deleting the system of record"
    explanation: "“When one dataset is derived from another, it goes through some kind of transformation function.” The chapter's examples are secondary indexes, full-text indexes, ML models and caches."
  - id: db-as-runtime
    prompt: "Why are databases poorly suited as environments for arbitrary application code?"
    options:
      - id: o480657
        text: "They cannot execute code"
      - id: o6e78b5
        text: "Code is always slower in databases"
      - id: oc139eb
        text: "They fit poorly with dependency management, version control, rolling upgrades, monitoring and external calls"
        correct: true
      - id: o08e194
        text: "Databases have no memory"
    explanation: "Databases “do not fit well with the requirements of modern application development, such as dependency and package management, version control, rolling upgrades, evolvability, monitoring, metrics, calls to network services”."
  - id: church-state
    prompt: "What does \"separation of Church and state\" refer to?"
    options:
      - id: odf8593
        text: "Splitting read and write databases"
      - id: o64355d
        text: "Separating religion from software"
      - id: o1353ff
        text: "Keeping stateless application logic (lambda calculus has no mutable state) separate from state management"
        correct: true
      - id: o9767d0
        text: "Encrypting state"
    explanation: "“Church is a reference to the mathematician Alonzo Church, who created the lambda calculus ... The lambda calculus has no mutable state”."
  - id: passive-db
    prompt: "What limitation do databases share with mutable variables in most programming languages?"
    options:
      - id: o551b2f
        text: "They can't be written"
      - id: oeeb039
        text: "They are always stale"
      - id: oc599e0
        text: "They can't be read concurrently"
      - id: o87559e
        text: "You can't subscribe to changes; you can only poll"
        correct: true
    explanation: "“in most programming languages you cannot subscribe to changes in a mutable variable—you can only read it periodically ... Databases have inherited this passive approach to mutable data.”"
  - id: dataflow-props
    prompt: "Which properties does maintaining derived data require?"
    type: multiple
    options:
      - id: ob65c4e
        text: "Fault-tolerant delivery, since losing one message desynchronizes the derived data"
        correct: true
      - id: o8320d0
        text: "Synchronous distributed transactions"
      - id: od4438b
        text: "Random message order"
      - id: o159ec5
        text: "Stable ordering of state changes"
        correct: true
    explanation: "“the order of state changes is often important” and “Fault tolerance is essential—losing just a single message causes the derived dataset to go permanently out of sync”."
  - id: exchange-rate-dataflow
    prompt: "How does the dataflow approach handle currency conversion during a purchase?"
    options:
      - id: o5b0976
        text: "It hardcodes rates"
      - id: o896a20
        text: "It asks the user"
      - id: o5594fa
        text: "It calls the exchange-rate service per purchase"
      - id: ob7bc8b
        text: "It subscribes to exchange-rate updates ahead of time and queries a local copy, replacing a synchronous request"
        correct: true
    explanation: "“the code that processes purchases would subscribe to a stream of exchange rate updates ahead of time and record the current rate in a local database ... The fastest and most reliable network request is no network request at all!”"
  - id: write-read-path
    prompt: "What do caches, indexes and materialized views do in terms of write and read paths?"
    options:
      - id: odcf0ee
        text: "Remove the read path"
      - id: o0eed1e
        text: "Make both paths synchronous"
      - id: ob53aa7
        text: "Shift the boundary — more precomputation on the write path to save work on the read path"
        correct: true
      - id: oc80a5b
        text: "Remove the write path"
    explanation: "“the role of caches, indexes, and materialized views is simple: they shift the boundary between the read path and the write path.”"
  - id: eager-path
    prompt: "Which path is like eager evaluation?"
    options:
      - id: oda462d
        text: "The write path"
        correct: true
      - id: oc050db
        text: "Both"
      - id: of0a4fd
        text: "Neither"
      - id: o71034f
        text: "The read path"
    explanation: "“the write path is similar to eager evaluation and the read path is similar to lazy evaluation.”"
  - id: full-circle
    prompt: "Why does the chapter say \"after 500 pages, we have come full circle\"?"
    options:
      - id: ob4e55b
        text: "The timeline case study in Chapter 2 was itself about shifting work between write and read paths"
        correct: true
      - id: od3a278
        text: "Chapter 13 repeats Chapter 1"
      - id: ofd2045
        text: "The book ends with SQL"
      - id: o97595b
        text: "The book returns to MapReduce"
    explanation: "“Shifting the boundary between work done on the write path and the read path was in fact the topic of the social networking example in “Case Study: Social Network Home Timelines””."
  - id: client-cache
    prompt: "How does the chapter reinterpret on-device state?"
    options:
      - id: o434e60
        text: "As the system of record"
      - id: ofcc91f
        text: "As a cache of server state: pixels are a view of model objects, which replicate server state"
        correct: true
      - id: od51f2a
        text: "As a backup"
      - id: oa76a4a
        text: "As untrusted input only"
    explanation: "“we can think of the on-device state as a cache of state on the server. The pixels on the screen are a materialized view of model objects in the client app; the model objects are a local replica of state in a remote datacenter.”"
  - id: push-write-path
    prompt: "What does pushing state changes to clients via WebSockets or SSE do in the write/read path model?"
    options:
      - id: o6d22d8
        text: "Extends the write path all the way to the end user"
        correct: true
      - id: o3e9090
        text: "Makes clients systems of record"
      - id: o6a71e4
        text: "Requires polling"
      - id: of1d426
        text: "Removes the read path entirely, even for initial state"
    explanation: "“actively pushing state changes all the way to client devices means extending the write path all the way to the end user. When a client is first initialized, it will still need to use a read path to get its initial state”."
  - id: offline-devices
    prompt: "How do offline devices catch up on missed state changes?"
    options:
      - id: o964cb6
        text: "They use 2PC"
      - id: oa7c7e9
        text: "Like log consumers reconnecting from their last offset"
        correct: true
      - id: o832c4e
        text: "They can't"
      - id: o135a49
        text: "They reload everything always"
    explanation: "“a consumer of a log-based message broker can reconnect after failing or becoming disconnected and ensure that it doesn’t miss any messages that arrived while it was disconnected. The … same technique works for individual users, where each device is a small subscriber to a small stream of events.”"
  - id: why-not-all
    prompt: "Why don't we build all applications with end-to-end event streams?"
    options:
      - id: o629788
        text: "Request/response and stateless clients are deeply ingrained in databases, libraries, frameworks and protocols"
        correct: true
      - id: o1515c1
        text: "It is impossible with TCP"
      - id: o81b471
        text: "It requires blockchains"
      - id: o1f9ee7
        text: "Users dislike responsive UIs"
    explanation: "“The challenge is that the assumption of stateless clients and request/response interactions is deeply ingrained in our databases, libraries, frameworks, and protocols.”"
  - id: reads-events
    prompt: "If reads are sent as events through a stream processor, what operation is being performed?"
    options:
      - id: o5bfc9f
        text: "A batch sort"
      - id: od69797
        text: "A two-phase commit"
      - id: o691d4f
        text: "A hash partition only"
      - id: o73252b
        text: "A stream–table join between read queries and the database"
        correct: true
    explanation: "“When both writes and reads are represented as events and routed to the same stream operator, we are in fact performing a stream-table join between the stream of read queries and the database.”"
  - id: log-reads
    prompt: "What benefit and cost come from logging read events?"
    options:
      - id: oea9dcb
        text: "Fewer writes"
      - id: o82fe75
        text: "Faster reads at no cost"
      - id: o7605f0
        text: "Elimination of joins"
      - id: odd2f2f
        text: "Better tracking of causal dependencies (what the user saw), at extra storage and I/O cost"
        correct: true
    explanation: "“Writing read requests to durable storage thus enables better tracking of causal relationships, but it incurs additional storage and I/O costs.”"
---
Treat the organization's dataflow as one giant database whose indexes are separate systems kept in
sync by logs. Then design applications as derivation functions over streams, and extend the write path
all the way to the user's screen.

## Learning objectives

- Contrast the Unix and relational philosophies
- Relate CREATE INDEX to followers and CDC bootstrapping
- Distinguish federated databases (unifying reads) from unbundled databases (unifying writes)
- Explain why log-based integration loosely couples systems and teams
- Explain dataflow application design, the write/read path boundary, and reads as events

## In a nutshell

- Batch and stream processors are like triggers, stored procedures and view maintenance at organization scale.
- Unbundling is about breadth, not depth — use one product if it does everything you need.
- Application code is a derivation function; keep it separate from state, but subscribe to state changes.
- Dataflow replaces synchronous RPC with stream joins against local state.
- Caches, indexes and views shift the boundary between write and read paths; clients can be subscribers too.

## Connections to today

- **Trino and Postgres FDWs** are federated reads; **Debezium + Kafka + IVM engines** are unbundled writes.
- **Live queries** in Firebase, Convex, Supabase Realtime and sync engines extend the write path to UIs.
- **React/Blazor state management** already re-renders on state changes — the missing piece is server push.
