---
title: "From Dirty Writes to Phantoms"
questions:
  - id: "classify-dirty-read"
    type: "single"
    prompt: "Classify this history (subscripts are transaction numbers):\n\n| step | T1 | T2 |\n|---|---|---|\n| 1 | `w1[x]` | |\n| 2 | | `r2[x]` |\n| 3 | `abort` | |\n| 4 | | `commit` |"
    correct:
      - "A dirty read (P1): T2 read a value T1 wrote and later rolled back"
    distractors:
      - "A dirty write (P0): two transactions touched `x` before either finished"
      - "A non-repeatable read (P2): T2 read `x` while T1 was changing it"
      - "No anomaly, because T1 aborted and its write never became visible"
      - "A phantom (P3): T2 read `x` while T1 was inserting it"
    explanation: |-
      Berenson et al. (1995) define P1 (dirty read) as `w1[x] ... r2[x] ... (c1 or a1)`: a transaction reads a data item modified by another transaction that has not yet committed or aborted. Here T2 reads `x` after `w1[x]` and before T1's abort, so T2 observed a value that, after the abort, never existed. T2 then commits, possibly having acted on it. This is even the strict form A1 (T1 aborts, T2 commits).

      "Dirty write" (P0) needs a *second write*, `w1[x] ... w2[x]`; T2 only reads.

      "Non-repeatable read" (P2) is `r1[x] ... w2[x]`: a read *followed* by another transaction's write; here the write comes first.

      "No anomaly" is the trap: T1's write was visible to T2 while uncommitted. That visibility is exactly the phenomenon.

      "A phantom" needs a predicate read `r2[P]` whose result set changes; this history has a single-item read.
  - id: "dirty-write-rollback"
    type: "single"
    prompt: "`x` starts at 0. Without any write locks, this history runs: `w1[x = 1]`, `w2[x = 2]`, `c2`, then T1 aborts and its rollback restores `x` to the value T1 saw before writing. What is the final value of `x`?"
    correct:
      - "0, so T2's committed write is lost"
    distractors:
      - "2, because T2 committed last"
      - "1, because T1 wrote first"
      - "It depends on whether T2 read `x` before writing"
    explanation: |-
      This is P0, a dirty write: `w1[x] ... w2[x] ... (c1 or a1)`. The usual way to undo a write is to restore its before-image.

      | step | action | x |
      |---|---|---|
      | start | | 0 |
      | w1[x = 1] | T1's before-image is 0 | 1 |
      | w2[x = 2] | overwrites uncommitted data | 2 |
      | c2 | T2 commits 2 | 2 |
      | a1 | restore T1's before-image | 0 |

      T2's committed update has vanished, a violation of durability for T2. The alternative (leave 2 in place on T1's abort) breaks atomicity for T1 if T2 had instead aborted first. There is no correct undo once two uncommitted writes interleave on the same item.

      "2" assumes rollback knows about T2's write; before-image recovery does not.

      "1" mistakes write order for commit order.

      "Depends on whether T2 read" is irrelevant: the problem arises from the two writes alone.
  - id: "non-repeatable-read-trace"
    type: "single"
    prompt: "In PostgreSQL, `acct(id, bal)` holds `(1, 100)`. T1 runs `BEGIN ISOLATION LEVEL READ COMMITTED; SELECT bal FROM acct WHERE id = 1;`. T2 then runs `UPDATE acct SET bal = 200 WHERE id = 1` in autocommit mode. T1 runs the same `SELECT` again. What do T1's two reads return, and what is the phenomenon?"
    correct:
      - "100, then 200: a non-repeatable (fuzzy) read, P2"
    distractors:
      - "100, then 100: READ COMMITTED takes a snapshot for the whole transaction"
      - "100, then 200: a phantom, P3"
      - "100, then T1 blocks until T2's update is rolled back"
    explanation: |-
      P2 is `r1[x] ... w2[x] ... (c1 or a1)`: after T1 reads an item, another transaction modifies it, so a re-read within T1 sees a different value.

      | step | T1 (READ COMMITTED) | T2 | T1 sees |
      |---|---|---|---|
      | 1 | SELECT bal | | 100 |
      | 2 | | UPDATE bal = 200; commit | |
      | 3 | SELECT bal | | 200 |

      Under READ COMMITTED, PostgreSQL takes a new snapshot for *each statement*, so the second `SELECT` sees T2's committed update. Under `REPEATABLE READ`, the transaction keeps the snapshot from its first statement and the second read returns 100 (verified by running both schedules).

      "100, then 100" describes REPEATABLE READ, not READ COMMITTED.

      "A phantom" concerns rows entering or leaving a *predicate's* result set; here an existing row changed value.

      "T1 blocks" assumes readers wait for writers; in a multiversion engine, plain reads do not block on writes.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE acct(id INT PRIMARY KEY, bal INT); INSERT INTO acct VALUES (1, 100);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL READ COMMITTED"]
          - [1, "SELECT bal FROM acct WHERE id = 1"]
          - [2, "UPDATE acct SET bal = 200 WHERE id = 1"]
          - [1, "SELECT bal FROM acct WHERE id = 1"]
          - [1, "COMMIT"]
        expect: ["ok", [[100]], "ok", [[200]], "ok"]
      - engine: "schedule"
        setup: "CREATE TABLE acct(id INT PRIMARY KEY, bal INT); INSERT INTO acct VALUES (1, 100);"
        steps:
          - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [1, "SELECT bal FROM acct WHERE id = 1"]
          - [2, "UPDATE acct SET bal = 200 WHERE id = 1"]
          - [1, "SELECT bal FROM acct WHERE id = 1"]
          - [1, "COMMIT"]
        expect: ["ok", [[100]], "ok", [[100]], "ok"]
  - id: "phantom-count-trace"
    type: "single"
    prompt: "In PostgreSQL, `staff(name, dept)` has two rows in `'ops'`. T1 (`READ COMMITTED`) runs `SELECT COUNT(*) FROM staff WHERE dept = 'ops'`. T2 inserts `('eve', 'ops')` and commits. T1 repeats the count. What does T1 see, and what is the phenomenon?"
    correct:
      - "2, then 3: a phantom (P3), a new row entered the predicate's result"
    distractors:
      - "2, then 3: a non-repeatable read (P2) of the row `('eve', 'ops')`"
      - "2, then 2: a new row cannot appear in a running transaction"
      - "2, then an error, because the predicate's rows changed"
    explanation: |-
      P3 is `r1[P] ... w2[y in P] ... (c1 or a1)`: T1 reads the set of rows satisfying a predicate P, and T2 inserts (or deletes, or updates into or out of P) a row satisfying P. Re-evaluating P in T1 returns a different *set*.

      | step | T1 | T2 | count |
      |---|---|---|---|
      | 1 | COUNT WHERE dept = 'ops' | | 2 |
      | 2 | | INSERT ('eve', 'ops'); commit | |
      | 3 | COUNT WHERE dept = 'ops' | | 3 |

      "P2 of `('eve', 'ops')`" is wrong because T1 never read that row before; P2 concerns re-reading an item T1 already read. Phantoms are why item-level locks alone cannot provide serializability: you cannot lock a row that does not exist yet.

      "2, then 2" is the REPEATABLE READ outcome in PostgreSQL (its snapshot excludes later commits); at READ COMMITTED each statement sees new commits.

      "An error" confuses phantoms with serialization failures, which only arise when writes conflict at stricter levels.
    checks:
      - engine: "schedule"
        setup: "CREATE TABLE staff(name TEXT, dept TEXT); INSERT INTO staff VALUES ('ann','ops'),('bob','ops'),('cy','dev');"
        steps:
          - [1, "BEGIN ISOLATION LEVEL READ COMMITTED"]
          - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
          - [2, "INSERT INTO staff VALUES ('eve', 'ops')"]
          - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
          - [1, "COMMIT"]
        expect: ["ok", [[2]], "ok", [[3]], "ok"]
      - engine: "schedule"
        setup: "CREATE TABLE staff(name TEXT, dept TEXT); INSERT INTO staff VALUES ('ann','ops'),('bob','ops'),('cy','dev');"
        steps:
          - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
          - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
          - [2, "INSERT INTO staff VALUES ('eve', 'ops')"]
          - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
          - [1, "COMMIT"]
        expect: ["ok", [[2]], "ok", [[2]], "ok"]
  - id: "broad-versus-strict"
    type: "single"
    prompt: "History: `w1[x]`, `r2[x]`, `c2`, `c1`. T1 commits. Using Berenson et al.'s terms, which statement is correct?"
    correct:
      - "It exhibits the broad phenomenon P1 but not the strict anomaly A1, and the authors argue the isolation levels should forbid P1"
    distractors:
      - "It exhibits neither P1 nor A1, because T1 committed, so the read was not dirty"
      - "It exhibits A1 but not P1, because T2 committed before T1"
      - "It exhibits P0, because both transactions accessed `x` before either committed"
    explanation: |-
      Berenson et al. distinguish two readings of the ANSI phenomena:

      | Name | History pattern | Requires |
      |---|---|---|
      | A1 (strict, "anomaly") | `w1[x] ... r2[x] ... (a1 and c2 in any order)` | T1 aborts, T2 commits |
      | P1 (broad, "phenomenon") | `w1[x] ... r2[x] ... (c1 or a1)` | T2 reads before T1 ends, whatever the outcome |

      Here T2 read `x` before T1 finished, so P1 holds; T1 committed, so A1 does not. The paper argues for the broad interpretations: a history that happens to end well may still have been unsafe (T2 committed having read data that might have been rolled back), and only forbidding P1 rules out every non-serializable outcome of this kind.

      "Neither" applies the strict reading only.

      "A1 but not P1" is impossible: every A1 history is a P1 history.

      "P0" requires two writes.
  - id: "why-forbid-dirty-writes"
    type: "multiple"
    prompt: "Why do Berenson et al. require that *every* isolation level, even READ UNCOMMITTED, forbid dirty writes (P0)? Select all that apply."
    correct:
      - "Once two uncommitted writes interleave on an item, no before-image undo can roll back one transaction without corrupting the other's effect"
      - "Interleaved writes can break a constraint spanning items, such as `x = y`, even if no transaction ever reads"
    distractors:
      - "Dirty writes always cause deadlocks, which READ UNCOMMITTED cannot detect"
      - "The ANSI SQL-92 standard already lists P0 in its isolation table, and the paper only restates it"
      - "Multiversion engines are unable to store two uncommitted versions of the same row"
    explanation: |-
      The paper adds P0 because the ANSI SQL-92 table omits it, while any useful system must prevent it:

      - **Recovery:** with `w1[x] w2[x]`, undoing T1 by restoring its before-image erases T2's write, and undoing T2 instead restores T1's uncommitted value. Neither is correct (see the rollback trace in this section).
      - **Consistency without reads:** `w1[x=1] w2[x=2] w2[y=2] c2 w1[y=1] c1` leaves x = 2 and y = 1, breaking `x = y` although each transaction alone preserves it, and nobody read anything.

      "Always cause deadlocks" is false; dirty writes are about missing locks, not cycles of waiting.

      "ANSI already lists P0" reverses history: the critique points out its absence.

      "MVCC cannot store two uncommitted versions" is not the reason; engines simply refuse to let a second writer proceed (PostgreSQL blocks the second `UPDATE` until the first transaction ends, as the body's trace shows).
body_checks:
  - engine: "schedule"
    setup: "CREATE TABLE staff(name TEXT, dept TEXT); INSERT INTO staff VALUES ('ann','ops'),('bob','ops'),('cy','dev');"
    steps:
      - [1, "BEGIN ISOLATION LEVEL READ COMMITTED"]
      - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
      - [2, "INSERT INTO staff VALUES ('eve', 'ops')"]
      - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
      - [2, "UPDATE staff SET dept = 'dev' WHERE name = 'ann'"]
      - [1, "SELECT dept FROM staff WHERE name = 'ann'"]
      - [1, "COMMIT"]
    expect: ["ok", [[2]], "ok", [[3]], "ok", [["dev"]], "ok"]
  - engine: "schedule"
    setup: "CREATE TABLE staff(name TEXT, dept TEXT); INSERT INTO staff VALUES ('ann','ops'),('bob','ops'),('cy','dev');"
    steps:
      - [1, "BEGIN ISOLATION LEVEL REPEATABLE READ"]
      - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
      - [2, "INSERT INTO staff VALUES ('eve', 'ops')"]
      - [1, "SELECT COUNT(*) FROM staff WHERE dept = 'ops'"]
      - [2, "UPDATE staff SET dept = 'dev' WHERE name = 'ann'"]
      - [1, "SELECT dept FROM staff WHERE name = 'ann'"]
      - [1, "COMMIT"]
    expect: ["ok", [[2]], "ok", [[2]], "ok", [["ops"]], "ok"]
  - engine: "schedule"
    setup: "CREATE TABLE acct(id INT PRIMARY KEY, bal INT); INSERT INTO acct VALUES (1, 0);"
    steps:
      - [1, "BEGIN"]
      - [1, "UPDATE acct SET bal = 1 WHERE id = 1"]
      - [2, "UPDATE acct SET bal = 2 WHERE id = 1"]
      - [1, "ROLLBACK"]
    expect: ["ok", "ok", "blocked->ok", "ok"]
    final:
      query: "SELECT bal FROM acct WHERE id = 1"
      expect: [[2]]
---
## Learning objectives

- Define P0 (dirty write), P1 (dirty read), P2 (non-repeatable read) and P3 (phantom) as history patterns.
- Classify short interleaved schedules by phenomenon.
- Distinguish the broad (P) and strict (A) interpretations.
- Explain why dirty writes must be prevented at every isolation level.

## The invariant

A **history** (or schedule) is the interleaved sequence of operations of concurrent transactions: `r1[x]` (T1 reads x), `w2[y]` (T2 writes y), `c1` (commit), `a2` (abort), `r1[P]` (T1 reads the rows satisfying predicate P). A history is **serializable** if it is equivalent to some serial execution of the same transactions. Isolation levels weaker than serializable permit some non-serializable histories, and they are defined by which patterns they exclude.

ANSI SQL-92 defined three phenomena in English. Berenson, Bernstein, Gray, Melton, O'Neil and O'Neil (1995) formalized them and added P0:

| Phenomenon | Broad pattern | Plain meaning |
|---|---|---|
| P0 dirty write | `w1[x] ... w2[x] ... (c1 or a1)` | overwrite another transaction's uncommitted write |
| P1 dirty read | `w1[x] ... r2[x] ... (c1 or a1)` | read another transaction's uncommitted write |
| P2 fuzzy (non-repeatable) read | `r1[x] ... w2[x] ... (c1 or a1)` | an item you read is changed by someone else before you finish |
| P3 phantom | `r1[P] ... w2[y in P] ... (c1 or a1)` | the set of rows matching your predicate changes before you finish |

The strict forms A1, A2, A3 require the full anomaly to complete (for A1: T1 aborts and T2 commits; for A2: T1 re-reads and sees a different value; for A3: T1 re-evaluates P and gets a different set). The paper shows the strict readings are too weak to characterize the locking levels and argues for the broad ones.

Three layers again: the ANSI standard defines levels by the phenomena P1–P3 in prose; the theory (Berenson et al.) adds P0 and precise patterns; each engine prevents them with locks, snapshots or both.

## Worked example

Phantoms versus non-repeatable reads, on `staff(name, dept)` with ann and bob in `ops`:

| step | T1 | T2 | T1 at READ COMMITTED | T1 at PostgreSQL REPEATABLE READ |
|---|---|---|---|---|
| 1 | `SELECT COUNT(*) ... dept = 'ops'` | | 2 | 2 |
| 2 | | `INSERT ('eve', 'ops')`; commit | | |
| 3 | `SELECT COUNT(*) ... dept = 'ops'` | | 3 (phantom) | 2 |
| 4 | | `UPDATE staff SET dept = 'dev' WHERE name = 'ann'`; commit | | |
| 5 | `SELECT dept FROM staff WHERE name = 'ann'` | | dev (non-repeatable) | ops |

Dirty writes in a real engine: in PostgreSQL, T1 `UPDATE acct SET bal = 1 WHERE id = 1` (uncommitted), then T2 `UPDATE acct SET bal = 2 WHERE id = 1` **blocks**; when T1 rolls back, T2's update proceeds and commits, and the final balance is 2. The engine serializes writers on the row lock, so P0 never occurs.

## Where this shows up today

As of November 2026, the PostgreSQL 19 documentation states that dirty reads are not possible at any level (its READ UNCOMMITTED behaves like READ COMMITTED), whereas SQL Server and MySQL InnoDB implement READ UNCOMMITTED with dirty reads permitted. Every mainstream engine prevents dirty writes by holding write locks until the transaction ends.

## Pitfalls checklist

- Calling every "changed under me" bug a phantom; phantoms are about predicate result sets.
- Assuming READ COMMITTED gives a consistent view across statements; each statement gets its own.
- Treating a history as safe because it happened to commit cleanly (strict versus broad reading).
- Using READ UNCOMMITTED or `NOLOCK` hints for "faster reports" and reading data that is later rolled back.
- Forgetting that phantoms include rows updated into or out of the predicate, not only inserts.

## Key takeaways

- P0 dirty write, P1 dirty read, P2 fuzzy read and P3 phantom are history patterns over r, w, c, a.
- P2 is about re-reading an item; P3 is about re-evaluating a predicate.
- Berenson et al. argue for broad interpretations and add P0, which every level must prevent.
- READ COMMITTED permits P2 and P3; snapshot-based REPEATABLE READ in PostgreSQL prevents both for reads.

## Sources

- H. Berenson, P. Bernstein, J. Gray, J. Melton, E. O'Neil and P. O'Neil, "A Critique of ANSI SQL Isolation Levels", *ACM SIGMOD*, 1995.
- ISO/IEC 9075:1992 (SQL-92), `<set transaction statement>` isolation levels.
- PostgreSQL 19 documentation, "Transaction Isolation".
