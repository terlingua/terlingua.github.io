using ObserverMagazine.ContentProcessor;
using Xunit;

namespace ObserverMagazine.Integration.Tests;

public class SyntaxHighlighterTests
{
    [Theory]
    [InlineData("csharp", "var name = $\"Hi {x}\"; // greet\nList<int> items = [1, 2];",
        "<span class=\"hl-keyword\">var</span> name = <span class=\"hl-string\">$\"Hi {x}\"</span>; <span class=\"hl-comment\">// greet</span>\n<span class=\"hl-type\">List</span>&lt;<span class=\"hl-keyword\">int</span>&gt; items = [<span class=\"hl-number\">1</span>, <span class=\"hl-number\">2</span>];")]
    [InlineData("json", "{\"id\": 1, \"ok\": true}",
        "{<span class=\"hl-attr\">\"id\"</span>: <span class=\"hl-number\">1</span>, <span class=\"hl-attr\">\"ok\"</span>: <span class=\"hl-literal\">true</span>}")]
    [InlineData("yml", "title: Hello\ndraft: false",
        "<span class=\"hl-attr\">title</span>: Hello\n<span class=\"hl-attr\">draft</span>: <span class=\"hl-literal\">false</span>")]
    [InlineData("html", "<a href=\"/x\">Go</a>",
        "&lt;<span class=\"hl-tag\">a</span> <span class=\"hl-attr\">href</span>=<span class=\"hl-string\">\"/x\"</span>&gt;Go&lt;/<span class=\"hl-tag\">a</span>&gt;")]
    [InlineData("css", ".a { color: #fff; }",
        "<span class=\"hl-selector\">.a </span>{ <span class=\"hl-property\">color</span>: <span class=\"hl-number\">#fff</span>; }")]
    [InlineData("sql", "select * from Users -- all",
        "<span class=\"hl-keyword\">select</span> * <span class=\"hl-keyword\">from</span> <span class=\"hl-type\">Users</span> <span class=\"hl-comment\">-- all</span>")]
    [InlineData("", "if (a < b && c > d) {}",
        "<span class=\"hl-function\">if</span> (a &lt; b &amp;&amp; c &gt; d) {}")]
    public void Highlights(string language, string code, string expected)
    {
        Assert.Equal(expected, SyntaxHighlighter.Highlight(code, language));
    }

    [Fact]
    public void FencedCodeIsHighlightedAtBuildTime()
    {
        var html = MarkdownProcessor.ToHtml("```csharp\nvar x = 1;\n```");

        Assert.Contains("<pre><code class=\"language-csharp\">", html);
        Assert.Contains("<span class=\"hl-keyword\">var</span>", html);
        Assert.Contains("</code></pre>", html);
    }

    [Fact]
    public void MarkupInCodeIsEscaped()
    {
        var html = MarkdownProcessor.ToHtml("```html\n<script>alert(1)</script>\n```");

        Assert.DoesNotContain("<script>", html);
        Assert.Contains("&lt;<span class=\"hl-tag\">script</span>&gt;", html);
    }
}
