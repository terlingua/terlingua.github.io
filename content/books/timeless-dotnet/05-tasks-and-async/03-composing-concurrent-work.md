---
title: Composing concurrent work
questions:
  - id: sequential
    prompt: "What is the total time of `var a = await GetAAsync(); var b = await GetBAsync();` if each call takes 1 second and they are independent?"
    options:
      - id: two
        text: About 2 seconds — the second call does not start until the first finishes
        correct: true
      - text: About 1 second
      - text: About 0.5 seconds
      - text: It depends on the number of CPU cores
    explanation: Await-in-sequence is correct when B depends on A. When they are independent, start both, then await both.
  - id: whenall-exceptions
    prompt: "If two tasks in `await Task.WhenAll(t1, t2)` both fault, what does the `await` throw?"
    options:
      - id: first
        text: The first exception only; all exceptions remain available on the `WhenAll` task's `Exception.InnerExceptions`
        correct: true
      - text: An `AggregateException` containing both
      - text: Nothing; `WhenAll` swallows exceptions
      - text: The last exception only
    explanation: "`await` unwraps to the first exception for convenience. Keep a reference to the `WhenAll` task if you need to log every failure."
  - id: unbounded
    prompt: "What is wrong with `await Task.WhenAll(customerIds.Select(id => api.GetAsync(id, ct)));` for 50,000 ids?"
    options:
      - text: Nothing, `WhenAll` limits concurrency automatically
      - id: unbounded
        text: It starts 50,000 requests at once, which can exhaust sockets, trip rate limits or overload the downstream service
        correct: true
      - text: It runs them one at a time
      - text: "`Select` cannot be used with async lambdas"
    explanation: "`WhenAll` has no concurrency limit. Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`, a `SemaphoreSlim`, or a `Channel` pipeline."
  - id: foreachasync
    prompt: What does `Parallel.ForEachAsync(items, new ParallelOptions { MaxDegreeOfParallelism = 8, CancellationToken = ct }, async (item, token) => ...)` provide?
    type: multiple
    options:
      - id: bounded
        text: At most 8 bodies running concurrently
        correct: true
      - id: cancel
        text: Cancellation that stops scheduling new items and is passed to each body as `token`
        correct: true
      - id: async-body
        text: Support for async bodies without blocking threads
        correct: true
      - text: Preservation of the input order in the side effects
    explanation: It is the simplest correct tool for "process this list with bounded concurrency" since .NET 6. Order of completion is not guaranteed.
  - id: wheneach
    prompt: What does `Task.WhenEach` (.NET 9) do?
    options:
      - id: as-completed
        text: Returns an `IAsyncEnumerable<Task<T>>` that yields tasks in the order they complete
        correct: true
      - text: Runs tasks one after another
      - text: Waits for each task synchronously
      - text: Cancels all but the fastest task
    explanation: It replaces the older loop of `WhenAny` + `Remove`, which was O(n²).
  - id: channels
    prompt: When is `System.Threading.Channels` a better fit than `Parallel.ForEachAsync`?
    options:
      - text: Never; channels are deprecated
      - text: For sorting a list in memory
      - id: producer-consumer
        text: For long-running producer/consumer pipelines where items arrive over time and you want back-pressure between stages
        correct: true
      - text: For a single HTTP call
    explanation: A bounded channel blocks (asynchronously) producers when consumers fall behind, which protects memory.
  - id: semaphore
    prompt: Why use `SemaphoreSlim` instead of `lock` to protect a section that contains an `await`?
    options:
      - text: "`lock` is slower"
      - text: "`SemaphoreSlim` is a reference type"
      - text: There is no difference
      - id: no-await-in-lock
        text: The compiler forbids `await` inside a `lock` block, because the lock is thread-affine and the continuation may run on a different thread
        correct: true
    explanation: "`await semaphore.WaitAsync(ct)` with `try`/`finally { semaphore.Release(); }` is the standard async mutual exclusion pattern."
  - id: iasyncenumerable
    prompt: What does `IAsyncEnumerable<T>` with `await foreach` enable?
    options:
      - text: Parallel iteration over all items at once
      - text: Synchronous blocking iteration
      - text: Automatic caching of results
      - id: streaming
        text: Streaming items one at a time as they become available, with asynchronous waits between them
        correct: true
    explanation: Ideal for paging through APIs or streaming database rows without buffering everything in memory. Use `[EnumeratorCancellation]` to flow a token into an async iterator.
---
## Learning objectives

- Start independent operations concurrently and await them together
- Bound concurrency correctly
- Choose between `WhenAll`, `WhenAny`, `WhenEach`, `Parallel.ForEachAsync` and channels

## Sequential versus concurrent

```csharp
var customer = await customers.GetAsync(id, ct);
var orderList = await orderStore.ListForAsync(id, ct);
```

```csharp
var customerTask = customers.GetAsync(id, ct);
var ordersTask = orderStore.ListForAsync(id, ct);
await Task.WhenAll(customerTask, ordersTask);
var customer = await customerTask;
var orderList = await ordersTask;
```

Awaiting an already completed task is free and avoids `.Result` (which is safe here but trains
bad habits).

**Caution with shared resources:** an EF Core `DbContext` or a single `DbConnection` cannot run two
commands concurrently. Concurrency across one connection needs separate connections.

## Bounded concurrency

```csharp
await Parallel.ForEachAsync(
    invoiceIds,
    new ParallelOptions { MaxDegreeOfParallelism = 8, CancellationToken = ct },
    async (id, token) =>
    {
        var pdf = await renderer.RenderAsync(id, token);
        await storage.UploadAsync($"invoices/{id}.pdf", pdf, token);
    });
```

The SemaphoreSlim equivalent, useful when you also need the results:

```csharp
using var gate = new SemaphoreSlim(8);
var tasks = invoiceIds.Select(async id =>
{
    await gate.WaitAsync(ct);
    try
    {
        return await renderer.RenderAsync(id, ct);
    }
    finally
    {
        gate.Release();
    }
});
byte[][] pdfs = await Task.WhenAll(tasks);
```

## As they complete

```csharp
var lookups = providers.Select(p => p.QuoteAsync(shipment, ct)).ToList();
await foreach (var finished in Task.WhenEach(lookups))
{
    var quote = await finished;
    progress.Report(quote);
}
```

## First one wins, with cleanup

```csharp
using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
var primary = primaryDns.ResolveAsync(host, cts.Token);
var secondary = secondaryDns.ResolveAsync(host, cts.Token);
var winner = await Task.WhenAny(primary, secondary);
await cts.CancelAsync();
return await winner;
```

Always cancel the losers; otherwise they keep consuming resources after you have returned.

## Channels for pipelines

```csharp
var channel = Channel.CreateBounded<OrderEvent>(new BoundedChannelOptions(1_000)
{
    FullMode = BoundedChannelFullMode.Wait,
    SingleReader = true,
});

async Task ProduceAsync(CancellationToken ct)
{
    await foreach (var evt in broker.SubscribeAsync(ct))
    {
        await channel.Writer.WriteAsync(evt, ct);
    }
    channel.Writer.Complete();
}

async Task ConsumeAsync(CancellationToken ct)
{
    await foreach (var evt in channel.Reader.ReadAllAsync(ct))
    {
        await projector.ApplyAsync(evt, ct);
    }
}
```

## Streaming with IAsyncEnumerable

```csharp
public async IAsyncEnumerable<Product> GetAllAsync([EnumeratorCancellation] CancellationToken ct = default)
{
    string? cursor = null;
    do
    {
        var page = await api.GetProductsAsync(cursor, ct);
        foreach (var product in page.Items) yield return product;
        cursor = page.NextCursor;
    } while (cursor is not null);
}
```

## Real-world scenario: the nightly export that took down a partner

A nightly job called a partner's API for 120,000 shipments with `Task.WhenAll`. On day one it
worked — the partner's sandbox was small. In production, the burst triggered the partner's DDoS
protection and blocked the company's IP for a day. `Parallel.ForEachAsync` with a degree of 10,
plus a resilience pipeline that honored `Retry-After`, made the job polite and only 4 minutes
slower.

## Sources

- Microsoft Learn, `Parallel.ForEachAsync` — <https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.parallel.foreachasync>
- Microsoft Learn, `Task.WhenEach` — <https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.wheneach>
- Microsoft Learn, "System.Threading.Channels" — <https://learn.microsoft.com/en-us/dotnet/core/extensions/channels>
- Microsoft Learn, "Generate and consume async streams" — <https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/generate-consume-asynchronous-stream>
