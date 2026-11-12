---
title: AI/BI dashboards and Genie
questions:
  - id: aibi
    prompt: What is Databricks AI/BI?
    options:
      - text: An external BI vendor Databricks resells
      - text: A machine learning library
      - id: aibi
        text: Databricks' built-in BI offering — AI/BI dashboards for visual reports and Genie for natural-language questions over data
        correct: true
      - text: A certification
    explanation: It lowers cost for organisations that would otherwise buy BI seats, though mature BI tools still have deeper modelling and formatting features.
  - id: genie-how
    prompt: How does a Genie space answer a business question in natural language?
    options:
      - text: It searches the web
      - text: It looks up a fixed list of answers
      - id: text-to-sql
        text: An LLM generates SQL against the tables in the space, guided by metadata, instructions, example queries and trusted assets, then runs it with the user's permissions
        correct: true
      - text: It asks a human analyst
    explanation: Genie is text-to-SQL with curation. Its quality depends heavily on table and column descriptions and the examples authors provide.
  - id: genie-risks
    prompt: Which are real risks of natural-language querying?
    type: multiple
    options:
      - id: wrong-sql
        text: Plausible but wrong SQL — for example the wrong join or a misunderstood metric definition — producing a confident wrong number
        correct: true
      - id: ambiguity
        text: Ambiguous business terms ("active customer", "revenue") interpreted differently from finance's definition
        correct: true
      - id: overtrust
        text: Users trusting answers without checking the generated SQL
        correct: true
      - text: Users being able to see data they have no permission to see
    explanation: Genie runs queries as the user, so permissions apply. Correctness is the harder problem; that is why trusted assets, metric views and benchmarks matter.
  - id: improve-genie
    prompt: What improves Genie accuracy most?
    type: multiple
    options:
      - id: metadata
        text: Clear table and column descriptions in Unity Catalog
        correct: true
      - id: examples
        text: Example SQL queries and instructions for business terms
        correct: true
      - id: metrics
        text: Governed metric definitions (metric views) and a small, focused set of tables per space
        correct: true
      - text: Adding every table in the metastore to one space
    explanation: Text-to-SQL accuracy falls with schema size and ambiguity. Benchmarks in a Genie space let authors measure accuracy on known questions.
  - id: benchmark
    prompt: Why should a Genie space author create benchmark questions with known correct answers?
    options:
      - id: measure
        text: To measure accuracy before release and detect regressions when instructions or tables change
        correct: true
      - text: Because the UI requires exactly ten
      - text: To train the underlying LLM from scratch
      - text: To reduce DBU usage
    explanation: Evaluation is as important for BI agents as for any GenAI system.
  - id: dashboard-share
    prompt: When a published AI/BI dashboard is shared with embedded credentials, whose permissions are used to query data?
    options:
      - text: Each viewer's, always
      - text: Nobody's — data is public
      - text: The account admin's
      - id: publisher
        text: The publisher's credentials, so viewers can see data they could not query directly; without embedded credentials, viewers' own permissions apply
        correct: true
    explanation: Embedding is convenient for sharing with non-Databricks users. It is also a way to leak data if the publisher has broad access. Choose deliberately.
  - id: genie-one
    prompt: What was announced as Genie One at Data + AI Summit 2026?
    options:
      - text: A new SQL warehouse size
      - id: coworker
        text: An agentic assistant for business users across structured and unstructured data that can create reports, schedule tasks and set alerts
        correct: true
      - text: A free dashboard template
      - text: A mobile game
    explanation: Recaps described it as GA. Treat agentic BI claims like any GenAI feature — evaluate on your own questions and data.
  - id: semantic
    prompt: Why do metric definitions matter more with natural-language BI than with hand-written dashboards?
    options:
      - text: They do not
      - id: llm-guess
        text: Without a governed definition, the model must guess how to compute a metric from raw columns, and different questions may compute it differently
        correct: true
      - text: LLMs cannot do arithmetic
      - text: Dashboards do not use metrics
    explanation: A semantic layer turns "revenue" into a single governed calculation that people and agents reuse.
  - id: bi-vs-tools
    prompt: Which statement about AI/BI versus Power BI or Tableau is fair?
    options:
      - text: AI/BI makes them obsolete for every organisation
      - text: AI/BI cannot visualise data
      - id: tradeoff
        text: AI/BI is integrated with Unity Catalog and included in compute costs; established BI tools offer richer formatting, modelling and ecosystem — many organisations use both
        correct: true
      - text: Power BI cannot connect to Databricks
    explanation: Platform vendors bundle BI to increase stickiness. Evaluate on your reporting requirements.
---
## Learning objectives

- Describe AI/BI dashboards and Genie spaces
- Understand how text-to-SQL works and how it fails
- Improve and measure Genie accuracy

## How Genie answers

```text
"What was revenue in EMEA last quarter?"
   │
   ▼  context: table/column descriptions, instructions, example SQL, metric views
LLM generates SQL ──► runs on a SQL warehouse as the user ──► result + chart + the SQL
   │
   └─ the user (or author) can inspect and correct the SQL
```

Always teach users to look at the SQL for numbers that matter.

## Sources

- Databricks docs, "AI/BI dashboards" — <https://docs.databricks.com/aws/en/dashboards/>
- Databricks docs, "AI/BI Genie spaces" — <https://docs.databricks.com/aws/en/genie/>
- Databricks docs, "Genie benchmarks" — <https://docs.databricks.com/aws/en/genie/benchmarks>
- Atlan, "Databricks Data + AI Summit 2026 announcements" — <https://atlan.com/know/ai-agent/databricks/databricks-data-ai-summit-2026-announcements/>
