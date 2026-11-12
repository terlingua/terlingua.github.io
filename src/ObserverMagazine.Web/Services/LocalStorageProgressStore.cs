namespace ObserverMagazine.Web.Services;

public sealed class LocalStorageProgressStore(IBrowserStorage storage) : IProgressStore
{
    private const string ProbeKey = "book:__probe";

    public ValueTask<string?> GetAsync(string key) => storage.GetAsync(key);

    public ValueTask SetAsync(string key, string value) => storage.SetAsync(key, value);

    public ValueTask RemoveAsync(string key) => storage.RemoveAsync(key);

    public async ValueTask<bool> IsPersistentAsync()
    {
        try
        {
            await storage.SetAsync(ProbeKey, "1");
            await storage.RemoveAsync(ProbeKey);
            return true;
        }
        catch (BrowserStorageException)
        {
            return false;
        }
    }
}
