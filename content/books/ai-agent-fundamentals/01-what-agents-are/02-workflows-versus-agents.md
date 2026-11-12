---
title: Workflows versus agents
questions:
  - id: anthropic-distinction
    prompt: In Anthropic's "Building effective agents" (2024), what distinguishes workflows from agents?
    options:
      - id: control
        text: In workflows, LLMs and tools are orchestrated through predefined code paths; in agents, the LLM dynamically directs its own process and tool usage
        correct: true
      - text: Workflows use smaller models
      - text: Agents cannot call tools
      - text: Workflows are only for data engineering
    explanation: Both are "agentic systems"; the difference is who decides the control flow.
  - id: augmented-llm
    prompt: What is the "augmented LLM" building block?
    options:
      - text: An LLM with more GPUs
      - text: A fine-tuned LLM
      - id: augmented
        text: An LLM enhanced with retrieval, tools and memory, which it can use itself
        correct: true
      - text: Two LLMs in a row
    explanation: Every pattern in this book is built from augmented LLM calls.
  - id: when-workflow
    prompt: When is a workflow preferable to an autonomous agent?
    options:
      - text: When the task is open-ended and unpredictable
      - id: predictable
        text: When the task's steps are well understood and predictable, so fixed paths give consistency, lower cost and easier testing
        correct: true
      - text: Never
      - text: When you have a large budget
    explanation: Most production "agents" are actually workflows with a few LLM decisions — and that is a good thing.
  - id: when-agent
    prompt: When is an autonomous agent most justified?
    options:
      - text: When you need deterministic output
      - text: When the steps are always identical
      - text: When errors are very costly and cannot be checked
      - id: open-ended
        text: When the number and order of steps cannot be predicted in advance, the environment gives feedback, and you trust the model's decisions in that domain
        correct: true
    explanation: Coding agents fit — tests and compilers give feedback at every step.
  - id: start-simple
    prompt: What does Anthropic's guidance recommend as a starting point?
    options:
      - id: simplest
        text: Start with the simplest solution — often a single well-prompted LLM call with retrieval — and add complexity only when it demonstrably improves outcomes
        correct: true
      - text: Start with a multi-agent framework
      - text: Start by fine-tuning
      - text: Start with full autonomy and restrict later
    explanation: "\"Success in the LLM space isn't about building the most sophisticated system. It's about building the right system for your needs.\""
  - id: frameworks
    prompt: What caution does the same guidance give about agent frameworks?
    options:
      - text: Never use them
      - text: Always use the most popular one
      - id: understand
        text: Frameworks speed up starting but add abstraction that can obscure prompts and responses; understand what happens underneath, and start with direct API calls if possible
        correct: true
      - text: Frameworks guarantee correctness
    explanation: Many patterns are a few dozen lines of code.
  - id: spectrum
    prompt: Order these from least to most autonomous.
    options:
      - text: Agent → router → single call → chain
      - text: Chain → agent → single call → router
      - text: Router → agent → chain → single call
      - id: order
        text: Single LLM call → prompt chain → router → autonomous agent
        correct: true
    explanation: Each step up adds flexibility, cost, latency and unpredictability.
  - id: deterministic-steps
    prompt: In a hybrid design, which steps should stay deterministic code?
    type: multiple
    options:
      - id: auth
        text: Authorisation checks
        correct: true
      - id: money
        text: Calculations involving money
        correct: true
      - id: validation
        text: Validation of tool inputs and outputs
        correct: true
      - text: Interpreting free-text customer complaints
    explanation: Use LLMs where language understanding is needed; use code where correctness must be guaranteed.
---
## Learning objectives

- Distinguish workflows from agents
- Choose the least autonomy that solves the problem
- Use frameworks with understanding

## A routing workflow (not an agent)

```python
category = llm_classify(ticket, ["billing", "technical", "account"])
match category:
    case "billing":   reply = billing_flow(ticket)
    case "technical": reply = technical_flow(ticket)
    case _:           reply = escalate_to_human(ticket)
```

The LLM makes one decision; code owns the control flow. This is easy to test and cheap to run.

## Sources

- Anthropic, "Building effective agents", December 2024 — <https://www.anthropic.com/engineering/building-effective-agents>
- LangChain, "How to think about agent frameworks", 2025 — <https://blog.langchain.com/how-to-think-about-agent-frameworks/>
