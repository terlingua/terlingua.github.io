---
title: The Runtime, Spark and Photon
questions:
  - id: dbr
    prompt: What is the Databricks Runtime (DBR)?
    options:
      - text: The open-source Apache Spark release, unchanged
      - id: dbr
        text: A versioned bundle of Apache Spark plus Databricks optimisations, Delta Lake, libraries and system components that compute runs on
        correct: true
      - text: A Python interpreter only
      - text: The web UI
    explanation: LTS runtime versions are supported for about three years. ML runtimes add ML libraries and GPU support.
  - id: lts
    prompt: Why pin production jobs to an LTS runtime?
    options:
      - text: LTS runtimes are faster
      - text: Non-LTS runtimes are unsupported on day one
      - text: LTS runtimes are free
      - id: stability
        text: LTS versions receive fixes for longer, reducing forced upgrades and behaviour changes in production
        correct: true
    explanation: Plan upgrades deliberately and test, because Spark minor versions can change behaviour (for example ANSI mode defaults in Spark 4).
  - id: spark
    prompt: Who created Apache Spark?
    options:
      - id: amplab
        text: Matei Zaharia and colleagues at UC Berkeley's AMPLab, around 2009; the creators later founded Databricks in 2013
        correct: true
      - text: Google, as part of BigQuery
      - text: Microsoft Research
      - text: Netflix
    explanation: Spark is an Apache Software Foundation project. Databricks is a major contributor, but it does not own Spark.
  - id: lazy
    prompt: Spark transformations such as `filter` and `select` are lazy. What does that mean?
    options:
      - text: They run slowly
      - text: They run on a single thread
      - id: lazy
        text: They build a plan; nothing executes until an action such as `count`, `write` or `display` is called, letting the optimiser plan the whole query
        correct: true
      - text: They are skipped
    explanation: The Catalyst optimiser rewrites the plan (predicate pushdown, column pruning) before execution.
  - id: shuffle
    prompt: Which operations typically cause a shuffle — moving data between executors over the network?
    type: multiple
    options:
      - id: join
        text: Joins between large tables
        correct: true
      - id: groupby
        text: "`groupBy` aggregations"
        correct: true
      - id: distinct
        text: "`distinct` and `orderBy`"
        correct: true
      - text: "`filter` on a single column"
    explanation: Shuffles are the main cost in many Spark jobs. Broadcasting a small table avoids shuffling the large one.
  - id: photon
    prompt: What is Photon?
    options:
      - text: An open-source Spark plugin anyone can use
      - text: A GPU-only engine
      - id: photon
        text: Databricks' proprietary vectorised query engine written in C++, compatible with Spark APIs, that accelerates SQL and DataFrame workloads
        correct: true
      - text: A data visualisation library
    explanation: Photon is on by default for SQL warehouses and serverless. Its DBU rate is higher on classic compute, so it pays off only when it speeds the workload enough.
  - id: photon-not
    prompt: Which workloads benefit least from Photon?
    options:
      - text: Large joins and aggregations
      - text: Writing Delta tables
      - text: SQL scans with filters
      - id: udfs
        text: Code dominated by Python UDFs or RDD operations, which Photon cannot vectorise
        correct: true
    explanation: Photon falls back to Spark for unsupported operations. Prefer built-in functions over UDFs.
  - id: aqe
    prompt: What does Adaptive Query Execution (AQE) do?
    options:
      - id: aqe
        text: Re-optimises a query at runtime using actual statistics — coalescing shuffle partitions, switching join strategies and handling skew
        correct: true
      - text: Translates SQL to Python
      - text: Adds more workers
      - text: Caches all tables
    explanation: AQE is on by default in Spark 3.2+. It reduces, but does not remove, the need to understand data skew.
  - id: spark-connect
    prompt: What is Spark Connect?
    options:
      - text: A JDBC driver for BI tools
      - id: client-server
        text: A client-server protocol that decouples client applications from the Spark driver, used by serverless compute and Databricks Connect
        correct: true
      - text: A networking cable
      - text: A data catalogue
    explanation: It is why some low-level APIs (RDDs, `SparkContext`) are unavailable on serverless and standard access mode.
  - id: python-udf
    prompt: Why are row-at-a-time Python UDFs slow?
    options:
      - text: Python cannot do arithmetic
      - text: UDFs run on the driver only
      - id: serialization
        text: Each row is serialised between the JVM (or Photon) and a Python worker; vectorised pandas UDFs with Arrow, or built-in functions, avoid most of this overhead
        correct: true
      - text: They are not slow
    explanation: Built-in SQL functions are almost always faster than UDFs.
  - id: pandas-api
    prompt: A data scientist calls `df.toPandas()` on a 500 GB DataFrame. What happens?
    options:
      - text: Spark converts it in a distributed way
      - text: It works, but slowly
      - text: Databricks samples 1% automatically
      - id: oom
        text: Spark tries to collect all data to the driver's memory, which fails with out-of-memory or crashes the driver
        correct: true
    explanation: Aggregate or sample in Spark first, or use the pandas API on Spark (`pyspark.pandas`) for distributed pandas-style code.
---
## Learning objectives

- Explain what the Databricks Runtime contains and how versions are supported
- Understand lazy evaluation, shuffles and the role of the driver
- Know what Photon accelerates and what it does not

## Spark execution in brief

```python
orders = spark.table("main.sales.orders")
daily = (orders
         .filter("order_ts >= '2026-01-01'")
         .groupBy(F.to_date("order_ts").alias("day"))
         .agg(F.sum("amount").alias("revenue")))
daily.write.mode("overwrite").saveAsTable("main.sales.daily_revenue")
```

Nothing runs until `saveAsTable`. Spark then optimises the plan: reads only two columns, pushes the
filter into the scan, and shuffles once for the aggregation.

## Proprietary versus open

| Open source | Databricks-specific |
|---|---|
| Apache Spark, Delta Lake, MLflow, Unity Catalog OSS | Photon, serverless, predictive optimisation, Genie, AI/BI |

Code written against the open APIs runs elsewhere. Performance characteristics, however, may not.

## Sources

- Databricks docs, "Databricks Runtime release notes and compatibility" — <https://docs.databricks.com/aws/en/release-notes/runtime/>
- Behm et al., "Photon: A Fast Query Engine for Lakehouse Systems", SIGMOD 2022 — <https://people.eecs.berkeley.edu/~matei/papers/2022/sigmod_photon.pdf>
- Apache Spark, "Adaptive Query Execution" — <https://spark.apache.org/docs/latest/sql-performance-tuning.html>
- Apache Spark, "Spark Connect overview" — <https://spark.apache.org/docs/latest/spark-connect-overview.html>
