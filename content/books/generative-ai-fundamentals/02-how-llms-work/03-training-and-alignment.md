---
title: Pretraining, fine-tuning and alignment
questions:
  - id: pretraining-objective
    prompt: What is the pretraining objective of a GPT-style LLM?
    options:
      - text: Answer questions correctly
      - text: Classify sentiment
      - id: next-token
        text: Predict the next token across enormous amounts of text (self-supervised learning)
        correct: true
      - text: Follow instructions
    explanation: Everything else — knowledge, grammar, some reasoning — emerges as useful for predicting text.
  - id: base-vs-instruct
    prompt: What is the difference between a base model and an instruction-tuned model?
    options:
      - id: base-continues
        text: A base model continues text; an instruction-tuned model has been further trained to follow instructions and behave as an assistant
        correct: true
      - text: Base models are larger
      - text: Instruction models cannot generate text
      - text: There is no difference
    explanation: Ask a base model "What is the capital of France?" and it may continue with more quiz questions.
  - id: sft
    prompt: What is supervised fine-tuning (SFT)?
    options:
      - text: Training without data
      - text: Training by a supervisor watching the GPU
      - text: Pruning weights
      - id: sft
        text: Further training on curated input–output examples (such as instruction–response pairs) so the model imitates desired behaviour
        correct: true
    explanation: Quality beats quantity — small, high-quality SFT datasets can change behaviour substantially.
  - id: rlhf
    prompt: How does reinforcement learning from human feedback (RLHF) work at a high level?
    options:
      - text: Humans rewrite the model's weights by hand
      - id: rlhf
        text: Humans rank model outputs; a reward model learns those preferences; the LLM is optimised (e.g. with PPO) to produce outputs the reward model scores highly
        correct: true
      - text: The model reads human feedback forms at runtime
      - text: Users click thumbs up to retrain the model instantly
    explanation: Described in OpenAI's InstructGPT paper (Ouyang et al., 2022). A 1.3B InstructGPT was preferred by labellers over 175B GPT-3.
  - id: dpo
    prompt: What does Direct Preference Optimization (DPO) change compared with classic RLHF?
    options:
      - text: It removes the need for preference data
      - text: It requires more human labellers
      - id: no-reward-model
        text: It optimises the model directly on preference pairs without training a separate reward model or running RL, which is simpler and more stable
        correct: true
      - text: It only works for images
    explanation: Rafailov et al., 2023. Many open models use DPO or related methods.
  - id: sycophancy
    prompt: Which side effect is linked to optimising for human approval?
    options:
      - text: Faster inference
      - text: Smaller models
      - id: syco
        text: Sycophancy — telling users what they want to hear, agreeing with mistaken premises, or flattering
        correct: true
      - text: Better arithmetic
    explanation: Anthropic researchers documented sycophancy across RLHF-trained assistants (Sharma et al., 2023). Ask models to critique, and phrase questions neutrally.
  - id: rlvr
    prompt: What is reinforcement learning with verifiable rewards?
    options:
      - id: verifiable
        text: RL where rewards come from automatically checkable outcomes — unit tests passing, maths answers matching — rather than human preference; central to training reasoning models
        correct: true
      - text: RL verified by lawyers
      - text: A blockchain technique
      - text: Training on verified Twitter accounts
    explanation: It works best where correctness is checkable, which is one reason reasoning models improved fastest at maths and code.
  - id: data-sources
    prompt: Which statements about pretraining data are accurate?
    type: multiple
    options:
      - id: web
        text: Much of it is filtered web crawl (e.g. derived from Common Crawl), plus code, books and papers
        correct: true
      - id: undisclosed
        text: Frontier labs rarely disclose their exact training data
        correct: true
      - id: litigation
        text: Use of copyrighted material in training is the subject of ongoing lawsuits
        correct: true
      - text: All training data is licensed and public domain
    explanation: The legal landscape is unsettled and varies by jurisdiction; see the governance chapter.
  - id: cutoff
    prompt: What is a model's knowledge cut-off?
    options:
      - text: The maximum answer length
      - text: The date the model is retired
      - id: cutoff
        text: The approximate date after which events are not represented in its training data
        correct: true
      - text: The maximum context length
    explanation: Models also struggle near the cut-off because recent events are sparsely represented. Use retrieval or search for current facts.
  - id: cost-pretraining
    prompt: Why do almost no enterprises pretrain their own LLMs?
    options:
      - text: It is illegal
      - text: It requires no data
      - text: It is trivial and not worth mentioning
      - id: cost
        text: Pretraining competitive models costs tens to hundreds of millions of dollars in compute, data and expertise; adapting existing models is far cheaper
        correct: true
    explanation: Databricks' own DBRX (2024) is a reminder — a capable model was overtaken within months and later retired.
---
## Learning objectives

- Describe pretraining, SFT and preference optimisation
- Understand side effects of alignment such as sycophancy
- Know why enterprises adapt rather than pretrain

## The pipeline

```text
Raw text (trillions of tokens) ──pretraining──► Base model (predicts text)
        curated instructions ──SFT──────────► Instruction-tuned model
     human / AI preferences ──RLHF / DPO───► Aligned assistant
     verifiable tasks (code, maths) ──RL───► Reasoning model
```

## Why this matters to users

- A model's "personality" — helpful, cautious, verbose, agreeable — is trained behaviour.
- Agreeableness can become sycophancy: prefer neutral questions ("evaluate this plan") over
  leading ones ("this plan is great, right?").
- Knowledge is frozen at the cut-off; behaviour can change silently when the provider updates the model.

## Sources

- Ouyang et al., "Training language models to follow instructions with human feedback", 2022 — <https://arxiv.org/abs/2203.02155>
- Rafailov et al., "Direct Preference Optimization", 2023 — <https://arxiv.org/abs/2305.18290>
- Sharma et al., "Towards Understanding Sycophancy in Language Models", 2023 — <https://arxiv.org/abs/2310.13548>
- DeepSeek-AI, "DeepSeek-R1", 2025 — <https://arxiv.org/abs/2501.12948>
