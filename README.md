# JJConsulting.Html – Fluent HTML Builder for .NET

`JJConsulting.Html` provides a fluent, low-allocation, strongly typed API for constructing HTML in .NET.
It is suitable for scenarios where a templating engine is not desirable, and where full programmatic control is required.

---

## Features

* Fluent API for building HTML trees
* Attribute helpers with conditional logic
* HTML encoding for text and attribute values
* Low allocations through `TextWriter`
* Built on ASP.NET Core `IHtmlContent` for direct interoperability with Razor and Tag Helpers
* Common element helpers: `Div`, `Span`, `Input`, `Label`, `A`, `Br`, `Hr` powered by source generators

---

## Use Cases

* Generating HTML fragments programmatically in backend code
* Creating ASP.NET Core [Tag Helpers](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-10.0)
* Creating reusable UI builders without Razor, like in [JJMasterData](https://www.github.com/jjconsulting/jjmasterdata).
* HTML emails or templated documents
* Automated content generators and utilities
* High-performance scenarios where templating engines are too heavy

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

## ASP.NET Core Integration

`HtmlBuilder` implements `IHtmlContent`, and child nodes are also handled as `IHtmlContent`.
That means ASP.NET Core content such as `TagHelperContent` can be appended or assigned directly without converting it to a string first.

```csharp
public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
{
    var content = await output.GetChildContentAsync();

    var card = new HtmlBuilder(HtmlTag.Div)
        .WithCssClass("card")
        .Append(content);

    output.Content.SetHtmlContent(card);
}
```

This makes it easy to compose `JJConsulting.Html` with custom Tag Helpers, Razor output, and other ASP.NET Core HTML primitives.

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

⚠️ Never trust user-provided input in this method and other overloads that don't encode HTML like `AppendStyle`.

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

## Alternative DSL

In addition to the fluent `HtmlBuilder` API, the library also provides an optional **declarative DSL** based on static extension methods such as `Div(...)`, `Span(...)`, `H1(...)`, `P(...)`, etc.
These helpers let you compose HTML trees using nested expressions instead of chained fluent calls.
It's directly inspired by [Giraffe](https://github.com/giraffe-fsharp/Giraffe.ViewEngine).

### Example

```csharp
using static JJConsulting.Html.Extensions.HtmlBuilderTagExtensions;

var html =
    Div(
        H1("Welcome"),
        P("This is a compact DSL."),
        Ul(
            Li("One"),
            Li("Two"),
            Li("Three")
        )
    ).ToString(true);
```

Produces:

```html
<div>
  <h1>Welcome</h1>
  <p>This is a compact DSL.</p>
  <ul>
    <li>One</li>
    <li>Two</li>
    <li>Three</li>
  </ul>
</div>
```

### When to Use the DSL

This style is ideal when:

* You prefer expression-based UI construction
* You want a clean declarative style similar to JSX, XML literals, or Razor Components
* You want minimal syntax noise while preserving strong typing

### Mixed Use

The DSL and the fluent API are fully interoperable:

```csharp
var block =
    Div(
        H2("Title"),
        Div().WithCssClass("box").AppendText("Inside")
    );
```

This allows you to use fluent attribute/child configuration while still benefiting from expressive tag factories.

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
            if (string.IsNullOrEmpty(tooltip)) 
                return html;
            
            html.WithAttribute("title", tooltip);
            html.WithData("toogle", "tooltip");

            return html;
        }
    }
}
```
