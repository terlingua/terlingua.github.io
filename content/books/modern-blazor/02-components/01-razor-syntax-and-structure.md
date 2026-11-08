---
title: Razor syntax and component structure
questions:
  - id: code-behind
    prompt: How do you move a component's C# code out of the `.razor` file?
    options:
      - text: Rename the file to `.cshtml`
      - id: partial
        text: Create a `partial class` with the same name in a `.razor.cs` file
        correct: true
      - text: Use `@include`
      - text: It is not possible
    explanation: A `.razor` file compiles to a partial class, so a matching partial class in `Counter.razor.cs` becomes part of the same component.
  - id: inject
    prompt: Which are valid ways to get a service from DI in a component?
    type: multiple
    options:
      - text: "`@inject NavigationManager Navigation` in the markup"
        correct: true
      - text: "`[Inject] private NavigationManager Navigation { get; set; } = default!;` in the code-behind"
        correct: true
      - text: Constructor injection in the component class (supported since .NET 9)
        correct: true
      - text: "`static NavigationManager.Current`"
    explanation: "`@inject`, `[Inject]` properties and, since .NET 9, constructor injection all work."
  - id: directive
    prompt: "What does the `@page \"/orders/{id:int}\"` directive do?"
    options:
      - text: Sets the browser tab title
      - id: route
        text: Makes the component routable at that URL and binds the `id` segment to a parameter
        correct: true
      - text: Creates a new Razor Page
    explanation: The route template becomes a `[Route]` attribute on the generated class; matching segments bind to parameters with the same name.
---
## Learning objectives

- Read and write Razor component syntax
- Split markup and code with partial classes
- Inject services and use common directives

## Anatomy

```razor
@page "/orders/{Id:int}"
@inject IOrderService Orders
@implements IDisposable

<PageTitle>Order @Id</PageTitle>

@if (order is null)
{
    <p>Loading…</p>
}
else
{
    <h1>Order @order.Number</h1>
    <ul>
        @foreach (var line in order.Lines)
        {
            <li @key="line.Sku">@line.Quantity × @line.Name</li>
        }
    </ul>
}

@code {
    [Parameter] public int Id { get; set; }
    private OrderDetails? order;

    protected override async Task OnParametersSetAsync() => order = await Orders.GetAsync(Id);

    public void Dispose() { }
}
```

## Common directives

| Directive | Purpose |
|-----------|---------|
| `@page` | Route template |
| `@rendermode` | Render mode for this component |
| `@inject` | Inject a service |
| `@using` / `@namespace` | Namespaces |
| `@implements` / `@inherits` | Interfaces and base class |
| `@typeparam` | Generic component type parameter |
| `@attribute` | Apply a C# attribute, e.g. `[Authorize]` |
| `@layout` | Choose a layout |

## Code-behind

```csharp
// OrderPage.razor.cs
public partial class OrderPage(IOrderService orders)
{
    [Parameter] public int Id { get; set; }
    private OrderDetails? order;

    protected override async Task OnParametersSetAsync() => order = await orders.GetAsync(Id);
}
```

Constructor injection in components is supported from .NET 9. Code-behind keeps large components
navigable and lets you unit test logic more easily.

## Razor expressions

- `@value` — implicit expression; HTML-encoded output
- `@(a + b)` — explicit expression
- `@((MarkupString)html)` — render trusted raw HTML (never untrusted input)
- `@* comment *@` — Razor comment, not sent to the browser
