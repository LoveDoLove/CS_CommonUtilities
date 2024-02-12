using Serilog;

namespace CommonUtilities.Utilities;

public static class FileUtilities
{
    public static void DeleteFolder(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath)) return;
            foreach (string filePath in Directory.GetFiles(directoryPath))
                File.SetAttributes(filePath, FileAttributes.Normal);

            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath)) DeleteFolder(subDirectoryPath);

            File.SetAttributes(directoryPath, FileAttributes.Normal);
            Directory.Delete(directoryPath, true);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    public static void WriteFile(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            Log.Information($"Data stored in {path}");
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
}