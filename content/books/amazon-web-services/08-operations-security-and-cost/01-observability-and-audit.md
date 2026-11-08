---
title: Observability and audit
questions:
  - id: cloudtrail
    prompt: Which service records API calls made in your AWS account, such as who deleted an S3 bucket and when?
    options:
      - text: CloudWatch Metrics
      - id: cloudtrail
        text: AWS CloudTrail
        correct: true
      - text: VPC Flow Logs
      - text: AWS X-Ray
    explanation: CloudTrail records management events by default (90 days of event history); create a trail to keep logs long term in S3.
  - id: alarm
    prompt: What does a CloudWatch alarm do?
    options:
      - text: Stores application logs
      - text: Records API calls
      - id: watch
        text: Watches a metric (or metric math expression) and changes state when it crosses a threshold, triggering actions such as SNS notifications or Auto Scaling
        correct: true
    explanation: Alarms turn metrics into notifications and automation.
  - id: services
    prompt: Which pairings of service and purpose are correct?
    type: multiple
    options:
      - text: CloudWatch Logs Insights — query log data interactively
        correct: true
      - text: AWS X-Ray / CloudWatch Application Signals — distributed tracing and service health
        correct: true
      - text: AWS Config — record resource configuration changes and evaluate compliance rules
        correct: true
      - text: CloudTrail — real-time CPU metrics for EC2
    explanation: CPU metrics come from CloudWatch, not CloudTrail.
---
## Learning objectives

- Collect metrics, logs and traces with CloudWatch and OpenTelemetry
- Audit activity with CloudTrail and AWS Config
- Alert on meaningful signals

## CloudWatch

| Feature | Purpose |
|---------|---------|
| Metrics | Built-in service metrics plus custom metrics |
| Logs + Logs Insights | Centralised logs with a query language |
| Alarms | Thresholds and anomaly detection that trigger actions |
| Dashboards | Visual overviews |
| Application Signals | Service-level health, SLOs and traces based on OpenTelemetry |

```text
fields @timestamp, @message
| filter level = "Error"
| stats count() by bin(5m)
```

## Instrumenting .NET with OpenTelemetry

```csharp
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("shop-api"))
    .WithTracing(t => t
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddAWSInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(m => m.AddAspNetCoreInstrumentation().AddOtlpExporter());
```

The **AWS Distro for OpenTelemetry (ADOT)** collector or the CloudWatch agent receives OTLP and
forwards traces to X-Ray/CloudWatch and metrics to CloudWatch. Write logs as structured JSON to
stdout; container and Lambda platforms ship them to CloudWatch Logs.

## Audit

- **CloudTrail** — every API call: who, what, when, from where. Create an organisation trail into
  a locked-down log-archive account
- **AWS Config** — configuration history and rules such as "S3 buckets must block public access"
- **VPC Flow Logs** — network traffic metadata

## Alarms that matter

Alarm on customer-facing symptoms: ALB 5xx rate, p99 latency, SQS age of oldest message, Lambda
errors and throttles, DynamoDB throttled requests. Route alarms to SNS → e-mail, chat or
incident-management tools.
