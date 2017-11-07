using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Ssc
{
    public class SeaSwimInstanceContextService
    {
        public SeaSwimInstanceContextService()
        {
        }

        public string CallerOrgId { get; set; }

        public string CallerServiceId { get; set; }

    }
}
