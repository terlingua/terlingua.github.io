namespace ObserverMagazine.ContentProcessor.Books;

public sealed class BookManifest
{
    public string Title { get; init; } = "";
    public string? Author { get; init; }
    public string? Summary { get; init; }
    public DateTime Date { get; init; }
    public DateTime? Updated { get; init; }
    public string[]? Tags { get; init; }
    public string? Image { get; init; }
    public bool Draft { get; init; }
}

public sealed class ChapterManifest
{
    public string Title { get; init; } = "";
    public string? Summary { get; init; }
    public bool Draft { get; init; }
}

public sealed class SubchapterFrontMatter
{
    public string Title { get; init; } = "";
    public bool Draft { get; init; }
    public List<QuestionDefinition>? Questions { get; init; }
}

public sealed class QuestionDefinition
{
    public string? Id { get; init; }
    public string Prompt { get; init; } = "";
    public string? Type { get; init; }
    public string? Explanation { get; init; }
    public List<OptionDefinition>? Options { get; init; }
}

public sealed class OptionDefinition
{
    public string? Id { get; init; }
    public string Text { get; init; } = "";
    public bool Correct { get; init; }
}
