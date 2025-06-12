namespace CommonUtilities.Services.IpInfo;

/// <summary>
///     Represents configuration settings related to an IP information service,
///     specifically an access token for an external IP geolocation API (e.g., ipinfo.io).
/// </summary>
public class IpInfoConfig
{
    /// <summary>
    ///     Gets or sets the access token for the IP information service.
    ///     This token is used to authenticate requests to the external API.
    /// </summary>
    /// <value>The API access token. Defaults to an empty string.</value>
    public string Token { get; set; } = string.Empty;
}