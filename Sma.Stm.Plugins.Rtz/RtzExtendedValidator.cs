using Sma.Stm.Common.PlugIns.ExtendedValidation;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sma.Stm.Common;
using System;

namespace Sma.Stm.Plugins.Rtz
{
    public class RtzExtendedValidator : IExtendedValidator
    {
        public List<string> Validate(string xml)
        {
            var result = new List<string>();
            var parser = RtzParserFactory.Create(xml);

            // Validate UVID
            var uvid = parser.VesselVoyage;
            if (string.IsNullOrEmpty(uvid))
            {
                result.Add("Vayage id is required");
            }
            else if (!new Regex(Constants.UVID_FORMAT).IsMatch(uvid))
            {
                result.Add("Invalid voyage id");
            }

            // Check for route info
            if (string.IsNullOrEmpty(parser.RouteInfo))
            {
                result.Add("Routeinfo is required");
            }

            // Check for waypoints
            if (string.IsNullOrEmpty(parser.WayPoints))
            {
                result.Add("Waypoints is required");
            }

            // If RTZ 1.1 check for STM extension
            if (parser is Rtz11Parser rtz11parser)
            {
                if (string.IsNullOrEmpty(rtz11parser.StmRouteInfoExtension))
                {
                    result.Add("StmRouteInfoExtension is required");
                }
            }

            // Check route status
            if (string.IsNullOrEmpty(parser.RouteStatus))
            {
                result.Add("RouteStatusEnum in STM extension is required");
            }
            else
            {
                int status = Convert.ToInt32(parser.RouteStatus);
                if (status < 1 || status > 8)
                {
                    result.Add("Invalid status value. Must be between 1 and 8");
                }
            }

            return result;
        }
    }
}