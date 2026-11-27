---
title: "Serializable Snapshot Isolation, Retries and Engine Defaults"
questions:
  - id: "ssi-dangerous-structure"
    type: "single"
    prompt: "Serializable Snapshot Isolation (SSI) runs transactions under snapshot isolation and adds a run-time check. What pattern does that check look for?"
    correct:
      - "A pivot: a transaction with both an incoming and an outgoing rw-antidependency among concurrent transactions"
    distractors:
      - "Two concurrent transactions that write the same row, aborting whichever writer tries to commit second, exactly as plain snapshot isolation does"
      - "Any transaction that reads a row another transaction later modifies, aborting the reader"
      - "Every cycle in the complete serialization graph, aborting exactly the transactions on each cycle"
    explanation: |-
      Fekete, Liarokapis, O'Neil, O'Neil and Shasha (2005) proved that every non-serializable execution under snapshot isolation contains a **dangerous structure**: two consecutive read-write antidependency edges T_in →rw T_pivot →rw T_out between concurrent transactions (where "T1 →rw T2" means T1 read a version that T2 overwrote). Cahill, Röhm and Fekete (2008) turned this into SSI: track rw-antidependencies at run time with non-blocking "SIREAD" markers, and abort a transaction when a pivot appears.

      | Check | What it detects | Effect |
      |---|---|---|
      | first committer wins (plain SI) | write-write conflicts | prevents lost updates, not write skew |
      | SSI dangerous structure | two consecutive rw edges | prevents write skew and all other SI anomalies |

      It is conservative: it may abort transactions that would in fact have been serializable (false positives), because tracking full cycles is too expensive.

      "Same row, later writer aborted" is plain SI's rule.

      "Any read followed by a write" would abort a huge fraction of transactions; one rw edge alone is harmless.

      "Every cycle exactly" describes serialization-graph testing, which SSI deliberately approximates.
  - id: "ssi-aborts-write-skew"
    type: "single"
    prompt: "Repeat the on-call schedule in PostgreSQL at `SERIALIZABLE`: alice and bob are on call; T1 and T2 each count on-call doctors (2) and set themselves off call; T1 commits first. What happens to T2, and how many doctors remain on call?"
    correct:
      - "T2's `COMMIT` fails with SQLSTATE 40001; one doctor remains on call"
    distractors:
      - "T2 commits; no doctor remains on call"
      - "T2's `UPDATE` blocks until T1 commits, then succeeds; no doctor remains on call"
      - "T1 and T2 deadlock, and PostgreSQL aborts both"
    explanation: |-
      Each transaction read both doctor rows and wrote a row the other had read: T1 →rw T2 (T1 read bob, T2 wrote bob) and T2 →rw T1 (T2 read alice, T1 wrote alice). Under SSI that is a dangerous structure, so PostgreSQL refuses to commit the second transaction.

      | step | T1 | T2 |
      |---|---|---|
      | 1 | count = 2 | |
      | 2 | | count = 2 |
      | 3 | alice off | |
      | 4 | | bob off |
      | 5 | COMMIT ok | |
      | 6 | | COMMIT → ERROR 40001 |

      After T2 rolls back, one doctor (bob) is on call (verified by running the schedule). If T2 retries, its new snapshot sees count = 1 and the application can refuse the request.

      "T2 commits" is the REPEATABLE READ (plain SI) outcome.

      "Blocks then succeeds" describes locking behavior; PostgreSQL's SSI markers never block.

      "Deadlock" requires lock waits; there are none here.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE doctors(name TEXT PRIMARY KEY, on_call BOOLEAN); INSERT INTO doctors VALUES ('alice', TRUE), ('bob', TRUE);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL SERIALIZABLE"]
          - [2, "BEGIN ISOLATION LEVEL SERIALIZABLE"]
          - [1, "SELECT COUNT(*) FROM doctors WHERE on_call"]
          - [2, "SELECT COUNT(*) FROM doctors WHERE on_call"]
          - [1, "UPDATE doctors SET on_call = FALSE WHERE name = 'alice'"]
          - [2, "UPDATE doctors SET on_call = FALSE WHERE name = 'bob'"]
          - [1, "COMMIT"]
          - [2, "COMMIT"]
        expect: ["ok", "ok", [[2]], [[2]], "ok", "ok", "ok", "err:40001"]
        final:
          query: "SELECT COUNT(*) FROM doctors WHERE on_call"
          expect: [[1]]
  - id: "retry-whole-transaction"
    type: "single"
    prompt: "This retry wrapper is used with a SERIALIZABLE database. What is wrong with it?\n\n```python\ndef transfer(conn, src, dst, amount):\n    with conn.cursor() as cur:\n        cur.execute(\"BEGIN ISOLATION LEVEL SERIALIZABLE\")\n        cur.execute(\"SELECT bal FROM acct WHERE id = %s\", (src,))\n        bal = cur.fetchone()[0]\n        if bal < amount:\n            raise ValueError(\"insufficient funds\")\n        cur.execute(\"UPDATE acct SET bal = bal - %s WHERE id = %s\", (amount, src))\n        cur.execute(\"UPDATE acct SET bal = bal + %s WHERE id = %s\", (amount, dst))\n        for attempt in range(3):\n            try:\n                cur.execute(\"COMMIT\")\n                return\n            except SerializationFailure:\n                continue\n```"
    correct:
      - "Retrying only `COMMIT` cannot work: the failed transaction is already rolled back, so the whole transaction, including its read, must rerun"
    distractors:
      - "Nothing; retrying `COMMIT` three times is the standard way to handle SQLSTATE 40001"
      - "It should retry only the two `UPDATE` statements, reusing the balance it already read, because reads cannot cause serialization failures"
      - "It should switch to READ COMMITTED on the second attempt so that the commit succeeds"
    explanation: |-
      A serialization failure (SQLSTATE 40001) means the database rolled the transaction back because its reads and writes could not be placed in a serial order with the others. Everything the transaction did is gone, and, crucially, the *decisions* it made (here `bal < amount`) were based on a snapshot that is no longer valid. PostgreSQL's documentation advises retrying the complete transaction, including all logic that decided which SQL to issue and which values to use.

      Correct shape: loop around the whole unit of work (`BEGIN` ... reads ... decisions ... writes ... `COMMIT`), retry on 40001 (and 40P01 for deadlocks), with a bounded number of attempts and some backoff.

      "Retry `COMMIT`" fails because there is nothing left to commit.

      "Reuse the balance" repeats the stale-read bug that caused the failure.

      "Switch to READ COMMITTED" silences the error by permitting the anomaly the SERIALIZABLE level was chosen to prevent.

      Side effects outside the database (emails, HTTP calls) inside the retried block must be idempotent or moved after the commit.
  - id: "read-only-anomaly"
    type: "single"
    prompt: "Under plain snapshot isolation, can a transaction that only *reads* observe a database state that is inconsistent with every serial order of the committed transactions?"
    correct:
      - "Yes; Fekete, O'Neil and O'Neil gave an SI execution that is serializable only without the read-only transaction"
    distractors:
      - "No; a read-only transaction always sees a consistent committed snapshot, which is by definition serializable"
      - "No; only transactions that write can take part in a serialization anomaly"
      - "Yes, but only if the read-only transaction reads uncommitted data"
    explanation: |-
      Fekete, O'Neil and O'Neil (2004, "A Read-Only Transaction Anomaly Under Snapshot Isolation") gave a bank example: two update transactions whose SI execution is serializable on its own, plus a read-only transaction whose snapshot falls between them in a way that shows a state no serial order produces. A snapshot is a consistent set of *committed* data, but "committed so far" is not the same as "a prefix of some serial order" once the writers themselves have a rw-antidependency.

      This is why SSI also tracks read-only transactions. PostgreSQL additionally offers `SERIALIZABLE READ ONLY DEFERRABLE`, which waits until it can take a snapshot guaranteed to be safe and then runs without any risk of serialization failure, useful for long reports.

      "Always consistent" confuses snapshot consistency with serializability.

      "Only writers" ignores that the reader's observation is part of the history.

      "Uncommitted data" is impossible under SI.
  - id: "engine-default-levels"
    type: "multiple"
    prompt: "As of November 2026, which of these default isolation levels are stated in the engines' own documentation? Select all that apply."
    correct:
      - "PostgreSQL 19: READ COMMITTED"
      - "MySQL 8.4 with InnoDB: REPEATABLE READ"
      - "SQL Server 2025 (on-premises): READ COMMITTED, implemented with locks unless `READ_COMMITTED_SNAPSHOT` is turned on"
    distractors:
      - "PostgreSQL 19: SERIALIZABLE"
      - "SQLite: READ UNCOMMITTED for all connections, because it has no row-level locking"
    explanation: |-
      The documented defaults:

      | Engine | Default | Notes from the documentation |
      |---|---|---|
      | PostgreSQL 19 | READ COMMITTED | snapshot per statement; REPEATABLE READ is snapshot isolation; SERIALIZABLE is SSI |
      | MySQL 8.4 InnoDB | REPEATABLE READ | consistent reads from the first read's snapshot; locking reads use next-key locks |
      | SQL Server 2025 | READ COMMITTED | `READ_COMMITTED_SNAPSHOT` is OFF by default on SQL Server and ON by default on Azure SQL Database |
      | SQLite | SERIALIZABLE | one writer at a time; in WAL mode readers see a snapshot |

      "PostgreSQL 19: SERIALIZABLE" confuses PostgreSQL with engines such as CockroachDB, whose documentation states SERIALIZABLE as the default.

      "SQLite: READ UNCOMMITTED" is wrong: SQLite's documentation says all transactions are serializable, except for connections sharing a cache that turn on `PRAGMA read_uncommitted`.

      Defaults are product decisions that can change between versions; the timeless point is to know which anomalies your actual level allows.
  - id: "ssi-catches-phantom-write-skew"
    type: "single"
    prompt: "`bookings(room, slot)` is empty. T1 and T2 each run `SELECT COUNT(*) FROM bookings WHERE room = 5 AND slot = 10` (getting 0) and then insert `(5, 10)`. T1 commits first. How many bookings exist after both finish, at PostgreSQL REPEATABLE READ and at SERIALIZABLE (where the failed transaction is not retried)?"
    correct:
      - "REPEATABLE READ: 2; SERIALIZABLE: 1"
    distractors:
      - "REPEATABLE READ: 1; SERIALIZABLE: 1"
      - "REPEATABLE READ: 2; SERIALIZABLE: 2"
      - "REPEATABLE READ: 1; SERIALIZABLE: 0"
    explanation: |-
      This is write skew in phantom form: each transaction reads a *predicate* (no booking for room 5 at slot 10) and inserts a row that would have changed the other's result. No existing row is written twice, so snapshot isolation's write-write check sees nothing.

      | level | T1 | T2 | bookings |
      |---|---|---|---|
      | REPEATABLE READ | count 0, insert, commit | count 0, insert, commit | 2 (double booking) |
      | SERIALIZABLE | count 0, insert, commit | count 0, insert, COMMIT → 40001 | 1 |

      PostgreSQL's SSI records SIREAD (predicate) locks on what each transaction read, at relation, page or tuple granularity, so an insert into the read range creates the rw-antidependency that completes the dangerous structure. Both outcomes were verified by running the schedules.

      "RR: 1" assumes snapshot isolation blocks the second insert; nothing conflicts at the row level.

      "SERIALIZABLE: 2" ignores SSI's predicate tracking.

      "SERIALIZABLE: 0" aborts both; SSI aborts only as many as needed, and the first committer succeeds.

      A `UNIQUE (room, slot)` constraint would also prevent this particular double booking at any level; SSI matters when the rule (for example, overlapping time ranges) cannot be expressed as a constraint.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE bookings(room INT, slot INT);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [2, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [1, "SELECT COUNT(*) FROM bookings WHERE room = 5 AND slot = 10"]
          - [2, "SELECT COUNT(*) FROM bookings WHERE room = 5 AND slot = 10"]
          - [1, "INSERT INTO bookings VALUES (5, 10)"]
          - [2, "INSERT INTO bookings VALUES (5, 10)"]
          - [1, "COMMIT"]
          - [2, "COMMIT"]
        expect: ["ok", "ok", [[0]], [[0]], "ok", "ok", "ok", "ok"]
        final:
          query: "SELECT COUNT(*) FROM bookings"
          expect: [[2]]
      - engine: "schedule"
        setup: "CREATE TABLE bookings(room INT, slot INT);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL SERIALIZABLE"]
          - [2, "BEGIN ISOLATION LEVEL SERIALIZABLE"]
          - [1, "SELECT COUNT(*) FROM bookings WHERE room = 5 AND slot = 10"]
          - [2, "SELECT COUNT(*) FROM bookings WHERE room = 5 AND slot = 10"]
          - [1, "INSERT INTO bookings VALUES (5, 10)"]
          - [2, "INSERT INTO bookings VALUES (5, 10)"]
          - [1, "COMMIT"]
          - [2, "COMMIT"]
        expect: ["ok", "ok", [[0]], [[0]], "ok", "ok", "ok", "err:40001"]
        final:
          query: "SELECT COUNT(*) FROM bookings"
          expect: [[1]]
body_checks:
  - engine: "python"
    code: |
      import time, psycopg2
      from psycopg2 import errors
      RETRYABLE = (errors.SerializationFailure, errors.DeadlockDetected)
      def run_serializable(conn, work, attempts=5):
          for attempt in range(attempts):
              try:
                  with conn:
                      with conn.cursor() as cur:
                          cur.execute("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE")
                          return work(cur)
              except RETRYABLE:
                  time.sleep(0.01 * 2 ** attempt)
          raise RuntimeError("gave up after repeated serialization failures")
      conn = psycopg2.connect(host="/tmp/pg", port=5433, user="postgres", dbname="postgres")
      def work(cur):
          cur.execute("SHOW transaction_isolation")
          return cur.fetchone()[0]
      assert run_serializable(conn, work) == "serializable"
      conn.close()
  - engine: "postgres"
    query: "SHOW default_transaction_isolation"
    expect: [["read committed"]]
---
## Learning objectives

- Explain how Serializable Snapshot Isolation extends snapshot isolation to full serializability.
- Predict which transaction fails under SSI in write-skew schedules, including phantom forms.
- Write correct retry logic for serialization failures and deadlocks.
- Look up and reason about the default isolation levels of current engines.

## The invariant

Serializability can be achieved two ways. **Strict two-phase locking** prevents conflicts by blocking. **Optimistic** schemes let transactions run and abort those whose combination would not be serializable. Snapshot isolation is optimistic for write-write conflicts only, which is why write skew slips through.

The theory that closes the gap:

1. Adya (1999) characterized isolation levels by cycles in a dependency graph with three kinds of edges: write-write, write-read, and read-write antidependencies.
2. Fekete et al. (2005) proved that under SI every non-serializable history contains two consecutive rw-antidependency edges between concurrent transactions, T_in →rw T_pivot →rw T_out (the **dangerous structure**).
3. Cahill, Röhm and Fekete (2008) built **SSI**: run SI, record reads with non-blocking markers, detect rw-antidependencies as writes happen, and abort when a dangerous structure forms. It may abort some serializable executions, but it never blocks readers or writers.

The practical consequence is the same for every serializable implementation, locking or optimistic: **transactions can fail and must be retried in full**. The standard reserves SQLSTATE class 40 (transaction rollback) for this; 40001 is `serialization_failure`. PostgreSQL reports deadlocks as 40P01.

Three layers once more: the standard defines SERIALIZABLE as equivalent to some serial execution; the theory gives the dependency-graph characterization and SSI; each engine picks its mechanism (SSI in PostgreSQL, strict 2PL with range locks in SQL Server and InnoDB, serialized writers in SQLite) and its default level.

## Worked example

A retry loop that is correct for any serializable engine:

```python
import time
from psycopg2 import errors

RETRYABLE = (errors.SerializationFailure, errors.DeadlockDetected)

def run_serializable(conn, work, attempts=5):
    for attempt in range(attempts):
        try:
            with conn:                                  # commits or rolls back
                with conn.cursor() as cur:
                    cur.execute("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE")
                    return work(cur)                    # all reads and decisions inside
        except RETRYABLE:
            time.sleep(0.01 * 2 ** attempt)             # back off, then rerun everything
    raise RuntimeError("gave up after repeated serialization failures")
```

Everything that depends on data read in the transaction lives inside `work`, so each retry re-reads and re-decides. With the on-call schedule at SERIALIZABLE, the first doctor's transaction commits, the second gets 40001, and its retry sees one doctor on call and refuses.

## Where this shows up today

As of November 2026, PostgreSQL 19 implements SERIALIZABLE as SSI; the MySQL 8.4 manual documents REPEATABLE READ as InnoDB's default; SQL Server 2025 defaults to lock-based READ COMMITTED; and SQLite documents serializable transactions with a single writer. Among distributed SQL systems, CockroachDB's documentation (v26.2) states SERIALIZABLE as its default, and Google Cloud Spanner's documentation states serializable isolation with external consistency as its default.

## Pitfalls checklist

- Turning on SERIALIZABLE without adding retry logic.
- Retrying only the failed statement or `COMMIT` instead of the whole transaction.
- Performing non-idempotent side effects inside a retried transaction.
- Assuming SERIALIZABLE means "slow because of locks"; under SSI it means "occasional aborts".
- Assuming the same level name means the same guarantees across engines.

## Key takeaways

- SSI = snapshot isolation + detection of two consecutive rw-antidependencies; it prevents write skew without blocking.
- Serializable levels abort transactions; SQLSTATE 40001 (and 40P01) mean "rerun the whole transaction".
- Read-only transactions can observe SI anomalies; PostgreSQL's `READ ONLY DEFERRABLE` avoids them.
- Defaults differ by engine; choose the level by the anomalies you must exclude.

## Sources

- A. Adya, *Weak Consistency: A Generalized Theory and Optimistic Implementations for Distributed Transactions*, PhD thesis, MIT, 1999.
- A. Fekete, D. Liarokapis, E. O'Neil, P. O'Neil and D. Shasha, "Making Snapshot Isolation Serializable", *ACM TODS* 30(2), 2005.
- A. Fekete, E. O'Neil and P. O'Neil, "A Read-Only Transaction Anomaly Under Snapshot Isolation", *ACM SIGMOD Record* 33(3), 2004.
- M. J. Cahill, U. Röhm and A. D. Fekete, "Serializable Isolation for Snapshot Databases", *ACM SIGMOD*, 2008.
- PostgreSQL 19 documentation, "Transaction Isolation" and "Serialization Failure Handling"; MySQL 8.4 Reference Manual, "Transaction Isolation Levels"; Microsoft Learn, "SET TRANSACTION ISOLATION LEVEL (Transact-SQL)"; SQLite documentation, "Isolation In SQLite"; CockroachDB v26.2 documentation, "Read Committed Transactions"; Google Cloud, "Spanner isolation levels overview".
