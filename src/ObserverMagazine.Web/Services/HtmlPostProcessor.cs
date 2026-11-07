namespace ObserverMagazine.Web.Services;

public static class HtmlPostProcessor
{
    private const string CodeWrapOpen =
        "<div class=\"code-block-wrap\"><span class=\"code-scroll-hint\" aria-hidden=\"true\">scroll →</span><pre";

    private const string TableWrapOpen = "<div class=\"table-scroll-wrap\"><table";

    public static string Process(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return "";
        }

        return html
            .Replace("<pre", CodeWrapOpen, StringComparison.Ordinal)
            .Replace("</pre>", "</pre></div>", StringComparison.Ordinal)
            .Replace("<table", TableWrapOpen, StringComparison.Ordinal)
            .Replace("</table>", "</table></div>", StringComparison.Ordinal);
    }
}
