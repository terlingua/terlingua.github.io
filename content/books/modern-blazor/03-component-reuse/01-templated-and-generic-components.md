---
title: Templated and generic components
questions:
  - id: childcontent
    prompt: What is special about a parameter named `ChildContent` of type `RenderFragment`?
    options:
      - id: implicit
        text: Markup placed between the component's opening and closing tags is assigned to it automatically
        correct: true
      - text: It must be set in code, never in markup
      - text: It is rendered before the component's own markup
      - text: It can only contain plain text
    explanation: By convention, `ChildContent` receives the component's inner content without an explicit tag.
  - id: render-fragment-t
    prompt: What does `RenderFragment<TItem>` allow?
    options:
      - text: Rendering only items of type `string`
      - id: template
        text: A template that receives a value — for example, how to render each row of a list
        correct: true
      - text: Running a fragment on a background thread
    explanation: The caller supplies markup that uses `context` (or a named `Context`) to access the item.
  - id: typeparam
    prompt: Which statements about generic components are true?
    type: multiple
    options:
      - text: "`@typeparam TItem` declares a type parameter"
        correct: true
      - text: The type argument can often be inferred from a parameter such as `Items`
        correct: true
      - text: "Constraints can be added, for example `@typeparam TItem where TItem : notnull`"
        correct: true
      - text: Generic components cannot accept `RenderFragment` parameters
    explanation: Generic components and render fragments combine to build reusable grids, lists and pickers.
---
Reusable components separate **what** to render (the caller's business) from **how** to lay it
out (the component's business).

## Learning objectives

- Accept child content with `RenderFragment`
- Accept item templates with `RenderFragment<T>`
- Build generic components with `@typeparam`

## Child content

```razor
@* Card.razor *@
<section class="card">
    <header>@Header</header>
    <div class="card-body">@ChildContent</div>
</section>

@code {
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

```razor
<Card>
    <Header><h2>Shipping</h2></Header>
    <ChildContent><p>Arrives in 2–3 days.</p></ChildContent>
</Card>

<Card>
    <p>Only child content — no tag needed.</p>
</Card>
```

## A generic, templated list

```razor
@* ItemList.razor *@
@typeparam TItem where TItem : notnull

@if (Items.Count == 0)
{
    @Empty
}
else
{
    <ul class="item-list">
        @foreach (var item in Items)
        {
            <li @key="item">@ItemTemplate(item)</li>
        }
    </ul>
}

@code {
    [Parameter, EditorRequired] public IReadOnlyList<TItem> Items { get; set; } = [];
    [Parameter, EditorRequired] public RenderFragment<TItem> ItemTemplate { get; set; } = default!;
    [Parameter] public RenderFragment Empty { get; set; } = @<p>Nothing here yet.</p>;
}
```

```razor
<ItemList Items="products" Context="product">
    <ItemTemplate>
        <strong>@product.Name</strong> — @product.Price.ToString("C")
    </ItemTemplate>
</ItemList>
```

`TItem` is inferred as `Product` from `Items`. `Context="product"` renames the implicit `context`
variable. The same `ItemList` now renders customers, orders or anything else.

## Fragments in C#

```csharp
private RenderFragment Badge(string text) => @<span class="badge">@text</span>;
```

Razor templates (`@<tag>...</tag>`) can be written inline in `@code` blocks to produce fragments.
