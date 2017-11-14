using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace Sma.Stm.Common.Web
{
    public class TextMediaTypeFormatter : TextInputFormatter
    {
        public TextMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/javascript"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            try
            {
                var request = context.HttpContext.Request;
                var contentType = context.HttpContext.Request.ContentType;

                using (var reader = new StreamReader(request.Body))
                {
                    var content = await reader.ReadToEndAsync();
                    return await InputFormatterResult.SuccessAsync(content);
                }
            }
            catch (Exception ex)
            {
                return await InputFormatterResult.FailureAsync();
            }
        }
    }
}
