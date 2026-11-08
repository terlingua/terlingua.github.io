---
title: Infrastructure as code with Bicep and azd
questions:
  - id: bicep
    prompt: What is Bicep?
    options:
      - text: A configuration management agent installed on VMs
      - text: A container orchestrator
      - id: dsl
        text: A domain-specific language for declaring Azure resources that compiles to ARM templates
        correct: true
      - text: A replacement for Azure Resource Manager
    explanation: Bicep is a friendlier syntax over ARM JSON, with modules, type checking and first-class tooling.
  - id: what-if
    prompt: What does `az deployment group what-if` do?
    options:
      - text: Deploys resources immediately
      - id: preview
        text: Previews the changes a deployment would make without applying them
        correct: true
      - text: Deletes resources not in the template
    explanation: Reviewing what-if output in pull requests catches unintended changes before they happen.
  - id: azd
    prompt: Which statements about the Azure Developer CLI (`azd`) are true?
    type: multiple
    options:
      - text: "`azd up` provisions infrastructure and deploys application code in one command"
        correct: true
      - text: It uses templates that combine app code, Bicep or Terraform, and an `azure.yaml` file
        correct: true
      - text: It can set up a CI/CD pipeline with `azd pipeline config`
        correct: true
      - text: It only works with Java applications
    explanation: "`azd` works with .NET, Node.js, Python, Java and more."
---
## Learning objectives

- Declare Azure resources with Bicep
- Preview and deploy changes safely
- Use `azd` for an end-to-end developer workflow

## A Bicep file

```bicep
param location string = resourceGroup().location
param env string
var suffix = '${env}-${uniqueString(resourceGroup().id)}'

resource plan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: 'asp-shop-${suffix}'
  location: location
  sku: { name: 'P1v3', capacity: 2 }
  kind: 'linux'
  properties: { reserved: true }
}

resource app 'Microsoft.Web/sites@2024-04-01' = {
  name: 'app-shop-${suffix}'
  location: location
  identity: { type: 'SystemAssigned' }
  properties: {
    serverFarmId: plan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      healthCheckPath: '/healthz/ready'
      minTlsVersion: '1.2'
    }
  }
}

output appPrincipalId string = app.identity.principalId
```

## Deploying

```bash
az deployment group what-if -g rg-shop-prod -f main.bicep -p env=prod
az deployment group create  -g rg-shop-prod -f main.bicep -p env=prod
```

Deployments are **declarative and idempotent**: running the same template twice makes no changes.

## Modules

```bicep
module sql 'modules/sql.bicep' = {
  name: 'sql'
  params: { location: location, env: env, adminGroupObjectId: adminGroupId }
}
```

Modules (local or from a Bicep registry) keep templates small and reusable. **Azure Verified
Modules** provide Microsoft-maintained building blocks.

## Azure Developer CLI

```bash
azd init --template todo-csharp-sql
azd up            # provision + deploy
azd deploy        # code only
azd down          # tear everything down
```

## Terraform

Terraform with the `azurerm` provider is a popular alternative, especially for multi-cloud teams.
Whatever the tool, the principles are the same: everything in Git, changes through pull requests,
previews before applies, and no manual portal changes in production.
