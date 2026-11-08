---
title: Azure SQL
questions:
  - id: options
    prompt: You are migrating an on-premises SQL Server that uses SQL Server Agent jobs and cross-database queries, and want a managed PaaS service with near-100% compatibility. Which option fits best?
    options:
      - text: Azure SQL Database (single database)
      - id: mi
        text: Azure SQL Managed Instance
        correct: true
      - text: Azure Cosmos DB
      - text: Azure Table Storage
    explanation: Managed Instance provides instance-level features such as SQL Agent and cross-database queries while remaining fully managed.
  - id: serverless
    prompt: What does the serverless compute tier of Azure SQL Database provide?
    options:
      - text: Free unlimited compute
      - text: A database with no storage
      - id: autoscale-pause
        text: Automatic compute scaling within configured limits, per-second compute billing, and optional auto-pause when idle
        correct: true
    explanation: Serverless suits intermittent workloads; auto-pause means a short delay on the first connection after a pause.
  - id: hyperscale
    prompt: Which statements about Azure SQL Database service tiers are true?
    type: multiple
    options:
      - text: General Purpose is a balanced, budget-friendly tier
        correct: true
      - text: Business Critical offers low-latency local SSD storage and built-in readable replicas
        correct: true
      - text: Hyperscale supports very large databases with fast scaling and fast backups
        correct: true
      - text: All tiers require you to patch the SQL Server engine yourself
    explanation: Patching, backups and high availability are managed by Azure in every PaaS tier.
---
## Learning objectives

- Choose between SQL Database, Managed Instance and SQL Server on VMs
- Pick service tiers and compute models
- Connect securely from .NET

## The Azure SQL family

| Option | Model | Use when |
|--------|-------|----------|
| Azure SQL Database | PaaS, database-scoped | New cloud apps, SaaS, most workloads |
| Elastic pools | PaaS, shared resources | Many databases with varying usage (multi-tenant SaaS) |
| SQL Managed Instance | PaaS, instance-scoped | Lift-and-shift needing instance features |
| SQL Server on Azure VMs | IaaS | Full OS/engine control, unsupported features |

## Tiers (vCore model)

| Tier | Characteristics |
|------|-----------------|
| General Purpose | Remote storage, balanced cost; provisioned or serverless |
| Business Critical | Local SSD, high IOPS, readable secondary included |
| Hyperscale | Up to very large sizes, rapid scale-out read replicas, near-instant backups |

## Connecting from .NET

```csharp
builder.Services.AddDbContext<ShopDbContext>(o => o.UseSqlServer(
    builder.Configuration.GetConnectionString("Shop"),
    sql => sql.EnableRetryOnFailure()));
```

```json
{
  "ConnectionStrings": {
    "Shop": "Server=tcp:sql-shop-prod.database.windows.net,1433;Database=shop;Authentication=Active Directory Default;Encrypt=True;"
  }
}
```

`EnableRetryOnFailure` handles transient faults common in cloud databases (failovers, throttling).
For Dapper, use a resilience library such as Polly around commands.

## Business continuity

- Automated backups with point-in-time restore (retention configurable, long-term retention optional)
- Zone-redundant configurations for zone failures
- Active geo-replication and **failover groups** for regional disasters, with a listener endpoint
  that follows the primary

## Security

- Entra-only authentication (disable SQL logins)
- Private endpoints and disabled public network access
- Transparent Data Encryption (on by default), Always Encrypted for highly sensitive columns
- Microsoft Defender for SQL for threat detection and vulnerability assessment
