---
title: Service registration patterns
questions:
  - id: tryadd
    prompt: "What is the difference between `services.AddSingleton<IClock, SystemClock>()` and `services.TryAddSingleton<IClock, SystemClock>()`?"
    options:
      - text: "`TryAdd*` catches exceptions during construction"
      - text: There is no difference
      - text: "`TryAdd*` registers lazily"
      - id: tryadd-skip
        text: "`TryAdd*` registers only if no registration for `IClock` exists yet, so library defaults do not override an application's choice"
        correct: true
    explanation: Libraries should use `TryAdd*` for defaults. With plain `Add*`, the *last* registration wins for single resolution, while all are returned from `IEnumerable<T>`.
  - id: enumerable
    prompt: "Three classes are registered as `IOrderValidator`. What does a constructor parameter `IEnumerable<IOrderValidator> validators` receive?"
    options:
      - text: Only the last registered validator
      - id: all
        text: All three, in registration order
        correct: true
      - text: An exception, because of ambiguity
      - text: An empty sequence
    explanation: This is the built-in way to implement plug-in style pipelines (validators, rules, handlers) without a third-party container.
  - id: keyed
    prompt: What are keyed services (.NET 8+)?
    options:
      - text: Services that require an API key
      - text: Services encrypted at rest
      - text: A replacement for scopes
      - id: key
        text: Registrations distinguished by a key, resolved with `[FromKeyedServices("key")]` or `GetRequiredKeyedService<T>(key)`
        correct: true
    explanation: Useful when several implementations of the same interface are needed in different places — for example two storage accounts or two payment providers.
  - id: options
    prompt: Which statements about the options pattern are accurate?
    type: multiple
    options:
      - id: ioptions
        text: "`IOptions<T>` is a singleton snapshot read once"
        correct: true
      - id: snapshot
        text: "`IOptionsSnapshot<T>` is scoped and re-reads configuration per scope (request)"
        correct: true
      - id: monitor
        text: "`IOptionsMonitor<T>` is a singleton that exposes `CurrentValue` and change notifications"
        correct: true
      - text: "`IOptions<T>` automatically validates data annotations without any setup"
    explanation: Validation requires `.ValidateDataAnnotations()` (or the source-generated `[OptionsValidator]`) and `.ValidateOnStart()` to fail fast.
  - id: decorator
    prompt: How can you implement a decorator (for example caching around `IProductCatalog`) with the built-in container and no extra libraries?
    options:
      - text: It is impossible
      - text: Use `[Decorator]` attributes
      - id: factory
        text: Register the concrete inner type, then register the interface with a factory that constructs the decorator around it
        correct: true
      - text: Register the interface twice and the container composes them
    explanation: Libraries like Scrutor add `Decorate<,>()` sugar, but a factory registration is three lines and has no dependency.
  - id: open-generics
    prompt: "What does `services.AddScoped(typeof(IRepository<>), typeof(Repository<>))` register?"
    options:
      - text: Only `IRepository<object>`
      - id: open
        text: An open generic registration — `IRepository<Order>` resolves to `Repository<Order>`, and so on for every `T`
        correct: true
      - text: Nothing; generics cannot be registered
      - text: A singleton for every type in the assembly
    explanation: Open generics are supported natively. Whether a generic repository is a good idea is a separate question (Chapter 1).
  - id: addx
    prompt: Why group registrations into extension methods like `services.AddBilling(configuration)`?
    options:
      - text: Extension methods run faster
      - text: It is required by ASP.NET Core
      - text: It enables lazy loading of assemblies
      - id: modules
        text: Each feature or library owns its registrations, keeping `Program.cs` readable and the feature's internals internal
        correct: true
    explanation: This is the convention the whole framework uses (`AddControllers`, `AddHttpClient`, `AddDbContext`). It replaces the need for container "modules".
  - id: assembly-scanning
    prompt: What is a long-term risk of reflection-based assembly scanning for registrations ("register every class ending in Service")?
    options:
      - id: implicit-aot
        text: Registrations become implicit and hard to trace, and reflection scanning conflicts with trimming and Native AOT
        correct: true
      - text: Scanning is always too slow to use
      - text: It registers classes twice by design
      - text: It is not supported on Linux
    explanation: Explicit registrations — or a source generator that emits them — keep the graph visible and AOT-compatible.
---
## Learning objectives

- Package registrations in `AddXyz` extension methods
- Use `TryAdd`, `IEnumerable<T>`, keyed services, open generics and factories
- Bind and validate options

## Feature modules as extension methods

```csharp
public static class BillingServiceCollectionExtensions
{
    public static IServiceCollection AddBilling(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BillingOptions>()
            .Bind(configuration.GetSection(BillingOptions.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<IInvoiceStore, SqlInvoiceStore>();
        services.AddScoped<InvoiceSender>();

        services.AddHttpClient<PaymentGatewayClient>(client =>
                client.BaseAddress = new Uri(configuration["Billing:GatewayUrl"]!))
            .AddStandardResilienceHandler();

        return services;
    }
}
```

```csharp
public sealed class BillingOptions
{
    public const string Section = "Billing";

    [Required, Url] public required string GatewayUrl { get; init; }
    [Range(1, 90)] public int PaymentTermsDays { get; init; } = 30;
}
```

## Many implementations

```csharp
services.AddScoped<IOrderValidator, StockValidator>();
services.AddScoped<IOrderValidator, CreditLimitValidator>();
services.AddScoped<IOrderValidator, SanctionsValidator>();

public sealed class OrderPlacement(IEnumerable<IOrderValidator> validators)
{
    public async Task<IReadOnlyList<string>> ValidateAsync(Order order, CancellationToken ct)
    {
        var errors = new List<string>();
        foreach (var validator in validators)
        {
            errors.AddRange(await validator.ValidateAsync(order, ct));
        }
        return errors;
    }
}
```

## Keyed services

```csharp
services.AddKeyedSingleton<IBlobStore>("invoices", (sp, _) => new AzureBlobStore("invoices"));
services.AddKeyedSingleton<IBlobStore>("exports", (sp, _) => new AzureBlobStore("exports"));

public sealed class InvoiceArchiver([FromKeyedServices("invoices")] IBlobStore store) { }
```

Prefer keyed services over the old "factory that switches on a string" pattern — but do not use
keys as a disguised service locator.

## A decorator with no library

```csharp
services.AddScoped<SqlProductCatalog>();
services.AddScoped<IProductCatalog>(sp => new CachedProductCatalog(
    sp.GetRequiredService<SqlProductCatalog>(),
    sp.GetRequiredService<HybridCache>()));
```

```csharp
public sealed class CachedProductCatalog(IProductCatalog inner, HybridCache cache) : IProductCatalog
{
    public async Task<Product?> GetAsync(string sku, CancellationToken ct) =>
        await cache.GetOrCreateAsync(
            $"product:{sku}",
            async token => await inner.GetAsync(sku, token),
            cancellationToken: ct);
}
```

`HybridCache` (.NET 9) combines in-memory and distributed caching with stampede protection.

## Sources

- Microsoft Learn, "Service registration" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-registration-methods>
- Microsoft Learn, "Options pattern" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/options>
- Microsoft Learn, "Keyed services" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services>
- Microsoft Learn, "HybridCache library" — <https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid>
