using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace JJConsulting.Html.SourceGenerators;

[Generator]
public sealed class HtmlBuilderAppendExtensionsGenerator : IIncrementalGenerator
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
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace JJConsulting.Html.Extensions;");
            sb.AppendLine();
            sb.AppendLine("public static partial class HtmlBuilderAppendExtensions");
            sb.AppendLine("{");
            sb.AppendLine("    extension(HtmlBuilder htmlBuilder)");
            sb.AppendLine("    {");

            foreach (var field in fields!)
            {
                var name = field.Name;
                sb.AppendLine(
                    $"        public HtmlBuilder Append{name}(Action<HtmlBuilder>? builderAction = null) => htmlBuilder.Append(HtmlTag.{name}, builderAction);"
                );
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            spc.AddSource(
                "Extensions/HtmlBuilderAppendExtensions.g.cs",
                SourceText.From(sb.ToString(), Encoding.UTF8)
            );
        });
    }
}
