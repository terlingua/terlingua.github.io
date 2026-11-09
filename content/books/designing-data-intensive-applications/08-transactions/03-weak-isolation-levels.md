---
title: "Weak Isolation Levels"
questions:
  - id: concurrency-hard
    prompt: "Why are concurrency bugs hard to find by testing?"
    options:
      - id: of1b83c
        text: "They are always caught by compilers"
      - id: o79e717
        text: "They require special hardware"
      - id: o04e648
        text: "They only happen in production databases"
      - id: o06c8b9
        text: "They appear only with unlucky timing and are hard to reproduce"
        correct: true
    explanation: "“Concurrency bugs are hard to find by testing, because such bugs are triggered only when you get unlucky with the timing.”"
  - id: acid-point
    prompt: "\"Use an ACID database for financial data!\" Why does the chapter say this misses the point?"
    options:
      - id: o37a310
        text: "Many ACID databases use weak isolation by default and would not necessarily prevent these bugs"
        correct: true
      - id: of0c48b
        text: "ACID databases cannot store money"
      - id: oeeb5fc
        text: "ACID is only about durability"
      - id: o1535fb
        text: "Financial systems never use databases"
    explanation: "“Even many popular relational database systems (which are usually considered ACID) use weak isolation, so they wouldn’t necessarily have prevented these bugs from occurring.”"
  - id: attacker
    prompt: "Why must race conditions be systematically prevented, not just made rare?"
    options:
      - id: of81e49
        text: "An attacker can send bursts of concurrent requests to exploit them"
        correct: true
      - id: oa65fa6
        text: "Rare races cost more CPU"
      - id: o08d1a7
        text: "Compliance auditors require it"
      - id: off19bd
        text: "Databases crash on races"
    explanation: "“you have to consider the possibility that an attacker might deliberately send a burst of highly concurrent requests to your API in an attempt to exploit concurrency bugs”."
  - id: rc-guarantees
    prompt: "Which two guarantees define read committed?"
    type: multiple
    options:
      - id: oe5502a
        text: "No dirty reads"
        correct: true
      - id: o019d17
        text: "No write skew"
      - id: o2b3d0c
        text: "No read skew"
      - id: o3f206f
        text: "No dirty writes"
        correct: true
    explanation: "“When reading from the database, you will see only data that has been committed (no dirty reads). When writing to the database, you will overwrite only data that has been committed (no dirty writes).”"
  - id: cascading-aborts
    prompt: "Why are dirty reads dangerous if transactions abort?"
    options:
      - id: o198ca3
        text: "Readers may act on data that is rolled back, requiring cascading aborts"
        correct: true
      - id: o1949b2
        text: "They slow down writes"
      - id: od7eb85
        text: "They violate durability"
      - id: o0eca6e
        text: "They corrupt indexes"
    explanation: "“a transaction may see data that is later rolled back ... Any transaction that read uncommitted data would also need to be aborted, leading to a problem called cascading aborts.”"
  - id: car-sale
    prompt: "In the used-car example, Bryce wins the listing and Aaliyah gets the invoice. What prevents this?"
    options:
      - id: o0f72c0
        text: "Preventing dirty writes, as read committed does"
        correct: true
      - id: o2ebcdf
        text: "Read uncommitted"
      - id: o133b63
        text: "Snapshot isolation only"
      - id: oe4f65b
        text: "A Bloom filter"
    explanation: "“the sale is awarded to Bryce ... but the invoice is sent to Aaliyah ... Read-committed isolation prevents such mishaps.”"
  - id: rc-counter
    prompt: "Does read committed prevent the lost counter increment of Figure 8-1?"
    options:
      - id: oa52fea
        text: "Only on PostgreSQL"
      - id: oc3a624
        text: "Yes — it locks reads"
      - id: odb6f73
        text: "Yes — it prevents all write conflicts"
      - id: o4c826f
        text: "No — the second write happens after the first commits, so it's not a dirty write"
        correct: true
    explanation: "“read-committed isolation does not prevent the race condition between two counter increments ... the second write happens after the first transaction has committed, so it’s not a dirty write.”"
  - id: read-locks
    prompt: "Why do most databases avoid read locks to prevent dirty reads?"
    options:
      - id: o02f20f
        text: "Read locks cause dirty writes"
      - id: ofeeb6f
        text: "One long write transaction would block many readers, harming latency and operability"
        correct: true
      - id: o10462a
        text: "Read locks are impossible to implement"
      - id: o87825a
        text: "They violate durability"
    explanation: "“one long-running write transaction can force many other transactions to wait ... a slowdown in one part of an application can have a knock-on effect in a completely different part”."
  - id: rc-impl
    prompt: "How do most databases prevent dirty reads?"
    options:
      - id: o4492df
        text: "They remember the old committed value and give it to readers until the writer commits"
        correct: true
      - id: o687dcb
        text: "They lock every row on read"
      - id: o0702be
        text: "They copy the whole table"
      - id: o67d94b
        text: "They delay reads until all writes finish"
    explanation: "“For every row that is written, the database remembers both the old committed value and the new value set by the transaction that currently holds the write lock. While the transaction is ongoing, any other transactions that read the row are simply given the old value.”"
  - id: read-skew
    prompt: "Aaliyah sees $500 + $400 = $900 during a transfer between her accounts. What is this, and is it allowed under read committed?"
    options:
      - id: o2c796f
        text: "A dirty read; no"
      - id: oa91d5b
        text: "Read skew (a nonrepeatable read); yes, it is acceptable under read committed"
        correct: true
      - id: o829f5b
        text: "Write skew; yes"
      - id: ocf93ed
        text: "A lost update; no"
    explanation: "“This anomaly is called read skew, and it is an example of a nonrepeatable read ... Read skew is considered acceptable under read-committed isolation”."
  - id: read-skew-matters
    prompt: "Where is read skew not tolerable?"
    type: multiple
    options:
      - id: o8f4948
        text: "A single-key lookup"
      - id: o2553e9
        text: "Backups that copy the database over hours"
        correct: true
      - id: oafdecf
        text: "Long analytical queries and integrity checks"
        correct: true
      - id: o999233
        text: "A user refreshing a page seconds later"
    explanation: "The chapter names backups — “If you need to restore from such a backup, the inconsistencies (such as disappearing money) become permanent” — and “Analytical queries and integrity checks” that “are likely to return nonsensical results”."
  - id: mvcc-principle
    prompt: "What is the key performance principle of snapshot isolation?"
    options:
      - id: ocd3295
        text: "All transactions run serially"
      - id: o79b580
        text: "Writers never block writers"
      - id: o13d0e9
        text: "Readers always block writers"
      - id: odbf7f2
        text: "Readers never block writers, and writers never block readers"
        correct: true
    explanation: "“a key principle of snapshot isolation is readers never block writers, and writers never block readers.”"
  - id: mvcc-versions
    prompt: "Why does snapshot isolation need potentially many versions of a row, not just two?"
    options:
      - id: o21dac7
        text: "To support replication"
      - id: o9b35f9
        text: "To prevent dirty writes"
      - id: ob6d30c
        text: "Different in-progress transactions may need to see the database at different points in time"
        correct: true
      - id: o3f43f3
        text: "To compress data"
    explanation: "“the database must potentially keep several committed versions of a row, because various in-progress transactions may need to see the state of the database at different points in time.”"
  - id: visibility
    prompt: "Under PostgreSQL-style MVCC, when is a row version visible to a reader?"
    options:
      - id: o3693d6
        text: "Its inserting transaction committed before the reader started, and it isn't deleted by a transaction committed before the reader started"
        correct: true
      - id: ob8d4f7
        text: "Its transaction ID is even"
      - id: o64d3bb
        text: "It was inserted most recently"
      - id: o24758f
        text: "It is in the buffer cache"
    explanation: "A row is visible if “the transaction that inserted the row had already committed” when the reader started, and it is not deleted or “the transaction that requested deletion had not yet committed at the time when the reader’s transaction started.”"
  - id: visibility-aborted
    prompt: "Why don't rows written by aborted transactions need immediate removal?"
    options:
      - id: ob697cc
        text: "Visibility rules filter them out, and garbage collection removes them later"
        correct: true
      - id: o792d3c
        text: "They are visible to everyone"
      - id: o5c3844
        text: "Aborted transactions write to a separate file"
      - id: o46ecd9
        text: "They are never written"
    explanation: "“Any writes made by aborted transactions are ignored ... we don’t need to immediately remove the rows it wrote from storage, since the visibility rule filters them out.”"
  - id: update-internal
    prompt: "How is an UPDATE represented internally in PostgreSQL's MVCC?"
    options:
      - id: o724b43
        text: "As an in-place overwrite"
      - id: o8a6cc4
        text: "As a log entry only"
      - id: oc04c94
        text: "As a delete of the old version and an insert of a new version"
        correct: true
      - id: o99e486
        text: "As two separate transactions"
    explanation: "“An update is internally translated into a delete and an insert.”"
  - id: immutable-btree
    prompt: "How do CouchDB, Datomic and LMDB provide snapshots without visibility rules?"
    options:
      - id: o0f75e7
        text: "Immutable copy-on-write B-trees: each write creates a new root, and each root is a consistent snapshot"
        correct: true
      - id: o48bdb7
        text: "They store only the latest version"
      - id: o0fcdc3
        text: "They copy the full database per transaction"
      - id: o2f10cc
        text: "They lock the whole database during reads"
    explanation: "“With immutable B-trees, every write transaction (or batch of transactions) creates a new B-tree root, and a particular root is a consistent snapshot of the database”."
  - id: rr-naming
    prompt: "Why does \"repeatable read\" mean different things in different databases?"
    options:
      - id: o612c54
        text: "It refers to replication"
      - id: o1bab13
        text: "It is a trademark"
      - id: o29fdba
        text: "The SQL standard predates snapshot isolation and its definitions are ambiguous, so vendors map the name differently"
        correct: true
      - id: o716786
        text: "All databases implement the same thing"
    explanation: "“the SQL standard doesn’t have the concept of snapshot isolation ... the SQL standard’s definition of isolation levels is flawed ... As a result, nobody really knows what repeatable read isolation means.”"
  - id: rr-examples
    prompt: "Which naming facts are correct?"
    type: multiple
    options:
      - id: o0103db
        text: "MySQL's repeatable read is full serializability"
      - id: ob82eb2
        text: "PostgreSQL's repeatable read is snapshot isolation"
        correct: true
      - id: oe43d7f
        text: "IBM Db2's repeatable read means serializability"
        correct: true
      - id: o2f52ad
        text: "Oracle calls snapshot isolation \"serializable\""
        correct: true
    explanation: "“snapshot isolation is called “repeatable read” in PostgreSQL and “serializable” in Oracle”, MySQL's is “an implementation of MVCC with weaker consistency than snapshot isolation”, and “IBM Db2 uses “repeatable read” to refer to serializability”."
  - id: lost-update-pattern
    prompt: "What pattern causes a lost update?"
    options:
      - id: o61b1a8
        text: "Two reads of the same row"
      - id: oab8321
        text: "A write to a different row"
      - id: o1b70bf
        text: "Two concurrent read-modify-write cycles, where the second write doesn't include the first's change"
        correct: true
      - id: of0fce0
        text: "Deleting and reinserting a row"
    explanation: "“The lost update problem can occur if an application reads a value from the database, modifies it, and writes back the modified value ... If two transactions do this concurrently, one of the modifications can be lost”."
  - id: atomic-op
    prompt: "Which statement is concurrency-safe in most relational databases?"
    options:
      - id: o391fce
        text: "Reading value into the app, adding 1, then `UPDATE … SET value = 43`"
      - id: ob38b51
        text: "`SELECT value FROM counters` twice"
      - id: o2471b8
        text: "Updating inside a read-only transaction"
      - id: oc0cf9c
        text: "`UPDATE counters SET value = value + 1 WHERE key = 'foo'`"
        correct: true
    explanation: "The chapter gives this statement as “concurrency-safe in most relational databases” and says atomic operations “are usually the best solution if your code can be expressed in terms of those operations.”"
  - id: orm-unsafe
    prompt: "Why do ORMs often cause lost updates?"
    options:
      - id: o32fa81
        text: "They use 2PL"
      - id: oc3083c
        text: "They disable transactions"
      - id: o17a61e
        text: "They use LWW"
      - id: o060189
        text: "They make it easy to write read-modify-write cycles in application code instead of atomic database operations"
        correct: true
    explanation: "“ORM frameworks make it easy to accidentally write code that performs unsafe read-modify-write cycles instead of using atomic operations provided by the database.”"
  - id: for-update
    prompt: "In the board game example, why use `SELECT … FOR UPDATE` instead of an atomic operation?"
    options:
      - id: o5fdc16
        text: "FOR UPDATE prevents write skew in all cases"
      - id: oe04e6e
        text: "Atomic operations are slower"
      - id: oea954c
        text: "The database lacks UPDATE"
      - id: o75514e
        text: "Validating a move needs application logic that can't be expressed as a single database operation"
        correct: true
    explanation: "“an atomic operation may not be sufficient, because the application also needs to ensure that a player’s move abides by the rules of the game, which involves some logic that you cannot sensibly implement as a database query.”"
  - id: lost-detect
    prompt: "Which databases automatically detect lost updates under snapshot-style isolation?"
    type: multiple
    options:
      - id: o5f494f
        text: "PostgreSQL repeatable read"
        correct: true
      - id: oc20b59
        text: "SQL Server snapshot isolation"
        correct: true
      - id: oc035ad
        text: "Oracle serializable"
        correct: true
      - id: o36e70a
        text: "MySQL/InnoDB repeatable read"
    explanation: "“PostgreSQL’s repeatable read, Oracle’s serializable, and SQL Server’s snapshot isolation levels automatically detect when a lost update has occurred ... However, MySQL/InnoDB’s repeatable read isolation level does not detect lost updates”."
  - id: detection-benefit
    prompt: "What is the big advantage of automatic lost-update detection?"
    options:
      - id: o4778ef
        text: "It never aborts transactions"
      - id: o6fa9fc
        text: "It prevents write skew too"
      - id: o833332
        text: "It is faster than any lock"
      - id: oe3d433
        text: "It requires no special code, so forgetting a lock or atomic op doesn't cause a bug"
        correct: true
    explanation: "“A big advantage of lost update detection is that it doesn’t require application code to use any special database features ... lost update detection happens automatically and is thus less error-prone.”"
  - id: optimistic-lock
    prompt: "What is \"optimistic locking\" with a version column?"
    options:
      - id: o05c092
        text: "Locking all rows optimistically at start"
      - id: o569147
        text: "Retrying until a lock is acquired"
      - id: o53162d
        text: "Using 2PL with short timeouts"
      - id: oc795fd
        text: "Increment a version on every update and apply an update only if the version is unchanged since the read"
        correct: true
    explanation: "“you could also use a version number column that you increment on every update and apply the update only if the current version number hasn’t changed. This approach is sometimes called optimistic locking”."
  - id: replication-locks
    prompt: "Why don't locks or conditional writes prevent lost updates in multi-leader or leaderless databases?"
    options:
      - id: of345ad
        text: "Conditional writes are always lost"
      - id: oa775cd
        text: "Replication disables transactions"
      - id: o57aebf
        text: "These databases have no locks"
      - id: o78d102
        text: "They assume a single up-to-date copy, which these systems don't guarantee"
        correct: true
    explanation: "“Locks and conditional write operations assume that there is a single up-to-date copy of the data ... databases with multi-leader or leaderless replication ... cannot guarantee a single up-to-date copy”."
  - id: commutative
    prompt: "Which operations can be merged without lost updates in replicated systems?"
    type: multiple
    options:
      - id: oe28daa
        text: "Conditional writes"
      - id: ob3d170
        text: "Adding an element to a set"
        correct: true
      - id: o12fbd3
        text: "LWW overwrites"
      - id: o9792b2
        text: "Incrementing a counter"
        correct: true
    explanation: "“incrementing a counter and adding an element to a set are commutative operations ... However, some operations, such as conditional writes, cannot be made commutative. In addition, the LWW conflict resolution method ... is prone to lost updates”."
  - id: write-skew-doctors
    prompt: "Two on-call doctors each check that two are on call, then each goes off call. Why is this neither a dirty write nor a lost update?"
    options:
      - id: o2a810d
        text: "They update the same row"
      - id: oc426a9
        text: "Snapshot isolation prevents it"
      - id: ocd4cc2
        text: "One of them never commits"
      - id: o99dcd2
        text: "They update different objects; the conflict is in the premise they both read"
        correct: true
    explanation: "“It is neither a dirty write nor a lost update, because the two transactions are updating two objects ... if the two transactions had run one after another, the second doctor would have been prevented from going off call.”"
  - id: write-skew-general
    prompt: "How does write skew relate to lost updates?"
    options:
      - id: o7419fe
        text: "They are unrelated"
      - id: oe0d295
        text: "It is a special case of lost updates"
      - id: o4249d8
        text: "It only affects reads"
      - id: ob30b0c
        text: "It generalizes them: two transactions read the same objects and update some, possibly different, objects"
        correct: true
    explanation: "“You can think of write skew as a generalization of the lost-update problem. Write skew can occur if two transactions read the same objects and then update some of those objects”."
  - id: write-skew-options
    prompt: "Which options help against write skew?"
    type: multiple
    options:
      - id: oa030a2
        text: "Atomic single-object operations"
      - id: obc9ea3
        text: "Lost-update detection in snapshot isolation"
      - id: o6999f0
        text: "True serializable isolation"
        correct: true
      - id: ocb700e
        text: "Explicitly locking the rows the decision depends on (FOR UPDATE)"
        correct: true
    explanation: "“Atomic single-object operations don’t help”, lost-update detection “doesn’t help either”, and “Automatically preventing write skew requires true serializable isolation”. Otherwise, “explicitly lock the rows that the transaction depends on”."
  - id: username
    prompt: "Two users try to register the same username concurrently. What is the simple fix?"
    options:
      - id: o1f0e96
        text: "A uniqueness constraint, which aborts the second transaction"
        correct: true
      - id: o66403e
        text: "Snapshot isolation"
      - id: o074811
        text: "LWW"
      - id: o007962
        text: "Read committed"
    explanation: "“Fortunately, a uniqueness constraint is a simple solution here (the second transaction that tries to register the username will be aborted for violating the constraint).”"
  - id: phantom-def
    prompt: "What is a phantom?"
    options:
      - id: oa80abf
        text: "An uncommitted insert visible to others"
      - id: oe50126
        text: "A row that is deleted but still visible"
      - id: o8d910e
        text: "A write in one transaction changing the result of a search query in another"
        correct: true
      - id: od24303
        text: "A dirty read"
    explanation: "“This effect, where a write in one transaction changes the result of a search query in another transaction, is called a phantom”."
  - id: phantom-lock
    prompt: "Why can't `SELECT … FOR UPDATE` prevent double-booking a meeting room?"
    options:
      - id: oadda5e
        text: "It only works for updates"
      - id: odac663
        text: "It locks too many rows"
      - id: oee4261
        text: "FOR UPDATE is not supported on bookings"
      - id: oc59c1b
        text: "The check looks for the absence of rows; if none exist, there is nothing to lock"
        correct: true
    explanation: "“they check for the absence of rows matching a search condition, and the write adds a row matching the same condition. If the query in step 1 doesn’t return any rows, SELECT FOR UPDATE can’t attach locks to anything”."
  - id: materialize
    prompt: "What is \"materializing conflicts\", and why is it a last resort?"
    options:
      - id: ob58f6d
        text: "Storing conflicts as siblings"
      - id: o0cb4bf
        text: "Creating materialized views of conflicts"
      - id: oe68195
        text: "Pre-creating rows (e.g., room–time slots) to lock; it's error-prone and leaks concurrency control into the data model"
        correct: true
      - id: o486577
        text: "Logging conflicts for later review"
    explanation: "It “takes a phantom and turns it into a lock conflict on a concrete set of rows ... it can be hard and error-prone ... and it’s ugly to let a concurrency control mechanism leak into the application data model.”"
---
Read committed, snapshot isolation, lost updates, write skew and phantoms — what each level prevents
and what it leaves to you.

## Learning objectives

- Explain dirty reads and dirty writes and how read committed prevents them
- Explain read skew and how MVCC snapshot isolation prevents it
- Navigate the "repeatable read" naming mess
- Prevent lost updates with atomic operations, locks, detection or CAS
- Recognize write skew and phantoms and the options for preventing them

## In a nutshell

| Anomaly | Prevented by |
|---|---|
| Dirty read / dirty write | Read committed |
| Read skew | Snapshot isolation (MVCC) |
| Lost update | Atomic ops, `FOR UPDATE`, auto-detection (some SI), CAS |
| Write skew / phantoms | Serializable (or locks, materialized conflicts) |

- Readers never block writers and writers never block readers under MVCC.
- An attacker can trigger rare races deliberately.

## Connections to today

- **PostgreSQL defaults to read committed**; many apps run there unaware of lost updates and write skew.
- **EF Core concurrency tokens** (`[Timestamp]`/rowversion) implement optimistic locking — the chapter's
  CAS with a version column.
- **Double-spend bugs** in crypto exchanges and fintech apps are textbook write skew.
