---
title: Practices that outlast models
questions:
  - id: what-lasts
    prompt: Models change every few months. Which assets keep their value for a decade?
    type: multiple
    options:
      - id: evals
        text: Evaluation datasets and success criteria grounded in your domain
        correct: true
      - id: data
        text: Clean, governed, well-documented data and metadata
        correct: true
      - id: abstractions
        text: Clean abstractions between your application and any model provider
        correct: true
      - id: workflow
        text: Redesigned workflows with clear human responsibilities
        correct: true
    explanation: Prompts tuned to one model, and benchmark positions, depreciate fastest.
  - id: bitter-lesson
    prompt: Rich Sutton's "The Bitter Lesson" (2019) argues what?
    options:
      - text: AI will never work
      - id: general-methods
        text: General methods that scale with computation (search and learning) have repeatedly beaten approaches built on hand-crafted human knowledge
        correct: true
      - text: Bitter flavours confuse LLMs
      - text: Rule-based systems are always better
    explanation: For builders, it suggests avoiding elaborate scaffolding that compensates for today's model weaknesses — it may be obsolete with the next model.
  - id: scaffolding
    prompt: You built complex prompt chains to work around a model's weakness. A new model fixes it. What should a good architecture allow?
    options:
      - text: Keeping the workaround forever
      - text: Rewriting the whole application
      - id: remove
        text: Removing or simplifying the workaround, verified by your evaluation suite
        correct: true
      - text: Nothing — architectures should not change
    explanation: Evaluations make simplification safe. Without them, accumulated workarounds become legacy.
  - id: deterministic-core
    prompt: Which design principle keeps systems reliable as models change?
    options:
      - text: Let the model handle everything, including permissions
      - text: Use the newest model for every call
      - text: Avoid code
      - id: core
        text: Keep a deterministic core — permissions, calculations, state transitions, validation — in ordinary code; use models for language understanding and generation at the edges
        correct: true
    explanation: Code you can test stays correct when models change; behaviour delegated to models does not.
  - id: provider-neutral
    prompt: Which approaches reduce provider lock-in for model calls?
    type: multiple
    options:
      - id: standard-api
        text: Standard client abstractions (e.g. OpenAI-compatible APIs, `IChatClient` in .NET)
        correct: true
      - id: config
        text: Model names and parameters in configuration
        correct: true
      - id: gateway
        text: A gateway that can route between providers
        correct: true
      - text: Vendor-specific features spread through every layer of code
    explanation: Use vendor features deliberately behind interfaces when they are worth it.
  - id: open-standards
    prompt: Which standards are likely to matter for long-lived GenAI systems?
    type: multiple
    options:
      - id: otel
        text: OpenTelemetry (including GenAI semantic conventions) for traces and metrics
        correct: true
      - id: mcp
        text: The Model Context Protocol for connecting models to tools and data
        correct: true
      - id: json-schema
        text: JSON Schema for structured outputs and tool definitions
        correct: true
      - text: Proprietary prompt formats unique to one vendor
    explanation: Standards outlive products; build on them where possible.
  - id: skills
    prompt: Which human skills grow more valuable as GenAI improves?
    options:
      - id: judgment
        text: Problem framing, domain expertise, evaluation design, and judgment about when outputs are wrong
        correct: true
      - text: Memorising prompt tricks
      - text: Typing speed
      - text: None; AI replaces all skills
    explanation: The jagged frontier means humans must know where the AI is unreliable.
  - id: forward
    prompt: Which forecasts about GenAI are the most defensible?
    options:
      - text: Specific capability dates announced by vendors
      - text: That progress will stop completely
      - id: defensible
        text: That costs per unit of capability keep falling, that models keep changing, and that evaluation, data quality and governance remain the bottlenecks
        correct: true
      - text: That one model will dominate forever
    explanation: Plan for change rather than for a particular model.
---
## Learning objectives

- Identify durable investments in GenAI
- Design systems that get simpler as models improve
- Prefer open standards and deterministic cores

## Ten-year rules

1. Own your evaluation sets; they are your specification.
2. Keep data clean, governed and described.
3. Put permissions, money and state in code, not in prompts.
4. Wrap model calls behind an interface; keep model names in configuration.
5. Log traces in open formats (OpenTelemetry).
6. Prefer protocols (MCP, JSON Schema) to proprietary plugins.
7. Remove workarounds when evaluations say you can.
8. Budget for model migrations every year.
9. Keep humans accountable for consequential decisions.
10. Distrust any claim — including this book's — that you have not measured.

## Sources

- Rich Sutton, "The Bitter Lesson", 2019 — <http://www.incompleteideas.net/IncIdeas/BitterLesson.html>
- OpenTelemetry, "Semantic conventions for generative AI" — <https://opentelemetry.io/docs/specs/semconv/gen-ai/>
- Model Context Protocol — <https://modelcontextprotocol.io/>
- Microsoft.Extensions.AI — <https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai>
