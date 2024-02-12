using CommonUtilities.Common;
using Serilog;

namespace CommonUtilities.Utilities;

public class GitUtilities
{
    public static Task<List<string>> ReadGitDirectory(string directoryPath)
    {
        List<string> filteredDirectory = new();
        try
        {
            string[]? gitDirectories = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);
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

    public static string ConvertToGitSshFormat(string localPath)
    {
        return localPath.Replace(Directory.GetCurrentDirectory() + "/Repos/", "").Replace("\\", "_");
    }

    public static string NameLimitRename(string gitUrlPath)
    {
        if (gitUrlPath.Replace(".git", "").Length <= Constants.MaxUrlLength) return gitUrlPath;

        Log.Information("Git Path Over 100 Characters!!!");
        string[] dirStrings = gitUrlPath.Split('_');
        string firstDirString = dirStrings.First();
        string lastDirString = dirStrings.Last();
        string combinedString = firstDirString + "_0_0_" + lastDirString; // cant put ... in gitlab
        return combinedString;
    }
}