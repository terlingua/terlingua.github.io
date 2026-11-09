---
title: "Linearizability"
questions:
  - id: lin-def
    prompt: "What is the basic idea of linearizability?"
    options:
      - id: o83424e
        text: "Make the system appear to have one copy of the data with atomic operations — a recency guarantee"
        correct: true
      - id: o4f33b8
        text: "Order events by wall-clock time"
      - id: o20301c
        text: "Replicate data asynchronously"
      - id: o8f6167
        text: "Make transactions serializable"
    explanation: "“The basic idea ... is to make a system appear as if there is only one copy of the data, and all operations on it are atomic ... In other words, linearizability is a recency guarantee.”"
  - id: sports
    prompt: "Why is Bryce's stale score a linearizability violation, but would not be if they reloaded simultaneously?"
    options:
      - id: o8a747c
        text: "He started his query after hearing Aaliyah's result, so his read must be at least as recent"
        correct: true
      - id: odf2da7
        text: "It is never a violation"
      - id: of03549
        text: "Because replicas must always agree"
      - id: of29d6c
        text: "Because his phone is slower"
    explanation: "“Bryce knows that he hit the reload button (initiated his query) after he heard Aaliyah exclaim the final score, and therefore he expects his query result to be at least as recent as Aaliyah’s.”"
  - id: concurrent-read
    prompt: "A read overlaps in time with a write. What may a linearizable system return?"
    options:
      - id: o7b8362
        text: "Either the old or the new value"
        correct: true
      - id: o54619f
        text: "Only the new value"
      - id: occ134b
        text: "An error"
      - id: od667a5
        text: "Only the old value"
    explanation: "“Any read operations that overlap in time with the write operation might return either 0 or 1”."
  - id: flip
    prompt: "Client A reads the new value 1 while the write is still in progress. B starts a read after A's read returns. What must B see?"
    options:
      - id: oa85fff
        text: "0, because the write isn't finished"
      - id: o48f43c
        text: "An error"
      - id: od8d36e
        text: "0 or 1"
      - id: o4f22cc
        text: "1, because once any read returns the new value, all later reads must too"
        correct: true
    explanation: "“if one client’s read returns the new value 1, all subsequent reads must also return the new value, even if the write operation has not yet completed.”"
  - id: timeline
    prompt: "What does linearizability require of the markers joining operations in Figure 10-4?"
    options:
      - id: o9ca4a0
        text: "They always move forward in time, never backward"
        correct: true
      - id: odcbe77
        text: "They can move backward for reads"
      - id: o16a86b
        text: "They must align with request send times"
      - id: o13651a
        text: "They must be equally spaced"
    explanation: "“The requirement of linearizability is that the lines joining up the operation markers always move forward in time (from left to right), never backward.”"
  - id: lin-vs-ser
    prompt: "How do linearizability and serializability differ?"
    options:
      - id: oe469b8
        text: "Linearizability prevents write skew"
      - id: oed7734
        text: "Serializability is a recency guarantee"
      - id: o464c5b
        text: "They are the same"
      - id: o9baf43
        text: "Serializability concerns multi-object transactions in some serial order; linearizability is a single-object recency guarantee"
        correct: true
    explanation: "Serializability “is an isolation level of transactions”; linearizability “is a guarantee on reads and writes of a register ... it does not prevent problems such as write skew”. “stale reads are allowed by serializability”."
  - id: strict-ser
    prompt: "What is the combination of serializability and linearizability called?"
    options:
      - id: o445196
        text: "Strict serializability (strong one-copy serializability)"
        correct: true
      - id: oe99245
        text: "Eventual consistency"
      - id: o6612a0
        text: "Causal consistency"
      - id: ob6a926
        text: "Snapshot isolation"
    explanation: "“A database may provide both serializability and linearizability; this combination is known as strict serializability or strong one-copy serializability (strong-1SR).”"
  - id: cockroach
    prompt: "Which provides strict serializability, per the chapter?"
    options:
      - id: o2bead1
        text: "Spanner and FoundationDB"
        correct: true
      - id: o19b0e6
        text: "Cassandra"
      - id: o436ed9
        text: "MongoDB with default settings"
      - id: o92a7e5
        text: "CockroachDB"
    explanation: "“CockroachDB provides serializability and some recency guarantees on reads, but not strict serializability ... On the other hand, Spanner and FoundationDB offer strict serializability”."
  - id: lin-uses
    prompt: "Which situations require linearizability?"
    type: multiple
    options:
      - id: of8fc6c
        text: "Leader election via leases"
        correct: true
      - id: o07ef6d
        text: "Displaying a sports score"
      - id: o5b910d
        text: "Avoiding races across two communication channels"
        correct: true
      - id: o81dee1
        text: "Hard uniqueness constraints such as usernames"
        correct: true
    explanation: "The chapter covers “Locking and leader election”, “Constraints and uniqueness guarantees” and “Cross-channel timing dependencies”, while calling the sports score “perhaps a frivolous example”."
  - id: zk-reads
    prompt: "What does ZooKeeper guarantee about reads, compared with etcd v3?"
    options:
      - id: o16b483
        text: "ZooKeeper writes are linearizable but reads may be stale; etcd v3 reads are linearizable by default"
        correct: true
      - id: o7e6eb3
        text: "Both always have stale reads"
      - id: oc6ba04
        text: "etcd has no reads"
      - id: of0fba2
        text: "ZooKeeper reads are always linearizable"
    explanation: "“ZooKeeper provides linearizable writes, but reads may be stale ... etcd since version 3 provides linearizable reads by default.”"
  - id: loose-constraints
    prompt: "When can constraints such as \"no overbooking\" be enforced loosely, without linearizability?"
    options:
      - id: o55f270
        text: "When violations can be compensated, e.g., moving overbooked passengers to another flight"
        correct: true
      - id: oedc531
        text: "Never"
      - id: oa3326c
        text: "Only for bank balances"
      - id: o970f92
        text: "Only in single-node databases"
    explanation: "“In real applications, it is sometimes acceptable to treat such constraints loosely—for example, if a flight is overbooked, you can move customers to a different flight and offer them compensation”."
  - id: fk-no-lin
    prompt: "Which constraints can be implemented without linearizability?"
    options:
      - id: oe26d59
        text: "Foreign-key and attribute constraints"
        correct: true
      - id: o05a5b5
        text: "Hard uniqueness constraints"
      - id: oebbf4f
        text: "Distributed locks"
      - id: o4a3831
        text: "Single-leader election"
    explanation: "“a hard uniqueness constraint ... requires linearizability. Other kinds of constraints, such as foreign-key or attribute constraints, can be implemented without linearizability”."
  - id: video
    prompt: "A web server writes a video to storage, then enqueues a transcode job. Why can the transcoder see an old file?"
    options:
      - id: o2d2d0f
        text: "Transcoders cache old files"
      - id: offa5ea
        text: "The queue is linearizable"
      - id: od9ddc6
        text: "If storage isn't linearizable, the queue can outrun storage replication — two channels race"
        correct: true
      - id: ob2a3c6
        text: "Videos are too large"
    explanation: "“the message queue ... might be faster than the internal replication inside the storage service ... This problem arises because there are two communication channels between the web server and the transcoder”."
  - id: impl-single-leader
    prompt: "When is single-leader replication linearizable?"
    options:
      - id: o89ec92
        text: "Potentially, if all reads and writes go to the leader — but a deluded leader or async failover can break it"
        correct: true
      - id: o7bf6ad
        text: "Always"
      - id: odd3cd9
        text: "Never"
      - id: obfc7b9
        text: "Only with multi-leader"
    explanation: "Single-leader is “potentially linearizable” but “it is quite possible for a node to think that it is the leader, when in fact it is not” and “failover may even result in committed writes being lost”."
  - id: consensus-reads
    prompt: "Does using a consensus algorithm make every read linearizable?"
    options:
      - id: o35b53b
        text: "Only with LWW"
      - id: od6b8e2
        text: "No — reading from a node without checking it's still leader can return stale data"
        correct: true
      - id: oae43d0
        text: "Yes, always"
      - id: o518219
        text: "Only with Paxos"
    explanation: "“just because a system uses consensus does not guarantee that all operations on it are linearizable. If it allows reads on a node without checking that it is still the leader, the results of the read may be stale”."
  - id: quorum-not-lin
    prompt: "Can Dynamo-style quorums with w + r > n be non-linearizable?"
    options:
      - id: odf3223
        text: "Only with sloppy quorums"
      - id: oe9ea3d
        text: "Yes — with variable delays, a later read can return an older value than an earlier read"
        correct: true
      - id: of74569
        text: "Only if w = n"
      - id: o6c3296
        text: "No, quorums guarantee linearizability"
    explanation: "In Figure 10-6 “The quorum condition is met (w + r > n), but this execution is nevertheless not linearizable.”"
  - id: quorum-fix
    prompt: "What makes Dynamo-style read/write quorums linearizable, and what can't be done this way?"
    options:
      - id: o329c11
        text: "Synchronous read repair and reading the latest timestamp before writing; linearizable CAS still needs consensus"
        correct: true
      - id: o1ef464
        text: "Using LWW with wall clocks"
      - id: o273a4f
        text: "Nothing can make them linearizable"
      - id: ob5cdc7
        text: "Setting r = 1"
    explanation: "“A reader must perform read repair synchronously ... before writing, a writer must read the latest state of a quorum” but “a linearizable CAS operation cannot, because it requires a consensus algorithm.”"
  - id: partition
    prompt: "During a network partition between regions, a single-leader database's follower region can't reach the leader. What happens to clients needing linearizable reads and writes there?"
    options:
      - id: obcccb0
        text: "They continue normally"
      - id: o4e5232
        text: "They switch to LWW"
      - id: oa3b7b4
        text: "They are unavailable until the network is repaired"
        correct: true
      - id: o3fb43b
        text: "They automatically become leaders"
    explanation: "“If the application requires linearizable reads and writes, the network interruption causes the application to become unavailable in the regions that cannot contact the leader.”"
  - id: cp-ap
    prompt: "What does \"CP\" mean in CAP terms?"
    options:
      - id: o5ba584
        text: "Replicas cut off from others wait or return errors to preserve linearizability"
        correct: true
      - id: ofca11a
        text: "Replicas keep serving possibly stale data"
      - id: oe7d1c2
        text: "Partitions are prevented by consensus"
      - id: o949cb8
        text: "The system is never partitioned"
    explanation: "CP: replicas “will be temporarily unable to process requests ... This choice is sometimes known as CP (consistent under network partitions).”"
  - id: cap-flaws
    prompt: "Which criticisms of CAP does the chapter make?"
    type: multiple
    options:
      - id: o1305b9
        text: "Its definition of availability doesn't match the usual meaning"
        correct: true
      - id: o9cb2ca
        text: "\"Pick two of three\" is misleading because partitions aren't optional"
        correct: true
      - id: o288e7f
        text: "It considers only linearizability and only network partitions"
        correct: true
      - id: oae6e94
        text: "It is the most useful tool for system design"
    explanation: "The chapter notes CAP “considers only one consistency model ... and one kind of fault”, that partitions “aren’t something you choose”, and “the formalization of availability does not match the usual meaning of the term”."
  - id: cap-rephrase
    prompt: "How does the chapter suggest phrasing CAP better?"
    options:
      - id: o5a7ed4
        text: "Pick any two of consistency, availability, partition tolerance"
      - id: o4496ae
        text: "Availability beats consistency"
      - id: o8a9366
        text: "Always consistent, never available"
      - id: o17303f
        text: "Either consistent or available when partitioned"
        correct: true
    explanation: "“a better way of phrasing CAP would be either consistent or available when partitioned.”"
  - id: pacelc
    prompt: "What does PACELC add to CAP?"
    options:
      - id: o64938f
        text: "A requirement for consensus"
      - id: o00ee32
        text: "A third partition type"
      - id: o1d8545
        text: "Byzantine faults"
      - id: of995bf
        text: "Even without a partition, designers may trade consistency for lower latency"
        correct: true
    explanation: "“during a network partition (P), we need to choose between availability (A) and consistency (C); else (E), when there is no partition, we may choose between low latency (L) and consistency (C).”"
  - id: cap-credit
    prompt: "What does the chapter credit CAP with?"
    options:
      - id: o68f76f
        text: "Proving consensus impossible"
      - id: o3f087d
        text: "Inventing Raft"
      - id: o5fad65
        text: "Encouraging exploration of shared-nothing designs and helping trigger the NoSQL movement"
        correct: true
      - id: ofd9aa8
        text: "Defining serializability"
    explanation: "“CAP deserves credit for this culture shift—it helped trigger the NoSQL movement”."
  - id: ram-lin
    prompt: "Why is RAM on a multicore CPU not linearizable, and what does that teach?"
    options:
      - id: oc48156
        text: "CPUs are partitioned"
      - id: o70a8fa
        text: "RAM loses data on power failure"
      - id: o9f7c6d
        text: "RAM uses LWW"
      - id: o082bd0
        text: "Per-core caches are updated asynchronously for performance — systems drop linearizability for speed, not fault tolerance"
        correct: true
    explanation: "“The reason for dropping linearizability is performance, not fault tolerance. The same is true of many distributed databases that choose not to provide linearizable guarantees”."
  - id: attiya
    prompt: "What does the Attiya–Welch result say?"
    options:
      - id: o996033
        text: "Linearizable reads and writes have response time at least proportional to network delay uncertainty"
        correct: true
      - id: o707f00
        text: "Linearizability is impossible"
      - id: o435056
        text: "Linearizability is free with fast networks"
      - id: o9a3d13
        text: "Consensus requires Byzantine tolerance"
    explanation: "“if you want linearizability, the response time of read and write requests is at least proportional to the uncertainty of delays in the network.”"
---
Make a replicated system look like one copy with atomic operations: a recency guarantee. Useful for
locks, uniqueness, and cross-channel races — but expensive.

## Learning objectives

- Define linearizability and apply it to timing diagrams
- Distinguish linearizability from serializability
- Identify when linearizability is required
- Judge which replication methods can be linearizable
- Explain CAP, its flaws, and the latency cost of linearizability

## In a nutshell

- Once any read sees a new value, all later reads must too.
- Serializability: multi-object transactions in some serial order; linearizability: single-object recency. Both = strict serializability.
- Needed for leader election, hard uniqueness constraints, and cross-channel dependencies.
- Single-leader (reads on leader) and consensus: potentially linearizable; multi-leader: not; Dynamo-style: assume not.
- CAP is "either consistent or available when partitioned" and mostly of historical interest.
- Linearizable latency ∝ network delay uncertainty (Attiya–Welch).

## Connections to today

- **etcd v3** provides linearizable reads by default, while ZooKeeper reads may be stale — a practical
  difference for Kubernetes operators.
- **S3 strong read-after-write consistency** (since 2020) removed a common cross-channel race in data
  pipelines.
- **Push notification races** in mobile apps — notification arrives, fetch hits a stale replica — are the
  chapter's example in everyday apps.
