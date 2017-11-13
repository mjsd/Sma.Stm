using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Sma.Stm.Ssc
{
    public class SeaSwimActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var instanceContextService = context.HttpContext.RequestServices.GetRequiredService<SeaSwimInstanceContextService>();

            var CallerOrgId = context.HttpContext.Request.Headers["SeaSWIM-CallerOrgId"];
            var CallerServiceId = context.HttpContext.Request.Headers["SeaSWIM-CallerServiceId"];

        }
    }
}
