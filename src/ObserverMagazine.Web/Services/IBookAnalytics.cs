namespace ObserverMagazine.Web.Services;

public interface IBookAnalytics
{
    Task RecordBookStartedAsync(string bookSlug);
    Task<int?> GetReaderCountAsync(string bookSlug);
    Task RecordSubchapterOpenedAsync(string bookSlug, string chapterSlug, string subchapterSlug, bool isFirstVisit);
    Task<int?> GetSubchapterReaderCountAsync(string bookSlug, string chapterSlug, string subchapterSlug);
    Task RecordQuizAnswerAsync(string bookSlug, string chapterSlug, string subchapterSlug, string questionId, bool isCorrect);
}
