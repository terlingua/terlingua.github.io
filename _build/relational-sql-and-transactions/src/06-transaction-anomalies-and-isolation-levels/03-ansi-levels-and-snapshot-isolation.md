---
title: "ANSI Isolation Levels, Their Critique, and Snapshot Isolation"
questions:
  - id: "ansi-repeatable-read-permits"
    type: "single"
    prompt: "According to the ANSI SQL-92 isolation table, which phenomenon is still possible at REPEATABLE READ?"
    correct:
      - "Phantom (P3) only"
    distractors:
      - "Non-repeatable read (P2) and phantom (P3)"
      - "None; REPEATABLE READ is serializable"
      - "Dirty read (P1) only"
    explanation: |-
      SQL-92 defines its four levels purely by which of three phenomena they forbid:

      | Level | P1 dirty read | P2 non-repeatable read | P3 phantom |
      |---|---|---|---|
      | READ UNCOMMITTED | possible | possible | possible |
      | READ COMMITTED | not possible | possible | possible |
      | REPEATABLE READ | not possible | not possible | possible |
      | SERIALIZABLE | not possible | not possible | not possible |

      REPEATABLE READ forbids P1 and P2, leaving P3: rows already read stay unchanged, but new rows matching a predicate may appear.

      "P2 and P3" describes READ COMMITTED.

      "None" confuses REPEATABLE READ with SERIALIZABLE.

      "P1 only" inverts the hierarchy; every level from READ COMMITTED up forbids dirty reads.
  - id: "berenson-criticisms"
    type: "multiple"
    prompt: "Which of these are criticisms that Berenson et al. (1995) make of the ANSI SQL-92 isolation definitions? Select all that apply."
    correct:
      - "The English definitions of the phenomena are ambiguous and admit both a broad and a strict reading"
      - "The definitions fail to characterize snapshot isolation, which avoids the ANSI phenomena under some readings yet is not serializable"
    distractors:
      - "The definitions mandate a lock-based implementation, which excludes multiversion engines from claiming any of the levels"
      - "READ UNCOMMITTED should permit dirty writes to maximize throughput"
      - "Phantoms cannot occur in practice, so REPEATABLE READ is effectively serializable"
    explanation: |-
      The paper's core points:

      - **Ambiguity:** each ANSI phenomenon can be read strictly (an anomaly that actually completes, A1–A3) or broadly (a pattern that could lead to one, P1–P3). The strict readings are too weak, so the paper argues for the broad ones.
      - **Missing phenomena and levels:** the table omits P0 (dirty write), and it cannot describe snapshot isolation, which prevents the strict anomalies A1–A3 yet permits write skew and is therefore not serializable. The authors add P4 (lost update), A5A (read skew) and A5B (write skew) to separate the levels.

      Distractors:

      - The ANSI text was meant to be implementation-neutral; the critique is that its phenomena *fail* to match the lock-based levels exactly, not that they mandate locks.
      - The paper argues the opposite about dirty writes: P0 must be forbidden at every level.
      - Phantoms are real, and the paper keeps P3 to distinguish REPEATABLE READ from SERIALIZABLE.
  - id: "si-prevents-read-skew"
    type: "single"
    prompt: "Repeat the read-skew schedule in PostgreSQL at `REPEATABLE READ`: `x = 50`, `y = 50`; T1 reads `x`; T2 moves 10 from `x` to `y` and commits; T1 reads `y`. What total does T1 observe?"
    correct:
      - "100"
    distractors:
      - "110"
      - "90"
      - "T1's read of `y` fails with a serialization error"
    explanation: |-
      Snapshot isolation (Berenson et al.) gives each transaction a snapshot of the data as of its start (in PostgreSQL, as of its first statement); all its reads see that snapshot and its own writes, never later commits.

      | step | T1 (snapshot taken at step 1) | T2 | T1 sees |
      |---|---|---|---|
      | 1 | read x | | 50 |
      | 2 | | x = 40, y = 60, commit | |
      | 3 | read y | | 50 (snapshot value) |

      Total 100: T1's reads are mutually consistent, so read skew (A5A) cannot happen (verified by running the schedule).

      "110" is the READ COMMITTED result, where each statement takes a new snapshot.

      "90" pairs the new x with the old y.

      "A serialization error" is not raised for reads; snapshot isolation raises errors only on conflicting writes.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE acct(id TEXT PRIMARY KEY, bal INT); INSERT INTO acct VALUES ('x', 50), ('y', 50);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [1, "SELECT bal FROM acct WHERE id = 'x'"]
          - [2, "BEGIN"]
          - [2, "UPDATE acct SET bal = bal - 10 WHERE id = 'x'"]
          - [2, "UPDATE acct SET bal = bal + 10 WHERE id = 'y'"]
          - [2, "COMMIT"]
          - [1, "SELECT bal FROM acct WHERE id = 'y'"]
          - [1, "COMMIT"]
        expect: ["ok", [[50]], "ok", "ok", "ok", "ok", [[50]], "ok"]
  - id: "first-committer-wins"
    type: "single"
    prompt: "In PostgreSQL, T1 and T2 both run at `REPEATABLE READ` and have taken their snapshots. T1 runs `UPDATE acct SET bal = bal + 5 WHERE id = 1` (uncommitted). T2 runs the same `UPDATE` on `id = 1`. Then T1 commits. What happens to T2?"
    correct:
      - "Its `UPDATE` waits for T1, then fails with SQLSTATE 40001 (`could not serialize access due to concurrent update`)"
    distractors:
      - "Its `UPDATE` waits for T1, then applies on top of T1's committed value"
      - "Its `UPDATE` succeeds immediately on its own snapshot version, and the later commit overwrites T1"
      - "Its `UPDATE` is silently skipped, because the row changed after its snapshot"
    explanation: |-
      Snapshot isolation forbids two concurrent transactions from both committing writes to the same item ("first committer wins"). PostgreSQL implements it as first *updater* wins: the second writer waits for the first's row lock; if the first commits, the second gets a serialization failure, because the row version it would update is not the one in its snapshot.

      | step | T1 | T2 |
      |---|---|---|
      | 1 | UPDATE id 1 (locks row) | |
      | 2 | | UPDATE id 1: waits |
      | 3 | COMMIT | ERROR 40001 |

      This is what rules out the lost update (P4) under snapshot isolation (verified by running the schedule).

      "Applies on top of T1's value" is READ COMMITTED behavior, where the waiting `UPDATE` re-evaluates against the new version.

      "Succeeds immediately ... overwrites T1" would be a lost update, which snapshot isolation exists to prevent.

      "Silently skipped" would discard a write without telling the application; engines raise an error so the application can retry.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE acct(id INT PRIMARY KEY, bal INT); INSERT INTO acct VALUES (1, 100);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [2, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [1, "SELECT bal FROM acct WHERE id = 1"]
          - [2, "SELECT bal FROM acct WHERE id = 1"]
          - [1, "UPDATE acct SET bal = bal + 5 WHERE id = 1"]
          - [2, "UPDATE acct SET bal = bal + 5 WHERE id = 1"]
          - [1, "COMMIT"]
          - [2, "ROLLBACK"]
        expect: ["ok", "ok", [[100]], [[100]], "ok", "blocked->err:40001", "ok", "ok"]
        final:
          query: "SELECT bal FROM acct WHERE id = 1"
          expect: [[105]]
  - id: "si-versus-locking-rr"
    type: "multiple"
    prompt: "Compare snapshot isolation with lock-based REPEATABLE READ (long read and write locks on items, no predicate locks), as analyzed by Berenson et al. Which statements are true? Select all that apply."
    correct:
      - "Lock-based REPEATABLE READ can exhibit phantoms that snapshot isolation's reads do not"
      - "Snapshot isolation can exhibit write skew that lock-based REPEATABLE READ prevents"
    distractors:
      - "Snapshot isolation is strictly stronger than lock-based REPEATABLE READ"
      - "Snapshot isolation is strictly weaker than lock-based REPEATABLE READ"
      - "Both are equivalent to SERIALIZABLE when no transaction aborts"
    explanation: |-
      Berenson et al. show the two levels are **incomparable**: each permits a history the other forbids.

      | Anomaly | Locking REPEATABLE READ | Snapshot isolation |
      |---|---|---|
      | A3 phantom (re-evaluated predicate changes) | possible: item locks do not cover new rows | not possible: the snapshot hides new rows |
      | A5B write skew | not possible: long read locks block the other's write | possible: only write-write conflicts are checked |

      In the doctors example, locking REPEATABLE READ makes each transaction hold read locks on both doctor rows, so each `UPDATE` waits for the other's read lock: a deadlock, one is aborted, and the invariant survives. Under snapshot isolation both commit.

      "Strictly stronger" and "strictly weaker" both fail on one of the two rows of the table.

      "Equivalent to SERIALIZABLE" fails because each permits one of these anomalies even with no aborts.
  - id: "postgres-snapshot-start"
    type: "single"
    prompt: "In PostgreSQL 19, T1 runs `BEGIN ISOLATION LEVEL REPEATABLE READ` and nothing else yet. T2 then updates a row from 100 to 200 and commits. T1 now runs its first `SELECT` of that row. What does it see?"
    correct:
      - "200, because the snapshot is taken at the transaction's first statement, not at `BEGIN`"
    distractors:
      - "100, because the snapshot is taken when `BEGIN ISOLATION LEVEL REPEATABLE READ` executes"
      - "An error, because the row changed after `BEGIN`"
      - "200 now, but 100 on any later read in the same transaction"
    explanation: |-
      PostgreSQL's documentation says a REPEATABLE READ transaction sees a snapshot as of the start of its first non-transaction-control statement. `BEGIN` alone takes no snapshot, so T2's commit (which happened before T1's first `SELECT`) is visible, and from then on T1 keeps that snapshot.

      | step | T1 | T2 | T1 sees |
      |---|---|---|---|
      | 1 | BEGIN (no snapshot yet) | | |
      | 2 | | UPDATE 100 → 200; commit | |
      | 3 | SELECT (snapshot taken now) | | 200 |
      | 4 | SELECT again | | 200 |

      "100" assumes `BEGIN` fixes the snapshot; it is a plausible reading of "transaction start" and is exactly the detail to check per engine.

      "An error" confuses visibility with write conflicts.

      "100 on later reads" would mean the snapshot moved backwards.

      Timeless lesson: "the snapshot is as of transaction start" always needs the engine's definition of *start*.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE t(id INT PRIMARY KEY, v INT); INSERT INTO t VALUES (1, 100);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [2, "UPDATE t SET v = 200 WHERE id = 1"]
          - [1, "SELECT v FROM t WHERE id = 1"]
          - [2, "UPDATE t SET v = 300 WHERE id = 1"]
          - [1, "SELECT v FROM t WHERE id = 1"]
          - [1, "COMMIT"]
        expect: ["ok", "ok", [[200]], "ok", [[200]], "ok"]
---
## Learning objectives

- State the ANSI SQL-92 isolation levels and the phenomena each forbids.
- Summarize Berenson et al.'s critique and the anomalies they added.
- Define snapshot isolation precisely, including first-committer-wins.
- Explain why snapshot isolation and lock-based REPEATABLE READ are incomparable.

## The invariant

SQL-92 defined isolation levels by forbidden phenomena: READ UNCOMMITTED forbids none of P1–P3, READ COMMITTED forbids P1, REPEATABLE READ forbids P1 and P2, SERIALIZABLE forbids all three (and the standard separately requires SERIALIZABLE to guarantee serializable execution). Gray et al.'s earlier "degrees of consistency" had defined levels by *locking* behavior; SQL-92 restated them as phenomena so that non-locking implementations could qualify.

Berenson, Bernstein, Gray, Melton, O'Neil and O'Neil (1995) showed that restatement is flawed. The phenomena are ambiguous; with the strict reading, a non-serializable history can avoid all three; and an important real-world level, **snapshot isolation (SI)**, does not fit anywhere. They defined SI as:

1. Each transaction reads from a snapshot of committed data as of its start (plus its own writes).
2. A transaction commits only if no concurrent transaction (one that committed after this one's snapshot) wrote any item it also wrote: **first committer wins**.

Their comparison table (abridged), with "possible" meaning the anomaly can occur:

| Level | P0 | P1 | P4 lost update | P2 | A5A read skew | P3 / A3 phantom | A5B write skew |
|---|---|---|---|---|---|---|---|
| READ COMMITTED (locking) | no | no | possible | possible | possible | possible | possible |
| REPEATABLE READ (locking) | no | no | no | no | no | possible | no |
| Snapshot isolation | no | no | no | no | no | sometimes | possible |
| SERIALIZABLE | no | no | no | no | no | no | no |

The "sometimes" for SI reflects that snapshot reads never see new rows (no A3), but a predicate-based constraint can still be broken by concurrent inserts, which is write skew in phantom form.

## Worked example

The same transfer schedule at two PostgreSQL levels (`x = 50`, `y = 50`, T2 moves 10 from x to y between T1's two reads):

| T1's level | T1 reads x | T1 reads y | T1's total |
|---|---|---|---|
| READ COMMITTED (snapshot per statement) | 50 | 60 | 110 |
| REPEATABLE READ (snapshot per transaction) | 50 | 50 | 100 |

And two concurrent increments of the same row at REPEATABLE READ: the second waits, then fails with SQLSTATE 40001 after the first commits; the final balance reflects one increment, and the application must retry the failed one. Retrying is not optional under SI: it is how first-committer-wins turns a lost update into a visible error.

## Where this shows up today

As of November 2026, PostgreSQL 19 documents its REPEATABLE READ as snapshot isolation (stronger than the standard requires for phantoms) and its default as READ COMMITTED; SQL Server 2025 offers a separate SNAPSHOT level (enabled per database with `ALLOW_SNAPSHOT_ISOLATION`) whose update conflicts roll the transaction back. Engine names for levels do not map one-to-one onto Berenson's definitions, so check each engine's documented anomalies rather than the label.

## Pitfalls checklist

- Equating an engine's "REPEATABLE READ" with ANSI's, or with serializable.
- Assuming SI never raises errors; concurrent writes to the same row fail and need a retry.
- Assuming the snapshot starts at `BEGIN` in every engine.
- Reading the ANSI table as a complete list of anomalies.
- Choosing a level by its name instead of by the anomalies the application must not see.

## Key takeaways

- SQL-92 defines levels by forbidding P1, P2, P3; Berenson et al. add P0, P4, A5A, A5B.
- Snapshot isolation: read a start-of-transaction snapshot; first committer wins on write-write conflicts.
- SI prevents lost updates and read skew but permits write skew.
- SI and lock-based REPEATABLE READ are incomparable.

## Sources

- ISO/IEC 9075:1992 (SQL-92), isolation levels.
- H. Berenson et al., "A Critique of ANSI SQL Isolation Levels", *ACM SIGMOD*, 1995.
- J. Gray, R. Lorie, G. Putzolu and I. Traiger, "Granularity of Locks and Degrees of Consistency in a Shared Data Base", 1976.
- PostgreSQL 19 documentation, "Transaction Isolation"; Microsoft Learn, "SET TRANSACTION ISOLATION LEVEL (Transact-SQL)".
