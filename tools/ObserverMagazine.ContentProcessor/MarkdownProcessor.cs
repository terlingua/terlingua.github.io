using Markdig;

namespace ObserverMagazine.ContentProcessor;

public static class MarkdownProcessor
{
    private const string ParagraphOpen = "<p>";
    private const string ParagraphClose = "</p>";

    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public static string ToHtml(string markdown)
    {
        return Markdown.ToHtml(markdown, Pipeline);
    }

    public static string ToInlineHtml(string markdown)
    {
        var html = ToHtml(markdown).Trim();
        var isSingleParagraph =
            html.StartsWith(ParagraphOpen, StringComparison.Ordinal) &&
            html.EndsWith(ParagraphClose, StringComparison.Ordinal) &&
            html.IndexOf(ParagraphOpen, ParagraphOpen.Length, StringComparison.Ordinal) < 0;

        return isSingleParagraph
            ? html[ParagraphOpen.Length..^ParagraphClose.Length]
            : html;
    }
}
