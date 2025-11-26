using System;
using System.Collections.Generic;
using System.Net;
using JJConsulting.Html.Utils;

namespace JJConsulting.Html;

/// <summary>
/// A utility class for building HTML structures programmatically.
/// Provides methods for creating, combining, and rendering HTML strings with nested elements, attributes,
/// and raw text support.
/// </summary>
public sealed class HtmlBuilder
{
    private readonly string? _rawText;
    private readonly Dictionary<string, string?> _attributes;
    private readonly List<HtmlBuilder?> _children;
    private readonly HtmlTag? _tag;

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlBuilder"/> class.
    /// </summary>
    public HtmlBuilder()
    {
        _attributes = new Dictionary<string, string?>(StringComparer.InvariantCultureIgnoreCase);
        _children = [];
    }

    /// <inheritdoc/>
    /// <param name="rawText"></param>
    /// <param name="encode"></param>
    public HtmlBuilder(string rawText, bool encode = true) : this()
    {
        _rawText = encode ? WebUtility.HtmlEncode(rawText) : rawText;
    }

    /// <inheritdoc/>
    /// <param name="tag"></param>
    public HtmlBuilder(HtmlTag tag) : this()
    {
        _tag = tag;
    }
    public override string ToString()
    {
        return ToHtmlString(false);
    }

    public string ToString(bool indented = false)
    {
        return ToHtmlString(indented);
    }

    private string ToHtmlString(bool indented, int indentLevel = 0)
    {
        var sb = StringBuilderPool.Rent();
        var indent = indented ? new string(' ', indentLevel * 2) : string.Empty;
        var newline = indented ? "\n" : string.Empty;

        if (!_tag.HasValue)
        {
            if (_rawText != null)
            {
                sb.Append(indent);
                sb.Append(_rawText);
                sb.Append(newline);
            }

            foreach (var child in _children)
            {
                if (child != null)
                    sb.Append(child.ToHtmlString(indented, indentLevel));
            }

            var html = sb.ToString();
            StringBuilderPool.Release(sb);
            return html;
        }

        var tagName = _tag.Value.Name;
        sb.Append(indent);
        sb.Append('<');
        sb.Append(tagName);
        sb.Append(GetAttributesHtml());

        if (!_tag.Value.HasClosingTag)
        {
            sb.Append(" />");
            sb.Append(newline);
            var html = sb.ToString();
            StringBuilderPool.Release(sb);
            return html;
        }

        sb.Append('>');
        sb.Append(newline);

        foreach (var child in _children)
        {
            if (child != null)
                sb.Append(child.ToHtmlString(indented, indentLevel + 1));
        }

        sb.Append(indent);
        sb.Append("</");
        sb.Append(tagName);
        sb.Append('>');
        sb.Append(newline);

        var result = sb.ToString();
        StringBuilderPool.Release(sb);
        return result;
    }

    private string GetAttributesHtml()
    {
        var attributesBuilder = StringBuilderPool.Rent();
        foreach (var item in _attributes)
        {
            var key = WebUtility.HtmlEncode(item.Key);
            var value = WebUtility.HtmlEncode(item.Value ?? "");

            attributesBuilder.Append($" {key}=\"{value}\"");
        }

        var attributes = attributesBuilder.ToString();

        StringBuilderPool.Release(attributesBuilder);

        return attributes;
    }

    public string? GetAttribute(string key) => _attributes[key];

    public bool TryGetAttribute(string key, out string? value) => _attributes.TryGetValue(key, out value);

    public HtmlBuilder WithAttribute(string name)
    {
        _attributes[name] = name;
        return this;
    }

    public HtmlBuilder WithAttribute(string name, string? value)
    {
        _attributes[name] = value;
        return this;
    }

    public HtmlBuilder Prepend(HtmlBuilder? builder)
    {
        if (ReferenceEquals(this, builder))
            throw new InvalidOperationException("Cannot prepend the same HtmlBuilder instance to itself.");

        if (builder != null)
            _children.Insert(0, builder);

        return this;
    }

    public HtmlBuilder Append(HtmlBuilder? builder)
    {
        if (ReferenceEquals(this, builder))
            throw new InvalidOperationException("Cannot append the same HtmlBuilder instance to itself.");

        if (builder != null)
            _children.Add(builder);

        return this;
    }

    public HtmlBuilder AppendRange(IEnumerable<HtmlBuilder> htmlEnumerable)
    {
        _children.AddRange(htmlEnumerable);
        return this;
    }

    /// <summary>
    /// Clears all attributes and child elements from the current HtmlBuilder instance.
    /// </summary>
    public void Clear()
    {
        _attributes.Clear();
        _children.Clear();
    }
}