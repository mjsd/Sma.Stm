using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sma.Stm.Common.PlugIns.ExtendedValidation;
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
        protected IConfiguration _configuration;

        protected void Validate(string message, IHostingEnvironment env)
        {
            // Validate schema
            ValidateSchema(message, Path.Combine(env.ContentRootPath, @"Schema/"));

            // Extended validation
            ExtendedValidation(message, Path.Combine(env.ContentRootPath, @"Plugins/"));
        }

        protected string GetDataId(string message)
        {
            var dataIdXPath = _configuration.GetValue<string>("DataIdXPath");
            var parser = new XmlParser(message);
            parser.SetNamespaces(_configuration.GetValue<string>("Namespaces"));

            return parser.GetValue(dataIdXPath);
        }

        protected string GetStatus(string message)
        {
            var dataIdXPath = _configuration.GetValue<string>("StatusXPath");
            var parser = new XmlParser(message);
            parser.SetNamespaces(_configuration.GetValue<string>("Namespaces"));

            return parser.GetValue(dataIdXPath);
        }

        private void ExtendedValidation(string message, string pluginPath)
        {
            var validator = new ExtendedValidationHandler(pluginPath);
            var errors = validator.Validate(message);

            if (errors != null && errors.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var error in errors)
                    sb.Append($"{ error}\r\n");

                throw new XmlException(sb.ToString());
            }
        }

        private void ValidateSchema(string message, string schemaPath)
        {
            var shcemas = new List<XmlSchema>();

            foreach (var schemaFile in Directory.EnumerateFiles(schemaPath, "*.xsd"))
            {
                var schema = XmlSchema.Read(new FileStream(schemaFile, FileMode.Open, FileAccess.Read), SchemaValidationEventhandler);
                shcemas.Add(schema);
            }

            var validator = new SchemaValidator(shcemas);
            var errors = validator.ValidateXml(message);

            if (errors != null && errors.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var error in errors)
                    sb.Append($"{error}\r\n");

                throw new XmlException(sb.ToString());
            }
        }

        private void SchemaValidationEventhandler(object sender, ValidationEventArgs e)
        {

        }
    }
}