using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace JJConsulting.Html;

/// <summary>
/// A utility class for building HTML structures programmatically.
/// Provides methods for creating, combining, and rendering HTML strings with nested elements, attributes,
/// and raw text support.
/// </summary>
public class HtmlBuilder() : IHtmlBuilder
{
    private readonly Dictionary<string, string?> _attributes = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly List<IHtmlBuilder?> _children = [];
    private readonly HtmlTag? _tag;

    public HtmlBuilder(string rawText, bool encode = true) : this()
    {
        _children.Add(new HtmlText(rawText, encode));
    }

    public HtmlBuilder(HtmlTag tag) : this()
    {
        _tag = tag;
    }

    public HtmlBuilder(HtmlTag tag, string rawText) : this()
    {
        _tag = tag;
        _children.Add(new HtmlBuilder(rawText));
    }

    public HtmlBuilder(HtmlTag tag, params List<IHtmlBuilder?> children) : this(tag)
    {
        _children.AddRange(children);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        using var writer = new StringWriter(sb);

        WriteTo(writer, HtmlEncoder.Default);

        var result = sb.ToString();

        return result;
    }

    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        if (!_tag.HasValue)
        {
            foreach (var c in _children)
            {
                c?.WriteTo(writer, encoder);
            }

            return;
        }

        var tagName = _tag.Value.Name;

        writer.Write('<');
        writer.Write(tagName);

        WriteAttributes(writer, encoder);

        if (!_tag.Value.HasClosingTag)
        {
            writer.Write(" />");
            return;
        }

        writer.Write('>');

        foreach (var child in _children)
        {
            child?.WriteTo(writer, encoder);
        }

        writer.Write("</");
        writer.Write(tagName);
        writer.Write('>');
    }

    private void WriteAttributes(TextWriter writer, HtmlEncoder encoder)
    {
        if (_attributes is not { Count: > 0 })
            return;

        foreach (var attribute in _attributes)
        {
            writer.Write(" ");
            writer.Write(attribute.Key);
            writer.Write("=\"");
            if (attribute.Value != null)
                encoder.Encode(writer, attribute.Value);
            writer.Write("\"");
        }
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

    public HtmlBuilder Prepend(IHtmlBuilder? html)
    {
        if (ReferenceEquals(this, html))
            throw new InvalidOperationException("Cannot prepend the same HtmlBuilder instance to itself.");

        if (html != null)
            _children.Insert(0, html);

        return this;
    }

    public HtmlBuilder Append(IHtmlBuilder? html)
    {
        if (ReferenceEquals(this, html))
            throw new InvalidOperationException("Cannot append the same HtmlBuilder instance to itself.");

        if (html != null)
            _children.Add(html);

        return this;
    }

    public HtmlBuilder AppendRange(IEnumerable<IHtmlBuilder> htmlEnumerable)
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