---
title: Middleware, HttpClient and resilience
questions:
  - id: order
    prompt: In which order should authentication and authorization middleware be added?
    options:
      - id: authn-first
        text: "`UseAuthentication()` before `UseAuthorization()`"
        correct: true
      - text: "`UseAuthorization()` before `UseAuthentication()`"
      - text: Order does not matter
    explanation: Middleware runs in registration order. Authorization needs the user identity that authentication establishes.
  - id: new-httpclient
    prompt: Why is creating `new HttpClient()` for every request a problem in long-running apps?
    options:
      - text: "`HttpClient` is not thread-safe"
      - text: It cannot send JSON
      - id: sockets
        text: It can exhaust sockets, and a single long-lived instance without care can miss DNS changes
        correct: true
    explanation: "`IHttpClientFactory` (or a shared `SocketsHttpHandler` with `PooledConnectionLifetime`) manages connection pooling and lifetime correctly."
  - id: standard-resilience
    prompt: What does `AddStandardResilienceHandler()` from `Microsoft.Extensions.Http.Resilience` add to an `HttpClient`?
    type: multiple
    options:
      - text: Retries with exponential backoff
        correct: true
      - text: A circuit breaker
        correct: true
      - text: Per-attempt and total request timeouts
        correct: true
      - text: Automatic caching of every response for 24 hours
    explanation: The standard pipeline (built on Polly) combines a rate limiter, total timeout, retry, circuit breaker and attempt timeout.
---
## Learning objectives

- Order middleware correctly and write custom middleware
- Use `IHttpClientFactory` with typed clients
- Add resilience with the standard resilience handler

## The pipeline

```csharp
app.UseExceptionHandler();
app.UseHsts();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapProductEndpoints();
```

Each middleware can act before and after the next one. Order matters: exception handling first so
it wraps everything; authentication before authorization.

## Custom middleware

```csharp
public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string Header = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var id = context.Request.Headers[Header].FirstOrDefault() ?? Guid.CreateVersion7().ToString();
        context.Response.Headers[Header] = id;
        using (context.RequestServices.GetRequiredService<ILogger<CorrelationIdMiddleware>>()
                   .BeginScope(new Dictionary<string, object> { ["CorrelationId"] = id }))
        {
            await next(context);
        }
    }
}

app.UseMiddleware<CorrelationIdMiddleware>();
```

`Guid.CreateVersion7()` (.NET 9) creates time-ordered GUIDs that index well in databases.

## Typed HTTP clients

```csharp
public sealed class GitHubClient(HttpClient http)
{
    public Task<Repo?> GetRepoAsync(string owner, string name, CancellationToken ct) =>
        http.GetFromJsonAsync<Repo>($"repos/{owner}/{name}", ct);
}

builder.Services.AddHttpClient<GitHubClient>(c =>
{
    c.BaseAddress = new Uri("https://api.github.com/");
    c.DefaultRequestHeaders.UserAgent.ParseAdd("shop-api/1.0");
})
.AddStandardResilienceHandler();
```

## Resilience

Networks fail. The standard handler adds, from outermost to innermost:

1. Rate limiter
2. Total request timeout
3. Retry with exponential backoff and jitter
4. Circuit breaker
5. Per-attempt timeout

Retries are only safe for **idempotent** operations; for POSTs, use idempotency keys or disable
retries for that client.

## Caching

`HybridCache` (`Microsoft.Extensions.Caching.Hybrid`) combines in-memory and distributed caching
with stampede protection:

```csharp
var product = await cache.GetOrCreateAsync($"product:{id}",
    async token => await store.FindAsync(id, token), cancellationToken: ct);
```
