using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.Common
{
    public static class Constants
    {
        // Regex
        public const string UVID_FORMAT = @"^urn:mrn:stm:voyage:id:[\w-]+:[\w-]+$";
        public const string ORG_MRN_FORMAT = @"^urn:mrn:stm:org:[\w-]+$";
        public const string SERVICE_MRN_FORMAT = @"^urn:mrn:stm:service:instance:[\w-]+:[\w-]+$";
        public const string SERVICE_FREETEXT_FORMAT = @"^[a-zA-Z0-9 +_:\-,.]*$";

        // Content types
        public const string CONTENT_TYPE_TEXT_XML = "text/xml; charset=utf-8";
        public const string CONTENT_TYPE_APPLICATION_JSON = "application/json; charset=utf-8";
    }
}
