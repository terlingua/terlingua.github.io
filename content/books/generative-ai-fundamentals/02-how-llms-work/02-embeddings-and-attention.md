---
title: Embeddings and attention
questions:
  - id: embedding
    prompt: What is an embedding?
    options:
      - text: An image embedded in a document
      - id: vector
        text: A dense vector of numbers representing meaning, where semantically similar items are close together in the vector space
        correct: true
      - text: A compressed zip file
      - text: A database index on text columns
    explanation: Embedding models power semantic search and RAG; LLMs also use embeddings internally for every token.
  - id: similarity
    prompt: Which measure is most commonly used to compare text embeddings?
    options:
      - text: Edit distance
      - text: File size
      - id: cosine
        text: Cosine similarity (or dot product on normalised vectors)
        correct: true
      - text: Alphabetical order
    explanation: Cosine similarity measures the angle between vectors, ignoring magnitude.
  - id: analogy
    prompt: The famous word2vec example "king − man + woman ≈ queen" illustrates what?
    options:
      - id: directions
        text: Directions in embedding space can encode semantic relationships
        correct: true
      - text: That embeddings are exact
      - text: That models understand monarchy
      - text: That arithmetic on words is always meaningful
    explanation: Such analogies are cherry-picked; they also showed that embeddings absorb societal biases (e.g. gender-occupation associations; Bolukbasi et al., 2016).
  - id: attention
    prompt: What does self-attention do in a transformer?
    options:
      - text: Monitors whether the user is paying attention
      - text: Stores the training data
      - id: weigh
        text: For each token, computes weighted combinations of other tokens' representations, so context (e.g. which noun "it" refers to) shapes each token's meaning
        correct: true
      - text: Translates text to images
    explanation: Queries, keys and values are learned projections; attention weights come from query–key similarity.
  - id: quadratic
    prompt: How does the cost of standard self-attention scale with sequence length n?
    options:
      - text: Constant
      - text: Linear, O(n)
      - id: quadratic
        text: Quadratic, O(n²)
        correct: true
      - text: Logarithmic
    explanation: Engineering (FlashAttention, sparse/linear attention, KV caching) mitigates this, but long context is still costlier.
  - id: decoder-only
    prompt: Most chat LLMs are decoder-only transformers. What does "causal" masking mean there?
    options:
      - text: The model explains causes
      - id: causal
        text: Each position can attend only to earlier positions, so the model learns to predict the next token from what came before
        correct: true
      - text: The model is trained on causal inference
      - text: Tokens are shuffled
    explanation: This is why generation is left-to-right, one token at a time.
  - id: parameters
    prompt: What are a model's parameters?
    options:
      - text: The options passed in an API call
      - text: The training documents
      - text: The number of users
      - id: weights
        text: The learned numeric weights (matrices) of the network, adjusted during training
        correct: true
    explanation: Don't confuse them with generation parameters like temperature, also casually called "parameters".
  - id: embedding-models
    prompt: Why use a dedicated embedding model rather than a chat LLM for semantic search?
    options:
      - id: purpose
        text: Embedding models are trained to produce vectors where similarity reflects relevance, and are far cheaper and faster per document
        correct: true
      - text: Chat LLMs cannot read text
      - text: Embedding models write better answers
      - text: There is no reason
    explanation: Choose embedding models by retrieval benchmarks (such as MTEB) and, more importantly, by tests on your documents.
  - id: model-mismatch
    prompt: You indexed documents with embedding model A, then switch queries to model B. What happens?
    options:
      - text: Search improves automatically
      - text: Nothing changes
      - id: incompatible
        text: Retrieval breaks — vectors from different models live in different spaces, so the whole corpus must be re-embedded with B
        correct: true
      - text: The index converts automatically
    explanation: Record the embedding model and version alongside the index. Plan re-embedding cost when upgrading.
  - id: mixture-of-experts
    prompt: What is a mixture-of-experts (MoE) model?
    options:
      - text: A panel of human experts reviewing outputs
      - text: An ensemble of separate chatbots
      - text: A model trained only on expert-written text
      - id: moe
        text: A model where a router activates only a subset of expert sub-networks per token, so total parameters are large but compute per token is lower
        correct: true
    explanation: Many recent large models (e.g. Mixtral, DeepSeek-V3) use MoE. Memory requirements still reflect total parameters.
---
## Learning objectives

- Explain embeddings and similarity
- Describe self-attention and causal masking
- Avoid embedding-model mismatches

## Embeddings in code

```python
import numpy as np

def cosine(a: np.ndarray, b: np.ndarray) -> float:
    return float(a @ b / (np.linalg.norm(a) * np.linalg.norm(b)))

q = embed("How do I reset my password?")
d1 = embed("Steps to change your account password")
d2 = embed("Quarterly revenue grew 12%")
print(cosine(q, d1), cosine(q, d2))
```

## Attention, intuitively

In "The trophy didn't fit in the suitcase because **it** was too big", attention lets the
representation of "it" draw mostly on "trophy". Change "big" to "small" and it shifts to
"suitcase". That context-sensitivity is what earlier word embeddings lacked.

## Sources

- Mikolov et al., "Efficient Estimation of Word Representations in Vector Space", 2013 — <https://arxiv.org/abs/1301.3781>
- Bolukbasi et al., "Man is to Computer Programmer as Woman is to Homemaker?", 2016 — <https://arxiv.org/abs/1607.06520>
- Vaswani et al., "Attention Is All You Need", 2017 — <https://arxiv.org/abs/1706.03762>
- Massive Text Embedding Benchmark (MTEB) — <https://huggingface.co/spaces/mteb/leaderboard>
- Jay Alammar, "The Illustrated Transformer" — <https://jalammar.github.io/illustrated-transformer/>
