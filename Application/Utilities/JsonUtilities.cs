using RestSharp;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Utilities;

public class JsonUtilities
{
    public static async Task<T> GetJsonResponseWithBearer<T>(string clientLink, string requestLink,
        string accessToken)
    {
        try
        {
            RestClient client = new RestClient(clientLink);
            RestRequest request = new RestRequest(requestLink);
            request.AddHeader("PRIVATE-TOKEN", accessToken);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    T result = JsonSerializer.Deserialize<T>(response.Content);
                    if (result != null)
                        return result;
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

        return default;
    }

    public static async Task<List<T>> GetJsonResponseListWithBearer<T>(string clientLink, string requestLink,
        string accessToken)
    {
        try
        {
            RestClient client = new RestClient(clientLink);
            RestRequest request = new RestRequest(requestLink);
            request.AddHeader("PRIVATE-TOKEN", accessToken);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    List<T> result = JsonSerializer.Deserialize<List<T>>(response.Content);
                    if (result != null && result.Count != 0)
                        return result;
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

        return new List<T>();
    }
}