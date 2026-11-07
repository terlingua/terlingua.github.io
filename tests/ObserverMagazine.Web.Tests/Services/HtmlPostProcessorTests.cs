using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class HtmlPostProcessorTests
{
    [Fact]
    public void WrapsCodeBlocksAndTables()
    {
        var html = HtmlPostProcessor.Process("<pre><code>x</code></pre><table><tr><td>1</td></tr></table>");

        Assert.Contains("<div class=\"code-block-wrap\">", html);
        Assert.Contains("</pre></div>", html);
        Assert.Contains("<div class=\"table-scroll-wrap\"><table", html);
        Assert.Contains("</table></div>", html);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void EmptyInput_ReturnsEmpty(string? html)
    {
        Assert.Equal("", HtmlPostProcessor.Process(html));
    }
}
