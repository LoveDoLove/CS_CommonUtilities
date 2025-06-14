namespace CommonUtilities.Models.Share;

/// <summary>
///     Provides various constants used throughout the application.
/// </summary>
public class Constants
{
    // WARNING: Using a fixed IV like this for cryptographic operations (e.g., AES-CBC, TripleDES-CBC)
    // is insecure if used for multiple messages with the same key.
    // IVs should ideally be unique and randomly generated for each encryption operation.
    // This default might be suitable for specific, controlled scenarios or testing.
    /// <summary>
    ///     Default Initialization Vector (IV) for cryptographic operations.
    ///     <remarks>
    ///         Using a fixed IV is insecure for multiple messages with the same key.
    ///         It should ideally be unique and randomly generated for each encryption.
    ///         This default may be suitable for specific, controlled scenarios or testing.
    ///     </remarks>
    /// </summary>
    public const string DefaultIv = "0000000000000000";

    /// <summary>
    ///     Maximum allowed length for a URL.
    /// </summary>
    public const int MaxUrlLength = 100;

    /// <summary>
    ///     A string used as a separator line in console output.
    /// </summary>
    public const string ConsoleSplitLine =
        "------------------------------------------------------------------------------------";
}