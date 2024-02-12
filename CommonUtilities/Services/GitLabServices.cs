using CommonUtilities.Models;
using CommonUtilities.Response.GitLab;
using CommonUtilities.Utilities;
using RestSharp;
using Serilog;

namespace CommonUtilities.Services;

public class GitLabServices : GitLab
{
    public async Task CallCloneGitLabPublicGroup()
    {
        Console.Clear();
        Log.Information($"Start: {nameof(CallCloneGitLabPublicGroup)}");
        try
        {
            Task<int> totalPublicPages = CallGetGitLabTotalPage($"{gitLabData.git_api_link}",
                "/projects", $"{gitLabData.access_token}");
            Log.Information($"Total group pages: {totalPublicPages.Result}");
            for (int i = 1; i <= totalPublicPages.Result; i++)
            {
                List<GitLabProjectResponse> projectResponsesList =
                    await JsonUtilities.GetJsonResponseListWithBearer<GitLabProjectResponse>(
                        $"{gitLabData.git_api_link}", $"/projects?page{i}", $"{gitLabData.access_token}");
                Log.Information($"Processing public page {i}");

                foreach (GitLabProjectResponse projectResponse in projectResponsesList)
                {
                    string pathWithNameSpace = $"{projectResponse.path_with_namespace}";
                    string sshUrlToRepo = $"{projectResponse.ssh_url_to_repo}";
                    string nameSpaceFullPath = $"{projectResponse.@namespace.full_path}";
                    Log.Information($"Path With NameSpace: {pathWithNameSpace}, SSH Url To Repo: {sshUrlToRepo}");
                    await GitServices.CloneRepositoryTask($"{nameSpaceFullPath}", $"{pathWithNameSpace}",
                        $"{sshUrlToRepo}");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        Log.Information($"End: {nameof(CallCloneGitLabPublicGroup)}");
    }

    public async Task CallCloneGitLabPrivateGroup()
    {
        Console.Clear();
        Log.Information($"Start: {nameof(CallCloneGitLabPrivateGroup)}");
        try
        {
            Task<int> totalGroupPages = CallGetGitLabTotalPage($"{gitLabData.git_api_link}",
                "/groups", $"{gitLabData.access_token}");
            Log.Information($"Total group pages: {totalGroupPages.Result}");
            for (int i = 1; i <= totalGroupPages.Result; i++)
            {
                List<GitLabGroupResponse> groupResponsesList =
                    await JsonUtilities.GetJsonResponseListWithBearer<GitLabGroupResponse>($"{gitLabData.git_api_link}",
                        $"/groups?page={i}", $"{gitLabData.access_token}");
                Log.Information($"Processing group page {i}");
                foreach (GitLabGroupResponse groupResponse in groupResponsesList)
                {
                    Log.Information($"Group ID: {groupResponse.id}, Group Path: {groupResponse.full_path}");
                    Task<int> totalInnerGroupPages = CallGetGitLabTotalPage($"{gitLabData.git_api_link}",
                        $"/groups/{groupResponse.id}/projects", $"{gitLabData.access_token}");
                    for (int j = 1; j <= totalInnerGroupPages.Result; j++)
                    {
                        List<GitLabGroupProjectResponse>? projectList =
                            await JsonUtilities.GetJsonResponseListWithBearer<GitLabGroupProjectResponse>(
                                $"{gitLabData.git_api_link}", $"/groups/{groupResponse.id}/projects?page={j}",
                                $"{gitLabData.access_token}");
                        if (projectList.Equals(null) || projectList.Count == 0) return;
                        foreach (GitLabGroupProjectResponse project in projectList)
                        {
                            Log.Information(
                                $"Path With NameSpace: {project.path_with_namespace}, SSH Url To Repo: {project.ssh_url_to_repo}");
                            await GitServices.CloneRepositoryTask($"{groupResponse.full_path}",
                                $"{project.path_with_namespace}", $"{project.ssh_url_to_repo}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        Log.Information($"End: {nameof(CallCloneGitLabPrivateGroup)}");
    }

    public async Task CallPushReposToGitLab()
    {
        Console.Clear();
        Log.Information($"Start: {nameof(CallPushReposToGitLab)}");
        try
        {
            GitLabUserResponse userData =
                await JsonUtilities.GetJsonResponseWithBearer<GitLabUserResponse>($"{gitLabData.git_api_link}", "/user",
                    $"{gitLabData.access_token}");
            string username = $"{userData.username}";
            List<string>? gitDirectories =
                await GitUtilities.ReadGitDirectory(Directory.GetCurrentDirectory() + "/Repos/");
            if (string.IsNullOrEmpty(username) || gitDirectories.Equals(null) || gitDirectories.Count == 0) return;

            foreach (string gitDirectory in gitDirectories)
            {
                string gitUrlPath = GitUtilities.ConvertToGitSshFormat(gitDirectory);
                Log.Information($"Processing: {gitUrlPath}");
                string gitName = GitUtilities.NameLimitRename(gitUrlPath);
                string gitSsh = $"git@{gitLabData.ip_address}:{username}/{gitName}";
                object requestBody = new { name = gitName.Replace(".git", "") };
                await GitServices.CreateRepositoryTask(gitLabData.git_api_link + "/projects", "", gitName,
                    $"{gitLabData.access_token}", requestBody);
                await GitServices.PushRepositoryTask(gitDirectory, gitSsh);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        Log.Information($"End: {nameof(CallPushReposToGitLab)}");
    }

    public Task<int> CallGetGitLabTotalPage(string clientLink, string requestLink, string accessToken)
    {
        Log.Information($"Start: {nameof(CallGetGitLabTotalPage)}");
        try
        {
            RestClient client = new(clientLink);
            RestRequest request = new(requestLink);
            request.AddHeader("PRIVATE-TOKEN", $"{accessToken}");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                if (response.Headers != null)
                {
                    HeaderParameter? totalPagesHeader = response.Headers.SingleOrDefault(h =>
                        h.Name != null && h.Name.Equals("x-total-pages", StringComparison.OrdinalIgnoreCase));
                    int totalPages = 1;
                    if (totalPagesHeader != null && totalPagesHeader.Value != null)
                        totalPages = int.TryParse(totalPagesHeader.Value.ToString(), out int pages) ? pages : 1;

                    return Task.FromResult(totalPages);
                }

                Log.Error("Headers Is Empty!");
            }
            else
            {
                Log.Error($"Status Code: {response.StatusCode} - {response.Content}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        Log.Information($"End: {nameof(CallGetGitLabTotalPage)}");
        return Task.FromResult(0);
    }
}