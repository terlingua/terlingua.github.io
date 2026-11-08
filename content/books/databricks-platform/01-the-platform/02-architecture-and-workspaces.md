---
title: Architecture, accounts and workspaces
questions:
  - id: planes
    prompt: In Databricks' architecture, what runs in the control plane?
    options:
      - text: All of the customer's data files
      - text: Only the customer's virtual machines
      - text: The customer's on-premises databases
      - id: control
        text: Backend services managed by Databricks, such as the web application, notebook storage, job scheduling and cluster management
        correct: true
    explanation: The control plane is Databricks-managed. Data processing happens in the compute plane, and data lives in your cloud storage.
  - id: classic-vs-serverless
    prompt: Where do classic compute resources (clusters you configure) run?
    options:
      - id: customer
        text: In the customer's cloud account, in the classic compute plane
        correct: true
      - text: In Databricks' own cloud account
      - text: On the user's laptop
    explanation: Classic compute runs in your cloud subscription; serverless compute runs in a compute plane that Databricks manages on your behalf.
  - id: account-workspace
    prompt: Which statements about Databricks accounts and workspaces are true?
    type: multiple
    options:
      - text: An account can contain multiple workspaces
        correct: true
      - text: A Unity Catalog metastore is a regional container that can be attached to multiple workspaces in that region
        correct: true
      - text: Users and groups are typically managed at the account level and assigned to workspaces
        correct: true
      - text: Each workspace must use a different cloud provider
    explanation: Account-level identity and governance let many workspaces share users, groups and catalogs.
---
## Learning objectives

- Distinguish the control plane from the compute plane
- Understand accounts, workspaces and metastores
- Know where your data actually lives

## Two planes

```text
┌──────────────── Control plane (Databricks-managed) ────────────────┐
│  Web UI · Notebooks · Jobs scheduler · Cluster manager · Unity Catalog │
└───────────────────────────────┬────────────────────────────────────┘
                                │
     ┌──────────────────────────┴──────────────────────────┐
     │                                                     │
Classic compute plane                              Serverless compute plane
(your cloud account)                               (managed by Databricks)
  clusters, SQL warehouses                           serverless notebooks, jobs,
                                                     SQL warehouses, pipelines
     │                                                     │
     └─────────────── Your cloud object storage ───────────┘
                    (S3 · ADLS Gen2 · GCS)
```

Your table data stays in **your** object storage. Compute reads and writes it; the control plane
orchestrates.

## Accounts, workspaces and metastores

| Concept | Scope | Purpose |
|---------|-------|---------|
| Account | Organisation | Identity (users, groups, service principals), billing, metastores |
| Workspace | Deployment | Notebooks, jobs, compute, dashboards — a place where teams work |
| Metastore | Region | Top-level Unity Catalog container, attached to workspaces in that region |

A common pattern is separate workspaces for development, staging and production, all attached to
the same regional metastore, with catalogs per environment (`dev`, `prod`) controlling access.

## Identities

- **Users** — people, usually synced from an identity provider via SCIM
- **Groups** — the unit you grant privileges to
- **Service principals** — identities for jobs and CI/CD pipelines

Grant privileges to groups, not individuals, and run production jobs as service principals.
