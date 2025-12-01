using JJConsulting.Html.Bootstrap.Abstractions;
using JJConsulting.Html.Bootstrap.Models;
using JJConsulting.Html.Extensions;

namespace JJConsulting.Html.Bootstrap.Components;

public sealed class JJToolbar : HtmlComponent
{
    public List<HtmlBuilder?> Items { get; set; }

    public JJToolbar()
    {
        Items = [];
    }

    internal override HtmlBuilder BuildHtml()
    {
        var html = HtmlBuilder.Div()
            .WithNameAndId(Name)
            .WithAttributes(Attributes)
            .WithCssClass(CssClass)
            .AppendDiv(row =>
            {
                row.WithCssClass("row");
                row.Append(GetHtmlCol());
            });

        return html;
    }

    private HtmlBuilder GetHtmlCol()
    {
        var div = new HtmlBuilder(HtmlTag.Div)
            .WithCssClass("col-sm-12");
        
        for (var i = 0; i < Items.Count; i++)
        {
            var htmlBuilder = Items[i];
            if (htmlBuilder == null)
                continue;

            if (i != 0)
                htmlBuilder.WithStyle("margin-right: 3px;");

            div.Append(htmlBuilder);
        }

        return div;
    }
}
