---
title: Formatting and consistency
questions:
  - id: editorconfig
    prompt: Which file lets a .NET team enforce formatting and code-style rules in the editor and during builds?
    options:
      - text: "`launchSettings.json`"
      - id: editorconfig
        text: "`.editorconfig`"
        correct: true
      - text: "`appsettings.json`"
      - text: "`nuget.config`"
    explanation: "`.editorconfig` holds formatting and analyzer settings that IDEs and `dotnet format` honour."
  - id: dotnet-format
    prompt: What does `dotnet format --verify-no-changes` do in a CI pipeline?
    options:
      - id: fail
        text: Fails if any file does not match the configured style, without changing files
        correct: true
      - text: Reformats all files and commits them
      - text: Deletes unused files
    explanation: It is a gate that keeps style consistent without manual review comments.
  - id: vertical
    prompt: Which formatting guidelines improve readability?
    type: multiple
    options:
      - text: Related code is kept vertically close together
        correct: true
      - text: Blank lines separate distinct concepts
        correct: true
      - text: A caller appears above the functions it calls, so the file reads top-down
        correct: true
      - text: Each developer uses their own preferred brace style
    explanation: Consistent, top-down layout lets readers skim a file like a newspaper article. Personal styles fragment a codebase.
---
## Learning objectives

- Use layout to communicate structure
- Automate formatting so humans do not argue about it

## The newspaper metaphor

A source file should read like a newspaper article: the headline (class name) first, then the
high-level summary (public methods), then the details (private helpers) in the order they are used.

```csharp
public sealed class InvoiceGenerator(ITaxCalculator tax)
{
    public Invoice Generate(Order order)
    {
        var lines = BuildLines(order);
        return new Invoice(order.Id, lines, Total(lines));
    }

    private IReadOnlyList<InvoiceLine> BuildLines(Order order) =>
        order.Lines.Select(l => new InvoiceLine(l.Sku, l.Quantity, l.Price, tax.For(l))).ToList();

    private static decimal Total(IEnumerable<InvoiceLine> lines) =>
        lines.Sum(l => l.Quantity * l.Price + l.Tax);
}
```

## Automate it

Formatting debates are a waste of review time. Commit an `.editorconfig`:

```ini
root = true

[*.cs]
indent_style = space
indent_size = 4
csharp_style_namespace_declarations = file_scoped:warning
csharp_prefer_braces = true:warning
dotnet_sort_system_directives_first = true
```

Then enforce it in CI:

```bash
dotnet format --verify-no-changes
```

Combine this with `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` and
`<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>` to make the build itself the style
checker.

## Consistency beats preference

The specific style matters much less than everyone using the same one. A codebase that looks like
one person wrote it is easier to read than one where every file has its own dialect.
