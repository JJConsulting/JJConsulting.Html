using JJConsulting.Html.Bootstrap.Abstractions;
using JJConsulting.Html.Bootstrap.TagHelpers.Adapters;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Extensions;

public static class TagHelperContentExtensions
{
    extension(TagHelperContent content)
    {
        public void SetHtmlContent(HtmlBuilder html)
        {
            content.SetHtmlContent(new HtmlContentAdapter(html));
        }

        public void SetHtmlContent(HtmlComponent component)
        {
            content.SetHtmlContent(new HtmlContentAdapter(component.GetHtmlBuilder()));
        }
    }
}