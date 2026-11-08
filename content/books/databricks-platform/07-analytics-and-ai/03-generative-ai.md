---
title: Generative AI on Databricks
questions:
  - id: ai-query
    prompt: What does the `ai_query()` SQL function do?
    options:
      - text: Searches the documentation
      - id: invoke
        text: Calls a model serving endpoint (such as a foundation model) from SQL, row by row or in batch
        correct: true
      - text: Explains a query plan
      - text: Generates a dashboard
    explanation: AI functions bring LLM inference into SQL pipelines, for example to classify or extract fields from text.
  - id: vector-search
    prompt: What is Mosaic AI Vector Search used for?
    options:
      - id: retrieval
        text: Storing embeddings and retrieving the most similar documents, typically for retrieval-augmented generation (RAG)
        correct: true
      - text: Searching table names in Catalog Explorer
      - text: Replacing Delta Lake
    explanation: A vector search index can sync automatically from a Delta table as source documents change.
  - id: components
    prompt: Which are part of Databricks' generative AI tooling?
    type: multiple
    options:
      - text: Model Serving endpoints for custom models, foundation models and agents
        correct: true
      - text: MLflow tracing and evaluation for GenAI applications
        correct: true
      - text: An AI Gateway for governing, rate limiting and logging model usage
        correct: true
      - text: A requirement that all models be trained from scratch
    explanation: Most applications use existing foundation models plus retrieval, evaluation and governance.
---
## Learning objectives

- Call foundation models from SQL and Python
- Build retrieval-augmented generation with Vector Search
- Evaluate and govern GenAI applications

## AI functions in SQL

```sql
SELECT ticket_id,
       ai_classify(body, ARRAY('billing', 'bug', 'feature request', 'other')) AS category,
       ai_query('databricks-meta-llama-3-3-70b-instruct',
                CONCAT('Summarise this support ticket in one sentence: ', body)) AS summary
FROM prod.support.tickets
WHERE created_at >= current_date() - INTERVAL 1 DAY;
```

Task-specific functions (`ai_classify`, `ai_extract`, `ai_summarize`, `ai_translate`, ...) and the
general `ai_query` make LLMs part of ordinary data pipelines. Endpoint names vary by workspace and
region — check the Serving page for what is available.

## Retrieval-augmented generation

```text
Documents (volume) → parse & chunk → Delta table with embeddings
                                         │ sync
                                         ▼
                                 Vector Search index
                                         │ similarity search
User question → retrieve top-k chunks → prompt LLM with context → answer with citations
```

```python
from databricks.vector_search.client import VectorSearchClient

index = VectorSearchClient().get_index(endpoint_name="docs-endpoint", index_name="prod.ai.docs_index")
hits = index.similarity_search(query_text="How do I rotate an API key?", columns=["chunk", "url"], num_results=5)
```

## Agents and evaluation

- **Agent Framework** — build tool-using agents (in Python, often with LangGraph or similar) and deploy them to Model Serving
- **MLflow tracing** — capture every step of an agent's execution for debugging
- **MLflow evaluation** — LLM judges and custom scorers measure correctness, groundedness and safety
- **AI Gateway** — rate limits, usage tracking, payload logging and guardrails on endpoints

## Governance

Models, functions used as agent tools, vector indexes and the underlying tables all live in Unity
Catalog, so the same privileges, lineage and audit logs apply to AI applications as to data.
