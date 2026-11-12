---
title: Benchmarks and contamination
questions:
  - id: mmlu
    prompt: What does MMLU measure?
    options:
      - text: Inference speed
      - id: knowledge
        text: Multiple-choice knowledge and reasoning across 57 academic subjects
        correct: true
      - text: Image quality
      - text: Cost per token
    explanation: Frontier models saturated it; harder successors (MMLU-Pro, GPQA, Humanity's Last Exam) followed.
  - id: saturation
    prompt: What does benchmark saturation mean?
    options:
      - text: The benchmark has too many questions
      - text: Too many people use it
      - id: saturated
        text: Top models score near the ceiling, so the benchmark no longer distinguishes them
        correct: true
      - text: The benchmark is wet
    explanation: Benchmarks have short useful lives; new ones appear constantly.
  - id: contamination
    prompt: What is benchmark contamination?
    options:
      - id: leak
        text: Test questions (or close variants) appearing in training data, inflating scores without real capability
        correct: true
      - text: Malware in benchmark files
      - text: Benchmarks run on dirty GPUs
      - text: Questions in the wrong language
    explanation: Web-scale training makes contamination hard to rule out. Held-out, private or time-separated benchmarks help.
  - id: vendor-bench
    prompt: How should you read a vendor's benchmark table?
    type: multiple
    options:
      - id: selection
        text: Vendors choose benchmarks and settings where they look best
        correct: true
      - id: settings
        text: Prompting, number of attempts and tool use differ between reports, making comparisons unfair
        correct: true
      - id: transfer
        text: High benchmark scores may not transfer to your task
        correct: true
      - text: Benchmarks are independent audits with no conflicts of interest
    explanation: Use public leaderboards for shortlisting, your evals for choosing.
  - id: arena
    prompt: What does LMArena (formerly Chatbot Arena) measure, and what is a known critique?
    options:
      - text: Speed; it is too slow
      - id: preference
        text: Crowd-sourced pairwise human preferences; critics showed providers could test many private variants and publish the best, and preferences reward style as well as substance
        correct: true
      - text: Cost; it ignores latency
      - text: Safety; it is too strict
    explanation: "\"The Leaderboard Illusion\" (Singh et al., 2025) documented these issues."
  - id: swe-bench
    prompt: What does SWE-bench evaluate?
    options:
      - text: Spoken Swedish
      - text: Software licence compliance
      - id: issues
        text: Whether a model can resolve real GitHub issues in Python repositories so that the repository's tests pass
        correct: true
      - text: Network switches
    explanation: It is closer to real work than multiple-choice tests; a human-validated subset (SWE-bench Verified) fixed flawed tasks.
  - id: metr
    prompt: METR's 2025 research tracked the length of tasks AI agents can complete. What trend did it report?
    options:
      - text: No change since 2019
      - text: Task length halves each year
      - text: Agents can already complete any task
      - id: doubling
        text: The length of tasks (measured in human time) that frontier agents complete with 50% reliability roughly doubled about every seven months since 2019
        correct: true
    explanation: Note the 50% reliability threshold — success at higher reliability corresponds to much shorter tasks.
  - id: your-bench
    prompt: What is the most useful benchmark for your organisation?
    options:
      - id: own
        text: A private evaluation built from your own tasks, data and success criteria, never published or used for training
        correct: true
      - text: Whichever leaderboard a model tops this week
      - text: MMLU
      - text: The vendor's marketing page
    explanation: Keep it private to avoid contamination and maintain it as your use cases evolve.
---
## Learning objectives

- Read benchmark claims critically
- Understand saturation, contamination and leaderboard effects
- Build a private benchmark

## Common benchmarks

| Benchmark | Measures |
|---|---|
| MMLU / MMLU-Pro | Multi-subject knowledge |
| GPQA | Graduate-level, "Google-proof" science questions |
| HumanEval / SWE-bench | Code generation / real issue resolution |
| GSM8K / MATH / AIME | Mathematical reasoning |
| Humanity's Last Exam | Expert-level questions across fields |
| LMArena | Human pairwise preference |

## Sources

- Hendrycks et al., "Measuring Massive Multitask Language Understanding", 2020 — <https://arxiv.org/abs/2009.03300>
- Jimenez et al., "SWE-bench", 2023 — <https://arxiv.org/abs/2310.06770>
- Singh et al., "The Leaderboard Illusion", 2025 — <https://arxiv.org/abs/2504.20879>
- METR, "Measuring AI Ability to Complete Long Tasks", 2025 — <https://arxiv.org/abs/2503.14499>
- Sainz et al., "NLP Evaluation in trouble: On the Need to Measure LLM Data Contamination", 2023 — <https://arxiv.org/abs/2310.18018>
