---
title: Ingestion — Lakeflow Connect, Auto Loader and friends
questions:
  - id: lakeflow
    prompt: What is Lakeflow?
    options:
      - text: A Delta Lake file format
      - text: A third-party ETL tool
      - text: A streaming protocol
      - id: umbrella
        text: Databricks' data engineering product family — Lakeflow Connect (ingestion), Spark Declarative Pipelines (formerly Delta Live Tables) and Lakeflow Jobs (formerly Workflows), plus Lakeflow Designer
        correct: true
    explanation: Names changed in 2025. Older material says DLT and Workflows; the concepts carry over.
  - id: connect
    prompt: What does Lakeflow Connect provide?
    options:
      - id: managed-connectors
        text: Managed connectors that ingest from SaaS applications (such as Salesforce, Workday) and databases (such as SQL Server, with change data capture) into Delta tables
        correct: true
      - text: A JDBC driver for BI tools
      - text: A replacement for Unity Catalog
      - text: VPN connectivity
    explanation: Managed connectors trade flexibility for reduced maintenance. Check connector maturity — some are in preview — and source-system load.
  - id: auto-loader
    prompt: What does Auto Loader (`cloudFiles`) do?
    options:
      - text: Loads clusters automatically
      - id: incremental-files
        text: Incrementally and exactly-once ingests new files arriving in cloud storage, tracking what has been processed and optionally inferring and evolving schema
        correct: true
      - text: Downloads files from the web
      - text: Backs up tables
    explanation: It is built on Structured Streaming. Run it continuously or with `trigger(availableNow=True)` for scheduled incremental batches.
  - id: auto-loader-code
    prompt: "In `spark.readStream.format(\"cloudFiles\").option(\"cloudFiles.format\", \"json\")`, what does the checkpoint location (set on the write) store?"
    options:
      - text: The data itself
      - text: User credentials
      - id: state
        text: Progress and state — which files were processed — so a restarted stream resumes without duplicates or gaps
        correct: true
      - text: The cluster configuration
    explanation: Deleting or sharing checkpoints between streams breaks exactly-once guarantees. Treat checkpoints as part of the table's state.
  - id: copy-into
    prompt: What is `COPY INTO`?
    options:
      - text: A command to clone a workspace
      - text: A Git command
      - id: idempotent-sql
        text: A SQL command that idempotently loads files from a location into a table, skipping files already loaded
        correct: true
      - text: A way to copy notebooks
    explanation: Good for simple, SQL-only, moderate-volume loads. Auto Loader scales better to millions of files.
  - id: rescued
    prompt: Auto Loader with schema inference meets a field with an unexpected type. What happens by default?
    options:
      - text: The stream crashes and loses data
      - text: The row is dropped silently
      - text: The table is deleted
      - id: rescued
        text: The value goes into the `_rescued_data` column so data is not lost, and you can handle it downstream
        correct: true
    explanation: Schema evolution mode `addNewColumns` stops the stream when new columns appear so the restarted stream picks up the new schema — expect and plan for these restarts.
  - id: cdc
    prompt: What is change data capture (CDC)?
    options:
      - id: cdc
        text: Capturing inserts, updates and deletes from a source system (often from its transaction log) and applying them downstream
        correct: true
      - text: A compression codec
      - text: A dashboard type
      - text: Copying the full table every night
    explanation: "Full reloads are simpler but scale poorly. CDC is incremental but adds complexity: ordering, deletes, and late or duplicate events."
  - id: zerobus
    prompt: What is Zerobus Ingest?
    options:
      - text: A bus service for the office
      - text: A Kafka replacement for on-premises use only
      - id: push
        text: A push-based ingestion API that lets applications write events directly into Delta tables without operating a message bus
        correct: true
      - text: A file format
    explanation: Announced in 2025 and reported as GA in 2026. It suits telemetry-style writes. Weigh it against Kafka or Event Hubs if other consumers also need the events.
  - id: federation
    prompt: What is Lakehouse Federation?
    options:
      - text: Copying all databases into Delta
      - id: query-in-place
        text: Querying external databases (PostgreSQL, SQL Server, Snowflake, BigQuery and others) in place through Unity Catalog foreign catalogs, without ingesting
        correct: true
      - text: A political alliance of vendors
      - text: Merging workspaces
    explanation: Federation is useful for exploration and small lookups. Heavy, repeated queries usually justify ingestion because each query hits the source system.
  - id: ingestion-choice
    prompt: Which ingestion choice fits each scenario best?
    type: multiple
    options:
      - id: files-autoloader
        text: Millions of JSON files landing in S3 daily → Auto Loader
        correct: true
      - id: saas-connect
        text: Salesforce objects → a Lakeflow Connect managed connector
        correct: true
      - id: occasional-federation
        text: Occasional lookups against a small operational Postgres → Lakehouse Federation
        correct: true
      - text: A 50-row reference table edited monthly → a 24/7 streaming cluster
    explanation: Match the tool to volume, frequency and ownership. A permanently running stream for a monthly change is waste.
---
## Learning objectives

- Map ingestion needs to Lakeflow Connect, Auto Loader, `COPY INTO`, Zerobus and Federation
- Explain checkpoints, rescued data and CDC

## Auto Loader

```python
(spark.readStream.format("cloudFiles")
   .option("cloudFiles.format", "json")
   .option("cloudFiles.schemaLocation", "/Volumes/main/raw/_schemas/orders")
   .load("/Volumes/main/raw/landing/orders/")
 .writeStream
   .option("checkpointLocation", "/Volumes/main/raw/_checkpoints/orders")
   .trigger(availableNow=True)
   .toTable("main.bronze.orders"))
```

## COPY INTO

```sql
COPY INTO main.bronze.orders
FROM '/Volumes/main/raw/landing/orders/'
FILEFORMAT = JSON
COPY_OPTIONS ('mergeSchema' = 'true');
```

## Sources

- Databricks docs, "Lakeflow Connect" — <https://docs.databricks.com/aws/en/ingestion/lakeflow-connect/>
- Databricks docs, "Auto Loader" — <https://docs.databricks.com/aws/en/ingestion/cloud-object-storage/auto-loader/>
- Databricks docs, "COPY INTO" — <https://docs.databricks.com/aws/en/ingestion/cloud-object-storage/copy-into/>
- Databricks docs, "Lakehouse Federation" — <https://docs.databricks.com/aws/en/query-federation/>
