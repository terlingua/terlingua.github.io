---
title: Hallucination
questions:
  - id: definition
    prompt: What is a hallucination in LLM terms?
    options:
      - text: A visual glitch in image models only
      - id: def
        text: Fluent, confident output that is false or unsupported by the provided sources
        correct: true
      - text: Any creative writing
      - text: A model refusing to answer
    explanation: Some researchers prefer "confabulation". Either way, plausibility is not truth.
  - id: why
    prompt: Why do LLMs hallucinate?
    type: multiple
    options:
      - id: objective
        text: They are trained to produce probable text, not verified facts
        correct: true
      - id: incentive
        text: Training and benchmarks often reward guessing over saying "I don't know"
        correct: true
      - id: gaps
        text: Rare facts are poorly represented in training data
        correct: true
      - text: They are deliberately programmed to lie
    explanation: OpenAI's 2025 paper "Why Language Models Hallucinate" argues evaluation that penalises abstention encourages confident guessing.
  - id: avianca
    prompt: In Mata v. Avianca (2023), what happened?
    options:
      - text: An airline's chatbot sold free tickets
      - text: A model leaked passenger data
      - id: fake-cases
        text: Lawyers filed a brief citing non-existent court cases invented by ChatGPT, and were sanctioned
        correct: true
      - text: A court banned AI
    explanation: Hundreds of similar incidents have been documented since. Verification is the user's responsibility.
  - id: air-canada
    prompt: In Moffatt v. Air Canada (2024), what did the tribunal decide about the airline's chatbot?
    options:
      - text: The chatbot was a separate legal entity, so the airline was not liable
      - id: liable
        text: The airline was responsible for its chatbot's incorrect statement about bereavement fares and had to compensate the customer
        correct: true
      - text: Chatbots cannot make statements
      - text: The customer should have known better
    explanation: Organisations own what their AI tells customers.
  - id: mitigations
    prompt: Which techniques reduce hallucination?
    type: multiple
    options:
      - id: grounding
        text: Grounding answers in retrieved sources and requiring citations
        correct: true
      - id: abstain
        text: Instructing and allowing the model to say it does not know
        correct: true
      - id: tools
        text: Using tools (search, calculators, databases) for facts and computation
        correct: true
      - id: verify
        text: Automated groundedness checks and human review for high-stakes outputs
        correct: true
    explanation: None eliminates hallucination; combined, they reduce it to manageable rates for many uses.
  - id: rag-not-cure
    prompt: Does RAG eliminate hallucinations?
    options:
      - text: Yes, completely
      - text: No, it makes them worse in all cases
      - text: Only for images
      - id: reduces
        text: No — it reduces them, but models can still misread, over-generalise or ignore retrieved context, and bad retrieval feeds wrong context
        correct: true
    explanation: Stanford researchers found legal research tools marketed as "hallucination-free" still hallucinated in 17–33% of queries (Magesh et al., 2024).
  - id: citations-fake
    prompt: A model's answer includes a citation. What should you assume?
    options:
      - id: verify
        text: The citation might be fabricated or not support the claim; check that the source exists and says what is claimed
        correct: true
      - text: It is correct, because models cannot invent citations
      - text: Citations prove the answer
      - text: Only URLs can be fake
    explanation: In RAG systems, constrain citations to IDs of retrieved chunks so they can be validated mechanically.
  - id: confidence
    prompt: Is a model's stated confidence ("I am certain") reliable?
    options:
      - text: Yes, models know when they are right
      - id: not-calibrated
        text: Not reliably — verbal confidence is poorly calibrated, especially after alignment training
        correct: true
      - text: Only at temperature 0
      - text: Only for maths
    explanation: Use external verification, not the model's self-assessment, for important claims.
  - id: summary-halluc
    prompt: Can a model hallucinate when summarising a document you provide?
    options:
      - text: No, because the source is in the prompt
      - text: Only if the document is in another language
      - id: can-hallucinate
        text: Yes — adding details not in the document, merging facts, or inverting a negation are documented failure modes
        correct: true
      - text: Only for documents over 1 million tokens
    explanation: Vectara's hallucination leaderboard measures exactly this; rates differ substantially between models.
---
## Learning objectives

- Define hallucination and explain its causes
- Learn from real cases
- Apply layered mitigations

## Defence in depth

```text
Question
  │
  ├─ retrieve authoritative sources ─► ground the answer, cite chunk IDs
  ├─ use tools for facts & maths
  ├─ allow "I don't know"
  ├─ check groundedness automatically (LLM judge or NLI model)
  └─ human review where errors are costly
```

## Sources

- Kalai et al. (OpenAI), "Why Language Models Hallucinate", 2025 — <https://arxiv.org/abs/2509.04664>
- Mata v. Avianca, Inc., S.D.N.Y. 2023 — <https://law.justia.com/cases/federal/district-courts/new-york/nysdce/1:2022cv01461/575368/54/>
- Moffatt v. Air Canada, 2024 BCCRT 149 — <https://www.canlii.org/en/bc/bccrt/doc/2024/2024bccrt149/2024bccrt149.html>
- Magesh et al., "Hallucination-Free? Assessing the Reliability of Leading AI Legal Research Tools", 2024 — <https://arxiv.org/abs/2405.20362>
- Vectara hallucination leaderboard — <https://github.com/vectara/hallucination-leaderboard>
