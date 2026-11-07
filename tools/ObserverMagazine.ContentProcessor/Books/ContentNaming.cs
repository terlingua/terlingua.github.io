using System.Globalization;
using System.Text.RegularExpressions;

namespace ObserverMagazine.ContentProcessor.Books;

public static partial class ContentNaming
{
    [GeneratedRegex(@"^(\d+)[-_]([a-z0-9]+(?:-[a-z0-9]+)*)$", RegexOptions.CultureInvariant)]
    private static partial Regex NumberedPattern();

    [GeneratedRegex(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
    private static partial Regex SlugPattern();

    public static bool IsValidSlug(string? slug) =>
        !string.IsNullOrEmpty(slug) && SlugPattern().IsMatch(slug);

    public static bool TryParseNumbered(string name, out int order, out string slug)
    {
        order = 0;
        slug = "";

        var match = NumberedPattern().Match(name);
        if (!match.Success ||
            !int.TryParse(match.Groups[1].ValueSpan, NumberStyles.None, CultureInfo.InvariantCulture, out order))
        {
            return false;
        }

        slug = match.Groups[2].Value;
        return true;
    }

    public static bool IsHidden(string path)
    {
        var name = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        return name.StartsWith('.') || name.StartsWith('_');
    }
}
