---
title: Dynamic components and error boundaries
questions:
  - id: dynamic
    prompt: When is `DynamicComponent` useful?
    options:
      - text: To render components faster
      - id: runtime-type
        text: When the component type is only known at runtime, for example from configuration or a CMS
        correct: true
      - text: To load components from JavaScript
      - text: To generate components from SQL tables
    explanation: "`DynamicComponent` takes a `Type` and a `Parameters` dictionary and renders that component."
  - id: boundary
    prompt: What does an `ErrorBoundary` do?
    options:
      - text: Prevents all exceptions in the app, including in background services
      - text: Logs exceptions to the browser console only
      - id: contain
        text: Catches unhandled exceptions thrown while rendering its child content and shows fallback UI instead of breaking the whole app
        correct: true
    explanation: Error boundaries contain rendering and lifecycle failures in a subtree. In Interactive Server, an unhandled exception without a boundary terminates the circuit.
  - id: recover
    prompt: How do you let users retry after an `ErrorBoundary` shows its error content?
    options:
      - text: Reload the page; there is no other way
      - id: recover-method
        text: Call the boundary's `Recover()` method, for example from a button or when navigating
        correct: true
      - text: Set `IsFixed` to false
    explanation: "`Recover()` resets the boundary and re-renders its child content."
---
## Learning objectives

- Render components chosen at runtime
- Contain failures with error boundaries

## DynamicComponent

```razor
@foreach (var widget in dashboard.Widgets)
{
    <DynamicComponent Type="@WidgetTypes[widget.Kind]" Parameters="@widget.Settings" />
}

@code {
    private static readonly Dictionary<string, Type> WidgetTypes = new()
    {
        ["sales"] = typeof(SalesChart),
        ["tasks"] = typeof(TaskList),
        ["weather"] = typeof(WeatherCard)
    };
}
```

`Parameters` is an `IDictionary<string, object>` whose keys must match the target component's
parameter names. Map from configuration through an allow-list like the dictionary above; never
resolve arbitrary type names from user input.

## Error boundaries

```razor
<ErrorBoundary @ref="boundary">
    <ChildContent>
        <OrderHistory CustomerId="customerId" />
    </ChildContent>
    <ErrorContent Context="ex">
        <div class="alert">
            <p>We couldn't load your orders.</p>
            <button @onclick="() => boundary?.Recover()">Try again</button>
        </div>
    </ErrorContent>
</ErrorBoundary>

@code {
    private ErrorBoundary? boundary;
}
```

Put boundaries around independent regions — a widget, a panel — so one failure does not take
down the page. A common pattern is a boundary in the main layout that calls `Recover()` whenever
the route changes:

```csharp
protected override void OnParametersSet() => boundary?.Recover();
```

## What boundaries do not catch

Exceptions in event handlers you `await` inside the component are caught, but exceptions in
fire-and-forget tasks, timers or background services are not. Handle those explicitly and log them.
