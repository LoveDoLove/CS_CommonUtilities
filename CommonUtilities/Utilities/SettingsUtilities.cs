using System.Text.Json;

namespace CommonUtilities.Utilities;

public static class SettingsUtilities
{
    public static string ReadSettings(string filePath)
    {
        if (!File.Exists(filePath)) return string.Empty;
        if (new FileInfo(filePath).Length == 0) return string.Empty;
        using StreamReader fileReader = File.OpenText(filePath);
        string encryptedData = fileReader.ReadToEnd();
        string decryptedData =
            AesUtilities.Aes256CbcDecrypt(encryptedData, Common.Common.AES_KEY, Common.Common.INIT_VECTOR);
        return decryptedData;
    }

    public static void SaveSettings(string filePath, object data)
    {
        string serializedData = JsonSerializer.Serialize(data);
        string encryptedData =
            AesUtilities.Aes256CbcEncrypt(serializedData, Common.Common.AES_KEY, Common.Common.INIT_VECTOR);
        FileUtilities.WriteFile(filePath, encryptedData);
    }
}