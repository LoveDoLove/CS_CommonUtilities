using System.Diagnostics;
using RestSharp;
using Serilog;

namespace CommonUtilities.Services;

public class GitServices
{
    public static int ErrorCount { get; set; }
    public static int Cloned { get; set; }
    public static int Exists { get; set; }

    public static Task CreateRepositoryTask(string clientLink, string requestLink, string gitName, string accessToken,
        object requestBody)
    {
        try
        {
            Log.Information($"Creating repository: {gitName}");
            gitName = gitName.Replace(".git", "");
            RestClient client = new(clientLink);
            RestRequest request = new(requestLink, Method.Post);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("PRIVATE-TOKEN", $"{accessToken}");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(requestBody);
            RestResponse response = client.Execute(request);
            Log.Information(response.IsSuccessful
                ? $"Repository '{gitName}' created successfully!"
                : $"Failed to create repository: {response.Content}");
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return Task.CompletedTask;
    }

    public static Task CloneRepositoryTask(string groupPath, string repoPath, string gitSsh)
    {
        try
        {
            string groupFullPath = Directory.GetCurrentDirectory() + "/Repos/" + groupPath;
            string projectPath = Directory.GetCurrentDirectory() + "/Repos/" + repoPath + ".git";

            if (!Directory.Exists(groupFullPath)) Directory.CreateDirectory(groupFullPath);

            // be careful for this part
            if (Directory.Exists(projectPath))
            {
                //Log.Information($"Auto Removing Exists Directory: {projectPath}");
                //FileUtil.DeleteFolder(projectPath);
                Log.Information($"{gitSsh} Path Exists!!!");
                ++Exists;
                return Task.CompletedTask;
            }

            Log.Information($"Start Cloning: {gitSsh}");
            ProcessStartInfo processStartInfo = new()
            {
                FileName = "git", Arguments = $"clone --mirror {gitSsh}", WorkingDirectory = groupFullPath
            };
            Process process = new() { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();
            if (exitCode != 0)
            {
                ++ErrorCount;
                Log.Error($"ExitCode: {exitCode}");
                Log.Error($"Cloned Failed: {gitSsh}");
                if (ErrorCount <= 5)
                {
                    Log.Information($"Retrying in 5 seconds ({ErrorCount})");
                    Thread.Sleep(5000);
                    CloneRepositoryTask(groupPath, repoPath, gitSsh);
                }
                else
                {
                    ErrorCount = 0;
                    Log.Error($"Mention Failed To Pushed: {gitSsh}");
                }

                return Task.CompletedTask;
            }

            ErrorCount = 0;
            Log.Information($"Cloned Success: {gitSsh}");
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        ++Cloned;
        return Task.CompletedTask;
    }

    public static Task PushRepositoryTask(string gitDirectory, string gitSsh)
    {
        try
        {
            Log.Information($"Push Git: {gitSsh}");
            ProcessStartInfo processStartInfo = new()
            {
                FileName = "git", Arguments = $"push {gitSsh} --mirror", WorkingDirectory = gitDirectory
            };
            Process process = new() { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();
            if (exitCode != 0)
            {
                ++ErrorCount;
                Log.Error($"ExitCode: {exitCode}");
                Log.Error($"Pushed Failed: {gitSsh}");
                if (ErrorCount <= 5)
                {
                    Log.Information($"Retrying in 5 seconds ({ErrorCount})");
                    Thread.Sleep(5000);
                    PushRepositoryTask(gitDirectory, gitSsh);
                }
                else
                {
                    ErrorCount = 0;
                    Log.Error($"Mention Failed To Pushed: {gitSsh}");
                }

                return Task.CompletedTask;
            }

            ErrorCount = 0;
            Log.Information($"Pushed Success: {gitSsh}");
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return Task.CompletedTask;
    }
}