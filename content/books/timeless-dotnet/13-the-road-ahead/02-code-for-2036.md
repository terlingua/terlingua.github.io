---
title: Writing code that still works well in 2036
questions:
  - id: bcl-first
    prompt: Which dependency strategy most improves a codebase's chance of aging well?
    options:
      - text: Adopt the most popular package for every concern
      - id: bcl-first
        text: Prefer the BCL and ASP.NET Core abstractions; add third-party packages deliberately, few in number, with licence and maintenance reviewed
        correct: true
      - text: Write everything yourself, including a JSON parser
      - text: Freeze all package versions forever
    explanation: The platform's abstractions (`ILogger`, `IOptions`, `TimeProvider`, `HttpClient`, `System.Text.Json`, `DbDataSource`) have long compatibility promises.
  - id: aot-ready
    prompt: Why write code that is trimming- and Native AOT-compatible even if you do not publish AOT today?
    type: multiple
    options:
      - id: options
        text: It keeps deployment options open (containers, serverless, fast startup) as the ecosystem moves that way
        correct: true
      - id: explicit
        text: The constraints push toward explicit, statically analyzable code — source generators instead of runtime reflection
        correct: true
      - id: warnings
        text: The analyzers (`IsAotCompatible`, trim warnings) catch fragile reflection early
        correct: true
      - text: AOT makes every application faster at steady state
    explanation: AOT mainly improves startup and memory; JIT can be faster at steady state due to dynamic PGO. The design benefits apply regardless.
  - id: standards
    prompt: Which choices bet on open standards rather than products?
    type: multiple
    options:
      - id: otel
        text: OpenTelemetry for telemetry
        correct: true
      - id: problem-details
        text: RFC 9457 problem details for API errors
        correct: true
      - id: openapi
        text: OpenAPI documents for HTTP contracts
        correct: true
      - text: A proprietary logging format invented by the team
    explanation: Standards outlive vendors. ASP.NET Core 10 generates OpenAPI 3.1 documents in the box.
  - id: language-features
    prompt: How should a team adopt new language features over a decade?
    options:
      - text: Never; use only C# 7 features for stability
      - text: Rewrite the whole codebase every November
      - id: purposeful
        text: Adopt features that make intent clearer or remove whole classes of bugs (nullable, records, pattern matching, unions); skip ones that are merely clever; let analyzers and code fixes modernize consistently
        correct: true
      - text: Use preview features in production for competitive advantage
    explanation: Consistency matters more than novelty. `dotnet format` and IDE code fixes can upgrade patterns across a solution in one reviewed commit.
  - id: tests-as-enabler
    prompt: What single investment most reduces the cost of every future upgrade?
    options:
      - id: tests
        text: A fast, trustworthy, behavior-focused automated test suite
        correct: true
      - text: Detailed comments on every method
      - text: A large architecture diagram
      - text: A dedicated upgrade team
    explanation: Tests turn upgrades and refactorings from risky projects into routine changes.
  - id: decision-records
    prompt: Why record *why* a decision was made (an ADR), not just what was decided?
    options:
      - text: Auditors require it in all industries
      - id: future-vestige
        text: So future developers can tell whether the reason still holds — the only reliable way to recognize a vestige later
        correct: true
      - text: To prevent anyone changing it
      - text: ADRs replace code review
    explanation: Every decision eventually outlives its context. A written reason lets your successors retire it without fear — or keep it with confidence.
  - id: future-vestiges
    prompt: Which of today's habits are plausible candidates to become vestiges by 2036?
    type: multiple
    options:
      - id: oneof
        text: Third-party discriminated-union libraries, as language unions mature
        correct: true
      - id: abstract-record-idiom
        text: The private-constructor abstract-record idiom for closed sets, as `closed` and `union` arrive
        correct: true
      - id: manual-di-scan
        text: Reflection-based assembly scanning, as source-generated registration and AOT spread
        correct: true
      - text: Parameterizing SQL
    explanation: Parameterization addresses a permanent problem (code/data confusion). The others are workarounds for missing language or platform features.
---
## Learning objectives

- Apply concrete rules for longevity to dependencies, language features and architecture
- Anticipate which of today's practices will become tomorrow's vestiges
- Leave a trail of reasons for future maintainers

## Twelve rules for code that ages well

1. **Stay on supported runtimes as a routine.** Upgrade every LTS at minimum; automate it.
2. **Prefer the platform.** BCL and ASP.NET Core abstractions first; third-party packages second.
3. **Keep dependencies few, and review their licences and funding.**
4. **Be explicit.** Explicit mapping, explicit registration, explicit SQL. Let generators type
   the boilerplate.
5. **Stay AOT- and trim-compatible** in libraries and new services (`<IsAotCompatible>true</IsAotCompatible>`).
6. **Encode intent in types.** Nullable on, records for data, strongly-typed ids, closed sets
   (abstract records today, unions tomorrow).
7. **Push I/O to the edges.** Pure decision logic in the core.
8. **Make async and cancellation consistent.** Async all the way, tokens all the way, with
   deliberate exceptions at points of no return.
9. **Bet on standards.** OpenTelemetry, problem details, OpenAPI, OAuth/OIDC, SQL.
10. **Treat SQL and schema as code.** Versioned, reviewed, tested, migrated automatically.
11. **Test behavior** through public seams, against real databases where it matters.
12. **Write down why.** ADRs and commit messages that record reasons.

## A small, timeless service

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddValidation();
builder.Services.AddOpenApi();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<DbDataSource>(_ =>
    SqlClientFactory.Instance.CreateDataSource(builder.Configuration.GetConnectionString("Shop")!));
builder.Services.AddScoped<ProductQueries>();
builder.Services.AddOpenTelemetry()
    .WithTracing(t => t.AddAspNetCoreInstrumentation().AddOtlpExporter())
    .WithMetrics(m => m.AddAspNetCoreInstrumentation().AddOtlpExporter());

var app = builder.Build();
app.UseExceptionHandler();
app.MapOpenApi();

app.MapGet("/products/{sku}", async (string sku, ProductQueries products, CancellationToken ct) =>
    await products.FindAsync(sku, ct) is { } product
        ? Results.Ok(product.ToResponse())
        : Results.NotFound());

app.Run();

public partial class Program;
```

Every line uses a platform abstraction or an open standard. Nothing here depends on a
convention engine, a container plug-in or a runtime mapper. In ten years, most of it will look
slightly old-fashioned and all of it will still compile.

## Looking past .NET 11

What can be said with reasonable confidence:

- **.NET 12** is expected in November 2027 as the next LTS, by the established cadence.
- The language design repository (`dotnet/csharplang`) continues to discuss extensions of the
  union and closed-type work (for example closed enums), more pattern-matching power and further
  memory-safety rules. Treat proposals as proposals until they ship.
- Runtime work trends toward lower overhead async, better Native AOT, more hardware intrinsics
  and smaller deployments.

What cannot be predicted: which libraries, cloud services and fashions will matter. That is
exactly why the rules above lean on principles and standards rather than products.

## Sources

- .NET support policy — <https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core>
- dotnet/csharplang — <https://github.com/dotnet/csharplang>
- Microsoft Learn, "Native AOT deployment" — <https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/>
- Microsoft Learn, "Generate OpenAPI documents" — <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi>
- Michael Nygard, "Documenting Architecture Decisions" — <https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions>
