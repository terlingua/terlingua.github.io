---
title: Lakeflow Jobs
questions:
  - id: what
    prompt: What is a Lakeflow Job (formerly Databricks Workflows/Jobs)?
    options:
      - text: A single notebook that runs on a timer and nothing else
      - id: dag
        text: An orchestrated set of tasks with dependencies, triggers, retries, parameters and notifications
        correct: true
      - text: A cluster type
      - text: A Delta table that stores job logs
    explanation: Jobs orchestrate notebooks, Python scripts, wheels, SQL, pipelines, dbt and more as a DAG of tasks.
  - id: triggers
    prompt: Which trigger starts a job when new files land in a Unity Catalog volume or external location?
    options:
      - id: file-arrival
        text: File arrival trigger
        correct: true
      - text: Cron schedule
      - text: Manual run only
    explanation: Jobs also support scheduled, continuous and table-update triggers.
  - id: features
    prompt: Which are features of Lakeflow Jobs?
    type: multiple
    options:
      - text: Task values, which let one task pass small values to downstream tasks
        correct: true
      - text: Conditional (if/else) and for-each tasks
        correct: true
      - text: Repair runs that re-run only failed tasks and their dependents
        correct: true
      - text: Automatic rewriting of notebooks into Scala
    explanation: Control-flow tasks and repair runs make complex pipelines manageable.
---
## Learning objectives

- Build multi-task jobs with dependencies
- Use triggers, parameters and task values
- Make jobs reliable with retries, timeouts and alerts

## A multi-task job

```text
ingest_orders ──┐
                ├──▶ build_silver ──▶ refresh_gold_pipeline ──▶ notify_dashboard
ingest_customers┘
```

Each box is a **task**: a notebook, Python script, Python wheel, SQL query or file, Lakeflow
pipeline, dbt project, JAR, or another job.

## Parameters and task values

```python
# task: validate
run_date = dbutils.widgets.get("run_date")          # job parameter
row_count = spark.read.table("main.silver.orders").where(f"order_date = '{run_date}'").count()
dbutils.jobs.taskValues.set(key="row_count", value=row_count)
```

```python
# downstream task
rows = dbutils.jobs.taskValues.get(taskKey="validate", key="row_count", default=0)
```

Dynamic value references such as `{{job.start_time.iso_date}}` and `{{tasks.validate.values.row_count}}`
can be used in task parameters and conditions.

## Control flow

- **If/else condition** task — branch on a task value or parameter
- **For each** task — run a nested task for each item in a list, with configurable concurrency
- **Run if** — run a task when all/any dependencies succeed or fail (for clean-up or alerts)

## Triggers

| Trigger | Use |
|---------|-----|
| Scheduled (cron) | Nightly or hourly batches |
| File arrival | Process files as they land |
| Table update | Run when upstream tables change |
| Continuous | Keep a streaming job always running |

## Reliability

- Retries with back-off per task
- Timeouts and duration warnings
- E-mail, Slack, webhook notifications on failure
- **Repair run** re-executes failed tasks without redoing successful ones
- Run production jobs as a **service principal**, not a person

Job run history is queryable in `system.lakeflow` tables for SLA reporting.
