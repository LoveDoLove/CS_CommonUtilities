// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using AngleSharp.Dom;
using Ganss.Xss;

namespace CommonUtilities.Helpers.Html;

/// <summary>
///     Helper class for sanitizing HTML-like input to prevent XSS attacks.
///     Allows img tags with src & class attributes; disallows script tags and unsafe attributes.
/// </summary>
public static class HtmlSanitizerHelper
{
    private static readonly HtmlSanitizer Sanitizer;

    static HtmlSanitizerHelper()
    {
        Sanitizer = new HtmlSanitizer();

        // Configure allowed tags (whitelist)
        Sanitizer.AllowedTags.Clear();
        string[] allowedTags = new[]
        {
            "a", "b", "i", "strong", "em", "u",
            "p", "br", "ul", "ol", "li", "blockquote",
            "code", "pre", "img"
        };
        foreach (string tag in allowedTags) Sanitizer.AllowedTags.Add(tag);

        // Configure allowed attributes
        Sanitizer.AllowedAttributes.Clear();
        Sanitizer.AllowedAttributes.Add("href");
        Sanitizer.AllowedAttributes.Add("title");
        Sanitizer.AllowedAttributes.Add("target");
        Sanitizer.AllowedAttributes.Add("rel");
        Sanitizer.AllowedAttributes.Add("src");
        Sanitizer.AllowedAttributes.Add("class");

        // Configure allowed URI schemes
        Sanitizer.AllowedSchemes.Clear();
        Sanitizer.AllowedSchemes.Add("http");
        Sanitizer.AllowedSchemes.Add("https");
        Sanitizer.AllowedSchemes.Add("mailto");

        // Configure which attributes are treated as URI attributes
        Sanitizer.UriAttributes.Clear();
        Sanitizer.UriAttributes.Add("href");
        Sanitizer.UriAttributes.Add("src");

        // Disallow any inline event attributes (onload, onclick, etc.)
        Sanitizer.RemovingAttribute += (sender, args) =>
        {
            if (args.Attribute.Name.StartsWith("on", StringComparison.OrdinalIgnoreCase))
                // Allow removal (nothing special to do)
                args.Cancel = false;
        };

        // Disallow all CSS properties if you do not want inline styles
        Sanitizer.AllowedCssProperties.Clear();

        // Post-process <a> tags to enforce rel & target policy
        Sanitizer.PostProcessNode += (sender, args) =>
        {
            if (args.Node is IElement element &&
                element.TagName.Equals("a", StringComparison.OrdinalIgnoreCase))
                // Ensure rel attribute to avoid reverse tabnabbing
                element.SetAttribute("rel", "noopener noreferrer nofollow");
            // (You may also enforce target="_blank" if desired)
        };
    }

    /// <summary>
    ///     Sanitizes the provided HTML-like input and returns a string safe to render as HTML.
    ///     Plain-text newlines are converted to &lt;br/&gt; so simple line breaks are preserved.
    /// </summary>
    /// <param name="input">The user-provided HTML-like string.</param>
    /// <returns>A sanitized string safe for HTML body rendering.</returns>
    public static string SanitizeAndFormat(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Sanitize the raw input (removes disallowed tags/attributes)
        string sanitized = Sanitizer.Sanitize(input);

        // Convert newline sequences to <br/> for simple formatting
        sanitized = sanitized
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace("\n", "<br/>");

        return sanitized;
    }
}