---
title: What the lakehouse does not solve
questions:
  - id: format-war
    prompt: Databricks bought Tabular, the company founded by Apache Iceberg's creators, in June 2024. What does this reveal?
    options:
      - text: Delta Lake was discontinued
      - id: format-war
        text: The "open format" market had split between Delta and Iceberg, and openness at the file level did not guarantee interoperability between vendors' engines
        correct: true
      - text: Iceberg became proprietary
      - text: Parquet was replaced
    explanation: Databricks responded with UniForm (Delta tables readable as Iceberg) and managed Iceberg tables. Open formats reduce lock-in but interoperability still needs work and verification.
  - id: catalog-lockin
    prompt: Even with data in open Parquet files, where can lock-in still occur?
    type: multiple
    options:
      - id: catalog
        text: In the catalog and governance layer — permissions, lineage, tags and metrics definitions
        correct: true
      - id: proprietary-features
        text: In proprietary engine features such as Photon, serverless, Genie, dashboards and pipeline-specific syntax
        correct: true
      - id: skills-jobs
        text: In jobs, notebooks and orchestration definitions written against platform APIs
        correct: true
      - text: In the Parquet files themselves
    explanation: Unity Catalog was open-sourced in 2024, but the Databricks-hosted version has features the OSS release does not. Moving data is easy; moving everything around it is not.
  - id: small-team
    prompt: A five-person company has 50 GB of data and two dashboards. Which statement is most accurate?
    options:
      - text: They need a lakehouse to be competitive
      - id: overkill
        text: A single Postgres instance, DuckDB, or a small managed warehouse is often simpler and cheaper; distributed platforms pay off at larger scale and complexity
        correct: true
      - text: Spark is always faster than a single machine
      - text: Data volume does not matter for architecture choices
    explanation: Distributed systems add overhead. Many datasets fit on one machine; the "your data fits in RAM" argument is worth remembering.
  - id: oltp-gap
    prompt: Why is a Delta table a poor backend for a web application's shopping cart?
    options:
      - text: Delta cannot store strings
      - id: latency
        text: Each commit writes files and a log entry to object storage, so single-row writes have high latency and concurrent writers to the same table can conflict
        correct: true
      - text: Delta tables are read-only
      - text: Delta does not support SQL
    explanation: Lakehouses target analytics. Databricks offers Lakebase (Postgres) for OLTP alongside the lakehouse — an admission that one engine does not fit every workload.
  - id: quality
    prompt: Which statement about data quality is accurate?
    options:
      - text: ACID transactions guarantee correct data
      - id: acid-not-correct
        text: ACID guarantees that writes are atomic and isolated, not that the data is correct; garbage written atomically is still garbage
        correct: true
      - text: Schema enforcement detects all wrong values
      - text: Lakehouses do not need tests
    explanation: Use expectations in pipelines, data contracts with producers, and monitoring. Correctness is a separate problem from consistency.
  - id: concurrency-conflicts
    prompt: Delta Lake uses optimistic concurrency control. What does that imply?
    options:
      - text: Writers lock the whole table before starting
      - id: optimistic
        text: Writers proceed without locks and check for conflicts at commit; conflicting concurrent writes (for example two MERGEs touching the same files) can fail and must be retried
        correct: true
      - text: Conflicts are impossible
      - text: Only one reader is allowed at a time
    explanation: Row-level concurrency and liquid clustering reduce conflicts but do not eliminate them. High-contention write patterns need design attention.
  - id: object-storage-cost
    prompt: Which costs are easy to overlook in a lakehouse?
    type: multiple
    options:
      - id: requests
        text: Object storage request charges (LIST/GET/PUT), which grow with many small files
        correct: true
      - id: egress
        text: Cross-region or cross-cloud egress when engines read data from elsewhere
        correct: true
      - id: retained
        text: Old file versions retained for time travel until `VACUUM` removes them
        correct: true
      - text: Storing the Parquet schema
    explanation: Storage per GB is cheap, but requests, egress and retained versions add up. Maintenance (compaction, vacuum) is part of the operating cost.
  - id: skills
    prompt: What is a realistic prerequisite the Fundamentals course mostly assumes away?
    options:
      - id: sql-python-distributed
        text: Working knowledge of SQL, some Python, and an understanding of distributed data concepts (partitions, shuffles, file layout) to use the platform well
        correct: true
      - text: A PhD in machine learning
      - text: Knowledge of Scala
      - text: Nothing — the platform requires no skills at all
    explanation: Natural-language tools such as Genie lower the bar for consumers, but builders still need SQL, Python and an understanding of how distributed engines work.
  - id: marketing-claim
    prompt: A slide claims a lakehouse gives "warehouse performance at lake cost". How should you treat this?
    options:
      - text: As a guaranteed fact for every workload
      - id: benchmark
        text: As a claim to test on your own workload — vendor benchmarks (such as TPC-DS results) are run under favourable conditions, and every vendor publishes ones it wins
        correct: true
      - text: As meaningless
      - text: As a legal guarantee
    explanation: Databricks and Snowflake publicly disputed each other's TPC-DS claims in 2021. Benchmark your own queries, data and concurrency.
  - id: governance-still-work
    prompt: True or false — unified governance means you no longer need to design a permission model.
    options:
      - text: "True"
      - id: not-true
        text: "False — a unified tool applies one model everywhere, but you must still define groups, ownership, catalogs and least privilege"
        correct: true
    explanation: One place to manage permissions is better than five. It is still your job to design what goes in it.
---
## Learning objectives

- Recognise the problems a lakehouse does not solve on its own
- Identify where lock-in remains despite open formats
- Know when a simpler architecture is the better choice

## Open is a spectrum

| Layer | How open? |
|---|---|
| Data files (Parquet) | Fully open |
| Table format (Delta, Iceberg) | Open specifications; vendor feature support varies |
| Catalog (Unity Catalog) | OSS version exists; hosted version has more features |
| Engine optimisations (Photon, serverless) | Proprietary |
| Notebooks, dashboards, Genie spaces, jobs | Platform-specific |

## When not to reach for a lakehouse

- Small data that fits on one machine — DuckDB, Postgres or SQLite may be enough
- Transactional application workloads — use an OLTP database
- Teams without data-engineering capacity — a fully managed SaaS warehouse may be easier
- Very low-latency serving — use a cache or key-value store (or Lakebase / online tables)

## Questions to ask any vendor

1. Can another engine read and write my tables without your service running?
2. What happens to permissions, lineage and metric definitions if I leave?
3. What does my workload cost — including cloud infrastructure, not only your licence units?
4. Which features I depend on are in preview?

## Sources

- Databricks blog, "Databricks Agrees to Acquire Tabular" (June 2024) — <https://www.databricks.com/blog/databricks-tabular>
- Unity Catalog open source — <https://www.unitycatalog.io/>
- Delta Lake concurrency control — <https://docs.delta.io/latest/concurrency-control.html>
- Databricks Lakebase — <https://www.databricks.com/product/lakebase>
