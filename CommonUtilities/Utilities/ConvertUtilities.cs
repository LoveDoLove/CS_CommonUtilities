using System.Security.Cryptography;
using System.Text;

namespace CommonUtilities.Utilities;

public static class ConvertUtilities
{
    public static byte[] btHexToByte(string cHex)
    {
        byte[] btOutput = new byte[cHex.Length / 2];
        try
        {
            for (int i = 0; i < cHex.Length; i += 2) btOutput[i / 2] = Convert.ToByte(cHex.Substring(i, 2), 16);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return btOutput;
    }

    public static string szStringToHex(string szText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(szText);
        StringBuilder szHex = new(bytes.Length * 2);

        foreach (byte b in bytes) szHex.AppendFormat("{0:X2}", b);

        return szHex.ToString();
    }

    public static byte[] btTransform(byte[] btInput, ICryptoTransform cryptoTransform)
    {
        MemoryStream objMemStream = new();
        CryptoStream objCryptStream = new(objMemStream, cryptoTransform, CryptoStreamMode.Write);
        byte[] btResult = [];
        try
        {
            objCryptStream.Write(btInput, 0, btInput.Length);
            objCryptStream.FlushFinalBlock();

            objMemStream.Position = 0;

            btResult = new byte[objMemStream.Length];
            objMemStream.Read(btResult, 0, btResult.Length);
            objMemStream.Close();
            objCryptStream.Close();
            return btResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return btResult;
    }

    public static string szWriteHex(byte[] btArray)
    {
        string szHex = "";
        try
        {
            int iCounter;
            for (iCounter = 0; iCounter < btArray.Length; iCounter++) szHex += btArray[iCounter].ToString("X2");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return szHex;
    }

    public static byte[] szConvertKeyToBytes(string szKey)
    {
        byte[] keyArray;

        if (szKey.Length is 16 or 32 or 48)
        {
            keyArray = Encoding.UTF8.GetBytes(szKey);
        }
        else
        {
            keyArray = new byte[szKey.Length / 2];
            for (int i = 0; i < keyArray.Length; i++) keyArray[i] = Convert.ToByte(szKey.Substring(i * 2, 2), 16);
        }

        return keyArray;
    }
}