using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public interface IBookService
{
    Task<BookSummary[]> GetBooksAsync();
    Task<BookSummary?> GetBookAsync(string bookSlug);
    Task<BookToc?> GetTocAsync(string bookSlug);
    Task<SubchapterContent?> GetSubchapterAsync(string bookSlug, string chapterSlug, string subchapterSlug);
}
