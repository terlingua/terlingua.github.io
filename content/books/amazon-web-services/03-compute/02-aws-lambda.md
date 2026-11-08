---
title: AWS Lambda
questions:
  - id: timeout
    prompt: What is the maximum execution time for a single Lambda invocation?
    options:
      - text: 30 seconds
      - text: 5 minutes
      - id: fifteen
        text: 15 minutes
        correct: true
      - text: Unlimited
    explanation: Longer work should be split up, orchestrated with Step Functions, or run on containers.
  - id: memory
    prompt: How do you give a Lambda function more CPU?
    options:
      - text: Set a CPU count in the function configuration
      - text: Lambda always uses one vCPU
      - id: memory
        text: Increase its memory setting; CPU is allocated in proportion to memory
        correct: true
    explanation: Memory ranges from 128 MB to 10,240 MB; at the top end a function gets up to 6 vCPUs.
  - id: cold-start
    prompt: Which techniques reduce cold-start latency for .NET Lambda functions?
    type: multiple
    options:
      - text: Native AOT compilation
        correct: true
      - text: SnapStart, which restores from a snapshot of an initialised execution environment
        correct: true
      - text: Provisioned concurrency
        correct: true
      - text: Making the deployment package as large as possible
    explanation: Smaller, faster-starting code and pre-initialised environments reduce cold starts.
---
## Learning objectives

- Understand Lambda's execution model and limits
- Build .NET Lambda functions, including ASP.NET Core APIs
- Tune performance and cost

## Execution model

An event (HTTP request, SQS message, S3 upload, schedule, ...) triggers your handler in an
**execution environment**. Environments are reused for later invocations ("warm"); new ones must be
initialised ("cold start").

| Limit | Value |
|-------|-------|
| Timeout | Up to 15 minutes |
| Memory | 128 MB – 10,240 MB (CPU scales with memory) |
| Ephemeral storage `/tmp` | 512 MB – 10,240 MB |
| Package | Zip (50 MB zipped / 250 MB unzipped) or container image up to 10 GB |

## A .NET handler with Lambda Annotations

```csharp
public sealed class Functions(IOrderService orders)
{
    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get, "/orders/{id}")]
    public async Task<IHttpResult> GetOrder(int id, ILambdaContext context)
    {
        var order = await orders.FindAsync(id);
        return order is null ? HttpResults.NotFound() : HttpResults.Ok(order);
    }
}

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services) =>
        services.AddSingleton<IOrderService, DynamoOrderService>();
}
```

The Annotations source generator creates the boilerplate handler and CloudFormation (SAM) template entries.

## ASP.NET Core on Lambda

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
var app = builder.Build();
app.MapGet("/health", () => "ok");
app.Run();
```

`Amazon.Lambda.AspNetCoreServer.Hosting` lets a minimal API run locally with Kestrel and on
Lambda behind API Gateway or a function URL.

## Processing queues

```csharp
public async Task<SQSBatchResponse> Handler(SQSEvent evt, ILambdaContext context)
{
    var failures = new List<SQSBatchResponse.BatchItemFailure>();
    foreach (var record in evt.Records)
    {
        try { await ProcessAsync(record.Body); }
        catch (Exception ex)
        {
            context.Logger.LogError(ex, "Failed {MessageId}", record.MessageId);
            failures.Add(new SQSBatchResponse.BatchItemFailure { ItemIdentifier = record.MessageId });
        }
    }
    return new SQSBatchResponse(failures);
}
```

With *ReportBatchItemFailures* enabled on the event source mapping, only failed messages are retried.

## Tuning

- Initialise SDK clients and connections **outside** the handler so warm invocations reuse them
- Right-size memory with AWS Lambda Power Tuning — more memory can be cheaper if it finishes faster
- Use **Native AOT** (on the `provided.al2023` runtime) or **SnapStart** for faster cold starts
- Use **Graviton (arm64)** for lower cost per GB-second
