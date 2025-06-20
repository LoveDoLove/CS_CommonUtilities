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