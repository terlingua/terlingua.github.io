---
title: "Review I: language, async, cancellation and DI"
questions:
  - id: r-record-with
    prompt: "`var b = a with { Tags = newTags };` where `a` is a record with a `List<string> Notes` property. After the expression, which is true?"
    options:
      - text: "`b.Notes` is a deep copy of `a.Notes`"
      - text: "`a.Tags` now equals `newTags`"
      - id: shared-list
        text: "`a.Notes` and `b.Notes` refer to the same list instance"
        correct: true
      - text: "`b` is the same object as `a`"
    explanation: "`with` performs a shallow copy."
  - id: r-pc
    prompt: "Primary constructor parameters on a `class` are:"
    options:
      - text: Public init-only properties
      - text: Readonly fields
      - text: Static fields
      - id: captured
        text: Captured variables in scope for the class body, mutable unless you copy them into `readonly` fields
        correct: true
    explanation: Records generate properties; classes do not.
  - id: r-field
    prompt: "In C# 14, `public int Quantity { get; set => field = Math.Max(0, value); }` uses `field` to refer to:"
    options:
      - text: A method parameter
      - id: backing
        text: The compiler-synthesized backing field
        correct: true
      - text: A field named `field` you must declare
      - text: The `value` keyword
    explanation: It removes the need to declare a backing field for one-line accessor logic.
  - id: r-extension-state
    prompt: Can a C# 14 extension property store a value per instance?
    options:
      - text: Yes, in a hidden field added by the compiler
      - text: Only for structs
      - text: Only for static extension members
      - id: cannot
        text: No — extension members add no storage
        correct: true
    explanation: Extension properties compute from the receiver.
  - id: r-hot-tasks
    prompt: Two independent queries each take 300 ms. How do you get roughly 300 ms total?
    options:
      - id: start-both
        text: Start both tasks, then `await Task.WhenAll(...)` (on separate connections)
        correct: true
      - text: Await the first, then start and await the second
      - text: Wrap each in `Task.Run` and call `.Wait()`
      - text: Use `ConfigureAwait(false)` on both
    explanation: Tasks are hot. One `DbConnection` or `DbContext` cannot run both at once.
  - id: r-cfa
    prompt: In an ASP.NET Core minimal API handler, adding `.ConfigureAwait(false)` to every await primarily…
    options:
      - text: Prevents deadlocks that would otherwise occur
      - id: noise
        text: Adds noise without practical benefit, because there is no synchronization context
        correct: true
      - text: Doubles throughput
      - text: Is required by the compiler
    explanation: Keep it for reusable libraries.
  - id: r-async-void
    prompt: Which signature is acceptable as `async void`?
    options:
      - text: "`public async void SaveAsync(Order order)`"
      - text: "`public async void Main(string[] args)`"
      - id: event-handler
        text: "`private async void OnButtonClick(object? sender, EventArgs e)` that catches its own exceptions"
        correct: true
      - text: "`public async void Handle(OrderPlaced e)` in a message handler interface"
    explanation: Only event handlers, and they must handle their own exceptions.
  - id: r-whenall-bound
    prompt: The safest way to call an API for 20,000 ids with at most 10 concurrent requests is…
    options:
      - text: "`Task.WhenAll(ids.Select(Call))`"
      - id: pfea
        text: "`Parallel.ForEachAsync` with `MaxDegreeOfParallelism = 10` and a token"
        correct: true
      - text: "`foreach` with `.Result`"
      - text: "`Parallel.ForEach` with blocking calls"
    explanation: Bounded, async and cancellable.
  - id: r-token-not
    prompt: "Cancelling a token passed to `HttpClient.PostAsync` that charges a card guarantees that…"
    options:
      - text: The card was not charged
      - id: stopped-waiting
        text: Only that your code stopped waiting; the server may or may not have processed it
        correct: true
      - text: The server rolled back
      - text: The request will be retried
    explanation: Use idempotency keys for operations with external side effects.
  - id: r-linked
    prompt: To add your own 5-second deadline on top of a caller's token, you…
    options:
      - id: linked
        text: Create a linked source from the caller's token, call `CancelAfter(5s)`, pass the linked token, and dispose the source
        correct: true
      - text: Ignore the caller's token and create a new one
      - text: Call `Thread.Sleep(5000)` then cancel
      - text: Use `Task.WaitAsync` only, which stops the work
    explanation: "`WaitAsync` stops waiting, not working."
  - id: r-oce-catch
    prompt: Which catch clause correctly treats caller cancellation as a normal outcome at a boundary?
    options:
      - text: "`catch (TaskCanceledException) { }`"
      - text: "`catch (Exception) { return Results.Ok(); }`"
      - id: filtered
        text: "`catch (OperationCanceledException) when (ct.IsCancellationRequested) { ... }`"
        correct: true
      - text: "`catch (TimeoutException) { }`"
    explanation: The filter distinguishes the caller's cancellation from other cancellations such as an internal timeout.
  - id: r-lifetimes
    prompt: Which registration combination is a bug?
    options:
      - text: Scoped service depending on a singleton
      - text: Transient service depending on a scoped service, resolved within a request
      - id: singleton-scoped
        text: Singleton service depending on a scoped service
        correct: true
      - text: Scoped service depending on a transient service
    explanation: A dependency must live at least as long as its consumer.
  - id: r-tryadd
    prompt: A library's `AddWidgets()` should register a default `IWidgetClock` that applications can override. It should use…
    options:
      - id: tryadd
        text: "`TryAddSingleton<IWidgetClock, SystemWidgetClock>()`"
        correct: true
      - text: "`AddSingleton` and document that apps must call it first"
      - text: A static property
      - text: "`Replace(...)`"
    explanation: "`TryAdd*` respects registrations made earlier by the application."
  - id: r-keyed
    prompt: Which attribute injects a keyed service into a constructor parameter?
    options:
      - text: "`[Inject(\"key\")]`"
      - text: "`[FromServices]`"
      - text: "`[Named(\"key\")]`"
      - id: fromkeyed
        text: "`[FromKeyedServices(\"key\")]`"
        correct: true
    explanation: Introduced with keyed services in .NET 8.
  - id: r-validate
    prompt: Which settings make DI mistakes fail at startup rather than at first use?
    type: multiple
    options:
      - id: on-build
        text: "`ValidateOnBuild = true`"
        correct: true
      - id: scopes
        text: "`ValidateScopes = true`"
        correct: true
      - id: options-start
        text: "`.ValidateOnStart()` on options"
        correct: true
      - text: "`TreatWarningsAsErrors`"
    explanation: "`TreatWarningsAsErrors` helps at compile time, not with DI graphs."
  - id: r-locator
    prompt: Injecting `IServiceProvider` into a domain service and resolving dependencies inside methods is…
    options:
      - id: locator
        text: The service locator anti-pattern
        correct: true
      - text: Pure DI
      - text: Required for scoped services
      - text: The decorator pattern
    explanation: It hides real dependencies and defers failures to runtime.
---
## Learning objectives

- Recall the core ideas of Chapters 3–7 under mild pressure

## How to use this review

Answer without scrolling back. For each miss, reread the explanation, then revisit the chapter
section it came from. Come back in a week and try again — spaced retrieval is one of the
best-evidenced learning techniques there is.

## Sources

- Roediger and Karpicke, "Test-Enhanced Learning" (2006), *Psychological Science* — <https://doi.org/10.1111/j.1467-9280.2006.01693.x>
