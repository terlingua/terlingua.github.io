---
title: Delta Lake fundamentals
questions:
  - id: format
    prompt: How does Delta Lake store a table?
    options:
      - id: parquet-log
        text: As Parquet data files plus a transaction log (`_delta_log`) of JSON commits and periodic Parquet checkpoints
        correct: true
      - text: In a proprietary binary database file
      - text: As CSV files with a header row
      - text: In the Databricks control plane
    explanation: The log records which files make up each version of the table, enabling ACID transactions on object storage.
  - id: enforcement
    prompt: You append a DataFrame with an extra column to a Delta table without any options. What happens?
    options:
      - id: fails
        text: The write fails because of schema enforcement
        correct: true
      - text: The extra column is silently dropped
      - text: The table is recreated with the new schema
    explanation: Delta rejects writes that don't match the schema unless you opt into schema evolution, for example with `mergeSchema`.
  - id: acid
    prompt: Which capabilities does the Delta transaction log enable?
    type: multiple
    options:
      - text: Atomic commits — readers never see a half-written write
        correct: true
      - text: Concurrent writers with optimistic concurrency control
        correct: true
      - text: Reading earlier versions of the table (time travel)
        correct: true
      - text: Unlimited time travel even after VACUUM removes old files
    explanation: VACUUM deletes data files no longer referenced by recent versions, which limits how far back you can travel.
---
## Learning objectives

- Explain how the transaction log provides ACID guarantees
- Understand schema enforcement and evolution
- Create managed Delta tables

## Anatomy of a Delta table

```text
orders/
  _delta_log/
    00000000000000000000.json
    00000000000000000001.json
    ...
    00000000000000000010.checkpoint.parquet
  part-00000-....snappy.parquet
  part-00001-....snappy.parquet
```

Each commit is a JSON file listing files **added** and **removed**. A reader determines the current
snapshot by replaying the log from the latest checkpoint. Because a commit is a single atomic file
creation, a write either fully happens or not at all.

## Creating tables

```sql
CREATE TABLE main.sales.orders (
  order_id BIGINT NOT NULL,
  customer_id BIGINT,
  status STRING,
  total DECIMAL(12, 2),
  ordered_at TIMESTAMP
)
CLUSTER BY (customer_id);
```

In Unity Catalog, this is a **managed** table: Databricks manages its storage location and lifecycle.
Delta is the default format, so `USING DELTA` is optional.

## Schema enforcement and evolution

```python
new_batch.write.mode("append").saveAsTable("main.sales.orders")
# AnalysisException if new_batch has unexpected columns or incompatible types

(new_batch.write
    .mode("append")
    .option("mergeSchema", "true")
    .saveAsTable("main.sales.orders"))   # adds new columns
```

```sql
ALTER TABLE main.sales.orders ADD COLUMNS (channel STRING);
ALTER TABLE main.sales.orders ADD CONSTRAINT positive_total CHECK (total >= 0);
```

`NOT NULL` and `CHECK` constraints are enforced on every write.

## Concurrency

Delta uses **optimistic concurrency**: writers prepare changes independently and check for conflicts
at commit time. Appends rarely conflict; concurrent updates to the same files may raise a
`ConcurrentModificationException`, which jobs can retry. Row-level concurrency with deletion vectors
reduces such conflicts on Databricks.

## Delta and Iceberg

Delta UniForm lets Iceberg (and Hudi) clients read Delta tables by generating their metadata
alongside Delta's, and Unity Catalog can also manage Apache Iceberg tables directly.
