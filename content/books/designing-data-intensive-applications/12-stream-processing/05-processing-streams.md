---
title: "Processing Streams"
questions:
  - id: three-options
    prompt: "Which are the three things you can do with a stream?"
    type: multiple
    options:
      - id: o9824b1
        text: "Delete it after reading"
      - id: o606e42
        text: "Write it to a storage system"
        correct: true
      - id: o262599
        text: "Process input streams into output streams"
        correct: true
      - id: ocf41ab
        text: "Push events to users"
        correct: true
    explanation: "The chapter lists writing to a storage system, pushing events to users, and “You can process one or more input streams to produce one or more output streams.”"
  - id: no-sort-merge
    prompt: "Why can't stream processors use sort-merge joins?"
    options:
      - id: o9f22c2
        text: "Streams are too small"
      - id: o9e7236
        text: "Sorting is too fast"
      - id: od02614
        text: "Streams have no keys"
      - id: o56afe4
        text: "Sorting makes no sense on an unbounded dataset"
        correct: true
    explanation: "“sorting does not make sense with an unbounded dataset, so sort-merge joins ... cannot be used.”"
  - id: cep
    prompt: "How does CEP reverse the usual roles of queries and data?"
    options:
      - id: of74f19
        text: "Queries are stored long-term and each arriving event is checked against them"
        correct: true
      - id: o9ebc0e
        text: "Both are transient"
      - id: o8c1163
        text: "Data is stored and queries are transient"
      - id: oa092d9
        text: "Both are stored in Kafka"
    explanation: "“Usually, a database stores data persistently and treats queries as transient ... CEP engines reverse these roles. Queries are stored long-term”."
  - id: approx
    prompt: "Is stream processing inherently approximate?"
    options:
      - id: ocac012
        text: "No — probabilistic algorithms are just an optimization"
        correct: true
      - id: oa5057c
        text: "Yes — windows are always estimates"
      - id: o0d941c
        text: "Only in Kafka"
      - id: o770044
        text: "Yes — streams always lose data"
    explanation: "“There is nothing inherently approximate about stream processing, and using probabilistic algorithms is merely an optimization.”"
  - id: mv-window
    prompt: "Why doesn't a time window suffice for maintaining a materialized view?"
    options:
      - id: o2de5f5
        text: "Views need only the last event"
      - id: o9fd429
        text: "Windows are too large"
      - id: oa82dfa
        text: "Views must be rebuilt every minute"
      - id: of52e08
        text: "It may need all events since the beginning of time, apart from those removed by compaction"
        correct: true
    explanation: "“considering only events within a certain time window is usually not sufficient ... In effect, you need a window that stretches all the way back to the beginning of time.”"
  - id: ivm
    prompt: "What problems of `REFRESH MATERIALIZED VIEW` does incremental view maintenance solve?"
    type: multiple
    options:
      - id: o878091
        text: "Lack of SQL support"
      - id: o9ecea6
        text: "Stale results between scheduled refreshes"
        correct: true
      - id: o8034f9
        text: "Views being too small"
      - id: oa1c5d4
        text: "Reprocessing all data on each refresh"
        correct: true
    explanation: "The drawbacks are “Poor efficiency All data is reprocessed every time the view is updated” and “Data freshness Changes in source data are not reflected in a materialized view until its query is run again”. IVM “recompute[s] and update[s] only data that has changed”."
  - id: percolator
    prompt: "How does stream search (e.g., Elasticsearch percolator) differ from normal search?"
    options:
      - id: o04be93
        text: "It only works offline"
      - id: oef3259
        text: "Documents are indexed then queried"
      - id: o1bfed3
        text: "Queries are stored and incoming documents are matched against them"
        correct: true
      - id: oa3b74d
        text: "It uses no queries"
    explanation: "“Conventional search engines first index the documents and then run queries over the index. By contrast, searching a stream turns the processing on its head. The queries are stored, and the documents are evaluated against them”."
  - id: actors-vs-streams
    prompt: "Why aren't actor frameworks usually considered stream processors?"
    options:
      - id: oc1de03
        text: "Actors are always faster"
      - id: o13193d
        text: "Streams don't use messages"
      - id: o22d839
        text: "Actors manage concurrency with ephemeral one-to-one messages in arbitrary patterns; streams are durable, multi-subscriber and acyclic"
        correct: true
      - id: o67b9ca
        text: "Actors can't send messages"
    explanation: "The chapter contrasts actors as “a mechanism for managing concurrency” with “ephemeral and one-to-one” communication versus durable, multi-subscriber event logs in “acyclic pipelines”."
  - id: processing-time-spike
    prompt: "A stream processor counting requests per second is down for a minute and then processes its backlog. Windowed by processing time, what does the chart show?"
    options:
      - id: o28fdd4
        text: "A gap with no data"
      - id: ob5d135
        text: "A drop to zero forever"
      - id: o165449
        text: "The correct steady rate"
      - id: of26699
        text: "A false spike, although the real request rate was steady"
        correct: true
    explanation: "“If you measure the rate based on the processing time, it will look as if there was a sudden anomalous spike of requests while processing the backlog, when in fact the real rate of requests was steady”."
  - id: star-wars
    prompt: "In the Star Wars analogy, what does the episode number represent?"
    options:
      - id: o7607ea
        text: "The consumer offset"
      - id: o14f93e
        text: "The partition key"
      - id: odb9372
        text: "The event timestamp"
        correct: true
      - id: o4ae423
        text: "The processing time"
    explanation: "“The episode number is like the event timestamp, and the date when you watched the movie is the processing time.”"
  - id: stragglers
    prompt: "What can you do with straggler events arriving after a window closed?"
    type: multiple
    options:
      - id: o448400
        text: "Publish a corrected value, possibly retracting the earlier output"
        correct: true
      - id: ofb5823
        text: "Delete the stream"
      - id: oc0fc7d
        text: "Ignore them and track dropped events as a metric"
        correct: true
      - id: o23274f
        text: "Reopen every window forever"
    explanation: "“Ignore the straggler events ... You can track the number of dropped events as a metric” or “Publish a correction, an updated value for the window with stragglers included.”"
  - id: three-timestamps
    prompt: "Why log three timestamps for events from mobile devices?"
    options:
      - id: o90266b
        text: "Comparing device send time with server receive time estimates the device clock offset, which corrects the event time"
        correct: true
      - id: o6eef52
        text: "To triple durability"
      - id: o49c05f
        text: "To sort events faster"
      - id: o7aec44
        text: "Because HTTP requires it"
    explanation: "“By subtracting the second timestamp from the third, you can estimate the offset between the device clock and the server clock ... You can then apply that offset to the event timestamp”."
  - id: windows
    prompt: "Match: \"fixed length, overlapping windows to provide smoothing\"."
    options:
      - id: oc31899
        text: "Hopping window"
        correct: true
      - id: oa348ac
        text: "Session window"
      - id: odb3ad7
        text: "Sliding window"
      - id: odc59f4
        text: "Tumbling window"
    explanation: "“A hopping window also has a fixed length, but there is overlap between consecutive windows to provide smoothing.”"
  - id: session-window
    prompt: "Which window has no fixed duration and ends after a period of user inactivity?"
    options:
      - id: oebd8a6
        text: "Sliding window"
      - id: o289c61
        text: "Session window"
        correct: true
      - id: o03db94
        text: "Tumbling window"
      - id: od2484d
        text: "Hopping window"
    explanation: "“a session window has no fixed duration ... the window ends when the user has been inactive for some time”."
  - id: sliding
    prompt: "Events at 10:03:39 and 10:08:12 — which five-minute window type groups them together?"
    options:
      - id: oe10c7c
        text: "A sliding window"
        correct: true
      - id: od4235b
        text: "A tumbling window"
      - id: oab9fff
        text: "A hopping window with 1-minute hops"
      - id: of9c2bb
        text: "None"
    explanation: "“a five-minute sliding window would cover events at 10:03:39 and 10:08:12, because they are less than five minutes apart (note that tumbling and hopping five-minute windows would not have put these two events in the same window”."
  - id: search-click
    prompt: "Why isn't embedding search details in click events equivalent to joining search and click streams?"
    options:
      - id: o7450f6
        text: "Clicks have no session ID"
      - id: oa09b8b
        text: "Click events are larger"
      - id: od2fe0e
        text: "You'd miss searches with no clicks, which are needed for accurate click-through rates"
        correct: true
      - id: o46ad35
        text: "Joins are slower"
    explanation: "“Doing so would tell you only about the cases where the user clicked a search result, not about the searches where the user did not click any of the results.”"
  - id: enrichment
    prompt: "How does a stream–table join keep its local copy of the user table current?"
    options:
      - id: ob94647
        text: "By querying the remote database for every event"
      - id: o8c7210
        text: "By reloading a snapshot daily"
      - id: o260d4f
        text: "It doesn't"
      - id: ob619d8
        text: "By subscribing to the table's CDC changelog"
        correct: true
    explanation: "“The stream processor can subscribe to a changelog of the user profile database as well as the stream of activity events. When a profile is created or modified, the stream processor updates its local copy.”"
  - id: table-table
    prompt: "How does the chapter describe the change stream of a materialized join of posts and follows?"
    options:
      - id: o51dc14
        text: "It cannot be computed incrementally"
      - id: o963169
        text: "It is the posts stream only"
      - id: o279f88
        text: "It follows the product rule: (u·v)′ = u′v + uv′"
        correct: true
      - id: ofb47ef
        text: "It equals the sum of both streams"
    explanation: "“the stream of changes to the materialized join follows the product rule (u·v)′ = u′v + uv′. Any change of posts is joined with the current followers, and any change of follows is joined with the current posts”."
  - id: scd
    prompt: "How do warehouses make joins against changing data (like tax rates) deterministic?"
    options:
      - id: oc50061
        text: "Always join with the current version"
      - id: oc84772
        text: "Ignore changes"
      - id: o159c5d
        text: "Use processing time"
      - id: o5fd4a5
        text: "Give each version of the joined record its own ID (slowly changing dimension), at the cost of no log compaction"
        correct: true
    explanation: "“every time the tax rate changes, it is given a new identifier, and the invoice includes the identifier for the tax rate at the time of sale ... it has the consequence that log compaction is not possible”."
  - id: effectively-once
    prompt: "What does \"exactly-once\" really mean, and what term does the chapter suggest?"
    options:
      - id: o82d16c
        text: "At-most-once delivery"
      - id: o29f2a8
        text: "Records may be processed multiple times, but the visible effect is as if once — \"effectively-once\""
        correct: true
      - id: o425b19
        text: "Each record is physically processed once"
      - id: oe4882d
        text: "Exactly one consumer"
    explanation: "“Although restarting tasks means that records may be processed multiple times, the visible effect in the output is as if they had been processed only once ... effectively-once would be a more descriptive term”."
  - id: microbatch
    prompt: "What trade-off sets Spark Streaming's ~1 s microbatch size?"
    options:
      - id: o4af504
        text: "Network MTU"
      - id: odc3ede
        text: "Kafka partition count"
      - id: o43036d
        text: "Smaller batches add scheduling overhead; larger ones delay results"
        correct: true
      - id: o297f55
        text: "Disk block size"
    explanation: "“smaller batches incur greater scheduling and coordination overhead, while larger batches mean a longer delay before results of the stream processor become visible.”"
  - id: side-effects
    prompt: "Why aren't microbatching or checkpointing enough when the processor sends emails or writes to a database?"
    options:
      - id: ofd843e
        text: "Emails can't be sent from processors"
      - id: o7be6a2
        text: "Databases reject writes"
      - id: o8cf0df
        text: "External side effects can't be discarded on failure, so restarts may repeat them"
        correct: true
      - id: o99dbe8
        text: "Checkpoints are too slow"
    explanation: "“as soon as output leaves the stream processor ... the framework is no longer able to discard the output of a failed microbatch. In this case, restarting a failed task causes the external side effect to happen twice”."
  - id: atomic-internal
    prompt: "How do Kafka and Google Cloud Dataflow achieve atomic commit efficiently, unlike XA?"
    options:
      - id: o5cbbaa
        text: "They use LWW"
      - id: o28b56a
        text: "They avoid transactions"
      - id: oa11088
        text: "They keep transactions internal to the framework, covering state and messaging, and amortize over batches"
        correct: true
      - id: obf60fe
        text: "They use XA across all systems"
    explanation: "“these implementations do not attempt to provide transactions across heterogeneous technologies, but instead keep the transactions internal by managing both state changes and messaging within the stream processing framework.”"
  - id: idempotence-offset
    prompt: "How can a non-idempotent database update be made idempotent when consuming from Kafka?"
    options:
      - id: o48abe0
        text: "Use a random ID each time"
      - id: oc4d2bb
        text: "Retry until it succeeds"
      - id: o7048e8
        text: "Disable retries"
      - id: o554152
        text: "Store the triggering message's offset with the value and skip updates already applied"
        correct: true
    explanation: "“When writing a value to an external database, you can include the offset of the message that triggered the last write with the value. Thus, you can tell whether an update has already been applied”."
  - id: idempotence-assumptions
    prompt: "Which assumptions does relying on idempotence require?"
    type: multiple
    options:
      - id: o2aba19
        text: "Messages are never retried"
      - id: of1301d
        text: "Replays deliver the same messages in the same order"
        correct: true
      - id: o1c71bf
        text: "Processing is deterministic"
        correct: true
      - id: o280873
        text: "No other node concurrently updates the same value (fencing may be needed)"
        correct: true
    explanation: "“restarting a failed task must replay the same messages in the same order ..., the processing must be deterministic, and no other node may concurrently update the same value”."
  - id: state-recovery
    prompt: "How do Flink and Kafka Streams make operator state recoverable?"
    options:
      - id: o7b3503
        text: "Neither recovers state"
      - id: oee28b4
        text: "Both recompute from the start of time always"
      - id: of289bf
        text: "Both query a remote database per message"
      - id: ob55352
        text: "Flink snapshots state to durable storage; Kafka Streams sends state changes to a compacted Kafka topic"
        correct: true
    explanation: "“Flink periodically captures snapshots of operator state and writes them to durable storage ... and Kafka Streams replicates state changes by sending them to a dedicated Kafka topic with log compaction”."
---
What to do with a stream: write it to storage, push it to people, or transform it into other streams. The
section covers uses, time, windows, joins and fault tolerance.

## Learning objectives

- Distinguish CEP, stream analytics, materialized view maintenance and stream search
- Explain incremental view maintenance
- Distinguish event time from processing time and handle stragglers
- Compare tumbling, hopping, sliding and session windows
- Explain stream–stream, stream–table and table–table joins and their time dependence
- Achieve exactly-once effects with microbatching, checkpoints, transactions or idempotence

## In a nutshell

- CEP and stream search store queries and run data past them.
- Approximation (HyperLogLog, Bloom filters) is an optimization, not inherent to streaming.
- IVM (Materialize, RisingWave, Feldera) updates views incrementally.
- Processing-time windows create fake spikes; event-time windows face stragglers.
- Joins need state; cross-stream ordering makes them nondeterministic unless versioned (SCD).
- Effectively-once = discard partial output: microbatches, checkpoints, internal transactions, idempotent writes.

## Connections to today

- **Flink SQL and Kafka Streams** are mainstream; **streaming databases** blur stream processors and
  warehouses.
- **Real-time feature pipelines** for ML and LLM retrieval (fresh embeddings) are stream–table joins.
- **Idempotency keys** in payment APIs (Stripe-style) are the chapter's idempotence technique applied to
  external side effects.
