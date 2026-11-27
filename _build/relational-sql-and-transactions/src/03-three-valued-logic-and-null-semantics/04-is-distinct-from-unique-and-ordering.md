---
title: "IS DISTINCT FROM, UNIQUE, GROUP BY and ORDER BY With NULL"
questions:
  - id: "is-distinct-from-count"
    type: "single"
    prompt: "`pairs(a, b)` holds `(1, 1)`, `(1, NULL)`, `(NULL, NULL)`, `(2, 3)`. How many rows do `WHERE a <> b` and `WHERE a IS DISTINCT FROM b` return?"
    correct:
      - "`a <> b`: 1 row; `a IS DISTINCT FROM b`: 2 rows"
    distractors:
      - "`a <> b`: 2 rows; `a IS DISTINCT FROM b`: 2 rows"
      - "`a <> b`: 1 row; `a IS DISTINCT FROM b`: 3 rows"
      - "`a <> b`: 1 row; `a IS DISTINCT FROM b`: 1 row"
    explanation: |-
      `IS DISTINCT FROM` is the standard's NULL-safe comparison: it treats two NULLs as not distinct, a NULL and a non-NULL as distinct, and otherwise behaves like `<>`. It never returns UNKNOWN.

      | a | b | `a <> b` | `a IS DISTINCT FROM b` |
      |---|---|---|---|
      | 1 | 1 | FALSE | FALSE |
      | 1 | NULL | UNKNOWN | TRUE |
      | NULL | NULL | UNKNOWN | FALSE |
      | 2 | 3 | TRUE | TRUE |

      `<>` keeps one row; `IS DISTINCT FROM` keeps two.

      "2 and 2" treats `1 <> NULL` as TRUE.

      "1 and 3" treats two NULLs as distinct from each other; `IS DISTINCT FROM` exists precisely to say they are not.

      "1 and 1" assumes `IS DISTINCT FROM` is only a synonym for `<>`.
    checks:
      - engine: "both"
        setup: "CREATE TABLE pairs(a INT, b INT); INSERT INTO pairs VALUES (1,1),(1,NULL),(NULL,NULL),(2,3);"
        query: "SELECT (SELECT COUNT(*) FROM pairs WHERE a <> b), (SELECT COUNT(*) FROM pairs WHERE a IS DISTINCT FROM b)"
        expect: [[1, 2]]
  - id: "group-by-null-group"
    type: "single"
    prompt: "`visits(region)` holds `'eu'`, `NULL`, `'us'`, `NULL`, `'eu'`. What does `SELECT region, COUNT(*) FROM visits GROUP BY region` return?"
    correct:
      - "Three rows: `('eu', 2)`, `('us', 1)`, `(NULL, 2)`"
    distractors:
      - "Four rows: `('eu', 2)`, `('us', 1)`, `(NULL, 1)`, `(NULL, 1)`"
      - "Two rows: `('eu', 2)`, `('us', 1)`"
      - "Three rows: `('eu', 2)`, `('us', 1)`, `(NULL, 0)`"
    explanation: |-
      Grouping does not use `=`, which would make every NULL a group of its own. ISO SQL forms groups by *not distinct*: two NULLs are not distinct, so all NULL rows fall into one group.

      | group | rows | COUNT(*) |
      |---|---|---|
      | eu | 2 | 2 |
      | us | 1 | 1 |
      | NULL | 2 | 2 |

      "Four rows" applies `=` semantics (NULL is not equal to NULL) to grouping.

      "Two rows" assumes NULL rows are dropped; only `WHERE` drops them, and there is no `WHERE` here.

      "`(NULL, 0)`" confuses `COUNT(*)` with `COUNT(region)`; the latter would indeed be 0 for the NULL group, but `COUNT(*)` counts its two rows.
    checks:
      - engine: "both"
        setup: "CREATE TABLE visits(region TEXT); INSERT INTO visits VALUES ('eu'),(NULL),('us'),(NULL),('eu');"
        query: "SELECT region, COUNT(*) FROM visits GROUP BY region"
        expect: [["eu", 2], ["us", 1], [null, 2]]
  - id: "distinct-vs-count-distinct"
    type: "single"
    prompt: "Same `visits` data. What do these return?\n\n```sql\nSELECT COUNT(*) FROM (SELECT DISTINCT region FROM visits) AS d;  -- Q1\nSELECT COUNT(DISTINCT region) FROM visits;                       -- Q2\n```"
    correct:
      - "Q1: 3; Q2: 2"
    distractors:
      - "Q1: 2; Q2: 2"
      - "Q1: 3; Q2: 3"
      - "Q1: 4; Q2: 2"
    explanation: |-
      `SELECT DISTINCT` removes duplicate *rows* using "not distinct", so the two NULLs collapse into one row: `eu`, `us`, NULL. Counting those rows gives 3.

      `COUNT(DISTINCT region)` is a set function: it first eliminates NULL values (like every aggregate except `COUNT(*)`), then counts distinct remaining values: `eu`, `us`, which is 2.

      | Query | values considered | result |
      |---|---|---|
      | Q1 | eu, us, NULL (one NULL row) | 3 |
      | Q2 | eu, us (NULLs eliminated) | 2 |

      "2 and 2" assumes `DISTINCT` drops NULLs.

      "3 and 3" assumes `COUNT(DISTINCT ...)` counts NULL as a value.

      "4" for Q1 keeps both NULLs as distinct rows.
    checks:
      - engine: "both"
        setup: "CREATE TABLE visits(region TEXT); INSERT INTO visits VALUES ('eu'),(NULL),('us'),(NULL),('eu');"
        query: "SELECT (SELECT COUNT(*) FROM (SELECT DISTINCT region FROM visits) AS d), (SELECT COUNT(DISTINCT region) FROM visits)"
        expect: [[3, 2]]
  - id: "unique-allows-many-nulls"
    type: "single"
    prompt: "`accounts(email TEXT UNIQUE)` uses the standard default null treatment. Four inserts run one after another as separate statements: `NULL`, `NULL`, `'a@x'`, `'a@x'`. How many rows does the table hold afterwards?"
    correct:
      - "3"
    distractors:
      - "2"
      - "4"
      - "1"
    explanation: |-
      A `UNIQUE` constraint forbids two rows whose constrained values are equal. With the default `NULLS DISTINCT` treatment, a NULL is never equal to anything, so the constraint is satisfied for any number of NULLs.

      | Insert | Conflicts with | Result |
      |---|---|---|
      | NULL | nothing | accepted |
      | NULL | nothing (NULLs are distinct here) | accepted |
      | 'a@x' | nothing | accepted |
      | 'a@x' | the existing 'a@x' | rejected |

      Total: 3 rows.

      "2" treats NULLs as equal, which is `NULLS NOT DISTINCT` behavior (and SQL Server's default).

      "4" ignores the constraint.

      "1" assumes the constraint forbids NULLs; that is `NOT NULL`'s job, and `UNIQUE` columns are nullable unless declared otherwise.

      Note the deliberate inconsistency: `GROUP BY` and `DISTINCT` treat NULLs as not distinct, while `UNIQUE` by default treats them as distinct.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE accounts(email TEXT UNIQUE); INSERT INTO accounts VALUES (NULL); INSERT INTO accounts VALUES (NULL); INSERT INTO accounts VALUES ('a@x');"
        query: "INSERT INTO accounts VALUES ('a@x')"
        expect_error: "23505"
      - engine: "both"
        setup: "CREATE TABLE accounts(email TEXT UNIQUE); INSERT INTO accounts VALUES (NULL); INSERT INTO accounts VALUES (NULL); INSERT INTO accounts VALUES ('a@x');"
        query: "SELECT COUNT(*) FROM accounts"
        expect: [[3]]
  - id: "nulls-not-distinct-constraint"
    type: "single"
    prompt: "In PostgreSQL 19, `settings(user_id, key, value)` stores per-user settings and *global* defaults, where a global row has `user_id IS NULL`. Different users may share a key, but there must be at most one global row per key. Which constraint enforces exactly that?"
    correct:
      - "`UNIQUE NULLS NOT DISTINCT (user_id, key)`"
    distractors:
      - "`UNIQUE (user_id, key)`"
      - "`UNIQUE (key)`"
      - "`CHECK (user_id IS NOT NULL)`"
    explanation: |-
      With `NULLS NOT DISTINCT` (SQL:2023 feature F292, supported by PostgreSQL since version 15), NULLs in the constrained columns compare as equal for uniqueness, so a second `(NULL, 'theme')` conflicts with the first. Rows with real user ids behave as before: `(1, 'theme')` and `(2, 'theme')` differ on `user_id`.

      | Insert | `UNIQUE (user_id, key)` | `UNIQUE NULLS NOT DISTINCT (user_id, key)` |
      |---|---|---|
      | (NULL, 'theme') | accepted | accepted |
      | (NULL, 'theme') | accepted (NULLs distinct) | rejected |
      | (1, 'theme') | accepted | accepted |
      | (2, 'theme') | accepted | accepted |

      "`UNIQUE (user_id, key)`" uses the default null treatment and lets any number of global rows for the same key in.

      "`UNIQUE (key)`" stops two users from having the same setting.

      "`CHECK (user_id IS NOT NULL)`" forbids global rows altogether.

      The timeless point: the default `UNIQUE` treats NULLs as distinct, so enforcing "one NULL" needs an explicit mechanism (this clause, or a partial unique index on the NULL rows in engines without it).
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE settings(user_id INT, key TEXT, value TEXT, UNIQUE NULLS NOT DISTINCT (user_id, key)); INSERT INTO settings VALUES (NULL,'theme','dark'),(1,'theme','light'),(2,'theme','dark');"
        query: "INSERT INTO settings VALUES (NULL,'theme','light')"
        expect_error: "23505"
      - engine: "postgres"
        setup: "CREATE TABLE settings(user_id INT, key TEXT, value TEXT, UNIQUE (user_id, key)); INSERT INTO settings VALUES (NULL,'theme','dark'),(NULL,'theme','light'),(1,'theme','light'),(2,'theme','dark');"
        query: "SELECT COUNT(*) FROM settings WHERE user_id IS NULL"
        expect: [[2]]
  - id: "null-sort-placement"
    type: "multiple"
    prompt: "Column `x` holds `3`, `NULL`, `1`, `NULL`. Which outputs of `SELECT x FROM t ORDER BY x` (ascending, no `NULLS FIRST/LAST`) are permitted by the SQL standard? Select all that apply."
    correct:
      - "`1`, `3`, `NULL`, `NULL`"
      - "`NULL`, `NULL`, `1`, `3`"
    distractors:
      - "`NULL`, `1`, `NULL`, `3`"
      - "`1`, `NULL`, `NULL`, `3`"
      - "`3`, `1`, `NULL`, `NULL`"
    explanation: |-
      The standard requires NULLs to sort *together*, either before all non-NULL values or after all of them; which of the two is implementation-defined. Non-NULL values follow the requested direction.

      - "`1`, `3`, `NULL`, `NULL`": ascending values, NULLs last. Permitted (PostgreSQL's default).
      - "`NULL`, `NULL`, `1`, `3`": NULLs first, then ascending values. Permitted (SQLite's default).

      Distractors:

      - "`NULL`, `1`, `NULL`, `3`" and "`1`, `NULL`, `NULL`, `3`" scatter or embed the NULLs among non-NULL values, as if NULL compared like some ordinary number. The standard forbids this.
      - "`3`, `1`, `NULL`, `NULL`" sorts the non-NULL values descending, which was not requested.

      Write `ORDER BY x NULLS LAST` (or `NULLS FIRST`) to make the placement explicit wherever the engine supports it.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (3),(NULL),(1),(NULL);"
        query: "SELECT x FROM t ORDER BY x"
        expect: [[1], [3], [null], [null]]
        ordered: true
      - engine: "sqlite"
        setup: "CREATE TABLE t(x INT); INSERT INTO t VALUES (3),(NULL),(1),(NULL);"
        query: "SELECT x FROM t ORDER BY x"
        expect: [[null], [null], [1], [3]]
        ordered: true
body_checks:
  - engine: "both"
    setup: "CREATE TABLE visits(region TEXT); INSERT INTO visits VALUES ('eu'),(NULL),('us'),(NULL),('eu');"
    query: "SELECT (SELECT COUNT(*) FROM visits v1 JOIN visits v2 ON v1.region = v2.region), (SELECT COUNT(*) FROM visits v1 JOIN visits v2 ON v1.region IS NOT DISTINCT FROM v2.region)"
    expect: [[5, 9]]
---
## Learning objectives

- Use `IS [NOT] DISTINCT FROM` for NULL-safe comparisons.
- Predict how `GROUP BY` and `DISTINCT` treat NULLs and how that differs from `COUNT(DISTINCT ...)`.
- Explain why a standard `UNIQUE` constraint accepts many NULLs, and how to change that.
- Describe the standard's rule for where NULLs sort.

## The invariant

SQL uses two different notions of sameness for NULL, and each operation picks one:

| Notion | NULL vs NULL | Used by |
|---|---|---|
| Equality (`=`) | UNKNOWN | `WHERE`, `ON`, `IN`, `CASE WHEN x = ...`, joins |
| Not distinct | treated as the same | `GROUP BY`, `DISTINCT`, `UNION`/`INTERSECT`/`EXCEPT`, `PARTITION BY`, `IS NOT DISTINCT FROM` |

`x IS DISTINCT FROM y` makes the second notion available in expressions: it is FALSE when both are NULL, TRUE when exactly one is NULL, and `x <> y` otherwise. It never returns UNKNOWN.

`UNIQUE` constraints historically used the first notion: the standard says a unique constraint is satisfied if no two rows have equal *non-NULL* values in all constrained columns, so any number of rows may contain a NULL. SQL:2023 made this explicit as an option (feature F292): `UNIQUE NULLS DISTINCT` (the usual default) or `UNIQUE NULLS NOT DISTINCT`.

Ordering: the standard specifies that NULLs sort together, either all before or all after every non-NULL value; which one is implementation-defined. `NULLS FIRST` and `NULLS LAST` in `ORDER BY` choose explicitly.

The three layers diverge a lot here. The theory (Codd's "value unknown") argues that two unknowns are not known to be equal; the standard nonetheless groups them together for practicality; engines differ on unique constraints and default sort placement.

## Worked example

```sql
CREATE TABLE visits (region TEXT);
INSERT INTO visits VALUES ('eu'), (NULL), ('us'), (NULL), ('eu');

SELECT region, COUNT(*) FROM visits GROUP BY region;
-- ('eu', 2), ('us', 1), (NULL, 2)          one NULL group

SELECT COUNT(*) FROM (SELECT DISTINCT region FROM visits) d;   -- 3
SELECT COUNT(DISTINCT region) FROM visits;                     -- 2

SELECT *
FROM visits v1 JOIN visits v2 ON v1.region = v2.region;
-- 5 rows: eu x eu (4) + us x us (1); NULLs never join
SELECT *
FROM visits v1 JOIN visits v2 ON v1.region IS NOT DISTINCT FROM v2.region;
-- 9 rows: the 5 above plus NULL x NULL (4)
```

The join pair shows the two notions side by side: `=` loses the NULL rows, `IS NOT DISTINCT FROM` matches them.

To allow at most one NULL in a unique column, declare `UNIQUE NULLS NOT DISTINCT` where it is supported, or, in engines without it, a partial or filtered unique index that covers only the NULL rows.

## Where this shows up today

As of November 2026, PostgreSQL 19 supports `NULLS NOT DISTINCT` on unique constraints and indexes (added in PostgreSQL 15), while SQL Server's documentation still states that a `UNIQUE` constraint allows only one NULL per column, so the usual workaround there is a filtered unique index `WHERE col IS NOT NULL`. PostgreSQL sorts NULLs last in ascending order by default; SQLite (3.53.1 in this book's checks) sorts them first and accepts explicit `NULLS LAST`.

## Pitfalls checklist

- Comparing nullable columns with `<>` in change-detection queries; use `IS DISTINCT FROM`.
- Assuming a `UNIQUE` column holds at most one NULL, or assuming the reverse when porting to SQL Server.
- Confusing `SELECT DISTINCT` (keeps one NULL) with `COUNT(DISTINCT ...)` (drops NULLs).
- Relying on an engine's default NULL sort position in paging or reports; state `NULLS FIRST/LAST`.
- Joining on nullable keys with `=` and expecting NULL keys to match.

## Key takeaways

- `=` makes NULL vs NULL UNKNOWN; grouping, `DISTINCT` and set operators treat NULLs as the same.
- `IS DISTINCT FROM` is the NULL-safe `<>`; it never returns UNKNOWN.
- Standard `UNIQUE` permits many NULLs; SQL:2023's `NULLS NOT DISTINCT` permits one.
- NULLs sort together, first or last by implementation; say which with `NULLS FIRST/LAST`.

## Sources

- ISO/IEC 9075-2:2023 (SQL/Foundation), `<distinct predicate>`, `<unique constraint definition>` (feature F292), `<sort specification>`.
- PostgreSQL 15 release notes; PostgreSQL 19 documentation, "CREATE TABLE" and "Sorting Rows".
- Microsoft Learn, "Unique constraints and check constraints".
