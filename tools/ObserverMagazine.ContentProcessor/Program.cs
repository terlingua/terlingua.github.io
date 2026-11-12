using System.Text.Json;
using ObserverMagazine.ContentProcessor;
using ObserverMagazine.ContentProcessor.Books;

string contentDir = "content/blog";
string outputDir = "src/ObserverMagazine.Web/wwwroot";
string authorsDir = "content/authors";
string booksDir = "content/books";
DateTime publishBefore = DateTime.UtcNow;

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--content-dir" && i + 1 < args.Length)
        contentDir = args[++i];
    else if (args[i] == "--output-dir" && i + 1 < args.Length)
        outputDir = args[++i];
    else if (args[i] == "--authors-dir" && i + 1 < args.Length)
        authorsDir = args[++i];
    else if (args[i] == "--books-dir" && i + 1 < args.Length)
        booksDir = args[++i];
    else if (args[i] == "--publish-before" && i + 1 < args.Length)
        publishBefore = DateTime.Parse(args[++i]);
}

Console.WriteLine($"Content directory: {contentDir}");
Console.WriteLine($"Output directory:  {outputDir}");
Console.WriteLine($"Authors directory: {authorsDir}");
Console.WriteLine($"Books directory:   {booksDir}");
Console.WriteLine($"Publish before:    {publishBefore:yyyy-MM-dd HH:mm:ss} UTC");

Dictionary<string, AuthorProfile> authorProfiles = [with(StringComparer.OrdinalIgnoreCase)];

if (Directory.Exists(authorsDir))
{
    var authorFiles = Directory.GetFiles(authorsDir, "*.yml", SearchOption.TopDirectoryOnly);
    Console.WriteLine($"Found {authorFiles.Length} author profile(s)");

    foreach (var authorFile in authorFiles)
    {
        var authorId = Path.GetFileNameWithoutExtension(authorFile);
        var yamlContent = File.ReadAllText(authorFile);
        var profile = FrontMatterParser.ParseAuthor(yamlContent, authorId);

        if (profile is not null)
        {
            authorProfiles[authorId] = profile;
            Console.WriteLine($"  Loaded author: {authorId} ({profile.Name})");
        }
        else
        {
            Console.WriteLine($"  WARNING: Could not parse author file {authorFile}");
        }
    }
}
else
{
    Console.WriteLine($"Authors directory '{authorsDir}' does not exist. No author profiles loaded.");
}

string blogDataDir = Path.Combine(outputDir, "blog-data");
Directory.CreateDirectory(blogDataDir);

var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

var authorsPath = Path.Combine(blogDataDir, "authors.json");
var authorsJson = JsonSerializer.Serialize(authorProfiles.Values.ToArray(), jsonOptions);
File.WriteAllText(authorsPath, authorsJson);
Console.WriteLine($"Wrote authors index: {authorsPath} ({authorProfiles.Count} authors)");

if (!Directory.Exists(contentDir))
{
    Console.WriteLine($"Content directory '{contentDir}' does not exist. Creating with no posts.");
    Directory.CreateDirectory(contentDir);
}

var markdownFiles = Directory.GetFiles(contentDir, "*.md", SearchOption.TopDirectoryOnly);
Console.WriteLine($"Found {markdownFiles.Length} markdown files");

List<PostIndexEntry> allPostMetadata = [];
Dictionary<string, string> postHtmlMap = [];
int skippedDrafts = 0;
int skippedFuture = 0;

foreach (var mdFile in markdownFiles)
{
    Console.WriteLine($"Processing: {Path.GetFileName(mdFile)}");

    var rawContent = File.ReadAllText(mdFile);
    var (frontMatter, markdownBody) = FrontMatterParser.Parse(rawContent);

    if (string.IsNullOrEmpty(frontMatter.Title))
    {
        Console.WriteLine($"  WARNING: No title in front matter, skipping {mdFile}");
        continue;
    }

    if (frontMatter.Draft)
    {
        Console.WriteLine($"  SKIPPED: Draft post '{frontMatter.Title}'");
        skippedDrafts++;
        continue;
    }

    if (frontMatter.Date > publishBefore)
    {
        Console.WriteLine($"  SKIPPED: Future post '{frontMatter.Title}' (date: {frontMatter.Date:yyyy-MM-dd}, publish-before: {publishBefore:yyyy-MM-dd})");
        skippedFuture++;
        continue;
    }

    var fileName = Path.GetFileNameWithoutExtension(mdFile);
    var slug = FrontMatterParser.DeriveSlug(fileName);

    var html = MarkdownProcessor.ToHtml(markdownBody);
    var readingTime = FrontMatterParser.CalculateReadingTime(markdownBody);

    var authorId = frontMatter.Author ?? "";
    var authorName = authorId;
    string? authorEmail = null;
    if (authorProfiles.TryGetValue(authorId, out var authorProfile))
    {
        authorName = authorProfile.Name;
        authorEmail = authorProfile.Email;
    }
    else if (!string.IsNullOrEmpty(authorId))
    {
        Console.WriteLine($"  WARNING: No author profile found for '{authorId}'");
    }

    var htmlPath = Path.Combine(blogDataDir, $"{slug}.html");
    File.WriteAllText(htmlPath, html);
    Console.WriteLine($"  Wrote: {htmlPath} (~{readingTime} min read)");

    postHtmlMap[slug] = html;

    allPostMetadata.Add(new PostIndexEntry
    {
        Slug = slug,
        Title = frontMatter.Title,
        Date = frontMatter.Date,
        Updated = frontMatter.Updated,
        Author = authorId,
        AuthorName = authorName,
        AuthorEmail = authorEmail,
        Summary = frontMatter.Summary ?? "",
        Tags = frontMatter.Tags ?? [],
        Featured = frontMatter.Featured,
        Series = frontMatter.Series,
        Image = frontMatter.Image,
        ReadingTimeMinutes = readingTime
    });
}

allPostMetadata.Sort((a, b) => b.Date.CompareTo(a.Date));

var indexPath = Path.Combine(blogDataDir, "posts-index.json");
var indexJson = JsonSerializer.Serialize(allPostMetadata, jsonOptions);
File.WriteAllText(indexPath, indexJson);
Console.WriteLine($"Wrote posts index: {indexPath} ({allPostMetadata.Count} posts, {skippedDrafts} drafts skipped, {skippedFuture} future posts skipped)");

var feedPath = Path.Combine(outputDir, "feed.xml");
var rssXml = RssGenerator.Generate(
    title: "Terlingua Magazine",
    description: "A free, open-source Blazor WebAssembly showcase on .NET 11",
    siteUrl: "https://terlingua.github.io",
    posts: allPostMetadata,
    getPostHtml: slug => postHtmlMap.GetValueOrDefault(slug)
);
File.WriteAllText(feedPath, rssXml);
Console.WriteLine($"Wrote RSS feed: {feedPath}");

var bookDiagnostics = new BuildDiagnostics();
var books = new BookCompiler(authorProfiles, publishBefore, bookDiagnostics, Console.Out).Compile(booksDir);

foreach (var warning in bookDiagnostics.Warnings)
{
    Console.WriteLine($"  WARNING: {warning}");
}

if (bookDiagnostics.HasErrors)
{
    foreach (var error in bookDiagnostics.Errors)
    {
        Console.Error.WriteLine($"  ERROR: {error}");
    }
    Console.Error.WriteLine($"Book processing failed with {bookDiagnostics.Errors.Count} error(s).");
    return 1;
}

var bookDataDir = BookOutputWriter.Write(books, outputDir, jsonOptions);
Console.WriteLine($"Wrote book data: {bookDataDir} ({books.Count} books)");

Console.WriteLine("Content processing complete.");
return 0;

public sealed class PostIndexEntry
{
    public string Slug { get; init; } = "";
    public string Title { get; init; } = "";
    public DateTime Date { get; init; }
    public DateTime? Updated { get; init; }
    public string Author { get; init; } = "";
    public string AuthorName { get; init; } = "";
    public string? AuthorEmail { get; init; }
    public string Summary { get; init; } = "";
    public string[] Tags { get; init; } = [];
    public bool Featured { get; init; }
    public string? Series { get; init; }
    public string? Image { get; init; }
    public int ReadingTimeMinutes { get; init; }
}
