using System;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal struct DTMXPathLinkedNode
	{
		public int FirstChild;

		public int Parent;

		public int PreviousSibling;

		public int NextSibling;

		public int FirstAttribute;

		public int FirstNamespace;

		public XPathNodeType NodeType;

		public string BaseURI;

		public bool IsEmptyElement;

		public string LocalName;

		public string NamespaceURI;

		public string Prefix;

		public string Value;

		public string XmlLang;

		public int LineNumber;

		public int LinePosition;
	}
}
