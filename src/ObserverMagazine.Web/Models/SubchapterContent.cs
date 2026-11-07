namespace ObserverMagazine.Web.Models;

public sealed record SubchapterContent
{
    public required string BookSlug { get; init; }
    public required string ChapterSlug { get; init; }
    public required string Slug { get; init; }
    public int ChapterNumber { get; init; }
    public int Number { get; init; }
    public required string Title { get; init; }
    public int ReadingTimeMinutes { get; init; }
    public string Html { get; init; } = "";
    public QuizQuestion[] Questions { get; init; } = [];
}

public sealed record QuizQuestion
{
    public required string Id { get; init; }
    public string PromptHtml { get; init; } = "";
    public bool MultipleAnswers { get; init; }
    public string ExplanationHtml { get; init; } = "";
    public QuizOption[] Options { get; init; } = [];
}

public sealed record QuizOption
{
    public required string Id { get; init; }
    public string TextHtml { get; init; } = "";
    public bool Correct { get; init; }
}

public sealed record QuizResult(bool IsCorrect, IReadOnlySet<string> Selected, IReadOnlySet<string> Correct)
{
    public bool IsWronglySelected(string optionId) => Selected.Contains(optionId) && !Correct.Contains(optionId);
}

public sealed record QuizAnswer(QuizQuestion Question, IReadOnlyCollection<string> SelectedOptionIds, QuizResult Result);
