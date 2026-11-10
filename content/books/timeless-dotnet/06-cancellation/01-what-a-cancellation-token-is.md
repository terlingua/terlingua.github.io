---
title: What a cancellation token is
questions:
  - id: cooperative
    prompt: What kind of cancellation model does .NET use?
    options:
      - text: Preemptive — the runtime stops the thread at any point
      - id: cooperative
        text: Cooperative — code observes a token and chooses where to stop
        correct: true
      - text: Signal-based — the OS sends SIGKILL to the task
      - text: Exception-based — the runtime injects an exception anywhere
    explanation: Nothing is forcibly interrupted. Cancellation only happens where code checks the token or passes it to an API that does.
  - id: source-vs-token
    prompt: What is the difference between `CancellationTokenSource` and `CancellationToken`?
    options:
      - text: They are the same type with different names
      - id: roles
        text: The source has the power to cancel; the token can only observe. You hand out tokens and keep the source
        correct: true
      - text: The token can cancel; the source can only observe
      - text: The source is for threads and the token is for tasks
    explanation: Separating the two roles means a callee cannot cancel its caller's other work. It is a capability design.
  - id: struct
    prompt: "`CancellationToken` is a struct. What does `default(CancellationToken)` (also `CancellationToken.None`) represent?"
    options:
      - text: A token that is already cancelled
      - text: An invalid token that throws when used
      - text: The current request's token
      - id: never
        text: A token that can never be cancelled
        correct: true
    explanation: "`CanBeCanceled` is `false` for it. That is why `CancellationToken ct = default` is a safe optional parameter."
  - id: oce
    prompt: Which exception signals that an operation was cancelled?
    options:
      - id: oce
        text: "`OperationCanceledException` (and its subclass `TaskCanceledException`)"
        correct: true
      - text: "`TimeoutException`"
      - text: "`ThreadAbortException`"
      - text: "`ObjectDisposedException`"
    explanation: Catch `OperationCanceledException`, not `TaskCanceledException`, so you handle both. A task that ends with this exception transitions to the *Canceled* state rather than *Faulted*.
  - id: observe-ways
    prompt: Which are legitimate ways for code to observe a token?
    type: multiple
    options:
      - id: pass
        text: Passing it to async APIs that accept a token
        correct: true
      - id: throwif
        text: Calling `ct.ThrowIfCancellationRequested()` between units of work in a loop
        correct: true
      - id: register
        text: "`ct.Register(callback)` to bridge into APIs that have their own cancel mechanism"
        correct: true
      - text: Calling `Thread.Abort()` when `ct.IsCancellationRequested` is true
    explanation: "`Thread.Abort` is not supported on modern .NET and throws `PlatformNotSupportedException`."
  - id: linked
    prompt: What does `CancellationTokenSource.CreateLinkedTokenSource(a, b)` produce?
    options:
      - text: A source that cancels `a` and `b`
      - id: either
        text: A source whose token is cancelled when either `a` or `b` is cancelled, or when it is cancelled itself
        correct: true
      - text: A token that requires both to be cancelled
      - text: A copy of `a`
    explanation: Linking combines "the caller gave up" with "my own timeout" or "the app is shutting down". Dispose linked sources — they register callbacks on their parents.
  - id: dispose-cts
    prompt: Why should you dispose a `CancellationTokenSource` you create?
    options:
      - text: Undisposed sources cancel themselves after 60 seconds
      - id: timers-registrations
        text: It may own a timer (from `CancelAfter`) and linked registrations on parent tokens, which otherwise live longer than needed
        correct: true
      - text: It holds an OS thread
      - text: Disposal is required to read `IsCancellationRequested`
    explanation: A long-lived parent token (like the host's shutdown token) with thousands of undisposed linked children is a real memory leak pattern.
---
## Learning objectives

- Explain the cooperative cancellation model
- Use `CancellationTokenSource`, tokens, linking and registration
- Handle `OperationCanceledException` correctly

## The model in four sentences

1. Someone who might want to stop work owns a **`CancellationTokenSource`**.
2. They hand its **`CancellationToken`** to the work.
3. The work **checks** the token at safe points (or passes it on to APIs that check it).
4. When cancelled, the work stops and signals it by throwing **`OperationCanceledException`**.

```csharp
using var cts = new CancellationTokenSource();
var work = ImportAsync(file, cts.Token);

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

try
{
    await work;
}
catch (OperationCanceledException) when (cts.IsCancellationRequested)
{
    Console.WriteLine("Import cancelled by user.");
}
```

## Checking in CPU-bound loops

```csharp
public static int CountPrimes(int limit, CancellationToken ct)
{
    var count = 0;
    for (var n = 2; n < limit; n++)
    {
        if ((n & 0xFFFF) == 0) ct.ThrowIfCancellationRequested();
        if (IsPrime(n)) count++;
    }
    return count;
}
```

Check often enough to be responsive and rarely enough to be cheap.

## Bridging with Register

```csharp
public static async Task<Process> RunAsync(ProcessStartInfo info, CancellationToken ct)
{
    var process = Process.Start(info) ?? throw new InvalidOperationException("Process did not start");
    await using var registration = ct.Register(() =>
    {
        if (!process.HasExited) process.Kill(entireProcessTree: true);
    });
    await process.WaitForExitAsync(ct);
    return process;
}
```

## Linking and timeouts

```csharp
public async Task<Quote> GetQuoteAsync(Shipment shipment, CancellationToken ct)
{
    using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
    timeout.CancelAfter(TimeSpan.FromSeconds(3));
    try
    {
        return await carrier.QuoteAsync(shipment, timeout.Token);
    }
    catch (OperationCanceledException) when (!ct.IsCancellationRequested)
    {
        throw new TimeoutException("Carrier did not quote within 3 seconds");
    }
}
```

The `when` filter distinguishes "our timeout fired" from "our caller gave up" — a distinction
that matters for logging, metrics and HTTP status codes.

## Useful members added in modern .NET

| Member | Since | Purpose |
|---|---|---|
| `CancellationTokenSource.CancelAsync()` | .NET 8 | Cancel without running callbacks synchronously on the caller |
| `CancellationTokenSource.TryReset()` | .NET 6 | Reuse a source that was not cancelled (pooling) |
| `Task.WaitAsync(TimeSpan, CancellationToken)` | .NET 6 | Stop *waiting* for a task (does not stop the task) |
| `CancellationToken.UnsafeRegister` | .NET 5 | Register without flowing `ExecutionContext` |
| `new CancellationTokenSource(TimeSpan, TimeProvider)` | .NET 8 | Timeouts that tests can control |

## Sources

- Microsoft Learn, "Cancellation in managed threads" — <https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads>
- Microsoft Learn, `CancellationTokenSource` — <https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource>
- Stephen Toub, "Recommended patterns for CancellationToken" — <https://devblogs.microsoft.com/premier-developer/recommended-patterns-for-cancellationtoken/>
