---
title: Infrastructure as code with CloudFormation and CDK
questions:
  - id: cdk
    prompt: What does the AWS CDK do?
    options:
      - text: Deploys infrastructure without CloudFormation by calling APIs directly
      - id: synth
        text: Lets you define infrastructure in programming languages such as C#, TypeScript or Python, and synthesizes CloudFormation templates that are then deployed
        correct: true
      - text: Monitors costs
      - text: Is a Kubernetes distribution
    explanation: CDK constructs produce CloudFormation, so you get CloudFormation's deployment engine with the expressiveness of a real language.
  - id: change-set
    prompt: What is a CloudFormation change set?
    options:
      - text: A backup of a stack
      - text: A list of IAM users
      - id: preview
        text: A preview of the changes an update would make to a stack, reviewed before execution
        correct: true
    explanation: Change sets show additions, modifications and — importantly — replacements before they happen.
  - id: bootstrap
    prompt: Which statements about CDK are true?
    type: multiple
    options:
      - text: "`cdk bootstrap` prepares an account and Region with resources CDK needs for deployments"
        correct: true
      - text: "`cdk diff` compares your code with the deployed stack"
        correct: true
      - text: L2 constructs provide higher-level defaults, such as granting permissions with methods like `GrantRead`
        correct: true
      - text: CDK apps can only be written in TypeScript
    explanation: CDK supports TypeScript, JavaScript, Python, Java, C# and Go.
---
## Learning objectives

- Deploy infrastructure declaratively with CloudFormation
- Write infrastructure in C# with the CDK
- Preview changes safely

## CloudFormation

Templates (YAML or JSON) describe resources; CloudFormation creates them as a **stack**, ordering
operations by dependencies and rolling back on failure.

```yaml
Resources:
  OrdersTable:
    Type: AWS::DynamoDB::Table
    Properties:
      BillingMode: PAY_PER_REQUEST
      AttributeDefinitions:
        - { AttributeName: PK, AttributeType: S }
        - { AttributeName: SK, AttributeType: S }
      KeySchema:
        - { AttributeName: PK, KeyType: HASH }
        - { AttributeName: SK, KeyType: RANGE }
      PointInTimeRecoverySpecification:
        PointInTimeRecoveryEnabled: true
```

## CDK in C#

```csharp
public sealed class ShopStack : Stack
{
    public ShopStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
    {
        var table = new Table(this, "Orders", new TableProps
        {
            PartitionKey = new Attribute { Name = "PK", Type = AttributeType.STRING },
            SortKey = new Attribute { Name = "SK", Type = AttributeType.STRING },
            BillingMode = BillingMode.PAY_PER_REQUEST,
            RemovalPolicy = RemovalPolicy.RETAIN
        });

        var api = new Function(this, "Api", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Architecture = Architecture.ARM_64,
            Handler = "Shop.Api",
            Code = Code.FromAsset("../src/Shop.Api/bin/Release/publish"),
            MemorySize = 1024,
            Environment = new Dictionary<string, string> { ["TABLE_NAME"] = table.TableName }
        });

        table.GrantReadWriteData(api);
    }
}
```

`GrantReadWriteData` writes a least-privilege IAM policy for you. (Use the newest managed .NET
runtime your CDK version offers.)

```bash
cdk bootstrap aws://111122223333/eu-central-1
cdk diff
cdk deploy
```

## Other tools

- **AWS SAM** — CloudFormation extension focused on serverless, with local testing (`sam local`)
- **Terraform** — popular multi-cloud alternative

Whatever the tool: everything in Git, deployed by pipelines, previewed before applying, and no
manual console changes in production. **Drift detection** finds resources changed outside IaC.
