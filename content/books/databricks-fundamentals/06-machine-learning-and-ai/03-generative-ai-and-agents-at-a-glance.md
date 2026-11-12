---
title: Generative AI and agents at a glance
questions:
  - id: mosaic
    prompt: Where does the "Mosaic AI" name come from?
    options:
      - text: A Databricks research paper
      - id: mosaicml
        text: Databricks' 2023 acquisition of MosaicML, a company focused on efficient training of large models
        correct: true
      - text: An open-source library
      - text: A partnership with Microsoft
    explanation: The acquisition was reported at about $1.3 billion. Mosaic AI became the umbrella for Databricks' GenAI tooling.
  - id: vector-search
    prompt: What is Vector Search used for?
    options:
      - text: Searching table names
      - text: Geographic queries
      - id: similarity
        text: Indexing embeddings and retrieving semantically similar content, typically for retrieval-augmented generation, with indexes that can sync from Delta tables
        correct: true
      - text: Replacing SQL
    explanation: Endpoints bill while running. Lakebase Search (announced 2026, beta) also offers vector and full-text search in Postgres.
  - id: agent-bricks
    prompt: What is Agent Bricks?
    options:
      - text: A Lego partnership
      - id: platform
        text: Databricks' platform for building, evaluating, optimising and governing AI agents on enterprise data, launched in 2025 with task-specific templates and expanded in 2026
        correct: true
      - text: A storage format for agents
      - text: An open-source protocol
    explanation: The AI Agent Fundamentals book covers it in detail.
  - id: governance-ai
    prompt: Why does Databricks emphasise governing AI assets in Unity Catalog?
    options:
      - text: Because models are tables
      - text: It is a legal requirement everywhere
      - text: For marketing only
      - id: one-model
        text: Models, functions used as tools, vector indexes, agents and their source data share one permission, lineage and audit model, which simplifies answering "who can do what with which data"
        correct: true
    explanation: The claim is substantive. Its value depends on your AI stack living mostly inside Databricks.
  - id: mlflow-genai
    prompt: Which MLflow capabilities target GenAI applications?
    type: multiple
    options:
      - id: tracing
        text: Tracing each step of an LLM or agent call
        correct: true
      - id: eval
        text: Evaluation with LLM judges and custom scorers
        correct: true
      - id: prompts
        text: A prompt registry for versioning prompts
        correct: true
      - text: Automatic guarantees that outputs are correct
    explanation: MLflow 3 (2025) reoriented substantially around GenAI. Evaluation provides evidence, not guarantees.
  - id: rag-on-db
    prompt: Put a minimal RAG flow on Databricks in order.
    options:
      - text: Serve endpoint → parse PDFs → chunk → evaluate
      - id: order
        text: Land documents in a volume → parse and chunk → embed into a Delta table → sync a vector index → retrieve at question time → prompt an LLM → evaluate and monitor
        correct: true
      - text: Prompt the LLM → land documents → train a model
      - text: Create a dashboard → add Genie
    explanation: Every stage has failure modes; the GenAI book covers them.
  - id: free-edition-ai
    prompt: Which GenAI capabilities did Databricks add to Free Edition in 2026, according to summit recaps?
    type: multiple
    options:
      - id: agent-bricks-free
        text: Agent Bricks
        correct: true
      - id: lakebase-free
        text: Lakebase
        correct: true
      - id: gpu-free
        text: Serverless GPUs
        correct: true
      - text: Unlimited free provisioned throughput
    explanation: Recaps listed Genie Code, serverless GPUs, Lakebase, Agent Bricks and Lakeflow Designer. Free Edition still has quotas.
  - id: hype-check
    prompt: Which question best tests a GenAI feature claim from any vendor?
    options:
      - id: eval-own
        text: How accurate is it on a labelled sample of our own data and questions, at what cost per request?
        correct: true
      - text: How many customers use it?
      - text: Was it announced at a keynote?
      - text: Does it use the largest model?
    explanation: Adoption counts and keynote demos are not evidence for your workload.
---
## Learning objectives

- Recognise Databricks' GenAI building blocks
- Outline a RAG pipeline on the platform
- Evaluate vendor claims with your own data

## The building blocks

| Need | Databricks component |
|---|---|
| Call an LLM | Foundation Model APIs, external models, AI Functions |
| Retrieve your documents | Vector Search, Lakebase Search |
| Parse documents | `ai_parse_document`, Document Intelligence |
| Build agents | Agent Bricks, agent frameworks deployed to serving or Apps |
| Observe and evaluate | MLflow tracing, evaluation, judges |
| Govern | Unity Catalog, AI gateway |

The two companion books — *Generative AI Fundamentals* and *AI Agent Fundamentals* — go deep on each.

## Sources

- Databricks press release, "Databricks Completes Acquisition of MosaicML" (2023) — <https://www.databricks.com/company/newsroom/press-releases/databricks-completes-acquisition-mosaicml>
- Databricks docs, "Mosaic AI Vector Search" — <https://docs.databricks.com/aws/en/vector-search/vector-search>
- Databricks blog, "Agent Bricks: Data + AI Summit 2026" — <https://www.databricks.com/blog/agent-bricks-dais-2026>
- MLflow, "MLflow 3" — <https://mlflow.org/docs/latest/genai/>
