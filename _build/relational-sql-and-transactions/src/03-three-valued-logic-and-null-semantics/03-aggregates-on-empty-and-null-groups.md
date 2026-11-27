---
title: "Aggregates on Empty and All-NULL Input"
questions:
  - id: "count-star-vs-count-col"
    type: "single"
    prompt: "`bonuses(emp, bonus)` has four rows with `bonus` values `100`, `NULL`, `50`, `NULL`. What do `COUNT(*)` and `COUNT(bonus)` return?"
    correct:
      - "`COUNT(*)` = 4, `COUNT(bonus)` = 2"
    distractors:
      - "`COUNT(*)` = 4, `COUNT(bonus)` = 4"
      - "`COUNT(*)` = 2, `COUNT(bonus)` = 2"
      - "`COUNT(*)` = 4, `COUNT(bonus)` = 3"
    explanation: |-
      ISO SQL defines `COUNT(*)` as the number of rows, and `COUNT(expr)` as the number of rows for which `expr` is not NULL. Every general set function other than `COUNT(*)` first eliminates NULL arguments (the standard raises a warning, "null value eliminated in set function", that most clients never show).

      | bonus | counted by COUNT(*) | counted by COUNT(bonus) |
      |---|---|---|
      | 100 | yes | yes |
      | NULL | yes | no |
      | 50 | yes | yes |
      | NULL | yes | no |
      | total | 4 | 2 |

      "4 and 4" treats `COUNT(bonus)` like `COUNT(*)`.

      "2 and 2" assumes `COUNT(*)` skips rows containing NULLs; it counts rows regardless of content.

      "4 and 3" counts the NULLs once, as if they were one distinct value; that is not what `COUNT(col)` does (and `COUNT(DISTINCT bonus)` would give 2, ignoring NULLs too).
    checks:
      - engine: "both"
        setup: "CREATE TABLE bonuses(emp TEXT, bonus INT); INSERT INTO bonuses VALUES ('a',100),('b',NULL),('c',50),('d',NULL);"
        query: "SELECT COUNT(*), COUNT(bonus), COUNT(DISTINCT bonus) FROM bonuses"
        expect: [[4, 2, 2]]
  - id: "sum-all-null-group"
    type: "single"
    prompt: "`bonuses(dept, bonus)` holds `('a', 100)`, `('a', 50)`, `('b', NULL)`, `('b', NULL)`. What does `SELECT dept, SUM(bonus) FROM bonuses GROUP BY dept` return?"
    correct:
      - "`('a', 150)` and `('b', NULL)`"
    distractors:
      - "`('a', 150)` and `('b', 0)`"
      - "`('a', 150)` only, because group `b` has nothing to sum"
      - "`('a', NULL)` and `('b', NULL)`, because the column contains NULLs"
    explanation: |-
      `SUM` eliminates NULL arguments and then sums what is left. For group `b` nothing is left, and the standard defines the result of `SUM`, `AVG`, `MIN` and `MAX` over an empty set of values as NULL.

      | dept | values after removing NULLs | SUM |
      |---|---|---|
      | a | 100, 50 | 150 |
      | b | (none) | NULL |

      "`('b', 0)`" is what most people expect; SQL's sum of nothing is NULL, not 0. Use `COALESCE(SUM(bonus), 0)` when 0 is the right business answer.

      "Only `a`" confuses an all-NULL group with a missing group: group `b` exists because it has rows.

      "`('a', NULL)`" applies arithmetic NULL propagation (`100 + NULL` is NULL) to aggregates, which skip NULLs instead.
    checks:
      - engine: "both"
        setup: "CREATE TABLE bonuses(dept TEXT, bonus INT); INSERT INTO bonuses VALUES ('a',100),('a',50),('b',NULL),('b',NULL);"
        query: "SELECT dept, SUM(bonus) FROM bonuses GROUP BY dept"
        expect: [["a", 150], ["b", null]]
  - id: "scalar-aggregate-empty-input"
    type: "single"
    prompt: "`orders` has rows, but none for 2031. What does `SELECT COUNT(*), SUM(total) FROM orders WHERE year = 2031` return?"
    correct:
      - "One row: `(0, NULL)`"
    distractors:
      - "No rows"
      - "One row: `(0, 0)`"
      - "One row: `(NULL, NULL)`"
    explanation: |-
      An aggregate query *without* `GROUP BY` treats the whole filtered input as a single group, even when that input is empty, so it always returns exactly one row. In that row, `COUNT(*)` of an empty set is 0, while `SUM` of an empty set is NULL.

      | Step | Result |
      |---|---|
      | WHERE year = 2031 | 0 rows |
      | implicit single group | one (empty) group |
      | COUNT(*) | 0 |
      | SUM(total) | NULL |

      "No rows" is the behavior of a *grouped* query (see the next question).

      "`(0, 0)`" assumes `SUM` behaves like `COUNT`; only the `COUNT` functions return 0 on empty input.

      "`(NULL, NULL)`" assumes `COUNT` can be NULL; it never is.
    checks:
      - engine: "both"
        setup: "CREATE TABLE orders(year INT, total INT); INSERT INTO orders VALUES (2025, 10),(2026, 20);"
        query: "SELECT COUNT(*), SUM(total) FROM orders WHERE year = 2031"
        expect: [[0, null]]
  - id: "group-by-empty-input"
    type: "single"
    prompt: "Same table. What does `SELECT year, COUNT(*) FROM orders WHERE year = 2031 GROUP BY year` return?"
    correct:
      - "No rows"
    distractors:
      - "One row: `(2031, 0)`"
      - "One row: `(NULL, 0)`"
      - "One row per existing year, each with count 0"
    explanation: |-
      `GROUP BY` forms one group per distinct grouping value present in its input. The `WHERE` clause left no rows, so there are no grouping values, no groups, and no output rows. A group cannot be "found" for a value that never occurs.

      | Step | Result |
      |---|---|
      | WHERE year = 2031 | 0 rows |
      | GROUP BY year | 0 groups |
      | SELECT | 0 rows |

      "`(2031, 0)`" expects groups for values named in the `WHERE` clause; to report zero counts for a list of years, outer-join a calendar or a `VALUES` list to the data.

      "`(NULL, 0)`" mixes this up with the single implicit group of an ungrouped aggregate.

      "One row per existing year" would require the `WHERE` filter to run after grouping, which reverses the logical order.
    checks:
      - engine: "both"
        setup: "CREATE TABLE orders(year INT, total INT); INSERT INTO orders VALUES (2025, 10),(2026, 20);"
        query: "SELECT year, COUNT(*) FROM orders WHERE year = 2031 GROUP BY year"
        expect: []
  - id: "avg-ignores-nulls"
    type: "single"
    prompt: "`bonus` holds `100`, `NULL`, `50`, `NULL`. What are `AVG(bonus)` and `AVG(COALESCE(bonus, 0))`?"
    correct:
      - "75 and 37.5"
    distractors:
      - "37.5 and 37.5"
      - "75 and 75"
      - "NULL and 37.5"
    explanation: |-
      `AVG` eliminates NULLs, so it divides the sum of the remaining values by their count: (100 + 50) / 2 = 75. `COALESCE(bonus, 0)` turns the NULLs into zeros *before* aggregation, so all four rows take part: (100 + 0 + 50 + 0) / 4 = 37.5.

      | Expression | values aggregated | sum | count | average |
      |---|---|---|---|---|
      | AVG(bonus) | 100, 50 | 150 | 2 | 75 |
      | AVG(COALESCE(bonus, 0)) | 100, 0, 50, 0 | 150 | 4 | 37.5 |

      Which one is right depends on meaning: "average bonus among people who received one" versus "average bonus per employee, counting none as zero".

      "37.5 and 37.5" assumes `AVG` counts NULL rows in the denominator.

      "75 and 75" assumes `COALESCE` changes nothing.

      "NULL and 37.5" applies arithmetic NULL propagation to an aggregate.
    checks:
      - engine: "both"
        setup: "CREATE TABLE b(bonus INT); INSERT INTO b VALUES (100),(NULL),(50),(NULL);"
        query: "SELECT AVG(bonus), AVG(COALESCE(bonus, 0) * 1.0) FROM b"
        expect: [[75, 37.5]]
  - id: "zero-instead-of-null"
    type: "multiple"
    prompt: "`t(x)` has rows, but the filter `WHERE x > 1000` matches none. Which queries return `0` rather than NULL? Select all that apply."
    correct:
      - "`SELECT COALESCE(SUM(x), 0) FROM t WHERE x > 1000`"
      - "`SELECT COUNT(x) FROM t WHERE x > 1000`"
    distractors:
      - "`SELECT SUM(COALESCE(x, 0)) FROM t WHERE x > 1000`"
      - "`SELECT MIN(COALESCE(x, 0)) FROM t WHERE x > 1000`"
      - "`SELECT AVG(COALESCE(x, 0)) FROM t WHERE x > 1000`"
    explanation: |-
      The empty-input result depends on the *aggregate*, not on the values: `COUNT` returns 0 on empty input; `SUM`, `MIN`, `MAX` and `AVG` return NULL.

      - `COALESCE(SUM(x), 0)` applies `COALESCE` *after* aggregation, to the NULL that `SUM` produced: 0.
      - `COUNT(x)` over no rows: 0.

      The distractors apply `COALESCE` *inside* the aggregate. That replaces NULL values in rows, but there are no rows: the aggregate still sees an empty set and returns NULL. All three return NULL.

      Rule: `COALESCE` inside an aggregate handles NULL *values*; `COALESCE` outside handles *empty input*.
    checks:
      - engine: "both"
        setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (1),(2),(NULL);"
        query: "SELECT COALESCE(SUM(x), 0), COUNT(x), SUM(COALESCE(x, 0)), MIN(COALESCE(x, 0)), AVG(COALESCE(x, 0)) FROM t WHERE x > 1000"
        expect: [[0, 0, null, null, null]]
body_checks:
  - engine: "both"
    setup: "CREATE TABLE bonuses(dept TEXT, bonus INT); INSERT INTO bonuses VALUES ('a',100),('a',NULL),('a',50),('b',NULL);"
    query: "SELECT dept, COUNT(*), COUNT(bonus), SUM(bonus), AVG(bonus), AVG(COALESCE(bonus, 0)), COALESCE(SUM(bonus), 0), SUM(bonus) FILTER (WHERE bonus > 500) FROM bonuses GROUP BY dept"
    expect: [["a", 3, 2, 150, 75, 50, 150, null], ["b", 1, 0, null, null, 0, 0, null]]
---
## Learning objectives

- Distinguish `COUNT(*)` from `COUNT(expr)` on columns that contain NULL.
- Predict `SUM`, `AVG`, `MIN` and `MAX` over all-NULL groups and empty input.
- Explain why an ungrouped aggregate always returns one row while a grouped one can return none.
- Place `COALESCE` inside or outside an aggregate according to what it should fix.

## The invariant

ISO SQL's general set functions share one rule: **NULL arguments are eliminated before the function is applied**. Only `COUNT(*)` is exempt, because it counts rows, not values. After elimination:

| Function | Result on a non-empty set of values | Result on an empty set |
|---|---|---|
| `COUNT(*)` | number of rows | 0 |
| `COUNT(expr)`, `COUNT(DISTINCT expr)` | number of non-NULL (distinct) values | 0 |
| `SUM`, `AVG`, `MIN`, `MAX` | the usual result | NULL |

"Empty" arises in two ways: an all-NULL group (rows exist but every value is NULL) and an empty input (no rows at all). Both give NULL for `SUM` and friends.

The number of output rows follows from grouping:

- **Without `GROUP BY`**, the filtered input forms exactly one group, even if it is empty. The query returns exactly one row.
- **With `GROUP BY`**, one group is formed per distinct grouping value that occurs in the input. An empty input has no groups and returns no rows.

This asymmetry explains why `SELECT COUNT(*) FROM t WHERE false` returns `0` but `SELECT k, COUNT(*) FROM t WHERE false GROUP BY k` returns nothing.

Theory and standard differ in one respect worth naming: mathematically, an empty sum is 0. SQL chose NULL, treating "no values" as "no answer". The standard requires a completion-condition warning when NULLs are eliminated; engines vary in whether they surface it.

## Worked example

`bonuses(dept, bonus)`: `('a', 100)`, `('a', NULL)`, `('a', 50)`, `('b', NULL)`.

```sql
SELECT dept,
       COUNT(*)                  AS n_rows,
       COUNT(bonus)              AS n_values,
       SUM(bonus)                AS total,
       AVG(bonus)                AS avg_given,
       AVG(COALESCE(bonus, 0))   AS avg_per_row,
       COALESCE(SUM(bonus), 0)   AS total_or_zero
FROM bonuses
GROUP BY dept;
```

| dept | n_rows | n_values | total | avg_given | avg_per_row | total_or_zero |
|---|---|---|---|---|---|---|
| a | 3 | 2 | 150 | 75 | 50 | 150 |
| b | 1 | 0 | NULL | NULL | 0 | 0 |

The standard `FILTER` clause applies a condition per aggregate and follows the same empty-set rules: `SUM(bonus) FILTER (WHERE bonus > 500)` is NULL for both groups.

## Where this shows up today

As of November 2026, the PostgreSQL 19 documentation still warns that, except for `count`, aggregate functions return NULL when no rows are selected, and that `sum` of no rows is NULL rather than zero. PostgreSQL and SQLite (3.53.1 in this book's checks) support the standard `FILTER (WHERE ...)` clause. Dashboards that show blank tiles instead of "0" usually trace back to this rule.

## Pitfalls checklist

- Using `COUNT(col)` when the question is "how many rows", or `COUNT(*)` when it is "how many have a value".
- Expecting `SUM` over no rows to be 0.
- Writing `SUM(COALESCE(x, 0))` to avoid a NULL total; it does not help when there are no rows.
- Expecting a grouped query to return zero-count rows for values that do not occur.
- Averaging with `AVG(col)` when missing values should count as zero, or the reverse.

## Key takeaways

- Every aggregate except `COUNT(*)` ignores NULLs.
- `COUNT` of nothing is 0; `SUM`, `AVG`, `MIN`, `MAX` of nothing is NULL.
- No `GROUP BY` means exactly one output row; `GROUP BY` over no rows means none.
- `COALESCE` inside an aggregate fixes NULL values; outside, it fixes empty input.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), `<set function specification>` and `<group by clause>`.
- PostgreSQL 19 documentation, "Aggregate Functions"; SQLite documentation, "Built-in Aggregate Functions".
