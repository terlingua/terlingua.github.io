using System.Text.Json;
using ObserverMagazine.ContentProcessor;
using ObserverMagazine.ContentProcessor.Books;
using Xunit;

namespace ObserverMagazine.Integration.Tests;

public sealed class BookCompilerTests : IDisposable
{
    private static readonly DateTime PublishBefore = new(2026, 12, 31);

    private readonly string _root = Path.Combine(Path.GetTempPath(), "book-tests-" + Guid.NewGuid().ToString("N"));
    private string BooksDir => Path.Combine(_root, "books");

    private static readonly Dictionary<string, AuthorProfile> Authors = new(StringComparer.OrdinalIgnoreCase)
    {
        ["team"] = new AuthorProfile { Id = "team", Name = "The Team" }
    };

    private const string ValidSubchapter = """
        ---
        title: Hello
        questions:
          - id: pick
            prompt: Pick B
            options:
              - text: A
              - text: B
                correct: true
        ---
        Some **content** here.
        """;

    private void WriteFile(string relativePath, string content)
    {
        var path = Path.Combine(BooksDir, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
    }

    private void WriteValidBook(string slug = "my-book", string date = "2026-01-01")
    {
        WriteFile($"{slug}/book.yml", $"title: My Book\nauthor: team\ndate: {date}\nsummary: A book\ntags: [one]\n");
        WriteFile($"{slug}/02-second/chapter.yml", "title: Second\n");
        WriteFile($"{slug}/02-second/01-only.md", "---\ntitle: Only\n---\nText.\n");
        WriteFile($"{slug}/01-first/chapter.yml", "title: First\nsummary: Start here\n");
        WriteFile($"{slug}/01-first/10-later.md", "---\ntitle: Later\n---\nLater text.\n");
        WriteFile($"{slug}/01-first/02-hello.md", ValidSubchapter);
    }

    private (IReadOnlyList<CompiledBook> Books, BuildDiagnostics Diagnostics) Compile()
    {
        var diagnostics = new BuildDiagnostics();
        var books = new BookCompiler(Authors, PublishBefore, diagnostics, TextWriter.Null).Compile(BooksDir);
        return (books, diagnostics);
    }

    [Fact]
    public void MissingBooksDirectory_ReturnsNoBooks()
    {
        var (books, diagnostics) = Compile();

        Assert.Empty(books);
        Assert.False(diagnostics.HasErrors);
    }

    [Fact]
    public void ValidBook_IsCompiledWithSequentialNumbering()
    {
        WriteValidBook();

        var (books, diagnostics) = Compile();

        Assert.False(diagnostics.HasErrors, string.Join("\n", diagnostics.Errors));
        var book = Assert.Single(books);
        Assert.Equal("my-book", book.Index.Slug);
        Assert.Equal("The Team", book.Index.AuthorName);
        Assert.Equal(2, book.Index.ChapterCount);
        Assert.Equal(3, book.Index.SubchapterCount);
        Assert.Equal(1, book.Index.QuestionCount);

        var first = book.Toc.Chapters[0];
        Assert.Equal((1, "first"), (first.Number, first.Slug));
        Assert.Equal(["hello", "later"], first.Subchapters.Select(s => s.Slug).ToArray());
        Assert.Equal([1, 2], first.Subchapters.Select(s => s.Number).ToArray());
        Assert.Equal(["pick"], first.Subchapters[0].QuestionIds);
        Assert.Equal((2, "second"), (book.Toc.Chapters[1].Number, book.Toc.Chapters[1].Slug));

        var hello = book.Documents.Single(d => d.Slug == "hello");
        Assert.Contains("<strong>content</strong>", hello.Html);
        Assert.Equal(1, hello.ChapterNumber);
        Assert.Equal("b", hello.Questions[0].Options.Single(o => o.Correct).Id);
    }

    [Fact]
    public void DraftAndFutureContent_IsSkipped()
    {
        WriteValidBook();
        WriteFile("my-book/01-first/03-draft.md", "---\ntitle: Draft\ndraft: true\n---\nHidden.\n");
        WriteFile("future-book/book.yml", "title: Future\ndate: 2099-01-01\n");
        WriteFile("future-book/01-a/chapter.yml", "title: A\n");
        WriteFile("future-book/01-a/01-b.md", "---\ntitle: B\n---\nB.\n");
        WriteFile("draft-book/book.yml", "title: Draft\ndate: 2026-01-01\ndraft: true\n");

        var (books, diagnostics) = Compile();

        Assert.False(diagnostics.HasErrors);
        var book = Assert.Single(books);
        Assert.Equal(3, book.Index.SubchapterCount);
    }

    [Fact]
    public void UnnumberedSubchapter_IsError()
    {
        WriteValidBook();
        WriteFile("my-book/01-first/no-number.md", "---\ntitle: Oops\n---\nText.\n");

        var (_, diagnostics) = Compile();

        Assert.Contains(diagnostics.Errors, e => e.Contains("no-number.md"));
    }

    [Fact]
    public void MissingChapterManifest_IsError()
    {
        WriteValidBook();
        WriteFile("my-book/03-third/01-x.md", "---\ntitle: X\n---\nX.\n");

        var (_, diagnostics) = Compile();

        Assert.Contains(diagnostics.Errors, e => e.Contains("chapter.yml") && e.Contains("missing"));
    }

    [Fact]
    public void InvalidQuestion_IsError()
    {
        WriteValidBook();
        WriteFile("my-book/01-first/04-bad.md", """
            ---
            title: Bad
            questions:
              - prompt: No correct answer
                options:
                  - text: A
                  - text: B
            ---
            Body.
            """);

        var (_, diagnostics) = Compile();

        Assert.Contains(diagnostics.Errors, e => e.Contains("04-bad.md"));
    }

    [Fact]
    public void MalformedYaml_IsErrorNotCrash()
    {
        WriteValidBook();
        WriteFile("my-book/01-first/05-broken.md", "---\ntitle: [unclosed\n---\nBody.\n");

        var (_, diagnostics) = Compile();

        Assert.Contains(diagnostics.Errors, e => e.Contains("05-broken.md"));
    }

    [Fact]
    public void UnknownAuthor_IsWarning()
    {
        WriteValidBook();
        WriteFile("my-book/book.yml", "title: My Book\nauthor: nobody\ndate: 2026-01-01\n");

        var (books, diagnostics) = Compile();

        Assert.False(diagnostics.HasErrors);
        Assert.Contains(diagnostics.Warnings, w => w.Contains("nobody"));
        Assert.Equal("nobody", books[0].Index.AuthorName);
    }

    [Fact]
    public void Books_AreSortedNewestFirst()
    {
        WriteValidBook("old-book", "2026-01-01");
        WriteValidBook("new-book", "2026-06-01");

        var (books, _) = Compile();

        Assert.Equal(["new-book", "old-book"], books.Select(b => b.Index.Slug).ToArray());
    }

    [Fact]
    public void Writer_ProducesExpectedFileLayout()
    {
        WriteValidBook();
        var (books, _) = Compile();
        var outputDir = Path.Combine(_root, "out");
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        Directory.CreateDirectory(Path.Combine(outputDir, BookOutputWriter.DataDirectoryName, "stale"));
        var dataDir = BookOutputWriter.Write(books, outputDir, jsonOptions);

        Assert.False(Directory.Exists(Path.Combine(dataDir, "stale")));
        Assert.True(File.Exists(Path.Combine(dataDir, "books-index.json")));
        Assert.True(File.Exists(Path.Combine(dataDir, "my-book", "toc.json")));
        Assert.True(File.Exists(Path.Combine(dataDir, "my-book", "first", "hello.json")));
        Assert.True(File.Exists(Path.Combine(dataDir, "my-book", "second", "only.json")));

        using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, "my-book", "first", "hello.json")));
        var question = doc.RootElement.GetProperty("questions")[0];
        Assert.Equal("pick", question.GetProperty("id").GetString());
        Assert.False(question.GetProperty("multipleAnswers").GetBoolean());
        Assert.True(question.GetProperty("options")[1].GetProperty("correct").GetBoolean());
    }

    [Fact]
    public void FrontMatterParser_GenericParse_IgnoresDashesInsideValues()
    {
        var (frontMatter, body) = FrontMatterParser.Parse<SubchapterFrontMatter>("---\ntitle: a---b\n---\nBody\n");

        Assert.Equal("a---b", frontMatter.Title);
        Assert.Equal("Body\n", body);
    }

    [Theory]
    [InlineData("01-intro", true, 1, "intro")]
    [InlineData("12_multi-word-slug", true, 12, "multi-word-slug")]
    [InlineData("intro", false, 0, "")]
    [InlineData("01-Upper", false, 0, "")]
    [InlineData("01-", false, 0, "")]
    public void ContentNaming_ParsesNumberedNames(string name, bool expected, int order, string slug)
    {
        var success = ContentNaming.TryParseNumbered(name, out var actualOrder, out var actualSlug);

        Assert.Equal(expected, success);
        Assert.Equal(order, actualOrder);
        Assert.Equal(slug, actualSlug);
    }

    public void Dispose()
    {
        if (Directory.Exists(_root))
        {
            Directory.Delete(_root, recursive: true);
        }
    }
}
