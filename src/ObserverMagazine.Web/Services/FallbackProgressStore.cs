namespace ObserverMagazine.Web.Services;

public sealed class FallbackProgressStore(
    IProgressStore primary,
    IProgressStore fallback,
    ILogger<FallbackProgressStore> logger) : IProgressStore
{
    private bool _primaryFailed;

    public bool IsUsingFallback => _primaryFailed;

    public ValueTask<string?> GetAsync(string key) =>
        ExecuteAsync(store => store.GetAsync(key));

    public async ValueTask SetAsync(string key, string value) =>
        await ExecuteAsync(async store =>
        {
            await store.SetAsync(key, value);
            return true;
        });

    public async ValueTask RemoveAsync(string key) =>
        await ExecuteAsync(async store =>
        {
            await store.RemoveAsync(key);
            return true;
        });

    public ValueTask<bool> IsPersistentAsync() =>
        ExecuteAsync(store => store.IsPersistentAsync());

    private async ValueTask<T> ExecuteAsync<T>(Func<IProgressStore, ValueTask<T>> operation)
    {
        if (!_primaryFailed)
        {
            try
            {
                return await operation(primary);
            }
            catch (Exception ex)
            {
                _primaryFailed = true;
                logger.LogWarning(ex, "Persistent progress storage is unavailable; progress will be kept in memory for this session");
            }
        }

        return await operation(fallback);
    }
}
