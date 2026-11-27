---
title: "GROUP BY, HAVING and Functional Dependence"
questions:
  - id: "ungrouped-column-error"
    type: "single"
    prompt: "`emp(id, dept_id, name, salary)` has no declared keys. What does PostgreSQL do with `SELECT dept_id, name, COUNT(*) FROM emp GROUP BY dept_id`?"
    correct:
      - "It rejects the query, because `name` is neither grouped, aggregated, nor functionally dependent on `dept_id`"
    distractors:
      - "It returns one row per department, with the first employee's name"
      - "It returns one row per employee, because `name` is unique"
      - "It returns one row per department, with all names concatenated"
    explanation: |-
      After `GROUP BY dept_id` (step 3), each output row stands for a whole group. A select-list item must have a single value per group: it must be a grouping column, an aggregate, or (since SQL:1999, feature T301) a column functionally dependent on the grouping columns. A department with two employees has two names, so `name` has no single value. PostgreSQL raises `column "emp.name" must appear in the GROUP BY clause or be used in an aggregate function` (SQLSTATE 42803).

      "The first employee's name" describes non-standard engines that pick an arbitrary row (SQLite's "bare columns", or MySQL with `ONLY_FULL_GROUP_BY` turned off). "First" is not even defined, because groups have no order.

      "One row per employee" ignores the `GROUP BY`; and nothing declares `name` unique.

      "Concatenated" requires an explicit aggregate such as `string_agg` or `LISTAGG`.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE emp(id INT, dept_id INT, name TEXT, salary INT);"
        query: "SELECT dept_id, name, COUNT(*) FROM emp GROUP BY dept_id"
        expect_error: "42803"
  - id: "functional-dependence-on-pk"
    type: "single"
    prompt: "`dept(id PRIMARY KEY, name)` and `emp(id, dept_id, name, salary)`. Which of these queries does PostgreSQL accept?"
    correct:
      - "`SELECT d.id, d.name, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY d.id`"
    distractors:
      - "`SELECT e.dept_id, d.name, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY e.dept_id`"
      - "`SELECT d.name, d.id, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY d.name`"
      - "`SELECT d.id, e.name, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY d.id`"
    explanation: |-
      The SQL:1999 rule allows a non-aggregated column when it is functionally dependent on the grouping columns. PostgreSQL implements the most common case: if the grouping columns include a table's primary key, every column of *that table* may be selected.

      | Query | Grouping columns | Selected non-aggregate | Accepted? |
      |---|---|---|---|
      | GROUP BY d.id | d.id (d's primary key) | d.name | yes: `d.id → d.name` |
      | GROUP BY e.dept_id | e.dept_id | d.name | no: the dependency holds only via the join equality, which PostgreSQL does not use |
      | GROUP BY d.name | d.name | d.id | no: `name` is not declared unique |
      | GROUP BY d.id | d.id | e.name | no: a department has many employees |

      The dependency must be *declared* (a primary key), not merely true in the data; that is the same "data cannot prove an FD" principle from Chapter 1. The standard also describes dependencies derived from equality predicates; PostgreSQL does not implement that part, which is why grouping by `e.dept_id` fails even though it equals `d.id`.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE dept(id INT PRIMARY KEY, name TEXT); CREATE TABLE emp(id INT, dept_id INT, name TEXT, salary INT); INSERT INTO dept VALUES (1,'ops'),(2,'dev'); INSERT INTO emp VALUES (1,1,'a',60),(2,1,'b',40),(3,2,'c',80),(4,2,'d',70);"
        query: "SELECT d.id, d.name, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY d.id"
        expect: [[1, "ops", 2], [2, "dev", 2]]
      - engine: "postgres"
        setup: "CREATE TABLE dept(id INT PRIMARY KEY, name TEXT); CREATE TABLE emp(id INT, dept_id INT, name TEXT, salary INT);"
        query: "SELECT e.dept_id, d.name, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY e.dept_id"
        expect_error: "42803"
      - engine: "postgres"
        setup: "CREATE TABLE dept(id INT PRIMARY KEY, name TEXT); CREATE TABLE emp(id INT, dept_id INT, name TEXT, salary INT);"
        query: "SELECT d.name, d.id, COUNT(*) FROM dept d JOIN emp e ON e.dept_id = d.id GROUP BY d.name"
        expect_error: "42803"
  - id: "where-then-having-trace"
    type: "single"
    prompt: "`emp(dept_id, salary)` holds `(1, 60)`, `(1, 40)`, `(2, 80)`, `(2, 70)`, `(3, 30)`. What does this return?\n\n```sql\nSELECT dept_id, SUM(salary)\nFROM emp\nWHERE salary > 50\nGROUP BY dept_id\nHAVING SUM(salary) > 50;\n```"
    correct:
      - "`(1, 60)`, `(2, 150)`"
    distractors:
      - "`(1, 100)`, `(2, 150)`"
      - "`(1, 60)`, `(2, 80)`, `(2, 70)`"
      - "`(1, 60)`, `(2, 150)`, `(3, 0)`"
    explanation: |-
      `WHERE` filters rows before grouping; `HAVING` filters groups after aggregation.

      | Step | Result |
      |---|---|
      | WHERE salary > 50 | (1, 60), (2, 80), (2, 70) |
      | GROUP BY dept_id | 1: {60}; 2: {80, 70} |
      | SUM | 1 → 60; 2 → 150 |
      | HAVING SUM > 50 | both groups pass |

      Department 3 disappeared at the `WHERE` step, and department 1 lost its 40 there too.

      "`(1, 100)`" sums all of department 1's salaries, as if `WHERE` ran after grouping.

      "`(1, 60)`, `(2, 80)`, `(2, 70)`" lists the filtered rows; `GROUP BY` collapses each department into one output row.

      "`(3, 0)`" expects an empty group to survive; a group exists only if rows reach `GROUP BY`, and `SUM` of nothing would be NULL, not 0.
    checks:
      - engine: "both"
        setup: "CREATE TABLE emp(dept_id INT, salary INT); INSERT INTO emp VALUES (1,60),(1,40),(2,80),(2,70),(3,30);"
        query: "SELECT dept_id, SUM(salary) FROM emp WHERE salary > 50 GROUP BY dept_id HAVING SUM(salary) > 50"
        expect: [[1, 60], [2, 150]]
  - id: "aggregate-in-where"
    type: "single"
    prompt: "A developer wants departments with more than one employee and writes `SELECT dept_id FROM emp WHERE COUNT(*) > 1 GROUP BY dept_id`. What happens?"
    correct:
      - "It fails, because aggregates do not exist yet when `WHERE` is evaluated; the condition belongs in `HAVING`"
    distractors:
      - "It works, because the optimizer moves the condition to `HAVING` automatically"
      - "It returns every department, because `COUNT(*)` in `WHERE` counts the whole table"
      - "It fails, because `COUNT(*)` must be written `COUNT(dept_id)` in a filter"
    explanation: |-
      `WHERE` is step 2: it evaluates a condition for one row at a time, before any groups exist. An aggregate summarizes a group, so it has no meaning there. The standard forbids aggregates in `WHERE`, and PostgreSQL reports `aggregate functions are not allowed in WHERE`.

      Correct form: `SELECT dept_id FROM emp GROUP BY dept_id HAVING COUNT(*) > 1`.

      "The optimizer moves it" confuses rewriting (which must preserve meaning) with repairing invalid SQL; an invalid query has no meaning to preserve.

      "Counts the whole table" is a plausible-sounding semantics, but SQL does not define one.

      "`COUNT(dept_id)`" changes nothing; any aggregate is illegal in `WHERE`.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE emp(dept_id INT, salary INT);"
        query: "SELECT dept_id FROM emp WHERE COUNT(*) > 1 GROUP BY dept_id"
        expect_error: "42803"
  - id: "having-without-group-by"
    type: "single"
    prompt: "`emp` has 5 rows. What do these return?\n\n```sql\nSELECT COUNT(*) FROM emp HAVING COUNT(*) > 10;  -- Q1\nSELECT COUNT(*) FROM emp HAVING COUNT(*) > 3;   -- Q2\n```"
    correct:
      - "Q1: no rows; Q2: one row containing 5"
    distractors:
      - "Q1: one row containing 0; Q2: one row containing 5"
      - "Q1: an error, because `HAVING` needs `GROUP BY`; Q2: the same error"
      - "Q1: one row containing NULL; Q2: one row containing 5"
    explanation: |-
      Without `GROUP BY`, the whole table forms a single group, and `HAVING` filters that one group like any other.

      | Query | single group's COUNT(*) | HAVING | output |
      |---|---|---|---|
      | Q1 | 5 | 5 > 10 is FALSE | group removed: no rows |
      | Q2 | 5 | 5 > 3 is TRUE | one row: 5 |

      This is the one way an ungrouped aggregate query can return zero rows.

      "One row containing 0" applies the empty-input rule (`COUNT` of nothing is 0) to a group that was *filtered out*; filtering removes the row entirely.

      "An error" is wrong in standard SQL; `HAVING` without `GROUP BY` is legal (SQLite accepted it only from 3.39.0).

      "NULL" invents a placeholder row.
    checks:
      - engine: "both"
        setup: "CREATE TABLE emp(dept_id INT, salary INT); INSERT INTO emp VALUES (1,60),(1,40),(2,80),(2,70),(3,30);"
        query: "SELECT (SELECT COUNT(*) FROM (SELECT COUNT(*) AS n FROM emp HAVING COUNT(*) > 10) q1), (SELECT n FROM (SELECT COUNT(*) AS n FROM emp HAVING COUNT(*) > 3) q2)"
        expect: [[0, 5]]
  - id: "mysql-only-full-group-by"
    type: "single"
    prompt: "A legacy MySQL 5.6 application ran `SELECT dept_id, name, COUNT(*) FROM emp GROUP BY dept_id` without errors. After an upgrade to MySQL 8.4 with the default SQL mode, the query fails. Why?"
    correct:
      - "The default SQL mode includes `ONLY_FULL_GROUP_BY`, which rejects non-aggregated columns not functionally dependent on the grouping columns"
    distractors:
      - "MySQL 8.4 removed support for `GROUP BY` on non-key columns"
      - "MySQL 8.4 requires every selected column to appear in `GROUP BY`, even dependent ones"
      - "MySQL 8.4 forbids `COUNT(*)` together with plain columns"
    explanation: |-
      The query was always outside standard SQL: `name` has several values per department. Old MySQL defaults silently returned a value from an arbitrary row. The MySQL 8.4 manual lists `ONLY_FULL_GROUP_BY` first in the default SQL mode; with it, MySQL rejects select-list, `HAVING` or `ORDER BY` references to non-aggregated columns that are neither grouped nor functionally dependent on grouped columns. That is the SQL:1999 rule, which MySQL implements including primary-key dependencies.

      "Removed support for `GROUP BY` on non-key columns" is false; grouping by any column is fine.

      "Every selected column must appear in `GROUP BY`" is the stricter SQL-92 rule; MySQL accepts dependent columns.

      "Forbids `COUNT(*)` with plain columns" is false; grouped columns may appear with aggregates.

      Fixes: aggregate the column (`MIN(name)`, `string_agg`), group by it, or, when any value will do, say so with `ANY_VALUE(name)` (standardized in SQL:2023 and available in MySQL and in PostgreSQL 16 and later).
---
## Learning objectives

- State which expressions may appear in the select list of a grouped query.
- Use functional dependence on a grouped primary key to avoid needless `GROUP BY` columns.
- Choose correctly between `WHERE` and `HAVING`.
- Predict the result of `HAVING` without `GROUP BY`.

## The invariant

`GROUP BY` partitions the rows that survive `WHERE` into groups of rows that are not distinct on the grouping columns. From that point on, the query works with groups, so every expression in `SELECT`, `HAVING` and `ORDER BY` must have **one value per group**. ISO SQL allows three kinds of expression:

1. Grouping columns (and expressions over them).
2. Aggregates (set functions) over the group's rows.
3. Since SQL:1999 (feature T301), columns **functionally dependent** on the grouping columns, for example any column of a table whose primary key is among the grouping columns.

SQL-92 allowed only the first two; the third is exactly the FD concept from Chapter 1 applied to queries.

Filtering happens in two places:

| Clause | Step | Filters | May use aggregates? |
|---|---|---|---|
| `WHERE` | 2 | individual rows, before grouping | no |
| `HAVING` | 4 | whole groups, after aggregation | yes |

Prefer `WHERE` for conditions on row columns: it reduces the work of grouping, and it means something different (it changes which rows feed each aggregate). A condition on a grouping column gives the same result in either clause; a condition on a non-grouped column is only valid in `WHERE`.

Without `GROUP BY`, an aggregate query has exactly one group (the whole filtered input), and `HAVING` may remove it, giving zero rows.

## Worked example

`dept(id PRIMARY KEY, name)`: (1, ops), (2, dev). `emp(dept_id, salary)`: (1, 60), (1, 40), (2, 80), (2, 70).

```sql
-- Valid since SQL:1999: d.name depends on the grouped primary key d.id
SELECT d.id, d.name, COUNT(*) AS staff, SUM(e.salary) AS payroll
FROM dept d
JOIN emp e ON e.dept_id = d.id
WHERE e.salary > 50             -- rows: (1,60), (2,80), (2,70)
GROUP BY d.id
HAVING SUM(e.salary) > 100;     -- groups: dev only
```

| id | name | staff | payroll |
|---|---|---|---|
| 2 | dev | 2 | 150 |

Trace: `WHERE` drops (1, 40); ops keeps only 60 and fails `HAVING`; dev's 150 passes. Under the SQL-92 rule you would have to write `GROUP BY d.id, d.name`, which gives the same result but obscures the key.

## Where this shows up today

As of November 2026, the MySQL 8.4 manual lists `ONLY_FULL_GROUP_BY` in the default SQL mode and detects functional dependence on grouped keys; PostgreSQL 19 accepts columns of a table whose primary key is grouped; SQLite (3.53.1 in this book's checks) still allows "bare" columns with arbitrary values. `ANY_VALUE()` (SQL:2023) is the explicit way to say "any value from the group is fine".

## Pitfalls checklist

- Selecting a non-grouped, non-dependent column and accepting whatever value a lenient engine returns.
- Putting row conditions in `HAVING`, making grouping do extra work, or aggregate conditions in `WHERE`.
- Adding every selected column to `GROUP BY` "to make it compile", which can split groups if a column is not actually dependent.
- Expecting a `HAVING` filter on an ungrouped aggregate to return a zero row.
- Relying on equality-derived dependencies (`GROUP BY e.dept_id`, select `d.name`) that the engine does not recognize.

## Key takeaways

- In a grouped query, every output expression needs one value per group.
- Grouping by a primary key makes the rest of that table's columns selectable.
- `WHERE` filters rows before grouping; `HAVING` filters groups after aggregation.
- Without `GROUP BY` there is one group, which `HAVING` can remove.

## Sources

- ISO/IEC 9075-2 (SQL/Foundation), `<group by clause>`, `<having clause>`; feature T301 "Functional dependencies" (SQL:1999); `ANY_VALUE` (SQL:2023).
- PostgreSQL 19 documentation, "SELECT" (GROUP BY clause); MySQL 8.4 Reference Manual, "Server SQL Modes" and "MySQL Handling of GROUP BY".
