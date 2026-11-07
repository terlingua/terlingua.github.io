namespace ObserverMagazine.Web.Services;

public interface IProgressStore
{
    ValueTask<string?> GetAsync(string key);
    ValueTask SetAsync(string key, string value);
    ValueTask RemoveAsync(string key);
    ValueTask<bool> IsPersistentAsync();
}
