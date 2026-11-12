using System.Text;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace ObserverMagazine.ContentProcessor;

public sealed class SyntaxHighlightingExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is not HtmlRenderer html)
        {
            return;
        }

        var original = html.ObjectRenderers.FindExact<CodeBlockRenderer>();
        if (original is not null)
        {
            html.ObjectRenderers.Remove(original);
        }

        html.ObjectRenderers.Add(new HighlightingCodeBlockRenderer(original ?? new CodeBlockRenderer()));
    }

    private sealed class HighlightingCodeBlockRenderer(CodeBlockRenderer fallback) : HtmlObjectRenderer<CodeBlock>
    {
        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var language = (obj as FencedCodeBlock)?.Info ?? "";

            if (!renderer.EnableHtmlForBlock || fallback.BlocksAsDiv.Contains(language))
            {
                fallback.Write(renderer, obj);
                return;
            }

            var lines = obj.Lines;
            var code = new StringBuilder();
            for (var i = 0; i < lines.Count; i++)
            {
                code.Append(lines.Lines[i].Slice.ToString()).Append('\n');
            }

            renderer.EnsureLine();
            renderer.Write("<pre><code");
            renderer.WriteAttributes(obj);
            renderer.Write(">");
            renderer.Write(SyntaxHighlighter.Highlight(code.ToString(), language));
            renderer.WriteLine("</code></pre>");
        }
    }
}
