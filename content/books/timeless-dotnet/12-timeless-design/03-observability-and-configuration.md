---
title: Observability and configuration
questions:
  - id: structured
    prompt: "Why is `logger.LogInformation(\"Order {OrderId} shipped\", id)` better than `logger.LogInformation($\"Order {id} shipped\")`?"
    options:
      - text: It is shorter
      - text: Interpolated strings cannot be logged
      - text: There is no difference
      - id: structured
        text: The template keeps `OrderId` as a structured property for querying, and avoids formatting the string when the level is disabled
        correct: true
    explanation: Analyzer CA2254 flags non-constant templates. Structured properties let you query "all events for order X" across services.
  - id: loggermessage
    prompt: What does the `[LoggerMessage]` source generator provide?
    options:
      - text: Automatic log shipping to the cloud
      - id: generated
        text: Strongly typed, allocation-efficient logging methods generated at compile time
        correct: true
      - text: A replacement for `ILogger`
      - text: Log encryption
    explanation: It is the high-performance path for hot logging code and documents your events in one place.
  - id: otel
    prompt: Why is OpenTelemetry the timeless choice for telemetry in .NET?
    type: multiple
    options:
      - id: standard
        text: It is a vendor-neutral CNCF standard, so you can change backends without changing instrumentation
        correct: true
      - id: built-in
        text: .NET's `ActivitySource`, `Meter` and `ILogger` map onto it natively; ASP.NET Core, HttpClient and SqlClient emit telemetry already
        correct: true
      - id: cli
        text: Even the .NET 11 CLI moved its own telemetry from Application Insights to OpenTelemetry
        correct: true
      - text: It requires a paid licence
    explanation: Instrument against standards, export to whatever vendor you use this decade.
  - id: three-signals
    prompt: Which are the three primary telemetry signals?
    options:
      - text: Errors, warnings and debug
      - text: CPU, memory and disk
      - id: signals
        text: Traces, metrics and logs
        correct: true
      - text: Requests, responses and headers
    explanation: Traces show the path of one request across services, metrics show aggregates over time, logs show discrete events. They are correlated by trace ids.
  - id: secrets
    prompt: Where should production secrets (connection strings, API keys) come from?
    options:
      - id: vault
        text: A secret store or managed identity (Key Vault, AWS Secrets Manager, environment-injected secrets); user secrets for local development only
        correct: true
      - text: "`appsettings.json` committed to source control"
      - text: Hard-coded constants
      - text: A shared spreadsheet
    explanation: Best of all, avoid secrets entirely with managed identities and token-based authentication to databases and services.
  - id: validate-start
    prompt: What does `.ValidateOnStart()` on an options registration do?
    options:
      - text: Validates options on every request
      - text: Disables validation in production
      - text: Reloads configuration on start
      - id: fail-fast
        text: Validates the options when the host starts, so misconfiguration fails the deployment instead of the first request that needs it
        correct: true
    explanation: Fail fast, fail loudly, fail before traffic arrives.
---
## Learning objectives

- Write structured, efficient logs
- Instrument with OpenTelemetry primitives built into .NET
- Configure safely and fail fast on misconfiguration

## Structured logging

```csharp
public static partial class OrderLog
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Order {OrderId} shipped with {Carrier}")]
    public static partial void Shipped(ILogger logger, Guid orderId, string carrier);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Carrier {Carrier} quote took {ElapsedMs} ms")]
    public static partial void SlowQuote(ILogger logger, string carrier, long elapsedMs);
}

OrderLog.Shipped(logger, order.Id.Value, label.Carrier);
```

## Traces and metrics with built-in types

```csharp
public sealed class CheckoutTelemetry
{
    public static readonly ActivitySource Source = new("Shop.Checkout");
    private static readonly Meter Meter = new("Shop.Checkout");
    public static readonly Counter<long> Orders = Meter.CreateCounter<long>("shop.orders.placed");
    public static readonly Histogram<double> Value = Meter.CreateHistogram<double>("shop.orders.value", "USD");
}

using var activity = CheckoutTelemetry.Source.StartActivity("PlaceOrder");
activity?.SetTag("order.items", cmd.Lines.Count);
CheckoutTelemetry.Orders.Add(1);
CheckoutTelemetry.Value.Record((double)total);
```

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(t => t
        .AddSource("Shop.Checkout")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(m => m
        .AddMeter("Shop.Checkout")
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter());
builder.Logging.AddOpenTelemetry(o => o.AddOtlpExporter());
```

Your code depends only on `System.Diagnostics` and `Microsoft.Extensions.Logging`. The exporter is
configuration.

## Configuration that fails fast

```csharp
builder.Services.AddOptions<ShippingOptions>()
    .Bind(builder.Configuration.GetSection("Shipping"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

Configuration precedence (later wins): `appsettings.json` → `appsettings.{Environment}.json` →
user secrets (Development) → environment variables → command-line arguments.

## Sources

- Microsoft Learn, "Compile-time logging source generation" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator>
- Microsoft Learn, ".NET observability with OpenTelemetry" — <https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel>
- OpenTelemetry .NET — <https://opentelemetry.io/docs/languages/dotnet/>
- Microsoft Learn, "Options pattern — validation" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/options#options-validation>
- Microsoft Learn, "What's new in .NET 11" (SDK telemetry) — <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-11/overview>
