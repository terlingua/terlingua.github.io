---
title: Defining AI agents
questions:
  - id: definition
    prompt: Which definition of an AI agent is most useful for builders?
    options:
      - text: Any chatbot
      - text: A model with more parameters
      - id: loop
        text: A system in which an LLM directs its own process — choosing actions and tool calls in a loop, observing results — to accomplish a goal
        correct: true
      - text: A robot with a body
    explanation: Simon Willison's short form — "an LLM agent runs tools in a loop to achieve a goal" — captures the same idea.
  - id: components
    prompt: According to the Databricks course description, which core components make up an agent?
    type: multiple
    options:
      - id: llm
        text: An LLM for reasoning
        correct: true
      - id: tools
        text: Tools for acting and retrieving information
        correct: true
      - id: memory
        text: Memory
        correct: true
      - id: workflow
        text: Workflows or control logic
        correct: true
    explanation: Different sources slice it differently, but these four recur.
  - id: vs-chatbot
    prompt: How does an agent differ from a plain chatbot?
    options:
      - id: acts
        text: A chatbot responds with text; an agent can decide to take actions through tools, observe results and continue over multiple steps toward a goal
        correct: true
      - text: Agents are always smarter models
      - text: Chatbots cannot use LLMs
      - text: There is no difference
    explanation: The ability to act is what creates both value and risk.
  - id: vs-traditional
    prompt: How do LLM agents differ from traditional rule-based automation (e.g. RPA)?
    options:
      - text: Agents are fully deterministic
      - text: RPA cannot click buttons
      - text: Agents never need integration work
      - id: flexible
        text: Agents handle unstructured inputs and unanticipated situations flexibly, but are non-deterministic and harder to verify; RPA is brittle but predictable
        correct: true
    explanation: Many good systems combine both — deterministic steps where possible, LLM judgment where needed.
  - id: autonomy
    prompt: Which statement about agent autonomy is accurate?
    options:
      - text: Agents are either fully autonomous or not agents
      - id: spectrum
        text: Autonomy is a spectrum — from a single LLM call, through fixed workflows and routers, to agents that plan and choose tools freely
        correct: true
      - text: More autonomy is always better
      - text: Autonomy is set by the model vendor
    explanation: Choose the least autonomy that solves the problem.
  - id: agent-washing
    prompt: What is "agent washing"?
    options:
      - text: Cleaning agent logs
      - text: Retraining agents daily
      - id: rebrand
        text: Rebranding existing chatbots, RPA or assistants as "agents" without real agentic capability; Gartner estimated only about 130 of thousands of "agentic AI" vendors were genuine in 2025
        correct: true
      - text: A security attack
    explanation: Ask vendors what the system decides by itself and what tools it can call.
  - id: goal
    prompt: Why does an agent need a clear goal and stopping condition?
    options:
      - text: For marketing copy
      - id: termination
        text: Without them, the loop may stop too early, wander, or run indefinitely, consuming tokens and taking unnecessary actions
        correct: true
      - text: Models require a goal parameter
      - text: To reduce the number of tools
    explanation: Combine a goal with explicit limits on steps, time and cost.
  - id: agent-not-new
    prompt: Is the concept of a software "agent" new?
    options:
      - text: Yes, it was invented in 2023
      - text: Yes, by Databricks
      - text: Yes, by OpenAI
      - id: old
        text: No — agents (perceive, decide, act) are a classic AI concept, central to Russell and Norvig's textbook; LLMs made general-purpose agents practical
        correct: true
    explanation: Older concepts — environments, policies, rewards — remain useful for thinking about LLM agents.
  - id: enterprise-components
    prompt: In an enterprise setting, what makes an agent "production-ready" beyond the loop itself?
    type: multiple
    options:
      - id: evaluation
        text: Evaluation showing it meets a quality bar
        correct: true
      - id: governance
        text: Governance — identity, permissions, audit
        correct: true
      - id: observability
        text: Observability — traces, cost and error monitoring
        correct: true
      - id: oversight
        text: Human oversight for consequential actions
        correct: true
    explanation: The Databricks course stresses evaluation and governance; they apply on any platform.
---
## Learning objectives

- Define an AI agent precisely
- Distinguish agents from chatbots and traditional automation
- Recognise agent washing

## The minimal agent loop

```text
goal ─► LLM decides ─┬─► final answer ─► stop
         ▲           └─► tool call ─► execute ─► observation
         └────────────────────────────────────────┘
```

```python
messages = [{"role": "system", "content": SYSTEM}, {"role": "user", "content": goal}]
for step in range(MAX_STEPS):
    reply = llm(messages, tools=TOOLS)
    if not reply.tool_calls:
        return reply.content
    for call in reply.tool_calls:
        result = run_tool(call.name, call.arguments)
        messages += [reply.as_message(), {"role": "tool", "tool_call_id": call.id, "content": result}]
raise RuntimeError("step budget exhausted")
```

## Sources

- Databricks, "AI Agent Fundamentals" course — <https://www.databricks.com/training/catalog/ai-agent-fundamentals-4482>
- Databricks blog, "Master AI Agents with New Databricks Training", 2025 — <https://www.databricks.com/blog/master-ai-agents-new-databricks-training>
- Simon Willison, "I think 'agent' may finally have a widely enough agreed upon definition", 2025 — <https://simonwillison.net/2025/Sep/18/agents/>
- Gartner press release, June 2025 — <https://www.gartner.com/en/newsroom/press-releases/2025-06-25-gartner-predicts-over-40-percent-of-agentic-ai-projects-will-be-canceled-by-end-of-2027>
- Russell and Norvig, *Artificial Intelligence: A Modern Approach*, 4th ed.
