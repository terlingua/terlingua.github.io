---
title: Azure Key Vault
questions:
  - id: what-stores
    prompt: What does Azure Key Vault store and manage?
    options:
      - text: Container images
      - id: secrets-keys-certs
        text: Secrets, cryptographic keys and certificates
        correct: true
      - text: Log data
      - text: Virtual machine disks
    explanation: Key Vault centralises sensitive material with access control, auditing and (for keys) HSM protection options.
  - id: config
    prompt: When loading Key Vault secrets into .NET configuration, which secret name maps to the configuration key `ConnectionStrings:Shop`?
    options:
      - text: "`ConnectionStrings:Shop`"
      - text: "`ConnectionStrings__Shop`"
      - id: dashes
        text: "`ConnectionStrings--Shop`"
        correct: true
    explanation: Key Vault secret names cannot contain colons, so the configuration provider translates `--` into the `:` separator.
  - id: protection
    prompt: Which statements about Key Vault data protection are true?
    type: multiple
    options:
      - text: Soft delete is enabled on all vaults, so deleted items can be recovered during the retention period
        correct: true
      - text: Purge protection prevents permanent deletion until the retention period ends
        correct: true
      - text: The Azure RBAC permission model is recommended over legacy access policies
        correct: true
      - text: Secrets should be copied into `appsettings.json` at deployment for speed
    explanation: Keep secrets in the vault; load them at runtime with a managed identity.
---
## Learning objectives

- Store and access secrets securely
- Load Key Vault secrets into .NET configuration
- Prefer identities over secrets where possible

## Creating a vault and granting access

```bash
az keyvault create -n kv-shop-prod -g rg-shop-prod --enable-rbac-authorization true
az role assignment create --assignee <app-principal-id> --role "Key Vault Secrets User" \
  --scope $(az keyvault show -n kv-shop-prod --query id -o tsv)
az keyvault secret set --vault-name kv-shop-prod --name "Payments--ApiKey" --value "<value>"
```

## Configuration provider

```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://kv-shop-prod.vault.azure.net/"),
    credential);

builder.Services.AddOptions<PaymentsOptions>().BindConfiguration("Payments").ValidateOnStart();
```

The `Azure.Extensions.AspNetCore.Configuration.Secrets` package loads secrets at startup (and can
reload them periodically). `Payments--ApiKey` becomes `Payments:ApiKey`.

## Direct access

```csharp
var client = new SecretClient(new Uri("https://kv-shop-prod.vault.azure.net/"), credential);
KeyVaultSecret secret = await client.GetSecretAsync("Payments--ApiKey", cancellationToken: ct);
```

## App Service and Functions references

App settings can reference secrets directly:

```text
@Microsoft.KeyVault(SecretUri=https://kv-shop-prod.vault.azure.net/secrets/Payments--ApiKey/)
```

The platform resolves the value using the app's managed identity, and your code just reads an
ordinary setting.

## Best practices

- Prefer managed identities to secrets; the best secret is the one you don't have
- One vault per application per environment limits blast radius
- Enable purge protection for production vaults
- Send diagnostic logs to Log Analytics and alert on unusual access
- Rotate secrets and use expiry dates; Event Grid can notify when secrets near expiry
