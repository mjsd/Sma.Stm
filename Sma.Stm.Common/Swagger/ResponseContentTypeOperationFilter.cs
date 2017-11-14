using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Sma.Stm.Common.Swagger
{
    public class ResponseContentTypeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var requestAttributes = context.ApiDescription.ActionAttributes().OfType<SwaggerResponseContentTypeAttribute>().FirstOrDefault();

            if (requestAttributes != null)
            {
                if (requestAttributes.Exclusive)
                    operation.Produces.Clear();

                operation.Produces.Add(requestAttributes.ResponseType);
            }
        }
    }

}
