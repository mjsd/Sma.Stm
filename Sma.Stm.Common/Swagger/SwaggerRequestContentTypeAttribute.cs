using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.Common.Swagger
{
    /// <summary>
    /// SwaggerResponseContentTypeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerRequestContentTypeAttribute : Attribute
    {
        /// <summary>
        /// SwaggerResponseContentTypeAttribute
        /// </summary>
        /// <param name="requestType"></param>
        public SwaggerRequestContentTypeAttribute(string requestType)
        {
            RequestType = requestType;
        }

        /// <summary>
        /// Response Content Type
        /// </summary>
        public string RequestType { get; private set; }

        /// <summary>
        /// Remove all other Response Content Types
        /// </summary>
        public bool Exclusive { get; set; }
    }
}
