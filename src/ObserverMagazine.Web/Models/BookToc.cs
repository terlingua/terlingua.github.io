namespace ObserverMagazine.Web.Models;

public sealed record BookToc
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public DateTime Date { get; init; }
    public DateTime? Updated { get; init; }
    public string Author { get; init; } = "";
    public string AuthorName { get; init; } = "";
    public string Summary { get; init; } = "";
    public string[] Tags { get; init; } = [];
    public string? Image { get; init; }
    public ChapterToc[] Chapters { get; init; } = [];

    public int SubchapterCount => Chapters.Sum(c => c.Subchapters.Length);
    public int QuestionCount => Chapters.Sum(c => c.Subchapters.Sum(s => s.QuestionIds.Length));
}

public sealed record ChapterToc
{
    public int Number { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string Summary { get; init; } = "";
    public SubchapterToc[] Subchapters { get; init; } = [];
}

public sealed record SubchapterToc
{
    public int Number { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public int ReadingTimeMinutes { get; init; }
    public string[] QuestionIds { get; init; } = [];
}

public sealed record SubchapterLocation(ChapterToc Chapter, SubchapterToc Subchapter)
{
    public string Label => $"{Chapter.Number}.{Subchapter.Number}";
    public string ProgressKey => BookProgress.SubchapterKey(Chapter.Slug, Subchapter.Slug);
    public string Href(string bookSlug) => $"books/{bookSlug}/{Chapter.Slug}/{Subchapter.Slug}";
}

public sealed record SubchapterNeighbors(SubchapterLocation? Previous, SubchapterLocation? Next);
