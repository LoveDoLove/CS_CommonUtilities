namespace CommonUtilities.Models;

/// <summary>
///     Represents configuration settings for Stripe integration, specifically the API key.
///     This class is typically used to store the secret API key required to authenticate with the Stripe API.
/// </summary>
public class StripeApp // Consider renaming to StripeConfig or StripeSettings for clarity if it holds configuration.
{
    /// <summary>
    ///     Gets or sets the Stripe API key (secret key).
    ///     This key is used to authenticate requests to the Stripe API.
    ///     It should be kept confidential.
    /// </summary>
    /// <value>The Stripe API secret key. Defaults to an empty string.</value>
    public string ApiKey { get; set; } = string.Empty;
}