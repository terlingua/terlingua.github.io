---
title: Amazon VPC
questions:
  - id: public-subnet
    prompt: What makes a subnet "public" in a VPC?
    options:
      - text: It has a public name tag
      - text: It is in us-east-1
      - id: igw-route
        text: Its route table has a route to an internet gateway
        correct: true
      - text: It contains a load balancer
    explanation: Instances in a public subnet also need public IP addresses to be reachable from the internet.
  - id: sg-vs-nacl
    prompt: Which statement correctly contrasts security groups and network ACLs?
    options:
      - text: Security groups are stateless; NACLs are stateful
      - id: correct
        text: Security groups are stateful and support only allow rules at the network interface level; NACLs are stateless, support allow and deny rules, and apply at the subnet level
        correct: true
      - text: Both are stateless and support only deny rules
    explanation: Because NACLs are stateless, return traffic must be explicitly allowed (typically ephemeral ports).
  - id: nat
    prompt: Which statements about private subnet connectivity are true?
    type: multiple
    options:
      - text: A NAT gateway in a public subnet lets instances in private subnets make outbound internet connections
        correct: true
      - text: Gateway VPC endpoints let private subnets reach S3 and DynamoDB without a NAT gateway
        correct: true
      - text: Interface endpoints (AWS PrivateLink) provide private IPs for many AWS services inside your VPC
        correct: true
      - text: A NAT gateway allows inbound connections from the internet to private instances
    explanation: NAT is outbound-only. Gateway endpoints are free and save NAT data processing charges.
---
## Learning objectives

- Design a VPC with public and private subnets across AZs
- Control traffic with security groups and NACLs
- Reach AWS services privately with VPC endpoints

## A standard layout

```text
VPC 10.0.0.0/16 (eu-central-1)
├── AZ a: public 10.0.0.0/24  │ private-app 10.0.10.0/24 │ private-data 10.0.20.0/24
├── AZ b: public 10.0.1.0/24  │ private-app 10.0.11.0/24 │ private-data 10.0.21.0/24
└── AZ c: public 10.0.2.0/24  │ private-app 10.0.12.0/24 │ private-data 10.0.22.0/24

Public subnets:  load balancers, NAT gateways   (route 0.0.0.0/0 → internet gateway)
App subnets:     ECS tasks, Lambda ENIs, EC2     (route 0.0.0.0/0 → NAT gateway in same AZ)
Data subnets:    RDS, ElastiCache                (no internet route)
```

## Security groups

```text
sg-alb:  inbound 443 from 0.0.0.0/0
sg-app:  inbound 8080 from sg-alb
sg-db:   inbound 5432 from sg-app
```

Referencing security groups instead of IP ranges builds a least-privilege chain that keeps working
as instances come and go.

## Security groups vs NACLs

| | Security group | Network ACL |
|-|----------------|-------------|
| Level | Network interface | Subnet |
| State | Stateful | Stateless |
| Rules | Allow only | Allow and deny, evaluated in number order |
| Typical use | Primary control | Coarse subnet-wide blocks |

## VPC endpoints

| Type | Services | Cost |
|------|----------|------|
| Gateway endpoint | S3, DynamoDB | Free |
| Interface endpoint (PrivateLink) | Most AWS services, your own services | Hourly + data |

Endpoints keep traffic off the internet and can carry endpoint policies restricting which resources
can be reached.

## Connecting networks

VPC peering (one-to-one), **Transit Gateway** (hub-and-spoke for many VPCs and on-premises),
Site-to-Site VPN and **Direct Connect** (dedicated private links).

## Visibility

Enable **VPC Flow Logs** to record accepted and rejected traffic for troubleshooting and security analysis.
