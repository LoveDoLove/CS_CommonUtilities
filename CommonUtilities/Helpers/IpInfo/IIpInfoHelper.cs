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

using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.IpInfo;

/// <summary>
///     Defines methods for retrieving IP information and extracting client IP addresses from HTTP requests.
/// </summary>
public interface IIpInfoHelper
{
    /// <summary>
    ///     Asynchronously retrieves IP information for the client making the HTTP request.
    /// </summary>
    /// <param name="context">The HTTP context containing the client request.</param>
    /// <returns>An <see cref="IpInfoResponse" /> with details about the client's IP address, or null if not found.</returns>
    Task<IpInfoResponse?> GetIpInfo(HttpContext context);

    /// <summary>
    ///     Extracts the client's IP address from the HTTP context, considering common proxy headers.
    /// </summary>
    /// <param name="context">The HTTP context containing the client request.</param>
    /// <returns>The client's IP address as a string.</returns>
    string GetClientIp(HttpContext context);
}