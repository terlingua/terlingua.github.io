---
title: "Single-Leader Replication"
questions:
  - id: leader-flow
    prompt: "In single-leader replication, which statement is true?"
    options:
      - id: ob39c3d
        text: "Followers forward reads to the leader only"
      - id: odc21f0
        text: "Writes go only to the leader; reads can go to the leader or any follower"
        correct: true
      - id: od9481b
        text: "Any replica accepts writes"
      - id: o2039ee
        text: "The leader never stores data locally"
    explanation: "“writes are accepted only by the leader (the followers are read-only from the client’s point of view).”"
  - id: follower-order
    prompt: "How do followers apply the leader's changes?"
    options:
      - id: o2489ae
        text: "In any order, since writes commute"
      - id: o057159
        text: "Sorted by key"
      - id: o2ae0e5
        text: "In the same order as they were processed on the leader"
        correct: true
      - id: o92e02b
        text: "In reverse order"
    explanation: "Each follower updates its copy “by applying all writes in the same order as they were processed on the leader.”"
  - id: sync-downside
    prompt: "Why is it impracticable for all followers to be synchronous?"
    options:
      - id: o789833
        text: "It requires multi-leader replication"
      - id: oa08d4d
        text: "Any one unresponsive node would block all writes"
        correct: true
      - id: ofc4a94
        text: "Followers cannot acknowledge writes"
      - id: o478f23
        text: "Synchronous replication is slower than disk"
    explanation: "“if the synchronous follower doesn’t respond ... the write cannot be processed ... any one node outage would cause the whole system to grind to a halt.”"
  - id: semisync
    prompt: "What is semisynchronous replication?"
    options:
      - id: oa45814
        text: "One follower is synchronous and the rest asynchronous, ensuring an up-to-date copy on at least two nodes"
        correct: true
      - id: o9a1b3f
        text: "Synchronous replication with no leader"
      - id: od0b726
        text: "Half of all writes are synchronous"
      - id: o1603a6
        text: "Replication that pauses during peak load"
    explanation: "“if a database offers synchronous replication, it often means that one of the followers is synchronous and the others are asynchronous ... This configuration is sometimes also called semisynchronous.”"
  - id: async-tradeoff
    prompt: "What is the trade-off of fully asynchronous replication?"
    options:
      - id: o0821e2
        text: "The leader keeps processing writes even if all followers lag, but confirmed writes may be lost if the leader fails"
        correct: true
      - id: o2a2755
        text: "Followers can accept writes"
      - id: o6a1ec3
        text: "Reads become linearizable"
      - id: oaab789
        text: "Writes are always durable but slow"
    explanation: "“if the leader fails and is not recoverable, any writes that have not yet been replicated to followers are lost ... However, a fully asynchronous configuration has the advantage that the leader can continue processing writes, even if all its followers have fallen behind.”"
  - id: new-follower
    prompt: "Why can't you set up a follower by simply copying the leader's data files?"
    options:
      - id: oa0c2d6
        text: "Copying files is illegal"
      - id: o009a0a
        text: "Followers use a different file format"
      - id: o065346
        text: "Writes are constantly happening, so a copy would see different parts at different times and might make no sense"
        correct: true
      - id: o08bfde
        text: "Files are encrypted"
    explanation: "“a standard file copy would see different parts of the database at different points in time. The result might not make any sense.”"
  - id: snapshot-position
    prompt: "Why must the snapshot used to seed a follower be associated with a precise log position?"
    options:
      - id: o9c8551
        text: "To identify the follower"
      - id: oc1847c
        text: "To encrypt the snapshot"
      - id: odea2ab
        text: "To sort the snapshot"
      - id: oaf001a
        text: "The follower must request exactly the changes since that snapshot to catch up"
        correct: true
    explanation: "“This requires that the snapshot is associated with an exact position in the leader’s replication log.” PostgreSQL calls it the log sequence number; MySQL uses binlog coordinates or GTIDs."
  - id: object-storage-benefits
    prompt: "Which are benefits of databases backed by object storage?"
    type: multiple
    options:
      - id: od66bcc
        text: "Inexpensive storage for less frequently queried data"
        correct: true
      - id: o09c79c
        text: "High durability with multi-zone or multi-region replication"
        correct: true
      - id: o4b07c5
        text: "Conditional writes (compare-and-set) for transactions and leader election"
        correct: true
      - id: o7594ea
        text: "Lower latency than local disks"
    explanation: "The sidebar lists low cost, durable multi-zone replication, “conditional write feature—essentially, a compare-and-set (CAS) operation—to implement transactions and leadership election”, and easier data integration. But “object stores have much higher read and write latencies than local disks”."
  - id: zda
    prompt: "What is a zero-disk architecture?"
    options:
      - id: o41e8ed
        text: "A system that uses only network-attached block storage"
      - id: oa6c7a2
        text: "A database without indexes"
      - id: o9f108a
        text: "All data is persisted to object storage; disks and memory are used only for caching, so nodes have no persistent state"
        correct: true
      - id: o60e98c
        text: "A database that keeps everything in RAM with no durability"
    explanation: "“ZDA-based systems persist all data to object storage and use disks and memory strictly for caching. This allows nodes to have no persistent state, which dramatically simplifies operations.”"
  - id: follower-catchup
    prompt: "A follower was offline for a week and the leader's log is filling its disk. What choice does the leader face?"
    options:
      - id: o545e74
        text: "Compress the follower"
      - id: o46c140
        text: "Keep the log and risk running out of disk, or delete it and force the follower to restore from a backup"
        correct: true
      - id: obfe91e
        text: "Promote the follower to leader"
      - id: o3bab0c
        text: "Stop accepting writes"
    explanation: "The leader can “retain the log until the follower recovers and catches up (at the risk of running out of disk space on the leader), or delete the log ... (in which case the follower ... will have to be restored from a backup”."
  - id: failover-steps
    prompt: "Which are steps of automatic failover?"
    type: multiple
    options:
      - id: o009a04
        text: "Choose a new leader, ideally the most up-to-date replica"
        correct: true
      - id: o82a9b7
        text: "Delete all followers' data"
      - id: of203a5
        text: "Determine that the leader has failed, usually by timeout"
        correct: true
      - id: od76bd2
        text: "Reconfigure clients and the old leader to use the new leader"
        correct: true
    explanation: "The steps: “Determining that the leader has failed”, “Choosing a new leader”, and “Reconfiguring the system to use the new leader.”"
  - id: github-incident
    prompt: "In the GitHub incident, what went wrong after an out-of-date follower was promoted?"
    options:
      - id: o531bc6
        text: "It deleted the old leader's data"
      - id: o545b7e
        text: "Its autoincrement counter lagged, so it reused primary keys also used in Redis, disclosing private data to the wrong users"
        correct: true
      - id: o2b3029
        text: "It caused a split brain that lasted days"
      - id: oaa2c46
        text: "It doubled all writes"
    explanation: "“because the new leader’s counter lagged behind the old leader’s, it reused some primary keys ... the reuse of primary keys resulted in inconsistency between MySQL and Redis, which caused some private data to be disclosed to the wrong users.”"
  - id: split-brain
    prompt: "What is split brain and why is it dangerous?"
    options:
      - id: o7b254c
        text: "Two nodes both believe they are leader; if both accept writes without conflict resolution, data can be lost or corrupted"
        correct: true
      - id: o65a95d
        text: "A network with two subnets"
      - id: o69987c
        text: "A leader split into two processes on one machine"
      - id: o9988ab
        text: "A database with two indexes on the same column"
    explanation: "“two nodes could both believe that they are the leader. This situation, called split brain, is dangerous; if both leaders accept writes, and there is no process for resolving conflicts ... data is likely to be lost or corrupted.”"
  - id: timeout-choice
    prompt: "Why can a short failover timeout make things worse?"
    options:
      - id: o3ce782
        text: "It makes the timeout infinite"
      - id: oa1981c
        text: "It always causes data loss"
      - id: o28cf2f
        text: "Load spikes or glitches trigger unnecessary failovers, adding disruption when the system is already struggling"
        correct: true
      - id: o6597ba
        text: "It prevents leader election"
    explanation: "“if the timeout is too short, unnecessary failovers could occur ... If the system is already struggling with high load or network problems, an unnecessary failover is likely to make the situation worse, not better.”"
  - id: manual-failover
    prompt: "Why do some teams prefer manual failover even when automatic failover exists?"
    options:
      - id: o92d82e
        text: "Databases do not support automatic failover"
      - id: o0bdf88
        text: "Manual failover is faster"
      - id: o9c715a
        text: "Automatic failover is illegal"
      - id: o700577
        text: "Failover has many hard failure modes with no easy solutions"
        correct: true
    explanation: "“these problems have no easy solutions. For this reason, some operations teams prefer to perform failovers manually, even if the software supports automatic failover.”"
  - id: statement-problems
    prompt: "Why can statement-based replication diverge?"
    type: multiple
    options:
      - id: o2721c4
        text: "Triggers and stored procedures with nondeterministic side effects"
        correct: true
      - id: o9dac09
        text: "Statements depending on existing data must run in the same order"
        correct: true
      - id: o017558
        text: "Nondeterministic functions such as NOW or RAND"
        correct: true
      - id: oaa2312
        text: "Statements are too large to send"
    explanation: "The chapter lists nondeterministic functions, autoincrement or data-dependent statements that “must be executed in exactly the same order on each replica”, and side effects “unless the side effects are absolutely deterministic.”"
  - id: wal-shipping
    prompt: "What is the main disadvantage of shipping the physical WAL to followers?"
    options:
      - id: o45f344
        text: "It is slower than statement replication in all cases"
      - id: oc16a07
        text: "It loses data on every crash"
      - id: o0808a7
        text: "It cannot replicate deletes"
      - id: obb0a3a
        text: "It is coupled to the storage engine, often preventing different software versions on leader and followers — so upgrades need downtime"
        correct: true
    explanation: "“a WAL contains details of which bytes were changed in which disk blocks. This makes replication tightly coupled to the storage engine.” Without version mismatch support, “such upgrades require downtime.”"
  - id: zero-downtime-upgrade
    prompt: "How does a replication protocol that tolerates version mismatch enable zero-downtime upgrades?"
    options:
      - id: oeb0448
        text: "Run two separate clusters with no replication"
      - id: o32d21c
        text: "Upgrade the leader first, then followers"
      - id: o12d17f
        text: "Stop all writes during the upgrade"
      - id: o95c616
        text: "Upgrade followers first, then fail over to an upgraded follower"
        correct: true
    explanation: "“you can perform a zero-downtime upgrade of the database software by first upgrading the followers and then performing a failover to make one of the upgraded nodes the new leader.”"
  - id: logical-log
    prompt: "What does a logical (row-based) log record for a deleted row?"
    options:
      - id: odd1d86
        text: "The SQL DELETE statement"
      - id: o320545
        text: "Nothing; deletes are not logged"
      - id: o9d52a4
        text: "The disk block that held the row"
      - id: o622cac
        text: "Enough information to uniquely identify the row, typically the primary key"
        correct: true
    explanation: "“For a deleted row, the log contains enough information to uniquely identify the row that was deleted. Typically this would be the primary key”."
  - id: logical-benefits
    prompt: "Which are benefits of logical replication logs?"
    type: multiple
    options:
      - id: o19d279
        text: "Easier backward compatibility between database versions"
        correct: true
      - id: o578887
        text: "Smaller than every other format"
      - id: ob1f312
        text: "Easier for external systems to parse, enabling change data capture"
        correct: true
      - id: o2b024a
        text: "Byte-for-byte identical files on followers"
    explanation: "“a logical log is decoupled from the storage engine internals, it can more easily be kept backward compatible” and “A logical log format is also easier for external applications to parse ... This technique is called change data capture”."
  - id: read-scaling
    prompt: "Why does read scaling with many followers only realistically work with asynchronous replication?"
    options:
      - id: o215099
        text: "Synchronous followers cannot serve reads"
      - id: of83f4d
        text: "Async followers are always up to date"
      - id: ocd6368
        text: "Load balancers require async"
      - id: oad2371
        text: "With synchronous replication to all, any single failure would block writes, and more nodes make that more likely"
        correct: true
    explanation: "“If you tried to synchronously replicate to all followers, a single node failure or network outage would make the entire system unavailable for writing.”"
  - id: eventual
    prompt: "What does \"eventually\" in eventual consistency promise about timing?"
    options:
      - id: oc65586
        text: "Within one second"
      - id: oa5b75f
        text: "Within one minute"
      - id: o8dd859
        text: "Nothing specific — there is no limit to how far a replica can fall behind"
        correct: true
      - id: oabd6c2
        text: "Before the next write"
    explanation: "“The term “eventually” is deliberately vague; in general, there is no limit to how far a replica can fall behind.”"
  - id: ryw
    prompt: "A user posts a comment, reloads, and it is gone (read from a lagging follower). Which guarantee was violated?"
    options:
      - id: o2cfd76
        text: "Read-after-write (read-your-writes) consistency"
        correct: true
      - id: o3ff1fb
        text: "Monotonic reads"
      - id: oad1b0b
        text: "Linearizability of other users' writes"
      - id: o290a76
        text: "Consistent prefix reads"
    explanation: "Read-after-write consistency “is a guarantee that if the user reloads the page, they will always see any updates they submitted themselves. It makes no promises about other users”."
  - id: ryw-techniques
    prompt: "Which are ways to implement read-after-write consistency?"
    type: multiple
    options:
      - id: o621a9e
        text: "Read the user's own editable data from the leader"
        correct: true
      - id: o0d9e51
        text: "Remember the timestamp of the client's last write and only read from replicas caught up to it"
        correct: true
      - id: o85fd51
        text: "Read from a random follower"
      - id: o6ea415
        text: "Read from the leader for a minute after the user's last update"
        correct: true
    explanation: "The chapter suggests reading the user's own profile from the leader, tracking “the time of the last update and, for one minute after the last update, make all reads from the leader”, and having “The client ... remember the timestamp of its most recent write”."
  - id: cross-device
    prompt: "What extra difficulties arise for cross-device read-after-write consistency?"
    options:
      - id: oc2adab
        text: "Mobile devices have no clocks"
      - id: ofdec42
        text: "The last-update timestamp must be centralized, and different devices may be routed to different regions"
        correct: true
      - id: o446838
        text: "Cross-device consistency is impossible"
      - id: o960a95
        text: "Devices cannot read from leaders"
    explanation: "The chapter notes “This metadata will need to be centralized” and “there is no guarantee that connections from different devices will be routed to the same region.”"
  - id: monotonic
    prompt: "A user refreshes and a comment that appeared disappears again. Which guarantee prevents this, and how?"
    options:
      - id: oba6d81
        text: "Read-after-write, by reading from the leader"
      - id: ofc3185
        text: "Monotonic reads, e.g., by always routing a user's reads to the same replica chosen by hashing the user ID"
        correct: true
      - id: ofbf252
        text: "Linearizability, by using LWW"
      - id: oc21aa4
        text: "Consistent prefix reads, by sharding"
    explanation: "Monotonic reads mean “they will not read older data after having previously read newer data. One way of achieving monotonic reads is to make sure that each user always makes their reads from the same replica”."
  - id: consistent-prefix
    prompt: "An observer sees Mrs. Cake's answer before Mr. Poons's question. Which guarantee is violated?"
    options:
      - id: ode922e
        text: "Consistent prefix reads"
        correct: true
      - id: o493675
        text: "Durability"
      - id: oed4b1f
        text: "Monotonic reads"
      - id: oad5a7d
        text: "Read-after-write consistency"
    explanation: "“This guarantee says that if a sequence of writes happens in a certain order, anyone reading those writes will see them appear in the same order. This is a particular problem in sharded (partitioned) databases”."
  - id: lag-design
    prompt: "What does the chapter say about designing an eventually consistent system?"
    options:
      - id: o3ee72d
        text: "Always read from followers"
      - id: obb26ec
        text: "Eventual consistency never causes user-visible problems"
      - id: od1c251
        text: "Consider what happens if lag grows to minutes or hours; pretending async replication is sync is a recipe for problems"
        correct: true
      - id: occ1821
        text: "Assume lag is always under a second"
    explanation: "“it is worth thinking about how the application behaves if the replication lag increases to several minutes or even hours ... Pretending that replication is synchronous when in fact it is asynchronous is a recipe for problems down the line.”"
  - id: simplest-model
    prompt: "What is the simplest programming model for application developers dealing with replication?"
    options:
      - id: o895d29
        text: "Handling lag manually in every query"
      - id: o121dc8
        text: "Eventual consistency everywhere"
      - id: ocbdbab
        text: "A database with strong consistency for replicas (linearizability) and ACID transactions"
        correct: true
      - id: o3ab3bd
        text: "Multi-leader replication"
    explanation: "“The simplest programming model for application developers is to choose a database that provides a strong consistency guarantee for replicas, such as linearizability … and supports ACID transactions”"
  - id: regions-zones
    prompt: "What does a multi-zone deployment protect against, and what doesn't it?"
    options:
      - id: o9c6ebc
        text: "It protects only against disk failures"
      - id: o4f57e1
        text: "It survives region outages but not zone outages"
      - id: o46afbc
        text: "It protects against all outages"
      - id: oa4a29e
        text: "It survives a zone outage but not a whole-region outage"
        correct: true
    explanation: "“Multi-zone configurations allow distributed systems to survive zonal outages where one zone goes offline, but they do not protect against regional outages where all zones in a region are unavailable.”"
---
One leader accepts writes and streams changes to followers. The section covers sync versus async,
adding followers, failover, log formats, replication-lag anomalies and their remedies.

## Learning objectives

- Weigh synchronous, semisynchronous and asynchronous replication
- Describe how to add a follower without downtime
- Explain failover and its dangers: lost writes, split brain, bad timeouts
- Compare statement-based, WAL shipping and logical replication
- Define read-after-write, monotonic reads and consistent prefix reads, and how to provide them

## In a nutshell

- Fully synchronous is impractical; semisynchronous keeps one up-to-date follower.
- New follower = consistent snapshot + replay from the snapshot's log position.
- Failover: detect (timeout), choose (most up-to-date), reconfigure (fence the old leader).
- Logical logs decouple replication from storage format and enable CDC.
- Pretending async is sync is a recipe for problems.

## Connections to today

- **Zero-disk architectures** (WarpStream, Bufstream, SlateDB, Turbopuffer) push replication into object
  storage, as the sidebar describes.
- **Postgres logical replication** powers Debezium CDC and zero-downtime major-version upgrades.
- **Read-your-writes in practice:** many apps pin a user to the primary for a short window after a
  write, or pass an LSN/GTID "token" to read from a caught-up replica.
