---
title: A short history
questions:
  - id: transformer-year
    prompt: Which 2017 paper introduced the transformer architecture?
    options:
      - text: "\"ImageNet Classification with Deep Convolutional Neural Networks\""
      - id: attention
        text: "\"Attention Is All You Need\" (Vaswani et al., Google)"
        correct: true
      - text: "\"Deep Residual Learning\""
      - text: "\"Generative Adversarial Nets\""
    explanation: The transformer replaced recurrence with self-attention, allowing parallel training on huge datasets.
  - id: why-transformer
    prompt: Why did transformers scale better than recurrent neural networks (RNNs)?
    options:
      - text: They use less memory for long sequences
      - id: parallel
        text: All positions in a sequence are processed in parallel during training, which suits GPUs; RNNs process tokens sequentially
        correct: true
      - text: They do not need training data
      - text: They are rule-based
    explanation: Self-attention's cost grows quadratically with sequence length, which is why long context remains expensive.
  - id: gpt
    prompt: What does GPT stand for?
    options:
      - text: General Purpose Transformer
      - text: Graph Processing Technique
      - id: gpt
        text: Generative Pre-trained Transformer
        correct: true
      - text: Global Prediction Tool
    explanation: OpenAI's GPT-1 (2018) showed generative pre-training plus fine-tuning worked; GPT-3 (2020, 175B parameters) showed in-context few-shot learning.
  - id: bert
    prompt: How did BERT (2018) differ from GPT?
    options:
      - id: encoder
        text: BERT is an encoder-only model trained to fill in masked tokens, strong for understanding tasks and embeddings; GPT is decoder-only and generates text left to right
        correct: true
      - text: BERT generated images
      - text: BERT was rule-based
      - text: There is no difference
    explanation: Encoder models remain widely used for classification and embeddings.
  - id: chatgpt
    prompt: What made ChatGPT (November 2022) a turning point?
    options:
      - text: It was the first neural network
      - text: It was open source
      - text: It ran on phones without internet
      - id: chat
        text: Instruction-tuning and RLHF turned a base model into a helpful conversational assistant, exposed through a free chat interface — reaching a reported 100 million users in about two months
        correct: true
    explanation: The underlying capabilities existed in GPT-3.5; the interface and alignment made them usable by everyone.
  - id: scaling-laws
    prompt: What do neural scaling laws describe?
    options:
      - text: How to scale a Kubernetes cluster
      - id: power-law
        text: Predictable improvements in loss as model size, data and compute increase, roughly following power laws (Kaplan et al., 2020)
        correct: true
      - text: Laws regulating AI
      - text: The speed of light limit for GPUs
    explanation: Chinchilla (Hoffmann et al., 2022) showed many models were under-trained; compute-optimal training uses roughly 20 tokens per parameter.
  - id: open-weights
    prompt: Which model families made open-weight LLMs mainstream?
    type: multiple
    options:
      - id: llama
        text: Meta's Llama
        correct: true
      - id: mistral
        text: Mistral's models
        correct: true
      - id: qwen-deepseek
        text: Qwen (Alibaba) and DeepSeek
        correct: true
      - text: Microsoft Excel
    explanation: "Open weights is not the same as open source: training data and code are often unreleased and licences may restrict use."
  - id: reasoning-models
    prompt: What characterises "reasoning models" such as OpenAI o1 (2024) and successors?
    options:
      - text: They use no neural networks
      - text: They cannot produce text
      - id: think
        text: They are trained, often with reinforcement learning, to generate long intermediate reasoning before answering, trading more inference compute for better results on maths, code and multi-step problems
        correct: true
      - text: They guarantee correct answers
    explanation: This shifted some scaling from training compute to "test-time compute". They are slower and costlier per answer.
  - id: hype-cycles
    prompt: Which statement about AI history is accurate?
    options:
      - text: AI progress has been steady and uninterrupted since 1950
      - text: Deep learning was invented in 2022
      - id: winters
        text: AI has had "winters" — periods of reduced funding after over-promising (notably the 1970s and late 1980s) — a reminder that expectations and capabilities can diverge
        correct: true
      - text: AI has never been over-hyped
    explanation: Current capabilities are real; so is the tendency to overestimate near-term impact.
  - id: deepseek
    prompt: What did DeepSeek-R1 (January 2025) demonstrate?
    options:
      - id: efficient
        text: That a competitive open-weight reasoning model could be trained with reinforcement learning at reportedly much lower cost than assumed, intensifying price competition
        correct: true
      - text: That transformers do not work
      - text: That RL is useless for LLMs
      - text: That closed models are always better
    explanation: Reported training costs excluded prior research and infrastructure; still, it shifted assumptions about cost and openness.
---
## Learning objectives

- Trace the milestones from transformers to reasoning models
- Understand scaling laws and why they mattered
- Separate open weights from open source

## Timeline

| Year | Milestone |
|---|---|
| 2013 | word2vec — word embeddings |
| 2014 | GANs; sequence-to-sequence with attention |
| 2017 | Transformer ("Attention Is All You Need") |
| 2018 | GPT-1, BERT |
| 2020 | GPT-3; scaling laws; RAG paper; diffusion models |
| 2022 | InstructGPT/RLHF; Chinchilla; ChatGPT |
| 2023 | GPT-4; Llama; open-weight explosion |
| 2024 | Long context; multimodal; reasoning models (o1); MCP |
| 2025 | DeepSeek-R1; agents mainstream; MLflow 3 |
| 2026 | Agent platforms, protocols under foundations; regulation phases in |

## Sources

- Vaswani et al., "Attention Is All You Need", 2017 — <https://arxiv.org/abs/1706.03762>
- Devlin et al., "BERT", 2018 — <https://arxiv.org/abs/1810.04805>
- Brown et al., "Language Models are Few-Shot Learners", 2020 — <https://arxiv.org/abs/2005.14165>
- Kaplan et al., "Scaling Laws for Neural Language Models", 2020 — <https://arxiv.org/abs/2001.08361>
- Hoffmann et al., "Training Compute-Optimal Large Language Models", 2022 — <https://arxiv.org/abs/2203.15556>
- DeepSeek-AI, "DeepSeek-R1", 2025 — <https://arxiv.org/abs/2501.12948>
