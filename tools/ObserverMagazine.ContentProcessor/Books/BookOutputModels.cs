namespace ObserverMagazine.ContentProcessor.Books;

public sealed class BookIndexEntry
{
    public string Slug { get; init; } = "";
    public string Title { get; init; } = "";
    public DateTime Date { get; init; }
    public DateTime? Updated { get; init; }
    public string Author { get; init; } = "";
    public string AuthorName { get; init; } = "";
    public string Summary { get; init; } = "";
    public string[] Tags { get; init; } = [];
    public string? Image { get; init; }
    public int ChapterCount { get; init; }
    public int SubchapterCount { get; init; }
    public int QuestionCount { get; init; }
    public int ReadingTimeMinutes { get; init; }
}

public sealed class BookToc
{
    public string Slug { get; init; } = "";
    public string Title { get; init; } = "";
    public DateTime Date { get; init; }
    public DateTime? Updated { get; init; }
    public string Author { get; init; } = "";
    public string AuthorName { get; init; } = "";
    public string Summary { get; init; } = "";
    public string[] Tags { get; init; } = [];
    public string? Image { get; init; }
    public IReadOnlyList<ChapterToc> Chapters { get; init; } = [];
}

public sealed class ChapterToc
{
    public int Number { get; init; }
    public string Slug { get; init; } = "";
    public string Title { get; init; } = "";
    public string Summary { get; init; } = "";
    public IReadOnlyList<SubchapterToc> Subchapters { get; init; } = [];
}

public sealed class SubchapterToc
{
    public int Number { get; init; }
    public string Slug { get; init; } = "";
    public string Title { get; init; } = "";
    public int ReadingTimeMinutes { get; init; }
    public string[] QuestionIds { get; init; } = [];
}

public sealed class SubchapterDocument
{
    public string BookSlug { get; init; } = "";
    public string ChapterSlug { get; init; } = "";
    public string Slug { get; init; } = "";
    public int ChapterNumber { get; init; }
    public int Number { get; init; }
    public string Title { get; init; } = "";
    public int ReadingTimeMinutes { get; init; }
    public string Html { get; init; } = "";
    public IReadOnlyList<QuestionDocument> Questions { get; init; } = [];
}

public sealed class QuestionDocument
{
    public string Id { get; init; } = "";
    public string PromptHtml { get; init; } = "";
    public bool MultipleAnswers { get; init; }
    public string ExplanationHtml { get; init; } = "";
    public IReadOnlyList<OptionDocument> Options { get; init; } = [];
}

public sealed class OptionDocument
{
    public string Id { get; init; } = "";
    public string TextHtml { get; init; } = "";
    public bool Correct { get; init; }
}

public sealed record CompiledBook(
    BookIndexEntry Index,
    BookToc Toc,
    IReadOnlyList<SubchapterDocument> Documents);
