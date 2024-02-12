namespace CommonUtilities.Models;

public class Aws
{
    public static AwsData awsData { get; set; } = new();
}

public class AwsData
{
    public AwsConfigure awsConfigure { get; set; } = new();
    public AwsCodeCommit awsCodeCommit { get; set; } = new();
}

public class AwsConfigure
{
    public string access_key { get; set; }
    public string secret_access_key { get; set; }
    public string region_name { get; set; }
    public string output_format { get; set; }
}

public class AwsCodeCommit
{
    public string username { get; set; }
    public string password { get; set; }
}

public class Repository
{
    public string repositoryName { get; set; }
    public string repositoryId { get; set; }
}

public class RepositoryMetadata
{
    public string accountId { get; set; }
    public string repositoryId { get; set; }
    public string repositoryName { get; set; }
    public DateTime lastModifiedDate { get; set; }
    public DateTime creationDate { get; set; }
    public string cloneUrlHttp { get; set; }
    public string cloneUrlSsh { get; set; }
    public string Arn { get; set; }
    public string kmsKeyId { get; set; }
}