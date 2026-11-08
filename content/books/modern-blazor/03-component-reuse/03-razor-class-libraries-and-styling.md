---
title: Razor class libraries, CSS isolation and attribute splatting
questions:
  - id: rcl
    prompt: Why package components in a Razor class library (RCL)?
    options:
      - text: Because components cannot live in a web project
      - id: share
        text: To share components and their static assets across several apps, including Web Apps, WebAssembly and Hybrid apps
        correct: true
      - text: To compile components to JavaScript
    explanation: An RCL is a normal .NET library (`Microsoft.NET.Sdk.Razor`) that can be referenced or published as a NuGet package.
  - id: static-path
    prompt: How does an app reference `wwwroot/logo.svg` from an RCL whose package ID is `Contoso.UI`?
    options:
      - id: content
        text: "`_content/Contoso.UI/logo.svg`"
        correct: true
      - text: "`/logo.svg`"
      - text: "`/Contoso.UI/wwwroot/logo.svg`"
    explanation: Static web assets from libraries are exposed under `_content/{PackageId}/`.
  - id: css-isolation
    prompt: Which statements about CSS isolation are true?
    type: multiple
    options:
      - text: Styles in `Button.razor.css` apply only to the `Button` component's markup
        correct: true
      - text: "`::deep` lets a parent's isolated styles reach elements rendered by child components"
        correct: true
      - text: Isolated styles are bundled into a `{AssemblyName}.styles.css` file
        correct: true
      - text: Isolated styles are applied at runtime by JavaScript
    explanation: Scoping happens at build time by rewriting selectors with a unique attribute.
  - id: splatting
    prompt: What does `[Parameter(CaptureUnmatchedValues = true)]` on a dictionary parameter enable?
    options:
      - id: splat
        text: Collecting any extra attributes the caller sets, so they can be applied to an element with `@attributes`
        correct: true
      - text: Capturing exceptions thrown by children
      - text: Capturing user keystrokes
    explanation: Attribute splatting lets wrapper components pass through `id`, `aria-*`, `class` and other attributes they did not explicitly declare.
---
## Learning objectives

- Build a reusable component library
- Style components with CSS isolation
- Pass through arbitrary attributes with splatting

## Creating a library

```bash
dotnet new razorclasslib -n Contoso.UI
dotnet add Shop reference Contoso.UI
```

Add `@using Contoso.UI` to `_Imports.razor` and the components are available everywhere.

## A design-system button

```razor
@* Contoso.UI/Button.razor *@
<button type="@Type" class="cx-btn cx-btn-@Variant @Class" @attributes="AdditionalAttributes" @onclick="OnClick">
    @ChildContent
</button>

@code {
    [Parameter] public string Variant { get; set; } = "primary";
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public string? Class { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
```

```razor
<Button Variant="danger" aria-label="Delete order" title="Delete" OnClick="DeleteAsync">Delete</Button>
```

`aria-label` and `title` are not parameters, so they land in `AdditionalAttributes` and are splatted
onto the `<button>`.

## CSS isolation

```css
/* Button.razor.css */
.cx-btn { border-radius: .5rem; padding: .5rem 1rem; }
.cx-btn-danger { background: #b91c1c; color: white; }
```

At build time, selectors are rewritten with a unique attribute such as `b-3xxtam6d07`, and all
isolated CSS is bundled into `Contoso.UI.bundle.scp.css`, which flows into the consuming app's
`{AssemblyName}.styles.css`. Reference it once in `App.razor`:

```html
<link rel="stylesheet" href="@Assets["Shop.styles.css"]" />
```

Use `::deep` to style child components from a parent:

```css
.card ::deep h2 { margin-top: 0; }
```

## Static assets

Files in the library's `wwwroot` are served from `_content/Contoso.UI/...`:

```html
<img src="_content/Contoso.UI/logo.svg" alt="Contoso" />
```

## Reuse across hosts

Keep RCL components free of host-specific services. Depend on interfaces (`IProductService`) that
each host implements, and the same library works in a Blazor Web App, standalone WebAssembly and a
MAUI Hybrid app.
