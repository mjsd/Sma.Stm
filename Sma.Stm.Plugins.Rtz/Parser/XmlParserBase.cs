using System.Collections.Generic;
using System.Xml;

namespace Sma.Stm.Plugins.Rtz.Parser
{
    public class XmlParserBase
    {
        private readonly XmlDocument _document;
        private XmlNamespaceManager _namespaceManager;

        protected XmlParserBase(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            _document = doc;
        }

        protected void SetNamespaces (SortedList<string, string> namespaces = null)
        {
            _namespaceManager = new XmlNamespaceManager(_document.NameTable);
            if (namespaces == null)
                return;

            foreach (var item in namespaces)
                _namespaceManager.AddNamespace(item.Key, item.Value);
        }

        protected string GetXml(string xpath)
        {
            var node = GetNode(xpath);
            return node == null ? string.Empty : node.OuterXml;
        }

        protected string GetValue(string xpath)
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
