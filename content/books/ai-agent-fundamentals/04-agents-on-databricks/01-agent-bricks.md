---
title: Agent Bricks
questions:
  - id: what
    prompt: How does Databricks describe Agent Bricks in the AI Agent Fundamentals course?
    options:
      - text: A storage engine for agent logs
      - id: simplify
        text: A way to simplify building enterprise-ready agents on the Databricks platform (Mosaic AI), with demos of building and using agents
        correct: true
      - text: An open-source protocol
      - text: A GPU instance type
    explanation: Introduced in June 2025 and expanded at Data + AI Summit 2026.
  - id: launch-approach
    prompt: What was distinctive about Agent Bricks' approach at its 2025 launch?
    options:
      - text: It required users to write all evaluation code by hand
      - text: It only supported one model provider
      - id: auto-eval
        text: You describe the task and connect data; it generates task-specific evaluations (synthetic data and LLM judges) and optimises the agent for quality and cost
        correct: true
      - text: It trained a new foundation model per customer
    explanation: The idea addresses the hardest part of agent development — knowing whether the agent is good — but auto-generated evaluations still need human review.
  - id: templates
    prompt: Which task-specific templates did Agent Bricks offer at its 2025 launch?
    type: multiple
    options:
      - id: ie
        text: Information Extraction — turn unstructured documents into structured fields
        correct: true
      - id: ka
        text: Knowledge Assistant — question answering over documents with citations
        correct: true
      - id: custom-llm
        text: Custom LLM — text transformation tasks such as summarisation or classification
        correct: true
      - id: mas
        text: Multi-Agent Supervisor — coordinating multiple agents and tools
        correct: true
    explanation: Names and packaging evolved in 2026; the underlying task types remain common enterprise patterns.
  - id: alhf
    prompt: What is "agent learning from human feedback" as Databricks described it?
    options:
      - id: nl-feedback
        text: Subject-matter experts giving natural-language feedback on agent outputs, which the system uses to adjust the agent's behaviour
        correct: true
      - text: RLHF retraining of the base model weights by Databricks
      - text: Users clicking thumbs up
      - text: Feedback collected only from developers
    explanation: Expert feedback is valuable; verify that changes generalise using a held-out evaluation set.
  - id: dais-2026
    prompt: Which capabilities did Databricks describe for Agent Bricks at Data + AI Summit 2026?
    type: multiple
    options:
      - id: models
        text: Choice of models from several providers and support for agent harnesses such as LangGraph, CrewAI and OpenAI and Claude agent SDKs
        correct: true
      - id: mcp-uc
        text: MCP support in Unity Catalog so agents can reach external sources such as Jira, Slack and GitHub
        correct: true
      - id: memory
        text: An agent memory service powered by Lakebase
        correct: true
      - id: sandbox
        text: Sandboxes — secure VMs with down-scoped Unity Catalog data access
        correct: true
    explanation: Several items lacked a stated GA status in the announcement. Check documentation before depending on them.
  - id: optimise
    prompt: When Agent Bricks optimises an agent for "quality and cost", what should you independently verify?
    options:
      - text: Nothing; the platform handles it
      - text: Only the price
      - text: Only the latency
      - id: verify
        text: That the generated evaluations reflect your real success criteria, and that quality holds on a held-out set of real examples reviewed by experts
        correct: true
    explanation: Optimising against a flawed judge optimises the wrong thing (Goodhart's law).
  - id: lockin-ab
    prompt: What is the portability trade-off of a managed agent builder like Agent Bricks?
    options:
      - id: tradeoff
        text: Faster development and integrated governance, at the cost of agent definitions and optimisation tied to the platform; code-first agents in open frameworks are more portable but need more work
        correct: true
      - text: There is no trade-off
      - text: Agent Bricks agents run anywhere unchanged
      - text: Code-first agents cannot run on Databricks
    explanation: Many teams prototype in a managed builder and move critical agents to code when they need control.
  - id: knowledge-assistant-limits
    prompt: A Knowledge Assistant over policy documents gives a wrong answer about an exception buried in a table. Where should you look?
    options:
      - text: The model's training data
      - id: parsing
        text: Document parsing and retrieval — whether the table was parsed correctly and the right chunk retrieved — using traces
        correct: true
      - text: The user's browser
      - text: Agent Bricks cannot be debugged
    explanation: Managed RAG has the same failure points as hand-built RAG.
---
## Learning objectives

- Describe Agent Bricks and its task types
- Understand auto-generated evaluation and optimisation, and their limits
- Weigh managed builders against code-first agents

## The Agent Bricks idea

```text
Describe the task ──► connect data (Unity Catalog tables, volumes, Genie spaces, MCP)
        │
        ▼
Generated evaluation set + LLM judges ◄── expert feedback (natural language)
        │
        ▼
Optimise (prompts, models, retrieval, fine-tuning) for quality and cost
        │
        ▼
Deploy with governance (Unity Catalog, AI gateway, tracing)
```

## Sources

- Databricks, "AI Agent Fundamentals" — <https://www.databricks.com/training/catalog/ai-agent-fundamentals-4482>
- Databricks blog, "Introducing Agent Bricks", June 2025 — <https://www.databricks.com/blog/introducing-agent-bricks>
- Databricks blog, "Agent Bricks: Data + AI Summit 2026" — <https://www.databricks.com/blog/agent-bricks-dais-2026>
- Databricks docs, "Agent Bricks" — <https://docs.databricks.com/aws/en/generative-ai/agent-bricks/>
