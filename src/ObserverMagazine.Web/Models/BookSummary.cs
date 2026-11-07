namespace ObserverMagazine.Web.Models;

public sealed record BookSummary
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
    public int ChapterCount { get; init; }
    public int SubchapterCount { get; init; }
    public int QuestionCount { get; init; }
    public int ReadingTimeMinutes { get; init; }
}
