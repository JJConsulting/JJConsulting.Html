using System;
using System.Web;
using JetBrains.Annotations;

namespace JJConsulting.Html.Extensions;

#nullable enable

[PublicAPI]
public static class HtmlBuilderChildrenExtensions
{
    extension(HtmlBuilder htmlBuilder)
    {
        public HtmlBuilder Append(HtmlTag tag, [InstantHandle] Action<HtmlBuilder>? builderAction = null)
        {
            var child = new HtmlBuilder(tag);
            builderAction?.Invoke(child);
            htmlBuilder.Append(child);
            return htmlBuilder;
        }

        public HtmlBuilder Append<TState>(HtmlTag tag,
            TState state,
            [InstantHandle, RequireStaticDelegate] Action<TState, HtmlBuilder> builderAction)
        {
            var child = new HtmlBuilder(tag);
            builderAction(state, child);
            htmlBuilder.Append(child);
            return htmlBuilder;
        }

        public HtmlBuilder AppendDiv([InstantHandle] Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Div, builderAction);
        }

        public HtmlBuilder AppendDiv<TState>(TState state,
            [InstantHandle, RequireStaticDelegate] Action<TState, HtmlBuilder> builderAction)
        {
            return htmlBuilder.Append(HtmlTag.Div, state, builderAction);
        }

        public HtmlBuilder AppendSpan([InstantHandle] Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Span, builderAction);
        }

        public HtmlBuilder AppendSpan<TState>(TState state,
            [InstantHandle, RequireStaticDelegate] Action<TState, HtmlBuilder> builderAction)
        {
            return htmlBuilder.Append(HtmlTag.Span, state, builderAction);
        }

        public HtmlBuilder AppendInput([InstantHandle] Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Input, builderAction);
        }

        public HtmlBuilder AppendInput<TState>(TState state,
            [InstantHandle, RequireStaticDelegate] Action<TState, HtmlBuilder> builderAction)
        {
            return htmlBuilder.Append(HtmlTag.Input, state, builderAction);
        }

        public HtmlBuilder AppendLabel([InstantHandle] Action<HtmlBuilder>? builderAction = null)
        {
            return htmlBuilder.Append(HtmlTag.Label, builderAction);
        }

        public HtmlBuilder AppendLabel<TState>(TState state,
            [InstantHandle, RequireStaticDelegate] Action<TState, HtmlBuilder> builderAction)
        {
            return htmlBuilder.Append(HtmlTag.Label, state, builderAction);
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

        public HtmlBuilder AppendIf(bool condition, [InstantHandle] Func<HtmlBuilder> func)
        {
            if (condition)
                htmlBuilder.Append(func.Invoke());

            return htmlBuilder;
        }

        public HtmlBuilder AppendIf(bool condition, HtmlTag tag, [InstantHandle] Action<HtmlBuilder>? builderAction = null)
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

        public HtmlBuilder AppendStyle([LanguageInjection("css")] string rawCss)
        {
            var child = new HtmlBuilder(HtmlTag.Style)
                .Append(new HtmlBuilder(rawCss, encode:false));

            return htmlBuilder.Append(child);
        }

        public HtmlBuilder AppendStyleIf(bool condition, [LanguageInjection("css")] string rawCss)
        {
            if (!condition)
                return htmlBuilder;

            return htmlBuilder.AppendStyle(rawCss);
        }

        public HtmlBuilder AppendScript([LanguageInjection("javascript")] string rawScript)
        {
            var child = new HtmlBuilder(HtmlTag.Script)
                .WithAttribute("type", "text/javascript")
                .Append(new HtmlBuilder(rawScript, encode:false));

            return htmlBuilder.Append(child);
        }

        public HtmlBuilder AppendScriptIf(bool condition, [LanguageInjection("javascript")] string rawScript)
        {
            if (!condition)
                return htmlBuilder;

            return htmlBuilder.AppendScript(rawScript);
        }
    }
}
