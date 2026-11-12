---
title: The customisation ladder
questions:
  - id: order
    prompt: Which order goes from cheapest/fastest to most expensive way of adapting an LLM?
    options:
      - text: Pretraining → fine-tuning → RAG → prompting
      - text: Fine-tuning → prompting → pretraining → RAG
      - id: ladder
        text: Prompt engineering → RAG → fine-tuning → continued pretraining → pretraining from scratch
        correct: true
      - text: RAG → pretraining → prompting → fine-tuning
    explanation: Climb only as far as evaluation shows you must.
  - id: when-ft
    prompt: Which goals are good reasons to fine-tune?
    type: multiple
    options:
      - id: format
        text: Consistent output format, tone or style that prompting cannot achieve reliably
        correct: true
      - id: narrow
        text: High-volume narrow tasks where a small fine-tuned model can replace a large expensive one
        correct: true
      - id: domain-language
        text: Specialised vocabulary or task patterns the base model handles poorly
        correct: true
      - text: Teaching the model facts that change every week
    explanation: For changing facts, use retrieval.
  - id: ft-knowledge
    prompt: Does fine-tuning reliably add new factual knowledge?
    options:
      - id: unreliable
        text: Not reliably — studies found models learn new facts slowly via fine-tuning and that doing so can increase hallucination; fine-tuning shapes behaviour more than knowledge
        correct: true
      - text: Yes, perfectly
      - text: Yes, but only with temperature 0
      - text: Fine-tuning deletes all knowledge
    explanation: Gekhman et al. (2024) found that fine-tuning on new knowledge correlated with more hallucination.
  - id: prompt-first
    prompt: Why start with prompting even if you expect to fine-tune?
    options:
      - text: Fine-tuning requires a prompt licence
      - text: Prompting is always sufficient
      - id: baseline
        text: It establishes a baseline and an evaluation set cheaply; you need both to show fine-tuning helped
        correct: true
      - text: Providers require it
    explanation: Without a baseline, you cannot tell whether an expensive fine-tune was worth it.
  - id: data-needs
    prompt: What does fine-tuning need that prompting does not?
    type: multiple
    options:
      - id: labelled
        text: Hundreds to thousands of high-quality labelled examples
        correct: true
      - id: compute
        text: Training compute and pipelines
        correct: true
      - id: lifecycle
        text: A model lifecycle — versioning, re-training when the base model changes, and hosting the custom model
        correct: true
      - text: Nothing extra
    explanation: The ongoing maintenance cost is often underestimated; when the provider retires the base model, you re-fine-tune.
  - id: continued-pretraining
    prompt: What is continued (domain-adaptive) pretraining?
    options:
      - text: Restarting pretraining from zero
      - id: cpt
        text: Further self-supervised training of an existing model on large amounts of unlabelled domain text (e.g. legal, biomedical) before task tuning
        correct: true
      - text: Pretraining on a weekly schedule
      - text: Prompting with long documents
    explanation: Expensive and rarely necessary for enterprises; justified mainly for specialised domains with lots of text.
  - id: catastrophic-forgetting
    prompt: What is catastrophic forgetting?
    options:
      - text: A user forgetting their password
      - text: A model deleting its logs
      - text: Context truncation
      - id: forget
        text: Loss of previously learned general abilities when a model is trained intensively on a narrow new task
        correct: true
    explanation: Parameter-efficient methods and mixing in general data reduce it.
  - id: safety-ft
    prompt: What did Qi et al. (2023) find about fine-tuning aligned models?
    options:
      - id: safety-degrades
        text: Fine-tuning — even on a small number of examples, sometimes benign ones — can degrade a model's safety alignment
        correct: true
      - text: Fine-tuning always improves safety
      - text: Fine-tuning has no effect on safety
      - text: Only image models are affected
    explanation: Re-run safety evaluations after any fine-tune.
---
## Learning objectives

- Choose the lowest rung of the ladder that meets your quality bar
- Know what fine-tuning is good and bad at
- Understand the lifecycle cost of custom models

## The ladder

| Rung | Changes | Cost | Good for |
|---|---|---|---|
| Prompting | Instructions, examples | Minutes | Most tasks |
| RAG | What the model sees | Days | Private, changing knowledge |
| Fine-tuning | Behaviour, style, format | Weeks + data | Consistency, cost reduction |
| Continued pretraining | Domain language | Large | Highly specialised domains |
| Pretraining | Everything | Very large | Model companies |

## Sources

- Gekhman et al., "Does Fine-Tuning LLMs on New Knowledge Encourage Hallucinations?", 2024 — <https://arxiv.org/abs/2405.05904>
- Gururangan et al., "Don't Stop Pretraining", 2020 — <https://arxiv.org/abs/2004.10964>
- Qi et al., "Fine-tuning Aligned Language Models Compromises Safety", 2023 — <https://arxiv.org/abs/2310.03693>
