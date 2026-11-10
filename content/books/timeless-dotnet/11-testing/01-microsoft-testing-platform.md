---
title: Microsoft.Testing.Platform
questions:
  - id: what-mtp
    prompt: What is Microsoft.Testing.Platform (MTP)?
    options:
      - id: lightweight-runner
        text: A lightweight test platform in which each test project is an executable that hosts its own runner, replacing VSTest
        correct: true
      - text: A cloud service that runs your tests
      - text: A mocking library
      - text: A new name for MSTest only
    explanation: MTP is framework-agnostic — MSTest, NUnit, xUnit.net v3 and TUnit support it. A test project can be run with `dotnet run`, `dotnet test`, or by executing the built binary.
  - id: vstest-vs-mtp
    prompt: Which are differences between VSTest and MTP?
    type: multiple
    options:
      - id: no-testhost
        text: MTP does not need a separate `vstest.console` and testhost process orchestrating adapters
        correct: true
      - id: exe
        text: Under MTP, test projects build as executables (`OutputType Exe`)
        correct: true
      - id: extensions
        text: MTP features (TRX reports, coverage, crash and hang dumps, retries) are NuGet extensions you reference explicitly
        correct: true
      - text: MTP only runs on Windows
    explanation: The architecture is simpler, more portable and AOT-capable; the trade-off is that some VSTest-era tools needed updates.
  - id: global-json
    prompt: On the .NET 10 SDK, how do you make `dotnet test` use MTP natively for a repository?
    options:
      - text: Set `<UseVSTest>false</UseVSTest>` in every project
      - text: Install a Visual Studio extension
      - id: test-runner
        text: "Add `\"test\": { \"runner\": \"Microsoft.Testing.Platform\" }` to `global.json`"
        correct: true
      - text: It is impossible; MTP requires .NET 11
    explanation: On the .NET 8/9 SDKs the older opt-in was `<TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>` with options after `--`. On .NET 10+, the `global.json` setting makes options first-class.
  - id: filter
    prompt: "With xUnit v3 on MTP, how do you run only the tests in one class from the command line?"
    options:
      - text: "`dotnet test --filter \"FullyQualifiedName~MyClass\"` only"
      - text: "`dotnet run --class MyClass`"
      - id: filter-class
        text: "`dotnet test --filter-class My.Namespace.MyClass` (xUnit's MTP option); `--filter` with VSTest syntax is also available in xUnit 4"
        correct: true
      - text: Filtering is not supported
    explanation: Options come from the framework and extensions referenced by the project. Run `dotnet test -?` to see what your projects support.
  - id: coverage
    prompt: Which statement about code coverage on MTP is accurate according to the xUnit.net documentation?
    options:
      - text: Coverage is impossible with MTP
      - text: Coverage requires Visual Studio Enterprise
      - id: ms-coverage
        text: The `coverlet.collector` VSTest data collector does not work under MTP; use an MTP coverage extension such as `Microsoft.Testing.Extensions.CodeCoverage`
        correct: true
      - text: Coverlet works unchanged
    explanation: Migrating to MTP means checking every VSTest-era integration (collectors, loggers, CI tasks) — a small, one-time cost.
  - id: why-timeless
    prompt: Why does running tests as ordinary executables age well?
    options:
      - id: fewer-moving-parts
        text: Fewer hidden moving parts — the same binary runs in the IDE, CI, a container or under Native AOT, with behavior controlled by referenced packages you can see
        correct: true
      - text: Executables never need updating
      - text: It removes the need for test frameworks
      - text: It makes tests run in parallel automatically
    explanation: Explicit, inspectable composition beats an external orchestrator discovering adapters by convention.
---
## Learning objectives

- Explain what MTP is and how it differs from VSTest
- Configure a repository for MTP on the .NET 10 SDK
- Find and use command-line options and extensions

## From VSTest to MTP

VSTest (2016) used an orchestrator (`vstest.console`) that discovered *adapters* and launched a
*testhost* process. It worked, but was heavy, Windows-centric in design, and hard to extend.

MTP inverts the model: the test framework and platform are linked *into* your test project,
which becomes an executable. `dotnet test` (with MTP mode on) simply builds and runs those
executables and aggregates their output.

## Repository setup

```json
{
  "sdk": { "version": "10.0.100", "rollForward": "latestFeature" },
  "test": { "runner": "Microsoft.Testing.Platform" }
}
```

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="xunit.v3.mtp-v2" />
    <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" />
    <PackageReference Include="Microsoft.Testing.Extensions.TrxReport" />
  </ItemGroup>
</Project>
```

## Everyday commands

```bash
dotnet test
dotnet test --project tests/Orders.Tests
dotnet test --filter-class Orders.Tests.PricingTests
dotnet test --filter-trait Category=Integration
dotnet test --report-trx --coverage
dotnet run --project tests/Orders.Tests -- --filter-method "*Discount*"
dotnet test -?
```

## Configuration

xUnit.net reads `testconfig.json` (next to the test project) under MTP; `xunit.runner.json` is the
VSTest-era file.

```json
{
  "xUnit": {
    "parallelizeTestCollections": true,
    "maxParallelThreads": "2x",
    "diagnosticMessages": false
  }
}
```

Check the current xUnit documentation for the exact keys supported by your version; xUnit 4
introduced new parallel modes.

## Real-world scenario: CI that silently ran zero tests

A team migrated to a new runner and CI went green — because test discovery found nothing. MTP's
`--minimum-expected-tests` option turns that into a failure:

```bash
dotnet test --minimum-expected-tests 500
```

## Sources

- Microsoft Learn, "Microsoft.Testing.Platform overview" — <https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-intro>
- Microsoft Learn, "Testing with dotnet test" — <https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test>
- xUnit.net, "Microsoft Testing Platform (v3)" — <https://xunit.net/docs/getting-started/v3/microsoft-testing-platform>
- xUnit.net, "Code coverage with MTP" — <https://xunit.net/docs/getting-started/v3/code-coverage-with-mtp>
