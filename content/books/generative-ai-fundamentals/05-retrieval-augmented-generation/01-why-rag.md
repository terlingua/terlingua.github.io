---
title: Why RAG exists
questions:
  - id: rag-def
    prompt: What is retrieval-augmented generation (RAG)?
    options:
      - text: Training a model on your documents
      - id: def
        text: Retrieving relevant content from an external source at question time and supplying it to the model as context for its answer
        correct: true
      - text: Generating random documents
      - text: A compression algorithm
    explanation: Introduced by Lewis et al. (2020, Facebook AI). Today "RAG" usually means any retrieve-then-generate pattern.
  - id: why
    prompt: Which problems does RAG address?
    type: multiple
    options:
      - id: private
        text: The model does not know your private or internal data
        correct: true
      - id: fresh
        text: The model's knowledge is frozen at its cut-off
        correct: true
      - id: cite
        text: Answers need citations to verifiable sources
        correct: true
      - id: access
        text: Different users should only see answers based on documents they may access
        correct: true
    explanation: Retrieval can enforce per-user permissions; a fine-tuned model cannot "unlearn" data for one user.
  - id: rag-vs-ft
    prompt: Your support documents change weekly. Which approach suits injecting their knowledge?
    options:
      - text: Fine-tune weekly
      - text: Pretrain a new model
      - id: rag
        text: RAG — update the index as documents change; no retraining needed
        correct: true
      - text: Paste all documents into every prompt
    explanation: Fine-tuning is better for teaching style or format than for facts that change.
  - id: long-context-vs-rag
    prompt: Models now accept a million tokens. Does that make RAG obsolete?
    options:
      - text: Yes, always
      - id: not-obsolete
        text: Not generally — stuffing everything costs more per query, is slower, degrades with irrelevant context, and cannot enforce per-user access; long context complements retrieval
        correct: true
      - text: RAG never worked
      - text: Long context is free
    explanation: For a handful of documents, putting them all in context can be simpler and better. Measure both.
  - id: pipeline
    prompt: What are the two main phases of a RAG system?
    options:
      - text: Training and inference
      - text: Upload and download
      - text: Encrypt and decrypt
      - id: phases
        text: Offline indexing (parse, chunk, embed, store) and online serving (retrieve, augment the prompt, generate)
        correct: true
    explanation: Most quality problems originate in the offline phase — especially parsing and chunking.
  - id: hybrid
    prompt: What is hybrid search?
    options:
      - id: hybrid
        text: Combining keyword search (e.g. BM25) with vector similarity, then merging results (e.g. with reciprocal rank fusion)
        correct: true
      - text: Searching two clouds
      - text: Using two LLMs
      - text: Searching images and text
    explanation: Keyword search excels at exact terms — product codes, names, error numbers — that embeddings blur.
  - id: grounded-prompt
    prompt: What should a RAG prompt instruct the model to do?
    type: multiple
    options:
      - id: only-context
        text: Answer only from the provided context
        correct: true
      - id: say-unknown
        text: Say when the context does not contain the answer
        correct: true
      - id: cite
        text: Cite the IDs of the chunks used
        correct: true
      - text: Ignore the context if it disagrees with its training
    explanation: These instructions reduce, but do not eliminate, unsupported answers.
  - id: not-search
    prompt: Is RAG the same as search?
    options:
      - text: "Yes"
      - text: RAG never uses search
      - id: search-plus
        text: Retrieval is search; RAG adds synthesis by an LLM — which adds value and adds a new failure mode (wrong synthesis of right sources)
        correct: true
      - text: Search is a type of RAG
    explanation: Sometimes good search results with snippets are better for users than a synthesised answer.
---
## Learning objectives

- Explain what RAG is and why it exists
- Compare RAG, fine-tuning and long context
- Describe the indexing and serving phases

## The pipeline

```text
OFFLINE                                            ONLINE
documents ─► parse ─► chunk ─► embed ─► index      question ─► embed ─► retrieve top-k
                                   (+ metadata,             ─► (rerank) ─► prompt with chunks
                                     permissions)           ─► LLM ─► answer + citations
```

## Prompt template

```text
Answer the question using only the sources below. If they do not contain the answer,
say "I can't find that in the documentation." Cite sources as [id].

<sources>
[17] Password resets are available from Settings → Security...
[42] Administrators can force a reset for any user...
</sources>

Question: How do I reset another user's password?
```

## Sources

- Lewis et al., "Retrieval-Augmented Generation for Knowledge-Intensive NLP Tasks", 2020 — <https://arxiv.org/abs/2005.11401>
- Robertson and Zaragoza, "The Probabilistic Relevance Framework: BM25 and Beyond", 2009 — <https://dl.acm.org/doi/10.1561/1500000019>
- Cormack et al., "Reciprocal Rank Fusion", 2009 — <https://dl.acm.org/doi/10.1145/1571941.1572114>
