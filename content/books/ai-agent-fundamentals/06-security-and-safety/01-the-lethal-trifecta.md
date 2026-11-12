---
title: The lethal trifecta
questions:
  - id: trifecta
    prompt: Which three capabilities form Simon Willison's "lethal trifecta" for AI agents?
    type: multiple
    options:
      - id: private
        text: Access to private data
        correct: true
      - id: untrusted
        text: Exposure to untrusted content
        correct: true
      - id: exfil
        text: The ability to communicate externally (exfiltrate)
        correct: true
      - text: Use of a large model
    explanation: Combine all three and an attacker who controls some content can trick the agent into sending your data to them.
  - id: why-dangerous
    prompt: Why is the trifecta so dangerous?
    options:
      - id: injection
        text: Because prompt injection cannot be reliably prevented, any agent with all three can be induced to exfiltrate private data
        correct: true
      - text: Because models are malicious
      - text: Because it is expensive
      - text: Because it slows the agent down
    explanation: The fix is architectural — remove at least one leg for a given context.
  - id: rule-of-two
    prompt: Meta's "Agents Rule of Two" (2025) recommends that, within a session, an agent should satisfy no more than two of which properties?
    options:
      - text: Speed, cost, quality
      - text: Memory, tools, planning
      - id: three
        text: Processing untrustworthy inputs; accessing sensitive systems or private data; changing state or communicating externally
        correct: true
      - text: Python, SQL, Java
    explanation: If all three are needed, require human approval or other supervision.
  - id: example
    prompt: An email assistant reads your inbox, can read your company drive and can send email. An attacker emails you hidden instructions. What is the risk?
    options:
      - text: None, because emails are text
      - text: The assistant crashes
      - text: The model refuses all emails
      - id: exfiltration
        text: The assistant may follow the hidden instructions to search the drive for secrets and email them to the attacker
        correct: true
    explanation: This is the trifecta in its most common form.
  - id: break-leg
    prompt: Which designs break a leg of the trifecta?
    type: multiple
    options:
      - id: no-external
        text: Blocking outbound communication (no email, no web requests, no rendered external images) in sessions that read untrusted content
        correct: true
      - id: no-private
        text: Processing untrusted content in a context with no access to private data, returning only structured, validated results
        correct: true
      - id: approval
        text: Requiring explicit human approval for any outbound action
        correct: true
      - text: Telling the model to ignore malicious instructions
    explanation: Prompt-level instructions are not a security boundary.
  - id: real-incidents
    prompt: Which kinds of vulnerabilities were publicly reported in commercial AI assistants during 2024–2025?
    options:
      - text: None have been reported
      - id: reported
        text: Data exfiltration via indirect prompt injection — for example through rendered images or links — in several major assistants and coding tools, typically fixed after disclosure
        correct: true
      - text: Only hardware failures
      - text: Only billing bugs
    explanation: Willison and security researchers maintain catalogues of such disclosures.
  - id: mcp-trifecta
    prompt: Why does mixing MCP servers increase trifecta risk?
    options:
      - text: MCP is insecure by design
      - id: combination
        text: Users combine tools from different servers, easily assembling private-data, untrusted-content and outbound tools in one agent without noticing
        correct: true
      - text: MCP servers cannot authenticate
      - text: MCP forbids read-only tools
    explanation: Review the combined tool set, not each server in isolation.
---
## Learning objectives

- Recognise the lethal trifecta and the Rule of Two
- Design agents that remove at least one leg
- Understand why prompt instructions are not a defence

## The trifecta

```text
        Private data
           ▲    ▲
          /      \
 Untrusted ─────── External
  content          communication
   (all three together = exfiltration risk)
```

## Sources

- Simon Willison, "The lethal trifecta for AI agents", June 2025 — <https://simonwillison.net/2025/Jun/16/the-lethal-trifecta/>
- Meta AI, "Agents Rule of Two: A Practical Approach to AI Agent Security", October 2025 — <https://ai.meta.com/blog/practical-ai-agent-security/>
- OWASP, "Top 10 for LLM Applications 2025" — <https://genai.owasp.org/llm-top-10/>
