// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

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
    ///     Extracts the client IP address from an <see cref="HttpContext" />.
    ///     The method inspects several common proxy/CDN/load-balancer headers in priority order
    ///     (for example: CF-Connecting-IP, X-Forwarded-For) and uses the first non-empty
    ///     value it finds. If the header contains multiple comma-separated IPs, the first
    ///     address is selected. If none of the headers yield a value, it falls back to
    ///     <see cref="HttpContext.Connection.RemoteIpAddress" />. The returned address is
    ///     normalized: the IPv6 loopback "::1" is converted to the IPv4 loopback
    ///     "127.0.0.1", and IPv4-mapped IPv6 addresses (e.g. "::ffff:192.168.1.1") are
    ///     mapped back to their IPv4 representation.
    /// </summary>
    /// <param name="context">The current HTTP context (request + connection info).</param>
    /// <returns>
    ///     A string containing the best-effort client IP address. If none available the method
    ///     will return a string that may be "UNKNOWN" when no candidate is found.
    /// </returns>
    public string GetClientIp(HttpContext context)
    {
        string? ipAddress = "UNKNOWN";

        // These are common proxy / CDN / load-balancer headers; evaluated in priority order.
        string[] headersToCheck =
        {
            "CF-Connecting-IP",
            "True-Client-IP",
            "X-Forwarded-For",
            "HTTP_CLIENT_IP",
            "HTTP_X_FORWARDED_FOR",
            "HTTP_X_FORWARDED",
            "HTTP_FORWARDED_FOR",
            "HTTP_FORWARDED",
            "REMOTE_ADDR"
        };

        foreach (string header in headersToCheck)
            if (context.Request.Headers.TryGetValue(header, out StringValues values))
            {
                string? headerValue = values.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(headerValue))
                {
                    // Some headers provide multiple IPs separated by commas.
                    string firstIp = headerValue.Split(',').First().Trim();
                    if (!string.IsNullOrEmpty(firstIp))
                    {
                        ipAddress = firstIp;
                        break;
                    }
                }
            }

        // Fallback to RemoteIpAddress if no proxy headers provided a value.
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "UNKNOWN")
        {
            IPAddress? remote = context.Connection.RemoteIpAddress;
            if (remote != null) ipAddress = remote.ToString();
        }

        // Normalize IPv6 loopback
        if (ipAddress == "::1") ipAddress = "127.0.0.1";

        // Normalize IPv4-mapped IPv6 addresses (e.g., ::ffff:192.168.1.1)
        if (IPAddress.TryParse(ipAddress, out IPAddress? parsedAddress))
            if (parsedAddress.IsIPv4MappedToIPv6)
                ipAddress = parsedAddress.MapToIPv4().ToString();

        return ipAddress;
    }
}