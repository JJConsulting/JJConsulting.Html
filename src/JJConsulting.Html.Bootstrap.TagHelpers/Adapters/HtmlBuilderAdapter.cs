using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Adapters;

internal sealed class HtmlBuilderAdapter(IHtmlContent htmlContent) : IHtmlBuilder
{
    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        htmlContent.WriteTo(writer, encoder);
    }
}