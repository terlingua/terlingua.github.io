---
title: Generation and decoding
questions:
  - id: autoregressive
    prompt: How does an LLM produce a response?
    options:
      - text: It retrieves a stored answer from a database
      - text: It writes the whole response at once
      - id: one-token
        text: It repeatedly computes a probability distribution over the vocabulary for the next token, picks one, appends it, and repeats until a stop condition
        correct: true
      - text: It searches the internet
    explanation: This autoregressive loop is why responses stream token by token.
  - id: temperature
    prompt: What does the temperature parameter control?
    options:
      - text: GPU temperature
      - id: randomness
        text: How peaked or flat the next-token distribution is before sampling — low temperature favours the most likely tokens, high temperature increases variety
        correct: true
      - text: The length of output
      - text: The model's knowledge
    explanation: Low temperature for extraction and classification; higher for brainstorming. Temperature 0 reduces but does not always eliminate variation.
  - id: top-p
    prompt: What is top-p (nucleus) sampling?
    options:
      - text: Choosing the top p percent of users
      - text: Picking the first p tokens of the prompt
      - id: nucleus
        text: Sampling only from the smallest set of tokens whose cumulative probability reaches p (e.g. 0.9)
        correct: true
      - text: A pricing tier
    explanation: Top-k limits to the k most likely tokens. Providers generally advise tuning temperature or top-p, not both.
  - id: greedy
    prompt: What is greedy decoding?
    options:
      - id: argmax
        text: Always choosing the single most probable next token
        correct: true
      - text: Generating as many tokens as possible
      - text: Charging more per token
      - text: Sampling at random
    explanation: Greedy decoding can produce repetitive text and is not guaranteed to find the most probable overall sequence.
  - id: nondeterminism
    prompt: Why can the same prompt at temperature 0 sometimes produce different outputs via an API?
    options:
      - text: Temperature 0 means random
      - id: infra
        text: Floating-point non-associativity, batching with other requests, hardware differences and silent model updates can change results
        correct: true
      - text: The model learns from each request
      - text: It cannot; outputs are always identical
    explanation: Design systems to tolerate variation, and pin model versions where providers allow.
  - id: context-window
    prompt: What is the context window?
    options:
      - text: The UI window where you type
      - text: The training data size
      - text: The number of users at once
      - id: max-tokens
        text: The maximum number of tokens (input plus output) the model can consider in one request
        correct: true
    explanation: Windows grew from about 2K tokens (GPT-3) to hundreds of thousands or millions. Bigger is not automatically better — see "lost in the middle".
  - id: stateless
    prompt: Does a chat model remember previous conversations by itself?
    options:
      - text: Yes, it learns from every chat
      - id: stateless
        text: No — each API call is stateless; the application resends conversation history (or uses a memory feature) each turn
        correct: true
      - text: Only on weekends
      - text: Only if temperature is 0
    explanation: Long chats cost more because the history is re-sent, and old turns get truncated or summarised.
  - id: kv-cache
    prompt: What is the KV cache?
    options:
      - text: A key-value database for prompts
      - text: A cache of user passwords
      - id: kv
        text: Stored attention keys and values for already-processed tokens, so each new token does not recompute the whole prefix
        correct: true
      - text: A CDN for model weights
    explanation: Prompt caching by providers reuses prefixes across requests, cutting cost and latency for repeated system prompts.
  - id: latency-metrics
    prompt: Which latency metrics matter for LLM applications?
    type: multiple
    options:
      - id: ttft
        text: Time to first token
        correct: true
      - id: tps
        text: Output tokens per second
        correct: true
      - id: e2e
        text: End-to-end latency including retrieval and tool calls
        correct: true
      - text: Number of parameters
    explanation: Streaming improves perceived latency by showing tokens as they arrive.
  - id: max-tokens
    prompt: A response is cut off mid-sentence and the finish reason is "length". What happened?
    options:
      - text: The model crashed
      - text: The user's network failed
      - text: The model chose to stop
      - id: limit
        text: The output hit the max-tokens limit set in the request (or the context limit)
        correct: true
    explanation: Check finish reasons in code; a truncated JSON response will not parse.
  - id: quantization
    prompt: What does quantisation do to a model?
    options:
      - id: lower-precision
        text: Stores weights at lower precision (e.g. 8- or 4-bit) to reduce memory and speed inference, usually with some quality loss
        correct: true
      - text: Increases the number of parameters
      - text: Adds quantum computing
      - text: Encrypts the weights
    explanation: Quantisation is how large open models run on smaller GPUs or laptops. Measure quality on your task.
---
## Learning objectives

- Explain autoregressive generation
- Use temperature, top-p and max tokens deliberately
- Understand context windows, statelessness and latency

## Decoding parameters

| Parameter | Low value | High value | Typical use |
|---|---|---|---|
| temperature | Focused, repeatable | Varied, creative | 0–0.3 extraction; 0.7–1 ideation |
| top_p | Few candidate tokens | Many | Leave at default unless needed |
| max_tokens | Short, truncation risk | Long, costly | Set per task |

```python
response = client.chat.completions.create(
    model=model,
    temperature=0.1,
    max_tokens=300,
    messages=[
        {"role": "system", "content": "Extract the invoice number and total as JSON."},
        {"role": "user", "content": invoice_text},
    ],
)
```

## Sources

- Holtzman et al., "The Curious Case of Neural Text Degeneration" (nucleus sampling), 2019 — <https://arxiv.org/abs/1904.09751>
- Dao et al., "FlashAttention", 2022 — <https://arxiv.org/abs/2205.14135>
- Horace He (Thinking Machines Lab), "Defeating Nondeterminism in LLM Inference", 2025 — <https://thinkingmachines.ai/blog/defeating-nondeterminism-in-llm-inference/>
