---
title: A guided tour of the workspace
questions:
  - id: sidebar
    prompt: Which areas appear in the workspace sidebar?
    type: multiple
    options:
      - id: workspace-browser
        text: Workspace (files, notebooks, Git folders)
        correct: true
      - id: catalog
        text: Catalog (Catalog Explorer)
        correct: true
      - id: jobs-pipelines
        text: Jobs and pipelines
        correct: true
      - id: compute
        text: Compute, SQL warehouses and serving
        correct: true
    explanation: The layout changes regularly, and personas (SQL, data engineering, ML) change what is shown first.
  - id: catalog-explorer
    prompt: What can you do in Catalog Explorer?
    options:
      - id: explore
        text: Browse catalogs and tables, see schemas, samples, lineage, history, permissions and comments, and manage grants
        correct: true
      - text: Write Spark Scala code
      - text: Configure VPN settings
      - text: Edit cloud IAM roles
    explanation: It is the UI for Unity Catalog.
  - id: notebooks
    prompt: Which statements about Databricks notebooks are accurate?
    type: multiple
    options:
      - id: multi-lang
        text: Cells can mix languages with magic commands such as `%sql`, `%python`, `%md`
        correct: true
      - id: ipynb
        text: Notebooks can be stored in the Jupyter `.ipynb` format
        correct: true
      - id: collab
        text: Multiple users can edit simultaneously and comment
        correct: true
      - text: Notebooks cannot be scheduled
    explanation: Notebooks run as job tasks. For larger code bases, put logic in Python modules and keep notebooks thin.
  - id: notebook-pitfalls
    prompt: Which are common pitfalls of notebook-centric development?
    type: multiple
    options:
      - id: hidden-state
        text: Hidden state from running cells out of order
        correct: true
      - id: no-tests
        text: Logic that is hard to unit test and review
        correct: true
      - id: copy-paste
        text: Copy-pasted code across many notebooks
        correct: true
      - text: Notebooks cannot read tables
    explanation: Notebooks are excellent for exploration; production code benefits from modules, tests and CI.
  - id: git-folders
    prompt: What are Git folders (formerly Repos)?
    options:
      - text: A Databricks-hosted Git server
      - id: git
        text: Workspace folders linked to a remote Git repository (GitHub, Azure DevOps, GitLab and others) supporting clone, branch, commit, pull and push
        correct: true
      - text: Backup folders
      - text: Shared drives
    explanation: Combine with CI/CD and Asset Bundles; avoid editing production directly.
  - id: assistant
    prompt: What is the Databricks Assistant (and Genie Code, announced 2026)?
    options:
      - text: A human support engineer
      - text: A cluster autoscaler
      - text: A billing calculator
      - id: ai-assistant
        text: An AI coding and data assistant in notebooks, the SQL editor and other surfaces that uses Unity Catalog metadata as context to generate, explain and fix code
        correct: true
    explanation: Treat generated code like a junior colleague's pull request — review it, especially joins and filters.
  - id: dbutils
    prompt: What is `dbutils` used for?
    options:
      - text: Running SQL warehouses
      - text: Creating dashboards
      - id: utilities
        text: Notebook utilities — working with files, secrets (`dbutils.secrets.get`), widgets, task values and notebook workflows
        correct: true
      - text: Training models
    explanation: Prefer the Databricks SDK for automation outside notebooks; `dbutils` is notebook-oriented.
  - id: secrets
    prompt: Where should credentials for external systems be stored?
    options:
      - text: In notebook cells as plain text
      - text: In a Git repository
      - id: secret-scopes
        text: In secret scopes or Unity Catalog connections/service credentials, read at runtime; values are redacted in notebook output
        correct: true
      - text: In table comments
    explanation: Redaction is best effort — a determined user can still exfiltrate a secret they can read. Grant secret access narrowly.
  - id: sdk-cli
    prompt: Which tools automate Databricks outside the UI?
    type: multiple
    options:
      - id: cli
        text: The Databricks CLI
        correct: true
      - id: sdk
        text: Databricks SDKs (Python, Java, Go and others) and REST APIs
        correct: true
      - id: terraform
        text: The Databricks Terraform provider
        correct: true
      - text: Only the UI is supported
    explanation: Infrastructure (workspaces, catalogs, permissions) is usually Terraform; project assets are usually bundles.
---
## Learning objectives

- Find your way around the workspace
- Use notebooks, Git folders, the Assistant and secrets well
- Know the automation options

## A first notebook

```python
df = spark.table("samples.nyctaxi.trips")
display(df.groupBy("pickup_zip").count().orderBy("count", ascending=False).limit(10))
```

```sql
%sql
SELECT date_trunc('HOUR', tpep_pickup_datetime) AS hour, AVG(fare_amount) AS avg_fare
FROM samples.nyctaxi.trips
GROUP BY ALL ORDER BY hour;
```

The `samples` catalog is available in most workspaces for practice.

## Sources

- Databricks docs, "Navigate the workspace" — <https://docs.databricks.com/aws/en/workspace/>
- Databricks docs, "Git folders" — <https://docs.databricks.com/aws/en/repos/>
- Databricks docs, "Databricks Assistant" — <https://docs.databricks.com/aws/en/notebooks/databricks-assistant-faq>
- Databricks docs, "Secret management" — <https://docs.databricks.com/aws/en/security/secrets/>
