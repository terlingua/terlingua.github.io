---
title: Releases, support and the yearly rhythm
questions:
  - id: cadence
    prompt: How often does a new major version of .NET ship?
    options:
      - text: Every six months
      - id: yearly
        text: Once a year, in November
        correct: true
      - text: Every three years
      - text: Irregularly, when Windows ships
    explanation: Since .NET 5 (2020) Microsoft ships a major version every November, usually announced at .NET Conf.
  - id: lts-even
    prompt: Which statement about LTS and STS releases is correct?
    options:
      - id: even-lts
        text: Even-numbered releases (.NET 8, 10, 12…) are Long Term Support; odd-numbered ones are Standard Term Support
        correct: true
      - text: Odd-numbered releases are LTS
      - text: Every release is LTS
      - text: Only releases with a Windows version are LTS
    explanation: .NET 10 (November 2025) is LTS with three years of support. .NET 11 (November 2026) is STS.
  - id: sts-length
    prompt: How long are STS releases supported after Microsoft's 2025 policy change?
    options:
      - text: 6 months
      - id: two-years
        text: 24 months
        correct: true
      - text: 18 months
      - text: 5 years
    explanation: Microsoft extended STS support from 18 to 24 months (starting with .NET 9). LTS remains 36 months. As a result an STS and the preceding LTS can reach end of support at about the same time.
  - id: lts-choice
    prompt: A regulated enterprise upgrades its line-of-business systems only every two to three years. Which target is usually the right default in late 2026?
    options:
      - text: .NET Framework 4.8.1, because it never changes
      - text: .NET 11 (STS) only, because newer is always better
      - id: net10
        text: .NET 10 (LTS), planning the next hop to .NET 12 (LTS)
        correct: true
      - text: .NET 6, because it is proven
    explanation: .NET 6 is out of support; running it is a security liability. STS releases are fully production-quality, but organizations that upgrade slowly benefit from LTS windows.
  - id: patch-tuesday
    prompt: What does "supported" mean in practice for a .NET version?
    type: multiple
    options:
      - id: security
        text: It receives security and reliability patches, typically monthly
        correct: true
      - id: must-patch
        text: You must stay on the latest patch (for example 10.0.x) to be supported
        correct: true
      - text: Microsoft will add new language features to it
      - text: It guarantees that any NuGet package will keep supporting it
    explanation: Servicing releases fix bugs and vulnerabilities; features go into the next major version. Being on 10.0.0 when 10.0.14 is out is not "supported" in any useful sense.
  - id: framework-vs-modern
    prompt: Which statements about .NET Framework 4.8.x in 2026 are accurate?
    type: multiple
    options:
      - id: still-supported
        text: It is still supported as a component of Windows and receives security fixes
        correct: true
      - id: no-new
        text: It receives no new features, language versions or runtime improvements
        correct: true
      - text: It supports C# 14 officially
      - text: It is the recommended target for new server applications
    explanation: New work belongs on modern .NET. Framework applications can be migrated incrementally, and many libraries still multi-target `net472`/`net48` for those users.
---
## Learning objectives

- Explain the yearly release cadence and LTS/STS support windows
- Choose a target framework deliberately
- Keep a codebase on a supported runtime without heroics

## The rhythm

| Version | Released | Type | Support ends (approx.) |
|---|---|---|---|
| .NET 8 | Nov 2023 | LTS | Nov 2026 |
| .NET 9 | Nov 2024 | STS | Nov 2026 |
| .NET 10 | Nov 2025 | LTS | Nov 2028 |
| .NET 11 | Nov 2026 | STS | Nov 2028 |
| .NET 12 | Nov 2027 (expected) | LTS (by pattern) | Nov 2030 (by pattern) |

Confirm dates against the official support policy page; the table is a teaching aid, not a contract.

## The timeless lesson: upgrade is a routine, not a project

Teams that treat each upgrade as a project fall behind and end up on unsupported runtimes. Teams
that treat it as a routine — a tracked chore every November — spend a day or two per year.

What makes upgrades cheap:

- A **`global.json`** that pins the SDK, so builds are reproducible
- **Central package management**, so one file controls versions
- **Warnings as errors** and analyzers, so obsolete APIs are visible immediately
- **Good tests**, so you can upgrade with confidence
- **Few dependencies**, especially few reflection-heavy ones

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature"
  },
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

`rollForward: latestFeature` lets machines with a newer 10.0.x feature band build, while still
preventing an accidental jump to an 11.0 SDK.

## Target framework monikers

```xml
<TargetFramework>net10.0</TargetFramework>
```

Libraries that serve a wide audience may multi-target:

```xml
<TargetFrameworks>net10.0;net8.0;netstandard2.0</TargetFrameworks>
```

Applications should target exactly one framework. Multi-targeting an application multiplies test
and deployment work for no benefit.

## Real-world scenario: the forgotten .NET 6 service

A payments reconciliation service was "done" in 2022 and nobody touched it. In 2025 a container
scan flagged unpatched vulnerabilities; .NET 6 had been out of support since November 2024. The
upgrade to .NET 8 took three weeks because of a stale ORM and an abandoned logging package. The
upgrade from 8 to 10 the following year took one afternoon — because they had fixed the habits,
not just the version.

## Sources

- .NET and .NET Core official support policy — <https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core>
- Microsoft Learn, "What's new in .NET 10" — <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview>
- Microsoft Learn, "global.json overview" — <https://learn.microsoft.com/en-us/dotnet/core/tools/global-json>
- .NET Blog, "Get Ready for .NET Conf 2026" — <https://devblogs.microsoft.com/dotnet/get-ready-for-dotnet-conf-2026/>
