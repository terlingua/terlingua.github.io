---
title: Memory and context
questions:
  - id: short-term
    prompt: What is an agent's short-term (working) memory?
    options:
      - id: context
        text: The conversation and tool results kept in the context window during the current task
        correct: true
      - text: The model's weights
      - text: A database of all users
      - text: The GPU cache
    explanation: It is limited by the context window and grows with every step.
  - id: long-term
    prompt: What is long-term memory in agents?
    options:
      - text: Fine-tuning the model nightly
      - text: A larger context window
      - id: persisted
        text: Information persisted outside the model (databases, vector stores, files) and retrieved into context when relevant across sessions
        correct: true
      - text: The provider remembering conversations automatically
    explanation: Databricks announced an agent memory service backed by Lakebase in 2026; many frameworks provide memory stores.
  - id: memory-types
    prompt: Cognitive-science-inspired agent designs (e.g. CoALA) distinguish which memory types?
    type: multiple
    options:
      - id: episodic
        text: Episodic — past experiences and interactions
        correct: true
      - id: semantic
        text: Semantic — facts about the world or user
        correct: true
      - id: procedural
        text: Procedural — how to do things (prompts, code, skills)
        correct: true
      - text: Hydraulic — memory stored in pipes
    explanation: Sumers et al., "Cognitive Architectures for Language Agents", 2023.
  - id: overflow
    prompt: A long-running agent's context approaches the window limit. Which strategies help?
    type: multiple
    options:
      - id: summarise
        text: Summarise or compact older steps
        correct: true
      - id: offload
        text: Offload large tool results to files or stores and keep references
        correct: true
      - id: subagents
        text: Delegate sub-tasks to sub-agents with fresh contexts that return concise results
        correct: true
      - text: Increase temperature
    explanation: Summaries lose detail; decide what must be preserved verbatim.
  - id: memory-poisoning
    prompt: What is memory poisoning?
    options:
      - text: Corrupting RAM chips
      - id: poison
        text: An attacker causing false or malicious content to be stored in an agent's long-term memory, influencing future sessions
        correct: true
      - text: Forgetting old conversations
      - text: Running out of memory
    explanation: Validate what is written to memory, scope memories per user, and let users view and delete them.
  - id: privacy
    prompt: Which privacy considerations apply to agent memory?
    type: multiple
    options:
      - id: consent
        text: Users should know what is remembered and be able to delete it
        correct: true
      - id: isolation
        text: Memories must be isolated per user and tenant
        correct: true
      - id: sensitive
        text: Sensitive categories may need to be excluded or protected
        correct: true
      - text: Memory is exempt from privacy law
    explanation: GDPR and similar laws apply to remembered personal data.
  - id: state
    prompt: Where should durable task state (for example, whether an order was refunded) live?
    options:
      - text: Only in the model's context
      - text: In the model's weights
      - id: system-of-record
        text: In a system of record or database the agent updates through tools, not only in conversation history
        correct: true
      - text: In the system prompt
    explanation: Context is volatile and can be summarised away; state must survive restarts.
  - id: stateless-api
    prompt: Why does each agent step re-send the whole conversation in most implementations?
    options:
      - id: stateless
        text: Model APIs are stateless; the agent's "memory" within a task is the context it re-sends each call (sometimes managed server-side by the provider)
        correct: true
      - text: To make the model think harder
      - text: It does not
      - text: Because of HTTP limits
    explanation: This is why cost grows roughly quadratically with step count unless context is managed.
---
## Learning objectives

- Distinguish short-term and long-term memory
- Manage growing context
- Secure and govern memory

## Memory architecture

```text
┌──────────── context window (short-term) ────────────┐
│ system prompt · recent turns · tool results · notes  │
└───────────────▲──────────────────────────┬───────────┘
                │ retrieve relevant        │ write selected facts
┌───────────────┴──────────────────────────▼───────────┐
│ long-term stores: user profile · episodic log ·       │
│ vector memory · files · system-of-record databases    │
└───────────────────────────────────────────────────────┘
```

## Sources

- Sumers et al., "Cognitive Architectures for Language Agents" (CoALA), 2023 — <https://arxiv.org/abs/2309.02427>
- Packer et al., "MemGPT: Towards LLMs as Operating Systems", 2023 — <https://arxiv.org/abs/2310.08560>
- Databricks blog, "Agent Bricks: Data + AI Summit 2026" — <https://www.databricks.com/blog/agent-bricks-dais-2026>
