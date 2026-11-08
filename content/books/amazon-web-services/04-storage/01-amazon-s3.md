---
title: Amazon S3
questions:
  - id: consistency
    prompt: What consistency does Amazon S3 provide for new object writes and overwrites?
    options:
      - text: Eventual consistency only
      - text: Consistency only within the same Availability Zone
      - text: No consistency guarantees
      - id: strong
        text: Strong read-after-write consistency for PUTs and DELETEs of objects
        correct: true
    explanation: Since December 2020, S3 provides strong read-after-write consistency automatically, at no extra cost.
  - id: presigned
    prompt: How can a browser upload a file directly to a private S3 bucket without receiving AWS credentials?
    options:
      - text: Make the bucket public
      - id: presign
        text: The backend generates a presigned URL that grants time-limited permission for that specific upload
        correct: true
      - text: Send the access keys to the browser
    explanation: The presigned URL is signed with the backend's credentials and expires after a short time.
  - id: defaults
    prompt: Which statements about S3 security defaults for new buckets are true?
    type: multiple
    options:
      - text: Block Public Access is enabled by default
        correct: true
      - text: ACLs are disabled by default (Object Ownership set to bucket owner enforced)
        correct: true
      - text: New objects are encrypted at rest by default (SSE-S3)
        correct: true
      - text: Objects are publicly readable unless you add a deny policy
    explanation: Since 2023, new buckets are private, ACL-free and encrypted by default.
  - id: classes
    prompt: Which S3 storage class automatically moves objects between access tiers based on usage, with no retrieval fees?
    options:
      - text: S3 Standard
      - text: S3 Glacier Deep Archive
      - id: intelligent
        text: S3 Intelligent-Tiering
        correct: true
      - text: S3 One Zone-IA
    explanation: Intelligent-Tiering suits data with unknown or changing access patterns, for a small monitoring fee per object.
---
## Learning objectives

- Understand buckets, objects, durability and consistency
- Secure buckets and share objects safely
- Optimise cost with storage classes and lifecycle rules

## Basics

- **Buckets** have globally unique names and live in one Region
- **Objects** are up to 5 TB; use multipart upload for large objects (single PUT max 5 GB)
- Designed for **99.999999999% (11 nines) durability** across multiple AZs (except One Zone classes)
- **Strong read-after-write consistency**

## Storage classes

| Class | Use |
|-------|-----|
| Standard | Frequently accessed data |
| Intelligent-Tiering | Unknown or changing access patterns |
| Standard-IA / One Zone-IA | Infrequent access (One Zone = single AZ, cheaper, less resilient) |
| Glacier Instant Retrieval | Archive with millisecond access |
| Glacier Flexible Retrieval | Archive, minutes to hours retrieval |
| Glacier Deep Archive | Lowest cost, hours retrieval |
| Express One Zone | Single-digit-millisecond latency for hot data in one AZ |

## Working with S3 from .NET

```csharp
await s3.PutObjectAsync(new PutObjectRequest
{
    BucketName = "acme-uploads",
    Key = $"users/{userId}/avatar.png",
    InputStream = stream,
    ContentType = "image/png"
}, ct);

using var response = await s3.GetObjectAsync("acme-uploads", $"users/{userId}/avatar.png", ct);
await response.ResponseStream.CopyToAsync(destination, ct);
```

## Presigned URLs

```csharp
var url = await s3.GetPreSignedURLAsync(new GetPreSignedUrlRequest
{
    BucketName = "acme-uploads",
    Key = $"incoming/{Guid.CreateVersion7()}.pdf",
    Verb = HttpVerb.PUT,
    Expires = DateTime.UtcNow.AddMinutes(10),
    ContentType = "application/pdf"
});
```

## Protecting data

- **Versioning** — keep every version; protects against overwrites and deletes
- **Object Lock** — write-once-read-many retention for compliance
- **Replication** — same-Region or cross-Region copies
- **Bucket policies** — require TLS (`aws:SecureTransport`), restrict to your organisation or VPC endpoint
- **CloudFront with Origin Access Control** — serve public content without making the bucket public

## Lifecycle rules

```json
{
  "Rules": [{
    "ID": "logs",
    "Status": "Enabled",
    "Filter": { "Prefix": "logs/" },
    "Transitions": [
      { "Days": 30, "StorageClass": "STANDARD_IA" },
      { "Days": 180, "StorageClass": "GLACIER" }
    ],
    "Expiration": { "Days": 2555 }
  }]
}
```
