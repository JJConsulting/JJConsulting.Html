using JJConsulting.Html.Extensions;

namespace JJConsulting.Html.Tests;

public class HtmlBuilderChildrenExtensionsTests
{
    [Fact]
    public void AppendTag_ShouldAppendChild()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .Append(HtmlTag.Span, h => h.AppendText("x"));

        Assert.Equal("<div><span>x</span></div>", html.ToString());
    }

    [Fact]
    public void AppendStateful_ShouldAppendUsingState()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .Append(HtmlTag.Span, 10, static (value, h) => h.AppendText(value.ToString()));

        Assert.Equal("<div><span>10</span></div>", html.ToString());
    }

    [Fact]
    public void AppendDiv_ShouldAppendDivTag()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendDiv(h => h.AppendText("x"));

        Assert.Equal("<div><div>x</div></div>", html.ToString());
    }

    [Fact]
    public void AppendIf_True_ShouldAppend()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendIf(true, () => new HtmlBuilder("x"));

        Assert.Equal("<div>x</div>", html.ToString());
    }

    [Fact]
    public void AppendIf_False_ShouldNotAppend()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendIf(false, () => new HtmlBuilder("x"));

        Assert.Equal("<div></div>", html.ToString());
    }

    [Fact]
    public void AppendText_ShouldAddText()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendText("abc");

        Assert.Equal("<div>abc</div>", html.ToString());
    }
    
    [Fact]
    public void AppendText_ShouldAddTextEncoded()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendText("<span>abc</span>");

        Assert.Equal("<div>&lt;span&gt;abc&lt;/span&gt;</div>", html.ToString());
    }

    [Fact]
    public void AppendTextIf_ShouldAppendWhenTrue()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendTextIf(true, "x");

        Assert.Equal("<div>x</div>", html.ToString());
    }

    [Fact]
    public void AppendTextIf_ShouldNotAppendWhenFalse()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendTextIf(false, "x");

        Assert.Equal("<div></div>", html.ToString());
    }

    [Fact]
    public void AppendHiddenInput_ShouldAddInputWithIdAndValue()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendHiddenInput("myid", "val");

        Assert.Equal("<div><input hidden=\"hidden\" id=\"myid\" name=\"myid\" value=\"val\" /></div>", html.ToString());
    }

    [Fact]
    public void AppendScript_ShouldRenderScriptContent()
    {
        var html = new HtmlBuilder(HtmlTag.Div)
            .AppendScript("alert('x');");

        Assert.Equal("<div><script type=\"text/javascript\">alert('x');</script></div>", html.ToString());
    }
}