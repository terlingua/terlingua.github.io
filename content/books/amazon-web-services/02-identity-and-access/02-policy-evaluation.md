---
title: How AWS evaluates permissions
questions:
  - id: default
    prompt: If no policy mentions an action, is the request allowed?
    options:
      - text: Yes, everything is allowed by default
      - text: Only for the root user and only in us-east-1
      - id: implicit-deny
        text: No — requests are implicitly denied unless an applicable policy allows them
        correct: true
    explanation: AWS starts from deny. An explicit Allow is required (the root user is the special exception within its own account).
  - id: explicit-deny
    prompt: An identity policy allows `s3:DeleteObject`, but an SCP explicitly denies it. What is the result?
    options:
      - text: Allowed, because the identity policy is more specific
      - id: denied
        text: Denied, because an explicit deny in any applicable policy overrides every allow
        correct: true
      - text: It depends on which policy was created first
    explanation: Explicit deny always wins.
  - id: cross-account
    prompt: Which statements about cross-account access are true?
    type: multiple
    options:
      - text: Access generally requires permission on both sides — the caller's identity policy and the resource policy or role trust policy
        correct: true
      - text: A common pattern is for the other account to assume a role in your account
        correct: true
      - text: Condition keys such as `aws:PrincipalOrgID` can restrict access to principals in your organisation
        correct: true
      - text: Any AWS account can access your S3 bucket unless you block it
    explanation: S3 buckets are private by default, and Block Public Access is on by default for new buckets.
---
## Learning objectives

- Trace how AWS decides to allow or deny a request
- Use conditions to narrow permissions
- Reason about cross-account access

## The evaluation logic (simplified)

1. Start with **implicit deny**
2. Any applicable **explicit deny** (identity, resource, SCP, RCP, boundary, session) → **deny**
3. Organization guardrails (SCPs/RCPs) must allow the action
4. Permissions boundaries and session policies, if present, must allow it
5. An **identity-based or resource-based policy must allow** it → **allow**
6. Otherwise → **deny**

## Conditions

```json
{
  "Effect": "Allow",
  "Action": "s3:GetObject",
  "Resource": "arn:aws:s3:::acme-invoices/*",
  "Condition": {
    "StringEquals": { "aws:PrincipalTag/department": "finance" },
    "Bool": { "aws:SecureTransport": "true" }
  }
}
```

Common keys: `aws:SourceIp`, `aws:PrincipalOrgID`, `aws:RequestedRegion`, `aws:MultiFactorAuthPresent`,
`aws:SourceArn`, and resource or principal tags for attribute-based access control (ABAC).

## Resource policies and the confused deputy

When allowing a service to act on your resource, scope it to your source:

```json
{
  "Effect": "Allow",
  "Principal": { "Service": "sns.amazonaws.com" },
  "Action": "sqs:SendMessage",
  "Resource": "arn:aws:sqs:eu-central-1:111122223333:orders",
  "Condition": { "ArnEquals": { "aws:SourceArn": "arn:aws:sns:eu-central-1:111122223333:order-events" } }
}
```

Without the condition, any SNS topic could be configured to send to your queue.

## Debugging access

- The error message names the action and often which policy type denied it
- **IAM Policy Simulator** tests policies without making real calls
- **CloudTrail** records the denied API call with the principal and error code
