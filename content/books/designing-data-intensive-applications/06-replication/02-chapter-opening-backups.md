---
title: "Chapter Opening — Why Replicate, and Why You Still Need Backups"
questions:
  - id: why-replicate
    prompt: "Which are reasons to replicate data?"
    type: multiple
    options:
      - id: ob47bab
        text: "Keep working when some parts fail"
        correct: true
      - id: o4a2cd8
        text: "Store datasets too large for one machine"
      - id: o92a0e5
        text: "Keep data geographically close to users"
        correct: true
      - id: o4bbad6
        text: "Scale out read throughput"
        correct: true
    explanation: "The chapter lists latency, availability and durability, and read throughput. Datasets too big for one machine are the subject of sharding: “In Chapter 7 we will relax that assumption”."
  - id: difficulty
    prompt: "Where does the chapter say all the difficulty of replication lies?"
    options:
      - id: of79c93
        text: "In choosing hardware"
      - id: o7bcf1a
        text: "In copying data the first time"
      - id: oed2c17
        text: "In handling changes to replicated data"
        correct: true
      - id: o9a70a6
        text: "In encrypting replicas"
    explanation: "“If the data that you’re replicating does not change over time, replication is easy ... All the difficulty in replication lies in handling changes to replicated data”."
  - id: backups-needed
    prompt: "An engineer accidentally deletes a table on a three-replica cluster. Why doesn't replication help?"
    options:
      - id: odd2d64
        text: "Followers ignore deletes"
      - id: oce7e7d
        text: "Replication is paused during deletes"
      - id: oac9274
        text: "The deletion is propagated to the replicas too; only a backup of an older snapshot can restore it"
        correct: true
      - id: ofc9f61
        text: "Replicas keep deleted rows for 30 days"
    explanation: "“If you accidentally delete some data, replication doesn’t help since the deletion will also have been propagated to the replicas; you need a backup”."
  - id: backups-complementary
    prompt: "How are backups and replication complementary?"
    options:
      - id: o66cde0
        text: "They cannot be combined"
      - id: o40a84b
        text: "Backups replace followers"
      - id: of0fab0
        text: "Backups can be used to set up new followers, and archived replication logs can form part of a backup"
        correct: true
      - id: o99a346
        text: "Replication replaces backups after three replicas"
    explanation: "“Backups are sometimes part of the process of setting up replication ... Conversely, archiving replication logs can be part of a backup process.”"
  - id: old-principles
    prompt: "Why haven't replication principles changed much since the 1970s?"
    options:
      - id: oa3912a
        text: "Hardware has not changed"
      - id: o5a5cf2
        text: "Replication is a solved problem"
      - id: o7b6e20
        text: "The fundamental constraints of networks have remained the same"
        correct: true
      - id: o66c939
        text: "Nobody has researched replication since"
    explanation: "“The principles haven’t changed much since they were studied in the 1970s because the fundamental constraints of networks have remained the same.”"
---
Replication keeps copies close to users, available despite failures, and able to serve more reads. All
the difficulty lies in handling changes.

## Learning objectives

- List the reasons to replicate
- Explain why replication does not replace backups

## In a nutshell

- Three families: single-leader, multi-leader, leaderless.
- Replication propagates mistakes; backups let you go back in time.
- Archived replication logs plus snapshots can form a backup.

## Connections to today

- **Ransomware and accidental `DROP TABLE`** are replicated within milliseconds; point-in-time
  recovery from object storage is what saves you.
- **Litestream and WAL-G** stream SQLite or Postgres WAL to S3, combining replication and backup.
