---
title: Persisting prerendered state
questions:
  - id: double-load
    prompt: Why might an interactive component with prerendering load its data twice?
    options:
      - id: twice
        text: The component initialises once during server prerendering and again when it becomes interactive, losing the first result
        correct: true
      - text: Because of a bug in `HttpClient`
      - text: Because `OnInitializedAsync` is never called
      - text: Because the browser refreshes automatically
    explanation: Without persisted state, the interactive instance starts from scratch and repeats the work, often causing a visible flicker.
  - id: persistent-state
    prompt: In .NET 10, what is the simplest way to carry prerendered data into the interactive component?
    options:
      - text: Store it in a static field
      - text: Write it to a cookie manually
      - id: attribute
        text: Mark a public property with `[PersistentState]`
        correct: true
    explanation: The framework serializes the property into the page during prerender and restores it when the component becomes interactive.
  - id: facts
    prompt: Which statements about persistent component state are true?
    type: multiple
    options:
      - text: The persisted data is embedded in the HTML sent to the browser, so it must not contain secrets
        correct: true
      - text: Earlier versions used the `PersistentComponentState` service with `RegisterOnPersisting` and `TryTakeFromJson`
        correct: true
      - text: Persisted values must be JSON-serializable
        correct: true
      - text: Persisted state survives a full browser refresh forever
    explanation: The state transfers once from prerender to interactive; it is not long-term storage.
---
## Learning objectives

- Explain why prerendering can cause double data loading
- Persist state with `[PersistentState]` (.NET 10)
- Use the lower-level `PersistentComponentState` service

## The problem

1. Server prerenders the page — `OnInitializedAsync` loads products, HTML is sent
2. The component starts interactively (circuit or WebAssembly) — a new instance runs
   `OnInitializedAsync` again, momentarily shows "Loading…", then reloads the products

## Declarative persistence (.NET 10)

```razor
@inject IProductService Products

@code {
    [PersistentState]
    public List<ProductDto>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Items ??= [.. await Products.ListAsync(category: null)];
    }
}
```

During prerendering the property is serialized into the page. When the interactive instance
starts, the framework restores `Items` before `OnInitializedAsync` runs, so `??=` skips the second load.

## The underlying service

```razor
@implements IDisposable
@inject PersistentComponentState AppState

@code {
    private List<ProductDto>? items;
    private PersistingComponentStateSubscription subscription;

    protected override async Task OnInitializedAsync()
    {
        subscription = AppState.RegisterOnPersisting(() =>
        {
            AppState.PersistAsJson("products", items);
            return Task.CompletedTask;
        });

        if (!AppState.TryTakeFromJson<List<ProductDto>>("products", out items))
        {
            items = [.. await Products.ListAsync(category: null)];
        }
    }

    public void Dispose() => subscription.Dispose();
}
```

## Guidelines

- Persist only what is needed to render, and never secrets — it is visible in page source
- Keep payloads small; they add to the HTML size
- Alternatively, disable prerendering for components where it adds little value
