using Mono.Xml;
using System;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents an attribute. Valid and default values for the attribute are defined in a document type definition (DTD) or schema.</summary>
	public class XmlAttribute : XmlNode, IHasXmlChildNode
	{
		private XmlNameEntry name;

		internal bool isDefault;

		private XmlLinkedNode lastLinkedChild;

		private IXmlSchemaInfo schemaInfo;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlAttribute" /> class.</summary>
		/// <param name="prefix">The namespace prefix.</param>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="namespaceURI">The namespace uniform resource identifier (URI).</param>
		/// <param name="doc">The parent XML document.</param>
		protected internal XmlAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc) : this(prefix, localName, namespaceURI, doc, false, true)
		{
		}

		internal XmlAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc, bool atomizedNames, bool checkNamespace) : base(doc)
		{
			if (!atomizedNames)
			{
				if (prefix == null)
				{
					prefix = string.Empty;
				}
				if (namespaceURI == null)
				{
					namespaceURI = string.Empty;
				}
			}
			if (checkNamespace && (prefix == "xmlns" || (prefix == string.Empty && localName == "xmlns")))
			{
				if (namespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					throw new ArgumentException("Invalid attribute namespace for namespace declaration.");
				}
				if (prefix == "xml" && namespaceURI != "http://www.w3.org/XML/1998/namespace")
				{
					throw new ArgumentException("Invalid attribute namespace for namespace declaration.");
				}
			}
			if (!atomizedNames)
			{
				if (prefix != string.Empty && !XmlChar.IsName(prefix))
				{
					throw new ArgumentException("Invalid attribute prefix.");
				}
				if (!XmlChar.IsName(localName))
				{
					throw new ArgumentException("Invalid attribute local name.");
				}
				prefix = doc.NameTable.Add(prefix);
				localName = doc.NameTable.Add(localName);
				namespaceURI = doc.NameTable.Add(namespaceURI);
			}
			this.name = doc.NameCache.Add(prefix, localName, namespaceURI, true);
		}

		XmlLinkedNode IHasXmlChildNode.LastLinkedChild
		{
			get
			{
				return this.lastLinkedChild;
			}
			set
			{
				this.lastLinkedChild = value;
			}
		}

		/// <summary>Gets the base Uniform Resource Identifier (URI) of the node.</summary>
		/// <returns>The location from which the node was loaded or String.Empty if the node has no base URI. Attribute nodes have the same base URI as their owner element. If an attribute node does not have an owner element, BaseURI returns String.Empty.</returns>
		public override string BaseURI
		{
			get
			{
				return (this.OwnerElement == null) ? string.Empty : this.OwnerElement.BaseURI;
			}
		}

		/// <summary>Gets or sets the concatenated values of the node and all its children.</summary>
		/// <returns>The concatenated values of the node and all its children. For attribute nodes, this property has the same functionality as the <see cref="P:System.Xml.XmlAttribute.Value" /> property.</returns>
		public override string InnerText
		{
			set
			{
				this.Value = value;
			}
		}

		/// <summary>Gets or sets the value of the attribute.</summary>
		/// <returns>The attribute value.</returns>
		/// <exception cref="T:System.Xml.XmlException">The XML specified when setting this property is not well-formed. </exception>
		public override string InnerXml
		{
			set
			{
				this.RemoveAll();
				XmlNamespaceManager nsMgr = base.ConstructNamespaceManager();
				XmlParserContext context = new XmlParserContext(this.OwnerDocument.NameTable, nsMgr, (this.OwnerDocument.DocumentType == null) ? null : this.OwnerDocument.DocumentType.DTD, this.BaseURI, this.XmlLang, this.XmlSpace, null);
				XmlTextReader xmlTextReader = new XmlTextReader(value, XmlNodeType.Attribute, context);
				xmlTextReader.XmlResolver = this.OwnerDocument.Resolver;
				xmlTextReader.Read();
				this.OwnerDocument.ReadAttributeNodeValue(xmlTextReader, this);
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>The name of the attribute node with the prefix removed. In the following example &lt;book bk:genre= 'novel'&gt;, the LocalName of the attribute is genre.</returns>
		public override string LocalName
		{
			get
			{
				return this.name.LocalName;
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>The qualified name of the attribute node.</returns>
		public override string Name
		{
			get
			{
				return this.name.GetPrefixedName(this.OwnerDocument.NameCache);
			}
		}

		/// <summary>Gets the namespace URI of this node.</summary>
		/// <returns>The namespace URI of this node. If the attribute is not explicitly given a namespace, this property returns String.Empty.</returns>
		public override string NamespaceURI
		{
			get
			{
				return this.name.NS;
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>The node type for XmlAttribute nodes is XmlNodeType.Attribute.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Attribute;
			}
		}

		internal override XPathNodeType XPathNodeType
		{
			get
			{
				return XPathNodeType.Attribute;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlDocument" /> to which this node belongs.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlDocument" />.</returns>
		public override XmlDocument OwnerDocument
		{
			get
			{
				return base.OwnerDocument;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlElement" /> to which the attribute belongs.</summary>
		/// <returns>The XmlElement that the attribute belongs to or null if this attribute is not part of an XmlElement.</returns>
		public virtual XmlElement OwnerElement
		{
			get
			{
				return base.AttributeOwnerElement;
			}
		}

		/// <summary>Gets the parent of this node. For XmlAttribute nodes, this property always returns null.</summary>
		/// <returns>For XmlAttribute nodes, this property always returns null.</returns>
		public override XmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the namespace prefix of this node.</summary>
		/// <returns>The namespace prefix of this node. If there is no prefix, this property returns String.Empty.</returns>
		/// <exception cref="T:System.ArgumentException">This node is read-only. </exception>
		/// <exception cref="T:System.Xml.XmlException">The specified prefix contains an invalid character.The specified prefix is malformed.The namespaceURI of this node is null.The specified prefix is "xml", and the namespaceURI of this node is different from "http://www.w3.org/XML/1998/namespace".This node is an attribute, the specified prefix is "xmlns", and the namespaceURI of this node is different from "http://www.w3.org/2000/xmlns/".This node is an attribute, and the qualifiedName of this node is "xmlns" [Namespaces]. </exception>
		public override string Prefix
		{
			get
			{
				return this.name.Prefix;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new XmlException("This node is readonly.");
				}
				if (this.name.Prefix == "xmlns" && value != "xmlns")
				{
					throw new ArgumentException("Cannot bind to the reserved namespace.");
				}
				value = this.OwnerDocument.NameTable.Add(value);
				this.name = this.OwnerDocument.NameCache.Add(value, this.name.LocalName, this.name.NS, true);
			}
		}

		/// <summary>Gets the post-schema-validation-infoset that has been assigned to this node as a result of schema validation.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.IXmlSchemaInfo" /> containing the post-schema-validation-infoset of this node.</returns>
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.schemaInfo;
			}
			internal set
			{
				this.schemaInfo = value;
			}
		}

		/// <summary>Gets a value indicating whether the attribute value was explicitly set.</summary>
		/// <returns>true if this attribute was explicitly given a value in the original instance document; otherwise, false. A value of false indicates that the value of the attribute came from the DTD.</returns>
		public virtual bool Specified
		{
			get
			{
				return !this.isDefault;
			}
		}

		/// <summary>Gets or sets the value of the node.</summary>
		/// <returns>The value returned depends on the <see cref="P:System.Xml.XmlNode.NodeType" /> of the node. For XmlAttribute nodes, this property is the value of attribute.</returns>
		/// <exception cref="T:System.ArgumentException">The node is read-only and a set operation is called. </exception>
		public override string Value
		{
			get
			{
				return this.InnerText;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new ArgumentException("Attempt to modify a read-only node.");
				}
				this.OwnerDocument.CheckIdTableUpdate(this, this.InnerText, value);
				XmlNode xmlNode = this.FirstChild as XmlCharacterData;
				if (xmlNode == null)
				{
					this.RemoveAll();
					base.AppendChild(this.OwnerDocument.CreateTextNode(value), false);
				}
				else if (this.FirstChild.NextSibling != null)
				{
					this.RemoveAll();
					base.AppendChild(this.OwnerDocument.CreateTextNode(value), false);
				}
				else
				{
					xmlNode.Value = value;
				}
				this.isDefault = false;
			}
		}

		internal override string XmlLang
		{
			get
			{
				return (this.OwnerElement == null) ? string.Empty : this.OwnerElement.XmlLang;
			}
		}

		internal override XmlSpace XmlSpace
		{
			get
			{
				return (this.OwnerElement == null) ? XmlSpace.None : this.OwnerElement.XmlSpace;
			}
		}

		/// <summary>Adds the specified node to the end of the list of child nodes, of this node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> added.</returns>
		/// <param name="newChild">The <see cref="T:System.Xml.XmlNode" /> to add.</param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.This node is read-only. </exception>
		public override XmlNode AppendChild(XmlNode child)
		{
			return base.AppendChild(child);
		}

		/// <summary>Inserts the specified node immediately before the specified reference node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> inserted.</returns>
		/// <param name="newChild">The <see cref="T:System.Xml.XmlNode" /> to insert. </param>
		/// <param name="refChild">The <see cref="T:System.Xml.XmlNode" /> that is the reference node. The <paramref name="newChild" /> is placed before this node. </param>
		/// <exception cref="T:System.InvalidOperationException">The current node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.The <paramref name="refChild" /> is not a child of this node.This node is read-only. </exception>
		public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
		{
			return base.InsertBefore(newChild, refChild);
		}

		/// <summary>Inserts the specified node immediately after the specified reference node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> inserted.</returns>
		/// <param name="newChild">The <see cref="T:System.Xml.XmlNode" /> to insert. </param>
		/// <param name="refChild">The <see cref="T:System.Xml.XmlNode" /> that is the reference node. The <paramref name="newChild" /> is placed after the <paramref name="refChild" />.</param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.The <paramref name="refChild" /> is not a child of this node.This node is read-only. </exception>
		public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
		{
			return base.InsertAfter(newChild, refChild);
		}

		/// <summary>Adds the specified node to the beginning of the list of child nodes for this node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> added.</returns>
		/// <param name="newChild">The <see cref="T:System.Xml.XmlNode" /> to add. If it is an <see cref="T:System.Xml.XmlDocumentFragment" />, the entire contents of the document fragment are moved into the child list of this node.</param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.This node is read-only. </exception>
		public override XmlNode PrependChild(XmlNode node)
		{
			return base.PrependChild(node);
		}

		/// <summary>Removes the specified child node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> removed.</returns>
		/// <param name="oldChild">The <see cref="T:System.Xml.XmlNode" /> to remove.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="oldChild" /> is not a child of this node. Or this node is read-only. </exception>
		public override XmlNode RemoveChild(XmlNode node)
		{
			return base.RemoveChild(node);
		}

		/// <summary>Replaces the child node specified with the new child node specified.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> replaced.</returns>
		/// <param name="newChild">The new child <see cref="T:System.Xml.XmlNode" />.</param>
		/// <param name="oldChild">The <see cref="T:System.Xml.XmlNode" /> to replace. </param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.This node is read-only.The <paramref name="oldChild" /> is not a child of this node. </exception>
		public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
		{
			return base.ReplaceChild(newChild, oldChild);
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The duplicate node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself </param>
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = this.OwnerDocument.CreateAttribute(this.name.Prefix, this.name.LocalName, this.name.NS, true, false);
			if (deep)
			{
				for (XmlNode xmlNode2 = this.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
				{
					xmlNode.AppendChild(xmlNode2.CloneNode(deep), false);
				}
			}
			return xmlNode;
		}

		internal void SetDefault()
		{
			this.isDefault = true;
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				xmlNode.WriteTo(w);
			}
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			if (this.isDefault)
			{
				return;
			}
			w.WriteStartAttribute((this.name.NS.Length <= 0) ? string.Empty : this.name.Prefix, this.name.LocalName, this.name.NS);
			this.WriteContentTo(w);
			w.WriteEndAttribute();
		}

		internal DTDAttributeDefinition GetAttributeDefinition()
		{
			if (this.OwnerElement == null)
			{
				return null;
			}
			DTDAttListDeclaration dtdattListDeclaration = (this.OwnerDocument.DocumentType == null) ? null : this.OwnerDocument.DocumentType.DTD.AttListDecls[this.OwnerElement.Name];
			return (dtdattListDeclaration == null) ? null : dtdattListDeclaration[this.Name];
		}
	}
}
