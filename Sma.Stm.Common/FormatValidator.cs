using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sma.Stm.Common
{
    public class FormatValidator
    {
        private const string UVID_FORMAT = @"^urn:mrn:stm:voyage:id:[\w-]+:[\w-]+$";
        private const string ORG_MRN_FORMAT = @"^urn:mrn:stm:org:[\w-]+$";
        private const string SERVICE_MRN_FORMAT = @"^urn:mrn:stm:service:instance:[\w-]+:[\w-]+$";
        private const string SERVICE_FREETEXT = @"^[a-zA-Z0-9 +_:\-,.]*$";

        public static bool IsValidOrgId(string orgId)
        {
            Regex regex = new Regex(ORG_MRN_FORMAT);
            return regex.IsMatch(orgId);
        }

        public static bool IsValidServiceId(string serviceId)
        {
            Regex regex = new Regex(SERVICE_MRN_FORMAT);
            return regex.IsMatch(serviceId);
        }

        public static bool IsValidUvid(string uvid)
        {
            Regex regex = new Regex(UVID_FORMAT);
            return regex.IsMatch(uvid);
        }

        public static bool IsValidFreeText(string freeText)
        {
            Regex regex = new Regex(SERVICE_FREETEXT);
            return regex.IsMatch(freeText);
        }
    }

}
