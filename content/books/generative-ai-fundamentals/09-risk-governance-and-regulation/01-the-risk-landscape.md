---
title: The risk landscape
questions:
  - id: categories
    prompt: Which are major risk categories for enterprise GenAI?
    type: multiple
    options:
      - id: accuracy
        text: Accuracy — hallucination and over-reliance
        correct: true
      - id: privacy
        text: Privacy and data leakage
        correct: true
      - id: security
        text: Security — prompt injection, insecure output handling, supply chain
        correct: true
      - id: ip
        text: Intellectual property and copyright
        correct: true
    explanation: Add bias/fairness, safety, reputational and regulatory risk for a full picture.
  - id: samsung
    prompt: In 2023 Samsung engineers pasted confidential source code into a public chatbot. What is the lesson?
    options:
      - text: Chatbots steal code on purpose
      - id: policy
        text: Without approved tools and clear policies, employees use consumer AI services with sensitive data ("shadow AI"); provide sanctioned alternatives with suitable data terms
        correct: true
      - text: Code should never be written
      - text: Only open models are safe
    explanation: Bans without alternatives push usage underground.
  - id: data-terms
    prompt: What should you verify about an AI provider's data handling?
    type: multiple
    options:
      - id: training
        text: Whether your inputs and outputs are used to train their models
        correct: true
      - id: retention
        text: How long prompts and outputs are retained, and where
        correct: true
      - id: residency
        text: Data residency and sub-processors
        correct: true
      - text: The provider's logo licence
    explanation: Enterprise and API terms typically differ from consumer terms. Read the current ones.
  - id: copyright-output
    prompt: In the US, can purely AI-generated content be registered for copyright?
    options:
      - text: Yes, the AI owns the copyright
      - text: Yes, the provider owns it
      - id: human
        text: Generally no — the Copyright Office and courts (Thaler v. Perlmutter, D.C. Circuit 2025) require human authorship; human selection, arrangement or modification may be protectable
        correct: true
      - text: Copyright does not apply to text
    explanation: Rules differ by country. For commercial content, document human contributions.
  - id: copyright-training
    prompt: What is the legal status of training on copyrighted works, as of 2026?
    options:
      - text: Settled worldwide as legal
      - text: Settled worldwide as illegal
      - id: unsettled
        text: Unsettled and jurisdiction-dependent — with major lawsuits (e.g. NYT v. OpenAI), a reported $1.5 billion settlement in Bartz v. Anthropic over pirated books, and rulings going different ways on fair use
        correct: true
      - text: Irrelevant to enterprises
    explanation: Some providers offer IP indemnities for outputs; read their conditions.
  - id: indemnity
    prompt: What do provider IP indemnities usually require?
    options:
      - id: conditions
        text: Compliance with conditions such as using enterprise products, not disabling filters, and not knowingly prompting for infringing content
        correct: true
      - text: Nothing
      - text: Paying per lawsuit
      - text: Using only open models
    explanation: Indemnities shift some risk; they do not remove it.
  - id: overreliance
    prompt: Which design choices reduce over-reliance on AI outputs?
    type: multiple
    options:
      - id: show-sources
        text: Showing sources and confidence signals
        correct: true
      - id: friction
        text: Requiring review for high-impact decisions
        correct: true
      - id: training
        text: Training users on failure modes
        correct: true
      - text: Presenting answers as always authoritative
    explanation: Automation bias — trusting automated output over contrary evidence — is well documented in aviation and medicine.
  - id: supply-chain
    prompt: What is an AI supply-chain risk?
    options:
      - text: GPU shortages only
      - text: Late hardware deliveries
      - id: artefacts
        text: Compromised or malicious models, datasets, libraries or plugins — e.g. model files that execute code when loaded (pickle) or poisoned public datasets
        correct: true
      - text: Cloud invoices
    explanation: Prefer safe formats (safetensors), pin and scan dependencies, and use trusted registries.
---
## Learning objectives

- Map the main GenAI risk categories
- Understand data-handling and IP questions
- Recognise over-reliance and supply-chain risks

## A risk register sketch

| Risk | Example | Control |
|---|---|---|
| Hallucination | Wrong policy quoted | Grounding, citations, evaluation |
| Data leakage | Secrets in prompts | Approved tools, DLP, data terms |
| Injection | Email hijacks agent | Least privilege, confirmation |
| IP | Output mirrors licensed text | Filters, indemnity, review |
| Bias | Unequal treatment | Testing across groups |
| Over-reliance | Unchecked decisions | Review steps, training |

## Sources

- NIST AI 600-1, "Generative AI Profile", 2024 — <https://nvlpubs.nist.gov/nistpubs/ai/NIST.AI.600-1.pdf>
- Thaler v. Perlmutter, D.C. Cir. 2025 — <https://media.cadc.uscourts.gov/opinions/docs/2025/03/23-5233.pdf>
- U.S. Copyright Office, "Copyright and Artificial Intelligence" reports — <https://www.copyright.gov/ai/>
- Hugging Face, "Safetensors" — <https://huggingface.co/docs/safetensors/>
