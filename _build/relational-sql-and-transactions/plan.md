# Plan — Relational Theory, SQL Semantics & Transaction Anomalies

Slug `relational-sql-and-transactions`, author `terlingua-team`, date 2026-11-27.
7 chapters, 30 sections (s = book-wide section number used by the answer-position rule).
Each section lists its question concepts; no concept repeats outside Chapter 7.

## 01-relational-foundations-and-normalization — Relational Foundations and Normalization

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 1 | 01-relations-sets-and-bags.md | Distinguish Codd's set relations from SQL's bag tables | UNION vs UNION ALL row count; EXCEPT vs EXCEPT ALL multiplicities; join of bags multiplies duplicates; unordered relations (no ORDER BY = no order); which SQL operators eliminate duplicates by default; projection without DISTINCT keeps duplicates |
| 2 | 02-keys-and-functional-dependencies.md | Derive keys from functional dependencies | attribute closure computation; candidate key identification; superkey vs candidate key minimality; an instance can refute but not prove an FD; FOREIGN KEY with NULL under MATCH SIMPLE; primary key vs other candidate keys |
| 3 | 03-normal-forms-and-anomalies.md | Classify 1NF–BCNF and name the anomaly each prevents | partial dependency → 2NF violation; transitive dependency → 3NF violation; update anomaly trace; insert anomaly; delete anomaly; 3NF-but-not-BCNF schema |
| 4 | 04-lossless-decomposition-and-denormalization.md | Test decompositions for losslessness and justify denormalization | spurious tuples from a lossy join (count); lossless-join test via common-attribute key; dependency preservation lost by BCNF; when denormalization is justified; JSON column as deliberate 1NF trade-off (dated) |

## 02-sql-logical-query-processing-order — SQL's Logical Query Processing Order

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 5 | 01-the-logical-clause-order.md | Recite and apply the logical order of clauses | clause order recall; DISTINCT + ORDER BY on non-selected column error; LIMIT applied after ORDER BY (result trace); OFFSET paging trace; SELECT expressions evaluated before ORDER BY; FROM-clause cross join row count |
| 6 | 02-aliases-and-clause-scope.md | Predict where a column alias is visible | alias in WHERE error (standard); alias in ORDER BY works; derived-table fix; SQLite alias-in-WHERE extension (dated); ORDER BY name resolves to output column vs GROUP BY to input column (PostgreSQL trace); ORDER BY expression on alias not allowed in PostgreSQL |
| 7 | 03-outer-joins-where-vs-on.md | Place filters correctly in outer joins | ON filter on right table keeps all left rows (count); WHERE filter on right table turns LEFT into inner (count); anti-join with IS NULL; ON filter on left table does not remove left rows; chained LEFT JOIN with inner join collapse; FULL OUTER JOIN row count |
| 8 | 04-exists-in-join-and-duplicates.md | Choose semi-join forms that do not multiply rows | JOIN vs IN row count with duplicate matches; SUM inflated by one-to-many join (fan trap); fix by pre-aggregating; EXISTS ignores select list; correlated subquery evaluation per row; DISTINCT as a band-aid hides the bug |
| 9 | 05-logical-vs-physical-order-and-ctes.md | Separate logical semantics from execution plans | no guaranteed short-circuit (division guard via CASE); predicate pushdown legal for inner join; pushdown into outer join's nullable side changes result; LIMIT without ORDER BY nondeterministic; CTE inlining rule (dated) |

## 03-three-valued-logic-and-null-semantics — Three-Valued Logic and NULL Semantics

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 10 | 01-truth-tables-and-unknown.md | Evaluate predicates under 3VL | AND/OR truth table evaluation; `WHERE x <> 5` drops NULL rows (count); `NULL = NULL` is UNKNOWN; CHECK constraint accepts UNKNOWN; NOT UNKNOWN; CASE WHEN with UNKNOWN goes to ELSE |
| 11 | 02-not-in-versus-not-exists.md | Avoid the NOT IN NULL trap | NOT IN with NULL in subquery returns zero rows; NOT EXISTS returns the expected rows; IN with NULL in list still finds matches; fix NOT IN with IS NOT NULL filter; NULL on outer side of NOT IN; `x NOT IN (empty)` is TRUE |
| 12 | 03-aggregates-on-empty-and-null-groups.md | Predict aggregate results on empty and NULL inputs | COUNT(*) vs COUNT(col); SUM over all-NULL group is NULL; scalar aggregate over empty table returns one row; GROUP BY over empty input returns zero rows; AVG ignores NULLs (computed); COALESCE(SUM, 0) |
| 13 | 04-is-distinct-from-unique-and-ordering.md | Compare NULL-safe and use NULL-aware constraints | IS DISTINCT FROM truth values; GROUP BY puts NULLs in one group; DISTINCT collapses NULLs; UNIQUE allows multiple NULLs in standard semantics; NULLS NOT DISTINCT / SQL Server single NULL (dated); NULL sort position differs by engine |

## 04-grouping-aggregation-and-window-functions — Grouping, Aggregation and Window Functions

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 14 | 01-group-by-and-functional-dependence.md | Write valid grouped queries | non-grouped column error; functional dependence on grouped primary key allows column; HAVING vs WHERE result trace; WHERE cannot reference aggregates; HAVING without GROUP BY; MySQL ONLY_FULL_GROUP_BY (dated) |
| 15 | 02-window-frames-rows-range-groups.md | Choose frames deliberately | default frame RANGE ... CURRENT ROW includes peers (trace); ROWS frame running sum; GROUPS frame trace; LAST_VALUE default-frame trap; no ORDER BY → whole partition; engine support for GROUPS (dated) |
| 16 | 03-ranking-functions-and-top-n.md | Rank rows and select top N per group | ROW_NUMBER/RANK/DENSE_RANK values with ties; top-2 per group with ties; window function in WHERE error; ROW_NUMBER nondeterminism with ties; RANK gap size; top-N with ROW_NUMBER vs RANK count |
| 17 | 04-running-totals-and-gaps-and-islands.md | Solve sequential-row problems with windows | partitioned running total trace; LAG difference trace; LAG default NULL on first row; gaps-and-islands grouping key; island count; consecutive-date islands |

## 05-b-plus-tree-indexes-selectivity-and-joins — B+tree Indexes, Selectivity and Join Algorithms

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 18 | 01-b-plus-tree-anatomy-and-depth.md | Estimate B+tree height and lookup cost | depth from fanout and row count; fanout from page and entry size; range scan via linked leaves; why keys only in leaves of B+tree; pages read for point lookup; growth of height when rows ×100 |
| 19 | 02-composite-indexes-and-leftmost-prefix.md | Order composite-index columns | leftmost-prefix usability; equality-before-range rule; ORDER BY satisfied by index; skip of middle column; which index serves a query set; skip-scan (dated) |
| 20 | 03-sargability.md | Write index-friendly predicates | function on indexed column; leading wildcard LIKE; implicit type conversion; rewrite of date function to range; arithmetic on column; expression index fix |
| 21 | 04-covering-indexes-and-selectivity.md | Decide when an index beats a scan | covering index for query; selectivity computation; crossover arithmetic random vs sequential; correlated column clustering effect; INCLUDE columns; index-only scan visibility map (dated) |
| 22 | 05-join-algorithms-and-cardinality-estimation.md | Pick join algorithms and diagnose estimates | nested loop cost with index; hash join needs equality; merge join on sorted inputs; independence assumption estimate error (compute); hash join memory build side; LSM vs B-tree vs columnar vs vector index purposes |

## 06-transaction-anomalies-and-isolation-levels — Transaction Anomalies and Isolation Levels

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 23 | 01-dirty-write-to-phantom.md | Identify P0–P3 in schedules | dirty write (P0) schedule; dirty read (P1) schedule; non-repeatable read (P2) schedule; phantom (P3) schedule; broad vs strict interpretation; why P0 forbidden at every level |
| 24 | 02-lost-update-read-skew-write-skew.md | Identify P4, A5A, A5B | lost update trace (final value); read skew trace (observed sum); write skew trace (doctors on call); atomic UPDATE avoids lost update; write skew needs disjoint writes; constraint that write skew breaks |
| 25 | 03-ansi-levels-and-snapshot-isolation.md | Map ANSI levels and SI to anomalies | ANSI table recall; Berenson critique (phenomena vs anomalies); SI prevents read skew; SI first-committer-wins; SI permits write skew; SI is incomparable to ANSI REPEATABLE READ |
| 26 | 04-two-phase-locking-and-deadlocks.md | Reason about 2PL, FOR UPDATE, gap locks, deadlocks | 2PL growing/shrinking; strict 2PL holds until commit; SELECT FOR UPDATE prevents lost update; next-key lock blocks insert in gap; deadlock cycle detection in wait-for graph; lock ordering prevents deadlock |
| 27 | 05-ssi-retries-and-engine-defaults.md | Use SERIALIZABLE safely across engines | SSI dangerous structure; PostgreSQL SERIALIZABLE aborts write skew (trace); retry loop must rerun whole transaction; read-only transaction can still observe anomaly under SI; engine default isolation levels (dated); SQLSTATE 40001 |

## 07-cross-chapter-review-and-debugging — Cross-Chapter Review and Debugging

| s | File | Learning goal | Question concepts |
|---|------|---------------|-------------------|
| 28 | 01-query-output-puzzles.md | Combine join, NULL and aggregation rules | LEFT JOIN + COUNT(col) per group; NOT IN + outer join; window over grouped result; DISTINCT + NULL + UNION; HAVING with COUNT on outer join; logical order with alias and NULL |
| 29 | 02-index-selection-puzzles.md | Combine sargability, prefix and selectivity | choose composite index for workload; sargable rewrite + prefix; covering vs selectivity; join algorithm with index; OR predicate across columns; NULL and index for IS NULL |
| 30 | 03-schedule-anomaly-puzzles.md | Combine anomaly definitions with engine behavior | classify schedule under SI; FOR UPDATE + write skew; deadlock + retry; aggregate check under RC; phantom under locking RR vs SI |
