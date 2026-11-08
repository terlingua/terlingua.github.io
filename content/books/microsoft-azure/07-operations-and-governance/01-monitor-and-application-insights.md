---
title: Azure Monitor and Application Insights
questions:
  - id: kql
    prompt: Which query language is used in Log Analytics and Application Insights?
    options:
      - text: T-SQL
      - text: GraphQL
      - text: PromQL only
      - id: kql
        text: Kusto Query Language (KQL)
        correct: true
    explanation: KQL is a read-only query language optimised for log and telemetry analysis.
  - id: otel
    prompt: What is the recommended way to send telemetry from a modern ASP.NET Core app to Application Insights?
    options:
      - text: Writing log files to the App Service file system and downloading them daily
      - text: "`Console.WriteLine` only"
      - id: distro
        text: The Azure Monitor OpenTelemetry distro (`Azure.Monitor.OpenTelemetry.AspNetCore`) with `UseAzureMonitor()`
        correct: true
    explanation: The OpenTelemetry-based distro collects traces, metrics and logs with standard APIs.
  - id: monitor-parts
    prompt: Which are parts of Azure Monitor?
    type: multiple
    options:
      - text: Metrics — numeric time series with near-real-time alerting
        correct: true
      - text: Logs — stored in Log Analytics workspaces and queried with KQL
        correct: true
      - text: Alerts with action groups to notify people or trigger automation
        correct: true
      - text: Workbooks and dashboards for visualisation
        correct: true
    explanation: Together these give observability across Azure resources and applications.
---
## Learning objectives

- Collect telemetry with Application Insights and OpenTelemetry
- Query logs with KQL
- Alert on what matters

## Instrumenting an app

```csharp
builder.Services.AddOpenTelemetry().UseAzureMonitor();
```

With the `APPLICATIONINSIGHTS_CONNECTION_STRING` setting present, requests, dependencies (HTTP, SQL),
exceptions, logs and metrics flow to Application Insights. Custom `ActivitySource`s and `Meter`s
are picked up when added to the OpenTelemetry builder.

## Querying with KQL

```kusto
requests
| where timestamp > ago(24h)
| summarize count(), p95 = percentile(duration, 95), failures = countif(success == false) by name
| order by failures desc
```

```kusto
exceptions
| where timestamp > ago(1h)
| summarize count() by type, outerMessage
| top 10 by count_
```

## Platform logs

Resources emit metrics automatically; send their **resource logs** (for example SQL query stats,
Key Vault access, Front Door access logs) to a Log Analytics workspace with **diagnostic settings**.

## Alerting

| Alert type | Example |
|------------|---------|
| Metric | CPU > 80% for 10 minutes on the App Service plan |
| Log search | More than 20 failed requests to `/checkout` in 5 minutes |
| Activity log | Someone deleted a resource group |
| Smart detection | Unusual rise in failure rate |

Route alerts through **action groups** (e-mail, SMS, Teams, webhooks, Logic Apps). Alert on symptoms
users feel — errors and latency — rather than on every internal metric.

## Distributed tracing

Application Insights' **application map** and **transaction search** show a request flowing across
App Service, Functions, Service Bus and SQL, because W3C trace context propagates automatically
through instrumented clients.
