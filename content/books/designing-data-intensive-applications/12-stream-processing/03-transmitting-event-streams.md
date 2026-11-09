---
title: "Transmitting Event Streams"
questions:
  - id: event-def
    prompt: "What is an event in stream processing?"
    options:
      - id: of765ff
        text: "A batch file"
      - id: o9a2724
        text: "A TCP packet"
      - id: o623efe
        text: "A mutable database row"
      - id: o16b5ab
        text: "A small, self-contained, immutable object describing something that happened at a point in time"
        correct: true
    explanation: "An event is “a small, self-contained, immutable object containing the details of something that happened at a point in time.”"
  - id: polling-cost
    prompt: "Why is polling a datastore a poor way to deliver events with low delay?"
    options:
      - id: o081a37
        text: "Datastores forbid polling"
      - id: o983864
        text: "Polling loses events"
      - id: o3a341f
        text: "Polling requires 2PC"
      - id: o249b2b
        text: "The more often you poll, the fewer polls return new events, so overhead rises"
        correct: true
    explanation: "“The more often you poll, the lower the percentage of requests that return new events, and thus the higher the overheads become.”"
  - id: two-questions
    prompt: "Which two questions help differentiate messaging systems?"
    type: multiple
    options:
      - id: obd8370
        text: "What happens if producers outpace consumers?"
        correct: true
      - id: o37158f
        text: "Which programming language does the broker use?"
      - id: o230075
        text: "Does it support SQL joins?"
      - id: o4daf2d
        text: "What happens if nodes crash or go offline — are messages lost?"
        correct: true
    explanation: "The chapter asks “What happens if the producers send messages faster than the consumers can process them?” and “What happens if nodes crash or temporarily go offline—are any messages lost?”"
  - id: overload-options
    prompt: "What are the three options when producers outpace consumers?"
    options:
      - id: oda6c67
        text: "Retry, abort or commit"
      - id: odd6a54
        text: "Drop messages, buffer them in a queue, or apply backpressure"
        correct: true
      - id: o45d774
        text: "Shard, replicate or cache"
      - id: o026801
        text: "Encrypt, compress or replicate"
    explanation: "“the system has three options: drop messages, buffer messages in a queue, or apply backpressure”."
  - id: counting-loss
    prompt: "Why is message loss more harmful when counting events than for periodic sensor readings?"
    options:
      - id: ofc586a
        text: "Sensors never lose messages"
      - id: oa794fd
        text: "Every lost message makes the count wrong, while a missing reading is soon superseded"
        correct: true
      - id: oadefff
        text: "Counting uses UDP"
      - id: o7a917e
        text: "Counters are stored on disk"
    explanation: "“with sensor readings and metrics that are transmitted periodically, an occasional missing data point is perhaps not important ... If you are counting events, it is more important that they are delivered reliably”."
  - id: direct-limits
    prompt: "What is the main limitation of direct messaging (UDP multicast, ZeroMQ, webhooks)?"
    options:
      - id: oabca41
        text: "They require Kafka"
      - id: o8678c6
        text: "They can't send JSON"
      - id: of20dba
        text: "They are always slower than brokers"
      - id: o01365e
        text: "They generally assume producers and consumers are constantly online; an offline consumer may miss messages"
        correct: true
    explanation: "“they generally assume that producers and consumers are constantly online. If a consumer is offline, it may miss messages that were sent while it is unreachable.”"
  - id: broker-vs-db
    prompt: "How do traditional message brokers differ from databases?"
    type: multiple
    options:
      - id: o6e8f55
        text: "They support secondary indexes and ad hoc SQL"
      - id: oe02d8b
        text: "They often delete messages once delivered"
        correct: true
      - id: odcc362
        text: "They assume short queues; long backlogs degrade throughput"
        correct: true
      - id: of407b6
        text: "They notify clients of new data rather than supporting arbitrary queries"
        correct: true
    explanation: "The chapter notes “some message brokers automatically delete a message when it has been successfully delivered to its consumers”, that they “assume that their working set is fairly small”, and that while they don’t support arbitrary queries, “they do notify clients when data changes”."
  - id: lb-vs-fanout
    prompt: "Match: \"each message is delivered to all consumers\"."
    options:
      - id: of5af0b
        text: "Dead-letter queue"
      - id: odeba5b
        text: "Backpressure"
      - id: of66db0
        text: "Fan-out"
        correct: true
      - id: o70460a
        text: "Load balancing"
    explanation: "“Fan-out Each message is delivered to all the consumers.” Load balancing delivers each message “to one of the consumers”."
  - id: consumer-groups
    prompt: "How do Kafka consumer groups combine load balancing and fan-out?"
    options:
      - id: obc6fa9
        text: "One consumer in the whole cluster gets each message"
      - id: oaa444a
        text: "Groups cannot share a topic"
      - id: o0fe8b3
        text: "Within a group each message goes to one consumer; each separate group gets every message"
        correct: true
      - id: o1438a0
        text: "All consumers in all groups get every message"
    explanation: "“each message in the topic is sent to one of the consumers in the group ... If two separate consumer groups subscribe to the same topic, each message is sent to one consumer in each group”."
  - id: reorder
    prompt: "Why does load balancing with redelivery reorder messages?"
    options:
      - id: oc1c9b4
        text: "Producers send out of order"
      - id: o18b413
        text: "Acks are batched"
      - id: o07955c
        text: "An unacknowledged message from a crashed consumer is redelivered later to another consumer after newer messages"
        correct: true
      - id: ofe94a9
        text: "Brokers shuffle randomly"
    explanation: "“consumer 1 processes messages in the order m4, m3, m5 ... the combination of load balancing with redelivery inevitably leads to messages being reordered.”"
  - id: dlq
    prompt: "What problem do dead letter queues solve?"
    options:
      - id: o8e3edf
        text: "A poison message that crashes consumers being redelivered forever, blocking progress"
        correct: true
      - id: oca8de1
        text: "Consumers being too fast"
      - id: o4d6e86
        text: "Duplicate producers"
      - id: o82d0db
        text: "Messages being too large"
    explanation: "Instead of retrying forever, “the message is moved to a different queue to unblock consumers ... Monitoring is usually set up on DLQs—any message in the queue is an error.”"
  - id: destructive-read
    prompt: "Why is consuming from an AMQP/JMS broker \"destructive\", and what does that prevent?"
    options:
      - id: o08a439
        text: "Acknowledged messages are deleted, so you can't rerun a consumer to get the same result or let a new consumer read history"
        correct: true
      - id: oc69fee
        text: "It deletes the producer"
      - id: o64b01d
        text: "It corrupts the message"
      - id: o4f0b0c
        text: "It locks the topic"
    explanation: "“receiving a message is destructive if the acknowledgment causes it to be deleted from the broker, so you cannot run the same consumer again and expect to get the same result.”"
  - id: log-broker
    prompt: "How does a log-based broker work?"
    options:
      - id: oe8aebb
        text: "Consumers query a database"
      - id: o6e1fa1
        text: "Messages are deleted on read"
      - id: oa95f71
        text: "Producers append to a sharded log; consumers read sequentially and wait for new appends"
        correct: true
      - id: od67322
        text: "Producers push directly to consumers"
    explanation: "“A producer sends a message by appending it to the end of the log, and a consumer receives messages by reading the log sequentially.”"
  - id: ordering-scope
    prompt: "What ordering does a Kafka topic guarantee?"
    options:
      - id: o0df57c
        text: "Order by timestamp"
      - id: ob48932
        text: "No ordering at all"
      - id: o789109
        text: "Total order across the whole topic"
      - id: of33d6d
        text: "Total order within a partition, none across partitions"
        correct: true
    explanation: "“the messages within a partition are totally ordered. There is no ordering guarantee across different partitions.”"
  - id: log-downsides
    prompt: "Which are downsides of log-based load balancing by shard?"
    type: multiple
    options:
      - id: o700625
        text: "Messages can't be replayed"
      - id: o05089d
        text: "Consumer parallelism is limited to the number of shards"
        correct: true
      - id: o4ba2ad
        text: "One slow message blocks later messages in its shard"
        correct: true
      - id: o7a4fd4
        text: "Fan-out is impossible"
    explanation: "“The number of nodes sharing the work of consuming a topic can be at most the number of log shards” and a slow message “holds up the processing of subsequent messages in that shard (a form of head-of-line blocking”."
  - id: choose-broker
    prompt: "When is a JMS/AMQP-style broker preferable to a log?"
    options:
      - id: ob9cfdd
        text: "When messages are expensive to process, should be parallelized individually, and order matters little"
        correct: true
      - id: o721d59
        text: "When you need replay"
      - id: o4bc51c
        text: "When building materialized views"
      - id: o10d117
        text: "When throughput is high and order matters"
    explanation: "“when messages may be expensive to process and you want to parallelize processing on a message-by-message basis, and message ordering is not so important, the JMS/AMQP style of message broker is preferable.”"
  - id: per-user-order
    prompt: "How do you keep all events for one user in order in a sharded log?"
    options:
      - id: o56776b
        text: "Add timestamps"
      - id: o8b3b93
        text: "Use more consumer groups"
      - id: oc4ff6e
        text: "Use one topic per event type"
      - id: ofcc125
        text: "Use the user ID as the partition key so they land in one shard"
        correct: true
    explanation: "“This can be achieved by choosing the shard for an event based on the user ID of that event (in other words, making the user ID the partition key).”"
  - id: offsets
    prompt: "Why can log brokers avoid per-message acknowledgments?"
    options:
      - id: oce1b98
        text: "Messages below a consumer's offset are processed and those above aren't, so periodically recording the offset suffices"
        correct: true
      - id: od2cc10
        text: "Consumers never fail"
      - id: o528128
        text: "Messages are never processed"
      - id: o870f84
        text: "Offsets are random"
    explanation: "“the broker does not need to track acknowledgments for every single message but only to periodically record the consumer offsets.”"
  - id: offset-replay
    prompt: "A consumer crashes after processing messages but before recording their offset. What happens on restart?"
    options:
      - id: o34b461
        text: "They are skipped"
      - id: odce944
        text: "The log is truncated"
      - id: o799438
        text: "The consumer is fenced forever"
      - id: od6177d
        text: "Those messages are processed a second time"
        correct: true
    explanation: "“If the consumer had processed subsequent messages but not yet recorded their offset, those messages will be processed a second time upon restart.”"
  - id: offset-lsn
    prompt: "What is a consumer offset analogous to?"
    options:
      - id: o875fd4
        text: "A vector clock"
      - id: o4e0e43
        text: "A fencing token"
      - id: o429e61
        text: "The log sequence number in single-leader replication; the broker is like a leader and the consumer like a follower"
        correct: true
      - id: o7a886d
        text: "A Bloom filter"
    explanation: "“The offset is in fact very similar to the log sequence number ... the message broker behaves like a leader database and the consumer like a follower.”"
  - id: ring-buffer
    prompt: "What happens to a consumer that falls behind log retention?"
    options:
      - id: o9ba272
        text: "The broker blocks producers"
      - id: o24835b
        text: "Its offset points to deleted segments and it misses messages — the log is a large on-disk ring buffer"
        correct: true
      - id: o1bc4f7
        text: "The broker extends retention automatically"
      - id: o3f467b
        text: "It reads from other consumers"
    explanation: "“if a slow consumer cannot keep up ... and it falls so far behind that its consumer offset points to a deleted segment, it will miss some of the messages. Effectively, the log implements a bounded-size buffer ... also known as a circular buffer or ring buffer.”"
  - id: slow-consumer
    prompt: "What operational advantage does a log give when one consumer is slow?"
    options:
      - id: o595d8e
        text: "Only that consumer is affected; others are unaffected, and you can experiment with production logs safely"
        correct: true
      - id: o519929
        text: "The broker crashes"
      - id: o17912b
        text: "All consumers slow down equally"
      - id: o9fbeb5
        text: "Producers are blocked"
    explanation: "“Even if a consumer does fall too far behind and starts missing messages, only that consumer is affected; it does not disrupt the service for other consumers. This fact is a big operational advantage.”"
  - id: tiered
    prompt: "Why do brokers such as Kafka and Redpanda store older messages in object storage?"
    options:
      - id: o98376d
        text: "To delete messages sooner"
      - id: obe727f
        text: "To increase capacity cheaply, and Iceberg-formatted data lets batch and warehouse jobs read it directly"
        correct: true
      - id: o7645b6
        text: "Object storage is faster than disk"
      - id: o7172bd
        text: "To avoid replication"
    explanation: "Brokers “serve older messages from object storage as part of their tiered storage ... messages in object storage are stored as Iceberg tables, which enable batch and data warehouse job execution directly on the data”."
  - id: replay
    prompt: "How does replaying make log-based messaging like batch processing?"
    options:
      - id: o3fa844
        text: "Replay requires a database"
      - id: o15474a
        text: "Logs are deleted after reading"
      - id: o335bcd
        text: "Consuming doesn't change the log, so you can rerun with yesterday's offset and different code, writing output elsewhere"
        correct: true
      - id: o8b29ff
        text: "Logs are sorted like batch files"
    explanation: "“you can start a copy of a consumer with yesterday’s offset and write the output to a different location in order to reprocess the last day’s worth of messages ... This aspect makes log-based messaging more like the batch processes”."
---
Events, producers, consumers and topics; direct messaging versus brokers; AMQP/JMS-style queues versus
log-based brokers like Kafka.

## Learning objectives

- Define events, producers, consumers and topics
- Ask the two key questions about any messaging system
- Compare direct messaging, traditional brokers and log-based brokers
- Explain load balancing vs fan-out, acknowledgments, redelivery reordering and DLQs
- Explain offsets, retention as a ring buffer, and replay

## In a nutshell

- Overload options: drop, buffer, or backpressure; durability costs throughput.
- Traditional brokers delete on ack; logs keep messages and track consumer offsets.
- Load balancing + redelivery reorders messages.
- Log shards: total order within a shard, consumer parallelism ≤ shard count, head-of-line blocking.
- A slow log consumer affects only itself; replay from any offset makes streams batch-like.

## Connections to today

- **Kafka share groups** (queue-style consumption) and **diskless/object-storage Kafka** (WarpStream,
  Bufstream) are the convergence and tiering the chapter describes.
- **Webhooks** remain the default integration mechanism for SaaS events — with all the loss caveats of
  direct messaging.
- **Azure Service Bus and SQS dead-letter queues** are standard tools for poison messages.
