---
title: Lock-in and portability
questions:
  - id: what-portable
    prompt: Which assets move most easily off Databricks?
    type: multiple
    options:
      - id: tables
        text: Delta or Iceberg tables in your own storage
        correct: true
      - id: pyspark
        text: Plain PySpark and Spark SQL code
        correct: true
      - id: mlflow-models
        text: MLflow-format models
        correct: true
      - text: Genie spaces and AI/BI dashboards
    explanation: Open formats and open-source APIs move; product UIs and managed services do not.
  - id: least-portable
    prompt: Which is the least portable?
    options:
      - text: Parquet files
      - text: A Python module using only pandas
      - id: dbutils-heavy
        text: Notebooks heavy with `dbutils`, magic commands, widgets and Databricks-specific SQL functions, orchestrated by Lakeflow Jobs and permissions in Unity Catalog
        correct: true
      - text: A Delta table
    explanation: Isolate platform-specific calls behind small functions to reduce migration cost.
  - id: strategies
    prompt: Which strategies reduce lock-in without giving up the platform's benefits?
    type: multiple
    options:
      - id: modules
        text: Keep business logic in testable Python modules and SQL, with thin notebooks
        correct: true
      - id: iac
        text: Define infrastructure and assets as code (Terraform, bundles)
        correct: true
      - id: open-formats
        text: Keep data in open formats in storage you control
        correct: true
      - text: Avoid documentation so knowledge stays in the platform
    explanation: You will not be fully portable, and that is acceptable. Know your exit cost and keep it bounded.
  - id: exit-cost
    prompt: What is the most realistic measure of lock-in?
    options:
      - text: Whether the vendor says "open"
      - id: exit-cost
        text: The estimated time and cost to move your workloads, people's skills and governance to an alternative
        correct: true
      - text: The number of open-source projects the vendor contributes to
      - text: Contract length alone
    explanation: Every vendor claims openness. Estimate exit cost concretely, once a year.
  - id: open-core
    prompt: Databricks follows an "open core" pattern. What does that mean?
    options:
      - text: All of its products are open source
      - text: None of its products are open source
      - id: open-core
        text: Core technologies are open source (Spark, Delta Lake, MLflow, Unity Catalog OSS), while the hosted platform adds proprietary features that differentiate the paid product
        correct: true
      - text: The CPU cores are open hardware
    explanation: Open core is a business model with real benefits for users, but the commercial incentive is to keep the best features proprietary.
  - id: multi-cloud
    prompt: Does running Databricks on several clouds remove cloud lock-in?
    options:
      - id: partly
        text: Partly — the platform is consistent across clouds, but data gravity, networking, identity and egress still tie workloads to a cloud, and you now depend on Databricks instead
        correct: true
      - text: Yes, completely
      - text: No, Databricks only runs on AWS
      - text: Multi-cloud is always cheaper
    explanation: Multi-cloud often moves lock-in rather than removing it.
  - id: competitors
    prompt: Which are genuine competitors or alternatives for parts of the Databricks platform?
    type: multiple
    options:
      - id: snowflake
        text: Snowflake
        correct: true
      - id: fabric
        text: Microsoft Fabric
        correct: true
      - id: bigquery
        text: Google BigQuery and AWS analytics services
        correct: true
      - id: oss-stack
        text: Open-source stacks such as Spark, Trino, DuckDB, Iceberg and Airflow
        correct: true
    explanation: The right choice depends on workloads, skills, existing cloud commitments and cost.
---
## Learning objectives

- Distinguish portable from platform-specific assets
- Apply practical strategies to bound exit cost
- Understand the open-core business model

## A portability checklist

- [ ] Tables are in open formats in storage we control
- [ ] Logic is in modules with unit tests
- [ ] Infrastructure and jobs are defined as code
- [ ] Metric definitions are documented outside any one tool
- [ ] We know which preview features we depend on
- [ ] We could estimate a migration in weeks, not guess

## Sources

- Databricks, "Open source" — <https://www.databricks.com/product/open-source>
- Delta Lake (Linux Foundation) — <https://delta.io/>
- Unity Catalog OSS — <https://www.unitycatalog.io/>
