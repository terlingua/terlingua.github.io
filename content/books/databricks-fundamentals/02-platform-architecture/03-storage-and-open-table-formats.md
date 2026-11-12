---
title: Storage and open table formats
questions:
  - id: managed-external
    prompt: What is the difference between a managed and an external table in Unity Catalog?
    options:
      - text: Managed tables are in-memory only
      - text: External tables cannot be queried with SQL
      - id: lifecycle
        text: For managed tables Unity Catalog controls the storage location and lifecycle (dropping deletes data, after a retention period); external tables point at a path you manage, and dropping removes only metadata
        correct: true
      - text: There is no difference
    explanation: Databricks recommends managed tables because the platform can then optimise them automatically (predictive optimisation). External tables suit data shared with other systems that write to the same path.
  - id: volumes
    prompt: What are Unity Catalog volumes for?
    options:
      - text: Measuring audio loudness
      - id: files
        text: Governing non-tabular files — PDFs, images, CSV drops, model artefacts — under the same catalog.schema namespace and permissions as tables
        correct: true
      - text: Storing table statistics
      - text: Replacing Delta tables
    explanation: "Paths look like `/Volumes/catalog/schema/volume/path/file.pdf`. Volumes replace the old practice of using DBFS mounts."
  - id: dbfs
    prompt: What is the status of DBFS root and mounts?
    options:
      - text: Recommended for all new data
      - text: Required by Unity Catalog
      - id: legacy
        text: Legacy — Databricks recommends Unity Catalog volumes and external locations instead, because mounts bypass Unity Catalog governance
        correct: true
      - text: Deleted from all workspaces in 2023
    explanation: Mounts gave every user of a cluster the mount's credentials. That is the opposite of fine-grained governance.
  - id: external-location
    prompt: What is an external location?
    options:
      - id: el
        text: A Unity Catalog object pairing a cloud storage path with a storage credential, so access to that path is governed by grants
        correct: true
      - text: A table in another workspace
      - text: A VPN endpoint
      - text: A user's home directory
    explanation: Storage credentials wrap an IAM role, managed identity or service account. Users never see the underlying cloud keys.
  - id: uniform
    prompt: What does Delta Lake UniForm do?
    options:
      - text: Converts Delta tables to CSV
      - text: Enforces a uniform schema across all tables
      - text: Merges all tables into one
      - id: uniform
        text: Generates Iceberg (and historically Hudi) metadata for a Delta table so Iceberg clients can read the same Parquet files without copying
        correct: true
    explanation: The data files are shared; only metadata is written twice. Check feature compatibility, because not every Delta feature is representable in Iceberg.
  - id: iceberg-managed
    prompt: Which statements about Apache Iceberg on Databricks are accurate as of 2026?
    type: multiple
    options:
      - id: managed-iceberg
        text: Unity Catalog supports managed Iceberg tables
        correct: true
      - id: rest-catalog
        text: Unity Catalog exposes an Iceberg REST catalog interface so external engines can read tables
        correct: true
      - text: Iceberg is unsupported and must be converted to Delta first
      - id: v3
        text: Databricks announced support for Iceberg v3 features at Data + AI Summit 2026
        correct: true
    explanation: The format war has become a format truce — both are supported. Verify which operations external engines can perform (read versus write) before designing around them.
  - id: delta-log
    prompt: What is in a Delta table's `_delta_log` directory?
    options:
      - text: Error logs from failed jobs
      - id: commits
        text: Ordered JSON commit files describing added and removed data files and metadata changes, plus periodic Parquet checkpoints
        correct: true
      - text: User passwords
      - text: A copy of all data
    explanation: Readers reconstruct the current state from the latest checkpoint plus subsequent JSON commits.
  - id: vacuum
    prompt: What does `VACUUM` do to a Delta table?
    options:
      - text: Compacts small files
      - text: Deletes rows older than 7 days
      - id: remove-unreferenced
        text: Permanently deletes data files no longer referenced by the table and older than the retention threshold (7 days by default), limiting time travel
        correct: true
      - text: Rebuilds statistics
    explanation: "Deleting rows with `DELETE` only marks files as removed; `VACUUM` is what physically removes them. This matters for GDPR erasure: deleted data persists until vacuumed."
  - id: gdpr
    prompt: A customer exercises a GDPR right to erasure. You run `DELETE FROM customers WHERE id = 42`. Is the data gone?
    options:
      - text: Yes, immediately and everywhere
      - text: No, Delta cannot delete rows
      - text: Yes, if the table is managed
      - id: not-yet
        text: Not yet — older files remain readable via time travel until `VACUUM` removes them after the retention period; copies in other tables, logs and backups also need handling
        correct: true
    explanation: Deletion vectors make it even more indirect. Erasure is a process across all copies, not a single statement.
  - id: liquid
    prompt: What is liquid clustering?
    options:
      - id: liquid
        text: A data layout technique that incrementally clusters data by chosen keys, replacing static Hive-style partitioning and Z-ORDER for most new tables, with keys changeable later
        correct: true
      - text: A way to cool data centres
      - text: Streaming replication between regions
      - text: An autoscaling policy for clusters
    explanation: Over-partitioning (for example by a high-cardinality column) creates many small files; liquid clustering avoids that trap. Automatic liquid clustering can choose keys for managed tables.
  - id: own-storage
    prompt: Why does it matter that data files live in your own cloud storage account in the classic model?
    options:
      - text: It makes queries faster
      - id: control
        text: You retain physical control of the data, can apply your own storage policies and keys, and other engines can read it; leaving the vendor does not require exporting it
        correct: true
      - text: It removes the need for backups
      - text: It is required by Spark
    explanation: Default storage for some newer serverless workspaces is Databricks-managed. Know which model your workspace uses.
---
## Learning objectives

- Distinguish managed tables, external tables and volumes
- Explain external locations and storage credentials
- Understand Delta's log, `VACUUM`, liquid clustering and Iceberg interoperability

## Storage objects in Unity Catalog

| Object | Holds | Governed by |
|---|---|---|
| Managed table | Delta (or Iceberg) data in UC-managed storage | Table grants |
| External table | Data at a path you manage | Table grants + external location |
| Volume | Arbitrary files | Volume grants (`READ VOLUME`, `WRITE VOLUME`) |
| External location | A path + credential | `READ FILES`, `CREATE EXTERNAL TABLE`, … |

```sql
CREATE TABLE main.sales.orders (
  order_id BIGINT, customer_id BIGINT, amount DECIMAL(12,2), order_ts TIMESTAMP
) CLUSTER BY (order_ts);

DESCRIBE HISTORY main.sales.orders;
SELECT * FROM main.sales.orders VERSION AS OF 3;
```

## Maintenance you cannot ignore

- **OPTIMIZE** compacts small files
- **VACUUM** removes unreferenced files (and ends time travel past the threshold)
- **ANALYZE** collects statistics
- **Predictive optimisation** runs these automatically on managed tables — billed as serverless usage

## Sources

- Databricks docs, "Managed versus external tables" — <https://docs.databricks.com/aws/en/tables/>
- Databricks docs, "Volumes" — <https://docs.databricks.com/aws/en/volumes/>
- Delta Lake, "Remove files no longer referenced (VACUUM)" — <https://docs.delta.io/latest/delta-utility.html>
- Databricks docs, "Liquid clustering" — <https://docs.databricks.com/aws/en/delta/clustering>
- Databricks docs, "UniForm" — <https://docs.databricks.com/aws/en/delta/uniform>
