using ObserverMagazine.Web.Models;

namespace ObserverMagazine.Web.Services;

public static class BookNavigator
{
    public static IReadOnlyList<SubchapterLocation> Flatten(BookToc toc) =>
        toc.Chapters
            .SelectMany(chapter => chapter.Subchapters.Select(sub => new SubchapterLocation(chapter, sub)))
            .ToArray();

    public static SubchapterLocation? Find(BookToc toc, string chapterSlug, string subchapterSlug) =>
        Flatten(toc).FirstOrDefault(l =>
            string.Equals(l.Chapter.Slug, chapterSlug, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(l.Subchapter.Slug, subchapterSlug, StringComparison.OrdinalIgnoreCase));

    public static SubchapterLocation? First(BookToc toc) => Flatten(toc).FirstOrDefault();

    public static SubchapterNeighbors GetNeighbors(BookToc toc, SubchapterLocation current)
    {
        var all = Flatten(toc);
        var index = IndexOf(all, current);
        if (index < 0)
        {
            return new SubchapterNeighbors(null, null);
        }

        return new SubchapterNeighbors(
            index > 0 ? all[index - 1] : null,
            index < all.Count - 1 ? all[index + 1] : null);
    }

    internal static int IndexOf(IReadOnlyList<SubchapterLocation> all, SubchapterLocation location)
    {
        for (var i = 0; i < all.Count; i++)
        {
            if (all[i].ProgressKey == location.ProgressKey)
            {
                return i;
            }
        }
        return -1;
    }
}
