---
title: Regulation and frameworks
questions:
  - id: eu-approach
    prompt: What is the EU AI Act's basic approach?
    options:
      - text: It bans all AI
      - id: risk-based
        text: Risk-based — prohibited practices, obligations for high-risk systems, transparency duties for some systems, and specific obligations for general-purpose AI models
        correct: true
      - text: It only regulates robots
      - text: It applies only to EU companies
    explanation: It applies to providers and deployers placing systems on the EU market or affecting people in the EU, regardless of where they are based.
  - id: timeline
    prompt: Which EU AI Act milestones are correct?
    type: multiple
    options:
      - id: force
        text: Entered into force on 1 August 2024
        correct: true
      - id: prohibitions
        text: Prohibited practices applied from 2 February 2025
        correct: true
      - id: gpai
        text: General-purpose AI model obligations applied from 2 August 2025
        correct: true
      - text: All obligations applied on the day of publication
    explanation: A Code of Practice for GPAI providers was published in 2025.
  - id: omnibus
    prompt: What did the 2026 "Digital Omnibus" agreement do to high-risk obligations?
    options:
      - text: Repealed the AI Act
      - text: Moved all deadlines earlier
      - id: delay
        text: EU legislators provisionally agreed to delay high-risk obligations — to December 2027 for stand-alone (Annex III) systems and August 2028 for AI in regulated products — pending formal adoption
        correct: true
      - text: Nothing
    explanation: Check the final adopted text; timelines and details were still being finalised in 2026.
  - id: prohibited
    prompt: Which are examples of prohibited practices under the EU AI Act?
    type: multiple
    options:
      - id: social-scoring
        text: Social scoring by public or private actors leading to unjustified detrimental treatment
        correct: true
      - id: manipulation
        text: Manipulative techniques that materially distort behaviour causing significant harm
        correct: true
      - id: emotion-work
        text: Emotion recognition in workplaces and schools (with limited exceptions)
        correct: true
      - text: Using a chatbot to summarise emails
    explanation: Fines for prohibited practices can reach €35 million or 7% of worldwide annual turnover.
  - id: transparency
    prompt: Which transparency duty applies to chatbots under the EU AI Act?
    options:
      - text: None
      - text: They must publish their weights
      - text: They must be free of charge
      - id: disclose
        text: People must be informed they are interacting with an AI system unless it is obvious; certain synthetic content must be marked as AI-generated
        correct: true
    explanation: Deepfake labelling and machine-readable marking of synthetic content are related duties.
  - id: nist
    prompt: What are the four functions of the NIST AI Risk Management Framework?
    options:
      - id: gmmm
        text: Govern, Map, Measure, Manage
        correct: true
      - text: Plan, Do, Check, Act
      - text: Identify, Protect, Detect, Respond
      - text: Build, Test, Deploy, Monitor
    explanation: NIST AI RMF 1.0 (January 2023) is voluntary; NIST AI 600-1 (2024) adds a Generative AI profile.
  - id: iso42001
    prompt: What is ISO/IEC 42001?
    options:
      - text: A Wi-Fi standard
      - id: aims
        text: An international standard for AI management systems — organisational processes for responsible AI, certifiable like ISO 27001
        correct: true
      - text: A file format for models
      - text: A benchmark
    explanation: Published December 2023; certification is increasingly requested in procurement.
  - id: us
    prompt: Which describes the US federal approach to AI regulation through 2025–2026?
    options:
      - text: A single comprehensive AI law like the EU's
      - text: A complete ban
      - id: patchwork
        text: No comprehensive federal AI statute — executive orders that changed between administrations, sector regulators, and a growing patchwork of state laws (e.g. Colorado, California)
        correct: true
      - text: No rules of any kind
    explanation: Existing laws (consumer protection, discrimination, privacy) apply to AI regardless.
  - id: existing-laws
    prompt: Which existing laws already constrain GenAI use?
    type: multiple
    options:
      - id: gdpr
        text: GDPR and other privacy laws (lawful basis, data subject rights, automated decision-making)
        correct: true
      - id: consumer
        text: Consumer protection laws on deceptive claims
        correct: true
      - id: discrimination
        text: Anti-discrimination laws in hiring, lending and housing
        correct: true
      - text: None, until AI-specific laws pass
    explanation: Regulators have said there is no AI exemption from existing law.
---
## Learning objectives

- Summarise the EU AI Act's structure and timeline
- Know the NIST AI RMF and ISO/IEC 42001
- Recognise that existing laws already apply

## EU AI Act risk tiers

| Tier | Examples | Obligations |
|---|---|---|
| Unacceptable | Social scoring, manipulative AI | Prohibited |
| High risk | Hiring, credit scoring, critical infrastructure | Risk management, data governance, logging, human oversight, conformity assessment |
| Limited (transparency) | Chatbots, synthetic media | Disclosure, marking |
| Minimal | Spam filters, games | None specific |
| GPAI models | Foundation models | Documentation, copyright policy; more for systemic-risk models |

This is not legal advice — consult counsel for your situation.

## Sources

- Regulation (EU) 2024/1689 (AI Act) — <https://eur-lex.europa.eu/eli/reg/2024/1689/oj>
- Sidley, "EU Lawmakers Reach Provisional Agreement to Delay Key EU AI Act Obligations", June 2026 — <https://datamatters.sidley.com/2026/06/22/eu-lawmakers-reach-provisional-agreement-to-delay-key-eu-ai-act-obligations/>
- NIST AI Risk Management Framework — <https://www.nist.gov/itl/ai-risk-management-framework>
- ISO/IEC 42001:2023 — <https://www.iso.org/standard/81230.html>
