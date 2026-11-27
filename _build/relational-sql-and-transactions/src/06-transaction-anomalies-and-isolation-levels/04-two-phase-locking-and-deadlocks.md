---
title: "Two-Phase Locking, Locking Reads, Gap Locks and Deadlocks"
questions:
  - id: "two-phase-violation"
    type: "single"
    prompt: "Which lock sequence by a single transaction violates the two-phase locking rule?"
    correct:
      - "`lock(x)`, `unlock(x)`, `lock(y)`, `unlock(y)`"
    distractors:
      - "`lock(x)`, `lock(y)`, `unlock(x)`, `unlock(y)`"
      - "`lock(x)`, `lock(y)`, `unlock(y)`, `unlock(x)`"
      - "`lock(y)`, `lock(x)`, `unlock(x)`, `unlock(y)`"
    explanation: |-
      Two-phase locking (Eswaran, Gray, Lorie and Traiger, 1976) requires every transaction to acquire all its locks before releasing any: a **growing phase** followed by a **shrinking phase**. If every transaction obeys it, every resulting schedule is conflict-serializable.

      | sequence | first unlock at | any lock after it? | 2PL? |
      |---|---|---|---|
      | lock x, unlock x, lock y, unlock y | step 2 | yes, lock y at step 3 | violates |
      | lock x, lock y, unlock x, unlock y | step 3 | no | ok |
      | lock x, lock y, unlock y, unlock x | step 3 | no | ok |
      | lock y, lock x, unlock x, unlock y | step 3 | no | ok |

      The violating sequence lets another transaction change x between this transaction's use of x and its use of y, enabling exactly the read skew and lost update patterns from earlier sections.

      The order of acquisition or release among locks does not matter for 2PL; only that no lock follows an unlock.
  - id: "strict-2pl-purpose"
    type: "single"
    prompt: "Strict two-phase locking holds every *write* lock until the transaction commits or aborts, instead of releasing it as soon as the growing phase ends. What does that add?"
    correct:
      - "No other transaction can read or overwrite uncommitted data, which rules out dirty reads, dirty writes and cascading aborts"
    distractors:
      - "It makes deadlocks impossible, because locks are released in a fixed order"
      - "It prevents phantoms, because write locks also cover rows that do not exist yet"
      - "It lets readers proceed without locks, because writes are kept until commit"
    explanation: |-
      Basic 2PL guarantees serializable *orderings*, but if T1 releases a write lock on x before committing, T2 can read x; if T1 then aborts, T2 has read a value that never existed and must also be aborted (a cascading abort), or worse, T2 has already committed (the history is not recoverable). Holding write locks to the end makes each transaction's writes invisible and unwritable to others until commit: no P0, no P1, recoverable histories. Strong strict 2PL (rigorous 2PL) holds read locks to the end as well, which is what classic lock-based SERIALIZABLE does.

      "Deadlocks impossible" is false: holding locks longer makes deadlocks *more* likely.

      "Prevents phantoms" is false: item locks cannot cover nonexistent rows; that needs predicate, range or gap locks.

      "Readers proceed without locks" describes multiversion reads, not 2PL.
  - id: "select-for-update-serializes"
    type: "single"
    prompt: "In PostgreSQL at `READ COMMITTED`, `counters` holds `(1, 10)`. Both T1 and T2 run `SELECT n FROM counters WHERE id = 1 FOR UPDATE`, then `UPDATE counters SET n = <value read> + 1`, then `COMMIT`. T1 goes first; T2 starts its `SELECT ... FOR UPDATE` before T1 commits. What value does T2's `SELECT` return, and what is the final `n`?"
    correct:
      - "T2's `SELECT` waits, then returns 11; the final value is 12"
    distractors:
      - "T2's `SELECT` returns 10 immediately; the final value is 11"
      - "T2's `SELECT` waits, then returns 10; the final value is 11"
      - "T2's `SELECT` fails with a serialization error; the final value is 11"
    explanation: |-
      `SELECT ... FOR UPDATE` is a **locking read**: it takes the same row lock an `UPDATE` would, held until commit. A second locking read of the row waits for the first transaction to finish. At READ COMMITTED, PostgreSQL then re-reads the newest committed version of the row before returning it.

      | step | T1 | T2 | n |
      |---|---|---|---|
      | 1 | SELECT ... FOR UPDATE → 10 (row locked) | | 10 |
      | 2 | | SELECT ... FOR UPDATE: waits | |
      | 3 | UPDATE n = 11; COMMIT | | 11 |
      | 4 | | returns 11 | |
      | 5 | | UPDATE n = 12; COMMIT | 12 |

      This turns the read-modify-write into a critical section, preventing the lost update (verified by running the schedule).

      "Returns 10 immediately" describes a plain `SELECT`, which takes no row lock.

      "Waits, then returns 10" would hand T2 a stale value after waiting, defeating the purpose.

      "A serialization error" is what REPEATABLE READ would raise in this situation, not READ COMMITTED.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE counters(id INT PRIMARY KEY, n INT); INSERT INTO counters VALUES (1, 10);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL READ COMMITTED"]
          - [2, "BEGIN ISOLATION LEVEL READ COMMITTED"]
          - [1, "SELECT n FROM counters WHERE id = 1 FOR UPDATE"]
          - [2, "SELECT n FROM counters WHERE id = 1 FOR UPDATE"]
          - [1, "UPDATE counters SET n = 11 WHERE id = 1"]
          - [1, "COMMIT"]
          - [2, "UPDATE counters SET n = 12 WHERE id = 1"]
          - [2, "COMMIT"]
        expect: ["ok", "ok", [[10]], "blocked", "ok", "ok", "ok", "ok"]
        final:
          query: "SELECT n FROM counters WHERE id = 1"
          expect: [[12]]
      - engine: "schedule"
        setup: "CREATE TABLE counters(id INT PRIMARY KEY, n INT); INSERT INTO counters VALUES (1, 10);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL READ COMMITTED"]
          - [2, "BEGIN ISOLATION LEVEL READ COMMITTED"]
          - [1, "SELECT n FROM counters WHERE id = 1 FOR UPDATE"]
          - [2, "SELECT n FROM counters WHERE id = 1 FOR UPDATE"]
          - [1, "UPDATE counters SET n = 11 WHERE id = 1"]
          - [1, "COMMIT"]
        expect: ["ok", "ok", [[10]], "blocked->[[11]]", "ok", "ok"]
  - id: "innodb-next-key-lock"
    type: "single"
    prompt: "In MySQL 8.4 InnoDB at the default `REPEATABLE READ`, `t` has an index on `id` with rows 10 and 20. T1 runs `SELECT * FROM t WHERE id BETWEEN 10 AND 20 FOR UPDATE` and stays open. T2 runs `INSERT INTO t (id) VALUES (15)`. What happens to T2, according to the MySQL 8.4 manual?"
    correct:
      - "It waits, because T1's locking read locked the scanned index range with next-key and gap locks, which block inserts into the gap"
    distractors:
      - "It succeeds, because only the existing rows 10 and 20 are locked"
      - "It fails immediately with a duplicate-key error"
      - "It succeeds, but T1 never sees row 15 because of its snapshot"
    explanation: |-
      The MySQL 8.4 manual states that at REPEATABLE READ, for locking reads, `UPDATE` and `DELETE` with a range condition, InnoDB "locks the index range scanned, using gap locks or next-key locks to block insertions by other sessions into the gaps covered by the range". A next-key lock is a lock on an index record plus the gap before it. Locking the gap between 10 and 20 means id 15 cannot be inserted until T1 ends, so T1's repeated locking read cannot see a phantom.

      This is the engine-level answer to the theory problem from earlier: item locks cannot lock rows that do not exist yet, so lock-based engines lock **ranges of the index** instead (a practical form of predicate locking).

      "Only the existing rows" describes record locks only, which InnoDB uses at READ COMMITTED (gap locking mostly disabled there) or for unique-key equality lookups.

      "Duplicate-key error" needs an existing id 15.

      "Succeeds but invisible to T1's snapshot" describes plain consistent reads; locking reads read the latest committed data and must therefore block inserts to stay repeatable.
  - id: "wait-for-cycle"
    type: "single"
    prompt: "A lock manager's wait-for graph has the edges T1 → T2, T2 → T3, T3 → T1 and T4 → T1 (\"→\" means \"waits for a lock held by\"). Which transactions are deadlocked?"
    correct:
      - "T1, T2 and T3; T4 is blocked but not part of the cycle"
    distractors:
      - "T1, T2, T3 and T4"
      - "Only T1, because two transactions wait for it"
      - "None; a deadlock needs exactly two transactions waiting for each other"
      - "T2 and T3 only, because T1 has two waiters and will be chosen as the victim"
    explanation: |-
      A deadlock is a cycle in the wait-for graph: every transaction on the cycle waits for the next, so none can ever proceed. Detection means finding a cycle (for example by depth-first search) and aborting one transaction on it.

      | edge | on the cycle T1 → T2 → T3 → T1? |
      |---|---|
      | T1 → T2 | yes |
      | T2 → T3 | yes |
      | T3 → T1 | yes |
      | T4 → T1 | no: T4 waits *into* the cycle |

      Aborting any one of T1, T2, T3 breaks the cycle; T4 then proceeds once T1 (eventually) finishes.

      "Including T4" confuses being blocked with being deadlocked; T4 will run when the cycle is resolved.

      "Only T1" counts in-degree instead of looking for a cycle.

      "Exactly two transactions" is false; cycles of any length are deadlocks.

      "T2 and T3 only" confuses membership of the cycle with victim selection; which transaction an engine aborts does not change which ones are deadlocked.
    checks:
      - engine: "python"
        code: |
          edges = {"T1": ["T2"], "T2": ["T3"], "T3": ["T1"], "T4": ["T1"]}
          def on_cycle(n):
              seen, stack = set(), list(edges.get(n, []))
              while stack:
                  m = stack.pop()
                  if m == n: return True
                  if m in seen: continue
                  seen.add(m); stack.extend(edges.get(m, []))
              return False
          assert [t for t in sorted(edges) if on_cycle(t)] == ["T1", "T2", "T3"]
  - id: "lock-ordering-prevents-deadlock"
    type: "multiple"
    prompt: "Transfers between accounts `a` and `b` lock the source row first, then the destination. Two opposite transfers (1 → 2 and 2 → 1) occasionally deadlock. Which changes *prevent* this deadlock? Select all that apply."
    correct:
      - "Always lock the two accounts in ascending id order, whatever the transfer direction"
      - "Lock both rows in one statement first: `SELECT ... WHERE id IN (:a, :b) ORDER BY id FOR UPDATE`"
    distractors:
      - "Raise the lock wait timeout so the transactions wait longer"
      - "Catch the deadlock error and retry the transaction"
      - "Run both transfers at SERIALIZABLE in a lock-based engine"
    explanation: |-
      A deadlock needs a cycle, and a cycle needs two transactions to acquire the same locks in different orders. If every transaction acquires locks in one global order (here: ascending account id), the wait-for graph can only have edges from lower-held to higher-wanted locks, so no cycle can form.

      Trace without ordering: T1 locks 1, T2 locks 2, T1 waits for 2, T2 waits for 1: a cycle. PostgreSQL detected it after `deadlock_timeout` and aborted one transaction with SQLSTATE 40P01 (in the verified run, the one that started waiting first). With ordering, T2 must lock 1 first, so it waits behind T1 and no cycle forms.

      Distractors:

      - A longer timeout only makes the deadlock last longer before detection.
      - Retrying *recovers* from the deadlock (and every application should retry 40P01), but it does not prevent it.
      - Lock-based SERIALIZABLE holds more locks for longer; it makes deadlocks more likely, not less.
body_checks:
  - engine: "schedule"
    setup: "CREATE TABLE acct(id INT PRIMARY KEY, bal INT); INSERT INTO acct VALUES (1, 100), (2, 100);"
    wait: 0.3
    steps:
      - [1, "BEGIN"]
      - [2, "BEGIN"]
      - [1, "UPDATE acct SET bal = bal - 10 WHERE id = 1"]
      - [2, "UPDATE acct SET bal = bal - 10 WHERE id = 2"]
      - [1, "UPDATE acct SET bal = bal + 10 WHERE id = 2"]
      - [2, "UPDATE acct SET bal = bal + 10 WHERE id = 1"]
      - [0, "1.5"]
      - [1, "ROLLBACK"]
      - [2, "COMMIT"]
    expect: ["ok", "ok", "ok", "ok", "blocked->err:40P01", "blocked->ok", "ok", "ok", "ok"]
    final:
      query: "SELECT id, bal FROM acct ORDER BY id"
      expect: [[1, 110], [2, 90]]
      ordered: true
  - engine: "schedule"
    setup: "CREATE TABLE acct(id INT PRIMARY KEY, bal INT); INSERT INTO acct VALUES (1, 100), (2, 100);"
    wait: 0.3
    steps:
      - [1, "BEGIN"]
      - [2, "BEGIN"]
      - [1, "SELECT id FROM acct WHERE id IN (1, 2) ORDER BY id FOR UPDATE"]
      - [2, "SELECT id FROM acct WHERE id IN (2, 1) ORDER BY id FOR UPDATE"]
      - [1, "UPDATE acct SET bal = bal - 10 WHERE id = 1"]
      - [1, "UPDATE acct SET bal = bal + 10 WHERE id = 2"]
      - [1, "COMMIT"]
      - [2, "UPDATE acct SET bal = bal - 10 WHERE id = 2"]
      - [2, "UPDATE acct SET bal = bal + 10 WHERE id = 1"]
      - [2, "COMMIT"]
    expect: ["ok", "ok", [[1], [2]], "blocked->[[1], [2]]", "ok", "ok", "ok", "ok", "ok", "ok"]
    final:
      query: "SELECT id, bal FROM acct ORDER BY id"
      expect: [[1, 100], [2, 100]]
      ordered: true
  - engine: "postgres"
    query: "SHOW deadlock_timeout"
    expect: [["1s"]]
---
## Learning objectives

- State the two-phase locking rule and why strict 2PL holds write locks until commit.
- Use `SELECT ... FOR UPDATE` to serialize read-modify-write sequences.
- Explain how range (next-key, gap) locks prevent phantoms in lock-based engines.
- Detect deadlocks as wait-for cycles and prevent them with a global lock order.

## The invariant

**Two-phase locking (2PL)**: a transaction acquires locks (shared for reads, exclusive for writes) in a growing phase and releases them in a shrinking phase, never acquiring after releasing. Eswaran, Gray, Lorie and Traiger (1976) proved that if all transactions follow 2PL, every schedule is conflict-serializable. **Strict 2PL** additionally holds write locks until commit, giving recoverable schedules with no dirty reads or writes; holding *all* locks until commit (rigorous 2PL) is the classic implementation of SERIALIZABLE.

Two gaps remain:

1. **Phantoms.** Locking existing rows cannot stop a new row from entering a predicate. The theoretical fix is predicate locking (Eswaran et al.); practical engines lock **index ranges**: key-range locks in SQL Server, next-key and gap locks in InnoDB.
2. **Deadlocks.** Lock waits can form cycles. Engines either detect cycles in the **wait-for graph** and abort a victim, or time out waits. Applications prevent them by acquiring locks in a consistent global order.

Multiversion engines mix the two worlds: plain reads use snapshots and take no row locks, while writes and **locking reads** (`SELECT ... FOR UPDATE` / `FOR SHARE`) take row locks held to commit. A locking read is the standard way to make "read, decide, write" safe at READ COMMITTED.

## Worked example

The classic transfer deadlock, and its fix, in PostgreSQL:

| step | T1 (1 → 2) | T2 (2 → 1) |
|---|---|---|
| 1 | UPDATE acct 1 (locks 1) | |
| 2 | | UPDATE acct 2 (locks 2) |
| 3 | UPDATE acct 2: waits for T2 | |
| 4 | | UPDATE acct 1: waits for T1 → cycle |
| 5 | after `deadlock_timeout`, one transaction is aborted with SQLSTATE 40P01 | the other proceeds |

Fix by ordering:

```sql
BEGIN;
SELECT id FROM acct WHERE id IN (:src, :dst) ORDER BY id FOR UPDATE;  -- locks 1, then 2
UPDATE acct SET bal = bal - :amt WHERE id = :src;
UPDATE acct SET bal = bal + :amt WHERE id = :dst;
COMMIT;
```

Both transfers now request row 1 first; the second simply waits until the first commits. The same idea scales to any set of resources: sort lock requests by a stable key.

## Where this shows up today

As of November 2026, the MySQL 8.4 manual documents next-key and gap locks for locking reads and range-conditioned writes at REPEATABLE READ, and SQL Server uses key-range locks under SERIALIZABLE. PostgreSQL 19 detects deadlocks after `deadlock_timeout` (1 second by default) and does not use gap locks; it prevents phantoms through snapshots and, at SERIALIZABLE, predicate locks that never block.

## Pitfalls checklist

- Reading with a plain `SELECT` and then updating based on the value, at READ COMMITTED.
- Acquiring row locks in data-dependent order (source then destination).
- Treating lock timeouts as a deadlock cure.
- Running long transactions that hold locks across user think time or network calls.
- Being surprised by InnoDB inserts blocking on gaps after range locking reads.

## Key takeaways

- 2PL: no lock after the first unlock; strict 2PL holds write locks to commit.
- `SELECT ... FOR UPDATE` serializes read-modify-write on the locked rows.
- Range, gap and next-key locks are how lock-based engines stop phantoms.
- Deadlocks are wait-for cycles; a global lock order prevents them, retries recover from them.

## Sources

- K. P. Eswaran, J. N. Gray, R. A. Lorie and I. L. Traiger, "The Notions of Consistency and Predicate Locks in a Database System", *CACM* 19(11), 1976.
- P. A. Bernstein, V. Hadzilacos and N. Goodman, *Concurrency Control and Recovery in Database Systems*, Addison-Wesley, 1987.
- MySQL 8.4 Reference Manual, "InnoDB Locking" and "Transaction Isolation Levels"; PostgreSQL 19 documentation, "Explicit Locking" and "Lock Management" (`deadlock_timeout`); Microsoft Learn, "Transaction locking and row versioning guide".
