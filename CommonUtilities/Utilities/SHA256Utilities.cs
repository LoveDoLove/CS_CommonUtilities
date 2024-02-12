using System.Security.Cryptography;
using System.Text;

namespace CommonUtilities.Utilities;

public class SHA256Utilities
{
    public string ComputeSHA256Hash(string key)
    {
        string hashValue = "";
        SHA256 objSHA256 = null;
        byte[] hashbuffer = null;

        try
        {
            objSHA256 = new SHA256Managed();
            objSHA256.ComputeHash(Encoding.ASCII.GetBytes(key));
            hashbuffer = objSHA256.Hash;
            hashValue = WriteHex(hashbuffer).ToLower();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objSHA256.Dispose();
        }

        return hashValue;
    }

    private string WriteHex(byte[] array)
    {
        string hex = "";
        int count;

        for (count = 0; count < array.Length; count++) hex += array[count].ToString("X2");

        return hex;
    }
}