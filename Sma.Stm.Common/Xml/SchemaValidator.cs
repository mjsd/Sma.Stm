using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Sma.Stm.Common.Xml
{
    public class SchemaValidator
    {
        private XmlSchemaSet _schemaSet;
        private IList<string> _validationErrors;

        public SchemaValidator(List<XmlSchema> schemas)
        {
            PrepareSchemaSet(schemas);
        }

        public IList<string> ValidateXml(string xmlToValidate, bool ignoreWarnings = false)
        {
            _validationErrors = null;

            if (_schemaSet == null)
            {
                throw new Exception("There is no schema to validate against!");
            }

            var xmlSettings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            if (!ignoreWarnings)
                xmlSettings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;

            xmlSettings.Schemas = _schemaSet;
            xmlSettings.ValidationEventHandler += ValidationErrorHandler;

            var xmlReader = XmlReader.Create(new StringReader(xmlToValidate), xmlSettings);
            while (xmlReader.Read() && (_validationErrors == null || _validationErrors.Count < 5))
            {
                // Do nothing
            }

            return _validationErrors;
        }

        private void ValidationErrorHandler(object sender, ValidationEventArgs e)
        {
            if (_validationErrors == null)
                _validationErrors = new List<string>();

            _validationErrors.Add($"{e.Severity}: {e.Message}");
        }

        private void PrepareSchemaSet(List<XmlSchema> schemas)
        {
            if (schemas == null)
                return;

            _schemaSet = new XmlSchemaSet();
            foreach (var schema in schemas)
            {
                _schemaSet.Add(schema);
            }

            _schemaSet.Compile();
        }
    }
}
