using System.Text.Json;
using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public sealed class ReadingProgressService(
    IProgressStore store,
    TimeProvider time,
    ILogger<ReadingProgressService> logger) : IReadingProgressService, IDisposable
{
    public const string KeyPrefix = "book:";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly Dictionary<string, BookProgress> _cache = [with(StringComparer.Ordinal)];
    private readonly SemaphoreSlim _gate = new(1, 1);

    public static string StorageKey(string bookSlug) => KeyPrefix + bookSlug;

    public async Task<BookProgress> GetAsync(string bookSlug)
    {
        await _gate.WaitAsync();
        try
        {
            return await LoadAsync(bookSlug);
        }
        finally
        {
            _gate.Release();
        }
    }

    public Task<VisitOutcome> RecordVisitAsync(string bookSlug, string chapterSlug, string subchapterSlug) =>
        MutateAsync(bookSlug, (progress, now) =>
        {
            var firstVisitToBook = progress.StartedAt is null;
            progress.StartedAt ??= now;

            var subchapter = GetOrAddSubchapter(progress, chapterSlug, subchapterSlug);
            var firstVisitToSubchapter = subchapter.FirstVisitedAt is null;
            subchapter.FirstVisitedAt ??= now;
            subchapter.LastVisitedAt = now;

            progress.LastChapterSlug = chapterSlug;
            progress.LastSubchapterSlug = subchapterSlug;

            return new VisitOutcome(progress, firstVisitToBook, firstVisitToSubchapter);
        });

    public Task<BookProgress> RecordAnswerAsync(
        string bookSlug,
        string chapterSlug,
        string subchapterSlug,
        string questionId,
        IReadOnlyCollection<string> selectedOptionIds,
        bool isCorrect) =>
        MutateAsync(bookSlug, (progress, now) =>
        {
            progress.StartedAt ??= now;

            var subchapter = GetOrAddSubchapter(progress, chapterSlug, subchapterSlug);
            subchapter.FirstVisitedAt ??= now;

            if (!subchapter.Questions.TryGetValue(questionId, out var question))
            {
                question = new QuestionProgress();
                subchapter.Questions[questionId] = question;
            }

            question.Attempts++;
            question.LastAttemptCorrect = isCorrect;
            question.AnsweredCorrectly |= isCorrect;
            question.LastSelection = selectedOptionIds.ToArray();
            question.LastAttemptAt = now;

            return progress;
        });

    public async Task<BookProgress> ResetAsync(string bookSlug)
    {
        await _gate.WaitAsync();
        try
        {
            var fresh = new BookProgress { BookSlug = bookSlug };
            _cache[bookSlug] = fresh;

            try
            {
                await store.RemoveAsync(StorageKey(bookSlug));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to remove stored progress for {Book}", bookSlug);
            }

            return fresh;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<bool> IsPersistentAsync()
    {
        try
        {
            return await store.IsPersistentAsync();
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Could not determine whether progress storage is persistent");
            return false;
        }
    }

    public void Dispose() => _gate.Dispose();

    private async Task<T> MutateAsync<T>(string bookSlug, Func<BookProgress, DateTimeOffset, T> mutation)
    {
        await _gate.WaitAsync();
        try
        {
            var progress = await LoadAsync(bookSlug);
            var now = time.GetUtcNow();
            var result = mutation(progress, now);
            progress.UpdatedAt = now;
            await SaveAsync(progress);
            return result;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<BookProgress> LoadAsync(string bookSlug)
    {
        if (_cache.TryGetValue(bookSlug, out var cached))
        {
            return cached;
        }

        var progress = await ReadAsync(bookSlug) ?? new BookProgress { BookSlug = bookSlug };
        _cache[bookSlug] = progress;
        return progress;
    }

    private async Task<BookProgress?> ReadAsync(string bookSlug)
    {
        string? json;
        try
        {
            json = await store.GetAsync(StorageKey(bookSlug));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to read stored progress for {Book}", bookSlug);
            return null;
        }

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            var progress = JsonSerializer.Deserialize<BookProgress>(json, JsonOptions);
            if (progress is null ||
                progress.SchemaVersion != BookProgress.CurrentSchemaVersion ||
                !string.Equals(progress.BookSlug, bookSlug, StringComparison.Ordinal))
            {
                logger.LogWarning("Discarding incompatible stored progress for {Book}", bookSlug);
                return null;
            }

            return Normalize(progress);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Discarding corrupt stored progress for {Book}", bookSlug);
            return null;
        }
    }

    private async Task SaveAsync(BookProgress progress)
    {
        try
        {
            await store.SetAsync(StorageKey(progress.BookSlug), JsonSerializer.Serialize(progress, JsonOptions));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to save progress for {Book}", progress.BookSlug);
        }
    }

    private static SubchapterProgress GetOrAddSubchapter(BookProgress progress, string chapterSlug, string subchapterSlug)
    {
        var key = BookProgress.SubchapterKey(chapterSlug, subchapterSlug);
        if (!progress.Subchapters.TryGetValue(key, out var subchapter))
        {
            subchapter = new SubchapterProgress();
            progress.Subchapters[key] = subchapter;
        }
        return subchapter;
    }

    private static BookProgress Normalize(BookProgress progress)
    {
        Dictionary<string, SubchapterProgress> subchapters = [with(StringComparer.Ordinal)];

        foreach (var (key, subchapter) in progress.Subchapters ?? [])
        {
            if (subchapter is null)
            {
                continue;
            }

            Dictionary<string, QuestionProgress> questions = [with(StringComparer.Ordinal)];
            foreach (var (questionId, question) in subchapter.Questions ?? [])
            {
                if (question is null)
                {
                    continue;
                }
                question.LastSelection ??= [];
                questions[questionId] = question;
            }

            subchapter.Questions = questions;
            subchapters[key] = subchapter;
        }

        progress.Subchapters = subchapters;
        return progress;
    }
}
