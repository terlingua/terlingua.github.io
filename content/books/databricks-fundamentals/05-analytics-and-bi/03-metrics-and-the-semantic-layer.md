---
title: Metrics and the semantic layer
questions:
  - id: metric-view
    prompt: What is a Unity Catalog metric view?
    options:
      - text: A dashboard tile
      - text: A materialized table of aggregates
      - id: semantic
        text: A governed object defining measures and dimensions over source tables (in YAML), so any tool can query a metric with consistent logic
        correct: true
      - text: A permission level
    explanation: It is Databricks' semantic layer. Measures are queried with the `MEASURE()` function and aggregate correctly at any grouping.
  - id: why-semantic
    prompt: What problem does a semantic layer solve?
    options:
      - id: consistency
        text: The same business metric being calculated differently in different dashboards, notebooks and tools
        correct: true
      - text: Slow storage
      - text: Missing indexes
      - text: Network latency
    explanation: dbt's semantic layer, Cube, LookML and Power BI models solve the same problem. The open question is which layer is authoritative when several exist.
  - id: ratio
    prompt: Why can't you safely average a precomputed ratio (like conversion rate per day) to get a monthly rate?
    options:
      - text: Ratios cannot be stored
      - text: SQL has no AVG function
      - id: non-additive
        text: Ratios are non-additive — the correct monthly rate is total conversions / total visits, not the mean of daily rates
        correct: true
      - text: It is always fine
    explanation: Measures in a semantic layer define the numerator and denominator so aggregation is correct at any grain. LLMs generating SQL frequently make this mistake.
  - id: measure-query
    prompt: "How do you query a measure called `revenue` from a metric view `main.gold.sales_metrics`?"
    options:
      - text: "`SELECT revenue FROM main.gold.sales_metrics`"
      - text: "`SELECT SUM(revenue) FROM main.gold.sales_metrics`"
      - text: "`CALL revenue()`"
      - id: measure
        text: "`SELECT region, MEASURE(revenue) FROM main.gold.sales_metrics GROUP BY region`"
        correct: true
    explanation: The `MEASURE` function applies the measure's defined aggregation at the query's grouping.
  - id: uc-metrics
    prompt: What is Unity Catalog Metrics as described at Data + AI Summit 2026?
    options:
      - text: A billing report
      - id: governed-kpis
        text: Governed KPIs built on metric views with features such as multi-fact relationships, level-of-detail expressions and materialisation, usable by BI tools and agents
        correct: true
      - text: A cluster monitoring tool
      - text: A new storage format
    explanation: Some capabilities were in public preview at announcement. Verify status before depending on them.
  - id: interop
    prompt: What should you check before adopting any vendor's semantic layer?
    type: multiple
    options:
      - id: tools
        text: Which BI tools and agents can consume it
        correct: true
      - id: export
        text: Whether definitions can be exported or are tied to one platform
        correct: true
      - id: overlap
        text: How it coexists with semantic models already in tools like Power BI or dbt
        correct: true
      - text: Its logo colour
    explanation: Semantic layers are a classic lock-in point because so much business logic lives in them.
  - id: agents-metrics
    prompt: Why are governed metrics especially important for AI agents?
    options:
      - text: Agents cannot run SQL
      - text: They are not important for agents
      - id: grounding
        text: Agents generating SQL need authoritative definitions to avoid inventing calculations; metric views give them a vetted interface
        correct: true
      - text: Metrics make agents faster to train
    explanation: Giving an agent a governed tool ("get revenue by region") is safer than giving it free-form SQL over raw tables.
  - id: dimension
    prompt: In a metric view, what is a dimension?
    options:
      - id: dim
        text: An attribute used to group or filter measures, such as region, product or order month
        correct: true
      - text: The physical size of a table
      - text: A number of columns
      - text: A type of cluster
    explanation: Measures are aggregations; dimensions are the axes along which you slice them.
---
## Learning objectives

- Explain what a semantic layer is and why it matters
- Define and query a metric view
- Recognise non-additive measures

## A metric view

```yaml
version: 1.1
source: main.silver.orders
dimensions:
  - name: region
    expr: region
  - name: order_month
    expr: DATE_TRUNC('MONTH', order_ts)
measures:
  - name: revenue
    expr: SUM(amount)
  - name: orders
    expr: COUNT(1)
  - name: average_order_value
    expr: SUM(amount) / COUNT(1)
```

```sql
SELECT order_month, MEASURE(average_order_value)
FROM main.gold.sales_metrics
GROUP BY order_month;
```

The average order value is computed correctly at monthly grain, because the measure divides totals
rather than averaging averages.

## Sources

- Databricks docs, "Unity Catalog metric views" — <https://docs.databricks.com/aws/en/metric-views/>
- dbt Semantic Layer — <https://docs.getdbt.com/docs/use-dbt-semantic-layer/dbt-sl>
- Atlan, "Databricks Data + AI Summit 2026 announcements" — <https://atlan.com/know/ai-agent/databricks/databricks-data-ai-summit-2026-announcements/>
