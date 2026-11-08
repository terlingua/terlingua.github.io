---
title: What modern .NET is
questions:
  - id: lts
    prompt: What kind of release is .NET 10?
    options:
      - text: A Standard Term Support (STS) release
      - id: lts
        text: A Long Term Support (LTS) release, supported for three years
        correct: true
      - text: A preview release
      - text: A .NET Framework service pack
    explanation: Even-numbered releases are LTS with three years of support. .NET 10 shipped in November 2025 and is supported until November 2028.
  - id: framework
    prompt: How does modern .NET relate to .NET Framework 4.8?
    options:
      - text: .NET 10 is an update that installs on top of .NET Framework 4.8
      - text: They are the same product with different names
      - id: successor
        text: Modern .NET is the cross-platform successor; .NET Framework 4.8.x is Windows-only and receives no new features
        correct: true
    explanation: .NET Core was renamed .NET starting with .NET 5. .NET Framework remains supported as part of Windows but is in maintenance mode.
  - id: cadence
    prompt: Which statements about the .NET release cadence are true?
    type: multiple
    options:
      - text: A new major version ships every November
        correct: true
      - text: Odd-numbered releases are Standard Term Support
        correct: true
      - text: Monthly servicing updates deliver security and bug fixes
        correct: true
      - text: LTS releases are supported for ten years
    explanation: Major versions ship annually in November and receive monthly patches. LTS releases get three years of support; STS releases get 24 months.
  - id: tfm
    prompt: Which target framework moniker targets .NET 10?
    options:
      - text: "`netcoreapp10.0`"
      - text: "`netstandard10.0`"
      - text: "`net10`"
      - id: net10
        text: "`net10.0`"
        correct: true
    explanation: Since .NET 5, the TFM format is `netX.0`, optionally with a platform suffix such as `net10.0-windows`.
---
## Learning objectives

- Explain what modern .NET is and how it differs from .NET Framework
- Understand the LTS/STS release and support model
- Identify the main workloads built on .NET

## One platform

Modern .NET — the line that started as .NET Core and dropped "Core" at version 5 — is an open
source, cross-platform runtime and SDK for Windows, Linux and macOS on x64 and Arm64. One base
class library serves web apps, services, desktop, mobile (.NET MAUI), cloud functions, games and
command-line tools.

.NET Framework 4.8.x still ships with Windows and is supported, but it is Windows-only and receives
no new features. New development should target modern .NET.

## Release cadence

| Version | Released | Type | Support ends |
|---------|----------|------|--------------|
| .NET 8 | Nov 2023 | LTS | Nov 2026 |
| .NET 9 | Nov 2024 | STS | Nov 2026 |
| .NET 10 | Nov 2025 | LTS | Nov 2028 |

- A major version ships every **November**
- **Even** numbers are **LTS** (3 years); **odd** numbers are **STS** (24 months since the policy
  change announced in 2025, previously 18)
- **Patch Tuesday** servicing releases ship monthly

Pick LTS for systems that upgrade rarely; STS is fine for teams that upgrade every year.

## What ships in .NET 10

- **C# 14** — extension members, the `field` keyword, null-conditional assignment and more
- **ASP.NET Core 10** — minimal API validation, OpenAPI 3.1, Blazor improvements
- **EF Core 10** — `LeftJoin`/`RightJoin` operators, named query filters, vector search for SQL
- **SDK** — file-based apps with `dotnet run app.cs`, `dotnet tool exec`, SLNX solution files
- **Runtime** — JIT, GC and AVX10.2 improvements, more Native AOT support

## Target framework monikers

```xml
<TargetFramework>net10.0</TargetFramework>
<!-- or several -->
<TargetFrameworks>net8.0;net10.0</TargetFrameworks>
```

Platform-specific TFMs add a suffix: `net10.0-windows`, `net10.0-android`, `net10.0-ios`.
