using CommonUtilities.Models;

namespace CommonUtilities.Response.Aws;

public class AwsListRepoResponse
{
    public List<Repository> repositories { get; set; }
}