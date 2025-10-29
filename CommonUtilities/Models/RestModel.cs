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