---
title: Workflow patterns
questions:
  - id: chaining
    prompt: What is prompt chaining?
    options:
      - text: Running the same prompt many times
      - id: sequence
        text: Decomposing a task into a fixed sequence of LLM calls, each processing the previous output, optionally with programmatic checks ("gates") between steps
        correct: true
      - text: Linking prompts with a blockchain
      - text: Combining prompts into one long prompt
    explanation: Example — generate an outline, check it meets criteria, then write the document.
  - id: routing
    prompt: What is the routing pattern?
    options:
      - text: Network routing
      - text: Load balancing between identical models
      - id: classify
        text: Classifying an input and directing it to a specialised follow-up prompt, tool or model
        correct: true
      - text: Sending every request to every model
    explanation: Routing also enables cost savings — easy questions to small models, hard ones to large.
  - id: parallelization
    prompt: Which two variants of parallelisation does Anthropic describe?
    type: multiple
    options:
      - id: sectioning
        text: Sectioning — splitting a task into independent subtasks run in parallel
        correct: true
      - id: voting
        text: Voting — running the same task several times for diverse outputs or consensus
        correct: true
      - text: Sharding the model across GPUs
      - text: Running two users' requests together
    explanation: Example of sectioning — one call answers the question while another screens for inappropriate content.
  - id: orchestrator
    prompt: What characterises the orchestrator-workers pattern?
    options:
      - id: dynamic
        text: A central LLM dynamically breaks down a task, delegates subtasks to worker LLMs and synthesises their results — subtasks are not predefined
        correct: true
      - text: A fixed DAG of tasks
      - text: Workers that never return results
      - text: A Kubernetes operator
    explanation: It differs from parallelisation because the orchestrator decides the subtasks at runtime.
  - id: evaluator-optimizer
    prompt: When does the evaluator-optimizer pattern fit?
    options:
      - text: When there are no evaluation criteria
      - text: For single-word answers
      - text: When latency must be minimal
      - id: criteria
        text: When clear evaluation criteria exist and iterative refinement measurably improves results — e.g. translation or writing against a rubric
        correct: true
    explanation: One LLM generates; another evaluates and gives feedback; repeat until it passes or a limit is reached.
  - id: gates
    prompt: What is a "gate" in a chain?
    options:
      - text: A firewall
      - id: check
        text: A programmatic check between steps that stops or redirects the flow if an intermediate output fails validation
        correct: true
      - text: A paywall
      - text: A UI component
    explanation: Gates catch errors early instead of letting them compound.
  - id: pattern-match
    prompt: Match each scenario to the best pattern. Which pairings are sensible?
    type: multiple
    options:
      - id: support-routing
        text: Customer messages of distinct types → routing
        correct: true
      - id: code-review-voting
        text: Security review of code where missing an issue is costly → voting across several reviews
        correct: true
      - id: research-orchestrator
        text: Research across an unknown number of sources → orchestrator-workers
        correct: true
      - text: Extracting one field from a form → five-agent debate
    explanation: Use the simplest pattern that fits.
  - id: latency-tradeoff
    prompt: Which pattern typically increases latency the most?
    options:
      - text: A single call
      - text: Routing
      - id: iterative
        text: Iterative patterns such as evaluator-optimizer loops and multi-step orchestration
        correct: true
      - text: Parallel sectioning
    explanation: Parallel sectioning can reduce latency; loops add round trips.
---
## Learning objectives

- Recognise the five workflow patterns
- Choose patterns by task structure
- Use gates to stop error propagation

## Patterns at a glance

```text
Chaining:           A ──► gate ──► B ──► C
Routing:            in ──► classifier ──┬─► flow 1
                                        └─► flow 2
Parallelisation:    in ──┬─► A ─┐
                         └─► B ─┴─► aggregate
Orchestrator:       in ──► orchestrator ──► (workers decided at runtime) ──► synthesise
Evaluator-optimizer: generate ⇄ evaluate (until pass or limit)
```

## Sources

- Anthropic, "Building effective agents", 2024 — <https://www.anthropic.com/engineering/building-effective-agents>
- Anthropic cookbook, agent patterns — <https://github.com/anthropics/anthropic-cookbook/tree/main/patterns/agents>
