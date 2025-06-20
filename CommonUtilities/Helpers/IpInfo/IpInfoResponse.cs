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

using System.Text.Json.Serialization;

namespace CommonUtilities.Helpers.IpInfo;

/// <summary>
///     Represents the response data from the IP info API service.
/// </summary>
public class IpInfoResponse
{
    /// <summary>
    ///     Gets or sets the IP address.
    /// </summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the city associated with the IP address.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the region associated with the IP address.
    /// </summary>
    [JsonPropertyName("region")]
    public string? Region { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country associated with the IP address.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the latitude and longitude coordinates.
    /// </summary>
    [JsonPropertyName("loc")]
    public string? Loc { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the organization associated with the IP address.
    /// </summary>
    [JsonPropertyName("org")]
    public string? Org { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the postal code associated with the IP address.
    /// </summary>
    [JsonPropertyName("postal")]
    public string? Postal { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the timezone of the IP address location.
    /// </summary>
    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; } = string.Empty;
}