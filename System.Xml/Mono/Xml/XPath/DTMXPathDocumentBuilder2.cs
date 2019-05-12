using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class DTMXPathDocumentBuilder2
	{
		private XmlReader xmlReader;

		private XmlValidatingReader validatingReader;

		private XmlSpace xmlSpace;

		private XmlNameTable nameTable;

		private IXmlLineInfo lineInfo;

		private int nodeCapacity;

		private int attributeCapacity;

		private int nsCapacity;

		private DTMXPathLinkedNode2[] nodes;

		private DTMXPathAttributeNode2[] attributes;

		private DTMXPathNamespaceNode2[] namespaces;

		private string[] atomicStringPool;

		private int atomicIndex;

		private string[] nonAtomicStringPool;

		private int nonAtomicIndex;

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

		public DTMXPathDocumentBuilder2(string url) : this(url, XmlSpace.None, 200)
		{
		}

		public DTMXPathDocumentBuilder2(string url, XmlSpace space) : this(url, space, 200)
		{
		}

		public DTMXPathDocumentBuilder2(string url, XmlSpace space, int defaultCapacity)
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

		public DTMXPathDocumentBuilder2(XmlReader reader) : this(reader, XmlSpace.None, 200)
		{
		}

		public DTMXPathDocumentBuilder2(XmlReader reader, XmlSpace space) : this(reader, space, 200)
		{
		}

		public DTMXPathDocumentBuilder2(XmlReader reader, XmlSpace space, int defaultCapacity)
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
			this.nodes = new DTMXPathLinkedNode2[this.nodeCapacity];
			this.attributes = new DTMXPathAttributeNode2[this.attributeCapacity];
			this.namespaces = new DTMXPathNamespaceNode2[this.nsCapacity];
			this.atomicStringPool = new string[20];
			this.nonAtomicStringPool = new string[20];
			this.Compile();
		}

		public DTMXPathDocument2 CreateDocument()
		{
			return new DTMXPathDocument2(this.nameTable, this.nodes, this.attributes, this.namespaces, this.atomicStringPool, this.nonAtomicStringPool, this.idTable);
		}

		public void Compile()
		{
			this.atomicStringPool[0] = (this.nonAtomicStringPool[0] = string.Empty);
			this.atomicStringPool[1] = (this.nonAtomicStringPool[1] = null);
			this.atomicStringPool[2] = (this.nonAtomicStringPool[2] = "http://www.w3.org/XML/1998/namespace");
			this.atomicStringPool[3] = (this.nonAtomicStringPool[3] = "http://www.w3.org/2000/xmlns/");
			this.atomicIndex = (this.nonAtomicIndex = 4);
			this.AddNode(0, 0, 0, XPathNodeType.All, 0, false, 0, 0, 0, 0, 0, 0, 0, 0);
			this.nodeIndex++;
			this.AddAttribute(0, 0, 0, 0, 0, 0, 0);
			this.AddNsNode(0, 0, 0, 0);
			this.nsIndex++;
			this.AddNsNode(1, this.AtomicIndex("xml"), this.AtomicIndex("http://www.w3.org/XML/1998/namespace"), 0);
			this.AddNode(0, 0, 0, XPathNodeType.Root, this.AtomicIndex(this.xmlReader.BaseURI), false, 0, 0, 0, 0, 0, 1, 0, 0);
			this.nodeIndex = 1;
			this.lastNsInScope = 1;
			this.parentStack[0] = this.nodeIndex;
			if (this.xmlReader.ReadState == ReadState.Initial)
			{
				this.xmlReader.Read();
			}
			int depth = this.xmlReader.Depth;
			do
			{
				this.Read();
			}
			while (this.skipRead || (this.xmlReader.Read() && this.xmlReader.Depth >= depth));
			this.SetNodeArrayLength(this.nodeIndex + 1);
			this.SetAttributeArrayLength(this.attributeIndex + 1);
			this.SetNsArrayLength(this.nsIndex + 1);
			string[] destinationArray = new string[this.atomicIndex];
			Array.Copy(this.atomicStringPool, destinationArray, this.atomicIndex);
			this.atomicStringPool = destinationArray;
			destinationArray = new string[this.nonAtomicIndex];
			Array.Copy(this.nonAtomicStringPool, destinationArray, this.nonAtomicIndex);
			this.nonAtomicStringPool = destinationArray;
			this.xmlReader = null;
		}

		public void Read()
		{
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
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			{
				string text = null;
				XPathNodeType xpathNodeType = XPathNodeType.Root;
				bool flag = false;
				switch (this.xmlReader.NodeType)
				{
				case XmlNodeType.None:
					break;
				case XmlNodeType.Element:
					xpathNodeType = XPathNodeType.Element;
					break;
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
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.skipRead = true;
					for (;;)
					{
						XmlNodeType nodeType = this.xmlReader.NodeType;
						if (nodeType == XmlNodeType.Text)
						{
							goto IL_1B2;
						}
						if (nodeType == XmlNodeType.CDATA)
						{
							if (xpathNodeType != XPathNodeType.Text)
							{
								text = string.Empty;
							}
							flag = true;
							goto IL_1B2;
						}
						if (nodeType != XmlNodeType.Whitespace)
						{
							if (nodeType != XmlNodeType.SignificantWhitespace)
							{
								break;
							}
							if (xpathNodeType == XPathNodeType.Root || xpathNodeType == XPathNodeType.Whitespace)
							{
								xpathNodeType = XPathNodeType.SignificantWhitespace;
							}
							text += this.xmlReader.Value;
						}
						else
						{
							XPathNodeType xpathNodeType2 = xpathNodeType;
							if (xpathNodeType2 == XPathNodeType.Root)
							{
								xpathNodeType = XPathNodeType.Whitespace;
							}
							if (!flag)
							{
								text += this.xmlReader.Value;
							}
						}
						IL_1D1:
						if (!this.xmlReader.Read())
						{
							break;
						}
						continue;
						IL_1B2:
						xpathNodeType = XPathNodeType.Text;
						text += this.xmlReader.Value;
						goto IL_1D1;
					}
					break;
				case XmlNodeType.ProcessingInstruction:
					text = this.xmlReader.Value;
					xpathNodeType = XPathNodeType.ProcessingInstruction;
					break;
				case XmlNodeType.Comment:
					text = this.xmlReader.Value;
					xpathNodeType = XPathNodeType.Comment;
					break;
				default:
					return;
				}
				if (xpathNodeType == XPathNodeType.Root || (xpathNodeType == XPathNodeType.Whitespace && this.xmlSpace != XmlSpace.Preserve))
				{
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
				if (xpathNodeType == XPathNodeType.Element)
				{
					this.ProcessElement(num, num2);
				}
				else
				{
					this.AddNode(num, 0, num2, xpathNodeType, this.AtomicIndex(this.xmlReader.BaseURI), this.xmlReader.IsEmptyElement, (!this.skipRead) ? this.AtomicIndex(this.xmlReader.LocalName) : 0, (!this.skipRead) ? this.AtomicIndex(this.xmlReader.NamespaceURI) : 0, this.AtomicIndex(this.xmlReader.Prefix), (text != null) ? this.NonAtomicIndex(text) : 0, this.AtomicIndex(this.xmlReader.XmlLang), this.nsIndex, (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber, (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition);
				}
				return;
			}
			case XmlNodeType.EndElement:
			{
				int target = this.parentStack[this.parentStackIndex];
				this.AdjustLastNsInScope(target);
				this.parentStackIndex--;
				return;
			}
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
			this.AdjustLastNsInScope(previousSibling);
		}

		private void AdjustLastNsInScope(int target)
		{
			while (this.namespaces[this.lastNsInScope].DeclaredElement == target)
			{
				this.lastNsInScope = this.namespaces[this.lastNsInScope].NextNamespace;
			}
		}

		private void WriteStartElement(int parent, int previousSibling)
		{
			this.PrepareStartElement(previousSibling);
			this.AddNode(parent, 0, previousSibling, XPathNodeType.Element, this.AtomicIndex(this.xmlReader.BaseURI), this.xmlReader.IsEmptyElement, this.AtomicIndex(this.xmlReader.LocalName), this.AtomicIndex(this.xmlReader.NamespaceURI), this.AtomicIndex(this.xmlReader.Prefix), 0, this.AtomicIndex(this.xmlReader.XmlLang), this.lastNsInScope, (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber, (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition);
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
			this.AddNsNode(this.nodeIndex, this.AtomicIndex(prefix), this.AtomicIndex(ns), nextNs);
			this.hasLocalNs = true;
		}

		private void ProcessAttribute(string prefix, string localName, string ns, string value)
		{
			this.attributeIndex++;
			this.AddAttribute(this.nodeIndex, this.AtomicIndex(localName), this.AtomicIndex(ns), (prefix == null) ? 0 : this.AtomicIndex(prefix), this.NonAtomicIndex(value), (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber, (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition);
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

		private int AtomicIndex(string s)
		{
			if (s == string.Empty)
			{
				return 0;
			}
			if (s == null)
			{
				return 1;
			}
			for (int i = 2; i < this.atomicIndex; i++)
			{
				if (object.ReferenceEquals(s, this.atomicStringPool[i]))
				{
					return i;
				}
			}
			if (this.atomicIndex == this.atomicStringPool.Length)
			{
				string[] destinationArray = new string[this.atomicIndex * 2];
				Array.Copy(this.atomicStringPool, destinationArray, this.atomicIndex);
				this.atomicStringPool = destinationArray;
			}
			this.atomicStringPool[this.atomicIndex] = s;
			return this.atomicIndex++;
		}

		private int NonAtomicIndex(string s)
		{
			if (s == string.Empty)
			{
				return 0;
			}
			if (s == null)
			{
				return 1;
			}
			int i = 2;
			int num = (this.nonAtomicIndex >= 100) ? 100 : this.nonAtomicIndex;
			while (i < num)
			{
				if (s == this.nonAtomicStringPool[i])
				{
					return i;
				}
				i++;
			}
			if (this.nonAtomicIndex == this.nonAtomicStringPool.Length)
			{
				string[] destinationArray = new string[this.nonAtomicIndex * 2];
				Array.Copy(this.nonAtomicStringPool, destinationArray, this.nonAtomicIndex);
				this.nonAtomicStringPool = destinationArray;
			}
			this.nonAtomicStringPool[this.nonAtomicIndex] = s;
			return this.nonAtomicIndex++;
		}

		private void SetNodeArrayLength(int size)
		{
			DTMXPathLinkedNode2[] destinationArray = new DTMXPathLinkedNode2[size];
			Array.Copy(this.nodes, destinationArray, Math.Min(size, this.nodes.Length));
			this.nodes = destinationArray;
		}

		private void SetAttributeArrayLength(int size)
		{
			DTMXPathAttributeNode2[] destinationArray = new DTMXPathAttributeNode2[size];
			Array.Copy(this.attributes, destinationArray, Math.Min(size, this.attributes.Length));
			this.attributes = destinationArray;
		}

		private void SetNsArrayLength(int size)
		{
			DTMXPathNamespaceNode2[] destinationArray = new DTMXPathNamespaceNode2[size];
			Array.Copy(this.namespaces, destinationArray, Math.Min(size, this.namespaces.Length));
			this.namespaces = destinationArray;
		}

		public void AddNode(int parent, int firstAttribute, int previousSibling, XPathNodeType nodeType, int baseUri, bool isEmptyElement, int localName, int ns, int prefix, int value, int xmlLang, int namespaceNode, int lineNumber, int linePosition)
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

		public void AddAttribute(int ownerElement, int localName, int ns, int prefix, int value, int lineNumber, int linePosition)
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

		public void AddNsNode(int declaredElement, int name, int ns, int nextNs)
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
