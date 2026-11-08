---
title: Microsoft Entra ID and Azure RBAC
questions:
  - id: entra
    prompt: What is Microsoft Entra ID?
    options:
      - text: A virtual machine image
      - text: A database for storing customer orders
      - id: idp
        text: Microsoft's cloud identity and access management service, formerly called Azure Active Directory
        correct: true
      - text: The on-premises Active Directory Domain Services product
    explanation: Entra ID authenticates users, groups, applications and managed identities, and issues tokens (OAuth 2.0 / OpenID Connect).
  - id: assignment
    prompt: What three elements make up an Azure role assignment?
    options:
      - id: three
        text: Security principal, role definition and scope
        correct: true
      - text: User, password and region
      - text: Subscription, resource group and tag
    explanation: "Who (principal) can do what (role definition) where (scope: management group, subscription, resource group or resource)."
  - id: roles
    prompt: Which statements about built-in Azure roles are true?
    type: multiple
    options:
      - text: Owner can manage resources and grant access to others
        correct: true
      - text: Contributor can manage resources but cannot grant access
        correct: true
      - text: Reader can view resources but not change them
        correct: true
      - text: Contributor can read secret values in every Key Vault data plane by default when the vault uses the RBAC permission model
    explanation: Control-plane roles like Contributor do not grant data-plane access such as reading secrets or blobs; data roles (e.g. Key Vault Secrets User, Storage Blob Data Reader) do.
---
## Learning objectives

- Explain Entra ID's role in Azure
- Assign Azure RBAC roles at the right scope
- Distinguish control-plane and data-plane permissions

## Entra ID

Every Azure subscription trusts one Entra tenant. Entra ID holds **users**, **groups**,
**app registrations / service principals** and **managed identities**, and issues tokens your
applications and Azure services validate.

## Azure RBAC

```bash
az role assignment create \
  --assignee-object-id <group-object-id> --assignee-principal-type Group \
  --role "Reader" \
  --scope /subscriptions/<subId>/resourceGroups/rg-shop-prod
```

| Role | Can do |
|------|--------|
| Owner | Everything, including assigning roles |
| Contributor | Manage resources, no role assignment |
| Reader | View resources |
| User Access Administrator | Manage role assignments |
| Data roles (e.g. *Storage Blob Data Contributor*, *Key Vault Secrets User*) | Access data inside a resource |

Assignments are inherited downward. Assign to groups, at the narrowest scope that works.

## Control plane vs data plane

- **Control plane** — managing the resource via ARM (create a storage account, change its SKU)
- **Data plane** — using the resource (read a blob, get a secret, query a database)

Being *Contributor* on a storage account lets you change its settings, but reading blobs with Entra
authentication requires a data role such as *Storage Blob Data Reader*.

## Entra roles are different

Entra roles (Global Administrator, User Administrator, ...) manage the directory itself — users,
groups, app registrations. Azure roles manage Azure resources. They are assigned separately.

## Privileged Identity Management

PIM provides just-in-time, time-bound and approved elevation to privileged roles, so nobody holds
standing Owner access to production.
