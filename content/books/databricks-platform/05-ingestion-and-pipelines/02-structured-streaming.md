---
title: Structured Streaming
questions:
  - id: model
    prompt: How does Structured Streaming model a stream?
    options:
      - id: unbounded-table
        text: As an unbounded table to which new rows are continuously appended, queried with the same DataFrame API as batch
        correct: true
      - text: As a queue that must be polled manually
      - text: As a set of files that must be listed in each job
      - text: As an in-memory list on the driver
    explanation: You write the query once; the engine runs it incrementally as new data arrives.
  - id: checkpoint
    prompt: What is stored in a streaming query's checkpoint location?
    options:
      - text: The output data
      - id: progress
        text: Progress offsets and state, so the query can restart where it left off with exactly-once guarantees
        correct: true
      - text: A copy of the source code
    explanation: Each streaming query needs its own checkpoint location; never share one between queries.
  - id: watermark
    prompt: Which statements about watermarks are true?
    type: multiple
    options:
      - text: They define how late data can arrive and still be included in event-time aggregations
        correct: true
      - text: They let Spark discard old state, keeping memory bounded
        correct: true
      - text: "They are set with `withWatermark(\"event_time\", \"10 minutes\")`"
        correct: true
      - text: They guarantee that no data is ever dropped
    explanation: Data arriving later than the watermark may be dropped from windowed aggregations — that is the trade-off for bounded state.
---
## Learning objectives

- Write streaming queries with the DataFrame API
- Use checkpoints, triggers and output modes
- Aggregate by event time with watermarks

## A streaming query

```python
from pyspark.sql import functions as F

events = spark.readStream.table("main.bronze.clicks")

per_minute = (events
    .withWatermark("event_time", "10 minutes")
    .groupBy(F.window("event_time", "1 minute"), "page")
    .count())

(per_minute.writeStream
    .outputMode("append")
    .option("checkpointLocation", "/Volumes/main/checkpoints/clicks_per_minute")
    .trigger(processingTime="1 minute")
    .toTable("main.silver.clicks_per_minute"))
```

## Triggers

| Trigger | Behaviour |
|---------|-----------|
| Default (none) | Micro-batches back to back |
| `processingTime="1 minute"` | Micro-batch every minute |
| `availableNow=True` | Process everything available, then stop |
| Real-time mode | Low-latency continuous processing (newer Databricks feature) |

## Output modes

| Mode | Emits |
|------|-------|
| `append` | Only new rows that will not change (needs a watermark for aggregations) |
| `update` | Rows that changed in this micro-batch |
| `complete` | The entire result table every time |

## foreachBatch

```python
def upsert(batch_df, batch_id):
    (DeltaTable.forName(spark, "main.silver.customers").alias("t")
        .merge(batch_df.alias("s"), "t.id = s.id")
        .whenMatchedUpdateAll().whenNotMatchedInsertAll().execute())

stream.writeStream.foreachBatch(upsert).option("checkpointLocation", cp).start()
```

`foreachBatch` gives you full batch APIs — `MERGE`, writes to several tables, JDBC sinks — for each
micro-batch.

## Operational tips

- One checkpoint per query; deleting it restarts from scratch
- Use `availableNow` jobs for cost-efficient near-real-time pipelines
- Monitor with the streaming tab in the Spark UI or `query.lastProgress`
