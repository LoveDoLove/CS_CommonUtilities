using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CommonUtilities.Models;
using CommonUtilities.Response.Aws;
using CommonUtilities.Utilities;
using Serilog;

namespace CommonUtilities.Services;

public class AwsServices : Aws
{
    public async Task<AwsRepositoryMetadataResponse> CallCreateRepositories(string repositoryName, string directoryPath)
    {
        Log.Information($"Start: {nameof(CallCreateRepositories)}");
        string awsPath = "aws";
        string awsCliCommand = $"codecommit create-repository --repository-name {repositoryName}";
        using Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = awsPath,
                Arguments = awsCliCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = directoryPath
            }
        };
        StringBuilder outputBuilder = new StringBuilder();
        StringBuilder errorBuilder = new StringBuilder();
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        string output = outputBuilder.ToString();
        string error = errorBuilder.ToString();
        Console.WriteLine("AWS CLI command executed.");
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Error: {error}");
            throw new Exception($"AWS CLI command failed. Error: {error}");
        }

        AwsRepositoryMetadataResponse response = JsonSerializer.Deserialize<AwsRepositoryMetadataResponse>(output);
        Log.Information($"End: {nameof(CallCreateRepositories)}");
        return response;
    }

    public async Task<AwsListRepoResponse> CallListRepositories()
    {
        Log.Information($"Start: {nameof(CallListRepositories)}");
        string awsPath = "aws";
        string awsCliCommand = "codecommit list-repositories";
        using Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = awsPath,
                Arguments = awsCliCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        StringBuilder outputBuilder = new StringBuilder();
        StringBuilder errorBuilder = new StringBuilder();
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        string output = outputBuilder.ToString();
        string error = errorBuilder.ToString();
        Console.WriteLine("AWS CLI command executed.");
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Error: {error}");
            throw new Exception($"AWS CLI command failed. Error: {error}");
        }

        AwsListRepoResponse response = JsonSerializer.Deserialize<AwsListRepoResponse>(output);
        Log.Information($"End: {nameof(CallListRepositories)}");
        return response;
    }

    public async Task<AwsRepositoryMetadataResponse> CallGetRepositories(string repositoryName)
    {
        Log.Information($"Start: {nameof(CallGetRepositories)}");
        string awsPath = "aws";
        string awsCliCommand = $"codecommit get-repository --repository-name {repositoryName}";
        using Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = awsPath,
                Arguments = awsCliCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        StringBuilder outputBuilder = new StringBuilder();
        StringBuilder errorBuilder = new StringBuilder();
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        string output = outputBuilder.ToString();
        string error = errorBuilder.ToString();
        Console.WriteLine("AWS CLI command executed.");
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Error: {error}");
            throw new Exception($"AWS CLI command failed. Error: {error}");
        }

        AwsRepositoryMetadataResponse response = JsonSerializer.Deserialize<AwsRepositoryMetadataResponse>(output);
        Log.Information($"End: {nameof(CallGetRepositories)}");
        return response;
    }

    public async Task CallCloneRepositories()
    {
        Console.Clear();
        Log.Information($"Start: {nameof(CallCloneRepositories)}");
        try
        {
            AwsListRepoResponse awsListRepoResponse = await CallListRepositories();
            foreach (Repository repository in awsListRepoResponse.repositories)
            {
                AwsRepositoryMetadataResponse awsRepositoryMetadataResponse =
                    await CallGetRepositories(repository.repositoryName);
                Log.Information(
                    $"Repository Id: {repository.repositoryId}, Repository Name: {repository.repositoryName}");
                string replaceAwsUrl =
                    AwsUtilities.AutoAddCredentialsInLink(awsData.awsCodeCommit, awsRepositoryMetadataResponse);
                await GitServices.CloneRepositoryTask("", "", $"{replaceAwsUrl}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        Log.Information($"End: {nameof(CallCloneRepositories)}");
    }

    public async Task CallPushRepositories()
    {
        Console.Clear();
        Log.Information($"Start: {nameof(CallPushRepositories)}");
        try
        {
            List<string> gitDirectories =
                await GitUtilities.ReadGitDirectory(Directory.GetCurrentDirectory() + "/Repos/");
            AwsListRepoResponse awsListRepoResponse = await CallListRepositories();

            foreach (string gitDirectory in gitDirectories)
            {
                string gitUrlPath = GitUtilities.ConvertToGitSshFormat(gitDirectory);
                Log.Information($"Processing: {gitUrlPath}");

                string gitName = AwsUtilities.NameLimitRename(gitUrlPath).Replace(".git", "");

                Log.Information($"Checking existence of repository '{gitName}' in AWS...");

                // Check if the repository already exists in AWS
                if (awsListRepoResponse.repositories.Any(repo =>
                        repo.repositoryName.Equals(gitName, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.Information($"Repository '{gitName}' already exists in AWS. Skipping creation.");
                    AwsRepositoryMetadataResponse awsRepositoryMetadataResponse = await CallGetRepositories(gitName);
                    string replaceAwsUrl =
                        AwsUtilities.AutoAddCredentialsInLink(awsData.awsCodeCommit, awsRepositoryMetadataResponse);
                    await GitServices.PushRepositoryTask(gitDirectory, replaceAwsUrl);
                }
                else
                {
                    Log.Information($"Repository '{gitName}' does not exist in AWS. Proceeding with creation.");

                    AwsRepositoryMetadataResponse awsRepositoryMetadataResponse =
                        await CallCreateRepositories(gitName, gitDirectory);
                    string replaceAwsUrl =
                        AwsUtilities.AutoAddCredentialsInLink(awsData.awsCodeCommit, awsRepositoryMetadataResponse);
                    await GitServices.PushRepositoryTask(gitDirectory, replaceAwsUrl);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        Log.Information($"End: {nameof(CallPushRepositories)}");
    }
}