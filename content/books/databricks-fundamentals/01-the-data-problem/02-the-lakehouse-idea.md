---
title: The lakehouse idea
questions:
  - id: definition
    prompt: Which best defines a lakehouse?
    options:
      - text: A data warehouse hosted on a lake-side server
      - text: A data lake with a dashboard tool attached
      - text: Two separate systems, a lake and a warehouse, connected by ETL
      - id: def
        text: An architecture that adds warehouse capabilities — ACID transactions, schemas, indexing, governance and fast SQL — directly on open file formats in low-cost object storage
        correct: true
    explanation: The key idea is a transactional metadata layer over open files, so one copy of data serves BI, data science and ML.
  - id: metadata-layer
    prompt: What makes ACID transactions possible on object storage like S3 or ADLS, which has no native multi-file transactions?
    options:
      - id: log
        text: A transaction log (Delta's `_delta_log`, Iceberg's metadata and manifest files) that records which files make up each table version, with atomic commits
        correct: true
      - text: Object storage supports multi-file transactions natively
      - text: Locking every file with a mutex on the driver
      - text: Copying the whole table on every write
    explanation: Readers only see files listed in a committed log entry, so a half-finished write is invisible. Commits rely on atomic put-if-absent or a coordination service.
  - id: open-formats
    prompt: Which are open table formats?
    type: multiple
    options:
      - id: delta
        text: Delta Lake
        correct: true
      - id: iceberg
        text: Apache Iceberg
        correct: true
      - id: hudi
        text: Apache Hudi
        correct: true
      - text: Microsoft Excel
    explanation: All three add a transaction log over Parquet data files. Delta Lake is a Linux Foundation project created by Databricks; Iceberg originated at Netflix; Hudi at Uber.
  - id: parquet
    prompt: Delta Lake stores table data in which file format?
    options:
      - text: CSV
      - text: Avro
      - id: parquet
        text: Apache Parquet, a columnar format
        correct: true
      - text: A proprietary binary format
    explanation: Data files are Parquet; the `_delta_log` folder holds JSON commit files and Parquet checkpoints.
  - id: columnar-why
    prompt: Why are columnar formats like Parquet good for analytics?
    type: multiple
    options:
      - id: read-columns
        text: Queries read only the columns they need
        correct: true
      - id: compress
        text: Values of one type stored together compress well
        correct: true
      - id: stats
        text: Per-column min/max statistics let engines skip row groups and files
        correct: true
      - text: They make single-row updates cheap
    explanation: Columnar layouts are poor at single-row updates — one reason Delta uses deletion vectors and why OLTP remains a separate workload.
  - id: one-copy
    prompt: What is the central economic argument for a lakehouse?
    options:
      - text: Databricks storage is free
      - id: one-copy
        text: One copy of data in open formats serves BI, data engineering, data science and AI, removing duplicate storage, ETL between systems and duplicate governance
        correct: true
      - text: It eliminates the need for compute
      - text: SQL is no longer needed
    explanation: Whether the savings materialise depends on migration costs and how well the platform serves every workload — a point the Lakehouse paper itself raises as open research.
  - id: storage-compute
    prompt: What does "separation of storage and compute" mean in practice?
    options:
      - text: Data and code live in different Git repositories
      - text: You must buy storage and compute from different vendors
      - id: scale-independently
        text: Data lives in object storage; compute clusters start, stop and scale independently, and several engines can read the same data
        correct: true
      - text: Storage is on-premises and compute is in the cloud
    explanation: You pay for compute only while it runs, and storage costs object-storage prices. The trade-off is that remote storage is slower than local disk, which caching and file-skipping offset.
  - id: data-intelligence
    prompt: Databricks now calls its product a "Data Intelligence Platform". What does the extra word refer to?
    options:
      - id: ai-on-lakehouse
        text: AI applied to the platform itself — understanding the semantics of your data to power natural-language querying (Genie), assistants, automatic optimisation and governance — built on the lakehouse
        correct: true
      - text: A new storage format replacing Delta Lake
      - text: A certification for data scientists
      - text: A rebrand with no technical meaning at all
    explanation: The lakehouse remains the foundation. Some of the "intelligence" is real (predictive optimisation, Genie); some is positioning. Judge features individually.
  - id: lakehouse-not-new
    prompt: Which statement about the lakehouse concept is accurate?
    options:
      - text: Only Databricks can build a lakehouse
      - id: industry-wide
        text: It is now an industry-wide pattern — Snowflake, Google BigQuery, AWS, Microsoft Fabric (OneLake) and open-source engines all support open table formats over object storage
        correct: true
      - text: It was abandoned in 2023
      - text: It requires Hadoop
    explanation: Databricks popularised the term, but Iceberg support across vendors means "lakehouse" is now a pattern, not a product.
  - id: schema-enforcement
    prompt: A job tries to append rows with an extra column to a Delta table. By default, what happens?
    options:
      - text: The column is silently dropped
      - id: fails
        text: The write fails due to schema enforcement unless schema evolution is enabled (for example `mergeSchema`)
        correct: true
      - text: The table is recreated
      - text: The rows are written to a quarantine table automatically
    explanation: Schema enforcement on write is one of the warehouse-like guarantees that raw Parquet folders lack.
  - id: time-travel
    prompt: Which capability comes directly from keeping a versioned transaction log?
    options:
      - text: Automatic dashboards
      - text: Faster networking
      - id: tt
        text: Time travel — querying a table as of an earlier version or timestamp, and auditing its history
        correct: true
      - text: Unlimited retention for free
    explanation: "`SELECT * FROM t VERSION AS OF 12`. History is bounded by `VACUUM` and log retention settings: old files are deleted eventually, so time travel is not a backup strategy."
---
## Learning objectives

- Define the lakehouse and what makes it different from a lake or a warehouse
- Explain how a transaction log provides ACID guarantees on object storage
- Know the open table formats and why columnar storage matters

## The idea in one diagram

```text
             BI / SQL     Data engineering     Data science / ML     GenAI & agents
                 \               |                   |                   /
                  ───────── Unified governance (Unity Catalog) ─────────
                                         |
                  Transactional metadata layer (Delta Lake / Iceberg)
                                         |
                  Open files (Parquet) in cloud object storage you own
```

## What the metadata layer buys you

- **ACID transactions** — concurrent readers and writers see consistent snapshots
- **Schema enforcement and evolution** — bad writes fail, intended changes are explicit
- **Time travel** — query older versions; audit history
- **Upserts and deletes** — `MERGE`, `UPDATE`, `DELETE` on files that are immutable
- **Performance** — statistics for data skipping, compaction, clustering

## Where the data lives

In the classic deployment, data files sit in **your** cloud account's object storage. This is a real
difference from warehouses that store data in a vendor-managed format. It is also why "open" is
central to the pitch: you can read the same Parquet files with other engines.

## Sources

- Armbrust et al., "Lakehouse", CIDR 2021 — <https://www.cidrdb.org/cidr2021/papers/cidr2021_paper17.pdf>
- Armbrust et al., "Delta Lake: High-Performance ACID Table Storage over Cloud Object Stores", VLDB 2020 — <https://www.vldb.org/pvldb/vol13/p3411-armbrust.pdf>
- Delta Lake documentation — <https://docs.delta.io/>
- Apache Iceberg specification — <https://iceberg.apache.org/spec/>
