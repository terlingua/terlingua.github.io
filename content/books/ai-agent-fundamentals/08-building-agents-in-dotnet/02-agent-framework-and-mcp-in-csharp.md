---
title: Agent Framework and MCP in C#
questions:
  - id: maf
    prompt: What is Microsoft Agent Framework?
    options:
      - text: A JavaScript UI library
      - id: maf
        text: Microsoft's open-source framework for building agents and multi-agent workflows in .NET and Python, combining ideas from Semantic Kernel and AutoGen; version 1.0 shipped in April 2026
        correct: true
      - text: A Windows service for scheduling tasks
      - text: A replacement for the .NET runtime
    explanation: Microsoft positioned it as the successor for new agent work; AutoGen moved to maintenance, and Semantic Kernel remained supported.
  - id: maf-mea
    prompt: How does Agent Framework relate to Microsoft.Extensions.AI?
    options:
      - text: They are unrelated
      - text: Agent Framework replaces `IChatClient`
      - id: builds-on
        text: Agent Framework builds on `IChatClient` — any provider with an `IChatClient` implementation can back an agent, and tools are `AIFunction`s
        correct: true
      - text: Microsoft.Extensions.AI depends on Agent Framework
    explanation: The abstraction layer is the long-lived part; frameworks on top come and go.
  - id: maf-features
    prompt: Which capabilities were reported as stable in Agent Framework 1.0?
    type: multiple
    options:
      - id: agents
        text: The core agent abstraction with provider connectors
        correct: true
      - id: workflows
        text: Graph-based workflows
        correct: true
      - id: orchestrations
        text: Orchestration patterns such as sequential, concurrent, hand-off and group chat
        correct: true
      - id: middleware
        text: Middleware hooks and memory/context providers
        correct: true
    explanation: Several integrations (for example a browser-based debugging UI and some hosted features) remained in preview at 1.0.
  - id: version-churn
    prompt: Method and type names in agent frameworks changed between previews, release candidates and 1.0. What is the timeless lesson?
    options:
      - text: Avoid all frameworks forever
      - text: Only use preview releases
      - id: isolate
        text: Keep framework types at the edges — composition root and adapters — so domain logic, tools and tests depend on stable abstractions and are unaffected by renames
        correct: true
      - text: Pin to the first preview you tried
    explanation: The same principle protected codebases through ASP.NET and EF Core rewrites.
  - id: mcp-sdk
    prompt: What is the official MCP C# SDK?
    options:
      - id: sdk
        text: The `ModelContextProtocol` NuGet packages, maintained with Microsoft under the MCP organisation, for building MCP servers and clients in .NET (with an ASP.NET Core package for HTTP servers)
        correct: true
      - text: A Databricks-only library
      - text: A Visual Studio extension
      - text: A COM component
    explanation: It reached 1.0 alongside the stabilising MCP specification.
  - id: mcp-server-attrs
    prompt: How do you expose a C# method as an MCP tool with the SDK?
    options:
      - text: Write a JSON file by hand
      - text: Inherit from `McpBase`
      - id: attributes
        text: Mark the class with `[McpServerToolType]` and the method with `[McpServerTool]` plus `[Description]`, then register with `AddMcpServer()…WithToolsFromAssembly()`
        correct: true
      - text: Use reflection at runtime with no registration
    explanation: Parameters like `CancellationToken` and DI services are injected; the rest become the tool's JSON schema.
  - id: mcp-client-tools
    prompt: How do MCP tools from a server reach an `IChatClient` agent in .NET?
    options:
      - text: They cannot
      - text: By copying the server's source code
      - text: Through a REST proxy you must write
      - id: aifunction
        text: The client lists the server's tools; each MCP client tool is an `AIFunction`, so they can be passed directly in `ChatOptions.Tools` or to an agent
        correct: true
    explanation: That is the M+N benefit of a protocol plus a shared abstraction.
  - id: stdio-risk
    prompt: What is the risk of launching third-party MCP servers over stdio from a .NET app?
    options:
      - id: process
        text: They run as local processes with your application's privileges, so a malicious server can read files, environment variables and network resources
        correct: true
      - text: They are slower than HTTP
      - text: They cannot return results
      - text: None
    explanation: Vet and pin servers, run them in containers or sandboxes, and pass only the environment they need.
  - id: http-server
    prompt: How do you host an MCP server over HTTP in ASP.NET Core?
    options:
      - text: It is impossible
      - id: map
        text: Use the ASP.NET Core package, register with `WithHttpTransport()` and map the endpoint with `app.MapMcp()`, protecting it with normal ASP.NET Core authentication and authorisation
        correct: true
      - text: Through WCF
      - text: With SignalR only
    explanation: Remote MCP servers are web APIs — apply the same security discipline.
---
## Learning objectives

- Build agents with Microsoft Agent Framework on top of `IChatClient`
- Expose and consume MCP tools in C#
- Isolate framework churn

## An agent with Agent Framework

```csharp
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

AIAgent agent = new ChatClientAgent(
    chat,
    name: "OrderAssistant",
    instructions: "Answer questions about orders. Use tools. If unsure, say so and offer escalation.",
    tools: [AIFunctionFactory.Create(OrderTools.GetStatus)]);

var result = await agent.RunAsync("Where is order A-1042?");
Console.WriteLine(result);
```

API names in this area moved between previews and 1.x — confirm against the current docs for your
package version.

## An MCP server

```csharp
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly();
await builder.Build().RunAsync();

[McpServerToolType]
public static class OrderMcpTools
{
    [McpServerTool, Description("Gets status, carrier and ETA for an order ID in the format A-1234.")]
    public static async Task<OrderStatus> GetOrderStatus(
        IOrderRepository orders,
        [Description("Order ID, e.g. A-1042")] string orderId,
        CancellationToken cancellationToken) =>
        await orders.GetStatusAsync(OrderId.Parse(orderId), cancellationToken);
}
```

## An MCP client feeding an `IChatClient`

```csharp
using ModelContextProtocol.Client;

await using var mcp = await McpClient.CreateAsync(new StdioClientTransport(new()
{
    Name = "orders",
    Command = "dotnet",
    Arguments = ["run", "--project", "src/Orders.Mcp"],
}));

var tools = await mcp.ListToolsAsync();
var response = await chat.GetResponseAsync("Where is order A-1042?", new ChatOptions { Tools = [.. tools] });
```

## Sources

- Visual Studio Magazine, "Microsoft Ships Production-Ready Agent Framework 1.0 for .NET and Python", April 2026 — <https://visualstudiomagazine.com/articles/2026/04/06/microsoft-ships-production-ready-agent-framework-1-0-for-net-and-python.aspx>
- Microsoft Agent Framework — <https://github.com/microsoft/agent-framework>
- MCP C# SDK — <https://github.com/modelcontextprotocol/csharp-sdk>
- Microsoft Learn, "Build an MCP server in .NET" — <https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-mcp-server>
