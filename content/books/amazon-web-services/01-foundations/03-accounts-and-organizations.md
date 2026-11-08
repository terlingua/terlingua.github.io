---
title: Accounts and AWS Organizations
questions:
  - id: root
    prompt: What is the recommended practice for the root user of an AWS account?
    options:
      - text: Use it for daily administration
      - text: Share its password with the whole team
      - id: lock-away
        text: Enable MFA, avoid creating access keys, and use it only for the few tasks that require it
        correct: true
      - text: Delete it
    explanation: The root user has unrestricted access to the account. Daily work should use IAM Identity Center or roles.
  - id: scp
    prompt: What does a Service Control Policy (SCP) do?
    options:
      - text: Grants permissions to users in member accounts
      - id: guardrail
        text: Sets the maximum permissions available to principals in the accounts it applies to, without granting anything itself
        correct: true
      - text: Encrypts S3 buckets
    explanation: An action must be allowed by IAM policies and not blocked by any SCP in the hierarchy.
  - id: multi-account
    prompt: Why do organisations use multiple AWS accounts?
    type: multiple
    options:
      - text: Accounts are strong security and blast-radius boundaries
        correct: true
      - text: Separate accounts per environment simplify cost attribution
        correct: true
      - text: Service quotas apply per account, so workloads don't compete
        correct: true
      - text: AWS charges less per resource in new accounts forever
    explanation: The account is the fundamental isolation boundary in AWS.
---
## Learning objectives

- Structure workloads across multiple accounts
- Apply guardrails with Organizations and SCPs
- Protect the root user

## Why many accounts

An AWS account is a hard boundary for security, billing and quotas. A typical structure:

```text
Management account (billing, Organizations only — no workloads)
├── Security OU      log-archive, security-tooling
├── Infrastructure OU shared networking
├── Workloads OU
│   ├── Prod OU      shop-prod
│   └── NonProd OU   shop-dev, shop-staging
└── Sandbox OU       per-developer experiments
```

**AWS Control Tower** sets up such a landing zone with best-practice guardrails.

## Service Control Policies

```json
{
  "Version": "2012-10-17",
  "Statement": [{
    "Sid": "DenyOutsideApprovedRegions",
    "Effect": "Deny",
    "NotAction": ["iam:*", "organizations:*", "route53:*", "cloudfront:*", "sts:*", "support:*"],
    "Resource": "*",
    "Condition": { "StringNotEquals": { "aws:RequestedRegion": ["eu-central-1", "eu-west-1"] } }
  }]
}
```

SCPs never grant permissions; they cap what IAM policies in member accounts can allow. **Resource
control policies (RCPs)** complement them by capping what can be done *to* resources, such as
denying access to S3 buckets from outside the organisation.

## The root user

- Enable MFA (ideally a hardware key or passkey)
- No access keys
- Use only for tasks that require it (such as changing account settings or closing the account)

## Workforce access

Use **IAM Identity Center** for people: single sign-on from your identity provider into each
account with permission sets, and short-lived credentials for the console and CLI
(`aws sso login`). Avoid long-lived IAM users with access keys.
