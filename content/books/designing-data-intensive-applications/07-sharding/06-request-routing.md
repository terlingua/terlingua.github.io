---
title: "Request Routing"
questions:
  - id: routing-diff
    prompt: "How does request routing differ from service discovery for stateless services?"
    options:
      - id: oe5b43d
        text: "It uses only DNS"
      - id: o39822a
        text: "A key can only be handled by a replica of its shard, not by any instance"
        correct: true
      - id: o4b7e53
        text: "It requires no metadata"
      - id: o949a51
        text: "It doesn't involve IP addresses"
    explanation: "“with services running application code, each instance is usually stateless ... With sharded databases, a request for a key can be handled only by a node that is a replica for the shard containing that key.”"
  - id: three-approaches
    prompt: "Which are the three routing approaches?"
    type: multiple
    options:
      - id: o046ee1
        text: "Shard-aware clients that connect directly"
        correct: true
      - id: oc532a2
        text: "Broadcast every request to all nodes"
      - id: oeec468
        text: "Contact any node, which forwards if needed"
        correct: true
      - id: ob53a6e
        text: "A shard-aware routing tier"
        correct: true
    explanation: "The three approaches: “Allow clients to contact any node”, “Send all requests from clients to a routing tier first”, and “Require that clients be aware of the sharding”."
  - id: coordinator-problems
    prompt: "Which problems must routing solve?"
    type: multiple
    options:
      - id: o2dab76
        text: "Making the placement decision fault-tolerant without split brain"
        correct: true
      - id: ob5b45c
        text: "Choosing the hash function per request"
      - id: o6bdec0
        text: "Propagating assignment changes to routers"
        correct: true
      - id: o321828
        text: "Handling in-flight requests during a shard move"
        correct: true
    explanation: "The chapter asks who decides with a fault-tolerant coordinator avoiding “a split-brain situation”, “How does the component performing the routing ... learn about changes”, and how to handle “requests to the old node may still be in flight”."
  - id: zookeeper
    prompt: "What role does ZooKeeper play in HBase-style routing?"
    options:
      - id: oc847fd
        text: "It stores all data"
      - id: ocf145a
        text: "It hashes keys"
      - id: o50fa07
        text: "It replaces the routing tier"
      - id: o508074
        text: "It maintains the authoritative shard-to-node mapping, using consensus, and notifies routers of changes"
        correct: true
    explanation: "“ZooKeeper maintains the authoritative mapping of shards to nodes ... Whenever a shard changes ownership, or a node is added or removed, ZooKeeper notifies the routing tier”."
  - id: gossip
    prompt: "What is the downside of Riak's gossip-based approach, and why is it tolerable?"
    options:
      - id: o11b639
        text: "It loses data; tolerable because of backups"
      - id: o4375c6
        text: "It is slower than consensus; tolerable because Riak is small"
      - id: o89bf33
        text: "Weaker consistency, allowing split brain over shard assignment; tolerable because leaderless databases make weak guarantees anyway"
        correct: true
      - id: o8db5dd
        text: "It needs a leader; tolerable because of failover"
    explanation: "Gossip “provides much weaker consistency than a consensus protocol; it is possible to have split brain ... Leaderless databases can tolerate this because they generally make weak consistency guarantees anyway”."
  - id: dns-ok
    prompt: "Why is DNS often sufficient for clients to find routing-tier IPs?"
    options:
      - id: o06df79
        text: "DNS updates instantly"
      - id: o42c5ff
        text: "Routers have no IP addresses"
      - id: oa4b3c7
        text: "Router IPs change much less often than shard-to-node assignments"
        correct: true
      - id: o6f5fa1
        text: "DNS knows shard assignments"
    explanation: "“These are not as fast-changing as the assignment of shards to nodes, so it is often sufficient to use DNS for this purpose.”"
---
Which node owns this key? Routing can live in any node, a routing tier, or a shard-aware client, and
usually depends on a coordination service.

## Learning objectives

- Compare the three request routing approaches
- Explain the role of ZooKeeper, etcd or built-in Raft in tracking shard assignment
- Contrast gossip-based routing with consensus-based routing

## In a nutshell

- Unlike stateless services, a key can only be served by the replicas of its shard.
- Questions: who decides placement, how routers learn changes, how to handle in-flight requests during moves.
- Coordination services provide fault tolerance and split-brain protection; gossip is weaker.

## Connections to today

- **Kafka KRaft** replaced ZooKeeper with a built-in Raft quorum — one of the systems the chapter lists.
- **Vitess's VTGate** and **MongoDB's mongos** are routing tiers.
- **Smart clients** (e.g., Cassandra drivers with token-aware routing) are shard-aware clients.
