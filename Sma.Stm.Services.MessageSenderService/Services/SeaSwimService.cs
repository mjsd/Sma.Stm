using Newtonsoft.Json;
using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Sma.Stm.Common;

namespace Sma.Stm.Services.GenericMessageService.Services
{
    public class SeaSwimService
    {
        public static HttpStatusCode CallService(string body, string endpoint, string method = "GET", List<Ssc.Header> headers = null)
        {
            var callServiceObject = new Ssc.CallServiceRequestObj
            {
                Body = body,
                EndpointMethod = endpoint,
                RequestType = method,
                Headers = headers
            };

            var callServiceHeaders = new WebHeaderCollection
            {
                { "Content-type", Constants.CONTENT_TYPE_APPLICATION_JSON }
            };

            var response = WebRequestHelper.Post("http://sma.stm.ssc.services.private/api/v1/scc/callService", JsonConvert.SerializeObject(callServiceObject), callServiceHeaders);
            return response.HttpStatusCode;
        }
    }
}