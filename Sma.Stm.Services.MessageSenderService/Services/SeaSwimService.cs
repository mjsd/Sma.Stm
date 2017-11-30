using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Sma.Stm.Common;
using Sma.Stm.Common.Web;
using Sma.Stm.Ssc.Contract;

namespace Sma.Stm.Services.MessageSenderService.Services
{
    public static class SeaSwimService
    {
        public static HttpStatusCode CallService(string body, string endpoint, string method = "GET", List<Header> headers = null)
        {
            var callServiceObject = new CallServiceRequestObj
            {
                Body = body,
                EndpointMethod = endpoint,
                RequestType = method,
                Headers = headers
            };

            var callServiceHeaders = new WebHeaderCollection
            {
                { "Content-type", Constants.ContentTypeApplicationJson }
            };

            var response = WebRequestHelper.Post("http://sma.stm.ssc.services.private/api/v1/scc/callService", JsonConvert.SerializeObject(callServiceObject), callServiceHeaders);
            return response.HttpStatusCode;
        }
    }
}