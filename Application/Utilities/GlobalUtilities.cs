using Domain.Common;
using Serilog;

namespace Application.Utilities;

public class GlobalUtilities
{
    public static void PressAnyKeyToContinue()
    {
        Console.WriteLine("\nPress any key to continue ...");
        Console.ReadKey();
    }

    public static string ReadLine(string question, string value = "")
    {
        string? input;
        while (true)
        {
            Console.WriteLine($"{question} (Enter X = Cancel)");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            if (input.ToUpper().Equals("X")) return value;

            break;
        }

        return input.Trim();
    }

    public static string ConvertToGitSshFormat(string localPath)
    {
        return localPath.Replace(Directory.GetDirectoryRoot(localPath), "").Replace("\\", "_");
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