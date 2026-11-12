---
title: APIs, schemas and authentication
questions:
  - id: http-methods
    prompt: Which HTTP methods are conventionally safe (read-only)?
    options:
      - id: get
        text: GET and HEAD
        correct: true
      - text: POST and PUT
      - text: DELETE and PATCH
      - text: All methods
    explanation: Classifying agent tools by HTTP semantics is a quick first risk assessment.
  - id: idempotent-methods
    prompt: Which HTTP methods are defined as idempotent?
    type: multiple
    options:
      - id: get
        text: GET
        correct: true
      - id: put
        text: PUT
        correct: true
      - id: delete
        text: DELETE
        correct: true
      - text: POST
    explanation: POST is not idempotent by definition — use idempotency keys for retried creates.
  - id: status-codes
    prompt: A tool receives HTTP 429. What should it do?
    options:
      - text: Retry immediately in a tight loop
      - text: Treat it as success
      - id: backoff
        text: Back off and retry later (respecting Retry-After), or report rate limiting to the agent
        correct: true
      - text: Delete the request
    explanation: 429 means Too Many Requests. 4xx errors other than 408/429 usually should not be retried.
  - id: json-schema
    prompt: What is JSON Schema used for in agent systems?
    options:
      - text: Styling JSON
      - id: describe
        text: Describing the shape of tool parameters and structured outputs so models can produce valid arguments and applications can validate them
        correct: true
      - text: Compressing JSON
      - text: Encrypting JSON
    explanation: Function calling, MCP and structured outputs all use JSON Schema.
  - id: required-enum
    prompt: "In a tool schema, what does `\"enum\": [\"low\", \"medium\", \"high\"]` on a `priority` parameter do?"
    options:
      - text: Sets the default to low
      - text: Makes the parameter optional
      - text: Sorts the values
      - id: restrict
        text: Restricts valid values to those three, guiding the model and enabling validation
        correct: true
    explanation: Tight schemas reduce invalid tool calls.
  - id: oauth
    prompt: What is OAuth 2.x primarily for?
    options:
      - id: delegated
        text: Delegated authorisation — letting an application act on a user's behalf with scoped, revocable tokens without handling the user's password
        correct: true
      - text: Encrypting databases
      - text: Hashing passwords
      - text: Load balancing
    explanation: MCP's authorisation for remote servers builds on OAuth.
  - id: scopes
    prompt: What are OAuth scopes?
    options:
      - text: Telescope settings
      - id: scopes
        text: Named permissions requested by a client, limiting what an access token allows
        correct: true
      - text: Code blocks
      - text: Network ranges
    explanation: Request the narrowest scopes an agent needs.
  - id: api-keys
    prompt: Why are long-lived API keys risky for agents?
    options:
      - text: They expire too quickly
      - text: They are hard to type
      - id: leak
        text: If leaked (logs, prompts, repositories) they grant broad access until manually rotated; short-lived scoped tokens limit the damage
        correct: true
      - text: They cannot be used over HTTPS
    explanation: Keep secrets out of prompts and traces.
  - id: async
    prompt: Why do agent runtimes rely on asynchronous I/O?
    options:
      - text: It makes models smarter
      - text: It is required by JSON
      - text: It reduces token counts
      - id: waiting
        text: Agents spend most time waiting on model and tool calls; async I/O lets one process handle many concurrent runs efficiently and run independent calls in parallel
        correct: true
    explanation: In .NET, `async`/`await` with `CancellationToken`; in Python, `asyncio`.
  - id: state-machine
    prompt: Why model long-running agent tasks as explicit state machines or durable workflows?
    options:
      - id: durable
        text: So tasks can pause for approvals, survive restarts and resume from known states rather than restarting from scratch
        correct: true
      - text: Because LLMs are state machines
      - text: To avoid using a database
      - text: It is only for games
    explanation: Graph-based workflow engines in agent frameworks provide this.
  - id: webhooks
    prompt: What is a webhook?
    options:
      - text: A fishing tool
      - text: A browser plugin
      - id: callback
        text: An HTTP callback — a service calls your endpoint when an event happens, instead of you polling
        correct: true
      - text: A type of SQL join
    explanation: Useful for resuming agents when external events (approvals, payments) complete.
---
## Learning objectives

- Classify tools by HTTP semantics and idempotency
- Write precise JSON Schemas
- Understand OAuth, scopes and secrets handling
- Know why agents are asynchronous and stateful

## A precise tool schema

```json
{
  "type": "object",
  "properties": {
    "title":    { "type": "string", "maxLength": 120 },
    "priority": { "type": "string", "enum": ["low", "medium", "high"] },
    "due":      { "type": "string", "format": "date" }
  },
  "required": ["title", "priority"],
  "additionalProperties": false
}
```

## Sources

- RFC 9110, "HTTP Semantics" — <https://www.rfc-editor.org/rfc/rfc9110>
- JSON Schema — <https://json-schema.org/>
- OAuth 2.1 draft — <https://datatracker.ietf.org/doc/draft-ietf-oauth-v2-1/>
- MCP specification, "Authorization" — <https://modelcontextprotocol.io/specification>
