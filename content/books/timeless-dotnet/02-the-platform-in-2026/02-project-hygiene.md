---
title: Project hygiene that pays for a decade
questions:
  - id: cpm
    prompt: What does central package management (`Directory.Packages.props` with `ManagePackageVersionsCentrally`) do?
    options:
      - id: one-place
        text: Defines each NuGet package version once for the whole repository; project files reference packages without versions
        correct: true
      - text: Downloads packages from a central Microsoft server only
      - text: Merges all projects into one assembly
      - text: Disables transitive dependencies
    explanation: One file, one version per package. It eliminates version drift between projects and makes upgrades a one-line change.
  - id: build-props
    prompt: Where should settings that apply to every project in a repository — nullable, language version, warnings as errors — live?
    options:
      - text: Copied into every `.csproj`
      - text: In `appsettings.json`
      - id: dbp
        text: In a `Directory.Build.props` file at the repository root
        correct: true
      - text: In the `.sln` file
    explanation: MSBuild imports `Directory.Build.props` automatically for every project beneath it.
  - id: nullable
    prompt: Which are benefits of enabling nullable reference types (`<Nullable>enable</Nullable>`)?
    type: multiple
    options:
      - id: intent
        text: Method signatures state whether `null` is a legitimate value
        correct: true
      - id: compile-time
        text: Many null dereferences become compile-time warnings instead of production `NullReferenceException`s
        correct: true
      - text: The runtime throws if a non-nullable reference is ever null
      - id: errors
        text: With warnings as errors, the guarantees are enforced in CI
        correct: true
    explanation: Nullable annotations are a compile-time feature. They do not add runtime checks — `ArgumentNullException.ThrowIfNull` still has a place at public boundaries.
  - id: slnx
    prompt: What is `.slnx`?
    options:
      - text: A binary solution format for Visual Studio 2010
      - text: A NuGet package type
      - text: A Linux-only build format
      - id: xml-sln
        text: An XML-based solution file format, much simpler to read and merge than `.sln`, supported by the .NET SDK and Visual Studio
        correct: true
    explanation: The classic `.sln` format is full of GUIDs and is painful to merge. `.slnx` is a short XML file; `dotnet sln migrate` converts existing solutions.
  - id: file-based
    prompt: Which statements about file-based apps in .NET 10 are true?
    type: multiple
    options:
      - id: run-cs
        text: "`dotnet run app.cs` runs a single C# file without a project file"
        correct: true
      - id: directives
        text: Directives such as `#:package` and `#:sdk` at the top of the file add packages and SDKs
        correct: true
      - id: convert
        text: "`dotnet project convert` turns the file into a normal project when it grows"
        correct: true
      - text: File-based apps cannot reference NuGet packages
    explanation: File-based apps make C# a good scripting language — ideal for build helpers, data fixes and one-off tools that used to be PowerShell or Python.
  - id: warnings-as-errors
    prompt: Why is `TreatWarningsAsErrors` a long-term investment rather than a nuisance?
    options:
      - id: obsolete-visible
        text: Obsolete API warnings, nullable warnings and analyzer findings cannot accumulate silently, so each upgrade surfaces problems immediately
        correct: true
      - text: It makes builds faster
      - text: It is required by NuGet
      - text: It disables analyzers
    explanation: Warnings that nobody must fix become thousands of warnings that nobody reads. Make them errors from day one, and suppress deliberately, with a reason, where needed.
  - id: deps-budget
    prompt: When adding a third-party package to a long-lived system, which questions matter most?
    type: multiple
    options:
      - id: license
        text: What is the licence, and could it change? (Several popular .NET packages moved to commercial licences in 2025)
        correct: true
      - id: bcl
        text: Does the BCL or ASP.NET Core already do this?
        correct: true
      - id: maintainers
        text: How many maintainers does it have and how is it funded?
        correct: true
      - text: How many GitHub stars does it have this week?
    explanation: Every dependency is a liability with a time dimension. AutoMapper, MediatR and Fluent Assertions all changed licensing models in 2025; teams that depended on them heavily faced unplanned decisions.
---
## Learning objectives

- Set up a repository so upgrades are cheap
- Use central package management, shared build properties and analyzers
- Use file-based apps for scripts

## A repository skeleton that ages well

```text
/
  global.json
  Directory.Build.props
  Directory.Packages.props
  MyCompany.Orders.slnx
  src/
    Orders.Api/
    Orders.Core/
  tests/
    Orders.Tests/
```

### Directory.Build.props

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  </PropertyGroup>
</Project>
```

### Directory.Packages.props

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="Dapper" Version="2.1.89" />
    <PackageVersion Include="Microsoft.Data.SqlClient" Version="6.1.2" />
    <PackageVersion Include="xunit.v3" Version="4.0.2" />
  </ItemGroup>
</Project>
```

Versions shown are illustrative; pin to what you have tested.

### A project file becomes almost empty

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <ItemGroup>
    <PackageReference Include="Dapper" />
    <PackageReference Include="Microsoft.Data.SqlClient" />
  </ItemGroup>
</Project>
```

### The solution file

```xml
<Solution>
  <Folder Name="/src/">
    <Project Path="src/Orders.Api/Orders.Api.csproj" />
    <Project Path="src/Orders.Core/Orders.Core.csproj" />
  </Folder>
  <Folder Name="/tests/">
    <Project Path="tests/Orders.Tests/Orders.Tests.csproj" />
  </Folder>
</Solution>
```

## File-based apps: C# as a scripting language

```csharp
#:package Dapper@2.1.89
#:package Microsoft.Data.SqlClient@6.1.2

using Dapper;
using Microsoft.Data.SqlClient;

var connectionString = Environment.GetEnvironmentVariable("ORDERS_DB")
    ?? throw new InvalidOperationException("ORDERS_DB is not set");

await using var connection = new SqlConnection(connectionString);
var stuck = await connection.QueryAsync<(int Id, string Status)>(
    "select Id, Status from dbo.Orders where Status = @status and UpdatedAt < @cutoff",
    new { status = "Pending", cutoff = DateTime.UtcNow.AddDays(-3) });

foreach (var (id, status) in stuck)
{
    Console.WriteLine($"{id}\t{status}");
}
```

```bash
dotnet run find-stuck-orders.cs
```

Real-world use: the support team's "find stuck orders" script lives in the repository, is
type-checked, uses the same data access library as production, and runs on any machine with the
SDK.

## The dependency budget

Treat third-party packages like hiring: each one has ongoing costs. In 2025 alone, AutoMapper and
MediatR introduced commercial editions, and Fluent Assertions v8 moved to a paid licence for
commercial use. Earlier, Moq's SponsorLink episode (2023) taught the community that a test
dependency can become a privacy and compliance question overnight. None of these projects did
anything wrong by seeking funding — but a codebase that needs fewer of them is more resilient.

## Sources

- Microsoft Learn, "Central Package Management" — <https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management>
- Microsoft Learn, "Customize your build" (`Directory.Build.props`) — <https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory>
- Microsoft Learn, "What's new in the SDK for .NET 10" — <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/sdk>
- Jimmy Bogard, "AutoMapper and MediatR Commercial Editions Launch Today" — <https://www.jimmybogard.com/automapper-and-mediatr-commercial-editions-launch-today/>
- InfoQ, "Fluent Assertions v8 licence change" — <https://www.infoq.com/news/2025/01/fluent-assertions-v8-license>
