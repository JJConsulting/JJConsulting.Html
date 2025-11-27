using System;

namespace JJConsulting.Html.Extensions;

public static class HtmlBuilderChildrenExtensions
{
    extension(HtmlBuilder htmlBuilder)
    {
        public HtmlBuilder Append(HtmlTag tag, Action<HtmlBuilder>? builderAction = null)
        {
            var child = new HtmlBuilder(tag);
            builderAction?.Invoke(child);
            htmlBuilder.Append(child);
            return htmlBuilder;
        }

        public HtmlBuilder AppendDiv(Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Div, builderAction);
        }


        public HtmlBuilder AppendSpan(Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Span, builderAction);
        }

        public HtmlBuilder AppendInput(Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Input, builderAction);
        }

        public HtmlBuilder AppendLabel(Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Label, builderAction);
        }


        public HtmlBuilder AppendHr()
        {
            return htmlBuilder.Append(HtmlTag.Hr);
        }

        public HtmlBuilder AppendBr()
        {
            var child = new HtmlBuilder(HtmlTag.Br);
            htmlBuilder.Append(child);
            return htmlBuilder;
        }

        public HtmlBuilder AppendLink(string text, string link)
        {
            var child = new HtmlBuilder(HtmlTag.A)
                .AppendText(text)
                .WithAttribute("href", link);

            htmlBuilder.Append(child);

            return htmlBuilder;
        }

        public HtmlBuilder AppendIf(bool condition, Func<HtmlBuilder> func)
        {
            if (condition)
                htmlBuilder.Append(func.Invoke());

            return htmlBuilder;
        }

        public HtmlBuilder AppendIf(bool condition, HtmlTag tag, Action<HtmlBuilder>? builderAction = null)
        {
            if (condition)
                htmlBuilder.Append(tag, builderAction);

            return htmlBuilder;
        }

        public HtmlBuilder AppendText(string? rawText)
        {
            if (!string.IsNullOrEmpty(rawText))
            {
                var child = new HtmlBuilder(rawText!);
                htmlBuilder.Append(child);
            }

            return htmlBuilder;
        }

        public HtmlBuilder AppendTextIf(bool condition, string? rawText)
        {
            if (condition)
                htmlBuilder.AppendText(rawText);

            return htmlBuilder;
        }

        public HtmlBuilder AppendHiddenInput(string name, string value)
        {
            var input = new HtmlBuilder(HtmlTag.Input);
            input.WithAttribute("hidden", "hidden");
            input.WithNameAndId(name);
            input.WithValue(value);

            return htmlBuilder.Append(input);
        }

        public HtmlBuilder AppendHiddenInput(string name)
        {
            return htmlBuilder.AppendHiddenInput(name, string.Empty);
        }

        public HtmlBuilder AppendStyle(string rawCss)
        {
            var child = new HtmlBuilder(HtmlTag.Style)
                .Append(new HtmlBuilder(rawCss, encode: false));

            return htmlBuilder.Append(child);
        }

        public HtmlBuilder AppendStyleIf(bool condition, string rawCss)
        {
            if (!condition)
                return htmlBuilder;

            return htmlBuilder.AppendStyle(rawCss);
        }

        public HtmlBuilder AppendScript(string rawScript)
        {
            var child = new HtmlBuilder(HtmlTag.Script)
                .WithAttribute("type", "text/javascript")
                .Append(new HtmlBuilder(rawScript, encode: false));

            return htmlBuilder.Append(child);
        }

        public HtmlBuilder AppendScriptIf(bool condition, string rawScript)
        {
            if (!condition)
                return htmlBuilder;

            return htmlBuilder.AppendScript(rawScript);
        }
    }
}