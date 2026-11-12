---
title: Multi-agent systems
questions:
  - id: definition
    prompt: What is a multi-agent system in the LLM sense?
    options:
      - id: several
        text: Several agents — each with its own instructions, tools and context — collaborating, often coordinated by a supervisor or through hand-offs
        correct: true
      - text: One agent with many users
      - text: Several copies of the same model behind a load balancer
      - text: Agents competing in a game
    explanation: The Databricks course introduces multi-agent systems; Agent Bricks offered a multi-agent supervisor template.
  - id: why
    prompt: Which are legitimate reasons to use multiple agents?
    type: multiple
    options:
      - id: specialisation
        text: Specialisation — different tools, permissions and prompts per domain
        correct: true
      - id: context-isolation
        text: Context isolation — sub-agents explore in fresh contexts and return condensed results
        correct: true
      - id: parallel
        text: Parallel exploration of independent sub-problems
        correct: true
      - text: Because more agents are always smarter
    explanation: Each reason has a cost; justify it with evaluation.
  - id: anthropic-research
    prompt: What did Anthropic report about its multi-agent research system (2025)?
    options:
      - text: It used fewer tokens than a single agent
      - id: tokens
        text: It outperformed a single agent substantially on breadth-first research tasks but used many times more tokens — roughly 15× a chat interaction
        correct: true
      - text: It failed completely
      - text: It worked best for coding tasks with tight dependencies
    explanation: Multi-agent designs pay off when the task's value justifies the token cost and the work parallelises.
  - id: cognition
    prompt: Cognition's essay "Don't Build Multi-Agents" (2025) argued what?
    options:
      - text: Multi-agent systems are always best
      - text: Agents should not use tools
      - id: context-sharing
        text: Splitting work across agents that do not share full context leads to conflicting assumptions and fragile results; single-threaded agents with good context management are often more reliable
        correct: true
      - text: Agents should be written in Rust
    explanation: The two positions are compatible — parallelise read-only research; keep tightly coupled decisions in one context.
  - id: supervisor
    prompt: In a supervisor (hierarchical) architecture, what does the supervisor do?
    options:
      - id: delegate
        text: Interprets the request, delegates to specialised agents, and combines their outputs into a response
        correct: true
      - text: Monitors GPU temperature
      - text: Writes all answers itself
      - text: Approves expenses
    explanation: The supervisor's routing quality caps the whole system's quality.
  - id: handoff
    prompt: What is a hand-off pattern?
    options:
      - text: Passing a request to a human
      - text: Ending the conversation
      - text: Sharing API keys
      - id: transfer
        text: One agent transfers control of the conversation to another agent better suited to continue, along with relevant context
        correct: true
    explanation: Used in customer-service designs (triage → billing agent).
  - id: failure-modes
    prompt: Research on multi-agent failures (Cemri et al., 2025) identified which categories?
    type: multiple
    options:
      - id: spec
        text: Specification and system design problems
        correct: true
      - id: misalignment
        text: Inter-agent misalignment — agents ignoring or misunderstanding each other
        correct: true
      - id: verification
        text: Weak task verification and premature termination
        correct: true
      - text: Agents unionising
    explanation: Their taxonomy (MAST) found many failures arise from design, not model capability.
  - id: debugging
    prompt: Why are multi-agent systems harder to debug?
    options:
      - text: They do not produce logs
      - id: emergent
        text: Behaviour emerges from interactions; errors propagate between agents, and you need end-to-end traces across all of them
        correct: true
      - text: They run on different clouds
      - text: They cannot be tested
    explanation: Tracing that links spans across agents is essential.
  - id: cost-multi
    prompt: A two-level supervisor with four worker agents handles each request. Compared with one agent, what should you expect?
    options:
      - text: Lower cost and latency
      - text: Identical cost
      - text: Cost depends only on the supervisor
      - id: higher
        text: Higher token cost and latency, justified only if quality gains are measured
        correct: true
    explanation: Measure cost per successful task for both designs.
---
## Learning objectives

- Explain supervisor, hand-off and parallel multi-agent designs
- Weigh the evidence for and against multi-agent systems
- Recognise multi-agent failure modes

## Supervisor sketch

```text
User ──► Supervisor ──┬─► Sales-data agent (Genie / SQL tools, read-only)
                      ├─► Policy agent (document retrieval)
                      └─► Ticketing agent (create/update tickets, needs approval)
                 ◄── combine results ──
```

## Sources

- Anthropic, "How we built our multi-agent research system", 2025 — <https://www.anthropic.com/engineering/multi-agent-research-system>
- Cognition, "Don't Build Multi-Agents", 2025 — <https://cognition.ai/blog/dont-build-multi-agents>
- Cemri et al., "Why Do Multi-Agent LLM Systems Fail?", 2025 — <https://arxiv.org/abs/2503.13657>
- Databricks, "AI Agent Fundamentals" — <https://www.databricks.com/training/catalog/ai-agent-fundamentals-4482>
