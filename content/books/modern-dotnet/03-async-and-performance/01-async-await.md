---
title: Async and await done right
questions:
  - id: thread
    prompt: While an `await` on an I/O operation (such as an HTTP call) is pending, what happens to the thread that started it?
    options:
      - text: It is blocked until the operation completes
      - text: It is terminated
      - id: released
        text: It is released to do other work; a thread resumes the method when the operation completes
        correct: true
      - text: It spins in a loop checking for completion
    explanation: Async I/O frees threads while waiting, which is why async code scales better under load.
  - id: blocking
    prompt: Why should you avoid `.Result` and `.Wait()` on tasks in application code?
    options:
      - id: deadlock
        text: They block a thread and can cause deadlocks or thread-pool starvation
        correct: true
      - text: They are deprecated and do not compile
      - text: They return the wrong value
    explanation: Blocking on async code wastes threads, and in environments with a synchronization context it can deadlock. Use `await` all the way up.
  - id: async-void
    prompt: When is `async void` appropriate?
    options:
      - text: For any method that does not return a value
      - id: handlers
        text: Only for event handlers that require a `void` signature
        correct: true
      - text: For library methods to improve performance
    explanation: Exceptions from `async void` cannot be caught by the caller and crash the process. Return `Task` everywhere else.
  - id: cancellation
    prompt: Which are good practices for cancellation?
    type: multiple
    options:
      - text: Accept a `CancellationToken` in async APIs and pass it to the calls you make
        correct: true
      - text: In ASP.NET Core, bind the request's token by adding a `CancellationToken` parameter to the endpoint
        correct: true
      - text: Use `CancellationTokenSource.CancelAfter` or a linked source to impose timeouts
        correct: true
      - text: Catch `OperationCanceledException` and return success to hide it
    explanation: Flow tokens end-to-end and let cancellation surface as `OperationCanceledException`, which frameworks treat as a cancelled request rather than a failure.
---
## Learning objectives

- Explain what `await` does and does not do
- Avoid blocking, `async void` and other common mistakes
- Flow cancellation through an application

## What await does

`await` does not create threads. It registers a continuation and returns control to the caller.
For I/O-bound work — HTTP, databases, files — no thread is consumed while waiting.

```csharp
public async Task<WeatherForecast?> GetForecastAsync(string city, CancellationToken ct)
{
    using var response = await _http.GetAsync($"forecast/{Uri.EscapeDataString(city)}", ct);
    if (response.StatusCode == HttpStatusCode.NotFound) return null;
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<WeatherForecast>(ct);
}
```

## Concurrency, not just asynchrony

```csharp
var userTask = _users.GetAsync(id, ct);
var ordersTask = _orders.ListAsync(id, ct);
await Task.WhenAll(userTask, ordersTask);
var (user, orders) = (userTask.Result, ordersTask.Result); // safe: both tasks have completed
```

Starting both before awaiting lets them run at the same time.

## Common mistakes

| Mistake | Fix |
|---------|-----|
| `task.Result` / `task.Wait()` on incomplete tasks | `await task` |
| `async void` methods | `async Task` (except event handlers) |
| `Task.Run` around I/O in ASP.NET Core | Call the async API directly |
| Forgetting to pass the `CancellationToken` | Thread it through every call |
| Returning `Task` from inside `using` without awaiting | `await` inside the `using` scope |

## ConfigureAwait

`ConfigureAwait(false)` tells the continuation not to resume on the captured synchronization
context. ASP.NET Core has no synchronization context, so it rarely matters in app code; library
authors often still use it because their code may run under UI frameworks.

## ValueTask

`ValueTask<T>` avoids allocating a `Task<T>` when results are often available synchronously (for
example from a cache). It has rules — never await it twice — so use it only for measured hot paths.

## Timeouts with cancellation

```csharp
using var cts = CancellationTokenSource.CreateLinkedTokenSource(requestToken);
cts.CancelAfter(TimeSpan.FromSeconds(5));
await _payments.ChargeAsync(amount, cts.Token);
```
