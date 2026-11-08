---
title: The resource hierarchy and Azure Resource Manager
questions:
  - id: order
    prompt: What is the correct order of the Azure management hierarchy, from broadest to narrowest?
    options:
      - text: Resource group → subscription → management group → resource
      - text: Subscription → management group → resource → resource group
      - id: right
        text: Management group → subscription → resource group → resource
        correct: true
      - text: Resource → resource group → subscription → management group
    explanation: Policies and role assignments applied at a level are inherited by everything below it.
  - id: rg-delete
    prompt: What happens when you delete a resource group?
    options:
      - text: Only the group is deleted; resources move to a default group
      - id: all
        text: All resources inside it are deleted
        correct: true
      - text: Nothing, unless the group is empty
    explanation: Group resources by lifecycle so they can be deployed and deleted together. Resource locks can prevent accidental deletion.
  - id: arm
    prompt: Which statements about Azure Resource Manager (ARM) are true?
    type: multiple
    options:
      - text: The portal, CLI, PowerShell, SDKs, Bicep and Terraform all send requests through ARM
        correct: true
      - text: ARM applies authentication, RBAC, policy and locks consistently to every request
        correct: true
      - text: A resource belongs to exactly one resource group
        correct: true
      - text: All resources in a resource group must be in the same region as the group
    explanation: A resource group has a location for its metadata, but its resources can live in other regions.
---
## Learning objectives

- Navigate management groups, subscriptions, resource groups and resources
- Understand Azure Resource Manager's role
- Organise resources by lifecycle

## The hierarchy

```text
Microsoft Entra tenant
└── Root management group
    └── Management groups (e.g. Platform, Landing Zones, Sandbox)
        └── Subscriptions (billing + access boundary)
            └── Resource groups (lifecycle containers)
                └── Resources (web apps, databases, storage accounts...)
```

| Level | Typical use |
|-------|-------------|
| Management group | Apply policy and RBAC to many subscriptions |
| Subscription | Billing boundary, quotas, environment separation (prod/non-prod) |
| Resource group | Resources that share a lifecycle — deployed, updated and deleted together |

## Azure Resource Manager

Every management operation goes through ARM, which authenticates the caller, checks RBAC,
evaluates Azure Policy and locks, then forwards the request to the resource provider
(`Microsoft.Web`, `Microsoft.Sql`, ...). Resources have IDs like:

```text
/subscriptions/{subId}/resourceGroups/rg-shop-prod/providers/Microsoft.Web/sites/app-shop-prod
```

## Working with the CLI

```bash
az login
az account set --subscription "Shop Production"
az group create --name rg-shop-prod --location westeurope --tags env=prod app=shop
az resource list --resource-group rg-shop-prod -o table
```

## Naming and tagging

Adopt a convention, for example `{type}-{workload}-{env}-{region}`: `app-shop-prod-weu`,
`sql-shop-prod-weu`. Tag resources with `env`, `app`, `owner` and `costCenter` so costs and
ownership are traceable. Enforce both with Azure Policy (see the governance chapter).
