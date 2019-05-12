using System;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class XPathEditableDocument : IXPathNavigable
	{
		private XmlNode node;

		public XPathEditableDocument(XmlNode node)
		{
			this.node = node;
		}

		public XmlNode Node
		{
			get
			{
				return this.node;
			}
		}

		public XPathNavigator CreateNavigator()
		{
			return new XmlDocumentEditableNavigator(this);
		}
	}
}
