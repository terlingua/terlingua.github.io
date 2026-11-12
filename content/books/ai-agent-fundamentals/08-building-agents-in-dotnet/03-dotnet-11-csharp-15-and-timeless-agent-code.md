---
title: .NET 11, C# 15 and agent code that lasts
questions:
  - id: dotnet11
    prompt: What kind of release is .NET 11, launched at .NET Conf in November 2026?
    options:
      - text: Long Term Support, five years
      - id: sts
        text: Standard Term Support (24 months); .NET 10 remains the current LTS and .NET 12 is the next LTS
        correct: true
      - text: A preview only
      - text: Windows-only
    explanation: Agent libraries target `net8.0`/`net10.0` and run on 11; adopt 11 for its runtime and library gains, plan LTS upgrades around 12.
  - id: union-outcome
    prompt: How do C# 15 union types help model an agent's possible outcomes?
    options:
      - text: They make agents faster
      - text: They replace JSON
      - id: exhaustive
        text: "`union AgentOutcome(Answered, NeedsApproval, Escalated)` lets the compiler check that every outcome is handled; adding a new case produces warnings at every incomplete `switch`"
        correct: true
      - text: They are only for numeric types
    explanation: Agents have a closed set of terminal states; make the type system enforce handling all of them.
  - id: closed
    prompt: When would you choose a C# 15 `closed` record hierarchy over a `union` for agent steps?
    options:
      - id: shared-base
        text: When the cases share a common base with shared members (e.g. every step has a timestamp and trace ID); choose `union` when the cases are unrelated types
        correct: true
      - text: Never; `closed` is deprecated
      - text: Only for structs
      - text: When you want inheritance from other assemblies
    explanation: Both give exhaustive switches; the choice is about modelling.
  - id: runtime-async
    prompt: What does .NET 11's runtime async bring to agent code, which is mostly asynchronous I/O?
    options:
      - text: Synchronous execution of all awaits
      - text: A new `async2` keyword developers must adopt
      - id: lower-overhead
        text: Async methods implemented by the runtime itself, with lower overhead and better stack traces, without source changes
        correct: true
      - text: Removal of `Task`
    explanation: Timeless async code — `async` all the way, `CancellationToken` on every I/O path — benefits automatically.
  - id: timeless-principles
    prompt: Which practices make .NET agent code likely to work well in 2036?
    type: multiple
    options:
      - id: abstractions
        text: Depend on `IChatClient`/`AIFunction` and protocols (MCP, OpenTelemetry), not on one vendor's SDK types
        correct: true
      - id: deterministic-core
        text: Keep permissions, money, state and validation in ordinary, unit-tested C#; let the model choose among safe tools
        correct: true
      - id: config
        text: Keep model names, endpoints and limits in configuration
        correct: true
      - id: evals-in-ci
        text: Run evaluation suites as tests in CI on every model, prompt or tool change
        correct: true
    explanation: These are the agent-specific forms of the principles in this site's *Timeless .NET* book.
  - id: cancellation
    prompt: Why must every tool and model call accept a `CancellationToken`?
    options:
      - text: The compiler requires it
      - id: budgets
        text: Agent runs have time budgets and users cancel; without propagation, abandoned runs keep calling models and tools and spending money
        correct: true
      - text: It makes calls cheaper per token
      - text: For logging only
    explanation: Combine with `TimeProvider` for testable timeouts.
  - id: records-messages
    prompt: Why model tool inputs and outputs as small immutable records?
    options:
      - text: Records are faster than classes for every purpose
      - text: Models require records
      - text: It avoids JSON
      - id: contracts
        text: They make tool contracts explicit, serialisable with System.Text.Json, easy to validate and test, and stable as schemas the model sees
        correct: true
    explanation: The schema the model sees is part of your public API — version it deliberately.
  - id: avoid
    prompt: Which habits will age badly in agent code?
    type: multiple
    options:
      - id: prompt-strings
        text: Prompts scattered as string literals across business logic
        correct: true
      - id: vendor-leak
        text: Vendor SDK types leaking into domain and test code
        correct: true
      - id: model-trust
        text: Trusting model output for authorisation or calculations
        correct: true
      - text: Using dependency injection
    explanation: Centralise prompts as versioned assets; isolate vendors; keep a deterministic core.
  - id: testing
    prompt: How should you unit-test an agent's tool logic in .NET?
    options:
      - id: direct
        text: Test tools as ordinary methods with xUnit v3 on Microsoft.Testing.Platform, and test the orchestration with a fake `IChatClient` that returns scripted tool calls; run model-backed evaluations separately
        correct: true
      - text: Only through end-to-end runs against a live model
      - text: Tests are not possible for AI code
      - text: By asking the model to test itself
    explanation: Deterministic tests for deterministic code; statistical evaluations for model behaviour.
  - id: future-skeptic
    prompt: What is the most defensible expectation about the agent ecosystem over the next ten years?
    options:
      - text: Today's leading framework will remain unchanged
      - text: Agents will be abandoned
      - text: Protocols will disappear
      - id: churn
        text: Models and frameworks will churn repeatedly, while protocols, abstractions, evaluation data and well-tested deterministic code retain their value
        correct: true
    explanation: Bet on interfaces and evidence, not on brands.
---
## Learning objectives

- Use C# 15 unions and closed hierarchies to model agent outcomes
- Benefit from .NET 11 without coupling to it
- Apply timeless principles to agent code

## Modelling outcomes with a union

```csharp
public sealed record Answered(string Text, IReadOnlyList<Uri> Sources);
public sealed record NeedsApproval(string Action, string Reason, Guid IdempotencyKey);
public sealed record Escalated(string Reason);

public union AgentOutcome(Answered, NeedsApproval, Escalated);

public static IResult ToHttp(AgentOutcome outcome) => outcome switch
{
    Answered a => Results.Ok(new { a.Text, a.Sources }),
    NeedsApproval n => Results.Accepted(value: new { n.Action, n.Reason, n.IdempotencyKey }),
    Escalated e => Results.Json(new { e.Reason }, statusCode: StatusCodes.Status202Accepted),
};
```

## A deterministic core around the model

```csharp
public sealed class RefundTool(IOrderRepository orders, IRefundPolicy policy, TimeProvider clock)
{
    [Description("Requests a refund for an order. Amounts above the policy limit require human approval.")]
    public async Task<AgentOutcome> RequestRefundAsync(string orderId, decimal amount, CancellationToken cancellationToken)
    {
        var order = await orders.GetAsync(OrderId.Parse(orderId), cancellationToken);
        return policy.Evaluate(order, amount, clock.GetUtcNow()) switch
        {
            RefundDecision.Allowed => new Answered($"Refund of {amount:C} submitted.", []),
            RefundDecision.NeedsApproval => new NeedsApproval("refund", "Above automatic limit", Guid.CreateVersion7()),
            RefundDecision.Denied d => new Escalated(d.Reason),
        };
    }
}
```

The model decides *whether* to request a refund. The policy, in tested C#, decides *what happens*.

## Ten-year checklist for .NET agents

- `IChatClient` and `AIFunction` at the boundary; vendor SDKs only in the composition root
- MCP for tools you share; OpenTelemetry for traces
- Records for tool contracts; unions for outcomes
- `CancellationToken` and `TimeProvider` everywhere
- Evaluation suites in CI; regression cases from production traces
- Model names and limits in configuration
- Target LTS releases for long-lived services; keep code compiling on the current STS

## Sources

- Microsoft Learn, "What's new in C# 15" — <https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-15>
- .NET Blog, "Explore union types in C# 15" — <https://devblogs.microsoft.com/dotnet/csharp-15-union-types/>
- Microsoft Learn, "What's new in .NET 11" — <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-11/overview>
- .NET support policy — <https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core>
- Terlingua, *Timeless .NET* — chapters 12 and 13
