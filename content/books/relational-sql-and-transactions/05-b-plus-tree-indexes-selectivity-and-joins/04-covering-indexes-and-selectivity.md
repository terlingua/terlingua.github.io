---
title: "Covering Indexes, Selectivity and When a Scan Wins"
questions:
  - id: "covered-queries"
    type: "multiple"
    prompt: "`orders(id, customer_id, status, created_at, total)` has an index on `(customer_id, created_at)`. Which queries can be answered from the index alone, without reading table rows? Select all that apply."
    explanation: |-
      An index **covers** a query when every column the query references (in `SELECT`, `WHERE`, `ORDER BY`, `GROUP BY` and joins) is stored in the index. The engine can then skip the table entirely.

      | Query | Columns needed | In the index? |
      |---|---|---|
      | SELECT created_at ... WHERE customer_id = 7 | customer_id, created_at | yes |
      | SELECT COUNT(*) ... customer_id, created_at range | customer_id, created_at | yes |
      | SELECT total ... | customer_id, total | `total` is missing |
      | SELECT * ... | all columns | no |
      | SELECT created_at ... WHERE status = 'open' | status, created_at | `status` is missing |

      SQLite labels the first two plans `USING COVERING INDEX`. Covering removes the per-row table fetch, which is usually the dominant cost of an index plan.
    options:
      - text: "`SELECT total FROM orders WHERE customer_id = 7`"
      - text: "`SELECT * FROM orders WHERE customer_id = 7`"
      - text: "`SELECT created_at FROM orders WHERE customer_id = 7`"
        correct: true
      - text: "`SELECT COUNT(*) FROM orders WHERE customer_id = 7 AND created_at >= 701`"
        correct: true
      - text: "`SELECT created_at FROM orders WHERE status = 'open'`"
  - id: "selectivity-estimate"
    type: "single"
    prompt: "`tickets` has 1,000,000 rows and a `status` column with 50 distinct values, uniformly distributed. Assuming uniformity, what are the selectivity of `status = 'escalated'` and the estimated row count?"
    explanation: |-
      Selectivity is the fraction of rows a predicate keeps. Under the uniform assumption, an equality on a column with d distinct values has selectivity 1/d.

      | Quantity | Value |
      |---|---|
      | selectivity | 1 / 50 = 0.02 = 2% |
      | estimated rows | 1,000,000 × 0.02 = 20,000 |

      "50%" confuses the number of distinct values with a percentage.

      "0.02%" slips a factor of 100 (0.02 is already the fraction).

      "1/50 of the pages" confuses rows with pages; 20,000 matching rows may sit on anywhere from about 200 to 20,000 pages depending on physical order (next questions).

      Real optimizers refine the uniform guess with most-common-value lists and histograms, which matter for skewed columns like `status`.
    options:
      - text: "50%, about 500,000 rows"
      - text: "0.02%, about 200 rows"
      - text: "1/50 of the pages, so about 20,000 pages"
      - text: "2%, about 20,000 rows"
        correct: true
  - id: "scan-versus-index-crossover"
    type: "single"
    prompt: "A table has 1,000,000 rows on 10,000 pages (100 rows per page). Reading a page sequentially costs 1 unit; reading a random page costs 4 units. An index range scan fetches each matching row with one random page read, because matching rows are scattered. Ignoring index-page and CPU costs, at about how many matching rows does the index plan cost the same as a full sequential scan?"
    explanation: |-
      Full scan cost = pages × sequential cost = 10,000 × 1 = 10,000 units.
      Index cost for k scattered rows = k × random cost = 4k units.

      Setting 4k = 10,000 gives k = 2,500 rows, a selectivity of 2,500 / 1,000,000 = 0.25%.

      | matching rows | index cost | scan cost | cheaper |
      |---|---|---|---|
      | 1,000 | 4,000 | 10,000 | index |
      | 2,500 | 10,000 | 10,000 | equal |
      | 20,000 | 80,000 | 10,000 | scan |

      "10,000 rows" equates rows with pages and ignores the 4× random penalty.

      "250,000 rows" is the common "use an index below 25%" folklore; with scattered rows the break-even is far lower.

      "40,000 rows" multiplies by the random cost instead of dividing.

      This is why optimizers often choose a full scan for predicates that "only" match a few percent of a table, and why the ratio 4:1 here matches PostgreSQL's default `random_page_cost` to `seq_page_cost`.
    options:
      - text: "2,500 rows (0.25% of the table)"
        correct: true
      - text: "10,000 rows (1% of the table)"
      - text: "250,000 rows (25% of the table)"
      - text: "40,000 rows (4% of the table)"
  - id: "clustering-changes-cost"
    type: "single"
    prompt: "Same table (10,000 pages, 100 rows per page). A range on `created_at` matches 20,000 rows. How many table pages must the index plan read if the table is physically stored in `created_at` order, versus if matching rows are scattered uniformly?"
    explanation: |-
      When the table's physical order matches the index order (high correlation, a clustered table), consecutive index entries point to consecutive rows on the same pages: 20,000 rows ÷ 100 rows per page ≈ 200 pages, read almost sequentially.

      When rows are scattered, each match can be on a different page. 20,000 random fetches touch at most all 10,000 pages (many pages hold more than one match, and each page is read once if the engine sorts the row pointers first, as bitmap scans do).

      | Physical order | pages read | access pattern |
      |---|---|---|
      | correlated with created_at | ≈ 200 | sequential-ish |
      | scattered | up to 10,000 | random |

      "20,000 in both cases" counts rows as pages.

      "200 in both cases" ignores physical order, the very thing that makes the same index cheap or expensive.

      "2,000" slips a factor of 10.

      Optimizers track this as the correlation between index order and table order (PostgreSQL's `pg_stats.correlation`), and it can move the scan-versus-index crossover by orders of magnitude.
    options:
      - text: "About 20,000 pages in both cases"
      - text: "About 200 pages if stored in order; up to about 10,000 pages if scattered"
        correct: true
      - text: "About 200 pages in both cases"
      - text: "About 2,000 pages if stored in order; about 20,000 if scattered"
  - id: "include-columns"
    type: "single"
    prompt: "An index is declared `(customer_id) INCLUDE (total)`. What does the `INCLUDE` part do?"
    explanation: |-
      Included (non-key) columns live only in the leaf entries. They are not used to order the tree or to navigate it, so they add no seek or ordering capability, but they let the index cover more queries. They also do not participate in uniqueness when the index is `UNIQUE`, which is a common reason to use them: a unique key on `customer_id` plus covered payload.

      | Capability | key column | INCLUDE column |
      |---|---|---|
      | narrows a seek range | yes | no |
      | provides ORDER BY | yes | no |
      | checked by UNIQUE | yes | no |
      | available for covering | yes | yes |

      Keeping payload out of the key keeps internal pages small (higher fanout) while still avoiding table fetches.

      Each distractor attributes a key-column capability to an included column.

      "Stored in every internal page" is the opposite of the design: included columns stay out of internal pages, which is what keeps fanout high.
    options:
      - text: "It makes `total` a second key column after `customer_id`, so `WHERE total > 100` can seek the index even without a customer"
      - text: "It stores `total` in leaf entries only, so `customer_id` lookups reading `total` are covered, but `total` is not a search key"
        correct: true
      - text: "It sorts entries by `total` within each `customer_id`, so `ORDER BY total` needs no sort"
      - text: "It adds a uniqueness check on `total` within each `customer_id`"
      - text: "It stores `total` in every internal page, so the tree can be navigated by `total` as well"
  - id: "index-only-heap-fetches"
    type: "single"
    prompt: "In PostgreSQL 19, `EXPLAIN ANALYZE` of a query on a freshly loaded table shows `Index Only Scan ... Heap Fetches: 999`. After `VACUUM`, the same plan shows `Heap Fetches: 0`. What explains the difference?"
    explanation: |-
      PostgreSQL is a multiversion engine: an index entry may point to a row version that the current transaction must not see, and index entries carry no visibility information. An index-only scan can skip the heap only for pages marked *all-visible* in the visibility map; for other pages it fetches the heap tuple to check visibility. A freshly loaded table has no all-visible pages until `VACUUM` (or autovacuum) sets the bits, so every entry needed a heap fetch: 999 of 999.

      | State | all-visible pages | Heap Fetches for 999 rows |
      |---|---|---|
      | after bulk insert | none | 999 |
      | after VACUUM | all | 0 |

      "Not yet built" contradicts the plan, which already used the index.

      "Rebuilt to include all columns" is not what `VACUUM` does.

      "Cache misses" misreads the counter; it counts heap visits for visibility checks, cached or not.

      The general lesson: "covering" removes table reads only as far as the engine can prove visibility from the index side.

      "ANALYZE had not run" confuses planning with execution; statistics affect plan choice, and Heap Fetches is counted while executing the chosen plan.
    options:
      - text: "The index was not yet built when the first query ran, so the heap was read instead"
      - text: "VACUUM rebuilt the index so that it now stores every table column, which made the index-only scan possible"
      - text: "Until VACUUM marks pages all-visible, the scan must visit the heap to check each row version's visibility"
        correct: true
      - text: "The first run cached nothing, and Heap Fetches counts cache misses"
      - text: "ANALYZE had not run, so the planner fetched rows to estimate selectivity"
---
## Learning objectives

- Decide whether an index covers a query and why covering matters.
- Compute selectivity and estimated row counts under the uniformity assumption.
- Calculate the break-even point between an index plan and a full scan.
- Explain how physical clustering and included columns change index costs.

## The invariant

An index plan has two costs: navigating the index (small, logarithmic) and fetching each matching row from the table (one page access per row in the worst case). A full scan has one cost: reading every page sequentially. Which wins depends on three numbers:

| Quantity | Meaning |
|---|---|
| selectivity s | fraction of rows matched; for `col = c` under uniformity, 1 / distinct values |
| clustering | whether matching rows are physically adjacent (correlated) or scattered |
| random-to-sequential cost ratio r | how much more a random page read costs than a sequential one |

For N rows on P pages with scattered matches, the index plan costs about s·N·r and the scan costs about P. The break-even selectivity is s* = P / (N·r). With 100 rows per page and r = 4, s* = 1 / 400 = 0.25%: an index on scattered rows only wins for very selective predicates. If rows are clustered in index order, the index plan reads about s·P pages nearly sequentially, and it wins for much larger s.

Two techniques remove the table fetch altogether:

- **Covering indexes**: every column the query needs is in the index (as key columns or as included payload), so the table is not read. Included columns travel in the leaves only, so they do not lower the fanout of internal pages.
- **Clustering**: storing the table in index order (a clustered index in InnoDB and SQL Server; a periodic `CLUSTER` in PostgreSQL) turns random fetches into sequential ones for that one order.

None of this is in the SQL standard; it is the cost model that every optimizer since System R (Selinger et al., 1979) uses in some form.

## Worked example

Table `tickets`: 1,000,000 rows, 10,000 pages, index on `status` (50 values) and on `created_at` (table loaded in `created_at` order).

| Predicate | s | rows | index plan pages | scan pages | likely plan |
|---|---|---|---|---|---|
| `id = 42` (unique) | 0.0001% | 1 | 3–4 | 10,000 | index |
| `status = 'escalated'` | 2% | 20,000 | up to 10,000 random (×4 cost) | 10,000 | scan |
| `created_at` in one day (clustered) | 0.3% | 3,000 | ≈ 30 sequential | 10,000 | index |
| `status = 'escalated'`, covering index on `(status, id)` | 2% | 20,000 | ≈ 40 index leaves | 10,000 | index-only |

```sql
-- Turn the second case into the fourth with a covering index
CREATE INDEX tickets_status_id ON tickets (status, id);
SELECT id FROM tickets WHERE status = 'escalated';
```

The same predicate flips from "scan" to "index" purely by changing what the index stores.

## Where this shows up today

As of November 2026, the PostgreSQL 19 documentation lists `seq_page_cost` 1.0 and `random_page_cost` 4.0 as defaults, explaining that real random access is usually much slower but is assumed to be mostly cached, and that storage with cheap random reads may be better modeled with a lower value. `INCLUDE` columns are available in PostgreSQL and SQL Server. Index-only scans in multiversion engines also depend on visibility information, as the last question shows.

## Pitfalls checklist

- Expecting an index on a low-cardinality column to beat a scan for a common value.
- Adding `SELECT *` to a query that a covering index could have served.
- Using the "25% rule" for index usefulness; break-even depends on clustering and cost ratio.
- Putting payload columns into the key (hurting fanout and uniqueness semantics) instead of `INCLUDE`.
- Forgetting that physical order decays with updates unless the table is clustered by design.

## Key takeaways

- Index plans cost a table fetch per matching row unless the index covers the query.
- Break-even selectivity ≈ pages ÷ (rows × random-cost ratio), often well below 1% for scattered rows.
- Clustering in index order makes range plans cheap even at higher selectivity.
- `INCLUDE` adds coverage without changing key order, seeks or uniqueness.

## Sources

- P. G. Selinger et al., "Access Path Selection in a Relational Database Management System", *ACM SIGMOD*, 1979.
- PostgreSQL 19 documentation, "Query Planning" (planner cost constants), "Index-Only Scans and Covering Indexes" and "Visibility Map".
- Microsoft Learn, "Create indexes with included columns"; SQLite documentation, "The SQLite Query Optimizer Overview" (covering indexes).
