---
title: EC2 and Auto Scaling
questions:
  - id: spot
    prompt: Which EC2 purchasing option offers the deepest discounts but can be interrupted by AWS with a two-minute warning?
    options:
      - text: On-Demand
      - id: spot
        text: Spot Instances
        correct: true
      - text: Savings Plans
      - text: Dedicated Hosts
    explanation: Spot uses spare capacity at up to 90% off. It suits fault-tolerant, flexible workloads such as batch jobs and stateless workers.
  - id: asg
    prompt: What does an Auto Scaling group do?
    options:
      - text: Increases the size of a single instance automatically
      - text: Backs up EBS volumes
      - id: maintain
        text: Maintains a desired number of instances across AZs, replaces unhealthy ones, and scales in and out based on policies
        correct: true
    explanation: Combined with a load balancer, it provides elasticity and self-healing.
  - id: families
    prompt: Which statements about EC2 instance types are true?
    type: multiple
    options:
      - text: C-family instances are compute-optimised
        correct: true
      - text: R-family instances are memory-optimised
        correct: true
      - text: Graviton instances use AWS-designed Arm processors and often offer better price-performance
        correct: true
      - text: T-family burstable instances deliver full CPU at all times regardless of credits
    explanation: T instances accrue and spend CPU credits; in standard mode they are throttled to baseline when credits run out (unlimited mode charges for extra usage instead).
---
## Learning objectives

- Choose instance types and purchasing options
- Build self-healing, elastic fleets with Auto Scaling
- Run .NET on EC2 sensibly

## Instance families

| Family | Optimised for | Example |
|--------|---------------|---------|
| M, T | General purpose (T = burstable) | `m7g.large`, `t4g.small` |
| C | Compute | `c7i.xlarge` |
| R, X | Memory | `r7g.2xlarge` |
| I, D | Storage I/O | `i4i.large` |
| P, G, Inf, Trn | Accelerated (GPU, ML chips) | `g5.xlarge` |

The `g` suffix in `m7g` denotes Graviton (Arm). .NET runs natively on Arm64, so Graviton is
often an easy price-performance win.

## Purchasing options

| Option | Discount | Commitment | Use for |
|--------|----------|------------|---------|
| On-Demand | None | None | Spiky, unpredictable, short-term |
| Savings Plans | Significant | 1 or 3 years of $/hour spend | Steady baseline (also covers Fargate and Lambda for Compute Savings Plans) |
| Reserved Instances | Significant | 1 or 3 years, specific instance attributes | Steady, predictable usage |
| Spot | Up to 90% | None; interruptible | Fault-tolerant batch and stateless work |

## Auto Scaling

```bash
aws autoscaling create-auto-scaling-group \
  --auto-scaling-group-name shop-web \
  --launch-template LaunchTemplateName=shop-web,Version='$Latest' \
  --min-size 2 --max-size 10 --desired-capacity 2 \
  --vpc-zone-identifier "subnet-aaa,subnet-bbb,subnet-ccc" \
  --target-group-arns arn:aws:elasticloadbalancing:...:targetgroup/shop-web/abc \
  --health-check-type ELB --health-check-grace-period 120

aws autoscaling put-scaling-policy --auto-scaling-group-name shop-web \
  --policy-name cpu50 --policy-type TargetTrackingScaling \
  --target-tracking-configuration '{"PredefinedMetricSpecification":{"PredefinedMetricType":"ASGAverageCPUUtilization"},"TargetValue":50}'
```

A **launch template** defines the AMI, instance type, security groups, IAM instance profile and
user data. **Target tracking** keeps a metric near a target value.

## Treat servers as cattle

Bake images (EC2 Image Builder), keep instances stateless, store state in managed services and
replace instances rather than patching them by hand. Use **Systems Manager Session Manager** for
shell access instead of opening SSH to the internet.
