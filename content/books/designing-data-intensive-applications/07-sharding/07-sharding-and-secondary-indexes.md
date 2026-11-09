---
title: "Sharding and Secondary Indexes"
questions:
  - id: local-def
    prompt: "What is a local secondary index?"
    options:
      - id: o3fd673
        text: "Each shard indexes only its own records (document-partitioned)"
        correct: true
      - id: o70bb94
        text: "An index partitioned by term"
      - id: o429017
        text: "An index stored in the client"
      - id: oa3ef7b
        text: "One index covering all shards on one node"
    explanation: "“each shard independently maintains its own secondary indexes, covering only the records in that shard ... this type of secondary index is known as a local index ... a document-partitioned index”."
  - id: local-read
    prompt: "Searching all red cars with local indexes requires what?"
    options:
      - id: o9e10b2
        text: "Querying all shards and combining results"
        correct: true
      - id: of2bd01
        text: "Rebuilding the index"
      - id: o79670e
        text: "Querying one shard"
      - id: o4a7cc4
        text: "A distributed transaction"
    explanation: "“if you want all the results and don’t know their partition key in advance, you will need to send the query to all shards and combine the results”."
  - id: local-scale
    prompt: "Why does adding shards not raise query throughput for local-index queries?"
    options:
      - id: o5816cf
        text: "Every shard must process every such query"
        correct: true
      - id: o627138
        text: "Replicas can't serve index queries"
      - id: o5a2708
        text: "Indexes can't be sharded"
      - id: o34c370
        text: "Queries are serialized"
    explanation: "“adding more shards lets you store more data, but it doesn’t increase your query throughput if every shard has to process every query anyway.”"
  - id: local-any-shard
    prompt: "When can a local-index query go to just one shard?"
    options:
      - id: oc4d7ec
        text: "When the term is rare"
      - id: o17fee6
        text: "Never"
      - id: o9b39ae
        text: "When the index is global"
      - id: o48de4c
        text: "When you know the partition key, or when you only need some results"
        correct: true
    explanation: "“if you already know the partition key of the record you’re looking for, you can just perform the search on the appropriate shard. Moreover, if you want only some results and don’t need all of them, you can send the request to any shard.”"
  - id: diy-index
    prompt: "Why is hand-rolling a secondary index in a key-value store risky?"
    options:
      - id: ofb44b3
        text: "It is always slower than scanning"
      - id: o26948c
        text: "Race conditions and partial write failures easily let index and data go out of sync"
        correct: true
      - id: o99a3bf
        text: "It breaks hashing"
      - id: od487e5
        text: "Key-value stores forbid extra keys"
    explanation: "“Race conditions and intermittent write failures (where some changes were saved but others weren’t) can very easily cause the data to go out of sync”."
  - id: global-def
    prompt: "Why can't a global secondary index live on one node?"
    options:
      - id: o032062
        text: "It would become a bottleneck; it must itself be sharded, by the indexed term"
        correct: true
      - id: oba9980
        text: "Global indexes are stored on clients"
      - id: o6773e5
        text: "It would be too small"
      - id: oc4e79f
        text: "Indexes can't be replicated"
    explanation: "“we can’t just store that index on one node, since it would likely become a bottleneck and defeat the purpose of sharding. A global index must also be sharded, but it can be sharded differently from the primary-key index.”"
  - id: global-read
    prompt: "A query `color = red` uses a global index. How many index shards must it read, and what then?"
    options:
      - id: oca7ba6
        text: "All index shards"
      - id: oef99f6
        text: "One shard for everything"
      - id: o2cd267
        text: "One to get the postings list, then possibly many data shards to fetch the records"
        correct: true
      - id: oe93d37
        text: "None; IDs are enough"
    explanation: "“a query with a single condition (such as color = red) needs to read from only a single shard to fetch the postings list. However, if you want to fetch records and not just IDs, you still have to read from all the shards that are responsible for those IDs.”"
  - id: global-and
    prompt: "Why can `color = red AND make = Ford` be slow with a global index?"
    options:
      - id: o3c2de5
        text: "AND is not supported"
      - id: od574fd
        text: "It requires local indexes"
      - id: o2b2fc6
        text: "The two postings lists may live on different shards and must be sent over the network to intersect"
        correct: true
      - id: o4e5a8e
        text: "It scans all records"
    explanation: "“those terms will likely be assigned to different shards. To compute the logical AND ... if they are long, it can be slow to send them over the network to compute their intersection.”"
  - id: global-write
    prompt: "Why are writes harder with global indexes?"
    options:
      - id: oc5ab1a
        text: "Index entries are immutable"
      - id: obd861e
        text: "One record write may affect several index shards, making it harder to keep the index in sync"
        correct: true
      - id: o800093
        text: "Writes are forbidden"
      - id: o5a6708
        text: "Writes must go to every data shard"
    explanation: "“writing a single record might affect multiple shards of the index (every term in the document might be on a different shard). This makes it harder to keep the secondary index in sync”."
  - id: dynamodb-gsi
    prompt: "What consistency do DynamoDB global indexes provide?"
    options:
      - id: odf06a6
        text: "They are never updated"
      - id: o1c84b1
        text: "They are updated in the same transaction always"
      - id: oc1c60c
        text: "Writes are reflected asynchronously, so reads may be stale"
        correct: true
      - id: o30070d
        text: "Strictly serializable reads"
    explanation: "“In the case of DynamoDB, writes are asynchronously reflected in global indexes, so reads from a global index may be stale”."
  - id: when-global
    prompt: "When are global indexes most useful?"
    options:
      - id: o05a7c3
        text: "When writes dominate"
      - id: o18e1c7
        text: "Always"
      - id: o704976
        text: "When postings lists are very long"
      - id: obcaf7e
        text: "When read throughput exceeds write throughput and postings lists are not too long"
        correct: true
    explanation: "“global indexes are useful if read throughput is higher than write throughput, and if the postings lists are not too long.”"
---
Secondary indexes don't map neatly to shards. They can be local (document-partitioned) or global
(term-partitioned).

## Learning objectives

- Compare local and global secondary indexes for reads and writes
- Explain scatter/gather and why it limits scalability
- Explain why global indexes are often updated asynchronously

## In a nutshell

- Local: writes touch one shard; reads by index scatter to all shards.
- Global: a single-term lookup hits one index shard; writes may touch many; multi-term ANDs move postings lists over the network.
- DIY indexes in a key-value store are easy to get out of sync.

## Connections to today

- **DynamoDB GSIs** are eventually consistent, the situation the chapter describes.
- **Elasticsearch** uses local (document-partitioned) shards and scatter/gather queries.
- **Spanner and CockroachDB** keep global indexes transactionally consistent using distributed
  transactions.
