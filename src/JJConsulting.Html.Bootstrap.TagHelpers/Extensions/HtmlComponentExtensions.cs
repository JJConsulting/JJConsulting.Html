using JJConsulting.Html.Bootstrap.Abstractions;
using Microsoft.AspNetCore.Html;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Extensions;

public static class HtmlComponentExtensions
{
    extension(HtmlComponent component)
    {
        public IHtmlContent GetHtmlContent()
        {
            return component.GetHtmlBuilder().ToHtmlContent();
        }
    }
}