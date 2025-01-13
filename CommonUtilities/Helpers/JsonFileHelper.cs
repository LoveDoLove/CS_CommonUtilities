using System.Text.Json;
using CommonUtilities.Utilities;

namespace CommonUtilities.Helpers;

public static class JsonFileHelper
{
    private const string SupportedFileType = ".json";

    public static string ReadJsonFile(string filePath, string? aesKey = null, string? iv = null)
    {
        try
        {
            if (!File.Exists(filePath)) throw new Exception($"File not found: {filePath}");

            string fileType = Path.GetExtension(filePath);
            if (fileType != SupportedFileType) throw new Exception($"Invalid file type: {fileType}");

            if (new FileInfo(filePath).Length == 0) throw new Exception($"File is empty: {filePath}");

            using StreamReader fileReader = File.OpenText(filePath);
            string fileData = fileReader.ReadToEnd();

            if (aesKey == null || iv == null) return fileData;

            string decryptedFileData = AesUtilities.Aes256CbcDecrypt(fileData, aesKey, iv);

            return decryptedFileData;
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading file: {e.Message}");
        }
    }

    public static bool SaveFileAsJson(string filePath, object data, string? aesKey = null, string? iv = null)
    {
        try
        {
            string serializedData = JsonSerializer.Serialize(data);

            if (aesKey == null || iv == null)
            {
                FileUtilities.WriteFile(filePath, serializedData);
                return true;
            }

            string encryptedData =
                AesUtilities.Aes256CbcEncrypt(serializedData, Common.Common.AesKey, Common.Common.InitVector);

            FileUtilities.WriteFile(filePath, encryptedData);

            return true;
        }
        catch (Exception e)
        {
            throw new Exception($"Error saving file: {e.Message}");
        }
    }
}