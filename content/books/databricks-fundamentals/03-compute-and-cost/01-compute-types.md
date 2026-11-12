---
title: Compute types
questions:
  - id: all-purpose
    prompt: What is all-purpose (interactive) compute intended for?
    options:
      - text: Running scheduled production jobs at lowest cost
      - text: Serving models
      - text: Hosting web apps
      - id: interactive
        text: Interactive development and exploration in notebooks, shared by users, with a higher DBU rate than job compute
        correct: true
    explanation: Running production jobs on all-purpose clusters is a common and expensive mistake.
  - id: job-compute
    prompt: Why use job compute for scheduled workloads?
    options:
      - id: cheaper-isolated
        text: It is created for the run and terminated afterwards, billed at a lower DBU rate, and isolates the job from other users' work
        correct: true
      - text: It has more features than all-purpose compute
      - text: It never fails
      - text: It is free
    explanation: Serverless jobs are the default recommendation where available, removing even the cluster configuration.
  - id: sql-warehouse
    prompt: What are SQL warehouses optimised for?
    options:
      - text: Training deep learning models
      - id: sql
        text: SQL queries from Databricks SQL, dashboards, Genie and BI tools via JDBC/ODBC, with Photon and query-level concurrency handling
        correct: true
      - text: Running Scala notebooks
      - text: Storing data
    explanation: Warehouse types are serverless, pro and classic. Serverless starts in seconds and is the default recommendation.
  - id: autoscale
    prompt: What does cluster autoscaling do?
    options:
      - text: Changes the VM type automatically
      - text: Increases DBU rates during peaks
      - id: workers
        text: Adds or removes worker nodes between a minimum and maximum based on load
        correct: true
      - text: Duplicates the cluster in another region
    explanation: Autoscaling helps variable workloads but can scale up slowly on classic compute and does not fix skewed or inefficient code.
  - id: auto-termination
    prompt: An all-purpose cluster has auto-termination disabled and a developer goes on holiday. What happens?
    options:
      - text: Databricks pauses it automatically after an hour
      - text: Nothing is billed while idle
      - text: Spark shuts it down when no notebooks are attached
      - id: bills
        text: It keeps running and billing (DBUs plus VMs) until someone stops it
        correct: true
    explanation: Set auto-termination (for example 30–60 minutes) and enforce it with compute policies. Idle clusters are a classic source of surprise bills.
  - id: policies
    prompt: What are compute policies used for?
    type: multiple
    options:
      - id: limit-size
        text: Limiting instance types, cluster size and DBU spend per user or group
        correct: true
      - id: enforce-tags
        text: Enforcing tags for cost attribution
        correct: true
      - id: enforce-termination
        text: Enforcing auto-termination and runtime versions
        correct: true
      - text: Writing Spark code for users
    explanation: Policies let admins give users self-service compute without unlimited spend.
  - id: access-modes
    prompt: Which access modes exist for Unity Catalog-enabled classic compute?
    type: multiple
    options:
      - id: standard
        text: Standard (formerly shared) — multiple users, with isolation and some language/feature limits
        correct: true
      - id: dedicated
        text: Dedicated (formerly single user) — assigned to one user or group
        correct: true
      - text: No isolation shared, recommended for production
      - text: Anonymous, for public access
    explanation: "\"No isolation shared\" is a legacy mode that does not enforce Unity Catalog. The names changed in 2025, which confuses older tutorials."
  - id: driver-workers
    prompt: In a Spark cluster, what does the driver do?
    options:
      - id: driver
        text: Runs the main program, builds the execution plan and schedules tasks on executors running on worker nodes
        correct: true
      - text: Stores all the data
      - text: Only handles networking
      - text: Runs the web UI for Databricks
    explanation: Collecting large results to the driver (`collect()`, `toPandas()` on big data) is a common cause of out-of-memory failures.
  - id: single-node
    prompt: When is a single-node cluster appropriate?
    options:
      - text: Never; Spark requires workers
      - text: For petabyte-scale ETL
      - id: small
        text: For small data, pandas/scikit-learn work, or development, where distributed overhead brings no benefit
        correct: true
      - text: Only for SQL warehouses
    explanation: Distributed compute is not automatically faster. Many ML workloads train on a single (possibly GPU) node.
  - id: serverless-tradeoffs
    prompt: Which are genuine trade-offs of serverless compute?
    type: multiple
    options:
      - id: less-control
        text: Less control over instance types, Spark configuration and libraries
        correct: true
      - id: different-network
        text: Compute runs in Databricks' account, which changes networking and some security reviews
        correct: true
      - id: price-model
        text: Higher DBU rates (infrastructure included) that can cost more for steady, well-tuned workloads
        correct: true
      - text: It cannot read Delta tables
    explanation: Serverless removes idle time and tuning work, which often lowers total cost for spiky workloads. For steady, heavily tuned jobs, classic compute can be cheaper. Measure.
  - id: pools
    prompt: What are instance pools for?
    options:
      - text: Sharing notebooks
      - text: Swimming
      - text: Storing tables
      - id: warm
        text: Keeping idle, ready VMs to reduce classic cluster start-up time, at the cost of paying the cloud provider for the idle instances
        correct: true
    explanation: Pools reduce start-up time but idle pool VMs cost cloud money (no DBUs). Serverless largely removes the need for them.
  - id: gpu
    prompt: Which statement about GPU compute on Databricks is accurate?
    options:
      - id: gpu-ml
        text: GPU instance types and serverless GPU compute are available for deep learning and fine-tuning; most ETL and SQL do not benefit from GPUs
        correct: true
      - text: All Spark SQL runs on GPUs by default
      - text: GPUs are free on serverless
      - text: GPUs are required for Photon
    explanation: GPU capacity can be scarce in some regions. Check quotas and availability before planning.
---
## Learning objectives

- Choose between all-purpose, job, SQL warehouse and serverless compute
- Understand drivers, workers, autoscaling and access modes
- Avoid the most common compute cost mistakes

## Choosing compute

| Workload | Recommended compute |
|---|---|
| Notebook exploration | Serverless notebooks, or all-purpose with auto-termination |
| Scheduled ETL | Serverless jobs, or job compute |
| Declarative pipelines | Serverless pipelines |
| BI and SQL | Serverless SQL warehouse |
| Deep learning | GPU clusters or serverless GPU |
| Model and agent serving | Model Serving endpoints |

## A cluster definition

```json
{
  "cluster_name": "etl-dev",
  "spark_version": "17.3.x-scala2.13",
  "node_type_id": "i3.xlarge",
  "autoscale": { "min_workers": 1, "max_workers": 4 },
  "autotermination_minutes": 30,
  "data_security_mode": "SINGLE_USER",
  "custom_tags": { "team": "finance", "env": "dev" }
}
```

Runtime and node names vary by cloud and change over time; take them from your workspace.

## Sources

- Databricks docs, "Compute" — <https://docs.databricks.com/aws/en/compute/>
- Databricks docs, "Access modes" — <https://docs.databricks.com/aws/en/compute/access-mode-limitations>
- Databricks docs, "Compute policies" — <https://docs.databricks.com/aws/en/admin/clusters/policies>
- Databricks docs, "Serverless compute" — <https://docs.databricks.com/aws/en/compute/serverless/>
