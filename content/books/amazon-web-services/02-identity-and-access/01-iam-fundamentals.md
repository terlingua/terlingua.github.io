---
title: IAM fundamentals
questions:
  - id: role
    prompt: What is an IAM role?
    options:
      - text: A user with a permanent password
      - id: assumable
        text: An identity with permissions that trusted principals or services can assume to obtain temporary credentials
        correct: true
      - text: A group of users
      - text: A billing category
    explanation: Roles have no long-term credentials. AWS STS issues temporary credentials when the role is assumed.
  - id: trust
    prompt: What does a role's trust policy define?
    options:
      - id: who
        text: Who (which principals or services) is allowed to assume the role
        correct: true
      - text: What the role can do once assumed
      - text: Which Region the role lives in
    explanation: The trust policy controls who can assume; permission policies control what the role can do.
  - id: least
    prompt: Which practices follow IAM best practices?
    type: multiple
    options:
      - text: Grant least privilege, starting narrow and adding permissions as needed
        correct: true
      - text: Use roles and temporary credentials for workloads instead of long-term access keys
        correct: true
      - text: Use IAM Access Analyzer to find unused access and generate policies from activity
        correct: true
      - text: Attach `AdministratorAccess` to every application role to avoid errors
    explanation: Broad permissions turn small bugs into large incidents.
---
## Learning objectives

- Distinguish users, groups, roles and policies
- Understand trust policies and temporary credentials
- Apply least privilege

## Principals

| Principal | Description | Credentials |
|-----------|-------------|-------------|
| Root user | Owner of the account | Password + MFA; avoid |
| IAM user | Long-lived identity | Password and/or access keys; avoid for people (use Identity Center) |
| IAM group | Collection of users | — |
| IAM role | Assumable identity | Temporary credentials from STS |
| Federated user | Signed in through an identity provider | Temporary credentials |

## A role for a Lambda function

Trust policy — who can assume it:

```json
{
  "Version": "2012-10-17",
  "Statement": [{
    "Effect": "Allow",
    "Principal": { "Service": "lambda.amazonaws.com" },
    "Action": "sts:AssumeRole"
  }]
}
```

Permission policy — what it can do:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": ["dynamodb:GetItem", "dynamodb:PutItem", "dynamodb:Query"],
      "Resource": "arn:aws:dynamodb:eu-central-1:111122223333:table/Orders"
    },
    {
      "Effect": "Allow",
      "Action": ["sqs:SendMessage"],
      "Resource": "arn:aws:sqs:eu-central-1:111122223333:order-events"
    }
  ]
}
```

## Policy types

| Type | Attached to | Purpose |
|------|-------------|---------|
| Identity-based | Users, groups, roles | What the principal can do |
| Resource-based | Resources (S3 bucket, SQS queue, KMS key) | Who can access this resource, including other accounts |
| Permissions boundary | Users, roles | Maximum permissions identity policies can grant |
| SCP / RCP | Accounts, OUs | Organisation-wide guardrails |
| Session policy | An assumed-role session | Further restrict a session |

## Least privilege in practice

Start with AWS managed policies for exploration, then replace them with customer managed policies
scoped to specific actions and resource ARNs. IAM Access Analyzer can generate a policy from a role's
actual CloudTrail activity.
