---
title: The Databricks GenAI stack
questions:
  - id: course-topics
    prompt: According to its course page, which four themes does Databricks' Generative AI Fundamentals course cover?
    options:
      - id: four
        text: Core GenAI concepts; identifying high-value opportunities; ensuring governance and trust; implementing GenAI on Databricks
        correct: true
      - text: Python, Scala, Java and R
      - text: Networking, storage, compute and billing
      - text: Pretraining, quantisation, CUDA and kernels
    explanation: This book covers all four, and goes further on mechanics, failure modes and evaluation.
  - id: data-intelligence-claim
    prompt: Databricks argues GenAI success depends on "data intelligence" — models that understand your organisation's data. Which part of the claim is broadly supported?
    options:
      - text: That only Databricks can do this
      - id: data-matters
        text: That enterprise GenAI quality depends heavily on access to well-governed, high-quality internal data and metadata
        correct: true
      - text: That models no longer matter
      - text: That fine-tuning is always required
    explanation: The principle is vendor-neutral; the specific implementation is a product choice.
  - id: map
    prompt: Match the need to the Databricks component. Which pairs are correct?
    type: multiple
    options:
      - id: fm
        text: Call a hosted LLM → Foundation Model APIs / Model Serving
        correct: true
      - id: vs
        text: Semantic retrieval → Vector Search
        correct: true
      - id: sql
        text: Batch LLM over table rows → AI Functions in SQL
        correct: true
      - id: gov
        text: Govern models, tools and indexes → Unity Catalog and the AI gateway
        correct: true
    explanation: All four are standard building blocks.
  - id: vs-sync
    prompt: What is a Delta Sync vector index?
    options:
      - text: An index that must be updated by hand
      - text: A copy of the vector database in Delta
      - id: sync
        text: An index that stays in sync with a source Delta table, computing embeddings (or using provided ones) as rows change
        correct: true
      - text: A backup of embeddings
    explanation: It turns index freshness into a pipeline concern you already understand. Requires change data feed on the source table.
  - id: endpoint-cost
    prompt: Which Databricks GenAI resources bill while running even with no traffic?
    type: multiple
    options:
      - id: vs-endpoint
        text: Vector Search endpoints
        correct: true
      - id: provisioned
        text: Provisioned-throughput serving endpoints without scale-to-zero
        correct: true
      - text: Pay-per-token Foundation Model API calls that are not made
    explanation: Pay-per-token bills only per request. Inventory always-on resources in system tables.
  - id: inference-tables
    prompt: What are inference tables?
    options:
      - text: Tables of model weights
      - text: Tables used for training only
      - text: Tables of GPU temperatures
      - id: log
        text: Delta tables that log requests and responses of serving endpoints, used for monitoring, evaluation and audit
        correct: true
    explanation: Apply retention and access controls; they may contain personal data.
  - id: ai-query-batch
    prompt: When is `ai_query` over a table better than calling an endpoint from application code?
    options:
      - id: batch
        text: For batch enrichment of many rows in a pipeline, where SQL integration, parallelism and governance matter more than per-request latency
        correct: true
      - text: For a chatbot's interactive replies
      - text: Never
      - text: Only for images
    explanation: Use endpoints for interactive apps; AI Functions for data pipelines.
  - id: lock-check
    prompt: Which parts of a Databricks GenAI app are most portable?
    options:
      - text: AI Functions SQL and Agent Bricks configurations
      - id: open-parts
        text: Application code using OpenAI-compatible clients, MLflow traces and evaluation code, and Delta tables of chunks and embeddings
        correct: true
      - text: Vector Search endpoints
      - text: Genie spaces
    explanation: OpenAI-compatible endpoints and open-source MLflow keep the code layer portable.
---
## Learning objectives

- Map GenAI needs to Databricks components
- Understand index sync, inference logging and cost behaviour
- Separate portable from platform-specific parts

## End-to-end RAG on Databricks

```text
/Volumes/main/docs/raw/*.pdf
   └─ ai_parse_document ─► main.docs.parsed ─► chunking job ─► main.docs.chunks (CDF on)
                                                                     │ Delta Sync
                                                                     ▼
                                               Vector Search index main.docs.chunks_index
                                                                     │
App (Databricks Apps) ─► retrieve (filtered by user groups) ─► Foundation Model API
   │                                                                 │
   └──────────── MLflow tracing, evaluation, inference tables ◄──────┘
```

```sql
CREATE OR REPLACE TABLE main.docs.parsed AS
SELECT path, ai_parse_document(content) AS parsed
FROM read_files('/Volumes/main/docs/raw/', format => 'binaryFile');
```

## Sources

- Databricks, Generative AI Fundamentals — <https://www.databricks.com/learn/training/generative-ai-fundamentals-accreditation>
- Databricks docs, "Vector Search" — <https://docs.databricks.com/aws/en/vector-search/vector-search>
- Databricks docs, "ai_parse_document" — <https://docs.databricks.com/aws/en/sql/language-manual/functions/ai_parse_document>
- Databricks docs, "Inference tables" — <https://docs.databricks.com/aws/en/machine-learning/model-serving/inference-tables>
