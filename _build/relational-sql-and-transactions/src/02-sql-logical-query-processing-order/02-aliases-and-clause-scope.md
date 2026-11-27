---
title: "Aliases and Clause Scope"
questions:
  - id: "alias-in-where"
    type: "single"
    prompt: "In PostgreSQL, what does this query do?\n\n```sql\nSELECT price * qty AS total\nFROM lines\nWHERE total > 100;\n```"
    correct:
      - "It fails with `column \"total\" does not exist`: `WHERE` runs before the `SELECT` list defines `total`"
    distractors:
      - "It returns the lines whose `price * qty` exceeds 100"
      - "It returns every line, because an unknown name such as `total` is treated as NULL and the filter is skipped"
      - "It fails, because an alias may not be given to an arithmetic expression"
    explanation: |-
      A column alias is introduced by the `SELECT` list, which is step 5 of the logical order. `WHERE` is step 2: when it is evaluated, the only names in scope are the columns of the tables in `FROM`. The standard therefore rejects the reference, and PostgreSQL reports `column "total" does not exist` (SQLSTATE 42703).

      "It returns the lines whose `price * qty` exceeds 100" is the developer's intent, and it is what some dialects (for example SQLite) do as an extension, but not standard behavior and not PostgreSQL's.

      "Treated as NULL" invents a rule; unresolved names are errors, never NULLs.

      "An alias may not be given to an expression" is false; naming computed columns is the main purpose of `AS`.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE lines(name TEXT, price INT, qty INT);"
        query: "SELECT price * qty AS total FROM lines WHERE total > 100"
        expect_error: "42703"
  - id: "order-by-output-name-wins"
    type: "single"
    prompt: "Table `t(a, b)` contains `(1, 10)`, `(1, 20)`, `(2, 10)`. In standard SQL (and PostgreSQL), in what order does `SELECT b AS a FROM t ORDER BY a` return its values?"
    correct:
      - "`10`, `10`, `20`"
    distractors:
      - "`10`, `20`, `10`"
      - "`20`, `10`, `10`"
      - "An error, because `a` is ambiguous"
    explanation: |-
      `ORDER BY` runs after the `SELECT` list, so output column names are in scope. When a simple name in `ORDER BY` matches both an output column and an input column, the standard (and PostgreSQL) choose the **output** column. Here the output column `a` is `t.b`.

      | Row | input a | input b = output a |
      |---|---|---|
      | 1 | 1 | 10 |
      | 2 | 1 | 20 |
      | 3 | 2 | 10 |

      Sorting by output `a` (the `b` values) gives 10, 10, 20.

      "`10`, `20`, `10`" sorts by the input column `t.a` (1, 1, 2) and prints `b`, which is the `GROUP BY` resolution rule applied to `ORDER BY`.

      "`20`, `10`, `10`" is a descending sort that was not requested.

      "An error" is wrong: the ambiguity is resolved by a defined rule, not rejected.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE t(a INT, b INT); INSERT INTO t VALUES (1,10),(1,20),(2,10);"
        query: "SELECT b AS a FROM t ORDER BY a"
        expect: [[10], [10], [20]]
        ordered: true
  - id: "portable-alias-filter"
    type: "multiple"
    prompt: "You need to filter on `total` from `SELECT price * qty AS total FROM lines`. Which rewrites are valid, portable SQL? Select all that apply."
    correct:
      - "Wrap the query in a derived table or CTE and filter `total` in the outer `WHERE`"
      - "Repeat the expression in the same query: `WHERE price * qty > 100`"
    distractors:
      - "Add `HAVING total > 100` to the same query with no `GROUP BY`"
      - "Write `WHERE 1 > 100`, using the ordinal position of the select-list column"
      - "Quote the alias so it is treated as a column: `WHERE \"total\" > 100`"
    explanation: |-
      Both correct rewrites respect the logical order. A derived table (`SELECT * FROM (SELECT price * qty AS total FROM lines) AS x WHERE total > 100`) or a CTE turns the alias into a real column of the outer query's `FROM`, which its `WHERE` can see. Repeating the expression filters on source columns, which `WHERE` can always see; the optimizer evaluates it once per row anyway.

      "`HAVING total > 100`" is not standard: `HAVING` also runs before the `SELECT` list, and without `GROUP BY` the whole table is one group, so `total` would not even be a grouped value. MySQL accepts aliases in `HAVING` as an extension, which is why this pattern circulates.

      "`WHERE 1 > 100`" compares the integer literal 1 with 100; ordinals are interpreted only in `ORDER BY` (and as an extension in some `GROUP BY` implementations).

      "Quoting the alias" only changes case sensitivity of the identifier; the name is still out of scope.
    checks:
      - engine: "both"
        setup: "CREATE TABLE lines(name TEXT, price INT, qty INT); INSERT INTO lines VALUES ('x',10,5),('y',30,4),('z',60,2);"
        query: "SELECT name FROM (SELECT name, price * qty AS total FROM lines) AS x WHERE total > 100"
        expect: [["y"], ["z"]]
  - id: "sqlite-alias-extension"
    type: "single"
    prompt: "The query `SELECT price * qty AS total FROM lines WHERE total > 100` fails in PostgreSQL 19 but returns the expected rows in SQLite 3.53.1. What explains the difference?"
    correct:
      - "SQLite resolves select-list aliases in `WHERE` as a non-standard extension; standard scoping makes it an error"
    distractors:
      - "SQLite evaluates `SELECT` before `WHERE`, so its logical order differs from the standard's"
      - "PostgreSQL has a bug; the standard allows select-list aliases in every clause that comes after `FROM` in the query text"
      - "SQLite treats `total` as a built-in function name, so the comparison works by accident"
    explanation: |-
      The standard scope rule is fixed: select-list aliases are visible in `ORDER BY` only. SQLite deliberately goes further and substitutes the aliased expression when a name in `WHERE` (or `GROUP BY`, `HAVING`) matches no source column. The *result* is defined as if the expression were written out, so the logical order is unchanged; only name resolution is extended.

      "SQLite evaluates `SELECT` before `WHERE`" confuses name resolution with evaluation order; SQLite still filters rows before producing output.

      "PostgreSQL has a bug" reverses the roles: PostgreSQL follows the standard here.

      "`total` is a built-in" is wrong: SQLite's `total()` is an aggregate function, and the alias resolution is what makes the query work.

      Portable code should not depend on this extension, which is the practical lesson.
    checks:
      - engine: "sqlite"
        setup: "CREATE TABLE lines(name TEXT, price INT, qty INT); INSERT INTO lines VALUES ('x',10,5),('y',30,4),('z',60,2);"
        query: "SELECT price * qty AS total FROM lines WHERE total > 100"
        expect: [[120], [120]]
      - engine: "postgres"
        setup: "CREATE TABLE lines(name TEXT, price INT, qty INT);"
        query: "SELECT price * qty AS total FROM lines WHERE total > 100"
        expect_error: "42703"
  - id: "group-by-input-name-wins"
    type: "single"
    prompt: "Table `t(a, b)` contains `(1, 10)`, `(1, 20)`, `(2, 10)`. What does PostgreSQL do with `SELECT b AS a, COUNT(*) FROM t GROUP BY a`?"
    correct:
      - "It fails, because `GROUP BY a` means the input column `t.a`, so `b` is neither grouped nor aggregated"
    distractors:
      - "It returns `(10, 2)` and `(20, 1)`, because a name in `GROUP BY` always refers to the select-list column `a` first"
      - "It returns `(10, 2)` and `(20, 1)`, because aliases are visible in `GROUP BY` only"
      - "It returns `(1, 2)` and `(2, 1)`, printing the grouped input column under the name `a`"
    explanation: |-
      PostgreSQL extends the standard by allowing output column names in `GROUP BY`, but when a name matches both an input column and an output column it resolves to the **input** column, the opposite of the `ORDER BY` rule. So the query groups by `t.a`:

      | group t.a | rows | b values |
      |---|---|---|
      | 1 | 2 | 10, 20 |
      | 2 | 1 | 10 |

      In group 1, `b` has two values, so the select-list item `b` is not determined by the group. PostgreSQL raises `column "t.b" must appear in the GROUP BY clause or be used in an aggregate function`.

      "Grouping by the output column `a`" applies the `ORDER BY` precedence to `GROUP BY`.

      "Aliases are visible in `GROUP BY` only" mixes up two rules: standard aliases are visible in `ORDER BY`; `GROUP BY` visibility is an extension.

      "`(1, 2)` and `(2, 1)`" assumes the select list silently prints the grouping column instead of `b`.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE t(a INT, b INT); INSERT INTO t VALUES (1,10),(1,20),(2,10);"
        query: "SELECT b AS a, COUNT(*) FROM t GROUP BY a"
        expect_error: "42803"
  - id: "alias-inside-order-by-expression"
    type: "single"
    prompt: "In PostgreSQL, what happens with this query?\n\n```sql\nSELECT price * qty AS total\nFROM lines\nORDER BY COALESCE(total, 0) DESC;\n```"
    correct:
      - "It fails, because an output column name in `ORDER BY` must stand alone, not inside an expression"
    distractors:
      - "It sorts by `total` with NULL totals treated as 0"
      - "It sorts by `total`, ignoring the `COALESCE` because aliases cannot be wrapped"
      - "It fails, because `COALESCE` is not allowed in `ORDER BY`"
      - "It sorts by `total` but puts NULL totals first, because `COALESCE` flips the NULL ordering"
    explanation: |-
      In `ORDER BY`, a *bare* name may refer to an output column. Inside an expression, PostgreSQL resolves names as input columns only (its documentation states that an output column name has to stand alone), and `lines` has no column `total`. The query fails with `column "total" does not exist`.

      "Sorts with NULL treated as 0" is the intent; write `ORDER BY COALESCE(price * qty, 0) DESC` or sort a derived table.

      "Ignoring the `COALESCE`" invents silent behavior.

      "`COALESCE` is not allowed in `ORDER BY`" is false; any expression over input columns is allowed.

      SQLite again accepts the query, because it resolves aliases inside expressions as an extension.

      "Puts NULL totals first" assumes the query runs at all; it fails before sorting starts.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE lines(name TEXT, price INT, qty INT);"
        query: "SELECT price * qty AS total FROM lines ORDER BY COALESCE(total, 0) DESC"
        expect_error: "42703"
      - engine: "sqlite"
        setup: "CREATE TABLE lines(name TEXT, price INT, qty INT); INSERT INTO lines VALUES ('x',10,5),('y',NULL,4);"
        query: "SELECT price * qty AS total FROM lines ORDER BY COALESCE(total, 0) DESC"
        expect: [[50], [null]]
        ordered: true
---
## Learning objectives

- Explain, from the logical order, why a select-list alias is visible in `ORDER BY` but not in `WHERE` or `HAVING`.
- Resolve a name that matches both an input column and an output column.
- Rewrite an alias filter portably with a derived table, a CTE or a repeated expression.
- Recognize dialect extensions to the scope rule and avoid depending on them.

## The invariant

A name in SQL is resolved against the virtual table that the current clause receives. Column aliases (derived column names) are created by the `SELECT` list at step 5 of the logical order, so:

| Clause | Step | Can see select-list aliases? (ISO SQL) |
|---|---|---|
| `FROM` / `ON` | 1 | No |
| `WHERE` | 2 | No |
| `GROUP BY` | 3 | No |
| `HAVING` | 4 | No |
| `SELECT` list (other items) | 5 | No; items are evaluated "all at once" |
| `ORDER BY` | 7 | Yes |

`ORDER BY` can name an output column by its alias or (deprecated but common) its ordinal position. Since SQL:1999 it may also sort by expressions over input columns that are not in the select list, unless `DISTINCT` or a set operator makes those values ambiguous.

When an `ORDER BY` name matches both an output column and an input column, the output column wins. That rule is about bare names: in PostgreSQL, a name inside an expression in `ORDER BY` is always an input-column name.

Three layers again. The standard fixes the scope table above. The theory is simply the pipeline: a clause cannot refer to something not yet computed. Engines extend the rules: MySQL 8.4 documents aliases in `GROUP BY` and `HAVING`; PostgreSQL accepts output names in `GROUP BY` but prefers input columns on ambiguity; SQLite even resolves aliases in `WHERE`.

## Worked example

`lines(name, price, qty)`: `('x', 10, 5)`, `('y', 30, 4)`, `('z', 60, 2)`.

```sql
-- Fails in standard SQL and PostgreSQL: total is not in scope in WHERE
SELECT name, price * qty AS total FROM lines WHERE total > 100;

-- Portable 1: derived table
SELECT name, total
FROM (SELECT name, price * qty AS total FROM lines) AS x
WHERE total > 100
ORDER BY total DESC;

-- Portable 2: CTE
WITH x AS (SELECT name, price * qty AS total FROM lines)
SELECT name, total FROM x WHERE total > 100 ORDER BY total DESC;

-- Portable 3: repeat the expression; ORDER BY may use the alias
SELECT name, price * qty AS total
FROM lines
WHERE price * qty > 100
ORDER BY total DESC;
```

Totals are 50, 120 and 120, so each portable form returns `y` and `z` (both 120; their relative order is not defined because the sort key ties).

The ambiguity rule in action, with `t(a, b)` = `(1, 10)`, `(1, 20)`, `(2, 10)`:

| Query | Name `a` resolves to | Result in PostgreSQL |
|---|---|---|
| `SELECT b AS a FROM t ORDER BY a` | output column (`b`) | 10, 10, 20 |
| `SELECT b AS a, COUNT(*) FROM t GROUP BY a` | input column `t.a` | error: `t.b` not grouped |

Never give an alias the name of a different source column; it makes the two rules collide.

## Where this shows up today

As of November 2026, the MySQL 8.4 manual permits aliases in `GROUP BY`, `ORDER BY` and `HAVING` but not `WHERE`; PostgreSQL 19 permits output names in `ORDER BY` and `GROUP BY` but not `WHERE` or `HAVING`; SQLite (3.53.1 in this book's checks) resolves them almost everywhere. Code generated by ORMs is usually portable because it repeats expressions instead of reusing aliases.

## Pitfalls checklist

- Filtering on an alias in `WHERE` and "fixing" it with `HAVING`, which only works in some dialects.
- Naming an alias after an existing input column, so `GROUP BY` and `ORDER BY` resolve the same name differently.
- Wrapping an alias in an expression inside `ORDER BY` in PostgreSQL.
- Relying on SQLite's lenient resolution in tests and shipping SQL that fails on the production engine.
- Sorting by ordinal position (`ORDER BY 2`) and breaking it when the select list changes.

## Key takeaways

- Aliases are born in the `SELECT` list, so only `ORDER BY` sees them in standard SQL.
- On ambiguity, `ORDER BY` prefers output columns; PostgreSQL's `GROUP BY` prefers input columns.
- Derived tables, CTEs or repeated expressions are the portable ways to filter on a computed value.
- Dialect leniency is not a portability guarantee.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), `<sort specification list>` and scope of derived column names.
- PostgreSQL 19 documentation, "SELECT" (GROUP BY and ORDER BY name resolution) and "Sorting Rows".
- MySQL 8.4 Reference Manual, "Problems with Column Aliases".
- SQLite documentation, "SELECT".
