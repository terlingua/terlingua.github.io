---
title: Project files and build configuration
questions:
  - id: cpm
    prompt: With central package management enabled, where are NuGet package versions declared?
    options:
      - text: In every project file, on each `PackageReference`
      - id: packages-props
        text: Once, in `Directory.Packages.props`, using `PackageVersion` items
        correct: true
      - text: In `packages.config`
      - text: In `global.json`
    explanation: Projects keep `PackageReference Include="..."` without versions; `Directory.Packages.props` sets them for the whole repository.
  - id: build-props
    prompt: What is `Directory.Build.props` used for?
    options:
      - text: Declaring application configuration loaded at runtime
      - id: shared
        text: Sharing MSBuild properties such as `TargetFramework`, `Nullable` and `TreatWarningsAsErrors` across all projects below it
        correct: true
      - text: Defining Docker build steps
    explanation: MSBuild imports `Directory.Build.props` automatically into every project in that folder and its subfolders.
  - id: sdk-style
    prompt: Which statements about SDK-style project files are true?
    type: multiple
    options:
      - text: Source files in the project folder are included by default
        correct: true
      - text: The `Sdk` attribute (for example `Microsoft.NET.Sdk.Web`) brings in defaults for a workload
        correct: true
      - text: Every `.cs` file must be listed explicitly with `Compile Include`
    explanation: SDK-style projects use globbing, so files are included automatically.
---
## Learning objectives

- Read and write SDK-style project files
- Share settings with `Directory.Build.props`
- Manage package versions centrally

## A minimal project

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

All `.cs` files below the folder are compiled. `ImplicitUsings` adds common `global using`
directives (`System`, `System.Linq`, `System.Threading.Tasks`, and web namespaces for the Web SDK).

## Shared settings

`Directory.Build.props` at the repository root is imported into every project:

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
</Project>
```

## Central package management

```xml
<!-- Directory.Packages.props -->
<Project>
  <ItemGroup>
    <PackageVersion Include="Dapper" Version="2.1.66" />
    <PackageVersion Include="xunit.v3" Version="4.0.2" />
  </ItemGroup>
</Project>
```

```xml
<!-- a project file -->
<ItemGroup>
  <PackageReference Include="Dapper" />
</ItemGroup>
```

One version per package across the repo eliminates "works in project A, breaks in project B"
drift. Use `VersionOverride` on a `PackageReference` only for deliberate exceptions.

## Quality gates in the build

- `TreatWarningsAsErrors` — warnings cannot accumulate
- `Nullable` — null-safety analysis
- `AnalysisLevel` — enable more .NET analyzers
- `EnforceCodeStyleInBuild` — make `.editorconfig` style rules part of the build

> Treat the build as your first reviewer. Every rule it enforces is a comment nobody has to write.
