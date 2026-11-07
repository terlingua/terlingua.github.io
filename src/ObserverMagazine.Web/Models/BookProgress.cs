namespace ObserverMagazine.Web.Models;

public sealed class BookProgress
{
    public const int CurrentSchemaVersion = 1;

    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
    public string BookSlug { get; set; } = "";
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? LastChapterSlug { get; set; }
    public string? LastSubchapterSlug { get; set; }
    public Dictionary<string, SubchapterProgress> Subchapters { get; set; } = new(StringComparer.Ordinal);

    public bool IsStarted => StartedAt is not null;

    public static string SubchapterKey(string chapterSlug, string subchapterSlug) => $"{chapterSlug}/{subchapterSlug}";
}

public sealed class SubchapterProgress
{
    public DateTimeOffset? FirstVisitedAt { get; set; }
    public DateTimeOffset? LastVisitedAt { get; set; }
    public Dictionary<string, QuestionProgress> Questions { get; set; } = new(StringComparer.Ordinal);
}

public sealed class QuestionProgress
{
    public int Attempts { get; set; }
    public bool AnsweredCorrectly { get; set; }
    public bool LastAttemptCorrect { get; set; }
    public string[] LastSelection { get; set; } = [];
    public DateTimeOffset? LastAttemptAt { get; set; }
}

public enum SubchapterStatus
{
    NotStarted,
    InProgress,
    Completed
}

public sealed record BookCompletion(int CompletedSubchapters, int TotalSubchapters, int CorrectQuestions, int TotalQuestions)
{
    public static readonly BookCompletion Empty = new(0, 0, 0, 0);

    public int Percent => TotalSubchapters == 0 ? 0 : CompletedSubchapters * 100 / TotalSubchapters;
    public bool IsComplete => TotalSubchapters > 0 && CompletedSubchapters == TotalSubchapters;
}

public sealed record VisitOutcome(BookProgress Progress, bool IsFirstVisitToBook, bool IsFirstVisitToSubchapter);
