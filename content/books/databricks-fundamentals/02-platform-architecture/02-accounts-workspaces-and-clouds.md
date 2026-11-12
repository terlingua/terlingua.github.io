---
title: Accounts, workspaces and clouds
questions:
  - id: hierarchy
    prompt: What is the top-level container in Databricks?
    options:
      - text: The workspace
      - text: The cluster
      - id: account
        text: The account, which contains workspaces, metastores, identities and billing
        correct: true
      - text: The catalog
    explanation: Account admins manage workspaces, Unity Catalog metastores, users and groups, and usage. Workspace admins manage one workspace.
  - id: workspace
    prompt: What is a workspace?
    options:
      - id: ws
        text: A deployment in one cloud region with its own URL, where users work with notebooks, jobs, compute, SQL and other assets
        correct: true
      - text: A single notebook
      - text: A Delta table
      - text: A Git branch
    explanation: Organisations commonly separate dev, staging and prod workspaces, or separate business units.
  - id: metastore-region
    prompt: How do Unity Catalog metastores relate to regions and workspaces?
    options:
      - text: One metastore per workspace, always
      - text: One global metastore for all clouds
      - text: Metastores are optional and rarely used
      - id: one-per-region
        text: Typically one metastore per region, attached to many workspaces in that region, so data and permissions are shared across them
        correct: true
    explanation: Cross-region access uses Delta Sharing or catalog federation rather than one metastore spanning regions.
  - id: clouds
    prompt: On which clouds does Databricks run as a managed service?
    type: multiple
    options:
      - id: aws
        text: AWS
        correct: true
      - id: azure
        text: Microsoft Azure (as Azure Databricks, a first-party Azure service)
        correct: true
      - id: gcp
        text: Google Cloud
        correct: true
      - text: Only on Databricks-owned data centres
    explanation: Feature availability and release timing differ between clouds and regions. Always check the docs for your cloud.
  - id: azure-first-party
    prompt: What is distinctive about Azure Databricks?
    options:
      - text: It is a different product with no Spark
      - id: first-party
        text: It is sold and billed by Microsoft as a first-party Azure service, integrated with Entra ID and Azure billing
        correct: true
      - text: It does not support Unity Catalog
      - text: It is free with Azure subscriptions
    explanation: Microsoft also sells Fabric, which overlaps with Databricks — a relationship that is both partnership and competition.
  - id: identities
    prompt: Which identity types does Databricks support?
    type: multiple
    options:
      - id: users
        text: Users
        correct: true
      - id: groups
        text: Groups
        correct: true
      - id: sps
        text: Service principals for automation
        correct: true
      - text: Anonymous public users with write access
    explanation: Production jobs should run as service principals, not as individual users who may leave the company.
  - id: scim
    prompt: How are identities typically provisioned in enterprises?
    options:
      - text: Typed manually into each workspace
      - text: Shared accounts with one password
      - id: scim-sso
        text: Synced from the identity provider (Entra ID, Okta) via SCIM or automatic identity management, with single sign-on
        correct: true
      - text: Through notebook code
    explanation: Manage groups in the IdP and grant privileges to groups in Databricks, not to individuals.
  - id: tiers
    prompt: What has happened to the Databricks Standard pricing tier?
    options:
      - text: It became the only tier
      - id: retired
        text: It reached end of life on AWS and Google Cloud in 2025, and Azure announced its retirement by October 2026, leaving Premium (and Enterprise on AWS) as the tiers
        correct: true
      - text: It was renamed Free Edition
      - text: Nothing; it is still recommended for production
    explanation: Unity Catalog and most governance features require Premium. Check your cloud's notice for exact dates.
  - id: free-edition
    prompt: What is Databricks Free Edition?
    options:
      - text: A 14-day trial of the full Enterprise tier
      - text: The open-source Spark distribution
      - text: A free tier with unlimited production use
      - id: free
        text: A free, serverless-only workspace for learning and experimentation, launched in 2025, replacing Community Edition, with usage limits
        correct: true
    explanation: It is ideal for working through these books. Its quotas and limited compute mean it is not for production workloads.
  - id: workspace-sprawl
    prompt: What is a common anti-pattern with workspaces?
    options:
      - id: sprawl
        text: Creating one workspace per team or project without a plan, multiplying configuration, networking and admin effort
        correct: true
      - text: Using a separate production workspace
      - text: Using Unity Catalog across workspaces
      - text: Having an account admin
    explanation: With Unity Catalog, isolation is often better achieved with catalogs and workspace bindings than with many workspaces.
---
## Learning objectives

- Describe the account → workspace → compute hierarchy
- Know how metastores, identities and clouds fit together
- Understand the current pricing tiers and Free Edition

## The hierarchy

```text
Account
├── Identities (users, groups, service principals)  ← synced from IdP
├── Unity Catalog metastore (one per region)
│     └── catalogs → schemas → tables, views, volumes, functions, models
├── Workspace: prod-eastus   ── compute, notebooks, jobs, SQL warehouses, dashboards
├── Workspace: dev-eastus
└── Billing and usage (system tables)
```

## Practical guidance

- Separate environments (dev/test/prod) by workspace **and** catalog
- Bind production catalogs only to production workspaces
- Run production jobs as service principals
- Manage membership in your identity provider; grant to groups

## Sources

- Databricks docs, "Account and workspace" — <https://docs.databricks.com/aws/en/admin/>
- Databricks docs, "End of life for Standard tier workspaces" — <https://docs.databricks.com/aws/admin/account-settings/standard-tier>
- Azure retirement notice AZ502623 — <https://mc.merill.net/message/AZ502623>
- Databricks Free Edition — <https://docs.databricks.com/aws/en/getting-started/free-edition>
