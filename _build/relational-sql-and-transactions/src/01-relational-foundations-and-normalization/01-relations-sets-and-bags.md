---
title: "Relations Are Sets, SQL Tables Are Bags"
questions:
  - id: "union-vs-union-all"
    type: "single"
    prompt: "Table `a` holds the `x` values `1`, `1`, `2`. Table `b` holds `2`, `3`. How many rows do `SELECT x FROM a UNION SELECT x FROM b` and `SELECT x FROM a UNION ALL SELECT x FROM b` return?"
    correct:
      - "`UNION` returns 3 rows; `UNION ALL` returns 5 rows"
    distractors:
      - "`UNION` returns 4 rows; `UNION ALL` returns 5 rows"
      - "`UNION` returns 3 rows; `UNION ALL` returns 3 rows"
      - "`UNION` returns 2 rows; `UNION ALL` returns 5 rows"
    explanation: |-
      ISO SQL defines `UNION` as `UNION DISTINCT`: the result is the set of distinct rows that appear in either input. `UNION ALL` is bag union: it keeps every copy from both inputs.

      Trace:

      | Operation | Rows produced | Count |
      |---|---|---|
      | `UNION ALL` | 1, 1, 2, 2, 3 | 5 |
      | `UNION` | 1, 2, 3 | 3 |

      "`UNION` returns 4 rows" assumes `UNION` only removes rows duplicated *across* the two inputs (the shared `2`) and keeps the duplicate `1` that came from inside `a`. Duplicate elimination applies to the whole result, including duplicates within one operand.

      "`UNION ALL` returns 3 rows" treats `ALL` as a no-op; it is the keyword that switches off duplicate elimination.

      "`UNION` returns 2 rows" confuses union with intersection; only `2` is common to both tables, but union keeps values from either side.
    checks:
      - engine: "both"
        setup: "CREATE TABLE a(x INT); CREATE TABLE b(x INT); INSERT INTO a VALUES (1),(1),(2); INSERT INTO b VALUES (2),(3);"
        query: "SELECT (SELECT COUNT(*) FROM (SELECT x FROM a UNION SELECT x FROM b) u), (SELECT COUNT(*) FROM (SELECT x FROM a UNION ALL SELECT x FROM b) v)"
        expect: [[3, 5]]
  - id: "except-all-multiplicity"
    type: "single"
    prompt: "Table `a` holds `x` values `1`, `1`, `1`, `2`. Table `b` holds `1`, `2`, `2`. What does `SELECT x FROM a EXCEPT ALL SELECT x FROM b` return in an engine that implements the standard `EXCEPT ALL`?"
    correct:
      - "Two rows: `1`, `1`"
    distractors:
      - "No rows, because every value of `a` also appears in `b`"
      - "One row: `1`"
      - "Three rows: `1`, `1`, `1`"
    explanation: |-
      ISO SQL defines `EXCEPT ALL` on multiplicities: if a row occurs m times in the left input and n times in the right input, it occurs max(m − n, 0) times in the result.

      | Value | m (in `a`) | n (in `b`) | max(m − n, 0) |
      |---|---|---|---|
      | 1 | 3 | 1 | 2 |
      | 2 | 1 | 2 | 0 |

      So the result is `1`, `1`.

      "No rows" is the answer for plain `EXCEPT` (that is, `EXCEPT DISTINCT`), which asks only whether each distinct value of `a` appears in `b` at all.

      "One row: `1`" applies distinct semantics to the result after subtracting, which the `ALL` form does not do.

      "Three rows: `1`, `1`, `1`" forgets that each copy in `b` cancels one copy in `a`.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE a(x INT); CREATE TABLE b(x INT); INSERT INTO a VALUES (1),(1),(1),(2); INSERT INTO b VALUES (1),(2),(2);"
        query: "SELECT x FROM a EXCEPT ALL SELECT x FROM b"
        expect: [[1], [1]]
      - engine: "postgres"
        setup: "CREATE TABLE a(x INT); CREATE TABLE b(x INT); INSERT INTO a VALUES (1),(1),(1),(2); INSERT INTO b VALUES (1),(2),(2);"
        query: "SELECT COUNT(*) FROM (SELECT x FROM a EXCEPT SELECT x FROM b) d"
        expect: [[0]]
  - id: "bag-join-multiplies"
    type: "single"
    prompt: "Table `t1(k)` holds `1`, `1`, `2`. Table `t2(k)` holds `1`, `1`, `1`, `3`. How many rows does `SELECT * FROM t1 JOIN t2 ON t1.k = t2.k` return?"
    correct:
      - "6"
    distractors:
      - "2"
      - "3"
      - "7"
    explanation: |-
      A join is a filtered Cartesian product. Over bags, every left row pairs with every matching right row, so a key that appears m times on the left and n times on the right contributes m × n result rows.

      | k | copies in `t1` | copies in `t2` | rows |
      |---|---|---|---|
      | 1 | 2 | 3 | 6 |
      | 2 | 1 | 0 | 0 |
      | 3 | 0 | 1 | 0 |

      Total: 6.

      "2" counts the matching left rows only, as if the join were a semi-join (`EXISTS`).

      "3" counts the matching right rows only.

      "7" adds the row counts (2 + 3 = 5) plus the two unmatched keys, which mixes up join with union and ignores that unmatched rows disappear from an inner join.
    checks:
      - engine: "both"
        setup: "CREATE TABLE t1(k INT); CREATE TABLE t2(k INT); INSERT INTO t1 VALUES (1),(1),(2); INSERT INTO t2 VALUES (1),(1),(1),(3);"
        query: "SELECT COUNT(*) FROM t1 JOIN t2 ON t1.k = t2.k"
        expect: [[6]]
  - id: "no-order-by-no-order"
    type: "single"
    prompt: "A report query has no `ORDER BY`. For months it returned rows in `id` order; after an index was added, the order changed. What does the SQL standard say about the original behavior?"
    correct:
      - "Any row order was a correct result; without `ORDER BY` the order is implementation-dependent"
    distractors:
      - "The engine broke the standard, which requires insertion order when no `ORDER BY` is given"
      - "The engine broke the standard, which requires primary-key order when no `ORDER BY` is given"
      - "Order is guaranteed only while the table has no secondary indexes"
    explanation: |-
      A relation is a set of tuples, and sets have no order (Codd 1970). SQL keeps that rule for query results: ISO/IEC 9075 makes the order of rows implementation-dependent unless an `ORDER BY` is applied to the outermost query. The old order was a side effect of the plan (for example a scan of a clustered primary key), and a new index allowed a different plan.

      "Insertion order" is not a property tables have; deleted space is reused and pages are reorganized.

      "Primary-key order" is a common observation for engines that store tables clustered by key, but it is a plan detail, not a guarantee; parallel scans and index-only scans break it.

      "Only while the table has no secondary indexes" invents a rule; no plan property is promised in any case.
  - id: "duplicate-eliminating-operators"
    type: "multiple"
    prompt: "Which of these SQL constructs remove duplicate rows from their result by default, with no extra keyword? Select all that apply."
    correct:
      - "`UNION`"
      - "`INTERSECT`"
      - "`EXCEPT`"
    distractors:
      - "`UNION ALL`"
      - "`SELECT` without `DISTINCT`"
    explanation: |-
      ISO SQL makes `DISTINCT` the default quantifier for the three set operators: `UNION` means `UNION DISTINCT`, `INTERSECT` means `INTERSECT DISTINCT`, and `EXCEPT` means `EXCEPT DISTINCT`. So the set-operator keywords on their own behave like Codd's set algebra.

      `SELECT`, in contrast, defaults to `ALL`: a projection keeps every row, so `SELECT city FROM customers` returns one row per customer even if many share a city. That is why "`SELECT` without `DISTINCT`" is wrong.

      "`UNION ALL`" is wrong because `ALL` explicitly asks for bag semantics and keeps every copy.

      The asymmetry (set operators default to `DISTINCT`, `SELECT` defaults to `ALL`) is a frequent source of surprise and of accidental sort or hash steps in plans.
    checks:
      - engine: "sqlite"
        setup: "CREATE TABLE a(x INT); INSERT INTO a VALUES (1),(1);"
        query: "SELECT (SELECT COUNT(*) FROM (SELECT x FROM a INTERSECT SELECT x FROM a)), (SELECT COUNT(*) FROM (SELECT x FROM a EXCEPT SELECT 2)), (SELECT COUNT(*) FROM (SELECT x FROM a))"
        expect: [[1, 1, 2]]
  - id: "count-of-projection"
    type: "single"
    prompt: "A developer wants the number of distinct customers who placed orders and writes:\n\n```sql\nSELECT COUNT(*)\nFROM (SELECT customer_id FROM orders) AS t;\n```\n\nThe `customer_id` values in `orders` are `10`, `10`, `20`, `30`, `30`, `30`. What does the query return?"
    correct:
      - "6"
    distractors:
      - "3"
      - "1"
      - "An error, because a derived table must use `DISTINCT`"
    explanation: |-
      The derived table is a projection, and SQL projection keeps duplicates (bag semantics). It therefore holds six rows, one per order: 10, 10, 20, 30, 30, 30. `COUNT(*)` counts rows, so the result is 6.

      In relational algebra, projection onto `customer_id` would produce the set {10, 20, 30}, whose cardinality is 3. SQL needs `SELECT DISTINCT customer_id` or `COUNT(DISTINCT customer_id)` to get that answer. That is why "3" is what the developer wanted but not what the query computes.

      "1" would be the number of rows if the query grouped everything into a single group and selected the group, which this query does not.

      The "error" option is wrong: derived tables are ordinary query expressions and may contain duplicates.
    checks:
      - engine: "both"
        setup: "CREATE TABLE orders(customer_id INT); INSERT INTO orders VALUES (10),(10),(20),(30),(30),(30);"
        query: "SELECT (SELECT COUNT(*) FROM (SELECT customer_id FROM orders) AS t), (SELECT COUNT(DISTINCT customer_id) FROM orders)"
        expect: [[6, 3]]
---
## Learning objectives

- State the difference between a mathematical relation (a set) and an SQL table (a bag, or multiset).
- Predict row counts for `UNION`, `UNION ALL`, `INTERSECT [ALL]` and `EXCEPT [ALL]`.
- Explain why a join over bags multiplies duplicates.
- Explain why a result without `ORDER BY` has no defined order.

## The invariant

E. F. Codd's 1970 paper defines a relation as a *set* of n-tuples over named domains. Two consequences follow directly from "set": a relation never contains the same tuple twice, and its tuples have no order. Relational algebra is closed over sets: projection, union, difference and join all produce sets again, so projection removes duplicates automatically.

SQL deliberately departed from this. ISO/IEC 9075 defines a table as a *multiset* (a bag): a collection in which a row can occur any number of times. Each row has a *multiplicity*, and the operators are defined on multiplicities:

| Operator | Multiplicity of a row in the result (m in left, n in right) |
|---|---|
| `UNION ALL` | m + n |
| `INTERSECT ALL` | min(m, n) |
| `EXCEPT ALL` | max(m − n, 0) |
| `UNION`, `INTERSECT`, `EXCEPT` (the `DISTINCT` forms) | 1 if the `ALL` form gives at least 1, else 0 |
| inner join on a key | m × n for each matching key value |

Two defaults matter. The set operators default to `DISTINCT`: `UNION` means `UNION DISTINCT`. `SELECT` defaults to `ALL`: a projection keeps duplicates unless you write `DISTINCT`. The standard also says that without an `ORDER BY` on the outermost query, the order of rows is implementation-dependent.

Keep the three layers apart. The standard specifies bag semantics and the two defaults. The theory (Codd) says relations are sets, which is why keys exist: a declared key is how you make an SQL table behave like a relation. An engine decides how to execute duplicate elimination (sort or hash) and what order rows happen to come out in.

Why did SQL choose bags? Duplicate elimination costs a sort or a hash over the whole result, and some questions need duplicates: `SUM(amount)` over a projection of amounts must count two equal payments twice. The price is that the programmer must decide, at every projection and set operation, whether duplicates are meaningful.

## Worked example

Two tables of tags attached to articles, deliberately without keys:

| `draft_tags.tag` | `live_tags.tag` |
|---|---|
| sql | sql |
| sql | joins |
| nulls | |

```sql
SELECT tag FROM draft_tags
UNION ALL
SELECT tag FROM live_tags;   -- sql, sql, nulls, sql, joins   (5 rows)

SELECT tag FROM draft_tags
UNION
SELECT tag FROM live_tags;   -- sql, nulls, joins             (3 rows)

SELECT tag FROM draft_tags
INTERSECT ALL
SELECT tag FROM live_tags;   -- sql                           (min(2, 1) = 1 row)

SELECT tag FROM draft_tags
EXCEPT ALL
SELECT tag FROM live_tags;   -- sql, nulls                    (sql: 2 - 1 = 1; nulls: 1 - 0 = 1)
```

Now join the two on `tag`. `sql` appears twice on the left and once on the right, so it contributes 2 × 1 = 2 rows; `nulls` and `joins` have no partner and disappear. The join returns two identical rows `(sql, sql)`. Had `live_tags` also contained `sql` twice, the join would return four. This multiplication is the root of the "my totals doubled after I added a join" bug that Chapter 2 returns to.

None of these results has an order. A plan that sorts for duplicate elimination may return `joins, nulls, sql`; a hash-based plan may return any permutation. Both are correct.

## Where this shows up today

As of November 2026, the PostgreSQL 19 and MySQL 8.4 manuals document `INTERSECT ALL` and `EXCEPT ALL`; SQL Server's `INTERSECT` and `EXCEPT` return distinct rows only; and SQLite accepts `UNION ALL` but rejects `INTERSECT ALL` and `EXCEPT ALL` (checked against SQLite 3.53.1). Where the `ALL` forms are missing, emulate them by numbering copies with `ROW_NUMBER()` per value and comparing numbered rows. Data-frame libraries and many analytics engines also default to bag semantics, so the same multiplicity arithmetic applies there.

## Pitfalls checklist

- Writing `UNION` where `UNION ALL` was meant, paying for a sort or hash and silently merging rows that were legitimately equal (two identical payments).
- Writing `UNION ALL` where duplicates are wrong, for example when the two branches overlap.
- Counting a projection (`COUNT(*)` over a subquery) and expecting distinct entities.
- Relying on "natural" row order from a scan, an index or insertion; add `ORDER BY`.
- Assuming a join cannot change the number of rows on the "one" side; it multiplies by the number of matches.

## Key takeaways

- Codd's relations are sets; SQL tables and results are bags with per-row multiplicities.
- `UNION`, `INTERSECT` and `EXCEPT` default to `DISTINCT`; `SELECT` defaults to `ALL`.
- `UNION ALL` adds multiplicities, `INTERSECT ALL` takes the minimum, `EXCEPT ALL` subtracts with a floor of zero, and a join multiplies.
- Only `ORDER BY` on the outermost query defines row order.

## Sources

- E. F. Codd, "A Relational Model of Data for Large Shared Data Banks", *Communications of the ACM* 13(6), 1970.
- ISO/IEC 9075-2 (SQL/Foundation), query expressions and set operators.
- PostgreSQL 19 documentation, "Combining Queries (UNION, INTERSECT, EXCEPT)"; MySQL 8.4 Reference Manual, "EXCEPT Clause" and "INTERSECT Clause"; Microsoft Learn, "Set Operators - EXCEPT and INTERSECT (Transact-SQL)".
