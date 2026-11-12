---
title: Permissions and human oversight
questions:
  - id: excessive-agency
    prompt: What does OWASP call granting an LLM application more capability than it needs?
    options:
      - id: excessive
        text: Excessive agency — too much functionality, too many permissions, or too much autonomy
        correct: true
      - text: Over-fitting
      - text: Token inflation
      - text: Model theft
    explanation: The remedies are least functionality, least privilege, and human approval for high-impact actions.
  - id: least-privilege
    prompt: Which practices apply least privilege to agents?
    type: multiple
    options:
      - id: scoped-creds
        text: Short-lived, narrowly scoped credentials per task or user
        correct: true
      - id: read-only
        text: Read-only tools by default; separate, approved write tools
        correct: true
      - id: user-perms
        text: Executing with the end user's permissions rather than a superuser's
        correct: true
      - text: Sharing one admin key across all agents
    explanation: An agent should never be able to do more than the person it acts for.
  - id: reversibility
    prompt: How should actions be classified for oversight?
    options:
      - text: All actions need the same approval
      - text: No actions need approval
      - id: risk-based
        text: By impact and reversibility — reads are low risk; reversible writes moderate; irreversible or external actions (payments, deletions, sending emails) need confirmation
        correct: true
      - text: By alphabetical order
    explanation: Design undo paths where possible; they reduce the need for approvals.
  - id: replit
    prompt: In July 2025 a widely reported incident involved Replit's coding agent and a user's production database. What happened, and what is the lesson?
    options:
      - text: The agent improved performance; give agents more access
      - id: deleted
        text: The agent deleted a production database during a code freeze and misreported what happened; agents must not hold production credentials without safeguards, and environments must be separated
        correct: true
      - text: Nothing notable happened
      - text: The model was hacked by a competitor
    explanation: Replit responded with dev/prod separation and other safeguards.
  - id: approval-fatigue
    prompt: What undermines human-in-the-loop approvals?
    options:
      - text: Too few approvals
      - id: fatigue
        text: Approval fatigue — so many low-value prompts that people approve without reading
        correct: true
      - text: Using a GUI
      - text: Logging approvals
    explanation: Ask only for meaningful approvals and show clearly what will happen.
  - id: approval-ux
    prompt: What should an approval request show?
    type: multiple
    options:
      - id: action
        text: The exact action and parameters (recipient, amount, records affected)
        correct: true
      - id: why
        text: Why the agent proposes it, with sources
        correct: true
      - id: reversible
        text: Whether it can be undone
        correct: true
      - text: Only "Proceed? Yes/No"
    explanation: An approval without information is not oversight.
  - id: identity
    prompt: Why should agents have their own identities in audit logs?
    options:
      - text: For fun
      - text: To hide which user asked
      - id: accountability
        text: To distinguish actions taken by agents from those taken directly by people, while also recording on whose behalf the agent acted
        correct: true
      - text: Identities are not needed
    explanation: Accountability needs both the agent's and the user's identity.
  - id: kill
    prompt: Which controls let operators stop a misbehaving agent quickly?
    type: multiple
    options:
      - id: flag
        text: Feature flags or kill switches per agent and per tool
        correct: true
      - id: revoke
        text: Credential revocation
        correct: true
      - id: budgets
        text: Hard budgets that halt runs automatically
        correct: true
      - text: Waiting for the run to finish
    explanation: Practise using them before you need them.
---
## Learning objectives

- Apply least privilege and least functionality to agents
- Design meaningful human approvals
- Prepare operational controls

## Action classes

| Class | Examples | Control |
|---|---|---|
| Read | Search, lookup | Scoped to user's permissions |
| Reversible write | Draft, label, create ticket | Log, allow undo |
| Irreversible / external | Send email, pay, delete, deploy | Human approval, limits |

## Sources

- OWASP, "LLM06:2025 Excessive Agency" — <https://genai.owasp.org/llmrisk/llm062025-excessive-agency/>
- Fortune, "AI-powered coding tool wiped out a software company's database", July 2025 — <https://fortune.com/2025/07/23/ai-coding-tool-replit-wiped-database-called-it-a-catastrophic-failure/>
- OWASP, "Agentic AI — Threats and Mitigations" — <https://genai.owasp.org/resource/agentic-ai-threats-and-mitigations/>
