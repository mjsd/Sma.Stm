using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.Common.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Sma.Stm.Services.GenericMessageService.Controllers
{
    public abstract class MessageControllerBase : Controller
    {
        public static void Validate(string message, IHostingEnvironment env)
        {
            var schemaPath = Path.Combine(env.ContentRootPath, @"Schema/");
            var shcemas = new List<XmlSchema>();

            foreach (var schemaFile in Directory.EnumerateFiles(schemaPath, "*.xsd"))
            {
                var schema = XmlSchema.Read(new FileStream(schemaFile, FileMode.Open, FileAccess.Read), SchemaValidationEWventhandler);
                shcemas.Add(schema);
            }

            var validator = new SchemaValidator(shcemas);
            var errors = validator.ValidateXml(message);

            if (errors != null)
            {
                var sb = new StringBuilder();
                foreach (var error in errors)
                    sb.Append(string.Format("{0}\r\n", error));

                throw new XmlException(sb.ToString());
            }
        }

        private static void SchemaValidationEWventhandler(object sender, ValidationEventArgs e)
        {

        }
    }
}
