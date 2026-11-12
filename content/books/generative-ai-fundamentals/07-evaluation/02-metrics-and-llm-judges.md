---
title: Metrics and LLM judges
questions:
  - id: bleu-rouge
    prompt: Why are BLEU and ROUGE weak for evaluating chat answers?
    options:
      - id: overlap
        text: They measure n-gram overlap with a reference; correct answers phrased differently score low and wrong answers with similar wording score high
        correct: true
      - text: They are too slow
      - text: They only work in French
      - text: They require GPUs
    explanation: They remain useful for translation-like tasks with close references.
  - id: llm-judge
    prompt: What is LLM-as-a-judge?
    options:
      - text: A model presiding over court cases
      - text: Users voting on outputs
      - id: judge
        text: Using an LLM with a rubric to grade outputs (correctness, groundedness, relevance, safety), enabling scalable evaluation
        correct: true
      - text: A legal requirement
    explanation: Zheng et al. (2023) found strong judges agree with humans about as often as humans agree with each other on some tasks — with caveats.
  - id: judge-biases
    prompt: Which biases do LLM judges exhibit?
    type: multiple
    options:
      - id: position
        text: Position bias — favouring the first (or second) answer in pairwise comparisons
        correct: true
      - id: verbosity
        text: Verbosity bias — favouring longer answers
        correct: true
      - id: self
        text: Self-preference — favouring outputs from the same model family
        correct: true
      - text: Judges have no biases
    explanation: Mitigate with swapped orderings, length controls, different judge models, and calibration against human labels.
  - id: calibrate
    prompt: How do you know whether to trust an LLM judge?
    options:
      - text: Trust any judge from a famous provider
      - id: align
        text: Measure its agreement with human expert labels on a sample, refine the rubric, and re-check periodically
        correct: true
      - text: Use a higher temperature
      - text: Ask the judge if it is reliable
    explanation: A judge is a model; it needs its own evaluation.
  - id: groundedness
    prompt: What does a groundedness (faithfulness) metric check?
    options:
      - text: Whether the answer is polite
      - text: Whether the answer is short
      - text: Whether retrieval was fast
      - id: supported
        text: Whether each claim in the answer is supported by the provided context
        correct: true
    explanation: An answer can be grounded yet wrong if the context was wrong — check retrieval and correctness too.
  - id: binary
    prompt: Which judge rubric is usually more reliable?
    options:
      - id: binary
        text: Specific pass/fail criteria ("Does the answer state the refund window from the policy?")
        correct: true
      - text: "\"Rate quality from 1 to 10\""
      - text: "\"Is this good?\""
      - text: No rubric at all
    explanation: Fine-grained numeric scales drift and are hard to interpret; binary or few-level criteria with explanations are more stable.
  - id: code-checks
    prompt: When should you prefer deterministic checks over LLM judges?
    options:
      - text: Never
      - text: Only for long answers
      - id: when-possible
        text: Whenever the property is checkable in code — JSON validity, required fields, exact matches, citations pointing to retrieved IDs, banned words
        correct: true
      - text: Only on weekends
    explanation: Code checks are free, fast and deterministic; reserve judges for semantic qualities.
  - id: pairwise
    prompt: Why use pairwise comparison (A vs B) rather than absolute scores?
    options:
      - text: It is required by MLflow
      - id: easier
        text: Choosing the better of two outputs is easier and more consistent for humans and judges than assigning absolute scores
        correct: true
      - text: It needs fewer examples than any other method
      - text: It removes position bias automatically
    explanation: Chatbot Arena popularised pairwise human preference with Elo-style ratings.
  - id: goodhart
    prompt: Developers tune prompts until the judge score is 98%, but users complain more. What happened?
    options:
      - text: Users are wrong
      - text: The judge is perfect
      - text: Temperature was too low
      - id: goodhart
        text: Goodhart's law — the system was optimised for the judge's blind spots rather than real quality; re-validate the judge against humans
        correct: true
    explanation: "\"When a measure becomes a target, it ceases to be a good measure.\""
---
## Learning objectives

- Choose between lexical metrics, deterministic checks and LLM judges
- Recognise and mitigate judge biases
- Write reliable rubrics

## A judge in MLflow

```python
import mlflow
from mlflow.genai.scorers import Correctness, RelevanceToQuery, Guidelines

results = mlflow.genai.evaluate(
    data=eval_rows,
    predict_fn=answer_question,
    scorers=[
        Correctness(),
        RelevanceToQuery(),
        Guidelines(name="cites_policy", guidelines="The answer must cite a policy section."),
    ],
)
```

## Sources

- Zheng et al., "Judging LLM-as-a-Judge with MT-Bench and Chatbot Arena", 2023 — <https://arxiv.org/abs/2306.05685>
- Papineni et al., "BLEU", 2002 — <https://aclanthology.org/P02-1040/>
- MLflow GenAI evaluation — <https://mlflow.org/docs/latest/genai/eval-monitor/>
- Panickssery et al., "LLM Evaluators Recognize and Favor Their Own Generations", 2024 — <https://arxiv.org/abs/2404.13076>
