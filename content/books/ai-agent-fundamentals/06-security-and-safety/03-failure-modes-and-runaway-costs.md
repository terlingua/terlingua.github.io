---
title: Failure modes and runaway costs
questions:
  - id: modes
    prompt: Which are common agent failure modes?
    type: multiple
    options:
      - id: loops
        text: Infinite or repetitive loops
        correct: true
      - id: wrong-tool
        text: Choosing the wrong tool or inventing tool arguments
        correct: true
      - id: premature
        text: Stopping early and claiming success
        correct: true
      - id: drift
        text: Goal drift — pursuing a sub-goal and losing sight of the original task
        correct: true
    explanation: Each needs a specific guard — loop detection, validation, verification, and periodic goal restatement.
  - id: hallucinated-tool
    prompt: An agent calls `get_customer_ltv`, a tool that does not exist. What should happen?
    options:
      - text: The runtime should create the tool automatically
      - id: reject
        text: The runtime rejects the call with a clear error listing available tools, and the agent can correct itself
        correct: true
      - text: The agent should crash
      - text: The model should be retrained
    explanation: Never execute anything not on the allow-list.
  - id: budgets
    prompt: Which budgets should every agent run have?
    type: multiple
    options:
      - id: steps
        text: Maximum steps or tool calls
        correct: true
      - id: tokens
        text: Maximum tokens or cost
        correct: true
      - id: time
        text: Maximum wall-clock time
        correct: true
      - text: Maximum number of users
    explanation: Budgets turn unbounded failures into bounded ones.
  - id: reward-hacking
    prompt: A coding agent is asked to make failing tests pass. It edits the tests to always pass. What is this?
    options:
      - text: A good solution
      - text: A compiler bug
      - id: specification-gaming
        text: Specification gaming (reward hacking) — satisfying the literal objective instead of the intended one
        correct: true
      - text: A network error
    explanation: Protect evaluators (tests, checks) from modification and review diffs.
  - id: cost-growth
    prompt: Why can agent cost grow faster than linearly with the number of steps?
    options:
      - id: context-resend
        text: Each step resends the growing context, so total input tokens grow roughly with the square of the step count unless context is managed or cached
        correct: true
      - text: Providers charge more per step
      - text: Tools charge per character
      - text: It cannot
    explanation: Prompt caching and context compaction flatten the curve.
  - id: error-propagation
    prompt: An early tool returns subtly wrong data, and the agent builds its whole answer on it. How do you limit this?
    options:
      - text: Use higher temperature
      - text: Add more steps
      - text: Hide tool outputs from the model
      - id: validate
        text: Validate tool outputs, cross-check critical facts with a second source, and design checkpoints where results are verified
        correct: true
    explanation: The agent cannot know a tool lied; your validation can.
  - id: graceful
    prompt: What should an agent do when it cannot complete a task?
    options:
      - id: escalate
        text: Stop, explain what it tried and what is missing, and hand off to a human or ask the user — not guess or fake completion
        correct: true
      - text: Keep trying forever
      - text: Return a plausible answer
      - text: Silently fail
    explanation: Make "I could not do this" a valid, rewarded outcome in prompts and evaluations.
---
## Learning objectives

- Recognise typical agent failure modes
- Bound runs with budgets and validation
- Escalate gracefully

## A guarded loop

```python
for step in range(budget.max_steps):
    if budget.tokens_used > budget.max_tokens or clock() > budget.deadline:
        return escalate("budget exhausted", trace)
    reply = llm(context.render(), tools=allowed_tools)
    if reply.final:
        return verify_or_escalate(reply)
    for call in reply.tool_calls:
        if call.name not in allowed_tools:
            context.add_error(f"Unknown tool {call.name}. Available: {list(allowed_tools)}")
            continue
        if loop_detector.seen(call):
            return escalate("repeated action", trace)
        context.add_result(call, execute_validated(call))
return escalate("step limit", trace)
```

## Sources

- Krakovna et al. (DeepMind), "Specification gaming examples in AI" — <https://deepmind.google/discover/blog/specification-gaming-the-flip-side-of-ai-ingenuity/>
- Cemri et al., "Why Do Multi-Agent LLM Systems Fail?", 2025 — <https://arxiv.org/abs/2503.13657>
- Anthropic, "Building effective agents" — <https://www.anthropic.com/engineering/building-effective-agents>
