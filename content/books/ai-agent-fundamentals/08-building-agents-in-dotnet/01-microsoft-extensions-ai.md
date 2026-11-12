---
title: Microsoft.Extensions.AI
questions:
  - id: what
    prompt: What is Microsoft.Extensions.AI?
    options:
      - text: A hosted model from Microsoft
      - id: abstractions
        text: A set of .NET libraries providing provider-neutral abstractions — `IChatClient`, `IEmbeddingGenerator<TInput, TEmbedding>` and tool types — plus middleware such as function invocation, caching and telemetry
        correct: true
      - text: A replacement for ASP.NET Core
      - text: A Python package
    explanation: It plays the role for AI that `ILogger` plays for logging — application code depends on the abstraction; providers plug in.
  - id: why-abstraction
    prompt: Why depend on `IChatClient` rather than a vendor SDK throughout your code?
    type: multiple
    options:
      - id: swap
        text: Providers can be swapped by changing composition, not business code
        correct: true
      - id: middleware
        text: Cross-cutting concerns (telemetry, caching, retries, function invocation) compose as middleware
        correct: true
      - id: test
        text: Code can be unit-tested with a fake `IChatClient`
        correct: true
      - text: It makes every model produce identical output
    explanation: The same reasoning as the Dependency Inversion Principle.
  - id: function-invocation
    prompt: What does `UseFunctionInvocation()` add to a chat client pipeline?
    options:
      - text: It lets the model run arbitrary code
      - text: It compiles C# at runtime
      - id: loop
        text: Middleware that, when the model requests a tool call, invokes the matching `AIFunction`, appends the result and calls the model again — the basic agent loop
        correct: true
      - text: It validates JSON schemas only
    explanation: Configure limits such as maximum iterations; it is still your job to validate arguments inside tools.
  - id: aifunctionfactory
    prompt: What does `AIFunctionFactory.Create` do?
    options:
      - id: create
        text: Wraps a .NET method or delegate as an `AIFunction`, generating a JSON schema for its parameters from the signature and `[Description]` attributes
        correct: true
      - text: Calls a model to write a function
      - text: Deploys a function to Azure
      - text: Creates a SQL function
    explanation: Parameters such as `CancellationToken` and services can be supplied by the runtime rather than the model.
  - id: databricks-compat
    prompt: How can a .NET application call a Databricks model serving endpoint through `IChatClient`?
    options:
      - text: It cannot
      - text: Only through Python interop
      - text: Only with a Databricks-specific .NET SDK
      - id: openai-compat
        text: Databricks serving endpoints expose an OpenAI-compatible API, so the OpenAI .NET client pointed at `{workspace}/serving-endpoints` can be adapted with `AsIChatClient()`
        correct: true
    explanation: Protocol compatibility is what makes the abstraction portable across vendors.
  - id: structured
    prompt: How do you get a typed result from `IChatClient`?
    options:
      - text: Parse `response.Text` with regular expressions
      - id: generic
        text: Use the `GetResponseAsync<T>` extension, which requests structured output matching `T`'s schema and deserialises it — then validate the values
        correct: true
      - text: Set temperature to 0
      - text: It is not possible in .NET
    explanation: Schema conformance is not correctness; business validation still applies.
  - id: telemetry
    prompt: What does `UseOpenTelemetry()` provide?
    options:
      - text: Faster responses
      - text: Free tokens
      - id: otel
        text: Traces and metrics following OpenTelemetry's GenAI semantic conventions (model, tokens, duration), exportable to any OpenTelemetry backend
        correct: true
      - text: Automatic evaluation
    explanation: Decide deliberately whether to record message content — it may contain sensitive data.
  - id: evaluation-libs
    prompt: Which Microsoft libraries support evaluating AI responses in .NET tests?
    options:
      - id: eval
        text: The Microsoft.Extensions.AI.Evaluation libraries, offering quality and safety evaluators and reporting that can run inside ordinary test frameworks
        correct: true
      - text: There are none
      - text: Only Python libraries
      - text: Visual Studio's spell checker
    explanation: Running evaluations as tests on Microsoft.Testing.Platform keeps them in the normal CI pipeline.
  - id: embeddings
    prompt: Which abstraction generates embeddings for RAG in .NET?
    options:
      - text: "`IChatClient`"
      - text: "`ILogger`"
      - id: generator
        text: "`IEmbeddingGenerator<string, Embedding<float>>`"
        correct: true
      - text: "`IMemoryCache`"
    explanation: Microsoft.Extensions.VectorData provides matching abstractions over vector stores.
---
## Learning objectives

- Use `IChatClient` and middleware to build a minimal agent loop
- Define tools with `AIFunctionFactory`
- Add telemetry and structured output

## A minimal tool-using agent

```csharp
using System.ClientModel;
using System.ComponentModel;
using Microsoft.Extensions.AI;
using OpenAI;

var openAi = new OpenAIClient(
    new ApiKeyCredential(Environment.GetEnvironmentVariable("DATABRICKS_TOKEN")!),
    new OpenAIClientOptions { Endpoint = new Uri($"{workspaceUrl}/serving-endpoints") });

IChatClient chat = openAi.GetChatClient(chatEndpointName)
    .AsIChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .UseOpenTelemetry(sourceName: "Terlingua.Agents")
    .Build();

ChatOptions options = new() { Tools = [AIFunctionFactory.Create(OrderTools.GetStatus)] };

ChatResponse response = await chat.GetResponseAsync(
    [new(ChatRole.System, "You answer questions about orders. Use tools; never guess."),
     new(ChatRole.User, "Where is order A-1042?")],
    options);

Console.WriteLine(response.Text);

public static class OrderTools
{
    [Description("Gets status, carrier and ETA for an order ID in the format A-1234.")]
    public static OrderStatus GetStatus([Description("Order ID, e.g. A-1042")] string orderId) =>
        OrderId.TryParse(orderId, out var id)
            ? OrderRepository.Find(id)
            : throw new ArgumentException("Order IDs look like A-1234.", nameof(orderId));
}

public sealed record OrderStatus(string Status, string Carrier, DateOnly Eta);
```

## With dependency injection

```csharp
builder.Services.AddChatClient(sp => sp.GetRequiredService<OpenAIClient>()
        .GetChatClient(builder.Configuration["Ai:ChatModel"]!)
        .AsIChatClient())
    .UseFunctionInvocation()
    .UseOpenTelemetry();
```

## Sources

- Microsoft Learn, "Microsoft.Extensions.AI libraries" — <https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai>
- Microsoft Learn, "Microsoft.Extensions.AI.Evaluation" — <https://learn.microsoft.com/en-us/dotnet/ai/conceptual/evaluation-libraries>
- Databricks docs, "Query chat models with OpenAI clients" — <https://docs.databricks.com/aws/en/machine-learning/model-serving/query-chat-models>
