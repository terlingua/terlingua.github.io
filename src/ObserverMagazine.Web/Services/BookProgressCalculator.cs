using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public static class BookProgressCalculator
{
    public static SubchapterStatus GetStatus(SubchapterLocation location, BookProgress? progress)
    {
        var subProgress = GetSubchapterProgress(location, progress);
        if (subProgress is null || (subProgress.FirstVisitedAt is null && subProgress.Questions.Count == 0))
        {
            return SubchapterStatus.NotStarted;
        }

        return IsComplete(location.Subchapter, subProgress) ? SubchapterStatus.Completed : SubchapterStatus.InProgress;
    }

    public static bool IsComplete(SubchapterToc subchapter, SubchapterProgress? progress)
    {
        if (progress is null)
        {
            return false;
        }

        return subchapter.QuestionIds.Length == 0
            ? progress.FirstVisitedAt is not null
            : subchapter.QuestionIds.All(id => progress.Questions.TryGetValue(id, out var q) && q.AnsweredCorrectly);
    }

    public static int CountCorrect(SubchapterToc subchapter, SubchapterProgress? progress) =>
        progress is null
            ? 0
            : subchapter.QuestionIds.Count(id => progress.Questions.TryGetValue(id, out var q) && q.AnsweredCorrectly);

    public static BookCompletion GetCompletion(BookToc toc, BookProgress? progress)
    {
        var completed = 0;
        var correct = 0;
        var total = 0;
        var totalQuestions = 0;

        foreach (var location in BookNavigator.Flatten(toc))
        {
            var subProgress = GetSubchapterProgress(location, progress);
            total++;
            totalQuestions += location.Subchapter.QuestionIds.Length;
            correct += CountCorrect(location.Subchapter, subProgress);
            if (IsComplete(location.Subchapter, subProgress))
            {
                completed++;
            }
        }

        return new BookCompletion(completed, total, correct, totalQuestions);
    }

    public static SubchapterLocation? GetContinueLocation(BookToc toc, BookProgress? progress)
    {
        var all = BookNavigator.Flatten(toc);
        if (all.Count == 0)
        {
            return null;
        }

        var startIndex = 0;
        if (progress?.LastChapterSlug is { } chapterSlug && progress.LastSubchapterSlug is { } subSlug)
        {
            var last = BookNavigator.Find(toc, chapterSlug, subSlug);
            if (last is not null)
            {
                startIndex = BookNavigator.IndexOf(all, last);
                if (!IsComplete(last.Subchapter, GetSubchapterProgress(last, progress)))
                {
                    return last;
                }
            }
        }

        for (var offset = 0; offset < all.Count; offset++)
        {
            var candidate = all[(startIndex + offset) % all.Count];
            if (!IsComplete(candidate.Subchapter, GetSubchapterProgress(candidate, progress)))
            {
                return candidate;
            }
        }

        return all[0];
    }

    private static SubchapterProgress? GetSubchapterProgress(SubchapterLocation location, BookProgress? progress) =>
        progress?.Subchapters.GetValueOrDefault(location.ProgressKey);
}
