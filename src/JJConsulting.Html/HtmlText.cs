using System.IO;
using System.Text.Encodings.Web;

namespace JJConsulting.Html;

internal sealed class HtmlText(string rawText, bool encode) : IHtmlBuilder
{
    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        writer.Write(encode ? encoder.Encode(rawText) : rawText);
    }
}