using JJConsulting.FontAwesome;
using JJConsulting.Html.Bootstrap.Abstractions;
using JJConsulting.Html.Bootstrap.Models;

namespace JJConsulting.Html.Bootstrap.Components;

/// <summary>
/// Represents a <see cref="JJAlert"/> with error messages.
/// </summary>
public class JJValidationSummary : HtmlComponent
{
    public List<string> Errors { get; }

    public string? Title { get; set; }

    /// <summary>
    /// Enable close panel
    /// (Default = True)
    /// </summary>
    public bool ShowCloseButton { get; set; }

    public JJValidationSummary()
    {
        Visible = true;
        Errors = [];
        ShowCloseButton = true;
    }

    public void SetErrors(Dictionary<string, string>? errors)
    {
        if (errors == null)
            return;

        foreach (var err in errors)
        {
            Errors.Add(err.Value);
        }
    }

    protected override HtmlBuilder BuildHtml()
    {
        var alert = new JJAlert
        {
            Color = BootstrapColor.Danger,
            Icon = FontAwesomeIcon.ExclamationTriangle,
            Title = Title,
            ShowCloseButton = ShowCloseButton,
        };

        alert.Messages.AddRange(Errors);

        return alert.GetHtmlBuilder();
    }
}