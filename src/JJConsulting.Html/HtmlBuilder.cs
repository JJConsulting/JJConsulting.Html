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
public class HtmlBuilder()
{
    private readonly string? _rawText;
    private readonly bool _encode = true;
    private readonly Dictionary<string, string?> _attributes = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly List<HtmlBuilder?> _children = [];
    private readonly HtmlTag? _tag;

    public HtmlBuilder(string rawText, bool encode = true) : this()
    {
        _rawText = rawText;
        _encode = encode;
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

    public HtmlBuilder(HtmlTag tag, params List<HtmlBuilder?> children) : this(tag)
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
            if (_rawText != null)
                writer.Write(_encode ? encoder.Encode(_rawText) : _rawText);

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
        foreach (var item in _attributes)
        {
            var key = encoder.Encode(item.Key);
            var value = encoder.Encode(item.Value ?? "");
            writer.Write(' ');
            writer.Write(key);
            writer.Write("=\"");
            writer.Write(value);
            writer.Write('"');
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