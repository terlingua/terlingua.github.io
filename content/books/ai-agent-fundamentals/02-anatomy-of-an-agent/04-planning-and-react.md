---
title: Planning and ReAct
questions:
  - id: react
    prompt: What does the ReAct pattern combine?
    options:
      - text: React.js and Python
      - id: reason-act
        text: Interleaved reasoning ("thought") and actions (tool calls), with observations fed back, so the model can plan, act and adjust
        correct: true
      - text: Reactive programming and databases
      - text: Reinforcement learning and actors
    explanation: Yao et al., 2022. Modern tool-calling APIs implement the same loop natively.
  - id: plan-execute
    prompt: What is the plan-and-execute pattern?
    options:
      - text: Executing without a plan
      - text: Asking the user for a plan
      - id: plan-first
        text: A planner produces a multi-step plan up front; an executor carries out steps (possibly with a cheaper model), and the plan is revised if steps fail
        correct: true
      - text: Running the same step repeatedly
    explanation: Upfront plans are inspectable and approvable — useful for human oversight — but can be brittle if the world differs from expectations.
  - id: reflection
    prompt: What is reflection (e.g. Reflexion)?
    options:
      - id: self-critique
        text: The agent critiques its own outputs or failed attempts and uses the critique to improve the next attempt
        correct: true
      - text: Mirroring the user's tone
      - text: Reflecting data into another table
      - text: A security attack
    explanation: Shinn et al., 2023. Self-critique works best with external feedback (tests, validators); without it, models may confidently "fix" correct answers.
  - id: feedback
    prompt: Why do coding agents work comparatively well?
    options:
      - text: Code is easy
      - text: Programmers write better prompts
      - text: Coding models are larger
      - id: verifiable
        text: Compilers, tests and linters give fast, objective feedback at each step, letting the agent detect and correct errors
        correct: true
    explanation: Design other agents to obtain similar ground-truth feedback wherever possible.
  - id: stuck
    prompt: An agent repeatedly calls the same tool with the same arguments. What safeguards help?
    type: multiple
    options:
      - id: loop-detect
        text: Detecting repeated actions and intervening
        correct: true
      - id: budgets
        text: Step, time and token budgets
        correct: true
      - id: escalate
        text: Escalating to a human when stuck
        correct: true
      - text: Raising the step limit to infinity
    explanation: Loops are a common, expensive failure mode.
  - id: decomposition
    prompt: What is task decomposition?
    options:
      - text: Deleting tasks
      - id: subtasks
        text: Breaking a complex goal into smaller sub-tasks that are easier to solve and verify individually
        correct: true
      - text: Splitting a model into layers
      - text: Compressing context
    explanation: Decomposition can be fixed in code (workflow) or produced by the model (planning).
  - id: ask-user
    prompt: When should an agent ask the user instead of proceeding?
    options:
      - text: Never; autonomy means no questions
      - text: Before every step
      - id: ambiguity
        text: When the request is ambiguous in a way that changes the outcome, or before irreversible actions
        correct: true
      - text: Only when the model's confidence is above 90%
    explanation: Give the agent an explicit `ask_user` tool and guidance on when to use it.
  - id: tree-search
    prompt: What does Tree of Thoughts add?
    options:
      - id: explore
        text: Exploring several reasoning branches, evaluating them, and backtracking — search over thoughts instead of a single chain
        correct: true
      - text: A visual tree diagram for users
      - text: A database index
      - text: Storing thoughts in a B-tree
    explanation: Yao et al., 2023. Powerful for puzzles; often too expensive for routine business tasks.
---
## Learning objectives

- Explain ReAct, plan-and-execute and reflection
- Design feedback and stopping conditions
- Know when to ask the user

## ReAct trace

```text
Thought: The user wants the delivery date for order A-1042.
Action: lookup_order(order_id="A-1042")
Observation: {"status": "shipped", "carrier": "UPS", "tracking": "1Z999"}
Thought: I need the carrier's estimate.
Action: track_shipment(carrier="UPS", tracking="1Z999")
Observation: {"eta": "2026-11-14"}
Answer: Your order shipped with UPS and should arrive on 14 November.
```

## Sources

- Yao et al., "ReAct: Synergizing Reasoning and Acting in Language Models", 2022 — <https://arxiv.org/abs/2210.03629>
- Shinn et al., "Reflexion", 2023 — <https://arxiv.org/abs/2303.11366>
- Yao et al., "Tree of Thoughts", 2023 — <https://arxiv.org/abs/2305.10601>
- Wang et al., "Plan-and-Solve Prompting", 2023 — <https://arxiv.org/abs/2305.04091>
