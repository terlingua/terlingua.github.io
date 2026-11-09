---
title: "Sharding of Key-Value Data"
questions:
  - id: skew
    prompt: "What is a hot spot?"
    options:
      - id: o0f2abe
        text: "A shard with disproportionately high load"
        correct: true
      - id: o8340cd
        text: "A node with extra disk space"
      - id: o1d55ea
        text: "A shard that is replicated"
      - id: oaf4cdd
        text: "A key that is never accessed"
    explanation: "“A shard with disproportionately high load is called a hot shard or hot spot. If one key has a particularly high load ... we call it a hot key.”"
  - id: encyclopedia
    prompt: "Why are encyclopedia volumes not split evenly by letter?"
    options:
      - id: o4b4e5f
        text: "Some letters are reserved"
      - id: oa45419
        text: "Printers require equal page counts"
      - id: oae3f6c
        text: "Data isn't evenly distributed, so boundaries must adapt to the data"
        correct: true
      - id: of97fe9
        text: "Alphabetical order is not stable"
    explanation: "“Simply having one volume per two letters of the alphabet would lead to some volumes being much bigger than others. To distribute data evenly, the shard boundaries need to adapt to the data.”"
  - id: timestamp-hot
    prompt: "Sensor readings are keyed by timestamp and sharded by key range. What happens?"
    options:
      - id: o80e751
        text: "Reads become impossible"
      - id: o1548bf
        text: "Range queries become slow"
      - id: o8ea69f
        text: "Writes spread evenly"
      - id: of3caab
        text: "All current writes go to the shard for the latest time range, overloading it while others idle"
        correct: true
    explanation: "“If you write data from the sensors to the database as the measurements happen, all the writes will end up going to the same shard (the one for this month)”."
  - id: sensor-prefix
    prompt: "You prefix the key with sensor ID. What do you gain and lose?"
    options:
      - id: od7ec5a
        text: "Nothing changes"
      - id: oa782e0
        text: "Writes spread across shards, but fetching many sensors over a time range needs one range query per sensor"
        correct: true
      - id: oed4ad5
        text: "Range queries per sensor become impossible"
      - id: o7f591e
        text: "Writes concentrate further"
    explanation: "“Assuming you have many sensors active at the same time, the write load will end up more evenly spread ... The downside is that when you want to fetch the values of multiple sensors within a time range, you now need to perform a separate range query for each sensor.”"
  - id: presplit
    prompt: "What is pre-splitting, and what does it require?"
    options:
      - id: o5541d1
        text: "Replicating before sharding"
      - id: od9353f
        text: "Configuring initial shards on an empty database; it requires knowing the likely key distribution"
        correct: true
      - id: o9b299e
        text: "Hashing keys before writing"
      - id: o0a48d3
        text: "Splitting shards before every write"
    explanation: "HBase and MongoDB “allow you to configure an initial set of shards on an empty database, which is called pre-splitting. This requires that you already have some idea of what the key distribution is going to look like”."
  - id: split-cost
    prompt: "Why can splitting a hot shard make things worse?"
    options:
      - id: ofc5fee
        text: "It disables replication"
      - id: o0271be
        text: "Splitting rewrites all its data, adding load to a shard that is already under high load"
        correct: true
      - id: ob558a2
        text: "It makes the shard read-only forever"
      - id: o9d9380
        text: "It deletes data"
    explanation: "“splitting a shard is an expensive operation ... A shard that needs splitting is often also one that is under high load, and the cost of splitting can exacerbate that load”."
  - id: split-trigger
    prompt: "On systems with automatic key-range splitting, what can trigger a split?"
    type: multiple
    options:
      - id: ob4d93d
        text: "Persistently high write throughput, even with little data"
        correct: true
      - id: oba6e9c
        text: "Reaching a size threshold (e.g., 10 GB on HBase)"
        correct: true
      - id: o0ef77f
        text: "A user query"
      - id: of0095f
        text: "A schema change"
    explanation: "“a shard split is typically triggered by the shard reaching a configured size (e.g., on HBase, the default is 10 GB) or, in some systems, the write throughput being persistently above a certain threshold.”"
  - id: hash-functions
    prompt: "Why are Java's `Object.hashCode()` and Ruby's `Object#hash` unsuitable for sharding?"
    options:
      - id: oeb2c4c
        text: "They produce only positive numbers"
      - id: o9aee35
        text: "They are cryptographically weak"
      - id: obe33e4
        text: "They are too slow"
      - id: oa124ed
        text: "The same key may hash differently in different processes"
        correct: true
    explanation: "“the same key may have a different hash value in different processes, making them unsuitable for sharding”. The hash need not be cryptographic: “MongoDB uses MD5, whereas Cassandra and ScyllaDB use Murmur3.”"
  - id: mod-n
    prompt: "Why is `hash(key) % N` bad for assigning keys to nodes?"
    options:
      - id: o124027
        text: "It is slow to compute"
      - id: o272a4b
        text: "When N changes, most keys must move to a different node"
        correct: true
      - id: off09dc
        text: "It only works for strings"
      - id: o95f5f6
        text: "It doesn't distribute keys"
    explanation: "“if the number of nodes N changes, most of the keys have to be moved from one node to another ... it leads to very inefficient rebalancing”."
  - id: fixed-shards
    prompt: "With 1,000 fixed shards on 10 nodes, what changes when you add an 11th node?"
    options:
      - id: oe97d40
        text: "Every key is rehashed"
      - id: o3ddeb9
        text: "Nothing moves"
      - id: obde9d0
        text: "The number of shards grows to 1,100"
      - id: o077367
        text: "Only the assignment of some whole shards to nodes; keys stay in the same shards"
        correct: true
    explanation: "“only entire shards are moved between nodes ... The number of shards does not change, nor does the assignment of keys to shards. The only thing that changes is the assignment of shards to nodes.”"
  - id: fixed-limits
    prompt: "Which are limitations of a fixed number of shards?"
    type: multiple
    options:
      - id: obff612
        text: "Resharding when the count is wrong is expensive"
        correct: true
      - id: obcaed4
        text: "Shards can never be moved between nodes"
      - id: oedd544
        text: "You can't have more nodes than shards"
        correct: true
      - id: o5fe462
        text: "Shard size grows with total data, so the \"right\" size is hard to maintain"
        correct: true
    explanation: "“you can’t have more nodes than you have shards”, “an expensive resharding operation is required”, and “the size of each shard grows proportionally to the total amount of data”."
  - id: uneven-hardware
    prompt: "How can a fixed-shard scheme accommodate more powerful nodes?"
    options:
      - id: oef3fdd
        text: "It cannot"
      - id: o0990b5
        text: "Give them bigger hash values"
      - id: o119062
        text: "Replicate more to them"
      - id: oe477e8
        text: "Assign more shards to them"
        correct: true
    explanation: "“by assigning more shards to nodes that are more powerful, you can make those nodes take on a greater share of the load.”"
  - id: hash-range
    prompt: "What does hash-range sharding combine, and what does it give up?"
    options:
      - id: of6b4e4
        text: "Hashing plus adaptable range splitting; it gives up efficient range queries over the partition key"
        correct: true
      - id: ob2ebfb
        text: "Replication plus sharding; it gives up consistency"
      - id: o193c59
        text: "Fixed shards plus ordering; it gives up uniformity"
      - id: of24db6
        text: "Key ranges plus Bloom filters; it gives up splitting"
    explanation: "“combine key-range sharding with a hash function so that each shard contains a range of hash values ... The downside compared to key-range sharding is that range queries over the partition key are not efficient”."
  - id: compound-key
    prompt: "Keys are (user_id, timestamp), hash-sharded on user_id. Can you efficiently fetch one user's posts in a date range?"
    options:
      - id: of658d8
        text: "No — hashing destroys all ordering"
      - id: o7e1319
        text: "Only with a global index"
      - id: o36071f
        text: "Only on key-range sharded systems"
      - id: oe0c0c3
        text: "Yes — all that user's records share a partition key and are in one shard, sorted by timestamp"
        correct: true
    explanation: "“if keys consist of two or more columns and the partition key is only the first of these columns, you can still perform efficient range queries over the second and later columns.”"
  - id: cassandra-ranges
    prompt: "How do Cassandra and ScyllaDB split the hash space?"
    options:
      - id: o70ccb4
        text: "By key range, not hash"
      - id: o346a84
        text: "Into exactly one range per node"
      - id: o5b2231
        text: "Into many ranges per node (16 in Cassandra, 256 in ScyllaDB) with random boundaries that even out"
        correct: true
      - id: ob9289f
        text: "Into equal ranges fixed at creation"
    explanation: "“actual numbers are 16 per node in Cassandra by default, and 256 per node in ScyllaDB), with random boundaries ... by having multiple ranges per node, those imbalances tend to even out”."
  - id: consistent-hashing
    prompt: "Which properties define consistent hashing?"
    type: multiple
    options:
      - id: oe8f66b
        text: "It guarantees replica consistency"
      - id: o7030da
        text: "Roughly equal numbers of keys per shard"
        correct: true
      - id: o1edbce
        text: "Keys are ordered"
      - id: oc344ab
        text: "Minimal key movement when the number of shards changes"
        correct: true
    explanation: "“The number of keys mapped to each shard is roughly equal. When the number of shards changes, as few keys as possible are moved”. And “consistent here has nothing to do with replica consistency”."
  - id: skew-load
    prompt: "Consistent hashing spreads keys evenly. Why can load still be uneven?"
    options:
      - id: oc14335
        text: "Shards have different disks"
      - id: o3d548f
        text: "Some keys get far more data or requests than others, e.g., a celebrity's post"
        correct: true
      - id: od437c8
        text: "Consistent hashing only works for strings"
      - id: odff883
        text: "Hash functions are biased"
    explanation: "“Consistent hashing ensures that keys are uniformly distributed across nodes, but that doesn’t mean that the actual load is uniformly distributed.”"
  - id: random-suffix
    prompt: "You append two random digits to a hot key. What's the effect?"
    options:
      - id: o00b139
        text: "The key becomes cold"
      - id: o25ab32
        text: "Writes spread over 100 keys, but reads must query all 100 and combine; read load per shard isn't reduced"
        correct: true
      - id: oca597d
        text: "Reads and writes both become cheaper"
      - id: o280066
        text: "Nothing changes"
    explanation: "“Adding just two random digits would split the writes to the key evenly across 100 keys ... any reads now have to do additional work ... The volume of reads to each shard of the hot key is not reduced; only the write load is split.”"
  - id: hot-bookkeeping
    prompt: "Why apply random-suffix splitting only to a few keys?"
    options:
      - id: o6e96b6
        text: "It requires a global index"
      - id: od902be
        text: "It breaks hashing"
      - id: oec07bb
        text: "It only works on strings"
      - id: o6eba74
        text: "For most low-throughput keys it is unnecessary overhead, so you must track which keys are split"
        correct: true
    explanation: "“it makes sense to append the random number for only the small number of hot keys; for the vast majority of keys with low write throughput, this would be unnecessary overhead. Thus, you also need some way of keeping track of which keys are being split”."
  - id: auto-rebalance-danger
    prompt: "Why can automatic rebalancing combined with automatic failure detection be dangerous?"
    options:
      - id: ofbac53
        text: "It disables replication"
      - id: o3919ba
        text: "A slow, overloaded node may be declared dead, and moving its load overloads others — a cascading failure"
        correct: true
      - id: o967499
        text: "Failure detectors never misjudge"
      - id: oae55de
        text: "Rebalancing always deletes data"
    explanation: "“The other nodes conclude that the overloaded node is dead, and automatically rebalance the cluster to move load away from it. This puts additional load on other nodes and the network, making the situation worse.”"
  - id: manual-rebalance
    prompt: "When is manual rebalancing especially useful?"
    options:
      - id: o70fd12
        text: "Preemptively before a known traffic surge, such as Cyber Monday or a World Cup ticket sale"
        correct: true
      - id: o5b0f27
        text: "Never; automation is always better"
      - id: o9eac53
        text: "Only on single-node databases"
      - id: o5ad59a
        text: "When nodes fail unexpectedly"
    explanation: "“Manual rebalancing is also useful for preemptively rebalancing if a surge in traffic is expected because of a known event, such as Cyber Monday holiday sales or ticket sales”."
---
How to map keys to shards: key ranges, hashes, fixed shard counts, hash ranges and consistent hashing;
then hot spots and who decides to rebalance.

## Learning objectives

- Compare key-range and hash sharding for range queries and hot spots
- Explain why mod-N is bad and how fixed shard counts and hash ranges fix it
- Define consistent hashing
- Relieve hot keys and weigh automatic versus manual rebalancing

## In a nutshell

- Key range: efficient range scans; risk of sequential-key hot spots; split shards as they grow.
- Hash: uniform spread; range queries over the partition key are lost.
- Fixed shards (e.g., 1,000 for 10 nodes): move whole shards; resharding is expensive.
- Hash range: adapts shard count; Cassandra uses many random ranges per node.
- Compound keys: partition by the first column, range-scan the rest.
- Hot keys need splitting (random suffixes) at the cost of fan-in reads.

## Connections to today

- **DynamoDB adaptive capacity** isolates hot items automatically — the "heat management" the chapter
  mentions.
- **Clustering keys in BigQuery, Snowflake and Delta Lake** (liquid clustering) mirror the
  partition-then-sort pattern described in the sidebar.
- **UUIDv7 primary keys** are time-ordered; they help B-tree locality but create the timestamp hot-spot
  problem under key-range sharding.
