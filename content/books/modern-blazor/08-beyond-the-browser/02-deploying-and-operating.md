---
title: Deploying and operating Blazor
questions:
  - id: static-host
    prompt: Which kind of Blazor app can be deployed to a static host such as GitHub Pages or a CDN?
    options:
      - text: A Blazor Web App using Interactive Server
      - text: Any Blazor app using static SSR
      - id: standalone
        text: A standalone Blazor WebAssembly app
        correct: true
      - text: A Blazor Hybrid app
    explanation: Standalone WebAssembly produces only static files. Blazor Web Apps need an ASP.NET Core server.
  - id: spa-fallback
    prompt: Why does a standalone WebAssembly app on GitHub Pages need a special 404 page or fallback?
    options:
      - id: deep-links
        text: Deep links like `/books/x` don't exist as files, so the host must serve the app's entry page and let the Blazor router handle the URL
        correct: true
      - text: GitHub Pages blocks WebAssembly files
      - text: To display analytics
    explanation: Static hosts map URLs to files. A fallback (or a 404.html redirect trick) hands unknown paths to the client-side router.
  - id: server-ops
    prompt: Which are good practices when running Interactive Server in production?
    type: multiple
    options:
      - text: Enable WebSockets on the host and load balancer
        correct: true
      - text: Use sticky sessions or Azure SignalR Service when running multiple instances
        correct: true
      - text: Customise the reconnection UI so users understand temporary disconnects
        correct: true
      - text: Store per-user state in static fields for speed
    explanation: Circuits depend on a stable connection to the same server; static fields are shared by all users.
---
## Learning objectives

- Choose a hosting strategy for each Blazor model
- Deploy standalone WebAssembly to static hosting
- Operate Interactive Server reliably

## Hosting options

| App type | Host |
|----------|------|
| Blazor Web App (any render mode) | ASP.NET Core: App Service, containers, Kubernetes, IIS |
| Standalone WebAssembly | Any static host: GitHub Pages, Azure Static Web Apps, S3 + CloudFront |
| Hybrid | App stores, MSIX, installers |

## Standalone WebAssembly on GitHub Pages

```bash
dotnet publish src/MySite -c Release -o publish
```

Deploy `publish/wwwroot`. A few details matter on GitHub Pages:

- Add an empty `.nojekyll` file so folders starting with `_` (like `_framework`) are served
- Provide a fallback for deep links (a `404.html` that redirects to `index.html` with the original path)
- Set `<base href>` correctly if the site lives under a sub-path such as `/repo-name/`

This site uses exactly this setup.

## Interactive Server in production

- WebSockets enabled end to end; long-polling is a slow fallback
- Sticky sessions (ARR affinity) or Azure SignalR Service for scale-out
- Data Protection keys shared across instances (for antiforgery and auth cookies)
- Tune `CircuitOptions`, such as `DisconnectedCircuitMaxRetained` and `DisconnectedCircuitRetentionPeriod`

The .NET 10 templates include a `ReconnectModal` component with its own CSS and JavaScript, so
the reconnection experience is easy to customise.

## Observability

Blazor Server emits metrics and traces for circuits and component rendering through the standard
.NET diagnostics APIs, so the OpenTelemetry setup from any ASP.NET Core app applies here too.
