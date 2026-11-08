---
title: One component model, many hosts
questions:
  - id: what-is
    prompt: What is the core idea that ties all forms of Blazor together?
    options:
      - id: razor-components
        text: UI is built from Razor components written in C# and markup, and the same components can run under different hosts
        correct: true
      - text: Blazor compiles C# to JavaScript
      - text: Blazor is a wrapper around React
      - text: Every Blazor app needs a SignalR connection
    explanation: Razor components are the unit of UI. Where and how they run — server, browser or native app — is a hosting decision.
  - id: hybrid
    prompt: In a Blazor Hybrid app built with .NET MAUI, where does the component code execute?
    options:
      - text: In WebAssembly inside the embedded web view
      - id: native
        text: Natively in the app's .NET process, rendering into an embedded web view
        correct: true
      - text: On a remote web server
    explanation: Hybrid components run on the device's .NET runtime with full native API access; the `BlazorWebView` only displays the HTML.
  - id: hosts
    prompt: Which are real ways to host Blazor components?
    type: multiple
    options:
      - text: A Blazor Web App served by ASP.NET Core, with static and interactive render modes
        correct: true
      - text: A standalone Blazor WebAssembly app served as static files
        correct: true
      - text: A .NET MAUI, WPF or Windows Forms app using `BlazorWebView`
        correct: true
      - text: Directly inside SQL Server stored procedures
    explanation: Blazor Web App, standalone WebAssembly and Hybrid are the three hosting families.
---
Blazor is not one technology but one **component model** that runs in several places.

## Learning objectives

- Describe the Blazor hosting families
- Explain where component code runs in each model
- Choose a starting point for a new project

## Razor components

```razor
<h3>@Title</h3>
<button class="btn" @onclick="() => count++">Clicked @count times</button>

@code {
    [Parameter] public string Title { get; set; } = "Counter";
    private int count;
}
```

The same component can be rendered as static HTML on the server, run interactively on the server
over a real-time connection, run in the browser on WebAssembly, or run inside a native desktop or
mobile app.

## The hosting families

| Family | Code runs | Typical use |
|--------|-----------|-------------|
| **Blazor Web App** | Server, browser, or both — chosen per component by render mode | Most new web apps |
| **Standalone WebAssembly** | Browser only; host is any static file server | Offline-capable SPAs, GitHub Pages, CDNs |
| **Blazor Hybrid** | Natively on the device, displayed in a `BlazorWebView` | Desktop and mobile apps sharing web UI |

Before .NET 8, people spoke of "Blazor Server" and "Blazor WebAssembly" as separate project types.
Since .NET 8, the **Blazor Web App** template unifies them: one app, with each page or component
choosing how it renders.

## Choosing a starting point

- Public content site with some interactivity → Blazor Web App, static SSR by default
- Internal line-of-business app on a fast network → Blazor Web App with Interactive Server
- App that must work offline or be hosted purely as static files → standalone WebAssembly
- Desktop/mobile app with shared UI → Hybrid, with components in a shared Razor class library

> This site is itself a standalone Blazor WebAssembly app hosted on GitHub Pages.
