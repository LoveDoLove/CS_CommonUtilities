using System.Text.Json;
using CommonUtilities.Interfaces;
using CommonUtilities.Models.Response.IpInfo;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Services;

/// <summary>
///     Service for retrieving IP information using the ipinfo.io API.
/// </summary>
public class IpInfoService : IIpInfoService
{
    private const string IpV4InfoUrl = "https://ipinfo.io/";
    private const string IpV6InfoUrl = "https://v6.ipinfo.io/";
    private readonly IpInfo _ipInfo;

    /// <summary>
    ///     Initializes a new instance of the <see cref="IpInfoService" /> class.
    /// </summary>
    /// <param name="ipInfo">The IP information configuration containing the API token.</param>
    public IpInfoService(IpInfo ipInfo)
    {
        _ipInfo = ipInfo;
    }

    /// <summary>
    ///     Retrieves IP information for the client IP address from the provided HttpContext.
    /// </summary>
    /// <param name="context">The HttpContext containing the client request information.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation, with a result of <see cref="IpInfoResponse" /> containing the
    ///     IP details, or null if an error occurs.
    /// </returns>
    public async Task<IpInfoResponse?> GetIpInfo(HttpContext context)
    {
        string clientIp = GetClientIp(context);

        HttpClient client = new HttpClient();
        string url = clientIp.Contains(":") ? IpV6InfoUrl : IpV4InfoUrl;
        HttpResponseMessage response = await client.GetAsync($"{url}{clientIp}?token={_ipInfo.Token}");
        string responseContent = await response.Content.ReadAsStringAsync();

        IpInfoResponse? ipInfo = JsonSerializer.Deserialize<IpInfoResponse>(responseContent);
        return ipInfo;
    }

    /// <summary>
    ///     Gets the client's IP address from the HttpContext.
    ///     It checks various headers to find the most accurate IP address, especially when behind a proxy or load balancer.
    /// </summary>
    /// <param name="context">The HttpContext containing the client request information.</param>
    /// <returns>The client's IP address as a string, or "UNKNOWN" if it cannot be determined.</returns>
    public string GetClientIp(HttpContext context)
    {
        string? ipAddress = "UNKNOWN";

        string[] headersToCheck =
        {
            "CF-Connecting-IP",
            "True-Client-IP",
            "HTTP_CLIENT_IP",
            "HTTP_X_FORWARDED_FOR",
            "HTTP_X_FORWARDED",
            "HTTP_FORWARDED_FOR",
            "HTTP_FORWARDED",
            "REMOTE_ADDR",
            "X-Forwarded-For"
        };

        foreach (string header in headersToCheck)
        {
            string? headerValue = context.Request.Headers[header].FirstOrDefault();

            if (string.IsNullOrEmpty(headerValue)) continue;

            ipAddress = headerValue.Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(ipAddress)) break;
        }

        if (string.IsNullOrEmpty(ipAddress)) ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (ipAddress == "::1") ipAddress = "127.0.0.1";

        return ipAddress ?? "UNKNOWN";
    }
}