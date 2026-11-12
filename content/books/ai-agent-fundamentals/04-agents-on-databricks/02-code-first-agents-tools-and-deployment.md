---
title: Code-first agents, tools and deployment
questions:
  - id: uc-functions
    prompt: Why register agent tools as Unity Catalog functions?
    options:
      - text: They run faster than Python
      - id: governed
        text: Tools become governed, discoverable objects with permissions and lineage, reusable across agents and callable via managed MCP servers
        correct: true
      - text: It is the only way to write tools
      - text: Unity Catalog generates tool code
    explanation: Governance applies to who can execute a tool, not only who can read data.
  - id: managed-mcp
    prompt: What do Databricks managed MCP servers expose?
    options:
      - text: Cluster logs only
      - text: The Databricks web UI
      - id: assets
        text: Workspace assets such as Unity Catalog functions, Vector Search indexes and Genie spaces as MCP tools, with Unity Catalog permissions enforced
        correct: true
      - text: Third-party SaaS only
    explanation: Any MCP-capable agent — on or off Databricks — can use them, subject to authentication.
  - id: responses-agent
    prompt: What is MLflow's `ResponsesAgent` interface for?
    options:
      - id: interface
        text: A standard interface for authoring agents (compatible with the OpenAI Responses API shape) so they can be logged, evaluated, traced and served consistently regardless of framework
        correct: true
      - text: Automatically responding to emails
      - text: Training models
      - text: Creating dashboards
    explanation: It wraps agents built with LangGraph, OpenAI Agents SDK or plain Python.
  - id: auth-modes
    prompt: Which authentication approaches can a deployed agent use to access data?
    type: multiple
    options:
      - id: sp
        text: A service principal (system identity) with its own permissions
        correct: true
      - id: obo
        text: On-behalf-of-user — acting with the invoking user's permissions
        correct: true
      - text: An admin's personal access token embedded in code
    explanation: On-behalf-of-user prevents an agent from becoming a privilege-escalation path; system identities are simpler but must be scoped tightly.
  - id: escalation
    prompt: An agent running as a powerful service principal answers any employee's questions over HR tables. What is the risk?
    options:
      - text: None
      - text: Higher latency
      - id: escalation
        text: Privilege escalation — users obtain data through the agent that they could not query directly
        correct: true
      - text: The agent will refuse to answer
    explanation: The confused-deputy problem. Use on-behalf-of-user access or row-level controls keyed to the requesting user.
  - id: deploy-targets
    prompt: Where can code-first agents be deployed on Databricks?
    type: multiple
    options:
      - id: serving
        text: Model Serving endpoints
        correct: true
      - id: apps
        text: Databricks Apps, which Databricks highlighted for agent harnesses in 2026
        correct: true
      - text: Only on SQL warehouses
    explanation: Choose by latency, scaling, UI needs and how the agent is called.
  - id: review-app
    prompt: What is the purpose of a review app for agents?
    options:
      - text: App store reviews
      - id: sme
        text: Letting subject-matter experts chat with an agent and label or comment on responses, producing evaluation data and feedback
        correct: true
      - text: Code review for agent repositories
      - text: Reviewing cloud bills
    explanation: Expert labels are the most valuable evaluation input.
  - id: lifecycle
    prompt: Put the code-first agent lifecycle on Databricks in order.
    options:
      - text: Deploy → author → evaluate → trace
      - text: Evaluate → deploy → author → monitor
      - text: Monitor → author → deploy → evaluate
      - id: order
        text: Author (with tracing) → evaluate offline → log and register in Unity Catalog → deploy → monitor with judges and feedback → iterate
        correct: true
    explanation: The same lifecycle applies on any platform.
  - id: genie-as-tool
    prompt: Why expose a Genie space to an agent instead of a raw SQL tool?
    options:
      - id: curated
        text: The Genie space carries curated instructions, examples and governed metrics, so the agent's data questions are answered more reliably and with the user's permissions
        correct: true
      - text: Genie is cheaper than SQL
      - text: Raw SQL is not allowed
      - text: Agents cannot run SQL
    explanation: Prefer curated, governed interfaces over open-ended tools.
---
## Learning objectives

- Govern tools with Unity Catalog and MCP
- Choose authentication modes that avoid privilege escalation
- Follow the code-first agent lifecycle

## A Unity Catalog function as a tool

```sql
CREATE OR REPLACE FUNCTION main.support.order_status(order_id STRING)
RETURNS TABLE (status STRING, carrier STRING, eta DATE)
COMMENT 'Look up the status, carrier and estimated delivery date of an order by ID (format A-1234).'
RETURN SELECT status, carrier, eta FROM main.sales.orders WHERE id = order_id;

GRANT EXECUTE ON FUNCTION main.support.order_status TO `support-agents`;
```

## Logging and deploying a code-first agent

```python
import mlflow
from databricks import agents

with mlflow.start_run():
    info = mlflow.pyfunc.log_model(
        name="agent",
        python_model="agent.py",
        registered_model_name="main.support.order_agent",
    )

agents.deploy("main.support.order_agent", info.registered_model_version)
```

APIs evolve; check the current docs for the recommended deployment target.

## Sources

- Databricks docs, "Author AI agents in code" — <https://docs.databricks.com/aws/en/generative-ai/agent-framework/author-agent>
- Databricks docs, "Managed MCP servers" — <https://docs.databricks.com/aws/en/generative-ai/mcp/managed-mcp>
- Databricks docs, "Agent authentication" — <https://docs.databricks.com/aws/en/generative-ai/agent-framework/agent-authentication>
- Databricks docs, "Unity Catalog function tools" — <https://docs.databricks.com/aws/en/generative-ai/agent-framework/create-custom-tool>
