using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sma.Stm.Services.SubscriptionService.Services
{
    public class AuthorizationService
    {
        public static bool CheckAuthentication(string orgId, string dataId)
        {
            var response = WebRequestHelper.Get(string.Format("http://Sma.Stm.Services.AuthorizationService/api/v1/Authorization/Check?orgId={0}&dataId={1}", orgId, dataId));
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return bool.Parse(response.Body);
            }

            return false;
        }
    }
}