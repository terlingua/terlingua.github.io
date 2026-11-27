---
title: "Sargable Predicates"
questions:
  - id: "function-on-indexed-column"
    type: "single"
    prompt: "`users` has a B+tree index on `email` and no other indexes. How can an engine evaluate `WHERE lower(email) = 'ann@x.org'`?"
    correct:
      - "Only by scanning: the index is ordered by `email`, not by `lower(email)`, so there is no key range to seek to"
    distractors:
      - "By seeking the index for `'ann@x.org'` and then checking `lower` on the matches"
      - "By seeking the index, because `lower` preserves the order of strings"
      - "By building a temporary index on `lower(email)` for the duration of the query"
    explanation: |-
      An index can serve a predicate as a *seek* only if the predicate describes a contiguous range of the index's keys. `lower(email) = 'ann@x.org'` is satisfied by `'Ann@x.org'`, `'ANN@X.ORG'` and `'ann@x.org'`, which sort far apart in a case-sensitive order (uppercase letters sort before lowercase ones in byte order). No single range contains exactly them, so the engine must read every row (or every index entry) and apply the function.

      "Seek for `'ann@x.org'` and then check" would miss `'Ann@x.org'`, which is not stored under the lowercase key.

      "`lower` preserves order" is false in general: `'B' < 'a'` in byte order, but `lower('B') = 'b' > 'a'`.

      "A temporary index" is not something optimizers build for a single predicate; you create an expression index yourself (see the last question).

      Plan check in SQLite: with only an index on `email`, `WHERE lower(email) = ...` gives `SCAN users`, while `WHERE email = ...` gives `SEARCH users USING COVERING INDEX users_email (email=?)`.
    checks:
      - engine: "sqlite"
        setup: "CREATE TABLE users(id INTEGER PRIMARY KEY, email TEXT); CREATE INDEX users_email ON users(email);"
        query: "EXPLAIN QUERY PLAN SELECT * FROM users WHERE lower(email) = 'ann@x.org'"
        expect_contains: "SCAN users"
      - engine: "sqlite"
        setup: "CREATE TABLE users(id INTEGER PRIMARY KEY, email TEXT); CREATE INDEX users_email ON users(email);"
        query: "EXPLAIN QUERY PLAN SELECT * FROM users WHERE email = 'ann@x.org'"
        expect_contains: "INDEX users_email (email=?)"
  - id: "like-prefix-vs-suffix"
    type: "multiple"
    prompt: "`people(name)` has a B+tree index on `name` with a byte-order (`\"C\"`) collation. Which predicates can be answered with a range seek on that index? Select all that apply."
    correct:
      - "`name LIKE 'ann%'`"
      - "`name >= 'ann' AND name < 'ano'`"
    distractors:
      - "`name LIKE '%son'`"
      - "`name LIKE '%an%'`"
      - "`substr(name, 1, 3) = 'ann'`"
    explanation: |-
      A pattern with a fixed prefix and a trailing wildcard describes a key range: every string starting with `ann` lies between `'ann'` (inclusive) and `'ano'` (exclusive), the prefix with its last character incremented. PostgreSQL turns `LIKE 'ann%'` into exactly `Index Cond: name >= 'ann' AND name < 'ano'`, keeping the `LIKE` as a recheck filter. Writing the range by hand is the same seek.

      Distractors:

      - `LIKE '%son'` has a leading wildcard: strings ending in `son` begin with anything, so they are spread across the whole index.
      - `LIKE '%an%'` is worse still, matching anywhere.
      - `substr(name, 1, 3) = 'ann'` is logically a prefix test, but it wraps the column in a function; unless the optimizer has a rule for that exact function, it sees an opaque expression.

      The collation caveat matters: in linguistic collations, `'ann'..'ano'` may not bound all strings with that prefix, which is why some engines need a byte-order collation or a special operator class to optimize `LIKE` prefixes.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE people(name TEXT COLLATE \"C\"); INSERT INTO people SELECT md5(g::text) FROM generate_series(1, 20000) g; CREATE INDEX ON people(name); ANALYZE people;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM people WHERE name LIKE 'ann%'"
        expect_contains: "name < 'ano'"
      - engine: "postgres"
        setup: "CREATE TABLE people(name TEXT COLLATE \"C\"); INSERT INTO people SELECT md5(g::text) FROM generate_series(1, 20000) g; CREATE INDEX ON people(name); ANALYZE people;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM people WHERE name LIKE '%son'"
        expect_contains: "Seq Scan"
  - id: "implicit-conversion-mysql"
    type: "single"
    prompt: "In MySQL 8.4, `phone` is an indexed `VARCHAR` column. A developer writes `WHERE phone = 5551234` with an unquoted number. What does the MySQL 8.4 manual say happens?"
    correct:
      - "The comparison is done numerically, so the index on the string column cannot be used for the lookup"
    distractors:
      - "MySQL converts the literal to the string `'5551234'` and seeks the index normally"
      - "MySQL rejects the query, because a string column cannot be compared with a number"
      - "MySQL uses the index but returns only rows stored exactly as `'5551234'`"
    explanation: |-
      MySQL compares a string with a number by converting both to numbers. Many different strings convert to the same number (`'5551234'`, `' 5551234'`, `'5551234abc'`), and they are scattered throughout the string-ordered index. The manual states that for comparisons of a string column with a number, MySQL cannot use an index on the column to look up the value quickly.

      "Converts the literal to a string" is what developers expect; the conversion goes the other way, applied to the column, which makes the predicate non-sargable, exactly like wrapping the column in a function.

      "Rejects the query" describes engines with strict typing; PostgreSQL, for example, raises an error for `varchar = integer` without an explicit cast.

      "Uses the index but returns only exact matches" mixes the two behaviors.

      Portable fix: compare like with like, `WHERE phone = '5551234'`, or bind parameters with the column's type.
  - id: "date-function-to-range"
    type: "single"
    prompt: "`events.created_at` is an indexed `TIMESTAMP`. Which rewrite of `WHERE EXTRACT(YEAR FROM created_at) = 2026` is sargable *and* returns exactly the same rows?"
    correct:
      - "`WHERE created_at >= DATE '2026-01-01' AND created_at < DATE '2027-01-01'`"
    distractors:
      - "`WHERE created_at BETWEEN DATE '2026-01-01' AND DATE '2026-12-31'`"
      - "`WHERE CAST(created_at AS DATE) BETWEEN DATE '2026-01-01' AND DATE '2026-12-31'`"
      - "`WHERE created_at >= DATE '2026-01-01' AND created_at <= DATE '2027-01-01'`"
    explanation: |-
      The half-open range `[2026-01-01 00:00, 2027-01-01 00:00)` contains exactly the instants of 2026 and puts the bare column on one side of each comparison, so the index can seek it.

      Trace with three rows: `2026-06-01 10:00`, `2026-12-31 18:00`, `2027-01-01 00:00`.

      | Predicate | 2026-06-01 10:00 | 2026-12-31 18:00 | 2027-01-01 00:00 |
      |---|---|---|---|
      | EXTRACT(YEAR) = 2026 | yes | yes | no |
      | half-open range | yes | yes | no |
      | BETWEEN '2026-01-01' AND '2026-12-31' | yes | **no** (the upper bound is midnight) | no |
      | CAST AS DATE BETWEEN ... | yes | yes | no, but not sargable |
      | >= '2026-01-01' AND <= '2027-01-01' | yes | yes | **yes** |

      `BETWEEN` with a date upper bound silently drops most of December 31. Casting the column is correct but applies a function to it. The closed upper bound includes the first instant of 2027.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE events(created_at TIMESTAMP); INSERT INTO events VALUES ('2026-06-01 10:00'),('2026-12-31 18:00'),('2027-01-01 00:00');"
        query: "SELECT (SELECT COUNT(*) FROM events WHERE EXTRACT(YEAR FROM created_at) = 2026), (SELECT COUNT(*) FROM events WHERE created_at >= DATE '2026-01-01' AND created_at < DATE '2027-01-01'), (SELECT COUNT(*) FROM events WHERE created_at BETWEEN DATE '2026-01-01' AND DATE '2026-12-31'), (SELECT COUNT(*) FROM events WHERE CAST(created_at AS DATE) BETWEEN DATE '2026-01-01' AND DATE '2026-12-31'), (SELECT COUNT(*) FROM events WHERE created_at >= DATE '2026-01-01' AND created_at <= DATE '2027-01-01')"
        expect: [[2, 2, 1, 2, 3]]
      - engine: "postgres"
        setup: "CREATE TABLE events(created_at TIMESTAMP); INSERT INTO events SELECT TIMESTAMP '2025-01-01' + g * INTERVAL '1 hour' FROM generate_series(1, 30000) g; CREATE INDEX ON events(created_at); ANALYZE events;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM events WHERE EXTRACT(YEAR FROM created_at) = 2026"
        expect_contains: "Seq Scan"
  - id: "arithmetic-on-column"
    type: "single"
    prompt: "In SQLite, `t(id, v)` has an index on `id`. What plan does `SELECT * FROM t WHERE id + 0 = 5` get, compared with `WHERE id = 5`?"
    correct:
      - "`id + 0 = 5` scans the table; `id = 5` searches the index"
    distractors:
      - "Both search the index, because `id + 0` is simplified to `id`"
      - "Both scan the table, because the index needs `ANALYZE` first"
      - "`id + 0 = 5` searches the index on `id + 0`, which is created automatically"
    explanation: |-
      Any expression applied to the indexed column hides it from the optimizer's range analysis, even an arithmetic identity. SQLite's `EXPLAIN QUERY PLAN` shows `SCAN t` for `id + 0 = 5` and `SEARCH t USING INDEX ... (id=?)` for `id = 5`. (DBAs have long used `+ 0` or a unary `+` deliberately to *prevent* an index from being chosen.)

      The sargable rewrite of arithmetic moves it to the constant side: `price * 1.2 > 100` becomes `price > 100 / 1.2` (taking care with rounding and negative factors, which flip the inequality).

      "Simplified to `id`" assumes algebraic normalization that optimizers generally do not perform on indexable columns.

      "Needs `ANALYZE`" is unrelated; statistics influence cost choices, not whether a predicate is usable at all.

      "Created automatically" confuses this with expression indexes, which you must create.
    checks:
      - engine: "sqlite"
        setup: "CREATE TABLE t(id INT, v INT); CREATE INDEX t_id ON t(id);"
        query: "EXPLAIN QUERY PLAN SELECT * FROM t WHERE id + 0 = 5"
        expect_contains: "SCAN t"
      - engine: "sqlite"
        setup: "CREATE TABLE t(id INT, v INT); CREATE INDEX t_id ON t(id);"
        query: "EXPLAIN QUERY PLAN SELECT * FROM t WHERE id = 5"
        expect_contains: "SEARCH t USING INDEX t_id"
  - id: "expression-index-match"
    type: "single"
    prompt: "You create `CREATE INDEX users_lower_email ON users (lower(email))`. Which predicate can the engine answer with a seek on that index?"
    correct:
      - "`WHERE lower(email) = 'ann@x.org'`"
    distractors:
      - "`WHERE email = lower('Ann@X.org')`"
      - "`WHERE upper(email) = 'ANN@X.ORG'`"
      - "`WHERE email ILIKE 'ann@x.org'`"
    explanation: |-
      An expression index stores the *value of the expression* as its key, ordered by that value. The optimizer can seek it when a predicate compares the same expression, written the same way, to a constant. `lower(email) = 'ann@x.org'` matches; SQLite's plan shows `SEARCH users USING INDEX users_lower_email (<expr>=?)`.

      "`email = lower('Ann@X.org')`" lowers the constant, not the column; it compares raw `email` values, which the expression index does not store, and it would miss `'Ann@x.org'` anyway.

      "`upper(email) = ...`" is logically similar but a different expression; the optimizer does not prove the two equivalent.

      "`ILIKE`" (PostgreSQL's case-insensitive `LIKE`) is a different operator on the raw column.

      An expression index costs extra work on every write that changes `email`, so it pays off when the expression is queried often.
    checks:
      - engine: "sqlite"
        setup: "CREATE TABLE users(id INTEGER PRIMARY KEY, email TEXT); CREATE INDEX users_lower_email ON users(lower(email));"
        query: "EXPLAIN QUERY PLAN SELECT * FROM users WHERE lower(email) = 'ann@x.org'"
        expect_contains: "USING INDEX users_lower_email"
      - engine: "sqlite"
        setup: "CREATE TABLE users(id INTEGER PRIMARY KEY, email TEXT); CREATE INDEX users_lower_email ON users(lower(email));"
        query: "EXPLAIN QUERY PLAN SELECT * FROM users WHERE upper(email) = 'ANN@X.ORG'"
        expect_contains: "SCAN users"
---
## Learning objectives

- Define a sargable predicate and recognize the common ways predicates lose that property.
- Rewrite function, arithmetic and date predicates into equivalent index-friendly ranges.
- Explain why leading wildcards and implicit conversions defeat index seeks.
- Use expression indexes when the expression itself is what queries filter on.

## The invariant

"Sargable" comes from *Search ARGument ABLE*, the term used in the System R optimizer paper (Selinger et al., 1979) for predicates of the form `column comparison-operator value` that the storage layer can apply while scanning an index. The modern meaning: a predicate is sargable for an index if the set of rows it selects maps to **one or a few contiguous ranges of the index keys**, so the engine can seek instead of scanning.

That gives a mechanical test. Write the predicate as "indexed column, operator, expression not depending on the column":

| Predicate shape | Sargable for an index on `col`? | Why |
|---|---|---|
| `col = c`, `col > c`, `col BETWEEN a AND b`, `col IN (...)` | yes | one or a few key ranges |
| `col LIKE 'abc%'` | yes, with a suitable collation | range `['abc', 'abd')` |
| `f(col) = c` (functions, casts, arithmetic) | no (unless an index on `f(col)` exists) | the index is ordered by `col`, not `f(col)` |
| `col LIKE '%abc'` | no | matches spread across all keys |
| `col = c` with an implicit conversion applied to `col` | no | a conversion is a function on the column |
| `col1 = c1 OR col2 = c2` | not with one index | may use two indexes combined (bitmap/index merge) |

The fix is almost always to move work to the constant side (compute bounds once) or to index the expression itself. Sargability is a property of the predicate *and* the index, so the same predicate can be sargable for one index and not for another.

## Worked example

```sql
-- Non-sargable on an index over created_at
SELECT * FROM events WHERE EXTRACT(YEAR FROM created_at) = 2026;
SELECT * FROM orders WHERE total * 1.2 > 120;
SELECT * FROM users  WHERE lower(email) = 'ann@x.org';

-- Sargable equivalents
SELECT * FROM events
WHERE created_at >= DATE '2026-01-01' AND created_at < DATE '2027-01-01';
SELECT * FROM orders WHERE total > 100;          -- 120 / 1.2, factor positive
CREATE INDEX users_lower_email ON users (lower(email));
SELECT * FROM users  WHERE lower(email) = 'ann@x.org';
```

PostgreSQL shows the difference directly: the `EXTRACT` form plans as `Seq Scan ... Filter: (EXTRACT(year FROM created_at) = ...)`, and the half-open range plans as `Index Only Scan ... Index Cond: ((created_at >= ...) AND (created_at < ...))` when only `created_at` is selected. Half-open ranges are also the correct way to express "a whole year" for timestamps, as the date question shows.

## Where this shows up today

As of November 2026, the MySQL 8.4 manual still warns that comparing an indexed string column with a number prevents index lookups, and PostgreSQL 19 rejects `varchar = integer` comparisons without a cast. ORMs and query builders cause many non-sargable predicates in practice: a `.Date` property access or a culture-aware string comparison often becomes a function call on the column in generated SQL.

## Pitfalls checklist

- Wrapping indexed columns in `lower()`, `CAST`, `DATE()`, `COALESCE()` or arithmetic in `WHERE`.
- Using `BETWEEN` with a date upper bound on timestamp columns.
- Passing parameters of the wrong type, causing implicit conversion of the column.
- Expecting `LIKE '%term'` or `'%term%'` to use a B+tree index; use full-text or trigram indexes instead.
- Creating an expression index and then querying a slightly different expression.

## Key takeaways

- Sargable means the predicate maps to contiguous index key ranges.
- Functions, casts and arithmetic on the column break sargability; move work to the constant side.
- Leading wildcards and implicit conversions are hidden functions on the column.
- Expression indexes make a specific expression seekable, if queries match it exactly.

## Sources

- P. G. Selinger, M. M. Astrahan, D. D. Chamberlin, R. A. Lorie and T. G. Price, "Access Path Selection in a Relational Database Management System", *ACM SIGMOD*, 1979.
- MySQL 8.4 Reference Manual, "Type Conversion in Expression Evaluation".
- PostgreSQL 19 documentation, "Indexes on Expressions" and "Operator Classes and Operator Families"; SQLite documentation, "The SQLite Query Optimizer Overview".
