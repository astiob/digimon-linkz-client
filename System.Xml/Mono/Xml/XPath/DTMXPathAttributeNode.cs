using System;

namespace Mono.Xml.XPath
{
	internal struct DTMXPathAttributeNode
	{
		public int OwnerElement;

		public int NextAttribute;

		public string LocalName;

		public string NamespaceURI;

		public string Prefix;

		public string Value;

		public int LineNumber;

		public int LinePosition;
	}
}
