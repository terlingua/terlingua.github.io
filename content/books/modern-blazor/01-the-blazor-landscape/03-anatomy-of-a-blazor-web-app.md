---
title: Anatomy of a Blazor Web App
questions:
  - id: template
    prompt: Which command creates a Blazor Web App that supports both server and WebAssembly interactivity per component?
    options:
      - id: auto
        text: "`dotnet new blazor --interactivity Auto`"
        correct: true
      - text: "`dotnet new blazorserver`"
      - text: "`dotnet new razor --wasm`"
      - text: "`dotnet new mvc --blazor`"
    explanation: The `blazor` template with `--interactivity Auto` creates a server project and a `.Client` project with both render modes registered.
  - id: two-projects
    prompt: Why does an Auto or WebAssembly Blazor Web App have a separate `.Client` project?
    options:
      - text: To hold unit tests
      - text: Because Razor files cannot be in the server project
      - id: download
        text: Components in it are compiled into assemblies the browser downloads and runs on WebAssembly
        correct: true
    explanation: Server-only code (database access, secrets) stays in the server project; anything that may run in the browser goes in the client project.
  - id: program
    prompt: Which calls are needed in the server's `Program.cs` to support Interactive Server components?
    type: multiple
    options:
      - text: "`builder.Services.AddRazorComponents().AddInteractiveServerComponents()`"
        correct: true
      - text: "`app.MapRazorComponents<App>().AddInteractiveServerRenderMode()`"
        correct: true
      - text: "`app.UseAntiforgery()`"
        correct: true
      - text: "`builder.Services.AddControllersWithViews()`"
    explanation: Services register the component infrastructure, the endpoint mapping enables the render mode, and antiforgery middleware protects forms.
---
## Learning objectives

- Create a Blazor Web App from the template
- Understand the server and client projects
- Read the `Program.cs` that wires render modes together

## Creating the app

```bash
dotnet new blazor -n Shop --interactivity Auto
```

| Option | Values |
|--------|--------|
| `--interactivity` | `None`, `Server`, `WebAssembly`, `Auto` |
| `--all-interactive` | Make the whole app interactive instead of per page |
| `--auth` | `None` or `Individual` |

## Solution layout

```text
Shop/                    server project (ASP.NET Core)
  Components/
    App.razor            root HTML document
    Routes.razor         router
    Layout/
    Pages/               static or server-interactive pages
  Program.cs
Shop.Client/             WebAssembly project
  Pages/                 pages that can run in the browser
  Program.cs
```

## Server Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Shop.Client._Imports).Assembly);

app.Run();
```

`MapStaticAssets` (.NET 9+) serves static files with build-time compression and fingerprinting.

## Client Program.cs

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<ICartService, ClientCartService>();
await builder.Build().RunAsync();
```

Services used by components that may run in **both** places must be registered in **both**
`Program.cs` files — often with different implementations. That idea is the key to reusable
Blazor components and is covered in the data chapter.

## _Imports.razor

```razor
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
```

The static import is what lets you write `@rendermode InteractiveServer` instead of
`RenderMode.InteractiveServer`.
