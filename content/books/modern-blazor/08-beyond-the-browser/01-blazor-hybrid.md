---
title: Blazor Hybrid
questions:
  - id: blazorwebview
    prompt: Which control hosts Razor components inside a .NET MAUI, WPF or Windows Forms app?
    options:
      - id: bwv
        text: "`BlazorWebView`"
        correct: true
      - text: "`WebView2Host`"
      - text: "`RazorPanel`"
      - text: "`ComponentFrame`"
    explanation: "`BlazorWebView` renders components into an embedded browser control while the components run natively."
  - id: native-access
    prompt: What can a Hybrid component do that a WebAssembly component cannot?
    options:
      - text: Run without any .NET runtime
      - id: native
        text: Call native platform APIs directly through .NET, such as the file system, sensors or OS dialogs
        correct: true
      - text: Be indexed by search engines
    explanation: Hybrid components run in a full .NET process on the device, not in a browser sandbox.
  - id: share
    prompt: Which practices help share UI between a Hybrid app and a web app?
    type: multiple
    options:
      - text: Put components in a Razor class library referenced by both apps
        correct: true
      - text: Hide platform differences behind interfaces registered differently in each host
        correct: true
      - text: Use `RendererInfo.Name` (which is `WebView` in Hybrid) only when behaviour must truly differ
        correct: true
      - text: Reference MAUI APIs directly from shared components
    explanation: Shared components should depend on abstractions; host-specific code lives in the host projects.
---
## Learning objectives

- Understand how Blazor Hybrid works
- Host components in .NET MAUI and desktop apps
- Share components between web and native apps

## How it works

In a Hybrid app, Razor components run on the device's .NET runtime. The `BlazorWebView` control
uses the platform's web view (WebView2 on Windows, WKWebView on Apple platforms, Android WebView)
to display the HTML. Communication is a local in-process channel — no web server and no WebAssembly.

## .NET MAUI Blazor Hybrid

```bash
dotnet new maui-blazor -n Shop.Mobile
```

```xml
<!-- MainPage.xaml -->
<BlazorWebView HostPage="wwwroot/index.html">
    <BlazorWebView.RootComponents>
        <RootComponent Selector="#app" ComponentType="{x:Type local:Routes}" />
    </BlazorWebView.RootComponents>
</BlazorWebView>
```

```csharp
// MauiProgram.cs
builder.Services.AddMauiBlazorWebView();
builder.Services.AddSingleton<IGeolocationService, MauiGeolocationService>();
```

## Platform abstraction

```csharp
public interface IGeolocationService
{
    Task<(double Lat, double Lon)?> GetLocationAsync(CancellationToken ct);
}

public sealed class MauiGeolocationService : IGeolocationService
{
    public async Task<(double Lat, double Lon)?> GetLocationAsync(CancellationToken ct)
    {
        var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(), ct);
        return location is null ? null : (location.Latitude, location.Longitude);
    }
}
```

The web host registers a different implementation (for example, one that uses the browser
Geolocation API through JS interop). The shared component only knows `IGeolocationService`.

## The `maui-blazor-web` template

`dotnet new maui-blazor-web` creates a solution with a MAUI Hybrid app, a Blazor Web App and a
shared Razor class library already wired together — a quick start for apps that target web,
desktop and mobile from one set of components.
