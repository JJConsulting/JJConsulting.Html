using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Adapters;

public sealed class HtmlContentAdapter(HtmlBuilder htmlBuilder) : IHtmlContent
{
    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        htmlBuilder.WriteTo(writer, encoder);
    }
}