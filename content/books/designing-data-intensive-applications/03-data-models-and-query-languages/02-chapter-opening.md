---
title: "Chapter Opening — Layers of Data Models and Declarative Queries"
questions:
  - id: layers
    prompt: "In the chapter's layering, what does a database engineer decide?"
    options:
      - id: o0e46de
        text: "How to represent document, relational or graph data as bytes in memory, on disk or on the network"
        correct: true
      - id: o3af980
        text: "Which API the mobile app calls"
      - id: o354a32
        text: "How to represent bytes as electrical currents"
      - id: o709019
        text: "How to model the real world in application objects"
    explanation: "“The engineers who built your database software decided on a way of representing that document, relational, or graph data in terms of bytes in memory, on disk, or on a network.”"
  - id: layers-purpose
    prompt: "What do the layers of data models enable organizationally?"
    options:
      - id: o531c6c
        text: "They guarantee performance"
      - id: o8abe6d
        text: "Different groups, such as database vendors and application developers, can work together effectively"
        correct: true
      - id: ob23baa
        text: "They eliminate the need for documentation"
      - id: o18f0aa
        text: "They let one team own the whole stack"
    explanation: "“each layer hides the complexity of the layers below it by providing a clean data model. These abstractions allow different groups of people ... to work together effectively.”"
  - id: declarative-benefit
    prompt: "What is the *more important* benefit of declarative query languages according to the chapter?"
    options:
      - id: oc4e03a
        text: "They hide engine implementation details, so the database can improve performance without query changes"
        correct: true
      - id: o9c3027
        text: "They are always shorter than imperative code"
      - id: o112523
        text: "They run only on one CPU core"
      - id: o42cf57
        text: "They avoid the need for indexes"
    explanation: "“A declarative query language is attractive because it is typically more concise ... More importantly, it hides implementation details of the query engine, which makes it possible for the database system to introduce performance improvements without requiring any changes to queries”."
  - id: parallel
    prompt: "Why is parallel execution easier with declarative queries?"
    options:
      - id: o0b8bdc
        text: "Imperative code cannot use threads"
      - id: oe5a317
        text: "Declarative queries forbid joins"
      - id: oa014e5
        text: "The database can run the query across cores and machines without you implementing the parallelism"
        correct: true
      - id: o4ea9d0
        text: "SQL is always executed on GPUs"
    explanation: "“a database might be able to execute a declarative query in parallel across multiple CPU cores and machines, without you having to worry about how to implement that parallelism. In a hand-coded algorithm, it would be a lot of work”."
  - id: optimizer
    prompt: "In a declarative query, who decides which indexes and join algorithms to use?"
    options:
      - id: o7a6702
        text: "The database's query optimizer"
        correct: true
      - id: o939811
        text: "The network layer"
      - id: o12d46e
        text: "The storage hardware"
      - id: o74377c
        text: "The application developer, via hints in every query"
    explanation: "“The database system’s query optimizer can decide which indexes and join algorithms to use and in which order to execute various parts of the query.”"
---
Data models shape how we think about a problem. Each layer hides the one below it, and declarative query
languages let the engine choose the execution strategy.

## Learning objectives

- Describe the layering of data models from application objects to physical signals
- Explain why declarative query languages help databases evolve

## In a nutshell

- App objects → general-purpose model (documents, tables, graphs) → bytes → physical signals.
- Declarative languages say *what*, not *how*; optimizers can then add parallelism and new indexes
  without query changes.

## Connections to today

- **Text-to-SQL.** LLMs generating SQL work well partly because SQL is declarative: the model states the
  desired result and the optimizer handles execution.
- **ORMs and query builders** such as EF Core LINQ sit one layer above SQL, adding another translation.
