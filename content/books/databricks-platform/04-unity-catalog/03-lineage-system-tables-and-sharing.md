---
title: Lineage, system tables and Delta Sharing
questions:
  - id: lineage
    prompt: How is lineage captured in Unity Catalog?
    options:
      - text: Engineers must document it manually in a wiki
      - id: automatic
        text: Automatically, down to the column level, for queries run on Databricks compute
        correct: true
      - text: Only for tables created with a special LINEAGE keyword
      - text: Lineage is not available in Databricks
    explanation: Unity Catalog records table and column lineage from notebooks, jobs, pipelines and SQL, and shows it in Catalog Explorer.
  - id: system-tables
    prompt: Which system table would you query to analyse spending by workspace and SKU?
    options:
      - text: "`system.access.audit`"
      - id: billing
        text: "`system.billing.usage`"
        correct: true
      - text: "`information_schema.tables`"
    explanation: Billing usage records DBU consumption; join with `system.billing.list_prices` to estimate cost.
  - id: sharing
    prompt: Which statements about Delta Sharing are true?
    type: multiple
    options:
      - text: It is an open protocol for sharing live data without copying it
        correct: true
      - text: Recipients do not need to be Databricks customers when using open sharing
        correct: true
      - text: Providers create shares containing tables, views, volumes or models, and grant them to recipients
        correct: true
      - text: Sharing always requires exporting CSV files by e-mail
    explanation: Delta Sharing supports Databricks-to-Databricks and open sharing to tools like pandas, Power BI and Spark.
---
## Learning objectives

- Use lineage for impact analysis
- Query system tables for audit and cost
- Share data securely with Delta Sharing

## Lineage

Open a table in **Catalog Explorer** and choose *Lineage* to see upstream sources, downstream
tables, dashboards, notebooks and jobs — at table or column level. Typical uses:

- **Impact analysis** — what breaks if I change this column?
- **Root-cause analysis** — where did this bad value come from?
- **Compliance** — which reports use PII columns?

## System tables

```sql
-- Who accessed a sensitive table recently?
SELECT event_time, user_identity.email, action_name
FROM system.access.audit
WHERE request_params.full_name_arg = 'prod.sales.customers'
  AND event_date >= current_date() - INTERVAL 7 DAYS;

-- Which workspaces spent the most DBUs last month?
SELECT workspace_id, sku_name, SUM(usage_quantity) AS dbus
FROM system.billing.usage
WHERE usage_date >= add_months(trunc(current_date(), 'MM'), -1)
  AND usage_date <  trunc(current_date(), 'MM')
GROUP BY ALL
ORDER BY dbus DESC;
```

Other useful schemas include `system.lakeflow` (jobs and pipelines), `system.compute` and
`system.query` (query history).

## Delta Sharing

```sql
CREATE SHARE partner_sales;
ALTER SHARE partner_sales ADD TABLE prod.gold.daily_sales_by_region;

CREATE RECIPIENT acme_corp;                    -- open sharing: generates an activation link
GRANT SELECT ON SHARE partner_sales TO RECIPIENT acme_corp;
```

A recipient on another Databricks account (with Unity Catalog) mounts the share as a read-only
catalog. Open-sharing recipients use connectors:

```python
import delta_sharing
df = delta_sharing.load_as_pandas("config.share#partner_sales.gold.daily_sales_by_region")
```

The data is not copied; recipients read the current version directly, and the provider can revoke
access at any time.
