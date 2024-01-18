using Domain.Common;
using Serilog;

namespace Application.Utilities;

public class FileUtilities : Common
{
    public static Task<List<string>> ReadDirectory(string directoryPath)
    {
        List<string> filteredDirectory = new();
        try
        {
            string[]? gitDirectories =
                Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);
            if (gitDirectories.Equals(null) || gitDirectories.Length == 0)
                Log.Information("Directory List Empty!");
            else
                filteredDirectory.AddRange(gitDirectories.Where(gitDirectory =>
                    gitDirectory.EndsWith(".git") && !Directory.GetDirectories(gitDirectory).Length.Equals(0)));
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return Task.FromResult(filteredDirectory.Count > 0 ? filteredDirectory : new List<string>());
    }

    public static void DeleteDirectory(string directoryPath)
    {
        try
        {
            foreach (string filePath in Directory.GetFiles(directoryPath))
                File.SetAttributes(filePath, FileAttributes.Normal);

            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath))
                DeleteDirectory(subDirectoryPath);

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