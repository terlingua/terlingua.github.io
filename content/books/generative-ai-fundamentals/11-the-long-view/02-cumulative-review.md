---
title: Cumulative review
questions:
  - id: r-next-token
    prompt: At its core, what does an LLM compute?
    options:
      - text: The single correct answer
      - id: distribution
        text: A probability distribution over the next token given the preceding tokens
        correct: true
      - text: A database lookup
      - text: A web search
    explanation: Everything else is built on this loop.
  - id: r-tokens
    prompt: Which tasks are made harder by tokenisation?
    type: multiple
    options:
      - id: letters
        text: Counting letters in a word
        correct: true
      - id: arithmetic
        text: Exact arithmetic on long numbers
        correct: true
      - id: languages
        text: Cost parity across languages
        correct: true
      - text: Summarising paragraphs
    explanation: Tools and awareness fix the first two; the third is a pricing and fairness issue.
  - id: r-temp
    prompt: For invoice field extraction, which temperature is appropriate?
    options:
      - id: low
        text: Low (around 0–0.2), with structured output and validation
        correct: true
      - text: 1.5 for creativity
      - text: Temperature does not matter
      - text: As high as allowed
    explanation: Extraction wants consistency, not variety.
  - id: r-rlhf
    prompt: Which side effect of preference training should users guard against?
    options:
      - text: Slower inference
      - text: Larger vocabularies
      - id: sycophancy
        text: Sycophancy — agreeing with users' mistaken premises
        correct: true
      - text: Smaller context windows
    explanation: Ask neutral questions and request critiques.
  - id: r-rag-vs-ft
    prompt: For weekly-changing product documentation, which is appropriate?
    options:
      - text: Weekly full fine-tuning
      - text: Pretraining
      - text: Nothing; models know everything
      - id: rag
        text: RAG with an index refreshed as documents change
        correct: true
    explanation: Fine-tuning shapes behaviour; retrieval supplies changing facts.
  - id: r-rag-debug
    prompt: A RAG answer is wrong. What is the first diagnostic step?
    options:
      - id: check-retrieval
        text: Check whether the retrieved chunks contained the answer
        correct: true
      - text: Switch models
      - text: Add more GPUs
      - text: Increase chunk size to the maximum
    explanation: Separate retrieval failures from generation failures.
  - id: r-injection
    prompt: Which statement about prompt injection is correct?
    options:
      - text: Fixed by a good system prompt
      - id: unsolved
        text: Not reliably preventable at the model level; limit what a compromised model can do
        correct: true
      - text: Only affects open models
      - text: Only affects image models
    explanation: Least privilege, confirmations and output handling are the defences.
  - id: r-judge
    prompt: Before relying on an LLM judge, you should…
    options:
      - text: Choose the largest model
      - text: Use a 1–10 scale
      - id: calibrate
        text: Check its agreement with human expert labels and use specific rubrics
        correct: true
      - text: Run it once
    explanation: Judges have position, verbosity and self-preference biases.
  - id: r-bench
    prompt: Why might a model top a public benchmark but underperform on your task?
    type: multiple
    options:
      - id: contam
        text: Contamination of benchmark data in training
        correct: true
      - id: mismatch
        text: The benchmark does not reflect your domain or format
        correct: true
      - id: settings
        text: Different prompting and tool settings in the published result
        correct: true
      - text: Benchmarks are always identical to your task
    explanation: Your private evaluation set decides.
  - id: r-cost
    prompt: Which change usually cuts cost the most without hurting quality?
    options:
      - text: Increasing max_tokens
      - text: Always using the largest model
      - text: Sending the whole knowledge base each time
      - id: smallest
        text: Using the smallest model that passes evaluation, with caching and lean context
        correct: true
    explanation: Measure cost per successful task.
  - id: r-eu
    prompt: Under the EU AI Act, which obligations already applied by the end of 2025?
    type: multiple
    options:
      - id: prohib
        text: Prohibited practices
        correct: true
      - id: gpai
        text: General-purpose AI model obligations
        correct: true
      - id: literacy
        text: AI literacy obligations for providers and deployers
        correct: true
      - text: All high-risk system obligations
    explanation: High-risk obligations were later delayed by the 2026 Digital Omnibus agreement.
  - id: r-copyright
    prompt: Can purely AI-generated text be copyrighted in the US?
    options:
      - text: Yes, by the AI
      - id: no-human
        text: Generally not without sufficient human authorship
        correct: true
      - text: Yes, by the provider
      - text: Only if it is long
    explanation: Thaler v. Perlmutter (2025) affirmed the human authorship requirement.
  - id: r-databricks
    prompt: On Databricks, which component logs requests and responses of serving endpoints to Delta?
    options:
      - text: Genie
      - text: Delta Sharing
      - id: inference-tables
        text: Inference tables
        correct: true
      - text: Auto Loader
    explanation: Use them for monitoring, evaluation and audit.
  - id: r-durable
    prompt: Which investment is most durable as models change?
    options:
      - text: Prompt tricks for one model
      - text: Leaderboard rankings
      - id: evals
        text: Domain evaluation sets and clean, governed data
        correct: true
      - text: A specific model version
    explanation: They let you adopt each new model with confidence.
---
## Learning objectives

- Consolidate the book's core ideas
- Practise applying them to realistic decisions

## The book in one table

| Topic | Remember |
|---|---|
| Mechanics | Tokens in, probabilities out, one token at a time |
| Training | Pretrain → SFT → preference/RL; behaviour is trained |
| Limits | Hallucination, cut-off, context rot, brittleness, drift |
| Prompting | Specify task, context, format, missing-data behaviour; version and test |
| Security | Injection is unsolved; limit agency |
| RAG | Most failures are retrieval and parsing; enforce permissions |
| Customising | Climb the ladder only as far as evals require |
| Evaluation | Own your evals; calibrate judges; beware Goodhart |
| Economics | Cost per successful task; people cost dominates small apps |
| Governance | Risk-based; existing laws apply; disclose and log |

## Sources

- See the sources listed in each section of this book.
