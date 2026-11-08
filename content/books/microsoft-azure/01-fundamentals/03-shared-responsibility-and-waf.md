---
title: Shared responsibility and the Well-Architected Framework
questions:
  - id: paas
    prompt: In a PaaS service such as Azure App Service, who is responsible for patching the operating system?
    options:
      - text: The customer
      - text: Nobody; PaaS operating systems are never patched
      - id: microsoft
        text: Microsoft
        correct: true
    explanation: With PaaS, Microsoft manages the OS and runtime platform; you remain responsible for your code, data, identities and configuration.
  - id: always-customer
    prompt: Regardless of service model (IaaS, PaaS, SaaS), which responsibilities always remain with the customer?
    type: multiple
    options:
      - text: Data
        correct: true
      - text: Accounts and identities
        correct: true
      - text: Devices used to access the service
        correct: true
      - text: Physical datacenter security
    explanation: Physical security is always Microsoft's responsibility. Data, identities and endpoints are always yours.
  - id: pillars
    prompt: Which is NOT one of the five pillars of the Azure Well-Architected Framework?
    options:
      - text: Reliability
      - id: marketing
        text: Marketing Agility
        correct: true
      - text: Cost Optimization
      - text: Operational Excellence
    explanation: The pillars are Reliability, Security, Cost Optimization, Operational Excellence and Performance Efficiency.
---
## Learning objectives

- Apply the shared responsibility model to IaaS, PaaS and SaaS
- Use the Well-Architected Framework to evaluate designs

## Shared responsibility

| Responsibility | On-premises | IaaS | PaaS | SaaS |
|----------------|:-----------:|:----:|:----:|:----:|
| Data, identities, devices | You | You | You | You |
| Application code | You | You | You | Microsoft |
| Runtime, middleware | You | You | Microsoft | Microsoft |
| Operating system | You | You | Microsoft | Microsoft |
| Network controls | You | Shared | Shared | Microsoft |
| Physical hosts, datacenter | You | Microsoft | Microsoft | Microsoft |

The more managed the service, the more Microsoft handles — one reason PaaS is usually the right
default for application developers.

## The Well-Architected Framework

| Pillar | Key questions |
|--------|---------------|
| **Reliability** | Can it recover from failures? Are RTO/RPO met? |
| **Security** | Is access least-privilege? Is data protected in transit and at rest? |
| **Cost Optimization** | Are we paying only for what delivers value? |
| **Operational Excellence** | Is deployment automated? Is it observable? |
| **Performance Efficiency** | Does it scale to meet demand efficiently? |

Use the free **Azure Well-Architected Review** assessment and **Azure Advisor** recommendations to
find gaps in existing workloads.

## Trade-offs

Pillars pull against each other: multi-region deployment improves reliability but increases cost;
aggressive caching improves performance but complicates consistency. Good architecture makes these
trade-offs explicitly, guided by business requirements.
