---
title: The AI family tree
questions:
  - id: nesting
    prompt: Which nesting is correct?
    options:
      - text: Machine learning ⊃ AI ⊃ deep learning ⊃ generative AI
      - text: Generative AI ⊃ deep learning ⊃ machine learning ⊃ AI
      - id: nest
        text: AI ⊃ machine learning ⊃ deep learning ⊃ (most of today's) generative AI
        correct: true
      - text: They are four unrelated fields
    explanation: Generative models predate deep learning (for example Markov chains, Gaussian mixture models), but today's GenAI is deep-learning based.
  - id: ml-def
    prompt: What distinguishes machine learning from rule-based AI?
    options:
      - id: learn
        text: ML learns patterns from data rather than following rules explicitly written by people
        correct: true
      - text: ML always uses neural networks
      - text: ML cannot make predictions
      - text: Rule-based systems are always worse
    explanation: Rule-based systems are still the right answer for many deterministic tasks — tax rules, validation, routing.
  - id: generative-discriminative
    prompt: What is the difference between discriminative and generative models?
    options:
      - text: Generative models are always larger
      - text: Discriminative models generate images
      - text: There is no difference
      - id: gen-disc
        text: Discriminative models learn to separate classes (p(y|x)), e.g. spam or not; generative models learn the data distribution well enough to produce new samples (p(x) or p(x|context))
        correct: true
    explanation: An LLM is generative — it models the probability of the next token given the previous ones.
  - id: modalities
    prompt: Which are examples of generative AI output modalities?
    type: multiple
    options:
      - id: text
        text: Text and code
        correct: true
      - id: images
        text: Images and video
        correct: true
      - id: audio
        text: Speech and music
        correct: true
      - id: structured
        text: Structured data such as JSON or synthetic tables
        correct: true
    explanation: Multimodal models accept and produce several modalities in one model.
  - id: foundation-model
    prompt: What is a foundation model?
    options:
      - text: A model for building foundations in construction
      - id: fm
        text: A large model trained on broad data at scale that can be adapted to many downstream tasks
        correct: true
      - text: The first model a company trains
      - text: A small model for a single task
    explanation: The term was popularised by Stanford's Center for Research on Foundation Models in 2021 (Bommasani et al.).
  - id: llm
    prompt: What does the "large" in "large language model" refer to?
    options:
      - text: The physical size of the server
      - text: The length of answers
      - id: params-data
        text: The number of parameters (billions or more) and the volume of training data (trillions of tokens)
        correct: true
      - text: The number of languages supported
    explanation: Parameter counts for frontier proprietary models are mostly undisclosed; open-weight models publish them.
  - id: diffusion
    prompt: Which architecture underlies most modern image generators?
    options:
      - text: Decision trees
      - text: Recurrent neural networks
      - text: Linear regression
      - id: diffusion
        text: Diffusion models, which learn to reverse a gradual noising process, often combined with transformers
        correct: true
    explanation: LLMs are transformer-based autoregressive models; image generation mostly uses diffusion. The two converge in multimodal systems.
  - id: not-genai
    prompt: Which of these tasks usually does NOT need generative AI?
    options:
      - text: Drafting a reply to a customer email
      - text: Summarising a long contract
      - id: forecast
        text: Forecasting next month's sales from historical numbers
        correct: true
      - text: Generating product descriptions
    explanation: Classical ML and statistics often beat LLMs on tabular prediction at a fraction of the cost. Use GenAI where language or content generation is the job.
  - id: ai-agents-link
    prompt: How do AI agents relate to generative AI?
    options:
      - id: built-on
        text: Agents use generative models (usually LLMs) as a reasoning component, combined with tools, memory and control logic, to take actions toward goals
        correct: true
      - text: Agents are unrelated to GenAI
      - text: Agents replaced LLMs
      - text: Agents are a type of database
    explanation: The AI Agent Fundamentals book covers this in depth.
---
## Learning objectives

- Place generative AI within AI, ML and deep learning
- Distinguish generative from discriminative models
- Recognise tasks that do not need GenAI

## The family tree

```text
Artificial intelligence  — systems performing tasks that need "intelligence"
└── Machine learning     — learns from data
    └── Deep learning    — multi-layer neural networks
        └── Generative AI — models that produce new content
            ├── Large language models (text, code)
            ├── Diffusion models (images, video)
            └── Multimodal models (several modalities)
```

## Choosing the right tool

| Task | Usually best |
|---|---|
| Deterministic business rules | Code |
| Tabular prediction (churn, demand) | Classical ML (gradient boosting) |
| Search over known documents | Search engines, plus embeddings |
| Drafting, summarising, extracting from text | LLMs |
| Open-ended conversation | LLMs |

## Sources

- Bommasani et al., "On the Opportunities and Risks of Foundation Models", 2021 — <https://arxiv.org/abs/2108.07258>
- Ho, Jain, Abbeel, "Denoising Diffusion Probabilistic Models", 2020 — <https://arxiv.org/abs/2006.11239>
- Databricks, Generative AI Fundamentals — <https://www.databricks.com/learn/training/generative-ai-fundamentals-accreditation>
