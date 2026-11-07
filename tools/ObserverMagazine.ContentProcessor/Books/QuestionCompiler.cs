namespace ObserverMagazine.ContentProcessor.Books;

public static class QuestionCompiler
{
    public const string SingleType = "single";
    public const string MultipleType = "multiple";
    private const int MinimumOptions = 2;

    public static IReadOnlyList<QuestionDocument> Compile(
        IReadOnlyList<QuestionDefinition>? definitions,
        string location,
        BuildDiagnostics diagnostics)
    {
        if (definitions is null || definitions.Count == 0)
        {
            return [];
        }

        var questions = new List<QuestionDocument>(definitions.Count);
        var seenIds = new HashSet<string>(StringComparer.Ordinal);

        for (var i = 0; i < definitions.Count; i++)
        {
            var definition = definitions[i];
            var id = string.IsNullOrWhiteSpace(definition.Id) ? $"q{i + 1}" : definition.Id.Trim();
            var questionLocation = $"{location} question '{id}'";

            if (!ContentNaming.IsValidSlug(id))
            {
                diagnostics.Error(questionLocation, "id must contain only lowercase letters, digits and single hyphens");
                continue;
            }

            if (!seenIds.Add(id))
            {
                diagnostics.Error(questionLocation, "duplicate question id");
                continue;
            }

            var question = CompileQuestion(definition, id, questionLocation, diagnostics);
            if (question is not null)
            {
                questions.Add(question);
            }
        }

        return questions;
    }

    private static QuestionDocument? CompileQuestion(
        QuestionDefinition definition,
        string id,
        string location,
        BuildDiagnostics diagnostics)
    {
        var errorCount = diagnostics.Errors.Count;

        if (string.IsNullOrWhiteSpace(definition.Prompt))
        {
            diagnostics.Error(location, "prompt is required");
        }

        var options = CompileOptions(definition.Options ?? [], location, diagnostics);
        var correctCount = options.Count(o => o.Correct);

        if (options.Count < MinimumOptions)
        {
            diagnostics.Error(location, $"at least {MinimumOptions} options are required");
        }

        if (correctCount == 0)
        {
            diagnostics.Error(location, "at least one option must be marked correct");
        }

        var multipleAnswers = ResolveMultipleAnswers(definition.Type, correctCount, location, diagnostics);

        if (diagnostics.Errors.Count > errorCount)
        {
            return null;
        }

        return new QuestionDocument
        {
            Id = id,
            PromptHtml = MarkdownProcessor.ToHtml(definition.Prompt.Trim()),
            MultipleAnswers = multipleAnswers,
            ExplanationHtml = string.IsNullOrWhiteSpace(definition.Explanation)
                ? ""
                : MarkdownProcessor.ToHtml(definition.Explanation.Trim()),
            Options = options
        };
    }

    private static List<OptionDocument> CompileOptions(
        IReadOnlyList<OptionDefinition> definitions,
        string location,
        BuildDiagnostics diagnostics)
    {
        var options = new List<OptionDocument>(definitions.Count);
        var seenIds = new HashSet<string>(StringComparer.Ordinal);

        for (var i = 0; i < definitions.Count; i++)
        {
            var definition = definitions[i];
            var id = string.IsNullOrWhiteSpace(definition.Id) ? DefaultOptionId(i) : definition.Id.Trim();

            if (!ContentNaming.IsValidSlug(id))
            {
                diagnostics.Error(location, $"option id '{id}' must contain only lowercase letters, digits and single hyphens");
                continue;
            }

            if (!seenIds.Add(id))
            {
                diagnostics.Error(location, $"duplicate option id '{id}'");
                continue;
            }

            if (string.IsNullOrWhiteSpace(definition.Text))
            {
                diagnostics.Error(location, $"option '{id}' has no text");
                continue;
            }

            options.Add(new OptionDocument
            {
                Id = id,
                TextHtml = MarkdownProcessor.ToInlineHtml(definition.Text.Trim()),
                Correct = definition.Correct
            });
        }

        return options;
    }

    private static bool ResolveMultipleAnswers(
        string? type,
        int correctCount,
        string location,
        BuildDiagnostics diagnostics)
    {
        var normalized = type?.Trim().ToLowerInvariant();
        switch (normalized)
        {
            case null or "":
                return correctCount > 1;
            case MultipleType:
                return true;
            case SingleType:
                if (correctCount > 1)
                {
                    diagnostics.Error(location, "type is 'single' but more than one option is marked correct");
                }
                return false;
            default:
                diagnostics.Error(location, $"unknown type '{type}' (expected '{SingleType}' or '{MultipleType}')");
                return false;
        }
    }

    public static string DefaultOptionId(int index) =>
        index < 26 ? ((char)('a' + index)).ToString() : $"o{index + 1}";
}
