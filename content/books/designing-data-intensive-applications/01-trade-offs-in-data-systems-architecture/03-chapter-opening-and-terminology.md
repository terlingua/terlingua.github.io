---
title: "Chapter Opening — Data-Intensive Applications, Frontends and Backends"
questions:
  - id: data-intensive-def
    prompt: "Which system best fits the book's definition of *data-intensive*?"
    options:
      - id: oc3b1e8
        text: "A single-user desktop calculator"
      - id: o40d6ee
        text: "A ride-sharing backend whose hardest problems are keeping trip state consistent under concurrency and failure, at high volume"
        correct: true
      - id: o940a3b
        text: "A program that computes digits of π on a GPU cluster"
      - id: oca61b5
        text: "A climate simulation whose difficulty is parallelizing numerical computation"
    explanation: "“We call an application data-intensive if data management is one of the primary challenges in developing the application”. By contrast, “in compute-intensive systems the challenge is parallelizing a very large computation”."
  - id: building-blocks
    prompt: "Match the need to the building block: \"remember the result of an expensive operation, to speed up reads\"."
    options:
      - id: o481922
        text: "A cache"
        correct: true
      - id: oa95226
        text: "A stream processor"
      - id: o6a600d
        text: "A search index"
      - id: oacb102
        text: "Batch processing"
    explanation: "The chapter lists “Remember the result of an expensive operation, to speed up reads (caches)”."
  - id: building-blocks-2
    prompt: "An app must react to each new payment within seconds to flag fraud, and also compute monthly aggregates over all payments. Which two building blocks map most directly?"
    type: multiple
    options:
      - id: o05d656
        text: "A search index for both"
      - id: o046788
        text: "Stream processing for the per-event reaction"
        correct: true
      - id: o984f84
        text: "Batch processing for the periodic aggregate"
        correct: true
      - id: oe01a9d
        text: "A cache for both"
    explanation: "The list includes “Handle events and data changes as soon as they occur (stream processing)” and “Periodically crunch a large amount of accumulated data (batch processing)”."
  - id: glue-difficulty
    prompt: "According to the opening, when does gluing data systems together become hard?"
    options:
      - id: o8d57ff
        text: "It is never hard if you use managed services"
      - id: o8a3240
        text: "Only when using more than three programming languages"
      - id: ob770e7
        text: "Only when systems run in different clouds"
      - id: o6fc046
        text: "When the application needs something that no single tool does alone, so tools must be combined"
        correct: true
    explanation: "“If you are doing exactly what the data systems were designed for, this process can be quite easy. However, as your application becomes more ambitious, challenges arise.” and “it can be difficult to combine tools when you need to do something that a single tool cannot do alone.”"
  - id: backend-harder
    prompt: "Why do the greatest data infrastructure challenges usually lie in the backend rather than the frontend?"
    options:
      - id: o6b9837
        text: "Frontends never store data locally"
      - id: o1cc6f6
        text: "Backends always use more programming languages"
      - id: o914e41
        text: "Browsers forbid persistent storage"
      - id: o3659fe
        text: "A frontend handles one user's data, while the backend manages data for all users"
        correct: true
    explanation: "“a frontend needs to handle only one user’s data, whereas the backend manages data on behalf of all the users.”"
  - id: stateless-backend
    prompt: "A backend service keeps a user's shopping cart in an in-process dictionary. It is load-balanced across three instances. What problem follows from the book's description of stateless application code?"
    options:
      - id: o980552
        text: "The dictionary will be replicated automatically by HTTP"
      - id: ob56b41
        text: "Requests landing on a different instance will not see the cart; persistent state belongs in data infrastructure or on the client"
        correct: true
      - id: oe4dc15
        text: "None; HTTP guarantees sticky routing"
      - id: o044095
        text: "The cart will be stored in the TCP connection"
    explanation: "“The application code is often stateless (i.e., when it finishes handling one HTTP request, it forgets everything about that request), and any information that needs to persist from one request to another needs to be stored either on the client or in the server-side data infrastructure.”"
  - id: different-goals
    prompt: "Why does the chapter say disagreements about data architecture often arise inside a company?"
    options:
      - id: o920ea4
        text: "Because only enterprises use data warehouses"
      - id: o7e2efe
        text: "Different teams need very different things from the same data, and those goals may not be articulated"
        correct: true
      - id: o49ab04
        text: "Because data volumes are always too large for one team"
      - id: o48503d
        text: "Because SQL dialects differ between teams"
    explanation: "“One of the key challenges with data systems is that different people need to do very different things with data.” and “those goals might not be explicitly articulated, which can lead to misunderstandings and disagreement about the right approach.”"
  - id: sowell-epigraph
    prompt: "The chapter's epigraph says \"There are no solutions; there are only trade-offs.\" What practical stance does it set for the rest of the book?"
    options:
      - id: oedb60c
        text: "Prefer building everything in-house"
      - id: o68b6cf
        text: "Evaluate approaches by their pros and cons for your situation rather than looking for a universally best one"
        correct: true
      - id: oa9ee7c
        text: "Always choose the most popular tool"
      - id: oc0c2ae
        text: "Avoid making architectural decisions until requirements are fixed"
    explanation: "The chapter states “no one approach is fundamentally better than others; everything has pros and cons” and promises “you will learn to ask the right questions to evaluate and compare data systems”."
---
The chapter opens by defining *data-intensive* and naming the building blocks applications glue
together: databases, caches, search indexes, stream processing and batch processing.

## Learning objectives

- Distinguish data-intensive from compute-intensive applications
- Name the standard building blocks of data systems
- Explain why the hardest data problems usually sit in the backend

## In a nutshell

- An application is data-intensive when data management is a primary challenge: volume, change,
  consistency under failure and concurrency, availability.
- Most applications glue together standard components; the difficulty is choosing and combining them.
- Backends are often stateless; persistent state lives in data infrastructure or on the client.

## Connections to today

- **LLM applications.** A retrieval-augmented chatbot is a classic data-intensive app: a database for
  documents, a vector index for search, a cache for responses, and stream or batch jobs to keep embeddings
  fresh. The model call is one component among many.
- **Serverless backends.** Functions-as-a-service make the stateless backend explicit — anything not
  written to a datastore disappears when the invocation ends.
