using JJConsulting.FontAwesome;

namespace JJConsulting.Html.Bootstrap.Models;

public class NavContent
{
    public required string Title { get; set; }
    public required FontAwesomeIcon? Icon { get; set; }
    public IHtmlBuilder? Content { get; set; }
}
