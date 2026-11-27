# REPORT — relational-sql-and-transactions

Generated 2026-11-27 by an unattended run. See NOTES.md for rule sources and decisions.

## Summary

| Item | Count |
|---|---|
| Chapters | 7 |
| Sections | 30 |
| Questions | 180 (149 single, 31 multiple) |
| Prose words (excluding code, tables, Sources) | 14,757 |
| Executable check entries (check_sql.py) | 205: 106 SQLite runs, 141 PostgreSQL runs, 30 two-session schedules, 18 Python arithmetic checks |

Files delivered: `content/books/relational-sql-and-transactions/book.yml`, 7 `chapter.yml`, 30 section `.md` files.
Build sources and tools: `_build/relational-sql-and-transactions/` (src/, build.py, validate.py, check_sql.py, balance.py, combos.py, addopt.py, plan.md, progress.md, NOTES.md, this report, and the captured outputs `validate_output.txt` and `check_sql_output.txt`).

To regenerate the section files after editing a source: `python3 _build/relational-sql-and-transactions/build.py`, then `validate.py` and `check_sql.py` (the latter needs a PostgreSQL server on socket `/tmp/pg`, port 5433, user `postgres`; edit `PG` at the top of the script).

## Per-section question counts

| s | File | Questions | Multiple |
|---|---|---|---|
| 1 | 01/01-relations-sets-and-bags.md | 6 | 1 |
| 2 | 01/02-keys-and-functional-dependencies.md | 6 | 1 |
| 3 | 01/03-normal-forms-and-anomalies.md | 6 | 1 |
| 4 | 01/04-lossless-decomposition-and-denormalization.md | 6 | 1 |
| 5 | 02/01-the-logical-clause-order.md | 6 | 1 |
| 6 | 02/02-aliases-and-clause-scope.md | 6 | 1 |
| 7 | 02/03-outer-joins-where-vs-on.md | 6 | 1 |
| 8 | 02/04-exists-in-join-and-duplicates.md | 6 | 1 |
| 9 | 02/05-logical-vs-physical-order-and-ctes.md | 6 | 1 |
| 10 | 03/01-truth-tables-and-unknown.md | 6 | 1 |
| 11 | 03/02-not-in-versus-not-exists.md | 6 | 1 |
| 12 | 03/03-aggregates-on-empty-and-null-groups.md | 6 | 1 |
| 13 | 03/04-is-distinct-from-unique-and-ordering.md | 6 | 1 |
| 14 | 04/01-group-by-and-functional-dependence.md | 6 | 0 |
| 15 | 04/02-window-frames-rows-range-groups.md | 6 | 1 |
| 16 | 04/03-ranking-functions-and-top-n.md | 6 | 1 |
| 17 | 04/04-running-totals-and-gaps-and-islands.md | 6 | 1 |
| 18 | 05/01-b-plus-tree-anatomy-and-depth.md | 6 | 0 |
| 19 | 05/02-composite-indexes-and-leftmost-prefix.md | 6 | 2 |
| 20 | 05/03-sargability.md | 6 | 1 |
| 21 | 05/04-covering-indexes-and-selectivity.md | 6 | 1 |
| 22 | 05/05-join-algorithms-and-cardinality-estimation.md | 6 | 2 |
| 23 | 06/01-dirty-write-to-phantom.md | 6 | 1 |
| 24 | 06/02-lost-update-read-skew-write-skew.md | 6 | 2 |
| 25 | 06/03-ansi-levels-and-snapshot-isolation.md | 6 | 2 |
| 26 | 06/04-two-phase-locking-and-deadlocks.md | 6 | 1 |
| 27 | 06/05-ssi-retries-and-engine-defaults.md | 6 | 1 |
| 28 | 07/01-query-output-puzzles.md | 6 | 0 |
| 29 | 07/02-index-selection-puzzles.md | 6 | 1 |
| 30 | 07/03-schedule-anomaly-puzzles.md | 6 | 1 |

## Validation output (validate.py)

```
WARNING: 01-relational-foundations-and-normalization/03-normal-forms-and-anomalies.md: 'Where this shows up today' has no 'As of November 2026' claim
stats: {'chapters': 7, 'sections': 30, 'questions': 180, 'type_single': 149, 'type_multiple': 31, 'longest_correct': 59, 'prose_words': 14757}
correct-answer positions (book): {0: 44, 1: 52, 2: 52, 3: 57, 4: 16}
0 error(s), 1 warning(s)
```

The one warning is deliberate: section 3 (normal forms) has no version-specific claim, so its "Where this shows up today" block makes none.

`validate.py` checks, for every file: naming regexes from `ContentNaming.cs`; `book.yml` and `chapter.yml` keys, quoting, author profile, date not in the future; front matter split exactly as `FrontMatterParser` does (`\n---`); no unknown keys (YamlDotNet would silently ignore them); question keys, id slug and reserved words, `type` rules, 4–5 options (5 for multiple, 2–3 correct), `correct: true` only on right answers, "Select all that apply." placement, no references to options by letter or position, no all/none of the above; every string scalar double-quoted, no tabs, no `---` lines in front matter; body heading order, bullet counts, code-fence languages, prose word count 400–1200, the size of the "today" block, and the answer-position rule ((k + s) mod n, wrapping) plus the 40% per-position cap.

## Execution audit (check_sql.py)

```
checks: 205 entries; runs: {'sqlite': 106, 'postgres': 141, 'schedule': 30, 'python': 18}
all checks passed
```

Every SQL result, row count, truth table, plan property and schedule outcome used in a question or worked example has a `checks:` or `body_checks:` entry in its source file and was executed:

- Portable queries on SQLite 3.53.1 (Python `sqlite3`) and PostgreSQL 16.15; most run on both.
- PostgreSQL-only semantics (EXCEPT ALL, FETCH WITH TIES, NULLS NOT DISTINCT, IS UNKNOWN, error SQLSTATEs, EXPLAIN plan shapes, extended statistics) on PostgreSQL 16.15.
- Isolation behavior (non-repeatable reads, phantoms, read skew, write skew under READ COMMITTED / REPEATABLE READ / SERIALIZABLE, first-updater-wins errors, FOR UPDATE re-checks, deadlock detection and victim selection, unique-constraint waits) as interleaved two-session schedules with threads, so blocking is observed rather than assumed.
- B+tree depth/fanout, selectivity, break-even and join-cost arithmetic as Python assertions.

## Question audit

All 180 questions were re-read after building. Fixes made during the audit:

- Window-frame distractors in section 15 whose values matched no real frame were replaced by values produced by actual frames (re-verified in SQLite).
- The "top ten in a derived table" answer (section 9) was narrowed: ties at the cutoff make the selected rows non-deterministic.
- Distractor lengths were rebalanced in seven sections; the correct option is now strictly the longest in 59 of 180 questions (33%), with no section above 3 of 6.
- Answer positions were balanced by adding a fifth, misconception-based distractor where the position rule would otherwise exceed 40% in a section.

## Unresolved items and uncertainties

1. **Real compiler not run.** The sandbox has no .NET SDK and cannot reach dot.net or nuget.org, so `dotnet run --project tools/ObserverMagazine.ContentProcessor` was not executed. Validation used a port of the compiler's rules (see NOTES.md). Please run the content processor once before merging. The main residual risk is YamlDotNet's handling of `explanation: |-` block scalars, which no existing book uses; they are standard YAML 1.1/1.2 and PyYAML parses them, but this is the first use in the repository.
2. **PostgreSQL version.** Behavior was executed on PostgreSQL 16.15, while dated text cites PostgreSQL 19 documentation (released 2026-11-19). Every PostgreSQL 19 claim (default isolation level, isolation table, CTE folding, NULLS NOT DISTINCT, planner cost defaults) was checked against the current (19) online documentation; the PostgreSQL 19 documentation pages read during this run (Transaction Isolation, WITH Queries, CREATE TABLE, Query Planning) describe the same behavior that was executed on 16.15. Other PostgreSQL features used (EXCEPT ALL, FETCH ... WITH TIES) are long-standing and were executed on 16.15 only; skip scan (PostgreSQL 18 and later) was not executed and is stated from the release notes.
3. **MySQL and SQL Server were not executed.** Their claims (InnoDB default level, next-key/gap locks, SERIALIZABLE SELECT conversion, ONLY_FULL_GROUP_BY default, GROUPS unsupported, string-to-number index rule, EXCEPT/INTERSECT ALL, no FULL JOIN; SQL Server default READ COMMITTED with RCSI off on-premises and on in Azure SQL Database, single NULL in UNIQUE, distinct-only EXCEPT/INTERSECT) are stated only as documented in the MySQL 8.4 Reference Manual and Microsoft Learn pages read during this run.
4. **Distributed SQL claims** (CockroachDB v26.2 and Spanner default to serializable) come from their official documentation pages found by search; named only as examples.
5. **Primary-source details not confirmed to page level.** Citations are document-level only (Codd 1970; Berenson et al. 1995; Fekete et al. 2004/2005; Cahill et al. 2008; Comer 1979; Selinger et al. 1979; and others). No quotations from papers are used. The attribution of `GROUPS` frames to SQL:2011 and of `LAG`/`LEAD` to SQL:2011 follows common references (and PostgreSQL 11 release notes for the former) rather than the ISO text itself, which was not available.
6. **pgvector** is described as "0.8 series in 2026" based on release listings found by search; the exact latest patch version was not pinned.
7. **Dated-question limit** (at most one per section) was audited by hand; validate.py cannot detect it mechanically. Sections whose questions are phrased "In PostgreSQL ..." but test long-standing mechanisms (statement snapshots, SSI) were treated as mechanism questions.
8. **House style addition:** each section ends with a `## Sources` heading after the six required headings (matching existing books such as timeless-dotnet); remove it if the six-heading structure must be exact.
