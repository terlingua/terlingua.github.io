---
title: The medallion architecture
questions:
  - id: layers
    prompt: In the medallion architecture, which layer contains cleaned, validated and conformed data at roughly the grain of the source?
    options:
      - id: silver
        text: Silver
        correct: true
      - text: Bronze
      - text: Gold
      - text: Platinum
    explanation: Bronze holds raw data, silver holds cleaned and conformed data, and gold holds business-level aggregates and data products.
  - id: bronze
    prompt: Why keep a raw bronze layer instead of cleaning data on ingestion?
    options:
      - id: replay
        text: It preserves source fidelity so you can reprocess history when business rules or bugs change downstream logic
        correct: true
      - text: Because Delta Lake cannot store clean data
      - text: Bronze tables are faster to query for BI
    explanation: Raw data is the audit trail and the replay source for every downstream table.
  - id: practices
    prompt: Which are good medallion practices?
    type: multiple
    options:
      - text: Add ingestion metadata such as source file and load timestamp in bronze
        correct: true
      - text: Apply data quality rules when moving from bronze to silver
        correct: true
      - text: Model gold tables around business questions and consumers
        correct: true
      - text: Let BI dashboards query bronze tables directly
    explanation: Consumers should read curated silver or gold data, not raw ingestion tables.
---
## Learning objectives

- Describe the bronze, silver and gold layers
- Decide what logic belongs in each layer
- Map the layers onto Unity Catalog

## Three layers

| Layer | Contains | Typical operations |
|-------|----------|--------------------|
| **Bronze** | Raw data as ingested, plus metadata | Append-only ingestion with Auto Loader, minimal parsing |
| **Silver** | Cleaned, deduplicated, typed, conformed entities | Data quality rules, joins, `MERGE`, CDC application |
| **Gold** | Business-level aggregates and data products | Star schemas, KPIs, feature tables, materialized views |

## An end-to-end flow

```text
Landing volume ──Auto Loader──▶ bronze.orders (raw JSON + _metadata)
                                    │ expectations, casting, dedup
                                    ▼
                              silver.orders ◀── silver.customers (AUTO CDC from CRM)
                                    │ joins, aggregation
                                    ▼
                        gold.daily_revenue · gold.customer_lifetime_value
                                    │
                    AI/BI dashboards · Genie · ML features · Delta Sharing
```

## Mapping to Unity Catalog

Two common options:

- **Schema per layer**: `prod.bronze.orders`, `prod.silver.orders`, `prod.gold.daily_revenue`
- **Catalog per layer**: `bronze.sales.orders`, `silver.sales.orders`, `gold.sales.daily_revenue`

Grant data engineers write access to bronze and silver, analysts read access to gold, and data
scientists read access to silver and gold.

## Guidelines

- Keep bronze immutable and append-only; it is your replay log
- Make silver the "single source of truth" per entity
- Shape gold for consumers; it is fine to have several gold tables from the same silver data
- The layers are a guideline, not a law — small domains may combine silver and gold
