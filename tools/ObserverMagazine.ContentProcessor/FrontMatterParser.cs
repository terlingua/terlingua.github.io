using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ObserverMagazine.ContentProcessor;

public static class FrontMatterParser
{
    private const string Delimiter = "---";
    private const string ClosingDelimiter = "\n---";

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    public static (FrontMatter FrontMatter, string MarkdownBody) Parse(string rawContent) =>
        Parse<FrontMatter>(rawContent);

    public static (T FrontMatter, string MarkdownBody) Parse<T>(string rawContent) where T : new()
    {
        if (!rawContent.StartsWith(Delimiter, StringComparison.Ordinal))
        {
            return (new T(), rawContent);
        }

        var endIndex = rawContent.IndexOf(ClosingDelimiter, Delimiter.Length, StringComparison.Ordinal);
        if (endIndex < 0)
        {
            return (new T(), rawContent);
        }

        var yamlBlock = rawContent[Delimiter.Length..endIndex].Trim();
        var body = rawContent[(endIndex + ClosingDelimiter.Length)..].TrimStart('\r', '\n');

        return (DeserializeYaml<T>(yamlBlock), body);
    }

    public static T DeserializeYaml<T>(string yaml) where T : new() =>
        string.IsNullOrWhiteSpace(yaml) ? new T() : Deserializer.Deserialize<T>(yaml) ?? new T();

    public static AuthorProfile? ParseAuthor(string yamlContent, string id)
    {
        try
        {
            var profile = Deserializer.Deserialize<AuthorProfile>(yamlContent);
            profile.Id = id;
            return profile;
        }
        catch
        {
            return null;
        }
    }

    public static string DeriveSlug(string fileName)
    {
        if (fileName.Length > 11 &&
            char.IsDigit(fileName[0]) &&
            fileName[4] == '-' &&
            fileName[7] == '-' &&
            fileName[10] == '-')
        {
            return fileName[11..];
        }
        return fileName;
    }

    public static int CalculateReadingTime(string markdownBody)
    {
        var wordCount = markdownBody
            .Split([' ', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries)
            .Length;
        return Math.Max(1, (int)Math.Ceiling(wordCount / 200.0));
    }
}

public sealed class FrontMatter
{
    public string Title { get; init; } = "";
    public DateTime Date { get; init; }
    public DateTime? Updated { get; init; }
    public string? Author { get; init; }
    public string? Summary { get; init; }
    public string[]? Tags { get; init; }
    public bool Draft { get; init; }
    public bool Featured { get; init; }
    public string? Series { get; init; }
    public string? Image { get; init; }
}

public sealed class AuthorProfile
{
    public string Id { get; set; } = "";
    public string Name { get; init; } = "";
    public string? Email { get; init; }
    public string? Bio { get; init; }
    public string? Avatar { get; init; }
    public Dictionary<string, string>? Socials { get; init; }
}
