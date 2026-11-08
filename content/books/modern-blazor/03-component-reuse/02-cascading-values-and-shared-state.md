---
title: Cascading values and shared state
questions:
  - id: cascading
    prompt: What does a `CascadingValue` provide?
    options:
      - text: A value that is stored in the browser's local storage
      - text: A global static variable shared by all users
      - id: descendants
        text: A value available to every descendant component that declares a matching `[CascadingParameter]`
        correct: true
      - text: A CSS variable
    explanation: Cascading values avoid passing the same parameter through every level of the tree.
  - id: isfixed
    prompt: "What does `IsFixed=\"true\"` on a `CascadingValue` do?"
    options:
      - id: no-tracking
        text: Promises the value never changes, so Blazor skips change-notification subscriptions for better performance
        correct: true
      - text: Makes the value read-only for descendants
      - text: Pins the component to the top of the page
    explanation: Fixed cascading values are cheaper because descendants are not re-rendered when the cascading component re-renders.
  - id: state-container
    prompt: Which statements about a scoped "state container" service are true?
    type: multiple
    options:
      - text: In Interactive Server, a scoped service lives for the user's circuit
        correct: true
      - text: In WebAssembly, a scoped service effectively lives for the lifetime of the browser tab's app
        correct: true
      - text: Components should subscribe to its change event and unsubscribe when disposed
        correct: true
      - text: A singleton state container in Interactive Server is a safe place for per-user data
    explanation: Singletons on the server are shared by every user, so they must never hold per-user state.
---
## Learning objectives

- Share values down the tree with cascading values
- Register root-level cascading values
- Share state across components with a state container service

## Cascading values

```razor
<CascadingValue Value="theme" IsFixed="true">
    @Body
</CascadingValue>
```

```razor
@code {
    [CascadingParameter] public Theme Theme { get; set; } = default!;
}
```

Since .NET 8, cascading values can be registered at the root, which works across render-mode
boundaries where a `CascadingValue` component in a static layout could not reach:

```csharp
builder.Services.AddCascadingValue(sp => new Theme { Accent = "teal" });
builder.Services.AddCascadingAuthenticationState();
```

## A state container

```csharp
public sealed class CartState
{
    private readonly List<CartLine> _lines = [];
    public IReadOnlyList<CartLine> Lines => _lines;
    public decimal Total => _lines.Sum(l => l.Price * l.Quantity);
    public event Action? Changed;

    public void Add(Product product)
    {
        var line = _lines.FirstOrDefault(l => l.Sku == product.Sku);
        if (line is null) _lines.Add(new CartLine(product.Sku, product.Name, product.Price, 1));
        else line.Quantity++;
        Changed?.Invoke();
    }
}

builder.Services.AddScoped<CartState>();
```

```razor
@implements IDisposable
@inject CartState Cart

<span class="cart-badge">@Cart.Lines.Count</span>

@code {
    protected override void OnInitialized() => Cart.Changed += OnChanged;
    private void OnChanged() => InvokeAsync(StateHasChanged);
    public void Dispose() => Cart.Changed -= OnChanged;
}
```

## Where the state lives

| Host | Scoped service lifetime |
|------|-------------------------|
| Interactive Server | The user's circuit |
| WebAssembly | The app instance in that browser tab |
| Static SSR | A single HTTP request |

State in memory disappears on refresh. Persist anything important — to the server through an API,
or to browser storage (see the interop section).
