---
title: Principles and lifetimes
questions:
  - id: what-di
    prompt: What is the core idea of dependency injection?
    options:
      - text: Using a container library
      - id: receive
        text: An object receives the collaborators it needs (usually through its constructor) instead of creating or finding them itself
        correct: true
      - text: Making every class implement an interface
      - text: Loading assemblies at runtime
    explanation: DI is a design technique; a container is just one way to automate the wiring. "Pure DI" with `new` in a composition root is also DI.
  - id: lifetimes
    prompt: Match the default lifetime semantics in Microsoft.Extensions.DependencyInjection. Which are correct?
    type: multiple
    options:
      - id: transient
        text: Transient — a new instance every time it is resolved
        correct: true
      - id: scoped
        text: Scoped — one instance per scope; ASP.NET Core creates one scope per HTTP request
        correct: true
      - id: singleton
        text: Singleton — one instance for the lifetime of the root provider
        correct: true
      - text: Scoped — one instance per thread
    explanation: Scopes are explicit objects, not threads. Outside ASP.NET Core you create them with `IServiceScopeFactory.CreateAsyncScope()`.
  - id: captive
    prompt: A singleton `PriceCache` takes `AppDbContext` (registered as scoped) in its constructor. What is this called and why is it bad?
    options:
      - id: captive
        text: A captive dependency — the scoped `DbContext` is captured for the singleton's whole lifetime, shared across requests and threads, and never disposed per request
        correct: true
      - text: Constructor over-injection; it is merely verbose
      - text: A circular dependency; it fails to compile
      - text: Nothing wrong; DI handles it
    explanation: "`DbContext` is not thread-safe. In Development, `ValidateScopes` throws at resolve time; inject `IServiceScopeFactory` (or `IDbContextFactory<T>`) into the singleton instead."
  - id: validate-on-build
    prompt: What do `ValidateScopes` and `ValidateOnBuild` do?
    type: multiple
    options:
      - id: scopes
        text: "`ValidateScopes` throws when a scoped service is resolved from the root provider or captured by a singleton"
        correct: true
      - id: build
        text: "`ValidateOnBuild` tries to construct every registration's dependency graph at startup, surfacing missing registrations immediately"
        correct: true
      - id: dev-default
        text: Both are enabled by default in the Development environment for apps built with the generic host
        correct: true
      - text: They are only available in Autofac
    explanation: Enable them in all environments if startup cost is acceptable — failing at startup beats failing at 2 a.m. on the first request to a rare endpoint.
  - id: disposal
    prompt: Who disposes a transient `IDisposable` service resolved from a request scope?
    options:
      - text: The garbage collector, eventually
      - id: scope
        text: The scope that created it, when the scope is disposed
        correct: true
      - text: Nobody; you must dispose it manually
      - text: The singleton that consumed it
    explanation: "Containers track disposables they create. Beware: transient disposables resolved from the *root* provider are held until the application shuts down — a slow memory leak."
  - id: composition-root
    prompt: Where should service registrations live?
    options:
      - text: In every class's static constructor
      - id: root
        text: In the composition root — `Program.cs` and `AddXyz` extension methods it calls — close to the application's entry point
        correct: true
      - text: In a global static `ServiceLocator`
      - text: In `appsettings.json`
    explanation: One place assembles the graph. Everywhere else just declares what it needs.
  - id: httpclient-lifetime
    prompt: Why is injecting a typed `HttpClient` into a singleton a classic captive-dependency bug?
    options:
      - id: handler-rotation
        text: The typed client is transient, and capturing it forever defeats `IHttpClientFactory`'s handler rotation, so DNS changes are never picked up
        correct: true
      - text: Typed clients are not thread-safe
      - text: Singletons cannot use HTTP
      - text: It causes a compile error
    explanation: Consume typed clients from transient or scoped services, or use a long-lived client with `SocketsHttpHandler.PooledConnectionLifetime` set.
---
## Learning objectives

- Explain DI as a design technique independent of containers
- Choose lifetimes and avoid captive dependencies
- Turn on validation so mistakes fail at startup

## DI without a container

```csharp
var time = TimeProvider.System;
var dataSource = SqlClientFactory.Instance.CreateDataSource(connectionString);
var invoices = new SqlInvoiceStore(dataSource);
var sender = new InvoiceSender(invoices, new SmtpEmailGateway(smtpOptions), time);
await sender.SendDueInvoicesAsync(CancellationToken.None);
```

This is dependency injection. A container automates the same wiring for large graphs and gives
lifetimes and disposal for free.

## The same with the generic host

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<DbDataSource>(_ =>
    SqlClientFactory.Instance.CreateDataSource(builder.Configuration.GetConnectionString("Billing")!));
builder.Services.AddScoped<IInvoiceStore, SqlInvoiceStore>();
builder.Services.AddScoped<InvoiceSender>();
builder.Services.AddHostedService<InvoiceWorker>();

using var host = builder.Build();
await host.RunAsync();
```

## Choosing a lifetime

| Lifetime | Use for | Avoid for |
|---|---|---|
| Singleton | Stateless services, thread-safe caches, `TimeProvider`, `DbDataSource`, configuration objects | Anything holding per-request state or non-thread-safe resources |
| Scoped | Unit-of-work objects (`DbContext`), per-request context (current user) | Things needed from singletons |
| Transient | Lightweight, stateless helpers | Expensive-to-create or disposable services resolved from root |

**The rule:** a service may depend only on services with an equal or longer lifetime.

## Consuming scoped services from a singleton

```csharp
public sealed class NightlyRecalculation(IServiceScopeFactory scopes, TimeProvider time) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(24), time);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var scope = scopes.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.RecalculateBalancesAsync(stoppingToken);
        }
    }
}
```

## Validation everywhere

```csharp
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});
```

## Sources

- Microsoft Learn, "Dependency injection in .NET" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection>
- Microsoft Learn, "Dependency injection guidelines" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines>
- Mark Seemann and Steven van Deursen, *Dependency Injection Principles, Practices, and Patterns* (Manning, 2019)
- Microsoft Learn, "IHttpClientFactory with .NET" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory>
