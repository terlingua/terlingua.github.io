---
title: Tools and function calling
questions:
  - id: function-calling
    prompt: How does function (tool) calling work?
    options:
      - text: The model connects to your database over the network
      - id: schema
        text: You describe tools (name, description, JSON Schema parameters); the model returns a structured request to call one; your code validates and executes it and returns the result
        correct: true
      - text: The model downloads and runs your code
      - text: The provider executes your functions
    explanation: Some providers host built-in tools (web search, code execution), but for your own tools execution is always on your side.
  - id: tool-types
    prompt: Which are common categories of agent tools?
    type: multiple
    options:
      - id: retrieval
        text: Retrieval — search, vector search, SQL queries
        correct: true
      - id: action
        text: Actions — send email, create ticket, update records
        correct: true
      - id: compute
        text: Computation — code execution, calculators
        correct: true
      - id: human
        text: Human interaction — ask for clarification or approval
        correct: true
    explanation: Read-only tools are far less risky than action tools; classify every tool accordingly.
  - id: descriptions
    prompt: Why do tool names and descriptions matter so much?
    options:
      - text: They are shown to end users
      - text: They are only for documentation
      - id: selection
        text: The model chooses tools based on them; vague or overlapping descriptions cause wrong tool choices
        correct: true
      - text: They determine billing
    explanation: Anthropic recommends writing tool descriptions as you would for a new colleague, with examples and edge cases.
  - id: validation
    prompt: The model calls `refund(order_id="A-1042", amount=5000)`. What must the tool layer do?
    options:
      - text: Execute it; the model decided
      - id: validate
        text: Validate arguments and enforce business rules and the user's permissions (e.g. refund cap, order ownership) before executing
        correct: true
      - text: Ask the model if it is sure
      - text: Log it and execute
    explanation: Tool arguments are untrusted input.
  - id: too-many-tools
    prompt: What happens when an agent is given 200 tools?
    options:
      - text: It becomes 200 times more capable
      - text: Nothing changes
      - text: The provider rejects the request
      - id: degrade
        text: Tool-selection accuracy tends to drop, tool definitions consume context and tokens, and behaviour becomes harder to test; group tools, retrieve relevant tools dynamically, or split into specialised agents
        correct: true
    explanation: Fewer, well-designed tools beat many thin wrappers.
  - id: granularity
    prompt: Which tool design is usually better for agents?
    options:
      - id: task-level
        text: Task-level tools (e.g. `schedule_meeting(attendees, duration)`) that encapsulate several API calls and return concise, relevant results
        correct: true
      - text: One tool per REST endpoint, returning raw JSON
      - text: A single `execute_sql(any_query)` tool for everything
      - text: Tools that return entire database tables
    explanation: Tools designed for agents differ from APIs designed for programs — they should be meaningful units with token-efficient outputs.
  - id: errors
    prompt: How should tools report errors to the agent?
    options:
      - text: Crash the agent
      - text: Return an empty string
      - id: informative
        text: Return clear, actionable error messages (e.g. "order_id must look like A-1234; did you mean A-1042?") so the model can correct itself
        correct: true
      - text: Return a stack trace
    explanation: Good errors turn failures into self-correction; stack traces waste tokens and may leak details.
  - id: parallel-calls
    prompt: What are parallel tool calls?
    options:
      - text: Two agents calling each other
      - text: Retrying a failed call
      - id: parallel
        text: The model requesting several independent tool calls in one turn, which the application can execute concurrently to reduce latency
        correct: true
      - text: Calling the same tool twice
    explanation: Only safe when calls are independent; actions with ordering dependencies must run sequentially.
  - id: idempotent
    prompt: Why should action tools be idempotent where possible?
    options:
      - id: retry
        text: Agents and frameworks retry; a repeated `create_invoice` call must not create two invoices — use idempotency keys
        correct: true
      - text: To make them faster
      - text: Models require it
      - text: To reduce token count
    explanation: The same rule applies to any distributed system with retries.
  - id: code-tools
    prompt: What is the main risk of giving an agent a general code-execution or shell tool?
    options:
      - text: It is too slow
      - text: Models cannot write code
      - id: blast-radius
        text: It can do almost anything the execution environment allows, so it must run in a sandbox with restricted network, file system and credentials
        correct: true
      - text: It increases token usage slightly
    explanation: Sandboxing is non-negotiable for code tools.
---
## Learning objectives

- Explain the function-calling protocol
- Design tools that agents use well
- Validate and sandbox tool execution

## A tool definition

```json
{
  "name": "lookup_order",
  "description": "Look up one order by its ID (format A-1234). Returns status, items and total. Use when the user references a specific order.",
  "parameters": {
    "type": "object",
    "properties": { "order_id": { "type": "string", "pattern": "^A-\\d{4}$" } },
    "required": ["order_id"]
  }
}
```

## The execution boundary

```text
LLM ──proposes call──► Tool layer: parse → validate schema → check user's permission
                                 → enforce business rules → execute (sandboxed)
                                 → shape result (concise) ──► back to LLM
```

## Sources

- OpenAI, "Function calling" — <https://platform.openai.com/docs/guides/function-calling>
- Anthropic, "Writing effective tools for agents", 2025 — <https://www.anthropic.com/engineering/writing-tools-for-agents>
- Anthropic, "Tool use" — <https://docs.anthropic.com/en/docs/agents-and-tools/tool-use/overview>
