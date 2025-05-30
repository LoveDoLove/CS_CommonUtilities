namespace CommonUtilities.Models.Api;

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