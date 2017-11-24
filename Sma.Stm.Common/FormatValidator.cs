using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sma.Stm.Common
{
    public class FormatValidator
    {
        public static bool IsValidOrgId(string orgId)
        {
            Regex regex = new Regex(Constants.ORG_MRN_FORMAT);
            return regex.IsMatch(orgId);
        }

        public static bool IsValidServiceId(string serviceId)
        {
            Regex regex = new Regex(Constants.SERVICE_MRN_FORMAT);
            return regex.IsMatch(serviceId);
        }

        public static bool IsValidUvid(string uvid)
        {
            Regex regex = new Regex(Constants.UVID_FORMAT);
            return regex.IsMatch(uvid);
        }

        public static bool IsValidFreeText(string freeText)
        {
            Regex regex = new Regex(Constants.SERVICE_FREETEXT_FORMAT);
            return regex.IsMatch(freeText);
        }
    }
}