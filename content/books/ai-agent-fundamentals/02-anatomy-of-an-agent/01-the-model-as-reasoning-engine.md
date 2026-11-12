---
title: The model as reasoning engine
questions:
  - id: role
    prompt: What role does the LLM play in an agent?
    options:
      - text: It stores all the data
      - id: controller
        text: It interprets the goal and observations and decides the next action — call a tool, ask the user, or answer
        correct: true
      - text: It executes tools directly on servers
      - text: It replaces the database
    explanation: The model only emits text (including structured tool calls); your code executes tools.
  - id: model-choice
    prompt: Which model properties matter most for agents?
    type: multiple
    options:
      - id: tool-use
        text: Reliable tool calling and instruction following
        correct: true
      - id: long-context
        text: Robust handling of long, growing context
        correct: true
      - id: recovery
        text: Recovering from errors in tool results
        correct: true
      - text: The number of languages in its marketing
    explanation: Benchmarks like τ-bench, SWE-bench and BFCL (function calling) target these properties.
  - id: reasoning-models
    prompt: What are the trade-offs of using reasoning models in agents?
    options:
      - text: They are always faster
      - text: They cannot use tools
      - id: tradeoff
        text: Better planning and recovery on hard tasks, but more latency and tokens per step; use them for planning or hard steps and faster models for routine steps
        correct: true
      - text: No trade-offs
    explanation: Heterogeneous model choice per step is a common cost optimisation.
  - id: system-prompt
    prompt: What belongs in an agent's system prompt?
    type: multiple
    options:
      - id: role
        text: Role, goal and scope
        correct: true
      - id: tools-guidance
        text: When to use which tool, and when to ask the user
        correct: true
      - id: stop
        text: Stopping criteria and output format
        correct: true
      - text: API keys for tools
    explanation: Credentials belong in the tool execution layer, never in prompts.
  - id: determinism
    prompt: Why are agents harder to test than ordinary functions?
    options:
      - id: paths
        text: The same input can lead to different tool sequences and outcomes across runs, and the space of possible trajectories is large
        correct: true
      - text: They have no inputs
      - text: They are written in Python
      - text: Testing tools do not exist
    explanation: Test with many runs, measure distributions, and check invariants rather than exact outputs.
  - id: context-engineering
    prompt: What is "context engineering"?
    options:
      - text: Designing data centres
      - text: Writing a single clever prompt
      - id: context
        text: Deciding what information — instructions, tool results, memories, retrieved documents — goes into the model's limited context at each step, and in what form
        correct: true
      - text: Compressing model weights
    explanation: As agents run longer, managing context (summarising, pruning, retrieving) matters more than wording any single prompt.
  - id: model-swap
    prompt: A new model version is released. What must you do before switching your agent?
    options:
      - text: Nothing; newer is always better
      - text: Retrain the tools
      - text: Change all the tool names
      - id: eval
        text: Run your agent evaluation suite — tool-use behaviour, verbosity and failure modes can change in ways that break workflows
        correct: true
    explanation: Model upgrades are deployments; treat them as such.
---
## Learning objectives

- Understand what the LLM does and does not do in an agent
- Choose models for agentic workloads
- Practise context engineering

## What the model actually emits

```json
{
  "role": "assistant",
  "tool_calls": [
    { "id": "call_1", "type": "function",
      "function": { "name": "lookup_order", "arguments": "{\"order_id\": \"A-1042\"}" } }
  ]
}
```

The model proposes; your code disposes. Everything about safety follows from that boundary.

## Sources

- Anthropic, "Effective context engineering for AI agents", 2025 — <https://www.anthropic.com/engineering/effective-context-engineering-for-ai-agents>
- Berkeley Function Calling Leaderboard — <https://gorilla.cs.berkeley.edu/leaderboard.html>
- Yao et al., "τ-bench", 2024 — <https://arxiv.org/abs/2406.12045>
