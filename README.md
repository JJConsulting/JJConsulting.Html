# JJConsulting.Html – Fluent HTML Builder for .NET

`JJConsulting.Html` provides a fluent, low-allocation, strongly typed API for constructing HTML in .NET.
It is suitable for scenarios where a templating engine is not desirable, and where full programmatic control is required.

---

## Features

* Fluent API for building HTML trees
* Attribute helpers with conditional logic
* HTML encoding for text and attribute values
* Indented or compact output
* Low allocations through `ObjectPool<StringBuilder>`
* Common element helpers: `Div`, `Span`, `Input`, `Label`, `A`, `Br`, `Hr`

---

## Installation

```
dotnet add package JJConsulting.Html
```

---

## Basic Usage

```csharp
var html =
    new HtmlBuilder(HtmlTag.Div)
        .WithCssClass("container")
        .AppendDiv(d =>
            d.WithCssClass("header")
             .AppendText("Hello world!")
        )
        .AppendBr()
        .AppendLink("Click here", "https://example.com")
        .ToString(true);
```

Produces:

```html
<div class="container">
  <div class="header">Hello world!</div>
  <br />
  <a href="https://example.com">Click here</a>
</div>
```

---

## Raw Text

```csharp
new HtmlBuilder("Hello <b>world</b>");                // Encoded
new HtmlBuilder("Hello <b>world</b>", encode:false);  // Not encoded
```

---

## Working With Attributes

```csharp
new HtmlBuilder(HtmlTag.Input)
    .WithName("email")
    .WithId("email")
    .WithValue("test@example.com")
    .WithCssClass("form-control");
```

---

## Adding Children

```csharp
var root = new HtmlBuilder(HtmlTag.Div);

root.Append(HtmlTag.Span, span => span.AppendText("Inside span"));
root.AppendText(" Just text ");
root.AppendBr();
root.AppendHiddenInput("token", "abc123");
```

---

## Conditional Building

```csharp
builder.AppendIf(isLogged, HtmlTag.Div, div => div.AppendText("Welcome"));
builder.WithAttributeIf(isAdmin, "data-role", "admin");
builder.AppendTextIf(showText, "Visible text");
builder.AppendScriptIf(debug, "console.log('debug');");
```

---

## Script Injection

```csharp
builder.AppendScript("alert('Hello');");
```

Produces:

```html
<script type="text/javascript">alert('Hello');</script>
```

---

## Full Example

```csharp
var page =
    new HtmlBuilder(HtmlTag.Div)
        .WithCssClass("page")
        .AppendDiv("Header", (text, div) =>
            div.WithCssClass("header").AppendText(text)
        )
        .AppendDiv(div =>
        {
            div.WithCssClass("content");
            div.AppendText("Some content here.");
            div.AppendBr();
            div.AppendLink("Read more", "/more");
        })
        .AppendScript("console.log('Page loaded');")
        .ToString(true);
```

---

## Use Cases

* Generating HTML fragments programmatically in backend code
* Creating reusable UI builders without Razor, like in [JJMasterData](https://www.github.com/jjconsulting/jjmasterdata).
* HTML emails or templated documents
* Automated content generators and utilities
* High-performance scenarios where templating engines are too heavy

---

## Extending the Library

Using the new C# 14 [extension members](https://devblogs.microsoft.com/dotnet/csharp-exploring-extension-members/), you can easily extend `HtmlBuilder`.

```csharp
public static class HtmlBuilderExtensions
{
    extension(HtmlBuilder html)
    {
        public HtmlBuilder WithData(string name, string value)
        {
            return html.WithAttribute($"data-bs-{name}", value);
        }
        
        public HtmlBuilder WithToolTip(string? tooltip)
        {
            if (tooltip == null || string.IsNullOrEmpty(tooltip)) 
                return html;
            
            html.WithAttribute("title", tooltip);
            html.WithData("tooltip");

            return html;
        }
    }
}
```

