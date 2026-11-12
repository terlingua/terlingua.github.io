---
title: The control plane and the compute plane
questions:
  - id: control-plane
    prompt: What runs in the Databricks control plane?
    options:
      - text: Your Spark executors processing data
      - text: Your Parquet data files
      - text: Your cloud VPC's NAT gateway
      - id: backend
        text: Backend services managed by Databricks in its own cloud account — the web application, notebooks' metadata, job scheduler, cluster manager and REST APIs
        correct: true
    explanation: The control plane coordinates; it is where you log in and where job definitions live. Data processing happens in the compute plane.
  - id: classic-compute
    prompt: In the classic compute plane, where do cluster VMs run?
    options:
      - id: customer-account
        text: In the customer's own cloud account and virtual network
        correct: true
      - text: In Databricks' cloud account
      - text: On the user's laptop
      - text: In the control plane
    explanation: Classic clusters are VMs Databricks launches in your subscription/account. You pay the cloud provider for the VMs and Databricks for DBUs.
  - id: serverless-compute
    prompt: Where does serverless compute run?
    options:
      - text: In the customer's virtual network
      - text: On-premises
      - id: databricks-account
        text: In a serverless compute plane inside Databricks' cloud account, in the same region as the workspace, isolated per workspace
        correct: true
      - text: In the user's browser
    explanation: Serverless removes VM management and start-up time. The trade-off is that compute runs in Databricks' account, which matters to some security teams and changes networking (network connectivity configurations, private endpoints).
  - id: two-bills
    prompt: With classic compute, how many bills does a running cluster generate?
    options:
      - text: One, from Databricks, covering everything
      - id: two
        text: Two — DBUs from Databricks and VM, disk and network charges from the cloud provider
        correct: true
      - text: None, if it is a job cluster
      - text: Three, including a Spark licence
    explanation: Serverless pricing bundles the infrastructure into the DBU rate, which is why serverless DBU rates look higher. Compare total cost, not DBU price.
  - id: data-path
    prompt: When a classic cluster reads a Delta table, which path does the data take?
    options:
      - text: Storage → control plane → cluster
      - text: Storage → user's browser → cluster
      - id: direct
        text: Directly from cloud storage to the cluster in the compute plane; the control plane does not proxy table data
        correct: true
      - text: Control plane → storage → cluster
    explanation: Notebook results displayed in the UI do travel to the control plane (and can be stored there), which is a detail security reviews should know about.
  - id: results-storage
    prompt: Which data can leave the compute plane during normal use?
    type: multiple
    options:
      - id: notebook-results
        text: Notebook command results shown in the UI
        correct: true
      - id: logs
        text: Cluster logs and metadata sent to the control plane
        correct: true
      - id: query-results
        text: Query results returned to SQL clients and dashboards
        correct: true
      - text: Nothing ever leaves the compute plane
    explanation: Admins can configure where notebook results are stored and can restrict downloading results. Read the security docs before assuming nothing leaves your account.
  - id: secure-cluster-connectivity
    prompt: What is secure cluster connectivity (also called "no public IP")?
    options:
      - id: scc
        text: Cluster nodes have no public IP addresses and open no inbound ports; they initiate an outbound connection to the control plane relay
        correct: true
      - text: All traffic is sent over email
      - text: Clusters cannot reach the internet at all
      - text: A VPN client every user installs
    explanation: It is now the default. Private Link can further keep control-plane traffic off the public internet.
  - id: private-link
    prompt: What does Private Link (or Private Service Connect on GCP) add?
    options:
      - text: Faster Spark shuffles
      - text: Free egress
      - text: Encryption at rest
      - id: private
        text: Private network connectivity between your network and the Databricks control plane (front end for users, back end for clusters), so traffic does not traverse the public internet
        correct: true
    explanation: Typically required in regulated industries. It adds network complexity and requires premium/enterprise features.
  - id: shared-responsibility
    prompt: Under the shared responsibility model, which are the customer's responsibilities?
    type: multiple
    options:
      - id: access
        text: Configuring identity, access control and least privilege
        correct: true
      - id: data-classification
        text: Classifying data and choosing what to store
        correct: true
      - id: network-config
        text: Network configuration of classic compute and storage in their own account
        correct: true
      - text: Patching the Databricks control-plane servers
    explanation: Databricks secures the platform; customers secure how they use it. Most incidents in cloud platforms stem from misconfiguration on the customer side.
  - id: encryption
    prompt: What are customer-managed keys (CMK) used for?
    options:
      - text: Logging into the workspace
      - id: cmk
        text: Encrypting managed services data (notebooks, secrets) and/or workspace storage with keys you control in your cloud key service, so you can revoke access
        correct: true
      - text: Signing Git commits
      - text: Generating DBUs
    explanation: Data is encrypted at rest by default; CMK gives you control of the keys, at the cost of operational responsibility for them.
---
## Learning objectives

- Distinguish the control plane from the classic and serverless compute planes
- Trace where data and metadata travel
- Understand the networking and security features at a high level

## The two planes

```text
┌────────────────────── Databricks account (control plane) ─────────────────────┐
│ Web app · Notebooks metadata · Jobs scheduler · Cluster manager · REST APIs   │
│ Unity Catalog metastore service                                               │
└──────────────┬──────────────────────────────────────────────┬─────────────────┘
               │ secure relay                                  │
┌──────────────▼─────────────────────┐         ┌───────────────▼───────────────────┐
│ Your cloud account (classic)       │         │ Databricks account (serverless)   │
│ Cluster VMs in your VNet/VPC       │         │ Serverless compute, per region    │
└──────────────┬─────────────────────┘         └───────────────┬───────────────────┘
               │                                               │
               └────────────► Your cloud object storage ◄──────┘
```

## Classic versus serverless

| | Classic | Serverless |
|---|---|---|
| Where VMs run | Your account | Databricks' account |
| Start-up | Minutes | Seconds |
| You manage | Instance types, autoscaling, runtime, networking | Little — mostly budgets and policies |
| Billing | DBUs + cloud infrastructure | DBUs (infrastructure included) |
| Network controls | Your VNet/VPC, NSGs, firewalls | Network connectivity configs, private endpoints, egress controls |

Databricks has been steering new features toward serverless. Some capabilities are serverless-only;
check before standardising on classic compute.

## Sources

- Databricks docs, "High-level architecture" — <https://docs.databricks.com/aws/en/getting-started/overview>
- Databricks docs, "Serverless compute plane networking" — <https://docs.databricks.com/aws/en/security/network/serverless-network-security/>
- Databricks docs, "Secure cluster connectivity" — <https://docs.databricks.com/aws/en/security/network/classic/secure-cluster-connectivity>
- Databricks Security and Trust Center — <https://www.databricks.com/trust>
