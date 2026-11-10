---
title: How await works, and the truth about ConfigureAwait
questions:
  - id: state-machine
    prompt: What does the compiler turn an `async` method into?
    options:
      - text: A new thread per method
      - text: A synchronous method with `.Wait()` calls
      - id: state-machine
        text: A state machine that records where it paused and resumes there when an awaited operation completes
        correct: true
      - text: A `Parallel.For` loop
    explanation: Each `await` is a potential suspension point. If the awaited task has already completed, execution continues synchronously without suspending.
  - id: sync-context
    prompt: What is a `SynchronizationContext` used for in the context of `await`?
    options:
      - id: resume-where
        text: Deciding where the continuation runs — for example back on the UI thread in WPF or WinForms
        correct: true
      - text: Synchronizing access to static fields
      - text: Encrypting task results
      - text: Choosing which CPU core runs a method
    explanation: By default `await` captures the current context (or `TaskScheduler`) and posts the continuation back to it.
  - id: aspnetcore-context
    prompt: Does ASP.NET Core have a `SynchronizationContext`?
    options:
      - text: Yes, the same as classic ASP.NET
      - text: Only in Blazor Server
      - text: Only in Debug builds
      - id: no-context
        text: No — continuations run on thread-pool threads, so `ConfigureAwait(false)` has no practical effect in ASP.NET Core application code
        correct: true
    explanation: Classic ASP.NET (System.Web) had `AspNetSynchronizationContext`, which caused the infamous `.Result` deadlocks. ASP.NET Core removed it. Note that Blazor (Server and WebAssembly) does have its own renderer synchronization context.
  - id: when-cfa
    prompt: Where is `ConfigureAwait(false)` still recommended?
    type: multiple
    options:
      - id: libraries
        text: In general-purpose libraries that may be called from UI apps or other environments with a context
        correct: true
      - id: ui-background
        text: In UI apps, in code after an `await` that does not need to touch UI elements
        correct: true
      - text: Everywhere in ASP.NET Core controllers, as a mandatory style rule
      - text: Before `await Task.Yield()`
    explanation: The rule "always use `ConfigureAwait(false)`" is library guidance that hardened into dogma for application code. It is harmless there but adds noise.
  - id: deadlock
    prompt: Why does `var data = GetDataAsync().Result;` deadlock in a WPF button handler?
    options:
      - text: Because `.Result` is not allowed in WPF
      - id: ui-blocked
        text: The UI thread blocks waiting for the task, while the task's continuation waits to be posted back to that same blocked UI thread
        correct: true
      - text: Because the garbage collector pauses the UI thread
      - text: It does not deadlock
    explanation: Each side waits for the other. The fix is `await` all the way up; `ConfigureAwait(false)` inside `GetDataAsync` would only patch the symptom.
  - id: runtime-async
    prompt: What is "runtime async", arriving with .NET 11?
    options:
      - text: A replacement for the `async` keyword
      - text: A way to run async code without tasks
      - id: runtime-native
        text: Async state-machine support implemented in the runtime instead of compiler-generated code, giving lower overhead and cleaner stack traces without changing your source
        correct: true
      - text: A garbage collector mode
    explanation: Your `async`/`await` code stays the same; the mechanics underneath move into the runtime. This is a good example of why writing idiomatic code ages well — the platform improves underneath it.
  - id: completed-fast-path
    prompt: An awaited `ValueTask` is already completed when it is awaited. What happens?
    options:
      - text: The method always yields to the thread pool
      - text: An exception is thrown
      - text: The method restarts from the beginning
      - id: sync-continue
        text: Execution continues synchronously; no continuation is scheduled
        correct: true
    explanation: This synchronous fast path is why cached results and buffered streams make async code cheap.
---
## Learning objectives

- Describe the async state machine at a conceptual level
- Explain synchronization contexts and when `ConfigureAwait(false)` matters
- Identify the classic sync-over-async deadlock

## The mechanics in one picture

```csharp
public async Task<decimal> GetTotalAsync(OrderId id, CancellationToken ct)
{
    var order = await _orders.GetAsync(id, ct);
    var rate = await _rates.GetAsync(order.Currency, ct);
    return order.Total * rate.ToUsd;
}
```

Conceptually the compiler produces:

1. **State 0**: call `_orders.GetAsync`. If it is not complete, store state = 1, register a
   continuation and return an incomplete task to the caller.
2. **State 1**: read the order, call `_rates.GetAsync`. If not complete, store state = 2, register,
   return.
3. **State 2**: compute, set the task's result.

Local variables that live across awaits are hoisted into the state machine.

## Where the continuation runs

| Environment | Context captured by `await` | Consequence |
|---|---|---|
| WPF / WinForms / MAUI | UI `SynchronizationContext` | Continuation returns to the UI thread |
| Classic ASP.NET (System.Web) | `AspNetSynchronizationContext` | One-at-a-time request context; `.Result` deadlocks |
| ASP.NET Core | none | Continuation on any thread-pool thread |
| Console / worker service | none | Thread pool |
| Blazor | renderer context | Continuation on the renderer's logical thread |

## ConfigureAwait: a dogma case study

In 2012–2016, much .NET code ran under classic ASP.NET or WPF, and libraries were called from
both. Library authors were told "use `ConfigureAwait(false)` everywhere so your library cannot
deadlock someone's UI". Good advice for libraries.

The advice then spread, without its reason, into application code — including ASP.NET Core
applications, which have no context to capture. Today you can find codebases with
`.ConfigureAwait(false)` on every line of controller code, enforced by a rule nobody can explain.
That is legacy dogma.

**Rules of thumb in 2026:**

- Writing a reusable NuGet library: use `ConfigureAwait(false)` (an analyzer such as CA2007 can
  enforce it in library projects only).
- Writing ASP.NET Core, worker service or console application code: omit it.
- Writing UI code: omit it where you need the UI thread afterwards; use it in non-UI helpers.
- Never use it to "fix" a deadlock caused by blocking — remove the blocking instead.

.NET 8 added `ConfigureAwaitOptions` for advanced cases:

```csharp
await task.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
```

## Real-world scenario: the Friday deadlock

A WinForms tool called a shared library's `GetCustomersAsync().Result` in a `Form_Load` handler.
It worked for months — until the library added its first real `await` on network I/O. The UI
froze on startup. Two fixes were proposed: add `ConfigureAwait(false)` throughout the library, or
make `Form_Load` `async void` and `await` the call. The second fixes the cause; the first hides it
and leaves the next blocking call to deadlock elsewhere. (Do both if you own the library.)

## Sources

- Stephen Toub, "ConfigureAwait FAQ" — <https://devblogs.microsoft.com/dotnet/configureawait-faq/>
- Stephen Toub, "How Async/Await Really Works in C#" — <https://devblogs.microsoft.com/dotnet/how-async-await-really-works/>
- Stephen Cleary, "ASP.NET Core SynchronizationContext" — <https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html>
- Microsoft Learn, "What's new in .NET 11" (runtime async) — <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-11/overview>
