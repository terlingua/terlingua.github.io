---
title: Regions and availability zones
questions:
  - id: az
    prompt: What is an Azure availability zone?
    options:
      - text: A country where Azure is sold
      - text: A content delivery network edge location
      - id: zone
        text: One of several physically separate groups of datacenters within a region, each with independent power, cooling and networking
        correct: true
      - text: A single server rack
    explanation: Spreading resources across zones protects against a datacenter-level failure within a region.
  - id: zone-redundant
    prompt: A service is deployed "zone-redundant". What does that mean?
    options:
      - text: It runs in every Azure region worldwide
      - id: replicated
        text: Its instances or data are automatically spread across multiple availability zones in the region
        correct: true
      - text: It is pinned to one specific zone
    explanation: Zonal resources are pinned to one zone you choose; zone-redundant resources span zones automatically.
  - id: choose-region
    prompt: Which factors should influence your choice of Azure region?
    type: multiple
    options:
      - text: Proximity to users, for latency
        correct: true
      - text: Data residency and compliance requirements
        correct: true
      - text: Availability of the services and VM sizes you need
        correct: true
      - text: Price, which varies by region
        correct: true
    explanation: All four matter. Not every service or SKU is available in every region, and prices differ.
---
## Learning objectives

- Describe regions, availability zones and region pairs
- Distinguish zonal from zone-redundant deployments
- Choose regions deliberately

## Building blocks

| Concept | Description |
|---------|-------------|
| Region | A geographic area containing one or more datacenters, e.g. *East US 2*, *West Europe* |
| Availability zone | Physically separate datacenter groups inside a region (typically three) |
| Region pair | Many regions are paired with another in the same geography for disaster recovery and staggered updates; some newer regions have no pair |
| Geography | A market containing regions, preserving data residency boundaries |

## Resiliency options

| Deployment | Survives |
|------------|----------|
| Single instance | Nothing beyond the host's own redundancy |
| Availability set (VMs) | Rack/host failures within a datacenter |
| Zonal / zone-redundant | Loss of a datacenter (zone) |
| Multi-region | Loss of a whole region |

```bash
az account list-locations --query "[?metadata.regionType=='Physical'].{name:name, display:displayName}" -o table
```

## Zonal versus zone-redundant

- **Zonal** — you pin a resource to zone 1, 2 or 3 (for example a VM) and spread copies yourself
- **Zone-redundant** — the service spreads itself (for example zone-redundant storage, App Service
  plans with zone redundancy, Azure SQL zone-redundant configurations)

Higher resiliency costs more and can add latency. Decide per workload based on its availability
and recovery objectives (SLA, RTO, RPO).
