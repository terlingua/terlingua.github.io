---
title: Databricks SQL, dashboards and Genie
questions:
  - id: warehouse-type
    prompt: Which SQL warehouse type starts fastest and requires no capacity management?
    options:
      - text: Classic
      - text: Pro
      - id: serverless
        text: Serverless
        correct: true
      - text: All-purpose
    explanation: Serverless warehouses start in seconds and include features such as intelligent workload management.
  - id: genie
    prompt: What is an AI/BI Genie space?
    options:
      - id: nl
        text: A space where business users ask questions in natural language and get answers generated as SQL over curated tables
        correct: true
      - text: A notebook for training models
      - text: A storage location for files
    explanation: Data teams curate tables, instructions and example queries so Genie answers reliably.
  - id: features
    prompt: Which are features of Databricks SQL?
    type: multiple
    options:
      - text: A SQL editor with query history and query profiles
        correct: true
      - text: Alerts that notify when a query result meets a condition
        correct: true
      - text: AI/BI dashboards
        correct: true
      - text: Connectivity for BI tools such as Power BI and Tableau through JDBC/ODBC
        correct: true
    explanation: Databricks SQL is the warehousing and BI surface of the platform.
---
## Learning objectives

- Use SQL warehouses for analytics
- Build AI/BI dashboards and alerts
- Enable self-service questions with Genie

## SQL warehouses

| Type | Characteristics |
|------|-----------------|
| Serverless | Fast start, managed capacity, Photon, best price/performance for most |
| Pro | Runs in your cloud account, supports advanced features |
| Classic | Basic features, runs in your cloud account |

Warehouses scale out by adding clusters when concurrency rises and scale back when idle;
auto-stop prevents paying for idle time.

## Modern Databricks SQL

```sql
-- Named parameters in queries and dashboards
SELECT region, SUM(revenue) AS revenue
FROM prod.gold.daily_revenue
WHERE order_date BETWEEN :start_date AND :end_date
GROUP BY ALL
ORDER BY revenue DESC;

-- Lateral column aliases, QUALIFY, and AI functions
SELECT customer_id, order_date, total,
       total / SUM(total) OVER (PARTITION BY customer_id) AS share
FROM prod.silver.orders
QUALIFY ROW_NUMBER() OVER (PARTITION BY customer_id ORDER BY order_date DESC) <= 5;
```

## AI/BI dashboards

Dashboards (formerly "Lakeview") are built on datasets defined by SQL, with visualisations,
filters and parameters. They can be published with embedded credentials so viewers don't need
warehouse access, scheduled for e-mail delivery, and embedded in other applications.

## Alerts

An alert runs a query on a schedule and notifies e-mail, Slack or webhooks when a condition is met —
for example, when yesterday's revenue is 30% below the trailing average.

## Genie

1. Create a Genie space on a small set of well-documented gold tables
2. Add table and column comments, instructions ("fiscal year starts in April") and example SQL
3. Let business users ask questions; review answers and improve instructions over time

Good metadata — comments on tables and columns in Unity Catalog — improves Genie, AI-assisted SQL
and catalog search alike.

## Metric views

Unity Catalog **metric views** define business metrics (measures and dimensions) once in YAML, so
dashboards, Genie and SQL users compute "revenue" or "active customers" consistently.
