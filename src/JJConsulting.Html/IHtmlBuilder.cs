using System.IO;
using System.Text.Encodings.Web;

namespace JJConsulting.Html;

public interface IHtmlBuilder
{
    public void WriteTo(TextWriter writer, HtmlEncoder encoder);
}
