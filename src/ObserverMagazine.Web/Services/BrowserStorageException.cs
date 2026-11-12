namespace ObserverMagazine.Web.Services;

public sealed class BrowserStorageException(string message, Exception innerException)
    : Exception(message, innerException);
