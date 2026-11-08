---
title: Streams, channels and parallelism
questions:
  - id: async-enum
    prompt: "What does `IAsyncEnumerable<T>` combined with `await foreach` enable?"
    options:
      - text: Running every iteration on a different CPU core
      - id: stream
        text: Consuming items as they become available asynchronously, without loading them all first
        correct: true
      - text: Sorting large collections in parallel
    explanation: Async streams combine `yield return` with `await`, ideal for paging APIs or reading rows one by one.
  - id: channel
    prompt: What is `System.Threading.Channels` primarily used for?
    options:
      - id: producer-consumer
        text: In-process producer/consumer queues with async reads and writes and optional back-pressure
        correct: true
      - text: Network sockets
      - text: Inter-process communication between two machines
    explanation: Bounded channels make producers wait when consumers fall behind, which protects memory.
  - id: parallel
    prompt: Which statements about parallelism in .NET are true?
    type: multiple
    options:
      - text: "`Parallel.ForEachAsync` processes items concurrently with a configurable degree of parallelism"
        correct: true
      - text: CPU-bound work benefits from parallelism; I/O-bound work benefits from asynchrony
        correct: true
      - text: "`System.Threading.Lock` (C# 13 / .NET 9) can be used with the `lock` statement"
        correct: true
      - text: Shared mutable state needs no synchronization when using `Parallel.For`
    explanation: Parallel loops run bodies on multiple threads, so shared state still needs synchronization or thread-safe collections.
---
## Learning objectives

- Produce and consume async streams
- Build producer/consumer pipelines with channels
- Choose between async and parallel processing

## Async streams

```csharp
public async IAsyncEnumerable<Customer> AllCustomersAsync([EnumeratorCancellation] CancellationToken ct = default)
{
    string? continuation = null;
    do
    {
        var page = await _api.GetCustomersAsync(continuation, ct);
        foreach (var customer in page.Items) yield return customer;
        continuation = page.Next;
    } while (continuation is not null);
}

await foreach (var customer in client.AllCustomersAsync(ct))
{
    Console.WriteLine(customer.Name);
}
```

In .NET 10, the LINQ operators for `IAsyncEnumerable<T>` (`System.Linq.AsyncEnumerable`) are
part of the base libraries, so `WhereAwait`-style helpers from third-party packages are no longer
needed.

## Channels

```csharp
var channel = Channel.CreateBounded<Order>(new BoundedChannelOptions(100)
{
    FullMode = BoundedChannelFullMode.Wait
});

var producer = Task.Run(async () =>
{
    await foreach (var order in ReadOrdersAsync(ct)) await channel.Writer.WriteAsync(order, ct);
    channel.Writer.Complete();
});

var consumer = Task.Run(async () =>
{
    await foreach (var order in channel.Reader.ReadAllAsync(ct)) await ProcessAsync(order, ct);
});

await Task.WhenAll(producer, consumer);
```

In ASP.NET Core, a channel registered as a singleton plus a `BackgroundService` reader is a
lightweight in-process work queue.

## Parallelism

```csharp
await Parallel.ForEachAsync(imageUrls, new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = ct },
    async (url, token) =>
    {
        var bytes = await http.GetByteArrayAsync(url, token);
        await File.WriteAllBytesAsync(Path.Combine(dir, Path.GetFileName(url)), bytes, token);
    });
```

## Locks

```csharp
private readonly Lock _gate = new();

public void Increment()
{
    lock (_gate) { _count++; }
}
```

`System.Threading.Lock` is a dedicated lock type that the compiler recognises in `lock` statements.
For simple counters, `Interlocked.Increment(ref _count)` is cheaper still.
