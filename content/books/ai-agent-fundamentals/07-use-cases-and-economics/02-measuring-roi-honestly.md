---
title: Measuring ROI honestly
questions:
  - id: baseline
    prompt: What must you measure before deploying an agent to claim ROI?
    options:
      - id: baseline
        text: The current baseline — time, cost, error rate and throughput of the existing process
        correct: true
      - text: The model's parameter count
      - text: The number of demos given
      - text: Social media mentions
    explanation: Without a baseline, any claimed improvement is anecdote.
  - id: full-cost
    prompt: Which belong in the cost side of agent ROI?
    type: multiple
    options:
      - id: inference
        text: Model inference and tool costs
        correct: true
      - id: build
        text: Build, integration and evaluation effort
        correct: true
      - id: oversight
        text: Human review and exception handling
        correct: true
      - id: ops
        text: Monitoring, incidents and model migrations
        correct: true
    explanation: Inference is often the smallest item for internal agents.
  - id: cost-per-success
    prompt: Which unit metric best compares an agent with the human process?
    options:
      - text: Cost per token
      - text: Cost per request
      - id: per-success
        text: Fully loaded cost per successfully completed task, including review and rework
        correct: true
      - text: Cost per GPU hour
    explanation: An agent that is cheap per request but fails often may cost more per success.
  - id: ab-test
    prompt: How can you attribute improvement to the agent rather than to other changes?
    options:
      - text: Compare this quarter to last year
      - id: controlled
        text: Run a controlled comparison — randomised or staggered rollout with a control group
        correct: true
      - text: Ask the vendor
      - text: Survey users about how they feel
    explanation: Self-reported time savings are systematically optimistic (see the METR study).
  - id: quality-side
    prompt: Besides cost and speed, what must ROI measurement include?
    options:
      - text: Nothing else
      - text: Only employee satisfaction
      - text: Only token counts
      - id: quality
        text: Quality and risk outcomes — error rates, complaints, compliance issues — so speed gains are not bought with hidden failures
        correct: true
    explanation: Faster wrong answers are not a gain.
  - id: kill-criteria
    prompt: Why define stop criteria for an agent pilot in advance?
    options:
      - id: sunk-cost
        text: To avoid the sunk-cost trap — continuing a project that is not meeting its targets because effort has already been invested
        correct: true
      - text: Pilots should never stop
      - text: Regulators require it
      - text: To save GPU memory
    explanation: Gartner's cancellation forecast suggests many projects should stop earlier.
---
## Learning objectives

- Build an honest ROI model for agents
- Use controlled comparisons
- Define stop criteria

## An ROI sketch

```text
Benefit per month = tasks × (baseline cost per task − agent cost per successful task) − cost of agent errors
Agent cost per successful task = (inference + tools + review time × wage + rework) / successful tasks
Payback = (build + integration + evaluation) / monthly benefit
```

## Sources

- METR, "Measuring the Impact of Early-2025 AI on Experienced Open-Source Developer Productivity", 2025 — <https://arxiv.org/abs/2507.09089>
- Gartner, agentic AI cancellations forecast, 2025 — <https://www.gartner.com/en/newsroom/press-releases/2025-06-25-gartner-predicts-over-40-percent-of-agentic-ai-projects-will-be-canceled-by-end-of-2027>
- Brynjolfsson, Li, Raymond, "Generative AI at Work", 2025 — <https://academic.oup.com/qje/article/140/2/889/7990658>
