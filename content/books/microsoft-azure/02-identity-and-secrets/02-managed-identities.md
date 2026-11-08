---
title: Managed identities and passwordless connections
questions:
  - id: what
    prompt: What problem do managed identities solve?
    options:
      - text: They speed up virtual machines
      - id: no-secrets
        text: They give Azure resources an Entra identity whose credentials Azure manages, so code can access other services without storing secrets
        correct: true
      - text: They replace the need for RBAC
      - text: They encrypt databases
    explanation: The platform rotates the underlying credentials; your app just requests tokens.
  - id: types
    prompt: What is the difference between system-assigned and user-assigned managed identities?
    options:
      - id: lifecycle
        text: A system-assigned identity is tied to one resource's lifecycle; a user-assigned identity is a standalone resource that can be shared by several resources
        correct: true
      - text: System-assigned identities are for users; user-assigned identities are for systems
      - text: There is no difference
    explanation: User-assigned identities are useful when several apps need the same permissions or when the identity must exist before the app.
  - id: credential
    prompt: Which statements about credentials in the Azure SDK for .NET are true?
    type: multiple
    options:
      - text: "`DefaultAzureCredential` tries a chain of credential sources, such as environment variables, workload identity, managed identity and developer tools like the Azure CLI"
        correct: true
      - text: The same code can authenticate locally with your developer login and in Azure with a managed identity
        correct: true
      - text: In production you can use `ManagedIdentityCredential` directly for more predictable behaviour
        correct: true
      - text: "`DefaultAzureCredential` requires a client secret in `appsettings.json`"
    explanation: The chain avoids secrets entirely. Choosing a specific credential in production avoids surprises from earlier sources in the chain.
---
## Learning objectives

- Enable managed identities and grant them roles
- Use `DefaultAzureCredential` and specific credentials
- Connect to Azure services without secrets

## Enabling a managed identity

```bash
az webapp identity assign --name app-shop-prod --resource-group rg-shop-prod
az role assignment create --assignee <principalId> \
  --role "Storage Blob Data Contributor" \
  --scope $(az storage account show -n stshopprod --query id -o tsv)
```

## Using it from .NET

```csharp
using Azure.Identity;
using Azure.Storage.Blobs;

TokenCredential credential = builder.Environment.IsDevelopment()
    ? new DefaultAzureCredential()
    : new ManagedIdentityCredential(ManagedIdentityId.SystemAssigned);

builder.Services.AddSingleton(new BlobServiceClient(new Uri("https://stshopprod.blob.core.windows.net"), credential));
```

```csharp
public sealed class InvoiceStore(BlobServiceClient blobs)
{
    public async Task UploadAsync(string name, Stream content, CancellationToken ct)
    {
        var container = blobs.GetBlobContainerClient("invoices");
        await container.GetBlobClient(name).UploadAsync(content, overwrite: true, ct);
    }
}
```

Locally, `DefaultAzureCredential` picks up your `az login` or Visual Studio sign-in; in Azure, the
managed identity is used. No connection string with a key appears anywhere.

## The Microsoft.Extensions.Azure helpers

```csharp
builder.Services.AddAzureClients(clients =>
{
    clients.AddBlobServiceClient(new Uri("https://stshopprod.blob.core.windows.net"));
    clients.AddServiceBusClientWithNamespace("sb-shop-prod.servicebus.windows.net");
    clients.UseCredential(credential);
});
```

## Passwordless databases

```text
Server=tcp:sql-shop-prod.database.windows.net;Database=shop;Authentication=Active Directory Default;Encrypt=True
```

With `Microsoft.Data.SqlClient`, `Authentication=Active Directory Default` (or `Active Directory
Managed Identity`) uses Entra tokens. Create a database user for the identity:

```sql
CREATE USER [app-shop-prod] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [app-shop-prod];
ALTER ROLE db_datawriter ADD MEMBER [app-shop-prod];
```

## Workloads outside Azure

For GitHub Actions or Kubernetes, use **workload identity federation**: the external platform's
OIDC token is exchanged for an Entra token, again without storing secrets.
