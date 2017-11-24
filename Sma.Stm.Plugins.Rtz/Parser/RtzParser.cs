using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sma.Stm.Plugins.Rtz
{
    public interface IRtzParser
    {
        string RouteStatus
        {
            get;
        }

        string RouteInfo
        {
            get;
        }

        string WayPoints
        {
            get;
        }

        string VesselVoyage
        {
            get;
        }

        DateTime? ValidityPeriodStart
        {
            get;
        }

        DateTime? ValidityPeriodStop
        {
            get;
        }
    }
}