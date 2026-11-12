---
title: Cloud and distributed-systems basics
questions:
  - id: object-storage
    prompt: How does object storage (S3, ADLS, GCS) differ from a file system?
    options:
      - id: objects
        text: It stores immutable objects by key over HTTP APIs, with no in-place edits, no true directories and no atomic rename of folders
        correct: true
      - text: It is identical to a local disk
      - text: It supports database transactions natively
      - text: It is only for backups
    explanation: These properties are exactly why table formats need a transaction log.
  - id: iam
    prompt: What is an IAM role (or managed identity)?
    options:
      - text: A user's password
      - text: A network firewall
      - id: identity
        text: A cloud identity with permissions that services can assume to obtain temporary credentials, avoiding long-lived keys
        correct: true
      - text: A billing account
    explanation: Unity Catalog storage credentials wrap these.
  - id: vnet
    prompt: What is a VPC (AWS) or VNet (Azure)?
    options:
      - text: A virtual desktop
      - id: network
        text: An isolated private network in the cloud where you control address ranges, subnets, routing and firewall rules
        correct: true
      - text: A storage account
      - text: A video codec
    explanation: Classic Databricks compute runs in your VPC/VNet.
  - id: partitioning
    prompt: Why split data into partitions for distributed processing?
    options:
      - text: To encrypt it
      - text: To make it smaller
      - id: parallel
        text: So many machines can process different parts in parallel
        correct: true
      - text: Because files must be under 1 MB
    explanation: Too few partitions under-use the cluster; too many create overhead.
  - id: skew
    prompt: One Spark task runs for an hour while hundreds finish in seconds. What is the likely cause?
    options:
      - text: A slow network everywhere
      - text: Too much memory
      - text: A bad Python version
      - id: skew
        text: Data skew — one key (e.g. a null or a giant customer) holds far more rows than others
        correct: true
    explanation: AQE skew handling, salting keys, or filtering out nulls help.
  - id: scale-up-out
    prompt: What is the difference between scaling up and scaling out?
    options:
      - id: diff
        text: Scaling up uses a bigger machine; scaling out adds more machines
        correct: true
      - text: They are synonyms
      - text: Scaling up means more users
      - text: Scaling out means more storage only
    explanation: Distributed engines scale out; single-node engines scale up — and modern machines are very large.
  - id: small-files
    prompt: Why are millions of tiny files a performance problem?
    options:
      - text: Files have a minimum cost of $1
      - id: overhead
        text: Each file adds listing, open and metadata overhead and request costs, so time goes to file handling instead of reading data
        correct: true
      - text: Small files cannot be compressed
      - text: Parquet forbids small files
    explanation: Compaction (OPTIMIZE) and auto-optimise features address it.
  - id: acid
    prompt: What does each letter in ACID stand for?
    options:
      - text: Accuracy, Completeness, Integrity, Durability
      - text: Availability, Consistency, Isolation, Distribution
      - id: acid
        text: Atomicity, Consistency, Isolation, Durability
        correct: true
      - text: Atomicity, Caching, Indexing, Durability
    explanation: Delta Lake provides ACID transactions per table.
  - id: batch-stream
    prompt: What distinguishes batch from stream processing?
    options:
      - text: Batch is always faster
      - text: Streaming cannot be exactly-once
      - text: There is no difference
      - id: bounded
        text: Batch processes a bounded dataset at once; streaming processes unbounded data incrementally as it arrives
        correct: true
    explanation: Spark Structured Streaming treats a stream as an ever-growing table; `availableNow` triggers run streams in batch-like fashion.
  - id: egress
    prompt: What is cloud egress?
    options:
      - id: egress
        text: Data transferred out of a cloud region or provider, usually billed per GB
        correct: true
      - text: Data written to storage
      - text: A type of VM
      - text: A security group rule
    explanation: Cross-region sharing and multi-cloud architectures incur it.
---
## Learning objectives

- Understand object storage, identities and networks
- Recognise partitioning, skew and small-file problems
- Know ACID and batch-versus-streaming

## The CAP reminder

Distributed systems trade off consistency and availability during network partitions. Lakehouse
tables achieve consistency per table through atomic commits; cross-table consistency usually needs
pipeline design rather than database transactions (though multi-statement transactions are arriving).

## Sources

- AWS, "What is object storage?" — <https://aws.amazon.com/what-is/object-storage/>
- Kleppmann, *Designing Data-Intensive Applications*, 2nd ed. (O'Reilly)
- Apache Spark, "Structured Streaming Programming Guide" — <https://spark.apache.org/docs/latest/streaming/>
