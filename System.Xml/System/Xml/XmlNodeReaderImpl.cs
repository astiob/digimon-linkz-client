using Mono.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	internal class XmlNodeReaderImpl : XmlReader, IHasXmlParserContext, IXmlNamespaceResolver
	{
		private XmlDocument document;

		private XmlNode startNode;

		private XmlNode current;

		private XmlNode ownerLinkedNode;

		private ReadState state;

		private int depth;

		private bool isEndElement;

		private bool ignoreStartNode;

		internal XmlNodeReaderImpl(XmlNodeReaderImpl entityContainer) : this(entityContainer.current)
		{
		}

		public XmlNodeReaderImpl(XmlNode node)
		{
			this.startNode = node;
			this.depth = 0;
			this.document = ((this.startNode.NodeType != XmlNodeType.Document) ? this.startNode.OwnerDocument : (this.startNode as XmlDocument));
			XmlNodeType nodeType = node.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
				break;
			default:
				if (nodeType != XmlNodeType.EntityReference)
				{
					return;
				}
				break;
			}
			this.ignoreStartNode = true;
		}

		XmlParserContext IHasXmlParserContext.ParserContext
		{
			get
			{
				return new XmlParserContext(this.document.NameTable, (this.current != null) ? this.current.ConstructNamespaceManager() : new XmlNamespaceManager(this.document.NameTable), (this.document.DocumentType == null) ? null : this.document.DocumentType.DTD, (this.current != null) ? this.current.BaseURI : this.document.BaseURI, this.XmlLang, this.XmlSpace, Encoding.Unicode);
			}
		}

		public override int AttributeCount
		{
			get
			{
				if (this.state != ReadState.Interactive)
				{
					return 0;
				}
				if (this.isEndElement || this.current == null)
				{
					return 0;
				}
				XmlNode xmlNode = this.ownerLinkedNode;
				return (xmlNode.Attributes == null) ? 0 : xmlNode.Attributes.Count;
			}
		}

		public override string BaseURI
		{
			get
			{
				if (this.current == null)
				{
					return this.startNode.BaseURI;
				}
				return this.current.BaseURI;
			}
		}

		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		public override bool CanReadValueChunk
		{
			get
			{
				return true;
			}
		}

		public override bool CanResolveEntity
		{
			get
			{
				return false;
			}
		}

		public override int Depth
		{
			get
			{
				return (this.current != null) ? ((this.current != this.ownerLinkedNode) ? ((this.current.NodeType != XmlNodeType.Attribute) ? (this.depth + 2) : (this.depth + 1)) : this.depth) : 0;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.state == ReadState.EndOfFile || this.state == ReadState.Error;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				if (this.isEndElement || this.current == null)
				{
					return false;
				}
				XmlNode xmlNode = this.ownerLinkedNode;
				return xmlNode.Attributes != null && xmlNode.Attributes.Count != 0;
			}
		}

		public override bool HasValue
		{
			get
			{
				if (this.current == null)
				{
					return false;
				}
				XmlNodeType nodeType = this.current.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.EntityReference:
				case XmlNodeType.Document:
				case XmlNodeType.DocumentFragment:
				case XmlNodeType.Notation:
				case XmlNodeType.EndElement:
				case XmlNodeType.EndEntity:
					break;
				default:
					if (nodeType != XmlNodeType.Element)
					{
						return true;
					}
					break;
				}
				return false;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.current != null && this.current.NodeType == XmlNodeType.Attribute && !((XmlAttribute)this.current).Specified;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.current != null && this.current.NodeType == XmlNodeType.Element && ((XmlElement)this.current).IsEmpty;
			}
		}

		public override string LocalName
		{
			get
			{
				if (this.current == null)
				{
					return string.Empty;
				}
				XmlNodeType nodeType = this.current.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.Attribute:
				case XmlNodeType.EntityReference:
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.DocumentType:
					break;
				default:
					if (nodeType != XmlNodeType.XmlDeclaration)
					{
						return string.Empty;
					}
					break;
				}
				return this.current.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				if (this.current == null)
				{
					return string.Empty;
				}
				XmlNodeType nodeType = this.current.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.Attribute:
				case XmlNodeType.EntityReference:
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.DocumentType:
					break;
				default:
					if (nodeType != XmlNodeType.XmlDeclaration)
					{
						return string.Empty;
					}
					break;
				}
				return this.current.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (this.current == null)
				{
					return string.Empty;
				}
				return this.current.NamespaceURI;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.document.NameTable;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				if (this.current == null)
				{
					return XmlNodeType.None;
				}
				return (!this.isEndElement) ? this.current.NodeType : XmlNodeType.EndElement;
			}
		}

		public override string Prefix
		{
			get
			{
				if (this.current == null)
				{
					return string.Empty;
				}
				return this.current.Prefix;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.state;
			}
		}

		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				IXmlSchemaInfo result;
				if (this.current != null)
				{
					IXmlSchemaInfo schemaInfo = this.current.SchemaInfo;
					result = schemaInfo;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public override string Value
		{
			get
			{
				if (this.NodeType == XmlNodeType.DocumentType)
				{
					return ((XmlDocumentType)this.current).InternalSubset;
				}
				return (!this.HasValue) ? string.Empty : this.current.Value;
			}
		}

		public override string XmlLang
		{
			get
			{
				if (this.current == null)
				{
					return this.startNode.XmlLang;
				}
				return this.current.XmlLang;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				if (this.current == null)
				{
					return this.startNode.XmlSpace;
				}
				return this.current.XmlSpace;
			}
		}

		public override void Close()
		{
			this.current = null;
			this.state = ReadState.Closed;
		}

		public override string GetAttribute(int attributeIndex)
		{
			if (this.NodeType == XmlNodeType.XmlDeclaration)
			{
				XmlDeclaration xmlDeclaration = this.current as XmlDeclaration;
				if (attributeIndex == 0)
				{
					return xmlDeclaration.Version;
				}
				if (attributeIndex == 1)
				{
					if (xmlDeclaration.Encoding != string.Empty)
					{
						return xmlDeclaration.Encoding;
					}
					if (xmlDeclaration.Standalone != string.Empty)
					{
						return xmlDeclaration.Standalone;
					}
				}
				else if (attributeIndex == 2 && xmlDeclaration.Encoding != string.Empty && xmlDeclaration.Standalone != null)
				{
					return xmlDeclaration.Standalone;
				}
				throw new ArgumentOutOfRangeException("Index out of range.");
			}
			else
			{
				if (this.NodeType == XmlNodeType.DocumentType)
				{
					XmlDocumentType xmlDocumentType = this.current as XmlDocumentType;
					if (attributeIndex == 0)
					{
						if (xmlDocumentType.PublicId != string.Empty)
						{
							return xmlDocumentType.PublicId;
						}
						if (xmlDocumentType.SystemId != string.Empty)
						{
							return xmlDocumentType.SystemId;
						}
					}
					else if (attributeIndex == 1 && xmlDocumentType.PublicId == string.Empty && xmlDocumentType.SystemId != string.Empty)
					{
						return xmlDocumentType.SystemId;
					}
					throw new ArgumentOutOfRangeException("Index out of range.");
				}
				if (this.isEndElement || this.current == null)
				{
					return null;
				}
				if (attributeIndex < 0 || attributeIndex > this.AttributeCount)
				{
					throw new ArgumentOutOfRangeException("Index out of range.");
				}
				return this.ownerLinkedNode.Attributes[attributeIndex].Value;
			}
		}

		public override string GetAttribute(string name)
		{
			if (this.isEndElement || this.current == null)
			{
				return null;
			}
			if (this.NodeType == XmlNodeType.XmlDeclaration)
			{
				return this.GetXmlDeclarationAttribute(name);
			}
			if (this.NodeType == XmlNodeType.DocumentType)
			{
				return this.GetDocumentTypeAttribute(name);
			}
			if (this.ownerLinkedNode.Attributes == null)
			{
				return null;
			}
			XmlAttribute xmlAttribute = this.ownerLinkedNode.Attributes[name];
			if (xmlAttribute == null)
			{
				return null;
			}
			return xmlAttribute.Value;
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			if (this.isEndElement || this.current == null)
			{
				return null;
			}
			if (this.NodeType == XmlNodeType.XmlDeclaration)
			{
				return this.GetXmlDeclarationAttribute(name);
			}
			if (this.NodeType == XmlNodeType.DocumentType)
			{
				return this.GetDocumentTypeAttribute(name);
			}
			if (this.ownerLinkedNode.Attributes == null)
			{
				return null;
			}
			XmlAttribute xmlAttribute = this.ownerLinkedNode.Attributes[name, namespaceURI];
			if (xmlAttribute == null)
			{
				return null;
			}
			return xmlAttribute.Value;
		}

		private string GetXmlDeclarationAttribute(string name)
		{
			XmlDeclaration xmlDeclaration = this.current as XmlDeclaration;
			switch (name)
			{
			case "version":
				return xmlDeclaration.Version;
			case "encoding":
				return (!(xmlDeclaration.Encoding != string.Empty)) ? null : xmlDeclaration.Encoding;
			case "standalone":
				return xmlDeclaration.Standalone;
			}
			return null;
		}

		private string GetDocumentTypeAttribute(string name)
		{
			XmlDocumentType xmlDocumentType = this.current as XmlDocumentType;
			if (name != null)
			{
				if (XmlNodeReaderImpl.<>f__switch$map4E == null)
				{
					XmlNodeReaderImpl.<>f__switch$map4E = new Dictionary<string, int>(2)
					{
						{
							"PUBLIC",
							0
						},
						{
							"SYSTEM",
							1
						}
					};
				}
				int num;
				if (XmlNodeReaderImpl.<>f__switch$map4E.TryGetValue(name, out num))
				{
					if (num == 0)
					{
						return xmlDocumentType.PublicId;
					}
					if (num == 1)
					{
						return xmlDocumentType.SystemId;
					}
				}
			}
			return null;
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			XmlNode parentNode = this.current;
			while (parentNode.NodeType != XmlNodeType.Document)
			{
				for (int i = 0; i < this.current.Attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = this.current.Attributes[i];
					if (xmlAttribute.NamespaceURI == "http://www.w3.org/2000/xmlns/")
					{
						dictionary.Add((!(xmlAttribute.Prefix == "xmlns")) ? string.Empty : xmlAttribute.LocalName, xmlAttribute.Value);
					}
				}
				if (scope == XmlNamespaceScope.Local)
				{
					return dictionary;
				}
				parentNode = parentNode.ParentNode;
				if (parentNode == null)
				{
					IL_AE:
					if (scope == XmlNamespaceScope.All)
					{
						dictionary.Add("xml", "http://www.w3.org/XML/1998/namespace");
					}
					return dictionary;
				}
			}
			goto IL_AE;
		}

		private XmlElement GetCurrentElement()
		{
			XmlElement result = null;
			switch (this.current.NodeType)
			{
			case XmlNodeType.Element:
				result = (XmlElement)this.current;
				break;
			case XmlNodeType.Attribute:
				result = ((XmlAttribute)this.current).OwnerElement;
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.EntityReference:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				result = (this.current.ParentNode as XmlElement);
				break;
			}
			return result;
		}

		public override string LookupNamespace(string prefix)
		{
			if (this.current == null)
			{
				return null;
			}
			for (XmlElement xmlElement = this.GetCurrentElement(); xmlElement != null; xmlElement = (xmlElement.ParentNode as XmlElement))
			{
				for (int i = 0; i < xmlElement.Attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = xmlElement.Attributes[i];
					if (!(xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/"))
					{
						if (prefix == string.Empty)
						{
							if (xmlAttribute.Prefix == string.Empty)
							{
								return xmlAttribute.Value;
							}
						}
						else if (xmlAttribute.LocalName == prefix)
						{
							return xmlAttribute.Value;
						}
					}
				}
			}
			if (prefix != null)
			{
				if (XmlNodeReaderImpl.<>f__switch$map4F == null)
				{
					XmlNodeReaderImpl.<>f__switch$map4F = new Dictionary<string, int>(2)
					{
						{
							"xml",
							0
						},
						{
							"xmlns",
							1
						}
					};
				}
				int num;
				if (XmlNodeReaderImpl.<>f__switch$map4F.TryGetValue(prefix, out num))
				{
					if (num == 0)
					{
						return "http://www.w3.org/XML/1998/namespace";
					}
					if (num == 1)
					{
						return "http://www.w3.org/2000/xmlns/";
					}
				}
			}
			return null;
		}

		public string LookupPrefix(string ns)
		{
			return this.LookupPrefix(ns, false);
		}

		public string LookupPrefix(string ns, bool atomizedNames)
		{
			if (this.current == null)
			{
				return null;
			}
			for (XmlElement xmlElement = this.GetCurrentElement(); xmlElement != null; xmlElement = (xmlElement.ParentNode as XmlElement))
			{
				for (int i = 0; i < xmlElement.Attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = xmlElement.Attributes[i];
					if (atomizedNames)
					{
						if (object.ReferenceEquals(xmlAttribute.NamespaceURI, "http://www.w3.org/2000/xmlns/"))
						{
							if (object.ReferenceEquals(xmlAttribute.Value, ns))
							{
								return (!(xmlAttribute.Prefix != string.Empty)) ? string.Empty : xmlAttribute.LocalName;
							}
						}
					}
					else if (!(xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/"))
					{
						if (xmlAttribute.Value == ns)
						{
							return (!(xmlAttribute.Prefix != string.Empty)) ? string.Empty : xmlAttribute.LocalName;
						}
					}
				}
			}
			if (ns != null)
			{
				if (XmlNodeReaderImpl.<>f__switch$map50 == null)
				{
					XmlNodeReaderImpl.<>f__switch$map50 = new Dictionary<string, int>(2)
					{
						{
							"http://www.w3.org/XML/1998/namespace",
							0
						},
						{
							"http://www.w3.org/2000/xmlns/",
							1
						}
					};
				}
				int num;
				if (XmlNodeReaderImpl.<>f__switch$map50.TryGetValue(ns, out num))
				{
					if (num == 0)
					{
						return "xml";
					}
					if (num == 1)
					{
						return "xmlns";
					}
				}
			}
			return null;
		}

		public override void MoveToAttribute(int attributeIndex)
		{
			if (this.isEndElement || attributeIndex < 0 || attributeIndex > this.AttributeCount)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.state = ReadState.Interactive;
			this.current = this.ownerLinkedNode.Attributes[attributeIndex];
		}

		public override bool MoveToAttribute(string name)
		{
			if (this.isEndElement || this.current == null)
			{
				return false;
			}
			XmlNode xmlNode = this.current;
			if (this.current.ParentNode.NodeType == XmlNodeType.Attribute)
			{
				this.current = this.current.ParentNode;
			}
			if (this.ownerLinkedNode.Attributes == null)
			{
				return false;
			}
			XmlAttribute xmlAttribute = this.ownerLinkedNode.Attributes[name];
			if (xmlAttribute == null)
			{
				this.current = xmlNode;
				return false;
			}
			this.current = xmlAttribute;
			return true;
		}

		public override bool MoveToAttribute(string name, string namespaceURI)
		{
			if (this.isEndElement || this.current == null)
			{
				return false;
			}
			if (this.ownerLinkedNode.Attributes == null)
			{
				return false;
			}
			XmlAttribute xmlAttribute = this.ownerLinkedNode.Attributes[name, namespaceURI];
			if (xmlAttribute == null)
			{
				return false;
			}
			this.current = xmlAttribute;
			return true;
		}

		public override bool MoveToElement()
		{
			if (this.current == null)
			{
				return false;
			}
			XmlNode xmlNode = this.ownerLinkedNode;
			if (this.current != xmlNode)
			{
				this.current = xmlNode;
				return true;
			}
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			if (this.current == null)
			{
				return false;
			}
			if (this.ownerLinkedNode.Attributes == null)
			{
				return false;
			}
			if (this.ownerLinkedNode.Attributes.Count > 0)
			{
				this.current = this.ownerLinkedNode.Attributes[0];
				return true;
			}
			return false;
		}

		public override bool MoveToNextAttribute()
		{
			if (this.current == null)
			{
				return false;
			}
			XmlNode parentNode = this.current;
			if (this.current.NodeType != XmlNodeType.Attribute)
			{
				if (this.current.ParentNode == null || this.current.ParentNode.NodeType != XmlNodeType.Attribute)
				{
					return this.MoveToFirstAttribute();
				}
				parentNode = this.current.ParentNode;
			}
			XmlAttributeCollection attributes = ((XmlAttribute)parentNode).OwnerElement.Attributes;
			int i = 0;
			while (i < attributes.Count - 1)
			{
				XmlAttribute xmlAttribute = attributes[i];
				if (xmlAttribute == parentNode)
				{
					i++;
					if (i == attributes.Count)
					{
						return false;
					}
					this.current = attributes[i];
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		public override bool Read()
		{
			switch (this.state)
			{
			case ReadState.Error:
			case ReadState.EndOfFile:
			case ReadState.Closed:
				return false;
			default:
			{
				if (base.Binary != null)
				{
					base.Binary.Reset();
				}
				bool result = this.ReadContent();
				this.ownerLinkedNode = this.current;
				return result;
			}
			}
		}

		private bool ReadContent()
		{
			if (this.ReadState == ReadState.Initial)
			{
				this.current = this.startNode;
				this.state = ReadState.Interactive;
				if (this.ignoreStartNode)
				{
					this.current = this.startNode.FirstChild;
				}
				if (this.current == null)
				{
					this.state = ReadState.Error;
					return false;
				}
				return true;
			}
			else
			{
				this.MoveToElement();
				XmlNode xmlNode = (this.isEndElement || this.current.NodeType == XmlNodeType.EntityReference) ? null : this.current.FirstChild;
				if (xmlNode != null)
				{
					this.isEndElement = false;
					this.current = xmlNode;
					this.depth++;
					return true;
				}
				if (this.current == this.startNode)
				{
					if (this.IsEmptyElement || this.isEndElement)
					{
						this.isEndElement = false;
						this.current = null;
						this.state = ReadState.EndOfFile;
						return false;
					}
					this.isEndElement = true;
					return true;
				}
				else
				{
					if (!this.isEndElement && !this.IsEmptyElement && this.current.NodeType == XmlNodeType.Element)
					{
						this.isEndElement = true;
						return true;
					}
					XmlNode nextSibling = this.current.NextSibling;
					if (nextSibling != null)
					{
						this.isEndElement = false;
						this.current = nextSibling;
						return true;
					}
					XmlNode parentNode = this.current.ParentNode;
					if (parentNode == null || (parentNode == this.startNode && this.ignoreStartNode))
					{
						this.isEndElement = false;
						this.current = null;
						this.state = ReadState.EndOfFile;
						return false;
					}
					this.current = parentNode;
					this.depth--;
					this.isEndElement = true;
					return true;
				}
			}
		}

		public override bool ReadAttributeValue()
		{
			if (this.current.NodeType == XmlNodeType.Attribute)
			{
				if (this.current.FirstChild == null)
				{
					return false;
				}
				this.current = this.current.FirstChild;
				return true;
			}
			else
			{
				if (this.current.ParentNode.NodeType != XmlNodeType.Attribute)
				{
					return false;
				}
				if (this.current.NextSibling == null)
				{
					return false;
				}
				this.current = this.current.NextSibling;
				return true;
			}
		}

		public override string ReadString()
		{
			return base.ReadString();
		}

		public override void ResolveEntity()
		{
			throw new NotSupportedException("Should not happen.");
		}

		public override void Skip()
		{
			base.Skip();
		}
	}
}
