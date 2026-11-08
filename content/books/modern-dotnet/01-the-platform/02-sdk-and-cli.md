---
title: The SDK and the dotnet CLI
questions:
  - id: global-json
    prompt: What is the purpose of `global.json`?
    options:
      - text: It lists NuGet packages for every project
      - id: pin
        text: It selects which installed .NET SDK version is used in a directory tree
        correct: true
      - text: It stores application settings such as connection strings
      - text: It replaces the project file
    explanation: "`global.json` pins the SDK (with a `rollForward` policy) so builds are reproducible across machines and CI."
  - id: file-based
    prompt: What does `dotnet run hello.cs` do in .NET 10?
    options:
      - text: Fails, because a project file is always required
      - text: Interprets the file without compiling it
      - id: file-app
        text: Builds and runs a single C# file as a file-based app, without a project file
        correct: true
    explanation: File-based apps compile a single `.cs` file behind the scenes. Directives such as `#:package` add dependencies.
  - id: commands
    prompt: Match the intent. Which commands are used correctly?
    type: multiple
    options:
      - text: "`dotnet new webapi -n Orders.Api` creates a new project from a template"
        correct: true
      - text: "`dotnet publish -c Release` produces deployable output"
        correct: true
      - text: "`dotnet add package Dapper` adds a NuGet package reference"
        correct: true
      - text: "`dotnet restore --compile` builds and runs tests"
    explanation: "`restore` only restores packages; there is no `--compile` switch. `build`, `test` and `publish` restore implicitly."
---
## Learning objectives

- Use the core `dotnet` CLI commands
- Pin SDK versions with `global.json`
- Write file-based apps for scripts and prototypes

## Everyday commands

```bash
dotnet new list                         # available templates
dotnet new sln -n Shop                  # .NET 10 creates Shop.slnx by default
dotnet new webapi -n Shop.Api
dotnet sln add Shop.Api
dotnet add Shop.Api package Dapper
dotnet build
dotnet test
dotnet run --project Shop.Api
dotnet publish Shop.Api -c Release -o ./out
```

`build`, `run`, `test` and `publish` perform an implicit `restore`.

## Pinning the SDK

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature"
  }
}
```

`rollForward: latestFeature` accepts a newer feature band or patch of 10.0 but never jumps to 11.

## SLNX solution files

The XML-based `.slnx` format replaces the old `.sln` format with something humans can read and
merge. `dotnet sln migrate` converts an existing solution.

```xml
<Solution>
  <Folder Name="/src/">
    <Project Path="src/Shop.Api/Shop.Api.csproj" />
  </Folder>
  <Folder Name="/tests/">
    <Project Path="tests/Shop.Api.Tests/Shop.Api.Tests.csproj" />
  </Folder>
</Solution>
```

## File-based apps

.NET 10 can run a single file directly:

```csharp
#!/usr/bin/env dotnet
#:package Humanizer@2.14.1

using Humanizer;

var released = new DateTime(2025, 11, 11);
Console.WriteLine($".NET 10 shipped {(DateTime.Now - released).Humanize()} ago.");
```

```bash
dotnet run released.cs
```

Directives such as `#:package`, `#:sdk` and `#:property` replace the project file. When a script
outgrows one file, `dotnet project convert released.cs` turns it into a regular project.

## Tools

```bash
dotnet tool install --global dotnet-ef
dotnet tool exec dotnet-outdated-tool   # run a tool once without installing it (new in .NET 10)
dnx dotnet-outdated-tool                # shorthand for the same thing
```
