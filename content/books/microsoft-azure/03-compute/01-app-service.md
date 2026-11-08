---
title: Azure App Service
questions:
  - id: plan
    prompt: In App Service, what does the App Service plan define?
    options:
      - text: The source code of the app
      - text: The database schema
      - text: The DNS name of the app
      - id: compute
        text: The compute resources (region, OS, pricing tier, instance size and count) that one or more apps run on
        correct: true
    explanation: All apps in a plan share its instances; scaling the plan scales every app in it.
  - id: slots
    prompt: What is the main benefit of deployment slots?
    options:
      - text: They give each developer a free personal app
      - text: They store database backups
      - id: swap
        text: You deploy to a staging slot, warm it up and validate it, then swap it into production with no downtime — and swap back to roll back
        correct: true
    explanation: Slots are available on Standard tier and above. Settings can be marked as slot-specific so they don't move during a swap.
  - id: features
    prompt: Which are features of App Service?
    type: multiple
    options:
      - text: Built-in autoscale rules
        correct: true
      - text: Managed TLS certificates and custom domains
        correct: true
      - text: Running custom Linux containers
        correct: true
      - text: Built-in authentication ("Easy Auth") with Entra ID and other providers
        correct: true
    explanation: App Service is a fully managed web hosting platform with these features built in.
---
## Learning objectives

- Host .NET web apps on App Service
- Use plans, slots and scaling
- Deploy safely with zero downtime

## Plans and apps

| Tier | Typical use |
|------|-------------|
| Free / Basic | Dev/test, no slots |
| Standard | Production basics: slots, autoscale |
| Premium v3/v4 | Higher performance, more slots, zone redundancy |
| Isolated v2 | Dedicated environment (App Service Environment) for network isolation |

```bash
az appservice plan create -n asp-shop-prod -g rg-shop-prod --sku P1V3 --is-linux
az webapp create -n app-shop-prod -g rg-shop-prod --plan asp-shop-prod --runtime "DOTNETCORE:10.0"
```

## Deploying

```bash
dotnet publish -c Release -o publish
cd publish && zip -r ../app.zip . && cd ..
az webapp deploy -n app-shop-prod -g rg-shop-prod --src-path app.zip --slot staging
az webapp deployment slot swap -n app-shop-prod -g rg-shop-prod --slot staging --target-slot production
```

In practice, use GitHub Actions or Azure Pipelines with OIDC (workload identity federation) to deploy.

## Configuration

App settings appear to .NET as environment variables, overriding `appsettings.json`:

```bash
az webapp config appsettings set -n app-shop-prod -g rg-shop-prod \
  --settings ASPNETCORE_ENVIRONMENT=Production Smtp__Host=smtp.example.com
```

## Health and scaling

- Configure a **health check path** (for example `/healthz`) so unhealthy instances are replaced
- Use **autoscale** on CPU, memory or queue length; set sensible minimum instances (two or more for
  production, ideally zone-redundant)
- Enable **Always On** to avoid idle unloading on dedicated tiers

## When not to use App Service

Very spiky or event-driven workloads may suit Functions; microservices needing sidecars,
scale-to-zero or Dapr may suit Container Apps; full Kubernetes control means AKS.
