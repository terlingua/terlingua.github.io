---
title: SQS, SNS, EventBridge and Step Functions
questions:
  - id: visibility
    prompt: In Amazon SQS, what is the visibility timeout?
    options:
      - text: How long a message is stored before deletion
      - text: How long a consumer waits for messages to arrive
      - id: hidden
        text: How long a received message stays hidden from other consumers while it is being processed
        correct: true
      - text: The delay before a new message becomes available
    explanation: If the consumer doesn't delete the message before the timeout expires, it becomes visible again and may be processed again.
  - id: fifo
    prompt: Which SQS queue type guarantees ordering within a message group and deduplicates messages?
    options:
      - text: Standard queue
      - text: Delay queue
      - id: fifo
        text: FIFO queue
        correct: true
    explanation: Standard queues offer at-least-once delivery with best-effort ordering and very high throughput.
  - id: fanout
    prompt: Which statements about AWS messaging services are true?
    type: multiple
    options:
      - text: SNS pushes each published message to all of a topic's subscribers, enabling fan-out
        correct: true
      - text: A common pattern is SNS topic → several SQS queues, one per consumer
        correct: true
      - text: EventBridge routes events with content-based rules to targets, including across accounts
        correct: true
      - text: SQS consumers receive messages pushed to an HTTP endpoint without polling
    explanation: SQS is pull-based; consumers poll (ideally with long polling), or Lambda polls for you via an event source mapping.
  - id: dlq
    prompt: What is a dead-letter queue (DLQ) used for?
    options:
      - text: Storing deleted queues
      - id: poison
        text: Isolating messages that fail processing repeatedly (after maxReceiveCount attempts) so they can be inspected and redriven
        correct: true
      - text: Speeding up message delivery
    explanation: DLQs stop poison messages from blocking or endlessly cycling through a queue.
---
## Learning objectives

- Choose between SQS, SNS, EventBridge and Step Functions
- Build reliable consumers
- Orchestrate workflows

## Which service?

| Service | Model | Use for |
|---------|-------|---------|
| **SQS** | Queue (pull) | Decoupling producers and consumers, buffering, work distribution |
| **SNS** | Pub/sub (push) | Fan-out to many subscribers (SQS, Lambda, HTTP, e-mail, SMS) |
| **EventBridge** | Event bus with rules | Event-driven architectures, SaaS and AWS service events, scheduling |
| **Kinesis Data Streams** | Ordered, replayable stream | High-volume streaming analytics |
| **Step Functions** | State machine | Orchestrating multi-step workflows with retries and compensation |

## SQS consumer in .NET

```csharp
while (!ct.IsCancellationRequested)
{
    var response = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest
    {
        QueueUrl = queueUrl,
        MaxNumberOfMessages = 10,
        WaitTimeSeconds = 20,          // long polling
        VisibilityTimeout = 60
    }, ct);

    foreach (var message in response.Messages ?? [])
    {
        await handler.HandleAsync(message.Body, ct);
        await sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, ct);
    }
}
```

Delete only after successful processing. Handlers must be **idempotent**, since standard queues
deliver at least once. (In SDK v4, unset collections may be `null`, hence `?? []`.)

## EventBridge

```json
{
  "source": ["shop.orders"],
  "detail-type": ["OrderPlaced"],
  "detail": { "total": [{ "numeric": [">", 1000] }] }
}
```

This rule pattern matches large orders and can route them to a fraud-check Lambda, while other rules
send all orders to analytics — publishers don't know or care.

```csharp
await events.PutEventsAsync(new PutEventsRequest
{
    Entries = [new PutEventsRequestEntry
    {
        EventBusName = "shop",
        Source = "shop.orders",
        DetailType = "OrderPlaced",
        Detail = JsonSerializer.Serialize(orderPlaced)
    }]
}, ct);
```

**EventBridge Scheduler** runs one-off or recurring invocations (cron) of thousands of targets, and
**EventBridge Pipes** connects sources to targets with filtering and enrichment.

## Step Functions

State machines (defined in Amazon States Language) coordinate Lambda functions, ECS tasks and
direct service integrations, with built-in retries, timeouts, parallel branches, waits for human
approval and error handling — ideal for sagas such as "reserve stock → charge card → ship, or
compensate".
