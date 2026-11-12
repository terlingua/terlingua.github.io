---
title: Protocols — MCP and A2A
questions:
  - id: mcp-what
    prompt: What is the Model Context Protocol (MCP)?
    options:
      - text: A proprietary Databricks API
      - id: open
        text: An open protocol, introduced by Anthropic in November 2024, standardising how AI applications connect to tools, data and prompts through MCP servers
        correct: true
      - text: A model compression format
      - text: A messaging queue
    explanation: Often described as "USB-C for AI applications" — one integration works with many hosts.
  - id: mcp-problem
    prompt: Which integration problem does MCP address?
    options:
      - text: Slow GPUs
      - text: Token pricing
      - id: m-times-n
        text: The M×N problem — every AI application writing custom integrations for every tool — becoming M+N with a common protocol
        correct: true
      - text: Model hallucination
    explanation: Tool providers implement a server once; any MCP-capable client can use it.
  - id: primitives
    prompt: Which are MCP server primitives?
    type: multiple
    options:
      - id: tools
        text: Tools — functions the model can call
        correct: true
      - id: resources
        text: Resources — data the application can read into context
        correct: true
      - id: prompts
        text: Prompts — reusable templates
        correct: true
      - text: GPUs
    explanation: Clients can also offer capabilities to servers, such as sampling (asking the client's model to generate) and elicitation (asking the user for input).
  - id: transports
    prompt: Which transports does MCP define?
    options:
      - text: FTP and SMTP
      - id: stdio-http
        text: stdio for local servers launched as subprocesses, and Streamable HTTP for remote servers (which replaced the earlier HTTP+SSE transport in 2025)
        correct: true
      - text: Only WebSockets
      - text: Bluetooth
    explanation: Local stdio servers run with the user's privileges — a security consideration.
  - id: governance-mcp
    prompt: Where is MCP governed as of 2026?
    options:
      - text: Solely by Anthropic
      - text: By the W3C
      - text: Nobody governs it
      - id: aaif
        text: Anthropic donated MCP to the Agentic AI Foundation under the Linux Foundation in December 2025
        correct: true
    explanation: Neutral governance encouraged adoption across OpenAI, Google, Microsoft, Databricks and others.
  - id: mcp-risks
    prompt: Which are known MCP security risks?
    type: multiple
    options:
      - id: tool-poisoning
        text: Tool poisoning — malicious instructions hidden in tool descriptions
        correct: true
      - id: rug-pull
        text: "\"Rug pulls\" — a server changing tool definitions after approval"
        correct: true
      - id: confused-deputy
        text: Confused-deputy and token-passthrough problems in authorisation
        correct: true
      - id: untrusted-servers
        text: Installing untrusted local servers that run arbitrary code
        correct: true
    explanation: Treat MCP servers like third-party dependencies — vet, pin, sandbox and monitor them. The specification adopted OAuth-based authorisation for remote servers.
  - id: mcp-not
    prompt: What does MCP NOT do by itself?
    options:
      - id: not-safe
        text: Make tools safe or decide what an agent should be allowed to do — permissions and policies are still your responsibility
        correct: true
      - text: Define how tools are described
      - text: Allow remote servers
      - text: Support tool calling
    explanation: A protocol standardises connections, not trust.
  - id: a2a
    prompt: What is the Agent2Agent (A2A) protocol?
    options:
      - text: A protocol for agents to call databases
      - text: A replacement for MCP
      - id: agents-talk
        text: An open protocol (introduced by Google in April 2025, now under the Linux Foundation) for agents built on different frameworks to discover each other and collaborate on tasks
        correct: true
      - text: An authentication standard
    explanation: MCP connects an agent to tools and data; A2A connects agents to other agents. They are complementary.
  - id: agent-card
    prompt: In A2A, what is an Agent Card?
    options:
      - text: A business card for developers
      - id: card
        text: A JSON document (served at a well-known URL) describing an agent's identity, skills, endpoint and authentication requirements, used for discovery
        correct: true
      - text: A GPU accelerator
      - text: A credit card for agent purchases
    explanation: A2A also defines tasks with lifecycle states, messages and artifacts, with support for long-running work and streaming.
  - id: mcp-vs-a2a
    prompt: When would you use A2A rather than MCP?
    options:
      - text: To give an agent access to a file system
      - text: To call a calculator
      - text: To read a database table
      - id: opaque-agents
        text: When delegating a task to another autonomous agent (possibly from another vendor) that manages its own reasoning and tools, rather than calling a deterministic tool
        correct: true
    explanation: In practice MCP adoption has been much broader than A2A so far.
---
## Learning objectives

- Explain MCP's architecture, primitives and transports
- Recognise MCP's security risks
- Distinguish MCP from A2A

## MCP architecture

```text
Host application (IDE, chat app, agent)
 └─ MCP client ──(stdio | Streamable HTTP, JSON-RPC 2.0)──► MCP server
                                                            ├─ tools
                                                            ├─ resources
                                                            └─ prompts
```

## A2A at a glance

```text
Client agent ──GET /.well-known/agent-card.json──► Remote agent
             ──send message / create task─────────► (works autonomously)
             ◄─status updates, artifacts (streaming)─
```

## Sources

- Anthropic, "Introducing the Model Context Protocol", November 2024 — <https://www.anthropic.com/news/model-context-protocol>
- MCP specification — <https://modelcontextprotocol.io/specification>
- Linux Foundation, "Agentic AI Foundation" announcement, December 2025 — <https://www.linuxfoundation.org/press/linux-foundation-announces-the-formation-of-the-agentic-ai-foundation>
- Invariant Labs, "MCP Security Notification: Tool Poisoning Attacks", 2025 — <https://invariantlabs.ai/blog/mcp-security-notification-tool-poisoning-attacks>
- A2A protocol — <https://a2a-protocol.org/>
- Linux Foundation, "Linux Foundation Launches the Agent2Agent Protocol Project", June 2025 — <https://www.linuxfoundation.org/press/linux-foundation-launches-the-agent2agent-protocol-project-to-enable-secure-intelligent-communication-between-ai-agents>
