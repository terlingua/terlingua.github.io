---
title: "Sharding for Multitenancy"
questions:
  - id: resource-isolation
    prompt: "A tenant runs an enormous report. Why are other tenants less affected under tenant sharding?"
    options:
      - id: o8e5e16
        text: "Tenants share no network"
      - id: o845cdd
        text: "Reports are always throttled"
      - id: obae12b
        text: "Resource isolation: they run on different shards"
        correct: true
      - id: od4a25a
        text: "Reports are run on replicas"
    explanation: "“If one tenant performs a computationally expensive operation, it is less likely that other tenants’ performance will be affected if they are running on different shards.”"
  - id: permission-isolation
    prompt: "How does tenant sharding reduce the impact of an access-control bug?"
    options:
      - id: od634ea
        text: "Queries cannot span tables"
      - id: oa3cf47
        text: "Each shard has its own encryption"
      - id: o4d2740
        text: "Bugs are impossible with sharding"
      - id: ocd82a6
        text: "Physically separate datasets make it less likely one tenant can see another's data"
        correct: true
    explanation: "“it’s less likely that you will accidentally give one tenant access to another tenant’s data if those tenants’ datasets are stored physically separately”."
  - id: cell
    prompt: "What does a cell-based architecture provide?"
    options:
      - id: o55caef
        text: "Fault isolation: services and storage for a set of tenants run independently, so a fault stays in one cell"
        correct: true
      - id: o8825ff
        text: "Faster cross-tenant joins"
      - id: oeb57b9
        text: "A single shared database for all tenants"
      - id: o75b67c
        text: "Automatic schema migrations"
    explanation: "“the services and storage for a particular set of tenants are grouped into a self-contained cell ... This approach provides fault isolation: a fault in one cell remains limited to that cell”."
  - id: tenant-benefits
    prompt: "Which are benefits of tenant-based sharding?"
    type: multiple
    options:
      - id: oecc70b
        text: "Rolling schema migrations out one tenant at a time"
        correct: true
      - id: oa42130
        text: "Simple data export and deletion for privacy regulations"
        correct: true
      - id: o6e15c0
        text: "Assigning a tenant's shard to a region for data residency"
        correct: true
      - id: ob41c4f
        text: "Per-tenant backup and restore"
        correct: true
      - id: ob28593
        text: "Easy cross-tenant analytics"
    explanation: "The chapter lists per-tenant backup and restore, regulatory compliance, data residence, and gradual schema rollout. Cross-tenant features are a challenge: they “become harder to implement”."
  - id: tenant-challenges
    prompt: "Which are challenges of sharding by tenant?"
    type: multiple
    options:
      - id: ob51082
        text: "Many small tenants make one shard each too costly, and grouped tenants are hard to move"
        correct: true
      - id: oc1f59a
        text: "Features that connect data across tenants need cross-shard joins"
        correct: true
      - id: obebba3
        text: "Tenants cannot be backed up"
      - id: o50aaa9
        text: "A tenant may be too large for one node"
        correct: true
    explanation: "“It assumes that each individual tenant is small enough to fit on a single node”, “creating a separate shard for each one may incur too much overhead”, and cross-tenant features “become harder to implement”."
---
Giving each tenant (or group of small tenants) its own shard brings isolation benefits and some
challenges.

## Learning objectives

- List the benefits of tenant-based sharding
- Explain cell-based architecture
- Identify the limits of per-tenant sharding

## In a nutshell

- Benefits: resource and permission isolation, cells, per-tenant restore, GDPR operations, residency, gradual migrations.
- Challenges: tenants too big for one node, too many tiny tenants, cross-tenant features.

## Connections to today

- **AWS's cell-based architecture guidance** and Slack's cellular migration are high-profile examples of
  limiting blast radius.
- **Database-per-tenant** with SQLite (Turso) or Postgres branches (Neon) makes tiny tenants cheap.
