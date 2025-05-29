using CommonUtilities.Models.Response.IpInfo;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Interfaces;

/// <summary>
///     Defines the contract for a service that provides information about IP addresses,
///     including geolocation data and retrieving the client's IP address from an HTTP request.
/// </summary>
public interface IIpInfoService
{
    /// <summary>
    ///     Asynchronously retrieves detailed information for the client's IP address from the provided
    ///     <see cref="HttpContext" />.
    ///     This may involve an external API call to a geolocation service.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> of the current request, used to determine the client's IP address.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an <see cref="IpInfoResponse" />
    ///     object with details about the IP address if successful; otherwise, <c>null</c> if the information
    ///     could not be retrieved or if the IP address is invalid/local.
    /// </returns>
    Task<IpInfoResponse?> GetIpInfo(HttpContext context);

    /// <summary>
    ///     Gets the client's IP address from the provided <see cref="HttpContext" />.
    ///     This method attempts to correctly identify the client's IP, considering proxies and load balancers
    ///     by checking common headers like 'X-Forwarded-For'.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> of the current request.</param>
    /// <returns>
    ///     A string representing the client's IP address. Returns an empty string or a placeholder (e.g., "::1" for localhost)
    ///     if the IP address cannot be determined.
    /// </returns>
    string GetClientIp(HttpContext context);
}