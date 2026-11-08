---
title: The lakehouse
questions:
  - id: definition
    prompt: What is a lakehouse?
    options:
      - text: A data warehouse appliance installed on premises
      - text: A data lake with no schema at all
      - text: A replacement for cloud object storage
      - id: combo
        text: An architecture that adds warehouse capabilities — transactions, schemas, governance, fast SQL — directly on top of open files in low-cost cloud object storage
        correct: true
    explanation: The lakehouse keeps data in open formats on object storage while providing the reliability and performance traditionally associated with warehouses.
  - id: founders
    prompt: Who founded Databricks?
    options:
      - text: The creators of Hadoop MapReduce at Yahoo
      - id: spark
        text: The original creators of Apache Spark from UC Berkeley's AMPLab
        correct: true
      - text: The original Snowflake team
    explanation: Databricks was founded in 2013 by the team behind Apache Spark. It also created Delta Lake and MLflow.
  - id: problems
    prompt: Which problems of a two-tier "data lake plus separate warehouse" architecture does the lakehouse aim to solve?
    type: multiple
    options:
      - text: Duplicate copies of data and complex ETL between lake and warehouse
        correct: true
      - text: Stale data in the warehouse
        correct: true
      - text: Separate governance models for BI and for data science
        correct: true
      - text: The need to store data anywhere at all
    explanation: One copy of data in open formats, governed once, serves BI, data engineering and machine learning.
---
## Learning objectives

- Explain the lakehouse architecture and its motivation
- Identify the open-source projects at the heart of Databricks
- Describe the main workloads the platform supports

## From warehouses and lakes to lakehouses

| Generation | Strengths | Weaknesses |
|------------|-----------|------------|
| Data warehouse | Fast SQL, transactions, governance | Expensive, proprietary formats, poor for unstructured data and ML |
| Data lake | Cheap, open files, any data type | No transactions, "data swamp", weak governance |
| Two-tier (lake + warehouse) | Combines both | Duplicate data, complex ETL, staleness |
| **Lakehouse** | Warehouse features on open lake storage | Requires a capable table format and engine |

The enabling technology is an **open table format** — Delta Lake on Databricks, with Apache
Iceberg interoperability — that adds a transaction log on top of Parquet files.

## The Databricks Data Intelligence Platform

Databricks was founded in 2013 by the creators of Apache Spark. Its platform runs on AWS, Azure
and Google Cloud and is built from open-source foundations:

| Project | Role |
|---------|------|
| Apache Spark | Distributed processing engine |
| Delta Lake | ACID table format on object storage |
| MLflow | Machine learning lifecycle and GenAI evaluation |
| Unity Catalog (open-sourced in 2024) | Governance for data and AI assets |
| Delta Sharing | Open protocol for sharing data |

## Workloads on one platform

- **Data engineering** — batch and streaming pipelines (Lakeflow)
- **Data warehousing and BI** — Databricks SQL, AI/BI dashboards, Genie
- **Data science and ML** — notebooks, MLflow, Model Serving
- **Generative AI** — vector search, foundation model APIs, agents
- **Governance** — Unity Catalog across all of the above

> Databricks renames products often. This book uses current names and mentions earlier names where
> you are likely to meet them in documentation and existing code.
