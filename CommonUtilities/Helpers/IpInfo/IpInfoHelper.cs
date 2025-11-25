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

using System.Text.Json;
using CommonUtilities.Utilities.System;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.IpInfo;

/// <summary>
///     Provides helper methods for retrieving IP information and extracting client IP addresses from HTTP requests.
///     This version uses the dual-stack endpoint (https://ipinfo.io/) to safely retrieve full IP info
///     for both IPv4 and IPv6 clients, even if the server is IPv4-only.
/// </summary>
public class IpInfoHelper : IIpInfoHelper
{
    // Use dual-stack endpoint to support both IPv4 and IPv6 client IPs
    private const string IpInfoUrl = "https://ipinfo.io/";
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
    ///     Supports both IPv4 and IPv6 client IPs and avoids server DNS/socket issues.
    /// </summary>
    /// <param name="context">The HTTP context containing the client request.</param>
    /// <returns>An <see cref="IpInfoResponse" /> with details about the client's IP address, or null if not found.</returns>
    public async Task<IpInfoResponse?> GetIpInfo(HttpContext context)
    {
        string clientIp = GetClientIp(context);

        // Construct the URL with client IP and token
        string url = $"{IpInfoUrl}{clientIp}?token={_ipInfo.Token}";

        try
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();

            IpInfoResponse? ipInfo = JsonSerializer.Deserialize<IpInfoResponse>(responseContent);
            return ipInfo;
        }
        catch (Exception ex)
        {
            // Log exception here if needed
            LoggerUtilities.Error(ex, $"Failed to retrieve IP info for {clientIp}");
            return null;
        }
    }

    /// <summary>
    ///     Extracts the client's IP address from the HTTP context, considering common proxy headers.
    ///     Returns IPv4 or IPv6 addresses as-is.
    /// </summary>
    /// <param name="context">The HTTP context containing the client request.</param>
    /// <returns>The client's IP address as a string.</returns>
    public string GetClientIp(HttpContext context)
    {
        string? ipAddress = "UNKNOWN";

        // Check common headers for the original client IP
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

            // If multiple IPs, take the first one (original client IP)
            ipAddress = headerValue.Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(ipAddress)) break;
        }

        // Fallback to connection remote IP
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = context.Connection.RemoteIpAddress?.ToString();

        // Normalize localhost IPv6 to IPv4 for consistency
        if (ipAddress == "::1") ipAddress = "127.0.0.1";

        return ipAddress ?? "UNKNOWN";
    }
}