---
title: Parameters, events and data binding
questions:
  - id: eventcallback
    prompt: Why should child components expose events as `EventCallback` rather than `Action`?
    options:
      - id: rerender
        text: "`EventCallback` invokes the handler on the right component and automatically re-renders the parent afterwards, and supports async handlers"
        correct: true
      - text: "`Action` cannot be used in Razor files"
      - text: "`EventCallback` is faster because it uses reflection"
    explanation: With a plain `Action`, the parent's state changes but it does not know to re-render.
  - id: two-way
    prompt: To support `@bind-Value` on a custom component, which members must it expose?
    options:
      - text: Only a `[Parameter] public string Value { get; set; }`
      - text: "A `[Parameter] public string Bind { get; set; }`"
      - id: value-changed
        text: A `[Parameter] Value` property and a `[Parameter] EventCallback<T> ValueChanged`
        correct: true
    explanation: "`@bind-Value` expands to setting `Value` and handling `ValueChanged`. An optional `ValueExpression` supports validation in forms."
  - id: dont-mutate
    prompt: Why should a component avoid writing to its own `[Parameter]` properties?
    options:
      - text: It causes a compile error
      - text: Parameters are read-only at runtime
      - id: overwritten
        text: The parent may re-render and overwrite the value, discarding the child's changes
        correct: true
    explanation: Parameters are owned by the parent. Copy into private state or raise an event so the parent updates the value.
  - id: bind-modifiers
    prompt: Which binding features are available in Blazor?
    type: multiple
    options:
      - text: "`@bind:event=\"oninput\"` to update on every keystroke"
        correct: true
      - text: "`@bind:after` to run a method after the bound value changes"
        correct: true
      - text: "`@bind:get` and `@bind:set` to separate reading and writing the value"
        correct: true
      - text: "`@bind:sql` to save the value directly to a database"
    explanation: Bind modifiers customise when and how binding happens.
---
## Learning objectives

- Pass data down with parameters
- Raise events up with `EventCallback`
- Implement two-way binding on your own components

## Parameters

```razor
@* Rating.razor *@
<div class="rating" aria-label="@Value out of @Max stars">
    @for (var i = 1; i <= Max; i++)
    {
        var star = i;
        <button type="button" class="@(star <= Value ? "on" : "off")" @onclick="() => SelectAsync(star)">★</button>
    }
</div>

@code {
    [Parameter, EditorRequired] public int Value { get; set; }
    [Parameter] public EventCallback<int> ValueChanged { get; set; }
    [Parameter] public int Max { get; set; } = 5;

    private Task SelectAsync(int star) => ValueChanged.InvokeAsync(star);
}
```

`[EditorRequired]` makes the compiler warn when a parent forgets the parameter. Note the `var star = i;`
copy: the lambda must capture the loop value, not the loop variable.

## Two-way binding

```razor
<Rating @bind-Value="review.Stars" />
<p>You chose @review.Stars stars.</p>
```

`@bind-Value` expands to `Value="review.Stars"` plus `ValueChanged="v => review.Stars = v"`.

## Binding to inputs

```razor
<input @bind="search" @bind:event="oninput" @bind:after="SearchAsync" />

<input @bind:get="quantity" @bind:set="SetQuantity" type="number" />

@code {
    private string search = "";
    private int quantity = 1;

    private Task SearchAsync() => results.LoadAsync(search);
    private void SetQuantity(int value) => quantity = Math.Clamp(value, 1, 99);
}
```

## Events

```razor
<button @onclick="SaveAsync" @onclick:preventDefault>Save</button>
<input @onkeydown="OnKeyDown" />

@code {
    private async Task SaveAsync(MouseEventArgs e) { /* ... */ }
    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape") Close();
    }
}
```

Handlers can be sync or async. After an event handler completes, Blazor re-renders the component
automatically; for async handlers it renders once when the first `await` yields and again when it finishes.
