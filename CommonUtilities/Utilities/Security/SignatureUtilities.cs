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