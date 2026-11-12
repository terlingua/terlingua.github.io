---
title: Chunking, embeddings and vector search
questions:
  - id: why-chunk
    prompt: Why split documents into chunks?
    options:
      - text: Vector databases cannot store long text
      - id: precision
        text: Retrieval works best on focused passages; whole documents dilute embeddings, waste context and cost tokens
        correct: true
      - text: To make documents load faster in a browser
      - text: Because of copyright
    explanation: Chunk size trades precision (small) against context completeness (large).
  - id: strategies
    prompt: Which are legitimate chunking strategies?
    type: multiple
    options:
      - id: fixed
        text: Fixed-size token windows with overlap
        correct: true
      - id: structural
        text: Splitting by document structure — headings, sections, paragraphs
        correct: true
      - id: semantic
        text: Semantic chunking at topic boundaries
        correct: true
      - id: parent
        text: Small chunks for retrieval that return their larger parent section to the LLM
        correct: true
    explanation: Structure-aware chunking usually beats naive fixed windows for well-formatted documents.
  - id: tables
    prompt: A PDF contains tables of prices. Naive text extraction flattens them into a stream of numbers. What is the consequence?
    options:
      - text: None; LLMs understand any text
      - text: The PDF is deleted
      - id: garbage
        text: Rows and columns lose their association, so retrieved chunks are misleading and answers about prices are wrong
        correct: true
      - text: Embeddings become faster
    explanation: Use layout-aware parsing (for example `ai_parse_document` or dedicated parsers) and inspect parsed output before indexing.
  - id: metadata
    prompt: Why store metadata (source, section, date, access groups) with each chunk?
    options:
      - text: It is required by law
      - id: filter
        text: For filtering (e.g. by product or date), enforcing permissions, citing sources and debugging retrieval
        correct: true
      - text: To increase vector dimensions
      - text: It has no use
    explanation: Permission filtering at retrieval time is essential — otherwise the LLM may summarise documents the user cannot open.
  - id: ann
    prompt: Vector indexes typically use approximate nearest neighbour (ANN) algorithms such as HNSW. What is the trade-off?
    options:
      - id: approx
        text: Much faster search at large scale in exchange for occasionally missing some true nearest neighbours (recall below 100%)
        correct: true
      - text: Exact results, slower
      - text: No trade-off
      - text: Lower storage at the cost of encryption
    explanation: Index parameters tune the recall/latency balance.
  - id: rerank
    prompt: What does a reranker do?
    options:
      - text: Sorts results alphabetically
      - text: Removes duplicates only
      - text: Translates queries
      - id: cross-encoder
        text: Rescores the top candidates with a more accurate (often cross-encoder) model that reads query and passage together, improving precision before generation
        correct: true
    explanation: Retrieve, say, 50 cheaply, rerank to the best 5.
  - id: query-mismatch
    prompt: Users ask short, informal questions; documents are long and formal. Which techniques can close the gap?
    type: multiple
    options:
      - id: rewrite
        text: Query rewriting or expansion by an LLM
        correct: true
      - id: hyde
        text: HyDE — embedding a hypothetical answer instead of the question
        correct: true
      - id: hybrid
        text: Hybrid keyword + vector search
        correct: true
      - text: Increasing temperature
    explanation: Each adds latency and cost; validate with retrieval metrics.
  - id: dimensions
    prompt: Embeddings with more dimensions are…
    options:
      - text: Always better
      - text: Always worse
      - id: tradeoff
        text: Not automatically better — they cost more storage and compute; quality depends on the model and your data
        correct: true
      - text: Required by every vector database
    explanation: Some models support truncating dimensions (Matryoshka embeddings) with modest quality loss.
  - id: vector-db-choice
    prompt: Do you always need a dedicated vector database?
    options:
      - id: not-always
        text: No — Postgres with pgvector, search engines, lakehouse vector search or even in-memory indexes may suffice depending on scale and existing stack
        correct: true
      - text: Yes, always
      - text: Only for images
      - text: Only on AWS
    explanation: Prefer the store you already operate unless scale or features demand otherwise.
---
## Learning objectives

- Choose chunking strategies and handle complex layouts
- Use metadata for filtering and permissions
- Understand ANN search, reranking and query transformation

## Chunking in code

```python
def chunk(text: str, size: int = 400, overlap: int = 50) -> list[str]:
    tokens = enc.encode(text)
    step = size - overlap
    return [enc.decode(tokens[i:i + size]) for i in range(0, len(tokens), step)]
```

Simple, but structure-blind. For manuals and policies, split on headings first and fall back to windows for long sections.

## Retrieval with permission filtering

```python
hits = index.similarity_search(
    query_text=question,
    columns=["chunk_id", "text", "source_url"],
    filters={"access_group": user_groups},
    num_results=20,
    query_type="hybrid",
)
```

## Sources

- Malkov and Yashunin, "HNSW", 2016 — <https://arxiv.org/abs/1603.09320>
- Gao et al., "Precise Zero-Shot Dense Retrieval without Relevance Labels" (HyDE), 2022 — <https://arxiv.org/abs/2212.10496>
- Kusupati et al., "Matryoshka Representation Learning", 2022 — <https://arxiv.org/abs/2205.13147>
- pgvector — <https://github.com/pgvector/pgvector>
