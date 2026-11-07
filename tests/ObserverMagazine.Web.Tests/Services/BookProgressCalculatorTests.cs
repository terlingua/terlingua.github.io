using ObserverMagazine.Web.Models;
using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class BookProgressCalculatorTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 7, 12, 0, 0, TimeSpan.Zero);

    private static BookProgress ProgressWith(params (string Key, SubchapterProgress Progress)[] entries)
    {
        var progress = new BookProgress { BookSlug = "sample-book", StartedAt = Now };
        foreach (var (key, sub) in entries)
        {
            progress.Subchapters[key] = sub;
        }
        return progress;
    }

    private static SubchapterProgress Visited(params string[] correctQuestionIds)
    {
        var sub = new SubchapterProgress { FirstVisitedAt = Now, LastVisitedAt = Now };
        foreach (var id in correctQuestionIds)
        {
            sub.Questions[id] = new QuestionProgress { Attempts = 1, AnsweredCorrectly = true, LastAttemptCorrect = true };
        }
        return sub;
    }

    [Fact]
    public void GetCompletion_NoProgress_IsZero()
    {
        var completion = BookProgressCalculator.GetCompletion(BookTestData.Toc(), null);

        Assert.Equal(0, completion.CompletedSubchapters);
        Assert.Equal(3, completion.TotalSubchapters);
        Assert.Equal(3, completion.TotalQuestions);
        Assert.Equal(0, completion.Percent);
    }

    [Fact]
    public void SubchapterWithoutQuestions_CompletesOnVisit()
    {
        var progress = ProgressWith(("intro/reading", Visited()));
        var location = BookNavigator.Find(BookTestData.Toc(), "intro", "reading")!;

        Assert.Equal(SubchapterStatus.Completed, BookProgressCalculator.GetStatus(location, progress));
    }

    [Fact]
    public void SubchapterWithQuestions_InProgressUntilAllCorrect()
    {
        var toc = BookTestData.Toc();
        var location = BookNavigator.Find(toc, "intro", "hello")!;

        var partial = ProgressWith(("intro/hello", Visited("q1")));
        var complete = ProgressWith(("intro/hello", Visited("q1", "q2")));

        Assert.Equal(SubchapterStatus.InProgress, BookProgressCalculator.GetStatus(location, partial));
        Assert.Equal(SubchapterStatus.Completed, BookProgressCalculator.GetStatus(location, complete));
    }

    [Fact]
    public void GetCompletion_PercentRoundsDown()
    {
        var progress = ProgressWith(("intro/reading", Visited()), ("intro/hello", Visited("q1", "q2")));

        var completion = BookProgressCalculator.GetCompletion(BookTestData.Toc(), progress);

        Assert.Equal(2, completion.CompletedSubchapters);
        Assert.Equal(66, completion.Percent);
        Assert.Equal(2, completion.CorrectQuestions);
        Assert.False(completion.IsComplete);
    }

    [Fact]
    public void GetContinueLocation_NoProgress_ReturnsFirstSection()
    {
        var location = BookProgressCalculator.GetContinueLocation(BookTestData.Toc(), null);

        Assert.NotNull(location);
        Assert.Equal("1.1", location.Label);
    }

    [Fact]
    public void GetContinueLocation_ReturnsLastVisitedWhenIncomplete()
    {
        var progress = ProgressWith(("deeper/details", Visited()));
        progress.LastChapterSlug = "deeper";
        progress.LastSubchapterSlug = "details";

        var location = BookProgressCalculator.GetContinueLocation(BookTestData.Toc(), progress);

        Assert.Equal("2.1", location!.Label);
    }

    [Fact]
    public void GetContinueLocation_SkipsCompletedSections()
    {
        var progress = ProgressWith(("intro/hello", Visited("q1", "q2")));
        progress.LastChapterSlug = "intro";
        progress.LastSubchapterSlug = "hello";

        var location = BookProgressCalculator.GetContinueLocation(BookTestData.Toc(), progress);

        Assert.Equal("1.2", location!.Label);
    }

    [Fact]
    public void Navigator_GetNeighbors_CrossesChapterBoundaries()
    {
        var toc = BookTestData.Toc();
        var current = BookNavigator.Find(toc, "intro", "reading")!;

        var neighbors = BookNavigator.GetNeighbors(toc, current);

        Assert.Equal("1.1", neighbors.Previous!.Label);
        Assert.Equal("2.1", neighbors.Next!.Label);
    }

    [Fact]
    public void Navigator_GetNeighbors_AtEdges()
    {
        var toc = BookTestData.Toc();

        var first = BookNavigator.GetNeighbors(toc, BookNavigator.Find(toc, "intro", "hello")!);
        var last = BookNavigator.GetNeighbors(toc, BookNavigator.Find(toc, "deeper", "details")!);

        Assert.Null(first.Previous);
        Assert.Null(last.Next);
    }

    [Fact]
    public void Navigator_Find_IsCaseInsensitiveAndReturnsNullForUnknown()
    {
        var toc = BookTestData.Toc();

        Assert.NotNull(BookNavigator.Find(toc, "INTRO", "Hello"));
        Assert.Null(BookNavigator.Find(toc, "intro", "missing"));
    }
}
