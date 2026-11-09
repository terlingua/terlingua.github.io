---
title: "Data Systems, Law, and Society"
questions:
  - id: erasure-conflict
    prompt: "Why does the GDPR's right to erasure create an engineering challenge for many data systems?"
    options:
      - id: off1c81
        text: "Erasure requires quantum-safe encryption"
      - id: o0518e0
        text: "Deletion is impossible in SQL"
      - id: o357d6a
        text: "They rely on immutable constructs such as append-only logs, and data may be copied into derived datasets such as ML training data"
        correct: true
      - id: o5c669a
        text: "GDPR forbids databases from deleting rows"
    explanation: "“many data systems rely on immutable constructs such as append-only logs as part of their design. How can we ensure deletion of some data in the middle of a file that is supposed to be immutable? How do we handle deletion of data that has been incorporated into derived datasets”."
  - id: gdpr-tech
    prompt: "Why doesn't the GDPR name compliant technologies?"
    options:
      - id: oeba9a3
        text: "Because it only applies to paper records"
      - id: o018763
        text: "It deliberately sets high-level principles because technologies change quickly"
        correct: true
      - id: oc20b66
        text: "Because no compliant technology exists"
      - id: o7c67ca
        text: "Because each country must choose its own databases"
    explanation: "“The regulation deliberately does not mandate particular technologies, because these may quickly change as technology progresses. Instead, the legal texts set out high-level principles that are subject to interpretation.”"
  - id: true-cost
    prompt: "Beyond the storage bill, which costs should the decision to store data consider?"
    type: multiple
    options:
      - id: ob31505
        text: "Safety risks to users if the data reveals criminalized behaviour"
        correct: true
      - id: o78b394
        text: "The color of the dashboard"
      - id: o411960
        text: "Legal costs and fines for non-compliance"
        correct: true
      - id: o9c5245
        text: "Liability and reputational damage if it is leaked"
        correct: true
    explanation: "“The cost-benefit calculation should also take into account the risks of liability and reputational damage if the data were to be leaked ... and the risk of legal costs and fines”. The chapter also notes that storing data that could reveal criminalized behaviours “creates real safety risks for users.”"
  - id: minimization
    prompt: "What principle does *Datensparsamkeit* name, and what philosophy does it oppose?"
    options:
      - id: o372a15
        text: "Data sovereignty; it opposes cloud storage"
      - id: od3d6e7
        text: "Data lakes; it opposes warehouses"
      - id: o0efae8
        text: "Data maximization; it opposes deleting data"
      - id: ob8eca0
        text: "Data minimization; it opposes storing lots of data speculatively in case it is useful later"
        correct: true
    explanation: "“This principle of data minimization (sometimes known by the German term Datensparsamkeit) runs counter to the “big data” philosophy of storing lots of data speculatively in case it turns out to be useful in the future”."
  - id: purpose-limitation
    prompt: "A product team wants to reuse location data collected for delivery tracking to train an ad-targeting model. Which GDPR principle mentioned in the chapter does this conflict with?"
    options:
      - id: ob5bf97
        text: "Data must be shared with regulators"
      - id: o87375a
        text: "Personal data may be collected only for a specified, explicit purpose and not used for other purposes"
        correct: true
      - id: o5b6858
        text: "Data must be encrypted at rest"
      - id: o37e034
        text: "Data must be stored in the EU"
    explanation: "GDPR “mandates that personal data may be collected only for a specified, explicit purpose; cannot later be used for any other purpose; and must not be kept for longer than necessary for the purposes for which it was collected”."
  - id: ip-location
    prompt: "Why might even a log of IP addresses be sensitive?"
    options:
      - id: oe0b60c
        text: "IP logs are always public"
      - id: o7d9734
        text: "IP addresses indicate approximate location, which over time can reveal visits to places such as clinics"
        correct: true
      - id: o277c6a
        text: "IP addresses identify the device's manufacturer"
      - id: o9c6f4a
        text: "IP addresses contain passwords"
    explanation: "“Travel to an abortion clinic, for example, could easily be revealed by location data, or perhaps even by a log of the user’s IP addresses over time (which indicate approximate location).”"
  - id: industry-standards
    prompt: "Which industry standards does the chapter mention as examples of businesses responding to privacy and safety concerns?"
    type: multiple
    options:
      - id: o8c1899
        text: "SOC 2 for software vendors"
        correct: true
      - id: o9eda02
        text: "ISO 9001 for manufacturing quality"
      - id: o203e43
        text: "HIPAA for retail"
      - id: o7e8aeb
        text: "PCI for payment processing"
        correct: true
    explanation: "The chapter mentions “Payment Card Industry (PCI) standards” and “Service Organization Control (SOC) Type 2 standards”, both verified by third-party audits."
  - id: shared-responsibility
    prompt: "Who does the chapter say shares responsibility for the ethical impact of automated decision systems?"
    options:
      - id: o626285
        text: "Everyone who works on such systems"
        correct: true
      - id: o43ae94
        text: "Only the product manager"
      - id: of9bf48
        text: "Only lawyers"
      - id: ode7025
        text: "Only regulators"
    explanation: "“Everyone who works on such systems shares a responsibility for considering the ethical impact of their decisions and ensuring that they comply with relevant laws.”"
  - id: summary-distributed
    prompt: "The chapter summary gives advice about going distributed. What is it?"
    options:
      - id: o6e1f9d
        text: "Always start distributed to avoid later migrations"
      - id: o8b8e1e
        text: "Distribute only analytical systems"
      - id: o3a6036
        text: "Distribution is required for cloud deployment"
      - id: o348f5e
        text: "Do not rush into making a system distributed if it can stay on a single machine"
        correct: true
    explanation: "“In some situations you can’t avoid going distributed, but it’s advisable not to rush into making a system distributed if it’s possible to keep it on a single machine.”"
---
Architecture is shaped by law and by responsibility to the people whose data you hold.

## Learning objectives

- Explain how the GDPR's right to erasure collides with immutable designs
- Apply data minimization when deciding what to store
- Weigh the true costs of storing personal data

## In a nutshell

- Engineers share responsibility for the ethical and legal impact of systems.
- Append-only logs and derived datasets (including ML training data) make deletion hard.
- Regulations set principles, not technologies, so compliance is a matter of interpretation.
- Some data is not worth storing once liability, safety and compliance risks are counted.

## Connections to today

- **EU AI Act.** Obligations for high-risk AI systems now apply in stages, and they reach into training-data
  governance — derived data in the book's terms.
- **Machine unlearning.** Removing a person's data from a trained model is an open research problem, a
  concrete case of the book's question about deleting data incorporated into derived datasets.
- **Crypto-shredding.** Encrypting each person's data with a per-user key and deleting the key is a common
  way to "erase" data from immutable logs and backups.
