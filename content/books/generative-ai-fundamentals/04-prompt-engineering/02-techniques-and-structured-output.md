---
title: Techniques and structured output
questions:
  - id: zero-few
    prompt: What is few-shot prompting?
    options:
      - text: Sending only a few tokens
      - id: examples
        text: Including a few input–output examples in the prompt to demonstrate the task
        correct: true
      - text: Asking the model a few times
      - text: Using a small model
    explanation: GPT-3's paper showed in-context learning from examples. Choose diverse examples; models copy surface patterns of examples too.
  - id: example-bias
    prompt: All five few-shot examples you provide are labelled "positive". What risk does this create?
    options:
      - text: None
      - text: The model will refuse
      - id: majority
        text: The model may be biased toward the majority label and copy example format and length too closely
        correct: true
      - text: The prompt will be too short
    explanation: Zhao et al. (2021) documented majority-label and recency biases in few-shot prompting.
  - id: cot
    prompt: What is chain-of-thought prompting?
    options:
      - text: Linking several chatbots
      - text: A blockchain technique
      - id: steps
        text: Prompting the model to produce intermediate reasoning steps before the final answer, which improved multi-step reasoning in large models
        correct: true
      - text: Asking the same question repeatedly
    explanation: Wei et al., 2022. Reasoning models now do this internally; explicit CoT matters less with them and costs tokens.
  - id: self-consistency
    prompt: What is self-consistency?
    options:
      - id: vote
        text: Sampling several reasoning paths and taking the majority answer
        correct: true
      - text: Asking the model if it is consistent
      - text: Using the same prompt every time
      - text: A database constraint
    explanation: It trades cost for accuracy (Wang et al., 2022).
  - id: decompose
    prompt: When a single prompt does too much and quality suffers, what usually helps?
    options:
      - text: A longer single prompt
      - text: Higher temperature
      - text: Removing the system prompt
      - id: chain
        text: Decomposing into a chain of smaller prompts or steps, each with a clear job and checkable output
        correct: true
    explanation: Prompt chaining is the simplest "workflow" pattern; it also makes failures easier to locate.
  - id: structured
    prompt: Which approach most reliably produces JSON that matches a schema?
    options:
      - text: "Asking politely: \"please reply in JSON\""
      - id: constrained
        text: Using the API's structured-output / JSON-schema mode (constrained decoding) and validating the result in code
        correct: true
      - text: Setting temperature to 2
      - text: Adding "JSON" to the system prompt five times
    explanation: Constrained decoding guarantees syntax and schema shape; it does not guarantee correct values — still validate semantics.
  - id: schema-valid-wrong
    prompt: A structured-output response parses and validates against the schema. What can still be wrong?
    options:
      - text: Nothing
      - text: The JSON syntax
      - id: values
        text: The values — a field can be well-formed but hallucinated, mis-extracted or in the wrong unit
        correct: true
      - text: The field names
    explanation: Add business-rule validation (totals add up, dates in range) and confidence/abstention fields.
  - id: ask-clarify
    prompt: How should a prompt handle missing information?
    options:
      - text: Tell the model to guess
      - text: Ignore it
      - text: Set max_tokens to 1
      - id: explicit
        text: Specify the behaviour — return null, ask a clarifying question, or say what is missing
        correct: true
    explanation: Unspecified, models tend to fill gaps plausibly — i.e. hallucinate.
  - id: reasoning-models-prompt
    prompt: How does prompting reasoning models differ?
    options:
      - id: high-level
        text: Give clear goals, constraints and context; elaborate step-by-step instructions or "think step by step" are often unnecessary and can even hurt
        correct: true
      - text: They need longer few-shot examples
      - text: They ignore system prompts
      - text: They require Chinese
    explanation: Providers' guidance for reasoning models emphasises outcomes and constraints over prescribed reasoning steps.
  - id: prompt-caching
    prompt: How can prompt structure reduce cost with prompt caching?
    options:
      - text: Put variable content first and static instructions last
      - text: Caching is unrelated to structure
      - id: static-first
        text: Put static content (instructions, examples, documents) first and variable content last so the shared prefix can be cached
        correct: true
      - text: Use random whitespace
    explanation: Caches match on identical prefixes.
---
## Learning objectives

- Apply zero-shot, few-shot, chain-of-thought and decomposition
- Produce validated structured output
- Adapt prompting for reasoning models and caching

## Structured output

```python
from pydantic import BaseModel

class Invoice(BaseModel):
    invoice_number: str | None
    total: float | None
    currency: str | None

completion = client.beta.chat.completions.parse(
    model=model,
    messages=[{"role": "system", "content": "Extract fields. Use null if absent."},
              {"role": "user", "content": text}],
    response_format=Invoice,
)
invoice = completion.choices[0].message.parsed
assert invoice.total is None or invoice.total >= 0
```

## Sources

- Wei et al., "Chain-of-Thought Prompting Elicits Reasoning in Large Language Models", 2022 — <https://arxiv.org/abs/2201.11903>
- Wang et al., "Self-Consistency Improves Chain of Thought Reasoning", 2022 — <https://arxiv.org/abs/2203.11171>
- Zhao et al., "Calibrate Before Use", 2021 — <https://arxiv.org/abs/2102.09690>
- OpenAI, "Structured Outputs" — <https://platform.openai.com/docs/guides/structured-outputs>
- OpenAI, "Reasoning best practices" — <https://platform.openai.com/docs/guides/reasoning-best-practices>
