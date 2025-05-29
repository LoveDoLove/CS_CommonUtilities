using System.Text.Json.Serialization;

namespace CommonUtilities.Models.Response.IpInfo;

/// <summary>
///     Represents the response from an IP information service (e.g., ipinfo.io).
///     Contains geolocation and other details associated with an IP address.
/// </summary>
public class IpInfoResponse
{
    /// <summary>
    ///     Gets or sets the IP address.
    ///     Mapped from the "ip" field in the JSON response.
    /// </summary>
    /// <value>The IP address string, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("ip")]
    public string? Ip { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the city associated with the IP address.
    ///     Mapped from the "city" field in the JSON response.
    /// </summary>
    /// <value>The city name, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("city")]
    public string? City { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the region or state associated with the IP address.
    ///     Mapped from the "region" field in the JSON response.
    /// </summary>
    /// <value>The region or state name, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("region")]
    public string? Region { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the country code (e.g., "US", "GB") associated with the IP address.
    ///     Mapped from the "country" field in the JSON response.
    /// </summary>
    /// <value>The country code, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("country")]
    public string? Country { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the geographical coordinates (latitude,longitude) associated with the IP address.
    ///     Typically a comma-separated string, e.g., "34.0522,-118.2437".
    ///     Mapped from the "loc" field in the JSON response.
    /// </summary>
    /// <value>The location coordinates, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("loc")]
    public string? Loc { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the organization (ISP or company) associated with the IP address.
    ///     Often includes an ASN (Autonomous System Number) and the organization name, e.g., "AS7922 Comcast Cable
    ///     Communications, LLC".
    ///     Mapped from the "org" field in the JSON response.
    /// </summary>
    /// <value>The organization details, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("org")]
    public string? Org { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the postal code associated with the IP address.
    ///     Mapped from the "postal" field in the JSON response.
    /// </summary>
    /// <value>The postal code, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("postal")]
    public string? Postal { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the timezone (e.g., "America/Los_Angeles") associated with the IP address.
    ///     Mapped from the "timezone" field in the JSON response.
    /// </summary>
    /// <value>The timezone identifier, or <c>null</c> if not available. Defaults to an empty string.</value>
    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; } = string.Empty;

    // Other potential fields from ipinfo.io include:
    // [JsonPropertyName("hostname")]
    // public string? Hostname { get; set; }
    //
    // [JsonPropertyName("readme")] // Link to ipinfo.io's about page for this IP
    // public string? Readme { get; set; }
}