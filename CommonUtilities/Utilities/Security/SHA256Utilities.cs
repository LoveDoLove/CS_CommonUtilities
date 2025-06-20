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

using System.Security.Cryptography;
using System.Text;

namespace CommonUtilities.Utilities.Security;

/// <summary>
///     Provides utility methods for computing SHA256 hashes.
/// </summary>
public static class Sha256Utilities
{
    /// <summary>
    ///     Computes the SHA256 hash of a given string.
    /// </summary>
    /// <param name="key">The input string to hash.</param>
    /// <returns>The lowercase hexadecimal representation of the SHA256 hash.</returns>
    /// <exception cref="CryptographicException">Thrown if an error occurs during hash computation.</exception>
    public static string ComputeSha256Hash(string key)
    {
        try
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] hashBuffer = sha256.ComputeHash(keyBytes);
                return WriteHex(hashBuffer).ToLower();
            }
        }
        catch (Exception ex)
        {
            // Rethrow the original exception to preserve stack trace and type
            throw new CryptographicException($"Error computing SHA256 hash: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Converts a byte array to its hexadecimal string representation.
    /// </summary>
    /// <param name="array">The byte array to convert.</param>
    /// <returns>The hexadecimal string representation of the byte array, or an empty string if the input is null.</returns>
    private static string WriteHex(byte[] array)
    {
        if (array == null)
            return string.Empty;

        StringBuilder hex = new StringBuilder(array.Length * 2);
        foreach (byte b in array) hex.AppendFormat("{0:X2}", b);
        return hex.ToString();
    }
}