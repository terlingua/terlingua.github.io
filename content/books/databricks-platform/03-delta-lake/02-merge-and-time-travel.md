---
title: MERGE, updates and time travel
questions:
  - id: merge
    prompt: Which statement implements an upsert into a Delta table?
    options:
      - id: merge-into
        text: "`MERGE INTO target USING source ON ... WHEN MATCHED THEN UPDATE ... WHEN NOT MATCHED THEN INSERT ...`"
        correct: true
      - text: "`INSERT OVERWRITE`"
      - text: "`COPY INTO`"
      - text: "`UPSERT TABLE`"
    explanation: "`MERGE` combines updates, inserts and deletes in one atomic operation, driven by a join condition."
  - id: version-as-of
    prompt: How do you query a Delta table as it was at version 12?
    options:
      - id: version
        text: "`SELECT * FROM main.sales.orders VERSION AS OF 12`"
        correct: true
      - text: "`SELECT * FROM main.sales.orders@12`"
      - text: "`SELECT * FROM main.sales.orders WHERE _version = 12`"
    explanation: "`VERSION AS OF` and `TIMESTAMP AS OF` select an earlier snapshot. (`@v12` shorthand also exists in Databricks SQL, but the explicit syntax is clearest.)"
  - id: history
    prompt: Which statements about Delta history and restore are true?
    type: multiple
    options:
      - text: "`DESCRIBE HISTORY` shows each version with its operation, user and timestamp"
        correct: true
      - text: "`RESTORE TABLE ... TO VERSION AS OF n` rolls the table back by writing a new commit"
        correct: true
      - text: Time travel is limited by data file retention (VACUUM) and log retention settings
        correct: true
      - text: Restoring a table deletes all later versions from the log
    explanation: Restore is itself a new version, so you can even undo a restore.
---
## Learning objectives

- Upsert data with `MERGE`
- Query and restore earlier versions
- Use `MERGE` for slowly changing dimensions

## MERGE

```sql
MERGE INTO main.sales.customers AS t
USING main.staging.customer_updates AS s
  ON t.customer_id = s.customer_id
WHEN MATCHED AND s.is_deleted THEN DELETE
WHEN MATCHED THEN UPDATE SET *
WHEN NOT MATCHED AND NOT s.is_deleted THEN INSERT *
WHEN NOT MATCHED BY SOURCE AND t.source_system = 'crm' THEN DELETE;
```

- `UPDATE SET *` / `INSERT *` map columns by name
- `WHEN NOT MATCHED BY SOURCE` handles target rows absent from the source
- The source must not contain duplicate keys for the same target row — deduplicate first

The Python API offers the same operation:

```python
from delta.tables import DeltaTable

(DeltaTable.forName(spark, "main.sales.customers").alias("t")
    .merge(updates.alias("s"), "t.customer_id = s.customer_id")
    .whenMatchedUpdateAll()
    .whenNotMatchedInsertAll()
    .execute())
```

## UPDATE and DELETE

```sql
UPDATE main.sales.orders SET status = 'CANCELLED' WHERE order_id = 1001;
DELETE FROM main.sales.orders WHERE ordered_at < '2019-01-01';
```

With **deletion vectors** (enabled by default for new tables on recent runtimes), these operations
mark rows as deleted instead of rewriting whole Parquet files, making them much faster.

## Time travel

```sql
DESCRIBE HISTORY main.sales.orders;

SELECT * FROM main.sales.orders VERSION AS OF 12;
SELECT * FROM main.sales.orders TIMESTAMP AS OF '2026-10-31T23:59:59';

RESTORE TABLE main.sales.orders TO VERSION AS OF 12;
```

```python
v12 = spark.read.option("versionAsOf", 12).table("main.sales.orders")
```

Uses: auditing, reproducing ML training data, debugging pipelines and undoing mistakes.

## Retention

| Setting | Default | Controls |
|---------|---------|----------|
| `delta.deletedFileRetentionDuration` | 7 days | How long VACUUM keeps unreferenced files |
| `delta.logRetentionDuration` | 30 days | How long log history is kept |

Time travel needs both the log entries and the data files for the target version.
