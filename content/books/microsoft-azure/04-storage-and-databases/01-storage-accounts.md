---
title: Storage accounts and Blob Storage
questions:
  - id: tiers
    prompt: Which Blob Storage access tier is offline and requires rehydration before the data can be read?
    options:
      - id: archive
        text: Archive
        correct: true
      - text: Hot
      - text: Cool
      - text: Cold
    explanation: Archive is the cheapest for storage but data must be rehydrated (which can take hours) before reading.
  - id: redundancy
    prompt: Which redundancy option keeps three copies across availability zones in the primary region and asynchronously replicates to a secondary region?
    options:
      - text: LRS
      - text: ZRS
      - id: gzrs
        text: GZRS
        correct: true
      - text: GRS
    explanation: GZRS combines zone-redundant storage in the primary region with geo-replication. GRS uses LRS in the primary region.
  - id: access
    prompt: Which are recommended ways to secure access to Blob Storage?
    type: multiple
    options:
      - text: Entra ID authentication with data roles such as Storage Blob Data Reader
        correct: true
      - text: User delegation SAS tokens (signed with Entra credentials) for time-limited client access
        correct: true
      - text: Disabling shared key authorization where possible
        correct: true
      - text: Embedding the account key in a mobile app
    explanation: Account keys grant full access and are hard to rotate safely; prefer identities and short-lived user delegation SAS.
---
## Learning objectives

- Understand storage account services, tiers and redundancy
- Read and write blobs from .NET securely
- Optimise cost with lifecycle management

## Services in a storage account

| Service | Use |
|---------|-----|
| Blob Storage | Objects: documents, images, backups, data lake files (with hierarchical namespace = ADLS Gen2) |
| Azure Files | SMB/NFS file shares |
| Queue Storage | Simple message queues |
| Table Storage | Simple key/attribute NoSQL store |

## Access tiers

| Tier | Storage cost | Access cost | Minimum retention |
|------|--------------|-------------|-------------------|
| Hot | Highest | Lowest | — |
| Cool | Lower | Higher | 30 days |
| Cold | Lower still | Higher still | 90 days |
| Archive | Lowest | Highest; offline | 180 days |

## Redundancy

| Option | Copies |
|--------|--------|
| LRS | 3 copies in one datacenter |
| ZRS | 3 copies across zones in one region |
| GRS / RA-GRS | LRS + async copy to the paired region (RA = read access to the secondary) |
| GZRS / RA-GZRS | ZRS + async copy to the paired region |

## Blobs from .NET

```csharp
var container = blobs.GetBlobContainerClient("uploads");
await container.CreateIfNotExistsAsync(cancellationToken: ct);

var blob = container.GetBlobClient($"{userId}/{Guid.CreateVersion7()}.png");
await blob.UploadAsync(stream, new BlobUploadOptions
{
    HttpHeaders = new BlobHttpHeaders { ContentType = "image/png" }
}, ct);
```

## Letting clients upload directly

```csharp
var delegationKey = await blobs.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(15), ct);

var sas = new BlobSasBuilder(BlobSasPermissions.Create | BlobSasPermissions.Write, DateTimeOffset.UtcNow.AddMinutes(15))
{
    BlobContainerName = "uploads",
    BlobName = blobName
};

var uri = new BlobUriBuilder(blob.Uri)
{
    Sas = sas.ToSasQueryParameters(delegationKey, blobs.AccountName)
}.ToUri();
```

The browser uploads straight to storage with this short-lived URL; your API never handles the bytes.

## Lifecycle management

Policies move blobs to cooler tiers or delete them by age — for example, logs to Cool after 30
days, Archive after 180, delete after 7 years.
