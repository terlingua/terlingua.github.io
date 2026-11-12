---
title: Fine-grained access
questions:
  - id: row-filter
    prompt: What does a row filter do?
    options:
      - text: Removes duplicate rows physically
      - id: rf
        text: Applies a SQL function to a table so each query returns only rows the current user may see
        correct: true
      - text: Sorts rows
      - text: Deletes old rows
    explanation: The function typically checks group membership, for example `is_account_group_member('emea')`.
  - id: column-mask
    prompt: What does a column mask do?
    options:
      - text: Hides a column from the schema
      - text: Encrypts the column on disk
      - id: cm
        text: Replaces a column's value at query time based on a function — for example showing full emails to the privacy team and a masked value to others
        correct: true
      - text: Renames a column
    explanation: Masks apply at read time; the stored value is unchanged.
  - id: abac
    prompt: What does attribute-based access control (ABAC) with governed tags add?
    options:
      - text: Nothing beyond row filters
      - text: Multi-factor authentication
      - text: Faster queries
      - id: abac
        text: Policies defined once on tags (for example "mask any column tagged pii for non-privacy users") that apply automatically to every current and future object carrying the tag
        correct: true
    explanation: Per-table filters do not scale to thousands of tables; tag-based policies do. They are only as good as your tagging.
  - id: dynamic-view
    prompt: What is a dynamic view?
    options:
      - id: dv
        text: A view whose SQL uses functions like `current_user()` or `is_account_group_member()` to filter or mask data per user — the older approach to fine-grained access
        correct: true
      - text: A view that refreshes every second
      - text: A materialized view
      - text: A dashboard filter
    explanation: Dynamic views still work, but users must query the view, not the base table. Row filters and masks protect the table itself.
  - id: least-privilege
    prompt: Which practices follow least privilege?
    type: multiple
    options:
      - id: groups
        text: Grant to groups, not individuals
        correct: true
      - id: narrow
        text: Grant `SELECT` on specific schemas or tables rather than `ALL PRIVILEGES` on catalogs
        correct: true
      - id: separate-write
        text: Give write access only to pipelines' service principals in production
        correct: true
      - text: Make everyone a metastore admin to avoid tickets
    explanation: Broad grants are convenient and the usual root cause of data exposure.
  - id: compute-enforcement
    prompt: Why must fine-grained access be enforced by the compute, not just stored in the catalog?
    options:
      - text: It does not need enforcement
      - id: compute
        text: The engine reading the files applies filters and masks; compute without Unity Catalog enforcement (or direct storage access) would bypass them, which is why access modes and storage credentials matter
        correct: true
      - text: The browser enforces it
      - text: Parquet files enforce it
    explanation: Anyone with direct cloud-storage access to table files bypasses Unity Catalog entirely. Lock down storage so only Unity Catalog can read it.
  - id: storage-bypass
    prompt: An engineer has an IAM role with read access to the S3 bucket behind a managed table that has a row filter. What can they do?
    options:
      - text: Nothing; Unity Catalog encrypts everything
      - text: Only read filtered rows
      - id: bypass
        text: Read the raw Parquet files directly, bypassing row filters, masks and audit — governance requires locking down the storage layer too
        correct: true
      - text: Only read metadata
    explanation: This is the most important "blind spot" in catalog-based governance on any platform.
  - id: external-engines
    prompt: How do external engines (for example Spark outside Databricks, or Trino) get governed access to Unity Catalog tables?
    options:
      - text: They read storage with admin keys
      - text: They cannot access Unity Catalog tables
      - text: Through email requests
      - id: vending
        text: Through Unity Catalog's open APIs and credential vending — the catalog issues short-lived, scoped storage credentials after checking privileges
        correct: true
    explanation: Credential vending cannot enforce row filters or masks inside an external engine. Fine-grained policies may block or restrict external access to such tables.
  - id: audit
    prompt: Where are data access events recorded?
    options:
      - id: audit-table
        text: In audit logs, queryable as the `system.access.audit` system table
        correct: true
      - text: Nowhere
      - text: In the notebook output
      - text: Only in the cloud provider's logs
    explanation: Audit logs answer "who read this table" after an incident — provided retention is long enough for your needs.
---
## Learning objectives

- Implement row filters and column masks
- Understand ABAC with governed tags
- Recognise the storage-layer bypass and external-engine limits

## Row filter and column mask

```sql
CREATE FUNCTION main.security.region_filter(region STRING)
RETURN is_account_group_member('global-sales') OR region = 'EMEA' AND is_account_group_member('emea-sales');

ALTER TABLE main.sales.orders SET ROW FILTER main.security.region_filter ON (region);

CREATE FUNCTION main.security.mask_email(email STRING)
RETURN CASE WHEN is_account_group_member('privacy') THEN email
            ELSE regexp_replace(email, '^.*@', '***@') END;

ALTER TABLE main.sales.customers ALTER COLUMN email SET MASK main.security.mask_email;
```

## The layered model

| Layer | Control |
|---|---|
| Identity | IdP, groups, service principals |
| Catalog | Grants, filters, masks, ABAC |
| Compute | Access modes that enforce Unity Catalog |
| Storage | Bucket/container policies so only Unity Catalog's credentials can read |
| Network | Private connectivity, egress controls |

## Sources

- Databricks docs, "Row filters and column masks" — <https://docs.databricks.com/aws/en/tables/row-and-column-filters>
- Databricks docs, "Attribute-based access control" — <https://docs.databricks.com/aws/en/data-governance/unity-catalog/abac/>
- Databricks docs, "Credential vending for external engines" — <https://docs.databricks.com/aws/en/external-access/credential-vending>
- Databricks docs, "Audit log system table" — <https://docs.databricks.com/aws/en/admin/system-tables/audit-logs>
