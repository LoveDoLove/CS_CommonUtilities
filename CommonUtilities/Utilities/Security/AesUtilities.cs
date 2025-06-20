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
///     Provides utility methods for AES encryption and decryption.
/// </summary>
public static class AesUtilities
{
    // WARNING: ECB mode is generally insecure as it doesn't use an IV and identical plaintext blocks encrypt to identical ciphertext blocks.
    // Consider using CBC or GCM mode if security is a high priority.
    /// <summary>
    ///     Encrypts a string using AES-256 with ECB mode.
    ///     WARNING: ECB mode is generally insecure.
    /// </summary>
    /// <param name="szText">The plaintext string to encrypt.</param>
    /// <param name="szKey">The secret key for encryption.</param>
    /// <returns>The Base64 encoded encrypted string.</returns>
    /// <exception cref="CryptographicException">Thrown if an error occurs during encryption.</exception>
    public static string Aes256EcbEncrypt(string szText, string szKey)
    {
        try
        {
            byte[] text = Encoding.UTF8.GetBytes(szText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = ConvertUtilities.szConvertKeyToBytes(szKey);
                aes.Mode = CipherMode.ECB; // Insecure mode, see warning above.
                aes.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cTransform = aes.CreateEncryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(text, 0, text.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error during AES ECB encryption: {ex.Message}", ex);
        }
    }

    // WARNING: ECB mode is generally insecure. See comment on Aes256EcbEncrypt.
    /// <summary>
    ///     Decrypts a string using AES-256 with ECB mode.
    ///     WARNING: ECB mode is generally insecure.
    /// </summary>
    /// <param name="szText">The Base64 encoded string to decrypt.</param>
    /// <param name="szKey">The secret key for decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>
    /// <exception cref="CryptographicException">Thrown if an error occurs during decryption.</exception>
    public static string Aes256EcbDecrypt(string szText, string szKey)
    {
        try
        {
            byte[] text = Convert.FromBase64String(szText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = ConvertUtilities.szConvertKeyToBytes(szKey);
                aes.Mode = CipherMode.ECB; // Insecure mode, see warning above.
                aes.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cTransform = aes.CreateDecryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(text, 0, text.Length);
                    return Encoding.UTF8.GetString(resultArray);
                }
            }
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error during AES ECB decryption: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Encrypts a string using AES-256 with CBC mode.
    /// </summary>
    /// <param name="szText">The plaintext string to encrypt.</param>
    /// <param name="szKey">The secret key for encryption.</param>
    /// <param name="szIv">The initialization vector (must be 16 bytes). Defaults to Constants.DefaultIv.</param>
    /// <returns>The Base64 encoded encrypted string.</returns>
    /// <exception cref="ArgumentException">Thrown if the IV or key is not of the correct length.</exception>
    /// <exception cref="CryptographicException">Thrown if an error occurs during encryption.</exception>
    public static string Aes256CbcEncrypt(string szText, string szKey, string szIv = Constants.DefaultIv)
    {
        try
        {
            byte[] iv = Encoding.UTF8.GetBytes(szIv);
            if (iv.Length != 16)
                throw new ArgumentException("IV must be 16 bytes long for AES.", nameof(szIv));

            byte[] keyBytes = ConvertUtilities.szConvertKeyToBytes(szKey);
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
                throw new ArgumentException("Key must be 16, 24, or 32 bytes long for AES.", nameof(szKey));

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform aesCreateEncrypt = aes.CreateEncryptor())
                using (MemoryStream memoryStream = new())
                {
                    using (CryptoStream cryptoStream = new(memoryStream, aesCreateEncrypt, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(szText);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    byte[] cipherBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherBytes);
                }
            }
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error during AES CBC encryption: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Decrypts a string using AES-256 with CBC mode.
    /// </summary>
    /// <param name="szText">The Base64 encoded string to decrypt.</param>
    /// <param name="szKey">The secret key for decryption.</param>
    /// <param name="szIv">The initialization vector (must be 16 bytes). Defaults to Constants.DefaultIv.</param>
    /// <returns>The decrypted plaintext string.</returns>
    /// <exception cref="ArgumentException">Thrown if the IV or key is not of the correct length.</exception>
    /// <exception cref="CryptographicException">Thrown if an error occurs during decryption.</exception>
    public static string Aes256CbcDecrypt(string szText, string szKey, string szIv = Constants.DefaultIv)
    {
        try
        {
            byte[] iv = Encoding.UTF8.GetBytes(szIv);
            if (iv.Length != 16)
                throw new ArgumentException("IV must be 16 bytes long for AES.", nameof(szIv));

            byte[] keyBytes = ConvertUtilities.szConvertKeyToBytes(szKey);
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
                throw new ArgumentException("Key must be 16, 24, or 32 bytes long for AES.", nameof(szKey));

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform aesCreateDecrypt = aes.CreateDecryptor())
                using (MemoryStream memoryStream = new())
                {
                    using (CryptoStream cryptoStream = new(memoryStream, aesCreateDecrypt, CryptoStreamMode.Write))
                    {
                        byte[] cipherBytes = Convert.FromBase64String(szText);
                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    byte[] plainBytes = memoryStream.ToArray();
                    return Encoding.UTF8.GetString(plainBytes); // Use UTF8 for consistency
                }
            }
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error during AES CBC decryption: {ex.Message}", ex);
        }
    }
}