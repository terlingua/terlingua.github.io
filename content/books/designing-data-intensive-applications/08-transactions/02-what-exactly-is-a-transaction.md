---
title: "What Exactly Is a Transaction?"
questions:
  - id: why-transactions
    prompt: "What is the main purpose of transactions according to the chapter?"
    options:
      - id: oa451dd
        text: "To enforce schemas"
      - id: o00ecd3
        text: "To simplify the programming model by letting applications ignore certain errors and concurrency issues"
        correct: true
      - id: o13d2a1
        text: "To replace backups"
      - id: ocacb3f
        text: "To make databases faster"
    explanation: "“Transactions are not a law of nature; they were created with a purpose—namely, to simplify the programming model for applications accessing a database.”"
  - id: horizon-acid
    prompt: "What does the chapter suggest was the technical cause behind the Post Office Horizon scandal?"
    options:
      - id: o1e31e5
        text: "Clock skew"
      - id: oa94b00
        text: "A Bloom filter false positive"
      - id: oa25675
        text: "Probably a lack of ACID transactions in the accounting system"
        correct: true
      - id: oc93d56
        text: "A network partition"
    explanation: "“the technical cause behind the Post Office Horizon scandal ... was probably a lack of ACID transactions in the underlying accounting system”."
  - id: nosql-belief
    prompt: "What popular belief from the NoSQL era turned out to be wrong?"
    options:
      - id: oc2a93a
        text: "That SQL is declarative"
      - id: oa840e0
        text: "That sharding is useful"
      - id: o6e2054
        text: "That transactions are fundamentally unscalable"
        correct: true
      - id: o82edb3
        text: "That replication improves availability"
    explanation: "“The hype around NoSQL distributed databases led to a popular belief that transactions were fundamentally unscalable ... More recently, that belief has turned out to be wrong.”"
  - id: acid-marketing
    prompt: "Why is \"ACID compliant\" a weak claim?"
    options:
      - id: ofd9d72
        text: "No database implements atomicity"
      - id: oc67060
        text: "Implementations differ, especially around isolation, so the guarantees are unclear"
        correct: true
      - id: o9bf1d8
        text: "ACID only applies to NoSQL"
      - id: o82066a
        text: "ACID is a trademark"
    explanation: "“one database’s implementation of ACID does not equal another’s ... when a system claims to be ‘ACID compliant,’ it’s unclear what guarantees you can actually expect. ‘ACID’ has unfortunately become mostly a marketing term.”"
  - id: base
    prompt: "What is the only sensible definition of BASE, per the chapter?"
    options:
      - id: o649ae5
        text: "\"Not ACID\""
        correct: true
      - id: o464864
        text: "A specific replication protocol"
      - id: oc605cb
        text: "Batch analytics with sharded execution"
      - id: oa2c8aa
        text: "Basically available with strong consistency"
    explanation: "“It seems that the only sensible definition of BASE is “not ACID” (i.e., it can mean almost anything you want).”"
  - id: atomicity-meaning
    prompt: "In ACID, what does atomicity describe?"
    options:
      - id: o4e62a0
        text: "What concurrent transactions can see of each other"
      - id: o535e91
        text: "What happens if a fault occurs after some writes in a transaction: the transaction is aborted and its writes discarded"
        correct: true
      - id: o9e9045
        text: "That operations happen in a single CPU instruction"
      - id: of8ac6e
        text: "That data survives crashes"
    explanation: "“ACID atomicity describes what happens if a client wants to make several writes, but a fault occurs after some of the writes have been processed ... Perhaps abortability would have been a better term”."
  - id: atomicity-retry
    prompt: "Why does atomicity make retries safe?"
    options:
      - id: o3d8706
        text: "It ensures retries succeed"
      - id: o3e06d6
        text: "An aborted transaction changed nothing, so retrying won't duplicate partial changes"
        correct: true
      - id: o8ea3b3
        text: "It deduplicates all requests"
      - id: o61ef8f
        text: "It locks the database during retries"
    explanation: "“if a transaction was aborted, the application can be sure that it didn’t change anything, so it can safely be retried.”"
  - id: consistency-meanings
    prompt: "Which are among the meanings of \"consistency\" listed in the chapter?"
    type: multiple
    options:
      - id: o42e8b2
        text: "Consistent hashing"
        correct: true
      - id: o998d0a
        text: "Linearizability in the CAP theorem"
        correct: true
      - id: o51b746
        text: "Consistent snapshots"
        correct: true
      - id: o40de80
        text: "Consistent indentation of code"
      - id: o27eabc
        text: "Replica consistency (eventual consistency)"
        correct: true
    explanation: "The chapter lists replica consistency, consistent snapshots, consistent hashing, CAP's linearizability, and ACID's “application-specific notion of the database being in a “good state.”” — “at least five meanings”."
  - id: c-in-acid
    prompt: "Why is the C in ACID often not a property of the database alone?"
    options:
      - id: o42bc84
        text: "Consistency is guaranteed by replication"
      - id: o485064
        text: "It is defined by the SQL standard"
      - id: o770592
        text: "Databases do not support constraints"
      - id: o9fe484
        text: "Undeclared invariants can only be preserved by the application defining its transactions correctly"
        correct: true
    explanation: "“If you write bad data that violates your invariants, but you haven’t declared those invariants, the database can’t stop you. As such, the C in ACID often depends on how the application uses the database”."
  - id: isolation-theory
    prompt: "How do classic textbooks formalize isolation, and how does practice differ?"
    options:
      - id: ocdc280
        text: "As linearizability; practice is identical"
      - id: o1874e4
        text: "As eventual consistency; practice is stronger"
      - id: ob96a48
        text: "As durability; practice ignores it"
      - id: oa5fbc6
        text: "As serializability; in practice many databases use weaker levels for performance"
        correct: true
    explanation: "Textbooks formalize “isolation as serializability”, but “In practice, many databases use forms of isolation that are weaker than serializability”."
  - id: oracle-serializable
    prompt: "What does Oracle's \"serializable\" level actually implement?"
    options:
      - id: o3f9687
        text: "Snapshot isolation, which is weaker than serializability"
        correct: true
      - id: o1cedc7
        text: "SSI"
      - id: of4bab4
        text: "True serializability via 2PL"
      - id: oe1c2f9
        text: "Read committed"
    explanation: "“Oracle has an isolation level called “serializable,” but it actually implements snapshot isolation, which is a weaker guarantee than serializability”."
  - id: durability-risks
    prompt: "Which risks to durability does the \"Replication and Durability\" sidebar list?"
    type: multiple
    options:
      - id: o97f2a1
        text: "Writing to disk makes data permanently immune to loss"
      - id: of08ef3
        text: "Correlated faults can knock out all replicas, losing in-memory data"
        correct: true
      - id: o56c30a
        text: "SSDs may violate guarantees on power loss; even fsync can misbehave"
        correct: true
      - id: o09c5d0
        text: "Silent corruption can spread to replicas and recent backups"
        correct: true
    explanation: "The sidebar mentions correlated faults, SSDs that “sometimes violate the guarantees they are supposed to provide”, undetected corruption, and concludes “no one technique can provide absolute guarantees”."
  - id: fsync-postgres
    prompt: "What does the chapter say about PostgreSQL and fsync?"
    options:
      - id: o54dcc4
        text: "fsync is always reliable"
      - id: oa27129
        text: "PostgreSQL never calls fsync"
      - id: o93499f
        text: "PostgreSQL used fsync incorrectly for over 20 years, showing how hard it is to use"
        correct: true
      - id: ode19f9
        text: "fsync replaced the WAL"
    explanation: "“And fsync is hard to use; even PostgreSQL used it incorrectly for over 20 years”."
  - id: durability-practice
    prompt: "What practical stance does the chapter take on durability?"
    options:
      - id: oc3b5a8
        text: "Combine risk-reduction techniques — disk writes, replication and backups — and treat \"guarantees\" with skepticism"
        correct: true
      - id: o0a47cc
        text: "Rely on fsync alone"
      - id: o07a71b
        text: "Rely on replication alone"
      - id: oc6f7d4
        text: "Durability is perfect in cloud databases"
    explanation: "“There are only various risk-reduction techniques—including writing to disk, replicating to remote machines, and backups—and they can and should be used together.”"
  - id: unread-counter
    prompt: "An email app stores an unread counter separately. A user sees a new email but the counter still says 0. Which guarantee would have prevented this?"
    options:
      - id: o5122b7
        text: "Durability"
      - id: oaf54ae
        text: "Sharding"
      - id: od563d8
        text: "Isolation"
        correct: true
      - id: o8394f9
        text: "Replication"
    explanation: "“Isolation would have prevented this issue … by ensuring that user 2 sees either both the inserted email and the updated counter, or neither”."
  - id: tcp-transaction
    prompt: "How do relational databases typically group statements into a transaction?"
    options:
      - id: o9b7c90
        text: "By statement text"
      - id: o7b1e3d
        text: "By timestamps"
      - id: o497ce5
        text: "By the client's TCP connection: everything between BEGIN and COMMIT on that connection"
        correct: true
      - id: o948ff6
        text: "By the user ID"
    explanation: "“In relational databases, that is typically done based on the client’s TCP connection to the database server ... If the TCP connection is interrupted, the transaction must be aborted.”"
  - id: multi-put
    prompt: "Does a key-value store's multi-put operation imply transaction semantics?"
    options:
      - id: ob1f08b
        text: "No — it may succeed for some keys and fail for others"
        correct: true
      - id: o864b98
        text: "Only for two keys"
      - id: o174d49
        text: "Only on SSDs"
      - id: oa497b5
        text: "Yes — multi-put is always atomic"
    explanation: "“Even if there is a multi-object API ... that doesn’t necessarily mean it has transaction semantics: the command may succeed for some keys and fail for others”."
  - id: single-object
    prompt: "How do storage engines provide atomicity and isolation for single objects?"
    options:
      - id: obdb348
        text: "Bloom filters"
      - id: oa6e8a4
        text: "Distributed transactions"
      - id: o9ed3dd
        text: "A log for crash recovery and a per-object lock"
        correct: true
      - id: o40bd13
        text: "2PC"
    explanation: "“Atomicity can be implemented using a log for crash recovery ... and isolation can be implemented using a lock on each object”."
  - id: lightweight-txn
    prompt: "What do Cassandra's \"lightweight transactions\" provide?"
    options:
      - id: o8a6c93
        text: "Linearizable reads and conditional writes on a single object, with no multi-object guarantees"
        correct: true
      - id: o7e9488
        text: "Snapshot isolation"
      - id: ob435b1
        text: "Serializable multi-shard transactions"
      - id: o78bee9
        text: "Full ACID across tables"
    explanation: "They “offer linearizable ... reads and conditional writes on a single object, but no guarantees across multiple objects.”"
  - id: need-multi
    prompt: "Which situations need multi-object transactions?"
    type: multiple
    options:
      - id: of725f0
        text: "Reading a single key"
      - id: oea2515
        text: "Updating denormalized data across documents"
        correct: true
      - id: ob6bd3c
        text: "Keeping secondary indexes in sync with data"
        correct: true
      - id: o8679ab
        text: "Keeping foreign-key references valid"
        correct: true
    explanation: "The chapter lists foreign keys and graph edges, denormalized documents, and secondary indexes that “need to be updated every time you change a value”."
  - id: orm-retry
    prompt: "What does the chapter criticize about ORMs like ActiveRecord and Django?"
    options:
      - id: o6f1e09
        text: "They forbid transactions"
      - id: o6a5cc2
        text: "They use 2PC"
      - id: o636d27
        text: "They don't retry aborted transactions, so the whole point of rollback — safe retry — is wasted"
        correct: true
      - id: o9289f5
        text: "They retry too often"
    explanation: "“popular object-relational mapping (ORM) frameworks such as Rails ActiveRecord and Django don’t retry aborted transactions ... This is a shame, because the whole point of rolling back transactions is to enable safe retries.”"
  - id: retry-pitfalls
    prompt: "Which are pitfalls of retrying aborted transactions?"
    type: multiple
    options:
      - id: o778b59
        text: "A commit whose ack was lost would be performed twice"
        correct: true
      - id: o43c9b0
        text: "External side effects such as emails may repeat"
        correct: true
      - id: o90bda3
        text: "Retries are never safe"
      - id: ocebcdb
        text: "Retrying overload errors makes things worse"
        correct: true
      - id: od70de1
        text: "Retrying permanent errors such as constraint violations is pointless"
        correct: true
    explanation: "The chapter lists all four, plus “If the client process crashes while retrying, any data it was trying to write to the database is lost.”"
---
Transactions group reads and writes so they commit or abort as a unit. ACID names four guarantees, but
each letter is subtler than it looks.

## Learning objectives

- Define atomicity as abortability, not concurrency
- Explain why consistency is an application property
- Contrast isolation in theory (serializability) with practice
- Explain why durability is a set of risk reductions, not a guarantee
- Explain why multi-object transactions matter and when to retry aborts

## In a nutshell

- Transactions simplify error handling: on abort, retry safely.
- NewSQL showed transactions can scale.
- "ACID compliant" is mostly marketing; BASE means "not ACID".
- Atomicity = all-or-nothing on faults; isolation = concurrent transactions don't interfere.
- Retrying is not free: duplicates after lost commit acks, overload, side effects.

## Connections to today

- **Distributed SQL** (CockroachDB, Spanner, YugabyteDB, TiDB, Aurora DSQL) brings ACID to global scale.
- **EF Core's execution strategies** retry transient failures — but you must make the whole unit of
  work retryable, as the chapter warns.
- **fsync bugs.** PostgreSQL's "fsyncgate" is the chapter's example of how hard durability is.
