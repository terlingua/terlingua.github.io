namespace ObserverMagazine.Web.Services;

public sealed class BookAnalytics(IAnalyticsService analytics, ILogger<BookAnalytics> logger) : IBookAnalytics
{
    public static string BookReadersKey(string bookSlug) => $"book--{bookSlug}";

    public static string SubchapterReadersKey(string bookSlug, string chapterSlug, string subchapterSlug) =>
        $"book--{bookSlug}--{chapterSlug}--{subchapterSlug}";

    public Task RecordBookStartedAsync(string bookSlug) =>
        RunSafeAsync(async () =>
        {
            await analytics.TrackInteractionAsync("BookStarted", bookSlug);
            await analytics.IncrementViewAsync(BookReadersKey(bookSlug));
        });

    public Task<int?> GetReaderCountAsync(string bookSlug) =>
        QuerySafeAsync(() => analytics.GetViewCountAsync(BookReadersKey(bookSlug)));

    public Task RecordSubchapterOpenedAsync(string bookSlug, string chapterSlug, string subchapterSlug, bool isFirstVisit) =>
        RunSafeAsync(async () =>
        {
            await analytics.TrackPageViewAsync("BookSubchapter", $"{bookSlug}/{chapterSlug}/{subchapterSlug}");
            if (isFirstVisit)
            {
                await analytics.IncrementViewAsync(SubchapterReadersKey(bookSlug, chapterSlug, subchapterSlug));
            }
        });

    public Task<int?> GetSubchapterReaderCountAsync(string bookSlug, string chapterSlug, string subchapterSlug) =>
        QuerySafeAsync(() => analytics.GetViewCountAsync(SubchapterReadersKey(bookSlug, chapterSlug, subchapterSlug)));

    public Task RecordQuizAnswerAsync(string bookSlug, string chapterSlug, string subchapterSlug, string questionId, bool isCorrect) =>
        RunSafeAsync(() => analytics.TrackInteractionAsync(
            "QuizAnswer",
            $"{bookSlug}/{chapterSlug}/{subchapterSlug}#{questionId}: {(isCorrect ? "correct" : "incorrect")}"));

    private async Task RunSafeAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Book analytics call failed");
        }
    }

    private async Task<int?> QuerySafeAsync(Func<Task<int?>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Book analytics query failed");
            return null;
        }
    }
}
