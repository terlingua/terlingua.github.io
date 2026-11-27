---
title: "Ranking Functions and Top-N per Group"
questions:
  - id: "rank-dense-rank-gap"
    type: "single"
    prompt: "In department `a`, salaries are ann 100, bob 90, cy 90, di 80. Using `ORDER BY salary DESC` within the department, what are `RANK()` and `DENSE_RANK()` for di?"
    explanation: |-
      `RANK()` is 1 plus the number of rows that sort strictly before the current row; tied rows share a rank, and the next rank skips by the size of the tie. `DENSE_RANK()` is 1 plus the number of *distinct* values before the current row, so it never skips.

      | name | salary | rows strictly before | distinct values before | RANK | DENSE_RANK |
      |---|---|---|---|---|---|
      | ann | 100 | 0 | 0 | 1 | 1 |
      | bob | 90 | 1 | 1 | 2 | 2 |
      | cy | 90 | 1 | 1 | 2 | 2 |
      | di | 80 | 3 | 2 | 4 | 3 |

      "3 and 3" gives `RANK` the dense behavior.

      "4 and 4" gives `DENSE_RANK` the gap.

      "3 and 4" swaps the two definitions.
    options:
      - text: "`RANK()` = 3, `DENSE_RANK()` = 3"
      - text: "`RANK()` = 4, `DENSE_RANK()` = 3"
        correct: true
      - text: "`RANK()` = 4, `DENSE_RANK()` = 4"
      - text: "`RANK()` = 3, `DENSE_RANK()` = 4"
  - id: "top-two-with-ties"
    type: "single"
    prompt: "`emp(dept, name, salary)`: department `a` has ann 100, bob 90, cy 90, di 80; department `b` has ed 70, gus 70, fay 60. How many rows does each query return?\n\n```sql\nSELECT * FROM (\n  SELECT e.*, RANK() OVER (PARTITION BY dept ORDER BY salary DESC) AS r\n  FROM emp e) t\nWHERE r <= 2;          -- Q1\n\nSELECT * FROM (\n  SELECT e.*, ROW_NUMBER() OVER (PARTITION BY dept ORDER BY salary DESC) AS r\n  FROM emp e) t\nWHERE r <= 2;          -- Q2\n```"
    explanation: |-
      | dept | name | salary | RANK | ROW_NUMBER |
      |---|---|---|---|---|
      | a | ann | 100 | 1 | 1 |
      | a | bob | 90 | 2 | 2 or 3 |
      | a | cy | 90 | 2 | 3 or 2 |
      | a | di | 80 | 4 | 4 |
      | b | ed | 70 | 1 | 1 or 2 |
      | b | gus | 70 | 1 | 2 or 1 |
      | b | fay | 60 | 3 | 3 |

      Q1 keeps RANK ≤ 2: ann, bob, cy, ed, gus = **5 rows**. Q2 keeps exactly two rows per department = **4 rows** (which of bob and cy survives is not determined).

      "4 and 4" assumes `RANK` also cuts ties.

      "6 and 4" counts fay, whose rank is 3 (two rows rank ahead of her).

      "5 and 5" assumes `ROW_NUMBER` assigns equal numbers to ties; it never does.

      Pick the function by the business rule: "top 2 people, ties broken somehow" is `ROW_NUMBER`; "everyone in the top 2 salary positions" is `RANK`; "everyone earning one of the top 2 salaries" is `DENSE_RANK`.
    options:
      - text: "Q1: 4 rows; Q2: 4 rows"
      - text: "Q1: 6 rows; Q2: 4 rows"
      - text: "Q1: 5 rows; Q2: 4 rows"
        correct: true
      - text: "Q1: 5 rows; Q2: 5 rows"
  - id: "window-function-in-where"
    type: "single"
    prompt: "What happens when you run this in PostgreSQL?\n\n```sql\nSELECT dept, name\nFROM emp\nWHERE ROW_NUMBER() OVER (PARTITION BY dept ORDER BY salary DESC) <= 2;\n```"
    explanation: |-
      Window functions are evaluated with the `SELECT` list (step 5), after `WHERE` (step 2), `GROUP BY` and `HAVING`. When `WHERE` runs, row numbers do not exist yet. The standard forbids window functions in `WHERE`, `GROUP BY` and `HAVING`; PostgreSQL raises `window functions are not allowed in WHERE` (SQLSTATE 42P20).

      The fix is to compute the window function in a derived table or CTE and filter in the outer query, as in the previous question.

      "Returns the top two" is the intent, not the semantics.

      "Two rows in total" invents an evaluation for an illegal query.

      "Requires an alias" is half right about the fix (you will filter on an alias), but the reason is the logical order, and an alias would not be visible in `WHERE` either.
    options:
      - text: "It returns the top two earners per department"
      - text: "It returns two rows in total, because `WHERE` sees the whole table as one partition"
      - text: "It fails, because `ROW_NUMBER` requires an alias before it can be compared"
      - text: "It fails, because window functions are computed in the `SELECT` step, after `WHERE`"
        correct: true
  - id: "row-number-ties-nondeterministic"
    type: "single"
    prompt: "bob and cy both earn 90 in department `a`, second only to ann. What does SQL guarantee about `ROW_NUMBER() OVER (PARTITION BY dept ORDER BY salary DESC)` for them?"
    explanation: |-
      `ROW_NUMBER` numbers rows 1, 2, 3, ... in the window order. When the `ORDER BY` has ties, the relative order of tied rows is implementation-dependent, so the assignment of 2 and 3 between bob and cy is not determined. A different plan (an index added, a parallel scan) can swap them, which makes "top-1 per group" queries flicker.

      "Inserted first" relies on physical storage order, which SQL never promises.

      "Alphabetically" invents a tie-breaker; add one yourself, for example `ORDER BY salary DESC, name`, to make the numbering deterministic.

      "Both get 2" describes `RANK`/`DENSE_RANK`, not `ROW_NUMBER`.
    options:
      - text: "They receive 2 and 3, but which of them gets 2 can differ between executions"
        correct: true
      - text: "bob gets 2 because his row was inserted first"
      - text: "bob gets 2 because `ROW_NUMBER` breaks ties alphabetically by the other columns"
      - text: "Both get 2, because `ROW_NUMBER` assigns equal numbers to equal values"
  - id: "fetch-with-ties"
    type: "single"
    prompt: "Same seven employees. What does this return in PostgreSQL?\n\n```sql\nSELECT name FROM emp\nWHERE dept = 'a'\nORDER BY salary DESC\nFETCH FIRST 2 ROWS WITH TIES;\n```"
    explanation: |-
      `FETCH FIRST n ROWS WITH TIES` (SQL:2008) returns the first n rows of the ordered result *plus* any further rows that tie with the n-th row on the `ORDER BY` key.

      | position | name | salary | kept? |
      |---|---|---|---|
      | 1 | ann | 100 | yes |
      | 2 | bob or cy | 90 | yes (n-th row) |
      | 3 | cy or bob | 90 | yes, ties with row 2 |
      | 4 | di | 80 | no |

      "Two rows: ann and either bob or cy" is `FETCH FIRST 2 ROWS ONLY` (or `LIMIT 2`).

      "ann and bob" additionally assumes a deterministic tie order.

      "Four rows" would require di to tie with the second row.

      `WITH TIES` requires `ORDER BY`, and it gives a top-N over the *whole* result; per-group top-N still needs a window function.
    options:
      - text: "Two rows: ann and either bob or cy"
      - text: "Three rows: ann, bob and cy"
        correct: true
      - text: "Two rows: ann and bob"
      - text: "Four rows: every employee of department `a`"
  - id: "all-top-earners-per-dept"
    type: "multiple"
    prompt: "Same seven employees (ann 100 tops `a`; ed and gus tie at 70 in `b`). Which queries return *every* employee who earns their department's maximum salary (ann, ed, gus)? Select all that apply."
    explanation: |-
      `RANK() = 1` keeps every row with no strictly greater salary in its department: ann (a), ed and gus (b, tied). The correlated `MAX` subquery states the same thing directly: salary equals the department maximum. Both return ann, ed, gus.

      Distractors:

      - `ROW_NUMBER() = 1` keeps exactly one row per department, arbitrarily dropping either ed or gus: 2 rows.
      - `GROUP BY dept` with a bare `name` is invalid in standard SQL (`name` is not functionally dependent on `dept`); lenient engines would return one arbitrary name per department.
      - `FETCH FIRST 1 ROWS WITH TIES` ranks the whole table, not each department: it returns only ann (100).
    options:
      - text: "Filter `r = 1` on `ROW_NUMBER() OVER (PARTITION BY dept ORDER BY salary DESC) AS r` computed in a derived table"
      - text: "`SELECT dept, name, MAX(salary) FROM emp GROUP BY dept`"
      - text: "Filter `r = 1` on `RANK() OVER (PARTITION BY dept ORDER BY salary DESC) AS r` computed in a derived table"
        correct: true
      - text: "`SELECT dept, name FROM emp e WHERE salary = (SELECT MAX(salary) FROM emp x WHERE x.dept = e.dept)`"
        correct: true
      - text: "`SELECT dept, name FROM emp ORDER BY salary DESC FETCH FIRST 1 ROWS WITH TIES`"
---
## Learning objectives

- Compute `ROW_NUMBER`, `RANK` and `DENSE_RANK` by hand, including ties.
- Write top-N-per-group queries and choose the ranking function that matches the business rule.
- Explain why window functions cannot be filtered in `WHERE`.
- Make ranking deterministic with a unique ordering.

## The invariant

The standard ranking functions are defined over the ordered partition of each row:

| Function | Value for the current row | Ties | Gaps |
|---|---|---|---|
| `ROW_NUMBER()` | position in the window order (1, 2, 3, ...) | distinct numbers, arbitrary among ties | none |
| `RANK()` | 1 + rows strictly before | equal | yes, skips by tie size |
| `DENSE_RANK()` | 1 + distinct values strictly before | equal | none |

Two rules from the logical order decide how they are used:

1. Window functions are computed at step 5, so they cannot be referenced in `WHERE`, `GROUP BY` or `HAVING`. Filtering on a rank requires a derived table or CTE around the windowed query.
2. Ranking functions ignore frames; only partitioning and ordering matter.

If the window `ORDER BY` is not unique, `ROW_NUMBER` assigns tied rows an implementation-dependent order. That is the source of "top-1 per group" results that change between runs.

`FETCH FIRST n ROWS WITH TIES` (SQL:2008) is the whole-result counterpart of `RANK() <= n`: it keeps rows tying with the n-th row.

Choosing the function is a business decision, not a technical one. "The top three salespeople" usually means `ROW_NUMBER` with an explicit tie-breaker, because a prize list needs exactly three names. "Everyone in the top three positions" means `RANK`, which may return more than three rows when people tie. "Everyone earning one of the three highest amounts" means `DENSE_RANK`. Writing the rule down before writing the query avoids a surprisingly common class of reporting bugs.

## Worked example

Top two salaries per department, three ways:

```sql
WITH ranked AS (
  SELECT dept, name, salary,
         ROW_NUMBER() OVER w AS rn,
         RANK()       OVER w AS rk,
         DENSE_RANK() OVER w AS drk
  FROM emp
  WINDOW w AS (PARTITION BY dept ORDER BY salary DESC)
)
SELECT * FROM ranked WHERE rk <= 2 ORDER BY dept, rk;
```

| dept | name | salary | rn | rk | drk |
|---|---|---|---|---|---|
| a | ann | 100 | 1 | 1 | 1 |
| a | bob | 90 | 2 | 2 | 2 |
| a | cy | 90 | 3 | 2 | 2 |
| a | di | 80 | 4 | 4 | 3 |
| b | ed | 70 | 1 | 1 | 1 |
| b | gus | 70 | 2 | 1 | 1 |
| b | fay | 60 | 3 | 3 | 2 |

(The `rn` values among tied rows may be swapped.) Filtering `rk <= 2` keeps ann, bob, cy, ed, gus; `rn <= 2` keeps two per department; `drk <= 2` keeps ann, bob, cy, ed, gus and fay, because fay earns the second-highest *distinct* salary in `b`. The named `WINDOW` clause is standard and avoids repeating the window definition.

## Where this shows up today

As of November 2026, `FETCH FIRST ... WITH TIES` is available in PostgreSQL (since version 13, so including 19), and SQL Server's `TOP (n) WITH TIES` does the same; MySQL 8.4 and SQLite use `LIMIT`, which has no tie option. Some analytics engines add a non-standard `QUALIFY` clause that filters on window functions without a derived table; it is shorthand for the same two-step logic.

## Pitfalls checklist

- Using `ROW_NUMBER() = 1` for "the top earners" and silently dropping ties.
- Ranking on a non-unique ordering and getting results that change between runs.
- Putting a window function in `WHERE`.
- Confusing `RANK` gaps with `DENSE_RANK` and getting the wrong "second place".
- Using `WITH TIES` or `LIMIT` for a per-group top-N.

## Key takeaways

- `ROW_NUMBER` never ties, `RANK` ties with gaps, `DENSE_RANK` ties without gaps.
- Compute ranks in a derived table or CTE, then filter.
- Add a unique tie-breaker to the window `ORDER BY` for deterministic numbering.
- `WITH TIES` extends a whole-result top-N to include ties.

## Sources

- ISO/IEC 9075-2:2003 (`<window function>`: `RANK`, `DENSE_RANK`, `ROW_NUMBER`; `WINDOW` clause) and ISO/IEC 9075-2:2008 (`<fetch first clause>` with `WITH TIES`).
- PostgreSQL 13 release notes; PostgreSQL 19 documentation, "Window Functions" and "SELECT"; Microsoft Learn, "TOP (Transact-SQL)".
