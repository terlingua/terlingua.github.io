---
title: A field guide to .NET vestiges
questions:
  - id: newtonsoft
    prompt: You are starting a new ASP.NET Core project on .NET 10. A colleague adds `Newtonsoft.Json` "because it is the standard JSON library". What is the most accurate response?
    options:
      - text: Agree; `System.Text.Json` cannot serialize records
      - text: Agree; Microsoft deprecated `System.Text.Json` in .NET 9
      - id: stj-default
        text: "`System.Text.Json` is built in, is the framework default, supports records, polymorphism and source generation; add Newtonsoft only for a specific feature you actually need"
        correct: true
      - text: Use both, configured to run in sequence, for safety
    explanation: Newtonsoft.Json was the de facto standard before .NET Core 3.0. It remains a fine library, but adding it by reflex to a new project is anachronistic — a second serializer with different defaults is a source of bugs.
  - id: task-run-io
    prompt: "Why is `await Task.Run(() => _http.GetStringAsync(url))` in an ASP.NET Core endpoint a vestige?"
    options:
      - text: Because `Task.Run` was removed in .NET 8
      - text: Because it makes the call synchronous
      - text: Because it causes a compile error with nullable reference types
      - id: borrows-thread
        text: Because the I/O is already asynchronous; `Task.Run` only queues extra work to another thread-pool thread and adds overhead
        correct: true
    explanation: "`Task.Run` exists to move CPU-bound work off a thread that must stay responsive (like a UI thread). Server code has no such thread, and async I/O needs no thread while it waits."
  - id: datetime-now
    prompt: Which modern replacement makes the vestigial hand-rolled `IClock`/`IDateTimeProvider` interface unnecessary in most codebases?
    options:
      - text: "`DateTime.UtcNow`"
      - text: "`Stopwatch.GetTimestamp()` only"
      - id: timeprovider
        text: "`TimeProvider`, built into .NET 8 and later, with `FakeTimeProvider` for tests"
        correct: true
      - text: "`Environment.TickCount`"
    explanation: "`TimeProvider` abstracts wall-clock time, timestamps and timers. `FakeTimeProvider` (Microsoft.Extensions.TimeProvider.Testing) lets tests advance time deterministically, including `Task.Delay` and `CancellationTokenSource.CancelAfter` when created with the provider."
  - id: string-concat
    prompt: "A reviewer rejects `var label = $\"{first} {last}\";` and demands `StringBuilder` \"because string concatenation is slow\". Which statements are accurate?"
    type: multiple
    options:
      - id: small-fine
        text: For a handful of pieces, interpolation is clear and efficient; the compiler uses `DefaultInterpolatedStringHandler`
        correct: true
      - id: loops
        text: "`StringBuilder` (or `string.Join`/`string.Create`) still matters when building a string in a loop with many iterations"
        correct: true
      - text: "`StringBuilder` is always faster than interpolation"
      - id: overgeneralized
        text: The reviewer's rule is an over-generalized kernel of truth — a classic factoid shape
        correct: true
    explanation: The true part is "repeated concatenation in a loop is quadratic". The false generalization is "all concatenation is slow". Since C# 10, interpolated strings compile to an efficient handler.
  - id: repository-over-ef
    prompt: A team wraps EF Core's `DbContext` in a generic `IRepository<T>` with `GetAll()`, `Add()` and `Save()` "so we can swap the ORM later". What is the main problem?
    options:
      - text: Generic types cannot be registered in DI
      - id: leaky-and-redundant
        text: "`DbContext` already is a unit of work and `DbSet<T>` a repository; the generic wrapper hides useful features (projection, `AsNoTracking`, `Include`) and the swap rarely happens"
        correct: true
      - text: EF Core forbids wrapping `DbContext`
      - text: There is no problem; this is always best practice
    explanation: The pattern made more sense with older ORMs and pre-LINQ data access. A *specific*, query-shaped abstraction (an `IOrderQueries` with `GetOpenOrdersForCustomer`) can still be valuable at a boundary; the generic CRUD wrapper usually is not.
  - id: lock-object
    prompt: What is the modern replacement for `private readonly object _gate = new();` used with `lock` on .NET 9 and later?
    options:
      - id: lock-type
        text: "`private readonly Lock _gate = new();` — the `System.Threading.Lock` type, which the `lock` statement recognizes"
        correct: true
      - text: "`Monitor.Enter(this)`"
      - text: "`Mutex`"
      - text: "`SemaphoreSlim(1)` in all cases"
    explanation: "`System.Threading.Lock` (.NET 9, C# 13) gives the `lock` statement a dedicated, faster primitive. `SemaphoreSlim` is still the tool when you must `await` inside the critical section."
  - id: still-valid
    prompt: Which of these practices is still valid in 2026 — *not* a vestige?
    type: multiple
    options:
      - id: dispose
        text: Disposing `IDisposable` resources deterministically with `using`
        correct: true
      - text: Calling `GC.Collect()` at the end of each web request
      - id: httpclientfactory
        text: Using `IHttpClientFactory` or a long-lived `HttpClient` instead of creating and disposing one per request
        correct: true
      - id: parameterize
        text: Parameterizing every SQL statement that includes user input
        correct: true
    explanation: Deterministic disposal, connection reuse and parameterization address problems that still exist. Forced GC addresses a problem the runtime solves better on its own.
  - id: sealed
    prompt: "Marking classes `sealed` by default: vestige, dogma, or still sensible?"
    options:
      - id: sensible
        text: Still sensible — it states design intent (not built for inheritance) and lets the JIT devirtualize calls
        correct: true
      - text: Vestige — sealing stopped mattering after .NET Framework 2.0
      - text: Dogma — `sealed` makes classes untestable
      - text: Anachronism — C# 14 removed `sealed`
    explanation: Designing for inheritance is a deliberate act (Framework Design Guidelines). Sealing what was not designed for it is honest, and the runtime's devirtualization work since .NET 5 means it can also be slightly faster.
---
## Learning objectives

- Recognize the most common vestigial patterns in .NET codebases
- Know the modern replacement for each
- Avoid replacing one dogma with another

## The catalogue

Each entry has the same shape: what people do, why it once made sense, what changed, and what
to do now.

### Newtonsoft.Json by reflex

- **Then:** Before .NET Core 3.0 (2019) there was no high-performance JSON in the box.
- **Changed:** `System.Text.Json` became the framework default, then gained source generation,
  polymorphism (`[JsonDerivedType]`), `required` support, and (in .NET 11) union support.
- **Now:** Use `System.Text.Json`. Add Newtonsoft only for a feature you have identified.

```csharp
[JsonSerializable(typeof(OrderDto))]
[JsonSerializable(typeof(List<OrderDto>))]
internal partial class AppJsonContext : JsonSerializerContext;

var json = JsonSerializer.Serialize(order, AppJsonContext.Default.OrderDto);
```

### Task.Run around I/O on a server

```csharp
var before = await Task.Run(() => _http.GetStringAsync(url));
var after = await _http.GetStringAsync(url, ct);
```

The first line was copied from desktop code, where moving work off the UI thread mattered. On a
server it borrows a second thread to wait for work that needed no thread at all.

### `new HttpClient()` per call

```csharp
using var client = new HttpClient();
```

Creating and disposing clients per request can exhaust sockets; a single static client can miss
DNS changes. `IHttpClientFactory` (or a long-lived client with
`SocketsHttpHandler.PooledConnectionLifetime`) addresses both. This one is *not* vestigial — the
problem is real, and the fix is current.

### Hand-rolled clock interfaces

```csharp
public sealed class TrialService(TimeProvider time)
{
    public bool IsExpired(DateTimeOffset startedAt) =>
        time.GetUtcNow() - startedAt > TimeSpan.FromDays(30);
}

builder.Services.AddSingleton(TimeProvider.System);
```

In tests:

```csharp
var time = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
var sut = new TrialService(time);
time.Advance(TimeSpan.FromDays(31));
Assert.True(sut.IsExpired(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)));
```

### `lock (this)` and `object` gates

```csharp
private readonly Lock _gate = new();

public void Add(Item item)
{
    lock (_gate)
    {
        _items.Add(item);
    }
}
```

### Generic repositories over EF Core

`DbContext` is a unit of work; `DbSet<T>` is a repository. A generic `IRepository<T>` usually
hides features and adds nothing. Query-shaped interfaces at real boundaries are a different and
often good idea.

### AutoMapper for DTO copies

Covered in full in Chapter 8. In short: explicit mapping methods are shorter to read, safer to
refactor, faster, AOT-friendly, and no longer require a commercial licence decision.

### `ConfigureAwait(false)` everywhere in app code

Covered in Chapter 5. In ASP.NET Core application code it does nothing useful; in reusable
libraries it remains good practice.

### Defensive `GC.Collect()`

The garbage collector tunes itself using allocation rates and memory pressure. Forcing full
collections usually lowers throughput. If memory is a problem, find the allocation with a
profiler (`dotnet-counters`, `dotnet-trace`, `dotnet-gcdump`).

## Do not make a new dogma

It is tempting to turn this catalogue into a list of "never" rules. Resist. Each entry has an
exception:

- Newtonsoft is still right for a project relying on `JObject` dynamic manipulation everywhere.
- `Task.Run` is still right for CPU-heavy work in a desktop app.
- An interface is still right when there are, or will plausibly be, several implementations.

The goal is reasons, not rules.

## Sources

- Microsoft Learn, "Migrate from Newtonsoft.Json to System.Text.Json" — <https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft>
- Microsoft Learn, "Use IHttpClientFactory" — <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests>
- Microsoft Learn, `System.Threading.Lock` — <https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock>
- Microsoft Learn, `TimeProvider` — <https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider>
- David Fowler, "ASP.NET Core Diagnostic Scenarios: Async Guidance" — <https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md>
