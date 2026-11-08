---
title: Azure Functions
questions:
  - id: isolated
    prompt: Which .NET hosting model should new Azure Functions projects use?
    options:
      - text: The in-process model
      - id: isolated
        text: The isolated worker model
        correct: true
      - text: The WebJobs SDK only
      - text: Classic Cloud Services
    explanation: The isolated worker model runs your code in a separate process with full control over the .NET version and middleware. Support for the in-process model ends on 10 November 2026.
  - id: flex
    prompt: Which hosting plan is Microsoft's recommended serverless option for new function apps?
    options:
      - id: flex-consumption
        text: Flex Consumption
        correct: true
      - text: Classic Consumption on Windows
      - text: Dedicated App Service plan
    explanation: Flex Consumption offers scale to zero, fast scaling, virtual network integration and configurable always-ready instances.
  - id: bindings
    prompt: Which statements about triggers and bindings are true?
    type: multiple
    options:
      - text: Every function has exactly one trigger
        correct: true
      - text: Input and output bindings connect to services such as Blob Storage, Service Bus and Cosmos DB declaratively
        correct: true
      - text: An HTTP request, a timer, a queue message and a blob upload can all be triggers
        correct: true
      - text: A function can have several triggers at once
    explanation: One trigger per function; any number of input/output bindings.
  - id: durable
    prompt: What are Durable Functions used for?
    options:
      - text: Making functions run on more durable hardware
      - id: stateful
        text: Writing stateful, long-running workflows — orchestrations, fan-out/fan-in, human approval — in code
        correct: true
      - text: Storing files permanently
    explanation: The Durable Task framework checkpoints orchestration state so workflows survive restarts.
---
## Learning objectives

- Build functions with the .NET isolated worker model
- Choose a hosting plan
- Use triggers, bindings and Durable Functions

## A function app

```csharp
// Program.cs
var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Services.AddSingleton<IInvoiceRenderer, PdfInvoiceRenderer>();
builder.Build().Run();
```

```csharp
public sealed class InvoiceFunctions(IInvoiceRenderer renderer, ILogger<InvoiceFunctions> logger)
{
    [Function("RenderInvoice")]
    [BlobOutput("invoices/{OrderId}.pdf", Connection = "Storage")]
    public async Task<byte[]> Run(
        [ServiceBusTrigger("orders-paid", Connection = "ServiceBus")] OrderPaid message)
    {
        logger.LogInformation("Rendering invoice for {OrderId}", message.OrderId);
        return await renderer.RenderAsync(message);
    }

    [Function("Health")]
    public IActionResult Health([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) =>
        new OkObjectResult("ok");
}
```

With managed identities, the `Connection` settings point to service endpoints
(`ServiceBus__fullyQualifiedNamespace`) rather than connection strings with keys.

## Hosting plans

| Plan | Scale to zero | Notes |
|------|---------------|-------|
| Flex Consumption | Yes | Recommended serverless plan; VNet integration; always-ready instances |
| Consumption | Yes | Legacy serverless plan; short default timeouts |
| Premium (Elastic) | No (pre-warmed) | No cold starts, VNet, longer runs |
| Dedicated (App Service) | No | Run on an existing App Service plan |
| Container Apps | Yes | Functions on the Container Apps platform |

## Durable Functions

```csharp
[Function(nameof(ProcessOrder))]
public static async Task ProcessOrder([OrchestrationTrigger] TaskOrchestrationContext context)
{
    var order = context.GetInput<Order>()!;
    await context.CallActivityAsync(nameof(ReserveStock), order);
    var paid = await context.CallActivityAsync<bool>(nameof(ChargeCard), order);
    if (!paid)
    {
        await context.CallActivityAsync(nameof(ReleaseStock), order);
        return;
    }
    await context.CallActivityAsync(nameof(ShipOrder), order);
}
```

Orchestrator code must be deterministic (no `DateTime.Now`, random numbers or direct I/O — use the
context's APIs and activities), because it is replayed from history.

## Design tips

- Keep functions small and idempotent; triggers can deliver messages more than once
- Put business logic in plain classes injected via DI, not in the function method
- Use Application Insights (OpenTelemetry) for distributed tracing across functions
