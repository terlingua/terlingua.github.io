---
title: Lineage, system tables and sharing
questions:
  - id: lineage
    prompt: What lineage does Unity Catalog capture?
    options:
      - text: Only manual diagrams users draw
      - text: Git history
      - id: auto-lineage
        text: Table- and column-level lineage captured automatically from queries and jobs running on Unity Catalog compute, including notebooks, jobs, dashboards and models
        correct: true
      - text: Network traffic
    explanation: Lineage from tools outside Databricks requires integration (for example OpenLineage or external lineage APIs).
  - id: lineage-use
    prompt: Which tasks does lineage help with?
    type: multiple
    options:
      - id: impact
        text: Impact analysis before changing or dropping a column
        correct: true
      - id: root-cause
        text: Root-cause analysis when a dashboard shows bad numbers
        correct: true
      - id: compliance
        text: Showing regulators where personal data flows
        correct: true
      - text: Making queries faster
    explanation: Lineage is only as complete as the coverage of tools that report it.
  - id: system-tables
    prompt: What are system tables?
    options:
      - id: sys
        text: Databricks-hosted tables in the `system` catalog containing operational data — billing, audit, lineage, compute, jobs, query history and more
        correct: true
      - text: Tables created by the operating system
      - text: Temporary tables
      - text: Tables in the Hive metastore
    explanation: They must be enabled per schema in some cases, and have retention limits. Copy what you need for long-term analysis.
  - id: delta-sharing
    prompt: What is Delta Sharing?
    options:
      - text: A file-sync tool like Dropbox
      - text: Copying tables into the recipient's account
      - text: A Spark shuffle optimisation
      - id: open-protocol
        text: An open protocol for sharing live tables, views, volumes and models with other organisations — on Databricks or not — without copying data, with access governed and revocable
        correct: true
    explanation: Open recipients read with pandas, Spark, Power BI and others. Egress costs for cross-region or cross-cloud sharing fall on the provider.
  - id: sharing-cost
    prompt: A provider in AWS us-east-1 shares a large, frequently read table with a recipient in Azure Europe. What hidden cost should the provider expect?
    options:
      - text: None — sharing is free
      - id: egress
        text: Cross-cloud and cross-region egress charges each time data is read, which can be significant; replicating to the recipient's region may be cheaper
        correct: true
      - text: The recipient's compute costs
      - text: A Delta Sharing licence fee per row
    explanation: "\"Zero copy\" does not mean zero cost. Network egress is billed by the cloud provider."
  - id: marketplace
    prompt: What is Databricks Marketplace?
    options:
      - text: An app store for laptop software
      - id: exchange
        text: An exchange for data products, models, notebooks and solutions, delivered via Delta Sharing
        correct: true
      - text: The Databricks pricing page
      - text: A job board
    explanation: Listings can be free or paid; quality varies by provider.
  - id: clean-rooms
    prompt: What do clean rooms enable?
    options:
      - text: Cleaning dirty data automatically
      - text: Deleting old data
      - id: privacy-collab
        text: Two or more parties running agreed computations on combined data without either seeing the other's raw data
        correct: true
      - text: Air-gapped workspaces
    explanation: A typical use is advertising measurement between a retailer and a brand.
  - id: opensharing
    prompt: What was announced as OpenSharing in 2026?
    options:
      - text: Free storage for open-source projects
      - text: A Git hosting service
      - text: The end of Delta Sharing
      - id: superset
        text: An open superset of Delta Sharing extended to AI assets and on-premises support, according to summit recaps
        correct: true
    explanation: Check primary documentation for exact scope and status before adopting.
  - id: catalog-federation
    prompt: What does catalog federation do?
    options:
      - id: other-catalogs
        text: Lets Unity Catalog govern and query tables registered in other catalogs (such as a Hive metastore or AWS Glue) without migrating them first
        correct: true
      - text: Merges all workspaces into one
      - text: Exports Unity Catalog to Excel
      - text: Replicates data across clouds
    explanation: Useful as a migration bridge. Long term, governing everything in one place is simpler.
---
## Learning objectives

- Use lineage and system tables for impact analysis, audit and cost
- Share data across organisations with Delta Sharing, Marketplace and clean rooms
- Understand the egress cost of "zero-copy" sharing

## Useful system table queries

```sql
SELECT event_time, user_identity.email, action_name, request_params
FROM system.access.audit
WHERE action_name IN ('getTable', 'commandSubmit')
  AND request_params['full_name_arg'] = 'main.hr.salaries'
  AND event_date >= current_date() - INTERVAL 7 DAYS;

SELECT source_table_full_name, target_table_full_name, entity_type
FROM system.access.table_lineage
WHERE source_table_full_name = 'main.silver.orders';
```

## Delta Sharing

```sql
CREATE SHARE partner_share;
ALTER SHARE partner_share ADD TABLE main.gold.daily_revenue;
CREATE RECIPIENT acme;
GRANT SELECT ON SHARE partner_share TO RECIPIENT acme;
```

## Sources

- Databricks docs, "Data lineage" — <https://docs.databricks.com/aws/en/data-governance/unity-catalog/data-lineage>
- Databricks docs, "System tables" — <https://docs.databricks.com/aws/en/admin/system-tables/>
- Delta Sharing protocol — <https://github.com/delta-io/delta-sharing>
- Databricks docs, "Clean rooms" — <https://docs.databricks.com/aws/en/clean-rooms/>
- Flexera, "Databricks Data + AI Summit 2026" — <https://www.flexera.com/blog/perspectives/databricks-data-ai-summit-2026/>
