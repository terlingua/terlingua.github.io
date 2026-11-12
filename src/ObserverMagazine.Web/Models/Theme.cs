namespace ObserverMagazine.Web.Models;

public sealed record Theme(string Id, string Label)
{
    public const string StorageKey = "observer-theme";

    public static readonly Theme Default = new("system", "System");

    public static readonly IReadOnlyList<Theme> All =
    [
        Default,
        new("light", "Light"),
        new("dark", "Dark"),
        new("sepia", "Sepia"),
        new("solarized-light", "Solarized Light"),
        new("solarized-dark", "Solarized Dark"),
        new("high-contrast", "High Contrast")
    ];

    public static Theme FromId(string? id) =>
        All.FirstOrDefault(t => string.Equals(t.Id, id, StringComparison.Ordinal)) ?? Default;
}
