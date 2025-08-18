// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace CommonUtilities.Models;

/// <summary>
///     Represents a model for making REST API requests.
///     It encapsulates common elements needed for an HTTP request, such as URLs, access tokens, and request bodies.
/// </summary>
public class RestModel
{
    /// <summary>
    ///     Gets or sets the client link or base URL for the REST API.
    /// </summary>
    /// <value>The client link. Defaults to an empty string.</value>
    public string ClientLink { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the specific request link or endpoint for the API call.
    ///     This is often appended to the <see cref="ClientLink" />.
    /// </summary>
    /// <value>The request link. Defaults to an empty string.</value>
    public string RequestLink { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the access token for authenticating the API request.
    /// </summary>
    /// <value>The access token. Defaults to an empty string.</value>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the body of the request.
    ///     This can be any object that will be serialized (e.g., to JSON) for the request payload.
    /// </summary>
    /// <value>The request body object. Defaults to an empty string, but should typically be an object or null.</value>
    public object RequestBody { get; set; } =
        string.Empty; // Consider changing default to `new object()` or `null` if appropriate.
}