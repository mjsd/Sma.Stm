using Newtonsoft.Json;
using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Net;
using Sma.Stm.Ssc.Contract;

namespace Sma.Stm.Ssc
{
    public class SeaSwimConnectorPrivateService
    {
        private readonly IdentityRegistryService _identityRegistryService;
        private readonly ServiceRegistryService _serviceRegistryService;

        public SeaSwimConnectorPrivateService(IdentityRegistryService identityRegistryService,
            ServiceRegistryService serviceRegistryService)
        {
            _identityRegistryService = identityRegistryService ?? throw new ArgumentNullException(nameof(identityRegistryService));
            _serviceRegistryService = serviceRegistryService ?? throw new ArgumentNullException(nameof(serviceRegistryService));
        }

        public virtual CallServiceResponseObj CallService(CallServiceRequestObj data)
        {
            var result = new CallServiceResponseObj();

            var url = data.EndpointMethod;

            var headers = string.Empty;
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

            switch (data.RequestType)
            {
                case "GET":
                    response = WebRequestHelper.Get(url, headerCollection, true);
                    break;
                case "POST":
                    response = WebRequestHelper.Post(url, data.Body, headerCollection, true);
                    break;
                case "DELETE":
                    response = WebRequestHelper.Delete(url, data.Body, headerCollection, true);
                    break;
                case "PUT":
                    response = WebRequestHelper.Put(url, data.Body, headerCollection, true);
                    break;
                default:
                    throw new Exception($"The http method {data.RequestType} is not supported.");
            }

            result.Body = response.Body;
            result.StatusCode = (int)response.HttpStatusCode;
            return result;
        }

        public virtual FindIdentitiesResponseObj FindIdentities()
        {
            var result = new FindIdentitiesResponseObj();

            try
            {
                const string url = "/orgs?page=0&size=1000";

                var response = _identityRegistryService.MakeGenericCall(url, "GET");
                if (response.HttpStatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Body) ||
                    response.Body.Length <= 35)
                    return result;

                var responseObj = JsonConvert.DeserializeObject<IdRegistryResponeObject>(response.Body);
                result.Organizations = responseObj.Content;
                result.StatusMessage = response.ErrorMessage;
                result.StatusCode = (int)response.HttpStatusCode;

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
                const string msg = "Invalid request.";
                throw new Exception(msg);
            }

            var result = new FindServicesResponseObj();

            try
            {
                var serviceRegService = _serviceRegistryService;
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