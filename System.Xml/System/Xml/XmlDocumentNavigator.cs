using System;
using System.Collections;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	internal class XmlDocumentNavigator : XPathNavigator, IHasXmlNode
	{
		private const string Xmlns = "http://www.w3.org/2000/xmlns/";

		private const string XmlnsXML = "http://www.w3.org/XML/1998/namespace";

		private XmlNode node;

		private XmlAttribute nsNode;

		private ArrayList iteratedNsNames;

		internal XmlDocumentNavigator(XmlNode node)
		{
			this.node = node;
			if (node.NodeType == XmlNodeType.Attribute && node.NamespaceURI == "http://www.w3.org/2000/xmlns/")
			{
				this.nsNode = (XmlAttribute)node;
				node = this.nsNode.OwnerElement;
			}
		}

		XmlNode IHasXmlNode.GetNode()
		{
			return this.Node;
		}

		internal XmlDocument Document
		{
			get
			{
				return (this.node.NodeType != XmlNodeType.Document) ? this.node.OwnerDocument : (this.node as XmlDocument);
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.node.BaseURI;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				if (this.NsNode != null)
				{
					return false;
				}
				XmlElement xmlElement = this.node as XmlElement;
				if (xmlElement == null || !xmlElement.HasAttributes)
				{
					return false;
				}
				for (int i = 0; i < this.node.Attributes.Count; i++)
				{
					if (this.node.Attributes[i].NamespaceURI != "http://www.w3.org/2000/xmlns/")
					{
						return true;
					}
				}
				return false;
			}
		}

		public override bool HasChildren
		{
			get
			{
				if (this.NsNode != null)
				{
					return false;
				}
				XPathNodeType nodeType = this.NodeType;
				bool flag = nodeType == XPathNodeType.Root || nodeType == XPathNodeType.Element;
				return flag && this.GetFirstChild(this.node) != null;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.NsNode == null && this.node.NodeType == XmlNodeType.Element && ((XmlElement)this.node).IsEmpty;
			}
		}

		public XmlAttribute NsNode
		{
			get
			{
				return this.nsNode;
			}
			set
			{
				if (value == null)
				{
					this.iteratedNsNames = null;
				}
				else
				{
					if (this.iteratedNsNames == null)
					{
						this.iteratedNsNames = new ArrayList();
					}
					else if (this.iteratedNsNames.IsReadOnly)
					{
						this.iteratedNsNames = new ArrayList(this.iteratedNsNames);
					}
					this.iteratedNsNames.Add(value.Name);
				}
				this.nsNode = value;
			}
		}

		public override string LocalName
		{
			get
			{
				XmlAttribute xmlAttribute = this.NsNode;
				if (xmlAttribute == null)
				{
					XPathNodeType nodeType = this.NodeType;
					bool flag = nodeType == XPathNodeType.Element || nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.ProcessingInstruction || nodeType == XPathNodeType.Namespace;
					return (!flag) ? string.Empty : this.node.LocalName;
				}
				if (xmlAttribute == this.Document.NsNodeXml)
				{
					return "xml";
				}
				return (!(xmlAttribute.Name == "xmlns")) ? xmlAttribute.LocalName : string.Empty;
			}
		}

		public override string Name
		{
			get
			{
				if (this.NsNode != null)
				{
					return this.LocalName;
				}
				XPathNodeType nodeType = this.NodeType;
				bool flag = nodeType == XPathNodeType.Element || nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.ProcessingInstruction || nodeType == XPathNodeType.Namespace;
				return (!flag) ? string.Empty : this.node.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return (this.NsNode == null) ? this.node.NamespaceURI : string.Empty;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.Document.NameTable;
			}
		}

		public override XPathNodeType NodeType
		{
			get
			{
				if (this.NsNode != null)
				{
					return XPathNodeType.Namespace;
				}
				XmlNode xmlNode = this.node;
				bool flag = false;
				for (;;)
				{
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.CDATA)
					{
						break;
					}
					if (nodeType != XmlNodeType.Whitespace)
					{
						if (nodeType != XmlNodeType.SignificantWhitespace)
						{
							xmlNode = null;
						}
						else
						{
							flag = true;
							xmlNode = this.GetNextSibling(xmlNode);
						}
					}
					else
					{
						xmlNode = this.GetNextSibling(xmlNode);
					}
					if (xmlNode == null)
					{
						goto Block_6;
					}
				}
				return XPathNodeType.Text;
				Block_6:
				return (!flag) ? this.node.XPathNodeType : XPathNodeType.SignificantWhitespace;
			}
		}

		public override string Prefix
		{
			get
			{
				return (this.NsNode == null) ? this.node.Prefix : string.Empty;
			}
		}

		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				IXmlSchemaInfo result;
				if (this.NsNode != null)
				{
					IXmlSchemaInfo xmlSchemaInfo = null;
					result = xmlSchemaInfo;
				}
				else
				{
					result = this.node.SchemaInfo;
				}
				return result;
			}
		}

		public override object UnderlyingObject
		{
			get
			{
				return this.node;
			}
		}

		public override string Value
		{
			get
			{
				switch (this.NodeType)
				{
				case XPathNodeType.Root:
				case XPathNodeType.Element:
					return this.node.InnerText;
				case XPathNodeType.Attribute:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return this.node.Value;
				case XPathNodeType.Namespace:
					return (this.NsNode != this.Document.NsNodeXml) ? this.NsNode.Value : "http://www.w3.org/XML/1998/namespace";
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				{
					string text = this.node.Value;
					XmlNode nextSibling = this.GetNextSibling(this.node);
					while (nextSibling != null)
					{
						switch (nextSibling.XPathNodeType)
						{
						case XPathNodeType.Text:
						case XPathNodeType.SignificantWhitespace:
						case XPathNodeType.Whitespace:
							text += nextSibling.Value;
							nextSibling = this.GetNextSibling(nextSibling);
							break;
						default:
							return text;
						}
					}
					return text;
				}
				default:
					return string.Empty;
				}
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.node.XmlLang;
			}
		}

		private bool CheckNsNameAppearance(string name, string ns)
		{
			if (this.iteratedNsNames != null && this.iteratedNsNames.Contains(name))
			{
				return true;
			}
			if (ns == string.Empty)
			{
				if (this.iteratedNsNames == null)
				{
					this.iteratedNsNames = new ArrayList();
				}
				else if (this.iteratedNsNames.IsReadOnly)
				{
					this.iteratedNsNames = new ArrayList(this.iteratedNsNames);
				}
				this.iteratedNsNames.Add("xmlns");
				return true;
			}
			return false;
		}

		public override XPathNavigator Clone()
		{
			return new XmlDocumentNavigator(this.node)
			{
				nsNode = this.nsNode,
				iteratedNsNames = ((this.iteratedNsNames != null && !this.iteratedNsNames.IsReadOnly) ? ArrayList.ReadOnly(this.iteratedNsNames) : this.iteratedNsNames)
			};
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			if (this.HasAttributes)
			{
				XmlElement xmlElement = this.Node as XmlElement;
				return (xmlElement == null) ? string.Empty : xmlElement.GetAttribute(localName, namespaceURI);
			}
			return string.Empty;
		}

		public override string GetNamespace(string name)
		{
			return this.Node.GetNamespaceOfPrefix(name);
		}

		public override bool IsDescendant(XPathNavigator other)
		{
			if (this.NsNode != null)
			{
				return false;
			}
			XmlDocumentNavigator xmlDocumentNavigator = other as XmlDocumentNavigator;
			if (xmlDocumentNavigator == null)
			{
				return false;
			}
			for (XmlNode xmlNode = (xmlDocumentNavigator.node.NodeType != XmlNodeType.Attribute) ? xmlDocumentNavigator.node.ParentNode : ((XmlAttribute)xmlDocumentNavigator.node).OwnerElement; xmlNode != null; xmlNode = xmlNode.ParentNode)
			{
				if (xmlNode == this.node)
				{
					return true;
				}
			}
			return false;
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			XmlDocumentNavigator xmlDocumentNavigator = other as XmlDocumentNavigator;
			return xmlDocumentNavigator != null && this.node == xmlDocumentNavigator.node && this.NsNode == xmlDocumentNavigator.NsNode;
		}

		public override bool MoveTo(XPathNavigator other)
		{
			XmlDocumentNavigator xmlDocumentNavigator = other as XmlDocumentNavigator;
			if (xmlDocumentNavigator != null && this.Document == xmlDocumentNavigator.Document)
			{
				this.node = xmlDocumentNavigator.node;
				this.NsNode = xmlDocumentNavigator.NsNode;
				return true;
			}
			return false;
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			if (this.HasAttributes)
			{
				XmlAttribute xmlAttribute = this.node.Attributes[localName, namespaceURI];
				if (xmlAttribute != null)
				{
					this.node = xmlAttribute;
					this.NsNode = null;
					return true;
				}
			}
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			if (this.NodeType == XPathNodeType.Element)
			{
				XmlElement xmlElement = this.node as XmlElement;
				if (!xmlElement.HasAttributes)
				{
					return false;
				}
				for (int i = 0; i < this.node.Attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = this.node.Attributes[i];
					if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
					{
						this.node = xmlAttribute;
						this.NsNode = null;
						return true;
					}
				}
			}
			return false;
		}

		public override bool MoveToFirstChild()
		{
			if (!this.HasChildren)
			{
				return false;
			}
			XmlNode firstChild = this.GetFirstChild(this.node);
			if (firstChild == null)
			{
				return false;
			}
			this.node = firstChild;
			return true;
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			if (this.NodeType != XPathNodeType.Element)
			{
				return false;
			}
			XmlElement xmlElement = this.node as XmlElement;
			XmlAttribute xmlAttribute;
			for (;;)
			{
				if (xmlElement.HasAttributes)
				{
					for (int i = 0; i < xmlElement.Attributes.Count; i++)
					{
						xmlAttribute = xmlElement.Attributes[i];
						if (xmlAttribute.NamespaceURI == "http://www.w3.org/2000/xmlns/")
						{
							if (!this.CheckNsNameAppearance(xmlAttribute.Name, xmlAttribute.Value))
							{
								goto IL_6A;
							}
						}
					}
				}
				if (namespaceScope == XPathNamespaceScope.Local)
				{
					return false;
				}
				xmlElement = (this.GetParentNode(xmlElement) as XmlElement);
				if (xmlElement == null)
				{
					goto Block_6;
				}
			}
			IL_6A:
			this.NsNode = xmlAttribute;
			return true;
			Block_6:
			if (namespaceScope != XPathNamespaceScope.All)
			{
				return false;
			}
			if (this.CheckNsNameAppearance(this.Document.NsNodeXml.Name, this.Document.NsNodeXml.Value))
			{
				return false;
			}
			this.NsNode = this.Document.NsNodeXml;
			return true;
		}

		public override bool MoveToId(string id)
		{
			XmlElement elementById = this.Document.GetElementById(id);
			if (elementById == null)
			{
				return false;
			}
			this.node = elementById;
			return true;
		}

		public override bool MoveToNamespace(string name)
		{
			if (name == "xml")
			{
				this.NsNode = this.Document.NsNodeXml;
				return true;
			}
			if (this.NodeType != XPathNodeType.Element)
			{
				return false;
			}
			XmlElement xmlElement = this.node as XmlElement;
			XmlAttribute xmlAttribute;
			for (;;)
			{
				if (xmlElement.HasAttributes)
				{
					for (int i = 0; i < xmlElement.Attributes.Count; i++)
					{
						xmlAttribute = xmlElement.Attributes[i];
						if (xmlAttribute.NamespaceURI == "http://www.w3.org/2000/xmlns/" && xmlAttribute.Name == name)
						{
							goto Block_5;
						}
					}
				}
				xmlElement = (this.GetParentNode(this.node) as XmlElement);
				if (xmlElement == null)
				{
					return false;
				}
			}
			Block_5:
			this.NsNode = xmlAttribute;
			return true;
		}

		public override bool MoveToNext()
		{
			if (this.NsNode != null)
			{
				return false;
			}
			XmlNode nextSibling = this.node;
			if (this.NodeType == XPathNodeType.Text)
			{
				for (;;)
				{
					nextSibling = this.GetNextSibling(nextSibling);
					if (nextSibling == null)
					{
						break;
					}
					XmlNodeType nodeType = nextSibling.NodeType;
					if (nodeType != XmlNodeType.Text && nodeType != XmlNodeType.CDATA && nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
					{
						goto Block_6;
					}
				}
				return false;
				Block_6:;
			}
			else
			{
				nextSibling = this.GetNextSibling(nextSibling);
			}
			if (nextSibling == null)
			{
				return false;
			}
			this.node = nextSibling;
			return true;
		}

		public override bool MoveToNextAttribute()
		{
			if (this.node == null)
			{
				return false;
			}
			if (this.NodeType != XPathNodeType.Attribute)
			{
				return false;
			}
			int i = 0;
			XmlElement ownerElement = ((XmlAttribute)this.node).OwnerElement;
			if (ownerElement == null)
			{
				return false;
			}
			int count = ownerElement.Attributes.Count;
			while (i < count)
			{
				if (ownerElement.Attributes[i] == this.node)
				{
					break;
				}
				i++;
			}
			if (i == count)
			{
				return false;
			}
			for (i++; i < count; i++)
			{
				if (ownerElement.Attributes[i].NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					this.node = ownerElement.Attributes[i];
					this.NsNode = null;
					return true;
				}
			}
			return false;
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			if (this.NsNode == this.Document.NsNodeXml)
			{
				return false;
			}
			if (this.NsNode == null)
			{
				return false;
			}
			int i = 0;
			XmlElement xmlElement = this.NsNode.OwnerElement;
			if (xmlElement == null)
			{
				return false;
			}
			int count = xmlElement.Attributes.Count;
			while (i < count)
			{
				if (xmlElement.Attributes[i] == this.NsNode)
				{
					break;
				}
				i++;
			}
			if (i == count)
			{
				return false;
			}
			for (i++; i < count; i++)
			{
				if (xmlElement.Attributes[i].NamespaceURI == "http://www.w3.org/2000/xmlns/")
				{
					XmlAttribute xmlAttribute = xmlElement.Attributes[i];
					if (!this.CheckNsNameAppearance(xmlAttribute.Name, xmlAttribute.Value))
					{
						this.NsNode = xmlAttribute;
						return true;
					}
				}
			}
			if (namespaceScope == XPathNamespaceScope.Local)
			{
				return false;
			}
			for (xmlElement = (this.GetParentNode(xmlElement) as XmlElement); xmlElement != null; xmlElement = (this.GetParentNode(xmlElement) as XmlElement))
			{
				if (xmlElement.HasAttributes)
				{
					for (int j = 0; j < xmlElement.Attributes.Count; j++)
					{
						XmlAttribute xmlAttribute2 = xmlElement.Attributes[j];
						if (xmlAttribute2.NamespaceURI == "http://www.w3.org/2000/xmlns/")
						{
							if (!this.CheckNsNameAppearance(xmlAttribute2.Name, xmlAttribute2.Value))
							{
								this.NsNode = xmlAttribute2;
								return true;
							}
						}
					}
				}
			}
			if (namespaceScope != XPathNamespaceScope.All)
			{
				return false;
			}
			if (this.CheckNsNameAppearance(this.Document.NsNodeXml.Name, this.Document.NsNodeXml.Value))
			{
				return false;
			}
			this.NsNode = this.Document.NsNodeXml;
			return true;
		}

		public override bool MoveToParent()
		{
			if (this.NsNode != null)
			{
				this.NsNode = null;
				return true;
			}
			if (this.node.NodeType == XmlNodeType.Attribute)
			{
				XmlElement ownerElement = ((XmlAttribute)this.node).OwnerElement;
				if (ownerElement != null)
				{
					this.node = ownerElement;
					this.NsNode = null;
					return true;
				}
				return false;
			}
			else
			{
				XmlNode parentNode = this.GetParentNode(this.node);
				if (parentNode == null)
				{
					return false;
				}
				this.node = parentNode;
				this.NsNode = null;
				return true;
			}
		}

		public override bool MoveToPrevious()
		{
			if (this.NsNode != null)
			{
				return false;
			}
			XmlNode previousSibling = this.GetPreviousSibling(this.node);
			if (previousSibling == null)
			{
				return false;
			}
			this.node = previousSibling;
			return true;
		}

		public override void MoveToRoot()
		{
			XmlAttribute xmlAttribute = this.node as XmlAttribute;
			XmlNode xmlNode = (xmlAttribute == null) ? this.node : xmlAttribute.OwnerElement;
			if (xmlNode == null)
			{
				return;
			}
			for (XmlNode parentNode = this.GetParentNode(xmlNode); parentNode != null; parentNode = this.GetParentNode(parentNode))
			{
				xmlNode = parentNode;
			}
			this.node = xmlNode;
			this.NsNode = null;
		}

		private XmlNode Node
		{
			get
			{
				return (this.NsNode == null) ? this.node : this.NsNode;
			}
		}

		private XmlNode GetFirstChild(XmlNode n)
		{
			if (n.FirstChild == null)
			{
				return null;
			}
			XmlNodeType nodeType = n.FirstChild.NodeType;
			if (nodeType == XmlNodeType.EntityReference)
			{
				foreach (object obj in n.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType != XmlNodeType.EntityReference)
					{
						return xmlNode;
					}
					XmlNode firstChild = this.GetFirstChild(xmlNode);
					if (firstChild != null)
					{
						return firstChild;
					}
				}
				return null;
			}
			if (nodeType != XmlNodeType.DocumentType && nodeType != XmlNodeType.XmlDeclaration)
			{
				return n.FirstChild;
			}
			return this.GetNextSibling(n.FirstChild);
		}

		private XmlNode GetLastChild(XmlNode n)
		{
			if (n.LastChild == null)
			{
				return null;
			}
			XmlNodeType nodeType = n.LastChild.NodeType;
			if (nodeType == XmlNodeType.EntityReference)
			{
				for (XmlNode xmlNode = n.LastChild; xmlNode != null; xmlNode = xmlNode.PreviousSibling)
				{
					if (xmlNode.NodeType != XmlNodeType.EntityReference)
					{
						return xmlNode;
					}
					XmlNode lastChild = this.GetLastChild(xmlNode);
					if (lastChild != null)
					{
						return lastChild;
					}
				}
				return null;
			}
			if (nodeType != XmlNodeType.DocumentType && nodeType != XmlNodeType.XmlDeclaration)
			{
				return n.LastChild;
			}
			return this.GetPreviousSibling(n.LastChild);
		}

		private XmlNode GetPreviousSibling(XmlNode n)
		{
			XmlNode previousSibling = n.PreviousSibling;
			if (previousSibling != null)
			{
				XmlNodeType nodeType = previousSibling.NodeType;
				if (nodeType != XmlNodeType.EntityReference)
				{
					if (nodeType != XmlNodeType.DocumentType && nodeType != XmlNodeType.XmlDeclaration)
					{
						return previousSibling;
					}
					return this.GetPreviousSibling(previousSibling);
				}
				else
				{
					XmlNode lastChild = this.GetLastChild(previousSibling);
					if (lastChild != null)
					{
						return lastChild;
					}
					return this.GetPreviousSibling(previousSibling);
				}
			}
			else
			{
				if (n.ParentNode == null || n.ParentNode.NodeType != XmlNodeType.EntityReference)
				{
					return null;
				}
				return this.GetPreviousSibling(n.ParentNode);
			}
		}

		private XmlNode GetNextSibling(XmlNode n)
		{
			XmlNode nextSibling = n.NextSibling;
			if (nextSibling != null)
			{
				XmlNodeType nodeType = nextSibling.NodeType;
				if (nodeType != XmlNodeType.EntityReference)
				{
					if (nodeType != XmlNodeType.DocumentType && nodeType != XmlNodeType.XmlDeclaration)
					{
						return n.NextSibling;
					}
					return this.GetNextSibling(nextSibling);
				}
				else
				{
					XmlNode firstChild = this.GetFirstChild(nextSibling);
					if (firstChild != null)
					{
						return firstChild;
					}
					return this.GetNextSibling(nextSibling);
				}
			}
			else
			{
				if (n.ParentNode == null || n.ParentNode.NodeType != XmlNodeType.EntityReference)
				{
					return null;
				}
				return this.GetNextSibling(n.ParentNode);
			}
		}

		private XmlNode GetParentNode(XmlNode n)
		{
			if (n.ParentNode == null)
			{
				return null;
			}
			for (XmlNode parentNode = n.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
			{
				if (parentNode.NodeType != XmlNodeType.EntityReference)
				{
					return parentNode;
				}
			}
			return null;
		}

		public override string LookupNamespace(string prefix)
		{
			return base.LookupNamespace(prefix);
		}

		public override string LookupPrefix(string namespaceUri)
		{
			return base.LookupPrefix(namespaceUri);
		}

		public override bool MoveToChild(XPathNodeType type)
		{
			return base.MoveToChild(type);
		}

		public override bool MoveToChild(string localName, string namespaceURI)
		{
			return base.MoveToChild(localName, namespaceURI);
		}

		public override bool MoveToNext(string localName, string namespaceURI)
		{
			return base.MoveToNext(localName, namespaceURI);
		}

		public override bool MoveToNext(XPathNodeType type)
		{
			return base.MoveToNext(type);
		}

		public override bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end)
		{
			return base.MoveToFollowing(localName, namespaceURI, end);
		}

		public override bool MoveToFollowing(XPathNodeType type, XPathNavigator end)
		{
			return base.MoveToFollowing(type, end);
		}
	}
}
