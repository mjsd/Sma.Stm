using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Ssc
{
    public class SeaSwimInstanceContextService
    {
        public SeaSwimInstanceContextService(IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.GetValue<string>("CallerOrgId")))
            {
                CallerOrgId = configuration.GetValue<string>("CallerOrgId");
            }

            if (!string.IsNullOrEmpty(configuration.GetValue<string>("CallerServiceId")))
            {
                CallerServiceId = configuration.GetValue<string>("CallerServiceId");
            }
        }

        public string CallerOrgId { get; set; }

        public string CallerServiceId { get; set; }

    }
}
