---
title: "Pros and Cons of Sharding"
questions:
  - id: primary-reason
    prompt: "What is the primary reason to shard?"
    options:
      - id: o58ebd9
        text: "Scalability when data volume or write throughput exceeds one node"
        correct: true
      - id: od4dbf5
        text: "Read throughput"
      - id: oaeaa42
        text: "Fault tolerance"
      - id: o0452f5
        text: "Lower latency for single-key reads"
    explanation: "“The primary reason for sharding a database is scalability ... (If read throughput is the problem, you don’t necessarily need sharding—you can use read scaling”."
  - id: heavyweight
    prompt: "Your data fits easily on one modern server. What does the chapter advise?"
    options:
      - id: o8afd31
        text: "Shard anyway for future growth"
      - id: off123f
        text: "Shard by user ID to be safe"
      - id: o0d829c
        text: "Use multi-leader replication instead"
      - id: o1176e2
        text: "Avoid sharding; it is a heavyweight solution mostly relevant at large scale"
        correct: true
    explanation: "“sharding is a heavyweight solution that is mostly relevant at large scale. If your data volume and write throughput are such that a single machine can handle them ... it’s often better to avoid sharding”."
  - id: partition-key
    prompt: "Why does choosing the partition key matter so much?"
    options:
      - id: oa18876
        text: "It sets the encryption key"
      - id: o2a3116
        text: "It determines the table name"
      - id: o070c97
        text: "Accessing a record is fast only if you know its shard; otherwise you search all shards, and the scheme is hard to change"
        correct: true
      - id: o405092
        text: "It determines the replication factor"
    explanation: "“accessing a record is fast if you know which shard it’s in, but if you don’t, you have to do an inefficient search across all shards. The sharding scheme is also difficult to change.”"
  - id: cross-shard
    prompt: "What is needed to keep related records in several shards consistent on write?"
    options:
      - id: o5136a9
        text: "A Bloom filter"
      - id: of9626f
        text: "A read replica"
      - id: of191cf
        text: "Nothing; shards update atomically"
      - id: o3a4c3e
        text: "A distributed transaction, usually much slower than single-node transactions"
        correct: true
    explanation: "“ensuring consistency across multiple shards requires a distributed transaction ... they are usually much slower than single-node transactions and may become a bottleneck”."
  - id: per-core
    prompt: "Why do Redis, VoltDB and FoundationDB shard within a single machine?"
    options:
      - id: o8530be
        text: "To survive disk failures"
      - id: o7844e7
        text: "To reduce memory usage"
      - id: o58285b
        text: "To use CPU parallelism or NUMA by running a single-threaded process per core"
        correct: true
      - id: o3442c6
        text: "To support SQL joins"
    explanation: "“Some systems use sharding even on a single machine, typically running one single-threaded process per CPU core to make use of the parallelism in the CPU or to take advantage of a nonuniform memory access (NUMA) architecture”."
---
Sharding buys write and storage scalability at the price of complexity.

## Learning objectives

- Explain when sharding is and isn't warranted
- List the costs: partition key choice, cross-shard queries and transactions

## In a nutshell

- Shard for data volume or write throughput; use read replicas for read throughput.
- Prefer a single machine if it can cope.
- Cross-shard writes need distributed transactions, which are slow.
- Some systems shard per CPU core (Redis, VoltDB, FoundationDB).

## Connections to today

- **Large single nodes** with hundreds of cores and terabytes of RAM push the point at which you need to
  shard well beyond where it was a decade ago.
- **Thread-per-core designs** (ScyllaDB's Seastar, Redpanda) shard within a machine, like the chapter's
  examples.
