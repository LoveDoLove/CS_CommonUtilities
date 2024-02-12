using System.Security.Cryptography;
using System.Text;
using CommonUtilities.Common;

namespace CommonUtilities.Utilities;

public static class TripleDesUtilities
{
    public static string TripleDesCbcEncrypt(string szText, string szKey, string szIv = Constants.DefaultIv)
    {
        try
        {
            TripleDESCryptoServiceProvider obj3Des = new()
            {
                Padding = PaddingMode.PKCS7, Mode = CipherMode.CBC, Key = ConvertUtilities.btHexToByte(szKey)
            };

            if (!ValidationUtilities.bValidHex(szIv)) szIv = ConvertUtilities.szStringToHex(szIv);

            obj3Des.IV = ConvertUtilities.btHexToByte(szIv);

            if (!ValidationUtilities.bValidHex(szText)) szText = ConvertUtilities.szStringToHex(szText);

            byte[] btInput = ConvertUtilities.btHexToByte(szText);
            byte[] btOutput = ConvertUtilities.btTransform(btInput, obj3Des.CreateEncryptor());
            return ConvertUtilities.szWriteHex(btOutput);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static string TripleDesCbcDecrypt(string szText, string szKey, string szIv = Constants.DefaultIv)
    {
        try
        {
            TripleDESCryptoServiceProvider obj3Des = new()
            {
                Padding = PaddingMode.PKCS7, Mode = CipherMode.CBC, Key = ConvertUtilities.btHexToByte(szKey)
            };

            if (!ValidationUtilities.bValidHex(szIv)) szIv = ConvertUtilities.szStringToHex(szIv);

            obj3Des.IV = ConvertUtilities.btHexToByte(szIv);

            //szText = szText.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

            if (!ValidationUtilities.bValidHex(szText)) szText = ConvertUtilities.szStringToHex(szText);

            byte[] btInput = ConvertUtilities.btHexToByte(szText);
            byte[] btOutput = ConvertUtilities.btTransform(btInput, obj3Des.CreateDecryptor());
            return Encoding.UTF8.GetString(btOutput);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}