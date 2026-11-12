---
title: The Unity Catalog object model
questions:
  - id: namespace
    prompt: What is Unity Catalog's three-level namespace?
    options:
      - text: "`workspace.cluster.table`"
      - text: "`database.table.column`"
      - text: "`account.workspace.table`"
      - id: three-level
        text: "`catalog.schema.object`, for example `main.sales.orders`"
        correct: true
    explanation: The metastore sits above catalogs. Schemas are also called databases in older Spark terminology.
  - id: securables
    prompt: Which are securable objects in Unity Catalog?
    type: multiple
    options:
      - id: tables
        text: Tables and views
        correct: true
      - id: volumes
        text: Volumes
        correct: true
      - id: functions
        text: Functions and registered models
        correct: true
      - id: connections
        text: Connections, storage credentials and external locations
        correct: true
    explanation: One permission model spans data, files, code and AI assets. That breadth is its main selling point.
  - id: hive
    prompt: What was the governance situation before Unity Catalog?
    options:
      - id: hive
        text: Each workspace had its own Hive metastore with table ACLs that applied only in that workspace and could be bypassed via cluster credentials and mounts
        correct: true
      - text: There was no metadata at all
      - text: Unity Catalog has always existed
      - text: Governance was handled by Excel
    explanation: Migrating from the legacy `hive_metastore` to Unity Catalog is a significant project for older customers.
  - id: ownership
    prompt: Who owns a newly created table by default?
    options:
      - text: The metastore admin
      - text: Nobody
      - id: creator
        text: The principal that created it; owners have full privileges and can grant to others, so production objects should be owned by groups or service principals
        correct: true
      - text: All users
    explanation: Tables owned by individual employees become orphaned when they leave. Transfer ownership to a group.
  - id: inheritance
    prompt: "A group is granted `SELECT` on schema `main.sales`. What can its members read?"
    options:
      - text: Nothing, until each table is granted
      - id: inherit
        text: All current and future tables and views in that schema — privileges inherit down the hierarchy — provided they also have `USE CATALOG` on `main` and `USE SCHEMA` on `main.sales`
        correct: true
      - text: Only tables created before the grant
      - text: Every table in every catalog
    explanation: The USE privileges are a frequent source of "permission denied" confusion.
  - id: workspace-binding
    prompt: How do you prevent a production catalog from being accessed in a development workspace?
    options:
      - text: It is impossible
      - text: Delete the development workspace
      - text: Rename the catalog
      - id: binding
        text: Bind the catalog to specific workspaces (workspace-catalog binding)
        correct: true
    explanation: Bindings add environment isolation on top of user privileges.
  - id: oss-uc
    prompt: Which statement about open-source Unity Catalog is accurate?
    options:
      - id: oss
        text: Databricks open-sourced Unity Catalog in June 2024 under the Linux Foundation (LF AI & Data); the managed Databricks service includes features the open-source version does not
        correct: true
      - text: The open-source version is identical to the managed service
      - text: It is not open source
      - text: It only supports Iceberg
    explanation: Competing open catalogs include Apache Polaris (from Snowflake) and others. Catalog choice is the new battleground.
  - id: tags
    prompt: What are governed tags used for?
    options:
      - text: Billing only
      - text: Git versioning
      - id: classify
        text: Classifying objects and columns (for example `pii`, `confidential`) with controlled values, enabling discovery and attribute-based policies
        correct: true
      - text: Decorating dashboards
    explanation: Data classification can suggest tags automatically. Review its output; automatic classification produces false negatives.
  - id: comments
    prompt: Why invest time in table and column comments?
    type: multiple
    options:
      - id: discovery
        text: People can find and understand data
        correct: true
      - id: ai-context
        text: Genie, the Assistant and agents use metadata as context, improving accuracy
        correct: true
      - id: ai-generated
        text: AI-generated comments are a draft that a human should review
        correct: true
      - text: Comments speed up queries
    explanation: Metadata quality became an AI-quality issue. Auto-generated descriptions can be confidently wrong.
---
## Learning objectives

- Navigate the metastore → catalog → schema → object hierarchy
- Understand ownership, inheritance and USE privileges
- Know the legacy Hive metastore situation and open-source Unity Catalog

## The hierarchy

```text
Metastore (per region)
├── Catalog: main
│   ├── Schema: sales
│   │   ├── Table: orders
│   │   ├── View: orders_eu
│   │   ├── Volume: raw_files
│   │   ├── Function: mask_email
│   │   └── Model: churn
│   └── Schema: hr
├── Catalog: dev
├── Storage credential / External location
└── Connection (federation, MCP, HTTP)
```

## Grants

```sql
GRANT USE CATALOG ON CATALOG main TO `analysts`;
GRANT USE SCHEMA, SELECT ON SCHEMA main.sales TO `analysts`;
ALTER TABLE main.sales.orders OWNER TO `sales-data-owners`;
COMMENT ON COLUMN main.sales.orders.amount IS 'Order total in EUR including VAT';
```

## Sources

- Databricks docs, "What is Unity Catalog?" — <https://docs.databricks.com/aws/en/data-governance/unity-catalog/>
- Databricks docs, "Privileges and securable objects" — <https://docs.databricks.com/aws/en/data-governance/unity-catalog/manage-privileges/privileges>
- Unity Catalog OSS — <https://github.com/unitycatalog/unitycatalog>
- Apache Polaris — <https://polaris.apache.org/>
