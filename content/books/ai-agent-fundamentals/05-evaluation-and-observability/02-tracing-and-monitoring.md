---
title: Tracing and monitoring
questions:
  - id: trace
    prompt: What does a trace of an agent run contain?
    options:
      - id: spans
        text: A tree of spans — each LLM call, tool call and retrieval — with inputs, outputs, timings, token counts and errors
        correct: true
      - text: Only the final answer
      - text: The model's weights
      - text: The user's password
    explanation: Traces are to agents what stack traces and logs are to ordinary software.
  - id: otel
    prompt: Why does OpenTelemetry matter for agent observability?
    options:
      - text: It is required by law
      - text: It makes models faster
      - id: standard
        text: It is a vendor-neutral standard with GenAI semantic conventions, so traces from different frameworks and languages can flow to any compatible backend
        correct: true
      - text: It replaces evaluation
    explanation: MLflow, Microsoft.Extensions.AI and Agent Framework can emit OpenTelemetry data.
  - id: metrics
    prompt: Which production metrics should an agent dashboard show?
    type: multiple
    options:
      - id: success
        text: Task success and escalation rates
        correct: true
      - id: cost
        text: Cost and tokens per task
        correct: true
      - id: latency
        text: Latency distribution (p50, p95)
        correct: true
      - id: tool-errors
        text: Tool error rates and step counts
        correct: true
    explanation: Alert on regressions after deployments and model changes.
  - id: pii-traces
    prompt: What risk do traces introduce?
    options:
      - text: None
      - id: data
        text: They contain prompts, documents and tool outputs that may include personal or confidential data, so they need access control, retention limits and redaction
        correct: true
      - text: They slow agents down by 10×
      - text: They expose model weights
    explanation: Observability data is production data.
  - id: online-judges
    prompt: Why run LLM judges on a sample of production traces?
    options:
      - text: To replace offline evaluation
      - text: To train the model
      - text: To bill customers
      - id: drift
        text: To detect quality drift and new failure types on real traffic between offline evaluation runs
        correct: true
    explanation: Sample to control cost.
  - id: replay
    prompt: How do traces help reproduce a bug?
    options:
      - id: replay
        text: They record exact inputs and tool results, so the failing step can be replayed or turned into a test case
        correct: true
      - text: They cannot help
      - text: They roll back the database
      - text: They retrain the model
    explanation: Non-determinism means a replay may not fail the same way; capture enough context to analyse it.
  - id: cost-anomaly
    prompt: One request consumed 2 million tokens. What do traces typically reveal in such cases?
    options:
      - text: A billing error
      - text: A slow network
      - id: loop
        text: A loop — repeated tool calls or ever-growing context — that budgets and loop detection should have stopped
        correct: true
      - text: A model upgrade
    explanation: Per-request token budgets prevent single runaway runs from becoming bills.
---
## Learning objectives

- Trace agents with spans and OpenTelemetry
- Monitor quality, cost and latency in production
- Protect sensitive data in traces

## Tracing an agent with MLflow

```python
import mlflow
mlflow.openai.autolog()

@mlflow.trace(span_type="TOOL")
def lookup_order(order_id: str) -> dict:
    return orders_api.get(order_id)

@mlflow.trace(span_type="AGENT")
def run_agent(question: str) -> str:
    ...
```

## Sources

- OpenTelemetry, "Semantic conventions for generative AI systems" — <https://opentelemetry.io/docs/specs/semconv/gen-ai/>
- MLflow, "Tracing" — <https://mlflow.org/docs/latest/genai/tracing/>
- Microsoft Learn, "Microsoft.Extensions.AI telemetry" — <https://learn.microsoft.com/en-us/dotnet/ai/microsoft-extensions-ai>
