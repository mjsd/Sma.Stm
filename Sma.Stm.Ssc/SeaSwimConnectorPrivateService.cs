using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sma.Stm.Ssc
{
    public class SeaSwimConnectorPrivateService
    {
        private readonly IdentityRegistryService _identityRegistryService;

        public SeaSwimConnectorPrivateService(IdentityRegistryService identityRegistryService)
        {
            _identityRegistryService = identityRegistryService ?? throw new ArgumentNullException(nameof(identityRegistryService));
        }

        public virtual CallServiceResponseObj CallService(CallServiceRequestObj data)
        {
            var result = new CallServiceResponseObj();

            var url = data.EndpointMethod;

            string headers = string.Empty;
            var headerCollection = new WebHeaderCollection();
            if (data.Headers != null)
            {
                foreach (var h in data.Headers)
                {
                    headers += h + " ";
                    headerCollection.Add(h.Key, h.Value);
                }
            }


            WebRequestHelper.WebResponse response = null;

            if (data.RequestType == "GET")
                response = WebRequestHelper.Get(url, headerCollection, true);
            else if (data.RequestType == "POST")
                response = WebRequestHelper.Post(url, data.Body, headerCollection, true);
            else if (data.RequestType == "DELETE")
                response = WebRequestHelper.Delete(url, data.Body, headerCollection, true);
            else if (data.RequestType == "PUT")
                response = WebRequestHelper.Put(url, data.Body, headerCollection, true);
            else
                throw new Exception(string.Format("The request type {0} is not supported.", data.RequestType));

            result.Body = response.Body;
            result.StatusCode = (int)response.HttpStatusCode;
            return result;
        }

        public virtual FindIdentitiesResponseObj FindIdentities()
        {
            var result = new FindIdentitiesResponseObj();

            try
            {
                var url = "/orgs?page=0&size=1000";

                var response = _identityRegistryService.MakeGenericCall(url, "GET");
                if (response.HttpStatusCode == HttpStatusCode.OK
                    && !string.IsNullOrEmpty(response.Body)
                    && response.Body.Length > 35)
                {
                    var responseObj = JsonConvert.DeserializeObject<IdRegistryResponeObject>(response.Body);
                    result.Organizations = responseObj.content;
                    result.StatusMessage = response.ErrorMessage;
                    result.StatusCode = (int)response.HttpStatusCode;
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual FindServicesResponseObj FindServices(FindServicesRequestObj request)
        {
            if (request == null)
            {
                string msg = "Invalid request.";
                throw new Exception(msg);
            }

            var result = new FindServicesResponseObj();

            try
            {
                var serviceRegService = new ServiceRegistryService();
                var response = serviceRegService.FindServices(request);

                if (response.HttpStatusCode == HttpStatusCode.OK
                    && !string.IsNullOrEmpty(response.Body))
                {
                    var responseObj = JsonConvert.DeserializeObject<List<ServiceInstance>>(response.Body);
                    result.ServicesInstances = responseObj;
                }

                result.StatusCode = (int)response.HttpStatusCode;
                result.StatusMessage = response.ErrorMessage;

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}