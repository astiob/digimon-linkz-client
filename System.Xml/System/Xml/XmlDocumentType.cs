using Mono.Xml;
using Mono.Xml2;
using System;
using System.IO;

namespace System.Xml
{
	/// <summary>Represents the document type declaration.</summary>
	public class XmlDocumentType : XmlLinkedNode
	{
		internal XmlNamedNodeMap entities;

		internal XmlNamedNodeMap notations;

		private DTDObjectModel dtd;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlDocumentType" /> class.</summary>
		/// <param name="name">The qualified name; see the <see cref="P:System.Xml.XmlDocumentType.Name" /> property.</param>
		/// <param name="publicId">The public identifier; see the <see cref="P:System.Xml.XmlDocumentType.PublicId" /> property.</param>
		/// <param name="systemId">The system identifier; see the <see cref="P:System.Xml.XmlDocumentType.SystemId" /> property.</param>
		/// <param name="internalSubset">The DTD internal subset; see the <see cref="P:System.Xml.XmlDocumentType.InternalSubset" /> property.</param>
		/// <param name="doc">The parent document.</param>
		protected internal XmlDocumentType(string name, string publicId, string systemId, string internalSubset, XmlDocument doc) : base(doc)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(this.BaseURI, new StringReader(string.Empty), doc.NameTable);
			xmlTextReader.XmlResolver = doc.Resolver;
			xmlTextReader.GenerateDTDObjectModel(name, publicId, systemId, internalSubset);
			this.dtd = xmlTextReader.DTD;
			this.ImportFromDTD();
		}

		internal XmlDocumentType(DTDObjectModel dtd, XmlDocument doc) : base(doc)
		{
			this.dtd = dtd;
			this.ImportFromDTD();
		}

		private void ImportFromDTD()
		{
			this.entities = new XmlNamedNodeMap(this);
			this.notations = new XmlNamedNodeMap(this);
			foreach (DTDNode dtdnode in this.DTD.EntityDecls.Values)
			{
				DTDEntityDeclaration dtdentityDeclaration = (DTDEntityDeclaration)dtdnode;
				XmlNode namedItem = new XmlEntity(dtdentityDeclaration.Name, dtdentityDeclaration.NotationName, dtdentityDeclaration.PublicId, dtdentityDeclaration.SystemId, this.OwnerDocument);
				this.entities.SetNamedItem(namedItem);
			}
			foreach (DTDNode dtdnode2 in this.DTD.NotationDecls.Values)
			{
				DTDNotationDeclaration dtdnotationDeclaration = (DTDNotationDeclaration)dtdnode2;
				XmlNode namedItem2 = new XmlNotation(dtdnotationDeclaration.LocalName, dtdnotationDeclaration.Prefix, dtdnotationDeclaration.PublicId, dtdnotationDeclaration.SystemId, this.OwnerDocument);
				this.notations.SetNamedItem(namedItem2);
			}
		}

		internal DTDObjectModel DTD
		{
			get
			{
				return this.dtd;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Xml.XmlEntity" /> nodes declared in the document type declaration.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNamedNodeMap" /> containing the XmlEntity nodes. The returned XmlNamedNodeMap is read-only.</returns>
		public XmlNamedNodeMap Entities
		{
			get
			{
				return this.entities;
			}
		}

		/// <summary>Gets the value of the document type definition (DTD) internal subset on the DOCTYPE declaration.</summary>
		/// <returns>The DTD internal subset on the DOCTYPE. If there is no DTD internal subset, String.Empty is returned.</returns>
		public string InternalSubset
		{
			get
			{
				return this.dtd.InternalSubset;
			}
		}

		/// <summary>Gets a value indicating whether the node is read-only.</summary>
		/// <returns>true if the node is read-only; otherwise false.Because DocumentType nodes are read-only, this property always returns true.</returns>
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For DocumentType nodes, this property returns the name of the document type.</returns>
		public override string LocalName
		{
			get
			{
				return this.dtd.Name;
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>For DocumentType nodes, this property returns the name of the document type.</returns>
		public override string Name
		{
			get
			{
				return this.dtd.Name;
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>For DocumentType nodes, this value is XmlNodeType.DocumentType.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.DocumentType;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Xml.XmlNotation" /> nodes present in the document type declaration.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNamedNodeMap" /> containing the XmlNotation nodes. The returned XmlNamedNodeMap is read-only.</returns>
		public XmlNamedNodeMap Notations
		{
			get
			{
				return this.notations;
			}
		}

		/// <summary>Gets the value of the public identifier on the DOCTYPE declaration.</summary>
		/// <returns>The public identifier on the DOCTYPE. If there is no public identifier, null is returned.</returns>
		public string PublicId
		{
			get
			{
				return this.dtd.PublicId;
			}
		}

		/// <summary>Gets the value of the system identifier on the DOCTYPE declaration.</summary>
		/// <returns>The system identifier on the DOCTYPE. If there is no system identifier, null is returned.</returns>
		public string SystemId
		{
			get
			{
				return this.dtd.SystemId;
			}
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. For document type nodes, the cloned node always includes the subtree, regardless of the parameter setting. </param>
		public override XmlNode CloneNode(bool deep)
		{
			return new XmlDocumentType(this.dtd, this.OwnerDocument);
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />. For XmlDocumentType nodes, this method has no effect.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			w.WriteDocType(this.Name, this.PublicId, this.SystemId, this.InternalSubset);
		}
	}
}
