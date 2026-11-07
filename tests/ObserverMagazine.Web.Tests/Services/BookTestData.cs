using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Tests.Services;

internal static class BookTestData
{
    public static BookToc Toc() => new()
    {
        Slug = "sample-book",
        Title = "Sample Book",
        Chapters =
        [
            new ChapterToc
            {
                Number = 1,
                Slug = "intro",
                Title = "Intro",
                Subchapters =
                [
                    new SubchapterToc { Number = 1, Slug = "hello", Title = "Hello", QuestionIds = ["q1", "q2"] },
                    new SubchapterToc { Number = 2, Slug = "reading", Title = "Reading only" }
                ]
            },
            new ChapterToc
            {
                Number = 2,
                Slug = "deeper",
                Title = "Deeper",
                Subchapters =
                [
                    new SubchapterToc { Number = 1, Slug = "details", Title = "Details", QuestionIds = ["q1"] }
                ]
            }
        ]
    };

    public static QuizQuestion SingleChoice() => new()
    {
        Id = "single",
        PromptHtml = "<p>Pick one</p>",
        MultipleAnswers = false,
        ExplanationHtml = "<p>Because B.</p>",
        Options =
        [
            new QuizOption { Id = "a", TextHtml = "Alpha" },
            new QuizOption { Id = "b", TextHtml = "Bravo", Correct = true },
            new QuizOption { Id = "c", TextHtml = "Charlie" }
        ]
    };

    public static QuizQuestion MultipleChoice() => new()
    {
        Id = "multi",
        PromptHtml = "<p>Pick all</p>",
        MultipleAnswers = true,
        Options =
        [
            new QuizOption { Id = "a", TextHtml = "Alpha", Correct = true },
            new QuizOption { Id = "b", TextHtml = "Bravo" },
            new QuizOption { Id = "c", TextHtml = "Charlie", Correct = true }
        ]
    };
}

internal sealed class FakeTimeProvider(DateTimeOffset now) : TimeProvider
{
    private DateTimeOffset _now = now;

    public override DateTimeOffset GetUtcNow() => _now;

    public void Advance(TimeSpan by) => _now = _now.Add(by);
}
