using Microsoft.Extensions.Configuration;

namespace Sma.Stm.Ssc
{
    public class SeaSwimInstanceContextService
    {
        public SeaSwimInstanceContextService(IConfiguration configuration)
        {
        }

        public string CallerOrgId { get; set; }

        public string CallerServiceId { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}