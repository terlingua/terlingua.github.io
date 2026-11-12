---
title: Pipelines and the medallion pattern
questions:
  - id: sdp
    prompt: What are Spark Declarative Pipelines (formerly Delta Live Tables, DLT)?
    options:
      - text: A notebook formatting tool
      - id: declarative
        text: A framework where you declare tables and their queries; the system works out dependencies, orchestration, retries, incremental processing and data quality checks
        correct: true
      - text: A visual dashboard builder
      - text: A replacement for SQL
    explanation: Databricks contributed the declarative pipeline API to Apache Spark in 2025, so the core concept is becoming open. Databricks-specific features remain.
  - id: medallion
    prompt: Which describes the medallion architecture?
    options:
      - id: bsg
        text: Bronze (raw, as ingested), silver (cleaned, conformed, deduplicated), gold (business-level aggregates and models ready for consumption)
        correct: true
      - text: Three separate workspaces for three teams
      - text: Gold, silver and bronze pricing tiers
      - text: A security clearance model
    explanation: It is a convention, not a law. Some data needs two layers, some four. Do not create copies that add no value.
  - id: medallion-critique
    prompt: Which is a fair critique of the medallion pattern?
    options:
      - text: It is illegal under GDPR
      - text: Delta cannot support three layers
      - id: copies
        text: Applied mechanically, it multiplies copies, storage and pipeline code without clarifying what "silver" means for each dataset; it says nothing about data modelling
        correct: true
      - text: It only works with streaming
    explanation: You still need a modelling approach for gold (dimensional/Kimball, data vault, wide tables). Medallion describes quality stages, not models.
  - id: streaming-table
    prompt: In declarative pipelines, what is a streaming table?
    options:
      - text: A table that plays video
      - id: st
        text: A Delta table fed incrementally from append-only sources, processing each input record once
        correct: true
      - text: A temporary view
      - text: A table that cannot be queried
    explanation: Streaming tables suit ingestion and append-only transformations.
  - id: mv
    prompt: What is a materialized view in declarative pipelines and Databricks SQL?
    options:
      - text: A view with no storage
      - text: A copy that never updates
      - text: A dashboard
      - id: mv
        text: A precomputed query result that is refreshed — incrementally when possible — to reflect changes in its sources
        correct: true
    explanation: Incremental refresh is not always possible; some queries require full recomputation. Check the refresh type in the pipeline event log.
  - id: expectations
    prompt: What can a pipeline expectation do when a row fails a constraint?
    type: multiple
    options:
      - id: warn
        text: Keep the row and record the violation (warn)
        correct: true
      - id: drop
        text: Drop the row
        correct: true
      - id: fail
        text: Fail the update
        correct: true
      - text: Correct the value automatically using AI
    explanation: Quarantining bad rows to a separate table is a common pattern built from expectations.
  - id: auto-cdc
    prompt: What does `AUTO CDC` (formerly `APPLY CHANGES INTO`) handle?
    options:
      - id: scd
        text: Applying CDC feeds to a target table with correct ordering by a sequence column, deletes, and SCD type 1 or type 2 history
        correct: true
      - text: Automatic cluster sizing
      - text: Translating code between languages
      - text: Encrypting data
    explanation: Writing out-of-order CDC logic by hand with `MERGE` is error-prone; this is one of the strongest reasons to use declarative pipelines.
  - id: scd2
    prompt: What does a slowly changing dimension type 2 table keep?
    options:
      - text: Only the latest value
      - id: history
        text: A row per version of an entity, with validity start/end columns, preserving history
        correct: true
      - text: Only deleted rows
      - text: A daily snapshot of the whole table
    explanation: SCD1 overwrites; SCD2 versions. Choose based on whether reports must reflect historical attributes.
  - id: idempotent
    prompt: Why should pipeline steps be idempotent?
    options:
      - text: To make them faster
      - text: Because Spark requires it
      - id: retries
        text: Because retries and reruns happen; running a step twice must not duplicate or corrupt data
        correct: true
      - text: To avoid needing tests
    explanation: Use `MERGE` on keys, overwrite partitions or replace-where, and checkpoints. Blind `INSERT` on rerun duplicates data.
  - id: sdp-lockin
    prompt: What is a portability consideration for declarative pipelines?
    options:
      - text: There is none
      - text: They cannot be exported at all
      - id: features
        text: The open Spark version covers the core API, but features such as `AUTO CDC`, expectations UI, serverless pipelines and event log integration may be Databricks-specific, so code may need changes elsewhere
        correct: true
      - text: They only run on Azure
    explanation: Plain PySpark or SQL plus jobs is more portable; declarative pipelines are more productive. That is the usual trade.
---
## Learning objectives

- Explain declarative pipelines, streaming tables and materialized views
- Apply the medallion pattern critically
- Use expectations and AUTO CDC

## A declarative pipeline in SQL

```sql
CREATE OR REFRESH STREAMING TABLE bronze_orders
AS SELECT * FROM STREAM read_files('/Volumes/main/raw/landing/orders/', format => 'json');

CREATE OR REFRESH STREAMING TABLE silver_orders (
  CONSTRAINT valid_amount EXPECT (amount > 0) ON VIOLATION DROP ROW,
  CONSTRAINT has_customer EXPECT (customer_id IS NOT NULL) ON VIOLATION FAIL UPDATE
)
AS SELECT order_id, customer_id, CAST(amount AS DECIMAL(12,2)) AS amount, order_ts
FROM STREAM bronze_orders;

CREATE OR REFRESH MATERIALIZED VIEW gold_daily_revenue
AS SELECT DATE(order_ts) AS day, SUM(amount) AS revenue
FROM silver_orders GROUP BY ALL;
```

## Medallion, honestly

| Layer | Purpose | Typical consumer |
|---|---|---|
| Bronze | Raw, append-only, replayable | Engineers |
| Silver | Clean, typed, deduplicated, conformed | Analysts, data scientists |
| Gold | Business aggregates, dimensional models, features | BI, apps, agents |

## Sources

- Databricks docs, "Lakeflow Spark Declarative Pipelines" — <https://docs.databricks.com/aws/en/dlt/>
- Databricks blog, "Donating Declarative Pipelines to Apache Spark" (2025) — <https://www.databricks.com/blog/bringing-declarative-pipelines-apache-spark-open-source-project>
- Databricks glossary, "Medallion architecture" — <https://www.databricks.com/glossary/medallion-architecture>
- Kimball and Ross, *The Data Warehouse Toolkit*, 3rd ed. (Wiley, 2013)
