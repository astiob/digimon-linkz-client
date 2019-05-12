using System;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal struct DTMXPathLinkedNode2
	{
		public int FirstChild;

		public int Parent;

		public int PreviousSibling;

		public int NextSibling;

		public int FirstAttribute;

		public int FirstNamespace;

		public XPathNodeType NodeType;

		public int BaseURI;

		public bool IsEmptyElement;

		public int LocalName;

		public int NamespaceURI;

		public int Prefix;

		public int Value;

		public int XmlLang;

		public int LineNumber;

		public int LinePosition;
	}
}
