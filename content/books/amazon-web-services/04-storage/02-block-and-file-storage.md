---
title: Block and file storage
questions:
  - id: ebs-scope
    prompt: Which statement about Amazon EBS volumes is true?
    options:
      - text: A volume can be attached to instances in any Region
      - text: EBS volumes are object storage
      - text: Data on EBS is lost when the instance stops
      - id: az
        text: A volume lives in one Availability Zone and is attached to instances in that AZ
        correct: true
    explanation: EBS is network block storage scoped to an AZ. Snapshots (stored in S3) can be copied across AZs and Regions.
  - id: instance-store
    prompt: What happens to data on an EC2 instance store volume when the instance is stopped or terminated?
    options:
      - id: lost
        text: It is lost — instance store is ephemeral
        correct: true
      - text: It is automatically copied to S3
      - text: It is moved to an EBS volume
    explanation: Use instance store only for caches, buffers and scratch data that can be regenerated.
  - id: shared
    prompt: Which services provide shared file systems that many instances can mount at once?
    type: multiple
    options:
      - text: Amazon EFS (NFS, Linux)
        correct: true
      - text: Amazon FSx for Windows File Server (SMB)
        correct: true
      - text: Amazon FSx for Lustre (high-performance computing)
        correct: true
      - text: A standard gp3 EBS volume attached to 100 instances in different AZs
    explanation: EBS Multi-Attach is limited to specific volume types within one AZ; shared file systems are the job of EFS and FSx.
---
## Learning objectives

- Choose between EBS, instance store, EFS and FSx
- Understand EBS volume types and snapshots

## Options at a glance

| Service | Type | Scope | Use |
|---------|------|-------|-----|
| EBS | Block | One AZ, one instance (mostly) | Boot volumes, databases on EC2 |
| Instance store | Block, ephemeral | The host | Caches, scratch space |
| EFS | File (NFS) | Regional, many clients | Shared content for Linux, containers |
| FSx for Windows File Server | File (SMB) | Many Windows clients | Windows shares, .NET Framework apps |
| FSx for Lustre / NetApp ONTAP / OpenZFS | File | Specialised | HPC, enterprise NAS migrations |
| S3 | Object | Regional | Almost everything else |

## EBS volume types

| Type | Characteristics |
|------|-----------------|
| gp3 | General-purpose SSD; baseline performance with independently provisioned IOPS and throughput — the default choice |
| io2 Block Express | Highest IOPS and durability for critical databases |
| st1 / sc1 | Throughput-optimised / cold HDD for large sequential workloads |

## Snapshots

```bash
aws ec2 create-snapshot --volume-id vol-0abc --description "pre-upgrade"
```

Snapshots are incremental and stored in S3. Use **Amazon Data Lifecycle Manager** or **AWS Backup**
to schedule and retain them, and copy them to another Region for disaster recovery.

## EFS for containers

EFS volumes can be mounted by ECS tasks, EKS pods and Lambda functions, which makes them a simple way
to share files between stateless compute — though for most application data, S3 or a database is
a better fit than a shared file system.
