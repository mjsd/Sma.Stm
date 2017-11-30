using Microsoft.Extensions.Configuration;
using Sma.Stm.Common;
using Sma.Stm.Common.Web;
using System;
using System.Net;
using System.Text;
using System.Web;
using Sma.Stm.Ssc.Contract;

namespace Sma.Stm.Ssc
{
    public class ServiceRegistryService : IServiceRegistryService
    {
        private const string PathSearchGeneric = "/api/_search/serviceInstance?query=";
        private const string PathSearchGeoJson = "/api/_searchGeometryGeoJSON/serviceInstance?geometry=[GEOMETRY]&query=";
        private const string PathSearchWkt = "/api/_searchGeometryWKT/serviceInstance?geometry=[GEOMETRY]&query=";
        private const string PathAll = "/api/serviceInstance";

        private readonly string _serviceRegistryBasePath;
        private IdentityRegistryService _identityRegistryService;

        public ServiceRegistryService(IConfiguration configuration)
        {
            _serviceRegistryBasePath = configuration.GetValue<string>("ServiceRegistryBaseUrl");
        }

        public WebRequestHelper.WebResponse MakeGenericCall(string url, string method, string body = null, WebHeaderCollection headers = null)
        {
            WebRequestHelper.WebResponse response = null;

            url = _serviceRegistryBasePath + url;
            switch (method)
            {
                case "GET":
                    response = WebRequestHelper.Get(url, headers, false);
                    break;
                case "POST":
                    response = WebRequestHelper.Post(url, body, headers: headers, useCertificate: false);
                    break;
                default:
                    throw new Exception($"Http method {method} not allowed");
            }

            return response;
        }

        public WebRequestHelper.WebResponse FindServices(FindServicesRequestObj data)
        {
            var url = string.Empty;
            var isGeoSearch = false;

            if (data.Filter.CoverageArea != null
                && !string.IsNullOrEmpty(data.Filter.CoverageArea.Value))
            {
                switch (data.Filter.CoverageArea.CoverageType)
                {
                    case "WKT":
                        url = PathSearchWkt.Replace("[GEOMETRY]", HttpUtility.UrlEncode(data.Filter.CoverageArea.Value));
                        break;
                    case "GeoJSON":
                        url = PathSearchGeoJson.Replace("[GEOMETRY]", HttpUtility.UrlEncode(data.Filter.CoverageArea.Value));
                        break;
                    default:
                        throw new Exception($"Coverage area type {data.Filter.CoverageArea.CoverageType} is not valid");
                }

                isGeoSearch = true;
            }
            else
            {
                url = PathSearchGeneric;
            }

            var query = string.Empty;
            if (!string.IsNullOrEmpty(data.Filter.FreeText))
            {
                if (FormatValidator.IsValidFreeText(data.Filter.FreeText))
                {
                    query = data.Filter.FreeText.Replace(":", "\\:");
                    query = HttpUtility.UrlEncode(query);
                }
                else
                {
                    const string msg = "Forbidden character(s) in freetext search string.";
                    throw new Exception(msg);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(data.Filter.UnLoCode))
                {
                    query = AddToQuery(query, "unlocode", data.Filter.UnLoCode, "AND");
                }

                if (!string.IsNullOrEmpty(data.Filter.ServiceDesignId))
                {
                    query = AddToQuery(query, "designId", data.Filter.ServiceDesignId.Replace(":", "\\:"), "AND");
                }

                if (!string.IsNullOrEmpty(data.Filter.ServiceInstanceId))
                {
                    query = AddToQuery(query, "instanceId", data.Filter.ServiceInstanceId.Replace(":", "\\:"), "AND");
                }

                if (!string.IsNullOrEmpty(data.Filter.ServiceType))
                {
                    query = AddToQuery(query, "serviceType", data.Filter.ServiceType, "AND");
                }

                if (!string.IsNullOrEmpty(data.Filter.ServiceStatus))
                {
                    query = AddToQuery(query, "status", data.Filter.ServiceStatus, "AND");
                }

                if (!string.IsNullOrEmpty(data.Filter.Mmsi) && !string.IsNullOrEmpty(data.Filter.Imo))
                {
                    if (query == string.Empty)
                        query += "(";
                    else
                        query += " AND (";

                    query = AddToQuery(query, "mmsi", data.Filter.Mmsi, "OR");
                    query = AddToQuery(query, "imo", data.Filter.Imo, "OR");
                    query += ")";
                }
                else
                {
                    if (!string.IsNullOrEmpty(data.Filter.Mmsi))
                    {
                        query = AddToQuery(query, "mmsi", data.Filter.Mmsi, "AND");
                    }

                    if (!string.IsNullOrEmpty(data.Filter.Imo))
                    {
                        query = AddToQuery(query, "imo", data.Filter.Imo, "AND");
                    }
                }

                if (data.Filter.ServiceProviderIds != null && data.Filter.ServiceProviderIds.Count > 0)
                {
                    if (query == string.Empty)
                        query += "(";
                    else
                        query += " AND (";

                    foreach (var id in data.Filter.ServiceProviderIds)
                    {
                        query = AddToQuery(query, "organizationId", id.Replace(":", "\\:"), "OR");
                    }
                    query += ")";
                }

                if (data.Filter.ServiceProviderIds != null && data.Filter.ServiceProviderIds.Count > 0)
                {
                    if (query == string.Empty)
                        query += "(";
                    else
                        query += " AND (";

                    foreach (var id in data.Filter.ServiceProviderIds)
                    {
                        query = AddToQuery(query, "organizationId", id.Replace(":", "\\:"), "OR");
                    }
                    query += ")";
                }

                if (data.Filter.KeyWords != null && data.Filter.KeyWords.Count > 0)
                {
                    if (query == string.Empty)
                        query += "(";
                    else
                        query += " AND (";

                    foreach (var id in data.Filter.KeyWords)
                    {
                        query = AddToQuery(query, "keywords", id.Replace(":", "\\:"), "AND");
                    }
                    query += ")";
                }
            }

            if (string.IsNullOrEmpty(query) && !isGeoSearch)
            {
                url = PathAll;
            }

            if (data.Page != null)
            {
                if (url == PathAll)
                    query += "?page=" + data.Page.ToString();
                else
                    query += "&page=" + data.Page.ToString();
            }
            if (data.PageSize != null)
            {
                query += "&size=" + data.PageSize.ToString();
            }

            return MakeGenericCall(url + query, "GET");
        }

        private static string AddToQuery(string query, string key, string value, string op)
        {
            var sb = new StringBuilder(query);
            if (sb.Length > 0 && !query.EndsWith("("))
                sb.Append(string.Format(" {0} {1}:{2}", op, key, value));
            else
                sb.Append(string.Format("{0}:{1}", key, value));

            return sb.ToString();
        }
    }
}