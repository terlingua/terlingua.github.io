---
title: What is Blazor WebAssembly?
questions:
  - id: runtime
    prompt: Where does a Blazor WebAssembly app's C# code run?
    options:
      - text: On the web server, with UI updates sent over a socket
      - id: browser
        text: In the browser, on a .NET runtime compiled to WebAssembly
        correct: true
      - text: In a browser plugin that must be installed separately
    explanation: Blazor WebAssembly downloads the .NET runtime and your assemblies, then runs them directly in the browser's WebAssembly sandbox.
  - id: static-hosting
    prompt: Which statements about hosting a standalone Blazor WebAssembly app are true?
    type: multiple
    options:
      - text: It can be served from a static file host such as GitHub Pages
        correct: true
      - text: It requires ASP.NET Core on the server
      - text: The browser downloads the app's assemblies before it starts
        correct: true
      - text: Every click makes a round trip to the server
    explanation: A standalone app is just static files. Once downloaded, it runs entirely on the client.
---
Blazor is a .NET framework for building interactive web UIs with C# instead of JavaScript.
The **WebAssembly** hosting model runs your app directly in the browser.

## How it works

When a visitor opens the site, the browser downloads:

1. A small JavaScript loader, `blazor.webassembly.js`
2. The .NET runtime, compiled to WebAssembly
3. Your app's assemblies and their dependencies

From then on, your C# code executes on the client. There is no server process rendering pages.

## Why that matters

Because the output is plain static files, you can host the app anywhere that serves files —
including GitHub Pages, which is exactly how this site is deployed.

> The trade-off is a larger first download. After that, navigation is instant.
