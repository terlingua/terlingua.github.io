using YamlDotNet.Core;

namespace ObserverMagazine.ContentProcessor.Books;

public sealed class BookCompiler(
    IReadOnlyDictionary<string, AuthorProfile> authors,
    DateTime publishBefore,
    BuildDiagnostics diagnostics,
    TextWriter log)
{
    public const string BookManifestFile = "book.yml";
    public const string ChapterManifestFile = "chapter.yml";

    private string _root = "";

    public IReadOnlyList<CompiledBook> Compile(string booksDir)
    {
        if (!Directory.Exists(booksDir))
        {
            log.WriteLine($"Books directory '{booksDir}' does not exist. No books processed.");
            return [];
        }

        _root = booksDir;
        var books = new List<CompiledBook>();

        foreach (var bookDir in Directory.GetDirectories(booksDir).Where(d => !ContentNaming.IsHidden(d)).Order(StringComparer.Ordinal))
        {
            var book = CompileBook(bookDir);
            if (book is not null)
            {
                books.Add(book);
            }
        }

        return books
            .OrderByDescending(b => b.Index.Date)
            .ThenBy(b => b.Index.Slug, StringComparer.Ordinal)
            .ToArray();
    }

    private CompiledBook? CompileBook(string bookDir)
    {
        var slug = Path.GetFileName(bookDir);
        var location = Relative(bookDir);
        log.WriteLine($"Processing book: {slug}");

        if (!ContentNaming.IsValidSlug(slug))
        {
            diagnostics.Error(location, "book folder name must contain only lowercase letters, digits and single hyphens");
            return null;
        }

        var manifest = ReadYaml<BookManifest>(Path.Combine(bookDir, BookManifestFile));
        if (manifest is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(manifest.Title))
        {
            diagnostics.Error(Relative(Path.Combine(bookDir, BookManifestFile)), "title is required");
            return null;
        }

        if (manifest.Draft)
        {
            log.WriteLine($"  SKIPPED: Draft book '{manifest.Title}'");
            return null;
        }

        if (manifest.Date > publishBefore)
        {
            log.WriteLine($"  SKIPPED: Future book '{manifest.Title}' (date: {manifest.Date:yyyy-MM-dd})");
            return null;
        }

        var documents = new List<SubchapterDocument>();
        var chapters = CompileChapters(bookDir, slug, documents);

        if (chapters.Count == 0)
        {
            diagnostics.Warning(location, "book has no published chapters and was skipped");
            return null;
        }

        var authorId = manifest.Author ?? "";
        var authorName = ResolveAuthorName(authorId, location);
        var tags = manifest.Tags ?? [];
        var summary = manifest.Summary ?? "";

        var toc = new BookToc
        {
            Slug = slug,
            Title = manifest.Title,
            Date = manifest.Date,
            Updated = manifest.Updated,
            Author = authorId,
            AuthorName = authorName,
            Summary = summary,
            Tags = tags,
            Image = manifest.Image,
            Chapters = chapters
        };

        var index = new BookIndexEntry
        {
            Slug = slug,
            Title = manifest.Title,
            Date = manifest.Date,
            Updated = manifest.Updated,
            Author = authorId,
            AuthorName = authorName,
            Summary = summary,
            Tags = tags,
            Image = manifest.Image,
            ChapterCount = chapters.Count,
            SubchapterCount = documents.Count,
            QuestionCount = documents.Sum(d => d.Questions.Count),
            ReadingTimeMinutes = documents.Sum(d => d.ReadingTimeMinutes)
        };

        log.WriteLine($"  {chapters.Count} chapter(s), {index.SubchapterCount} subchapter(s), {index.QuestionCount} question(s)");
        return new CompiledBook(index, toc, documents);
    }

    private List<ChapterToc> CompileChapters(string bookDir, string bookSlug, List<SubchapterDocument> documents)
    {
        var chapters = new List<ChapterToc>();
        var seenSlugs = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in ReadNumberedEntries(Directory.GetDirectories(bookDir), p => Path.GetFileName(p)))
        {
            var location = Relative(entry.Path);

            if (!seenSlugs.Add(entry.Slug))
            {
                diagnostics.Error(location, $"duplicate chapter slug '{entry.Slug}'");
                continue;
            }

            var manifest = ReadYaml<ChapterManifest>(Path.Combine(entry.Path, ChapterManifestFile));
            if (manifest is null)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(manifest.Title))
            {
                diagnostics.Error(Relative(Path.Combine(entry.Path, ChapterManifestFile)), "title is required");
                continue;
            }

            if (manifest.Draft)
            {
                log.WriteLine($"  SKIPPED: Draft chapter '{manifest.Title}'");
                continue;
            }

            var chapterNumber = chapters.Count + 1;
            var subchapters = CompileSubchapters(entry.Path, bookSlug, entry.Slug, chapterNumber, documents);

            if (subchapters.Count == 0)
            {
                diagnostics.Warning(location, "chapter has no published subchapters and was skipped");
                continue;
            }

            chapters.Add(new ChapterToc
            {
                Number = chapterNumber,
                Slug = entry.Slug,
                Title = manifest.Title,
                Summary = manifest.Summary ?? "",
                Subchapters = subchapters
            });
        }

        return chapters;
    }

    private List<SubchapterToc> CompileSubchapters(
        string chapterDir,
        string bookSlug,
        string chapterSlug,
        int chapterNumber,
        List<SubchapterDocument> documents)
    {
        var subchapters = new List<SubchapterToc>();
        var seenSlugs = new HashSet<string>(StringComparer.Ordinal);

        foreach (var entry in ReadNumberedEntries(Directory.GetFiles(chapterDir, "*.md"), p => Path.GetFileNameWithoutExtension(p)))
        {
            var location = Relative(entry.Path);

            if (!seenSlugs.Add(entry.Slug))
            {
                diagnostics.Error(location, $"duplicate subchapter slug '{entry.Slug}'");
                continue;
            }

            var parsed = ParseSubchapter(entry.Path, location);
            if (parsed is null)
            {
                continue;
            }

            var (frontMatter, body) = parsed.Value;

            if (string.IsNullOrWhiteSpace(frontMatter.Title))
            {
                diagnostics.Error(location, "title is required");
                continue;
            }

            if (frontMatter.Draft)
            {
                log.WriteLine($"  SKIPPED: Draft subchapter '{frontMatter.Title}'");
                continue;
            }

            var questions = QuestionCompiler.Compile(frontMatter.Questions, location, diagnostics);
            var number = subchapters.Count + 1;
            var readingTime = FrontMatterParser.CalculateReadingTime(body);

            documents.Add(new SubchapterDocument
            {
                BookSlug = bookSlug,
                ChapterSlug = chapterSlug,
                Slug = entry.Slug,
                ChapterNumber = chapterNumber,
                Number = number,
                Title = frontMatter.Title,
                ReadingTimeMinutes = readingTime,
                Html = MarkdownProcessor.ToHtml(body),
                Questions = questions
            });

            subchapters.Add(new SubchapterToc
            {
                Number = number,
                Slug = entry.Slug,
                Title = frontMatter.Title,
                ReadingTimeMinutes = readingTime,
                QuestionIds = questions.Select(q => q.Id).ToArray()
            });
        }

        return subchapters;
    }

    private IEnumerable<NumberedEntry> ReadNumberedEntries(IEnumerable<string> paths, Func<string, string> nameSelector)
    {
        var entries = new List<NumberedEntry>();

        foreach (var path in paths.Where(p => !ContentNaming.IsHidden(p)))
        {
            var name = nameSelector(path);
            if (ContentNaming.TryParseNumbered(name, out var order, out var slug))
            {
                entries.Add(new NumberedEntry(order, slug, path));
            }
            else
            {
                diagnostics.Error(Relative(path), "name must look like '01-my-slug' (number, hyphen, lowercase slug)");
            }
        }

        return entries
            .OrderBy(e => e.Order)
            .ThenBy(e => e.Path, StringComparer.Ordinal);
    }

    private (SubchapterFrontMatter FrontMatter, string MarkdownBody)? ParseSubchapter(string path, string location)
    {
        try
        {
            return FrontMatterParser.Parse<SubchapterFrontMatter>(File.ReadAllText(path));
        }
        catch (YamlException ex)
        {
            diagnostics.Error(location, $"invalid front matter: {ex.Message}");
            return null;
        }
    }

    private T? ReadYaml<T>(string path) where T : class, new()
    {
        var location = Relative(path);

        if (!File.Exists(path))
        {
            diagnostics.Error(location, "file is missing");
            return null;
        }

        try
        {
            return FrontMatterParser.DeserializeYaml<T>(File.ReadAllText(path));
        }
        catch (YamlException ex)
        {
            diagnostics.Error(location, $"invalid YAML: {ex.Message}");
            return null;
        }
    }

    private string ResolveAuthorName(string authorId, string location)
    {
        if (string.IsNullOrEmpty(authorId))
        {
            return "";
        }

        if (authors.TryGetValue(authorId, out var profile))
        {
            return profile.Name;
        }

        diagnostics.Warning(location, $"no author profile found for '{authorId}'");
        return authorId;
    }

    private string Relative(string path) =>
        string.IsNullOrEmpty(_root) ? path : Path.GetRelativePath(_root, path).Replace('\\', '/');

    private sealed record NumberedEntry(int Order, string Slug, string Path);
}
