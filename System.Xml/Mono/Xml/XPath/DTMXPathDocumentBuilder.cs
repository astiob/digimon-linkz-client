using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class DTMXPathDocumentBuilder
	{
		private XmlReader xmlReader;

		private XmlValidatingReader validatingReader;

		private XmlSpace xmlSpace;

		private XmlNameTable nameTable;

		private IXmlLineInfo lineInfo;

		private int nodeCapacity;

		private int attributeCapacity;

		private int nsCapacity;

		private DTMXPathLinkedNode[] nodes;

		private DTMXPathAttributeNode[] attributes;

		private DTMXPathNamespaceNode[] namespaces;

		private Hashtable idTable;

		private int nodeIndex;

		private int attributeIndex;

		private int nsIndex;

		private bool hasAttributes;

		private bool hasLocalNs;

		private int attrIndexAtStart;

		private int nsIndexAtStart;

		private int lastNsInScope;

		private bool skipRead;

		private int[] parentStack;

		private int parentStackIndex;

		public DTMXPathDocumentBuilder(string url) : this(url, XmlSpace.None, 200)
		{
		}

		public DTMXPathDocumentBuilder(string url, XmlSpace space) : this(url, space, 200)
		{
		}

		public DTMXPathDocumentBuilder(string url, XmlSpace space, int defaultCapacity)
		{
			this.parentStack = new int[10];
			base..ctor();
			XmlReader xmlReader = null;
			try
			{
				xmlReader = new XmlTextReader(url);
				this.Init(xmlReader, space, defaultCapacity);
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
		}

		public DTMXPathDocumentBuilder(XmlReader reader) : this(reader, XmlSpace.None, 200)
		{
		}

		public DTMXPathDocumentBuilder(XmlReader reader, XmlSpace space) : this(reader, space, 200)
		{
		}

		public DTMXPathDocumentBuilder(XmlReader reader, XmlSpace space, int defaultCapacity)
		{
			this.parentStack = new int[10];
			base..ctor();
			this.Init(reader, space, defaultCapacity);
		}

		private void Init(XmlReader reader, XmlSpace space, int defaultCapacity)
		{
			this.xmlReader = reader;
			this.validatingReader = (reader as XmlValidatingReader);
			this.lineInfo = (reader as IXmlLineInfo);
			this.xmlSpace = space;
			this.nameTable = reader.NameTable;
			this.nodeCapacity = defaultCapacity;
			this.attributeCapacity = this.nodeCapacity;
			this.nsCapacity = 10;
			this.idTable = new Hashtable();
			this.nodes = new DTMXPathLinkedNode[this.nodeCapacity];
			this.attributes = new DTMXPathAttributeNode[this.attributeCapacity];
			this.namespaces = new DTMXPathNamespaceNode[this.nsCapacity];
			this.Compile();
		}

		public DTMXPathDocument CreateDocument()
		{
			return new DTMXPathDocument(this.nameTable, this.nodes, this.attributes, this.namespaces, this.idTable);
		}

		public void Compile()
		{
			this.AddNode(0, 0, 0, XPathNodeType.All, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
			this.nodeIndex++;
			this.AddAttribute(0, null, null, null, null, 0, 0);
			this.AddNsNode(0, null, null, 0);
			this.nsIndex++;
			this.AddNsNode(1, "xml", "http://www.w3.org/XML/1998/namespace", 0);
			this.AddNode(0, 0, 0, XPathNodeType.Root, this.xmlReader.BaseURI, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 1, 0, 0);
			this.nodeIndex = 1;
			this.lastNsInScope = 1;
			this.parentStack[0] = this.nodeIndex;
			while (!this.xmlReader.EOF)
			{
				this.Read();
			}
			this.SetNodeArrayLength(this.nodeIndex + 1);
			this.SetAttributeArrayLength(this.attributeIndex + 1);
			this.SetNsArrayLength(this.nsIndex + 1);
			this.xmlReader = null;
		}

		public void Read()
		{
			if (!this.skipRead && !this.xmlReader.Read())
			{
				return;
			}
			this.skipRead = false;
			int num = this.parentStack[this.parentStackIndex];
			int num2 = this.nodeIndex;
			switch (this.xmlReader.NodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.SignificantWhitespace:
				break;
			case XmlNodeType.Attribute:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentType:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				return;
			case XmlNodeType.Whitespace:
				if (this.xmlSpace != XmlSpace.Preserve)
				{
					return;
				}
				break;
			case XmlNodeType.EndElement:
				this.parentStackIndex--;
				return;
			default:
				return;
			}
			if (num == this.nodeIndex)
			{
				num2 = 0;
			}
			else
			{
				while (this.nodes[num2].Parent != num)
				{
					num2 = this.nodes[num2].Parent;
				}
			}
			this.nodeIndex++;
			if (num2 != 0)
			{
				this.nodes[num2].NextSibling = this.nodeIndex;
			}
			if (this.parentStack[this.parentStackIndex] == this.nodeIndex - 1)
			{
				this.nodes[num].FirstChild = this.nodeIndex;
			}
			string text = null;
			XPathNodeType nodeType = XPathNodeType.Text;
			switch (this.xmlReader.NodeType)
			{
			case XmlNodeType.Element:
				this.ProcessElement(num, num2);
				return;
			case XmlNodeType.Attribute:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentType:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				return;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			case XmlNodeType.ProcessingInstruction:
				text = this.xmlReader.Value;
				nodeType = XPathNodeType.ProcessingInstruction;
				break;
			case XmlNodeType.Comment:
				text = this.xmlReader.Value;
				nodeType = XPathNodeType.Comment;
				break;
			case XmlNodeType.Whitespace:
				nodeType = XPathNodeType.Whitespace;
				break;
			case XmlNodeType.SignificantWhitespace:
				nodeType = XPathNodeType.SignificantWhitespace;
				break;
			default:
				return;
			}
			this.AddNode(num, 0, num2, nodeType, this.xmlReader.BaseURI, this.xmlReader.IsEmptyElement, this.xmlReader.LocalName, this.xmlReader.NamespaceURI, this.xmlReader.Prefix, text, this.xmlReader.XmlLang, this.nsIndex, (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber, (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition);
			if (text == null)
			{
				text = string.Empty;
				XPathNodeType xpathNodeType = XPathNodeType.Whitespace;
				for (;;)
				{
					XmlNodeType nodeType2 = this.xmlReader.NodeType;
					if (nodeType2 == XmlNodeType.Text || nodeType2 == XmlNodeType.CDATA)
					{
						xpathNodeType = XPathNodeType.Text;
						goto IL_2B2;
					}
					if (nodeType2 == XmlNodeType.Whitespace)
					{
						goto IL_2B2;
					}
					if (nodeType2 == XmlNodeType.SignificantWhitespace)
					{
						if (xpathNodeType == XPathNodeType.Whitespace)
						{
							xpathNodeType = XPathNodeType.SignificantWhitespace;
						}
						goto IL_2B2;
					}
					bool flag = false;
					IL_303:
					if (!flag)
					{
						break;
					}
					continue;
					IL_2B2:
					if (this.xmlReader.NodeType != XmlNodeType.Whitespace || this.xmlSpace == XmlSpace.Preserve)
					{
						text += this.xmlReader.Value;
					}
					flag = this.xmlReader.Read();
					this.skipRead = true;
					goto IL_303;
				}
				this.nodes[this.nodeIndex].Value = text;
				this.nodes[this.nodeIndex].NodeType = xpathNodeType;
			}
		}

		private void ProcessElement(int parent, int previousSibling)
		{
			this.WriteStartElement(parent, previousSibling);
			if (this.xmlReader.MoveToFirstAttribute())
			{
				do
				{
					string prefix = this.xmlReader.Prefix;
					string namespaceURI = this.xmlReader.NamespaceURI;
					if (namespaceURI == "http://www.w3.org/2000/xmlns/")
					{
						this.ProcessNamespace((prefix != null && !(prefix == string.Empty)) ? this.xmlReader.LocalName : string.Empty, this.xmlReader.Value);
					}
					else
					{
						this.ProcessAttribute(prefix, this.xmlReader.LocalName, namespaceURI, this.xmlReader.Value);
					}
				}
				while (this.xmlReader.MoveToNextAttribute());
				this.xmlReader.MoveToElement();
			}
			this.CloseStartElement();
		}

		private void PrepareStartElement(int previousSibling)
		{
			this.hasAttributes = false;
			this.hasLocalNs = false;
			this.attrIndexAtStart = this.attributeIndex;
			this.nsIndexAtStart = this.nsIndex;
			while (this.namespaces[this.lastNsInScope].DeclaredElement == previousSibling)
			{
				this.lastNsInScope = this.namespaces[this.lastNsInScope].NextNamespace;
			}
		}

		private void WriteStartElement(int parent, int previousSibling)
		{
			this.PrepareStartElement(previousSibling);
			this.AddNode(parent, 0, previousSibling, XPathNodeType.Element, this.xmlReader.BaseURI, this.xmlReader.IsEmptyElement, this.xmlReader.LocalName, this.xmlReader.NamespaceURI, this.xmlReader.Prefix, string.Empty, this.xmlReader.XmlLang, this.lastNsInScope, (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber, (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition);
		}

		private void CloseStartElement()
		{
			if (this.attrIndexAtStart != this.attributeIndex)
			{
				this.nodes[this.nodeIndex].FirstAttribute = this.attrIndexAtStart + 1;
			}
			if (this.nsIndexAtStart != this.nsIndex)
			{
				this.nodes[this.nodeIndex].FirstNamespace = this.nsIndex;
				if (!this.xmlReader.IsEmptyElement)
				{
					this.lastNsInScope = this.nsIndex;
				}
			}
			if (!this.nodes[this.nodeIndex].IsEmptyElement)
			{
				this.parentStackIndex++;
				if (this.parentStack.Length == this.parentStackIndex)
				{
					int[] destinationArray = new int[this.parentStackIndex * 2];
					Array.Copy(this.parentStack, destinationArray, this.parentStackIndex);
					this.parentStack = destinationArray;
				}
				this.parentStack[this.parentStackIndex] = this.nodeIndex;
			}
		}

		private void ProcessNamespace(string prefix, string ns)
		{
			int nextNs = (!this.hasLocalNs) ? this.nodes[this.nodeIndex].FirstNamespace : this.nsIndex;
			this.nsIndex++;
			this.AddNsNode(this.nodeIndex, prefix, ns, nextNs);
			this.hasLocalNs = true;
		}

		private void ProcessAttribute(string prefix, string localName, string ns, string value)
		{
			this.attributeIndex++;
			this.AddAttribute(this.nodeIndex, localName, ns, (prefix == null) ? string.Empty : prefix, value, (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber, (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition);
			if (this.hasAttributes)
			{
				this.attributes[this.attributeIndex - 1].NextAttribute = this.attributeIndex;
			}
			else
			{
				this.hasAttributes = true;
			}
			if (this.validatingReader != null)
			{
				XmlSchemaDatatype xmlSchemaDatatype = this.validatingReader.SchemaType as XmlSchemaDatatype;
				if (xmlSchemaDatatype == null)
				{
					XmlSchemaType xmlSchemaType = this.validatingReader.SchemaType as XmlSchemaType;
					if (xmlSchemaType != null)
					{
						xmlSchemaDatatype = xmlSchemaType.Datatype;
					}
				}
				if (xmlSchemaDatatype != null && xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
				{
					this.idTable.Add(value, this.nodeIndex);
				}
			}
		}

		private void SetNodeArrayLength(int size)
		{
			DTMXPathLinkedNode[] destinationArray = new DTMXPathLinkedNode[size];
			Array.Copy(this.nodes, destinationArray, Math.Min(size, this.nodes.Length));
			this.nodes = destinationArray;
		}

		private void SetAttributeArrayLength(int size)
		{
			DTMXPathAttributeNode[] destinationArray = new DTMXPathAttributeNode[size];
			Array.Copy(this.attributes, destinationArray, Math.Min(size, this.attributes.Length));
			this.attributes = destinationArray;
		}

		private void SetNsArrayLength(int size)
		{
			DTMXPathNamespaceNode[] destinationArray = new DTMXPathNamespaceNode[size];
			Array.Copy(this.namespaces, destinationArray, Math.Min(size, this.namespaces.Length));
			this.namespaces = destinationArray;
		}

		public void AddNode(int parent, int firstAttribute, int previousSibling, XPathNodeType nodeType, string baseUri, bool isEmptyElement, string localName, string ns, string prefix, string value, string xmlLang, int namespaceNode, int lineNumber, int linePosition)
		{
			if (this.nodes.Length < this.nodeIndex + 1)
			{
				this.nodeCapacity *= 4;
				this.SetNodeArrayLength(this.nodeCapacity);
			}
			this.nodes[this.nodeIndex].FirstChild = 0;
			this.nodes[this.nodeIndex].Parent = parent;
			this.nodes[this.nodeIndex].FirstAttribute = firstAttribute;
			this.nodes[this.nodeIndex].PreviousSibling = previousSibling;
			this.nodes[this.nodeIndex].NextSibling = 0;
			this.nodes[this.nodeIndex].NodeType = nodeType;
			this.nodes[this.nodeIndex].BaseURI = baseUri;
			this.nodes[this.nodeIndex].IsEmptyElement = isEmptyElement;
			this.nodes[this.nodeIndex].LocalName = localName;
			this.nodes[this.nodeIndex].NamespaceURI = ns;
			this.nodes[this.nodeIndex].Prefix = prefix;
			this.nodes[this.nodeIndex].Value = value;
			this.nodes[this.nodeIndex].XmlLang = xmlLang;
			this.nodes[this.nodeIndex].FirstNamespace = namespaceNode;
			this.nodes[this.nodeIndex].LineNumber = lineNumber;
			this.nodes[this.nodeIndex].LinePosition = linePosition;
		}

		public void AddAttribute(int ownerElement, string localName, string ns, string prefix, string value, int lineNumber, int linePosition)
		{
			if (this.attributes.Length < this.attributeIndex + 1)
			{
				this.attributeCapacity *= 4;
				this.SetAttributeArrayLength(this.attributeCapacity);
			}
			this.attributes[this.attributeIndex].OwnerElement = ownerElement;
			this.attributes[this.attributeIndex].LocalName = localName;
			this.attributes[this.attributeIndex].NamespaceURI = ns;
			this.attributes[this.attributeIndex].Prefix = prefix;
			this.attributes[this.attributeIndex].Value = value;
			this.attributes[this.attributeIndex].LineNumber = lineNumber;
			this.attributes[this.attributeIndex].LinePosition = linePosition;
		}

		public void AddNsNode(int declaredElement, string name, string ns, int nextNs)
		{
			if (this.namespaces.Length < this.nsIndex + 1)
			{
				this.nsCapacity *= 4;
				this.SetNsArrayLength(this.nsCapacity);
			}
			this.namespaces[this.nsIndex].DeclaredElement = declaredElement;
			this.namespaces[this.nsIndex].Name = name;
			this.namespaces[this.nsIndex].Namespace = ns;
			this.namespaces[this.nsIndex].NextNamespace = nextNs;
		}
	}
}
