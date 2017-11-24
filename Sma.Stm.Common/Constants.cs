using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.Common
{
    public static class Constants
    {
        public const string UVID_FORMAT = @"^urn:mrn:stm:voyage:id:[\w-]+:[\w-]+$";
        public const string ORG_MRN_FORMAT = @"^urn:mrn:stm:org:[\w-]+$";
        public const string SERVICE_MRN_FORMAT = @"^urn:mrn:stm:service:instance:[\w-]+:[\w-]+$";
        public const string SERVICE_FREETEXT_FORMAT = @"^[a-zA-Z0-9 +_:\-,.]*$";
    }
}
