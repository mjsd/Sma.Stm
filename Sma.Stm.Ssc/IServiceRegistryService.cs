using Sma.Stm.Common.Web;
using System.Net;
using Sma.Stm.Ssc.Contract;

namespace Sma.Stm.Ssc
{
    public interface IServiceRegistryService
    {
        WebRequestHelper.WebResponse MakeGenericCall(string url, string method, string body = null, WebHeaderCollection headers = null);

        WebRequestHelper.WebResponse FindServices(FindServicesRequestObj data);
    }
}
