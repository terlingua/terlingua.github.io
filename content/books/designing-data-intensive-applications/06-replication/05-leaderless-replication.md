---
title: "Leaderless Replication"
questions:
  - id: dynamodb
    prompt: "Does Amazon DynamoDB use Dynamo-style leaderless replication?"
    options:
      - id: o5c588c
        text: "No — it uses single-leader replication based on Multi-Paxos"
        correct: true
      - id: ocbfe25
        text: "Yes — with sloppy quorums"
      - id: oe85aab
        text: "It has no replication"
      - id: of5d751
        text: "Yes — it is the open source release of Dynamo"
    explanation: "“The similarly named DynamoDB, a more recent cloud database from Amazon, has a completely different architecture: it uses single-leader replication based on the Multi-Paxos consensus algorithm”."
  - id: no-failover
    prompt: "In a leaderless system with one replica down, what happens to writes?"
    options:
      - id: odd4cb9
        text: "Writes go only to the down replica's neighbour"
      - id: obd0ce0
        text: "A failover must elect a new leader first"
      - id: oaf7f99
        text: "They continue; clients send writes to all replicas and succeed once enough acknowledge"
        correct: true
      - id: od54282
        text: "All writes are rejected until the replica returns"
    explanation: "“in a leaderless configuration, there is no such thing as failover, since all replicas are equal and there are no leaders.”"
  - id: stale-choose
    prompt: "A read returns version 6 from one replica and version 7 from two. Which value does the client use, and what can it do next?"
    options:
      - id: oefdfc4
        text: "Version 7; it can write version 7 back to the stale replica (read repair)"
        correct: true
      - id: o8725d7
        text: "Version 6, because it is older and safer"
      - id: o8ab05c
        text: "Both, as siblings always"
      - id: odc9153
        text: "Neither; it must retry"
    explanation: "The client “uses the one with the greatest timestamp” and with read repair “The client sees that replica 3 has a stale value and writes the newer value back to that replica.”"
  - id: catchup-mechanisms
    prompt: "Match: \"a background process copies missing data between replicas in no particular order\"."
    options:
      - id: o6d29d0
        text: "Anti-entropy"
        correct: true
      - id: of37d6d
        text: "Read repair"
      - id: oa03984
        text: "Sloppy quorum"
      - id: o7188a2
        text: "Hinted handoff"
    explanation: "“a background process periodically looks for differences in the data between replicas and then copies any missing data ... this anti-entropy process does not copy writes in any particular order”."
  - id: hinted-handoff
    prompt: "Why is hinted handoff useful in addition to read repair?"
    options:
      - id: oe249a5
        text: "It provides linearizability"
      - id: o3930e6
        text: "It replaces anti-entropy entirely"
      - id: ocef4c7
        text: "It brings replicas up to date even for values that are never read"
        correct: true
      - id: o526627
        text: "It is faster than read repair for hot keys"
    explanation: "Hinted handoff “helps bring replicas up-to-date, even for values that are never read and therefore not handled by read repair.”"
  - id: quorum-condition
    prompt: "With n = 5, which settings satisfy the quorum condition?"
    type: multiple
    options:
      - id: o599982
        text: "w = 1, r = 1"
      - id: o4d4484
        text: "w = 2, r = 2"
      - id: o4eed46
        text: "w = 3, r = 3"
        correct: true
      - id: oeef734
        text: "w = 5, r = 1"
        correct: true
    explanation: "“As long as w + r > n, we expect to get an up-to-date value when reading”. 3 + 3 = 6 > 5 and 5 + 1 = 6 > 5, but 2 + 2 = 4 is not."
  - id: w-n-r-1
    prompt: "What is the downside of w = n, r = 1?"
    options:
      - id: o15e65d
        text: "Reads are fast, but one failed node makes all writes fail"
        correct: true
      - id: o1d2c61
        text: "Writes become faster than reads"
      - id: o0c9132
        text: "Reads become stale"
      - id: o666f0d
        text: "The quorum condition is violated"
    explanation: "“a workload with few writes and many reads may benefit from setting w = n and r = 1. This makes reads faster but has the disadvantage that just one failed node causes all database writes to fail.”"
  - id: tolerate
    prompt: "How many unavailable nodes can n = 5, w = 3, r = 3 tolerate?"
    options:
      - id: ob1ba97
        text: "1"
      - id: o862c42
        text: "0"
      - id: o043e4a
        text: "3"
      - id: ocba371
        text: "2"
        correct: true
    explanation: "“With n = 5, w = 3, r = 3, we can tolerate two unavailable nodes.”"
  - id: quorum-not-majority
    prompt: "Must quorums be majorities?"
    options:
      - id: of20e44
        text: "Yes — always more than n/2"
      - id: o4003c3
        text: "Yes — exactly n/2"
      - id: o7ac7fc
        text: "No — they need not overlap at all"
      - id: o1c7612
        text: "No — what matters is that read and write sets overlap in at least one node"
        correct: true
    explanation: "“But quorums are not necessarily majorities—it matters only that the sets of nodes used by the read and write operations overlap in at least one node.”"
  - id: small-quorum
    prompt: "Why might you deliberately choose w + r ≤ n?"
    options:
      - id: o2f9e5d
        text: "To support transactions"
      - id: o266b99
        text: "Stronger consistency"
      - id: oe60559
        text: "Lower latency and higher availability, at the cost of more stale reads"
        correct: true
      - id: ob6b64f
        text: "To avoid read repair"
    explanation: "“With a smaller w and r, you are more likely to read stale values ... On the upside, this configuration allows lower latency ... This setup is also more highly available”."
  - id: quorum-edge
    prompt: "Which can cause stale reads even with w + r > n?"
    type: multiple
    options:
      - id: o2dd10e
        text: "A read concurrent with a write"
        correct: true
      - id: o7e19d1
        text: "A write that succeeded on fewer than w replicas is not rolled back"
        correct: true
      - id: o3f82c6
        text: "Clock-based LWW drops writes from a node with a slower clock"
        correct: true
      - id: od3e2f9
        text: "The client uses TLS"
      - id: of08e09
        text: "A node with the new value fails and is restored from a replica with an old value"
        correct: true
    explanation: "The chapter lists restoring from stale replicas, rebalancing, concurrent reads and writes, partially failed writes that are “not rolled back”, real-time-clock LWW, and concurrent writes. Quorum parameters should not be taken “as absolute guarantees”."
  - id: monitor-lag
    prompt: "Why is monitoring staleness harder in leaderless systems?"
    options:
      - id: o9ae67a
        text: "Hints are always zero"
      - id: o0fb861
        text: "They have no metrics at all"
      - id: o1acbf3
        text: "They never fall behind"
      - id: o4da573
        text: "There is no fixed order of writes, so you can't subtract log positions as with leader-based lag"
        correct: true
    explanation: "“in systems with leaderless replication, there is no fixed order in which writes are applied, which makes monitoring more difficult.”"
  - id: leader-perf
    prompt: "Which are performance problems of reading from the leader in a single-leader system?"
    type: multiple
    options:
      - id: o1f701e
        text: "Read throughput limited to the leader's capacity"
        correct: true
      - id: o8c2dd3
        text: "Sensitivity to the leader being slow"
        correct: true
      - id: od61321
        text: "Inability to read your own writes"
      - id: o119713
        text: "Unavailability or latency during failover"
        correct: true
    explanation: "“Read throughput is limited by the leader’s capacity”, during failover “users will notice it”, and “The system is very sensitive to performance problems on the leader.”"
  - id: hedging
    prompt: "What is request hedging and why does it reduce tail latency?"
    options:
      - id: od06b33
        text: "Limiting requests per client"
      - id: o45f6af
        text: "Caching responses"
      - id: o1331ac
        text: "Retrying after a timeout"
      - id: oaaee6e
        text: "Sending to several replicas and using the fastest responses, so one slow replica barely matters"
        correct: true
    explanation: "“Using the fastest responses is called request hedging, and it can significantly reduce tail latency.”"
  - id: gray-failure
    prompt: "Why are leaderless systems resilient to gray failures?"
    options:
      - id: oa98e2c
        text: "They detect gray failures instantly"
      - id: oe47aac
        text: "They don't distinguish normal and failure cases, so no failover decision is needed for a slow node"
        correct: true
      - id: oba5e2c
        text: "They have no slow nodes"
      - id: o8bb4e2
        text: "They use synchronous replication"
    explanation: "“the resilience of a leaderless system comes from the fact that it doesn’t distinguish between the normal case and the failure case. This is especially helpful when handling gray failures”."
  - id: bigger-quorum
    prompt: "Why are quorums seldom larger than 4 of 7 or 5 of 9?"
    options:
      - id: o4e6469
        text: "Larger quorums are illegal"
      - id: o9460c0
        text: "More responses to wait for raises the chance of hitting a slow replica"
        correct: true
      - id: o2b141a
        text: "Hinted handoff stops working"
      - id: o9e2057
        text: "Consistency gets weaker"
    explanation: "“a bigger r or w raises the chances of you hitting a slow replica, increasing the overall response time ... In practice, quorums are seldom more than four out of seven nodes or five out of nine nodes.”"
  - id: sloppy
    prompt: "What is a sloppy quorum (Cassandra: consistency level ANY)?"
    options:
      - id: o312cd2
        text: "Any reachable replica accepts a write even if not one of the key's usual replicas, with no guarantee later reads see it"
        correct: true
      - id: o6ee266
        text: "A quorum computed by a leader"
      - id: oe64cd1
        text: "A quorum with w + r > n"
      - id: od2af32
        text: "A quorum that requires all nodes"
    explanation: "“Some leaderless databases offer a configuration option that allows any reachable replica to accept writes, even if it’s not one of the usual replicas for that key ... There is no guarantee that subsequent reads will see the written value”."
  - id: multi-region-coord
    prompt: "How does Cassandra reduce cross-region traffic for a multi-region write?"
    options:
      - id: o8dc8e3
        text: "It sends the write to every replica in every region directly"
      - id: o323031
        text: "The local coordinator sends the write to one replica in each other region, which forwards it locally"
        correct: true
      - id: o5fd87b
        text: "It writes only to the local region"
      - id: o8f7802
        text: "It uses a global leader"
    explanation: "“The coordinator node forwards the write to all replicas in its own region and to one replica in every other region, which then forwards it to the other replicas in that region.”"
  - id: happens-before
    prompt: "When does operation A happen before operation B?"
    options:
      - id: of7320f
        text: "If B knows about, depends on, or builds upon A"
        correct: true
      - id: o29fda7
        text: "If A is a write and B is a read"
      - id: o697b13
        text: "If A and B are on the same node"
      - id: o7e1338
        text: "If A has an earlier wall-clock timestamp"
    explanation: "“An operation A happens before another operation B if B knows about A, or depends on A, or builds upon A in some way.”"
  - id: concurrency-relativity
    prompt: "Two operations occur 10 seconds apart, but a network outage prevented either from knowing of the other. Are they concurrent?"
    options:
      - id: o20665b
        text: "Only if both are writes"
      - id: oc20522
        text: "Yes — concurrency is about mutual unawareness, not physical time"
        correct: true
      - id: of7f97b
        text: "Only if on the same node"
      - id: o066e73
        text: "No — they were 10 seconds apart"
    explanation: "“two operations can occur some time apart and still be concurrent, because the network problems prevented one operation from being able to know about the other.”"
  - id: version-algo
    prompt: "In the single-replica version-number algorithm, what does the server do with a write carrying version 3?"
    options:
      - id: o7c09a2
        text: "Overwrite everything"
      - id: o33ebf4
        text: "Overwrite all values with version ≤ 3 and keep values with higher versions as concurrent siblings"
        correct: true
      - id: o3e7afa
        text: "Keep all values forever"
      - id: o65cffc
        text: "Reject it if a higher version exists"
    explanation: "“it can overwrite all values with that version number or below ... but it must keep all values with a higher version number (because those values are concurrent with the incoming write).”"
  - id: no-version
    prompt: "What happens if a client writes without including a version number?"
    options:
      - id: o7126fa
        text: "It is rejected"
      - id: o4d36d8
        text: "It overwrites everything"
      - id: oad622f
        text: "It gets version 0 and is discarded"
      - id: ocac12a
        text: "The write is concurrent with all others, overwrites nothing, and appears as a sibling"
        correct: true
    explanation: "“If you make a write without including a version number, it is concurrent with all other writes, so it will not overwrite anything”."
  - id: version-vector
    prompt: "Why is a single version number insufficient with multiple replicas accepting writes?"
    options:
      - id: oce2dad
        text: "Version numbers overflow"
      - id: o27ec28
        text: "You need a version number per replica as well as per key — a version vector — to distinguish overwrites from concurrent writes"
        correct: true
      - id: o82cd53
        text: "Replicas can't store numbers"
      - id: o360863
        text: "A single number is sufficient"
    explanation: "“we need to use a version number per replica as well as per key ... The collection of version numbers from all the replicas is called a version vector.”"
  - id: vv-vs-vc
    prompt: "Which data structure is right for comparing the state of replicas?"
    options:
      - id: o474345
        text: "Wall-clock timestamps"
      - id: o9fee90
        text: "Lamport timestamps"
      - id: o901c74
        text: "Vector clocks"
      - id: oa0732d
        text: "Version vectors"
        correct: true
    explanation: "“A version vector is sometimes also called a vector clock, even though they are not quite the same ... when comparing the state of replicas, version vectors are the right data structure to use.”"
  - id: summary-purposes
    prompt: "Which are purposes of replication listed in the chapter summary?"
    type: multiple
    options:
      - id: odb4afd
        text: "Durability"
        correct: true
      - id: o1d45cc
        text: "Disconnected operation"
        correct: true
      - id: o3ab668
        text: "Latency and read scalability"
        correct: true
      - id: o9cfe85
        text: "Storing datasets larger than one machine"
      - id: o6b48fd
        text: "High availability"
        correct: true
    explanation: "The summary lists “High availability”, “Durability”, “Disconnected operation”, latency and “Scalability Being able to handle a higher volume of reads”. Datasets larger than one machine need sharding, the next chapter."
---
Dynamo-style systems let any replica accept writes. Clients write to and read from several replicas, and
quorums make it likely that reads see recent writes.

## Learning objectives

- Explain read repair, hinted handoff and anti-entropy
- Use the quorum condition w + r > n and know its limits
- Compare the performance resilience of leaderless and leader-based systems
- Define happens-before and concurrency and walk through version numbers and version vectors

## In a nutshell

- No failover: writes go to all n replicas, success after w acks; reads ask r replicas.
- w + r > n makes overlap likely, but edge cases still return stale data.
- Leaderless treats normal and failure cases alike; request hedging cuts tail latency.
- Concurrency means neither operation knew about the other.
- Version vectors (per replica, per key) distinguish overwrites from concurrent writes.

## Connections to today

- **Cassandra and ScyllaDB** remain the main Dynamo-style databases; DynamoDB itself is single-leader
  per partition, as the chapter points out.
- **Gray failures** in cloud environments — slow but not dead nodes — are why hedged requests are common
  in storage clients.
- **LOCAL_QUORUM** is the usual Cassandra consistency level for multi-region apps.
