using System.Text.RegularExpressions;

namespace Sma.Stm.Common
{
    public static class FormatValidator
    {
        public static bool IsValidOrgId(string orgId)
        {
            var regex = new Regex(Constants.OrgMrnFormat);
            return regex.IsMatch(orgId);
        }

        public static bool IsValidServiceId(string serviceId)
        {
            var regex = new Regex(Constants.ServiceMrnFormat);
            return regex.IsMatch(serviceId);
        }

        public static bool IsValidUvid(string uvid)
        {
            var regex = new Regex(Constants.UvidFormat);
            return regex.IsMatch(uvid);
        }

        public static bool IsValidFreeText(string freeText)
        {
            var regex = new Regex(Constants.ServiceFreetextFormat);
            return regex.IsMatch(freeText);
        }
    }
}