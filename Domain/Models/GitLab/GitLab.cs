namespace Domain.Models.GitLab;

public class GitLab : Common.Common
{
    public static string GitLabApiLink { get; set; } = "http://###/api/v4";
    public static GitLabData gitLabData { get; set; } = new();
}

public class GitLabData
{
    public string ip_address { get; set; }
    public string access_token { get; set; }
    public string git_api_link { get; set; }
}

public class ContainerExpirationPolicy
{
    public string cadence { get; set; }
    public bool enabled { get; set; }
    public int keep_n { get; set; }
    public string older_than { get; set; }
    public string name_regex { get; set; }
    public object name_regex_keep { get; set; }
    public DateTime next_run_at { get; set; }
}

public class Links
{
    public string self { get; set; }
    public string issues { get; set; }
    public string merge_requests { get; set; }
    public string repo_branches { get; set; }
    public string labels { get; set; }
    public string events { get; set; }
    public string members { get; set; }
    public string cluster_agents { get; set; }
}

public class Namespace
{
    public int id { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    public string kind { get; set; }
    public string full_path { get; set; }
    public int? parent_id { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
}

public class Owner
{
    public int id { get; set; }
    public string username { get; set; }
    public string name { get; set; }
    public string state { get; set; }
    public bool locked { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
}

public class Permissions
{
    public object project_access { get; set; }
    public object group_access { get; set; }
}

public class DefaultBranchProtectionDefaults
{
}

public class Identity
{
    public string provider { get; set; }
    public string extern_uid { get; set; }
    public object saml_provider_id { get; set; }
}