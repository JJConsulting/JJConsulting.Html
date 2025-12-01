using System.Text.Encodings.Web;
using JJConsulting.Html.Bootstrap.Abstractions;
using Microsoft.AspNetCore.Html;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Adapters;

internal sealed class HtmlContentAdapter(HtmlComponent component) : IHtmlContent
{
    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        component.WriteTo(writer, encoder);
    }
}