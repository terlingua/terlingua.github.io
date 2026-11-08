---
title: Azure Cosmos DB
questions:
  - id: partition-key
    prompt: What is the most important design decision when creating a Cosmos DB container?
    options:
      - text: The container's display name
      - id: pk
        text: The partition key
        correct: true
      - text: The colour scheme in Data Explorer
      - text: The number of indexes to disable
    explanation: The partition key determines how data and throughput are distributed. A good key has high cardinality and spreads reads and writes evenly.
  - id: default-consistency
    prompt: What is the default consistency level for a Cosmos DB account?
    options:
      - text: Strong
      - text: Eventual
      - id: session
        text: Session
        correct: true
      - text: Bounded staleness
    explanation: Session consistency guarantees read-your-own-writes within a session and is the most widely used level.
  - id: ru
    prompt: Which statements about Request Units (RUs) are true?
    type: multiple
    options:
      - text: RUs normalise the cost of database operations (CPU, IOPS, memory) into one currency
        correct: true
      - text: A point read of a 1 KB item by ID and partition key costs about 1 RU
        correct: true
      - text: Cross-partition queries generally cost more than single-partition queries
        correct: true
      - text: Every operation costs exactly 1 RU regardless of size or complexity
    explanation: Model data and queries so most operations are point reads or single-partition queries.
---
## Learning objectives

- Understand Cosmos DB's APIs, partitioning and consistency
- Model data around access patterns
- Use the .NET SDK efficiently

## What Cosmos DB is

A globally distributed, multi-model NoSQL database with single-digit-millisecond latency, automatic
indexing and multi-region replication. APIs include **NoSQL** (native), **MongoDB**, **Apache
Cassandra**, **Apache Gremlin** and **Table**.

## Partitioning

```text
Container "orders"  —  partition key: /customerId
  logical partition "c-1001": order-1, order-7, order-9
  logical partition "c-1002": order-2
  ...
```

- All items with the same key live in one **logical partition** (max 20 GB)
- Logical partitions are spread over **physical partitions** automatically
- Queries that include the partition key go to one partition; others fan out

## Consistency levels

| Level | Guarantee |
|-------|-----------|
| Strong | Linearizable reads |
| Bounded staleness | Reads lag by at most K versions or T time |
| **Session** (default) | Read your own writes, monotonic reads within a session |
| Consistent prefix | Never see out-of-order writes |
| Eventual | No ordering guarantee; lowest latency and cost |

## Throughput

- **Provisioned** (manual or autoscale) — RU/s reserved per container or database
- **Serverless** — pay per RU consumed; good for low or spiky traffic

## The .NET SDK

```csharp
var client = new CosmosClient("https://cosmos-shop-prod.documents.azure.com:443/", credential,
    new CosmosClientOptions { ApplicationName = "shop-api" });
var orders = client.GetContainer("shop", "orders");

await orders.UpsertItemAsync(order, new PartitionKey(order.CustomerId), cancellationToken: ct);

var response = await orders.ReadItemAsync<Order>(orderId, new PartitionKey(customerId), cancellationToken: ct);
Console.WriteLine($"Cost: {response.RequestCharge} RU");

var query = orders.GetItemQueryIterator<Order>(
    new QueryDefinition("SELECT * FROM o WHERE o.customerId = @c AND o.status = 'Open'").WithParameter("@c", customerId),
    requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(customerId) });
```

Register `CosmosClient` as a **singleton** — it manages connections and should live for the app's
lifetime.

## Modelling tips

- Model for your queries; denormalise data that is read together
- Embed child items that are bounded (order lines); reference unbounded relationships
- Use the change feed to build materialized views and trigger downstream processing
