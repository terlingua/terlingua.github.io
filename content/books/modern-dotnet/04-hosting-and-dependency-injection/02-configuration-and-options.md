---
title: Configuration and the options pattern
questions:
  - id: precedence
    prompt: With the default `WebApplication` configuration, which source wins if the same key is set in all of them?
    options:
      - text: "`appsettings.json`"
      - text: "`appsettings.Production.json`"
      - text: Environment variables
      - id: cli
        text: Command-line arguments
        correct: true
    explanation: Sources added later override earlier ones. The default order ends with environment variables and then command-line arguments.
  - id: env-var
    prompt: Which environment variable sets the configuration key `Smtp:Host` on every platform?
    options:
      - text: "`Smtp.Host`"
      - text: "`SMTP-HOST`"
      - id: double-underscore
        text: "`Smtp__Host`"
        correct: true
    explanation: A double underscore is translated to the `:` hierarchy separator, which works on Linux where colons are not allowed in variable names.
  - id: options-types
    prompt: Which statements about the options interfaces are correct?
    type: multiple
    options:
      - text: "`IOptions<T>` is a singleton that reads values once"
        correct: true
      - text: "`IOptionsSnapshot<T>` is scoped and recomputed per scope (per request)"
        correct: true
      - text: "`IOptionsMonitor<T>` is a singleton that reflects changes and raises change notifications"
        correct: true
      - text: "`IOptions<T>` automatically reloads when `appsettings.json` changes"
    explanation: Use `IOptionsSnapshot` or `IOptionsMonitor` when you need reloadable configuration.
  - id: secrets
    prompt: Where should a database password used in local development be stored?
    options:
      - id: user-secrets
        text: In user secrets (`dotnet user-secrets`), and in a vault such as Azure Key Vault in production
        correct: true
      - text: In `appsettings.json`, committed to Git
      - text: Hard-coded in `Program.cs`
    explanation: User secrets live outside the repository. Production secrets belong in a managed secret store.
---
## Learning objectives

- Understand configuration sources and precedence
- Bind strongly typed options and validate them at startup
- Keep secrets out of source control

## Sources and precedence

`WebApplication.CreateBuilder` loads, in order (later wins):

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User secrets (Development only)
4. Environment variables
5. Command-line arguments

```bash
export ConnectionStrings__Shop="Server=db;Database=shop;..."
dotnet run -- --Smtp:Port=2525
```

## Strongly typed options

```json
{
  "Smtp": { "Host": "smtp.example.com", "Port": 587, "From": "shop@example.com" }
}
```

```csharp
public sealed class SmtpOptions
{
    public const string Section = "Smtp";

    [Required] public required string Host { get; init; }
    [Range(1, 65535)] public int Port { get; init; } = 587;
    [Required, EmailAddress] public required string From { get; init; }
}

builder.Services.AddOptions<SmtpOptions>()
    .BindConfiguration(SmtpOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

`ValidateOnStart` turns a typo in configuration into a startup failure instead of a 3 a.m. bug.

```csharp
public sealed class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _options = options.Value;
}
```

## Choosing an interface

| Interface | Lifetime | Reloads | Use when |
|-----------|----------|---------|----------|
| `IOptions<T>` | Singleton | No | Settings fixed for the app's lifetime |
| `IOptionsSnapshot<T>` | Scoped | Per request | Per-request consistency with reload |
| `IOptionsMonitor<T>` | Singleton | Yes, with `OnChange` | Singletons that must see changes |

## Secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Shop" "Server=localhost;..."
```

In the cloud, use a managed identity and a secret store such as Azure Key Vault or AWS Secrets
Manager — ideally with no secret at all, using identity-based authentication to the database.
