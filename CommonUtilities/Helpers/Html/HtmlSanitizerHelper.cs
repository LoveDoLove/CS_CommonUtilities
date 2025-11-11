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

using Ganss.Xss;

namespace CommonUtilities.Helpers.Html;

/// <summary>
///     Helper class for sanitizing HTML-like input to prevent XSS attacks.
/// </summary>
public static class HtmlSanitizerHelper
{
    /// <summary>
    ///     The HTML sanitizer instance with a custom whitelist.
    /// </summary>
    private static readonly HtmlSanitizer Sanitizer;

    /// <summary>
    ///     Static constructor to initialize the HTML sanitizer with a custom whitelist.
    /// </summary>
    static HtmlSanitizerHelper()
    {
        Sanitizer = new HtmlSanitizer();

        // Start with a conservative whitelist of tags and attributes
        Sanitizer.AllowedTags.Clear();
        string[] allowedTags =
        [
            "a", "b", "i", "strong", "em", "u", "p", "br", "ul", "ol", "li", "blockquote", "code", "pre"
        ];
        foreach (string t in allowedTags) Sanitizer.AllowedTags.Add(t);

        Sanitizer.AllowedAttributes.Clear();
        Sanitizer.AllowedAttributes.Add("href");
        Sanitizer.AllowedAttributes.Add("title");
        Sanitizer.AllowedAttributes.Add("target");
        Sanitizer.AllowedAttributes.Add("rel");
    }

    /// <summary>
    ///     Sanitizes the provided HTML-like input and returns a string safe to render as HTML.
    ///     Newlines are converted to &lt;br/&gt; so plain-text line breaks are preserved.
    ///     Allowed tags are limited by the sanitizer configuration above.
    /// </summary>
    public static string SanitizeAndFormat(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        // First sanitize the raw input (this removes unsafe tags/attributes)
        string sanitized = Sanitizer.Sanitize(input);

        // Convert remaining newlines to <br/> to preserve simple formatting when users paste plain text
        // (If input already contains <p> or <br>, this is harmless.)
        sanitized = sanitized.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br/>");

        return sanitized;
    }
}