---
title: "Distributed Transactions"
questions:
  - id: commit-record
    prompt: "On a single node, what is the key deciding moment for commit?"
    options:
      - id: obb6ba7
        text: "When the WAL begins"
      - id: o31375b
        text: "When the data pages are flushed"
      - id: o622969
        text: "When the client sends COMMIT"
      - id: of993f3
        text: "When the disk finishes writing the commit record"
        correct: true
    explanation: "“The key deciding moment for whether the transaction commits or aborts occurs when the disk finishes writing the commit record”."
  - id: per-node-commit
    prompt: "Why not just send a commit request to every node independently?"
    options:
      - id: of0a671
        text: "Some nodes may commit while others abort due to constraints, lost messages or crashes, and committed data can't be retracted"
        correct: true
      - id: odf9b2f
        text: "Nodes cannot receive commit requests"
      - id: o08393e
        text: "It is too slow"
      - id: o0137e5
        text: "It violates durability on every node"
    explanation: "“It could easily happen that the commit succeeds on some nodes and fails on others ... once a transaction has been committed on one node, it cannot be retracted again”."
  - id: prepare-yes
    prompt: "What does a participant promise when it votes yes in 2PC?"
    options:
      - id: oe9858d
        text: "Nothing binding"
      - id: ob7b641
        text: "That it can definitely commit later under all circumstances — it surrenders the right to abort"
        correct: true
      - id: ob6980a
        text: "That it has already committed"
      - id: o1ba5fb
        text: "That it will abort if the coordinator is slow"
    explanation: "“By replying yes to the coordinator, the node promises to commit the transaction without error if requested. In other words, the participant surrenders the right to abort the transaction, but without actually committing it.”"
  - id: commit-point
    prompt: "What is the 2PC commit point?"
    options:
      - id: oca0248
        text: "The coordinator writing its commit/abort decision to its transaction log on disk"
        correct: true
      - id: o38133a
        text: "All participants acknowledging"
      - id: o8dcc24
        text: "The client calling COMMIT"
      - id: o052b52
        text: "The first participant committing"
    explanation: "“The coordinator must write that decision to its transaction log on disk ... This is called the commit point.”"
  - id: retry-forever
    prompt: "After deciding to commit, what does the coordinator do if a commit request times out?"
    options:
      - id: o67ca7c
        text: "Ask the client"
      - id: o6fb3e9
        text: "Commit locally only"
      - id: o59afc9
        text: "Abort the transaction"
      - id: ob63edc
        text: "Retry forever until it succeeds"
        correct: true
    explanation: "“If this request fails or times out, the coordinator must retry forever until it succeeds. There is no more going back”."
  - id: points-no-return
    prompt: "What are 2PC's two \"points of no return\"?"
    options:
      - id: o5ffc9a
        text: "A participant voting yes, and the coordinator deciding"
        correct: true
      - id: o2d03d5
        text: "Writing the WAL, and flushing pages"
      - id: oc9e739
        text: "The first prepare, and the last ack"
      - id: o6d38ec
        text: "The client sending BEGIN, and COMMIT"
    explanation: "“when a participant votes yes, it promises that it will definitely be able to commit later ... and once the coordinator decides, that decision is irrevocable. Those promises ensure the atomicity of 2PC.”"
  - id: in-doubt
    prompt: "The coordinator crashes after participants vote yes. What can a participant do?"
    options:
      - id: o998c7d
        text: "Commit after a timeout safely"
      - id: oe01251
        text: "Abort after a timeout safely"
      - id: oda3dfb
        text: "Ask a random participant"
      - id: o4ba6eb
        text: "Nothing but wait for the coordinator to recover — it is \"in doubt\""
        correct: true
    explanation: "“If the coordinator crashes or the network fails at this point, the participant can do nothing but wait. A participant’s transaction in this state is called in doubt or uncertain.”"
  - id: 3pc
    prompt: "Why isn't three-phase commit used in practice?"
    options:
      - id: ob7fa49
        text: "It assumes bounded network delay and response times, which most systems don't have"
        correct: true
      - id: o057c2b
        text: "It is slower than 2PC"
      - id: o07c28a
        text: "It doesn't support aborts"
      - id: oc53e70
        text: "It requires a coordinator"
    explanation: "“3PC assumes a network with bounded delay and nodes with bounded response times; in most practical systems with unbounded network delay and process pauses ..., 3PC cannot guarantee atomicity.”"
  - id: 2pc-fix
    prompt: "What does the chapter suggest as a better fix for 2PC blocking?"
    options:
      - id: o94e181
        text: "Use LWW"
      - id: o7772c8
        text: "Use 3PC"
      - id: o195baa
        text: "Skip prepare"
      - id: o52c0ab
        text: "Replace the single-node coordinator with a fault-tolerant consensus protocol"
        correct: true
    explanation: "“A better solution in practice is to replace the single-node coordinator with a fault-tolerant consensus protocol.”"
  - id: two-kinds
    prompt: "Which two kinds of distributed transactions are often conflated?"
    options:
      - id: oa85eb3
        text: "Read-only and read-write"
      - id: ofcaa18
        text: "Database-internal and heterogeneous"
        correct: true
      - id: o29ec51
        text: "Local and global"
      - id: oc60221
        text: "Synchronous and asynchronous"
    explanation: "“Two quite different types of distributed transactions are often conflated: Database-internal distributed transactions ... Heterogeneous distributed transactions”."
  - id: xa-what
    prompt: "What is XA?"
    options:
      - id: oe1e64e
        text: "A network protocol for replication"
      - id: oa4d934
        text: "A SQL isolation level"
      - id: o491025
        text: "A C API standard for 2PC across heterogeneous systems, not a network protocol"
        correct: true
      - id: o2883d7
        text: "A consensus algorithm"
    explanation: "“XA is not a network protocol—it is merely a C API for interfacing with a transaction coordinator.”"
  - id: locks-in-doubt
    prompt: "Why are in-doubt transactions so harmful?"
    options:
      - id: o732696
        text: "They slow down reads only slightly"
      - id: oc241f3
        text: "They delete data"
      - id: o774d42
        text: "They corrupt the WAL"
      - id: oc78ce5
        text: "They keep holding row locks, blocking other transactions, possibly until an administrator intervenes"
        correct: true
    explanation: "“a transaction must hold onto the locks throughout the time it is in doubt ... This can cause large parts of your application to become unavailable until the in-doubt transaction is resolved.”"
  - id: reboot
    prompt: "Does rebooting the database fix orphaned in-doubt transactions?"
    options:
      - id: obe1353
        text: "No — correct 2PC must preserve their locks across restarts"
        correct: true
      - id: oe487ce
        text: "Yes — locks are released on restart"
      - id: ob685e3
        text: "Yes — the WAL is cleared"
      - id: oa2d0fa
        text: "Only on PostgreSQL"
    explanation: "“Even rebooting your database servers will not fix this problem, since a correct implementation of 2PC must preserve the locks of an in-doubt transaction even across restarts”."
  - id: heuristic
    prompt: "What are \"heuristic decisions\" in XA?"
    options:
      - id: o1f50ef
        text: "A participant unilaterally committing or aborting in doubt — a euphemism for probably breaking atomicity"
        correct: true
      - id: o72724c
        text: "Machine-learning-based commit decisions"
      - id: o57d1c6
        text: "Automatic deadlock detection"
      - id: od54abb
        text: "Retrying with backoff"
    explanation: "“heuristic here is a euphemism for probably breaking atomicity, since the heuristic decision violates the system of promises in 2PC.”"
  - id: xa-problems
    prompt: "Which are problems with XA transactions?"
    type: multiple
    options:
      - id: o003304
        text: "As a lowest common denominator, it can't detect cross-system deadlocks or work with SSI"
        correct: true
      - id: o090271
        text: "It cannot work with relational databases"
      - id: o0d4def
        text: "The coordinator (often in the app server) is a single point of failure"
        correct: true
      - id: o841abf
        text: "Coordinator and participants communicate only via application code"
        correct: true
    explanation: "The chapter lists all three: the coordinator's logs become crucial durable state, “it provides no way for the coordinator and the participants of a transaction to communicate with each other directly”, and it “cannot detect deadlocks across different systems ... and it does not work with SSI”."
  - id: internal-fixes
    prompt: "How do NewSQL databases avoid XA's problems?"
    type: multiple
    options:
      - id: oa2ddc1
        text: "Replicated participant shards"
        correct: true
      - id: oecd5ef
        text: "Direct communication between coordinator and shards"
        correct: true
      - id: o72128d
        text: "Using heuristic decisions routinely"
      - id: o866b51
        text: "Atomic commit coupled with distributed concurrency control"
        correct: true
      - id: o6f2c8e
        text: "Replicated coordinator with failover"
        correct: true
    explanation: "The chapter lists these four fixes, and notes “Consensus algorithms are commonly used to replicate the coordinator and the database shards.”"
  - id: exactly-once-idempotent
    prompt: "How can you achieve exactly-once message processing without a distributed transaction?"
    options:
      - id: od4a442
        text: "Disable retries"
      - id: oacf235
        text: "Use XA across broker and database"
      - id: o205ad7
        text: "Acknowledge the message before processing"
      - id: o11829c
        text: "Record processed message IDs in the database within the same transaction as the processing, and ack after commit"
        correct: true
    explanation: "“achieving exactly-once processing requires only transactions within the database ... Recording the message ID in the database makes the message processing idempotent”."
  - id: crash-after-commit
    prompt: "The processor crashes after committing but before acknowledging the message. What happens?"
    options:
      - id: o1dda32
        text: "The broker redelivers; the retry sees the message ID in the database and drops it"
        correct: true
      - id: o1f7de6
        text: "The transaction is rolled back"
      - id: oedc1bd
        text: "The message is lost"
      - id: o43a440
        text: "The message is processed twice"
    explanation: "“If it crashes after committing but before acknowledging the message to the broker, it will also retry processing, but the retry will see the message ID in the database and drop it.”"
  - id: concurrent-retry
    prompt: "What protects against a retry racing with a still-open first attempt?"
    options:
      - id: oc0980d
        text: "A Bloom filter"
      - id: obfc115
        text: "A uniqueness constraint on the message-ID table"
        correct: true
      - id: ob9fbe7
        text: "LWW"
      - id: o017f65
        text: "Snapshot isolation alone"
    explanation: "“a uniqueness constraint on the table of message IDs should prevent the same message ID from being inserted by two concurrent transactions.”"
  - id: summary-table
    prompt: "According to Table 8-1, which anomaly does snapshot isolation still allow?"
    options:
      - id: o5949dd
        text: "Write skew"
        correct: true
      - id: od35c80
        text: "Phantom reads (straightforward)"
      - id: o34461f
        text: "Read skew"
      - id: o2422ed
        text: "Dirty reads"
    explanation: "Table 8-1 marks write skew as “✗ Possible” under snapshot isolation, and the recap says “Only serializable isolation prevents this anomaly.” Lost updates are marked as depending on the implementation."
---
When a transaction spans nodes, committing atomically needs a protocol. Two-phase commit works, but it
blocks on coordinator failure and is painful across heterogeneous systems.

## Learning objectives

- Explain why per-node commits can leave nodes inconsistent
- Walk through 2PC and its two points of no return
- Explain in-doubt transactions, held locks and heuristic decisions
- Contrast heterogeneous (XA) and database-internal distributed transactions
- Achieve exactly-once message processing without distributed transactions

## In a nutshell

- On one node, writing the commit record is the atomic moment.
- 2PC: prepare (participants promise) → coordinator logs decision (commit point) → commit/abort retried forever.
- Coordinator crash leaves participants in doubt, holding locks.
- XA is a lowest common denominator; the app is a single point of failure.
- NewSQL databases fix 2PC's problems with replicated coordinators and shards.
- Idempotence via a processed-message-ID table gives exactly-once without XA.

## Connections to today

- **The outbox and inbox patterns** in microservices (e.g., MassTransit, NServiceBus, Debezium outbox)
  are the chapter's message-ID technique in practice.
- **Kafka transactions** are database-internal distributed transactions, as the chapter notes.
- **MSDTC** — named in the chapter — is the classic Windows coordinator; most cloud services avoid it.
