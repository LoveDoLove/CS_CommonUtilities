using System.Security.Cryptography;
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