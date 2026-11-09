---
title: "Serializability"
questions:
  - id: sad-situation
    prompt: "Which reasons does the chapter give for the \"sad situation\" of weak isolation?"
    type: multiple
    options:
      - id: of7df55
        text: "Serializable isolation is not defined"
      - id: o53bc08
        text: "Isolation levels are hard to understand and inconsistently implemented"
        correct: true
      - id: oc82f5c
        text: "There are no good tools to detect race conditions"
        correct: true
      - id: o12403f
        text: "It's hard to tell from code whether it's safe at a given level"
        correct: true
    explanation: "The three bullets: isolation levels are “hard to understand and inconsistently implemented”, it “can be difficult to tell by looking at the application code”, and “There are no good tools to help us detect race conditions.”"
  - id: serial-why-now
    prompt: "What two developments made single-threaded serial execution viable in the 2000s?"
    type: multiple
    options:
      - id: ob83d06
        text: "Networks became faster than RAM"
      - id: ob15426
        text: "RAM became cheap enough to keep the active dataset in memory"
        correct: true
      - id: oa48cbc
        text: "OLTP transactions are usually short with few reads and writes"
        correct: true
      - id: o26ef87
        text: "CPUs became multicore"
    explanation: "“RAM became cheap enough” and “Database designers realized that OLTP transactions are usually short and make only a small number of reads and writes”."
  - id: interactive
    prompt: "Why do serial-execution systems disallow interactive multi-statement transactions?"
    options:
      - id: o94b245
        text: "The single thread would mostly wait on network round trips to the application"
        correct: true
      - id: o0594bd
        text: "They use too much memory"
      - id: o287d1b
        text: "Interactive transactions are not ACID"
      - id: oa31271
        text: "SQL does not support them"
    explanation: "“In this interactive style of transaction, a lot of time is spent in network communication ... the throughput would be dreadful because the database would spend most of its time waiting for the application”."
  - id: stored-proc-cons
    prompt: "Which criticisms of stored procedures does the chapter list?"
    type: multiple
    options:
      - id: of731c7
        text: "They cannot read data"
      - id: o2a3a6f
        text: "Hard to debug, version, test and monitor"
        correct: true
      - id: o39b52b
        text: "Archaic vendor-specific languages"
        correct: true
      - id: o43a1c0
        text: "Running untrusted tenant code in the database is a security risk"
        correct: true
      - id: o9851b4
        text: "A bad procedure can hurt a shared database more than bad app code"
        correct: true
    explanation: "All four are listed. Modern systems use general-purpose languages: “VoltDB uses Java or Groovy, Datomic uses Java or Clojure, Redis uses Lua, and MongoDB uses JavaScript.”"
  - id: voltdb-repl
    prompt: "How does VoltDB replicate, and what does that require?"
    options:
      - id: o9aa2e1
        text: "It uses LWW"
      - id: o70a32c
        text: "It executes the same stored procedure on each replica, so procedures must be deterministic"
        correct: true
      - id: o363ac7
        text: "It ships the WAL"
      - id: oee9407
        text: "It uses 2PC per write"
    explanation: "“Instead of copying a transaction’s writes from one node to another, it executes the same stored procedure on each replica. VoltDB therefore requires that stored procedures are deterministic”."
  - id: cross-shard-serial
    prompt: "What throughput does VoltDB report for cross-shard writes?"
    options:
      - id: oe24922
        text: "Unlimited with enough nodes"
      - id: oa58757
        text: "About 1 million per second"
      - id: o3ff535
        text: "The same as single-shard"
      - id: o0ebfce
        text: "About 1,000 per second, not increasable by adding machines"
        correct: true
    explanation: "“VoltDB reports a throughput of about 1,000 cross-shard writes per second, which is orders of magnitude below its single-shard throughput and cannot be increased by adding more machines”."
  - id: serial-constraints
    prompt: "Which are constraints of serial execution?"
    type: multiple
    options:
      - id: o3537d4
        text: "Write throughput must fit one core, or sharding without cross-shard coordination is needed"
        correct: true
      - id: o5b5882
        text: "Transactions must be interactive"
      - id: obce363
        text: "Every transaction must be small and fast"
        correct: true
      - id: o783271
        text: "The active dataset should fit in memory"
        correct: true
    explanation: "The summary lists all three, plus “Cross-shard transactions are possible, but their throughput is hard to scale.”"
  - id: 2pl-2pc
    prompt: "How do 2PL and 2PC differ?"
    options:
      - id: o6556ca
        text: "They are the same protocol"
      - id: o1e8a5f
        text: "2PL is for distributed systems only"
      - id: ob5e8f0
        text: "2PC prevents deadlocks"
      - id: o52c1de
        text: "2PL provides serializable isolation; 2PC provides atomic commit across nodes"
        correct: true
    explanation: "“2PL provides serializable isolation, whereas 2PC provides atomic commit in a distributed database ... it’s best to think of them as entirely separate concepts”."
  - id: 2pl-blocking
    prompt: "How does 2PL differ from snapshot isolation in who blocks whom?"
    options:
      - id: o61dfdb
        text: "They behave identically"
      - id: ob80be9
        text: "In 2PL nobody blocks anybody"
      - id: oe4f99e
        text: "In 2PL writers block readers and readers block writers"
        correct: true
      - id: o0e7050
        text: "In 2PL only writers block writers"
    explanation: "“In 2PL, writers don’t just block other writers; they also block readers, and vice versa.”"
  - id: 2pl-phases
    prompt: "Where does the name \"two-phase\" come from?"
    options:
      - id: oe0a023
        text: "Prepare and commit phases"
      - id: o4b6095
        text: "Two replicas"
      - id: ob97aa6
        text: "A growing phase acquiring locks during execution, and a shrinking phase releasing all locks at the end"
        correct: true
      - id: oec190e
        text: "Read and write phases"
    explanation: "“the first phase (the growing phase, while the transaction is executing) is when the locks are acquired, and the second phase (the shrinking phase, at the end of the transaction) is when all the locks are released.”"
  - id: 2pl-perf
    prompt: "Why can 2PL make a database effectively unavailable for writes?"
    options:
      - id: ob3666c
        text: "2PL disables writes on weekends"
      - id: o2c8cc5
        text: "Deadlocks are impossible to detect"
      - id: oc88e9d
        text: "2PL requires serial execution"
      - id: o9ef344
        text: "A long read of a whole table takes a shared lock that blocks all writers until it commits"
        correct: true
    explanation: "“while the whole table is being read ... all other transactions that want to write to that table are blocked until the big read-only transaction commits. In effect, the database becomes unavailable for writes for an extended time.”"
  - id: 2pl-latency
    prompt: "What happens to latency under 2PL with contention?"
    options:
      - id: o6637c8
        text: "It becomes unstable and very slow at high percentiles; one slow transaction can stall the system"
        correct: true
      - id: o7e62d7
        text: "It becomes perfectly stable"
      - id: o8fb3fb
        text: "Only throughput suffers"
      - id: oeee11c
        text: "It improves because of locking"
    explanation: "“databases running 2PL can have quite unstable latencies, and they can be very slow at high percentiles ... if there is contention in the workload.”"
  - id: predicate
    prompt: "What makes a predicate lock different from a row lock?"
    options:
      - id: o9cee69
        text: "It applies to all objects matching a search condition, including ones that don't exist yet"
        correct: true
      - id: odc1921
        text: "It locks the entire database"
      - id: o347c60
        text: "It locks a single row exclusively"
      - id: o923433
        text: "It is held only during reads"
    explanation: "“The key idea here is that a predicate lock applies even to objects that do not yet exist in the database, but that might be added in the future (phantoms).”"
  - id: index-range
    prompt: "Why do databases use index-range locks instead of predicate locks?"
    options:
      - id: oa62fe2
        text: "Predicate locks are slow to check; locking a broader index range is a safe, cheaper approximation"
        correct: true
      - id: o30eca8
        text: "Predicate locks cause write skew"
      - id: o9a10ed
        text: "Index-range locks are more precise"
      - id: o577846
        text: "Index-range locks avoid deadlocks"
    explanation: "“predicate locks do not perform well ... most databases with 2PL implement index-range locking ... which is a simplified approximation of predicate locking. It’s safe to simplify a predicate by making it match a greater set of objects.”"
  - id: no-index
    prompt: "If no suitable index exists for a range lock, what can the database do?"
    options:
      - id: o592e38
        text: "Skip locking"
      - id: o70814d
        text: "Fall back to a shared lock on the entire table — safe but bad for performance"
        correct: true
      - id: o4d234f
        text: "Use LWW"
      - id: o77f363
        text: "Abort every transaction"
    explanation: "“the database can fall back to a shared lock on the entire table. This will not be good for performance ... but it’s a safe fallback position.”"
  - id: optimistic-vs-pessimistic
    prompt: "When does optimistic concurrency control (like SSI) perform poorly?"
    options:
      - id: o99205b
        text: "Under high contention, where many transactions abort and retries add load"
        correct: true
      - id: o531030
        text: "When there is spare capacity"
      - id: oe9cad5
        text: "When contention is low"
      - id: ob45f2c
        text: "When transactions are short"
    explanation: "“It performs badly if there is high contention ... However, if there is enough spare capacity, and if contention between transactions is not too high, optimistic concurrency control techniques tend to perform better than pessimistic ones.”"
  - id: serial-pessimistic
    prompt: "How does the chapter characterize serial execution?"
    options:
      - id: of6e2e2
        text: "Equivalent to SSI"
      - id: oebf458
        text: "Pessimistic to the extreme — like an exclusive lock on the whole database per transaction"
        correct: true
      - id: o93f700
        text: "Neither"
      - id: o88043c
        text: "Optimistic"
    explanation: "“Serial execution is, in a sense, pessimistic to the extreme; it is essentially equivalent to each transaction having an exclusive lock on the entire database”."
  - id: premise
    prompt: "What does SSI need to detect?"
    options:
      - id: o4a56a7
        text: "Transactions that acted on an outdated premise, by tracking stale MVCC reads and writes affecting prior reads"
        correct: true
      - id: o3add32
        text: "Network partitions"
      - id: o43e688
        text: "Dirty writes only"
      - id: o03d5a1
        text: "Clock skew"
    explanation: "“the database must detect situations in which a transaction may have acted on an outdated premise and abort the transaction”. The two cases: “Detecting reads of a stale MVCC object version” and “Detecting writes that affect prior reads”."
  - id: ssi-wait
    prompt: "Why does SSI wait until commit rather than aborting immediately on a stale read?"
    options:
      - id: o36113b
        text: "Aborts are only allowed at commit"
      - id: o03f291
        text: "Stale reads are always harmless"
      - id: o8e4a86
        text: "It saves memory"
      - id: o365053
        text: "The transaction might be read-only, or the other transaction might abort, so aborting early could be unnecessary"
        correct: true
    explanation: "“if transaction 43 was a read-only transaction, it wouldn’t need to be aborted ... transaction 42 may yet abort ... By avoiding unnecessary aborts, SSI preserves snapshot isolation’s support for long-running reads”."
  - id: ssi-tripwire
    prompt: "How do SSI's \"locks\" on read ranges differ from 2PL locks?"
    options:
      - id: o0effc9
        text: "They don't block; they act as tripwires notifying transactions that their reads may be outdated"
        correct: true
      - id: oc1e460
        text: "They are exclusive locks"
      - id: o30868a
        text: "They block writers until readers commit"
      - id: o3cf3c2
        text: "They block readers"
    explanation: "“rather than blocking until the readers have committed, the lock acts as a tripwire; it simply notifies the transactions that the data they read may no longer be up-to-date.”"
  - id: ssi-advantages
    prompt: "Which are advantages of SSI?"
    type: multiple
    options:
      - id: o991573
        text: "Not limited to one CPU core; FoundationDB distributes conflict detection"
        correct: true
      - id: oab29ed
        text: "It never aborts transactions"
      - id: o98d76b
        text: "Read-only queries run on a snapshot without locks"
        correct: true
      - id: oe188af
        text: "No blocking on locks, so latency is more predictable"
        correct: true
    explanation: "“one transaction doesn’t need to block waiting for locks”, “serializable snapshot isolation is not limited to the throughput of a single CPU core”, and “read-only queries can run on a consistent snapshot without requiring any locks”."
  - id: ssi-short
    prompt: "What does SSI require of read/write transactions?"
    options:
      - id: o660114
        text: "They should be fairly short, because long ones are likely to conflict and abort"
        correct: true
      - id: o9670e8
        text: "They must be single-statement"
      - id: o61b27e
        text: "They must be stored procedures"
      - id: o9ab183
        text: "They must avoid indexes"
    explanation: "“a transaction that reads and writes data over a long period of time is likely to run into conflicts and abort, so SSI requires that read/write transactions be fairly short (long-running read-only transactions are OK).”"
---
Serializable isolation prevents all race conditions. Three implementations: actual serial execution,
two-phase locking, and serializable snapshot isolation.

## Learning objectives

- Explain when single-threaded serial execution works and its constraints
- Describe 2PL locks, deadlocks, predicate and index-range locks, and 2PL's performance problems
- Contrast pessimistic and optimistic concurrency control
- Explain how SSI detects stale reads and writes that affect prior reads

## In a nutshell

- Serial execution: in-memory data, short stored procedures, single-shard transactions.
- 2PL: shared/exclusive locks held to the end; readers and writers block each other; unstable latency.
- Index-range (next-key) locks approximate predicate locks to stop phantoms.
- SSI: snapshot isolation plus conflict detection at commit; optimistic, so contention hurts.

## Connections to today

- **PostgreSQL SERIALIZABLE** uses SSI; CockroachDB defaults to serializable.
- **FoundationDB** scales SSI-style conflict checking across machines.
- **Redis Lua scripts and functions** are stored procedures executed serially.
