---
title: Containers on ECS, Fargate and EKS
questions:
  - id: fargate
    prompt: What is AWS Fargate?
    options:
      - text: A container image registry
      - text: A Kubernetes distribution you install yourself
      - text: A CI/CD service
      - id: serverless
        text: A serverless compute engine for containers — you run ECS tasks or EKS pods without managing EC2 instances
        correct: true
    explanation: With Fargate you specify CPU and memory per task; AWS provisions and patches the underlying hosts.
  - id: ecs-concepts
    prompt: In Amazon ECS, what keeps a specified number of tasks running and can register them with a load balancer?
    options:
      - id: service
        text: A service
        correct: true
      - text: A task definition
      - text: A cluster
    explanation: The task definition is the blueprint; the service runs and maintains copies of it; the cluster is the logical grouping.
  - id: choose
    prompt: Which statements help choose between ECS and EKS?
    type: multiple
    options:
      - text: ECS is AWS-native and simpler to operate
        correct: true
      - text: EKS runs upstream Kubernetes and suits teams standardised on Kubernetes tooling
        correct: true
      - text: Both can run on EC2 capacity or Fargate
        correct: true
      - text: Only EKS can pull images from Amazon ECR
    explanation: Both integrate with ECR, IAM, load balancers and CloudWatch.
---
## Learning objectives

- Push images to Amazon ECR
- Run containers on ECS with Fargate
- Decide between ECS and EKS

## ECR

```bash
aws ecr create-repository --repository-name shop-api
aws ecr get-login-password | docker login --username AWS --password-stdin 111122223333.dkr.ecr.eu-central-1.amazonaws.com
dotnet publish src/Shop.Api -c Release -t:PublishContainer \
  -p ContainerRegistry=111122223333.dkr.ecr.eu-central-1.amazonaws.com -p ContainerRepository=shop-api
```

Enable image scanning and lifecycle policies to remove old images.

## ECS concepts

| Concept | Meaning |
|---------|---------|
| Cluster | Logical grouping of capacity and services |
| Task definition | Blueprint: image, CPU/memory, ports, environment, roles, logging |
| Task | A running instance of a task definition |
| Service | Keeps N tasks running, integrates with load balancers and auto scaling |
| Capacity provider | Fargate, Fargate Spot, or EC2 Auto Scaling groups |

```json
{
  "family": "shop-api",
  "requiresCompatibilities": ["FARGATE"],
  "networkMode": "awsvpc",
  "cpu": "512",
  "memory": "1024",
  "runtimePlatform": { "cpuArchitecture": "ARM64", "operatingSystemFamily": "LINUX" },
  "executionRoleArn": "arn:aws:iam::111122223333:role/ecsTaskExecutionRole",
  "taskRoleArn": "arn:aws:iam::111122223333:role/shop-api-task",
  "containerDefinitions": [{
    "name": "api",
    "image": "111122223333.dkr.ecr.eu-central-1.amazonaws.com/shop-api:1.4.0",
    "portMappings": [{ "containerPort": 8080 }],
    "secrets": [{ "name": "ConnectionStrings__Shop", "valueFrom": "arn:aws:secretsmanager:eu-central-1:111122223333:secret:shop-db" }],
    "logConfiguration": { "logDriver": "awslogs", "options": { "awslogs-group": "/ecs/shop-api", "awslogs-region": "eu-central-1", "awslogs-stream-prefix": "api" } }
  }]
}
```

Secrets are injected as environment variables at start-up; the execution role needs permission to
read them, and the task role is what the app uses at runtime.

## ECS or EKS?

| | ECS | EKS |
|-|-----|-----|
| API | AWS-native | Kubernetes |
| Operational effort | Lower | Higher (add-ons, upgrades) |
| Portability | AWS-specific | Kubernetes ecosystem |
| Fargate support | Yes | Yes |

Start with ECS on Fargate unless you already run Kubernetes or need its ecosystem.
