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
using CommonUtilities.Models.Share;
using CommonUtilities.Utilities.Data;

namespace CommonUtilities.Utilities.Security;

/// <summary>
///     Provides utility methods for TripleDES encryption and decryption.
/// </summary>
public static class TripleDesUtilities
{
    /// <summary>
    ///     Encrypts a string using TripleDES with CBC mode.
    /// </summary>
    /// <param name="szText">The plaintext string to encrypt.</param>
    /// <param name="szKey">The secret key for encryption (hex string).</param>
    /// <param name="szIv">The initialization vector (hex string or passphrase). Defaults to Constants.DefaultIv.</param>
    /// <returns>The encrypted string (hex encoded).</returns>
    /// <exception cref="CryptographicException">Thrown if an error occurs during encryption.</exception>
    /// <exception cref="ArgumentException">Thrown if the IV is invalid.</exception>
    public static string TripleDesCbcEncrypt(string szText, string szKey, string szIv = Constants.DefaultIv)
    {
        try
        {
            byte[]
                key = ConvertUtilities
                    .btHexToByte(
                        szKey); // Assuming szKey is always hex. If not, use ConvertUtilities.szConvertKeyToBytes
            byte[] ivBytes;

            if (ValidationUtilities.IsValidHex(szIv) && szIv.Length == 16) // 16 hex chars = 8 bytes
            {
                ivBytes = ConvertUtilities.btHexToByte(szIv);
            }
            else
            {
                // If not a valid 16-char hex string, derive an 8-byte IV from szIv.
                // This is a simple approach; for production, a proper key derivation function (KDF) like PBKDF2 should be used.
                // For this example, we'll hash it and take the first 8 bytes.
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedIv = sha256.ComputeHash(Encoding.UTF8.GetBytes(szIv));
                    ivBytes = new byte[8];
                    Array.Copy(hashedIv, ivBytes, 8);
                }

                // Alternatively, if szIv is a passphrase, ensure it's handled consistently or throw if not in expected format.
                // Forcing a specific format/length for szIv is safer.
                // For now, we'll stick to the original logic's spirit if DefaultIv is used:
                // if (!ValidationUtilities.IsValidHex(szIv)) szIv = ConvertUtilities.szStringToHex(szIv);
                // ivBytes = ConvertUtilities.btHexToByte(szIv);
                // The above original logic might result in variable length IVs if szIv is not hex and its hex form is not 16 chars.
                // Let's refine to ensure 8 bytes for IV:
                if (ValidationUtilities.IsValidHex(szIv) && szIv.Length == 16)
                {
                    ivBytes = ConvertUtilities.btHexToByte(szIv);
                }
                else // If szIv is not a 16-char hex string, treat as passphrase, encode to UTF8, then take first 8 bytes or hash.
                {
                    // This example takes first 8 bytes of UTF8 encoding, ensure it's at least 8 bytes or pad/hash.
                    // A robust solution would use a KDF or require szIv to be hex.
                    byte[] tempIv = Encoding.UTF8.GetBytes(szIv);
                    if (tempIv.Length < 8)
                        throw new ArgumentException("IV must be at least 8 bytes or a 16-character hex string.",
                            nameof(szIv));
                    ivBytes = new byte[8];
                    Array.Copy(tempIv, ivBytes, 8);
                }
            }


            using (TripleDES obj3Des = TripleDES.Create())
            {
                obj3Des.Key = key;
                obj3Des.IV = ivBytes;
                obj3Des.Padding = PaddingMode.PKCS7;
                obj3Des.Mode = CipherMode.CBC;

                byte[] btInput = Encoding.UTF8.GetBytes(szText); // Encrypt plaintext directly
                using (ICryptoTransform encryptor = obj3Des.CreateEncryptor())
                {
                    byte[] btOutput = ConvertUtilities.btTransform(btInput, encryptor);
                    return ConvertUtilities.szWriteHex(btOutput);
                }
            }
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error during TripleDES encryption: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Decrypts a string using TripleDES with CBC mode.
    /// </summary>
    /// <param name="szText">The encrypted string to decrypt (hex encoded).</param>
    /// <param name="szKey">The secret key for decryption (hex string).</param>
    /// <param name="szIv">The initialization vector (hex string or passphrase). Defaults to Constants.DefaultIv.</param>
    /// <returns>The decrypted plaintext string.</returns>
    /// <exception cref="CryptographicException">Thrown if an error occurs during decryption.</exception>
    /// <exception cref="ArgumentException">Thrown if the input text or IV is invalid.</exception>
    public static string TripleDesCbcDecrypt(string szText, string szKey, string szIv = Constants.DefaultIv)
    {
        try
        {
            byte[]
                key = ConvertUtilities
                    .btHexToByte(
                        szKey); // Assuming szKey is always hex. If not, use ConvertUtilities.szConvertKeyToBytes
            byte[] ivBytes;

            // Consistent IV handling with encrypt method
            if (ValidationUtilities.IsValidHex(szIv) && szIv.Length == 16) // 16 hex chars = 8 bytes
            {
                ivBytes = ConvertUtilities.btHexToByte(szIv);
            }
            else
            {
                byte[] tempIv = Encoding.UTF8.GetBytes(szIv);
                if (tempIv.Length < 8)
                    throw new ArgumentException("IV must be at least 8 bytes or a 16-character hex string.",
                        nameof(szIv));
                ivBytes = new byte[8];
                Array.Copy(tempIv, ivBytes, 8);
            }

            using (TripleDES obj3Des = TripleDES.Create())
            {
                obj3Des.Key = key;
                obj3Des.IV = ivBytes;
                obj3Des.Padding = PaddingMode.PKCS7;
                obj3Des.Mode = CipherMode.CBC;

                // Assuming szText is hex encoded ciphertext
                if (!ValidationUtilities.IsValidHex(szText))
                    throw new ArgumentException("Input text for decryption must be a hex string.", nameof(szText));

                byte[] btInput = ConvertUtilities.btHexToByte(szText);
                using (ICryptoTransform decryptor = obj3Des.CreateDecryptor())
                {
                    byte[] btOutput = ConvertUtilities.btTransform(btInput, decryptor);
                    return Encoding.UTF8.GetString(btOutput); // Decode to UTF-8 string
                }
            }
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error during TripleDES decryption: {ex.Message}", ex);
        }
    }
}