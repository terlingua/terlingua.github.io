namespace ObserverMagazine.Web.Services;

public sealed class TelemetryService(ILogger<TelemetryService> logger)
{
    private readonly Dictionary<string, long> _counters = [];
    private readonly Lock _lock = new();

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
        var props = properties is not null
            ? string.Join(", ", properties.Select(kv => $"{kv.Key}={kv.Value}"))
            : "";
        logger.LogInformation("[Telemetry] Event: {EventName} {Properties}", eventName, props);
    }

    public void IncrementCounter(string counterName, long value = 1)
    {
        lock (_lock)
        {
            _counters.TryGetValue(counterName, out var current);
            _counters[counterName] = current + value;
        }
        logger.LogDebug("[Telemetry] Counter: {CounterName} += {Value}", counterName, value);
    }

    public long GetCounter(string counterName)
    {
        lock (_lock)
        {
            return _counters.GetValueOrDefault(counterName, 0);
        }
    }

    public void TrackPageView(string pageName)
    {
        IncrementCounter($"pageview:{pageName}");
        TrackEvent("PageView", new Dictionary<string, string> { ["page"] = pageName });
    }
}
