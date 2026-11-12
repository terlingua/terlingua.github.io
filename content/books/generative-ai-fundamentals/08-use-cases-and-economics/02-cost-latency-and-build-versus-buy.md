---
title: Cost, latency and build versus buy
questions:
  - id: unit-econ
    prompt: An assistant uses 4,000 input and 500 output tokens per request at $3 and $15 per million tokens. What is the model cost per request?
    options:
      - text: $0.0045
      - id: correct
        text: About $0.0195
        correct: true
      - text: $0.195
      - text: $19.50
    explanation: 4,000 × $3/1M = $0.012; 500 × $15/1M = $0.0075; total ≈ $0.0195. At a million requests a month that is about $19,500.
  - id: cost-drivers
    prompt: Which design choices most reduce GenAI running cost?
    type: multiple
    options:
      - id: smaller
        text: Using the smallest model that passes evaluation, or routing
        correct: true
      - id: context
        text: Sending less context — better retrieval rather than stuffing
        correct: true
      - id: cache
        text: Prompt caching and result caching
        correct: true
      - id: batch
        text: Batch APIs for non-urgent workloads
        correct: true
    explanation: Each can cut cost substantially; combined, often by an order of magnitude.
  - id: price-trend
    prompt: What has happened to the price of a given level of LLM capability since 2023?
    options:
      - text: It has risen sharply
      - text: It has stayed flat
      - id: fallen
        text: It has fallen dramatically — analyses estimate declines of roughly an order of magnitude or more per year for equivalent capability
        correct: true
      - text: It is impossible to measure
    explanation: Epoch AI and a16z have tracked this. Frontier models still command premium prices; and reasoning models use more tokens per answer.
  - id: reasoning-cost
    prompt: Why can a reasoning model cost more per answer than its per-token price suggests?
    options:
      - text: It charges a subscription
      - id: hidden-tokens
        text: It generates many reasoning tokens (billed as output) before the visible answer
        correct: true
      - text: It cannot be cached
      - text: It has a smaller context window
    explanation: Measure cost per completed task, not per token.
  - id: agents-cost
    prompt: Why do agentic workflows multiply cost?
    options:
      - text: Agents use more expensive GPUs
      - text: They do not; agents are cheaper
      - text: Agents pay licence fees per tool
      - id: loops
        text: Each step re-sends growing context to the model, tools add calls, and loops or retries compound — a task can take dozens of model calls
        correct: true
    explanation: Set budgets and step limits for agents.
  - id: latency-ux
    prompt: Which techniques improve user-perceived latency?
    type: multiple
    options:
      - id: stream
        text: Streaming tokens as they are generated
        correct: true
      - id: parallel
        text: Running independent retrieval and tool calls in parallel
        correct: true
      - id: small-first
        text: Using a fast model for simple steps
        correct: true
      - text: Increasing max_tokens
    explanation: Time to first token matters most for chat; total time matters for pipelines.
  - id: build-buy
    prompt: When is buying a GenAI product generally better than building?
    options:
      - id: buy
        text: When the use case is common (meeting notes, coding assistance, generic support) and not a differentiator; build where your data and workflow are the differentiator
        correct: true
      - text: Never; always build
      - text: Always; never build
      - text: Only when the vendor is the largest
    explanation: Buying also brings vendor risk and data-handling questions. Building brings maintenance.
  - id: tco
    prompt: What is usually the largest cost of an internal GenAI application over three years?
    options:
      - text: Tokens
      - text: GPUs
      - id: people
        text: Engineering and operations — evaluation, maintenance, model migrations, monitoring, support — often exceeds inference spend for internal tools
        correct: true
      - text: Licences for prompt templates
    explanation: Inference spend dominates only at large scale. Budget for maintenance.
---
## Learning objectives

- Calculate token costs and identify cost drivers
- Improve latency
- Decide between building and buying

## A cost model

```python
def monthly_cost(requests, tokens_in, tokens_out, price_in, price_out, cache_hit=0.0, cached_discount=0.9):
    effective_in = tokens_in * (1 - cache_hit * cached_discount)
    per_request = (effective_in * price_in + tokens_out * price_out) / 1_000_000
    return requests * per_request

print(monthly_cost(1_000_000, 4000, 500, 3.0, 15.0))
print(monthly_cost(1_000_000, 4000, 500, 3.0, 15.0, cache_hit=0.75))
```

Prices change constantly; plug in current numbers from your provider.

## Sources

- Epoch AI, "LLM inference prices have fallen rapidly but unequally across tasks", 2025 — <https://epoch.ai/data-insights/llm-inference-price-trends>
- a16z, "Welcome to LLMflation", 2024 — <https://a16z.com/llmflation-llm-inference-cost/>
- Anthropic, "Prompt caching" — <https://docs.anthropic.com/en/docs/build-with-claude/prompt-caching>
