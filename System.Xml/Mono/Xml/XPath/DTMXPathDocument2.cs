using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class DTMXPathDocument2 : IXPathNavigable
	{
		private readonly XPathNavigator root;

		internal readonly XmlNameTable NameTable;

		internal readonly DTMXPathLinkedNode2[] Nodes;

		internal readonly DTMXPathAttributeNode2[] Attributes;

		internal readonly DTMXPathNamespaceNode2[] Namespaces;

		internal readonly string[] AtomicStringPool;

		internal readonly string[] NonAtomicStringPool;

		internal readonly Hashtable IdTable;

		public DTMXPathDocument2(XmlNameTable nameTable, DTMXPathLinkedNode2[] nodes, DTMXPathAttributeNode2[] attributes, DTMXPathNamespaceNode2[] namespaces, string[] atomicStringPool, string[] nonAtomicStringPool, Hashtable idTable)
		{
			this.Nodes = nodes;
			this.Attributes = attributes;
			this.Namespaces = namespaces;
			this.AtomicStringPool = atomicStringPool;
			this.NonAtomicStringPool = nonAtomicStringPool;
			this.IdTable = idTable;
			this.NameTable = nameTable;
			this.root = new DTMXPathNavigator2(this);
		}

		public XPathNavigator CreateNavigator()
		{
			return this.root.Clone();
		}
	}
}
