---
title: Load balancing and global delivery
questions:
  - id: front-door
    prompt: You need a global HTTP entry point with CDN caching, a web application firewall and failover between regions. Which service fits best?
    options:
      - id: front-door
        text: Azure Front Door
        correct: true
      - text: Azure Load Balancer
      - text: Application Gateway
      - text: NAT Gateway
    explanation: Front Door is a global layer-7 service at Microsoft's edge with WAF, caching and health-based routing.
  - id: layer4
    prompt: Which service is a regional layer-4 (TCP/UDP) load balancer?
    options:
      - text: Azure Front Door
      - text: Traffic Manager
      - id: lb
        text: Azure Load Balancer
        correct: true
    explanation: Load Balancer distributes TCP/UDP flows within a region; it does not inspect HTTP.
  - id: matrix
    prompt: Which pairings are correct?
    type: multiple
    options:
      - text: Application Gateway — regional layer-7 load balancing with optional WAF
        correct: true
      - text: Traffic Manager — DNS-based global traffic routing
        correct: true
      - text: Front Door — global layer-7 routing and acceleration
        correct: true
      - text: Load Balancer — global HTTP path-based routing
    explanation: Path-based HTTP routing needs a layer-7 service such as Application Gateway or Front Door.
---
## Learning objectives

- Choose the right load-balancing service
- Combine global and regional services

## The four options

| Service | Scope | Layer | Highlights |
|---------|-------|-------|------------|
| Azure Front Door | Global | 7 (HTTP/S) | Edge network, CDN caching, WAF, TLS offload, path routing, health probes |
| Traffic Manager | Global | DNS | Routes by priority, weight, performance or geography; any protocol |
| Application Gateway | Regional | 7 (HTTP/S) | Path/host routing, WAF, TLS termination, AKS ingress |
| Load Balancer | Regional | 4 (TCP/UDP) | Ultra-low latency, high throughput, internal or public |

## Decision guide

- Public web app or API, possibly multi-region → **Front Door**
- Regional web traffic that must stay inside a VNet with WAF → **Application Gateway**
- Non-HTTP traffic within a region → **Load Balancer**
- Non-HTTP global failover → **Traffic Manager**

## A typical multi-region web app

```text
Users ──▶ Front Door (WAF, caching)
            ├──▶ App Service (West Europe)  ──▶ Azure SQL failover group primary
            └──▶ App Service (North Europe) ──▶ Azure SQL failover group secondary
```

Front Door health probes route around a failed region automatically. Restrict the App Services to
accept traffic only from your Front Door profile (access restrictions with the `AzureFrontDoor.Backend`
service tag plus the `X-Azure-FDID` header check).

## Health endpoints

Every load balancer needs a meaningful health endpoint:

```csharp
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: ["ready"]);

app.MapHealthChecks("/healthz/live", new() { Predicate = _ => false });
app.MapHealthChecks("/healthz/ready", new() { Predicate = c => c.Tags.Contains("ready") });
```

Liveness says "the process works"; readiness says "I can serve traffic, dependencies included".
