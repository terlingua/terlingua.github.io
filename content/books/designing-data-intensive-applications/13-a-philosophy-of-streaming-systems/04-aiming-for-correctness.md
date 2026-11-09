---
title: "Aiming for Correctness"
questions:
  - id: stateful-care
    prompt: "Why do stateful systems need more careful thought than stateless services?"
    options:
      - id: o845d57
        text: "Their mistakes can last forever, since they remember things"
        correct: true
      - id: oc71c20
        text: "They use more CPU"
      - id: oc89e16
        text: "They are always distributed"
      - id: o4e3fab
        text: "They are slower"
    explanation: "“Stateful systems such as databases ... are designed to remember things forever (more or less), so if something goes wrong, the effects also potentially last forever”."
  - id: jepsen
    prompt: "What have Kyle Kingsbury's Jepsen experiments highlighted?"
    options:
      - id: ocd07ae
        text: "Discrepancies between products' claimed safety guarantees and actual behaviour under network problems and crashes"
        correct: true
      - id: o198c53
        text: "That clocks are accurate"
      - id: ocb98eb
        text: "That all databases are correct"
      - id: o4e9835
        text: "That SQL is slow"
    explanation: "Jepsen “highlighted the stark discrepancies between some products’ claimed safety guarantees and their actual behavior in the presence of network problems and crashes.”"
  - id: serializable-not-enough
    prompt: "Why doesn't serializable isolation guarantee freedom from data corruption?"
    options:
      - id: o09767f
        text: "Serializable is a weak level"
      - id: ocff693
        text: "Application bugs can still write wrong data, and duplicates can arise outside the transaction's scope"
        correct: true
      - id: o2d1d23
        text: "Serializable allows dirty reads"
      - id: o48f202
        text: "It does guarantee it"
    explanation: "“if an application has a bug that causes it to write incorrect data or delete data from a database, serializable transactions aren’t going to save you.”"
  - id: double-transfer
    prompt: "A client sends COMMIT for an $11 transfer, the connection times out, and it retries. What can happen?"
    options:
      - id: o54a93a
        text: "The retry is rejected"
      - id: o4416c7
        text: "Nothing is transferred"
      - id: o9be9e5
        text: "The database deduplicates automatically"
      - id: o2e54ea
        text: "$22 is transferred, because the retry is a new transaction outside TCP's duplicate suppression"
        correct: true
    explanation: "“The client can reconnect to the database and retry the transaction, but now it is outside the scope of TCP duplicate suppression. Since the transaction ... is not idempotent, $22 could be transferred instead of the desired $11.”"
  - id: browser-retry
    prompt: "Why don't 2PC or database deduplication stop a user resubmitting a timed-out POST?"
    options:
      - id: o20fc5b
        text: "From the server's and database's view, the resubmission is a separate request and transaction"
        correct: true
      - id: o9459d0
        text: "2PC detects browser retries"
      - id: o57b0d1
        text: "POST is idempotent"
      - id: o46581b
        text: "Browsers never resubmit"
    explanation: "“From the web server’s point of view, the retry is a separate request, and from the database’s point of view, it is a separate transaction. The usual deduplication mechanisms don’t help.”"
  - id: request-id
    prompt: "How does Example 13-2 suppress duplicate requests?"
    options:
      - id: o7f5032
        text: "A client-generated request ID with a uniqueness constraint; a duplicate insert aborts the transaction"
        correct: true
      - id: o4ec42e
        text: "A Bloom filter"
      - id: odea873
        text: "A timestamp check"
      - id: oace2c7
        text: "Read committed isolation"
    explanation: "“This code relies on a uniqueness constraint on the request_id column. If a transaction attempts to insert an ID that already exists, the INSERT fails and the transaction is aborted”."
  - id: uniqueness-weak-isolation
    prompt: "Why is a uniqueness constraint safer than an application-level check-then-insert?"
    options:
      - id: o1014be
        text: "Databases maintain uniqueness constraints correctly even at weak isolation, while check-then-insert can suffer write skew"
        correct: true
      - id: oba8a71
        text: "Constraints are faster"
      - id: o9c8670
        text: "Check-then-insert is always serializable"
      - id: o4efd75
        text: "Constraints don't need indexes"
    explanation: "“Relational databases can generally maintain a uniqueness constraint correctly, even at weak isolation levels (whereas an application-level check-then-insert may fail under nonserializable isolation”."
  - id: e2e-argument
    prompt: "What does the end-to-end argument state?"
    options:
      - id: ob33883
        text: "Some functions can be fully and correctly implemented only with the help of the application at the endpoints; lower layers can only partially help"
        correct: true
      - id: o1ffebf
        text: "All reliability should be in the network"
      - id: o5987d4
        text: "Encryption is unnecessary"
      - id: o5aa7f5
        text: "Endpoints should never check data"
    explanation: "“The function in question can completely and correctly be implemented only with the knowledge and help of the application standing at the endpoints of the communication system ... (Sometimes an incomplete version of the function provided by the communication system may be useful as a performance enhancement.)”"
  - id: e2e-examples
    prompt: "Which are applications of the end-to-end argument in the chapter?"
    type: multiple
    options:
      - id: o445e07
        text: "End-to-end checksums for corruption from software bugs or disks"
        correct: true
      - id: oeda1a5
        text: "End-to-end encryption and authentication"
        correct: true
      - id: o115de8
        text: "Duplicate suppression with a request ID from client to database"
        correct: true
      - id: obd3feb
        text: "Relying only on WiFi passwords for security"
    explanation: "The chapter applies it to duplicate suppression, notes TCP/TLS checksums “cannot detect corruption due to bugs in the software ... or corruption on the disks”, and says “Only end-to-end encryption and authentication can protect against all these things.”"
  - id: low-level-useful
    prompt: "Are low-level reliability features useless given the end-to-end argument?"
    options:
      - id: o7ecbc4
        text: "They only add latency"
      - id: oa03572
        text: "No — they reduce the probability of higher-level problems, but aren't sufficient alone"
        correct: true
      - id: o9eb72e
        text: "Yes — they should be removed"
      - id: o02284f
        text: "They are sufficient alone"
    explanation: "“Although the low-level features ... cannot provide the desired end-to-end features by themselves, they are still useful, since they reduce the probability of problems at higher levels.”"
  - id: reimplement
    prompt: "What happens when teams refuse distributed transactions as too expensive?"
    options:
      - id: o72a6e0
        text: "Everything becomes correct automatically"
      - id: of5eb69
        text: "They switch to LWW safely"
      - id: o34af5a
        text: "They reimplement fault tolerance in application code, which usually doesn't work correctly"
        correct: true
      - id: o25919d
        text: "They stop needing correctness"
    explanation: "“When we refuse to use distributed transactions because they are too expensive, we end up having to reimplement fault-tolerance mechanisms in application code ... most application-level mechanisms do not work correctly.”"
  - id: uniqueness-consensus
    prompt: "Why is asynchronous multi-leader replication ruled out for immediate uniqueness enforcement?"
    options:
      - id: o22b721
        text: "It lacks indexes"
      - id: o9f7521
        text: "It is too slow"
      - id: of4f957
        text: "Different leaders could accept conflicting writes concurrently; immediate rejection needs synchronous coordination"
        correct: true
      - id: ob03fb3
        text: "Multi-leader can't store strings"
    explanation: "“asynchronous multi-leader replication is ruled out, because different leaders could concurrently accept conflicting writes ... If you want to be able to immediately reject any writes that would violate the constraint, synchronous coordination is unavoidable.”"
  - id: username-log
    prompt: "How can a stream processor enforce unique usernames?"
    options:
      - id: odfaf36
        text: "Every node checks a global lock"
      - id: o5ec4f7
        text: "Accept all and dedupe nightly"
      - id: o83069c
        text: "Use LWW on usernames"
      - id: o635a2b
        text: "Shard requests by username hash; a single-threaded processor per shard checks a local table and emits success or rejection"
        correct: true
    explanation: "“Every request for a username is encoded as a message and appended to a shard determined by the hash of the username ... For every request for a username that is already taken, it emits a rejection message”."
  - id: conflict-principle
    prompt: "What is the fundamental principle behind log-based constraint enforcement?"
    options:
      - id: o8d96d6
        text: "All writes go to one global shard"
      - id: ofa79ce
        text: "Conflicts are resolved by LWW"
      - id: o52d3b0
        text: "Writes that may conflict are routed to the same shard and processed sequentially"
        correct: true
      - id: od394f2
        text: "Constraints are checked weekly"
    explanation: "“Its fundamental principle is that any writes that may conflict are routed to the same shard and processed sequentially.”"
  - id: payment-atomicity
    prompt: "In the multi-shard payment design, where does atomicity come from?"
    options:
      - id: oa5bbf1
        text: "A distributed lock"
      - id: o7e3642
        text: "Synchronous replication"
      - id: o35729d
        text: "A 2PC across all three shards"
      - id: o5e4dce
        text: "Appending the initial request event to the source-account log; everything downstream eventually follows, deduplicated by request ID"
        correct: true
    explanation: "“Atomicity in this system comes not from transactions, but from the fact that writing the initial request event to the source account log is an atomic action.”"
  - id: payment-reqs
    prompt: "What does the multi-shard payment design require of stream processors?"
    type: multiple
    options:
      - id: o98e810
        text: "Deterministic processing"
        correct: true
      - id: oa49a46
        text: "Linearizable reads across shards"
      - id: o8011b1
        text: "Strict log-order processing per account with at-least-once semantics"
        correct: true
      - id: o23af84
        text: "Exactly one delivery with no retries ever"
    explanation: "“The only requirements are that the events for any given account are processed strictly in log order with at-least-once semantics and that the stream processors are deterministic.”"
  - id: timeliness-integrity
    prompt: "Distinguish timeliness and integrity."
    options:
      - id: oe2d873
        text: "Timeliness is permanent; integrity is temporary"
      - id: o320237
        text: "Integrity means low latency"
      - id: o2f468f
        text: "They are synonyms"
      - id: o9fefc9
        text: "Timeliness means observing an up-to-date state (violations are temporary); integrity means no corruption or loss (violations are permanent)"
        correct: true
    explanation: "“violations of timeliness are allowed under eventual consistency, whereas violations of integrity result in perpetual inconsistency.”"
  - id: credit-card
    prompt: "On a credit card statement, which is an integrity violation rather than a timeliness issue?"
    options:
      - id: o000755
        text: "Pending authorizations"
      - id: o1928f1
        text: "The balance not equalling the previous balance plus transactions"
        correct: true
      - id: o3055c0
        text: "A transaction from the last 24 hours not yet appearing"
      - id: oeba7a7
        text: "Statements arriving monthly"
    explanation: "“it is not surprising if a transaction that you made within the last 24 hours does not yet appear ... However, it would be very bad if the statement balance was not equal to the sum of the transactions plus the previous statement balance”."
  - id: decouple
    prompt: "What do event-based dataflow systems decouple?"
    options:
      - id: o00ca43
        text: "Timeliness and integrity"
        correct: true
      - id: o7f493c
        text: "Storage and compute only"
      - id: oc6bee1
        text: "Reads and writes entirely"
      - id: o281bb2
        text: "Consistency and durability"
    explanation: "“an interesting property of the event-based dataflow systems that we have discussed in this chapter is that they decouple timeliness and integrity.”"
  - id: integrity-mechanisms
    prompt: "Which mechanisms achieve integrity without distributed transactions?"
    type: multiple
    options:
      - id: o59911d
        text: "Passing a client request ID through all levels"
        correct: true
      - id: o296589
        text: "Immutable messages with periodic reprocessing"
        correct: true
      - id: od0ba7b
        text: "Representing each write as a single atomically appended message"
        correct: true
      - id: o197888
        text: "LWW with wall clocks"
      - id: o8f1ab5
        text: "Deriving other state via deterministic functions"
        correct: true
    explanation: "These four mechanisms are listed; reliable stream processing “can preserve integrity without requiring distributed transactions and an atomic commit protocol”."
  - id: compensating
    prompt: "What is a compensating transaction?"
    options:
      - id: o393ed7
        text: "A later change that corrects a constraint violation, such as a refund or apology with a discount"
        correct: true
      - id: od5b084
        text: "A nightly backup"
      - id: o867159
        text: "A transaction that compresses data"
      - id: of3eb5a
        text: "A 2PC rollback"
    explanation: "“In many business contexts, it is therefore acceptable to temporarily violate a constraint and fix it up later by apologizing. This kind of change to correct a mistake is called a compensating transaction.”"
  - id: forklift
    prompt: "Why might a hard \"never oversell\" constraint be unnecessary for a warehouse?"
    options:
      - id: o18b049
        text: "Overselling is illegal"
      - id: o8ef2df
        text: "Apology workflows already exist for events like a forklift destroying stock"
        correct: true
      - id: o946a85
        text: "Warehouses never run out"
      - id: o606158
        text: "Constraints are always enforced by hardware"
    explanation: "“This is the same as what you’d have to do if, say, a forklift truck ran over some of the items in your warehouse ... the apology workflow already needs to be part of your business processes anyway”."
  - id: coord-avoiding
    prompt: "What do coordination-avoiding data systems offer?"
    options:
      - id: o9fc53f
        text: "No integrity guarantees"
      - id: o3af444
        text: "Linearizability everywhere"
      - id: oeba6c7
        text: "Strong integrity with weak timeliness, enabling multi-region operation without synchronous coordination"
        correct: true
      - id: oe6af69
        text: "Mandatory XA transactions"
    explanation: "“Such a system would have weak … timeliness guarantees—it could not be linearizable without introducing coordination—but it can still have strong integrity guarantees.”"
  - id: apologies
    prompt: "How does the chapter frame the trade-off of coordination?"
    options:
      - id: o0b4d21
        text: "Apologies are irrelevant"
      - id: odd50db
        text: "Coordination reduces apologies for inconsistencies but may increase apologies for outages; aim for the sweet spot"
        correct: true
      - id: ocb09dc
        text: "Coordination eliminates all apologies"
      - id: o8a5db8
        text: "Avoiding coordination eliminates all apologies"
    explanation: "“You cannot reduce the number of apologies to zero, but you can aim to find the best trade-off for your needs”."
  - id: probabilistic
    prompt: "How should system-model assumptions really be viewed?"
    options:
      - id: obd08de
        text: "As probabilities — unlikely violations do happen at scale"
        correct: true
      - id: oc00ae4
        text: "As irrelevant in practice"
      - id: o6bcefb
        text: "As absolute truths"
      - id: o0e8c23
        text: "As only about networks"
    explanation: "“In reality, it is more a question of probabilities ... If you are operating at large enough scale, even very unlikely things do happen.”"
  - id: db-bugs
    prompt: "What examples show even mature databases have integrity bugs?"
    type: multiple
    options:
      - id: o7fba38
        text: "All bugs are hardware faults"
      - id: oc4ecba
        text: "SQLite never had bugs"
      - id: o73573f
        text: "Past MySQL versions failed to maintain uniqueness constraints"
        correct: true
      - id: o494c13
        text: "PostgreSQL's serializable level once exhibited write skew"
        correct: true
    explanation: "“past versions of MySQL have failed to correctly maintain uniqueness constraints, and PostgreSQL’s serializable isolation level has exhibited write skew anomalies in the past”."
  - id: trust-verify
    prompt: "How do HDFS and S3 practise \"trust, but verify\"?"
    options:
      - id: o8a8efd
        text: "They never move files"
      - id: oc93144
        text: "Background processes continually read back files and compare them with replicas"
        correct: true
      - id: o6af00d
        text: "They encrypt everything twice"
      - id: od55211
        text: "They trust disks absolutely"
    explanation: "“These systems run background processes that continually read back files, compare them to other replicas, and move files from one disk to another, in order to mitigate the risk of silent corruption.”"
  - id: restore-test
    prompt: "Why test restoring backups regularly?"
    options:
      - id: o2f4f03
        text: "Restores speed up queries"
      - id: odf0a8f
        text: "It is a regulatory requirement only"
      - id: od85dc5
        text: "Otherwise you may discover the backup is broken only after you've lost data"
        correct: true
      - id: o0fc991
        text: "Backups degrade if not read"
    explanation: "“it is important to try restoring from your backups from time to time—otherwise, you may find out that your backup is broken when it is too late and you have already lost data.”"
  - id: auditability
    prompt: "Why are event-based systems more auditable than ones that mutate several tables?"
    options:
      - id: od9c2a7
        text: "User input is a single immutable event and state is derived deterministically, so provenance is clear and derivations can be rerun to check"
        correct: true
      - id: o4855b1
        text: "They don't need logs"
      - id: o880439
        text: "They store less data"
      - id: o71a469
        text: "They use 2PC"
    explanation: "“In the event sourcing approach, user input to the system is represented as a single immutable event ... running the same log of events through the same version of the derivation code will result in the same state updates.”"
  - id: blockchain
    prompt: "How do blockchains differ from Chapter 10's consensus logs, and what lighter tools can be borrowed?"
    options:
      - id: of24e34
        text: "They are faster; nothing can be borrowed"
      - id: oaaeb48
        text: "They have no logs"
      - id: obc062d
        text: "They are Byzantine fault-tolerant; Merkle trees and signed append-only logs (as in Certificate Transparency) can be used more cheaply"
        correct: true
      - id: o6adfd8
        text: "They use LWW"
    explanation: "“blockchains are Byzantine fault-tolerant ... For most applications, blockchains have too high an overhead to be useful. However, some of their cryptographic tools can also be used in a lighter-weight context. For example, Merkle trees”."
---
Strong database guarantees don't make an application correct. End-to-end request IDs, log-based
constraint checking, integrity over timeliness, and auditing get closer.

## Learning objectives

- Explain why serializable transactions don't prevent duplicate requests from users
- State the end-to-end argument and apply it to dedup, checksums and encryption
- Enforce uniqueness and multi-shard operations with sharded logs and request IDs
- Distinguish timeliness from integrity and use compensating transactions
- Explain "trust, but verify" and designing for auditability

## In a nutshell

- TCP and DB transactions dedupe only within their scope; a client-generated request ID must flow end to end.
- Uniqueness needs consensus; shard by the unique value and process sequentially.
- Multi-shard payments: atomicity comes from appending one request event; downstream steps dedupe by request ID.
- Timeliness violations are temporary; integrity violations are permanent.
- Many "hard" constraints are business-negotiable with apologies.
- Coordination-avoiding systems: strong integrity, weak timeliness.
- Verify continually: read data back, test restores, recompute derived state.

## Connections to today

- **Idempotency keys** in payment APIs and ASP.NET Core idempotency middleware implement end-to-end
  request IDs.
- **Certificate Transparency** — cited in the chapter — is a production Merkle-tree audit log.
- **Data quality checks** (Great Expectations, dbt tests) are a form of continuous end-to-end auditing.
