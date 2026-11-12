---
title: Cumulative review
questions:
  - id: r-lakehouse
    prompt: Which single feature most distinguishes a lakehouse from a plain data lake?
    options:
      - text: Cheap storage
      - text: Support for Python
      - id: txn
        text: A transactional metadata layer providing ACID tables over open files
        correct: true
      - text: Dashboards
    explanation: Everything else — time travel, MERGE, schema enforcement — follows from the transaction log.
  - id: r-planes
    prompt: Where do classic cluster VMs run, and where does serverless run?
    options:
      - id: classic-yours
        text: Classic in your cloud account; serverless in Databricks' account in the same region
        correct: true
      - text: Both in the control plane
      - text: Both in your account
      - text: Classic on-premises; serverless in your account
    explanation: This affects billing (two bills versus one) and networking.
  - id: r-cost
    prompt: Which is the most common avoidable cost mistake?
    options:
      - text: Using Delta Lake
      - text: Using Unity Catalog
      - text: Writing comments
      - id: interactive-prod
        text: Running production jobs on all-purpose clusters without auto-termination
        correct: true
    explanation: Use job or serverless compute and enforce policies.
  - id: r-uc
    prompt: "A user has `SELECT` on `main.sales.orders` but gets permission denied. What is likely missing?"
    options:
      - id: use
        text: "`USE CATALOG` on `main` and/or `USE SCHEMA` on `main.sales`"
        correct: true
      - text: "`MODIFY` on the table"
      - text: Workspace admin rights
      - text: A bigger cluster
    explanation: USE privileges gate traversal of the hierarchy.
  - id: r-bypass
    prompt: What undermines all Unity Catalog fine-grained controls?
    options:
      - text: Using SQL warehouses
      - text: Using managed tables
      - id: storage
        text: Users or services with direct cloud-storage access to the table files
        correct: true
      - text: Adding comments
    explanation: Lock down storage so only Unity Catalog credentials reach it.
  - id: r-medallion
    prompt: Which statement about the medallion architecture is true?
    options:
      - text: It is required by Delta Lake
      - id: convention
        text: It is a convention for quality stages; it does not replace data modelling and should not create copies that add no value
        correct: true
      - text: It requires streaming
      - text: Gold tables must be materialized views
    explanation: Use as many layers as the data needs.
  - id: r-genie
    prompt: What most improves the reliability of Genie answers?
    options:
      - text: Adding more tables
      - text: Using a larger warehouse
      - text: Turning off permissions
      - id: curation
        text: Curated metadata, example queries, governed metrics and benchmarks
        correct: true
    explanation: Text-to-SQL accuracy is a function of context quality.
  - id: r-vacuum
    prompt: After `DELETE`, when are deleted rows physically gone?
    options:
      - text: Immediately
      - id: vacuum
        text: After `VACUUM` removes the old files beyond the retention period
        correct: true
      - text: Never
      - text: After the next OPTIMIZE only
    explanation: Relevant to GDPR erasure and storage cost.
  - id: r-sharing
    prompt: Which statement about Delta Sharing is correct?
    options:
      - id: open
        text: It is an open protocol that lets recipients on or off Databricks read live data without copies, but cross-region reads incur egress
        correct: true
      - text: It copies tables into the recipient's account
      - text: Recipients must be Databricks customers
      - text: It is free of any cost
    explanation: Zero-copy is not zero-cost.
  - id: r-oltp
    prompt: Why does Lakebase exist alongside Delta tables?
    options:
      - text: Delta is deprecated
      - text: Postgres is better for analytics
      - text: To store notebooks
      - id: oltp
        text: Applications and agents need low-latency transactional storage that object-storage tables cannot provide
        correct: true
    explanation: Analytical and transactional workloads still need different engines.
  - id: r-names
    prompt: Match old names to new. Which mappings are correct?
    type: multiple
    options:
      - id: dlt
        text: Delta Live Tables → Lakeflow Spark Declarative Pipelines
        correct: true
      - id: workflows
        text: Workflows → Lakeflow Jobs
        correct: true
      - id: repos
        text: Repos → Git folders
        correct: true
      - text: Delta Lake → Lakebase
    explanation: Lakebase is a Postgres database, not a renamed Delta Lake. Renames are frequent; learn concepts, not labels.
  - id: r-photon
    prompt: Which workload benefits most from Photon?
    options:
      - text: Row-by-row Python UDFs
      - text: RDD-based legacy code
      - id: sql-scan
        text: Large SQL scans, joins and aggregations
        correct: true
      - text: Model training with PyTorch
    explanation: Photon vectorises SQL/DataFrame operators; it cannot accelerate Python UDFs.
  - id: r-serverless
    prompt: When can classic compute be cheaper than serverless?
    options:
      - id: steady
        text: For steady, long-running, well-tuned workloads where idle time is already minimal
        correct: true
      - text: Never
      - text: For spiky interactive use
      - text: For dashboards with infrequent use
    explanation: Serverless wins on spiky workloads by removing idle time and start-up.
  - id: r-mlflow
    prompt: Which MLflow concept lets you promote a model without changing serving code?
    options:
      - text: Experiments
      - text: Runs
      - text: Artifacts
      - id: alias
        text: Registered model aliases such as `@champion`
        correct: true
    explanation: Move the alias to the new version.
  - id: r-eval
    prompt: What is the best evidence that a GenAI feature works for you?
    options:
      - id: eval
        text: Measured accuracy and cost on your own labelled examples
        correct: true
      - text: A vendor benchmark
      - text: A keynote demo
      - text: The model's parameter count
    explanation: Evaluation on your data is the only evidence that transfers to your use case.
---
## Learning objectives

- Consolidate the book's main ideas
- Practise applying them to scenarios

## Ten ideas to keep

1. A lakehouse is a transaction log over open files, plus governance and engines.
2. Control plane coordinates; compute plane processes; serverless runs in Databricks' account.
3. Cost = DBUs × rate (+ cloud infrastructure for classic). Tag and measure.
4. Job and serverless compute for production; auto-termination everywhere.
5. Lakeflow: Connect to ingest, declarative pipelines to transform, Jobs to orchestrate.
6. Medallion is a convention, not a data model.
7. Unity Catalog: `catalog.schema.object`, inheritance, USE privileges, filters, masks, ABAC.
8. Lock down storage, or catalog governance is bypassable.
9. Genie and agents are only as good as metadata, metrics and evaluation.
10. Open formats reduce lock-in; platform features add it. Know your exit cost.

## Sources

- See the sources listed in each section of this book.
