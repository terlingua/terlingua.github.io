---
title: Amazon RDS and Aurora
questions:
  - id: multi-az
    prompt: What is the primary purpose of an RDS Multi-AZ deployment?
    options:
      - text: Scaling read traffic
      - text: Cheaper storage
      - id: ha
        text: High availability — a synchronously replicated standby in another AZ takes over automatically on failure
        correct: true
      - text: Cross-Region disaster recovery
    explanation: In the classic Multi-AZ instance deployment, the standby does not serve reads; read replicas are the tool for read scaling.
  - id: aurora-storage
    prompt: How does Aurora store data?
    options:
      - text: On a single EBS volume attached to the primary
      - id: six-copies
        text: In a distributed storage volume that keeps six copies across three Availability Zones
        correct: true
      - text: In S3 only
    explanation: The shared storage layer lets Aurora replicas read the same volume with low replica lag.
  - id: features
    prompt: Which statements about RDS and Aurora are true?
    type: multiple
    options:
      - text: RDS supports engines including PostgreSQL, MySQL, MariaDB, Oracle and SQL Server
        correct: true
      - text: Aurora is compatible with MySQL and PostgreSQL
        correct: true
      - text: RDS Proxy pools connections, which helps with many short-lived connections such as from Lambda
        correct: true
      - text: With RDS you must patch the database engine binaries on the host yourself over SSH
    explanation: RDS manages patching during maintenance windows; you don't get host access.
---
## Learning objectives

- Choose between RDS engines and Aurora
- Design for availability and read scaling
- Connect securely from .NET

## RDS

Managed relational databases: provisioning, patching, backups, monitoring and failover are handled
for you. Engines: **PostgreSQL, MySQL, MariaDB, Oracle, SQL Server, Db2**.

| Feature | Purpose |
|---------|---------|
| Multi-AZ | Automatic failover to a synchronous standby |
| Read replicas | Asynchronous copies to scale reads (also cross-Region) |
| Automated backups | Point-in-time restore within the retention window |
| Snapshots | Manual backups kept until deleted |

## Aurora

Aurora re-architects MySQL- and PostgreSQL-compatible databases on a distributed storage layer:

- Six copies across three AZs; tolerates the loss of an AZ for writes
- Up to 15 low-lag read replicas with a reader endpoint
- **Aurora Serverless v2** scales capacity in fine-grained increments
- **Global Database** for low-latency cross-Region reads and fast regional failover

## Connecting from .NET

```csharp
builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("Shop")!);
```

```json
{
  "ConnectionStrings": {
    "Shop": "Host=shop.cluster-abc.eu-central-1.rds.amazonaws.com;Database=shop;Username=shop_app;SSL Mode=VerifyFull"
  }
}
```

Keep the password out of configuration files: inject it from **Secrets Manager** (with automatic
rotation) or use **IAM database authentication**, where the app generates a short-lived token with
its role instead of a password.

## Networking

Place databases in **private subnets**, allow inbound traffic only from the application's security
group, and never make production databases publicly accessible.

## Serverless options

For spiky or unpredictable workloads, Aurora Serverless v2 avoids paying for idle capacity, and
Aurora DSQL offers a distributed, serverless PostgreSQL-compatible database for active-active
multi-Region designs.
