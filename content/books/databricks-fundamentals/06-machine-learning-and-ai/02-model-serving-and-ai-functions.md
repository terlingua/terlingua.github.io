---
title: Model Serving and AI Functions
questions:
  - id: serving
    prompt: What is Model Serving?
    options:
      - text: A batch scoring notebook
      - id: endpoints
        text: Managed, autoscaling REST endpoints for custom models, foundation models, external models and agents
        correct: true
      - text: A model training service
      - text: A data catalog
    explanation: Endpoints can scale to zero to save cost — at the price of cold-start latency.
  - id: fm-api
    prompt: What are Foundation Model APIs?
    options:
      - text: APIs for training models from scratch
      - text: Open-source code for transformers
      - id: hosted
        text: Databricks-hosted access to popular open and proprietary LLMs and embedding models, billed pay-per-token or with provisioned throughput
        correct: true
      - text: A list of public datasets
    explanation: Model availability changes often and varies by region. Pin model names in config, not in code, and expect retirements.
  - id: external-models
    prompt: What are external models in Model Serving?
    options:
      - id: proxy
        text: Endpoints that route to third-party providers (such as OpenAI, Anthropic, Google) with credentials stored centrally and governance applied by the AI gateway
        correct: true
      - text: Models trained outside the company and copied in
      - text: Models running on laptops
      - text: Models without weights
    explanation: Data sent to external providers leaves Databricks. Check data-processing terms and residency.
  - id: ai-functions
    prompt: What do AI Functions such as `ai_query`, `ai_classify` and `ai_extract` let you do?
    options:
      - text: Train models in SQL
      - text: Write Python inside SQL
      - text: Speed up joins
      - id: llm-sql
        text: Call LLMs and models from SQL over table rows, for tasks such as classification, extraction, summarisation and translation
        correct: true
    explanation: They make batch LLM inference accessible to analysts. Cost scales with rows × tokens, so test on a sample first.
  - id: ai-functions-cost
    prompt: "An analyst runs `ai_summarize(body)` on a 50-million-row table. What should they consider first?"
    type: multiple
    options:
      - id: cost
        text: Token cost and throughput limits across 50 million calls
        correct: true
      - id: sample
        text: Testing quality on a sample and measuring it
        correct: true
      - id: incremental
        text: Processing only new rows incrementally rather than the whole table each run
        correct: true
      - text: Nothing; AI Functions are free
    explanation: LLM inference is far more expensive per row than SQL functions. Batch inference with provisioned throughput can lower unit cost.
  - id: parse-document
    prompt: What does `ai_parse_document` do?
    options:
      - text: Parses SQL syntax
      - id: parse
        text: Extracts structured content (text, tables, layout) from unstructured documents such as PDFs and images, typically as the first step of document intelligence or RAG
        correct: true
      - text: Validates JSON
      - text: Translates code
    explanation: Parsing quality determines everything downstream; scanned, handwritten or complex-layout documents need checking.
  - id: gateway
    prompt: Which capabilities does an AI gateway on serving endpoints provide?
    type: multiple
    options:
      - id: rate
        text: Rate limits and usage tracking
        correct: true
      - id: logging
        text: Payload logging to inference tables for audit and evaluation
        correct: true
      - id: guardrails
        text: Guardrails such as PII detection and safety filtering
        correct: true
      - id: fallback
        text: Traffic splitting and fallbacks between models
        correct: true
    explanation: Announced as Unity AI Gateway in 2026 with MCP and agent governance. Guardrails reduce but do not eliminate risk.
  - id: scale-zero
    prompt: What is the trade-off of scale-to-zero on a serving endpoint?
    options:
      - text: There is none
      - text: It costs more
      - id: cold-start
        text: It saves cost while idle, but the first request after idle waits for a cold start, which can be unacceptable for user-facing latency
        correct: true
      - text: It disables logging
    explanation: For production user-facing endpoints, keep minimum capacity; for internal batch-ish use, scale to zero.
  - id: batch-vs-realtime
    prompt: Which inference mode suits scoring 10 million customers nightly?
    options:
      - id: batch
        text: Batch inference in a job (Spark UDF over the model, or AI Functions), not millions of REST calls to a real-time endpoint
        correct: true
      - text: Real-time endpoint calls from a for-loop
      - text: A dashboard
      - text: Manual entry
    explanation: Real-time endpoints are for low-latency, per-request predictions. Batch is cheaper and simpler when latency does not matter.
  - id: model-deprecation
    prompt: Databricks hosted its own DBRX model in 2024 and later retired it from Foundation Model APIs. What lesson does this teach?
    options:
      - text: Never use hosted models
      - text: Only use proprietary models
      - text: Models never change
      - id: churn
        text: Model availability churns quickly; design applications to swap models via configuration and keep evaluation sets to validate replacements
        correct: true
    explanation: Every provider retires models. Abstraction plus evaluation is the timeless defence.
---
## Learning objectives

- Choose between real-time endpoints and batch inference
- Use Foundation Model APIs, external models and AI Functions
- Understand the AI gateway and cost trade-offs

## AI Functions in SQL

```sql
SELECT ticket_id,
       ai_classify(body, ARRAY('billing', 'bug', 'feature', 'other')) AS category,
       ai_extract(body, ARRAY('product', 'order_id'))                 AS entities
FROM main.support.tickets
WHERE created_date = current_date();
```

## Calling an endpoint with the OpenAI-compatible client

```python
from openai import OpenAI
client = OpenAI(api_key=token, base_url=f"{host}/serving-endpoints")
reply = client.chat.completions.create(
    model="my-chat-endpoint",
    messages=[{"role": "user", "content": "Summarise our refund policy."}])
```

## Sources

- Databricks docs, "Model Serving" — <https://docs.databricks.com/aws/en/machine-learning/model-serving/>
- Databricks docs, "AI Functions" — <https://docs.databricks.com/aws/en/large-language-models/ai-functions>
- Databricks docs, "AI Gateway" — <https://docs.databricks.com/aws/en/ai-gateway/>
- Databricks docs, "Foundation Model APIs retired models" — <https://docs.databricks.com/aws/en/machine-learning/retired-models-policy>
