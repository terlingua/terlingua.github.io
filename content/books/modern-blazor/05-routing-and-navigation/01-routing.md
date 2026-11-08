---
title: Routing
questions:
  - id: constraint
    prompt: "What happens when a user visits `/products/abc` and the only matching page is `@page \"/products/{id:int}\"`?"
    options:
      - text: "`id` is set to 0"
      - text: An exception is thrown inside the component
      - id: no-match
        text: The route does not match, so the router shows its not-found content
        correct: true
    explanation: Route constraints filter matches; `abc` is not an int, so no page matches.
  - id: query
    prompt: How does a routable component read `?page=2` from the URL?
    options:
      - text: "With a route template `@page \"/list?page={page}\"`"
      - text: Query strings are not supported in Blazor
      - id: supply-query
        text: With a property marked `[SupplyParameterFromQuery]`, such as `public int? Page { get; set; }`
        correct: true
    explanation: Query parameters are bound by name (or by the `Name` property of the attribute).
  - id: not-found
    prompt: Which .NET 10 features improve handling of missing content?
    type: multiple
    options:
      - text: "`NavigationManager.NotFound()` signals that the requested resource does not exist"
        correct: true
      - text: The `Router` component's `NotFoundPage` parameter renders a dedicated not-found page component
        correct: true
      - text: "`NotFound()` returns HTTP 404 during static server rendering"
        correct: true
      - text: Not-found pages require JavaScript
    explanation: .NET 10 gives a consistent not-found experience across render modes, including a real 404 status for static SSR.
---
## Learning objectives

- Write route templates with parameters and constraints
- Read query strings and navigate in code
- Handle not-found content correctly

## Route templates

```razor
@page "/products"
@page "/products/{Category}"
@page "/products/{Category}/{Id:int}"
@page "/files/{*Path}"
```

| Feature | Example |
|---------|---------|
| Parameter | `{Category}` → `[Parameter] public string? Category { get; set; }` |
| Constraint | `{Id:int}`, `{Date:datetime}`, `{Id:guid}`, `{Active:bool}` |
| Optional | `{Page:int?}` |
| Catch-all | `{*Path}` |

## Query strings

```razor
@page "/search"

@code {
    [SupplyParameterFromQuery(Name = "q")] public string? Query { get; set; }
    [SupplyParameterFromQuery] public int? Page { get; set; }
}
```

```csharp
var url = Navigation.GetUriWithQueryParameters(new Dictionary<string, object?>
{
    ["q"] = query,
    ["page"] = 2
});
Navigation.NavigateTo(url);
```

## Navigation

```razor
<NavLink href="orders" Match="NavLinkMatch.Prefix" ActiveClass="active">Orders</NavLink>
```

```csharp
Navigation.NavigateTo("/orders/42");
Navigation.NavigateTo("/login", forceLoad: true);    // full page load
Navigation.NavigateTo("/step-2", replace: true);     // replace history entry
```

## Not found (.NET 10)

```razor
<Router AppAssembly="typeof(Program).Assembly" NotFoundPage="typeof(Pages.NotFound)">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

```csharp
protected override async Task OnInitializedAsync()
{
    product = await Catalog.FindAsync(Id);
    if (product is null) Navigation.NotFound();
}
```

During static SSR this sets a 404 status code; in interactive modes it renders the not-found page.
`FocusOnNavigate` moves focus to the page heading after navigation, which helps screen-reader users.
