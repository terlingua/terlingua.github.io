---
title: Orchestration with Lakeflow Jobs
questions:
  - id: job
    prompt: What is a Lakeflow Job?
    options:
      - text: A single SQL query
      - text: A cluster type
      - id: dag
        text: An orchestrated workflow of tasks (notebooks, Python, SQL, pipelines, dbt, other jobs) with dependencies, schedules or triggers, retries and notifications
        correct: true
      - text: An HR posting
    explanation: Formerly called Databricks Workflows. It competes with Airflow, and many teams use both.
  - id: triggers
    prompt: Which triggers can start a job?
    type: multiple
    options:
      - id: schedule
        text: A cron-style schedule
        correct: true
      - id: file-arrival
        text: File arrival in a storage location
        correct: true
      - id: table-update
        text: Updates to source tables
        correct: true
      - id: continuous
        text: Continuous mode, restarting the job as soon as a run ends
        correct: true
    explanation: Event-based triggers avoid polling schedules that run when there is nothing to do.
  - id: task-values
    prompt: How can one task pass a small value to a downstream task?
    options:
      - id: task-values
        text: With task values (`dbutils.jobs.taskValues.set` / `get`) or job parameters
        correct: true
      - text: By writing to a global Python variable
      - text: It is impossible
      - text: By email
    explanation: Use task values for small metadata like row counts or flags; use tables for data.
  - id: retries
    prompt: A task fails due to a transient cloud error. What job feature helps?
    options:
      - text: Deleting the job
      - text: Running on a bigger cluster
      - text: Disabling alerts
      - id: retry
        text: Task-level retries with a delay, combined with idempotent task logic
        correct: true
    explanation: Retries without idempotency duplicate data. The two go together.
  - id: run-as
    prompt: Who should a production job run as?
    options:
      - text: The engineer who created it
      - id: sp
        text: A service principal with only the privileges the job needs
        correct: true
      - text: An account admin
      - text: Anonymous
    explanation: Jobs owned by people break when those people leave and usually run with excessive privileges.
  - id: airflow
    prompt: When might a team keep Apache Airflow instead of Lakeflow Jobs?
    options:
      - text: Never; Lakeflow Jobs replaces Airflow everywhere
      - id: cross-system
        text: When orchestration spans many systems outside Databricks and the team already operates Airflow; Lakeflow Jobs is strongest for work inside Databricks
        correct: true
      - text: Only on Google Cloud
      - text: When they use SQL
    explanation: Airflow can trigger Databricks jobs. Avoid two orchestrators competing for the same responsibility.
  - id: repair
    prompt: What does "repair run" do?
    options:
      - text: Fixes code bugs automatically
      - text: Restores a deleted job
      - id: rerun-failed
        text: Re-runs only failed tasks (and their dependents) of a job run, rather than the whole job
        correct: true
      - text: Restarts the workspace
    explanation: This saves time and cost on long DAGs where an early task succeeded.
  - id: for-each
    prompt: What is a For each task used for?
    options:
      - text: Looping through notebook cells
      - text: Sending an email per row
      - id: fanout
        text: Running a nested task once per item in a list (for example per country or per table), with configurable concurrency
        correct: true
      - text: Iterating a Spark DataFrame row by row
    explanation: Fan-out patterns used to need custom code; For each makes them declarative.
  - id: observability
    prompt: Where can you analyse job run history across the account?
    options:
      - id: lakeflow-system
        text: "In system tables such as `system.lakeflow.job_run_timeline` and `system.lakeflow.jobs`"
        correct: true
      - text: Only in the job UI, one job at a time
      - text: In the Spark UI
      - text: Not possible
    explanation: Combine with billing tables to see the cost per job and spot regressions.
  - id: serverless-jobs
    prompt: What changes when you run jobs on serverless compute?
    options:
      - text: Nothing at all
      - text: Jobs cannot use Python
      - text: They must be rewritten in SQL
      - id: no-config
        text: No cluster configuration or start-up wait; Databricks chooses and scales infrastructure, and you can choose between a cost-optimised and a performance-optimised mode
        correct: true
    explanation: Libraries are installed via environments rather than init scripts. Some legacy configurations do not carry over.
---
## Learning objectives

- Build multi-task jobs with dependencies and triggers
- Make jobs reliable with retries, idempotency and service principals
- Monitor runs with system tables

## A job as code (Databricks Asset Bundles YAML)

```yaml
resources:
  jobs:
    daily_sales:
      name: daily-sales
      schedule:
        quartz_cron_expression: "0 0 6 * * ?"
        timezone_id: UTC
      tasks:
        - task_key: ingest
          pipeline_task:
            pipeline_id: ${resources.pipelines.sales.id}
        - task_key: report
          depends_on: [{ task_key: ingest }]
          notebook_task:
            notebook_path: ../src/report.ipynb
          max_retries: 2
      run_as:
        service_principal_name: 6f1c...-sp
```

Keep jobs in source control and deploy them with the CLI from CI — not by clicking in production.

## Sources

- Databricks docs, "Lakeflow Jobs" — <https://docs.databricks.com/aws/en/jobs/>
- Databricks docs, "Jobs system tables" — <https://docs.databricks.com/aws/en/admin/system-tables/jobs>
- Databricks docs, "Asset Bundles" — <https://docs.databricks.com/aws/en/dev-tools/bundles/>
