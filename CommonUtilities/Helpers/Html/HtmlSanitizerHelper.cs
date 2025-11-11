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
/// </summary>
public static class HtmlSanitizerHelper
{
    /// <summary>
    ///     The HTML sanitizer instance with a configurable whitelist/policy.
    /// </summary>
    private static readonly HtmlSanitizer Sanitizer;

    static HtmlSanitizerHelper()
    {
        Sanitizer = new HtmlSanitizer();

        // Clear default allowed tags and define our own conservative whitelist
        Sanitizer.AllowedTags.Clear();
        string[] allowedTags =
        [
            "a", "b", "i", "strong", "em", "u", "p", "br",
            "ul", "ol", "li", "blockquote", "code", "pre"
        ];
        foreach (string tag in allowedTags) Sanitizer.AllowedTags.Add(tag);

        // Clear default allowed attributes and define ours
        Sanitizer.AllowedAttributes.Clear();
        Sanitizer.AllowedAttributes.Add("href");
        Sanitizer.AllowedAttributes.Add("title");
        Sanitizer.AllowedAttributes.Add("target");
        Sanitizer.AllowedAttributes.Add("rel");

        // Limit allowed URI schemes for safe links
        Sanitizer.AllowedSchemes.Clear();
        Sanitizer.AllowedSchemes.Add("http");
        Sanitizer.AllowedSchemes.Add("https");
        Sanitizer.AllowedSchemes.Add("mailto");

        // Forbid all event attributes (onload, onclick, etc.)
        Sanitizer.RemovingAttribute += (sender, args) =>
        {
            if (args.Attribute.Name.StartsWith("on", StringComparison.OrdinalIgnoreCase))
                args.Cancel = false; // allow removal
        };

        // Optional: disallow style attributes entirely (for simplicity)
        Sanitizer.AllowedCssProperties.Clear();

        // Optional: For links enforce rel="nofollow" (or other policy)
        Sanitizer.PostProcessNode += (sender, args) =>
        {
            if (args.Node is IElement element &&
                element.TagName.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                element.GetAttribute("href");
                element.SetAttribute("rel", "noopener noreferrer nofollow");
            }
        };
    }

    /// <summary>
    ///     Sanitizes the provided HTML-like input and returns a string safe to render as HTML.
    ///     Newlines are converted to &lt;br/&gt; so plain-text line breaks are preserved.
    ///     Allowed tags and attributes are limited by the sanitizer configuration above.
    /// </summary>
    /// <param name="input">The user-provided HTML-like string.</param>
    /// <returns>A sanitized string safe for HTML rendering.</returns>
    public static string SanitizeAndFormat(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Sanitize the raw input (removes unsafe tags/attributes, normalizes HTML)
        string sanitized = Sanitizer.Sanitize(input);

        // Convert newline sequences to <br/>. 
        // Note: If you prefer using <p> wrappers, you might adjust this accordingly.
        sanitized = sanitized
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace("\n", "<br/>");

        return sanitized;
    }
}