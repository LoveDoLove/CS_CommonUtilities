using System.Diagnostics;
using CommonUtilities.Common;
using CommonUtilities.Models;
using CommonUtilities.Response.Aws;
using Serilog;

namespace CommonUtilities.Utilities;

public class AwsUtilities
{
    public static void SetupAwsConfigure(AwsConfigure awsConfigure)
    {
        Log.Information($"Start: {nameof(SetupAwsConfigure)}");
        string awsPath = "aws";
        string awsCliCommand = "configure";

        using Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = awsPath, Arguments = awsCliCommand, RedirectStandardInput = true
            }
        };
        process.Start();
        process.StandardInput.WriteLine(awsConfigure.access_key);
        process.StandardInput.WriteLine(awsConfigure.secret_access_key);
        process.StandardInput.WriteLine(awsConfigure.region_name);
        process.StandardInput.WriteLine(awsConfigure.output_format);
        process.StandardInput.Close();
        process.WaitForExit();
        Log.Information($"End: {nameof(SetupAwsConfigure)}");
    }

    public static string AutoAddCredentialsInLink(AwsCodeCommit awsData,
        AwsRepositoryMetadataResponse awsRepositoryMetadataResponse)
    {
        string linkWithCredentials = "https://" + awsData.username + ":" + awsData.password + "@";
        return awsRepositoryMetadataResponse.repositoryMetadata.cloneUrlHttp.Replace("https://", linkWithCredentials);
    }

    public static string NameLimitRename(string gitUrlPath)
    {
        if (gitUrlPath.Replace(".git", "").Length <= Constants.MaxUrlLength) return gitUrlPath;

        Log.Information("Git Path Over 100 Characters!!!");
        string[] dirStrings = gitUrlPath.Split('_');
        string firstDirString = dirStrings.First();
        string lastDirString = dirStrings.Last();
        string combinedString = firstDirString + "..." + lastDirString; // cant put ... in gitlab
        return combinedString;
    }
}