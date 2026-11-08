---
title: Auto Loader
questions:
  - id: format
    prompt: Which source format name is used for Auto Loader?
    options:
      - text: "`autoloader`"
      - text: "`delta`"
      - text: "`files`"
      - id: cloudfiles
        text: "`cloudFiles`"
        correct: true
    explanation: "`spark.readStream.format(\"cloudFiles\")` with `cloudFiles.format` set to the file type (json, csv, parquet, ...)."
  - id: exactly-once
    prompt: How does Auto Loader avoid processing the same file twice?
    options:
      - text: It deletes files after reading them
      - id: checkpoint
        text: It records discovered files in the stream's checkpoint, giving exactly-once ingestion
        correct: true
      - text: It compares file contents with every existing row
    explanation: State in the checkpoint location tracks which files have been processed.
  - id: schema
    prompt: Which statements about Auto Loader schema handling are true?
    type: multiple
    options:
      - text: "`cloudFiles.schemaLocation` stores the inferred schema and its evolution over time"
        correct: true
      - text: By default, new columns cause the stream to stop so the schema can be updated, then continue on restart
        correct: true
      - text: Data that doesn't match the schema can be captured in a `_rescued_data` column
        correct: true
      - text: Auto Loader can only read CSV files
    explanation: The default `addNewColumns` evolution mode fails the stream once when new columns appear, records them, and resumes with the new schema on restart.
---
## Learning objectives

- Incrementally ingest files from cloud storage with Auto Loader
- Handle schema inference and evolution
- Run ingestion as a scheduled incremental batch

## Basic ingestion

```python
(spark.readStream
    .format("cloudFiles")
    .option("cloudFiles.format", "json")
    .option("cloudFiles.schemaLocation", "/Volumes/main/checkpoints/schemas/orders")
    .load("/Volumes/main/raw/landing/orders/")
    .writeStream
    .option("checkpointLocation", "/Volumes/main/checkpoints/bronze_orders")
    .trigger(availableNow=True)
    .toTable("main.bronze.orders"))
```

- `trigger(availableNow=True)` processes all new files, then stops — perfect for a scheduled job
- Remove the trigger (or use `processingTime`) for continuous ingestion
- The checkpoint makes the job restartable and exactly-once

## Schema inference and evolution

| `cloudFiles.schemaEvolutionMode` | Behaviour on new columns |
|----------------------------------|--------------------------|
| `addNewColumns` (default) | Stream fails once, schema updated, continues on restart |
| `rescue` | Schema fixed; unexpected data goes to `_rescued_data` |
| `failOnNewColumns` | Stream fails until you update the schema |
| `none` | New columns ignored |

```python
.option("cloudFiles.inferColumnTypes", "true")
.option("cloudFiles.schemaHints", "amount DECIMAL(12,2), ordered_at TIMESTAMP")
```

Schema hints pin types you care about while inferring the rest. In production jobs, configure
automatic retries so the one-time failure on schema change resolves itself.

## File discovery modes

- **Directory listing** — lists the input path incrementally; simplest to set up
- **File notification** — subscribes to cloud storage events; scales to very large numbers of files

## SQL alternative

```sql
CREATE OR REFRESH STREAMING TABLE main.bronze.orders
AS SELECT * FROM STREAM read_files('/Volumes/main/raw/landing/orders/', format => 'json');
```

`read_files` with `STREAM` uses Auto Loader under the hood, and streaming tables manage the
checkpoint for you.
