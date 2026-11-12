---
title: Apps, Lakebase and the newer products
questions:
  - id: apps
    prompt: What are Databricks Apps?
    options:
      - text: Mobile apps for monitoring clusters
      - id: hosted-apps
        text: Hosted data and AI applications (for example Streamlit, Dash, Gradio, Flask, Node.js) running on serverless compute inside the workspace, with identity and Unity Catalog integration
        correct: true
      - text: Marketplace listings
      - text: Notebook extensions
    explanation: They remove the need for a separate hosting platform for internal tools. They are not a general-purpose public web host.
  - id: lakebase
    prompt: What is Lakebase?
    options:
      - text: A new Delta Lake version
      - text: A data lake on-premises
      - id: postgres
        text: A managed, serverless PostgreSQL (OLTP) database integrated with the lakehouse, built on technology from the 2025 Neon acquisition, with separated storage and compute and branching
        correct: true
      - text: A vector database only
    explanation: Recaps of Data + AI Summit 2026 reported Lakebase as GA with features such as branching and disaster recovery. It targets apps, agents' state and low-latency serving.
  - id: lakebase-why
    prompt: Why did a lakehouse vendor add an OLTP database?
    options:
      - id: gap
        text: Applications and agents need low-latency transactional reads and writes that Delta tables on object storage cannot provide; previously teams ran a separate database and synced data
        correct: true
      - text: To replace Delta Lake
      - text: Because Postgres is faster for all analytics
      - text: For marketing only
    explanation: Synced tables move data between Delta and Lakebase. Note that this adds another engine to operate and secure.
  - id: branching
    prompt: What is database branching (as in Neon/Lakebase)?
    options:
      - text: Splitting a table into partitions
      - text: Replicating to another region
      - id: copy-on-write
        text: Creating an instant copy-on-write clone of a database for development, testing or an agent's experiment, without copying the data up front
        correct: true
      - text: A Git branch for SQL files
    explanation: Branching makes "test against production-like data" cheap. Governance of sensitive data in branches still matters.
  - id: lakewatch
    prompt: What is Lakewatch?
    options:
      - text: A wristwatch integration
      - text: A monitoring dashboard for clusters
      - text: A backup service
      - id: siem
        text: Databricks' agentic security (SIEM) product built on security data in the lakehouse, reported as GA in 2026
        correct: true
    explanation: It shows Databricks expanding into adjacent markets. Evaluate against dedicated SIEM vendors on detection quality, not architecture alone.
  - id: one
    prompt: Who is Databricks One (and Genie One) aimed at?
    options:
      - id: business
        text: Business users who consume dashboards, Genie and apps without needing the full technical workspace
        correct: true
      - text: Spark committers
      - text: Database administrators only
      - text: Cloud network engineers
    explanation: Simplified consumer experiences are how platforms grow beyond technical users.
  - id: preview
    prompt: Many newer features launch as Beta, Private Preview or Public Preview. What does that imply for production?
    type: multiple
    options:
      - id: sla
        text: Previews may lack SLAs and support commitments
        correct: true
      - id: change
        text: APIs, behaviour and pricing may change before GA
        correct: true
      - id: availability
        text: Availability may be limited by cloud or region
        correct: true
      - text: Preview features are guaranteed never to change
    explanation: Track GA status for anything business-critical; read the release notes.
  - id: sprawl-products
    prompt: Databricks now offers ingestion, ETL, warehouse, BI, ML, GenAI, agents, OLTP, apps, a CDP and a SIEM. What is a fair concern?
    options:
      - text: There is no concern
      - text: These products cannot share data
      - id: breadth
        text: Breadth increases dependence on one vendor and each product's maturity varies; an integrated suite is convenient but not every component is best in class
        correct: true
      - text: It is illegal to bundle products
    explanation: Integration value is real; so is concentration risk. Decide component by component.
---
## Learning objectives

- Recognise Databricks Apps, Lakebase and newer products
- Understand why OLTP and apps joined the platform
- Treat preview features with appropriate caution

## The platform in 2026, at a glance

| Area | Products |
|---|---|
| Ingest and engineer | Lakeflow Connect, Zerobus, Spark Declarative Pipelines, Lakeflow Jobs, Lakeflow Designer |
| Store and govern | Delta/Iceberg, Unity Catalog, metric views, Delta Sharing |
| Analyse | Databricks SQL, AI/BI dashboards, Genie |
| Build AI | MLflow, Model Serving, Vector Search, Agent Bricks |
| Operational | Lakebase, Databricks Apps |
| Security | Lakewatch |

## Sources

- Databricks docs, "Databricks Apps" — <https://docs.databricks.com/aws/en/dev-tools/databricks-apps/>
- Databricks, "Lakebase" — <https://www.databricks.com/product/lakebase>
- Databricks press release, "Databricks agrees to acquire Neon" (May 2025) — <https://www.databricks.com/company/newsroom/press-releases/databricks-agrees-acquire-neon-help-developers-deliver-ai-systems>
- Atlan, "Databricks Data + AI Summit 2026" — <https://atlan.com/know/ai-agent/databricks/databricks-data-ai-summit-2026-announcements/>
- Databricks docs, "Preview releases" — <https://docs.databricks.com/aws/en/release-notes/release-types>
