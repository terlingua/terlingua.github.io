---
title: Component lifecycle
questions:
  - id: async-load
    prompt: Which lifecycle methods are good places to load data asynchronously?
    type: multiple
    options:
      - text: "`OnInitializedAsync`"
        correct: true
      - text: "`OnParametersSetAsync`"
        correct: true
      - text: The component constructor
    explanation: Use `OnInitializedAsync` for one-time loads and `OnParametersSetAsync` when the data depends on parameters that can change.
  - id: js-interop
    prompt: When is it safe to call JavaScript interop that touches the rendered DOM?
    options:
      - text: In `OnInitialized`
      - text: In `OnAfterRenderAsync`
        correct: true
      - text: In `SetParametersAsync`
---
Every component goes through the same sequence of lifecycle methods:

1. `SetParametersAsync` — parameters arrive from the parent
2. `OnInitialized` / `OnInitializedAsync` — runs once
3. `OnParametersSet` / `OnParametersSetAsync` — runs whenever parameters change
4. `OnAfterRender` / `OnAfterRenderAsync` — runs after the DOM is updated

## Reacting to route changes

When the user navigates between two URLs handled by the **same** page, Blazor reuses the
component instance. `OnInitialized` does not run again, but `OnParametersSet` does — so load
route-dependent data there.
