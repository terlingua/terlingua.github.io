---
title: Security services and cost management
questions:
  - id: secrets
    prompt: Which service is designed to store database credentials and rotate them automatically?
    options:
      - text: Amazon S3
      - text: AWS KMS
      - text: Amazon Inspector
      - id: secrets-manager
        text: AWS Secrets Manager
        correct: true
    explanation: Secrets Manager stores secrets encrypted with KMS and can rotate them with Lambda-based rotation, including built-in rotation for RDS.
  - id: guardduty
    prompt: What does Amazon GuardDuty do?
    options:
      - text: Encrypts EBS volumes
      - text: Generates IAM policies
      - id: threat
        text: Continuously analyses logs such as CloudTrail, VPC Flow Logs and DNS to detect threats like compromised credentials or crypto-mining
        correct: true
    explanation: GuardDuty is a managed threat-detection service you can enable organisation-wide.
  - id: cost
    prompt: Which are effective AWS cost practices?
    type: multiple
    options:
      - text: Activate cost allocation tags and analyse spend in Cost Explorer
        correct: true
      - text: Set AWS Budgets alerts and enable Cost Anomaly Detection
        correct: true
      - text: Cover steady usage with Savings Plans and right-size with Compute Optimizer
        correct: true
      - text: Leave unattached EBS volumes, idle NAT gateways and old snapshots running indefinitely
    explanation: Waste hides in idle resources; visibility and commitments do the rest.
---
## Learning objectives

- Protect data with KMS and Secrets Manager
- Detect threats and misconfigurations
- Control and optimise costs

## Encryption with KMS

**AWS KMS** manages encryption keys used by S3, EBS, RDS, DynamoDB, Secrets Manager and more.
Services use **envelope encryption**: data keys encrypt data, and a KMS key encrypts the data keys.
Customer managed keys let you control key policies, rotation and cross-account use; every key use
is logged in CloudTrail.

## Secrets

| Service | Use |
|---------|-----|
| Secrets Manager | Credentials with rotation, cross-account sharing |
| Systems Manager Parameter Store | Configuration values and simple secrets (`SecureString`) |

```csharp
var secret = await secretsManager.GetSecretValueAsync(new GetSecretValueRequest { SecretId = "shop/db" }, ct);
var credentials = JsonSerializer.Deserialize<DbCredentials>(secret.SecretString);
```

Cache secrets in memory and refresh periodically rather than calling the API per request.

## Detection and posture

| Service | Purpose |
|---------|---------|
| GuardDuty | Threat detection from logs and runtime signals |
| Security Hub | Aggregated findings and best-practice checks |
| Inspector | Vulnerability scanning for EC2, container images and Lambda |
| Macie | Discover sensitive data (PII) in S3 |
| IAM Access Analyzer | External and unused access findings |
| AWS WAF / Shield | Web exploits and DDoS protection |

Enable these organisation-wide from a delegated security account.

## Cost management

- **Cost Explorer** — analyse by service, account, Region and tag
- **Budgets** — alerts on actual or forecast spend
- **Cost Anomaly Detection** — machine-learning alerts on unusual spend
- **Compute Optimizer** and **Trusted Advisor** — right-sizing and idle resources
- **Savings Plans / Reserved Instances** — commitments for steady usage
- **Spot and Graviton** — cheaper compute where workloads allow

Common hidden costs: NAT gateway data processing (use gateway endpoints for S3/DynamoDB),
cross-AZ data transfer, unattached EBS volumes, old snapshots, idle load balancers and verbose
CloudWatch Logs retained forever.

> Treat cost like any other metric: tag it, watch it, alert on it, and review it in your
> engineering rituals.
