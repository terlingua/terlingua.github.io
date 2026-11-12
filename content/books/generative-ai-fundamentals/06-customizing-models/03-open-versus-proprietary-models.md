---
title: Open versus proprietary models
questions:
  - id: open-weights
    prompt: What does "open weights" mean?
    options:
      - text: The training data is public
      - text: The model is free for any use
      - id: weights
        text: The trained parameters are downloadable so you can run and fine-tune the model yourself; training data, code and licence terms vary
        correct: true
      - text: The model has no safety training
    explanation: The Open Source Initiative's Open Source AI Definition (2024) requires more — including sufficient information about training data.
  - id: licence
    prompt: Why read an open-weight model's licence?
    type: multiple
    options:
      - id: commercial
        text: Some restrict commercial use or impose user-count thresholds
        correct: true
      - id: aup
        text: Some include acceptable-use policies that bind you
        correct: true
      - id: derivative
        text: Some restrict using outputs to train other models or require attribution in derivative names
        correct: true
      - text: Licences never matter for AI models
    explanation: Meta's Llama licences, for instance, have included a monthly-active-user threshold and naming requirements.
  - id: why-open
    prompt: Which are genuine advantages of open-weight models?
    type: multiple
    options:
      - id: control
        text: Data stays in your environment; no third-party API
        correct: true
      - id: customize
        text: Full control to fine-tune, quantise and pin versions forever
        correct: true
      - id: cost-scale
        text: Potentially lower unit cost at high, steady volume
        correct: true
      - text: Always the highest quality available
    explanation: Frontier proprietary models have often led on the hardest tasks, though the gap has narrowed.
  - id: why-proprietary
    prompt: Which are genuine advantages of proprietary API models?
    options:
      - id: managed
        text: Often top capability, no infrastructure to run, fast access to new features — at the cost of dependency, per-token pricing and less control
        correct: true
      - text: They never change
      - text: They are always cheaper
      - text: Their training data is public
    explanation: Providers update and retire models on their schedule, not yours.
  - id: self-host-cost
    prompt: A team self-hosts a 70B model for 50 requests a day. What is the likely economic outcome?
    options:
      - text: Much cheaper than an API
      - text: Free
      - text: Identical to an API
      - id: expensive
        text: Expensive — GPUs sit mostly idle, so cost per request is far higher than pay-per-token APIs; self-hosting pays off at high utilisation
        correct: true
    explanation: Utilisation dominates self-hosting economics, plus engineering time.
  - id: small-models
    prompt: When can a small model (a few billion parameters) be the right choice?
    options:
      - text: Never
      - id: narrow-fast
        text: For narrow, well-defined tasks (classification, extraction, routing), on-device or low-latency use, often after fine-tuning
        correct: true
      - text: Only for poetry
      - text: Only when offline
    explanation: Use the smallest model that passes your evaluation.
  - id: routing
    prompt: What is model routing?
    options:
      - text: Network routing for GPUs
      - text: Choosing a cloud region
      - id: route
        text: Sending each request to the cheapest model likely to handle it — small models for easy requests, larger ones for hard ones
        correct: true
      - text: Load-balancing identical replicas
    explanation: Routing can cut cost substantially; it adds a classifier and evaluation burden.
  - id: abstraction
    prompt: What protects an application from any single model provider's changes?
    options:
      - text: Hard-coding one provider's SDK everywhere
      - id: abstract
        text: A thin abstraction over model calls (or a gateway), model names in configuration, and an evaluation suite to qualify replacements
        correct: true
      - text: Using only the newest model
      - text: Avoiding evaluation
    explanation: In .NET this is `IChatClient` from Microsoft.Extensions.AI; on Databricks, serving endpoints and the AI gateway play that role.
---
## Learning objectives

- Distinguish open weights from open source
- Weigh open and proprietary models on control, cost and capability
- Design for model replaceability

## Decision factors

| Factor | Favour open weights | Favour API |
|---|---|---|
| Data residency / sovereignty | ✓ | |
| Peak capability | | ✓ (often) |
| Steady high volume | ✓ | |
| Low or spiky volume | | ✓ |
| Team to run GPUs | Needed | Not needed |
| Version stability | ✓ (pin forever) | Provider schedule |

## Sources

- Open Source Initiative, "The Open Source AI Definition 1.0" — <https://opensource.org/ai/open-source-ai-definition>
- Meta Llama licence — <https://www.llama.com/llama3/license/>
- Ong et al., "RouteLLM", 2024 — <https://arxiv.org/abs/2406.18665>
