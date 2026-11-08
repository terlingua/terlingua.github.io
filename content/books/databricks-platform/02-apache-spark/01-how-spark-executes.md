---
title: How Spark executes work
questions:
  - id: lazy
    prompt: "What happens when you run `df2 = df.filter(df.amount > 100).select(\"id\")` in PySpark?"
    options:
      - id: plan
        text: Nothing is computed yet; Spark records the transformations in a logical plan
        correct: true
      - text: Spark immediately reads the data and filters it
      - text: The data is copied to the driver
      - text: Spark writes a temporary table
    explanation: Transformations are lazy. Work only happens when an action such as `count()`, `show()`, `collect()` or a write is called.
  - id: wide
    prompt: Which operation causes a shuffle?
    options:
      - id: groupby
        text: "`groupBy(...).agg(...)`"
        correct: true
      - text: "`filter`"
      - text: "`select`"
      - text: "`withColumn`"
    explanation: Wide transformations such as aggregations, joins (other than broadcast joins) and `repartition` move data between executors across the network.
  - id: roles
    prompt: Which statements about Spark's architecture are true?
    type: multiple
    options:
      - text: The driver builds the execution plan and schedules tasks
        correct: true
      - text: Executors run tasks, each task processing one partition
        correct: true
      - text: A job is split into stages at shuffle boundaries
        correct: true
      - text: "`collect()` on a huge DataFrame is safe because data stays on the executors"
    explanation: "`collect()` brings every row to the driver and can exhaust its memory."
---
## Learning objectives

- Describe the driver, executors, jobs, stages and tasks
- Distinguish transformations from actions
- Recognise narrow and wide transformations

## Architecture

| Component | Role |
|-----------|------|
| Driver | Runs your program, builds the plan, schedules tasks |
| Executors | JVM processes on worker nodes that run tasks and cache data |
| Partition | A chunk of data processed by one task |
| Job | Triggered by an action |
| Stage | A set of tasks that can run without a shuffle |
| Task | Unit of work on one partition |

## Lazy evaluation

```python
from pyspark.sql import functions as F

trips = spark.read.table("samples.nyctaxi.trips")              # transformation
long_trips = trips.where(F.col("trip_distance") > 10)          # transformation
by_zip = long_trips.groupBy("pickup_zip").agg(F.avg("fare_amount").alias("avg_fare"))  # transformation

by_zip.show(5)                                                 # action → job runs
```

Laziness lets Spark's **Catalyst** optimiser rewrite the whole plan: pushing filters down to the
file scan, pruning unused columns and choosing join strategies.

```python
by_zip.explain(mode="formatted")
```

## Narrow vs wide

| Narrow (no shuffle) | Wide (shuffle) |
|---------------------|----------------|
| `select`, `where`, `withColumn`, `union` | `groupBy`, `join`, `distinct`, `orderBy`, `repartition` |

Shuffles write data to disk and send it over the network, so they dominate the cost of most jobs.
Each shuffle boundary starts a new stage.

## Actions to be careful with

- `collect()` / `toPandas()` — pull all data to the driver; only for small results
- `count()` on huge tables just to log a number — it triggers a full job
- Repeated actions on the same expensive DataFrame — consider caching or writing an intermediate table
