---
title: When not to build an agent
questions:
  - id: gartner
    prompt: What did Gartner predict in June 2025 about agentic AI projects?
    options:
      - text: All will succeed by 2026
      - id: forty
        text: Over 40% will be cancelled by the end of 2027 due to escalating costs, unclear business value or inadequate risk controls
        correct: true
      - text: Agents will replace all software by 2027
      - text: No change from traditional projects
    explanation: Predictions are not facts, but the stated reasons — cost, value, risk — are the ones to plan against.
  - id: compounding
    prompt: Each step of a 20-step agent task succeeds independently with 95% probability. Roughly how likely is the whole task to succeed?
    options:
      - text: 95%
      - text: 75%
      - id: thirty-six
        text: About 36% (0.95²⁰)
        correct: true
      - text: 5%
    explanation: Errors compound across steps. Reliability per step must be very high, or steps must be verified and recoverable.
  - id: red-flags
    prompt: Which are red flags that an agent is the wrong tool?
    type: multiple
    options:
      - id: deterministic
        text: The process is fully specified and deterministic
        correct: true
      - id: zero-error
        text: Errors are catastrophic and cannot be checked or undone
        correct: true
      - id: no-eval
        text: There is no way to tell whether the agent succeeded
        correct: true
      - id: latency
        text: Strict low-latency requirements in a user-facing path
        correct: true
    explanation: Each pushes toward code, workflows or human processes instead.
  - id: cheaper-alternatives
    prompt: A team wants an agent to "answer questions about our sales data". What should they consider first?
    options:
      - id: simpler
        text: Whether a dashboard, a governed text-to-SQL tool (like a Genie space) or a few parameterised reports already meet the need
        correct: true
      - text: A five-agent system
      - text: Fine-tuning a model on the sales database
      - text: Nothing; agents are always best
    explanation: Agents should earn their complexity.
  - id: process-first
    prompt: Why does automating a broken process with an agent rarely work?
    options:
      - text: Agents refuse broken processes
      - text: Agents only work on new processes
      - text: It is illegal
      - id: amplify
        text: Agents amplify the process's ambiguity and data problems; redesigning the process often delivers more value than automating it
        correct: true
    explanation: Workflow redesign was the strongest predictor of value in several 2025 enterprise surveys.
  - id: reliability-metric
    prompt: Sierra's τ-bench introduced pass^k. What does it measure?
    options:
      - text: The pass rate on the first attempt only
      - id: consistency
        text: The probability that an agent succeeds on all k independent trials of the same task — exposing inconsistency that single-run accuracy hides
        correct: true
      - text: The number of passes through a loop
      - text: Password strength
    explanation: Agents that succeed 60% of the time on one try can succeed far less often when consistency across 8 tries is required.
  - id: human-cost
    prompt: Which costs are often missed when estimating an agent's ROI?
    type: multiple
    options:
      - id: review
        text: Time humans spend reviewing and correcting agent outputs
        correct: true
      - id: incidents
        text: Cost of incidents caused by wrong actions
        correct: true
      - id: maintenance
        text: Ongoing evaluation and maintenance as models and tools change
        correct: true
      - text: The agent's salary
    explanation: If reviewing takes as long as doing, the agent adds cost.
---
## Learning objectives

- Recognise when an agent is the wrong tool
- Understand compounding error and consistency
- Account for hidden costs

## Decision checklist

| Question | If "no" |
|---|---|
| Can we define and measure success? | Don't build yet |
| Are steps unpredictable enough to need LLM planning? | Use a workflow |
| Can wrong actions be detected and undone? | Add human approval or don't automate |
| Is per-step reliability high on our tasks? | Narrow the scope |
| Is the process itself sound? | Fix the process first |

## Sources

- Gartner, "Gartner Predicts Over 40% of Agentic AI Projects Will Be Canceled by End of 2027", 2025 — <https://www.gartner.com/en/newsroom/press-releases/2025-06-25-gartner-predicts-over-40-percent-of-agentic-ai-projects-will-be-canceled-by-end-of-2027>
- Yao et al., "τ-bench", 2024 — <https://arxiv.org/abs/2406.12045>
- Anthropic, "Building effective agents" — <https://www.anthropic.com/engineering/building-effective-agents>
