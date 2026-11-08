---
title: Enhanced navigation and streaming rendering
questions:
  - id: enhanced
    prompt: What does enhanced navigation do in a Blazor Web App?
    options:
      - text: Prefetches every link on the page
      - id: fetch-patch
        text: Intercepts same-app link clicks, fetches the new page and patches the DOM instead of doing a full page reload
        correct: true
      - text: Converts the app into a WebAssembly app
      - text: Adds animations between pages
    explanation: "`blazor.web.js` gives static SSR sites SPA-like navigation while keeping server rendering."
  - id: opt-out
    prompt: How do you make one link perform a full page load instead of enhanced navigation?
    options:
      - text: "Add `target=\"_self\"`"
      - text: Use `<NavLink>` instead of `<a>`
      - id: data-enhance-nav
        text: "Add `data-enhance-nav=\"false\"` to the link or an ancestor element"
        correct: true
    explanation: Opt out where a page relies on scripts that expect a full load, or links to non-Blazor endpoints.
  - id: streaming
    prompt: What does `@attribute [StreamRendering]` do on a statically rendered page?
    options:
      - text: Streams video content
      - id: stream
        text: Sends the initial HTML immediately, then streams updated content over the same response when async work completes
        correct: true
      - text: Makes the page interactive
    explanation: Users see a loading placeholder at once instead of waiting for slow data before any HTML arrives.
  - id: permanent
    prompt: Which statements are true?
    type: multiple
    options:
      - text: "`data-permanent` preserves an element's content across enhanced navigations"
        correct: true
      - text: Streaming rendering works without WebSockets or WebAssembly
        correct: true
      - text: Enhanced navigation keeps interactive components on the page alive when they still exist after the update
        correct: true
      - text: Streaming rendering requires `InteractiveServer` mode
    explanation: Streaming is a static SSR feature that uses a single HTTP response.
---
## Learning objectives

- Understand enhanced navigation and how to opt out
- Use streaming rendering for slow data
- Preserve DOM regions across navigations

## Enhanced navigation

With `blazor.web.js` on the page, clicking an internal link triggers a `fetch` of the next page,
and Blazor patches the differences into the current DOM. The result feels like a single-page app,
while every page remains server-rendered HTML that search engines and no-JavaScript browsers can use.

```html
<a href="reports/export.csv" data-enhance-nav="false">Download CSV</a>
```

## Preserving regions

```html
<div data-permanent id="chat-widget">…</div>
```

Content marked `data-permanent` is left untouched by enhanced page updates — useful for
third-party widgets that keep their own state.

## Streaming rendering

```razor
@page "/weather"
@attribute [StreamRendering]

@if (forecasts is null)
{
    <p><em>Loading…</em></p>
}
else
{
    <table>
        @foreach (var f in forecasts)
        {
            <tr><td>@f.Date</td><td>@f.TemperatureC °C</td></tr>
        }
    </table>
}

@code {
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync() =>
        forecasts = await Weather.GetForecastsAsync();
}
```

The server sends the page with "Loading…" immediately and keeps the response open. When
`OnInitializedAsync` completes, it streams the updated markup, and `blazor.web.js` swaps it in.

## Choosing the right tool

| Need | Use |
|------|-----|
| Fast pages, mostly content | Static SSR + enhanced navigation |
| Slow data on a static page | `[StreamRendering]` |
| Rich interaction (drag, live updates) | An interactive render mode for that component |
