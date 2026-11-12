---
title: Bias, context and reasoning limits
questions:
  - id: bias-source
    prompt: Where does bias in LLM outputs come from?
    type: multiple
    options:
      - id: data
        text: Patterns in training data reflecting societal biases and over-representation of some languages and cultures
        correct: true
      - id: labelling
        text: Choices made by annotators and alignment processes
        correct: true
      - id: deployment
        text: How the model is prompted and deployed, including which users it serves
        correct: true
      - text: Nowhere; models are neutral by construction
    explanation: Bias testing on your use case — for example comparing outputs across demographic variations — is part of responsible deployment.
  - id: lost-middle
    prompt: What did the "Lost in the Middle" study find?
    options:
      - text: Models forget the beginning of long prompts
      - id: middle
        text: Models used information at the beginning and end of long contexts better than information in the middle
        correct: true
      - text: Models perform equally across positions
      - text: Long context always improves accuracy
    explanation: Liu et al., 2023. Newer models improved, but "context rot" — degradation as inputs grow — is still observed. Put key instructions and evidence where they count and keep context lean.
  - id: needle
    prompt: Why is a "needle in a haystack" score a weak measure of long-context ability?
    options:
      - text: It is too hard
      - text: It uses images
      - id: easy
        text: Finding one verbatim sentence is much easier than reasoning across many scattered facts; models that ace it can still fail realistic long-context tasks
        correct: true
      - text: It measures cost, not quality
    explanation: Benchmarks like RULER and multi-hop tests show larger drops than simple retrieval.
  - id: reasoning
    prompt: Which statement about LLM reasoning is best supported by evidence?
    options:
      - text: LLMs never reason
      - text: LLMs reason exactly like humans
      - id: brittle
        text: LLMs can solve many multi-step problems, but performance can be brittle — sensitive to irrelevant details or rephrasing — suggesting partial reliance on pattern matching
        correct: true
      - text: Reasoning ability is fixed and cannot improve
    explanation: Apple's GSM-Symbolic study (Mirzadeh et al., 2024) showed drops when numbers or irrelevant clauses changed. Reasoning models narrowed but did not close such gaps.
  - id: cot-faithful
    prompt: Does a model's displayed chain of thought faithfully show how it reached its answer?
    options:
      - text: Yes, always
      - id: not-always
        text: Not necessarily — research shows stated reasoning can omit factors that actually influenced the answer
        correct: true
      - text: Chains of thought are random text
      - text: Only for small models
    explanation: Anthropic (2025) found reasoning models often did not mention hints they used. Treat reasoning text as helpful but not as an audit log.
  - id: drift
    prompt: Your prompt worked last month but quality dropped this week without code changes. What is a plausible cause?
    options:
      - text: Prompts expire
      - text: The internet changed
      - text: Tokens wore out
      - id: model-update
        text: The provider updated the model behind an alias; behaviour of the "same" model can drift over time
        correct: true
    explanation: Chen, Zaharia and Zou (2023) documented GPT-4 behaviour changes over months. Pin versions and run regression evaluations.
  - id: math
    prompt: What is the most reliable way to get exact arithmetic from an LLM application?
    options:
      - text: Ask the model to double-check
      - text: Use a larger model
      - text: Set temperature to 0
      - id: tool
        text: Have the model call a code interpreter or calculator and use the tool's result
        correct: true
    explanation: Tools convert a probabilistic problem into a deterministic one.
  - id: injection-link
    prompt: Why can't an LLM reliably tell instructions from data in its context?
    options:
      - id: one-stream
        text: Everything — system prompt, user input, retrieved documents — is one token sequence; there is no hard boundary separating "code" from "data"
        correct: true
      - text: It can, perfectly
      - text: Because of temperature
      - text: Because it is stateless
    explanation: This is the root of prompt injection, covered in the next chapter.
  - id: anthropomorphism
    prompt: Why is anthropomorphising chatbots risky?
    options:
      - text: It is not risky
      - id: overtrust
        text: Human-like language encourages over-trust, emotional reliance and attributing understanding or intent that may not be there
        correct: true
      - text: It makes models slower
      - text: It violates copyright
    explanation: The "ELIZA effect" was observed in 1966 with a far simpler program.
---
## Learning objectives

- Recognise sources of bias
- Understand long-context degradation
- Know what research says about LLM reasoning and its faithfulness

## Practical rules

- Keep prompts focused; more context is not always better
- Put critical instructions at the start (and repeat key constraints at the end for long inputs)
- Use tools for computation and lookups
- Pin model versions and run regression evaluations on every change
- Test outputs across demographic and linguistic variations

## Sources

- Liu et al., "Lost in the Middle", 2023 — <https://arxiv.org/abs/2307.03172>
- Hsieh et al., "RULER: What's the Real Context Size of Your Long-Context Language Models?", 2024 — <https://arxiv.org/abs/2404.06654>
- Mirzadeh et al., "GSM-Symbolic", 2024 — <https://arxiv.org/abs/2410.05229>
- Anthropic, "Reasoning models don't always say what they think", 2025 — <https://www.anthropic.com/research/reasoning-models-dont-say-think>
- Chen, Zaharia, Zou, "How Is ChatGPT's Behavior Changing over Time?", 2023 — <https://arxiv.org/abs/2307.09009>
- Chroma, "Context Rot", 2025 — <https://research.trychroma.com/context-rot>
