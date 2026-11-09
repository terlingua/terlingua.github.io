---
title: "Consensus"
questions:
  - id: all-consensus
    prompt: "Which problems does the chapter show are instances of consensus?"
    type: multiple
    options:
      - id: oad03f4
        text: "Fault-tolerant leader failover without split brain"
        correct: true
      - id: o6006e3
        text: "A fault-tolerant atomic CAS"
        correct: true
      - id: oc575c1
        text: "A fault-tolerant linearizable ID generator"
        correct: true
      - id: o3b6881
        text: "Sorting a list on one machine"
    explanation: "“It turns out that all of these are instances of the same fundamental distributed systems problem: consensus.”"
  - id: flp
    prompt: "What does the FLP result actually prove, and why can we still build consensus systems?"
    options:
      - id: oaed3c0
        text: "Consensus requires synchronized clocks"
      - id: o9a7624
        text: "Consensus is always impossible"
      - id: oca9cca
        text: "No deterministic algorithm in the asynchronous model can guarantee termination if a node may crash; timeouts or randomness make it solvable"
        correct: true
      - id: oc92416
        text: "Consensus only fails with Byzantine nodes"
    explanation: "“FLP doesn’t say that we can never reach consensus; it only says that we can’t guarantee that a consensus algorithm will always terminate ... If it can use timeouts ... then consensus becomes solvable. Even allowing the algorithm to use random numbers is sufficient”."
  - id: bft-third
    prompt: "What do Byzantine-tolerant consensus algorithms commonly assume?"
    options:
      - id: obefd6e
        text: "Exactly one node lies"
      - id: o9cfa7e
        text: "Fewer than one-third of nodes are Byzantine-faulty"
        correct: true
      - id: o8ee695
        text: "All nodes are honest"
      - id: ofdea2b
        text: "Fewer than half are slow"
    explanation: "“A common assumption is that fewer than one-third of the nodes are Byzantine-faulty. Such algorithms are used in blockchains”."
  - id: props
    prompt: "Which consensus property rules out an algorithm that always decides null?"
    options:
      - id: o68cbb2
        text: "Integrity"
      - id: o5e3492
        text: "Termination"
      - id: o77c6d8
        text: "Validity"
        correct: true
      - id: o09cf43
        text: "Uniform agreement"
    explanation: "“The validity property rules out trivial solutions—for example, you could have an algorithm that always decides null”."
  - id: termination
    prompt: "Why is termination the hard property?"
    options:
      - id: oa825c2
        text: "It is a safety property"
      - id: o45e66f
        text: "It formalizes fault tolerance: the algorithm must decide even if some nodes crash and never return"
        correct: true
      - id: o155611
        text: "It requires Byzantine tolerance"
      - id: o4b830f
        text: "It is trivially satisfied by a dictator"
    explanation: "“If you don’t care about fault tolerance, satisfying the first three properties is easy ... All the difficulty arises from the need for fault tolerance. The termination property formalizes the idea of fault tolerance.”"
  - id: majority-needed
    prompt: "How many nodes must be working for consensus to terminate?"
    options:
      - id: obb6937
        text: "All of them"
      - id: o71fb8f
        text: "At least two-thirds in crash models"
      - id: o2231ae
        text: "At least a majority"
        correct: true
      - id: oed7248
        text: "At least one"
    explanation: "“any consensus algorithm requires at least a majority of nodes to be functioning correctly in order to assure termination”."
  - id: safety-outage
    prompt: "What happens to a consensus system if a majority of nodes fails?"
    options:
      - id: o5eefd2
        text: "It switches to LWW"
      - id: oa060bb
        text: "It makes random decisions"
      - id: o7e1fdb
        text: "It stops making progress but still won't make inconsistent decisions"
        correct: true
      - id: oc32929
        text: "It corrupts its log"
    explanation: "“a large-scale outage can stop the system from being able to process requests, but it cannot corrupt the consensus system by causing it to make inconsistent decisions.”"
  - id: cas-consensus
    prompt: "How do you solve consensus given a fault-tolerant linearizable CAS?"
    options:
      - id: o03d6d2
        text: "Start from null; each proposer CASes from null to its value; the value set wins"
        correct: true
      - id: o238a8e
        text: "Each node increments a counter"
      - id: o1cb670
        text: "Use LWW"
      - id: ofaa9a5
        text: "Each node writes its value; the last write wins"
    explanation: "“Initially set the object to a null value, then have each node that wants to propose a value perform a CAS, with the expected value being null ... The decided value is then whatever value the object is set to.”"
  - id: log-props
    prompt: "Which are properties of a shared log?"
    type: multiple
    options:
      - id: o50d440
        text: "Eventual append"
        correct: true
      - id: o9f3f30
        text: "Agreement on the prefix before any entry"
        correct: true
      - id: obbc2de
        text: "Append-only immutability"
        correct: true
      - id: oeacbaa
        text: "Reliable delivery"
        correct: true
      - id: o3c9223
        text: "Entries may be reordered on reread"
    explanation: "The properties are “Eventual append”, “Reliable delivery”, “Append-only”, “Agreement” and “Validity”. Rereading yields “the same log entries in the same order”."
  - id: log-from-consensus
    prompt: "How do you build a shared log from single-value consensus?"
    options:
      - id: obdd2df
        text: "Use one consensus instance for the whole log"
      - id: o38886c
        text: "Append in arrival order without consensus"
      - id: odc2db8
        text: "Run a separate consensus instance per log slot; append decided values in slot order; retry undecided proposals in later slots"
        correct: true
      - id: oc663b6
        text: "Use vector clocks"
    explanation: "“You have a slot in the log for every future log entry, and you run a separate instance of the consensus algorithm for every such slot”."
  - id: fetch-add-number
    prompt: "What is the consensus number of fetch-and-add, and why?"
    options:
      - id: o03a678
        text: "2 — the losers learn they lost but not who won, so beyond two proposers they can be left hanging"
        correct: true
      - id: ob65c22
        text: "1"
      - id: o9de5a2
        text: "0"
      - id: o8d5a19
        text: "∞, like CAS"
    explanation: "“This solves the consensus problem for two nodes, which is why we can say that fetch-and-add has a consensus number of 2. In contrast, CAS and shared logs solve consensus for any number of nodes”."
  - id: atomic-commit-diff
    prompt: "How does atomic commitment differ from consensus?"
    options:
      - id: o7f3770
        text: "Consensus may decide any proposed value; atomic commit must abort if any participant votes abort"
        correct: true
      - id: o1cfb16
        text: "They are identical in every property"
      - id: ob07921
        text: "Consensus requires all votes yes"
      - id: od76c38
        text: "Atomic commit allows any value"
    explanation: "“with consensus it’s OK to decide any value that was proposed, whereas with atomic commitment the algorithm must abort if any of the participants voted to abort.”"
  - id: nontriviality
    prompt: "What does atomic commitment's nontriviality property prevent?"
    options:
      - id: o089032
        text: "An algorithm that always commits"
      - id: o5476c9
        text: "Timeouts"
      - id: od95983
        text: "Deadlocks"
      - id: o42ecbd
        text: "An algorithm that simply always aborts"
        correct: true
    explanation: "“the nontriviality property ensures that the algorithm can’t simply always abort (but it allows an abort if any of the communication among the nodes times out).”"
  - id: practical-form
    prompt: "Which formulation of consensus do most systems provide?"
    options:
      - id: ob3d9e3
        text: "Vector clocks"
      - id: o9e8efd
        text: "Fetch-and-add"
      - id: o202d50
        text: "Shared logs (total order broadcast)"
        correct: true
      - id: o147139
        text: "Single-value consensus only"
    explanation: "“most consensus systems provide shared logs ... Raft, Viewstamped Replication, and Zab provide shared logs right out of the box ... most systems using Paxos actually use the extension called Multi-Paxos”."
  - id: smr
    prompt: "What is state machine replication?"
    options:
      - id: o8b6baa
        text: "Copying WAL bytes"
      - id: o1bc126
        text: "Using LWW"
      - id: oc429fd
        text: "Replicating a finite-state machine diagram"
      - id: o9db3e7
        text: "Every replica applies the same deterministic writes from a shared log in the same order, reaching the same state"
        correct: true
    explanation: "“If every log entry represents a write to the database, and every replica processes the same writes in the same order by using deterministic logic, then all the replicas will end up in a consistent state. This idea is known as state machine replication”."
  - id: epochs
    prompt: "How do consensus algorithms avoid needing consensus to elect a leader?"
    options:
      - id: o66facf
        text: "They pick the node with the fastest clock"
      - id: oc2a4fc
        text: "They use a single dictator forever"
      - id: o83a559
        text: "They guarantee a unique leader per epoch; higher epochs win, and leaders check with a quorum before appending"
        correct: true
      - id: o264b44
        text: "They forbid leader changes"
    explanation: "They “define an epoch number ... and guarantee that within each epoch, the leader is unique” and “Before a leader is allowed to append the next entry ... it must first check that there isn’t another leader with a higher epoch”."
  - id: quorum-overlap
    prompt: "Why must the leader-election quorum and the proposal quorum overlap?"
    options:
      - id: o492269
        text: "They need not overlap"
      - id: of7abaf
        text: "To speed up writes"
      - id: o5d770c
        text: "So any successful proposal vote includes a node that saw the most recent election, revealing any higher epoch"
        correct: true
      - id: o99ea3e
        text: "To avoid fsync"
    explanation: "“if a vote on a proposal succeeds, at least one of the nodes that voted for it must have also participated in the most recent successful leader election”."
  - id: vs-2pc
    prompt: "How do consensus votes differ from 2PC votes?"
    options:
      - id: o5cb4f6
        text: "Consensus needs all nodes"
      - id: ob67c77
        text: "2PC needs only a quorum"
      - id: o936d47
        text: "They are identical"
      - id: o2fbb5d
        text: "Any node can start an election and a quorum suffices; in 2PC only the coordinator asks and every participant must say yes"
        correct: true
    explanation: "“In consensus algorithms, any node can start an election, and it requires only a quorum of nodes to respond; in 2PC, only the coordinator can request votes, and it requires a yes vote from every participant”."
  - id: raft-vs-paxos
    prompt: "How do Raft and Paxos differ in electing a leader with an incomplete log?"
    options:
      - id: od80c71
        text: "Both pick randomly"
      - id: o3ce573
        text: "Neither cares about logs"
      - id: o878f94
        text: "Paxos requires the most up-to-date node; Raft doesn't"
      - id: o04129a
        text: "Raft only elects a node whose log is at least as up-to-date as a majority's; Paxos lets any node lead but it must catch up first"
        correct: true
    explanation: "“Raft does this by allowing a node to become the new leader only if its log is at least as up-to-date as those of a majority of its followers. In contrast, Paxos allows any node to become the new leader, but requires it to bring its log up-to-date”."
  - id: unclean
    prompt: "What does Kafka's unclean leader election trade?"
    options:
      - id: of2a4dd
        text: "Durability for consistency"
      - id: od9da9e
        text: "Consistency for nothing"
      - id: o6c77ac
        text: "Faster or possible recovery at the risk of data loss, since a stale replica may lead"
        correct: true
      - id: ob4e01d
        text: "It is always safe"
    explanation: "“Kafka offers the option of enabling unclean leader election, which allows any replica to become leader, even if it is not up-to-date ... you are on thin ice, since the theory of consensus no longer applies.”"
  - id: lin-reads-raft
    prompt: "In a Raft-based database, what must linearizable reads do?"
    options:
      - id: ob4d193
        text: "Use LWW"
      - id: oaff87b
        text: "Go through a quorum check confirming the leader is still current, like etcd"
        correct: true
      - id: oe38828
        text: "Skip the leader"
      - id: odc66ba
        text: "Be served from any follower"
    explanation: "“If you want to guarantee linearizable reads, they also have to go through a quorum vote, similarly to a write ... Linearizable reads in etcd work like this”."
  - id: consensus-costs
    prompt: "Which are costs of consensus?"
    type: multiple
    options:
      - id: o7590d2
        text: "It allows split brain"
      - id: oe363d9
        text: "Timeout tuning is hard, especially across regions"
        correct: true
      - id: o756ed1
        text: "Adding nodes doesn't increase throughput (it slows things)"
        correct: true
      - id: o3c82fc
        text: "It needs a strict majority to operate"
        correct: true
    explanation: "“Consensus systems always require a strict majority ... you can’t increase throughput by adding more nodes ... tuning these timeouts can be difficult.”"
  - id: single-leader-done-right
    prompt: "How does the chapter summarize consensus?"
    options:
      - id: o063f8c
        text: "\"Single-leader replication done right\": automatic failover without losing committed data or split brain"
        correct: true
      - id: o3921d3
        text: "A caching layer"
      - id: ofa2265
        text: "A replacement for transactions"
      - id: oecb064
        text: "Multi-leader replication with LWW"
    explanation: "“Consensus is essentially “single-leader replication done right,” with automatic failover on leader failure, ensuring that no committed data is lost and split brain is not possible”."
  - id: raft-edge
    prompt: "What Raft edge case does the chapter describe, and what fixed it?"
    options:
      - id: o155c6d
        text: "Clock skew breaks elections; GPS fixed it"
      - id: o0bb716
        text: "One consistently unreliable link can make leadership bounce forever; a pre-vote phase was added"
        correct: true
      - id: o343851
        text: "Large logs crash nodes; compaction fixed it"
      - id: o0b9b09
        text: "Byzantine nodes; signatures fixed it"
    explanation: "“Raft can get into situations where leadership continually bounces between two nodes ... The original Raft algorithm was extended with a pre-vote phase to address this.”"
  - id: coord-features
    prompt: "Which features do coordination services offer?"
    type: multiple
    options:
      - id: o932528
        text: "Session-based failure detection (ephemeral nodes)"
        correct: true
      - id: o51cae6
        text: "High-volume general-purpose data storage"
      - id: o3cea91
        text: "Locks and leases via linearizable CAS"
        correct: true
      - id: o7f00d4
        text: "Change notifications"
        correct: true
      - id: o145951
        text: "Fencing tokens such as zxid or etcd revisions"
        correct: true
    explanation: "The chapter lists locks and leases, fencing support, failure detection and change notifications, and notes they “are not designed for high write volumes or general-purpose data storage”."
  - id: coord-small
    prompt: "Why run a coordination service on 3–5 nodes rather than consensus over thousands of storage nodes?"
    options:
      - id: o874902
        text: "It is cheaper to license"
      - id: o0c82d0
        text: "Consensus over thousands of nodes would be terribly inefficient; outsource it to a small cluster"
        correct: true
      - id: off52ec
        text: "Storage nodes have no network"
      - id: od90bae
        text: "ZooKeeper cannot run on more than five nodes"
    explanation: "“in a storage system with thousands of shards, running a consensus algorithm over thousands of nodes would be terribly inefficient; it’s much better to “outsource” the consensus to a small number of nodes”."
  - id: coord-data
    prompt: "What kind of data belongs in a coordination service?"
    options:
      - id: o250c9e
        text: "Data changing thousands of times per second"
      - id: o3bb90a
        text: "Large blobs"
      - id: o8b2685
        text: "Analytics tables"
      - id: of0f4a9
        text: "Slow-changing metadata such as \"node 10.1.1.23 leads shard 7\""
        correct: true
    explanation: "“the kind of data managed by a coordination service is quite slow-changing ... Coordination services are not intended for storing data that may change thousands of times per second.”"
  - id: discovery-overkill
    prompt: "Why is consensus often overkill for service discovery?"
    options:
      - id: o0a4328
        text: "It usually doesn't need linearizability; availability and speed matter more, so caching (as with DNS) is preferable"
        correct: true
      - id: o65c295
        text: "Discovery data never changes"
      - id: o7612dd
        text: "Consensus can't store IPs"
      - id: oa4f0d9
        text: "Service discovery needs Byzantine tolerance"
    explanation: "“using consensus for service discovery is often overkill. This use case generally doesn’t require linearizability, and it’s more important that service discovery is highly available and fast”."
  - id: observers
    prompt: "What are ZooKeeper observers?"
    options:
      - id: o3f8d09
        text: "Monitoring dashboards"
      - id: o8a1d55
        text: "Leaders in standby"
      - id: o663e1f
        text: "Clients that watch keys"
      - id: o25805f
        text: "Replicas that receive the log but don't vote, giving available, scalable but possibly stale reads"
        correct: true
    explanation: "Observers “receive the log and maintain a copy of the data ... but do not participate in the consensus algorithm’s voting process. Reads from an observer are not linearizable”."
  - id: summary-advice
    prompt: "What does the summary advise if you need something reducible to consensus to be fault-tolerant?"
    options:
      - id: o3e956a
        text: "Avoid fault tolerance"
      - id: o9982b6
        text: "Use LWW"
      - id: oc4e39d
        text: "Write your own Paxos"
      - id: oafbfbb
        text: "Use a coordination service rather than implementing consensus yourself"
        correct: true
    explanation: "“If you find yourself wanting to do one of those things that is reducible to consensus, and you want it to be fault-tolerant, it is advisable to use a coordination service.”"
---
Getting nodes to agree. Many problems — CAS, shared logs, fetch-and-add, atomic commit — turn out to be
consensus in disguise. Raft, Paxos, Zab and VR solve it with epochs and quorums; coordination services
package it for applications.

## Learning objectives

- State FLP and why consensus is still achievable in practice
- State the four properties of consensus and which are safety vs liveness
- Explain equivalences: CAS, shared log/total order broadcast, fetch-and-add (consensus number 2), atomic commit
- Explain epochs and two rounds of quorum voting; contrast with 2PC
- Weigh the costs of consensus and the uses of coordination services

## In a nutshell

- FLP: no deterministic algorithm always terminates in the asynchronous model; timeouts or randomness escape it.
- Uniform agreement, integrity, validity (safety) + termination (liveness, needs a majority).
- Most systems expose a shared log; state machine replication follows.
- Within each epoch the leader is unique; higher epoch wins.
- Consensus = "single-leader replication done right"; costs: majority required, no throughput scaling, timeout tuning.
- ZooKeeper/etcd: leases, fencing tokens, sessions, notifications — for slow-changing metadata.

## Connections to today

- **Kafka KRaft** and **Kubernetes on etcd** are consensus in everyday infrastructure.
- **Raft pre-vote and check-quorum** settings exist because of the flapping-leadership edge cases the
  chapter mentions.
- **Unclean leader election** in Kafka is the explicit availability-over-consistency switch the sidebar
  describes.
