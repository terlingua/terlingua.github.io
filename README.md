# Terlingua Magazine

A free, open-source Blazor WebAssembly application deployed to GitHub Pages.

Built with **.NET 10** (LTS) and **C# 14**.

## Features

- **Blog** — Write posts in Markdown with YAML front matter; compiled to static JSON at build time
- **RSS Feed** — Auto-generated `feed.xml`
- **Responsive Tables** — Sortable, filterable data tables that work on mobile
- **Master-Detail Flow** — Click a list item to view full details
- **OpenTelemetry-ready Logging** — Structured logging via `ILogger`
- **Full Test Suite** — bUnit component tests + xUnit.net v3 integration tests, running on Microsoft.Testing.Platform

## Quick Start
```bash
# Restore & process blog content
dotnet run --project tools/ObserverMagazine.ContentProcessor

# Run the app locally
dotnet run --project src/ObserverMagazine.Web

# Run all tests
dotnet test
```

## Testing

Tests run on **Microsoft.Testing.Platform (MTP)**, not VSTest. The opt-in lives in
`global.json`:

```json
{ "test": { "runner": "Microsoft.Testing.Platform" } }
```

Test projects are ordinary executables, so each one can also be run on its own:

```bash
# Whole solution
dotnet test --solution ObserverMagazine.slnx

# One project
dotnet test --project tests/ObserverMagazine.Web.Tests

# Filter by class, or by trait
dotnet test --filter-class ObserverMagazine.Web.Tests.Services.BlogServiceTests
dotnet test --filter-trait Category=Integration

# Run a test project directly, no dotnet test involved
dotnet run --project tests/ObserverMagazine.Integration.Tests
```

Run `dotnet test -?` from the repo root for the full option list, which is built
from whichever MTP extensions the test projects reference.

xUnit configuration for MTP lives in `testconfig.json` next to a test project
(not `xunit.runner.json`, which is the VSTest-era file).

## Technology

| Component | Technology | License |
|-----------|-----------|---------|
| Framework | .NET 10 / Blazor WASM 10.0.12 | MIT |
| Markdown | Markdig 1.3.2 | BSD-2-Clause |
| YAML | YamlDotNet 18.1.0 | MIT |
| Test framework | xUnit.net v3 4.0.1 | Apache-2.0 |
| Component testing | bUnit 2.10.3 | MIT |
| Test runner | Microsoft.Testing.Platform v2 | MIT |

All dependencies are free for any use — no payment required, ever.

## License

AGPLv3
