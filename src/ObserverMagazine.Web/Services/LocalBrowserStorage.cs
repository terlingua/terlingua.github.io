using Microsoft.JSInterop;

namespace ObserverMagazine.Web.Services;

public sealed class LocalBrowserStorage(IJSRuntime js) : IBrowserStorage
{
    public async ValueTask<string?> GetAsync(string key)
    {
        try
        {
            return await js.InvokeAsync<string?>("localStorage.getItem", key);
        }
        catch (JSException ex)
        {
            throw Unavailable(ex);
        }
    }

    public async ValueTask SetAsync(string key, string value)
    {
        try
        {
            await js.InvokeVoidAsync("localStorage.setItem", key, value);
        }
        catch (JSException ex)
        {
            throw Unavailable(ex);
        }
    }

    public async ValueTask RemoveAsync(string key)
    {
        try
        {
            await js.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (JSException ex)
        {
            throw Unavailable(ex);
        }
    }

    private static BrowserStorageException Unavailable(JSException ex) =>
        new("localStorage is not available", ex);
}
