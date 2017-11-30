using System;
using System.Xml;

namespace Sma.Stm.Common.Xml
{
    public class XmlParser
    {
        private readonly XmlDocument _document;
        private XmlNamespaceManager _namespaceManager;

        public XmlParser(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            _document = doc;
        }

        public void SetNamespaces(string namespaces = null)
        {
            _namespaceManager = new XmlNamespaceManager(_document.NameTable);
            if (namespaces != null)
            {
                var rows = namespaces.Split("|");
                foreach (var row in rows)
                {
                    var keyValue = row.Split(";");
                    if (keyValue == null || keyValue.Length != 2)
                        throw new Exception($"Invalid namespace configuration {namespaces}");

                    _namespaceManager.AddNamespace(keyValue[0], keyValue[1]);
                }
            }
        }

        public string GetXml(string xpath)
        {
            var node = GetNode(xpath);
            return node == null ? string.Empty : node.OuterXml;
        }

        public string GetValue(string xpath)
        {
            var node = GetNode(xpath);
            return node == null ? string.Empty : node.InnerText;
        }

        private XmlNode GetNode(string xpath)
        {
            XmlNode root = _document.DocumentElement;
            var node = _namespaceManager != null ? root.SelectSingleNode(xpath, _namespaceManager) : root.SelectSingleNode(xpath);
            return node;
        }
    }
}
