---
title: "Batch Use Cases"
questions:
  - id: batch-fit
    prompt: "Where are batch jobs a good fit?"
    options:
      - id: o3f7bbe
        text: "Low-latency request serving"
      - id: o656c4e
        text: "Interactive UI updates"
      - id: o55e73b
        text: "Large data where freshness isn't important, e.g., reconciliation, forecasting, recommendation training"
        correct: true
      - id: o831a2d
        text: "Single-record lookups"
    explanation: "“you’ll find batch jobs wherever there’s a lot of data and data freshness isn’t important.” Examples include reconciliation, demand forecasting and recommendation model training."
  - id: etl-why
    prompt: "Why are batch jobs a great fit for ETL?"
    options:
      - id: o6db6c5
        text: "Many transformations are embarrassingly parallel, and workflow schedulers make pipelines easy to schedule, retry and debug"
        correct: true
      - id: ob944c7
        text: "ETL must be single-threaded"
      - id: oaf8c15
        text: "ETL requires low latency"
      - id: o025531
        text: "Batch jobs can't fail"
    explanation: "“much of which involves “embarrassingly parallel” workloads” and “Batch processing environments also come with robust workflow schedulers”."
  - id: data-mesh
    prompt: "What do data mesh and data contracts enable?"
    options:
      - id: oce135b
        text: "Replacing SQL"
      - id: ofdea2f
        text: "Encryption at rest"
      - id: od45b95
        text: "Teams across an organization safely publishing and managing their own data for others to consume"
        correct: true
      - id: o20eb16
        text: "A single central data team owning everything"
    explanation: "“Data mesh, data contract, and data fabric practices provide standards and tools to help teams safely publish their data for consumption by anybody in the organization.”"
  - id: lakehouse
    prompt: "What is a data lakehouse?"
    options:
      - id: o4e1024
        text: "A data lake without SQL"
      - id: o6de463
        text: "A warehouse on special hardware"
      - id: o1cf1a9
        text: "A streaming system"
      - id: o574837
        text: "Analysts query object-store data via a query engine, with table formats like Iceberg and catalogs like Unity"
        correct: true
    explanation: "“Analysts write SQL queries that execute atop a query engine, which reads from and writes to a distributed filesystem or object store ... This architecture is known as a data lakehouse.”"
  - id: analytics-styles
    prompt: "Which two analytics query styles does the chapter describe?"
    type: multiple
    options:
      - id: o6d99b3
        text: "Pre-aggregation into cubes or marts on a schedule"
        correct: true
      - id: o8ea69b
        text: "Point lookups by primary key"
      - id: oe6a692
        text: "Ad hoc queries where response time matters"
        correct: true
      - id: od493ec
        text: "Streaming joins"
    explanation: "“Pre-aggregation queries” roll data into “OLAP cubes or data marts”, and “Ad hoc queries” are run iteratively, so “Response times are important for this use case.”"
  - id: ml-uses
    prompt: "Which are common ML uses of batch processing?"
    type: multiple
    options:
      - id: o7f8da6
        text: "Feature engineering"
        correct: true
      - id: of6cca9
        text: "Batch inference"
        correct: true
      - id: o122d57
        text: "Model training"
        correct: true
      - id: o93acf5
        text: "Real-time fraud blocking per transaction"
    explanation: "“Common uses include the following: Feature engineering ... Model training ... Batch inference”."
  - id: bsp
    prompt: "What is the bulk synchronous parallel (Pregel) model used for?"
    options:
      - id: oaf2c40
        text: "Iterative graph processing: propagate information along edges in rounds until convergence"
        correct: true
      - id: o375f37
        text: "Sorting strings"
      - id: ocef956
        text: "Transaction processing"
      - id: o2ab390
        text: "Video encoding"
    explanation: "“Many graph algorithms are expressed by traversing one edge at a time ... The bulk synchronous parallel (BSP) model of computation has become popular for batch processing graphs ... It is also known as the Pregel model”."
  - id: llm-prep
    prompt: "Which LLM preprocessing steps suit batch frameworks?"
    type: multiple
    options:
      - id: oa2b7aa
        text: "Removing low-quality and duplicate documents"
        correct: true
      - id: oec83e7
        text: "Serving chat responses"
      - id: o888060
        text: "Extracting plain text from HTML"
        correct: true
      - id: odfad79
        text: "Tokenizing and converting to embeddings"
        correct: true
    explanation: "The chapter lists these three preprocessing steps and notes “OpenAI uses Ray as part of its ChatGPT training process”."
  - id: direct-write-bad
    prompt: "Why is writing records one by one from a batch job into a production database a bad idea?"
    type: multiple
    options:
      - id: o2c07f7
        text: "Databases cannot accept writes from batch jobs"
      - id: o9392a9
        text: "Partial or duplicated output becomes visible, breaking all-or-nothing semantics"
        correct: true
      - id: o1c3c80
        text: "Many parallel tasks can overwhelm the database and hurt production queries"
        correct: true
      - id: o8303c1
        text: "Per-record network requests are orders of magnitude slower than batch throughput"
        correct: true
    explanation: "The chapter lists all three: slowness, overwhelming the database, and that “writing to an external system from inside a job produces externally visible side effects that cannot be hidden”."
  - id: via-streams
    prompt: "What does pushing batch output through Kafka fix, and what doesn't it fix?"
    options:
      - id: od6e9cb
        text: "It removes the need for consumers"
      - id: oe45253
        text: "It provides all-or-nothing automatically"
      - id: o0a5764
        text: "It makes writes random"
      - id: obdab6e
        text: "It buffers and throttles writes, fans out to many consumers and acts as a security boundary, but all-or-nothing needs a completion notification"
        correct: true
    explanation: "Streams are “optimized for sequential writes”, “act as a buffer”, allow multiple consumers and serve as a “security boundary”. However, “One issue streaming doesn’t inherently solve is that of the all-or-nothing guarantee.”"
  - id: bulk-load
    prompt: "What are the pros and cons of building a new database in the batch job and bulk-loading it?"
    options:
      - id: o92dde8
        text: "Very fast with atomic switching between versions, but incremental updates are hard"
        correct: true
      - id: odc6b13
        text: "It cannot be used with RocksDB"
      - id: o8938f1
        text: "Slow but supports incremental updates easily"
      - id: o53cafd
        text: "It requires writing each row over the network"
    explanation: "Bulk-importing “is very fast and makes it easier for systems to atomically switch between dataset versions. On the other hand, it can be challenging to incrementally update datasets”."
---
ETL, analytics in the lakehouse, machine learning and LLM data preparation, and pushing derived data
back into production systems safely.

## Learning objectives

- Explain why batch suits ETL and how data mesh and contracts change ownership
- Describe lakehouse analytics: pre-aggregation and ad hoc queries
- List batch stages in ML and LLM pipelines, including BSP/Pregel for graphs
- Explain why writing directly to production databases from batch jobs is a bad idea, and better options

## In a nutshell

- Batch fits where data is large and freshness isn't critical.
- Lakehouse = query engine + object storage + table format + catalog.
- ML: feature engineering, training, batch inference; LLMs: extract text, dedupe, tokenize, embed.
- Serve derived data via streams (Kafka) or bulk-loaded files, preserving all-or-nothing semantics.

## Connections to today

- **Ray** — named in the chapter for ChatGPT training — is widely used for distributed data prep and
  inference.
- **Batch inference APIs** from LLM providers process large prompt sets cheaply when latency doesn't
  matter.
- **Data contracts** are increasingly enforced in CI for dbt and Kafka schemas.
