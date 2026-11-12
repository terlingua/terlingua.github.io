---
title: Responsible AI in practice
questions:
  - id: governance-body
    prompt: What does a practical AI governance process include?
    type: multiple
    options:
      - id: inventory
        text: An inventory of AI systems and their owners
        correct: true
      - id: risk-review
        text: Risk classification and review before launch, proportionate to risk
        correct: true
      - id: monitoring
        text: Monitoring and incident response after launch
        correct: true
      - text: A single approval meeting, after which nothing is checked again
    explanation: Governance is continuous; models, data and usage change.
  - id: hitl
    prompt: What does "human in the loop" mean, and when is it essential?
    options:
      - text: A human writes every answer
      - id: hitl
        text: A person reviews or approves AI outputs before they take effect — essential for high-impact decisions about people, money or safety
        correct: true
      - text: A human watches the GPU
      - text: Users can chat with humans if they want
    explanation: Review must be meaningful — rubber-stamping under time pressure is not oversight.
  - id: red-team
    prompt: What is red-teaming?
    options:
      - text: Painting servers red
      - text: Competing teams writing prompts
      - id: adversarial
        text: Deliberately attacking a system — jailbreaks, injection, harmful requests, bias probes — to find failures before release
        correct: true
      - text: A sales technique
    explanation: Combine manual creativity with automated attack libraries.
  - id: guardrails
    prompt: What are guardrails?
    options:
      - id: guardrails
        text: Input and output checks around the model — topic restrictions, PII detection, toxicity filters, schema validation — that enforce policy outside the model
        correct: true
      - text: Physical barriers in data centres
      - text: The model's training data
      - text: A type of GPU
    explanation: Guardrails reduce risk but have false positives and negatives; measure both.
  - id: model-cards
    prompt: What is a model card or system card?
    options:
      - text: A business card for a model
      - text: A GPU specification
      - id: doc
        text: Documentation of a model or system's intended use, limitations, evaluation results and risks
        correct: true
      - text: A licence key
    explanation: Mitchell et al. (2019) proposed model cards; providers publish system cards for major releases.
  - id: logging
    prompt: Why log prompts and responses (with appropriate privacy controls)?
    type: multiple
    options:
      - id: debug
        text: Debugging and evaluating failures
        correct: true
      - id: audit
        text: Audit and incident investigation
        correct: true
      - id: improve
        text: Building evaluation sets from real traffic
        correct: true
      - text: Selling user data
    explanation: Logs contain personal data; apply retention limits, access controls and redaction.
  - id: disclosure
    prompt: Should customers be told when they are talking to AI?
    options:
      - text: No, it reduces trust
      - text: Only if they ask three times
      - text: Only in the US
      - id: yes-disclose
        text: Yes — it is required in the EU in many cases and is good practice everywhere; deception erodes trust when discovered
        correct: true
    explanation: Disclose clearly and provide a route to a human for consequential issues.
  - id: kill-switch
    prompt: Why should every production GenAI feature have a way to disable it quickly?
    options:
      - text: To save electricity
      - id: incident
        text: Incidents — a jailbreak trend, a harmful output going viral, a provider outage — require fast containment without a full deployment
        correct: true
      - text: It is never needed
      - text: Regulators require daily shutdowns
    explanation: Feature flags and fallbacks are cheap insurance.
---
## Learning objectives

- Set up proportionate AI governance
- Use human review, red-teaming and guardrails effectively
- Document and log responsibly

## A launch checklist

- [ ] Owner, purpose and users documented
- [ ] Risk tier assessed (internal policy and applicable law)
- [ ] Evaluation results meet thresholds, including safety and bias tests
- [ ] Red-team findings addressed
- [ ] Guardrails and permission model in place
- [ ] Logging, retention and privacy reviewed
- [ ] Human review for consequential outputs
- [ ] Disclosure to users
- [ ] Monitoring, incident runbook and kill switch

## Sources

- Mitchell et al., "Model Cards for Model Reporting", 2019 — <https://arxiv.org/abs/1810.03993>
- Microsoft, "Responsible AI Standard v2" — <https://www.microsoft.com/en-us/ai/principles-and-approach>
- NIST AI 600-1 — <https://nvlpubs.nist.gov/nistpubs/ai/NIST.AI.600-1.pdf>
- OWASP GenAI Security Project — <https://genai.owasp.org/>
