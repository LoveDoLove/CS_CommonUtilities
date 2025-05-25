using System.Security.Cryptography;

namespace CommonUtilities.Utilities;

/// <summary>
/// Provides utility methods for data type conversions.
/// </summary>
public static class ConvertUtilities
{
    /// <summary>
    /// Converts a hexadecimal string to a byte array.
    /// </summary>
    /// <param name="cHex">The hexadecimal string to convert.</param>
    /// <returns>A byte array representing the hexadecimal string.</returns>
    /// <exception cref="ArgumentException">Thrown if the hex string is null, empty, has an odd number of characters, or contains invalid hex characters.</exception>
    /// <exception cref="InvalidOperationException">Thrown if an error occurs during conversion.</exception>
    public static byte[] btHexToByte(string cHex)
    {
        if (string.IsNullOrEmpty(cHex))
            return []; // Or throw new ArgumentNullException(nameof(cHex));
        if (cHex.Length % 2 != 0)
            throw new ArgumentException("Hex string must have an even number of characters.", nameof(cHex));

        byte[] btOutput = new byte[cHex.Length / 2];
        try
        {
            for (int i = 0; i < cHex.Length; i += 2)
                btOutput[i / 2] = Convert.ToByte(cHex.Substring(i, 2), 16);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Invalid hex character found in string.", nameof(cHex), ex);
        }
        catch (Exception ex)
        {
            // Rethrow with more context or the original exception
            throw new InvalidOperationException($"Error converting hex string to byte array: {ex.Message}", ex);
        }

        return btOutput;
    }

    /// <summary>
    /// Converts a string to its hexadecimal representation.
    /// </summary>
    /// <param name="szText">The string to convert.</param>
    /// <returns>The hexadecimal representation of the input string, or an empty string if the input is null.</returns>
    public static string szStringToHex(string szText)
    {
        if (szText == null)
            return string.Empty; // Or throw new ArgumentNullException(nameof(szText));

        byte[] bytes = Encoding.UTF8.GetBytes(szText);
        StringBuilder szHex = new(bytes.Length * 2);

        foreach (byte b in bytes)
            szHex.AppendFormat("{0:X2}", b);

        return szHex.ToString();
    }

    /// <summary>
    /// Performs a cryptographic transformation on a byte array.
    /// </summary>
    /// <param name="btInput">The input byte array to transform.</param>
    /// <param name="cryptoTransform">The cryptographic transform to apply.</param>
    /// <returns>The transformed byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if btInput or cryptoTransform is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if an error occurs during the cryptographic transformation.</exception>
    public static byte[] btTransform(byte[] btInput, ICryptoTransform cryptoTransform)
    {
        if (btInput == null) throw new ArgumentNullException(nameof(btInput));
        if (cryptoTransform == null) throw new ArgumentNullException(nameof(cryptoTransform));

        try
        {
            using (MemoryStream objMemStream = new())
            {
                using (CryptoStream objCryptStream = new(objMemStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    objCryptStream.Write(btInput, 0, btInput.Length);
                    objCryptStream.FlushFinalBlock();
                }

                return objMemStream.ToArray();
            }
        }
        catch (CryptographicException ex) // Catch more specific exceptions if possible
        {
            throw new InvalidOperationException($"Error during cryptographic transformation: {ex.Message}", ex);
        }
        // Removed unreachable code: return btResult;
    }

    /// <summary>
    /// Converts a byte array to its hexadecimal string representation.
    /// </summary>
    /// <param name="btArray">The byte array to convert.</param>
    /// <returns>The hexadecimal string representation of the byte array, or an empty string if the input is null.</returns>
    /// <exception cref="InvalidOperationException">Thrown if an error occurs during conversion.</exception>
    public static string szWriteHex(byte[] btArray)
    {
        if (btArray == null)
            return string.Empty; // Or throw new ArgumentNullException(nameof(btArray));

        StringBuilder szHex = new StringBuilder(btArray.Length * 2);
        try
        {
            foreach (byte b in btArray) szHex.AppendFormat("{0:X2}", b);
        }
        catch (Exception ex) // This catch might be overly broad if AppendFormat is the only concern.
        {
            throw new InvalidOperationException($"Error writing byte array to hex string: {ex.Message}", ex);
        }

        return szHex.ToString();
    }

    /// <summary>
    /// Converts a key string (either hex or plain text) to a byte array.
    /// It attempts to intelligently determine if the key is hex or plain text based on common key lengths.
    /// </summary>
    /// <param name="szKey">The key string to convert.</param>
    /// <returns>A byte array representing the key.</returns>
    /// <exception cref="ArgumentNullException">Thrown if szKey is null or empty.</exception>
    public static byte[] szConvertKeyToBytes(string szKey)
    {
        if (string.IsNullOrEmpty(szKey))
            throw new ArgumentNullException(nameof(szKey));

        byte[] keyArray;

        // Priority to hex if it's valid hex and typical hex key length (32, 48, 64 chars for 16, 24, 32 bytes)
        bool isHex = ValidationUtilities.IsValidHex(szKey);
        if (isHex && (szKey.Length == 32 || szKey.Length == 48 || szKey.Length == 64))
            keyArray = btHexToByte(szKey);
        // If not a clear hex key, try to interpret as UTF8, especially for common plain text key lengths for AES/TripleDES
        // This part remains heuristic. Consider an explicit parameter for key type.
        else if (!isHex && (szKey.Length == 16 || szKey.Length == 24 ||
                            szKey.Length == 32)) // Common byte lengths for keys
            keyArray = Encoding.UTF8.GetBytes(szKey);
        // Fallback: if it's hex but not a typical key length, still convert from hex.
        else if (isHex && szKey.Length % 2 == 0)
            keyArray = btHexToByte(szKey);
        // Final fallback: treat as UTF8.
        else
            keyArray = Encoding.UTF8.GetBytes(szKey);
        // Consider warning or throwing if format is ambiguous and doesn't fit common patterns.
        // e.g., Log.Warning($"Key format for '{szKey.Substring(0, Math.Min(szKey.Length,5))}(...)' is ambiguous, treated as UTF-8.");
        return keyArray;
    }
}