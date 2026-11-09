---
title: "Knowledge, Truth, and Lies"
questions:
  - id: own-judgment
    prompt: "A node pauses for a minute, is declared dead, then resumes. What is the moral?"
    options:
      - id: o61f487
        text: "A node can't trust its own judgment; decisions such as declaring nodes dead rely on a quorum"
        correct: true
      - id: ob479eb
        text: "The node should keep acting as leader"
      - id: o3a20f7
        text: "The node should declare the others dead"
      - id: ofd8f4f
        text: "Pauses never happen in practice"
    explanation: "“a node cannot necessarily trust its own judgment of a situation ... If a quorum of nodes declares another node dead, then it must be considered dead, even if that node still very much feels alive.”"
  - id: majority-safe
    prompt: "Why is a majority quorum safe?"
    options:
      - id: od3a777
        text: "Majorities include every node"
      - id: o65c06d
        text: "Majorities avoid network delay"
      - id: o322067
        text: "Majorities are always fast"
      - id: ob49936
        text: "There can be only one majority, so no two conflicting decisions at the same time"
        correct: true
    explanation: "“It is also safe, because there can be only one majority in the system—there cannot be two majorities with conflicting decisions at the same time.”"
  - id: lease-uses
    prompt: "Which lease uses have serious consequences if two nodes believe they hold it?"
    type: multiple
    options:
      - id: o54b893
        text: "Being leader of a database shard"
        correct: true
      - id: oc7980e
        text: "Being the only writer to an object"
        correct: true
      - id: oc82837
        text: "Being the only processor of a batch input file (only wasted work)"
    explanation: "“In the third example, the consequence is only some wasted computational resources ... But in the first two cases, the consequence could be lost or corrupted data”."
  - id: zombie
    prompt: "What is a zombie?"
    options:
      - id: oa2f8d9
        text: "A crashed node that never returns"
      - id: o7d0263
        text: "A node running old software"
      - id: o2758a2
        text: "A former leaseholder that hasn't realized it lost the lease and keeps acting as if it holds it"
        correct: true
      - id: o1dbeb5
        text: "A duplicate request"
    explanation: "“The term zombie is sometimes used to describe a former leaseholder that has not yet found out that it lost the lease and is still acting as if it were the current leaseholder.”"
  - id: stonith
    prompt: "Why is shutting down suspected zombies (STONITH) not particularly effective?"
    options:
      - id: o6c5b49
        text: "It doesn't stop long-delayed requests, nodes may shut each other down, and damage may already be done"
        correct: true
      - id: oc45241
        text: "It works perfectly"
      - id: o75012c
        text: "It requires physical access"
      - id: o54ec3c
        text: "It is too violent"
    explanation: "“this approach does not protect against the large network delays depicted in Figure 9-5; all the nodes could shut one another down; and by the time a zombie has been detected and shut down, it may be too late”."
  - id: fencing
    prompt: "How do fencing tokens protect storage?"
    options:
      - id: oac6d6b
        text: "Each lease grant returns an increasing token; storage rejects writes with a token lower than one it has already processed"
        correct: true
      - id: oc65947
        text: "Tokens expire after a fixed time"
      - id: ob0d7e7
        text: "Tokens encrypt writes"
      - id: o5e4e44
        text: "Tokens are random and unique"
    explanation: "“the storage service remembers that it has already processed a write with a higher token number (34), so it rejects the request with token 33.”"
  - id: fencing-names
    prompt: "Which are names for fencing-token-like numbers?"
    type: multiple
    options:
      - id: o9bf1c3
        text: "Ballot numbers in Paxos and term numbers in Raft"
        correct: true
      - id: oc61c38
        text: "Bloom filter bits"
      - id: o6f3580
        text: "Sequencers in Chubby"
        correct: true
      - id: o30d650
        text: "Epoch numbers in Kafka"
        correct: true
    explanation: "“In Chubby, Google’s lock service, they are called sequencers, and in Kafka they are called epoch numbers. In consensus algorithms ... the ballot number (Paxos) or term number (Raft) serves a similar purpose.”"
  - id: fencing-cas
    prompt: "What storage feature can substitute for checking tokens?"
    options:
      - id: oc44ef5
        text: "Secondary indexes"
      - id: oac07f8
        text: "Conditional writes that succeed only if nobody else wrote since your read (like CAS)"
        correct: true
      - id: od91ddb
        text: "Read repair"
      - id: o4311e2
        text: "Bloom filters"
    explanation: "“it’s sufficient for the service to support a write that succeeds only if the object has not been written by another client since the current client last read it, similarly to an atomic CAS operation.” S3 calls these conditional writes."
  - id: fencing-leaderless
    prompt: "How can fencing tokens protect a leaderless LWW store?"
    options:
      - id: o566a5b
        text: "Put the token in the most significant bits of the write timestamp, so the new leaseholder's writes always win"
        correct: true
      - id: oeab860
        text: "Use read repair only"
      - id: ofc2b9d
        text: "Disable LWW"
      - id: obe66d0
        text: "Store tokens in a separate table"
    explanation: "“you can put the writer’s fencing token in the most significant bits or digits of the timestamp. You can then be sure that any timestamp generated by the new leaseholder will be greater than any timestamp from the old leaseholder”."
  - id: fencing-vs-occ
    prompt: "How does fencing differ from optimistic concurrency control failures?"
    options:
      - id: o94d11e
        text: "Fencing requires 2PL"
      - id: ob4d230
        text: "They are identical"
      - id: oa35902
        text: "Fencing is permanent, while OCC failures can be retried"
        correct: true
      - id: oe1243b
        text: "Fencing can be retried; OCC cannot"
    explanation: "“These operations are similar to the optimistic concurrency control (OCC) technique ... except that fencing is permanent, while concurrency control failures can be retried.”"
  - id: byzantine-def
    prompt: "What is a Byzantine fault?"
    options:
      - id: ob85209
        text: "A node crashing"
      - id: o1b23f8
        text: "A slow network"
      - id: o258d43
        text: "A node \"lying\" — sending arbitrary, faulty or malicious responses, e.g., contradictory votes"
        correct: true
      - id: o616d90
        text: "A clock drifting"
    explanation: "“Distributed systems problems become much harder if there is a risk that nodes may “lie” ... Such behavior is known as a Byzantine fault”."
  - id: bft-where
    prompt: "Where is Byzantine fault tolerance relevant?"
    type: multiple
    options:
      - id: o1c939d
        text: "Multitenant clouds isolated by firewalls and virtualization"
      - id: of5b24c
        text: "A single company's datacenter with trusted nodes"
      - id: o293e2d
        text: "Blockchains and other mutually untrusting multi-party systems"
        correct: true
      - id: o75cd37
        text: "Aerospace systems exposed to radiation"
        correct: true
    explanation: "The chapter names flight control systems and cryptocurrencies, while “in the kinds of systems we discuss in this book, we can usually safely assume that there are no Byzantine faults”. Tenants are isolated “not using Byzantine fault tolerance”."
  - id: bft-bugs
    prompt: "Why can't BFT protocols save you from a software bug deployed on every node?"
    options:
      - id: o0b2fca
        text: "BFT is too slow"
      - id: o949cd7
        text: "BFT ignores software"
      - id: o6194a2
        text: "BFT needs a supermajority of correct nodes; identical software has identical bugs"
        correct: true
      - id: o382382
        text: "Bugs are not faults"
    explanation: "“Most Byzantine fault-tolerant algorithms require a supermajority of more than two-thirds of the nodes to be functioning correctly ... you would have to have four independent implementations of the same software”."
  - id: weak-lying
    prompt: "Which are pragmatic defences against weak forms of \"lying\"?"
    type: multiple
    options:
      - id: oa29ece
        text: "Configuring NTP with multiple servers to exclude outliers"
        correct: true
      - id: o7e5b82
        text: "Full BFT consensus between web servers"
      - id: odde51a
        text: "Application-level checksums"
        correct: true
      - id: o3fbb1f
        text: "Sanitizing and range-checking user input"
        correct: true
    explanation: "The chapter lists checksums and TLS, input sanitization, and multiple NTP servers. These “are not full-blown Byzantine fault tolerance ... but they are nevertheless simple and pragmatic steps”."
  - id: timing-models
    prompt: "Which timing model is a realistic model of many systems?"
    options:
      - id: o115a35
        text: "Partially synchronous: usually bounded, occasionally unbounded"
        correct: true
      - id: o7bad81
        text: "Real-time"
      - id: o367a8c
        text: "Synchronous: always bounded"
      - id: o86ddd7
        text: "Asynchronous: no clocks at all"
    explanation: "Partial synchrony “means that a system behaves like a synchronous system most of the time, but it sometimes exceeds the bounds ... This is a realistic model of many systems.”"
  - id: node-models
    prompt: "In the crash-recovery model, what is assumed about storage?"
    options:
      - id: o826dea
        text: "Nothing survives crashes"
      - id: of5c465
        text: "Stable storage survives crashes; in-memory state is lost"
        correct: true
      - id: o0e589e
        text: "Nodes never crash"
      - id: ob4b7bc
        text: "Memory survives crashes"
    explanation: "“Nodes are assumed to have stable storage (i.e., nonvolatile disk storage) that is preserved across crashes, while the in-memory state is assumed to be lost.”"
  - id: gray-failure
    prompt: "Why can a limping (fail-slow) node be harder to handle than a crashed one?"
    options:
      - id: o5c48af
        text: "It always crashes eventually"
      - id: o681699
        text: "It sends Byzantine messages"
      - id: o62b694
        text: "It may pass health checks while being too slow to do real work"
        correct: true
      - id: o3f5ea5
        text: "It has no network"
    explanation: "Nodes “may still be able to respond to health check requests, while being too slow to get any real work done ... it can be even more difficult to deal with than a cleanly failed node.”"
  - id: useful-model
    prompt: "Which model combination does the chapter call generally most useful?"
    options:
      - id: ocf0a21
        text: "Partially synchronous with crash-recovery faults"
        correct: true
      - id: od4651b
        text: "Asynchronous with Byzantine faults"
      - id: oe18421
        text: "Real-time with no faults"
      - id: odce2ca
        text: "Synchronous with crash-stop faults"
    explanation: "“For modeling real systems, the partially synchronous model with crash-recovery faults is generally the most useful.”"
  - id: safety-liveness
    prompt: "For fencing tokens, which properties are safety and which liveness?"
    options:
      - id: o452097
        text: "Uniqueness is liveness"
      - id: o5d9747
        text: "Uniqueness and monotonic sequence are safety; availability is liveness"
        correct: true
      - id: o7be743
        text: "Availability is safety"
      - id: o087b37
        text: "All three are liveness"
    explanation: "“uniqueness and monotonic sequence are safety properties, but availability is a liveness property.”"
  - id: eventual-liveness
    prompt: "Eventual consistency is which kind of property?"
    options:
      - id: o04b69d
        text: "Safety"
      - id: o2847d5
        text: "Neither"
      - id: o36eee3
        text: "Both"
      - id: oeece9e
        text: "Liveness"
        correct: true
    explanation: "“A giveaway is that liveness properties often include the word “eventually” in their definition. (And yes, you guessed it— eventual consistency is a liveness property”."
  - id: safety-violation
    prompt: "What distinguishes a safety violation?"
    options:
      - id: odf7aae
        text: "It might be satisfied in the future"
      - id: o38c949
        text: "It only occurs after crashes"
      - id: oc64b69
        text: "It is always recoverable"
      - id: o55d7df
        text: "You can point to the moment it was broken, and it cannot be undone"
        correct: true
    explanation: "“If a safety property is violated, we can point to the particular point in time that it was broken ... After a safety property has been violated, the violation cannot be undone”."
  - id: amnesia
    prompt: "Why does a node with \"amnesia\" break quorum algorithms?"
    options:
      - id: o1dde09
        text: "Quorums rely on nodes remembering data they claimed to have stored"
        correct: true
      - id: o9701ff
        text: "It slows writes"
      - id: o245670
        text: "It increases replication factor"
      - id: o0867d6
        text: "It causes Byzantine faults only"
    explanation: "“Quorum algorithms ... rely on a node remembering the data that it claims to have stored. If a node may suffer from amnesia and forget previously stored data, that breaks the quorum condition”."
  - id: model-checking
    prompt: "What is a limitation of model checkers such as TLA+?"
    options:
      - id: o7abab4
        text: "They require production traffic"
      - id: odd869e
        text: "They check a simplified model with bounded executions, which can drift from the real implementation"
        correct: true
      - id: o11c307
        text: "They prove correctness for infinite states automatically"
      - id: o053835
        text: "They run your production code"
    explanation: "Model checkers “limit the execution to an upper bound” and “don’t run your actual code, but rather a simplified model ... it risks that your specification and your implementation go out of sync”."
  - id: dst
    prompt: "What distinguishes deterministic simulation testing from fault injection?"
    options:
      - id: o4033e8
        text: "It doesn't use randomness"
      - id: o7572cf
        text: "It runs the real code with all nondeterminism controlled, so failures can be replayed exactly"
        correct: true
      - id: o3fd115
        text: "It injects faults into production"
      - id: o1a51de
        text: "It runs a model, not code"
    explanation: "DST “tests your actual code, not a model” and “If a test fails, it can be rerun since the simulator knows the exact order of operations that triggered the failure—in contrast to fault injection”."
  - id: dst-levels
    prompt: "Match the DST strategy: \"a custom hypervisor makes an entire machine deterministic\"."
    options:
      - id: o3c2f01
        text: "Runtime-level (e.g., MadSim)"
      - id: o920f66
        text: "Model checking (e.g., TLA+)"
      - id: o5e2296
        text: "Application-level (e.g., FoundationDB's Flow)"
      - id: o8cfcc2
        text: "Machine-level (e.g., Antithesis)"
        correct: true
    explanation: "“Tools such as Antithesis do this by building a custom hypervisor that replaces normally nondeterministic operations with deterministic ones.”"
  - id: determinism
    prompt: "Where else in the book does determinism appear as a powerful idea?"
    type: multiple
    options:
      - id: of57580
        text: "Workflow engines' durable execution"
        correct: true
      - id: o041b16
        text: "Replaying event logs in event sourcing"
        correct: true
      - id: o3100ab
        text: "State machine replication"
        correct: true
      - id: o796e63
        text: "LWW with wall clocks"
    explanation: "The sidebar lists event sourcing, workflow engines and state machine replication, noting “making things deterministic is a simple but powerful idea that arises again and again in distributed system design.”"
  - id: remaining-nondeterminism
    prompt: "Even after mocking I/O, clocks and randomness, what nondeterminism may remain?"
    type: multiple
    options:
      - id: ocb73d2
        text: "The source code"
      - id: o9ecd43
        text: "Hash table iteration order in some languages"
        correct: true
      - id: oa87626
        text: "The values of constants"
      - id: o21303a
        text: "Hitting resource limits such as allocation failure"
        correct: true
    explanation: "“in some programming languages, the order in which you iterate over the elements of a hash table may be nondeterministic. Whether you run into a resource limit (memory allocation failure, stack overflow) is also nondeterministic.”"
  - id: summary-single
    prompt: "What does the summary advise if you can keep things on a single machine?"
    options:
      - id: o83817f
        text: "Always distribute anyway"
      - id: o83644e
        text: "Distribution is free"
      - id: obf4785
        text: "Single machines are unreliable"
      - id: oc2eed1
        text: "It is generally worth doing so, though fault tolerance and low latency may still require distribution"
        correct: true
    explanation: "“If you can avoid opening Pandora’s box and simply keep things on a single machine ... it is generally worth doing so. However ... Fault tolerance and low latency ... are equally important goals”."
---
A node can only guess about others from messages. Quorums decide, fencing tokens stop zombies, system
models make assumptions explicit, and testing techniques check the result.

## Learning objectives

- Explain why a node can't trust its own judgment and why quorums decide
- Use fencing tokens to make leases safe, including with leaderless storage
- Explain Byzantine faults and when BFT is (not) worth it
- Name timing and failure system models and safety vs liveness
- Compare model checking, fault injection and deterministic simulation testing

## In a nutshell

- A node declared dead by a quorum must step down.
- Leases + pauses = zombies; fencing tokens (monotonic numbers) let storage reject stale writers.
- Most systems assume crash-recovery nodes in a partially synchronous model.
- Safety must always hold; liveness may depend on conditions.
- TLA+ models, Jepsen, and DST (FoundationDB, TigerBeetle, Antithesis) find bugs tests miss.

## Connections to today

- **S3 conditional writes** (`If-Match`/`If-None-Match`) now allow fencing and leader election directly
  on object storage, as the chapter notes.
- **Deterministic simulation** is spreading: Antithesis, TigerBeetle's VOPR and Rust's MadSim.
- **AI-generated code** benefits from property-based and simulation testing that explores states humans
  wouldn't think to write tests for.
