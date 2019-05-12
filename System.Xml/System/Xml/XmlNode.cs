using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents a single node in the XML document. </summary>
	public abstract class XmlNode : IEnumerable, ICloneable, IXPathNavigable
	{
		private static XmlNode.EmptyNodeList emptyList = new XmlNode.EmptyNodeList();

		private XmlDocument ownerDocument;

		private XmlNode parentNode;

		private XmlNodeListChildren childNodes;

		internal XmlNode(XmlDocument ownerDocument)
		{
			this.ownerDocument = ownerDocument;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.XmlNode.Clone" />.</summary>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.XmlNode.GetEnumerator" />.</summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Gets an <see cref="T:System.Xml.XmlAttributeCollection" /> containing the attributes of this node.</summary>
		/// <returns>An XmlAttributeCollection containing the attributes of the node.If the node is of type XmlNodeType.Element, the attributes of the node are returned. Otherwise, this property returns null.</returns>
		public virtual XmlAttributeCollection Attributes
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the base URI of the current node.</summary>
		/// <returns>The location from which the node was loaded or String.Empty if the node has no base URI.</returns>
		public virtual string BaseURI
		{
			get
			{
				return (this.ParentNode == null) ? string.Empty : this.ParentNode.ChildrenBaseURI;
			}
		}

		internal virtual string ChildrenBaseURI
		{
			get
			{
				return this.BaseURI;
			}
		}

		/// <summary>Gets all the child nodes of the node.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNodeList" /> that contains all the child nodes of the node.If there are no child nodes, this property returns an empty <see cref="T:System.Xml.XmlNodeList" />.</returns>
		public virtual XmlNodeList ChildNodes
		{
			get
			{
				IHasXmlChildNode hasXmlChildNode = this as IHasXmlChildNode;
				if (hasXmlChildNode == null)
				{
					return XmlNode.emptyList;
				}
				if (this.childNodes == null)
				{
					this.childNodes = new XmlNodeListChildren(hasXmlChildNode);
				}
				return this.childNodes;
			}
		}

		/// <summary>Gets the first child of the node.</summary>
		/// <returns>The first child of the node. If there is no such node, null is returned.</returns>
		public virtual XmlNode FirstChild
		{
			get
			{
				IHasXmlChildNode hasXmlChildNode = this as IHasXmlChildNode;
				XmlLinkedNode xmlLinkedNode = (hasXmlChildNode != null) ? hasXmlChildNode.LastLinkedChild : null;
				return (xmlLinkedNode != null) ? xmlLinkedNode.NextLinkedSibling : null;
			}
		}

		/// <summary>Gets a value indicating whether this node has any child nodes.</summary>
		/// <returns>true if the node has child nodes; otherwise, false.</returns>
		public virtual bool HasChildNodes
		{
			get
			{
				return this.LastChild != null;
			}
		}

		/// <summary>Gets or sets the concatenated values of the node and all its child nodes.</summary>
		/// <returns>The concatenated values of the node and all its child nodes.</returns>
		public virtual string InnerText
		{
			get
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.CDATA || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
				{
					return this.Value;
				}
				if (this.FirstChild == null)
				{
					return string.Empty;
				}
				if (this.FirstChild == this.LastChild)
				{
					return (this.FirstChild.NodeType == XmlNodeType.Comment) ? string.Empty : this.FirstChild.InnerText;
				}
				StringBuilder stringBuilder = null;
				this.AppendChildValues(ref stringBuilder);
				return (stringBuilder != null) ? stringBuilder.ToString() : string.Empty;
			}
			set
			{
				if (!(this is XmlDocumentFragment))
				{
					throw new InvalidOperationException("This node is read only. Cannot be modified.");
				}
				this.RemoveAll();
				this.AppendChild(this.OwnerDocument.CreateTextNode(value));
			}
		}

		private void AppendChildValues(ref StringBuilder builder)
		{
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				XmlNodeType nodeType = xmlNode.NodeType;
				if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.CDATA || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
				{
					if (builder == null)
					{
						builder = new StringBuilder();
					}
					builder.Append(xmlNode.Value);
				}
				xmlNode.AppendChildValues(ref builder);
			}
		}

		/// <summary>Gets or sets the markup representing only the child nodes of this node.</summary>
		/// <returns>The markup of the child nodes of this node.Note:InnerXml does not return default attributes.</returns>
		/// <exception cref="T:System.InvalidOperationException">Setting this property on a node that cannot have child nodes. </exception>
		/// <exception cref="T:System.Xml.XmlException">The XML specified when setting this property is not well-formed. </exception>
		public virtual string InnerXml
		{
			get
			{
				StringWriter stringWriter = new StringWriter();
				XmlTextWriter w = new XmlTextWriter(stringWriter);
				this.WriteContentTo(w);
				return stringWriter.GetStringBuilder().ToString();
			}
			set
			{
				throw new InvalidOperationException("This node is readonly or doesn't have any children.");
			}
		}

		/// <summary>Gets a value indicating whether the node is read-only.</summary>
		/// <returns>true if the node is read-only; otherwise false.</returns>
		public virtual bool IsReadOnly
		{
			get
			{
				XmlNode xmlNode = this;
				for (;;)
				{
					switch (xmlNode.NodeType)
					{
					case XmlNodeType.Attribute:
						xmlNode = ((XmlAttribute)xmlNode).OwnerElement;
						break;
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						goto IL_3D;
					case XmlNodeType.EntityReference:
					case XmlNodeType.Entity:
						return true;
					default:
						goto IL_3D;
					}
					IL_49:
					if (xmlNode == null)
					{
						return false;
					}
					continue;
					IL_3D:
					xmlNode = xmlNode.ParentNode;
					goto IL_49;
				}
				return true;
			}
		}

		/// <summary>Gets the first child element with the specified <see cref="P:System.Xml.XmlNode.Name" />.</summary>
		/// <returns>The first <see cref="T:System.Xml.XmlElement" /> that matches the specified name.</returns>
		/// <param name="name">The qualified name of the element to retrieve. </param>
		public virtual XmlElement this[string name]
		{
			get
			{
				for (int i = 0; i < this.ChildNodes.Count; i++)
				{
					XmlNode xmlNode = this.ChildNodes[i];
					if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == name)
					{
						return (XmlElement)xmlNode;
					}
				}
				return null;
			}
		}

		/// <summary>Gets the first child element with the specified <see cref="P:System.Xml.XmlNode.LocalName" /> and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The first <see cref="T:System.Xml.XmlElement" /> with the matching <paramref name="localname" /> and <paramref name="ns" />.</returns>
		/// <param name="localname">The local name of the element. </param>
		/// <param name="ns">The namespace URI of the element. </param>
		public virtual XmlElement this[string localname, string ns]
		{
			get
			{
				for (int i = 0; i < this.ChildNodes.Count; i++)
				{
					XmlNode xmlNode = this.ChildNodes[i];
					if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.LocalName == localname && xmlNode.NamespaceURI == ns)
					{
						return (XmlElement)xmlNode;
					}
				}
				return null;
			}
		}

		/// <summary>Gets the last child of the node.</summary>
		/// <returns>The last child of the node. If there is no such node, null is returned.</returns>
		public virtual XmlNode LastChild
		{
			get
			{
				IHasXmlChildNode hasXmlChildNode = this as IHasXmlChildNode;
				return (hasXmlChildNode != null) ? hasXmlChildNode.LastLinkedChild : null;
			}
		}

		/// <summary>When overridden in a derived class, gets the local name of the node.</summary>
		/// <returns>The name of the node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.The name returned is dependent on the <see cref="P:System.Xml.XmlNode.NodeType" /> of the node: Type Name Attribute The local name of the attribute. CDATA #cdata-section Comment #comment Document #document DocumentFragment #document-fragment DocumentType The document type name. Element The local name of the element. Entity The name of the entity. EntityReference The name of the entity referenced. Notation The notation name. ProcessingInstruction The target of the processing instruction. Text #text Whitespace #whitespace SignificantWhitespace #significant-whitespace XmlDeclaration #xml-declaration </returns>
		public abstract string LocalName { get; }

		/// <summary>When overridden in a derived class, gets the qualified name of the node.</summary>
		/// <returns>The qualified name of the node. The name returned is dependent on the <see cref="P:System.Xml.XmlNode.NodeType" /> of the node: Type Name Attribute The qualified name of the attribute. CDATA #cdata-section Comment #comment Document #document DocumentFragment #document-fragment DocumentType The document type name. Element The qualified name of the element. Entity The name of the entity. EntityReference The name of the entity referenced. Notation The notation name. ProcessingInstruction The target of the processing instruction. Text #text Whitespace #whitespace SignificantWhitespace #significant-whitespace XmlDeclaration #xml-declaration </returns>
		public abstract string Name { get; }

		/// <summary>Gets the namespace URI of this node.</summary>
		/// <returns>The namespace URI of this node. If there is no namespace URI, this property returns String.Empty.</returns>
		public virtual string NamespaceURI
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>Gets the node immediately following this node.</summary>
		/// <returns>The next XmlNode. If there is no next node, null is returned.</returns>
		public virtual XmlNode NextSibling
		{
			get
			{
				return null;
			}
		}

		/// <summary>When overridden in a derived class, gets the type of the current node.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlNodeType" /> values.</returns>
		public abstract XmlNodeType NodeType { get; }

		internal virtual XPathNodeType XPathNodeType
		{
			get
			{
				throw new InvalidOperationException("Can not get XPath node type from " + base.GetType().ToString());
			}
		}

		/// <summary>Gets the markup representing this node and all its child nodes.</summary>
		/// <returns>The markup containing this node and all its child nodes.Note:OuterXml does not return default attributes.</returns>
		public virtual string OuterXml
		{
			get
			{
				StringWriter stringWriter = new StringWriter();
				XmlTextWriter w = new XmlTextWriter(stringWriter);
				this.WriteTo(w);
				return stringWriter.ToString();
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlDocument" /> to which this node belongs.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlDocument" /> to which this node belongs.If the node is an <see cref="T:System.Xml.XmlDocument" /> (NodeType equals XmlNodeType.Document), this property returns null.</returns>
		public virtual XmlDocument OwnerDocument
		{
			get
			{
				return this.ownerDocument;
			}
		}

		/// <summary>Gets the parent of this node (for nodes that can have parents).</summary>
		/// <returns>The XmlNode that is the parent of the current node. If a node has just been created and not yet added to the tree, or if it has been removed from the tree, the parent is null. For all other nodes, the value returned depends on the <see cref="P:System.Xml.XmlNode.NodeType" /> of the node. The following table describes the possible return values for the ParentNode property.NodeType Return Value of ParentNode Attribute, Document, DocumentFragment, Entity, Notation Returns null; these nodes do not have parents. CDATA Returns the element or entity reference containing the CDATA section. Comment Returns the element, entity reference, document type, or document containing the comment. DocumentType Returns the document node. Element Returns the parent node of the element. If the element is the root node in the tree, the parent is the document node. EntityReference Returns the element, attribute, or entity reference containing the entity reference. ProcessingInstruction Returns the document, element, document type, or entity reference containing the processing instruction. Text Returns the parent element, attribute, or entity reference containing the text node. </returns>
		public virtual XmlNode ParentNode
		{
			get
			{
				return this.parentNode;
			}
		}

		/// <summary>Gets or sets the namespace prefix of this node.</summary>
		/// <returns>The namespace prefix of this node. For example, Prefix is bk for the element &lt;bk:book&gt;. If there is no prefix, this property returns String.Empty.</returns>
		/// <exception cref="T:System.ArgumentException">This node is read-only. </exception>
		/// <exception cref="T:System.Xml.XmlException">The specified prefix contains an invalid character.The specified prefix is malformed.The specified prefix is "xml" and the namespaceURI of this node is different from "http://www.w3.org/XML/1998/namespace".This node is an attribute and the specified prefix is "xmlns" and the namespaceURI of this node is different from "http://www.w3.org/2000/xmlns/ ".This node is an attribute and the qualifiedName of this node is "xmlns". </exception>
		public virtual string Prefix
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		/// <summary>Gets the node immediately preceding this node.</summary>
		/// <returns>The preceding XmlNode. If there is no preceding node, null is returned.</returns>
		public virtual XmlNode PreviousSibling
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the value of the node.</summary>
		/// <returns>The value returned depends on the <see cref="P:System.Xml.XmlNode.NodeType" /> of the node: Type Value Attribute The value of the attribute. CDATASection The content of the CDATA Section. Comment The content of the comment. Document null. DocumentFragment null. DocumentType null. Element null. You can use the <see cref="P:System.Xml.XmlElement.InnerText" /> or <see cref="P:System.Xml.XmlElement.InnerXml" /> properties to access the value of the element node. Entity null. EntityReference null. Notation null. ProcessingInstruction The entire content excluding the target. Text The content of the text node. SignificantWhitespace The white space characters. White space can consist of one or more space characters, carriage returns, line feeds, or tabs. Whitespace The white space characters. White space can consist of one or more space characters, carriage returns, line feeds, or tabs. XmlDeclaration The content of the declaration (that is, everything between &lt;?xml and ?&gt;). </returns>
		/// <exception cref="T:System.ArgumentException">Setting the value of a node that is read-only. </exception>
		/// <exception cref="T:System.InvalidOperationException">Setting the value of a node that is not supposed to have a value (for example, an Element node). </exception>
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("This node does not have a value");
			}
		}

		internal virtual string XmlLang
		{
			get
			{
				if (this.Attributes != null)
				{
					for (int i = 0; i < this.Attributes.Count; i++)
					{
						XmlAttribute xmlAttribute = this.Attributes[i];
						if (xmlAttribute.Name == "xml:lang")
						{
							return xmlAttribute.Value;
						}
					}
				}
				return (this.ParentNode == null) ? this.OwnerDocument.XmlLang : this.ParentNode.XmlLang;
			}
		}

		internal virtual XmlSpace XmlSpace
		{
			get
			{
				if (this.Attributes != null)
				{
					for (int i = 0; i < this.Attributes.Count; i++)
					{
						XmlAttribute xmlAttribute = this.Attributes[i];
						if (xmlAttribute.Name == "xml:space")
						{
							string value = xmlAttribute.Value;
							if (value != null)
							{
								if (XmlNode.<>f__switch$map44 == null)
								{
									XmlNode.<>f__switch$map44 = new Dictionary<string, int>(2)
									{
										{
											"preserve",
											0
										},
										{
											"default",
											1
										}
									};
								}
								int num;
								if (XmlNode.<>f__switch$map44.TryGetValue(value, out num))
								{
									if (num == 0)
									{
										return XmlSpace.Preserve;
									}
									if (num == 1)
									{
										return XmlSpace.Default;
									}
								}
							}
							break;
						}
					}
				}
				return (this.ParentNode == null) ? this.OwnerDocument.XmlSpace : this.ParentNode.XmlSpace;
			}
		}

		/// <summary>Gets the post schema validation infoset that has been assigned to this node as a result of schema validation.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.IXmlSchemaInfo" /> object containing the post schema validation infoset of this node</returns>
		public virtual IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return null;
			}
			internal set
			{
			}
		}

		/// <summary>Adds the specified node to the end of the list of child nodes, of this node.</summary>
		/// <returns>The node added.</returns>
		/// <param name="newChild">The node to add. All the contents of the node to be added are moved into the specified location. </param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.This node is read-only. </exception>
		public virtual XmlNode AppendChild(XmlNode newChild)
		{
			return this.InsertBefore(newChild, null);
		}

		internal XmlNode AppendChild(XmlNode newChild, bool checkNodeType)
		{
			return this.InsertBefore(newChild, null, checkNodeType, true);
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned node.</returns>
		public virtual XmlNode Clone()
		{
			return this.CloneNode(true);
		}

		/// <summary>When overridden in a derived class, creates a duplicate of the node.</summary>
		/// <returns>The cloned node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. </param>
		/// <exception cref="T:System.InvalidOperationException">Calling this method on a node type that cannot be cloned. </exception>
		public abstract XmlNode CloneNode(bool deep);

		/// <summary>Creates an <see cref="T:System.Xml.XPath.XPathNavigator" /> for navigating this object.</summary>
		/// <returns>An XPathNavigator object. The XPathNavigator is positioned on the node from which the method was called. It is not positioned on the root of the document.</returns>
		public virtual XPathNavigator CreateNavigator()
		{
			return this.OwnerDocument.CreateNavigator(this);
		}

		/// <summary>Provides support for the for each style iteration over the nodes in the XmlNode.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return this.ChildNodes.GetEnumerator();
		}

		/// <summary>Looks up the closest xmlns declaration for the given prefix that is in scope for the current node and returns the namespace URI in the declaration.</summary>
		/// <returns>The namespace URI of the specified prefix.</returns>
		/// <param name="prefix">Prefix whose namespace URI you want to find. </param>
		public virtual string GetNamespaceOfPrefix(string prefix)
		{
			if (prefix != null)
			{
				if (XmlNode.<>f__switch$map45 == null)
				{
					XmlNode.<>f__switch$map45 = new Dictionary<string, int>(2)
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
				if (XmlNode.<>f__switch$map45.TryGetValue(prefix, out num))
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
				XmlNodeType nodeType = this.NodeType;
				XmlNode xmlNode;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						xmlNode = this.ParentNode;
					}
					else
					{
						xmlNode = ((XmlAttribute)this).OwnerElement;
						if (xmlNode == null)
						{
							return string.Empty;
						}
					}
				}
				else
				{
					xmlNode = this;
				}
				while (xmlNode != null)
				{
					if (xmlNode.Prefix == prefix)
					{
						return xmlNode.NamespaceURI;
					}
					if (xmlNode.NodeType == XmlNodeType.Element && ((XmlElement)xmlNode).HasAttributes)
					{
						int count = xmlNode.Attributes.Count;
						for (int i = 0; i < count; i++)
						{
							XmlAttribute xmlAttribute = xmlNode.Attributes[i];
							if ((prefix == xmlAttribute.LocalName && xmlAttribute.Prefix == "xmlns") || (xmlAttribute.Name == "xmlns" && prefix == string.Empty))
							{
								return xmlAttribute.Value;
							}
						}
					}
					xmlNode = xmlNode.ParentNode;
				}
				return string.Empty;
			}
			throw new ArgumentNullException("prefix");
		}

		/// <summary>Looks up the closest xmlns declaration for the given namespace URI that is in scope for the current node and returns the prefix defined in that declaration.</summary>
		/// <returns>The prefix for the specified namespace URI.</returns>
		/// <param name="namespaceURI">Namespace URI whose prefix you want to find. </param>
		public virtual string GetPrefixOfNamespace(string namespaceURI)
		{
			if (namespaceURI != null)
			{
				if (XmlNode.<>f__switch$map46 == null)
				{
					XmlNode.<>f__switch$map46 = new Dictionary<string, int>(2)
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
				if (XmlNode.<>f__switch$map46.TryGetValue(namespaceURI, out num))
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
			XmlNodeType nodeType = this.NodeType;
			XmlNode xmlNode;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					xmlNode = this.ParentNode;
				}
				else
				{
					xmlNode = ((XmlAttribute)this).OwnerElement;
				}
			}
			else
			{
				xmlNode = this;
			}
			while (xmlNode != null)
			{
				if (xmlNode.NodeType == XmlNodeType.Element && ((XmlElement)xmlNode).HasAttributes)
				{
					for (int i = 0; i < xmlNode.Attributes.Count; i++)
					{
						XmlAttribute xmlAttribute = xmlNode.Attributes[i];
						if (xmlAttribute.Prefix == "xmlns" && xmlAttribute.Value == namespaceURI)
						{
							return xmlAttribute.LocalName;
						}
						if (xmlAttribute.Name == "xmlns" && xmlAttribute.Value == namespaceURI)
						{
							return string.Empty;
						}
					}
				}
				xmlNode = xmlNode.ParentNode;
			}
			return string.Empty;
		}

		/// <summary>Inserts the specified node immediately after the specified reference node.</summary>
		/// <returns>The node being inserted.</returns>
		/// <param name="newChild">The XmlNode to insert. </param>
		/// <param name="refChild">The XmlNode that is the reference node. The <paramref name="newNode" /> is placed after the <paramref name="refNode" />. </param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.The <paramref name="refChild" /> is not a child of this node.This node is read-only. </exception>
		public virtual XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
		{
			XmlNode refChild2 = null;
			if (refChild != null)
			{
				refChild2 = refChild.NextSibling;
			}
			else if (this.FirstChild != null)
			{
				refChild2 = this.FirstChild;
			}
			return this.InsertBefore(newChild, refChild2);
		}

		/// <summary>Inserts the specified node immediately before the specified reference node.</summary>
		/// <returns>The node being inserted.</returns>
		/// <param name="newChild">The XmlNode to insert. </param>
		/// <param name="refChild">The XmlNode that is the reference node. The <paramref name="newChild" /> is placed before this node. </param>
		/// <exception cref="T:System.InvalidOperationException">The current node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.The <paramref name="refChild" /> is not a child of this node.This node is read-only. </exception>
		public virtual XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
		{
			return this.InsertBefore(newChild, refChild, true, true);
		}

		internal bool IsAncestor(XmlNode newChild)
		{
			for (XmlNode xmlNode = this.ParentNode; xmlNode != null; xmlNode = xmlNode.ParentNode)
			{
				if (xmlNode == newChild)
				{
					return true;
				}
			}
			return false;
		}

		internal XmlNode InsertBefore(XmlNode newChild, XmlNode refChild, bool checkNodeType, bool raiseEvent)
		{
			if (checkNodeType)
			{
				this.CheckNodeInsertion(newChild, refChild);
			}
			if (newChild == refChild)
			{
				return newChild;
			}
			IHasXmlChildNode hasXmlChildNode = (IHasXmlChildNode)this;
			XmlDocument xmlDocument = (this.NodeType != XmlNodeType.Document) ? this.OwnerDocument : ((XmlDocument)this);
			if (raiseEvent)
			{
				xmlDocument.onNodeInserting(newChild, this);
			}
			if (newChild.ParentNode != null)
			{
				newChild.ParentNode.RemoveChild(newChild, checkNodeType);
			}
			if (newChild.NodeType == XmlNodeType.DocumentFragment)
			{
				while (newChild.FirstChild != null)
				{
					this.InsertBefore(newChild.FirstChild, refChild);
				}
			}
			else
			{
				XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)newChild;
				xmlLinkedNode.parentNode = this;
				if (refChild == null)
				{
					if (hasXmlChildNode.LastLinkedChild != null)
					{
						XmlLinkedNode nextLinkedSibling = (XmlLinkedNode)this.FirstChild;
						hasXmlChildNode.LastLinkedChild.NextLinkedSibling = xmlLinkedNode;
						hasXmlChildNode.LastLinkedChild = xmlLinkedNode;
						xmlLinkedNode.NextLinkedSibling = nextLinkedSibling;
					}
					else
					{
						hasXmlChildNode.LastLinkedChild = xmlLinkedNode;
						hasXmlChildNode.LastLinkedChild.NextLinkedSibling = xmlLinkedNode;
					}
				}
				else
				{
					XmlLinkedNode xmlLinkedNode2 = refChild.PreviousSibling as XmlLinkedNode;
					if (xmlLinkedNode2 == null)
					{
						hasXmlChildNode.LastLinkedChild.NextLinkedSibling = xmlLinkedNode;
					}
					else
					{
						xmlLinkedNode2.NextLinkedSibling = xmlLinkedNode;
					}
					xmlLinkedNode.NextLinkedSibling = (refChild as XmlLinkedNode);
				}
				switch (newChild.NodeType)
				{
				case XmlNodeType.EntityReference:
					((XmlEntityReference)newChild).SetReferencedEntityContent();
					break;
				}
				if (raiseEvent)
				{
					xmlDocument.onNodeInserted(newChild, newChild.ParentNode);
				}
			}
			return newChild;
		}

		private void CheckNodeInsertion(XmlNode newChild, XmlNode refChild)
		{
			XmlDocument xmlDocument = (this.NodeType != XmlNodeType.Document) ? this.OwnerDocument : ((XmlDocument)this);
			if (this.NodeType != XmlNodeType.Element && this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Document && this.NodeType != XmlNodeType.DocumentFragment)
			{
				throw new InvalidOperationException(string.Format("Node cannot be appended to current node {0}.", this.NodeType));
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				XmlNodeType nodeType2 = newChild.NodeType;
				switch (nodeType2)
				{
				case XmlNodeType.Entity:
				case XmlNodeType.Document:
				case XmlNodeType.DocumentType:
				case XmlNodeType.Notation:
					break;
				default:
					if (nodeType2 != XmlNodeType.Attribute && nodeType2 != XmlNodeType.XmlDeclaration)
					{
						goto IL_125;
					}
					break;
				}
				throw new InvalidOperationException("Cannot insert specified type of node as a child of this node.");
			}
			if (nodeType == XmlNodeType.Attribute)
			{
				switch (newChild.NodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.EntityReference:
					goto IL_125;
				}
				throw new InvalidOperationException(string.Format("Cannot insert specified type of node {0} as a child of this node {1}.", newChild.NodeType, this.NodeType));
			}
			IL_125:
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException("The node is readonly.");
			}
			if (newChild.OwnerDocument != xmlDocument)
			{
				throw new ArgumentException("Can't append a node created by another document.");
			}
			if (refChild != null && refChild.ParentNode != this)
			{
				throw new ArgumentException("The reference node is not a child of this node.");
			}
			if (this == xmlDocument && xmlDocument.DocumentElement != null && newChild is XmlElement && newChild != xmlDocument.DocumentElement)
			{
				throw new XmlException("multiple document element not allowed.");
			}
			if (newChild == this || this.IsAncestor(newChild))
			{
				throw new ArgumentException("Cannot insert a node or any ancestor of that node as a child of itself.");
			}
		}

		/// <summary>Puts all XmlText nodes in the full depth of the sub-tree underneath this XmlNode into a "normal" form where only markup (that is, tags, comments, processing instructions, CDATA sections, and entity references) separates XmlText nodes, that is, there are no adjacent XmlText nodes.</summary>
		public virtual void Normalize()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.ChildNodes.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				XmlNode xmlNode = this.ChildNodes[i];
				XmlNodeType nodeType = xmlNode.NodeType;
				if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace && nodeType != XmlNodeType.Text)
				{
					xmlNode.Normalize();
					this.NormalizeRange(num, i, stringBuilder);
					num = i + 1;
				}
				else
				{
					stringBuilder.Append(xmlNode.Value);
				}
			}
			if (num < count)
			{
				this.NormalizeRange(num, count, stringBuilder);
			}
		}

		private void NormalizeRange(int start, int i, StringBuilder tmpBuilder)
		{
			int num = -1;
			for (int j = start; j < i; j++)
			{
				XmlNode xmlNode = this.ChildNodes[j];
				if (xmlNode.NodeType == XmlNodeType.Text)
				{
					num = j;
					break;
				}
				if (xmlNode.NodeType == XmlNodeType.SignificantWhitespace)
				{
					num = j;
				}
			}
			if (num >= 0)
			{
				for (int k = start; k < num; k++)
				{
					this.RemoveChild(this.ChildNodes[start]);
				}
				int num2 = i - num - 1;
				for (int l = 0; l < num2; l++)
				{
					this.RemoveChild(this.ChildNodes[start + 1]);
				}
			}
			if (num >= 0)
			{
				this.ChildNodes[start].Value = tmpBuilder.ToString();
			}
			tmpBuilder.Length = 0;
		}

		/// <summary>Adds the specified node to the beginning of the list of child nodes for this node.</summary>
		/// <returns>The node added.</returns>
		/// <param name="newChild">The node to add. All the contents of the node to be added are moved into the specified location.</param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.This node is read-only. </exception>
		public virtual XmlNode PrependChild(XmlNode newChild)
		{
			return this.InsertAfter(newChild, null);
		}

		/// <summary>Removes all the child nodes and/or attributes of the current node.</summary>
		public virtual void RemoveAll()
		{
			if (this.Attributes != null)
			{
				this.Attributes.RemoveAll();
			}
			XmlNode nextSibling;
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = nextSibling)
			{
				nextSibling = xmlNode.NextSibling;
				this.RemoveChild(xmlNode);
			}
		}

		/// <summary>Removes specified child node.</summary>
		/// <returns>The node removed.</returns>
		/// <param name="oldChild">The node being removed. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="oldChild" /> is not a child of this node. Or this node is read-only. </exception>
		public virtual XmlNode RemoveChild(XmlNode oldChild)
		{
			return this.RemoveChild(oldChild, true);
		}

		private void CheckNodeRemoval()
		{
			if (this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Element && this.NodeType != XmlNodeType.Document && this.NodeType != XmlNodeType.DocumentFragment)
			{
				throw new ArgumentException(string.Format("This {0} node cannot remove its child.", this.NodeType));
			}
			if (this.IsReadOnly)
			{
				throw new ArgumentException(string.Format("This {0} node is read only.", this.NodeType));
			}
		}

		internal XmlNode RemoveChild(XmlNode oldChild, bool checkNodeType)
		{
			if (oldChild == null)
			{
				throw new NullReferenceException();
			}
			XmlDocument xmlDocument = (this.NodeType != XmlNodeType.Document) ? this.OwnerDocument : ((XmlDocument)this);
			if (oldChild.ParentNode != this)
			{
				throw new ArgumentException("The node to be removed is not a child of this node.");
			}
			if (checkNodeType)
			{
				xmlDocument.onNodeRemoving(oldChild, oldChild.ParentNode);
			}
			if (checkNodeType)
			{
				this.CheckNodeRemoval();
			}
			IHasXmlChildNode hasXmlChildNode = (IHasXmlChildNode)this;
			if (object.ReferenceEquals(hasXmlChildNode.LastLinkedChild, hasXmlChildNode.LastLinkedChild.NextLinkedSibling) && object.ReferenceEquals(hasXmlChildNode.LastLinkedChild, oldChild))
			{
				hasXmlChildNode.LastLinkedChild = null;
			}
			else
			{
				XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)oldChild;
				XmlLinkedNode xmlLinkedNode2 = hasXmlChildNode.LastLinkedChild;
				XmlLinkedNode xmlLinkedNode3 = (XmlLinkedNode)this.FirstChild;
				while (!object.ReferenceEquals(xmlLinkedNode2.NextLinkedSibling, hasXmlChildNode.LastLinkedChild) && !object.ReferenceEquals(xmlLinkedNode2.NextLinkedSibling, xmlLinkedNode))
				{
					xmlLinkedNode2 = xmlLinkedNode2.NextLinkedSibling;
				}
				if (!object.ReferenceEquals(xmlLinkedNode2.NextLinkedSibling, xmlLinkedNode))
				{
					throw new ArgumentException();
				}
				xmlLinkedNode2.NextLinkedSibling = xmlLinkedNode.NextLinkedSibling;
				if (xmlLinkedNode.NextLinkedSibling == xmlLinkedNode3)
				{
					hasXmlChildNode.LastLinkedChild = xmlLinkedNode2;
				}
				xmlLinkedNode.NextLinkedSibling = null;
			}
			if (checkNodeType)
			{
				xmlDocument.onNodeRemoved(oldChild, oldChild.ParentNode);
			}
			oldChild.parentNode = null;
			return oldChild;
		}

		/// <summary>Replaces the child node <paramref name="oldChild" /> with <paramref name="newChild" /> node.</summary>
		/// <returns>The node replaced.</returns>
		/// <param name="newChild">The new node to put in the child list. </param>
		/// <param name="oldChild">The node being replaced in the list. </param>
		/// <exception cref="T:System.InvalidOperationException">This node is of a type that does not allow child nodes of the type of the <paramref name="newChild" /> node.The <paramref name="newChild" /> is an ancestor of this node. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newChild" /> was created from a different document than the one that created this node.This node is read-only.The <paramref name="oldChild" /> is not a child of this node. </exception>
		public virtual XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
		{
			if (oldChild.ParentNode != this)
			{
				throw new ArgumentException("The node to be removed is not a child of this node.");
			}
			if (newChild == this || this.IsAncestor(newChild))
			{
				throw new InvalidOperationException("Cannot insert a node or any ancestor of that node as a child of itself.");
			}
			XmlNode nextSibling = oldChild.NextSibling;
			this.RemoveChild(oldChild);
			this.InsertBefore(newChild, nextSibling);
			return oldChild;
		}

		internal XmlElement AttributeOwnerElement
		{
			get
			{
				return (XmlElement)this.parentNode;
			}
			set
			{
				this.parentNode = value;
			}
		}

		internal void SearchDescendantElements(string name, bool matchAll, ArrayList list)
		{
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (matchAll || xmlNode.Name == name)
					{
						list.Add(xmlNode);
					}
					xmlNode.SearchDescendantElements(name, matchAll, list);
				}
			}
		}

		internal void SearchDescendantElements(string name, bool matchAllName, string ns, bool matchAllNS, ArrayList list)
		{
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if ((matchAllName || xmlNode.LocalName == name) && (matchAllNS || xmlNode.NamespaceURI == ns))
					{
						list.Add(xmlNode);
					}
					xmlNode.SearchDescendantElements(name, matchAllName, ns, matchAllNS, list);
				}
			}
		}

		/// <summary>Selects a list of nodes matching the XPath expression.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNodeList" /> containing a collection of nodes matching the XPath query.</returns>
		/// <param name="xpath">The XPath expression. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression contains a prefix. </exception>
		public XmlNodeList SelectNodes(string xpath)
		{
			return this.SelectNodes(xpath, null);
		}

		/// <summary>Selects a list of nodes matching the XPath expression. Any prefixes found in the XPath expression are resolved using the supplied <see cref="T:System.Xml.XmlNamespaceManager" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNodeList" /> containing a collection of nodes matching the XPath query.</returns>
		/// <param name="xpath">The XPath expression. </param>
		/// <param name="nsmgr">An <see cref="T:System.Xml.XmlNamespaceManager" /> to use for resolving namespaces for prefixes in the XPath expression. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression contains a prefix which is not defined in the XmlNamespaceManager. </exception>
		public XmlNodeList SelectNodes(string xpath, XmlNamespaceManager nsmgr)
		{
			XPathNavigator xpathNavigator = this.CreateNavigator();
			XPathExpression xpathExpression = xpathNavigator.Compile(xpath);
			if (nsmgr != null)
			{
				xpathExpression.SetContext(nsmgr);
			}
			XPathNodeIterator iter = xpathNavigator.Select(xpathExpression);
			return new XmlIteratorNodeList(iter);
		}

		/// <summary>Selects the first XmlNode that matches the XPath expression.</summary>
		/// <returns>The first XmlNode that matches the XPath query or null if no matching node is found. The XmlNode should not be expected to be connected "live" to the XML document. That is, changes that appear in the XML document may not appear in the XmlNode, and vice versa.</returns>
		/// <param name="xpath">The XPath expression. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression contains a prefix. </exception>
		public XmlNode SelectSingleNode(string xpath)
		{
			return this.SelectSingleNode(xpath, null);
		}

		/// <summary>Selects the first XmlNode that matches the XPath expression. Any prefixes found in the XPath expression are resolved using the supplied <see cref="T:System.Xml.XmlNamespaceManager" />.</summary>
		/// <returns>The first XmlNode that matches the XPath query or null if no matching node is found. The XmlNode should not be expected to be connected "live" to the XML document. That is, changes that appear in the XML document may not appear in the XmlNode, and vice versa.</returns>
		/// <param name="xpath">The XPath expression. </param>
		/// <param name="nsmgr">An <see cref="T:System.Xml.XmlNamespaceManager" /> to use for resolving namespaces for prefixes in the XPath expression. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression contains a prefix which is not defined in the XmlNamespaceManager. </exception>
		public XmlNode SelectSingleNode(string xpath, XmlNamespaceManager nsmgr)
		{
			XPathNavigator xpathNavigator = this.CreateNavigator();
			XPathExpression xpathExpression = xpathNavigator.Compile(xpath);
			if (nsmgr != null)
			{
				xpathExpression.SetContext(nsmgr);
			}
			XPathNodeIterator xpathNodeIterator = xpathNavigator.Select(xpathExpression);
			if (!xpathNodeIterator.MoveNext())
			{
				return null;
			}
			return ((IHasXmlNode)xpathNodeIterator.Current).GetNode();
		}

		/// <summary>Test if the DOM implementation implements a specific feature.</summary>
		/// <returns>true if the feature is implemented in the specified version; otherwise, false. The following table describes the combinations that return true.Feature Version XML 1.0 XML 2.0 </returns>
		/// <param name="feature">The package name of the feature to test. This name is not case-sensitive. </param>
		/// <param name="version">This is the version number of the package name to test. If the version is not specified (null), supporting any version of the feature causes the method to return true. </param>
		public virtual bool Supports(string feature, string version)
		{
			return string.Compare(feature, "xml", true, CultureInfo.InvariantCulture) == 0 && (string.Compare(version, "1.0", true, CultureInfo.InvariantCulture) == 0 || string.Compare(version, "2.0", true, CultureInfo.InvariantCulture) == 0);
		}

		/// <summary>When overridden in a derived class, saves all the child nodes of the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public abstract void WriteContentTo(XmlWriter w);

		/// <summary>When overridden in a derived class, saves the current node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public abstract void WriteTo(XmlWriter w);

		internal XmlNamespaceManager ConstructNamespaceManager()
		{
			XmlDocument xmlDocument = (!(this is XmlDocument)) ? this.OwnerDocument : ((XmlDocument)this);
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			XmlNodeType nodeType = this.NodeType;
			XmlElement xmlElement;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					xmlElement = (this.ParentNode as XmlElement);
				}
				else
				{
					xmlElement = ((XmlAttribute)this).OwnerElement;
				}
			}
			else
			{
				xmlElement = (this as XmlElement);
			}
			while (xmlElement != null)
			{
				for (int i = 0; i < xmlElement.Attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = xmlElement.Attributes[i];
					if (xmlAttribute.Prefix == "xmlns")
					{
						if (xmlNamespaceManager.LookupNamespace(xmlAttribute.LocalName) != xmlAttribute.Value)
						{
							xmlNamespaceManager.AddNamespace(xmlAttribute.LocalName, xmlAttribute.Value);
						}
					}
					else if (xmlAttribute.Name == "xmlns" && xmlNamespaceManager.LookupNamespace(string.Empty) != xmlAttribute.Value)
					{
						xmlNamespaceManager.AddNamespace(string.Empty, xmlAttribute.Value);
					}
				}
				xmlElement = (xmlElement.ParentNode as XmlElement);
			}
			return xmlNamespaceManager;
		}

		private class EmptyNodeList : XmlNodeList
		{
			private static IEnumerator emptyEnumerator = new object[0].GetEnumerator();

			public override int Count
			{
				get
				{
					return 0;
				}
			}

			public override IEnumerator GetEnumerator()
			{
				return XmlNode.EmptyNodeList.emptyEnumerator;
			}

			public override XmlNode Item(int index)
			{
				return null;
			}
		}
	}
}
