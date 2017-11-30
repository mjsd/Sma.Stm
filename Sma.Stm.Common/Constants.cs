namespace Sma.Stm.Common
{
    public static class Constants
    {
        // Regex
        public const string UvidFormat = @"^urn:mrn:stm:voyage:id:[\w-]+:[\w-]+$";
        public const string OrgMrnFormat = @"^urn:mrn:stm:org:[\w-]+$";
        public const string ServiceMrnFormat = @"^urn:mrn:stm:service:instance:[\w-]+:[\w-]+$";
        public const string ServiceFreetextFormat = @"^[a-zA-Z0-9 +_:\-,.]*$";

        // Content types
        public const string ContentTypeTextXml = "text/xml; charset=utf-8";
        public const string ContentTypeApplicationJson = "application/json; charset=utf-8";
    }
}
