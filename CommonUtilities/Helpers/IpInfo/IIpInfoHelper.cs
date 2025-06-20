using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.IpInfo;

public interface IIpInfoHelper
{
    Task<IpInfoResponse?> GetIpInfo(HttpContext context);
    string GetClientIp(HttpContext context);
}