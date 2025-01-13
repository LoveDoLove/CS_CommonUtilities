using Serilog;

namespace CommonUtilities.Utilities;

public static class FileUtilities
{
    public static bool DeleteFolder(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            foreach (string filePath in Directory.GetFiles(directoryPath))
                File.SetAttributes(filePath, FileAttributes.Normal);

            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath)) DeleteFolder(subDirectoryPath);

            File.SetAttributes(directoryPath, FileAttributes.Normal);

            Directory.Delete(directoryPath, true);

            Log.Information($"Directory deleted: {directoryPath}");
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw new Exception($"Error deleting directory: {directoryPath}", ex);
        }
    }

    public static bool WriteFile(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            Log.Information($"Data stored in {path}");
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw new Exception($"Error writing file: {path}", ex);
        }
    }
}