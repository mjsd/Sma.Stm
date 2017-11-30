using Sma.Stm.Common.Web;
using System.Net;

namespace Sma.Stm.Services.GenericMessageService.Services
{
    public static class AuthorizationService
    {
        public static bool CheckAuthentication(string orgId, string dataId)
        {
            var response = WebRequestHelper.Get(string.Format("http://Sma.Stm.Services.AuthorizationService/api/v1/Authorization/Check?orgId={0}&dataId={1}", orgId, dataId));
            return response.HttpStatusCode == HttpStatusCode.OK && bool.Parse(response.Body);
        }
    }
}