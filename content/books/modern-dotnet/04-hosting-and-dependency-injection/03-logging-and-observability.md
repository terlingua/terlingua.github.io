---
title: Logging, metrics and tracing
questions:
  - id: template
    prompt: Which logging call follows structured logging best practice?
    options:
      - text: "`logger.LogInformation($\"Order {order.Id} placed\");`"
      - id: template
        text: "`logger.LogInformation(\"Order {OrderId} placed\", order.Id);`"
        correct: true
      - text: "`logger.LogInformation(\"Order \" + order.Id + \" placed\");`"
    explanation: A message template keeps `OrderId` as a separate, queryable property and avoids formatting cost when the level is disabled.
  - id: loggermessage
    prompt: What does the `[LoggerMessage]` source generator provide?
    options:
      - text: Automatic log shipping to the cloud
      - text: Encryption of log files
      - id: fast
        text: Compile-time generated, strongly typed, allocation-efficient logging methods
        correct: true
    explanation: The generator creates the logging code at compile time, avoiding boxing and template parsing at runtime.
  - id: signals
    prompt: Which are the three main OpenTelemetry signals?
    type: multiple
    options:
      - text: Traces
        correct: true
      - text: Metrics
        correct: true
      - text: Logs
        correct: true
      - text: Breakpoints
    explanation: OpenTelemetry standardises traces, metrics and logs, exported with OTLP to any compatible backend.
  - id: activity
    prompt: In .NET, which built-in type represents an OpenTelemetry span?
    options:
      - text: "`Span<T>`"
      - id: activity
        text: "`System.Diagnostics.Activity`"
        correct: true
      - text: "`Task`"
    explanation: .NET's tracing API predates OpenTelemetry; `ActivitySource` and `Activity` map directly to OpenTelemetry tracers and spans.
---
## Learning objectives

- Write structured logs with message templates
- Use the `LoggerMessage` source generator
- Instrument an app with OpenTelemetry traces and metrics

## Structured logging

```csharp
logger.LogInformation("Order {OrderId} placed by {CustomerId} for {Total}", order.Id, order.CustomerId, order.Total);
```

Sinks store `OrderId`, `CustomerId` and `Total` as fields you can filter on. Never use string
interpolation in log calls — it destroys structure and always pays the formatting cost.

## Source-generated logging

```csharp
public static partial class Log
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Warning, Message = "Payment declined for order {OrderId}: {Reason}")]
    public static partial void PaymentDeclined(this ILogger logger, int orderId, string reason);
}

logger.PaymentDeclined(order.Id, result.Reason);
```

## OpenTelemetry

```csharp
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("shop-api"))
    .WithTracing(t => t
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource(ShopTelemetry.Source.Name))
    .WithMetrics(m => m
        .AddAspNetCoreInstrumentation()
        .AddMeter(ShopTelemetry.Meter.Name))
    .UseOtlpExporter();

builder.Logging.AddOpenTelemetry(o => o.IncludeFormattedMessage = true);
```

## Custom traces and metrics

```csharp
public static class ShopTelemetry
{
    public static readonly ActivitySource Source = new("Shop.Orders");
    public static readonly Meter Meter = new("Shop.Orders");
    public static readonly Counter<long> OrdersPlaced = Meter.CreateCounter<long>("shop.orders.placed");
}

using var activity = ShopTelemetry.Source.StartActivity("PlaceOrder");
activity?.SetTag("order.id", order.Id);
ShopTelemetry.OrdersPlaced.Add(1, new KeyValuePair<string, object?>("channel", "web"));
```

`StartActivity` returns `null` when nobody is listening, so instrumentation costs almost nothing
until a collector is attached. Aspire's dashboard is a convenient local OTLP viewer during
development.
