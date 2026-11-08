---
title: Table layout and maintenance
questions:
  - id: vacuum
    prompt: What does `VACUUM` do on a Delta table?
    options:
      - text: Compacts small files into larger ones
      - text: Deletes old rows from the table
      - id: delete-files
        text: Deletes data files that are no longer referenced by the table and are older than the retention threshold
        correct: true
      - text: Rebuilds statistics
    explanation: VACUUM removes unreferenced files to save storage. It does not change current table data, but it limits time travel.
  - id: liquid
    prompt: What is liquid clustering?
    options:
      - id: clustering
        text: A flexible data layout (`CLUSTER BY`) that replaces Hive-style partitioning and Z-ordering, and whose keys can be changed without rewriting the table
        correct: true
      - text: A way to replicate tables across regions
      - text: A type of cluster autoscaling
    explanation: Databricks recommends liquid clustering for new tables. `CLUSTER BY AUTO` lets the platform choose keys based on query patterns.
  - id: optimize
    prompt: Which statements about table maintenance are true?
    type: multiple
    options:
      - text: "`OPTIMIZE` compacts small files into larger ones"
        correct: true
      - text: Predictive optimization can run `OPTIMIZE`, `VACUUM` and statistics collection automatically for Unity Catalog managed tables
        correct: true
      - text: Over-partitioning by a high-cardinality column creates many small files and hurts performance
        correct: true
      - text: Liquid clustering and Z-ORDER can be used on the same table at the same time
    explanation: Liquid clustering is not compatible with partitioning or Z-ORDER on the same table.
---
## Learning objectives

- Keep Delta tables fast with compaction and clustering
- Choose liquid clustering over partitioning for new tables
- Understand VACUUM and predictive optimization

## The small-file problem

Streaming and frequent appends create many small files. Each file adds open/read overhead and
weakens data skipping. Delta collects min/max statistics per file, so well-organised files let the
engine skip most of a table for selective queries.

## OPTIMIZE

```sql
OPTIMIZE main.sales.orders;
```

## Liquid clustering

```sql
CREATE TABLE main.sales.events (
  event_id STRING, customer_id BIGINT, event_type STRING, event_time TIMESTAMP
) CLUSTER BY (customer_id, event_time);

ALTER TABLE main.sales.events CLUSTER BY (event_type, event_time);   -- change keys later
ALTER TABLE main.sales.events CLUSTER BY AUTO;                       -- let Databricks choose
OPTIMIZE main.sales.events;                                          -- cluster incrementally
```

| Approach | Status |
|----------|--------|
| Hive-style `PARTITIONED BY` | Still useful for very large tables with a natural low-cardinality key, but rigid |
| `ZORDER BY` (with `OPTIMIZE`) | Legacy; superseded by liquid clustering |
| `CLUSTER BY` (liquid) | Recommended for new tables |

## VACUUM

```sql
VACUUM main.sales.orders;                  -- default 7-day retention
VACUUM main.sales.orders RETAIN 240 HOURS; -- keep 10 days
```

Never set retention lower than the longest-running query or stream that might read older files.

## Predictive optimization

For Unity Catalog **managed** tables, predictive optimization automatically runs `OPTIMIZE`,
`VACUUM` and `ANALYZE` when it predicts a benefit — removing most manual maintenance jobs. It can be
enabled at the account, catalog or schema level.

## Statistics

```sql
ANALYZE TABLE main.sales.orders COMPUTE STATISTICS FOR ALL COLUMNS;
```

Table and column statistics help the optimizer choose join strategies.
