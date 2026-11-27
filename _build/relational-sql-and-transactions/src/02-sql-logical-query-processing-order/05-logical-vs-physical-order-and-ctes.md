---
title: "Logical Order Versus Physical Plans, and CTEs"
questions:
  - id: "no-short-circuit-guarantee"
    type: "multiple"
    prompt: "`lines(qty, total)` contains a row with `qty = 0`. Which filters are guaranteed by the semantics of SQL never to raise a division-by-zero error, whatever plan the optimizer picks? Select all that apply."
    correct:
      - "`WHERE CASE WHEN qty = 0 THEN NULL ELSE total / qty END > 10`"
      - "`WHERE total / NULLIF(qty, 0) > 10`"
    distractors:
      - "`WHERE qty <> 0 AND total / qty > 10`"
      - "`WHERE total / qty > 10 AND qty <> 0`"
      - "`SELECT * FROM (SELECT * FROM lines WHERE qty <> 0) AS s WHERE total / qty > 10`"
    explanation: |-
      SQL does not define the evaluation order of the operands of `AND` or `OR`: the engine may evaluate either side first, or reorder predicates by cost. So `qty <> 0 AND total / qty > 10` may well evaluate the division for the `qty = 0` row, whichever side is written first. The derived-table version is no safer: the optimizer may merge the subquery and push or reorder predicates, because doing so does not change the *defined* result.

      What *is* defined:

      - `CASE` evaluates its `WHEN` conditions in order and evaluates only the chosen branch's result. For `qty = 0` it yields NULL without computing `total / qty`. (PostgreSQL documents `CASE` as the way to force evaluation order, with a caveat about constant subexpressions folded at planning time.)
      - `NULLIF(qty, 0)` returns NULL when `qty = 0`; `total / NULL` is NULL, not an error.

      In both cases the comparison is then `NULL > 10`, which is UNKNOWN, so the row is filtered out. Trace on `(qty 0, total 10)` and `(qty 2, total 50)`: the first row yields UNKNOWN and is dropped; the second gives 25 > 10, TRUE; one row is returned.
    checks:
      - engine: "both"
        setup: "CREATE TABLE lines(qty INT, total INT); INSERT INTO lines VALUES (0,10),(2,50);"
        query: "SELECT (SELECT COUNT(*) FROM lines WHERE CASE WHEN qty = 0 THEN NULL ELSE total / qty END > 10), (SELECT COUNT(*) FROM lines WHERE total / NULLIF(qty, 0) > 10)"
        expect: [[1, 1]]
  - id: "pushdown-inner-join-equivalent"
    type: "single"
    prompt: "`customers`: ann (1), bob (2), cy (3). `orders(id, customer_id, status)`: `(10, 1, 'paid')`, `(11, 1, 'open')`, `(12, 2, NULL)`. How many rows does each query return?\n\n```sql\n-- Q1: filter after the inner join\nSELECT c.name FROM customers c JOIN orders o ON o.customer_id = c.id\nWHERE o.status IS NULL;\n\n-- Q2: filter pushed below the inner join\nSELECT c.name FROM customers c\nJOIN (SELECT * FROM orders WHERE status IS NULL) o ON o.customer_id = c.id;\n```"
    correct:
      - "Q1: 1 row; Q2: 1 row"
    distractors:
      - "Q1: 1 row; Q2: 2 rows"
      - "Q1: 2 rows; Q2: 1 row"
      - "Q1: 3 rows; Q2: 1 row"
    explanation: |-
      For an inner join, a selection on one input commutes with the join: σ_p(A ⋈ B) = A ⋈ σ_p(B) when p mentions only B's columns. That algebraic identity is what lets optimizers push filters down to the scans.

      | Query | Trace | Rows |
      |---|---|---|
      | Q1 | join gives ann–10, ann–11, bob–12; `status IS NULL` keeps bob–12 | bob |
      | Q2 | filter orders to {12}; join gives bob–12 | bob |

      Both return 1 row.

      "Q1: 1; Q2: 2" and "Q1: 2; Q2: 1" assume the two placements differ for inner joins; they cannot.

      "Q1: 3" treats `IS NULL` as if it were evaluated before the join on the customers side.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT); CREATE TABLE orders(id INT, customer_id INT, status TEXT); INSERT INTO customers VALUES (1,'ann'),(2,'bob'),(3,'cy'); INSERT INTO orders VALUES (10,1,'paid'),(11,1,'open'),(12,2,NULL);"
        query: "SELECT (SELECT COUNT(*) FROM customers c JOIN orders o ON o.customer_id = c.id WHERE o.status IS NULL), (SELECT COUNT(*) FROM customers c JOIN (SELECT * FROM orders WHERE status IS NULL) o ON o.customer_id = c.id)"
        expect: [[1, 1]]
  - id: "pushdown-outer-join-changes-result"
    type: "single"
    prompt: "Same data, now with `LEFT JOIN`. How many rows does each query return?\n\n```sql\n-- Q3\nSELECT c.name FROM customers c LEFT JOIN orders o ON o.customer_id = c.id\nWHERE o.status IS NULL;\n\n-- Q4\nSELECT c.name FROM customers c\nLEFT JOIN (SELECT * FROM orders WHERE status IS NULL) o ON o.customer_id = c.id;\n```"
    correct:
      - "Q3: 2 rows; Q4: 3 rows"
    distractors:
      - "Q3: 2 rows; Q4: 2 rows"
      - "Q3: 1 row; Q4: 3 rows"
      - "Q3: 3 rows; Q4: 3 rows"
    explanation: |-
      Below an outer join, a filter on the null-supplying side changes *which rows match*, while above it, the filter also sees the NULLs the join itself created. `IS NULL` is TRUE for those padding NULLs, so it is not null-rejecting and cannot be pushed down.

      Q3: the join produces ann–10 (paid), ann–11 (open), bob–12 (NULL status), cy–NULL (no order). `o.status IS NULL` keeps bob–12 and cy–NULL: **bob, cy** (2 rows).

      Q4: the filtered orders are {12}. ann has no match and is NULL-extended, bob matches 12, cy is NULL-extended: **ann, bob, cy** (3 rows).

      "2 and 2" assumes pushdown is always legal; that holds for inner joins only.

      "1 and 3" forgets that cy's padded row also satisfies `IS NULL` in Q3.

      "3 and 3" treats `WHERE` in Q3 as part of `ON`.

      Optimizers apply this rule: they push down only predicates that are safe for the join type.
    checks:
      - engine: "both"
        setup: "CREATE TABLE customers(id INT, name TEXT); CREATE TABLE orders(id INT, customer_id INT, status TEXT); INSERT INTO customers VALUES (1,'ann'),(2,'bob'),(3,'cy'); INSERT INTO orders VALUES (10,1,'paid'),(11,1,'open'),(12,2,NULL);"
        query: "SELECT (SELECT COUNT(*) FROM customers c LEFT JOIN orders o ON o.customer_id = c.id WHERE o.status IS NULL), (SELECT COUNT(*) FROM customers c LEFT JOIN (SELECT * FROM orders WHERE status IS NULL) o ON o.customer_id = c.id)"
        expect: [[2, 3]]
  - id: "join-reordering-legal"
    type: "single"
    prompt: "A query is written `FROM a JOIN b ON a.id = b.a_id JOIN c ON c.b_id = b.id`. `EXPLAIN` shows the engine joining `c` with `b` first and `a` last. What does this mean for the result?"
    correct:
      - "Nothing; inner joins are commutative and associative, so any join order produces the same bag of rows"
    distractors:
      - "The result may contain extra rows, because `a` is no longer the driving table"
      - "The result is the same only if all three tables have primary keys"
      - "The query is now evaluated as `LEFT JOIN`s, because `a` is joined last"
    explanation: |-
      The logical order says `FROM` is evaluated first and defines its result as the joined table; it does not dictate which pair is joined first. For inner joins, (A ⋈ B) ⋈ C = A ⋈ (B ⋈ C) and A ⋈ B = B ⋈ A (up to column order), so the optimizer chooses the order by estimated cost. Only the row *order* of the output may change, and that was never defined without `ORDER BY`.

      "Extra rows" would make the optimizer incorrect; equivalence is the precondition for any rewrite.

      "Only with primary keys" invents a restriction; the identities hold for bags.

      "Evaluated as `LEFT JOIN`s" confuses order with join type. Outer joins are where reordering is restricted, because they are not associative with inner joins in general.
  - id: "cte-inlined-once"
    type: "single"
    prompt: "In PostgreSQL 19, `big(key PRIMARY KEY, v)` has many rows. Can this query use the primary-key index on `big.key`?\n\n```sql\nWITH w AS (SELECT * FROM big)\nSELECT * FROM w WHERE key = 123;\n```"
    correct:
      - "Yes; a non-recursive, side-effect-free CTE referenced once is folded into the query, so the predicate reaches `big`"
    distractors:
      - "No; every CTE is an optimization fence that is computed in full first"
      - "Only if the CTE is declared `WITH w AS MATERIALIZED`"
      - "Only if the CTE is declared `WITH RECURSIVE`"
    explanation: |-
      Logically, a CTE is a named query expression; nothing in the standard requires computing it separately. PostgreSQL 19's documentation states that a non-recursive, side-effect-free `WITH` query referenced once is folded into the parent query by default, while one referenced more than once is materialized unless written `NOT MATERIALIZED`. Folded, the query becomes `SELECT * FROM big WHERE key = 123`, and `EXPLAIN` shows an index scan on the primary key.

      "Every CTE is an optimization fence" was PostgreSQL's behavior before version 12, and is still the folklore; it no longer describes the default.

      "`MATERIALIZED`" does the opposite: it forces the fence.

      "`RECURSIVE`" CTEs are never inlined.

      This is the one engine-specific fact in this section; the timeless rule is that a CTE's *meaning* is that of the substituted query, whatever the engine does physically.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE big(key INT PRIMARY KEY, v TEXT); INSERT INTO big SELECT g, 'x' FROM generate_series(1, 20000) g; ANALYZE big;"
        query: "EXPLAIN (COSTS OFF) WITH w AS (SELECT * FROM big) SELECT * FROM w WHERE key = 123"
        expect_contains: "Index Scan using big_pkey"
      - engine: "postgres"
        setup: "CREATE TABLE big(key INT PRIMARY KEY, v TEXT); INSERT INTO big SELECT g, 'x' FROM generate_series(1, 20000) g; ANALYZE big;"
        query: "EXPLAIN (COSTS OFF) WITH w AS MATERIALIZED (SELECT * FROM big) SELECT * FROM w WHERE key = 123"
        expect_contains: "CTE Scan"
  - id: "inner-order-by-not-propagated"
    type: "single"
    prompt: "A developer writes `SELECT name FROM (SELECT name FROM players ORDER BY score DESC FETCH FIRST 10 ROWS ONLY) AS top10;` and expects the output sorted by score. What does standard SQL guarantee?"
    correct:
      - "That the rows are a top ten by score (ties at the cutoff aside), but not their order; only the outermost `ORDER BY` orders output"
    distractors:
      - "Both the ten rows and their descending order, because the derived table is sorted"
      - "Neither; `ORDER BY` inside a derived table is ignored entirely, so any ten rows may be returned"
      - "The order, but not which rows, because `FETCH FIRST` runs before `ORDER BY`"
    explanation: |-
      Inside the derived table, `ORDER BY` with `FETCH FIRST` has a defined meaning: it selects ten highest-scoring rows (steps 7 and 8 of that inner query; if several rows tie at the cutoff, which of them make the ten is not determined). But the derived table is then a table in the outer `FROM`, and tables have no order. The outer query has no `ORDER BY`, so its output order is implementation-dependent. Engines are free to, for example, hash-join or parallelize over the derived table.

      "Both" relies on an implementation detail; it often holds in simple plans and breaks when the query grows.

      "Neither" goes too far: the inner `ORDER BY` determines *which* rows `FETCH FIRST` keeps. Without `FETCH`, an inner `ORDER BY` is indeed meaningless, and some engines discard it.

      "`FETCH FIRST` runs before `ORDER BY`" reverses steps 7 and 8.

      Fix: `SELECT name FROM (...) AS top10 ORDER BY score DESC`, selecting `score` in the inner query.
---
## Learning objectives

- Separate the logical order, which defines a query's result, from the physical plan, which computes it.
- Explain why `AND` and `OR` give no short-circuit guarantee and how `CASE` and `NULLIF` do.
- Decide when a filter may be pushed below a join and when that would change the result.
- Predict whether a CTE is inlined or materialized, and why its meaning is the same either way.

## The invariant

The logical processing order (FROM, WHERE, GROUP BY, HAVING, SELECT, DISTINCT, ORDER BY, OFFSET/FETCH) is a *specification of the result*. ISO SQL lets an implementation compute the result any way it likes, provided the result is the one the specification defines. Query optimizers exploit this through algebraic equivalences:

| Rewrite | Valid when |
|---|---|
| Reorder inner joins | always (join is commutative and associative over bags) |
| Push a filter below an inner join | the filter mentions columns of one input only |
| Push a filter below an outer join's optional side | never for non-null-rejecting filters such as `IS NULL`; a null-rejecting filter instead converts the outer join to an inner join |
| Unnest a subquery into a (semi-)join | the semi-join semantics are preserved |
| Inline a CTE or view | it is non-recursive and has no side effects |

Two things are *not* specified, and code must not rely on them. First, the evaluation order of the operands of `AND` and `OR`, so a "guard" condition gives no protection to the other operand. `CASE` is specified to evaluate its conditions in order and only the selected result, so it can guard expressions. Second, any order of rows that is not established by the outermost `ORDER BY`, including an `ORDER BY` inside a derived table, CTE or view.

## Worked example

```sql
-- Not guaranteed: the division may be evaluated for qty = 0
SELECT * FROM lines WHERE qty <> 0 AND total / qty > 10;

-- Guaranteed: NULLIF turns the divisor into NULL; NULL > 10 is UNKNOWN
SELECT * FROM lines WHERE total / NULLIF(qty, 0) > 10;
```

Filter pushdown, using `orders` with statuses paid, open and NULL:

| Query shape | Filter `status IS NULL` placed | Rows |
|---|---|---|
| inner join | after the join | bob |
| inner join | pushed below the join | bob |
| left join | after the join | bob, cy |
| left join | pushed below the join | ann, bob, cy |

The inner-join rows agree; the left-join rows do not, so an optimizer may push the filter in the first case and must not in the second.

CTEs in PostgreSQL 19, with a primary key on `big.key`:

```sql
WITH w AS (SELECT * FROM big)
SELECT * FROM w WHERE key = 123;      -- folded: Index Scan using big_pkey

WITH w AS MATERIALIZED (SELECT * FROM big)
SELECT * FROM w WHERE key = 123;      -- fenced: CTE Scan over all rows
```

Both return the same row; only the work differs.

## Where this shows up today

As of November 2026, the PostgreSQL 19 documentation describes folding a side-effect-free, non-recursive CTE referenced once into its parent, with `MATERIALIZED` and `NOT MATERIALIZED` to override; this default dates from PostgreSQL 12. The MySQL 8.4 manual describes merging or materializing derived tables, views and CTEs by the same optimizer logic. Older advice ("CTEs are always optimization fences") is a vestige of earlier PostgreSQL releases.

## Pitfalls checklist

- Using `a <> 0 AND b / a > x` as a guard; use `CASE` or `NULLIF`.
- Moving `IS NULL` or `COALESCE` filters into a subquery on the optional side of an outer join.
- Assuming the written join order is the execution order, or that changing it changes the result.
- Sorting inside a CTE or view and expecting the outer query to inherit the order.
- Using `MATERIALIZED` CTEs by habit and blocking predicate pushdown.

## Key takeaways

- The logical order defines results; the optimizer may compute them in any equivalent way.
- `AND`/`OR` operand order is unspecified; `CASE` and `NULLIF` are the reliable guards.
- Filters commute with inner joins but not with the optional side of outer joins.
- A CTE means its substituted query; inlining versus materializing is a performance choice.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), general rules on evaluation of `<search condition>` and `<case expression>`.
- C. Galindo-Legaria and A. Rosenthal, "Outerjoin Simplification and Reordering for Query Optimization", *ACM TODS* 22(1), 1997.
- PostgreSQL 19 documentation, "Expression Evaluation Rules" and "WITH Queries (Common Table Expressions)"; PostgreSQL 12 release notes.
- MySQL 8.4 Reference Manual, "Optimizing Derived Tables, View References, and Common Table Expressions with Merging or Materialization".
