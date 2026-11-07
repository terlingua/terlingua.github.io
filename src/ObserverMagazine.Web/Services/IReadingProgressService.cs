using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public interface IReadingProgressService
{
    Task<BookProgress> GetAsync(string bookSlug);

    Task<VisitOutcome> RecordVisitAsync(string bookSlug, string chapterSlug, string subchapterSlug);

    Task<BookProgress> RecordAnswerAsync(
        string bookSlug,
        string chapterSlug,
        string subchapterSlug,
        string questionId,
        IReadOnlyCollection<string> selectedOptionIds,
        bool isCorrect);

    Task<BookProgress> ResetAsync(string bookSlug);

    Task<bool> IsPersistentAsync();
}
