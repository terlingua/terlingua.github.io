---
title: Async mistakes that still ship to production
questions:
  - id: async-void-crash
    prompt: What happens when an exception escapes an `async void` method in an ASP.NET Core or worker service process?
    options:
      - text: It is logged and ignored
      - text: It is stored in a task nobody awaits
      - id: crash
        text: It is rethrown on the thread pool with no caller to catch it, which typically terminates the process
        correct: true
      - text: The method retries automatically
    explanation: "`async void` has no task to carry the exception. Event handlers are the only legitimate use, and they should catch their own exceptions."
  - id: fire-forget
    prompt: An endpoint does `_ = emailSender.SendWelcomeAsync(user);` and returns immediately. Which problems can this cause?
    type: multiple
    options:
      - id: lost-errors
        text: Exceptions are silently lost
        correct: true
      - id: disposed-scope
        text: Scoped services (such as a `DbContext`) used by the background work may be disposed when the request ends
        correct: true
      - id: shutdown
        text: Work in flight is lost when the process restarts or scales in
        correct: true
      - text: The response is delayed until the email is sent
    explanation: Use a queue (a `Channel` consumed by a `BackgroundService` for in-process work, or a durable broker for work that must survive restarts).
  - id: sync-over-async
    prompt: Why is `.GetAwaiter().GetResult()` in an ASP.NET Core request path harmful even though there is no synchronization context?
    options:
      - text: It always deadlocks
      - id: starvation
        text: It blocks a thread-pool thread while waiting; under load this causes thread-pool starvation and latency collapse
        correct: true
      - text: It does not compile
      - text: It changes the exception type
    explanation: The thread pool grows slowly. When many requests block, new work queues behind them and the service stalls — a classic production incident pattern.
  - id: elide
    prompt: "When is it safe to drop `async`/`await` and return the task directly, as in `Task<User> GetAsync(Guid id) => _inner.GetAsync(id);`?"
    options:
      - text: Always, it is strictly better
      - id: simple-passthrough
        text: In a simple pass-through with no `using`, `try`/`catch`, or code after the call
        correct: true
      - text: Never, it does not compile
      - text: Only inside `using` blocks
    explanation: Inside a `using`, returning the task without awaiting disposes the resource before the operation finishes. Exceptions also surface differently. When in doubt, keep `async`/`await` — the overhead is small.
  - id: lazy-async-init
    prompt: Why should constructors not start asynchronous work (for example `_ready = LoadAsync();`)?
    options:
      - text: Constructors run on a separate thread
      - text: The compiler forbids it
      - text: Tasks cannot be stored in fields
      - id: unobserved
        text: Exceptions become unobserved or surface at surprising times, and DI resolution cannot await; use an explicit `InitializeAsync`, a factory, or a hosted service
        correct: true
    explanation: Construction should be cheap and side-effect free. Asynchronous start-up belongs in `IHostedService.StartAsync` or in lazily awaited initialization.
  - id: asynclocal
    prompt: What is `AsyncLocal<T>` used for?
    options:
      - text: Storing data per thread
      - text: Making fields thread-safe
      - id: flows
        text: Storing ambient data that flows with the logical async call path, such as a correlation id or `Activity.Current`
        correct: true
      - text: Caching tasks
    explanation: "`ThreadStatic` breaks across awaits because continuations can run on different threads. `AsyncLocal` flows with the `ExecutionContext`. Use sparingly — ambient state is hidden coupling."
  - id: backgroundservice
    prompt: In a `BackgroundService`, what should `ExecuteAsync` do with its `stoppingToken`?
    options:
      - text: Ignore it
      - text: Call `Environment.Exit` when it fires
      - text: Store it in a static field
      - id: pass-it
        text: Pass it to every async call in the loop and exit the loop when it is cancelled
        correct: true
    explanation: The host signals shutdown through this token and waits (by default 30 seconds since .NET 6) for services to stop.
---
## Learning objectives

- Recognize `async void`, fire-and-forget and sync-over-async in code review
- Move background work to a proper queue and hosted service
- Know when eliding `async` is safe

## A gallery of bugs

### async void

```csharp
public async void OnOrderPlaced(object? sender, OrderPlaced e)
{
    await _billing.ChargeAsync(e.OrderId);
}
```

If `ChargeAsync` throws, the process may terminate. If you must use an event handler, catch:

```csharp
public async void OnOrderPlaced(object? sender, OrderPlaced e)
{
    try
    {
        await _billing.ChargeAsync(e.OrderId, CancellationToken.None);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Charging order {OrderId} failed", e.OrderId);
    }
}
```

### Fire-and-forget from a request

```csharp
app.MapPost("/users", async (CreateUser cmd, AppDb db, IEmailSender email) =>
{
    var user = await db.CreateAsync(cmd);
    _ = email.SendWelcomeAsync(user);
    return Results.Created($"/users/{user.Id}", user);
});
```

### The fix: a channel-backed queue and a hosted service

```csharp
public sealed class EmailQueue
{
    private readonly Channel<WelcomeEmail> _channel = Channel.CreateBounded<WelcomeEmail>(10_000);
    public ValueTask EnqueueAsync(WelcomeEmail email, CancellationToken ct) => _channel.Writer.WriteAsync(email, ct);
    public IAsyncEnumerable<WelcomeEmail> ReadAllAsync(CancellationToken ct) => _channel.Reader.ReadAllAsync(ct);
}

public sealed class EmailWorker(EmailQueue queue, IServiceScopeFactory scopes, ILogger<EmailWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var email in queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                await using var scope = scopes.CreateAsyncScope();
                var sender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                await sender.SendAsync(email, stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Welcome email to {UserId} failed", email.UserId);
            }
        }
    }
}

builder.Services.AddSingleton<EmailQueue>();
builder.Services.AddHostedService<EmailWorker>();
```

For work that must survive a restart (payments, compliance emails), use a durable queue — Azure
Service Bus, Amazon SQS, RabbitMQ — or the transactional outbox pattern.

### Sync over async

```csharp
public IActionResult Get(int id)
{
    var product = _catalog.GetAsync(id).GetAwaiter().GetResult();
    return Ok(product);
}
```

Make the action `async Task<IActionResult>` and `await`. Every framework entry point in modern
.NET — controllers, minimal APIs, hosted services, `Main`, xUnit tests — accepts async.

### Returning a task from inside a using

```csharp
public Task<string> ReadAsync(string path)
{
    using var reader = new StreamReader(path);
    return reader.ReadToEndAsync();
}
```

The reader is disposed while the read is still in flight. Add `async` and `await`.

## A checklist for code review

- [ ] No `async void` except event handlers that catch everything
- [ ] No `.Result`, `.Wait()`, `.GetAwaiter().GetResult()` on incomplete tasks
- [ ] No discarded tasks (`_ =`) without a deliberate, logged, owned lifetime
- [ ] Every async method accepts and passes a `CancellationToken` where the caller can cancel
- [ ] No `Task.Run` around I/O in server code
- [ ] Unbounded `WhenAll` over user-controlled sizes has a concurrency limit

## Sources

- David Fowler, "Async Guidance" — <https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md>
- Microsoft Learn, "Background tasks with hosted services" — <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services>
- Stephen Cleary, "Eliding Async and Await" — <https://blog.stephencleary.com/2016/12/eliding-async-await.html>
- Microsoft Learn, "Debug thread-pool starvation" — <https://learn.microsoft.com/en-us/dotnet/core/diagnostics/debug-threadpool-starvation>
