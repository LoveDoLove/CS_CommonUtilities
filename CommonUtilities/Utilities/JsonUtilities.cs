using RestSharp;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CommonUtilities.Utilities;

public class JsonUtilities
{
    public static Task<T> GetJsonResponseWithBearer<T>(string clientLink, string requestLink, string accessToken)
    {
        try
        {
            RestClient client = new RestClient(clientLink);
            RestRequest request = new RestRequest(requestLink);
            request.AddHeader("PRIVATE-TOKEN", accessToken);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    T result = JsonSerializer.Deserialize<T>(response.Content);
                    if (result != null)
                        return Task.FromResult(result);
                }

                Log.Error("Content Is Empty!");
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

        return Task.FromResult<T>(default);
    }

    public static Task<List<T>> GetJsonResponseListWithBearer<T>(string clientLink, string requestLink,
        string accessToken)
    {
        try
        {
            RestClient client = new RestClient(clientLink);
            RestRequest request = new RestRequest(requestLink);
            request.AddHeader("PRIVATE-TOKEN", accessToken);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    List<T> result = JsonSerializer.Deserialize<List<T>>(response.Content);
                    if (result != null && result.Count != 0)
                        return Task.FromResult(result);
                }

                Log.Error("Content Is Empty!");
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

        return Task.FromResult(new List<T>());
    }
}