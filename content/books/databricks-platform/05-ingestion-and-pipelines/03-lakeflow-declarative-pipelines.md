---
title: Lakeflow Declarative Pipelines
questions:
  - id: what
    prompt: What is the main idea of Lakeflow Declarative Pipelines (formerly Delta Live Tables)?
    options:
      - id: declarative
        text: You declare the tables and the queries that produce them; the platform builds the dependency graph and manages orchestration, checkpoints, retries and infrastructure
        correct: true
      - text: You write imperative code that manages checkpoints, retries and table creation yourself
      - text: It is a BI dashboard tool
      - text: It replaces Delta Lake as a storage format
    explanation: Declarative pipelines let you focus on transformations while the engine handles the operational plumbing.
  - id: drop-row
    prompt: "What does `CONSTRAINT valid_amount EXPECT (amount > 0) ON VIOLATION DROP ROW` do?"
    options:
      - text: Fails the whole pipeline update when any row violates the rule
      - text: Keeps all rows and only logs a warning
      - id: drop
        text: Drops rows that violate the rule and records the violation counts in pipeline metrics
        correct: true
    explanation: Without `ON VIOLATION`, rows are kept and counted; `FAIL UPDATE` stops the update.
  - id: table-types
    prompt: Which statements about pipeline dataset types are true?
    type: multiple
    options:
      - text: Streaming tables process each input row once, suiting append-only ingestion
        correct: true
      - text: Materialized views store precomputed results that are refreshed, incrementally when possible
        correct: true
      - text: "`AUTO CDC` (previously `APPLY CHANGES`) applies change data capture feeds, including SCD type 1 and 2"
        correct: true
      - text: Materialized views must always be recomputed from scratch
    explanation: Serverless pipelines can refresh many materialized views incrementally.
---
## Learning objectives

- Build a pipeline with streaming tables and materialized views
- Enforce data quality with expectations
- Apply CDC feeds with AUTO CDC

## A SQL pipeline

```sql
CREATE OR REFRESH STREAMING TABLE bronze_orders
COMMENT 'Raw orders from the landing volume'
AS SELECT *, _metadata.file_path AS source_file
FROM STREAM read_files('/Volumes/main/raw/landing/orders/', format => 'json');

CREATE OR REFRESH STREAMING TABLE silver_orders (
  CONSTRAINT valid_id     EXPECT (order_id IS NOT NULL) ON VIOLATION DROP ROW,
  CONSTRAINT valid_amount EXPECT (total >= 0)            ON VIOLATION DROP ROW,
  CONSTRAINT known_status EXPECT (status IN ('NEW', 'PAID', 'SHIPPED', 'CANCELLED'))
)
AS SELECT CAST(order_id AS BIGINT) AS order_id, customer_id, status,
          CAST(total AS DECIMAL(12,2)) AS total, CAST(ordered_at AS TIMESTAMP) AS ordered_at
FROM STREAM(bronze_orders);

CREATE OR REFRESH MATERIALIZED VIEW gold_daily_revenue
AS SELECT DATE(ordered_at) AS order_date, SUM(total) AS revenue, COUNT(*) AS orders
FROM silver_orders
WHERE status <> 'CANCELLED'
GROUP BY ALL;
```

The pipeline infers that `gold_daily_revenue` depends on `silver_orders`, which depends on
`bronze_orders`, and runs them in order.

## The same in Python

```python
import dlt
from pyspark.sql import functions as F

@dlt.table(comment="Raw orders")
def bronze_orders():
    return (spark.readStream.format("cloudFiles")
            .option("cloudFiles.format", "json")
            .load("/Volumes/main/raw/landing/orders/"))

@dlt.table
@dlt.expect_or_drop("valid_id", "order_id IS NOT NULL")
def silver_orders():
    return dlt.read_stream("bronze_orders").withColumn("total", F.col("total").cast("decimal(12,2)"))
```

Newer runtimes also expose these APIs through the `pyspark.pipelines` module, reflecting the
donation of declarative pipelines to Apache Spark; existing `dlt` code keeps working.

## Expectations

| Clause | Effect on bad rows |
|--------|--------------------|
| `EXPECT (...)` | Kept; violations counted |
| `... ON VIOLATION DROP ROW` | Dropped; counted |
| `... ON VIOLATION FAIL UPDATE` | Update fails |

## Change data capture

```sql
CREATE OR REFRESH STREAMING TABLE customers;

CREATE FLOW customers_cdc AS AUTO CDC INTO customers
FROM STREAM(cdc_customers_raw)
KEYS (customer_id)
APPLY AS DELETE WHEN operation = 'DELETE'
SEQUENCE BY event_ts
COLUMNS * EXCEPT (operation, event_ts)
STORED AS SCD TYPE 2;
```

`SEQUENCE BY` resolves out-of-order events; `SCD TYPE 2` keeps history with validity columns.

## Running pipelines

Pipelines run in **triggered** mode (process available data, then stop) or **continuous** mode.
Development mode reuses compute for fast iteration; production mode adds retries and fresh compute.
Serverless pipelines are recommended for most new work.
