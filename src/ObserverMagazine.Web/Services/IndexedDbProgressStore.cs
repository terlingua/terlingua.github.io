using Microsoft.JSInterop;

namespace ObserverMagazine.Web.Services;

public sealed class IndexedDbProgressStore(IJSRuntime js) : IProgressStore, IAsyncDisposable
{
    private const string ModulePath = "./js/book-progress.js";
    private static readonly TimeSpan CallTimeout = TimeSpan.FromSeconds(5);

    private Task<IJSObjectReference>? _module;

    public async ValueTask<string?> GetAsync(string key)
    {
        var module = await GetModuleAsync();
        return await module.InvokeAsync<string?>("get", CallTimeout, key);
    }

    public async ValueTask SetAsync(string key, string value)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("set", CallTimeout, key, value);
    }

    public async ValueTask RemoveAsync(string key)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("remove", CallTimeout, key);
    }

    public async ValueTask<bool> IsPersistentAsync()
    {
        var module = await GetModuleAsync();
        return await module.InvokeAsync<bool>("isPersistent", CallTimeout);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not { IsCompletedSuccessfully: true })
        {
            return;
        }

        try
        {
            await _module.Result.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }

    private Task<IJSObjectReference> GetModuleAsync() =>
        _module ??= js.InvokeAsync<IJSObjectReference>("import", CallTimeout, ModulePath).AsTask();
}
