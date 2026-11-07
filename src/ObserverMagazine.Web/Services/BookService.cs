using System.Net.Http.Json;
using System.Text.Json;
using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public sealed class BookService(HttpClient http, ILogger<BookService> logger) : IBookService
{
    public const string DataRoot = "book-data";

    private readonly Dictionary<string, BookToc> _tocCache = new(StringComparer.Ordinal);
    private BookSummary[]? _indexCache;

    public async Task<BookSummary[]> GetBooksAsync()
    {
        if (_indexCache is not null)
        {
            return _indexCache;
        }

        var books = await FetchAsync<BookSummary[]>($"{DataRoot}/books-index.json");
        if (books is null)
        {
            return [];
        }

        _indexCache = books.OrderByDescending(b => b.Date).ToArray();
        logger.LogInformation("Loaded {Count} books", _indexCache.Length);
        return _indexCache;
    }

    public async Task<BookSummary?> GetBookAsync(string bookSlug)
    {
        if (!ContentSlug.TryNormalize(bookSlug, out var slug))
        {
            return null;
        }

        var books = await GetBooksAsync();
        return books.FirstOrDefault(b => string.Equals(b.Slug, slug, StringComparison.Ordinal));
    }

    public async Task<BookToc?> GetTocAsync(string bookSlug)
    {
        if (!ContentSlug.TryNormalize(bookSlug, out var slug))
        {
            return null;
        }

        if (_tocCache.TryGetValue(slug, out var cached))
        {
            return cached;
        }

        var toc = await FetchAsync<BookToc>($"{DataRoot}/{slug}/toc.json");
        if (toc is not null)
        {
            _tocCache[slug] = toc;
        }
        return toc;
    }

    public async Task<SubchapterContent?> GetSubchapterAsync(string bookSlug, string chapterSlug, string subchapterSlug)
    {
        if (!ContentSlug.TryNormalize(bookSlug, out var book) ||
            !ContentSlug.TryNormalize(chapterSlug, out var chapter) ||
            !ContentSlug.TryNormalize(subchapterSlug, out var subchapter))
        {
            return null;
        }

        return await FetchAsync<SubchapterContent>($"{DataRoot}/{book}/{chapter}/{subchapter}.json");
    }

    private async Task<T?> FetchAsync<T>(string path) where T : class
    {
        try
        {
            return await http.GetFromJsonAsync<T>(path);
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Book data not available at {Path}", path);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Book data at {Path} is not valid JSON", path);
        }
        catch (NotSupportedException ex)
        {
            logger.LogError(ex, "Book data at {Path} has an unsupported content type", path);
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Timed out loading book data at {Path}", path);
        }
        return null;
    }
}
