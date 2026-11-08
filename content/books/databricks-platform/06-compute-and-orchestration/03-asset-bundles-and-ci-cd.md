---
title: Databricks Asset Bundles and CI/CD
questions:
  - id: what
    prompt: What is a Databricks Asset Bundle?
    options:
      - text: A ZIP archive of notebook outputs
      - id: iac
        text: A project format that defines jobs, pipelines and other resources as YAML alongside source code, deployable to multiple environments with the Databricks CLI
        correct: true
      - text: A compressed Delta table
      - text: A marketplace listing
    explanation: Bundles bring infrastructure-as-code and CI/CD practices to Databricks projects.
  - id: targets
    prompt: What are targets in `databricks.yml`?
    options:
      - text: Tables that a job writes to
      - id: environments
        text: Named deployment environments such as dev, staging and prod, each with its own workspace and overrides
        correct: true
      - text: Performance goals for queries
    explanation: "`databricks bundle deploy -t prod` deploys the same definitions with production settings."
  - id: commands
    prompt: Which Databricks CLI bundle commands exist?
    type: multiple
    options:
      - text: "`databricks bundle validate`"
        correct: true
      - text: "`databricks bundle deploy`"
        correct: true
      - text: "`databricks bundle run`"
        correct: true
      - text: "`databricks bundle compile-to-exe`"
    explanation: "`init`, `validate`, `deploy`, `run` and `destroy` cover the typical lifecycle."
---
## Learning objectives

- Define jobs and pipelines as code with Asset Bundles
- Deploy to multiple environments
- Build a CI/CD pipeline for Databricks

## A bundle

```yaml
# databricks.yml
bundle:
  name: sales_pipeline

include:
  - resources/*.yml

targets:
  dev:
    mode: development
    default: true
    workspace:
      host: https://adb-1111111111111111.11.azuredatabricks.net
  prod:
    mode: production
    workspace:
      host: https://adb-2222222222222222.22.azuredatabricks.net
    run_as:
      service_principal_name: 6a1b2c3d-0000-4000-8000-000000000000
```

```yaml
# resources/sales_job.yml
resources:
  jobs:
    sales_daily:
      name: sales-daily
      schedule:
        quartz_cron_expression: "0 0 3 * * ?"
        timezone_id: UTC
      tasks:
        - task_key: ingest
          notebook_task:
            notebook_path: ../src/ingest.ipynb
        - task_key: transform
          depends_on:
            - task_key: ingest
          python_wheel_task:
            package_name: sales
            entry_point: transform
          libraries:
            - whl: ../dist/*.whl
```

`mode: development` prefixes resource names with your user name and pauses schedules, so every
developer gets an isolated copy.

## Workflow

```bash
databricks bundle init            # start from a template
databricks bundle validate
databricks bundle deploy -t dev
databricks bundle run sales_daily -t dev
databricks bundle deploy -t prod  # from CI, authenticated as a service principal
```

## CI/CD

1. Pull request → run unit tests (`pytest` on pure Python transformation functions) and
   `databricks bundle validate`
2. Merge to main → `bundle deploy -t staging`, run integration tests against staging data
3. Release → `bundle deploy -t prod`

Authenticate CI with workload identity federation (OIDC) or OAuth for a service principal rather
than personal access tokens.

## Testing transformations

```python
# src/sales/transform.py
from pyspark.sql import DataFrame, functions as F

def completed_revenue(orders: DataFrame) -> DataFrame:
    return (orders.where(F.col("status") == "COMPLETED")
                  .groupBy("order_date").agg(F.sum("total").alias("revenue")))
```

Pure functions that take and return DataFrames can be unit tested locally with a small
`SparkSession` or with Databricks Connect.
