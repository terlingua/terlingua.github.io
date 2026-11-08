---
title: Amazon DynamoDB
questions:
  - id: query-scan
    prompt: Why should you prefer `Query` over `Scan` in DynamoDB?
    options:
      - id: efficient
        text: Query reads only the items with a given partition key (optionally filtered by sort key), while Scan reads the entire table
        correct: true
      - text: Scan is not supported by the .NET SDK
      - text: Query returns results in random order
      - text: Scan costs nothing
    explanation: Scans consume capacity for every item they read, even if a filter discards most of them.
  - id: consistency
    prompt: What is the default read consistency for DynamoDB `GetItem` and `Query`?
    options:
      - id: eventual
        text: Eventually consistent; strongly consistent reads can be requested per call (not on global secondary indexes)
        correct: true
      - text: Always strongly consistent
      - text: Read-uncommitted
    explanation: Strongly consistent reads cost twice as much read capacity as eventually consistent reads.
  - id: indexes
    prompt: Which statements about DynamoDB secondary indexes are true?
    type: multiple
    options:
      - text: A global secondary index (GSI) can have a different partition key from the base table
        correct: true
      - text: GSIs can be added after the table is created
        correct: true
      - text: Local secondary indexes (LSIs) must be defined when the table is created
        correct: true
      - text: GSIs support strongly consistent reads
    explanation: GSI reads are always eventually consistent; LSIs share the table's partition key and allow strong consistency.
  - id: item-size
    prompt: What is the maximum size of a single DynamoDB item?
    options:
      - text: 64 KB
      - id: kb400
        text: 400 KB
        correct: true
      - text: 16 MB
      - text: 5 GB
    explanation: Store large blobs in S3 and keep a reference in the item.
---
## Learning objectives

- Model data around access patterns with partition and sort keys
- Use queries, indexes and conditional writes
- Choose capacity modes

## Keys

| Key | Role |
|-----|------|
| Partition key | Determines the partition; items with the same value form an *item collection* |
| Sort key (optional) | Orders items within a partition; enables range queries |

## Single-table modelling

| PK | SK | Attributes |
|----|----|-----------|
| `CUSTOMER#42` | `PROFILE` | name, email |
| `CUSTOMER#42` | `ORDER#2026-11-01#1001` | total, status |
| `CUSTOMER#42` | `ORDER#2026-11-05#1007` | total, status |

"Get a customer and their recent orders" becomes one `Query` on `PK = CUSTOMER#42`.

## .NET SDK

```csharp
var response = await dynamo.QueryAsync(new QueryRequest
{
    TableName = "Shop",
    KeyConditionExpression = "PK = :pk AND begins_with(SK, :prefix)",
    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
    {
        [":pk"] = new() { S = "CUSTOMER#42" },
        [":prefix"] = new() { S = "ORDER#2026-11" }
    },
    ScanIndexForward = false,
    Limit = 20
}, ct);
```

## Conditional writes

```csharp
await dynamo.PutItemAsync(new PutItemRequest
{
    TableName = "Shop",
    Item = item,
    ConditionExpression = "attribute_not_exists(PK)"   // create only if new
}, ct);
```

Conditions give optimistic concurrency and idempotency without locks. `TransactWriteItems` makes
up to 100 actions atomic.

## Capacity modes

| Mode | Billing | Use when |
|------|---------|----------|
| On-demand | Per request | Unknown or spiky traffic; the default for new workloads |
| Provisioned (with auto scaling) | Per provisioned RCU/WCU | Predictable traffic at lower cost |

## Other features

- **Streams** — ordered change log per item, triggering Lambda for event-driven processing
- **TTL** — automatically expire items such as sessions
- **Global tables** — multi-Region, multi-active replication
- **Point-in-time recovery** — restore to any second in the retention window

## Design rule

List your access patterns **first**, then design keys and indexes to serve them. DynamoDB rewards
up-front modelling and punishes ad-hoc querying.
