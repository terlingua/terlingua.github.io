---
title: Change data feed
questions:
  - id: enable
    prompt: How do you enable change data feed on an existing Delta table?
    options:
      - text: It is always on and cannot be disabled
      - text: "`ENABLE CDC ON t`"
      - id: property
        text: "`ALTER TABLE t SET TBLPROPERTIES (delta.enableChangeDataFeed = true)`"
        correct: true
      - text: Restart the cluster with a special flag
    explanation: Changes are recorded from the version at which the property is enabled onward.
  - id: change-type
    prompt: Which value of `_change_type` represents a row's values after an update?
    options:
      - text: "`update`"
      - id: postimage
        text: "`update_postimage`"
        correct: true
      - text: "`update_preimage`"
      - text: "`after`"
    explanation: An update produces a `update_preimage` (old values) and a `update_postimage` (new values) row.
  - id: uses
    prompt: What is change data feed useful for?
    type: multiple
    options:
      - text: Incrementally propagating changes from silver to gold tables
        correct: true
      - text: Sending row-level changes to downstream systems
        correct: true
      - text: Auditing what changed between versions
        correct: true
      - text: Replacing the need for a transaction log
    explanation: CDF is built on top of the transaction log and exposes row-level changes.
---
## Learning objectives

- Enable and read Delta change data feed (CDF)
- Use CDF for incremental downstream processing

## Enabling CDF

```sql
ALTER TABLE main.silver.customers SET TBLPROPERTIES (delta.enableChangeDataFeed = true);
```

## Reading changes in batch

```sql
SELECT customer_id, email, _change_type, _commit_version, _commit_timestamp
FROM table_changes('main.silver.customers', 15, 20);
```

| `_change_type` | Meaning |
|----------------|---------|
| `insert` | New row |
| `update_preimage` | Row values before an update |
| `update_postimage` | Row values after an update |
| `delete` | Deleted row |

## Reading changes as a stream

```python
changes = (spark.readStream
    .option("readChangeFeed", "true")
    .table("main.silver.customers"))

(changes
    .where("_change_type != 'update_preimage'")
    .writeStream
    .option("checkpointLocation", "/Volumes/main/checkpoints/cdf/customers_gold")
    .foreachBatch(upsert_into_gold)
    .start())
```

`foreachBatch` lets you run a `MERGE` for each micro-batch, so only changed rows are processed
instead of recomputing the whole gold table.

## CDF versus CDC from source systems

Change data feed exposes changes **to a Delta table**. Capturing changes **from an operational
database** (CDC) is a separate concern, handled by ingestion tools such as Lakeflow Connect, and
then applied with `AUTO CDC`/`APPLY CHANGES` in Lakeflow Declarative Pipelines (covered later).
