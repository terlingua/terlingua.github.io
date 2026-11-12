---
title: Glossary
questions:
  - id: g-agent
    prompt: "Agent:"
    options:
      - id: def
        text: A system where an LLM directs its own process, using tools in a loop to achieve a goal
        correct: true
      - text: Any chatbot interface
      - text: A background Windows service
      - text: A human operator
    explanation: Workflows follow predefined paths; agents choose their own.
  - id: g-tool
    prompt: "Tool (function) call:"
    options:
      - text: The model executing code on your server directly
      - text: A phone call to support
      - id: def
        text: A structured request emitted by the model, which the application validates and executes
        correct: true
      - text: A shell alias
    explanation: The application owns execution and permissions.
  - id: g-mcp
    prompt: "MCP:"
    options:
      - text: Microsoft Certified Professional
      - text: Multi-Cloud Platform
      - text: Model Compression Protocol
      - id: def
        text: Model Context Protocol — an open standard connecting AI applications to tools, resources and prompts
        correct: true
    explanation: Governed by the Agentic AI Foundation under the Linux Foundation since December 2025.
  - id: g-a2a
    prompt: "A2A:"
    options:
      - id: def
        text: Agent2Agent — an open protocol for agents to discover each other (Agent Cards) and collaborate on tasks
        correct: true
      - text: Application-to-application file transfer
      - text: An authentication standard
      - text: A Databricks product
    explanation: Complements MCP.
  - id: g-trifecta
    prompt: "Lethal trifecta:"
    options:
      - text: Three models voting
      - id: def
        text: Private data + untrusted content + external communication in one agent — an exfiltration risk
        correct: true
      - text: Three failed deployments
      - text: A benchmark
    explanation: Remove at least one leg.
  - id: g-trajectory
    prompt: "Trajectory:"
    options:
      - text: The model's training curve
      - text: A network route
      - id: def
        text: The sequence of reasoning steps, tool calls and observations an agent took during a run
        correct: true
      - text: The final answer only
    explanation: Evaluated alongside the final outcome.
  - id: g-passk
    prompt: "pass^k:"
    options:
      - text: Passing k unit tests
      - text: A password policy
      - text: The pass rate on the best of k tries
      - id: def
        text: The probability that an agent succeeds on all k independent attempts — a consistency measure
        correct: true
    explanation: Distinct from pass@k, which counts success in any of k attempts.
  - id: g-agent-bricks
    prompt: "Agent Bricks:"
    options:
      - id: def
        text: Databricks' platform for building, evaluating, optimising and governing enterprise agents
        correct: true
      - text: An open protocol
      - text: A Lego set
      - text: A vector database
    explanation: Launched 2025; expanded 2026.
  - id: g-ichatclient
    prompt: "`IChatClient`:"
    options:
      - text: A SignalR hub
      - id: def
        text: The Microsoft.Extensions.AI abstraction for chat models in .NET, composable with middleware
        correct: true
      - text: A WinForms control
      - text: An HTTP client for REST
    explanation: Agent Framework and MCP tools build on it.
  - id: g-hitl
    prompt: "Human in the loop:"
    options:
      - text: A person typing every response
      - text: Users watching a progress bar
      - id: def
        text: A person reviewing or approving agent actions before they take effect
        correct: true
      - text: An offshore support team
    explanation: Must be meaningful to count as oversight.
---
## Learning objectives

- Recall core terms precisely

## Terms

| Term | Meaning |
|---|---|
| Augmented LLM | LLM + retrieval + tools + memory |
| Workflow | Predefined code paths orchestrating LLM calls |
| ReAct | Interleaved reasoning and acting |
| Orchestrator-workers | Runtime decomposition and delegation |
| Context engineering | Curating what enters the context each step |
| Excessive agency | More capability or permission than needed |
| Rule of Two | At most two of: untrusted input, sensitive access, external action |
| On-behalf-of-user | Agent acts with the invoking user's permissions |

## Sources

- See the sources listed in each section of this book.
