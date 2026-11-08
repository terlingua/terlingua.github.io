---
title: Credentials for applications
questions:
  - id: ec2-creds
    prompt: How should an application running on EC2 obtain AWS credentials?
    options:
      - id: instance-profile
        text: From the instance profile role, delivered through the instance metadata service and picked up automatically by the SDK
        correct: true
      - text: Read access keys from a file baked into the AMI
      - text: Hard-code keys in `appsettings.json`
      - text: Prompt an operator at startup
    explanation: The SDK's default credential chain retrieves and refreshes temporary credentials from the instance's role.
  - id: ecs
    prompt: On ECS or Fargate, which role should the application code use to call AWS services?
    options:
      - id: task-role
        text: The task role
        correct: true
      - text: The task execution role
      - text: The root user
    explanation: The execution role lets the ECS agent pull images and write logs; the task role is what your application's SDK calls use.
  - id: ci
    prompt: Which are good ways to give a GitHub Actions workflow access to AWS?
    type: multiple
    options:
      - text: An IAM OIDC identity provider for GitHub plus a role the workflow assumes
        correct: true
      - text: A trust policy condition restricting the role to a specific repository and branch
        correct: true
      - text: Short-lived credentials obtained at the start of each run
        correct: true
      - text: Long-lived access keys stored as repository secrets for years
    explanation: OIDC federation removes stored secrets entirely.
---
## Learning objectives

- Understand the SDK's default credential chain
- Give workloads roles instead of keys
- Configure the AWS SDK for .NET with dependency injection

## The default credential chain

The AWS SDKs look for credentials in a well-defined order, including:

1. Environment variables (`AWS_ACCESS_KEY_ID`, ... or web identity token settings)
2. Shared config and credentials files (`~/.aws/config`) — profiles, including IAM Identity Center (SSO) profiles
3. Container credentials (ECS/Fargate task role, EKS Pod Identity)
4. Instance profile credentials from the EC2 instance metadata service

Your code stays the same everywhere: locally it uses your SSO profile, in AWS it uses the role.

## AWS SDK for .NET with DI

```csharp
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonDynamoDB>();
```

```json
{ "AWS": { "Region": "eu-central-1", "Profile": "shop-dev" } }
```

The `AWSSDK.Extensions.NETCore.Setup` package provides these extensions. In AWS environments, omit
`Profile` so the role is used.

```csharp
public sealed class InvoiceArchive(IAmazonS3 s3)
{
    public Task SaveAsync(string key, Stream pdf, CancellationToken ct) =>
        s3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = "acme-invoices",
            Key = key,
            InputStream = pdf,
            ContentType = "application/pdf"
        }, ct);
}
```

## Locally

```bash
aws configure sso          # once
aws sso login --profile shop-dev
dotnet run
```

## Outside AWS

- **GitHub Actions / GitLab** — OIDC federation to an IAM role
- **On-premises servers** — IAM Roles Anywhere with X.509 certificates
- **Kubernetes on EKS** — EKS Pod Identity or IAM roles for service accounts (IRSA)

## EC2 metadata security

Require **IMDSv2** (session-token based) on instances; it mitigates SSRF attacks that tried to steal
credentials from the metadata endpoint.
