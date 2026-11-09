---
title: "Multi-Leader Replication"
questions:
  - id: multi-leader-sync
    prompt: "Why does the chapter treat synchronous multi-leader replication as equivalent to single-leader?"
    options:
      - id: o91f32d
        text: "If writes must reach the other leader synchronously, a network interruption blocks writes just as with one leader"
        correct: true
      - id: o9c9d1e
        text: "Synchronous multi-leader is impossible"
      - id: o2d382e
        text: "It has no conflicts at all"
      - id: oc74541
        text: "It uses the same storage format"
    explanation: "“If writes are synchronously replicated from A to B, and the network between the two nodes is interrupted, you can’t write to A until the connection is restored. Synchronous multi-leader replication thus gives you a model that is very similar to single-leader replication”."
  - id: geo-benefits
    prompt: "Which are advantages of multi-leader over single-leader in a multi-region deployment?"
    type: multiple
    options:
      - id: o60849e
        text: "Better tolerance of inter-region network problems"
        correct: true
      - id: o000e30
        text: "Stronger consistency guarantees"
      - id: ofaaf7d
        text: "Each region can operate independently during a regional outage"
        correct: true
      - id: o3fba12
        text: "Writes are processed locally, hiding inter-region latency"
        correct: true
    explanation: "The chapter compares performance, “Tolerance of regional outages” and “Tolerance of network problems”, but notes “The biggest downside of multi-leader systems is that the consistency they can achieve is much weaker.”"
  - id: constraints
    prompt: "Why can't a multi-leader system guarantee unique usernames?"
    options:
      - id: o1f22c8
        text: "Usernames are not indexed"
      - id: o9805b4
        text: "Leaders don't replicate inserts"
      - id: o760dd2
        text: "Two leaders can each accept a write that is fine alone but violates the constraint together"
        correct: true
      - id: ofea018
        text: "It is a bug in specific products"
    explanation: "“it’s always possible for different leaders to process writes that are individually fine ... but that violate the constraint when taken together with another write on another leader. This is simply a fundamental limitation of distributed systems.”"
  - id: dangerous
    prompt: "Why is multi-leader replication often considered dangerous territory?"
    options:
      - id: o8a1265
        text: "It requires custom hardware"
      - id: ocd120a
        text: "It always loses data"
      - id: ofe5e4e
        text: "It is often retrofitted, with subtle pitfalls involving autoincrement keys, triggers and integrity constraints"
        correct: true
      - id: o128aaa
        text: "It is slower than leaderless"
    explanation: "“As multi-leader replication is a retrofitted feature in many databases, there are often subtle configuration pitfalls and surprising interactions with other database features. For example, autoincrementing keys, triggers, and integrity constraints can be problematic.”"
  - id: loops
    prompt: "How do circular and star topologies prevent infinite replication loops?"
    options:
      - id: o8591a0
        text: "Writes expire after a fixed time"
      - id: oe93f02
        text: "Each write is tagged with the IDs of nodes it has passed through, and a node ignores writes tagged with its own ID"
        correct: true
      - id: of27efd
        text: "Writes are deduplicated by content hash"
      - id: o12e7b7
        text: "Only the root forwards writes"
    explanation: "“each write is tagged with the identifiers of all the nodes it has passed through. When a node receives a data change that is tagged with its own identifier, that data change is ignored”."
  - id: topology-tradeoff
    prompt: "What is the trade-off between circular/star and all-to-all topologies?"
    options:
      - id: od94615
        text: "Circular/star have single points of failure; all-to-all is more fault tolerant but messages can overtake each other"
        correct: true
      - id: o99792c
        text: "All-to-all has single points of failure"
      - id: o4a2cd1
        text: "Circular topologies are faster and safer"
      - id: ob4db57
        text: "There is no difference"
    explanation: "“if just one node fails, it can interrupt the flow of replication messages” versus all-to-all where “some replication messages may “overtake” others”."
  - id: overtake
    prompt: "Leader 2 receives an update to a row before the insert that created it. Why doesn't attaching timestamps fix this?"
    options:
      - id: o6d6826
        text: "Clocks can't be trusted to be in sync enough to order the events; version vectors are needed"
        correct: true
      - id: oc2e933
        text: "Timestamps are too large"
      - id: o78e4e4
        text: "Updates have no timestamps"
      - id: o85813b
        text: "Leader 2 ignores timestamps"
    explanation: "“Simply attaching a timestamp to every write is not sufficient, because clocks cannot be trusted to be sufficiently in sync to correctly order these events ... a technique called version vectors can be used”."
  - id: offline-calendar
    prompt: "How does a calendar app that works offline on several devices relate to multi-leader replication?"
    options:
      - id: o7f2ac2
        text: "It uses single-leader replication with the phone as leader"
      - id: oab5c90
        text: "It is not replication at all"
      - id: o002472
        text: "Each device is a leader with a local replica; sync is asynchronous multi-leader replication with lag of hours or days"
        correct: true
      - id: o8acf8a
        text: "It uses leaderless quorums"
    explanation: "“every device has a local database replica that acts as a leader ... and there is an asynchronous multi-leader replication process (sync) between the replicas” — “multi-leader replication between regions, taken to the extreme.”"
  - id: collab-multi-leader
    prompt: "Why is a real-time collaborative editor multi-leader even if it doesn't support offline editing?"
    options:
      - id: o1211b6
        text: "It uses multiple databases"
      - id: of0c6ac
        text: "It runs on multiple browsers"
      - id: o22be69
        text: "Users can make edits without waiting for a response from the server"
        correct: true
      - id: o9ec584
        text: "It stores data in multiple regions"
    explanation: "“Even if the app does not allow you to continue editing a file while offline, the fact that multiple users can make edits without waiting for a response from the server already makes it multi-leader.”"
  - id: local-first
    prompt: "What distinguishes local-first software from offline-first?"
    options:
      - id: oc8c4c4
        text: "It works only offline"
      - id: o6ed799
        text: "It stores data only on servers"
      - id: o12c6d7
        text: "It keeps working even if the developer shuts down all their online services, e.g., via an open sync protocol with multiple providers"
        correct: true
      - id: oe45bc6
        text: "It requires a blockchain"
    explanation: "“The term local-first software refers to collaborative apps that are not only offline-first but are also designed to continue working even if the developer who made the software shuts down all of their online services”. Git is given as an example."
  - id: sync-engine-pros
    prompt: "Which are advantages of the sync engine approach?"
    type: multiple
    options:
      - id: o1e0fe4
        text: "Suitable for apps where users can access a huge catalog"
      - id: o18b00c
        text: "No separate offline mode — offline is just a large network delay"
        correct: true
      - id: oc5ab17
        text: "Simpler, more declarative frontend programming with fewer error paths"
        correct: true
      - id: o8a1bf0
        text: "Faster UI responses from local data"
        correct: true
    explanation: "The chapter lists responsiveness, offline work where “being offline is the same as having a very large network delay”, a simpler programming model, and real-time updates. But “sync engines are not suitable if the user has access to a very large amount of data.”"
  - id: concurrent-def
    prompt: "When are two writes concurrent in the chapter's sense?"
    options:
      - id: o05f9bd
        text: "When neither was aware of the other when made, regardless of physical timing"
        correct: true
      - id: oc34d79
        text: "When they come from the same user"
      - id: o755ab2
        text: "When they write different keys"
      - id: oa3368d
        text: "Only when they occur at exactly the same millisecond"
    explanation: "“It doesn’t matter whether the writes literally happened at the same time ... What matters is whether one write occurred in a state where the other write had already taken effect.”"
  - id: conflict-avoidance
    prompt: "When does conflict avoidance by routing each user to a \"home\" region break down?"
    options:
      - id: o11424e
        text: "When users read their own data"
      - id: oe7bc6b
        text: "When the designated leader for a record changes, e.g., during a regional outage or when a user moves"
        correct: true
      - id: o7bcc26
        text: "It never breaks down"
      - id: od4037f
        text: "When there are more than two regions"
    explanation: "“sometimes you might want to change the designated leader for a record ... There is now a risk that the user performs a write while the change of designated leader is in progress ... conflict avoidance breaks down if you allow the leader to be changed.”"
  - id: odd-even
    prompt: "How can two leaders generate autoincrementing IDs without conflicts?"
    options:
      - id: oc90f58
        text: "Both use random 8-bit numbers"
      - id: o000a7b
        text: "One generates only odd numbers and the other only even numbers"
        correct: true
      - id: o0c9438
        text: "Both use the current time"
      - id: o710d5a
        text: "They take turns via a lock"
    explanation: "“you could set them up so that one leader generates only odd numbers and the other generates only even numbers.”"
  - id: lww-meaning
    prompt: "What does last write wins (LWW) really do with concurrent writes?"
    options:
      - id: o3197bd
        text: "Keeps the write that happened last in real time"
      - id: o7640fe
        text: "Randomly picks one to win and silently discards the others, even though they were acknowledged"
        correct: true
      - id: o0ef66d
        text: "Rejects both writes"
      - id: od983cb
        text: "Merges both values"
    explanation: "“the real meaning of LWW is this: when the same record is concurrently written on different leaders, one of those writes is randomly chosen to be the winner and the other writes are silently discarded, even though they were successfully processed”."
  - id: lww-ok
    prompt: "When is LWW unproblematic?"
    options:
      - id: o1d48d0
        text: "When writes are small"
      - id: o77988a
        text: "When there are only two leaders"
      - id: o001307
        text: "When clocks are synchronized to within a second"
      - id: ob1e769
        text: "When conflicts can be avoided, e.g., inserting records with unique keys and never updating them"
        correct: true
    explanation: "“If you can avoid conflicts—for example, by only inserting records with a unique key and never updating them—then LWW is no problem.”"
  - id: lww-clock
    prompt: "Why is LWW with wall-clock timestamps sensitive to clock skew?"
    options:
      - id: o91299c
        text: "If a node's clock is ahead, later writes from other nodes may get lower timestamps and be ignored"
        correct: true
      - id: o2bbc1a
        text: "Timestamps are too coarse"
      - id: o29cf45
        text: "Clocks run backward during conflicts"
      - id: o8b742c
        text: "LWW never uses clocks"
    explanation: "“If one node has a clock that is ahead of the others, and you try to overwrite a value written by that node, your write may be ignored as it may have a lower timestamp, even though it clearly occurred later.”"
  - id: siblings-problems
    prompt: "Which problems come with storing siblings for manual resolution?"
    type: multiple
    options:
      - id: o03204a
        text: "Siblings are never returned to clients"
      - id: of8006a
        text: "The API changes: a single value becomes a set of values"
        correct: true
      - id: od7ef7e
        text: "Asking users to merge is a lot of work for developers and users"
        correct: true
      - id: o7676fc
        text: "Concurrent resolutions can themselves conflict"
        correct: true
    explanation: "The chapter lists the API change, the burden of manual merging, surprising automatic merges, and “the conflict resolution process can itself introduce a new conflict.”"
  - id: amazon-cart
    prompt: "Why did deleted items reappear in Amazon's shopping cart?"
    options:
      - id: o9ba754
        text: "LWW discarded the deletions"
      - id: o410310
        text: "The cart was cached"
      - id: o04d86a
        text: "The deletion was never written"
      - id: o3e8c5f
        text: "Siblings were merged by set union, so an item removed in one sibling survived if another still contained it"
        correct: true
    explanation: "Amazon merged carts “by keeping all the shopping cart items that appeared in any of the siblings (i.e., taking the set union of the carts) ... the removed item would unexpectedly reappear”."
  - id: sec
    prompt: "What is strong eventual consistency?"
    options:
      - id: o6509bc
        text: "Eventual consistency with a time bound"
      - id: od70ccd
        text: "Linearizability across regions"
      - id: obc8cea
        text: "Consistency enforced by a single leader"
      - id: o568ca8
        text: "Eventual consistency plus a guarantee that replicas that processed the same writes have the same state, regardless of order"
        correct: true
    explanation: "Automatic conflict resolution ensures “all replicas that have processed the same set of writes have the same state, regardless of the order in which the writes arrived. Combining eventual consistency with a convergence guarantee is known as strong eventual consistency”."
  - id: counter-merge
    prompt: "How should a merge algorithm handle a counter of likes edited concurrently?"
    options:
      - id: o9065bd
        text: "Average the values"
      - id: o01875f
        text: "Take the maximum value"
      - id: of4746c
        text: "Count increments and decrements on each sibling and add them, avoiding double-counting and dropped updates"
        correct: true
      - id: o575827
        text: "Take the most recent value"
    explanation: "“the merge algorithm can tell how many increments and decrements happened on each sibling and add them together correctly so that the result does not double-count and does not drop updates.”"
  - id: resolution-limits
    prompt: "A list must contain at most five items, and two users concurrently add items to exceed it. What can automatic resolution do?"
    options:
      - id: o59233d
        text: "Ask the database to grow the limit"
      - id: o981d45
        text: "Guarantee both additions are kept within five"
      - id: o904970
        text: "Reject both writes retroactively without loss"
      - id: oc7258a
        text: "Only drop some items — there are limits to what conflict resolution can achieve"
        correct: true
    explanation: "“if you want to enforce that a list contains no more than five items ... your only option is to drop some of the items.”"
  - id: ot-vs-crdt
    prompt: "Two replicas start with \"ice\"; one prepends \"n\", the other appends \"!\". How do OT and CRDTs reach \"nice!\"?"
    options:
      - id: o68dab5
        text: "OT transforms the \"!\" insertion's index from 3 to 4; a CRDT uses immutable character IDs so no transformation is needed"
        correct: true
      - id: o208d83
        text: "Both use LWW on the whole string"
      - id: o108feb
        text: "OT uses IDs; CRDTs transform indexes"
      - id: o389b85
        text: "Neither can merge this case"
    explanation: "OT: “the insertion of ! is transformed to index 4”. CRDT: “Most CRDTs give each character a unique, immutable ID and use those to determine the positions ... This ensures that replicas converge without performing any transformation.”"
  - id: where-used
    prompt: "Where are OT and CRDTs most often found, per the chapter?"
    options:
      - id: o9938d6
        text: "OT in databases; CRDTs in text editors only"
      - id: o4de309
        text: "Both only in academic prototypes"
      - id: obea737
        text: "OT in real-time text editing such as Google Docs; CRDTs in databases such as Redis Enterprise, Riak and Cosmos DB"
        correct: true
      - id: o372973
        text: "OT in Git; CRDTs in Kafka"
    explanation: "“OT is most often used for real-time collaborative editing of text, such as in Google Docs, whereas CRDTs can be found in distributed databases such as Redis Enterprise, Riak, and Azure Cosmos DB.”"
  - id: booking-conflict
    prompt: "Why is a double-booked meeting room a subtle conflict?"
    options:
      - id: o9cd414
        text: "Because rooms have no IDs"
      - id: of049cd
        text: "Because LWW resolves it automatically"
      - id: o58ff3c
        text: "Because the database rejects overlapping rows"
      - id: o84f240
        text: "Each booking inserts a different record, so no single field is written twice; both may pass an availability check before either inserts"
        correct: true
    explanation: "“a conflict may arise if two bookings are created for the same room at the same time. Even if the application checks availability before allowing a user to make a booking, a conflict can arise if the two bookings are made close enough that they both see the room as unbooked”."
---
Several nodes accept writes: across regions, on offline devices, and in real-time collaboration. The
price is conflicts.

## Learning objectives

- Compare single- and multi-leader setups across regions
- Explain topology problems, including writes overtaking each other
- Describe sync engines, offline-first and local-first software
- Evaluate conflict avoidance, LWW, manual resolution and automatic merging
- Contrast OT and CRDTs

## In a nutshell

- Multi-leader within one region rarely pays off; across regions it hides latency and tolerates outages.
- It cannot enforce constraints such as unique usernames or non-negative balances.
- A device that works offline is a leader with lag measured in hours.
- LWW silently discards concurrent writes; union merges resurrect deletes.
- CRDTs use immutable element IDs; OT transforms indexes.

## Connections to today

- **Local-first frameworks** (Automerge, Yjs, ElectricSQL, Replicache/Zero, Liveblocks) are mainstream for
  collaborative apps in 2026.
- **Figma, Linear and Google Docs** are the chapter's examples of multi-leader UIs.
- **Azure Cosmos DB multi-region writes** offer LWW and custom conflict resolution — the options in this
  section.
