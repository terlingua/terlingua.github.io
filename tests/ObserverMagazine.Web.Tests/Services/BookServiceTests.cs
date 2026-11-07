using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class BookServiceTests
{
    private const string IndexJson = """
        [
          { "slug": "older", "title": "Older", "date": "2026-01-01T00:00:00", "chapterCount": 1 },
          { "slug": "newer", "title": "Newer", "date": "2026-06-01T00:00:00", "chapterCount": 2 }
        ]
        """;

    private const string TocJson = """
        {
          "slug": "newer",
          "title": "Newer",
          "chapters": [
            { "number": 1, "slug": "intro", "title": "Intro",
              "subchapters": [ { "number": 1, "slug": "hello", "title": "Hello", "questionIds": ["q1"] } ] }
          ]
        }
        """;

    private const string SubchapterJson = """
        {
          "bookSlug": "newer", "chapterSlug": "intro", "slug": "hello", "title": "Hello",
          "chapterNumber": 1, "number": 1, "html": "<p>Hi</p>",
          "questions": [
            { "id": "q1", "promptHtml": "<p>?</p>", "multipleAnswers": true,
              "options": [ { "id": "a", "textHtml": "A", "correct": true }, { "id": "b", "textHtml": "B" } ] }
          ]
        }
        """;

    private static (BookService Service, RoutingHttpHandler Handler) Create(Dictionary<string, string> routes)
    {
        var handler = new RoutingHttpHandler(routes);
        var http = new HttpClient(handler) { BaseAddress = new Uri("https://test.local/") };
        return (new BookService(http, NullLogger<BookService>.Instance), handler);
    }

    [Fact]
    public async Task GetBooksAsync_ReturnsBooksNewestFirst()
    {
        var (service, _) = Create(new() { ["/book-data/books-index.json"] = IndexJson });

        var books = await service.GetBooksAsync();

        Assert.Equal(["newer", "older"], books.Select(b => b.Slug).ToArray());
    }

    [Fact]
    public async Task GetBooksAsync_MissingIndex_ReturnsEmptyAndRetriesLater()
    {
        var (service, handler) = Create(new());

        Assert.Empty(await service.GetBooksAsync());
        Assert.Empty(await service.GetBooksAsync());
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task GetBooksAsync_InvalidJson_ReturnsEmpty()
    {
        var (service, _) = Create(new() { ["/book-data/books-index.json"] = "<html>not json</html>" });

        Assert.Empty(await service.GetBooksAsync());
    }

    [Fact]
    public async Task GetTocAsync_IsCached()
    {
        var (service, handler) = Create(new() { ["/book-data/newer/toc.json"] = TocJson });

        var first = await service.GetTocAsync("newer");
        var second = await service.GetTocAsync("NEWER");

        Assert.NotNull(first);
        Assert.Same(first, second);
        Assert.Single(handler.Requests);
        Assert.Equal(["q1"], first.Chapters[0].Subchapters[0].QuestionIds);
    }

    [Fact]
    public async Task GetSubchapterAsync_DeserializesQuestions()
    {
        var (service, _) = Create(new() { ["/book-data/newer/intro/hello.json"] = SubchapterJson });

        var content = await service.GetSubchapterAsync("newer", "intro", "hello");

        Assert.NotNull(content);
        var question = Assert.Single(content.Questions);
        Assert.True(question.MultipleAnswers);
        Assert.Equal(2, question.Options.Length);
    }

    [Theory]
    [InlineData("../secrets")]
    [InlineData("has space")]
    [InlineData("")]
    public async Task InvalidSlugs_ReturnNullWithoutRequests(string slug)
    {
        var (service, handler) = Create(new());

        Assert.Null(await service.GetTocAsync(slug));
        Assert.Null(await service.GetSubchapterAsync("newer", slug, "hello"));
        Assert.Empty(handler.Requests);
    }
}

internal sealed class RoutingHttpHandler(Dictionary<string, string> routes) : HttpMessageHandler
{
    public List<string> Requests { get; } = [];

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var path = request.RequestUri?.AbsolutePath ?? "";
        Requests.Add(path);

        var response = routes.TryGetValue(path, out var body)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json")
            }
            : new HttpResponseMessage(HttpStatusCode.NotFound);

        return Task.FromResult(response);
    }
}
