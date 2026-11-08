---
title: The generic host and the DI container
questions:
  - id: host-provides
    prompt: Which services does the .NET generic host provide out of the box?
    type: multiple
    options:
      - text: Dependency injection
        correct: true
      - text: Configuration from files, environment variables and the command line
        correct: true
      - text: Logging
        correct: true
      - text: Application lifetime management with graceful shutdown
        correct: true
    explanation: "`Host.CreateApplicationBuilder` and `WebApplication.CreateBuilder` wire up DI, configuration, logging and lifetime for you."
  - id: background
    prompt: Which base class is the usual way to implement a long-running background worker?
    options:
      - text: "`Thread`"
      - text: "`Timer`"
      - text: "`ControllerBase`"
      - id: background-service
        text: "`BackgroundService`"
        correct: true
    explanation: "`BackgroundService` implements `IHostedService`; you override `ExecuteAsync` and honour the `stoppingToken`."
  - id: keyed
    prompt: "How do you inject a keyed service registered with `AddKeyedSingleton<ICache, RedisCache>(\"distributed\")`?"
    options:
      - id: attribute
        text: "Annotate the constructor parameter with `[FromKeyedServices(\"distributed\")]`"
        correct: true
      - text: "Inject `IServiceProvider` and call `GetService(\"distributed\")`"
      - text: Keyed services cannot be injected into constructors
    explanation: Keyed services (.NET 8+) let several implementations of one interface coexist and be selected by key.
  - id: scope-in-singleton
    prompt: A `BackgroundService` (a singleton) needs a scoped `DbContext`. What is the correct approach?
    options:
      - text: Inject the `DbContext` directly into the constructor
      - id: scope-factory
        text: Inject `IServiceScopeFactory`, create a scope per unit of work, and resolve the `DbContext` from it
        correct: true
      - text: Register the `DbContext` as a singleton
    explanation: Creating a scope per unit of work gives each iteration a fresh `DbContext` and disposes it afterwards.
---
## Learning objectives

- Build console and worker apps on the generic host
- Register services with the right lifetime
- Use keyed services and scopes correctly

## The host

```csharp
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHostedService<CleanupWorker>();
await builder.Build().RunAsync();
```

`WebApplication.CreateBuilder` is the same idea with the web server added. Both give you DI,
configuration, logging and graceful shutdown on Ctrl+C or SIGTERM.

## A background worker

```csharp
public sealed partial class CleanupWorker(
    IServiceScopeFactory scopes,
    TimeProvider time,
    ILogger<CleanupWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(5), time);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var scope = scopes.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
            var removed = await db.Carts
                .Where(c => c.UpdatedAt < time.GetUtcNow().AddDays(-30))
                .ExecuteDeleteAsync(stoppingToken);
            LogRemoved(removed);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Removed {Count} abandoned carts")]
    private partial void LogRemoved(int count);
}
```

## Registration patterns

```csharp
services.AddScoped<IOrderRepository, SqlOrderRepository>();
services.AddSingleton<IShippingMethod, StandardShipping>();
services.AddSingleton<IShippingMethod, ExpressShipping>();      // IEnumerable<IShippingMethod> gets both
services.TryAddSingleton<IClock, SystemClock>();                // only if not already registered
services.AddKeyedSingleton<ICache, MemoryCache>("local");
services.AddKeyedSingleton<ICache, RedisCache>("distributed");
```

```csharp
public sealed class CatalogService([FromKeyedServices("distributed")] ICache cache) { }
```

## Validation

In the Development environment, ASP.NET Core enables `ValidateScopes` and `ValidateOnBuild`, so
captive dependencies and missing registrations fail at startup. You can enable them for any host:

```csharp
builder.ConfigureContainer(new DefaultServiceProviderFactory(new ServiceProviderOptions
{
    ValidateScopes = true,
    ValidateOnBuild = true
}));
```
