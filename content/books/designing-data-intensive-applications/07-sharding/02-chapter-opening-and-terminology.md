---
title: "Chapter Opening — Shards, Partitions and Replication"
questions:
  - id: shard-vs-replica
    prompt: "How do sharding and replication combine?"
    options:
      - id: oc19057
        text: "Each record belongs to exactly one shard, and each shard is copied to several nodes"
        correct: true
      - id: oed796c
        text: "Replicas hold different records from their leader"
      - id: odd602b
        text: "Each record is in every shard"
      - id: o4e96ce
        text: "Sharding replaces replication"
    explanation: "“even though each record belongs to exactly one shard, it may still be stored on several different nodes for fault tolerance.”"
  - id: node-roles
    prompt: "In single-leader sharded replication, what roles can one node have?"
    options:
      - id: ob76627
        text: "Either all leaders or all followers"
      - id: of7bce9
        text: "Leader for all shards"
      - id: o8a0b21
        text: "Follower only"
      - id: o770eb5
        text: "Leader for some shards and follower for others"
        correct: true
    explanation: "“Each node may be the leader for some shards and a follower for other shards, but each shard still has only one leader.”"
  - id: pg-partitioning
    prompt: "In PostgreSQL, how does partitioning differ from sharding?"
    options:
      - id: o7d4edc
        text: "Partitioning spans machines; sharding does not"
      - id: o47d248
        text: "Partitioning splits a table into files on the same machine; sharding splits data across machines"
        correct: true
      - id: o7864b7
        text: "They are identical"
      - id: o21057d
        text: "Partitioning only applies to indexes"
    explanation: "“in PostgreSQL, partitioning is a way of splitting a large table into several files that are stored on the same machine ... whereas sharding splits a dataset across multiple machines”."
  - id: names
    prompt: "Which system calls its shards \"tablets\"?"
    options:
      - id: o26afc3
        text: "Kafka"
      - id: oe973d5
        text: "Bigtable"
        correct: true
      - id: of61625
        text: "HBase"
      - id: o78babf
        text: "CockroachDB"
    explanation: "A shard is “called a partition in Kafka, a range in CockroachDB, a region in HBase and TiDB ... and a tablet in Bigtable, YugabyteDB, and ScyllaDB”."
  - id: netsplit
    prompt: "Is sharding related to network partitions?"
    options:
      - id: o832828
        text: "Yes — they are synonyms"
      - id: o6d6872
        text: "Yes — shards form when the network splits"
      - id: o7e1046
        text: "No — network partitions are a type of network fault"
        correct: true
      - id: o72d247
        text: "Only in Kafka"
    explanation: "“partitioning has nothing to do with network partitions (netsplits), a type of fault in the network between nodes.”"
---
Sharding splits data so each record belongs to exactly one shard; replication then copies each shard.

## Learning objectives

- Distinguish sharding from replication
- Recognize the many names for shards

## In a nutshell

- Each node may lead some shards and follow for others.
- Partition (Kafka), range (CockroachDB), region (HBase, TiDB), tablet (Bigtable)… all mean shard.
- PostgreSQL "partitioning" splits a table on one machine; sharding splits across machines.
- Network partitions are unrelated.

## Connections to today

- **Citus and Neon/Postgres sharding extensions** bring true sharding to PostgreSQL, distinct from its
  built-in table partitioning.
