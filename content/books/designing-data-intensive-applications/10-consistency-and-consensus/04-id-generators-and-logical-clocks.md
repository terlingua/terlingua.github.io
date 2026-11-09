---
title: "ID Generators and Logical Clocks"
questions:
  - id: autoinc-problems
    prompt: "Which are problems with a single-node ID generator?"
    type: multiple
    options:
      - id: od248c7
        text: "It is a single point of failure"
        correct: true
      - id: oc87652
        text: "Cross-region requests need a round trip"
        correct: true
      - id: od1a3b3
        text: "It can become a throughput bottleneck"
        correct: true
      - id: o51c503
        text: "It cannot order IDs"
    explanation: "“A single-node ID generator is not fault-tolerant ... It’s slow if you want to create a record in another region ... That single node could become a bottleneck”."
  - id: sharded-ids
    prompt: "Two nodes hand out odd and even IDs. You see messages 16 and 17. What can you conclude?"
    options:
      - id: o8a387d
        text: "Nothing about which was sent first, because different nodes assigned them"
        correct: true
      - id: o3f38b9
        text: "They were concurrent"
      - id: o040d53
        text: "16 was sent first"
      - id: oeabe34
        text: "17 was sent first"
    explanation: "“if you have chat messages with IDs 16 and 17, you don’t know whether message 16 was actually sent first, because the IDs were assigned by different nodes”."
  - id: timestamp-ids
    prompt: "How do UUIDv7, Snowflake and ULID generate IDs, and what's their ordering guarantee?"
    options:
      - id: o44ffbd
        text: "Pure random bits; no ordering"
      - id: o05660b
        text: "Lamport counters; causal"
      - id: o73a834
        text: "Timestamp in the high bits plus unique filler; ordering is only approximate and not linearizable"
        correct: true
      - id: of12548
        text: "A central counter; linearizable"
    explanation: "They put “a timestamp from this clock in the most significant bits and filling the remaining bits with extra information that ensures the ID is unique”. “ID generators based on wall-clock time are therefore unlikely to be linearizable.”"
  - id: logical-reqs
    prompt: "Which are requirements for a logical clock?"
    type: multiple
    options:
      - id: oa1e03c
        text: "Timestamps show the time of day"
      - id: oc1fc4d
        text: "Compact, unique timestamps"
        correct: true
      - id: o80a3f6
        text: "Any two timestamps can be compared (total order)"
        correct: true
      - id: ob9c1b1
        text: "Order consistent with causality"
        correct: true
    explanation: "The three requirements: compact and unique, totally ordered, and “consistent with causality”. A logical clock “doesn’t tell you what time it is”."
  - id: lamport-rule
    prompt: "How does a Lamport clock update when a node receives a timestamp with a larger counter?"
    options:
      - id: o2f61ea
        text: "It resets to zero"
      - id: od1a750
        text: "It raises its local counter to that value; generating a timestamp increments the counter"
        correct: true
      - id: od21c14
        text: "It takes the average"
      - id: o19411f
        text: "It ignores remote timestamps"
    explanation: "“Every time a node sees a timestamp from another node, if the counter value in that timestamp is greater than its local counter value, it increases its local counter to match the value in the timestamp.”"
  - id: lamport-order
    prompt: "Order these Lamport timestamps: (2, \"Bryce\"), (1, \"Caleb\"), (1, \"Aaliyah\")."
    options:
      - id: oc44817
        text: "(1, \"Aaliyah\") < (1, \"Caleb\") < (2, \"Bryce\")"
        correct: true
      - id: o0e8d40
        text: "They cannot be ordered"
      - id: ob2e150
        text: "(1, \"Caleb\") < (1, \"Aaliyah\") < (2, \"Bryce\")"
      - id: oca08b8
        text: "(2, \"Bryce\") < (1, \"Aaliyah\") < (1, \"Caleb\")"
    explanation: "Compare counters first, then node IDs lexicographically: “Thus, the timestamp order in this example is (1, ‘Aaliyah’) < (1, ‘Caleb’) < (2, ‘Bryce’).”"
  - id: lamport-not-lin
    prompt: "Do Lamport clocks provide linearizability?"
    options:
      - id: oe3612e
        text: "No — they give a total order consistent with causality, not a recency guarantee"
        correct: true
      - id: oc9beec
        text: "Yes, always"
      - id: o443109
        text: "Only on one node"
      - id: o860b50
        text: "Only with NTP"
    explanation: "“Although Lamport clocks provide a total ordering, they do not provide linearizability—that is, they are not a way of ensuring that a value is up-to-date.”"
  - id: lamport-limits
    prompt: "Which are limitations of Lamport timestamps?"
    type: multiple
    options:
      - id: o4675ea
        text: "No relation to physical time, so you can't query by date"
        correct: true
      - id: oca6da1
        text: "They aren't unique"
      - id: o8ded1e
        text: "Nodes that never communicate can have wildly different counters"
        correct: true
      - id: o1e4a3e
        text: "They can't be compared"
    explanation: "“Since they have no direct relation to physical time, you can’t use them to find, say, all the messages that were posted on a particular date” and “If two nodes never communicate ... events generated around the same time on different nodes could have wildly different counter values.”"
  - id: hlc
    prompt: "What does a hybrid logical clock combine?"
    options:
      - id: o544c52
        text: "Vector clocks and GPS"
      - id: oc2cd99
        text: "UUIDs and timestamps"
      - id: o3782fb
        text: "Random numbers and counters"
      - id: o12eb83
        text: "Physical-time-like values with Lamport-style forward jumps and monotonic increments, consistent with happens-before"
        correct: true
    explanation: "HLC “combines the advantages of physical time-of-day clocks with the ordering guarantees of Lamport clocks ... It doesn’t depend on any special hardware and requires only roughly synchronized clocks.”"
  - id: vector-vs-lamport
    prompt: "When do you need vector clocks instead of Lamport or hybrid clocks?"
    options:
      - id: o9fbf33
        text: "When you need compact timestamps"
      - id: o7c318b
        text: "Never"
      - id: ocb66a6
        text: "When you must tell whether two events were concurrent"
        correct: true
      - id: o94b17c
        text: "When you need time-of-day"
    explanation: "Lamport/HLC order concurrent events arbitrarily, so “If you want to be able to determine when records were created concurrently, you need a different algorithm, such as a vector clock.” The downside is size."
  - id: photo-leak
    prompt: "How can a causal (non-linearizable) ID generator leak a photo after the user set their account to private?"
    options:
      - id: oac2ebe
        text: "The photo is cached in a CDN"
      - id: o3fb1c8
        text: "The user's laptop clock is wrong"
      - id: ob8584f
        text: "The photos database's counter lags, so the upload gets a lower timestamp than the privacy change, and a snapshot read between them sees \"public\""
        correct: true
      - id: o3c7c73
        text: "Lamport clocks are random"
    explanation: "“it’s possible that the local counter in the photos database is slightly behind, and therefore the photo upload is assigned a lower timestamp than the update of the account settings”. The simplest fix is “a linearizable ID generator”."
  - id: tso
    prompt: "How do systems like TiDB implement a linearizable ID generator efficiently?"
    options:
      - id: obd7f99
        text: "Wall clocks without uncertainty"
      - id: oe736d4
        text: "Random UUIDs"
      - id: o1c57c9
        text: "Each shard generates IDs independently"
      - id: o383abc
        text: "A single replicated node (timestamp oracle) that persists and replicates batches of IDs"
        correct: true
    explanation: "TiDB/TiKV “calls it a timestamp oracle”. As an optimization “the ID generator can write a record describing a batch of IDs ... some IDs will be skipped ... but you won’t issue any duplicate or out-of-order IDs.”"
  - id: tso-limits
    prompt: "Why can't a linearizable ID generator be easily sharded or spread across regions?"
    options:
      - id: odf68b4
        text: "It uses too much memory"
      - id: oe81a69
        text: "It requires GPUs"
      - id: odfb23a
        text: "Independent shards can't guarantee linearizable order, so all ID requests go to one region"
        correct: true
      - id: o55f577
        text: "IDs are too large"
    explanation: "“You can’t easily shard the ID generator ... in a geographically distributed database, all requests for IDs will have to go to a node in a single region.”"
  - id: spanner-ids
    prompt: "How does Spanner get linearizable IDs without a single ID node?"
    options:
      - id: oda37ff
        text: "Lamport clocks"
      - id: o326a2d
        text: "Using clock uncertainty intervals and waiting them out before returning"
        correct: true
      - id: oac3e21
        text: "Random UUIDs"
      - id: oa75b58
        text: "A global counter in one region"
    explanation: "Spanner “relies on a physical clock that returns not just a single timestamp, but a range ... Spanner then waits for the duration of that uncertainty interval to elapse before returning.”"
  - id: clocks-locks
    prompt: "Why can't even a linearizable clock alone implement a fault-tolerant lock?"
    options:
      - id: o135517
        text: "A node must hear from every other node to know its timestamp is lowest, so one unreachable node halts progress"
        correct: true
      - id: o807e80
        text: "Clocks can't be compared"
      - id: o796caa
        text: "Locks require GPS"
      - id: o7dd914
        text: "Clocks are too slow"
    explanation: "“how does a node know whether its own timestamp is the lowest? To be sure, it needs to hear from every other node ... this system would grind to a halt ... We need consensus.”"
---
Unique IDs that also tell you order are surprisingly subtle. Distributed schemes give uniqueness but
weak ordering; Lamport and hybrid logical clocks give causal order; linearizable IDs need a single
authority or bounded clocks.

## Learning objectives

- Compare single-node, sharded, block, UUID and timestamp-based ID schemes
- Explain Lamport clocks and hybrid logical clocks, and how they differ from vector clocks
- Explain why causal IDs can still leak a private photo, and how linearizable IDs fix it
- Explain why logical clocks alone can't implement locks

## In a nutshell

- Single-node autoincrement: linearizable but a SPOF, slow cross-region, a bottleneck.
- UUIDv7, Snowflake, ULID: unique, roughly time-ordered, not linearizable.
- Lamport: (counter, node ID), advance on receive; total order consistent with causality.
- HLC: physical-time-like, monotonic, causal — used by CockroachDB.
- Vector clocks detect concurrency at the cost of size.
- Timestamp oracles (TiDB) or Spanner's commit wait give linearizable IDs.

## Connections to today

- **UUIDv7** (RFC 9562) is now the common default for new primary keys; .NET 9 added
  `Guid.CreateVersion7()`.
- **TiDB's PD timestamp oracle** is the chapter's single-node linearizable ID generator in production.
