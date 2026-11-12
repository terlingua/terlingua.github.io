using ObserverMagazine.Web.Services;
using Xunit;

namespace ObserverMagazine.Web.Tests.Services;

public class HtmlPostProcessorTests
{
    [Fact]
    public void WrapsTables()
    {
        var html = HtmlPostProcessor.Process("<table><tr><td>1</td></tr></table>");

        Assert.Equal("<div class=\"table-scroll-wrap\"><table><tr><td>1</td></tr></table></div>", html);
    }

    [Fact]
    public void LeavesCodeBlocksUntouched()
    {
        const string code = "<pre><code class=\"language-csharp\"><span class=\"hl-keyword\">var</span></code></pre>";

        Assert.Equal(code, HtmlPostProcessor.Process(code));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void EmptyInput_ReturnsEmpty(string? html)
    {
        Assert.Equal("", HtmlPostProcessor.Process(html));
    }
}
