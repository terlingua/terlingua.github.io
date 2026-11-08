---
title: Publishing, containers and Native AOT
questions:
  - id: container
    prompt: How can you build a container image for a .NET app without writing a Dockerfile?
    options:
      - text: It is not possible
      - text: "`dotnet build --docker`"
      - id: publish-container
        text: "`dotnet publish -t:PublishContainer`"
        correct: true
      - text: "`dotnet new docker`"
    explanation: The SDK has built-in container support and can push to a registry or load into a local daemon.
  - id: aot-tradeoff
    prompt: What is the main trade-off of Native AOT?
    options:
      - text: Smaller binaries but much slower steady-state performance than JIT in every case
      - id: tradeoff
        text: Much faster startup and lower memory, at the cost of restrictions on unbounded reflection and runtime code generation
        correct: true
      - text: It only works on Windows
    explanation: AOT compiles ahead of time and trims unused code, so features that discover code at runtime need source generators or annotations instead.
  - id: aot-json
    prompt: Why does a Native AOT ASP.NET Core app typically declare a `JsonSerializerContext`?
    options:
      - text: To encrypt JSON payloads
      - id: source-gen
        text: To use source-generated serialization metadata instead of runtime reflection
        correct: true
      - text: To enable XML output
    explanation: Reflection-based serialization is not trim- or AOT-safe; the source generator produces the metadata at compile time.
  - id: options
    prompt: Which publish options exist in modern .NET?
    type: multiple
    options:
      - text: Framework-dependent deployment, which relies on an installed runtime
        correct: true
      - text: Self-contained deployment, which bundles the runtime
        correct: true
      - text: Single-file publishing
        correct: true
      - text: ReadyToRun, which precompiles code to improve startup while keeping the JIT
        correct: true
    explanation: These options can be combined to balance size, startup time and compatibility.
---
## Learning objectives

- Choose between framework-dependent, self-contained, single-file and ReadyToRun
- Build container images with the SDK
- Decide when Native AOT is worth it

## Publish options

| Option | Property | Effect |
|--------|----------|--------|
| Framework-dependent | default | Small output; needs the runtime installed |
| Self-contained | `--self-contained` | Bundles the runtime for a specific RID |
| Single file | `PublishSingleFile` | One executable |
| Trimmed | `PublishTrimmed` | Removes unused code (self-contained only) |
| ReadyToRun | `PublishReadyToRun` | Precompiled code for faster startup, JIT still available |
| Native AOT | `PublishAot` | Fully native executable, no JIT |

```bash
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true
```

## Containers without a Dockerfile

```xml
<PropertyGroup>
  <ContainerRepository>shop-api</ContainerRepository>
  <ContainerImageTags>1.4.0;latest</ContainerImageTags>
</PropertyGroup>
```

```bash
dotnet publish -c Release -t:PublishContainer
docker run -p 8080:8080 shop-api:1.4.0
```

Since .NET 8, official images run as a non-root `app` user and ASP.NET Core listens on port 8080
by default. `ContainerFamily` can select smaller variants such as chiseled Ubuntu images.

## Native AOT

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
  <InvariantGlobalization>true</InvariantGlobalization>
</PropertyGroup>
```

```csharp
var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default));

var app = builder.Build();
app.MapGet("/health", () => new HealthStatus("ok", DateTimeOffset.UtcNow));
app.Run();

public sealed record HealthStatus(string Status, DateTimeOffset At);

[JsonSerializable(typeof(HealthStatus))]
internal partial class AppJsonContext : JsonSerializerContext;
```

AOT apps start in milliseconds and use less memory — ideal for serverless functions, CLI tools and
dense container hosting. Libraries that rely on runtime reflection (some ORMs, older serializers,
dynamic proxies) may not be compatible; the build emits trim/AOT warnings that you should treat as
errors.

## Checklist before production

- Health checks (`AddHealthChecks`, `MapHealthChecks("/healthz")`)
- Structured logging and OpenTelemetry exporters configured
- Configuration validated at startup
- Secrets from a vault or managed identity, never from the image
- Graceful shutdown honoured (`stoppingToken`, request cancellation)
