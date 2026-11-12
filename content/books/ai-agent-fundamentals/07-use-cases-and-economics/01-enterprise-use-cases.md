---
title: Enterprise use cases
questions:
  - id: course-cases
    prompt: Which enterprise agent use cases are commonly cited across industries?
    type: multiple
    options:
      - id: support
        text: Customer support — resolving routine requests with system access, escalating others
        correct: true
      - id: documents
        text: Document-heavy processes — claims, invoices, contracts, KYC
        correct: true
      - id: coding
        text: Software engineering — coding agents
        correct: true
      - id: research
        text: Research and analysis across many sources
        correct: true
    explanation: Coding agents have the strongest adoption evidence because outcomes are verifiable.
  - id: good-traits
    prompt: Which traits make a process a good candidate for an agent?
    type: multiple
    options:
      - id: volume
        text: High volume of similar but not identical cases
        correct: true
      - id: verifiable
        text: Outcomes that can be checked
        correct: true
      - id: tools-exist
        text: Systems accessible via APIs the agent can use
        correct: true
      - text: Rare, high-stakes, one-off decisions
    explanation: Rare high-stakes decisions lack data to evaluate and carry outsized risk.
  - id: information-extraction
    prompt: Why is information extraction a popular first agent-adjacent project?
    options:
      - id: measurable
        text: It has clear inputs and outputs, is easy to evaluate against labelled examples, and replaces tedious manual work
        correct: true
      - text: It requires no data
      - text: It is fully autonomous
      - text: It never makes mistakes
    explanation: It is often a workflow rather than a true agent — which is fine.
  - id: coding-evidence
    prompt: What did METR's 2025 randomised trial with experienced open-source developers find?
    options:
      - text: AI tools made them 50% faster
      - text: No change at all
      - id: slower
        text: Developers using early-2025 AI tools took about 19% longer on their own mature repositories, despite believing they were faster
        correct: true
      - text: Developers refused to use AI
    explanation: A striking result for experts on familiar codebases; other studies show gains for other populations and tasks. Perception is not measurement.
  - id: knowledge-work
    prompt: Which is a realistic framing for agents in knowledge work today?
    options:
      - text: Full replacement of professional roles
      - id: augmentation
        text: Augmentation — agents draft, gather and pre-process; people decide, verify and own outcomes
        correct: true
      - text: No impact at all
      - text: Only for entertainment
    explanation: Accountability remains with people and organisations.
  - id: horizontal-vertical
    prompt: What is the difference between horizontal and vertical agents?
    options:
      - text: Their screen orientation
      - text: Their model sizes
      - text: Their cloud provider
      - id: scope
        text: Horizontal agents serve general tasks across functions (e.g. assistants); vertical agents are specialised for one domain or process with deep integration
        correct: true
    explanation: Vertical agents tend to show clearer ROI because success is easier to define.
  - id: customer-facing
    prompt: What extra considerations apply to customer-facing agents?
    type: multiple
    options:
      - id: liability
        text: The company is liable for what the agent says (cf. Moffatt v. Air Canada)
        correct: true
      - id: disclosure
        text: Disclosure that the customer is talking to AI
        correct: true
      - id: escalation
        text: Easy escalation to a human
        correct: true
      - text: No logging is needed
    explanation: Internal pilots first, then limited external rollout.
---
## Learning objectives

- Identify strong agent use cases
- Interpret productivity evidence critically
- Plan customer-facing deployments responsibly

## Use-case scoring

| Criterion | Weight | Score 1–5 |
|---|---|---|
| Volume and repetition | High | |
| Verifiability of outcome | High | |
| API access to systems | Medium | |
| Cost of errors (inverse) | High | |
| Data readiness | Medium | |
| Sponsor and process owner | Medium | |

## Sources

- METR, "Measuring the Impact of Early-2025 AI on Experienced Open-Source Developer Productivity", 2025 — <https://arxiv.org/abs/2507.09089>
- Databricks blog, "Master AI Agents with New Databricks Training" — <https://www.databricks.com/blog/master-ai-agents-new-databricks-training>
- Moffatt v. Air Canada, 2024 BCCRT 149 — <https://www.canlii.org/en/bc/bccrt/doc/2024/2024bccrt149/2024bccrt149.html>
