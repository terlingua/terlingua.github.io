---
title: Making Blazor fast
questions:
  - id: virtualize
    prompt: A component renders a list of 50,000 rows and is slow. Which built-in component helps most?
    options:
      - id: virtualize
        text: "`Virtualize`"
        correct: true
      - text: "`ErrorBoundary`"
      - text: "`DynamicComponent`"
      - text: "`CascadingValue`"
    explanation: "`Virtualize` renders only the rows visible in the viewport (plus a small overscan) and can fetch items on demand through an `ItemsProvider`."
  - id: aot
    prompt: What is the trade-off of WebAssembly AOT compilation (`RunAOTCompilation`)?
    options:
      - id: tradeoff
        text: Faster execution of CPU-heavy code, but a larger download and longer build times
        correct: true
      - text: Smaller downloads and faster builds, but slower execution
      - text: It removes the need for the .NET runtime in the browser entirely and halves the download
    explanation: AOT compiles .NET code to WebAssembly ahead of time; it speeds up runtime work at the cost of size.
  - id: techniques
    prompt: Which techniques improve Blazor performance?
    type: multiple
    options:
      - text: Using `@key` on list items so diffing preserves elements correctly
        correct: true
      - text: Avoiding large numbers of tiny components in hot loops when a single component can render the markup
        correct: true
      - text: Lazy-loading rarely used assemblies in WebAssembly
        correct: true
      - text: Calling `StateHasChanged()` in a tight loop
    explanation: Each render has a cost. Render less, render smaller subtrees and download less.
  - id: server-scale
    prompt: What is the main scaling consideration for Interactive Server?
    options:
      - text: Browser CPU usage
      - id: memory-connections
        text: Each active user holds a circuit with server memory and a persistent connection
        correct: true
      - text: Download size of the .NET runtime
    explanation: Plan capacity around concurrent circuits and keep per-circuit state small.
---
## Learning objectives

- Render large data sets efficiently
- Reduce download size and startup time for WebAssembly
- Scale Interactive Server

## Virtualization

```razor
<div style="height: 600px; overflow-y: auto">
    <Virtualize ItemsProvider="LoadRows" Context="row" ItemSize="40">
        <ItemContent><OrderRow Order="row" /></ItemContent>
        <Placeholder><div class="row-skeleton"></div></Placeholder>
    </Virtualize>
</div>

@code {
    private async ValueTask<ItemsProviderResult<OrderDto>> LoadRows(ItemsProviderRequest request)
    {
        var page = await Orders.PageAsync(request.StartIndex, request.Count, request.CancellationToken);
        return new ItemsProviderResult<OrderDto>(page.Items, page.TotalCount);
    }
}
```

## QuickGrid

`Microsoft.AspNetCore.Components.QuickGrid` is a fast, minimal data grid with sorting, paging and
virtualization:

```razor
<QuickGrid Items="people" Pagination="pagination">
    <PropertyColumn Property="p => p.Name" Sortable="true" />
    <PropertyColumn Property="p => p.JoinedOn" Format="yyyy-MM-dd" Sortable="true" />
</QuickGrid>
<Paginator State="pagination" />

@code {
    private readonly PaginationState pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Person> people = Enumerable.Empty<Person>().AsQueryable();
}
```

## Rendering less

- Keep component state close to where it is used, so changes re-render small subtrees
- Use `@key` on lists
- Override `ShouldRender` for expensive components with infrequent changes
- Avoid creating new delegates or objects as parameters on every render in hot paths

## WebAssembly download and startup

| Technique | Effect |
|-----------|--------|
| Trimming (on by default when publishing) | Removes unused IL |
| Compression (Brotli/gzip) and fingerprinted static assets | Smaller, cacheable downloads |
| Lazy loading (`BlazorWebAssemblyLazyLoad`) | Download feature assemblies only when needed |
| AOT compilation | Faster CPU-bound code; larger download |
| `InvariantGlobalization` | Skips culture data if you do not need it |

## Interactive Server at scale

Each circuit holds component state in server memory and keeps a WebSocket open. Use sticky
sessions when running multiple servers (or Azure SignalR Service), keep circuit state lean, and set
sensible limits for disconnected circuit retention.
