using System.Security.Cryptography;
using System.Text;
using Domain.Common;

namespace Application.Utilities;

public class AesUtilities
{
    public static string Aes256EcbEncrypt(string szText, string szKey)
    {
        try
        {
            byte[] text = Encoding.UTF8.GetBytes(szText);
            Aes aes = Aes.Create();
            aes.Key = ConvertUtilities.szConvertKeyToBytes(szKey);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(text, 0, text.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static string Aes256EcbDecrypt(string szText, string szKey)
    {
        try
        {
            byte[] text = Convert.FromBase64String(szText);
            Aes aes = Aes.Create();
            aes.Key = ConvertUtilities.szConvertKeyToBytes(szKey);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(text, 0, text.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static string Aes256CbcEncrypt(string szText, string szKey, string szIv = Common.C_3DESAPPVECTOR)
    {
        try
        {
            byte[] iv = Encoding.UTF8.GetBytes(szIv);

            Aes aes = Aes.Create();
            aes.Key = ConvertUtilities.szConvertKeyToBytes(szKey);
            aes.Mode = CipherMode.CBC;
            aes.IV = iv;

            ICryptoTransform aesCreateEncrypt = aes.CreateEncryptor();
            MemoryStream memoryStream = new();
            CryptoStream cryptoStream = new(memoryStream, aesCreateEncrypt, CryptoStreamMode.Write);

            byte[] plainBytes = Encoding.UTF8.GetBytes(szText);

            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] cipherBytes = memoryStream.ToArray();
            string szOutput = Convert.ToBase64String(cipherBytes);

            memoryStream.Close();
            cryptoStream.Close();
            return szOutput;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static string Aes256CbcDecrypt(string szText, string szKey, string szIv = Common.C_3DESAPPVECTOR)
    {
        try
        {
            Aes aes = Aes.Create();

            aes.Key = ConvertUtilities.szConvertKeyToBytes(szKey);
            aes.Mode = CipherMode.CBC;
            aes.IV = Encoding.UTF8.GetBytes(szIv);

            ICryptoTransform aesCreateDecrypt = aes.CreateDecryptor();
            MemoryStream memoryStream = new();
            CryptoStream cryptoStream = new(memoryStream, aesCreateDecrypt, CryptoStreamMode.Write);

            byte[] cipherBytes = Convert.FromBase64String(szText);

            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] plainBytes = memoryStream.ToArray();
            string szOutput = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();
            return szOutput;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}