using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace JJConsulting.Html.SourceGenerators;

[Generator]
public sealed class HtmlBuilderTagExtensionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var htmlTagProvider = context.CompilationProvider
            .Select((c, _) => c.GetTypeByMetadataName("JJConsulting.Html.HtmlTag"));

        var fieldsProvider = htmlTagProvider.Select((tag, _) =>
            tag!.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(f => f.IsStatic && f.Type.Name == "HtmlTag")
                .ToList()
        );

        context.RegisterSourceOutput(fieldsProvider, (spc, fields) =>
        {
            var sb = new StringBuilder();

            sb.AppendLine("#nullable enable");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Microsoft.AspNetCore.Html;");
            sb.AppendLine();
            sb.AppendLine("namespace JJConsulting.Html.Extensions;");
            sb.AppendLine();
            sb.AppendLine("public static class HtmlBuilderTagExtensions");
            sb.AppendLine("{");
            sb.AppendLine("    extension(HtmlBuilder)");
            sb.AppendLine("    {");

            foreach (var field in fields!)
            {
                var name = field.Name;
                sb.AppendLine($"        public static HtmlBuilder {name}() => new(HtmlTag.{name});");
                sb.AppendLine($"        public static HtmlBuilder {name}(params List<IHtmlContent?> children) => new(HtmlTag.{name}, children);");
                sb.AppendLine($"        public static HtmlBuilder {name}(string rawText) => new(HtmlTag.{name}, rawText);");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            spc.AddSource(
                "Extensions/HtmlBuilderTagExtensions.g.cs",
                SourceText.From(sb.ToString(), Encoding.UTF8)
            );
        });
    }
}