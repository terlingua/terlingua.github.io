---
title: Containers on Azure
questions:
  - id: aca
    prompt: Which service offers serverless containers with scale to zero, revisions, KEDA-based autoscaling and optional Dapr, without managing Kubernetes?
    options:
      - text: Azure Kubernetes Service
      - text: Azure Container Instances
      - id: aca
        text: Azure Container Apps
        correct: true
      - text: Azure Batch
    explanation: Container Apps runs on Kubernetes under the hood but hides the cluster.
  - id: aks
    prompt: When is Azure Kubernetes Service (AKS) the better choice?
    options:
      - text: When you want to avoid learning anything about containers
      - id: control
        text: When you need full Kubernetes API access, custom operators, or fine control over networking and node pools
        correct: true
      - text: When you run a single static website
    explanation: AKS gives you a managed control plane, but you operate workloads, upgrades and node pools.
  - id: acr
    prompt: Which statements about Azure Container Registry (ACR) are true?
    type: multiple
    options:
      - text: It stores private container images and OCI artefacts
        correct: true
      - text: Apps can pull images using a managed identity with the AcrPull role
        correct: true
      - text: ACR Tasks can build images in the cloud
        correct: true
      - text: Images must be public to be used by Container Apps
    explanation: Private registries with identity-based pulls are the norm.
---
## Learning objectives

- Choose between Container Apps, AKS and Container Instances
- Build and push images to Azure Container Registry
- Deploy a .NET container to Container Apps

## Choosing a container platform

| Service | Best for | You manage |
|---------|----------|------------|
| Container Instances (ACI) | Single containers, short jobs | Almost nothing |
| Container Apps (ACA) | Microservices, APIs, event-driven workers, jobs | Apps and scaling rules |
| AKS | Complex platforms needing full Kubernetes | Workloads, upgrades, node pools |
| App Service (containers) | Web apps packaged as containers | Very little |

## Build and push

```bash
az acr create -n acrshopprod -g rg-shop-prod --sku Standard
dotnet publish src/Shop.Api -c Release -t:PublishContainer \
  -p ContainerRegistry=acrshopprod.azurecr.io -p ContainerRepository=shop-api -p ContainerImageTag=1.4.0
```

(Run `az acr login -n acrshopprod` first so the SDK can push.)

## Deploy to Container Apps

```bash
az containerapp env create -n cae-shop-prod -g rg-shop-prod --location westeurope
az containerapp create -n ca-shop-api -g rg-shop-prod --environment cae-shop-prod \
  --image acrshopprod.azurecr.io/shop-api:1.4.0 \
  --registry-server acrshopprod.azurecr.io --registry-identity system \
  --target-port 8080 --ingress external \
  --min-replicas 0 --max-replicas 10
```

## Scaling with KEDA

Container Apps scale on HTTP concurrency, CPU, memory or event sources via KEDA scalers — for
example, the number of messages in a Service Bus queue. Workers can scale to zero when the queue
is empty and burst when messages arrive.

## Revisions and traffic splitting

Each deployment creates a **revision**. In multiple-revision mode you can send 10% of traffic to
the new revision, watch metrics, then shift the rest — canary releases without extra tooling.

## Container Apps jobs

Jobs run containers to completion on a schedule, on demand or in response to events — a good home
for batch tasks and migrations.
