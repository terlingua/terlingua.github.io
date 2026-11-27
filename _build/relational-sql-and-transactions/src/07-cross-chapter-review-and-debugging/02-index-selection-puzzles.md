---
title: "Index-Selection Puzzles"
questions:
  - id: "sargable-plus-prefix"
    type: "single"
    prompt: "`events` has an index on `(tenant_id, created_at)`. Tenant 7 has 50,000 events, 200 of them on 2026-11-01. How many index entries fall inside the scanned range for each predicate?\n\n```sql\n-- P1\nWHERE tenant_id = 7 AND DATE(created_at) = DATE '2026-11-01'\n-- P2\nWHERE tenant_id = 7 AND created_at >= DATE '2026-11-01'\n                    AND created_at <  DATE '2026-11-02'\n```"
    correct:
      - "P1: 50,000; P2: 200"
    distractors:
      - "P1: 200; P2: 200"
      - "P1: every entry in the index; P2: 200"
      - "P1: 50,000; P2: 50,000"
    explanation: |-
      Two rules combine: the leftmost-prefix rule (Chapter 5, composite indexes) and sargability (Chapter 5, sargable predicates).

      - P1: `tenant_id = 7` is a sargable equality on the leading column, so the scan seeks tenant 7's range. `DATE(created_at)` wraps the second column in a function, so it cannot narrow the range; it is checked on each of the 50,000 entries. PostgreSQL shows `Index Cond: (tenant_id = 7)` plus a separate `Filter` on `date(created_at)`.
      - P2: the half-open timestamp range is sargable on the second column, right after an equality: `Index Cond` contains all three conditions, and the range holds 200 entries.

      "200 and 200" assumes the function is harmless because the first column is already bounded.

      "Every entry" for P1 ignores that the leading equality is still usable.

      "50,000 for P2" ignores that a range on the column after an equality prefix narrows the scan.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE events(tenant_id INT, created_at TIMESTAMP); INSERT INTO events SELECT g % 50, TIMESTAMP '2026-01-01' + (g || ' minutes')::interval FROM generate_series(1, 200000) g; CREATE INDEX ON events(tenant_id, created_at); ANALYZE events;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM events WHERE tenant_id = 7 AND DATE(created_at) = DATE '2026-03-01'"
        expect_contains: "Filter: (date(created_at)"
      - engine: "postgres"
        setup: "CREATE TABLE events(tenant_id INT, created_at TIMESTAMP); INSERT INTO events SELECT g % 50, TIMESTAMP '2026-01-01' + (g || ' minutes')::interval FROM generate_series(1, 200000) g; CREATE INDEX ON events(tenant_id, created_at); ANALYZE events;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM events WHERE tenant_id = 7 AND created_at >= DATE '2026-03-01' AND created_at < DATE '2026-03-02'"
        expect_contains: "Index Cond: ((tenant_id = 7) AND (created_at >="
  - id: "latest-n-per-tenant-index"
    type: "single"
    prompt: "The hottest query is `SELECT * FROM events WHERE tenant_id = ? AND created_at >= ? ORDER BY created_at DESC FETCH FIRST 20 ROWS ONLY`. Which index lets it seek on both conditions *and* return rows without a sort?"
    correct:
      - "`(tenant_id, created_at)`"
    distractors:
      - "`(created_at, tenant_id)`"
      - "`(tenant_id, status, created_at)`"
      - "Two separate indexes, `(tenant_id)` and `(created_at)`"
    explanation: |-
      Apply "equality first, range last" and "a pinned prefix yields the next column in order" (Chapter 5).

      | Index | Seek | Order without sort |
      |---|---|---|
      | (tenant_id, created_at) | tenant equality, then created_at range | yes: within the tenant, entries are ordered by created_at; scan backwards and stop after 20 |
      | (created_at, tenant_id) | created_at range only; tenant filtered inside | rows come in created_at order across all tenants, so many entries are read and discarded |
      | (tenant_id, status, created_at) | tenant only (status unconstrained) | no: within a tenant, entries are ordered by status first |
      | two single-column indexes | each alone, or combined by bitmap | no: bitmap results are in physical order, needing a sort |

      With the first index, the query reads about 20 index entries plus 20 rows, regardless of how many events the tenant has.
  - id: "covering-beats-selectivity"
    type: "single"
    prompt: "`tickets` has 1,000,000 rows on 10,000 pages; 30% have `status = 'open'`. There is an index on `(status)`, and the table is freshly vacuumed. Why can `SELECT COUNT(*) FROM tickets WHERE status = 'open'` still use the index efficiently, despite 30% selectivity?"
    correct:
      - "The index covers the query: an index-only scan reads about 300,000 small leaf entries in order and never visits the table pages"
    distractors:
      - "Because 30% is below the 50% selectivity threshold under which optimizers always prefer an index"
      - "Because `COUNT(*)` is precomputed and stored in the index root"
      - "It cannot; with 30% selectivity a full table scan is always cheaper"
    explanation: |-
      Chapter 5's break-even analysis assumed each matching row costs a random *table* fetch. With scattered rows, the index loses long before 30%. But this query needs no table columns: `COUNT(*)` with a predicate on `status` is covered by the index on `status`. An index-only scan walks the leaf range for `'open'` sequentially, and with pages marked all-visible (freshly vacuumed) it skips the heap entirely. Index leaves are much smaller than table rows, so reading 30% of them can be cheaper than reading 100% of the table.

      "A 50% threshold" is folklore; break-even depends on clustering, cost ratios and whether table fetches are needed.

      "`COUNT(*)` stored in the root" is not how B+trees work; counts are computed by scanning entries.

      "Always cheaper" ignores covering; it is right only when every match requires a table fetch.
  - id: "join-algorithm-by-cardinality"
    type: "single"
    prompt: "`orders` joins `customers` on `customers.id` (primary key, 3-level B+tree). A filter on `orders` yields 50 rows in report A and 2,000,000 rows in report B; `customers` has 1,000,000 rows on 10,000 pages. Which plans are sensible?"
    correct:
      - "A: index nested loop into `customers`; B: hash join with `customers` as the build side"
    distractors:
      - "A: hash join; B: index nested loop"
      - "Index nested loop for both, because the join key is a primary key"
      - "Merge join for both, because primary keys are always sorted"
    explanation: |-
      Cost the options (Chapter 5, join algorithms):

      | Report | Index nested loop on customers | Hash join (scan customers once) |
      |---|---|---|
      | A, 50 outer rows | 50 × 4 ≈ 200 page reads | ≈ 10,000 page reads + build |
      | B, 2,000,000 outer rows | 2,000,000 × 4 ≈ 8,000,000 page reads | ≈ 10,000 page reads + build; probe streams |

      Small outer input: probe the index. Large outer input: read `customers` once into a hash table (it is the smaller side) and stream orders through it. The cardinality *estimate* drives this choice, so a misestimate (say 50 estimated, 2,000,000 actual) produces the catastrophic plan.

      "A: hash, B: nested loop" is exactly backwards.

      "Nested loop for both" ignores that per-row probes scale linearly with the outer input.

      "Merge join for both" assumes both inputs arrive sorted on the key; the filtered `orders` rows do not, and sorting 2,000,000 rows has its own cost.
    checks:
      - engine: "python"
        code: |
          assert 50 * 4 == 200 and 2_000_000 * 4 == 8_000_000
  - id: "or-across-columns"
    type: "single"
    prompt: "In PostgreSQL, a lookup uses `WHERE email = ? OR phone = ?`. Which index setup lets the planner avoid a full table scan?"
    correct:
      - "Separate indexes on `email` and on `phone`, whose results are combined with a bitmap OR"
    distractors:
      - "One composite index on `(email, phone)`"
      - "One composite index on `(phone, email)`"
      - "One index on `email` only, because the planner can derive the matching `phone` values from it"
    explanation: |-
      An `OR` across two columns selects the *union* of two sets that live in different key orders. No single composite index orders both: `(email, phone)` can seek `email = ?` but finds `phone = ?` rows scattered across all emails (and vice versa). Two single-column indexes each give a sargable range; PostgreSQL scans both, ORs the resulting bitmaps of row locations, and visits only those table pages (`BitmapOr` in `EXPLAIN`). MySQL's index-merge union does the same job.

      The composite indexes fail for the second disjunct, so the planner must scan (or scan a whole index).

      "Derive `phone` from `email`" invents a dependency the index cannot know.

      An equivalent manual rewrite is `... WHERE email = ? UNION SELECT ... WHERE phone = ?`, each branch using its own index.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE u(id INT, email TEXT, phone TEXT); INSERT INTO u SELECT g, 'e' || g, 'p' || g FROM generate_series(1, 100000) g; CREATE INDEX ON u(email); CREATE INDEX ON u(phone); ANALYZE u;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM u WHERE email = 'e5' OR phone = 'p7'"
        expect_contains: "BitmapOr"
  - id: "null-and-indexes"
    type: "multiple"
    prompt: "`users.deleted_at` is NULL for active users (1% of rows). Which statements are true in PostgreSQL? Select all that apply."
    correct:
      - "`WHERE deleted_at = NULL` returns no rows, whatever the data"
      - "A B-tree index on `deleted_at` can serve `WHERE deleted_at IS NULL` with an index condition"
      - "A partial index `ON users (id) WHERE deleted_at IS NULL` can serve `WHERE deleted_at IS NULL AND id < 5000`"
    distractors:
      - "`WHERE deleted_at IS NULL` is non-sargable, because NULL is an unknown value"
      - "Any index on `deleted_at` is useless here, because B-tree indexes never contain NULL keys"
    explanation: |-
      - `deleted_at = NULL` is UNKNOWN for every row (Chapter 3), so `WHERE` keeps nothing: 0 rows.
      - PostgreSQL B-tree indexes store NULL keys (sorted last by default), and `IS NULL` is an indexable condition: `EXPLAIN` shows `Index Cond: (deleted_at IS NULL)`.
      - A partial index contains only the rows satisfying its `WHERE`; a query whose predicate implies that condition can use it. It is small (1% of rows), and here even index-only.

      Distractors:

      - `IS NULL` is a well-defined, two-valued test, and it maps to one contiguous key range (all NULLs together), so it is sargable.
      - "B-trees never contain NULLs" is an engine-specific belief (some engines omit entries whose key columns are all NULL); it is false for PostgreSQL, SQL Server and InnoDB, so check your engine rather than assuming.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE users(id INT, deleted_at TIMESTAMP); INSERT INTO users SELECT g, CASE WHEN g % 100 = 0 THEN NULL ELSE now() END FROM generate_series(1, 100000) g; CREATE INDEX users_deleted ON users(deleted_at); ANALYZE users;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM users WHERE deleted_at IS NULL"
        expect_contains: "Index Cond: (deleted_at IS NULL)"
      - engine: "postgres"
        setup: "CREATE TABLE users(id INT, deleted_at TIMESTAMP); INSERT INTO users SELECT g, CASE WHEN g % 100 = 0 THEN NULL ELSE now() END FROM generate_series(1, 100000) g; CREATE INDEX users_active ON users(id) WHERE deleted_at IS NULL; ANALYZE users;"
        query: "EXPLAIN (COSTS OFF) SELECT id FROM users WHERE deleted_at IS NULL AND id < 5000"
        expect_contains: "users_active"
      - engine: "both"
        setup: "CREATE TABLE users(id INT, deleted_at TEXT); INSERT INTO users VALUES (1, NULL), (2, 'x');"
        query: "SELECT COUNT(*) FROM users WHERE deleted_at = NULL"
        expect: [[0]]
---
## Learning objectives

- Choose composite indexes by combining the leftmost-prefix, equality-before-range and ordering rules.
- Spot predicates that defeat an otherwise good index.
- Decide when covering changes the scan-versus-index verdict.
- Match join algorithms and index combinations to cardinalities and predicate shapes.

## The invariant

Index design is a small search problem with fixed rules. For each important query, write down:

1. **Equality columns** (pinned exactly), **one range column**, and **order columns** (`ORDER BY`).
2. **Columns needed but not filtered** (candidates for covering).
3. **Expected cardinality**: rows matched, and rows on the other side of each join.

Then apply the rules from Chapter 5:

| Rule | Consequence for the index |
|---|---|
| leftmost prefix | equality columns first |
| range stops the prefix | the range column comes after all equalities; later columns only filter |
| pinned prefix yields order | an `ORDER BY` column placed right after the equalities avoids sorts |
| sargability | predicates must compare bare columns (or exactly an indexed expression) |
| covering | add needed columns (as key or `INCLUDE`) to skip table fetches |
| selectivity and clustering | scattered matches above a fraction of a percent favor scans unless covered |
| join cardinality | small outer: index nested loop; large equi-join: hash; sorted inputs: merge |

Most bad plans trace back to one rule being violated, or to a cardinality estimate being wrong, which makes the optimizer apply the right rule to the wrong numbers.

The cost of an index is paid on every write: each insert, each update of an indexed column and each delete must maintain every index on the table, and each index competes for cache. So the goal is not "an index for every query" but the smallest set of indexes whose column orders serve the important queries. Composite indexes often let one index replace two or three single-column ones, because a query that pins a leftmost prefix can use the longer index just as well as a shorter one with the same prefix. Conversely, an index whose leading column no important query constrains is usually dead weight.

## Worked example

A multi-tenant `events(tenant_id, created_at, status, payload)` table and three queries:

| Query | Equality | Range | Order | Needs |
|---|---|---|---|---|
| Q1 latest 20 events of a tenant since t | tenant_id | created_at | created_at | all columns |
| Q2 count of open events of a tenant | tenant_id, status | | | nothing else |
| Q3 events of a tenant on a given day | tenant_id | created_at (as a half-open range) | | all columns |

```sql
CREATE INDEX events_tenant_time   ON events (tenant_id, created_at);   -- Q1, Q3
CREATE INDEX events_tenant_status ON events (tenant_id, status);       -- Q2, covered
```

Q3 must be written as `created_at >= DATE '2026-11-01' AND created_at < DATE '2026-11-02'`; written with `DATE(created_at) = ...`, PostgreSQL seeks only `tenant_id = 7` and filters every one of the tenant's rows. Q2 is answered by an index-only scan on the second index. Two indexes serve three queries because each index's column order was derived from the queries, not from the table.

## Where this shows up today

As of November 2026, PostgreSQL (16.15 in this book's checks, with the same behavior documented for 19) plans an `OR` across two indexed columns as a `BitmapOr`, uses B-tree indexes for `IS NULL`, and supports partial indexes; MySQL 8.4 documents index-merge union for the `OR` case. Workload-driven index advisors in managed databases automate the bookkeeping, but the rules they apply are the ones above.

## Pitfalls checklist

- Designing indexes per table instead of per query.
- Wrapping the range column in a function and losing half of a composite index.
- Expecting a composite index to serve an `OR` across its columns.
- Judging an index by selectivity alone and ignoring covering and clustering.
- Trusting a plan without comparing estimated and actual row counts.

## Key takeaways

- Order composite keys as equality columns, then the range or order column.
- Sargable predicates are what let the later columns of a composite index help.
- Covering can make an index win even at high selectivity.
- Cardinality decides the join algorithm; check estimates against reality.

## Sources

- D. Comer, "The Ubiquitous B-Tree", *ACM Computing Surveys* 11(2), 1979.
- P. G. Selinger et al., "Access Path Selection in a Relational Database Management System", *ACM SIGMOD*, 1979.
- PostgreSQL 19 documentation, "Combining Multiple Indexes", "Partial Indexes" and "Index-Only Scans and Covering Indexes"; MySQL 8.4 Reference Manual, "Index Merge Optimization".
- Chapter 5 of this book.
