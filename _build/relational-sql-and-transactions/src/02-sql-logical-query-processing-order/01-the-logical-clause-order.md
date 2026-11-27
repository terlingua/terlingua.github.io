---
title: "The Logical Order of Query Clauses"
questions:
  - id: "clauses-before-select"
    type: "multiple"
    prompt: "In SQL's logical query processing order, which clauses are evaluated *before* the `SELECT` list? Select all that apply."
    correct:
      - "`WHERE`"
      - "`GROUP BY`"
      - "`HAVING`"
    distractors:
      - "`DISTINCT`"
      - "`ORDER BY`"
    explanation: |-
      ISO SQL defines a query specification as a pipeline in which each clause consumes the table produced by the previous one:

      | Step | Clause | Produces |
      |---|---|---|
      | 1 | `FROM` (with `JOIN ... ON`) | a table from the sources |
      | 2 | `WHERE` | the rows satisfying the predicate |
      | 3 | `GROUP BY` | groups |
      | 4 | `HAVING` | the groups satisfying the predicate |
      | 5 | `SELECT` (including window functions) | output columns |
      | 6 | `DISTINCT` | duplicate-free output |
      | 7 | `ORDER BY` | ordered output |
      | 8 | `OFFSET` / `FETCH` (`LIMIT`) | a slice of the ordered output |

      `WHERE`, `GROUP BY` and `HAVING` come before `SELECT`. `DISTINCT` operates on the rows the `SELECT` list produced, so it must come after; `ORDER BY` sorts the final output, so it comes later still.
  - id: "distinct-order-by-hidden-column"
    type: "single"
    prompt: "In PostgreSQL, what happens when you run this query?\n\n```sql\nSELECT DISTINCT city\nFROM customers\nORDER BY signup_date;\n```"
    correct:
      - "It fails, because after `DISTINCT` one `city` row can stand for several `signup_date` values, so the sort key is undefined"
    distractors:
      - "It returns each city once, ordered by the earliest `signup_date` of that city"
      - "It returns each city once, ordered by the latest `signup_date` of that city"
      - "It returns one row per customer, because `ORDER BY` runs before `DISTINCT`"
    explanation: |-
      `DISTINCT` (step 6) collapses rows that are equal on the select list, and `ORDER BY` (step 7) can only sort what `DISTINCT` produced. If customers in Lyon signed up on three different dates, the single output row `Lyon` has no single `signup_date` to sort by. The SQL standard therefore requires `ORDER BY` items to appear in the select list when `DISTINCT` is used, and PostgreSQL raises `for SELECT DISTINCT, ORDER BY expressions must appear in select list`.

      "Ordered by the earliest" and "ordered by the latest" invent an aggregate that the query never asked for. To get that, write `GROUP BY city ORDER BY MIN(signup_date)`.

      "One row per customer, because `ORDER BY` runs before `DISTINCT`" reverses the logical order.

      Dialect note: SQLite accepts this query and sorts by an arbitrary row's `signup_date` from each group, which is exactly the undefined behavior the standard forbids.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE customers(city TEXT, signup_date DATE);"
        query: "SELECT DISTINCT city FROM customers ORDER BY signup_date"
        expect_error: "42P10"
      - engine: "sqlite"
        setup: "CREATE TABLE customers(city TEXT, signup_date TEXT); INSERT INTO customers VALUES ('Lyon','2026-01-01'),('Oslo','2026-02-01');"
        query: "SELECT DISTINCT city FROM customers ORDER BY signup_date"
        expect: [["Lyon"], ["Oslo"]]
        ordered: true
  - id: "where-then-top-n"
    type: "single"
    prompt: "`players(name, team, score)` contains `('ann', 'red', 90)`, `('bob', 'blue', 95)`, `('cid', 'red', 70)`, `('dee', 'red', 85)`. What does this return?\n\n```sql\nSELECT name\nFROM players\nWHERE team = 'red'\nORDER BY score DESC\nLIMIT 2;\n```"
    correct:
      - "`ann`, `dee`"
    distractors:
      - "`bob`, `ann`"
      - "`ann`, `cid`"
      - "`ann`"
    explanation: |-
      Follow the logical order: `WHERE` (step 2) runs before `ORDER BY` (step 7), and `LIMIT` (step 8) takes the first rows of the sorted output.

      | Step | Rows |
      |---|---|
      | FROM | ann 90, bob 95, cid 70, dee 85 |
      | WHERE team = 'red' | ann 90, cid 70, dee 85 |
      | ORDER BY score DESC | ann 90, dee 85, cid 70 |
      | LIMIT 2 | ann, dee |

      "`bob`, `ann`" ignores the `WHERE` clause and returns the overall top two; `bob` is on the blue team.

      "`ann`, `cid`" filters correctly but takes the first two red rows in stored order, as if `LIMIT` ran before `ORDER BY`.

      "`ann`" applies `LIMIT 2` to the stored table first (ann, bob) and filters afterwards, which reverses steps 2 and 8.
    checks:
      - engine: "both"
        setup: "CREATE TABLE players(name TEXT, team TEXT, score INT); INSERT INTO players VALUES ('ann','red',90),('bob','blue',95),('cid','red',70),('dee','red',85);"
        query: "SELECT name FROM players WHERE team = 'red' ORDER BY score DESC LIMIT 2"
        expect: [["ann"], ["dee"]]
        ordered: true
  - id: "offset-counts-filtered-rows"
    type: "single"
    prompt: "`items(id, active)` contains ids 1 to 6, with `active` true for ids 1, 2, 4, 5, 6 and false for id 3. What does `SELECT id FROM items WHERE active ORDER BY id LIMIT 2 OFFSET 2` return?"
    correct:
      - "`4`, `5`"
    distractors:
      - "`3`, `4`"
      - "`5`, `6`"
      - "`4`"
    explanation: |-
      `OFFSET` and `LIMIT` are applied last, to the filtered and sorted result, not to the stored table.

      | Step | ids |
      |---|---|
      | WHERE active | 1, 2, 4, 5, 6 |
      | ORDER BY id | 1, 2, 4, 5, 6 |
      | OFFSET 2 (skip two rows) | 4, 5, 6 |
      | LIMIT 2 | 4, 5 |

      "`3`, `4`" skips two *stored* rows (1, 2) and ignores the filter, as if `OFFSET` ran before `WHERE`.

      "`5`, `6`" skips three rows; `OFFSET 2` skips exactly two.

      "`4`" slices the stored table first (ids 3 and 4) and then filters out the inactive id 3, which reverses the order of steps 2 and 8.
    checks:
      - engine: "both"
        setup: "CREATE TABLE items(id INT, active BOOLEAN); INSERT INTO items VALUES (1,TRUE),(2,TRUE),(3,FALSE),(4,TRUE),(5,TRUE),(6,TRUE);"
        query: "SELECT id FROM items WHERE active ORDER BY id LIMIT 2 OFFSET 2"
        expect: [[4], [5]]
        ordered: true
  - id: "cross-product-then-where"
    type: "single"
    prompt: "Table `a(x)` has 3 rows with `x` values `1`, `1`, `2`. Table `b(y)` has 4 rows. How many rows does `SELECT COUNT(*) FROM a, b WHERE a.x = 1` return as its count?"
    correct:
      - "8"
    distractors:
      - "2"
      - "4"
      - "12"
    explanation: |-
      The `FROM` clause with a comma is a cross join: logically it produces every pairing, 3 × 4 = 12 rows. `WHERE a.x = 1` then keeps the pairings whose `a` row has `x = 1`. Two `a` rows qualify, each paired with all 4 `b` rows: 2 × 4 = 8.

      | Step | Rows |
      |---|---|
      | FROM a, b | 12 |
      | WHERE a.x = 1 | 8 |
      | COUNT(*) | one row containing 8 |

      "2" counts qualifying rows of `a` alone, forgetting the product with `b`.

      "4" counts `b`, as if the condition selected one `a` row.

      "12" ignores the `WHERE` clause.

      Physically, an engine will filter `a` first and never build 12 rows; the logical order defines the result, not the plan.
    checks:
      - engine: "both"
        setup: "CREATE TABLE a(x INT); CREATE TABLE b(y INT); INSERT INTO a VALUES (1),(1),(2); INSERT INTO b VALUES (1),(2),(3),(4);"
        query: "SELECT COUNT(*) FROM a, b WHERE a.x = 1"
        expect: [[8]]
  - id: "distinct-after-expressions"
    type: "single"
    prompt: "`people(name)` contains `'Ann'`, `'ann'`, `'Bob'`. How many rows does `SELECT DISTINCT lower(name) FROM people` return?"
    correct:
      - "2"
    distractors:
      - "3"
      - "1"
      - "An error, because `DISTINCT` needs a plain column"
    explanation: |-
      The `SELECT` list (step 5) computes `lower(name)` for every row first, producing `ann`, `ann`, `bob`. `DISTINCT` (step 6) then removes duplicates from those computed rows, leaving `ann` and `bob`: 2 rows.

      "3" applies `DISTINCT` to the stored values (`Ann`, `ann`, `Bob` are distinct strings) before the expression, which reverses steps 5 and 6.

      "1" would require collapsing every name into one value.

      "An error" is wrong: `DISTINCT` applies to whatever the select list produces, expressions included.
    checks:
      - engine: "both"
        setup: "CREATE TABLE people(name TEXT); INSERT INTO people VALUES ('Ann'),('ann'),('Bob');"
        query: "SELECT COUNT(*) FROM (SELECT DISTINCT lower(name) FROM people) t"
        expect: [[2]]
---
## Learning objectives

- Recite the logical evaluation order of the clauses of a `SELECT` statement.
- Predict which clause can see which columns and rows.
- Trace `WHERE`, `ORDER BY`, `LIMIT` and `OFFSET` on a small table.
- Separate the logical order (which defines results) from the plan (which computes them).

## The invariant

SQL is declarative, but its meaning is defined procedurally. ISO/IEC 9075 specifies a query specification as a sequence of table-to-table transformations; each clause receives the *virtual table* produced by the clause before it:

| Step | Clause | Input → output |
|---|---|---|
| 1 | `FROM`, including `JOIN ... ON` | source tables → one joined table |
| 2 | `WHERE` | rows → rows for which the predicate is TRUE |
| 3 | `GROUP BY` | rows → groups |
| 4 | `HAVING` | groups → groups for which the predicate is TRUE |
| 5 | `SELECT` list (and window functions) | rows or groups → output rows |
| 6 | `DISTINCT` | output rows → duplicate-free output rows |
| 7 | `ORDER BY` | output rows → ordered output |
| 8 | `OFFSET` / `FETCH FIRST` (`LIMIT` in many dialects) | ordered output → a slice |

Set operators (`UNION` and friends) combine whole query specifications after step 6 of each branch; an `ORDER BY` and `FETCH` at the end apply to the combined result.

Three consequences follow mechanically. A clause can only use what earlier steps produced, so `WHERE` cannot use an aggregate or a window function (they do not exist yet). `ORDER BY` sorts the output of `DISTINCT`, so after `DISTINCT` it may only sort by output columns. `LIMIT` and `OFFSET` count rows of the final, sorted output, never rows of the stored table.

The standard spelling for step 8 is `OFFSET n ROWS FETCH FIRST m ROWS ONLY` (SQL:2008). `LIMIT` is a widespread non-standard equivalent. Without `ORDER BY`, a `FETCH` or `LIMIT` returns *some* m rows, with no guarantee which.

## Worked example

```sql
SELECT team, COUNT(*) AS n
FROM players
WHERE score >= 80
GROUP BY team
HAVING COUNT(*) >= 2
ORDER BY n DESC
FETCH FIRST 1 ROWS ONLY;
```

With `players`: `('ann', 'red', 90)`, `('bob', 'blue', 95)`, `('cid', 'red', 70)`, `('dee', 'red', 85)`, `('eve', 'blue', 60)`:

| Step | Result |
|---|---|
| FROM | 5 rows |
| WHERE score >= 80 | ann, bob, dee |
| GROUP BY team | red: {ann, dee}; blue: {bob} |
| HAVING COUNT(*) >= 2 | red |
| SELECT | ('red', 2) |
| ORDER BY, FETCH | ('red', 2) |

Moving `score >= 80` from `WHERE` into `HAVING` would be an error in standard SQL, because `score` is neither grouped nor aggregated; Chapter 4 covers that rule. Reading a query in the order above, rather than top to bottom, is the single most effective habit for predicting SQL results.

## Where this shows up today

As of November 2026, PostgreSQL 19 and SQL Server 2025 accept the standard `OFFSET ... FETCH` form, while the MySQL 8.4 and SQLite manuals document `LIMIT ... OFFSET`; SQL Server also has its own `TOP`. Whatever the spelling, the clause sits at step 8. Query builders and ORMs emit these clauses for paging, so the logical order is what explains their results.

## Pitfalls checklist

- Reading a query top to bottom and assuming `SELECT` runs first.
- Expecting `LIMIT` to sample the table rather than the filtered, sorted result.
- Paging with `LIMIT`/`OFFSET` without a deterministic `ORDER BY` (rows repeat or vanish between pages).
- Sorting by a non-selected column after `DISTINCT` in dialects that allow it and getting arbitrary order.
- Assuming the engine executes in the logical order; it only promises the same result.

## Key takeaways

- The logical order is FROM, WHERE, GROUP BY, HAVING, SELECT, DISTINCT, ORDER BY, OFFSET/FETCH.
- Each clause sees only what earlier clauses produced.
- `ORDER BY` after `DISTINCT` may only use output columns.
- `LIMIT`/`FETCH` slice the final ordered result.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), query specification and query expression semantics; result offset and fetch first clauses (SQL:2008).
- PostgreSQL 19 documentation, "SELECT"; Microsoft Learn, "SELECT - ORDER BY Clause (Transact-SQL)"; MySQL 8.4 Reference Manual, "SELECT Statement"; SQLite documentation, "SELECT".
