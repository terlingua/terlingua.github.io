---
title: "EXISTS, IN, JOIN and the Duplicate-Multiplication Trap"
questions:
  - id: "join-versus-in-row-count"
    type: "single"
    prompt: "`customers(id, name)`: `(1, 'ann')`, `(2, 'bob')`, `(3, 'cy')`. `orders(id, customer_id, total)`: `(10, 1, 100)`, `(11, 1, 40)`, `(12, 2, 40)`. How many rows do these two queries return?\n\n```sql\n-- Q1\nSELECT c.name FROM customers c JOIN orders o ON o.customer_id = c.id;\n-- Q2\nSELECT c.name FROM customers c WHERE c.id IN (SELECT customer_id FROM orders);\n```"
    explanation: |-
      A join pairs each customer with every matching order, so a customer appears once per order. `IN (subquery)` is a **semi-join**: it asks only *whether* a match exists, so each customer appears at most once, however many orders match.

      | customer | matching orders | Q1 rows | Q2 rows |
      |---|---|---|---|
      | ann | 10, 11 | 2 | 1 |
      | bob | 12 | 1 | 1 |
      | cy | none | 0 | 0 |
      | total | | 3 | 2 |

      "Both return 2 rows" treats a join as a semi-join.

      "Both return 3 rows" assumes `IN` repeats the outer row per match; duplicates in the subquery's result do not multiply outer rows.

      "Q1 returns 2; Q2 returns 3" reverses the two semantics.

      "Q1 returns 4 rows" counts cy as a NULL-padded row, which only a `LEFT JOIN` would produce.
    options:
      - text: "Both return 2 rows"
      - text: "Both return 3 rows"
      - text: "Q1 returns 2 rows; Q2 returns 3 rows"
      - text: "Q1 returns 4 rows; Q2 returns 2 rows"
      - text: "Q1 returns 3 rows; Q2 returns 2 rows"
        correct: true
  - id: "fan-trap-sum"
    type: "single"
    prompt: "`orders(id, total)`: `(10, 100)`, `(11, 40)`, `(12, 40)`. `order_items(order_id, sku)`: `(10, 'a')`, `(10, 'b')`, `(10, 'c')`, `(11, 'd')`, `(12, 'e')`. A dashboard computes revenue of orders that have items:\n\n```sql\nSELECT SUM(o.total)\nFROM orders o\nJOIN order_items i ON i.order_id = o.id;\n```\n\nWhat does it return?"
    explanation: |-
      The join produces one row per order *item*, and each row carries its order's total. Summing those rows counts an order's total once per item:

      | order | total | items | contribution |
      |---|---|---|---|
      | 10 | 100 | 3 | 300 |
      | 11 | 40 | 1 | 40 |
      | 12 | 40 | 1 | 40 |
      | sum | | | 380 |

      This is the *fan trap*: aggregating a "one" side after joining it to a "many" side multiplies its values by the fan-out.

      "180" is the correct revenue (100 + 40 + 40), which the query does *not* compute.

      "140" is what `SUM(DISTINCT o.total)` would return: it de-duplicates the values 100, 40, 40 into 100 and 40.

      "900" is the cross-join result: every total (sum 180) repeated for all five item rows. The `ON` condition pairs each order only with its own items.
    options:
      - text: "180"
      - text: "140"
      - text: "380"
        correct: true
      - text: "900"
  - id: "fan-trap-fixes"
    type: "multiple"
    prompt: "Same `orders` and `order_items`. Which queries correctly return 180, the revenue of orders that have at least one item? Select all that apply."
    explanation: |-
      Both correct queries make the "many" side contribute at most one row per order before summing. `EXISTS` is a semi-join; joining to `SELECT DISTINCT order_id` pre-aggregates the item side to one row per order. Each returns 100 + 40 + 40 = 180.

      Distractor traces:

      - `SUM(DISTINCT o.total)` sums the distinct *values* 100 and 40 = **140**. It only looks right when no two orders share a total; `DISTINCT` hides the fan-out on some data and corrupts the answer on other data.
      - `SUM(o.total) / COUNT(*)` = 380 / 5 = **76**, an average per item row, not a revenue.
      - The `LEFT JOIN` keeps the same fan-out: **380** again.

      General fix: aggregate each child table to the parent's grain in a subquery first, then join.
    options:
      - text: "`SELECT SUM(DISTINCT o.total) FROM orders o JOIN order_items i ON i.order_id = o.id`"
      - text: "`SELECT SUM(o.total) FROM orders o WHERE EXISTS (SELECT 1 FROM order_items i WHERE i.order_id = o.id)`"
        correct: true
      - text: "`SELECT SUM(o.total) FROM orders o JOIN (SELECT DISTINCT order_id FROM order_items) i ON i.order_id = o.id`"
        correct: true
      - text: "`SELECT SUM(o.total) / COUNT(*) FROM orders o JOIN order_items i ON i.order_id = o.id`"
      - text: "`SELECT SUM(o.total) FROM orders o LEFT JOIN order_items i ON i.order_id = o.id`"
  - id: "exists-ignores-select-list"
    type: "single"
    prompt: "Using the same `customers` and `orders` (ann has two orders, bob one, cy none), what does this return?\n\n```sql\nSELECT c.name\nFROM customers c\nWHERE EXISTS (SELECT NULL FROM orders o WHERE o.customer_id = c.id);\n```"
    explanation: |-
      `EXISTS (subquery)` is TRUE when the subquery returns at least one row and FALSE when it returns none; it never looks at the values in those rows. `SELECT NULL`, `SELECT 1` and `SELECT *` are equivalent inside `EXISTS`. `EXISTS` is also never UNKNOWN, unlike `IN`.

      | customer | rows in subquery | EXISTS |
      |---|---|---|
      | ann | 2 (each a NULL) | TRUE |
      | bob | 1 | TRUE |
      | cy | 0 | FALSE |

      "No rows" assumes a row containing NULL counts as "nothing"; a row is a row.

      "`ann`, `ann`, `bob`" treats `EXISTS` like a join; it is a semi-join, so ann appears once.

      "`ann`, `bob`, `cy`" treats the subquery as uncorrelated.
    options:
      - text: "`ann`, `bob`"
        correct: true
      - text: "No rows, because the subquery selects only NULL"
      - text: "`ann`, `ann`, `bob`"
      - text: "`ann`, `bob`, `cy`"
  - id: "correlated-scalar-subquery"
    type: "single"
    prompt: "Same data. What does this return?\n\n```sql\nSELECT c.name,\n       (SELECT MAX(o.total) FROM orders o WHERE o.customer_id = c.id) AS biggest\nFROM customers c;\n```"
    explanation: |-
      A correlated subquery is defined as if it were evaluated once per outer row, with `c.id` bound to that row's value. In the select list it is a scalar subquery: zero rows yield NULL, and more than one row would be an error, which `MAX` prevents by aggregating.

      | customer | orders matched | MAX(total) |
      |---|---|---|
      | ann | 100, 40 | 100 |
      | bob | 40 | 40 |
      | cy | none | NULL |

      "Without cy" treats the scalar subquery as a filter; it never removes outer rows.

      "100 for everyone" ignores the correlation and computes the global maximum.

      "An error" would be right for `SELECT o.total` without an aggregate, because ann has two orders; `MAX` makes it a single value.

      "`(ann, 140)`" adds ann's two orders (100 + 40); that would be `SUM`, not `MAX`.
    options:
      - text: "`(ann, 100)`, `(bob, 40)`"
      - text: "`(ann, 100)`, `(bob, 100)`, `(cy, 100)`"
      - text: "An error, because the subquery can match more than one order"
      - text: "`(ann, 100)`, `(bob, 40)`, `(cy, NULL)`"
        correct: true
      - text: "`(ann, 140)`, `(bob, 40)`, `(cy, NULL)`"
  - id: "outer-reference-capture"
    type: "single"
    prompt: "`vip_list` has a single column `customer_id` and one row, `(2)`. A developer runs this before a `DELETE` with the same predicate:\n\n```sql\nSELECT name FROM customers\nWHERE id IN (SELECT id FROM vip_list);\n```\n\nWith customers ann (1), bob (2) and cy (3), what does it return?"
    explanation: |-
      SQL resolves a column name in a subquery against the subquery's own `FROM` first and then against enclosing queries. `vip_list` has no `id`, so `id` silently becomes an *outer reference* to `customers.id`, and the subquery becomes correlated: for each customer it returns that customer's own `id`, once per row of `vip_list`.

      | customer | subquery result | `id IN (...)` |
      |---|---|---|
      | ann (1) | {1} | TRUE |
      | bob (2) | {2} | TRUE |
      | cy (3) | {3} | TRUE |

      Every customer qualifies, so the follow-up `DELETE` would delete them all (and if `vip_list` were empty, none).

      "`bob`" is the intended result, achievable with `SELECT customer_id FROM vip_list` or, better, a qualified `SELECT v.customer_id FROM vip_list v`.

      "An error" is what developers expect; it happens only if the name exists nowhere in scope.

      "No rows" misunderstands name resolution.

      "After a warning" assumes the engine sees an ambiguity; there is none, because only one table in scope has an `id`, and resolution is silent.

      Qualify every column in subqueries with a table alias to make this bug impossible.
    options:
      - text: "`bob`, because the subquery returns `2`"
      - text: "An error, because `vip_list` has no column named `id`"
      - text: "No rows, because the unknown column makes the subquery empty"
      - text: "`bob`, after the engine warns that `id` is ambiguous between the two tables"
      - text: "`ann`, `bob`, `cy`, because `id` in the subquery resolves to the outer `customers.id`"
        correct: true
---
## Learning objectives

- Distinguish a join from a semi-join (`EXISTS`, `IN`) by how they treat multiple matches.
- Diagnose and fix the fan trap, where a join multiplies the values being aggregated.
- Explain why `DISTINCT` and `SUM(DISTINCT ...)` are not fixes for fan-out.
- Recognize outer-reference capture in subqueries and prevent it with qualified names.

## The invariant

Relational algebra has two different operators that are easy to confuse in SQL:

| Operator | SQL forms | Output for a left row with k matches |
|---|---|---|
| Join (⋈) | `JOIN ... ON` | k rows (the left row paired with each match) |
| Semi-join (⋉) | `EXISTS`, `IN (subquery)`, `= ANY` | 1 row if k ≥ 1, else 0 |
| Anti-join (▷) | `NOT EXISTS`, `LEFT JOIN ... IS NULL` | 1 row if k = 0, else 0 |

When the question is "which parents have a child that ...", the answer is a semi-join. A join gives the right rows but in the wrong *multiplicity*, which is harmless for a display and catastrophic for an aggregate.

The **fan trap** follows directly from bag semantics (Chapter 1): joining a parent to a child table repeats each parent row once per child, so any aggregate over parent columns after the join is multiplied by the fan-out. Joining two independent child tables to the same parent (orders to items *and* to payments) multiplies them by each other, the "chasm trap".

Correlated subqueries are defined per outer row: the standard specifies the result as if the subquery were evaluated with the outer columns bound to the current row's values. The optimizer usually unnests them into joins or semi-joins, but the per-row definition is what determines the result. Name resolution searches inner scopes first, then outer ones, so a misspelled inner column can silently bind to the outer query.

## Worked example

`orders`: 10 (total 100), 11 (40), 12 (40). `order_items`: three for 10, one each for 11 and 12.

```sql
-- Wrong: fan trap, 380
SELECT SUM(o.total)
FROM orders o JOIN order_items i ON i.order_id = o.id;

-- Wrong in general: hides the bug on some data, 140 here
SELECT SUM(DISTINCT o.total)
FROM orders o JOIN order_items i ON i.order_id = o.id;

-- Right: semi-join, 180
SELECT SUM(o.total)
FROM orders o
WHERE EXISTS (SELECT 1 FROM order_items i WHERE i.order_id = o.id);

-- Right: pre-aggregate the child to the parent's grain, then join
SELECT SUM(o.total) AS revenue, SUM(i.n_items) AS items
FROM orders o
JOIN (SELECT order_id, COUNT(*) AS n_items
      FROM order_items GROUP BY order_id) AS i
  ON i.order_id = o.id;                 -- revenue 180, items 5
```

The last form generalizes: when a report needs aggregates from several child tables, aggregate each child separately to the parent key and join the results, so that every join is one-to-one.

## Where this shows up today

As of November 2026, the MySQL 8.4 manual describes semijoin and antijoin transformations for `IN` and `EXISTS` subqueries, and PostgreSQL plans `NOT EXISTS` as an anti-join and `EXISTS` either as a semi-join or as a join against a de-duplicated inner side (observed with `EXPLAIN` on PostgreSQL 16.15). Either way, the optimizer preserves semi-join semantics; only the written form decides whether you asked for a join or a semi-join. BI tools that join fact tables at different grains are a common modern source of fan traps.

## Pitfalls checklist

- Writing `JOIN` when the question is "has at least one", then adding `DISTINCT` to hide duplicates.
- Summing parent columns after joining child rows.
- Joining two child tables to one parent in the same query before aggregating.
- Using `SUM(DISTINCT ...)` as a fan-out fix; it removes equal values, not repeated rows.
- Leaving subquery columns unqualified, so a typo binds to the outer query.

## Key takeaways

- A join repeats the left row per match; a semi-join (`EXISTS`, `IN`) never does.
- Aggregate each child table to the parent's grain before joining it.
- `EXISTS` ignores the subquery's select list and is never UNKNOWN.
- Qualify every column in a subquery with its table alias.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), `<exists predicate>`, `<in predicate>`, scalar subqueries and column reference resolution.
- W. Kim, "On Optimizing an SQL-like Nested Query", *ACM TODS* 7(3), 1982.
- MySQL 8.4 Reference Manual, "Optimizing IN and EXISTS Subquery Predicates with Semijoin and Antijoin Transformations"; PostgreSQL 19 documentation, "Subquery Expressions".
