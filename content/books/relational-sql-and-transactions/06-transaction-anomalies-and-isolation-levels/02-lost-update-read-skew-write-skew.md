---
title: "Lost Updates, Read Skew and Write Skew"
questions:
  - id: "lost-update-trace"
    type: "single"
    prompt: "A counter `n` starts at 10. Two application threads each run `v = SELECT n` and then `UPDATE ... SET n = v + 1` in separate READ COMMITTED transactions, interleaved as `r1[n] r2[n] w1[n=11] c1 w2[n=11] c2`. What is the final value?"
    explanation: |-
      This is the **lost update** (Berenson et al.'s P4): `r1[x] ... w2[x] ... w1[x] ... c1`, here with the roles of T1 and T2 swapped. Both transactions read 10 before either wrote, so both compute 11.

      | step | T1 | T2 | n |
      |---|---|---|---|
      | r1[n] | reads 10 | | 10 |
      | r2[n] | | reads 10 | 10 |
      | w1[n=11], c1 | writes 11 | | 11 |
      | w2[n=11], c2 | | writes 11 (computed from stale 10) | 11 |

      One increment is lost: the intended result was 12.

      "12" is what serial execution would give.

      "10" assumes the second write is rejected and the first rolled back.

      "An error" is wrong: READ COMMITTED does not check whether the value T2 read is still current; T2's `UPDATE` simply waits for T1's row lock (if T1 had not committed yet) and then overwrites. (Stricter snapshot levels do raise an error here, as the next sections show.)
    options:
      - text: "12"
      - text: "11"
        correct: true
      - text: "10"
      - text: "An error, because READ COMMITTED detects the conflict"
  - id: "read-skew-trace"
    type: "single"
    prompt: "In PostgreSQL, `acct(id, bal)` holds `('x', 50)` and `('y', 50)`; transfers keep `x + y = 100`. T1 (`READ COMMITTED`) reads `x`. T2 moves 10 from `x` to `y` in one transaction and commits. T1 then reads `y`. What total does T1 observe?"
    explanation: |-
      **Read skew** (Berenson et al.'s A5A): `r1[x] ... w2[x] ... w2[y] ... c2 ... r1[y]`. T1 reads x before the transfer and y after it, so it sees a combination of states that never existed together.

      | step | T1 | T2 | T1 has |
      |---|---|---|---|
      | 1 | read x = 50 | | x = 50 |
      | 2 | | x = 40, y = 60; commit | |
      | 3 | read y = 60 | | 50 + 60 = 110 |

      At READ COMMITTED, each statement sees the latest committed data, so the second read sees T2's commit (verified by running the schedule). Under a transaction-level snapshot (REPEATABLE READ in PostgreSQL), T1 would read y = 50 and see 100.

      "100" is the consistent total that a snapshot would give.

      "90" pairs the new x with the old y, the opposite order.

      "Blocks" assumes readers wait for writers; multiversion reads do not.
    options:
      - text: "100"
      - text: "90"
      - text: "110"
        correct: true
      - text: "T1's second read blocks until T2's transfer is undone"
  - id: "write-skew-doctors"
    type: "single"
    prompt: "In PostgreSQL at `REPEATABLE READ` (snapshot isolation), `doctors(name, on_call)` has alice and bob both on call; the rule is \"at least one doctor on call\". T1 counts on-call doctors (2) and sets alice off call; T2, concurrently, counts (2) and sets bob off call. Both try to commit. How many doctors are on call afterwards?"
    explanation: |-
      **Write skew** (A5B): `r1[x] ... r2[y] ... w1[y] ... w2[x] ... (c1 and c2)`, generalized here to a predicate read. Each transaction reads an overlapping set (all on-call doctors), checks the rule against its snapshot, and writes a *different* row.

      | step | T1 | T2 | on call (committed) |
      |---|---|---|---|
      | 1 | count = 2 | | alice, bob |
      | 2 | | count = 2 | alice, bob |
      | 3 | alice off | | |
      | 4 | | bob off | |
      | 5 | commit | | bob |
      | 6 | | commit | none |

      Snapshot isolation only checks *write-write* conflicts (first committer wins on the same row). The writes touch different rows, so both commits succeed (verified on PostgreSQL REPEATABLE READ), and the invariant is broken although each transaction alone preserved it.

      "A serialization error" happens at PostgreSQL SERIALIZABLE, not REPEATABLE READ.

      "The second UPDATE blocks" would require both to update the same row.

      "Both rolled back" has no cause; nothing conflicts at this level.
    options:
      - text: "1, because the second commit fails with a serialization error"
      - text: "1, because the second `UPDATE` blocks on the first"
      - text: "2, because both transactions are rolled back"
      - text: "0"
        correct: true
  - id: "preventing-lost-update"
    type: "multiple"
    prompt: "Which approaches prevent the lost increment from the first question while staying at READ COMMITTED? Select all that apply."
    explanation: |-
      The lost update needs a read whose result is used for a later write, with another write slipping in between.

      - **Atomic update:** `n = n + 1` reads and writes in one statement. In PostgreSQL at READ COMMITTED, a second concurrent `UPDATE` waits for the first transaction's row lock, then re-evaluates against the newly committed row version, so it increments 11 to 12 (verified: the final value is 12).
      - **Compare-and-set (optimistic concurrency):** the `WHERE version = 7` condition makes the write succeed only if nobody changed the row since it was read; a stale writer updates 0 rows and retries.

      Distractors:

      - One READ COMMITTED transaction does not help: the `SELECT` takes no lock, so the interleaving is unchanged.
      - An application-side counter has the same race across instances and loses updates on crashes.
      - `CHECK (n >= 0)` validates values; both writes of 11 satisfy it.
    options:
      - text: "Run `SELECT n` and `UPDATE ... SET n = v + 1` inside one READ COMMITTED transaction"
      - text: "Keep the counter in application memory and write it back every minute"
      - text: "Add `CHECK (n >= 0)` to the table"
      - text: "Let the database compute the new value: `UPDATE counters SET n = n + 1 WHERE id = 1`"
        correct: true
      - text: "Compare-and-set: `UPDATE counters SET n = 11, version = version + 1 WHERE id = 1 AND version = 7`, retrying when it updates 0 rows"
        correct: true
  - id: "why-si-misses-write-skew"
    type: "single"
    prompt: "Snapshot isolation aborts one of two concurrent transactions that write the *same* row (first committer wins). Why does that rule not stop write skew?"
    explanation: |-
      Write skew is a cycle of **read-write** dependencies: T1 reads something T2 writes, and T2 reads something T1 writes (each read happened in a snapshot that missed the other's write). Snapshot isolation tracks only **write-write** overlaps. With disjoint write sets, both transactions pass the check.

      | transaction | reads | writes |
      |---|---|---|
      | T1 | alice, bob (both on call) | alice |
      | T2 | alice, bob (both on call) | bob |

      Write sets {alice} and {bob} do not overlap; the conflict lives entirely in the read sets.

      "Only checked at READ COMMITTED" is backwards: READ COMMITTED has no first-committer rule at all.

      "Never read the rows" is wrong; the reads are essential: they are the stale premises.

      "Allows dirty writes" is false; snapshot isolation prevents P0 like every level.
    options:
      - text: "Because first committer wins is only checked at READ COMMITTED"
      - text: "In write skew the two transactions write different rows, so there is no write-write conflict for the rule to detect"
        correct: true
      - text: "Because write skew transactions never read the rows they depend on"
      - text: "Because snapshot isolation allows dirty writes on rows read by the other transaction"
  - id: "invariants-exposed-to-write-skew"
    type: "multiple"
    prompt: "Under snapshot isolation with no extra locking, which invariants can be broken by write skew? Select all that apply."
    explanation: |-
      Write skew threatens invariants that span several rows (or rows that do not exist yet) and are checked by the application reading data that other transactions then change *without writing the same row*.

      - On-call: each transaction updates a different doctor row.
      - Bookings: each transaction inserts a new, different row; the conflict is between a predicate read ("overlapping bookings for room 5") and an insert into that predicate, the phantom form of write skew.

      Distractors:

      - A `CHECK` on a single row is evaluated on the row being written; concurrent updates of the same row also conflict on that row.
      - A `UNIQUE` index is enforced by the engine across transactions: the second insert of a duplicate waits and then fails.
      - If every line change also updates the order row, two concurrent changes write the same row and first-committer-wins aborts one. Deliberately writing a shared row like this is called **materializing the conflict**, a standard remedy.
    options:
      - text: "\"At least one doctor is on call\", checked by counting rows and updating one's own row"
        correct: true
      - text: "\"No two bookings for the same room overlap\", checked by a query before inserting a booking"
        correct: true
      - text: "\"An account balance is never negative\", enforced by `CHECK (bal >= 0)` on that row"
      - text: "\"Usernames are unique\", enforced by a `UNIQUE` index"
      - text: "\"An order's total equals the sum of its lines\", when every line change also updates the order row"
---
## Learning objectives

- Trace the lost update, read skew and write skew anomalies on interleaved schedules.
- Explain which anomaly each isolation mechanism prevents.
- Prevent lost updates with atomic updates or compare-and-set.
- Recognize invariants that write skew can break and why snapshot isolation misses it.

## The invariant

Berenson et al. (1995) showed that the ANSI phenomena P0–P3 do not capture several real anomalies, and named them:

| Anomaly | Pattern | Story |
|---|---|---|
| P4 lost update | `r1[x] ... w2[x] ... w1[x] ... c1` | T1 writes a value computed from a stale read, overwriting T2 |
| A5A read skew | `r1[x] ... w2[x] ... w2[y] ... c2 ... r1[y] ... (c1 or a1)` | T1 sees x before and y after another transaction's change |
| A5B write skew | `r1[x] ... r2[y] ... w1[y] ... w2[x] ... (c1 and c2)` | both read overlapping data, write disjoint items, and together break a constraint |

Each is a non-serializable history, and each is excluded by a different mechanism:

- **Lost update** needs a read-then-write with an intervening write. Long read locks (two-phase locking), first-committer-wins under snapshot isolation, or doing the read and write in one atomic statement all prevent it.
- **Read skew** needs reads from two different database states. A single snapshot per transaction (REPEATABLE READ in PostgreSQL, SNAPSHOT in SQL Server) prevents it.
- **Write skew** needs only overlapping *reads* and disjoint *writes*. Snapshot isolation does not prevent it; serializable two-phase locking and Serializable Snapshot Isolation do. Applications can also prevent it by materializing the conflict (making both transactions write a common row) or by locking what they read.

The common thread: the transaction's decision depends on data it read, and the decision is invalid if that data changed before commit.

## Worked example

The on-call rule, under three regimes, with alice and bob both on call:

| regime | T1 and T2 both: count on-call, set self off, commit | result |
|---|---|---|
| READ COMMITTED | both counts see 2; both commit | 0 on call |
| REPEATABLE READ (snapshot) | both counts see 2; different rows written; both commit | 0 on call |
| SERIALIZABLE (PostgreSQL SSI) | one commit fails with SQLSTATE 40001 | 1 on call after retry |

Materializing the conflict at the lower levels:

```sql
-- Every transaction that changes the on-call roster also updates this row
UPDATE roster_guard SET version = version + 1 WHERE ward = 'ER';
SELECT COUNT(*) FROM doctors WHERE ward = 'ER' AND on_call;
UPDATE doctors SET on_call = FALSE WHERE name = 'alice';
```

Under snapshot isolation, the two transactions now write the same `roster_guard` row, so one fails (PostgreSQL REPEATABLE READ raises `could not serialize access due to concurrent update`) and must retry, seeing the other's change.

## Where this shows up today

As of November 2026, PostgreSQL 19's REPEATABLE READ is snapshot isolation with first-updater-wins errors, so it prevents lost updates and read skew but permits write skew; its SERIALIZABLE level also prevents write skew. In MySQL 8.4 InnoDB's default REPEATABLE READ, plain reads use a snapshot but `UPDATE` reads the latest committed row, so a read-then-write pattern can lose updates unless the read is a locking read. Booking, inventory and scheduling systems are where write skew bites most often.

## Pitfalls checklist

- Implementing counters and balances as `SELECT` then `UPDATE` with a computed constant.
- Assuming a transaction's reads are consistent at READ COMMITTED.
- Assuming REPEATABLE READ or snapshot isolation is serializable; write skew remains.
- Checking a multi-row rule in the application without locking or materializing the conflict.
- Treating an empty "no overlapping booking" query as a reservation; it locks nothing.

## Key takeaways

- Lost update: a stale read feeds a write; fix with atomic updates, compare-and-set, or locks.
- Read skew: reads from two states; fix with one snapshot per transaction.
- Write skew: overlapping reads, disjoint writes; snapshot isolation misses it.
- Remedies for write skew are true serializability, locking reads, or materializing the conflict.

## Sources

- H. Berenson et al., "A Critique of ANSI SQL Isolation Levels", *ACM SIGMOD*, 1995.
- A. Fekete, D. Liarokapis, E. O'Neil, P. O'Neil and D. Shasha, "Making Snapshot Isolation Serializable", *ACM TODS* 30(2), 2005.
- PostgreSQL 19 documentation, "Transaction Isolation"; MySQL 8.4 Reference Manual, "Consistent Nonlocking Reads" and "Locking Reads".
