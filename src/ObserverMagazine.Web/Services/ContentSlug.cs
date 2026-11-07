using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ObserverMagazine.Web.Services;

public static partial class ContentSlug
{
    private const int MaxLength = 200;

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
    private static partial Regex Pattern();

    public static bool TryNormalize(string? value, [NotNullWhen(true)] out string? slug)
    {
        slug = null;
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxLength)
        {
            return false;
        }

        var candidate = value.Trim().ToLowerInvariant();
        if (!Pattern().IsMatch(candidate))
        {
            return false;
        }

        slug = candidate;
        return true;
    }
}
