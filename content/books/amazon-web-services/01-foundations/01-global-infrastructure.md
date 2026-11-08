---
title: Regions, Availability Zones and edge locations
questions:
  - id: az
    prompt: What is an AWS Availability Zone?
    options:
      - text: A single physical server
      - text: A country where AWS operates
      - text: A CloudFront cache
      - id: az
        text: One or more discrete data centres with redundant power, networking and connectivity, isolated from other AZs in the same Region
        correct: true
    explanation: Each Region has multiple AZs connected by low-latency links; deploying across AZs protects against a data-centre failure.
  - id: edge
    prompt: What are AWS edge locations primarily used for?
    options:
      - text: Running RDS databases
      - text: Storing S3 buckets
      - id: cdn
        text: Serving content and DNS close to users through services such as CloudFront and Route 53
        correct: true
    explanation: Edge locations far outnumber Regions and terminate user connections close to where users are.
  - id: scope
    prompt: Which statements about service scope are true?
    type: multiple
    options:
      - text: An EBS volume lives in a single Availability Zone
        correct: true
      - text: An S3 bucket is created in a specific Region
        correct: true
      - text: IAM is a global service
        correct: true
      - text: A VPC subnet can span several Availability Zones
    explanation: A subnet belongs to exactly one AZ; a VPC spans all AZs in its Region.
---
## Learning objectives

- Describe Regions, Availability Zones, Local Zones and edge locations
- Know which services are zonal, regional or global
- Design for AZ-level failure

## The hierarchy

| Concept | Description |
|---------|-------------|
| Region | A geographic area such as `us-east-1` (N. Virginia) or `eu-central-1` (Frankfurt); fully isolated from other Regions |
| Availability Zone | Isolated data-centre group within a Region, e.g. `eu-central-1a`; Regions have at least three |
| Local Zone / Wavelength | Extensions placing compute closer to specific metros or 5G networks |
| Edge location | Points of presence for CloudFront, Route 53 and Global Accelerator |

## Scope of common services

| Scope | Examples |
|-------|----------|
| Global | IAM, Route 53, CloudFront, Organizations |
| Regional | S3 buckets, DynamoDB tables, Lambda functions, VPCs, SQS queues |
| Zonal | EC2 instances, EBS volumes, subnets, NAT gateways |

## Designing for failure

> "Everything fails, all the time." — Werner Vogels, Amazon CTO

- Run at least two instances in **different AZs** behind a load balancer
- Use Multi-AZ databases
- Prefer regional, managed services (S3, DynamoDB, Lambda, SQS) that are multi-AZ by design
- Consider multi-Region only when requirements justify the cost and complexity

## Choosing a Region

Latency to users, data residency law, service and feature availability, and price all vary by
Region. New services often launch first in `us-east-1`.
