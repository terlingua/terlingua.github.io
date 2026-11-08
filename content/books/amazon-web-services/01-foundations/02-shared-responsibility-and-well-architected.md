---
title: Shared responsibility and the Well-Architected Framework
questions:
  - id: model
    prompt: How does AWS summarise the shared responsibility model?
    options:
      - text: AWS is responsible for everything
      - text: Customers are responsible for physical data centre security
      - id: of-in
        text: AWS is responsible for security "of" the cloud; customers are responsible for security "in" the cloud
        correct: true
      - text: Responsibility depends only on the Region
    explanation: AWS secures hardware, facilities and the managed service layers; you secure your data, identities, configuration and code.
  - id: customer-ec2
    prompt: On Amazon EC2, which tasks are the customer's responsibility?
    type: multiple
    options:
      - text: Patching the guest operating system
        correct: true
      - text: Configuring security groups
        correct: true
      - text: Managing IAM permissions for the instance
        correct: true
      - text: Replacing failed physical disks in the host
    explanation: EC2 is infrastructure as a service, so everything from the guest OS up is yours.
  - id: pillars
    prompt: How many pillars does the AWS Well-Architected Framework have, and which one was added most recently?
    options:
      - text: Five; Reliability
      - id: six-sustainability
        text: Six; Sustainability
        correct: true
      - text: Four; Security
      - text: Seven; Observability
    explanation: The six pillars are Operational Excellence, Security, Reliability, Performance Efficiency, Cost Optimization and Sustainability (added in 2021).
---
## Learning objectives

- Apply the shared responsibility model to different services
- Use the six Well-Architected pillars to review designs

## Shared responsibility by service type

| Service | AWS manages | You manage |
|---------|-------------|------------|
| EC2 (IaaS) | Hardware, hypervisor, facilities | Guest OS, patches, runtime, app, security groups, data |
| RDS (managed) | Plus OS, database engine patching, backups infrastructure | Schema, users, parameters, network access, data |
| Lambda, S3, DynamoDB (serverless) | Almost all infrastructure and runtime | Code, IAM permissions, data, configuration |

Always yours: **data classification, identity and access, encryption choices and configuration**.
Most cloud breaches are misconfigurations — public buckets, over-permissive IAM — not failures of
the provider.

## The six pillars

| Pillar | Focus |
|--------|-------|
| Operational Excellence | Automate, observe, improve operations |
| Security | Identity, detection, data protection, incident response |
| Reliability | Recover from failure, scale to demand |
| Performance Efficiency | Use the right resources efficiently |
| Cost Optimization | Avoid unnecessary cost |
| Sustainability | Minimise environmental impact |

The free **AWS Well-Architected Tool** walks a workload through questions per pillar and tracks
improvement items. Specialised **lenses** (Serverless, SaaS, Machine Learning, ...) add
domain-specific guidance.
