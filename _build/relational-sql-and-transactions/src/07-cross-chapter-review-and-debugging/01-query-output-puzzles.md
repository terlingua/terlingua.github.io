---
title: "Query-Output Puzzles"
questions:
  - id: "left-join-count-column"
    type: "single"
    prompt: "Using the puzzle data in this section (customers ann, bob, cy, di; five orders), what does this return for `di`?\n\n```sql\nSELECT c.name, COUNT(o.id) AS n_orders, COUNT(*) AS n_rows\nFROM customers c\nLEFT JOIN orders o ON o.customer_id = c.id\nGROUP BY c.name;\n```"
    correct:
      - "`n_orders` = 0, `n_rows` = 1"
    distractors:
      - "`n_orders` = 0, `n_rows` = 0"
      - "`n_orders` = 1, `n_rows` = 1"
      - "`di` is missing from the result"
    explanation: |-
      Combine three rules: the `LEFT JOIN` keeps di with NULL-padded order columns (Chapter 2); `COUNT(*)` counts rows while `COUNT(col)` skips NULLs (Chapter 3); grouping happens after the join (logical order).

      | name | joined rows | COUNT(o.id) | COUNT(*) |
      |---|---|---|---|
      | ann | (ann, 10), (ann, 11) | 2 | 2 |
      | bob | (bob, 12) | 1 | 1 |
      | cy | (cy, 14) | 1 | 1 |
      | di | (di, NULL) | 0 | 1 |

      "0 and 0" assumes `COUNT(*)` ignores the NULL-padded row; it counts rows, and the padded row is a row.

      "1 and 1" counts the padding as an order.

      "Missing" would be true for an inner join; the outer join preserves di.

      This is why "orders per customer" reports must count a column of the optional table, never `*`.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu'); CREATE TABLE orders(id INT, customer_id INT, amount INT, status TEXT); INSERT INTO orders VALUES (10,1,100,'paid'),(11,1,NULL,'paid'),(12,2,50,'open'),(13,NULL,70,'paid'),(14,3,30,'refunded');"
        query: "SELECT c.name, COUNT(o.id), COUNT(*) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id GROUP BY c.name"
        expect: [["ann", 2, 2], ["bob", 1, 1], ["cy", 1, 1], ["di", 0, 1]]
  - id: "customers-without-orders"
    type: "single"
    prompt: "Same data (order 13 has `customer_id` NULL). A report of customers without orders uses `SELECT name FROM customers WHERE id NOT IN (SELECT customer_id FROM orders)`. What does it return, and what returns the intended answer?"
    correct:
      - "No rows; `NOT EXISTS (SELECT 1 FROM orders o WHERE o.customer_id = c.id)` returns `di`"
    distractors:
      - "`di`; the query is already correct"
      - "`di` and `bob`; bob's only order is still open"
      - "No rows; adding `DISTINCT` to the subquery returns `di`"
    explanation: |-
      The subquery's values are 1, 1, 2, NULL, 3. For di (id 4): `4 <> 1 AND 4 <> 2 AND 4 <> NULL AND 4 <> 3` = TRUE AND TRUE AND UNKNOWN AND TRUE = UNKNOWN, so di is filtered out; every other customer matches some value and is FALSE. Result: no rows (Chapter 3's NOT IN trap).

      `NOT EXISTS` asks whether any order matches di's id; none does (the NULL row matches nothing), so it returns di.

      "Already correct" would hold only if `orders.customer_id` had no NULLs.

      "`di` and `bob`" mixes in a status condition the query never states.

      "`DISTINCT`" removes duplicate 1s but keeps the NULL, so the result is still empty.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu'); CREATE TABLE orders(id INT, customer_id INT, amount INT, status TEXT); INSERT INTO orders VALUES (10,1,100,'paid'),(11,1,NULL,'paid'),(12,2,50,'open'),(13,NULL,70,'paid'),(14,3,30,'refunded');"
        query: "SELECT (SELECT COUNT(*) FROM customers WHERE id NOT IN (SELECT customer_id FROM orders)), (SELECT COUNT(*) FROM customers WHERE id NOT IN (SELECT DISTINCT customer_id FROM orders)), (SELECT name FROM customers c WHERE NOT EXISTS (SELECT 1 FROM orders o WHERE o.customer_id = c.id))"
        expect: [[0, 0, "di"]]
  - id: "paid-revenue-by-region"
    type: "single"
    prompt: "Same data. What does this return?\n\n```sql\nSELECT c.region, SUM(o.amount) AS paid\nFROM customers c\nLEFT JOIN orders o\n  ON o.customer_id = c.id AND o.status = 'paid'\nGROUP BY c.region;\n```"
    correct:
      - "Three rows: `('eu', 100)`, `('us', NULL)`, `(NULL, NULL)`"
    distractors:
      - "Two rows: `('eu', 100)`, `('us', 0)`"
      - "Three rows: `('eu', 100)`, `('us', 0)`, `(NULL, 0)`"
      - "One row: `('eu', 170)`"
    explanation: |-
      Trace: the status test is in `ON`, so every customer survives (Chapter 2). Matching paid orders: ann has 10 (100) and 11 (NULL amount); bob's 12 is open; cy's 14 is refunded; di has none. Order 13 (paid, 70) has no customer and is not joined.

      | region | rows after join | SUM(amount) |
      |---|---|---|
      | eu | ann–100, ann–NULL, di–(none) | 100 (NULLs skipped) |
      | NULL (bob) | bob–(none) | NULL (no values) |
      | us | cy–(none) | NULL |

      Three groups, because NULL regions form one group (Chapter 3), and `SUM` over no values is NULL, not 0.

      "Two rows" drops the NULL region group.

      "`0`" values confuse `SUM` with `COUNT` on empty input; use `COALESCE(SUM(o.amount), 0)` for zeros.

      "`('eu', 170)`" pulls in order 13, which has no customer, and loses the other groups.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu'); CREATE TABLE orders(id INT, customer_id INT, amount INT, status TEXT); INSERT INTO orders VALUES (10,1,100,'paid'),(11,1,NULL,'paid'),(12,2,50,'open'),(13,NULL,70,'paid'),(14,3,30,'refunded');"
        query: "SELECT c.region, SUM(o.amount) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid' GROUP BY c.region"
        expect: [["eu", 100], ["us", null], [null, null]]
  - id: "window-over-groups"
    type: "single"
    prompt: "Same data. What does this return?\n\n```sql\nSELECT status, COUNT(*) AS n,\n       DENSE_RANK() OVER (ORDER BY COUNT(*) DESC) AS r\nFROM orders\nGROUP BY status;\n```"
    correct:
      - "`('paid', 3, 1)`, `('open', 1, 2)`, `('refunded', 1, 2)`"
    distractors:
      - "`('paid', 3, 1)`, `('open', 1, 2)`, `('refunded', 1, 3)`"
      - "An error, because a window function cannot contain an aggregate"
      - "Five rows, one per order, because window functions do not collapse rows"
    explanation: |-
      Logical order (Chapters 2 and 4): `GROUP BY` produces one row per status, aggregates are computed, and *then* window functions run over those grouped rows. So `COUNT(*)` inside `OVER (ORDER BY ...)` refers to each group's count.

      | status | n | DENSE_RANK by n DESC |
      |---|---|---|
      | paid | 3 | 1 |
      | open | 1 | 2 |
      | refunded | 1 | 2 |

      "refunded 3" applies `RANK`-with-tiebreak or `ROW_NUMBER` thinking; `DENSE_RANK` gives ties the same rank. (`RANK` would also give 2 to both, then 4 to a next value.)

      "An error" is wrong: aggregates may appear in a window's `ORDER BY` when the query is grouped.

      "Five rows" forgets that grouping happened before the window step.
    checks:
      - engine: "both"
        setup: "CREATE TABLE orders(id INT, customer_id INT, amount INT, status TEXT); INSERT INTO orders VALUES (10,1,100,'paid'),(11,1,NULL,'paid'),(12,2,50,'open'),(13,NULL,70,'paid'),(14,3,30,'refunded');"
        query: "SELECT status, COUNT(*) AS n, DENSE_RANK() OVER (ORDER BY COUNT(*) DESC) AS r FROM orders GROUP BY status"
        expect: [["paid", 3, 1], ["open", 1, 2], ["refunded", 1, 2]]
  - id: "union-collapses-nulls"
    type: "single"
    prompt: "Same customers (regions eu, NULL, us, eu). How many rows does `SELECT region FROM customers UNION SELECT NULL` return?"
    correct:
      - "3"
    distractors:
      - "4"
      - "5"
      - "2"
    explanation: |-
      `UNION` means `UNION DISTINCT` (Chapter 1), and duplicate elimination uses "not distinct", under which two NULLs are the same (Chapter 3).

      | input rows | after duplicate elimination |
      |---|---|
      | eu, NULL, us, eu, NULL | eu, NULL, us |

      Result: 3 rows.

      "4" keeps the two NULLs apart, applying `=` semantics.

      "5" is the `UNION ALL` count.

      "2" drops NULL entirely, as `COUNT(DISTINCT region)` would.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu');"
        query: "SELECT COUNT(*) FROM (SELECT region FROM customers UNION SELECT NULL) u"
        expect: [[3]]
  - id: "order-by-alias-coalesce"
    type: "single"
    prompt: "Same customers. In which position (1 to 4) does `bob` appear in the output of this query?\n\n```sql\nSELECT name, COALESCE(region, 'unknown') AS r\nFROM customers\nORDER BY r;\n```"
    correct:
      - "3"
    distractors:
      - "1"
      - "4"
      - "It depends on whether the engine sorts NULLs first or last"
    explanation: |-
      `ORDER BY` may use the select-list alias `r` (Chapter 2), and `r` is never NULL: bob's region becomes the string `'unknown'`. Sorting the strings:

      | name | r |
      |---|---|
      | ann | eu |
      | di | eu |
      | bob | unknown |
      | cy | us |

      `'eu' < 'unknown' < 'us'`, so bob is third (ann and di tie, in either order).

      "1" and "4" are where NULL-first or NULL-last engines would put bob if the query sorted by `region` itself.

      "It depends on NULL ordering" misses that `COALESCE` removed the NULL before sorting, which is a portable way to make NULL placement explicit.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu');"
        query: "SELECT name FROM (SELECT name, COALESCE(region, 'unknown') AS r FROM customers) t ORDER BY r, name"
        expect: [["ann"], ["di"], ["bob"], ["cy"]]
        ordered: true
body_checks:
  - engine: "both"
    setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu'); CREATE TABLE orders(id INT, customer_id INT, amount INT, status TEXT); INSERT INTO orders VALUES (10,1,100,'paid'),(11,1,NULL,'paid'),(12,2,50,'open'),(13,NULL,70,'paid'),(14,3,30,'refunded');"
    query: "SELECT (SELECT COUNT(*) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id), (SELECT COUNT(*) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid'), (SELECT COUNT(*) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid' WHERE o.id IS NOT NULL)"
    expect: [[5, 5, 2]]
  - engine: "both"
    setup: "CREATE TABLE customers(id INT, name TEXT, region TEXT); INSERT INTO customers VALUES (1,'ann','eu'),(2,'bob',NULL),(3,'cy','us'),(4,'di','eu'); CREATE TABLE orders(id INT, customer_id INT, amount INT, status TEXT); INSERT INTO orders VALUES (10,1,100,'paid'),(11,1,NULL,'paid'),(12,2,50,'open'),(13,NULL,70,'paid'),(14,3,30,'refunded');"
    query: "SELECT c.region, COALESCE(SUM(o.amount), 0) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid' GROUP BY c.region"
    expect: [["eu", 100], ["us", 0], [null, 0]]
---
## Learning objectives

- Predict query output by applying the logical order, join semantics, three-valued logic and aggregate rules together.
- Locate the exact step where a surprising row appears or disappears.
- Choose the rewrite that expresses the intended question.

## The invariant

Every result in this chapter follows from five rules introduced earlier, applied in the logical processing order:

| Step | Rule | Chapter |
|---|---|---|
| FROM / JOIN | inner joins multiply matches; outer joins add NULL-padded rows; `ON` decides matching | 1, 2 |
| WHERE / HAVING | keep only TRUE; comparisons with NULL are UNKNOWN | 2, 3 |
| GROUP BY | NULLs group together; one row per group | 3, 4 |
| aggregates | NULLs are skipped; `COUNT` of nothing is 0, `SUM` of nothing is NULL | 3 |
| window functions, DISTINCT, ORDER BY | run on grouped rows; `DISTINCT`/`UNION` treat NULLs as equal; aliases visible in `ORDER BY` | 1–4 |

A reliable debugging method: rewrite the query as a pipeline of derived tables, one per step, and run each prefix. The step at which a row count changes unexpectedly is where the misunderstanding lives. Because SQL's meaning is defined by this pipeline, not by the plan, the method works on every engine.

Two habits make the method fast. First, count rows after every step and compare with what you expected *before* running it; a guess written down beforehand exposes the misconception, while a number read afterwards tends to be rationalized. Second, whenever a NULL is involved, ask which of SQL's two notions of sameness the current step uses: `=` (joins, `WHERE`, `IN`), where NULL matches nothing, or "not distinct" (`GROUP BY`, `DISTINCT`, `UNION`), where NULLs collapse together. Most puzzles in this section hinge on exactly one step using the notion you did not expect.

## Worked example

The puzzle data used by every question in this section:

| customers.id | name | region |
|---|---|---|
| 1 | ann | eu |
| 2 | bob | NULL |
| 3 | cy | us |
| 4 | di | eu |

| orders.id | customer_id | amount | status |
|---|---|---|---|
| 10 | 1 | 100 | paid |
| 11 | 1 | NULL | paid |
| 12 | 2 | 50 | open |
| 13 | NULL | 70 | paid |
| 14 | 3 | 30 | refunded |

Debugging "paid revenue by region" step by step:

```sql
-- Step 1: the join alone (5 rows; order 13 has no customer)
SELECT c.name, c.region, o.id, o.amount, o.status
FROM customers c LEFT JOIN orders o ON o.customer_id = c.id;

-- Step 2: status test in ON (5 rows; only ann matches, twice)
SELECT c.name, c.region, o.id, o.amount
FROM customers c LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid';

-- Step 3: aggregate (eu 100, us NULL, NULL NULL), then decide on zeros
SELECT c.region, COALESCE(SUM(o.amount), 0) AS paid
FROM customers c LEFT JOIN orders o ON o.customer_id = c.id AND o.status = 'paid'
GROUP BY c.region;
```

Step 1 shows five joined rows (ann–10, ann–11, bob–12, cy–14, di–NULL); order 13 is absent because no customer has a NULL id. Step 2 keeps one row per customer except ann, who matches two paid orders. Each step answers one question about the data before the next is added.

## Where this shows up today

As of November 2026, every engine used for this book's checks (PostgreSQL 16.15 and SQLite 3.53.1) returns the results shown, because these rules come from the standard, not from optimizer choices. The same puzzles reappear in ORM-generated SQL and BI tools, where the generated query is often the first thing to print and step through.

## Pitfalls checklist

- Counting `*` after an outer join.
- Using `NOT IN` against a nullable column.
- Expecting `SUM` to return 0 for groups with no matching rows.
- Forgetting that window functions see grouped rows, not base rows.
- Forgetting that `UNION`, `DISTINCT` and `GROUP BY` treat NULLs as equal.

## Key takeaways

- Read queries in logical order and track row counts at each step.
- NULLs change results at joins, filters, aggregates and set operations, in different ways.
- Window functions run after grouping and see one row per group.
- Debug by materializing each step as a derived table.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation): joined tables, search conditions, set functions, window functions and query expressions.
- Chapters 1–4 of this book.
