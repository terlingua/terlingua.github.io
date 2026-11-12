---
title: Tokens and tokenization
questions:
  - id: token
    prompt: What is a token?
    options:
      - text: A password for the API
      - text: Always exactly one word
      - text: Always exactly one character
      - id: subword
        text: A unit of text — often a word piece — from the model's fixed vocabulary; models read and write sequences of token IDs
        correct: true
    explanation: Most LLMs use subword tokenisers such as byte-pair encoding (BPE), so common words are one token and rare words are split.
  - id: rule-of-thumb
    prompt: For English text, roughly how many characters is one token on average with common tokenisers?
    options:
      - text: "1"
      - id: four
        text: About 4 (roughly ¾ of a word)
        correct: true
      - text: About 20
      - text: About 100
    explanation: OpenAI's rule of thumb. Other languages, code and numbers often use more tokens per character.
  - id: why-matters
    prompt: Why do tokens matter in practice?
    type: multiple
    options:
      - id: billing
        text: APIs bill per input and output token
        correct: true
      - id: context
        text: Context windows are measured in tokens
        correct: true
      - id: latency
        text: Output generation time scales with the number of output tokens
        correct: true
      - text: Tokens determine the model's licence
    explanation: Counting tokens is the basis of GenAI cost estimation.
  - id: language-tax
    prompt: Why can the same message cost more in Hindi or Burmese than in English?
    options:
      - text: Providers charge more for non-English languages by policy
      - text: Non-English text is compressed
      - id: tokenizer-bias
        text: Tokenisers trained mostly on English split many other scripts into more tokens per word, so the same content uses more tokens — a documented fairness issue
        correct: true
      - text: It does not; tokens are language-neutral
    explanation: Studies (e.g. Petrov et al., 2023) measured up to an order of magnitude more tokens for some languages.
  - id: strawberry
    prompt: Why did LLMs famously miscount the letter "r" in "strawberry"?
    options:
      - text: They cannot read English
      - id: no-letters
        text: The model sees tokens, not letters; character-level tasks require knowledge the token representation hides
        correct: true
      - text: The word was not in the training data
      - text: A bug that was fixed in all models
    explanation: The same blind spot affects spelling, character counting and some arithmetic on long numbers. Tools (code execution) fix it reliably.
  - id: numbers
    prompt: How does tokenisation affect arithmetic?
    options:
      - text: It has no effect
      - text: Numbers are always single tokens
      - text: LLMs compute with a built-in calculator
      - id: chunked
        text: Numbers may be split into irregular chunks, making digit-level arithmetic harder; models are unreliable calculators unless given a tool
        correct: true
    explanation: For exact computation, have the model call code or a calculator.
  - id: special-tokens
    prompt: What are special tokens used for?
    options:
      - id: structure
        text: Marking structure such as the start/end of a message, roles (system, user, assistant) and tool calls in chat templates
        correct: true
      - text: Encrypting prompts
      - text: Counting money
      - text: Formatting Markdown
    explanation: Chat APIs hide templates from you, but they are why you send structured messages rather than raw text.
  - id: vocab
    prompt: Typical modern LLM vocabularies contain how many tokens?
    options:
      - text: About 100
      - text: About 1,000
      - id: tens-of-thousands
        text: Tens of thousands to a few hundred thousand
        correct: true
      - text: Billions
    explanation: Larger vocabularies encode text more efficiently but enlarge the embedding and output layers.
  - id: output-pricing
    prompt: Most APIs price output tokens higher than input tokens. Why?
    options:
      - text: Output tokens are longer
      - id: sequential
        text: Input tokens are processed in parallel; output tokens are generated one at a time, each needing a forward pass, which costs more compute per token
        correct: true
      - text: To discourage questions
      - text: Output tokens are stored forever
    explanation: Asking for concise outputs is one of the simplest cost controls.
---
## Learning objectives

- Explain tokens and subword tokenisation
- Estimate token counts and costs
- Recognise tasks tokenisation makes hard

## See tokens

```python
import tiktoken
enc = tiktoken.get_encoding("o200k_base")
ids = enc.encode("Unbelievably, Databricks tokenises strawberries.")
print(len(ids), [enc.decode([i]) for i in ids])
```

Different model families use different tokenisers — always count with the target model's tokeniser.

## Cost arithmetic

```text
cost = input_tokens × input_price + output_tokens × output_price
1,000 requests × (1,500 in + 300 out) at $1 / $4 per million tokens
= 1.5M × $1 + 0.3M × $4 = $1.50 + $1.20 = $2.70
```

## Sources

- Sennrich et al., "Neural Machine Translation of Rare Words with Subword Units" (BPE), 2016 — <https://arxiv.org/abs/1508.07909>
- OpenAI, "What are tokens and how to count them?" — <https://help.openai.com/en/articles/4936856-what-are-tokens-and-how-to-count-them>
- Petrov et al., "Language Model Tokenizers Introduce Unfairness Between Languages", 2023 — <https://arxiv.org/abs/2305.15425>
