using Microsoft.Extensions.Logging.Abstractions;
using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class FallbackProgressStoreTests
{
    [Fact]
    public async Task HealthyPrimary_IsUsed()
    {
        var primary = new InMemoryProgressStore();
        var fallback = new InMemoryProgressStore();
        var store = new FallbackProgressStore(primary, fallback, NullLogger<FallbackProgressStore>.Instance);

        await store.SetAsync("k", "v");

        Assert.Equal("v", await primary.GetAsync("k"));
        Assert.Null(await fallback.GetAsync("k"));
        Assert.False(store.IsUsingFallback);
    }

    [Fact]
    public async Task FailingPrimary_SwitchesToFallbackPermanently()
    {
        var primary = new ThrowingProgressStore();
        var fallback = new InMemoryProgressStore();
        var store = new FallbackProgressStore(primary, fallback, NullLogger<FallbackProgressStore>.Instance);

        await store.SetAsync("k", "v");
        var value = await store.GetAsync("k");
        var persistent = await store.IsPersistentAsync();

        Assert.Equal("v", value);
        Assert.False(persistent);
        Assert.True(store.IsUsingFallback);
        Assert.Equal(1, primary.Calls);
    }

    [Fact]
    public async Task RemoveAsync_WorksOnFallback()
    {
        var store = new FallbackProgressStore(new ThrowingProgressStore(), new InMemoryProgressStore(), NullLogger<FallbackProgressStore>.Instance);

        await store.SetAsync("k", "v");
        await store.RemoveAsync("k");

        Assert.Null(await store.GetAsync("k"));
    }
}
