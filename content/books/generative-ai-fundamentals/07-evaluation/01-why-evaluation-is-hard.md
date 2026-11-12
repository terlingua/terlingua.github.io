---
title: Why evaluation is hard
questions:
  - id: hard
    prompt: Why is evaluating GenAI harder than evaluating a classifier?
    type: multiple
    options:
      - id: open-ended
        text: Outputs are open-ended text with many acceptable answers
        correct: true
      - id: multi-dim
        text: Quality has several dimensions — correctness, groundedness, completeness, tone, safety
        correct: true
      - id: nondeterministic
        text: Outputs vary between runs
        correct: true
      - text: Accuracy cannot be computed for any GenAI task
    explanation: Some GenAI tasks (classification, extraction) can be scored exactly; open-ended ones need judges or humans.
  - id: vibes
    prompt: What is "vibe-checking"?
    options:
      - id: vibe
        text: Judging quality by trying a few prompts and seeing if outputs feel right — useful early, insufficient for production decisions
        correct: true
      - text: A formal statistical test
      - text: A music recommendation feature
      - text: A Databricks product
    explanation: Replace it with a fixed evaluation set as soon as possible.
  - id: eval-set
    prompt: What makes a good evaluation set?
    type: multiple
    options:
      - id: representative
        text: Representative of real user requests, including messy ones
        correct: true
      - id: edge
        text: Edge cases and known failure modes
        correct: true
      - id: expected
        text: Expected answers or grading criteria defined by domain experts
        correct: true
      - id: adversarial
        text: Adversarial and out-of-scope inputs the system should refuse
        correct: true
    explanation: Start with 50–100 examples; grow it from production failures.
  - id: offline-online
    prompt: What is the difference between offline and online evaluation?
    options:
      - text: Offline means without internet
      - id: phases
        text: Offline runs a fixed dataset before release; online monitors live traffic (user feedback, judges on samples, business metrics) after release
        correct: true
      - text: There is none
      - text: Online is always cheaper
    explanation: You need both — offline to gate releases, online to catch drift and unforeseen inputs.
  - id: regression
    prompt: Why run the evaluation suite on every prompt, model or retrieval change?
    options:
      - text: Providers require it
      - text: To increase costs
      - id: regress
        text: A change that fixes one case often breaks others; regression evaluation catches it before users do
        correct: true
      - text: It is only needed once
    explanation: Treat evaluations like unit tests in CI.
  - id: business-metric
    prompt: Which measure ultimately matters most for a customer-support assistant?
    options:
      - text: BLEU score
      - text: Number of tokens generated
      - text: Model parameter count
      - id: outcome
        text: Business outcomes — resolution rate, escalation rate, customer satisfaction, cost per resolved ticket — alongside quality and safety
        correct: true
    explanation: Model metrics are proxies; connect them to outcomes.
  - id: human-eval
    prompt: Which statement about human evaluation is accurate?
    options:
      - id: gold-costly
        text: It is the reference standard for nuanced quality but is slow, costly and inconsistent between raters; use clear rubrics and measure agreement
        correct: true
      - text: It is always perfectly consistent
      - text: It is never needed
      - text: It is cheaper than automated evaluation
    explanation: Inter-rater agreement (e.g. Cohen's kappa) tells you whether the rubric is clear.
  - id: sample-size
    prompt: Version B scores 82% and version A 80% on a 50-question eval. What should you conclude?
    options:
      - text: B is definitely better
      - text: A is definitely better
      - id: noise
        text: The difference (one question) is within noise; you need more examples or repeated runs to claim improvement
        correct: true
      - text: Both are perfect
    explanation: Small eval sets have wide confidence intervals. Report uncertainty, not just point scores.
---
## Learning objectives

- Explain why GenAI evaluation is hard
- Build an evaluation set and use it for regression testing
- Interpret differences with statistical caution

## An evaluation loop

```text
collect real questions ─► label expected answers/criteria ─► run system ─► score
        ▲                                                                   │
        └───────── add production failures ◄── monitor live traffic ◄───────┘
```

## Sources

- Hamel Husain, "Your AI Product Needs Evals", 2024 — <https://hamel.dev/blog/posts/evals/>
- Shankar et al., "Who Validates the Validators?", 2024 — <https://arxiv.org/abs/2404.12272>
- Miller, "Adding Error Bars to Evals", Anthropic 2024 — <https://arxiv.org/abs/2411.00640>
