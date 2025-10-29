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