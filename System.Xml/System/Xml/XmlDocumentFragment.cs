using System;
using System.Text;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents a lightweight object that is useful for tree insert operations.</summary>
	public class XmlDocumentFragment : XmlNode, IHasXmlChildNode
	{
		private XmlLinkedNode lastLinkedChild;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlDocumentFragment" /> class.</summary>
		/// <param name="ownerDocument">The XML document that is the source of the fragment.</param>
		protected internal XmlDocumentFragment(XmlDocument doc) : base(doc)
		{
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

		/// <summary>Gets or sets the markup representing the children of this node.</summary>
		/// <returns>The markup of the children of this node.</returns>
		/// <exception cref="T:System.Xml.XmlException">The XML specified when setting this property is not well-formed. </exception>
		public override string InnerXml
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.ChildNodes.Count; i++)
				{
					stringBuilder.Append(this.ChildNodes[i].OuterXml);
				}
				return stringBuilder.ToString();
			}
			set
			{
				for (int i = 0; i < this.ChildNodes.Count; i++)
				{
					this.RemoveChild(this.ChildNodes[i]);
				}
				XmlNamespaceManager nsMgr = base.ConstructNamespaceManager();
				XmlParserContext context = new XmlParserContext(this.OwnerDocument.NameTable, nsMgr, (this.OwnerDocument.DocumentType == null) ? null : this.OwnerDocument.DocumentType.DTD, this.BaseURI, this.XmlLang, this.XmlSpace, null);
				XmlTextReader xmlTextReader = new XmlTextReader(value, XmlNodeType.Element, context);
				xmlTextReader.XmlResolver = this.OwnerDocument.Resolver;
				for (;;)
				{
					XmlNode xmlNode = this.OwnerDocument.ReadNode(xmlTextReader);
					if (xmlNode == null)
					{
						break;
					}
					this.AppendChild(xmlNode);
				}
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For XmlDocumentFragment nodes, the local name is #document-fragment.</returns>
		public override string LocalName
		{
			get
			{
				return "#document-fragment";
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>For XmlDocumentFragment, the name is #document-fragment.</returns>
		public override string Name
		{
			get
			{
				return "#document-fragment";
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>For XmlDocumentFragment nodes, this value is XmlNodeType.DocumentFragment.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.DocumentFragment;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlDocument" /> to which this node belongs.</summary>
		/// <returns>The XmlDocument to which this node belongs.</returns>
		public override XmlDocument OwnerDocument
		{
			get
			{
				return base.OwnerDocument;
			}
		}

		/// <summary>Gets the parent of this node (for nodes that can have parents).</summary>
		/// <returns>The parent of this node.For XmlDocumentFragment nodes, this property is always null.</returns>
		public override XmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		internal override XPathNodeType XPathNodeType
		{
			get
			{
				return XPathNodeType.Root;
			}
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. </param>
		public override XmlNode CloneNode(bool deep)
		{
			if (deep)
			{
				XmlNode xmlNode = this.FirstChild;
				while (xmlNode != null && xmlNode.HasChildNodes)
				{
					this.AppendChild(xmlNode.NextSibling.CloneNode(false));
					xmlNode = xmlNode.NextSibling;
				}
				return xmlNode;
			}
			return new XmlDocumentFragment(this.OwnerDocument);
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
			for (int i = 0; i < this.ChildNodes.Count; i++)
			{
				this.ChildNodes[i].WriteContentTo(w);
			}
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			for (int i = 0; i < this.ChildNodes.Count; i++)
			{
				this.ChildNodes[i].WriteTo(w);
			}
		}
	}
}
