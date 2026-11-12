namespace ObserverMagazine.Web.Services;

public static class HtmlPostProcessor
{
    private const string TableWrapOpen = "<div class=\"table-scroll-wrap\"><table";

    public static string Process(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return "";
        }

        return html
            .Replace("<table", TableWrapOpen, StringComparison.Ordinal)
            .Replace("</table>", "</table></div>", StringComparison.Ordinal);
    }
}
