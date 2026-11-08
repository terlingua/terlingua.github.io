---
title: The Unity Catalog object model
questions:
  - id: namespace
    prompt: What is the three-level namespace for a table in Unity Catalog?
    options:
      - id: three
        text: "`catalog.schema.table`"
        correct: true
      - text: "`workspace.database.table`"
      - text: "`account.catalog.table`"
      - text: "`metastore.table.column`"
    explanation: The metastore sits above catalogs; within it, every securable is addressed as catalog, schema, then object.
  - id: managed-external
    prompt: What happens to the data files when you drop a managed table in Unity Catalog?
    options:
      - text: The files always remain in place forever
      - id: managed-delete
        text: Unity Catalog manages the data, so the files are deleted (after a short recovery window in which `UNDROP` is possible)
        correct: true
      - text: The files are moved to a different cloud
    explanation: For external tables, dropping only removes metadata; the files stay in your external location.
  - id: securables
    prompt: Which are securable objects in Unity Catalog?
    type: multiple
    options:
      - text: Tables and views
        correct: true
      - text: Volumes, for non-tabular files
        correct: true
      - text: Functions and registered ML models
        correct: true
      - text: Storage credentials and external locations
        correct: true
    explanation: Unity Catalog governs data and AI assets, and the credentials used to reach cloud storage.
---
## Learning objectives

- Navigate the Unity Catalog hierarchy
- Distinguish managed and external tables and volumes
- Organise catalogs and schemas sensibly

## The hierarchy

```text
Metastore (one per region)
└── Catalog            e.g. prod, dev, sandbox
    └── Schema         e.g. sales, finance
        ├── Table / View / Materialized view
        ├── Volume     (files)
        ├── Function   (SQL/Python UDFs)
        └── Model      (MLflow registered model)
```

```sql
USE CATALOG prod;
USE SCHEMA sales;
SELECT * FROM orders;          -- resolves to prod.sales.orders
SELECT * FROM dev.sales.orders; -- fully qualified
```

## Managed vs external

| | Managed | External |
|-|---------|----------|
| Storage location | Chosen and managed by Unity Catalog | Path you specify in an external location |
| `DROP TABLE` | Deletes data (recoverable with `UNDROP` for a period) | Removes metadata only |
| Predictive optimization | Yes | No |
| Recommended for | Most new tables | Data shared with other tools that write directly |

## Reaching cloud storage

```sql
CREATE STORAGE CREDENTIAL raw_cred ...;             -- an IAM role / managed identity
CREATE EXTERNAL LOCATION raw_landing
  URL 's3://acme-raw/landing/' WITH (STORAGE CREDENTIAL raw_cred);
```

Users never handle cloud keys; Unity Catalog checks privileges and vends short-lived credentials.

## Volumes

```sql
CREATE VOLUME prod.raw.landing;
```

```python
df = spark.read.format("csv").option("header", True).load("/Volumes/prod/raw/landing/2026/11/")
```

Volumes govern files — CSVs, images, PDFs, model artefacts — with the same privileges as tables.

## Organising

A common layout: catalogs per environment or per business domain, schemas per subject area or per
medallion layer (`bronze`, `silver`, `gold`). The `hive_metastore` catalog exposes the legacy
per-workspace metastore for migration.
