---
title: What a Task is (and is not)
questions:
  - id: task-not-thread
    prompt: Which statement best describes a `Task`?
    options:
      - id: promise
        text: An object representing an operation that will complete in the future, with a result or an exception
        correct: true
      - text: A thread with a nicer API
      - text: A unit of work that always runs on the thread pool
      - text: A synonym for `Thread.Start`
    explanation: A task is a promise of a result. Some tasks represent CPU work queued to the thread pool; most in server code represent I/O, during which *no thread* is running for that operation.
  - id: hot
    prompt: "When does the HTTP request start in `var t = http.GetStringAsync(url);`?"
    options:
      - text: When `await t` is executed
      - text: When `t.Start()` is called
      - text: When the garbage collector runs
      - id: immediately
        text: Immediately — tasks returned from async methods are "hot"; awaiting only observes completion
        correct: true
    explanation: This is why starting several operations and then awaiting them gives concurrency. Calling `Start()` on a task from an async method throws.
  - id: no-thread
    prompt: Stephen Cleary's well-known article is titled "There Is No Thread". What does it explain?
    options:
      - id: io-no-thread
        text: That during a true asynchronous I/O operation no thread is blocked waiting for it; the OS signals completion
        correct: true
      - text: That .NET does not use threads
      - text: That `Task.Run` never uses a thread
      - text: That async methods run on the UI thread only
    explanation: The device driver and OS complete the I/O; a thread-pool thread is borrowed only briefly to run the continuation.
  - id: taskcompletionsource
    prompt: What is `TaskCompletionSource<T>` for?
    options:
      - text: Running CPU work in parallel
      - id: bridge
        text: Creating a task whose completion you control manually — bridging callback- or event-based APIs into `await`
        correct: true
      - text: Cancelling tasks
      - text: Logging task exceptions
    explanation: Construct it with `TaskCreationOptions.RunContinuationsAsynchronously` to avoid running awaiters' continuations inline on the thread that calls `SetResult`.
  - id: valuetask-rules
    prompt: Which are rules for consuming a `ValueTask<T>`?
    type: multiple
    options:
      - id: once
        text: Await it at most once
        correct: true
      - id: no-concurrent
        text: Do not await it concurrently from multiple places
        correct: true
      - id: astask
        text: If you need to do either of those, call `.AsTask()` once and use the resulting `Task`
        correct: true
      - text: Always prefer `ValueTask` over `Task` for every public API
    explanation: "`ValueTask` can be backed by a pooled, reusable object. Using it twice may observe another operation's result. Default to `Task`; use `ValueTask` for hot paths that frequently complete synchronously."
  - id: completed-task
    prompt: A method implementing an async interface has nothing asynchronous to do. What should it return?
    options:
      - text: "`Task.Run(() => result)`"
      - text: "`new Task<T>(() => result)`"
      - text: "`null`"
      - id: fromresult
        text: "`Task.FromResult(result)` (or `Task.CompletedTask` for non-generic `Task`)"
        correct: true
    explanation: Returning `null` from a `Task`-returning method makes the caller's `await` throw `NullReferenceException`. `Task.Run` wastes a thread hop.
  - id: exceptions
    prompt: "An async method throws before its first `await`. When does the caller see the exception?"
    options:
      - id: stored
        text: When the returned task is awaited — the exception is stored in the task
        correct: true
      - text: Immediately, as a synchronous throw from the call
      - text: Never; it is swallowed
      - text: On the finalizer thread
    explanation: Everything inside an `async` method, including argument validation, is captured in the task. If you need eager validation, validate in a non-async wrapper that then calls a private async core.
---
## Learning objectives

- Describe a task as a promise, not a thread
- Distinguish I/O-bound and CPU-bound work
- Use `Task.FromResult`, `TaskCompletionSource` and `ValueTask` correctly

## Promises, not threads

```csharp
Task<string> pending = http.GetStringAsync("https://example.org/rates");
DoOtherWork();
string rates = await pending;
```

From the moment `GetStringAsync` returns, the request is in flight. While the network does its
work, no thread in your process is dedicated to it. `await` subscribes a continuation and returns
control to the caller.

## Two kinds of work

| | I/O-bound | CPU-bound |
|---|---|---|
| Examples | HTTP, database, file, queue | Image resize, compression, hashing large data |
| Uses a thread while waiting? | No | Yes, the whole time |
| Right tool | Async APIs + `await` | Synchronous code; `Task.Run` or `Parallel` to offload where a thread must stay free |
| Scales with | Concurrency (thousands in flight) | Cores |

Server code is overwhelmingly I/O-bound. That is why async matters so much there: a thread pool of
a few dozen threads can service thousands of concurrent requests that are mostly waiting.

## Bridging old APIs with TaskCompletionSource

```csharp
public static Task<ScanResult> ScanAsync(this BarcodeScanner scanner, CancellationToken ct)
{
    var tcs = new TaskCompletionSource<ScanResult>(TaskCreationOptions.RunContinuationsAsynchronously);

    void OnScanned(object? sender, ScanResult result)
    {
        scanner.Scanned -= OnScanned;
        tcs.TrySetResult(result);
    }

    scanner.Scanned += OnScanned;
    var registration = ct.Register(() =>
    {
        scanner.Scanned -= OnScanned;
        tcs.TrySetCanceled(ct);
    });
    _ = tcs.Task.ContinueWith(_ => registration.Dispose(), TaskScheduler.Default);

    scanner.Start();
    return tcs.Task;
}
```

## Eager validation with an async core

```csharp
public Task<Invoice> GetInvoiceAsync(InvoiceId id, CancellationToken ct)
{
    ArgumentOutOfRangeException.ThrowIfEqual(id.Value, Guid.Empty);
    return GetInvoiceCoreAsync(id, ct);
}

private async Task<Invoice> GetInvoiceCoreAsync(InvoiceId id, CancellationToken ct)
{
    await using var connection = await dataSource.OpenConnectionAsync(ct);
    return await connection.QuerySingleAsync<Invoice>(
        new CommandDefinition("select * from Invoices where Id = @Value", id, cancellationToken: ct));
}
```

Most application code does not need this split; it is a library-quality nicety.

## ValueTask: a measured optimization

```csharp
public ValueTask<Rate> GetRateAsync(string currency, CancellationToken ct)
{
    if (_cache.TryGetValue(currency, out var rate))
    {
        return ValueTask.FromResult(rate);
    }
    return new ValueTask<Rate>(LoadRateAsync(currency, ct));
}
```

When the cache hit rate is 99%, this avoids a `Task<Rate>` allocation on almost every call.

## Sources

- Stephen Cleary, "There Is No Thread" — <https://blog.stephencleary.com/2013/11/there-is-no-thread.html>
- Stephen Toub, "How Async/Await Really Works in C#" — <https://devblogs.microsoft.com/dotnet/how-async-await-really-works/>
- Stephen Toub, "Understanding the Whys, Whats, and Whens of ValueTask" — <https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/>
- Microsoft Learn, "Asynchronous programming" — <https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/>
