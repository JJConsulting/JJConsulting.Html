using JJConsulting.Html.Bootstrap.Abstractions;
using JJConsulting.Html.Bootstrap.TagHelpers.Adapters;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Extensions;

public static class TagHelperContentExtensions
{
    public static void SetHtmlContent(this TagHelperContent content, HtmlComponent component)
    {
        content.SetHtmlContent(new HtmlContentAdapter(component));
    }
}