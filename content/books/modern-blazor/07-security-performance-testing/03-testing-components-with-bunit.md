---
title: Testing components with bUnit
questions:
  - id: what
    prompt: What does bUnit let you do?
    options:
      - id: render
        text: Render components in a test, interact with the markup and assert on the output — without a browser
        correct: true
      - text: Run end-to-end tests in a real browser
      - text: Load-test a Blazor Server deployment
      - text: Generate components from screenshots
    explanation: bUnit renders components in memory with a test renderer and works with xUnit, NUnit or MSTest.
  - id: services
    prompt: How do you supply a fake `IProductService` to a component under test in bUnit?
    options:
      - text: Edit `Program.cs`
      - text: bUnit cannot inject services
      - id: services-add
        text: Register it on the test context's `Services` collection before rendering
        correct: true
    explanation: The bUnit context has its own service collection used for `@inject` and `[Inject]`.
  - id: assertions
    prompt: Which bUnit capabilities are real?
    type: multiple
    options:
      - text: "`cut.Find(\"button\").Click()` triggers the component's click handler"
        correct: true
      - text: "`cut.Markup` returns the rendered HTML"
        correct: true
      - text: JavaScript interop can be emulated through the context's `JSInterop` property
        correct: true
      - text: bUnit executes real JavaScript modules in Chromium
    explanation: bUnit fakes JS interop; use Playwright for tests that need real JavaScript.
---
## Learning objectives

- Render and interact with components in unit tests
- Provide fake services and parameters
- Know when to use bUnit versus browser tests

## A first test

```csharp
public class RatingTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    [Fact]
    public void Clicking_the_third_star_raises_ValueChanged_with_3()
    {
        int? selected = null;
        var cut = _ctx.Render<Rating>(p => p
            .Add(c => c.Value, 1)
            .Add(c => c.ValueChanged, v => selected = v));

        cut.FindAll("button")[2].Click();

        Assert.Equal(3, selected);
    }

    public void Dispose() => _ctx.Dispose();
}
```

## Injecting fakes

```csharp
[Fact]
public void Shows_products_from_the_service()
{
    _ctx.Services.AddSingleton<IProductService>(new FakeProductService(
        new ProductDto(1, "Pen", 2.50m),
        new ProductDto(2, "Notebook", 4.00m)));

    var cut = _ctx.Render<ProductList>();

    cut.WaitForAssertion(() => Assert.Equal(2, cut.FindAll(".product-card").Count));
}
```

`WaitForAssertion` retries until async rendering completes or a timeout passes.

## JavaScript interop

```csharp
_ctx.JSInterop.Mode = JSRuntimeMode.Loose;          // unconfigured calls return defaults
var module = _ctx.JSInterop.SetupModule("./Components/CopyButton.razor.js");
module.SetupVoid("copy", "hello");
```

## Choosing the right test

| Test | Tool | Checks |
|------|------|--------|
| Component logic and markup | bUnit | Rendering, parameters, events, services |
| Services and handlers | xUnit | Pure C# logic |
| Full user journeys with real JS | Playwright | Browser behaviour end to end |

This site's own components are tested with bUnit on Microsoft.Testing.Platform.
