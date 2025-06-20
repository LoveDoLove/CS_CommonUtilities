// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.IpInfo;

/// <summary>
///     Provides helper methods for retrieving IP information and extracting client IP addresses from HTTP requests.
/// </summary>
public class IpInfoHelper : IIpInfoHelper
{
    private const string IpV4InfoUrl = "https://ipinfo.io/";
    private const string IpV6InfoUrl = "https://v6.ipinfo.io/";
    private readonly IpInfoConfig _ipInfo;

    /// <summary>
    ///     Initializes a new instance of the <see cref="IpInfoHelper" /> class with the specified configuration.
    /// </summary>
    /// <param name="ipInfo">The IP info configuration containing the API token.</param>
    public IpInfoHelper(IpInfoConfig ipInfo)
    {
        _ipInfo = ipInfo;
    }

    /// <summary>
    ///     Asynchronously retrieves IP information for the client making the HTTP request.
    /// </summary>
    /// <param name="context">The HTTP context containing the client request.</param>
    /// <returns>An <see cref="IpInfoResponse" /> with details about the client's IP address, or null if not found.</returns>
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
    ///     Extracts the client's IP address from the HTTP context, considering common proxy headers.
    /// </summary>
    /// <param name="context">The HTTP context containing the client request.</param>
    /// <returns>The client's IP address as a string.</returns>
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