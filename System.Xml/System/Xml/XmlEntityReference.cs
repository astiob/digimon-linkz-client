using System;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents an entity reference node.</summary>
	public class XmlEntityReference : XmlLinkedNode, IHasXmlChildNode
	{
		private string entityName;

		private XmlLinkedNode lastLinkedChild;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlEntityReference" /> class.</summary>
		/// <param name="name">The name of the entity reference; see the <see cref="P:System.Xml.XmlEntityReference.Name" /> property.</param>
		/// <param name="doc">The parent XML document.</param>
		protected internal XmlEntityReference(string name, XmlDocument doc) : base(doc)
		{
			XmlConvert.VerifyName(name);
			this.entityName = doc.NameTable.Add(name);
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

		/// <summary>Gets the base Uniform Resource Identifier (URI) of the current node.</summary>
		/// <returns>The location from which the node was loaded.</returns>
		public override string BaseURI
		{
			get
			{
				return base.BaseURI;
			}
		}

		private XmlEntity Entity
		{
			get
			{
				XmlDocumentType documentType = this.OwnerDocument.DocumentType;
				if (documentType == null)
				{
					return null;
				}
				if (documentType.Entities == null)
				{
					return null;
				}
				return documentType.Entities.GetNamedItem(this.Name) as XmlEntity;
			}
		}

		internal override string ChildrenBaseURI
		{
			get
			{
				XmlEntity entity = this.Entity;
				if (entity == null)
				{
					return string.Empty;
				}
				if (entity.SystemId == null || entity.SystemId.Length == 0)
				{
					return entity.BaseURI;
				}
				if (entity.BaseURI == null || entity.BaseURI.Length == 0)
				{
					return entity.SystemId;
				}
				Uri baseUri = null;
				try
				{
					baseUri = new Uri(entity.BaseURI);
				}
				catch (UriFormatException)
				{
				}
				XmlResolver resolver = this.OwnerDocument.Resolver;
				if (resolver != null)
				{
					return resolver.ResolveUri(baseUri, entity.SystemId).ToString();
				}
				return new Uri(baseUri, entity.SystemId).ToString();
			}
		}

		/// <summary>Gets a value indicating whether the node is read-only.</summary>
		/// <returns>true if the node is read-only; otherwise false.Because XmlEntityReference nodes are read-only, this property always returns true.</returns>
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For XmlEntityReference nodes, this property returns the name of the entity referenced.</returns>
		public override string LocalName
		{
			get
			{
				return this.entityName;
			}
		}

		/// <summary>Gets the name of the node.</summary>
		/// <returns>The name of the entity referenced.</returns>
		public override string Name
		{
			get
			{
				return this.entityName;
			}
		}

		/// <summary>Gets the type of the node.</summary>
		/// <returns>The node type. For XmlEntityReference nodes, the value is XmlNodeType.EntityReference.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.EntityReference;
			}
		}

		/// <summary>Gets or sets the value of the node.</summary>
		/// <returns>The value of the node. For XmlEntityReference nodes, this property returns null.</returns>
		/// <exception cref="T:System.ArgumentException">Node is read-only. </exception>
		/// <exception cref="T:System.InvalidOperationException">Setting the property. </exception>
		public override string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new XmlException("entity reference cannot be set value.");
			}
		}

		internal override XPathNodeType XPathNodeType
		{
			get
			{
				return XPathNodeType.Text;
			}
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. For XmlEntityReference nodes, this method always returns an entity reference node with no children. The replacement text is set when the node is inserted into a parent. </param>
		public override XmlNode CloneNode(bool deep)
		{
			return new XmlEntityReference(this.Name, this.OwnerDocument);
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
			for (int i = 0; i < this.ChildNodes.Count; i++)
			{
				this.ChildNodes[i].WriteTo(w);
			}
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			w.WriteRaw("&");
			w.WriteName(this.Name);
			w.WriteRaw(";");
		}

		internal void SetReferencedEntityContent()
		{
			if (this.FirstChild != null)
			{
				return;
			}
			if (this.OwnerDocument.DocumentType == null)
			{
				return;
			}
			XmlEntity entity = this.Entity;
			if (entity == null)
			{
				base.InsertBefore(this.OwnerDocument.CreateTextNode(string.Empty), null, false, true);
			}
			else
			{
				for (int i = 0; i < entity.ChildNodes.Count; i++)
				{
					base.InsertBefore(entity.ChildNodes[i].CloneNode(true), null, false, true);
				}
			}
		}
	}
}
