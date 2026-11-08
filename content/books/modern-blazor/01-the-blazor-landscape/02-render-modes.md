---
title: Render modes
questions:
  - id: static-ssr
    prompt: A component with no render mode in a Blazor Web App renders with static server-side rendering. What does that mean?
    options:
      - id: html-only
        text: The server renders HTML once per request; `@onclick` handlers do not run
        correct: true
      - text: It runs in WebAssembly after the page loads
      - text: It keeps a SignalR circuit open
      - text: It cannot use any C# code
    explanation: Static SSR produces plain HTML. Interactivity requires an interactive render mode; forms and links can still work through enhanced navigation and form posts.
  - id: auto
    prompt: How does the `InteractiveAuto` render mode decide how to run a component?
    options:
      - text: It always uses WebAssembly
      - id: auto-choice
        text: It uses Interactive Server while the WebAssembly runtime is not yet available, and WebAssembly once it can be loaded quickly
        correct: true
      - text: It asks the user to choose
      - text: It switches back and forth on every click
    explanation: Auto gives fast first interaction via the server, then moves to client-side execution on later visits once assets are cached. A component keeps its chosen mode once it starts.
  - id: prerender
    prompt: How do you disable prerendering for an interactive server component?
    options:
      - text: Add `@prerender false` to the component
      - text: Prerendering cannot be disabled
      - id: new-mode
        text: "`@rendermode @(new InteractiveServerRenderMode(prerender: false))`"
        correct: true
    explanation: Each interactive render mode class takes a `prerender` argument; prerendering is on by default.
  - id: rules
    prompt: Which statements about render modes are true?
    type: multiple
    options:
      - text: Interactive WebAssembly and Auto components must be in an assembly the browser can download, usually the `.Client` project
        correct: true
      - text: Parameters passed from a static parent to an interactive child must be serializable
        correct: true
      - text: A child of an interactive component inherits its parent's render mode
        correct: true
      - text: A child of an Interactive Server component can switch itself to Interactive WebAssembly
    explanation: Render mode flows down the tree and cannot be switched to a different interactive mode inside an interactive subtree.
---
## Learning objectives

- Describe the four render modes
- Apply render modes per component, per page or globally
- Understand prerendering and the rules for mixing modes

## The four modes

| Mode | Where it runs | Interactive | Notes |
|------|---------------|-------------|-------|
| Static SSR | Server, per request | No | Fast, SEO-friendly HTML |
| `InteractiveServer` | Server, over a SignalR circuit | Yes | Tiny download; needs a constant connection |
| `InteractiveWebAssembly` | Browser | Yes | Works offline after download; larger first load |
| `InteractiveAuto` | Server first, then browser | Yes | Best of both, more moving parts |

## Applying a render mode

```razor
@page "/counter"
@rendermode InteractiveServer
```

```razor
<Counter @rendermode="InteractiveWebAssembly" />
```

Make an entire app interactive by setting the mode on the root `Routes` and `HeadOutlet`
components in `App.razor`:

```razor
<HeadOutlet @rendermode="InteractiveAuto" />
<Routes @rendermode="InteractiveAuto" />
```

## Prerendering

Interactive components are **prerendered** on the server by default: the user sees HTML
immediately, then the component becomes interactive. Lifecycle methods therefore run twice — once
during prerender and once when interactive — which matters for data loading (see the state
chapter).

## Knowing where you are

.NET 9 added `RendererInfo` and `AssignedRenderMode`:

```razor
@if (!RendererInfo.IsInteractive)
{
    <p>Loading interactive features…</p>
}
<button disabled="@(!RendererInfo.IsInteractive)" @onclick="Save">Save</button>

<small>Running on: @RendererInfo.Name</small>
```

`RendererInfo.Name` is `Static`, `Server`, `WebAssembly` or `WebView`. Disabling buttons until
interactive prevents clicks that would do nothing during prerendering.

## Rules for mixing modes

- Interactive modes flow **down** the component tree
- A child cannot switch to a *different* interactive mode
- Parameters from a static parent to an interactive child must be JSON-serializable — so you
  cannot pass `RenderFragment` child content across that boundary
- WebAssembly/Auto components must live where the browser can download them
