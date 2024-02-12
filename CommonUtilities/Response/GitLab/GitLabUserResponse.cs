using CommonUtilities.Models;

namespace CommonUtilities.Response.GitLab;

public class GitLabUserResponse
{
    public int id { get; set; }
    public string username { get; set; }
    public string name { get; set; }
    public string state { get; set; }
    public bool locked { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
    public DateTime created_at { get; set; }
    public string bio { get; set; }
    public string location { get; set; }
    public object public_email { get; set; }
    public string skype { get; set; }
    public string linkedin { get; set; }
    public string twitter { get; set; }
    public string discord { get; set; }
    public string website_url { get; set; }
    public string organization { get; set; }
    public string job_title { get; set; }
    public object pronouns { get; set; }
    public bool bot { get; set; }
    public object work_information { get; set; }
    public object local_time { get; set; }
    public DateTime last_sign_in_at { get; set; }
    public DateTime confirmed_at { get; set; }
    public string last_activity_on { get; set; }
    public string email { get; set; }
    public int theme_id { get; set; }
    public int color_scheme_id { get; set; }
    public int projects_limit { get; set; }
    public DateTime current_sign_in_at { get; set; }
    public List<Identity> identities { get; set; }
    public bool can_create_group { get; set; }
    public bool can_create_project { get; set; }
    public bool two_factor_enabled { get; set; }
    public bool external { get; set; }
    public bool private_profile { get; set; }
    public string commit_email { get; set; }
    public object shared_runners_minutes_limit { get; set; }
    public object extra_shared_runners_minutes_limit { get; set; }
    public List<object> scim_identities { get; set; }
}