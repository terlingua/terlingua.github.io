---
title: Databricks SQL and warehouses
questions:
  - id: dbsql
    prompt: What is Databricks SQL?
    options:
      - id: dbsql
        text: The SQL analytics experience — SQL editor, queries, alerts, dashboards and SQL warehouses — for analysts and BI tools on lakehouse data
        correct: true
      - text: A separate database product with its own storage
      - text: A dialect of PL/SQL
      - text: A Spark library for Scala
    explanation: It queries the same Delta/Iceberg tables in Unity Catalog; there is no separate warehouse copy.
  - id: warehouse-types
    prompt: Which SQL warehouse types exist?
    type: multiple
    options:
      - id: serverless
        text: Serverless
        correct: true
      - id: pro
        text: Pro
        correct: true
      - id: classic
        text: Classic
        correct: true
      - text: Embedded
    explanation: Serverless is recommended for most use; pro and classic run in your account. Not every feature is available on classic.
  - id: sizing
    prompt: How do SQL warehouses handle more concurrent users?
    options:
      - text: By making each query slower
      - text: They cannot
      - id: scale-clusters
        text: By adding clusters between a minimum and maximum (scaling out), while T-shirt size (2X-Small…4X-Large) determines per-query power (scaling up)
        correct: true
      - text: By queuing all queries forever
    explanation: Size for the heaviest query, cluster count for concurrency. Intelligent workload management on serverless adjusts more dynamically.
  - id: caching
    prompt: Which caches can speed up repeated queries?
    type: multiple
    options:
      - id: result
        text: Query result cache
        correct: true
      - id: disk
        text: Disk (local SSD) cache of remote data files
        correct: true
      - text: The browser's cookie cache
    explanation: Result caches are invalidated when underlying tables change.
  - id: ansi
    prompt: Databricks SQL defaults to ANSI SQL mode. What changes for a query doing `CAST('abc' AS INT)`?
    options:
      - text: It returns 0
      - text: It returns NULL silently
      - text: It returns 'abc'
      - id: error
        text: It raises an error rather than returning NULL; `try_cast` returns NULL when that is desired
        correct: true
    explanation: ANSI mode surfaces data problems instead of hiding them. Migrations from older Spark code sometimes trip on this.
  - id: bi-tools
    prompt: How do external BI tools such as Power BI and Tableau connect?
    options:
      - text: They read Parquet files directly from storage
      - id: jdbc
        text: Through JDBC/ODBC drivers or native connectors to a SQL warehouse, with Unity Catalog permissions applied to the connecting identity
        correct: true
      - text: Through email exports
      - text: They cannot connect
    explanation: Power BI import mode copies data into Power BI; DirectQuery sends queries each time. That choice affects cost and freshness.
  - id: alerts
    prompt: What does a Databricks SQL alert do?
    options:
      - text: Alerts the user when the warehouse is idle
      - id: condition
        text: Runs a query on a schedule and notifies destinations (email, Slack, webhooks) when a condition on the result is met
        correct: true
      - text: Sends all query results by email
      - text: Detects security breaches automatically
    explanation: Alerts are a simple way to monitor data quality and business metrics.
  - id: query-profile
    prompt: A dashboard query is slow. Which tool shows where time is spent?
    options:
      - text: The job run history
      - text: The billing page
      - id: profile
        text: The query profile in query history, showing operators, rows, time and bytes read, including how many files were pruned
        correct: true
      - text: The notebook revision history
    explanation: Look for full scans, spills and skew. Fixes usually involve clustering, filters on clustering keys, or pre-aggregation in gold tables.
  - id: sql-scripting
    prompt: Which features show Databricks SQL moving toward a full warehouse dialect?
    type: multiple
    options:
      - id: scripting
        text: SQL scripting (variables, loops, control flow) and stored procedures
        correct: true
      - id: constraints
        text: Primary and foreign key constraints (informational) and identity columns
        correct: true
      - id: multi-statement
        text: Multi-statement transactions
        correct: true
      - text: COBOL support
    explanation: These features help warehouse migrations. Check status; some were in preview through 2025–2026. Note that primary/foreign keys are not enforced.
  - id: pk-not-enforced
    prompt: You declare a primary key constraint on a Delta table. Will a duplicate key insert fail?
    options:
      - id: informational
        text: No — primary and foreign keys are informational (used by tools and the optimiser) and are not enforced; enforce uniqueness in pipelines
        correct: true
      - text: Yes, always
      - text: Only on serverless
      - text: Only for integer keys
    explanation: "`NOT NULL` and `CHECK` constraints are enforced; PK/FK are not. Many analysts are surprised by this."
---
## Learning objectives

- Describe Databricks SQL and SQL warehouse types and sizing
- Use caching, alerts and query profiles
- Know which constraints are enforced

## Example

```sql
CREATE TABLE main.gold.customers (
  customer_id BIGINT NOT NULL,
  email STRING,
  CONSTRAINT customers_pk PRIMARY KEY (customer_id)
);

SELECT c.customer_id, SUM(o.amount) AS lifetime_value
FROM main.gold.customers c
JOIN main.silver.orders o USING (customer_id)
GROUP BY ALL
ORDER BY lifetime_value DESC
LIMIT 20;
```

## Sources

- Databricks docs, "Databricks SQL" — <https://docs.databricks.com/aws/en/sql/>
- Databricks docs, "SQL warehouse types" — <https://docs.databricks.com/aws/en/compute/sql-warehouse/warehouse-types>
- Databricks docs, "Constraints" — <https://docs.databricks.com/aws/en/tables/constraints>
- Databricks docs, "ANSI compliance" — <https://docs.databricks.com/aws/en/sql/language-manual/sql-ref-ansi-compliance>
