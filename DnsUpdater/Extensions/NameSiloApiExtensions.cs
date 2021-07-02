using DnsUpdater.NameSilo;

namespace DnsUpdater.Extensions
{
    public static class NameSiloApiExtensions
    {
        public static ApiResponse WarningOnFail(this ApiResponse resp)
        {
            if (resp.Reply.Code != NameSiloApi.API_SUCCESS)
            {
                Logger.Get().LogWarning($"Api response to '{resp.Request.Operation}' doesn't indicate success. " +
                                        $"Code: {resp.Reply.Code} | Details: {resp.Reply.Detail}");
            }
            
            return resp;
        }
    }
}