---
title: DBUs and the real bill
questions:
  - id: dbu-def
    prompt: What is a DBU?
    options:
      - text: A unit of storage, 1 GB per month
      - text: A Databricks user licence
      - id: dbu
        text: A Databricks Unit — a normalised unit of processing capability per hour, billed at a rate that depends on compute type, tier, cloud and region
        correct: true
      - text: A data backup unit
    explanation: Cost = DBUs consumed × price per DBU, plus (for classic compute) cloud infrastructure. Different SKUs (jobs, all-purpose, SQL, serverless, model serving) have different rates.
  - id: rate-diff
    prompt: Which typically has the highest DBU list price per unit for comparable work?
    options:
      - text: Jobs compute
      - id: all-purpose
        text: All-purpose (interactive) compute
        correct: true
      - text: Pipelines Core
      - text: They are all identical
    explanation: Exact rates change; check the pricing page for your cloud and tier. The pattern — interactive costs more than automated — has held for years.
  - id: hidden-costs
    prompt: A team compares Databricks to a warehouse vendor by DBU price only. What do they miss?
    type: multiple
    options:
      - id: infra
        text: Cloud VM, disk and network costs for classic compute
        correct: true
      - id: storage
        text: Object storage and request costs
        correct: true
      - id: background
        text: Background serverless usage such as predictive optimisation, monitoring and vector search endpoints
        correct: true
      - id: people
        text: Engineering time spent tuning and operating the platform
        correct: true
    explanation: Total cost of ownership includes all four. Vendors on both sides tend to compare the line items that favour them.
  - id: system-billing
    prompt: Where can you query detailed Databricks usage?
    options:
      - text: Only on the monthly invoice PDF
      - text: It is not available to customers
      - text: In the Spark UI
      - id: system-tables
        text: "In system tables such as `system.billing.usage` and `system.billing.list_prices`, joinable with tags for attribution"
        correct: true
    explanation: Build a cost dashboard from system tables early; the account console also offers usage dashboards and budgets.
  - id: tags
    prompt: How do you attribute spend to teams or projects?
    options:
      - id: tags
        text: Apply custom tags to compute, jobs, warehouses and serverless budget policies, then group usage by tag
        correct: true
      - text: Ask teams to estimate
      - text: Use one workspace per employee
      - text: Not possible on Databricks
    explanation: Enforce tags through compute policies; untagged spend is unattributable spend.
  - id: budgets
    prompt: What do Databricks budgets do by default when exceeded?
    options:
      - text: Shut down all compute immediately
      - text: Delete expensive tables
      - id: alert
        text: Send alerts — they track and notify; they do not by themselves stop workloads
        correct: true
      - text: Increase the credit card limit
    explanation: Hard limits need policies (cluster size, max DBUs per hour), serverless budget policies and warehouse sizing. Alerts alone do not cap spend.
  - id: commit
    prompt: What is a committed-use contract?
    options:
      - id: commit
        text: A pre-purchase of Databricks usage over a term in exchange for a discount on list prices
        correct: true
      - text: A Git commit to a Databricks repo
      - text: A guarantee of uptime
      - text: A free tier
    explanation: Commitments lower unit prices but create pressure to consume. Model realistic usage before signing.
  - id: optimize-cost
    prompt: Which actions reduce cost most reliably?
    type: multiple
    options:
      - id: right-compute
        text: Running production on job or serverless compute rather than all-purpose clusters
        correct: true
      - id: terminate
        text: Enforcing auto-termination and right-sizing
        correct: true
      - id: layout
        text: Fixing data layout (compaction, clustering) so queries read less data
        correct: true
      - text: Always choosing the largest cluster so jobs finish faster
    explanation: A bigger cluster that finishes in half the time costs about the same; one that does not scale linearly costs more. Reading less data is the cheapest optimisation.
  - id: serverless-sql-idle
    prompt: A serverless SQL warehouse has auto-stop set to 10 minutes and receives a dashboard refresh every 5 minutes all day. What happens?
    options:
      - text: It stops between refreshes and costs almost nothing
      - text: Serverless is never billed for idle time
      - id: always-on
        text: It never stops, because each refresh resets the idle timer, so it bills all day
        correct: true
      - text: The refresh fails
    explanation: Schedules and auto-stop interact. Reduce refresh frequency, cache results, or accept the cost deliberately.
  - id: list-vs-discounts
    prompt: Why can't you just multiply DBUs by a published price to forecast your bill?
    options:
      - text: Published prices are fake
      - id: discounts
        text: Actual prices depend on negotiated discounts, cloud marketplace agreements and region; list prices are a ceiling for estimation
        correct: true
      - text: DBUs are random
      - text: Prices are only in euros
    explanation: Use `system.billing.list_prices` for estimates and your contract for actuals.
---
## Learning objectives

- Explain DBUs and how they combine with cloud costs
- Find and attribute usage with system tables and tags
- Know which actions reliably reduce spend

## A cost query

```sql
SELECT u.usage_date,
       u.custom_tags['team']                      AS team,
       u.sku_name,
       SUM(u.usage_quantity)                      AS dbus,
       SUM(u.usage_quantity * p.pricing.default)  AS list_cost
FROM system.billing.usage u
JOIN system.billing.list_prices p
  ON u.sku_name = p.sku_name
 AND u.usage_start_time >= p.price_start_time
 AND (p.price_end_time IS NULL OR u.usage_start_time < p.price_end_time)
WHERE u.usage_date >= current_date() - INTERVAL 30 DAYS
GROUP BY ALL
ORDER BY list_cost DESC;
```

## The honest version

Databricks' pricing is flexible, which also makes it hard to predict. The cheapest-looking SKU is
not always the cheapest workload. Track spend weekly, tag everything, and treat cost as a
non-functional requirement from the first day.

## Sources

- Databricks pricing — <https://www.databricks.com/product/pricing>
- Databricks docs, "Billable usage system table" — <https://docs.databricks.com/aws/en/admin/system-tables/billing>
- Databricks docs, "Budgets" — <https://docs.databricks.com/aws/en/admin/account-settings/budgets>
- Databricks docs, "Cost optimization best practices" — <https://docs.databricks.com/aws/en/lakehouse-architecture/cost-optimization/best-practices>
