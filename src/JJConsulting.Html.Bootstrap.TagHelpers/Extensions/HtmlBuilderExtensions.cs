using JJConsulting.Html.Bootstrap.TagHelpers.Adapters;
using Microsoft.AspNetCore.Html;

namespace JJConsulting.Html.Bootstrap.TagHelpers.Extensions;

public static class HtmlBuilderExtensions
{
    extension(HtmlBuilder htmlBuilder)
    {
        public IHtmlContent ToHtmlContent()
        {
            return new HtmlContentAdapter(htmlBuilder);
        }
    }
}