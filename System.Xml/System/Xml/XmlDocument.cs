using Mono.Xml;
using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents an XML document.</summary>
	public class XmlDocument : XmlNode, IHasXmlChildNode
	{
		private static readonly Type[] optimal_create_types = new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		};

		private bool optimal_create_element;

		private bool optimal_create_attribute;

		private XmlNameTable nameTable;

		private string baseURI = string.Empty;

		private XmlImplementation implementation;

		private bool preserveWhitespace;

		private XmlResolver resolver;

		private Hashtable idTable = new Hashtable();

		private XmlNameEntryCache nameCache;

		private XmlLinkedNode lastLinkedChild;

		private XmlAttribute nsNodeXml;

		private XmlSchemaSet schemas;

		private IXmlSchemaInfo schemaInfo;

		private bool loadMode;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlDocument" /> class.</summary>
		public XmlDocument() : this(null, null)
		{
		}

		/// <summary>Initializes a new instance of the XmlDocument class with the specified <see cref="T:System.Xml.XmlImplementation" />.</summary>
		/// <param name="imp">The XmlImplementation to use. </param>
		protected internal XmlDocument(XmlImplementation imp) : this(imp, null)
		{
		}

		/// <summary>Initializes a new instance of the XmlDocument class with the specified <see cref="T:System.Xml.XmlNameTable" />.</summary>
		/// <param name="nt">The XmlNameTable to use. </param>
		public XmlDocument(XmlNameTable nt) : this(null, nt)
		{
		}

		private XmlDocument(XmlImplementation impl, XmlNameTable nt) : base(null)
		{
			if (impl == null)
			{
				this.implementation = new XmlImplementation();
			}
			else
			{
				this.implementation = impl;
			}
			this.nameTable = ((nt == null) ? this.implementation.InternalNameTable : nt);
			this.nameCache = new XmlNameEntryCache(this.nameTable);
			this.AddDefaultNameTableKeys();
			this.resolver = new XmlUrlResolver();
			Type type = base.GetType();
			this.optimal_create_element = (type.GetMethod("CreateElement", XmlDocument.optimal_create_types).DeclaringType == typeof(XmlDocument));
			this.optimal_create_attribute = (type.GetMethod("CreateAttribute", XmlDocument.optimal_create_types).DeclaringType == typeof(XmlDocument));
		}

		/// <summary>Occurs when the <see cref="P:System.Xml.XmlNode.Value" /> of a node belonging to this document has been changed.</summary>
		public event XmlNodeChangedEventHandler NodeChanged;

		/// <summary>Occurs when the <see cref="P:System.Xml.XmlNode.Value" /> of a node belonging to this document is about to be changed.</summary>
		public event XmlNodeChangedEventHandler NodeChanging;

		/// <summary>Occurs when a node belonging to this document has been inserted into another node.</summary>
		public event XmlNodeChangedEventHandler NodeInserted;

		/// <summary>Occurs when a node belonging to this document is about to be inserted into another node.</summary>
		public event XmlNodeChangedEventHandler NodeInserting;

		/// <summary>Occurs when a node belonging to this document has been removed from its parent.</summary>
		public event XmlNodeChangedEventHandler NodeRemoved;

		/// <summary>Occurs when a node belonging to this document is about to be removed from the document.</summary>
		public event XmlNodeChangedEventHandler NodeRemoving;

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

		internal XmlAttribute NsNodeXml
		{
			get
			{
				if (this.nsNodeXml == null)
				{
					this.nsNodeXml = this.CreateAttribute("xmlns", "xml", "http://www.w3.org/2000/xmlns/");
					this.nsNodeXml.Value = "http://www.w3.org/XML/1998/namespace";
				}
				return this.nsNodeXml;
			}
		}

		/// <summary>Gets the base URI of the current node.</summary>
		/// <returns>The location from which the node was loaded.</returns>
		public override string BaseURI
		{
			get
			{
				return this.baseURI;
			}
		}

		/// <summary>Gets the root <see cref="T:System.Xml.XmlElement" /> for the document.</summary>
		/// <returns>The XmlElement that represents the root of the XML document tree. If no root exists, null is returned.</returns>
		public XmlElement DocumentElement
		{
			get
			{
				XmlNode xmlNode;
				for (xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					if (xmlNode is XmlElement)
					{
						break;
					}
				}
				return (xmlNode == null) ? null : (xmlNode as XmlElement);
			}
		}

		/// <summary>Gets the node containing the DOCTYPE declaration.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> containing the DocumentType (DOCTYPE declaration).</returns>
		public virtual XmlDocumentType DocumentType
		{
			get
			{
				for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					if (xmlNode.NodeType == XmlNodeType.DocumentType)
					{
						return (XmlDocumentType)xmlNode;
					}
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						return null;
					}
				}
				return null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlImplementation" /> object for the current document.</summary>
		/// <returns>The XmlImplementation object for the current document.</returns>
		public XmlImplementation Implementation
		{
			get
			{
				return this.implementation;
			}
		}

		/// <summary>Gets or sets the markup representing the children of the current node.</summary>
		/// <returns>The markup of the children of the current node.</returns>
		/// <exception cref="T:System.Xml.XmlException">The XML specified when setting this property is not well-formed. </exception>
		public override string InnerXml
		{
			get
			{
				return base.InnerXml;
			}
			set
			{
				this.LoadXml(value);
			}
		}

		/// <summary>Gets a value indicating whether the current node is read-only.</summary>
		/// <returns>true if the current node is read-only; otherwise false. XmlDocument nodes always return false.</returns>
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		internal bool IsStandalone
		{
			get
			{
				return this.FirstChild != null && this.FirstChild.NodeType == XmlNodeType.XmlDeclaration && ((XmlDeclaration)this.FirstChild).Standalone == "yes";
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For XmlDocument nodes, the local name is #document.</returns>
		public override string LocalName
		{
			get
			{
				return "#document";
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>For XmlDocument nodes, the name is #document.</returns>
		public override string Name
		{
			get
			{
				return "#document";
			}
		}

		internal XmlNameEntryCache NameCache
		{
			get
			{
				return this.nameCache;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlNameTable" /> associated with this implementation.</summary>
		/// <returns>An XmlNameTable enabling you to get the atomized version of a string within the document.</returns>
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>The node type. For XmlDocument nodes, this value is XmlNodeType.Document.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Document;
			}
		}

		internal override XPathNodeType XPathNodeType
		{
			get
			{
				return XPathNodeType.Root;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlDocument" /> to which the current node belongs.</summary>
		/// <returns>For XmlDocument nodes (<see cref="P:System.Xml.XmlDocument.NodeType" /> equals XmlNodeType.Document), this property always returns null.</returns>
		public override XmlDocument OwnerDocument
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets a value indicating whether to preserve white space in element content.</summary>
		/// <returns>true to preserve white space; otherwise false. The default is false.</returns>
		public bool PreserveWhitespace
		{
			get
			{
				return this.preserveWhitespace;
			}
			set
			{
				this.preserveWhitespace = value;
			}
		}

		internal XmlResolver Resolver
		{
			get
			{
				return this.resolver;
			}
		}

		internal override string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>Sets the <see cref="T:System.Xml.XmlResolver" /> to use for resolving external resources.</summary>
		/// <returns>The XmlResolver to use.In version 1.1 of the.NET Framework, the caller must be fully trusted in order to specify an XmlResolver.</returns>
		/// <exception cref="T:System.Xml.XmlException">This property is set to null and an external DTD or entity is encountered. </exception>
		public virtual XmlResolver XmlResolver
		{
			set
			{
				this.resolver = value;
			}
		}

		internal override XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.None;
			}
		}

		internal Encoding TextEncoding
		{
			get
			{
				XmlDeclaration xmlDeclaration = this.FirstChild as XmlDeclaration;
				if (xmlDeclaration == null || xmlDeclaration.Encoding == string.Empty)
				{
					return null;
				}
				return Encoding.GetEncoding(xmlDeclaration.Encoding);
			}
		}

		/// <summary>Gets the parent node of this node (for nodes that can have parents).</summary>
		/// <returns>Always returns null.</returns>
		public override XmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object associated with this <see cref="T:System.Xml.XmlDocument" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object containing the XML Schema Definition Language (XSD) schemas associated with this <see cref="T:System.Xml.XmlDocument" />; otherwise, an empty <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object.</returns>
		public XmlSchemaSet Schemas
		{
			get
			{
				if (this.schemas == null)
				{
					this.schemas = new XmlSchemaSet();
				}
				return this.schemas;
			}
			set
			{
				this.schemas = value;
			}
		}

		/// <summary>Returns the Post-Schema-Validation-Infoset (PSVI) of the node.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.IXmlSchemaInfo" /> object representing the PSVI of the node.</returns>
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

		internal void AddIdenticalAttribute(XmlAttribute attr)
		{
			this.idTable[attr.Value] = attr;
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned XmlDocument node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. </param>
		public override XmlNode CloneNode(bool deep)
		{
			XmlDocument xmlDocument = (this.implementation == null) ? new XmlDocument() : this.implementation.CreateDocument();
			xmlDocument.baseURI = this.baseURI;
			if (deep)
			{
				for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					xmlDocument.AppendChild(xmlDocument.ImportNode(xmlNode, deep), false);
				}
			}
			return xmlDocument;
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlAttribute" /> with the specified <see cref="P:System.Xml.XmlDocument.Name" />.</summary>
		/// <returns>The new XmlAttribute.</returns>
		/// <param name="name">The qualified name of the attribute. If the name contains a colon, the <see cref="P:System.Xml.XmlNode.Prefix" /> property reflects the part of the name preceding the first colon and the <see cref="P:System.Xml.XmlDocument.LocalName" /> property reflects the part of the name following the first colon. The <see cref="P:System.Xml.XmlNode.NamespaceURI" /> remains empty unless the prefix is a recognized built-in prefix such as xmlns. In this case NamespaceURI has a value of http://www.w3.org/2000/xmlns/. </param>
		public XmlAttribute CreateAttribute(string name)
		{
			string namespaceURI = string.Empty;
			string text;
			string text2;
			this.ParseName(name, out text, out text2);
			if (text == "xmlns" || (text == string.Empty && text2 == "xmlns"))
			{
				namespaceURI = "http://www.w3.org/2000/xmlns/";
			}
			else if (text == "xml")
			{
				namespaceURI = "http://www.w3.org/XML/1998/namespace";
			}
			return this.CreateAttribute(text, text2, namespaceURI);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlAttribute" /> with the specified qualified name and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new XmlAttribute.</returns>
		/// <param name="qualifiedName">The qualified name of the attribute. If the name contains a colon then the <see cref="P:System.Xml.XmlNode.Prefix" /> property will reflect the part of the name preceding the colon and the <see cref="P:System.Xml.XmlDocument.LocalName" /> property will reflect the part of the name after the colon. </param>
		/// <param name="namespaceURI">The namespaceURI of the attribute. If the qualified name includes a prefix of xmlns, then this parameter must be http://www.w3.org/2000/xmlns/. </param>
		public XmlAttribute CreateAttribute(string qualifiedName, string namespaceURI)
		{
			string prefix;
			string localName;
			this.ParseName(qualifiedName, out prefix, out localName);
			return this.CreateAttribute(prefix, localName, namespaceURI);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlAttribute" /> with the specified <see cref="P:System.Xml.XmlNode.Prefix" />, <see cref="P:System.Xml.XmlDocument.LocalName" />, and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new XmlAttribute.</returns>
		/// <param name="prefix">The prefix of the attribute (if any). String.Empty and null are equivalent. </param>
		/// <param name="localName">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute (if any). String.Empty and null are equivalent. If <paramref name="prefix" /> is xmlns, then this parameter must be http://www.w3.org/2000/xmlns/; otherwise an exception is thrown. </param>
		public virtual XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			if (localName == null || localName == string.Empty)
			{
				throw new ArgumentException("The attribute local name cannot be empty.");
			}
			return new XmlAttribute(prefix, localName, namespaceURI, this, false, true);
		}

		internal XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI, bool atomizedNames, bool checkNamespace)
		{
			if (this.optimal_create_attribute)
			{
				return new XmlAttribute(prefix, localName, namespaceURI, this, atomizedNames, checkNamespace);
			}
			return this.CreateAttribute(prefix, localName, namespaceURI);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlCDataSection" /> containing the specified data.</summary>
		/// <returns>The new XmlCDataSection.</returns>
		/// <param name="data">The content of the new XmlCDataSection. </param>
		public virtual XmlCDataSection CreateCDataSection(string data)
		{
			return new XmlCDataSection(data, this);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlComment" /> containing the specified data.</summary>
		/// <returns>The new XmlComment.</returns>
		/// <param name="data">The content of the new XmlComment. </param>
		public virtual XmlComment CreateComment(string data)
		{
			return new XmlComment(data, this);
		}

		/// <summary>Creates a default attribute with the specified prefix, local name and namespace URI.</summary>
		/// <returns>The new <see cref="T:System.Xml.XmlAttribute" />.</returns>
		/// <param name="prefix">The prefix of the attribute (if any). </param>
		/// <param name="localName">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute (if any). </param>
		protected internal virtual XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			XmlAttribute xmlAttribute = this.CreateAttribute(prefix, localName, namespaceURI);
			xmlAttribute.isDefault = true;
			return xmlAttribute;
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlDocumentFragment" />.</summary>
		/// <returns>The new XmlDocumentFragment.</returns>
		public virtual XmlDocumentFragment CreateDocumentFragment()
		{
			return new XmlDocumentFragment(this);
		}

		/// <summary>Returns a new <see cref="T:System.Xml.XmlDocumentType" /> object.</summary>
		/// <returns>The new XmlDocumentType.</returns>
		/// <param name="name">Name of the document type. </param>
		/// <param name="publicId">The public identifier of the document type or null. You can specify a public URI and also a system identifier to identify the location of the external DTD subset.</param>
		/// <param name="systemId">The system identifier of the document type or null. Specifies the URL of the file location for the external DTD subset.</param>
		/// <param name="internalSubset">The DTD internal subset of the document type or null. </param>
		public virtual XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentType(name, publicId, systemId, internalSubset, this);
		}

		private XmlDocumentType CreateDocumentType(DTDObjectModel dtd)
		{
			return new XmlDocumentType(dtd, this);
		}

		/// <summary>Creates an element with the specified name.</summary>
		/// <returns>The new XmlElement.</returns>
		/// <param name="name">The qualified name of the element. If the name contains a colon then the <see cref="P:System.Xml.XmlNode.Prefix" /> property reflects the part of the name preceding the colon and the <see cref="P:System.Xml.XmlDocument.LocalName" /> property reflects the part of the name after the colon. The qualified name cannot include a prefix of'xmlns'. </param>
		public XmlElement CreateElement(string name)
		{
			return this.CreateElement(name, string.Empty);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlElement" /> with the qualified name and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new XmlElement.</returns>
		/// <param name="qualifiedName">The qualified name of the element. If the name contains a colon then the <see cref="P:System.Xml.XmlNode.Prefix" /> property will reflect the part of the name preceding the colon and the <see cref="P:System.Xml.XmlDocument.LocalName" /> property will reflect the part of the name after the colon. The qualified name cannot include a prefix of'xmlns'. </param>
		/// <param name="namespaceURI">The namespace URI of the element. </param>
		public XmlElement CreateElement(string qualifiedName, string namespaceURI)
		{
			string prefix;
			string localName;
			this.ParseName(qualifiedName, out prefix, out localName);
			return this.CreateElement(prefix, localName, namespaceURI);
		}

		/// <summary>Creates an element with the specified <see cref="P:System.Xml.XmlNode.Prefix" />, <see cref="P:System.Xml.XmlDocument.LocalName" />, and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new <see cref="T:System.Xml.XmlElement" />.</returns>
		/// <param name="prefix">The prefix of the new element (if any). String.Empty and null are equivalent. </param>
		/// <param name="localName">The local name of the new element. </param>
		/// <param name="namespaceURI">The namespace URI of the new element (if any). String.Empty and null are equivalent. </param>
		public virtual XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			return new XmlElement((prefix == null) ? string.Empty : prefix, localName, (namespaceURI == null) ? string.Empty : namespaceURI, this, false);
		}

		internal XmlElement CreateElement(string prefix, string localName, string namespaceURI, bool nameAtomized)
		{
			if (localName == null || localName == string.Empty)
			{
				throw new ArgumentException("The local name for elements or attributes cannot be null or an empty string.");
			}
			if (this.optimal_create_element)
			{
				return new XmlElement((prefix == null) ? string.Empty : prefix, localName, (namespaceURI == null) ? string.Empty : namespaceURI, this, nameAtomized);
			}
			return this.CreateElement(prefix, localName, namespaceURI);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlEntityReference" /> with the specified name.</summary>
		/// <returns>The new XmlEntityReference.</returns>
		/// <param name="name">The name of the entity reference. </param>
		/// <exception cref="T:System.ArgumentException">The name is invalid (for example, names starting with'#' are invalid.) </exception>
		public virtual XmlEntityReference CreateEntityReference(string name)
		{
			return new XmlEntityReference(name, this);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XPath.XPathNavigator" /> object for navigating this document.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object.</returns>
		public override XPathNavigator CreateNavigator()
		{
			return this.CreateNavigator(this);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XPath.XPathNavigator" /> object for navigating this document positioned on the <see cref="T:System.Xml.XmlNode" /> specified.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object.</returns>
		/// <param name="node">The <see cref="T:System.Xml.XmlNode" /> you want the navigator initially positioned on. </param>
		protected internal virtual XPathNavigator CreateNavigator(XmlNode node)
		{
			return new XPathEditableDocument(node).CreateNavigator();
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlNode" /> with the specified node type, <see cref="P:System.Xml.XmlDocument.Name" />, and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new XmlNode.</returns>
		/// <param name="nodeTypeString">String version of the <see cref="T:System.Xml.XmlNodeType" /> of the new node. This parameter must be one of the values listed in the table below. </param>
		/// <param name="name">The qualified name of the new node. If the name contains a colon, it is parsed into <see cref="P:System.Xml.XmlNode.Prefix" /> and <see cref="P:System.Xml.XmlDocument.LocalName" /> components. </param>
		/// <param name="namespaceURI">The namespace URI of the new node. </param>
		/// <exception cref="T:System.ArgumentException">The name was not provided and the XmlNodeType requires a name; or <paramref name="nodeTypeString" /> is not one of the strings listed below. </exception>
		public virtual XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
		{
			return this.CreateNode(this.GetNodeTypeFromString(nodeTypeString), name, namespaceURI);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlNode" /> with the specified <see cref="T:System.Xml.XmlNodeType" />, <see cref="P:System.Xml.XmlDocument.Name" />, and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new XmlNode.</returns>
		/// <param name="type">The XmlNodeType of the new node. </param>
		/// <param name="name">The qualified name of the new node. If the name contains a colon then it is parsed into <see cref="P:System.Xml.XmlNode.Prefix" /> and <see cref="P:System.Xml.XmlDocument.LocalName" /> components. </param>
		/// <param name="namespaceURI">The namespace URI of the new node. </param>
		/// <exception cref="T:System.ArgumentException">The name was not provided and the XmlNodeType requires a name. </exception>
		public virtual XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
		{
			string prefix = null;
			string name2 = name;
			if (type == XmlNodeType.Attribute || type == XmlNodeType.Element || type == XmlNodeType.EntityReference)
			{
				this.ParseName(name, out prefix, out name2);
			}
			return this.CreateNode(type, prefix, name2, namespaceURI);
		}

		/// <summary>Creates a <see cref="T:System.Xml.XmlNode" /> with the specified <see cref="T:System.Xml.XmlNodeType" />, <see cref="P:System.Xml.XmlNode.Prefix" />, <see cref="P:System.Xml.XmlDocument.Name" />, and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The new XmlNode.</returns>
		/// <param name="type">The XmlNodeType of the new node. </param>
		/// <param name="prefix">The prefix of the new node. </param>
		/// <param name="name">The local name of the new node. </param>
		/// <param name="namespaceURI">The namespace URI of the new node. </param>
		/// <exception cref="T:System.ArgumentException">The name was not provided and the XmlNodeType requires a name. </exception>
		public virtual XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
		{
			switch (type)
			{
			case XmlNodeType.Element:
				return this.CreateElement(prefix, name, namespaceURI);
			case XmlNodeType.Attribute:
				return this.CreateAttribute(prefix, name, namespaceURI);
			case XmlNodeType.Text:
				return this.CreateTextNode(null);
			case XmlNodeType.CDATA:
				return this.CreateCDataSection(null);
			case XmlNodeType.EntityReference:
				return this.CreateEntityReference(null);
			case XmlNodeType.ProcessingInstruction:
				return this.CreateProcessingInstruction(null, null);
			case XmlNodeType.Comment:
				return this.CreateComment(null);
			case XmlNodeType.Document:
				return new XmlDocument();
			case XmlNodeType.DocumentType:
				return this.CreateDocumentType(null, null, null, null);
			case XmlNodeType.DocumentFragment:
				return this.CreateDocumentFragment();
			case XmlNodeType.Whitespace:
				return this.CreateWhitespace(string.Empty);
			case XmlNodeType.SignificantWhitespace:
				return this.CreateSignificantWhitespace(string.Empty);
			case XmlNodeType.XmlDeclaration:
				return this.CreateXmlDeclaration("1.0", null, null);
			}
			throw new ArgumentException(string.Format("{0}\nParameter name: {1}", "Specified argument was out of the range of valid values", type.ToString()));
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlProcessingInstruction" /> with the specified name and data.</summary>
		/// <returns>The new XmlProcessingInstruction.</returns>
		/// <param name="target">The name of the processing instruction. </param>
		/// <param name="data">The data for the processing instruction. </param>
		public virtual XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new XmlProcessingInstruction(target, data, this);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlSignificantWhitespace" /> node.</summary>
		/// <returns>A new XmlSignificantWhitespace node.</returns>
		/// <param name="text">The string must contain only the following characters &amp;#20; &amp;#10; &amp;#13; and &amp;#9; </param>
		public virtual XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			if (!XmlChar.IsWhitespace(text))
			{
				throw new ArgumentException("Invalid whitespace characters.");
			}
			return new XmlSignificantWhitespace(text, this);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlText" /> with the specified text.</summary>
		/// <returns>The new XmlText node.</returns>
		/// <param name="text">The text for the Text node. </param>
		public virtual XmlText CreateTextNode(string text)
		{
			return new XmlText(text, this);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlWhitespace" /> node.</summary>
		/// <returns>A new XmlWhitespace node.</returns>
		/// <param name="text">The string must contain only the following characters &amp;#20; &amp;#10; &amp;#13; and &amp;#9; </param>
		public virtual XmlWhitespace CreateWhitespace(string text)
		{
			if (!XmlChar.IsWhitespace(text))
			{
				throw new ArgumentException("Invalid whitespace characters.");
			}
			return new XmlWhitespace(text, this);
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlDeclaration" /> node with the specified values.</summary>
		/// <returns>The new XmlDeclaration node.</returns>
		/// <param name="version">The version must be "1.0". </param>
		/// <param name="encoding">The value of the encoding attribute. This is the encoding that is used when you save the <see cref="T:System.Xml.XmlDocument" /> to a file or a stream; therefore, it must be set to a string supported by the <see cref="T:System.Text.Encoding" /> class, otherwise <see cref="M:System.Xml.XmlDocument.Save(System.String)" /> fails. If this is null or String.Empty, the Save method does not write an encoding attribute on the XML declaration and therefore the default encoding, UTF-8, is used.Note: If the XmlDocument is saved to either a <see cref="T:System.IO.TextWriter" /> or an <see cref="T:System.Xml.XmlTextWriter" />, this encoding value is discarded. Instead, the encoding of the TextWriter or the XmlTextWriter is used. This ensures that the XML written out can be read back using the correct encoding. </param>
		/// <param name="standalone">The value must be either "yes" or "no". If this is null or String.Empty, the Save method does not write a standalone attribute on the XML declaration. </param>
		/// <exception cref="T:System.ArgumentException">The values of <paramref name="version" /> or <paramref name="standalone" /> are something other than the ones specified above. </exception>
		public virtual XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			if (version != "1.0")
			{
				throw new ArgumentException("version string is not correct.");
			}
			if (standalone != null && standalone != string.Empty && !(standalone == "yes") && !(standalone == "no"))
			{
				throw new ArgumentException("standalone string is not correct.");
			}
			return new XmlDeclaration(version, encoding, standalone, this);
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlElement" /> with the specified ID.</summary>
		/// <returns>The XmlElement with the matching ID or null if no matching element is found.</returns>
		/// <param name="elementId">The attribute ID to match. </param>
		public virtual XmlElement GetElementById(string elementId)
		{
			XmlAttribute identicalAttribute = this.GetIdenticalAttribute(elementId);
			return (identicalAttribute == null) ? null : identicalAttribute.OwnerElement;
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlNodeList" /> containing a list of all descendant elements that match the specified <see cref="P:System.Xml.XmlDocument.Name" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNodeList" /> containing a list of all matching nodes. If no nodes match <paramref name="name" />, the returned collection will be empty.</returns>
		/// <param name="name">The qualified name to match. It is matched against the Name property of the matching node. The special value "*" matches all tags. </param>
		public virtual XmlNodeList GetElementsByTagName(string name)
		{
			ArrayList arrayList = new ArrayList();
			base.SearchDescendantElements(name, name == "*", arrayList);
			return new XmlNodeArrayList(arrayList);
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlNodeList" /> containing a list of all descendant elements that match the specified <see cref="P:System.Xml.XmlDocument.LocalName" /> and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNodeList" /> containing a list of all matching nodes. If no nodes match the specified <paramref name="localName" /> and <paramref name="namespaceURI" />, the returned collection will be empty.</returns>
		/// <param name="localName">The LocalName to match. The special value "*" matches all tags. </param>
		/// <param name="namespaceURI">NamespaceURI to match. </param>
		public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
		{
			ArrayList arrayList = new ArrayList();
			base.SearchDescendantElements(localName, localName == "*", namespaceURI, namespaceURI == "*", arrayList);
			return new XmlNodeArrayList(arrayList);
		}

		private XmlNodeType GetNodeTypeFromString(string nodeTypeString)
		{
			if (nodeTypeString == null)
			{
				throw new ArgumentNullException("nodeTypeString");
			}
			switch (nodeTypeString)
			{
			case "attribute":
				return XmlNodeType.Attribute;
			case "cdatasection":
				return XmlNodeType.CDATA;
			case "comment":
				return XmlNodeType.Comment;
			case "document":
				return XmlNodeType.Document;
			case "documentfragment":
				return XmlNodeType.DocumentFragment;
			case "documenttype":
				return XmlNodeType.DocumentType;
			case "element":
				return XmlNodeType.Element;
			case "entityreference":
				return XmlNodeType.EntityReference;
			case "processinginstruction":
				return XmlNodeType.ProcessingInstruction;
			case "significantwhitespace":
				return XmlNodeType.SignificantWhitespace;
			case "text":
				return XmlNodeType.Text;
			case "whitespace":
				return XmlNodeType.Whitespace;
			}
			throw new ArgumentException(string.Format("The string doesn't represent any node type : {0}.", nodeTypeString));
		}

		internal XmlAttribute GetIdenticalAttribute(string id)
		{
			XmlAttribute xmlAttribute = this.idTable[id] as XmlAttribute;
			if (xmlAttribute == null)
			{
				return null;
			}
			if (xmlAttribute.OwnerElement == null || !xmlAttribute.OwnerElement.IsRooted)
			{
				return null;
			}
			return xmlAttribute;
		}

		/// <summary>Imports a node from another document to the current document.</summary>
		/// <returns>The imported <see cref="T:System.Xml.XmlNode" />.</returns>
		/// <param name="node">The node being imported. </param>
		/// <param name="deep">true to perform a deep clone; otherwise, false. </param>
		/// <exception cref="T:System.InvalidOperationException">Calling this method on a node type which cannot be imported. </exception>
		public virtual XmlNode ImportNode(XmlNode node, bool deep)
		{
			if (node == null)
			{
				throw new NullReferenceException("Null node cannot be imported.");
			}
			switch (node.NodeType)
			{
			case XmlNodeType.None:
				throw new XmlException("Illegal ImportNode call for NodeType.None");
			case XmlNodeType.Element:
			{
				XmlElement xmlElement = (XmlElement)node;
				XmlElement xmlElement2 = this.CreateElement(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI);
				for (int i = 0; i < xmlElement.Attributes.Count; i++)
				{
					XmlAttribute xmlAttribute = xmlElement.Attributes[i];
					if (xmlAttribute.Specified)
					{
						xmlElement2.SetAttributeNode((XmlAttribute)this.ImportNode(xmlAttribute, deep));
					}
				}
				if (deep)
				{
					for (XmlNode xmlNode = xmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
					{
						xmlElement2.AppendChild(this.ImportNode(xmlNode, deep));
					}
				}
				return xmlElement2;
			}
			case XmlNodeType.Attribute:
			{
				XmlAttribute xmlAttribute2 = node as XmlAttribute;
				XmlAttribute xmlAttribute3 = this.CreateAttribute(xmlAttribute2.Prefix, xmlAttribute2.LocalName, xmlAttribute2.NamespaceURI);
				for (XmlNode xmlNode2 = xmlAttribute2.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
				{
					xmlAttribute3.AppendChild(this.ImportNode(xmlNode2, deep));
				}
				return xmlAttribute3;
			}
			case XmlNodeType.Text:
				return this.CreateTextNode(node.Value);
			case XmlNodeType.CDATA:
				return this.CreateCDataSection(node.Value);
			case XmlNodeType.EntityReference:
				return this.CreateEntityReference(node.Name);
			case XmlNodeType.ProcessingInstruction:
			{
				XmlProcessingInstruction xmlProcessingInstruction = node as XmlProcessingInstruction;
				return this.CreateProcessingInstruction(xmlProcessingInstruction.Target, xmlProcessingInstruction.Data);
			}
			case XmlNodeType.Comment:
				return this.CreateComment(node.Value);
			case XmlNodeType.Document:
				throw new XmlException("Document cannot be imported.");
			case XmlNodeType.DocumentType:
				throw new XmlException("DocumentType cannot be imported.");
			case XmlNodeType.DocumentFragment:
			{
				XmlDocumentFragment xmlDocumentFragment = this.CreateDocumentFragment();
				if (deep)
				{
					for (XmlNode xmlNode3 = node.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
					{
						xmlDocumentFragment.AppendChild(this.ImportNode(xmlNode3, deep));
					}
				}
				return xmlDocumentFragment;
			}
			case XmlNodeType.Whitespace:
				return this.CreateWhitespace(node.Value);
			case XmlNodeType.SignificantWhitespace:
				return this.CreateSignificantWhitespace(node.Value);
			case XmlNodeType.EndElement:
				throw new XmlException("Illegal ImportNode call for NodeType.EndElement");
			case XmlNodeType.EndEntity:
				throw new XmlException("Illegal ImportNode call for NodeType.EndEntity");
			case XmlNodeType.XmlDeclaration:
			{
				XmlDeclaration xmlDeclaration = node as XmlDeclaration;
				return this.CreateXmlDeclaration(xmlDeclaration.Version, xmlDeclaration.Encoding, xmlDeclaration.Standalone);
			}
			}
			throw new InvalidOperationException("Cannot import specified node type: " + node.NodeType);
		}

		/// <summary>Loads the XML document from the specified stream.</summary>
		/// <param name="inStream">The stream containing the XML document to load. </param>
		/// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, a <see cref="T:System.IO.FileNotFoundException" /> is raised. </exception>
		public virtual void Load(Stream inStream)
		{
			this.Load(new XmlValidatingReader(new XmlTextReader(inStream, this.NameTable)
			{
				XmlResolver = this.resolver
			})
			{
				EntityHandling = EntityHandling.ExpandCharEntities,
				ValidationType = ValidationType.None
			});
		}

		/// <summary>Loads the XML document from the specified URL.</summary>
		/// <param name="filename">URL for the file containing the XML document to load. The URL can be either a local file or an HTTP URL (a Web address).</param>
		/// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, a <see cref="T:System.IO.FileNotFoundException" /> is raised. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="filename" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="filename" /> is null. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">
		///   <paramref name="filename" /> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="filename" /> specified a directory.-or- The caller does not have the required permission. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="filename" /> was not found. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="filename" /> is in an invalid format. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public virtual void Load(string filename)
		{
			XmlTextReader xmlTextReader = null;
			try
			{
				xmlTextReader = new XmlTextReader(filename, this.NameTable);
				xmlTextReader.XmlResolver = this.resolver;
				this.Load(new XmlValidatingReader(xmlTextReader)
				{
					EntityHandling = EntityHandling.ExpandCharEntities,
					ValidationType = ValidationType.None
				});
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
			}
		}

		/// <summary>Loads the XML document from the specified <see cref="T:System.IO.TextReader" />.</summary>
		/// <param name="txtReader">The TextReader used to feed the XML data into the document. </param>
		/// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
		public virtual void Load(TextReader txtReader)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(txtReader, this.NameTable);
			XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(xmlTextReader);
			xmlValidatingReader.EntityHandling = EntityHandling.ExpandCharEntities;
			xmlValidatingReader.ValidationType = ValidationType.None;
			xmlTextReader.XmlResolver = this.resolver;
			this.Load(xmlValidatingReader);
		}

		/// <summary>Loads the XML document from the specified <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The XmlReader used to feed the XML data into the document. </param>
		/// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
		public virtual void Load(XmlReader xmlReader)
		{
			this.RemoveAll();
			this.baseURI = xmlReader.BaseURI;
			try
			{
				this.loadMode = true;
				for (;;)
				{
					XmlNode xmlNode = this.ReadNode(xmlReader);
					if (xmlNode == null)
					{
						break;
					}
					if (this.preserveWhitespace || xmlNode.NodeType != XmlNodeType.Whitespace)
					{
						base.AppendChild(xmlNode, false);
					}
				}
				if (xmlReader.Settings != null)
				{
					this.schemas = xmlReader.Settings.Schemas;
				}
			}
			finally
			{
				this.loadMode = false;
			}
		}

		/// <summary>Loads the XML document from the specified string.</summary>
		/// <param name="xml">String containing the XML document to load. </param>
		/// <exception cref="T:System.Xml.XmlException">There is a load or parse error in the XML. In this case, the document remains empty. </exception>
		public virtual void LoadXml(string xml)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(xml, XmlNodeType.Document, new XmlParserContext(this.NameTable, new XmlNamespaceManager(this.NameTable), null, XmlSpace.None));
			try
			{
				xmlTextReader.XmlResolver = this.resolver;
				this.Load(xmlTextReader);
			}
			finally
			{
				xmlTextReader.Close();
			}
		}

		internal void onNodeChanged(XmlNode node, XmlNode parent, string oldValue, string newValue)
		{
			if (this.NodeChanged != null)
			{
				this.NodeChanged(node, new XmlNodeChangedEventArgs(node, parent, parent, oldValue, newValue, XmlNodeChangedAction.Change));
			}
		}

		internal void onNodeChanging(XmlNode node, XmlNode parent, string oldValue, string newValue)
		{
			if (node.IsReadOnly)
			{
				throw new ArgumentException("Node is read-only.");
			}
			if (this.NodeChanging != null)
			{
				this.NodeChanging(node, new XmlNodeChangedEventArgs(node, parent, parent, oldValue, newValue, XmlNodeChangedAction.Change));
			}
		}

		internal void onNodeInserted(XmlNode node, XmlNode newParent)
		{
			if (this.NodeInserted != null)
			{
				this.NodeInserted(node, new XmlNodeChangedEventArgs(node, null, newParent, null, null, XmlNodeChangedAction.Insert));
			}
		}

		internal void onNodeInserting(XmlNode node, XmlNode newParent)
		{
			if (this.NodeInserting != null)
			{
				this.NodeInserting(node, new XmlNodeChangedEventArgs(node, null, newParent, null, null, XmlNodeChangedAction.Insert));
			}
		}

		internal void onNodeRemoved(XmlNode node, XmlNode oldParent)
		{
			if (this.NodeRemoved != null)
			{
				this.NodeRemoved(node, new XmlNodeChangedEventArgs(node, oldParent, null, null, null, XmlNodeChangedAction.Remove));
			}
		}

		internal void onNodeRemoving(XmlNode node, XmlNode oldParent)
		{
			if (this.NodeRemoving != null)
			{
				this.NodeRemoving(node, new XmlNodeChangedEventArgs(node, oldParent, null, null, null, XmlNodeChangedAction.Remove));
			}
		}

		private void ParseName(string name, out string prefix, out string localName)
		{
			int num = name.IndexOf(':');
			if (num != -1)
			{
				prefix = name.Substring(0, num);
				localName = name.Substring(num + 1);
			}
			else
			{
				prefix = string.Empty;
				localName = name;
			}
		}

		private XmlAttribute ReadAttributeNode(XmlReader reader)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				reader.MoveToFirstAttribute();
			}
			else if (reader.NodeType != XmlNodeType.Attribute)
			{
				throw new InvalidOperationException(this.MakeReaderErrorMessage("bad position to read attribute.", reader));
			}
			XmlAttribute xmlAttribute = this.CreateAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
			if (reader.SchemaInfo != null)
			{
				this.SchemaInfo = new XmlSchemaInfo(reader.SchemaInfo);
			}
			bool isDefault = reader.IsDefault;
			this.ReadAttributeNodeValue(reader, xmlAttribute);
			if (isDefault)
			{
				xmlAttribute.SetDefault();
			}
			return xmlAttribute;
		}

		internal void ReadAttributeNodeValue(XmlReader reader, XmlAttribute attribute)
		{
			while (reader.ReadAttributeValue())
			{
				if (reader.NodeType == XmlNodeType.EntityReference)
				{
					attribute.AppendChild(this.CreateEntityReference(reader.Name), false);
				}
				else
				{
					attribute.AppendChild(this.CreateTextNode(reader.Value), false);
				}
			}
		}

		/// <summary>Creates an <see cref="T:System.Xml.XmlNode" /> object based on the information in the <see cref="T:System.Xml.XmlReader" />. The reader must be positioned on a node or attribute.</summary>
		/// <returns>The new XmlNode or null if no more nodes exist.</returns>
		/// <param name="reader">The XML source </param>
		/// <exception cref="T:System.NullReferenceException">The reader is positioned on a node type that does not translate to a valid DOM node (for example, EndElement or EndEntity). </exception>
		public virtual XmlNode ReadNode(XmlReader reader)
		{
			if (this.PreserveWhitespace)
			{
				return this.ReadNodeCore(reader);
			}
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader != null && xmlTextReader.WhitespaceHandling == WhitespaceHandling.All)
			{
				try
				{
					xmlTextReader.WhitespaceHandling = WhitespaceHandling.Significant;
					return this.ReadNodeCore(reader);
				}
				finally
				{
					xmlTextReader.WhitespaceHandling = WhitespaceHandling.All;
				}
			}
			return this.ReadNodeCore(reader);
		}

		private XmlNode ReadNodeCore(XmlReader reader)
		{
			ReadState readState = reader.ReadState;
			if (readState != ReadState.Initial)
			{
				if (readState != ReadState.Interactive)
				{
					return null;
				}
			}
			else
			{
				if (reader.SchemaInfo != null)
				{
					this.SchemaInfo = new XmlSchemaInfo(reader.SchemaInfo);
				}
				reader.Read();
			}
			XmlNode xmlNode;
			switch (reader.NodeType)
			{
			case XmlNodeType.None:
				return null;
			case XmlNodeType.Element:
			{
				XmlElement xmlElement = this.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.NameTable == this.NameTable);
				if (reader.SchemaInfo != null)
				{
					this.SchemaInfo = new XmlSchemaInfo(reader.SchemaInfo);
				}
				xmlElement.IsEmpty = reader.IsEmptyElement;
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					xmlElement.SetAttributeNode(this.ReadAttributeNode(reader));
					reader.MoveToElement();
				}
				reader.MoveToElement();
				int depth = reader.Depth;
				if (reader.IsEmptyElement)
				{
					xmlNode = xmlElement;
					goto IL_36A;
				}
				reader.Read();
				while (reader.Depth > depth)
				{
					xmlNode = this.ReadNodeCore(reader);
					if (this.preserveWhitespace || xmlNode.NodeType != XmlNodeType.Whitespace)
					{
						xmlElement.AppendChild(xmlNode, false);
					}
				}
				xmlNode = xmlElement;
				goto IL_36A;
			}
			case XmlNodeType.Attribute:
			{
				string localName = reader.LocalName;
				string namespaceURI = reader.NamespaceURI;
				xmlNode = this.ReadAttributeNode(reader);
				reader.MoveToAttribute(localName, namespaceURI);
				return xmlNode;
			}
			case XmlNodeType.Text:
				xmlNode = this.CreateTextNode(reader.Value);
				goto IL_36A;
			case XmlNodeType.CDATA:
				xmlNode = this.CreateCDataSection(reader.Value);
				goto IL_36A;
			case XmlNodeType.EntityReference:
				if (this.loadMode && this.DocumentType != null && this.DocumentType.Entities.GetNamedItem(reader.Name) == null)
				{
					throw new XmlException("Reference to undeclared entity was found.");
				}
				xmlNode = this.CreateEntityReference(reader.Name);
				if (reader.CanResolveEntity)
				{
					reader.ResolveEntity();
					reader.Read();
					XmlNode newChild;
					while (reader.NodeType != XmlNodeType.EndEntity && (newChild = this.ReadNode(reader)) != null)
					{
						xmlNode.InsertBefore(newChild, null, false, false);
					}
				}
				goto IL_36A;
			case XmlNodeType.ProcessingInstruction:
				xmlNode = this.CreateProcessingInstruction(reader.Name, reader.Value);
				goto IL_36A;
			case XmlNodeType.Comment:
				xmlNode = this.CreateComment(reader.Value);
				goto IL_36A;
			case XmlNodeType.DocumentType:
			{
				DTDObjectModel dtdobjectModel = null;
				IHasXmlParserContext hasXmlParserContext = reader as IHasXmlParserContext;
				if (hasXmlParserContext != null)
				{
					dtdobjectModel = hasXmlParserContext.ParserContext.Dtd;
				}
				if (dtdobjectModel != null)
				{
					xmlNode = this.CreateDocumentType(dtdobjectModel);
				}
				else
				{
					xmlNode = this.CreateDocumentType(reader.Name, reader["PUBLIC"], reader["SYSTEM"], reader.Value);
				}
				goto IL_36A;
			}
			case XmlNodeType.Whitespace:
				xmlNode = this.CreateWhitespace(reader.Value);
				goto IL_36A;
			case XmlNodeType.SignificantWhitespace:
				xmlNode = this.CreateSignificantWhitespace(reader.Value);
				goto IL_36A;
			case XmlNodeType.XmlDeclaration:
				xmlNode = this.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
				xmlNode.Value = reader.Value;
				goto IL_36A;
			}
			throw new NullReferenceException("Unexpected node type " + reader.NodeType + ".");
			IL_36A:
			reader.Read();
			return xmlNode;
		}

		private string MakeReaderErrorMessage(string message, XmlReader reader)
		{
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} Line number = {1}, Inline position = {2}.", new object[]
				{
					message,
					xmlLineInfo.LineNumber,
					xmlLineInfo.LinePosition
				});
			}
			return message;
		}

		internal void RemoveIdenticalAttribute(string id)
		{
			this.idTable.Remove(id);
		}

		/// <summary>Saves the XML document to the specified stream.</summary>
		/// <param name="outStream">The stream to which you want to save. </param>
		/// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
		public virtual void Save(Stream outStream)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(outStream, this.TextEncoding);
			if (!this.PreserveWhitespace)
			{
				xmlTextWriter.Formatting = Formatting.Indented;
			}
			this.WriteContentTo(xmlTextWriter);
			xmlTextWriter.Flush();
		}

		/// <summary>Saves the XML document to the specified file.</summary>
		/// <param name="filename">The location of the file where you want to save the document. </param>
		/// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
		public virtual void Save(string filename)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(filename, this.TextEncoding);
			try
			{
				if (!this.PreserveWhitespace)
				{
					xmlTextWriter.Formatting = Formatting.Indented;
				}
				this.WriteContentTo(xmlTextWriter);
			}
			finally
			{
				xmlTextWriter.Close();
			}
		}

		/// <summary>Saves the XML document to the specified <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="writer">The TextWriter to which you want to save. </param>
		/// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
		public virtual void Save(TextWriter writer)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(writer);
			if (!this.PreserveWhitespace)
			{
				xmlTextWriter.Formatting = Formatting.Indented;
			}
			if (this.FirstChild != null && this.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
			{
				xmlTextWriter.WriteStartDocument();
			}
			this.WriteContentTo(xmlTextWriter);
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
		}

		/// <summary>Saves the XML document to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		/// <exception cref="T:System.Xml.XmlException">The operation would not result in a well formed XML document (for example, no document element or duplicate XML declarations). </exception>
		public virtual void Save(XmlWriter xmlWriter)
		{
			bool flag = this.FirstChild != null && this.FirstChild.NodeType != XmlNodeType.XmlDeclaration;
			if (flag)
			{
				xmlWriter.WriteStartDocument();
			}
			this.WriteContentTo(xmlWriter);
			if (flag)
			{
				xmlWriter.WriteEndDocument();
			}
			xmlWriter.Flush();
		}

		/// <summary>Saves all the children of the XmlDocument node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="xw">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				xmlNode.WriteTo(w);
			}
		}

		/// <summary>Saves the XmlDocument node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			this.WriteContentTo(w);
		}

		private void AddDefaultNameTableKeys()
		{
			this.nameTable.Add("#text");
			this.nameTable.Add("xml");
			this.nameTable.Add("xmlns");
			this.nameTable.Add("#entity");
			this.nameTable.Add("#document-fragment");
			this.nameTable.Add("#comment");
			this.nameTable.Add("space");
			this.nameTable.Add("id");
			this.nameTable.Add("#whitespace");
			this.nameTable.Add("http://www.w3.org/2000/xmlns/");
			this.nameTable.Add("#cdata-section");
			this.nameTable.Add("lang");
			this.nameTable.Add("#document");
			this.nameTable.Add("#significant-whitespace");
		}

		internal void CheckIdTableUpdate(XmlAttribute attr, string oldValue, string newValue)
		{
			if (this.idTable[oldValue] == attr)
			{
				this.idTable.Remove(oldValue);
				this.idTable[newValue] = attr;
			}
		}

		/// <summary>Validates the <see cref="T:System.Xml.XmlDocument" /> against the XML Schema Definition Language (XSD) schemas contained in the <see cref="P:System.Xml.XmlDocument.Schemas" /> property.</summary>
		/// <param name="validationEventHandler">The <see cref="T:System.Xml.Schema.ValidationEventHandler" /> object that receives information about schema validation warnings and errors.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">A schema validation event occurred and no <see cref="T:System.Xml.Schema.ValidationEventHandler" /> object was specified.</exception>
		public void Validate(ValidationEventHandler handler)
		{
			this.Validate(handler, this, XmlSchemaValidationFlags.ProcessIdentityConstraints);
		}

		/// <summary>Validates the <see cref="T:System.Xml.XmlNode" /> object specified against the XML Schema Definition Language (XSD) schemas in the <see cref="P:System.Xml.XmlDocument.Schemas" /> property.</summary>
		/// <param name="validationEventHandler">The <see cref="T:System.Xml.Schema.ValidationEventHandler" /> object that receives information about schema validation warnings and errors.</param>
		/// <param name="nodeToValidate">The <see cref="T:System.Xml.XmlNode" /> object created from an <see cref="T:System.Xml.XmlDocument" /> to validate.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XmlNode" /> object parameter was not created from an <see cref="T:System.Xml.XmlDocument" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlNode" /> object parameter is not an element, attribute, document fragment, or the root node.</exception>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">A schema validation event occurred and no <see cref="T:System.Xml.Schema.ValidationEventHandler" /> object was specified.</exception>
		public void Validate(ValidationEventHandler handler, XmlNode node)
		{
			this.Validate(handler, node, XmlSchemaValidationFlags.ProcessIdentityConstraints);
		}

		private void Validate(ValidationEventHandler handler, XmlNode node, XmlSchemaValidationFlags flags)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.NameTable = this.NameTable;
			xmlReaderSettings.Schemas = this.schemas;
			xmlReaderSettings.Schemas.XmlResolver = this.resolver;
			xmlReaderSettings.XmlResolver = this.resolver;
			xmlReaderSettings.ValidationFlags = flags;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			XmlReader xmlReader = XmlReader.Create(new XmlNodeReader(node), xmlReaderSettings);
			while (!xmlReader.EOF)
			{
				xmlReader.Read();
			}
		}
	}
}
