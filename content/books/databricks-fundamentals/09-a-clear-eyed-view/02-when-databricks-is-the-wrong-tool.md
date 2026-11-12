---
title: When Databricks is the wrong tool
questions:
  - id: small-data
    prompt: A team has 20 GB of data and runs a few SQL reports daily. Which is likely the best fit?
    options:
      - text: A large Databricks deployment with streaming
      - text: A Hadoop cluster
      - id: simple
        text: A single database (Postgres) or embedded analytics engine (DuckDB), or a small managed warehouse
        correct: true
      - text: A GPU cluster
    explanation: Fixed overhead — admin, security, skills — dominates at small scale.
  - id: realtime-app
    prompt: A mobile app needs sub-10 ms per-user reads and writes. What is appropriate?
    options:
      - id: oltp
        text: An OLTP database or key-value store (possibly Lakebase), not queries against Delta tables
        correct: true
      - text: A SQL warehouse query per request
      - text: A Spark job per request
      - text: A notebook per request
    explanation: Analytical engines are not designed for this latency or write pattern.
  - id: no-skills
    prompt: An organisation has no data engineers and only needs standard SaaS reports. What risk does a lakehouse pose?
    options:
      - text: None
      - text: It cannot produce reports
      - id: ops
        text: The platform's flexibility requires engineering skills to operate well; without them, costs and quality problems grow — a SaaS BI tool or managed warehouse may serve better
        correct: true
      - text: Lakehouses do not support SaaS data
    explanation: Tools are not substitutes for capability. Natural-language features reduce, but do not eliminate, the need for data engineering.
  - id: ai-hype
    prompt: A team plans to "add an AI agent" to a process whose input data is incomplete and inconsistent. What should come first?
    options:
      - text: Choosing the largest model
      - id: data-first
        text: Fixing data quality and defining success metrics — agents amplify data problems rather than solving them
        correct: true
      - text: Buying more GPUs
      - text: Writing a press release
    explanation: The Fundamentals course itself argues data quality drives AI quality; take it seriously.
  - id: honest-strengths
    prompt: Where is Databricks genuinely strong?
    type: multiple
    options:
      - id: large-eng
        text: Large-scale data engineering with Spark and Delta
        correct: true
      - id: unified
        text: Unifying data engineering, analytics and ML on one governed copy of data
        correct: true
      - id: ml-lifecycle
        text: ML and GenAI lifecycle tooling close to the data (MLflow, serving, vector search)
        correct: true
      - text: Running desktop applications
    explanation: Knowing the strengths helps you choose it for the right reasons.
  - id: evaluate
    prompt: Which approach best evaluates whether Databricks fits your organisation?
    options:
      - text: Read analyst reports only
      - text: Choose what competitors chose
      - text: Watch keynote demos
      - id: poc
        text: A time-boxed proof of concept on representative workloads, measuring cost, performance, developer productivity and governance against an alternative
        correct: true
    explanation: Decide on evidence from your workloads. Include the boring parts — identity, networking, cost reporting.
---
## Learning objectives

- Identify workloads where Databricks is a poor fit
- Recognise its genuine strengths
- Evaluate platforms with evidence

## A decision sketch

| Situation | Consider |
|---|---|
| Large, varied data + ML + many teams | Databricks (or similar lakehouse) |
| Small data, SQL reporting | Postgres, DuckDB, managed warehouse |
| OLTP application | Postgres/Lakebase, key-value stores |
| Already deep in one cloud's suite | Compare with that cloud's native services |
| Mostly SaaS reporting, no engineers | SaaS BI with built-in connectors |

## Sources

- DuckDB — <https://duckdb.org/why_duckdb>
- Jordan Tigani, "Big Data is Dead" (MotherDuck, 2023) — <https://motherduck.com/blog/big-data-is-dead/>
- Databricks Well-Architected Lakehouse — <https://docs.databricks.com/aws/en/lakehouse-architecture/>
