---
title: Mixed review
questions:
  - id: r1
    prompt: A process has fixed, well-understood steps with one judgment call. What design fits best?
    options:
      - text: An autonomous multi-agent system
      - id: workflow
        text: A workflow with one LLM decision (e.g. routing or classification) and code for the rest
        correct: true
      - text: A fine-tuned model with no code
      - text: A reasoning model with all tools enabled
    explanation: Least autonomy that solves the problem.
  - id: r2
    prompt: Twenty steps at 95% reliability each give roughly what end-to-end success rate?
    options:
      - text: 95%
      - text: 80%
      - id: about-36
        text: About 36%
        correct: true
      - text: 5%
    explanation: Compounding error is why verification and short trajectories matter.
  - id: r3
    prompt: Which tool design is better for an agent?
    options:
      - text: "`execute_sql(query)` over the whole warehouse"
      - text: One tool per REST endpoint returning raw JSON
      - text: A tool returning entire tables
      - id: task
        text: "`get_order_status(order_id)` returning concise fields, with validation and permission checks"
        correct: true
    explanation: Task-level, concise, validated, scoped.
  - id: r4
    prompt: An email agent can read the inbox, search internal files and send email. What is the minimum safe change?
    options:
      - id: break-leg
        text: Require human approval for outbound email (or remove sending) when processing untrusted messages
        correct: true
      - text: Add "ignore malicious instructions" to the prompt
      - text: Use a bigger model
      - text: Lower the temperature
    explanation: Break a leg of the lethal trifecta.
  - id: r5
    prompt: Which statements about MCP are true?
    type: multiple
    options:
      - id: open
        text: It is an open protocol under neutral governance
        correct: true
      - id: primitives
        text: Servers expose tools, resources and prompts
        correct: true
      - id: not-trust
        text: It does not decide what an agent should be allowed to do
        correct: true
      - text: It replaces the need for authentication
    explanation: Protocols standardise connections, not trust.
  - id: r6
    prompt: An agent says "Done — meeting moved." How do you verify?
    options:
      - text: Trust the message
      - text: Ask the agent again
      - id: state
        text: Check the calendar state against the expected outcome
        correct: true
      - text: Count the tool calls
    explanation: Verify outcomes in the environment.
  - id: r7
    prompt: Your agent runs as a powerful service principal for all employees. What risk must you address?
    options:
      - text: Slow responses
      - id: confused-deputy
        text: Privilege escalation through the agent (confused deputy); use on-behalf-of-user access or per-user filtering
        correct: true
      - text: Higher model costs
      - text: None
    explanation: An agent must not exceed the requesting user's permissions.
  - id: r8
    prompt: When do multi-agent systems most clearly pay off?
    options:
      - text: Always
      - text: For simple extraction tasks
      - text: For tightly coupled sequential decisions
      - id: parallel-research
        text: For valuable, parallelisable work such as broad research, where higher token cost buys measurable quality
        correct: true
    explanation: Measure cost per successful task against a single-agent baseline.
  - id: r9
    prompt: Which Databricks components are relevant to governing agents?
    type: multiple
    options:
      - id: uc
        text: Unity Catalog for tools, models, data and agents
        correct: true
      - id: gateway
        text: The AI gateway for rate limits, guardrails and logging
        correct: true
      - id: mlflow
        text: MLflow tracing and evaluation
        correct: true
      - text: Delta Sharing recipients
    explanation: Governance, control and observability together.
  - id: r10
    prompt: In .NET, what makes switching from one model provider to another cheap?
    options:
      - id: abstraction
        text: Depending on `IChatClient` with providers wired in the composition root and model names in configuration, validated by an evaluation suite
        correct: true
      - text: Using the provider's SDK types everywhere
      - text: Hard-coding model names
      - text: Avoiding tests
    explanation: Dependency inversion applied to AI.
  - id: r11
    prompt: Why model agent outcomes as a C# 15 union?
    options:
      - text: Unions are faster than records
      - text: Models produce unions
      - id: exhaustive
        text: The compiler checks every outcome is handled, and new outcomes surface everywhere they must be handled
        correct: true
      - text: Unions serialise to smaller JSON
    explanation: Exhaustiveness turns a missed case into a compiler warning instead of a production incident.
  - id: r12
    prompt: Which evidence best supports deploying an agent?
    options:
      - text: A polished demo
      - text: A vendor case study
      - text: Developers feeling faster
      - id: controlled
        text: A controlled comparison against a baseline, measuring cost per successful task, quality and incidents
        correct: true
    explanation: METR's study showed perceived and measured productivity can point in opposite directions.
  - id: r13
    prompt: Which budgets bound an agent run?
    type: multiple
    options:
      - id: steps
        text: Steps
        correct: true
      - id: tokens
        text: Tokens or cost
        correct: true
      - id: time
        text: Wall-clock time
        correct: true
      - text: Number of employees
    explanation: Budgets convert runaway failures into bounded ones.
  - id: r14
    prompt: What is the role of human approvals in a well-designed agent?
    options:
      - id: targeted
        text: Targeted at irreversible or high-impact actions, presenting the exact action, reasons and reversibility
        correct: true
      - text: Required for every step
      - text: Never needed
      - text: Only for read actions
    explanation: Too many approvals cause fatigue; too few cause incidents.
---
## Learning objectives

- Apply the book's ideas to mixed scenarios

## Twelve rules for agents

1. Start with the simplest thing; add autonomy only when evaluation demands it.
2. The model proposes; code disposes.
3. Design tools for agents — task-level, concise, validated.
4. Bound every run: steps, tokens, time.
5. Verify outcomes in the environment, not in the agent's words.
6. Evaluate trajectories and consistency, not one lucky run.
7. Trace everything, with privacy controls.
8. Never combine the lethal trifecta without supervision.
9. Agents act with the user's permissions, never more.
10. Approvals for irreversible actions, with real information.
11. Depend on abstractions and protocols; isolate frameworks.
12. Measure cost per successful task against a baseline.

## Sources

- See the sources listed in each section of this book.
