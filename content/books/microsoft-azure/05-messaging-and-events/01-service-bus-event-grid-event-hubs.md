---
title: Service Bus, Event Grid and Event Hubs
questions:
  - id: orders
    prompt: An order-processing system needs reliable queues, FIFO per customer, duplicate detection, dead-lettering and transactions. Which service fits best?
    options:
      - text: Event Grid
      - text: Event Hubs
      - id: service-bus
        text: Service Bus
        correct: true
      - text: Blob Storage
    explanation: Service Bus is the enterprise message broker; sessions provide ordered processing per key.
  - id: telemetry
    prompt: You need to ingest millions of telemetry events per second from devices and process them as a stream. Which service fits best?
    options:
      - text: Service Bus queues
      - text: Storage Queues
      - id: event-hubs
        text: Event Hubs
        correct: true
    explanation: Event Hubs is a partitioned, high-throughput event streaming service, with a Kafka-compatible endpoint.
  - id: event-grid
    prompt: Which statements about Event Grid are true?
    type: multiple
    options:
      - text: It routes discrete events from sources (such as Blob Storage or your own topics) to handlers
        correct: true
      - text: It pushes events to subscribers such as Functions, webhooks and Service Bus
        correct: true
      - text: It supports the CloudEvents schema
        correct: true
      - text: It is designed to store event streams for days so consumers can replay them
    explanation: Event Grid is for reactive event distribution. Replayable streams are the job of Event Hubs.
---
## Learning objectives

- Distinguish messages from events
- Choose the right Azure messaging service
- Send and receive Service Bus messages from .NET

## Messages vs events

- A **message** is a command or data the sender expects someone to process ("charge this card")
- An **event** is a notification that something happened ("blob created"); the publisher doesn't
  care who reacts

## Choosing

| Service | Model | Use for |
|---------|-------|---------|
| **Service Bus** | Broker: queues and topics/subscriptions | Business workflows, commands, ordered and transactional processing |
| **Event Grid** | Push-based pub/sub of discrete events | Reacting to resource events and app events |
| **Event Hubs** | Partitioned streaming log | Telemetry, clickstreams, high-volume ingestion |
| **Storage Queues** | Simple queue | Basic, cheap, large backlogs, minimal features |

## Service Bus from .NET

```csharp
await using var client = new ServiceBusClient("sb-shop-prod.servicebus.windows.net", credential);

await using var sender = client.CreateSender("orders");
await sender.SendMessageAsync(new ServiceBusMessage(BinaryData.FromObjectAsJson(orderPlaced))
{
    MessageId = orderPlaced.OrderId.ToString(),   // enables duplicate detection
    SessionId = orderPlaced.CustomerId.ToString(),
    Subject = "OrderPlaced"
}, ct);
```

```csharp
await using var processor = client.CreateSessionProcessor("orders", new ServiceBusSessionProcessorOptions
{
    MaxConcurrentSessions = 8,
    AutoCompleteMessages = false
});

processor.ProcessMessageAsync += async args =>
{
    var evt = args.Message.Body.ToObjectFromJson<OrderPlaced>();
    await handler.HandleAsync(evt, args.CancellationToken);
    await args.CompleteMessageAsync(args.Message);
};
processor.ProcessErrorAsync += args => { logger.LogError(args.Exception, "Service Bus error"); return Task.CompletedTask; };

await processor.StartProcessingAsync(ct);
```

## Reliability patterns

- **Peek-lock** — a message is locked while processed; complete it on success, abandon it to retry
- **Dead-letter queue** — messages that exceed max delivery count go to a DLQ for inspection
- **Idempotent handlers** — at-least-once delivery means duplicates can happen
- **Outbox pattern** — write the message to your database in the same transaction as the state
  change, then publish, so you never lose or invent events

## Event Hubs basics

Producers send to partitions; consumer groups read independently, each tracking its position
(checkpoint) — like Kafka. Retention lets consumers replay; Capture writes events to storage
automatically.
