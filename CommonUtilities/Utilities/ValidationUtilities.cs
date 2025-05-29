namespace CommonUtilities.Utilities;

/// <summary>
///     Provides utility methods for validation.
/// </summary>
public static class ValidationUtilities
{
    /// <summary>
    ///     Checks if a string is a valid hexadecimal representation.
    /// </summary>
    /// <param name="szText">The string to validate.</param>
    /// <returns>True if the string is a valid hex string, false otherwise.</returns>
    public static bool IsValidHex(string szText)
    {
        if (string.IsNullOrEmpty(szText))
            return false;

        // Hex strings often have an even length (each byte is two hex chars).
        // This check can be added if strictness is required:
        // if (szText.Length % 2 != 0)
        // return false;

        foreach (char c in szText)
        {
            bool isHexChar = (c >= '0' && c <= '9') ||
                             (c >= 'a' && c <= 'f') ||
                             (c >= 'A' && c <= 'F');
            if (!isHexChar)
                return false;
        }

        return true;
    }
}