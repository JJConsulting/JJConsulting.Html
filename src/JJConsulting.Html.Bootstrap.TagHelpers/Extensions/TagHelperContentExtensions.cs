using JJConsulting.Html.Bootstrap.Abstractions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Extensions;

public static class TagHelperContentExtensions
{
    extension(TagHelperContent content)
    {
        public void SetHtmlContent(HtmlBuilder html)
        {
            content.SetHtmlContent(html.ToHtmlContent());
        }

        public void SetHtmlContent(HtmlComponent component)
        {
            content.SetHtmlContent(component.GetHtmlContent());
        }
    }
}