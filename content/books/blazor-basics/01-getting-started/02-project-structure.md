---
title: Project structure
questions:
  - id: entry-point
    prompt: Which file configures services and starts the app?
    options:
      - text: "`App.razor`"
      - text: "`Program.cs`"
        correct: true
      - text: "`wwwroot/index.html`"
    explanation: "`Program.cs` builds the `WebAssemblyHost`, registers services for dependency injection and calls `RunAsync`."
  - id: wwwroot
    prompt: What belongs in `wwwroot`?
    type: multiple
    options:
      - text: Static assets such as CSS, images and JavaScript
        correct: true
      - text: The host page `index.html`
        correct: true
      - text: Razor components
---
A new Blazor WebAssembly project has a small, predictable layout:

| Path | Purpose |
|------|---------|
| `Program.cs` | Builds the host and registers services |
| `App.razor` | Sets up the router |
| `Pages/` | Routable components (`@page`) |
| `Components/` | Reusable components |
| `wwwroot/` | Static files served as-is |

## Program.cs

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddScoped<IBookService, BookService>();
await builder.Build().RunAsync();
```

Services registered here can be injected into any component with `@inject`.
