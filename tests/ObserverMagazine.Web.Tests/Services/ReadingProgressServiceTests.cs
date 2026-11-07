using Microsoft.Extensions.Logging.Abstractions;
using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class ReadingProgressServiceTests
{
    private static readonly DateTimeOffset Start = new(2026, 11, 7, 12, 0, 0, TimeSpan.Zero);

    private static ReadingProgressService CreateService(IProgressStore store, FakeTimeProvider? time = null) =>
        new(store, time ?? new FakeTimeProvider(Start), NullLogger<ReadingProgressService>.Instance);

    [Fact]
    public async Task GetAsync_NoStoredData_ReturnsFreshProgress()
    {
        using var service = CreateService(new InMemoryProgressStore());

        var progress = await service.GetAsync("book");

        Assert.Equal("book", progress.BookSlug);
        Assert.False(progress.IsStarted);
        Assert.Empty(progress.Subchapters);
    }

    [Fact]
    public async Task RecordVisitAsync_FlagsFirstVisitsOnlyOnce()
    {
        using var service = CreateService(new InMemoryProgressStore());

        var first = await service.RecordVisitAsync("book", "ch", "a");
        var second = await service.RecordVisitAsync("book", "ch", "a");
        var other = await service.RecordVisitAsync("book", "ch", "b");

        Assert.True(first.IsFirstVisitToBook);
        Assert.True(first.IsFirstVisitToSubchapter);
        Assert.False(second.IsFirstVisitToBook);
        Assert.False(second.IsFirstVisitToSubchapter);
        Assert.False(other.IsFirstVisitToBook);
        Assert.True(other.IsFirstVisitToSubchapter);
        Assert.Equal("b", other.Progress.LastSubchapterSlug);
    }

    [Fact]
    public async Task RecordAnswerAsync_CorrectnessIsSticky()
    {
        using var service = CreateService(new InMemoryProgressStore());

        await service.RecordAnswerAsync("book", "ch", "a", "q1", ["x"], isCorrect: true);
        var progress = await service.RecordAnswerAsync("book", "ch", "a", "q1", ["y"], isCorrect: false);

        var question = progress.Subchapters["ch/a"].Questions["q1"];
        Assert.Equal(2, question.Attempts);
        Assert.True(question.AnsweredCorrectly);
        Assert.False(question.LastAttemptCorrect);
        Assert.Equal(["y"], question.LastSelection);
    }

    [Fact]
    public async Task Progress_PersistsAcrossServiceInstances()
    {
        var store = new InMemoryProgressStore();
        using (var writer = CreateService(store))
        {
            await writer.RecordVisitAsync("book", "ch", "a");
            await writer.RecordAnswerAsync("book", "ch", "a", "q1", ["x"], isCorrect: true);
        }

        using var reader = CreateService(store);
        var progress = await reader.GetAsync("book");

        Assert.True(progress.IsStarted);
        Assert.Equal<DateTimeOffset?>(Start, progress.StartedAt);
        Assert.True(progress.Subchapters["ch/a"].Questions["q1"].AnsweredCorrectly);
    }

    [Fact]
    public async Task CorruptStoredData_StartsFresh()
    {
        var store = new InMemoryProgressStore();
        await store.SetAsync(ReadingProgressService.StorageKey("book"), "{ not json");
        using var service = CreateService(store);

        var progress = await service.GetAsync("book");

        Assert.False(progress.IsStarted);
    }

    [Fact]
    public async Task IncompatibleSchemaVersion_StartsFresh()
    {
        var store = new InMemoryProgressStore();
        await store.SetAsync(ReadingProgressService.StorageKey("book"),
            """{ "schemaVersion": 999, "bookSlug": "book", "startedAt": "2026-01-01T00:00:00Z" }""");
        using var service = CreateService(store);

        var progress = await service.GetAsync("book");

        Assert.False(progress.IsStarted);
    }

    [Fact]
    public async Task NullCollectionsInStoredData_AreNormalized()
    {
        var store = new InMemoryProgressStore();
        await store.SetAsync(ReadingProgressService.StorageKey("book"),
            """{ "schemaVersion": 1, "bookSlug": "book", "subchapters": { "ch/a": { "questions": null } } }""");
        using var service = CreateService(store);

        var progress = await service.GetAsync("book");

        Assert.Empty(progress.Subchapters["ch/a"].Questions);
    }

    [Fact]
    public async Task ResetAsync_ClearsMemoryAndStorage()
    {
        var store = new InMemoryProgressStore();
        using var service = CreateService(store);
        await service.RecordVisitAsync("book", "ch", "a");

        var reset = await service.ResetAsync("book");

        Assert.False(reset.IsStarted);
        Assert.Null(await store.GetAsync(ReadingProgressService.StorageKey("book")));
    }

    [Fact]
    public async Task FailingStore_DoesNotThrow()
    {
        using var service = CreateService(new ThrowingProgressStore());

        var visit = await service.RecordVisitAsync("book", "ch", "a");
        var persistent = await service.IsPersistentAsync();

        Assert.True(visit.IsFirstVisitToBook);
        Assert.False(persistent);
    }
}

internal sealed class ThrowingProgressStore : IProgressStore
{
    public int Calls { get; private set; }

    public ValueTask<string?> GetAsync(string key) => Fail<string?>();
    public ValueTask SetAsync(string key, string value) => new(Fail<bool>().AsTask());
    public ValueTask RemoveAsync(string key) => new(Fail<bool>().AsTask());
    public ValueTask<bool> IsPersistentAsync() => Fail<bool>();

    private ValueTask<T> Fail<T>()
    {
        Calls++;
        return ValueTask.FromException<T>(new InvalidOperationException("storage unavailable"));
    }
}
