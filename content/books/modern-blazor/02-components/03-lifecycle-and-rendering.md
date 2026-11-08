---
title: Lifecycle and rendering
questions:
  - id: order
    prompt: In which order do these lifecycle methods first run for a new component?
    options:
      - text: OnAfterRender, OnInitialized, OnParametersSet
      - text: OnParametersSet, SetParametersAsync, OnInitialized, OnAfterRender
      - text: OnInitialized, OnAfterRender, OnParametersSet
      - id: correct-order
        text: SetParametersAsync, OnInitialized, OnParametersSet, OnAfterRender
        correct: true
    explanation: Parameters are set, then initialization runs once, then parameter processing, then rendering and finally after-render callbacks.
  - id: statehaschanged
    prompt: When do you need to call `StateHasChanged()` manually?
    options:
      - text: After every event handler
      - text: At the end of `OnInitializedAsync`
      - id: external
        text: When state changes outside Blazor's own event handling, such as from a timer or a service event
        correct: true
    explanation: Blazor re-renders after its own events and lifecycle methods. For external triggers, call `InvokeAsync(StateHasChanged)`.
  - id: key
    prompt: What does the `@key` directive do in a list?
    options:
      - id: identity
        text: Tells the diffing algorithm which rendered element or component corresponds to which data item, preserving them correctly when the list changes
        correct: true
      - text: Encrypts the element
      - text: Sets the HTML `id` attribute
    explanation: Without `@key`, Blazor matches by position, which can reuse the wrong component instance (and its state) after inserts or reorders.
  - id: render-facts
    prompt: Which statements about Blazor rendering are true?
    type: multiple
    options:
      - text: Blazor builds a render tree and applies only the differences to the DOM
        correct: true
      - text: Overriding `ShouldRender` to return false can skip unnecessary renders
        correct: true
      - text: "`OnAfterRenderAsync(bool firstRender)` is the right place to call JavaScript that needs rendered elements"
        correct: true
      - text: "`OnAfterRender` runs during static server-side rendering"
    explanation: After-render callbacks only run for interactive components, because static SSR has no live DOM on the server.
---
## Learning objectives

- Know the order and purpose of lifecycle methods
- Trigger re-renders correctly
- Use `@key` and `ShouldRender` to control rendering

## The lifecycle

1. `SetParametersAsync` — parameters arrive
2. `OnInitialized{Async}` — first time only
3. `OnParametersSet{Async}` — first time and whenever the parent passes new parameters
4. Render
5. `OnAfterRender{Async}(firstRender)` — DOM updated (interactive only)
6. `Dispose` / `DisposeAsync` — removed from the tree

```razor
@implements IAsyncDisposable
@inject IJSRuntime JS

<div @ref="chartHost"></div>

@code {
    [Parameter] public IReadOnlyList<Point> Data { get; set; } = [];
    private ElementReference chartHost;
    private IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Chart.razor.js");
        }
        if (module is not null)
        {
            await module.InvokeVoidAsync("draw", chartHost, Data);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (module is not null)
        {
            try { await module.DisposeAsync(); } catch (JSDisconnectedException) { }
        }
    }
}
```

## Re-rendering from outside

```csharp
protected override void OnInitialized() => cart.Changed += OnCartChanged;

private void OnCartChanged() => _ = InvokeAsync(StateHasChanged);

public void Dispose() => cart.Changed -= OnCartChanged;
```

`InvokeAsync` marshals onto the renderer's synchronization context — essential in Interactive
Server, where the event may arrive on another thread. Always unsubscribe to avoid memory leaks.

## Diffing and @key

```razor
@foreach (var todo in todos)
{
    <TodoItem @key="todo.Id" Item="todo" />
}
```

## Skipping renders

```csharp
private int lastVersion = -1;

protected override bool ShouldRender()
{
    var changed = Data.Version != lastVersion;
    lastVersion = Data.Version;
    return changed;
}
```

Use `ShouldRender` only for measured hot spots; most components render quickly enough.
