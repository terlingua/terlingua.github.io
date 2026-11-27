---
title: "Composite Indexes and the Leftmost-Prefix Rule"
questions:
  - id: "leftmost-prefix-seek"
    type: "multiple"
    prompt: "`orders` has a B+tree index on `(customer_id, status, created_at)`. Which predicates let the engine *seek* to a contiguous range of index entries using the index's sort order (rather than scanning the whole index)? Select all that apply."
    correct:
      - "`customer_id = 7`"
      - "`customer_id = 7 AND status = 'open'`"
      - "`customer_id = 7 AND created_at >= DATE '2026-01-01'`"
    distractors:
      - "`status = 'open'`"
      - "`created_at >= DATE '2026-01-01'`"
    explanation: |-
      A composite index is sorted by `customer_id`, then by `status` within each customer, then by `created_at` within each `(customer_id, status)`. Entries matching a condition form one contiguous range only if the condition fixes a **leftmost prefix** of the key.

      | Predicate | Prefix bounded | Seek range |
      |---|---|---|
      | customer_id = 7 | (customer_id) | all of customer 7 |
      | customer_id = 7 AND status = 'open' | (customer_id, status) | customer 7's open orders |
      | customer_id = 7 AND created_at >= ... | (customer_id) only | all of customer 7; `created_at` is checked inside that range |
      | status = 'open' | none | open orders are scattered across every customer |
      | created_at >= ... | none | scattered across every customer and status |

      The third predicate does use the index for a seek, but only on `customer_id`: because `status` is skipped, `created_at` values are not in one ordered run, so the date can only filter entries within the customer's range.

      The two distractors have no leading `customer_id` condition. A plain B+tree descent cannot use them; some engines can still scan the whole index or apply a skip scan, but that is not a prefix seek.
  - id: "equality-before-range"
    type: "single"
    prompt: "Customer 7 has 1,000 orders: 100 are `'open'`, 300 were created on or after day 701, and 30 are both. For `WHERE customer_id = 7 AND status = 'open' AND created_at >= 701`, how many index entries fall inside the scanned range for index A `(customer_id, status, created_at)` and index B `(customer_id, created_at, status)`?"
    correct:
      - "A: 30; B: 300"
    distractors:
      - "A: 30; B: 30"
      - "A: 100; B: 300"
      - "A: 300; B: 30"
    explanation: |-
      An index range is bounded by equality conditions on a leftmost prefix, plus at most one range condition on the *next* column. Columns after the first range column cannot narrow the range; they can only filter entries inside it.

      | Index | Bounded columns | Range covers | Entries |
      |---|---|---|---|
      | A (customer_id, status, created_at) | customer_id =, status =, created_at ≥ | customer 7, open, day ≥ 701 | 30 |
      | B (customer_id, created_at, status) | customer_id =, created_at ≥ | customer 7, day ≥ 701 (all statuses) | 300 |

      In B, `status` comes after the range column, so all 300 entries are read and 270 are discarded.

      "A: 30; B: 30" assumes column order does not matter when all columns are constrained.

      "A: 100" stops A's range at `status`, ignoring that `created_at` follows two equality columns and can still bound it.

      "A: 300; B: 30" reverses the rule.

      Rule of thumb: equality columns first, the range column last.
    checks:
      - engine: "both"
        setup: "CREATE TABLE orders(customer_id INT, status TEXT, created_at INT); WITH RECURSIVE n(i) AS (SELECT 1 UNION ALL SELECT i + 1 FROM n WHERE i < 1000) INSERT INTO orders SELECT 7, CASE WHEN i % 10 = 0 THEN 'open' ELSE 'done' END, i FROM n;"
        query: "SELECT (SELECT COUNT(*) FROM orders WHERE customer_id = 7 AND status = 'open'), (SELECT COUNT(*) FROM orders WHERE customer_id = 7 AND created_at >= 701), (SELECT COUNT(*) FROM orders WHERE customer_id = 7 AND status = 'open' AND created_at >= 701)"
        expect: [[100, 300, 30]]
  - id: "order-by-from-index"
    type: "multiple"
    prompt: "`orders` has an index on `(customer_id, created_at)`. Which queries can return rows in the requested order directly from an index scan, with no sort step? Select all that apply."
    correct:
      - "`SELECT * FROM orders WHERE customer_id = 7 ORDER BY created_at DESC LIMIT 10`"
      - "`SELECT * FROM orders ORDER BY customer_id, created_at`"
    distractors:
      - "`SELECT * FROM orders WHERE customer_id IN (7, 8) ORDER BY created_at`"
      - "`SELECT * FROM orders ORDER BY created_at`"
      - "`SELECT * FROM orders ORDER BY customer_id DESC, created_at ASC`"
    explanation: |-
      Index entries are ordered by `(customer_id, created_at)`. A scan delivers that order forwards, or its exact reverse backwards. A query's `ORDER BY` is satisfied if, after fixing the columns pinned by equality, it matches the remaining key order in one direction.

      - `customer_id = 7 ORDER BY created_at DESC`: within customer 7, entries are ordered by `created_at`; scanning backwards gives descending order, and `LIMIT 10` stops after ten entries. No sort.
      - `ORDER BY customer_id, created_at`: exactly the index order. No sort.

      Distractors (each needs a sort, verified with PostgreSQL `EXPLAIN`):

      - `IN (7, 8) ORDER BY created_at`: two separate runs (customer 7's dates, then customer 8's) must be merged.
      - `ORDER BY created_at`: dates are ordered only within each customer.
      - `customer_id DESC, created_at ASC`: mixed directions match neither the forward nor the backward scan (an index declared `(customer_id DESC, created_at ASC)` would).
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE orders(id INT, customer_id INT, created_at INT); INSERT INTO orders SELECT g, g % 100, g FROM generate_series(1, 50000) g; CREATE INDEX ON orders (customer_id, created_at); ANALYZE orders; SET enable_seqscan = off; SET enable_bitmapscan = off;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM orders WHERE customer_id = 7 ORDER BY created_at DESC LIMIT 10"
        expect_not_contains: "Sort"
      - engine: "postgres"
        setup: "CREATE TABLE orders(id INT, customer_id INT, created_at INT); INSERT INTO orders SELECT g, g % 100, g FROM generate_series(1, 50000) g; CREATE INDEX ON orders (customer_id, created_at); ANALYZE orders; SET enable_seqscan = off; SET enable_bitmapscan = off;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM orders ORDER BY customer_id, created_at"
        expect_not_contains: "Sort"
      - engine: "postgres"
        setup: "CREATE TABLE orders(id INT, customer_id INT, created_at INT); INSERT INTO orders SELECT g, g % 100, g FROM generate_series(1, 50000) g; CREATE INDEX ON orders (customer_id, created_at); ANALYZE orders; SET enable_seqscan = off; SET enable_bitmapscan = off;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM orders WHERE customer_id IN (7, 8) ORDER BY created_at"
        expect_contains: "Sort"
      - engine: "postgres"
        setup: "CREATE TABLE orders(id INT, customer_id INT, created_at INT); INSERT INTO orders SELECT g, g % 100, g FROM generate_series(1, 50000) g; CREATE INDEX ON orders (customer_id, created_at); ANALYZE orders; SET enable_seqscan = off; SET enable_bitmapscan = off;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM orders ORDER BY customer_id DESC, created_at ASC"
        expect_contains: "Sort"
  - id: "skipped-middle-column"
    type: "single"
    prompt: "Index on `(a, b, c)`. There are 2,000 entries with `a = 1`, and 20 of those have `c = 5`. For `WHERE a = 1 AND c = 5`, how many index entries does a classic B+tree range scan examine?"
    correct:
      - "2,000"
    distractors:
      - "20"
      - "Every entry in the index"
      - "None, because the index cannot be used without `b`"
    explanation: |-
      The condition fixes only the prefix `(a)`; `b` is unconstrained, so entries with `c = 5` are spread throughout the `a = 1` range, ordered by `b` first. The scan seeks to the start of `a = 1`, reads all 2,000 entries of that range, and checks `c = 5` on each (inside the index, so non-matching entries cost no table reads).

      | Step | Entries |
      |---|---|
      | seek to `a = 1` | start of range |
      | read the range | 2,000 |
      | keep `c = 5` | 20 |

      "20" assumes `c` can bound the range despite the gap at `b`.

      "Every entry" ignores that `a = 1` is a usable prefix.

      "None" is too strict: a leftmost prefix (here just `a`) is enough to use the index; the later columns only filter.

      An index on `(a, c, b)` would make the range exactly 20 entries.
    checks:
      - engine: "python"
        code: |
          import random
          random.seed(1)
          rows = [(1, b, 5 if i < 20 else 6) for i, b in enumerate(random.sample(range(10000), 2000))]
          rows += [(2, i, 5) for i in range(500)]
          rows.sort()
          rng = [r for r in rows if r[0] == 1]
          assert len(rng) == 2000 and sum(1 for r in rng if r[2] == 5) == 20
  - id: "one-index-for-workload"
    type: "single"
    prompt: "A service runs three queries on `orders`: (1) `WHERE customer_id = ?`; (2) `WHERE customer_id = ? AND status = ?`; (3) `WHERE status = ? AND customer_id = ? ORDER BY created_at`. Which single index lets every query seek using *all* of its equality columns, and lets query 3 avoid a sort?"
    correct:
      - "`(customer_id, status, created_at)`"
    distractors:
      - "`(status, customer_id, created_at)`"
      - "`(customer_id, created_at, status)`"
      - "`(created_at, customer_id, status)`"
    explanation: |-
      Check each query against each index. The order of predicates in `WHERE` is irrelevant; only which columns are constrained matters.

      | Index | Q1 seek on customer_id | Q2 seek on both equalities | Q3 seek on both equalities, no sort |
      |---|---|---|---|
      | (customer_id, status, created_at) | yes | yes | yes: the pinned prefix leaves created_at in order |
      | (status, customer_id, created_at) | no: leading column is status | yes | yes |
      | (customer_id, created_at, status) | yes | no: status comes after created_at, so it only filters | no: the seek covers all statuses (order by created_at still holds) |
      | (created_at, customer_id, status) | no | no | no |

      Only the first index meets every requirement.

      "`(status, customer_id, created_at)`" fails query 1, which constrains only `customer_id`.

      "`(customer_id, created_at, status)`" seeks on `customer_id` only for queries 2 and 3, reading every status and discarding most entries.

      "`(created_at, ...)`" leads with a column none of the queries constrain by equality.
  - id: "skip-scan-postgres"
    type: "single"
    prompt: "In PostgreSQL 19, `events` has an index on `(tenant_id, event_type)`, with only 5 distinct `tenant_id` values. A query filters only `WHERE event_type = 'login'`. What can the planner do with the index?"
    correct:
      - "Use a skip scan: one index search per distinct `tenant_id`, each seeking `event_type = 'login'` within that tenant"
    distractors:
      - "Nothing; a B+tree index can never be used without a condition on its leading column"
      - "Seek directly to `'login'`, because PostgreSQL re-sorts the index by the filtered column"
      - "Use the index only if the query adds `ORDER BY tenant_id`"
    explanation: |-
      Skip scan treats the unconstrained leading column as if it had an equality condition for each of its distinct values: the scan seeks to `(tenant 1, 'login')`, reads that run, skips to `(tenant 2, 'login')`, and so on. With 5 tenants that is 5 short seeks instead of a full scan. It pays off only when the skipped column has few distinct values. PostgreSQL added it in version 18 (the release notes describe it as allowing multicolumn B-tree indexes to be used in more cases), so PostgreSQL 19 has it; MySQL has a similar `skip_scan` optimization.

      "Never usable" was the classic rule and is still the right mental model for *prefix seeks*; skip scan is a smarter traversal, not a different index order.

      "Re-sorts the index" is impossible; an index has one physical order.

      "Only with `ORDER BY tenant_id`" confuses ordering with access; ordering does not supply a value to seek.
---
## Learning objectives

- Explain how a composite B+tree index is ordered and which predicates can seek into it.
- Apply "equality columns first, range column last" when choosing column order.
- Predict when an index delivers `ORDER BY` without a sort.
- Pick one composite index that serves several queries.

## The invariant

A composite index on `(c1, c2, ..., cn)` is a B+tree whose keys are compared **lexicographically**: by `c1`, then `c2` among equal `c1`, and so on. Everything about composite indexes follows from that ordering:

1. **Leftmost prefix.** The entries satisfying a predicate form one contiguous key range only if the predicate constrains a leftmost prefix `c1 … ck` with equalities, optionally followed by one range condition on `c(k+1)`.
2. **Range stops the prefix.** After the first range-constrained column, later columns are no longer sorted within the range, so their conditions can only filter entries inside it.
3. **Order for free.** If equalities pin `c1 … ck`, the matching entries are ordered by `c(k+1), c(k+2), …`; a forward or backward scan returns that order (or its exact reverse) without sorting.
4. **Filtering in the index.** Conditions on non-prefix columns can still be evaluated against index entries, avoiding table reads for non-matching rows, but they do not shrink the range.

The SQL standard defines none of this; it is a property of ordered indexes, the same in every engine that uses B+trees.

## Worked example

`orders(customer_id, status, created_at)` with index `(customer_id, status, created_at)`:

| Query predicate | Range bounded by | Notes |
|---|---|---|
| `customer_id = 7` | c1 | all of customer 7 |
| `customer_id = 7 AND status = 'open'` | c1, c2 | |
| `customer_id = 7 AND status = 'open' AND created_at >= 701` | c1, c2, c3 | tightest range |
| `customer_id = 7 AND created_at >= 701` | c1 | `created_at` filters inside the range |
| `customer_id > 5 AND status = 'open'` | c1 (range) | `status` only filters |
| `status = 'open'` | none | full index scan, or skip scan if supported |

And for ordering:

```sql
-- No sort: customer and status pinned, created_at ordered within them
SELECT * FROM orders
WHERE customer_id = 7 AND status = 'open'
ORDER BY created_at DESC
FETCH FIRST 10 ROWS ONLY;
```

This "latest N for an entity" shape is the most valuable composite index pattern: the engine reads exactly ten index entries (plus their rows) regardless of how many orders the customer has.

## Where this shows up today

As of November 2026, PostgreSQL 19 includes the B-tree skip scan introduced in PostgreSQL 18, and the MySQL 8.4 manual lists a `skip_scan` optimizer switch that is on by default. Skip scans relax the leftmost-prefix rule only when the skipped columns have few distinct values; designing indexes by the prefix rule remains the reliable approach.

## Pitfalls checklist

- Creating `(status, customer_id)` when most queries filter only on `customer_id`.
- Putting a range column before an equality column, inflating the scanned range.
- Assuming predicate order in `WHERE` must match index column order; it need not.
- Adding a separate single-column index for every column instead of one well-ordered composite.
- Expecting an index to remove a sort when the query mixes sort directions the index lacks.

## Key takeaways

- Composite keys sort lexicographically; seeks need a leftmost prefix.
- Equality columns first, then the single range column; later columns only filter.
- Pinning a prefix with equalities yields the remaining columns in index order, avoiding sorts.
- One well-ordered composite index often replaces several single-column indexes.

## Sources

- D. Comer, "The Ubiquitous B-Tree", *ACM Computing Surveys* 11(2), 1979.
- PostgreSQL 19 documentation, "Multicolumn Indexes" and "Indexes and ORDER BY"; PostgreSQL 18 release notes (skip scan).
- MySQL 8.4 Reference Manual, "Multiple-Column Indexes" and "Switchable Optimizations" (`skip_scan`).
