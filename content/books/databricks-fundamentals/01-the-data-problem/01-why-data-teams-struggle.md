---
title: Why data and AI teams struggle
questions:
  - id: silo-cost
    prompt: A company keeps BI data in a cloud warehouse, raw files in a data lake, and ML features in a separate feature store, each with its own access controls. What is the most direct consequence?
    options:
      - text: Queries become faster because each system is specialised
      - text: Storage costs fall because data is split across systems
      - id: dup-governance
        text: Data is copied between systems, permissions drift apart, and nobody can say with confidence which copy is the truth
        correct: true
      - text: Machine learning becomes impossible
    explanation: Copies multiply cost and staleness, and every system has its own permission model. The governance gap is usually worse than the storage bill.
  - id: two-tier
    prompt: In the classic "two-tier" architecture, what are the two tiers?
    options:
      - id: lake-then-warehouse
        text: A data lake of raw files, with a subset loaded (ETL'd) into a data warehouse for BI
        correct: true
      - text: An OLTP database and a cache
      - text: A front end and a back end
      - text: Hot storage on SSD and cold storage on tape
    explanation: The Lakehouse paper (Armbrust et al., CIDR 2021) argues this two-tier design causes staleness, reliability problems and extra cost because data must be copied from lake to warehouse.
  - id: lake-weakness
    prompt: Which were well-known weaknesses of first-generation data lakes built on Hadoop or raw object storage?
    type: multiple
    options:
      - id: no-acid
        text: No ACID transactions — a failed job could leave half-written files that readers would see
        correct: true
      - id: no-schema
        text: Weak schema enforcement, so bad data landed silently ("data swamp")
        correct: true
      - id: small-files
        text: Many small files and slow file listings that hurt query performance
        correct: true
      - text: They could only store structured data
    explanation: Lakes were cheap and flexible but lacked the reliability guarantees warehouses provided. Table formats such as Delta Lake, Apache Iceberg and Apache Hudi were created to fix exactly this.
  - id: warehouse-weakness
    prompt: Which is a genuine limitation of a traditional proprietary data warehouse for AI workloads?
    options:
      - text: It cannot run SQL
      - text: It cannot store numbers
      - id: closed-format
        text: Data sits in a proprietary format that ML frameworks such as PyTorch cannot read directly, so data is exported first, and unstructured data (images, audio, PDFs) fits poorly
        correct: true
      - text: It is always more expensive than any lake
    explanation: Warehouses excel at SQL BI. ML tools want files or DataFrames; exporting creates yet another copy. Note that modern warehouses (Snowflake, BigQuery) have narrowed this gap with Iceberg support and Python runtimes.
  - id: structured-unstructured
    prompt: Which of the following is unstructured data?
    options:
      - text: A table of orders with typed columns
      - text: A Parquet file with a fixed schema
      - id: pdf-contracts
        text: Scanned PDF contracts and call-centre audio recordings
        correct: true
      - text: A CSV of sensor readings
    explanation: Estimates commonly cited by analysts put unstructured data at 80–90% of enterprise data. GenAI made it valuable, which is one reason platforms now emphasise it.
  - id: semi-structured
    prompt: JSON event payloads with nested, optional fields are best described as…
    options:
      - text: Unstructured
      - id: semi
        text: Semi-structured
        correct: true
      - text: Relational
      - text: Binary
    explanation: Semi-structured data has a self-describing structure that varies per record. Databricks offers the `VARIANT` type for it, as do Snowflake and others.
  - id: people-problem
    prompt: The course frames data problems as technology problems. Which statement is the most honest addition?
    options:
      - text: Buying one platform removes all organisational problems
      - id: org-problems
        text: Many data failures are organisational — unclear ownership, no data contracts, poor quality at the source — and no platform fixes those by itself
        correct: true
      - text: Data problems only exist in large enterprises
      - text: Data quality is solved by using Spark
    explanation: A unified platform reduces friction, but ownership, definitions and incentives are people problems. Industry surveys repeatedly cite data quality and skills as top barriers to AI adoption.
  - id: ai-needs-data
    prompt: Why does the course link data platform choices to AI success?
    options:
      - text: Because AI models are stored in tables
      - id: ai-data-quality
        text: Because model quality, especially for enterprise GenAI and agents, depends on governed, high-quality, accessible company data rather than on the model alone
        correct: true
      - text: Because only Databricks can run AI
      - text: Because AI removes the need for data engineering
    explanation: Foundation models are commodities available to every competitor; a company's own data and its context are the differentiator. That argument is sound, though it is also the core of every data vendor's pitch.
  - id: etl-elt
    prompt: What is the difference between ETL and ELT?
    options:
      - text: There is no difference
      - text: ELT never transforms data
      - id: order
        text: ETL transforms before loading into the target; ELT loads raw data first and transforms inside the target platform
        correct: true
      - text: ETL is for streaming, ELT for batch
    explanation: Cheap storage and scalable compute made ELT the norm in lakehouses and cloud warehouses — land raw data (bronze) and transform it in place.
  - id: oltp-olap
    prompt: Which workload is a lakehouse primarily designed for?
    options:
      - text: High-concurrency single-row transactional updates, like a bank's ledger
      - id: olap
        text: Analytical workloads — large scans, aggregations, ML training — over large volumes of data
        correct: true
      - text: Sub-millisecond key-value lookups
      - text: Document editing
    explanation: OLTP needs row-level, low-latency transactions. Databricks added Lakebase (managed Postgres, from the 2025 Neon acquisition) precisely because Delta tables are not an OLTP database.
---
## Learning objectives

- Describe the silos that data and AI teams commonly face
- Explain the two-tier lake-plus-warehouse architecture and its costs
- Distinguish structured, semi-structured and unstructured data
- Separate technical problems from organisational ones

## The problem the course starts with

Most organisations accumulated their data stack one need at a time:

```text
Operational DBs ──► ETL ──► Data warehouse ──► BI dashboards
       │
       └────► Raw files ──► Data lake ──► Data science notebooks ──► ML models
                                  │
                                  └──► Streaming system ──► Real-time apps
```

Each box has its own copy of the data, its own security model, its own team and its own bill. The
results are familiar:

- **Staleness** — the warehouse is a nightly copy, so dashboards lag reality
- **Inconsistency** — "revenue" is computed three ways in three places
- **Governance gaps** — revoking an analyst's access in one system does not revoke it in the others
- **Cost** — storage and compute are paid for several times
- **AI friction** — ML teams export data from the warehouse into files, creating more copies

## Data warehouses and data lakes

| | Data warehouse | Data lake |
|---|---|---|
| Data | Structured | Any: structured, semi-structured, unstructured |
| Format | Usually proprietary | Open files (Parquet, JSON, CSV, images) |
| Reliability | ACID transactions, schemas | Historically none |
| Cost | Higher per TB | Cheap object storage |
| Best for | SQL BI and reporting | Data science, ML, archival |

## People problems are still problems

A platform can remove copies and unify permissions. It cannot decide who owns the `customer`
table, agree what "active user" means, or force upstream systems to send clean data. Keep that in
mind whenever a slide promises that a product "solves" data quality.

## Sources

- Armbrust, Ghodsi, Xin, Zaharia, "Lakehouse: A New Generation of Open Platforms that Unify Data Warehousing and Advanced Analytics", CIDR 2021 — <https://www.cidrdb.org/cidr2021/papers/cidr2021_paper17.pdf>
- Databricks Fundamentals course catalog page — <https://www.databricks.com/training/catalog/databricks-fundamentals-2206>
- Databricks docs, "VARIANT type" — <https://docs.databricks.com/aws/en/sql/language-manual/data-types/variant-type>
