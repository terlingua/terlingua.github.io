---
title: Spark performance fundamentals
questions:
  - id: broadcast
    prompt: Joining a 2 TB fact table with a 5 MB dimension table, which join strategy is usually best?
    options:
      - text: Sort-merge join, shuffling both tables
      - id: broadcast
        text: Broadcast hash join, sending the small table to every executor
        correct: true
      - text: Cartesian join
      - text: Collect both tables to the driver
    explanation: Broadcasting the small side avoids shuffling the large table. Spark does this automatically below `spark.sql.autoBroadcastJoinThreshold` (10 MB by default), and AQE can switch to it at runtime.
  - id: aqe
    prompt: What does Adaptive Query Execution (AQE) do?
    options:
      - text: Automatically adds indexes to tables
      - text: Rewrites Python code into Scala
      - id: runtime
        text: Re-optimises the query plan at runtime using statistics from completed stages, for example coalescing shuffle partitions and handling skewed joins
        correct: true
    explanation: AQE is enabled by default in modern Spark and Databricks Runtime.
  - id: small-files
    prompt: Which are common causes of slow Spark jobs?
    type: multiple
    options:
      - text: Data skew, where a few partitions are much larger than the rest
        correct: true
      - text: Many tiny files, which add per-file overhead
        correct: true
      - text: Unnecessary shuffles and wide transformations
        correct: true
      - text: Using Delta Lake instead of CSV
    explanation: Skew, small files and shuffles dominate. Delta Lake usually improves performance through data skipping and compaction.
---
## Learning objectives

- Read the Spark UI to find bottlenecks
- Choose join strategies and handle skew
- Know when caching helps

## Where time goes

Use the **Spark UI** (or the query profile in Databricks SQL) to find:

- Stages with the most time and shuffle read/write
- Tasks much slower than the median — a sign of **skew**
- Spill to disk — partitions too large for executor memory

## Joins

```python
from pyspark.sql.functions import broadcast

enriched = sales.join(broadcast(stores), "store_id")
```

| Strategy | When |
|----------|------|
| Broadcast hash join | One side is small |
| Sort-merge join | Both sides large |
| Shuffle hash join | Medium, unsorted sides (AQE may choose it) |

## Adaptive Query Execution

AQE (on by default) uses runtime statistics to:

- Coalesce many small shuffle partitions into fewer, right-sized ones
- Convert sort-merge joins to broadcast joins when one side turns out small
- Split skewed partitions in joins

## Partitions and files

- `spark.sql.shuffle.partitions` defaults to 200; AQE adjusts it at runtime
- Avoid writing thousands of tiny files: compact Delta tables with `OPTIMIZE` or enable
  auto-optimise / predictive optimization (see the Delta chapter)
- Filter on clustering or partition columns so Delta can skip files

## Caching

```python
features = spark.read.table("main.ml.features").where("country = 'DE'").cache()
features.count()   # materialise
```

Cache only when the same DataFrame is reused several times in one job. On Databricks, the
**disk cache** automatically caches remote Parquet/Delta data on local SSDs for repeated reads.

## Photon

Photon is Databricks' vectorised query engine written in C++. It accelerates SQL and DataFrame
operations without code changes and is enabled by default on SQL warehouses and serverless compute.
