---
title: Compute options
questions:
  - id: jobs-compute
    prompt: Why run scheduled production workloads on jobs compute rather than an all-purpose cluster?
    options:
      - text: All-purpose clusters cannot run notebooks
      - id: cheaper
        text: Jobs compute is created for the run and terminated afterwards, is isolated from interactive users, and is billed at a lower DBU rate
        correct: true
      - text: Jobs compute has no Spark
      - text: All-purpose clusters are always serverless
    explanation: All-purpose compute is meant for interactive development and is more expensive per DBU.
  - id: sql-warehouse
    prompt: Which compute type is designed for SQL queries, BI tools and dashboards?
    options:
      - text: All-purpose cluster with the ML runtime
      - text: Single-node cluster
      - id: warehouse
        text: SQL warehouse
        correct: true
    explanation: SQL warehouses (serverless, pro or classic) are optimised for SQL with Photon and handle concurrency by scaling clusters.
  - id: serverless
    prompt: Which statements about serverless compute on Databricks are true?
    type: multiple
    options:
      - text: Compute starts in seconds and Databricks manages capacity and runtime upgrades
        correct: true
      - text: It is available for notebooks, jobs, pipelines and SQL warehouses
        correct: true
      - text: You don't choose instance types or Spark configurations
        correct: true
      - text: It runs in your own cloud account's VPC/VNet like classic compute
    explanation: Serverless compute runs in a Databricks-managed compute plane, with network controls to reach your resources.
  - id: dbu
    prompt: What is a DBU?
    options:
      - id: unit
        text: A Databricks Unit — a normalised unit of processing capacity, billed per second of use, whose rate depends on the compute type
        correct: true
      - text: A Databricks storage unit equal to 1 GB
      - text: A unit of network bandwidth
    explanation: Cost is DBUs consumed multiplied by the price per DBU for that SKU (plus cloud infrastructure for classic compute).
---
## Learning objectives

- Choose the right compute for each workload
- Configure classic clusters sensibly
- Understand serverless compute and DBU billing

## Compute types

| Type | For | Notes |
|------|-----|-------|
| Serverless notebooks/jobs | Most Python and SQL workloads | Fast start, no infrastructure to manage |
| Jobs compute (classic) | Scheduled production jobs needing custom config | Ephemeral, cheaper DBU rate |
| All-purpose compute (classic) | Interactive development | Shared by users; auto-terminate when idle |
| SQL warehouse (serverless/pro/classic) | SQL, BI, dashboards | Photon; scales for concurrency |
| Lakeflow pipeline compute | Declarative pipelines | Serverless recommended |

## Configuring classic compute

- **Databricks Runtime** — pick a recent **LTS** version for stability; the ML runtime adds ML libraries
- **Access mode** — *standard* (formerly "shared") for multi-user isolation with Unity Catalog;
  *dedicated* (formerly "single user") for workloads needing features such as RDD APIs or GPU libraries
- **Autoscaling** — min/max workers; let the cluster grow under load
- **Auto termination** — stop idle all-purpose clusters (e.g. after 30–60 minutes)
- **Photon** — enable for SQL/DataFrame-heavy workloads
- **Spot/preemptible instances** — for workers in fault-tolerant jobs

## Compute policies

Admins define **compute policies** that restrict and default settings — maximum size, allowed
runtimes, required tags, auto termination — so teams can create compute without blowing the budget.

```json
{
  "autotermination_minutes": { "type": "range", "maxValue": 60, "defaultValue": 30 },
  "custom_tags.team": { "type": "fixed", "value": "analytics" },
  "spark_version": { "type": "regex", "pattern": "1[5-9]\\.[0-9]+\\.x-scala.*" }
}
```

## Billing

You pay **DBUs × price per DBU** for the SKU, plus, for classic compute, the cloud provider's VM
costs. Serverless prices include the infrastructure. Tags on compute and jobs flow into
`system.billing.usage` for chargeback.
