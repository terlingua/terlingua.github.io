---
title: MLflow for GenAI
questions:
  - id: tracing
    prompt: What does MLflow tracing capture?
    options:
      - text: Only the final answer
      - id: spans
        text: A tree of spans for each request — LLM calls, retrieval, tool calls — with inputs, outputs, latency and token usage
        correct: true
      - text: Network packets
      - text: GPU utilisation only
    explanation: Tracing is OpenTelemetry-compatible and autologs popular libraries (OpenAI, LangChain, LlamaIndex and others).
  - id: why-trace
    prompt: Why is tracing essential for RAG and agents?
    type: multiple
    options:
      - id: localise
        text: To see which step failed — retrieval, prompt, model or tool
        correct: true
      - id: cost
        text: To measure latency and token cost per step
        correct: true
      - id: dataset
        text: To turn real traces into evaluation examples
        correct: true
      - text: To make the model smarter automatically
    explanation: Without traces, debugging a multi-step system is guesswork.
  - id: autolog
    prompt: "What does `mlflow.openai.autolog()` do?"
    options:
      - text: Logs in to OpenAI automatically
      - text: Trains an OpenAI model
      - id: trace-calls
        text: Automatically records traces for OpenAI client calls without changing application code
        correct: true
      - text: Deletes logs
    explanation: Use `@mlflow.trace` to add your own functions as spans.
  - id: scorers
    prompt: What are MLflow scorers?
    options:
      - text: Sports statistics
      - id: scorers
        text: Functions — built-in LLM judges or custom code — that evaluate traces or outputs on criteria such as correctness, groundedness, safety or guidelines
        correct: true
      - text: Billing metrics
      - text: Cluster health checks
    explanation: The same scorers can run offline in evaluation and on sampled production traffic.
  - id: prompt-registry
    prompt: What does the MLflow prompt registry provide?
    options:
      - text: A marketplace for prompts
      - text: Encryption of prompts
      - text: A prompt-writing AI
      - id: versioning
        text: Versioning, aliases and lineage for prompt templates, so applications load a named version and changes are traceable to evaluation results
        correct: true
    explanation: It applies the model-registry idea to prompts.
  - id: feedback
    prompt: How can human feedback be captured?
    options:
      - id: assessments
        text: Attaching user or expert feedback (assessments) to traces, enabling labelled datasets and judge calibration
        correct: true
      - text: It cannot be captured
      - text: Only by email
      - text: Only through Genie
    explanation: Thumbs up/down is weak signal; expert labels with reasons are strong.
  - id: oss
    prompt: Is MLflow GenAI tracing usable outside Databricks?
    options:
      - text: "No"
      - text: Only on Azure
      - id: yes-oss
        text: Yes — MLflow is open source and can run with its own tracking server; Databricks adds managed hosting and Unity Catalog integration
        correct: true
      - text: Only with Spark
    explanation: Competing open tools include Langfuse, Arize Phoenix and OpenTelemetry-based backends.
  - id: production-monitoring
    prompt: What does production monitoring for a GenAI app typically include?
    type: multiple
    options:
      - id: quality
        text: Judges running on a sample of live traces
        correct: true
      - id: ops
        text: Latency, error rates and token cost
        correct: true
      - id: user
        text: User feedback and escalation rates
        correct: true
      - text: Nothing once evaluation passed
    explanation: Inputs drift; models change; monitoring closes the loop.
---
## Learning objectives

- Trace GenAI applications with MLflow
- Evaluate with scorers and judges
- Version prompts and monitor production

## Tracing a RAG function

```python
import mlflow

mlflow.openai.autolog()

@mlflow.trace(span_type="RETRIEVER")
def retrieve(question: str) -> list[dict]:
    return index.similarity_search(query_text=question, columns=["chunk_id", "text"], num_results=5)["result"]["data_array"]

@mlflow.trace
def answer(question: str) -> str:
    chunks = retrieve(question)
    prompt = mlflow.genai.load_prompt("prompts:/main.support.answer@production")
    return client.chat.completions.create(
        model="databricks-claude-sonnet-4",
        messages=[{"role": "user", "content": prompt.format(question=question, chunks=chunks)}],
    ).choices[0].message.content
```

Endpoint names vary; read them from configuration.

## Sources

- MLflow, "Tracing" — <https://mlflow.org/docs/latest/genai/tracing/>
- MLflow, "Evaluate and monitor" — <https://mlflow.org/docs/latest/genai/eval-monitor/>
- MLflow, "Prompt registry" — <https://mlflow.org/docs/latest/genai/prompt-registry/>
- Databricks docs, "MLflow 3 for GenAI" — <https://docs.databricks.com/aws/en/mlflow3/genai/>
