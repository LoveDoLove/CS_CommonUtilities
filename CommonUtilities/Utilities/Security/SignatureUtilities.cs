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
using System.Text.Json;

namespace CommonUtilities.Utilities.Security;

/// <summary>
///     Provides utility methods for creating digital signatures.
/// </summary>
public static class SignatureUtilities
{
    /// <summary>
    ///     Creates a Base64 encoded RSA-SHA256 signature for a given object.
    /// </summary>
    /// <param name="timeStamp">The timestamp to include in the signature data.</param>
    /// <param name="pemKey">The PEM-encoded RSA private key.</param>
    /// <param name="request">The object to serialize and include in the signature data.</param>
    /// <returns>A Base64 encoded string representing the signature.</returns>
    public static string ObjectBase64Signer(long timeStamp, string pemKey, object request)
    {
        string requestBody = JsonSerializer.Serialize(request);
        string strToSign = string.Concat(timeStamp, requestBody);
        byte[] bytesToSign = Encoding.UTF8.GetBytes(strToSign); // Changed to UTF8
        byte[] hash;

        using (SHA256 sha256 = SHA256.Create())
        {
            hash = sha256.ComputeHash(bytesToSign);
        }

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportFromPem(pemKey); // Pass the PEM-encoded key directly as a string
            byte[] signature = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signature);
        }
    }
}