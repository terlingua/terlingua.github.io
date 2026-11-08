---
title: Governance and cost management
questions:
  - id: policy
    prompt: Which Azure Policy effect blocks the creation of a non-compliant resource?
    options:
      - text: Audit
      - text: Disabled
      - id: deny
        text: Deny
        correct: true
      - text: AuditIfNotExists
    explanation: Audit only reports; Deny rejects the request at ARM; Modify and DeployIfNotExists can remediate.
  - id: lock
    prompt: What does a `CanNotDelete` resource lock do?
    options:
      - text: Prevents anyone from reading the resource
      - id: no-delete
        text: Allows reading and modifying the resource but prevents deleting it, even for Owners, until the lock is removed
        correct: true
      - text: Encrypts the resource
    explanation: A `ReadOnly` lock additionally blocks modifications.
  - id: cost
    prompt: Which are effective ways to reduce Azure costs?
    type: multiple
    options:
      - text: Reservations or savings plans for steady, predictable usage
        correct: true
      - text: Right-sizing or shutting down idle resources based on Azure Advisor recommendations
        correct: true
      - text: Budgets with alerts per subscription or resource group
        correct: true
      - text: Scaling every service to its largest SKU just in case
    explanation: Commit for steady load, eliminate waste and make spending visible.
---
## Learning objectives

- Enforce standards with Azure Policy
- Protect critical resources with locks
- Monitor and optimise cost

## Azure Policy

```json
{
  "if": {
    "allOf": [
      { "field": "type", "equals": "Microsoft.Storage/storageAccounts" },
      { "field": "Microsoft.Storage/storageAccounts/allowBlobPublicAccess", "equals": true }
    ]
  },
  "then": { "effect": "deny" }
}
```

| Effect | Behaviour |
|--------|-----------|
| `audit` / `auditIfNotExists` | Report non-compliance |
| `deny` | Block non-compliant requests |
| `modify` / `append` | Change properties (e.g. add tags) |
| `deployIfNotExists` | Deploy a related resource (e.g. diagnostic settings) |

Group policies into **initiatives** (for example, the *Microsoft cloud security benchmark*) and
assign them at management group scope so every subscription inherits them.

## Locks

```bash
az lock create --name protect-prod-sql --lock-type CanNotDelete \
  --resource-group rg-shop-prod --resource-name sql-shop-prod --resource-type Microsoft.Sql/servers
```

## Landing zones

The **Cloud Adoption Framework** describes landing zones: a pre-configured hierarchy of
management groups, subscriptions, networking, identity and policies into which application teams
deploy. It turns governance from a review meeting into guardrails.

## Cost management

- **Cost analysis** — break down spend by subscription, resource group, service and tag
- **Budgets** — alerts at thresholds such as 80% and 100% of a monthly amount
- **Advisor** — right-sizing and idle resource recommendations
- **Reservations and savings plans** — discounts for one- or three-year commitments
- **Azure Hybrid Benefit** — reuse existing Windows Server and SQL Server licences
- **Dev/test pricing** and auto-shutdown for non-production resources

```bash
az consumption budget create --budget-name shop-prod-monthly --amount 5000 \
  --category cost --time-grain monthly --start-date 2026-11-01 --end-date 2027-10-31 \
  --resource-group rg-shop-prod
```

Make cost a non-functional requirement reviewed like performance and security.
