namespace ObserverMagazine.Web.Services;

public sealed class InMemoryProgressStore : IProgressStore
{
    private readonly Dictionary<string, string> _items = [with(StringComparer.Ordinal)];

    public ValueTask<string?> GetAsync(string key) =>
        ValueTask.FromResult(_items.GetValueOrDefault(key));

    public ValueTask SetAsync(string key, string value)
    {
        _items[key] = value;
        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveAsync(string key)
    {
        _items.Remove(key);
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> IsPersistentAsync() => ValueTask.FromResult(false);
}
