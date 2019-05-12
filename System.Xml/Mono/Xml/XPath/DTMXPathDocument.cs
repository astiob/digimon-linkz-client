using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class DTMXPathDocument : IXPathNavigable
	{
		private XPathNavigator root;

		public DTMXPathDocument(XmlNameTable nameTable, DTMXPathLinkedNode[] nodes, DTMXPathAttributeNode[] attributes, DTMXPathNamespaceNode[] namespaces, Hashtable idTable)
		{
			this.root = new DTMXPathNavigator(this, nameTable, nodes, attributes, namespaces, idTable);
		}

		public XPathNavigator CreateNavigator()
		{
			return this.root.Clone();
		}
	}
}
