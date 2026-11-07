---
title: Your first component
questions:
  - id: file-extension
    prompt: What file extension do Blazor components use?
    options:
      - text: "`.cshtml`"
      - text: "`.razor`"
        correct: true
      - text: "`.blazor`"
  - id: parameters
    prompt: How does a parent pass data into a child component?
    options:
      - text: Through public properties marked with `[Parameter]`
        correct: true
      - text: Through a global static variable
      - text: By reading the URL query string
    explanation: Public properties decorated with `[Parameter]` become attributes the parent can set.
---
A component is a `.razor` file that mixes HTML markup with C#.

```razor
<button class="btn" @onclick="Increment">Clicked @count times</button>

@code {
    private int count;
    private void Increment() => count++;
}
```

## Parameters

Components accept input through parameters:

```razor
<h2>@Title</h2>

@code {
    [Parameter] public string Title { get; set; } = "";
}
```

A parent uses it like an HTML element: `<Heading Title="Hello" />`.
