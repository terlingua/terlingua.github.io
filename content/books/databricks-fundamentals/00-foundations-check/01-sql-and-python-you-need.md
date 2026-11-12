---
title: SQL and Python you need
questions:
  - id: inner-join
    prompt: What does an INNER JOIN between `orders` and `customers` on `customer_id` return?
    options:
      - text: All orders, with NULLs for missing customers
      - id: matches
        text: Only rows where a matching `customer_id` exists in both tables
        correct: true
      - text: All customers, with NULLs for missing orders
      - text: Every combination of rows
    explanation: LEFT JOIN keeps all left rows; CROSS JOIN produces every combination.
  - id: left-join-filter
    prompt: "`SELECT ... FROM customers c LEFT JOIN orders o ON c.id = o.customer_id WHERE o.amount > 100` — what is the subtle effect?"
    options:
      - text: None; it keeps all customers
      - text: It causes a syntax error
      - text: It duplicates every row
      - id: becomes-inner
        text: The WHERE filter on the right table removes rows where `o.amount` is NULL, effectively turning the LEFT JOIN into an INNER JOIN; put the condition in the ON clause to keep unmatched customers
        correct: true
    explanation: A classic analytics bug, and one LLM-generated SQL makes too.
  - id: fanout
    prompt: You join `orders` (one row per order) to `order_items` (several rows per order) and then `SUM(orders.shipping_fee)`. What goes wrong?
    options:
      - id: double-count
        text: Shipping fees are counted once per item row — join fan-out inflates the sum
        correct: true
      - text: Nothing
      - text: The query fails
      - text: The sum is rounded
    explanation: Aggregate at the right grain before joining, or use a semantic layer that understands grain.
  - id: group-by
    prompt: What does GROUP BY do?
    options:
      - text: Sorts rows
      - text: Removes NULLs
      - id: groups
        text: Collapses rows with equal grouping values into one row per group, so aggregate functions compute per group
        correct: true
      - text: Creates an index
    explanation: "Databricks SQL supports `GROUP BY ALL`, which groups by all non-aggregated select columns."
  - id: window
    prompt: Which query finds each customer's most recent order?
    options:
      - text: "`SELECT MAX(*) FROM orders`"
      - text: "`SELECT * FROM orders ORDER BY order_ts DESC LIMIT 1`"
      - id: row-number
        text: "A window function — `ROW_NUMBER() OVER (PARTITION BY customer_id ORDER BY order_ts DESC)` — then keep rows where it equals 1 (or use `QUALIFY`)"
        correct: true
      - text: "`SELECT DISTINCT customer_id FROM orders`"
    explanation: Window functions compute across related rows without collapsing them.
  - id: null-logic
    prompt: What does `WHERE status <> 'cancelled'` return for rows where `status` is NULL?
    options:
      - text: They are included
      - id: excluded
        text: They are excluded — comparisons with NULL are unknown, not true
        correct: true
      - text: An error
      - text: They are converted to 'cancelled'
    explanation: Use `status IS DISTINCT FROM 'cancelled'` or handle NULLs explicitly.
  - id: count-null
    prompt: How do `COUNT(*)` and `COUNT(email)` differ?
    options:
      - text: They are identical
      - text: "`COUNT(email)` counts distinct emails"
      - id: nulls
        text: "`COUNT(*)` counts rows; `COUNT(email)` counts rows where email is not NULL"
        correct: true
      - text: "`COUNT(*)` ignores NULL rows"
    explanation: Small differences like this produce mismatched dashboard numbers.
  - id: python-df
    prompt: In PySpark, what does `df.filter(df.amount > 100).select("id", "amount")` return?
    options:
      - text: A Python list
      - id: dataframe
        text: A new DataFrame describing the transformation; nothing is computed until an action
        correct: true
      - text: The original DataFrame, modified in place
      - text: A pandas DataFrame
    explanation: DataFrames are immutable plans.
  - id: python-loop
    prompt: Why is looping over rows in Python (e.g. `for row in df.collect()`) usually wrong in Spark?
    options:
      - text: Python has no loops
      - text: It is fine for any size
      - text: Spark forbids it
      - id: driver
        text: It pulls all data to the driver and processes it on one machine, losing parallelism and risking out-of-memory errors
        correct: true
    explanation: Express logic as DataFrame or SQL operations so Spark can distribute it.
  - id: dates-timezones
    prompt: Which practice avoids timezone bugs in data pipelines?
    options:
      - id: utc
        text: Store timestamps in UTC with explicit types, convert to local time only for presentation, and know the session timezone
        correct: true
      - text: Store dates as strings in local formats
      - text: Use the server's local time everywhere
      - text: Ignore daylight saving time
    explanation: Daylight-saving transitions create duplicated or missing local hours.
---
## Learning objectives

- Refresh the SQL patterns that analytics depends on
- Avoid join fan-out, NULL logic and driver-collection traps
- Think in DataFrame transformations

## Joins at a glance

| Join | Keeps |
|---|---|
| INNER | Matching rows only |
| LEFT | All left rows, NULLs where no match |
| FULL OUTER | All rows from both sides |
| CROSS | Every combination |
| LEFT SEMI | Left rows that have a match (no right columns) |
| LEFT ANTI | Left rows without a match |

```sql
SELECT *
FROM orders
QUALIFY ROW_NUMBER() OVER (PARTITION BY customer_id ORDER BY order_ts DESC) = 1;
```

## Sources

- Databricks SQL language reference — <https://docs.databricks.com/aws/en/sql/language-manual/>
- PySpark DataFrame API — <https://spark.apache.org/docs/latest/api/python/reference/pyspark.sql/dataframe.html>
