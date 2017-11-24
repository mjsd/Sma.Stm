using Sma.Stm.Common.PlugIns.ExtendedValidation;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sma.Stm.Common;

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

            return result;
        }
    }
}
