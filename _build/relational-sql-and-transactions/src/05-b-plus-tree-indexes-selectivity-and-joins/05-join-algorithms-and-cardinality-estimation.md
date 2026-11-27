---
title: "Join Algorithms and Cardinality Estimation"
questions:
  - id: "index-nested-loop-cost"
    type: "single"
    prompt: "A filter on `orders` leaves 1,000 rows. Each must be joined to `customers` (1,000,000 rows on 10,000 pages) on its primary key, whose B+tree has 3 levels. Ignoring caching, about how many page reads does an index nested-loop join spend on `customers`, compared with one full scan of `customers`?"
    correct:
      - "About 4,000 page reads, versus 10,000 for a full scan"
    distractors:
      - "About 3,000 page reads, versus 10,000 for a full scan"
      - "About 1,000 page reads, versus 1,000,000 for a full scan"
      - "About 10,000,000 page reads, because every outer row scans the inner table"
    explanation: |-
      An index nested loop probes the inner index once per outer row: a root-to-leaf descent (3 pages) plus one table page to fetch the customer row.

      | Plan | Computation | Page reads |
      |---|---|---|
      | index nested loop | 1,000 probes × (3 index + 1 table) | 4,000 |
      | full scan of customers | 10,000 pages × 1 | 10,000 |

      (With the upper index levels cached, as they normally are, the loop costs closer to 2,000.) So the nested loop wins here; with 100,000 outer rows it would cost about 400,000 page reads and a hash join over one scan would win.

      "About 3,000" forgets the table fetch.

      "1,000 vs 1,000,000" counts rows, not pages.

      "10,000,000" describes a *naive* nested loop that scans the whole inner table for every outer row (1,000 × 10,000 pages), which is exactly what an index avoids.
    checks:
      - engine: "python"
        code: |
          assert 1000 * (3 + 1) == 4000
          assert 100_000 * 4 == 400_000 and 1000 * 10_000 == 10_000_000
  - id: "hash-join-needs-equality"
    type: "multiple"
    prompt: "Which join conditions can a hash join use as its hash key? Select all that apply."
    correct:
      - "`ON a.customer_id = b.id`"
      - "`ON a.region = b.region AND a.year = b.year`"
    distractors:
      - "`ON a.ts BETWEEN b.start_ts AND b.end_ts`"
      - "`ON a.amount < b.limit_amount`"
      - "`ON a.name LIKE b.pattern`"
    explanation: |-
      A hash join builds a hash table on the join key of one input and probes it with the other. Hashing maps *equal* keys to the same bucket and says nothing about order or patterns, so only equality conditions (including multi-column equality, hashed as a composite key) can drive it.

      - `a.customer_id = b.id`: equi-join, hashable.
      - `a.region = b.region AND a.year = b.year`: composite equi-join, hashable on `(region, year)`.

      Distractors:

      - `BETWEEN` and `<` are inequality (band or theta) joins: matching rows are not in the same hash bucket. Engines use nested loops (often with an index range probe) or merge-based band joins. PostgreSQL plans the `BETWEEN` join as `Nested Loop` with a `Join Filter`.
      - `LIKE` with a column pattern is not an equality either.

      A join may combine an equality key with extra non-equality conditions; the hash join uses the equality part and checks the rest as a filter.
    checks:
      - engine: "postgres"
        setup: "CREATE TABLE a(ts INT); CREATE TABLE b(start_ts INT, end_ts INT); INSERT INTO a SELECT generate_series(1, 1000); INSERT INTO b SELECT g, g + 5 FROM generate_series(1, 1000, 10) g; ANALYZE a; ANALYZE b;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM a JOIN b ON a.ts BETWEEN b.start_ts AND b.end_ts"
        expect_contains: "Nested Loop"
      - engine: "postgres"
        setup: "CREATE TABLE a(k INT); CREATE TABLE b(k INT); INSERT INTO a SELECT generate_series(1, 100000); INSERT INTO b SELECT generate_series(1, 100000); ANALYZE a; ANALYZE b;"
        query: "EXPLAIN (COSTS OFF) SELECT * FROM a JOIN b ON a.k = b.k"
        expect_contains: "Hash Join"
  - id: "merge-join-when"
    type: "single"
    prompt: "When is a sort-merge join the natural choice?"
    correct:
      - "When both inputs already arrive sorted on the join key, for example from index scans, so one pass joins them"
    distractors:
      - "When one input is tiny and the other has an index on the join key, so each outer row can be matched by a quick lookup"
      - "When the join condition is `<>`, because merging handles non-matches efficiently"
      - "When neither input fits in memory and neither is sorted, because merging needs no sorting"
    explanation: |-
      A merge join walks two inputs sorted on the join key in lockstep, advancing whichever side has the smaller key and emitting matches (handling runs of duplicates on both sides). Its join work is linear in the input sizes, O(n + m) plus output, so it is ideal when sort order is already available: both tables read through indexes on the key, or the output of an earlier sort or merge.

      | Situation | Usually best |
      |---|---|
      | small outer, indexed inner key | index nested loop |
      | equality key, unsorted inputs, one side fits in memory | hash join |
      | both inputs sorted on the key | merge join |
      | inequality or arbitrary condition | nested loop (with index if possible) |

      "Tiny input with an index on the other" describes the index nested loop.

      "`<>`" is the worst case for merging: almost every pair matches.

      "Needs no sorting" is backwards: unsorted inputs must be sorted first, costing O(n log n) each, which is why merge join wins mainly when order is free.
  - id: "independence-assumption-error"
    type: "single"
    prompt: "`cars` has 1,000,000 rows. `make = 'Honda'` matches 10% of rows and `model = 'Civic'` matches 1%, and every Civic is a Honda. With per-column statistics only, what does an optimizer that assumes independence estimate for `make = 'Honda' AND model = 'Civic'`, and what is the actual count?"
    correct:
      - "Estimate about 1,000 rows; actual 10,000 rows"
    distractors:
      - "Estimate about 10,000 rows; actual 10,000 rows"
      - "Estimate about 110,000 rows; actual 10,000 rows"
      - "Estimate about 1,000 rows; actual 1,000 rows"
    explanation: |-
      Under independence, the selectivity of a conjunction is the product of the selectivities: 0.10 × 0.01 = 0.001, so the estimate is 1,000,000 × 0.001 = 1,000 rows. But `model = 'Civic'` implies `make = 'Honda'`, so the conjunction matches exactly the Civics: 1% of rows, 10,000.

      | Quantity | Value |
      |---|---|
      | sel(make) × sel(model) | 0.10 × 0.01 = 0.001 |
      | estimate | 1,000 |
      | actual | 10,000 (all Civics) |
      | error | 10× underestimate |

      PostgreSQL, with default statistics, estimates 957 rows for this exact data (checked with `EXPLAIN`).

      "10,000 / 10,000" assumes the optimizer knows the dependency; it does not without extended (multi-column) statistics.

      "110,000" adds selectivities, which is the formula for `OR` (minus the overlap), not `AND`.

      "1,000 / 1,000" assumes the data really is independent.

      Underestimates like this compound across joins and push the optimizer toward nested loops that then run far longer than planned.
    checks:
      - engine: "python"
        code: |
          assert round(1_000_000 * 0.10 * 0.01) == 1_000
      - engine: "postgres"
        setup: "CREATE TABLE cars(id INT, make TEXT, model TEXT); INSERT INTO cars SELECT g, CASE WHEN g % 10 = 0 THEN 'Honda' ELSE 'Other' || (g % 10) END, CASE WHEN g % 100 < 3 AND g % 10 = 0 THEN 'Civic' WHEN g % 10 = 0 THEN 'Accord' ELSE 'm' || (g % 37) END FROM generate_series(1, 1000000) g; ANALYZE cars;"
        query: "SELECT COUNT(*) FILTER (WHERE make = 'Honda'), COUNT(*) FILTER (WHERE model = 'Civic'), COUNT(*) FILTER (WHERE make = 'Honda' AND model = 'Civic') FROM cars"
        expect: [[100000, 10000, 10000]]
  - id: "hash-build-side"
    type: "single"
    prompt: "A hash join combines `sales` (50,000,000 rows) with `stores` (2,000 rows) on `store_id`. Which input should be the build side (the one loaded into the hash table)?"
    correct:
      - "`stores`, so the hash table fits in memory and `sales` streams through once as the probe side"
    distractors:
      - "`sales`, because building the hash table on the larger input makes each of the 2,000 probes cheaper"
      - "Either; hash join cost is the same whichever side is built"
      - "`sales`, because the build side must be the table with the foreign key"
    explanation: |-
      Hash join cost is roughly: read the build input and insert each row into a hash table, then read the probe input and look up each row. Both inputs are read once if the hash table fits in memory. If it does not, the engine partitions both inputs to disk and processes them in batches (Grace hash join), multiplying I/O.

      | Build side | Hash table size | Fits in memory? | Extra I/O |
      |---|---|---|---|
      | stores (2,000 rows) | tiny | yes | none |
      | sales (50 million rows) | huge | likely not | partitioning both inputs |

      So the optimizer builds on the smaller input, by its *estimate*, which is another reason cardinality errors hurt.

      "Probes are cheaper on a larger table" is backwards; probe cost per row is about constant either way.

      "Either side" ignores the memory limit.

      "Must be the foreign-key table" invents a rule; key roles do not determine sides.
  - id: "structure-workload-match"
    type: "multiple"
    prompt: "Which pairings of index or storage structure and workload are sound? Select all that apply."
    correct:
      - "LSM tree: write-heavy ingest, turning random writes into sequential flushes and background merges"
      - "Columnar storage: aggregating a few columns over many rows"
      - "HNSW graph index: approximate nearest-neighbor search over embedding vectors"
    distractors:
      - "Hash index: range predicates such as `created_at BETWEEN x AND y`"
      - "B+tree on an embedding column: finding the vectors closest by cosine distance"
    explanation: |-
      Each structure exploits a different invariant:

      - **LSM tree** (O'Neil et al., 1996): buffer writes in memory, flush sorted runs sequentially, merge in the background. Writes become cheap and sequential; reads may consult several runs, so point reads cost more than in a B+tree.
      - **Columnar storage**: store each column contiguously, so a query reading 3 of 50 columns reads about 3/50 of the data and compresses well; single-row updates and lookups of whole rows are the weak spot.
      - **HNSW** (Malkov and Yashunin): a layered proximity graph searched greedily, giving fast *approximate* nearest neighbors in high-dimensional spaces, trading exact recall for speed.

      Distractors:

      - A **hash index** supports only equality; hashing destroys order, so ranges need an ordered structure.
      - A **B+tree** orders by a one-dimensional key; "closest in 768 dimensions" is not a key range, so a B+tree cannot answer nearest-neighbor queries on embeddings beyond scanning.
---
## Learning objectives

- Describe nested-loop, hash and merge joins and the conditions each needs.
- Estimate join costs in page reads and pick the cheaper algorithm.
- Explain how the independence assumption produces cardinality errors.
- Match LSM trees, columnar storage and vector indexes to the problems they solve.

## The invariant

A join's *result* is defined by relational algebra; its *cost* depends on which algorithm computes it. Three families cover almost every plan:

| Algorithm | Works for | Cost (inputs R, S; output aside) | Wins when |
|---|---|---|---|
| Nested loop (naive) | any condition | \|R\| × \|S\| comparisons | inputs are tiny |
| Index nested loop | any condition an index on S can seek | \|R\| × (index depth + fetch) | R is small, S has an index on the join key |
| Hash join | equality | read R and S once if the build side fits in memory | large unsorted inputs with an equi-join |
| Merge join | equality (and some range conditions) | read both once if already sorted; plus sorting otherwise | both inputs arrive sorted on the key |

The optimizer chooses among them using **cardinality estimates**: how many rows each step will produce. Estimates come from statistics (row counts, distinct values, most common values, histograms) combined with assumptions. The most consequential is **independence**: the selectivity of `p AND q` is estimated as sel(p) × sel(q). Correlated columns (city and zip code, make and model) break it, and errors multiply through joins: Ioannidis and Christodoulakis (1991) showed that estimation errors can grow exponentially with the number of joins. A tenfold underestimate early in a plan can turn a hash join into a nested loop that runs millions of probes.

Other structures solve other problems. B+trees serve equality, ranges and order on one key; hash indexes serve equality only; LSM trees optimize write throughput; columnar layouts optimize wide scans of few columns; vector indexes answer approximate similarity queries. None replaces the others; each wins on a different invariant.

## Worked example

```sql
EXPLAIN
SELECT *
FROM cars
WHERE make = 'Honda' AND model = 'Civic';
-- PostgreSQL 16.15, default statistics: rows=957 estimated
-- actual rows: 10,000 (every Civic is a Honda)
```

| Step | Estimate | Actual |
|---|---|---|
| make = 'Honda' | 100,000 | 100,000 |
| model = 'Civic' | 10,000 | 10,000 |
| both (independence) | ≈ 1,000 | 10,000 |

If this result is joined to a 10,000,000-row `service_visits` table, the optimizer budgets about 1,000 index probes and might choose an index nested loop; the real plan performs ten times as many. PostgreSQL's `CREATE STATISTICS ... (dependencies, mcv) ON make, model FROM cars` gives the planner multi-column statistics that capture the dependency.

## Where this shows up today

As of November 2026, PostgreSQL 19 core provides B-tree, hash, GiST, SP-GiST, GIN and BRIN index types, while vector similarity indexing comes from extensions such as pgvector (0.8 series in 2026), which offers HNSW and IVFFlat. LSM-based storage engines and columnar formats are common beneath analytics and key-value systems. The mechanisms above, not product names, are what decide which one fits a workload.

## Pitfalls checklist

- Blaming "the optimizer" for a bad plan without comparing estimated and actual row counts.
- Filtering on correlated columns and expecting accurate estimates without multi-column statistics.
- Writing non-equi joins on large tables and expecting hash-join performance.
- Forcing a join order or algorithm with hints that becomes wrong as data grows.
- Using a B+tree or hash index for similarity search, or a vector index where exact answers are required.

## Key takeaways

- Index nested loops suit small outer inputs; hash joins suit large equi-joins; merge joins suit pre-sorted inputs.
- Hash joins need equality and build on the smaller input.
- Independence makes `AND` estimates multiply, which underestimates correlated predicates.
- B+tree, hash, LSM, columnar and vector structures each optimize a different access pattern.

## Sources

- L. D. Shapiro, "Join Processing in Database Systems with Large Main Memories", *ACM TODS* 11(3), 1986.
- Y. E. Ioannidis and S. Christodoulakis, "On the Propagation of Errors in the Size of Join Results", *ACM SIGMOD*, 1991.
- P. O'Neil, E. Cheng, D. Gawlick and E. O'Neil, "The Log-Structured Merge-Tree (LSM-Tree)", *Acta Informatica* 33(4), 1996.
- Y. A. Malkov and D. A. Yashunin, "Efficient and Robust Approximate Nearest Neighbor Search Using Hierarchical Navigable Small World Graphs", *IEEE TPAMI* 42(4), 2020.
- PostgreSQL 19 documentation, "Index Types" and "Extended Statistics"; pgvector project documentation.
