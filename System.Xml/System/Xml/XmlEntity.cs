using Mono.Xml;
using System;

namespace System.Xml
{
	/// <summary>Represents an entity declaration, such as &lt;!ENTITY... &gt;.</summary>
	public class XmlEntity : XmlNode, IHasXmlChildNode
	{
		private string name;

		private string NDATA;

		private string publicId;

		private string systemId;

		private string baseUri;

		private XmlLinkedNode lastLinkedChild;

		private bool contentAlreadySet;

		internal XmlEntity(string name, string NDATA, string publicId, string systemId, XmlDocument doc) : base(doc)
		{
			this.name = doc.NameTable.Add(name);
			this.NDATA = NDATA;
			this.publicId = publicId;
			this.systemId = systemId;
			this.baseUri = doc.BaseURI;
		}

		XmlLinkedNode IHasXmlChildNode.LastLinkedChild
		{
			get
			{
				if (this.lastLinkedChild != null)
				{
					return this.lastLinkedChild;
				}
				if (!this.contentAlreadySet)
				{
					this.contentAlreadySet = true;
					this.SetEntityContent();
				}
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
				return this.baseUri;
			}
		}

		/// <summary>Gets the concatenated values of the entity node and all its children.</summary>
		/// <returns>The concatenated values of the node and all its children.</returns>
		/// <exception cref="T:System.InvalidOperationException">Attempting to set the property. </exception>
		public override string InnerText
		{
			get
			{
				return base.InnerText;
			}
			set
			{
				throw new InvalidOperationException("This operation is not supported.");
			}
		}

		/// <summary>Gets the markup representing the children of this node.</summary>
		/// <returns>For XmlEntity nodes, String.Empty is returned.</returns>
		/// <exception cref="T:System.InvalidOperationException">Attempting to set the property. </exception>
		public override string InnerXml
		{
			get
			{
				return base.InnerXml;
			}
			set
			{
				throw new InvalidOperationException("This operation is not supported.");
			}
		}

		/// <summary>Gets a value indicating whether the node is read-only.</summary>
		/// <returns>true if the node is read-only; otherwise false.Because XmlEntity nodes are read-only, this property always returns true.</returns>
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the name of the node without the namespace prefix.</summary>
		/// <returns>For XmlEntity nodes, this property returns the name of the entity.</returns>
		public override string LocalName
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets the name of the node.</summary>
		/// <returns>The name of the entity.</returns>
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets the type of the node.</summary>
		/// <returns>The node type. For XmlEntity nodes, the value is XmlNodeType.Entity.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Entity;
			}
		}

		/// <summary>Gets the name of the optional NDATA attribute on the entity declaration.</summary>
		/// <returns>The name of the NDATA attribute. If there is no NDATA, null is returned.</returns>
		public string NotationName
		{
			get
			{
				if (this.NDATA == null)
				{
					return null;
				}
				return this.NDATA;
			}
		}

		/// <summary>Gets the markup representing this node and all its children.</summary>
		/// <returns>For XmlEntity nodes, String.Empty is returned.</returns>
		public override string OuterXml
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>Gets the value of the public identifier on the entity declaration.</summary>
		/// <returns>The public identifier on the entity. If there is no public identifier, null is returned.</returns>
		public string PublicId
		{
			get
			{
				return this.publicId;
			}
		}

		/// <summary>Gets the value of the system identifier on the entity declaration.</summary>
		/// <returns>The system identifier on the entity. If there is no system identifier, null is returned.</returns>
		public string SystemId
		{
			get
			{
				return this.systemId;
			}
		}

		/// <summary>Creates a duplicate of this node. Entity nodes cannot be cloned. Calling this method on an <see cref="T:System.Xml.XmlEntity" /> object throws an exception.</summary>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
		/// <exception cref="T:System.InvalidOperationException">Entity nodes cannot be cloned. Calling this method on an <see cref="T:System.Xml.XmlEntity" /> object throws an exception.</exception>
		public override XmlNode CloneNode(bool deep)
		{
			throw new InvalidOperationException("This operation is not supported.");
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />. For XmlEntity nodes, this method has no effect.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />. For XmlEntity nodes, this method has no effect.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
		}

		private void SetEntityContent()
		{
			if (this.lastLinkedChild != null)
			{
				return;
			}
			XmlDocumentType documentType = this.OwnerDocument.DocumentType;
			if (documentType == null)
			{
				return;
			}
			DTDEntityDeclaration dtdentityDeclaration = documentType.DTD.EntityDecls[this.name];
			if (dtdentityDeclaration == null)
			{
				return;
			}
			XmlNamespaceManager nsMgr = base.ConstructNamespaceManager();
			XmlParserContext context = new XmlParserContext(this.OwnerDocument.NameTable, nsMgr, (documentType == null) ? null : documentType.DTD, this.BaseURI, this.XmlLang, this.XmlSpace, null);
			XmlTextReader xmlTextReader = new XmlTextReader(dtdentityDeclaration.EntityValue, XmlNodeType.Element, context);
			xmlTextReader.XmlResolver = this.OwnerDocument.Resolver;
			for (;;)
			{
				XmlNode xmlNode = this.OwnerDocument.ReadNode(xmlTextReader);
				if (xmlNode == null)
				{
					break;
				}
				base.InsertBefore(xmlNode, null, false, false);
			}
		}
	}
}
