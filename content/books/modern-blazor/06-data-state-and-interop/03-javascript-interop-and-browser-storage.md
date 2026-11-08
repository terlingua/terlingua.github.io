---
title: JavaScript interop and browser storage
questions:
  - id: module
    prompt: What is the recommended way to call your own JavaScript from a component?
    options:
      - id: es-module
        text: "Import a JavaScript module with `IJSRuntime.InvokeAsync<IJSObjectReference>(\"import\", path)` and call its exports"
        correct: true
      - text: Add functions to `window` in a global script and call them by name
      - text: Use `eval` through interop
      - text: Write JavaScript inside `@code` blocks
    explanation: Modules avoid polluting the global namespace, load on demand and can be collocated with the component as `Component.razor.js`.
  - id: when
    prompt: Why must DOM-related interop calls happen in `OnAfterRenderAsync` rather than `OnInitializedAsync`?
    options:
      - id: dom
        text: The elements do not exist until after the first render, and during prerendering there is no browser to call
        correct: true
      - text: Interop is only allowed on Mondays
      - text: "`OnInitializedAsync` runs on a background thread"
    explanation: "`OnAfterRenderAsync(firstRender: true)` runs only in an interactive context with the DOM in place."
  - id: dotnet-from-js
    prompt: Which are required for JavaScript to call an instance method on a .NET object?
    type: multiple
    options:
      - text: Mark the method with `[JSInvokable]`
        correct: true
      - text: Pass a `DotNetObjectReference.Create(this)` to JavaScript
        correct: true
      - text: Dispose the `DotNetObjectReference` when the component is disposed
        correct: true
      - text: Make the method `static` and `private`
    explanation: JavaScript calls `dotNetRef.invokeMethodAsync('Name', args)`. Undisposed references leak memory.
---
## Learning objectives

- Call JavaScript modules from components
- Call .NET from JavaScript
- Wrap browser storage in a resilient service

## Collocated modules

```javascript
// Components/CopyButton.razor.js
export function copy(text) {
  return navigator.clipboard.writeText(text);
}
```

```razor
@implements IAsyncDisposable
@inject IJSRuntime JS

<button @onclick="CopyAsync">Copy</button>

@code {
    [Parameter, EditorRequired] public string Text { get; set; } = "";
    private IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/CopyButton.razor.js");
    }

    private async Task CopyAsync()
    {
        if (module is not null) await module.InvokeVoidAsync("copy", Text);
    }

    public async ValueTask DisposeAsync()
    {
        try { if (module is not null) await module.DisposeAsync(); }
        catch (JSDisconnectedException) { }
    }
}
```

`JSDisconnectedException` occurs in Interactive Server when the circuit is gone — the browser has
already cleaned up, so it is safe to ignore during disposal.

## Calling .NET from JavaScript

```csharp
private DotNetObjectReference<Map>? selfRef;

protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (!firstRender) return;
    selfRef = DotNetObjectReference.Create(this);
    module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Map.razor.js");
    await module.InvokeVoidAsync("init", mapElement, selfRef);
}

[JSInvokable]
public Task OnMarkerClicked(int id) => SelectAsync(id);
```

```javascript
export function init(element, dotNet) {
  element.addEventListener('marker-click', e => dotNet.invokeMethodAsync('OnMarkerClicked', e.detail.id));
}
```

## A resilient storage service

```csharp
public sealed class BrowserStorage(IJSRuntime js, ILogger<BrowserStorage> logger)
{
    public async ValueTask<T?> GetAsync<T>(string key)
    {
        try
        {
            var json = await js.InvokeAsync<string?>("localStorage.getItem", key);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex) when (ex is JSException or JsonException or InvalidOperationException)
        {
            logger.LogWarning(ex, "Could not read {Key} from local storage", key);
            return default;
        }
    }
}
```

Storage can be disabled, full or unavailable during prerendering (`InvalidOperationException`).
Fail soft: the app should keep working with in-memory state. For larger or structured data,
IndexedDB is a better fit than `localStorage` — this site stores reading progress there.

## WebAssembly-only: JSImport/JSExport

In WebAssembly, `[JSImport]` and `[JSExport]` from `System.Runtime.InteropServices.JavaScript`
offer faster, source-generated interop. They do not work in Interactive Server, so prefer
`IJSRuntime` in components that may run in either mode.
