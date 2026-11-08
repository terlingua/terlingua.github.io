---
title: Security, cost and production best practices
questions:
  - id: cost-lever
    prompt: Which change usually saves the most money for a team running nightly jobs on an always-on all-purpose cluster?
    options:
      - text: Switching notebooks from Python to SQL
      - text: Disabling Photon everywhere
      - text: Writing data as CSV instead of Delta
      - id: jobs-compute
        text: Running the jobs on jobs compute or serverless jobs, and auto-terminating interactive clusters
        correct: true
    explanation: Idle and mis-sized all-purpose compute is the most common source of waste.
  - id: identity
    prompt: Which identity should a production job run as?
    options:
      - id: sp
        text: A service principal with only the privileges the job needs
        correct: true
      - text: The personal account of the engineer who wrote it
      - text: A workspace admin account
    explanation: Service principals survive staff changes and make least privilege enforceable.
  - id: practices
    prompt: Which are recommended production practices?
    type: multiple
    options:
      - text: Tag compute and jobs for cost attribution, and set budgets with alerts
        correct: true
      - text: Use Unity Catalog managed tables with predictive optimization
        correct: true
      - text: Deploy jobs and pipelines from Git with Asset Bundles and CI/CD
        correct: true
      - text: Grant ALL PRIVILEGES on every catalog to all users to avoid access tickets
    explanation: Least privilege, infrastructure as code and cost visibility are the foundations of a well-run platform.
---
## Learning objectives

- Secure a Databricks deployment
- Control and attribute costs
- Apply a production readiness checklist

## Security

| Area | Practice |
|------|----------|
| Identity | SSO and SCIM provisioning; groups for all grants; service principals for automation |
| Data access | Unity Catalog everywhere; least privilege; row filters and column masks for sensitive data |
| Secrets | Secret scopes or UC connections; never credentials in code |
| Network | Private connectivity (PrivateLink/Private Link), IP access lists, serverless network policies |
| Audit | `system.access.audit` monitored and retained |
| Compute | Standard access mode for shared compute; compute policies |

## Cost

- **Right compute** — serverless or jobs compute for production; auto-terminate interactive compute
- **Right size** — use the query profile and Spark UI before adding nodes
- **Photon** where workloads benefit; spot instances for fault-tolerant classic jobs
- **Data layout** — liquid clustering and predictive optimization reduce bytes scanned
- **Incremental processing** — Auto Loader, streaming tables and incremental materialized views
  instead of full recomputes
- **Visibility** — tags, `system.billing.usage`, budgets and alerts

```sql
SELECT custom_tags['team'] AS team, SUM(usage_quantity) AS dbus
FROM system.billing.usage
WHERE usage_date >= current_date() - INTERVAL 30 DAYS
GROUP BY ALL
ORDER BY dbus DESC;
```

## Production readiness checklist

- [ ] Code in Git; jobs and pipelines deployed by Asset Bundles from CI
- [ ] Separate dev/staging/prod targets and catalogs
- [ ] Jobs run as service principals with least privilege
- [ ] Data quality expectations on silver tables; alerts on violations
- [ ] Retries, timeouts and failure notifications configured
- [ ] Tables are UC managed, documented with comments and owned by groups
- [ ] Costs tagged and reviewed monthly
- [ ] Runbooks for reprocessing from bronze

> The platform evolves quickly. Check the release notes regularly — many manual tasks from a year
> ago (file compaction, cluster tuning, schema management) are now automated.
