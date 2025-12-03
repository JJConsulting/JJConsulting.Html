using System.IO;
using System.Text.Encodings.Web;

namespace JJConsulting.Html;

public sealed class HtmlText(string rawText, bool encode = true) : IHtmlBuilder
{
    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        writer.Write(encode ? encoder.Encode(rawText) : rawText);
    }
}