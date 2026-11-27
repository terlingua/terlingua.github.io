---
title: "Schedule and Anomaly Puzzles"
questions:
  - id: "classify-and-predict-si"
    type: "single"
    prompt: "`x` and `y` are different rows, and the application requires `x + y >= 0`. Both start at 50. T1 reads both and withdraws 80 from `x`; T2 reads both and withdraws 80 from `y`:\n\n`r1[x] r1[y] r2[x] r2[y] w1[x] w2[y] c1 c2`\n\nWhich anomaly is this, and what happens under snapshot isolation?"
    explanation: |-
      Each transaction checks `x + y = 100 ≥ 80` against its snapshot and writes a *different* row: T1 writes x, T2 writes y. That is A5B write skew (Chapter 6): overlapping reads, disjoint writes.

      | step | T1 | T2 | committed x, y |
      |---|---|---|---|
      | reads | x = 50, y = 50 | x = 50, y = 50 | 50, 50 |
      | writes | x = -30 | y = -30 | |
      | c1, c2 | commit | commit | -30, -30 |

      Under snapshot isolation only write-write overlaps abort, and the write sets {x} and {y} are disjoint, so both commit: x + y = -60.

      "Lost update" requires both to write the same item.

      "Aborts T2 because both touched x and y" confuses reading with writing; reads do not count for first-committer-wins.

      "Read skew" requires a transaction to see another's committed change; under SI neither sees the other.

      Under SERIALIZABLE (SSI, or strict 2PL), one of the two would fail and retry.
    options:
      - text: "Lost update; first committer wins aborts T2"
      - text: "Write skew; first committer wins aborts T2 because both touched `x` and `y`"
      - text: "Read skew; T2 sees `x` after T1's write and refuses the withdrawal"
      - text: "Write skew; both transactions commit and leave `x + y = -60`"
        correct: true
  - id: "for-update-fixes-on-call"
    type: "single"
    prompt: "At PostgreSQL `READ COMMITTED`, both doctors' transactions now run `SELECT name FROM doctors WHERE on_call ORDER BY name FOR UPDATE` before deciding. T1 gets `alice, bob`, sets alice off call and commits while T2's `SELECT ... FOR UPDATE` is waiting. What does T2's query return?"
    explanation: |-
      This combines locking reads (Chapter 6, two-phase locking) with READ COMMITTED semantics. T1's `FOR UPDATE` locks both on-call rows. T2's locking read waits on them. When T1 commits, PostgreSQL re-checks each locked row's newest version against T2's `WHERE on_call`: alice's new version is off call and drops out; bob still qualifies.

      | step | T1 | T2 |
      |---|---|---|
      | 1 | FOR UPDATE → alice, bob (locked) | |
      | 2 | | FOR UPDATE: waits |
      | 3 | alice off; COMMIT | |
      | 4 | | returns bob |

      T2 counts one doctor and declines, so the invariant holds (verified by running the schedule). The locking read turned the write skew into a serialized decision, because both transactions lock the rows their decision depends on.

      "As they were when T2 started" would be a snapshot read, which plain `SELECT` does, not `FOR UPDATE`.

      "No rows" misreads locking as hiding rows.

      "40001" is REPEATABLE READ's reaction to a concurrently updated locked row, not READ COMMITTED's.
    options:
      - text: "Only `bob`, so T2 sees one doctor on call and can refuse to go off call"
        correct: true
      - text: "`alice, bob`, the rows as they were when T2 started waiting"
      - text: "No rows, because the rows were locked when T2 started"
      - text: "An error 40001, because the rows changed while T2 waited"
  - id: "deadlock-then-retry"
    type: "single"
    prompt: "Accounts 1 and 2 hold 100 each. T1 transfers 10 from 1 to 2; T2 transfers 20 from 2 to 1; each updates its source row first. They deadlock, PostgreSQL aborts T1 (40P01), T2 commits, and the application then retries T1 in full. What are the final balances of accounts 1 and 2?"
    explanation: |-
      Deadlock handling (Chapter 6) plus retry semantics: the victim's work is rolled back entirely, the survivor commits, and the retried transaction runs again on the new state.

      | event | account 1 | account 2 |
      |---|---|---|
      | start | 100 | 100 |
      | T1 aborted (its −10 on account 1 undone) | 100 | 100 |
      | T2 commits: 2 → 1, 20 | 120 | 80 |
      | T1 retried and committed: 1 → 2, 10 | 110 | 90 |

      Both transfers applied exactly once, as in a serial execution (T2 then T1), which is the point of abort-and-retry (verified by running the schedule).

      "120 and 80" forgets the retry.

      "90 and 110" applies T1 and forgets T2.

      "100 and 100" assumes the deadlock aborts both.
    options:
      - text: "120 and 80"
      - text: "110 and 90"
        correct: true
      - text: "90 and 110"
      - text: "100 and 100"
  - id: "single-statement-sum"
    type: "single"
    prompt: "Accounts `x` and `y` always sum to 100; transfers between them run constantly. A report runs a *single* statement, `SELECT SUM(bal) FROM acct`, at PostgreSQL `READ COMMITTED`. Which totals can it return?"
    explanation: |-
      Read skew (Chapter 6) needs two reads from different states. In PostgreSQL's READ COMMITTED, the unit of consistency is the *statement*: one snapshot is taken when the statement starts and every row it reads comes from that snapshot. A single `SUM` therefore sees each transfer either completely or not at all: 100.

      Split the report into two statements (`SELECT bal ... 'x'`, then `SELECT bal ... 'y'`) and read skew becomes possible: 90 or 110 if a transfer commits between them.

      | report shape | READ COMMITTED (PostgreSQL) | REPEATABLE READ |
      |---|---|---|
      | one statement | 100 | 100 |
      | two statements | 90, 100 or 110 | 100 |

      Caveat: this relies on statement-level snapshots. A purely lock-based READ COMMITTED (for example SQL Server with `READ_COMMITTED_SNAPSHOT` off) releases read locks row by row, so even one statement can see a transfer half-applied.

      "An index on `bal`" is irrelevant to visibility.

      "Uncommitted data" is excluded at READ COMMITTED.
    options:
      - text: "100, 90 or 110, because READ COMMITTED allows read skew"
      - text: "Only 100, but only if the table has an index on `bal`"
      - text: "Only 100, because each statement reads from one snapshot"
        correct: true
      - text: "Any value, because aggregates read uncommitted data"
  - id: "update-under-snapshot"
    type: "single"
    prompt: "In PostgreSQL, `staff` has ann and bob in `ops`. T1 (`REPEATABLE READ`) counts `ops` staff (2). T2 inserts `('eve', 'ops')` and commits. T1 runs `UPDATE staff SET bonus = 1 WHERE dept = 'ops'` and commits. Who has a bonus? And at `READ COMMITTED`?"
    explanation: |-
      Phantoms (Chapter 6) meet write semantics. At REPEATABLE READ, PostgreSQL finds the rows to update using T1's snapshot, which predates eve's insert, so the `UPDATE` touches ann and bob only. No row that T1 modifies was changed concurrently, so there is no serialization error. At READ COMMITTED, the `UPDATE` statement takes a fresh snapshot and also sees eve.

      | level | rows matched by T1's UPDATE | bonus holders |
      |---|---|---|
      | REPEATABLE READ | ann, bob (snapshot) | ann, bob |
      | READ COMMITTED | ann, bob, eve | ann, bob, eve |

      Both outcomes were verified by running the schedules. Whether eve *should* get the bonus is a business question; the point is that the level silently decides it.

      "Fails with 40001" would require T1 to update a row another transaction changed after T1's snapshot; eve's row is new and invisible to T1, not concurrently updated.

      "Ann and bob at READ COMMITTED" applies the snapshot rule to the wrong level.
    options:
      - text: "REPEATABLE READ: ann, bob and eve; READ COMMITTED: ann, bob and eve"
      - text: "REPEATABLE READ: T1's UPDATE fails with 40001; READ COMMITTED: ann, bob and eve"
      - text: "REPEATABLE READ: ann and bob; READ COMMITTED: ann and bob"
      - text: "REPEATABLE READ: ann and bob; READ COMMITTED: ann, bob and eve"
        correct: true
  - id: "making-on-call-safe"
    type: "multiple"
    prompt: "Which changes reliably prevent the \"at least one doctor on call\" violation in PostgreSQL? Select all that apply."
    explanation: |-
      Each correct option removes the condition write skew needs (Chapter 6):

      - **SERIALIZABLE (SSI):** detects the rw-antidependency cycle and aborts one transaction; the retry sees the other's commit.
      - **Locking read at READ COMMITTED:** both transactions lock the rows their decision reads, so the second waits and re-reads current data (the second puzzle in this section).
      - **Materialized conflict:** both transactions write the same guard row, so first-updater-wins aborts one at REPEATABLE READ; its retry sees the new roster.

      Distractors:

      - REPEATABLE READ alone is snapshot isolation, which permits write skew: both commit and nobody is on call.
      - An index changes speed, not the interleaving; the race window shrinks but does not close.

      All three working options can produce errors or waits, so the application must be written to retry; that is the price of correctness under concurrency.
    options:
      - text: "Run the transactions at REPEATABLE READ with no other change"
      - text: "Run the transactions at SERIALIZABLE and retry on SQLSTATE 40001"
        correct: true
      - text: "At READ COMMITTED, read the on-call rows with `SELECT ... FOR UPDATE` before deciding"
        correct: true
      - text: "Have every roster change also update a shared guard row for the ward, at REPEATABLE READ with retries"
        correct: true
      - text: "Add an index on `on_call` so the count is computed faster"
---
## Learning objectives

- Classify interleaved schedules by anomaly and predict each engine level's outcome.
- Combine locking reads, snapshots and retries to protect multi-row invariants.
- Predict the effect of deadlock victims and retries on final state.
- Distinguish statement-level from transaction-level consistency.

## The invariant

Every concurrency puzzle reduces to three questions, asked in order:

1. **What does each transaction read, and from which state?** Statement snapshots (READ COMMITTED in MVCC engines), transaction snapshots (snapshot isolation), or the latest data under locks (locking reads, 2PL).
2. **What does each transaction write, and do write sets overlap?** Overlapping writes block (locks) or abort (first committer wins); disjoint writes go through unless something tracks read-write dependencies.
3. **What does the isolation mechanism check before commit?** Nothing (READ COMMITTED), write-write conflicts (SI), or dependency cycles (SSI, strict 2PL).

| Mechanism | Prevents | Leaves open |
|---|---|---|
| statement snapshot (READ COMMITTED) | dirty reads, inconsistency within one statement | non-repeatable reads, phantoms, lost updates, read and write skew |
| transaction snapshot + first committer wins (SI) | the above plus lost update, read skew, phantoms in reads | write skew |
| SSI or strict 2PL with range locks | every anomaly (serializable) | nothing, at the cost of aborts or waits |
| locking reads on the rows a decision depends on | write skew over those rows | anything not locked (for example rows not yet inserted) |

From Berenson et al. (1995) to Cahill, Röhm and Fekete (2008), the theory says the same thing: correctness comes from what the mechanism checks, not from the level's name.

## Worked example

A booking service must not double-book a room. Three designs at PostgreSQL defaults:

| design | concurrent requests for room 5 at 10:00 | result |
|---|---|---|
| READ COMMITTED: `SELECT COUNT(*)`, then `INSERT` | both counts 0, both insert | double booking |
| `UNIQUE (room, slot)` constraint | second insert waits, then fails with 23505 | one booking |
| SERIALIZABLE: count, then insert, retry on 40001 | second commit fails, retry sees the booking | one booking |

The constraint is the best answer when the rule fits a key. When it does not (overlapping time *ranges*, "at least one doctor"), use SERIALIZABLE with retries, locking reads on the rows that matter, or a materialized conflict row. PostgreSQL also offers exclusion constraints for range overlaps, which turn that rule back into an index-enforced constraint.

## Where this shows up today

As of November 2026, PostgreSQL 19 (default READ COMMITTED), MySQL 8.4 InnoDB (default REPEATABLE READ) and SQL Server 2025 (default lock-based READ COMMITTED) all leave write skew to the application unless SERIALIZABLE or explicit locking is used, while distributed SQL systems such as CockroachDB and Spanner default to serializable isolation. Retry logic is therefore a design requirement whenever correctness depends on multi-row invariants.

## Pitfalls checklist

- Trusting a "check, then write" sequence without locks, constraints or SERIALIZABLE.
- Retrying only part of an aborted transaction.
- Assuming READ COMMITTED reports are inconsistent within one statement (in MVCC engines they are not) or consistent across statements (they are not).
- Forgetting that a snapshot-level `UPDATE` will not touch rows inserted after the snapshot.
- Choosing an isolation level by name across engines.

## Key takeaways

- Ask what is read, from which state; what is written, and whether write sets overlap; and what is checked at commit.
- Snapshot isolation stops lost updates and read skew but not write skew.
- Locking reads, materialized conflicts, constraints or SERIALIZABLE close write skew.
- Every correct concurrent design needs retries for aborts and deadlocks.

## Sources

- H. Berenson et al., "A Critique of ANSI SQL Isolation Levels", *ACM SIGMOD*, 1995.
- A. Fekete et al., "Making Snapshot Isolation Serializable", *ACM TODS* 30(2), 2005.
- M. J. Cahill, U. Röhm and A. D. Fekete, "Serializable Isolation for Snapshot Databases", *ACM SIGMOD*, 2008.
- PostgreSQL 19 documentation, "Transaction Isolation", "Explicit Locking" and "Exclusion Constraints".
- Chapter 6 of this book.
