using System;

namespace Sma.Stm.Plugins.Rtz.Parser
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