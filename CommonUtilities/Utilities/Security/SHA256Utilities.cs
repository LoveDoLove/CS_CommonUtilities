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