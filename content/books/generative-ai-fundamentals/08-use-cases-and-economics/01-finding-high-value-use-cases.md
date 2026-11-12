---
title: Finding high-value use cases
questions:
  - id: good-fit
    prompt: Which characteristics make a task a good fit for GenAI?
    type: multiple
    options:
      - id: language
        text: It is language- or content-heavy (reading, writing, summarising, extracting)
        correct: true
      - id: tolerant
        text: Errors are tolerable or can be caught by review before harm occurs
        correct: true
      - id: volume
        text: It is frequent enough that time savings add up
        correct: true
      - text: It requires perfect, auditable numerical precision with no review
    explanation: High-stakes, zero-error tasks need strong verification or a different approach.
  - id: matrix
    prompt: A common prioritisation approach plots use cases on which two axes?
    options:
      - text: Model size and context length
      - id: value-feasibility
        text: Business value (impact) against feasibility (data readiness, risk, complexity)
        correct: true
      - text: Cost and colour
      - text: Number of users and number of GPUs
    explanation: Start with high-value, high-feasibility cases; avoid flagship projects that are high-value but infeasible.
  - id: categories
    prompt: Which are common enterprise GenAI use-case categories?
    type: multiple
    options:
      - id: support
        text: Customer support assistance and ticket triage
        correct: true
      - id: knowledge
        text: Internal knowledge search over documents
        correct: true
      - id: docs
        text: Document processing — extraction from contracts, invoices, forms
        correct: true
      - id: code
        text: Software development assistance
        correct: true
    explanation: Coding assistance and support have the most measured evidence of productivity effects.
  - id: augment
    prompt: Brynjolfsson et al. studied a GenAI assistant for customer support agents. What did they find?
    options:
      - text: It replaced all agents
      - text: It had no effect
      - id: novices
        text: About a 14% average productivity increase, with the largest gains for less experienced workers and little effect for top performers
        correct: true
      - text: It reduced customer satisfaction
    explanation: Published in the Quarterly Journal of Economics (2025). Augmenting people was the measurable win.
  - id: jagged
    prompt: The Harvard/BCG study with consultants described a "jagged technological frontier". What does that mean?
    options:
      - id: jagged
        text: AI improved performance on tasks inside its capability frontier but made consultants worse on a task just outside it, where they trusted wrong outputs
        correct: true
      - text: AI capability grows smoothly
      - text: AI only helps senior consultants
      - text: AI should be banned in consulting
    explanation: Dell'Acqua et al., 2023. Knowing where the frontier lies for your task is essential — and it moves.
  - id: pilots-fail
    prompt: MIT NANDA's 2025 report claimed that about 95% of enterprise GenAI pilots delivered no measurable P&L impact. How should you treat this?
    options:
      - text: As proof GenAI is useless
      - text: As irrelevant
      - id: careful
        text: As a widely cited but methodologically debated signal — its main lessons (integration into workflows, learning systems, and buying over building often succeeding more) are worth considering alongside other evidence
        correct: true
      - text: As an audited financial statement
    explanation: Critics noted its small sample and definitions. Most surveys agree that value comes from workflow change, not from the model alone.
  - id: success-metric
    prompt: Before building, what should every use case define?
    options:
      - text: Which model to use
      - text: The UI colours
      - text: The GPU type
      - id: metric
        text: A measurable success metric and baseline (time per task, error rate, cost), plus who reviews outputs
        correct: true
    explanation: Without a baseline, you cannot show value or decide to stop.
  - id: data-readiness
    prompt: A use case depends on documents scattered across email attachments and personal drives. What is the realistic first step?
    options:
      - id: data-work
        text: Data work — consolidating, cleaning and governing the documents — before or alongside the AI build
        correct: true
      - text: Fine-tune a model on the emails
      - text: Choose the largest model
      - text: Skip it; GenAI handles messy data
    explanation: This is the "data intelligence" argument from the course, and it holds regardless of vendor.
---
## Learning objectives

- Identify good-fit tasks and prioritise use cases
- Know the strongest evidence about productivity effects
- Define success before building

## A use-case canvas

| Question | Example answer |
|---|---|
| Who is the user? | Tier-1 support agents |
| What task? | Draft replies to billing tickets |
| Data needed? | Billing policies, customer account data |
| Success metric? | Handle time −20%, CSAT unchanged |
| Who checks outputs? | Agent edits before sending |
| Failure cost? | Wrong refund amount → policy check in code |
| Build or buy? | Buy assistant, build retrieval over policies |

## Sources

- Brynjolfsson, Li, Raymond, "Generative AI at Work", QJE 2025 — <https://academic.oup.com/qje/article/140/2/889/7990658>
- Dell'Acqua et al., "Navigating the Jagged Technological Frontier", 2023 — <https://www.hbs.edu/faculty/Pages/item.aspx?num=64700>
- MIT NANDA, "The GenAI Divide: State of AI in Business 2025" — <https://mlq.ai/media/quarterly_decks/v0.1_State_of_AI_in_Business_2025_Report.pdf>
- Databricks, Generative AI Fundamentals — <https://www.databricks.com/learn/training/generative-ai-fundamentals-accreditation>
