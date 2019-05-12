using Mono.Xml;
using Mono.Xml.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	/// <summary>Represents a reader that provides document type definition (DTD), XML-Data Reduced (XDR) schema, and XML Schema definition language (XSD) validation.</summary>
	[Obsolete("Use XmlReader created by XmlReader.Create() method using appropriate XmlReaderSettings instead.")]
	public class XmlValidatingReader : XmlReader, IHasXmlParserContext, IXmlLineInfo, IXmlNamespaceResolver
	{
		private EntityHandling entityHandling;

		private XmlReader sourceReader;

		private XmlTextReader xmlTextReader;

		private XmlReader validatingReader;

		private XmlResolver resolver;

		private bool resolverSpecified;

		private ValidationType validationType;

		private XmlSchemaCollection schemas;

		private DTDValidatingReader dtdReader;

		private IHasXmlSchemaInfo schemaInfo;

		private StringBuilder storedCharacters;

		/// <summary>Initializes a new instance of the XmlValidatingReader class that validates the content returned from the given <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="reader">The XmlReader to read from while validating. The current implementation supports only <see cref="T:System.Xml.XmlTextReader" />. </param>
		/// <exception cref="T:System.ArgumentException">The reader specified is not an XmlTextReader. </exception>
		public XmlValidatingReader(XmlReader reader)
		{
			this.sourceReader = reader;
			this.xmlTextReader = (reader as XmlTextReader);
			if (this.xmlTextReader == null)
			{
				this.resolver = new XmlUrlResolver();
			}
			this.entityHandling = EntityHandling.ExpandEntities;
			this.validationType = ValidationType.Auto;
			this.storedCharacters = new StringBuilder();
		}

		/// <summary>Initializes a new instance of the XmlValidatingReader class with the specified values.</summary>
		/// <param name="xmlFragment">The stream containing the XML fragment to parse. </param>
		/// <param name="fragType">The <see cref="T:System.Xml.XmlNodeType" /> of the XML fragment. This determines what the fragment can contain (see table below). </param>
		/// <param name="context">The <see cref="T:System.Xml.XmlParserContext" /> in which the XML fragment is to be parsed. This includes the <see cref="T:System.Xml.XmlNameTable" /> to use, encoding, namespace scope, current xml:lang, and xml:space scope. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="fragType" /> is not one of the node types listed in the table below. </exception>
		public XmlValidatingReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context) : this(new XmlTextReader(xmlFragment, fragType, context))
		{
		}

		/// <summary>Initializes a new instance of the XmlValidatingReader class with the specified values.</summary>
		/// <param name="xmlFragment">The string containing the XML fragment to parse. </param>
		/// <param name="fragType">The <see cref="T:System.Xml.XmlNodeType" /> of the XML fragment. This also determines what the fragment string can contain (see table below). </param>
		/// <param name="context">The <see cref="T:System.Xml.XmlParserContext" /> in which the XML fragment is to be parsed. This includes the <see cref="T:System.Xml.NameTable" /> to use, encoding, namespace scope, current xml:lang, and xml:space scope. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="fragType" /> is not one of the node types listed in the table below. </exception>
		public XmlValidatingReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context) : this(new XmlTextReader(xmlFragment, fragType, context))
		{
		}

		/// <summary>Sets an event handler for receiving information about document type definition (DTD), XML-Data Reduced (XDR) schema, and XML Schema definition language (XSD) schema validation errors.</summary>
		public event ValidationEventHandler ValidationEventHandler;

		XmlParserContext IHasXmlParserContext.ParserContext
		{
			get
			{
				if (this.dtdReader != null)
				{
					return this.dtdReader.ParserContext;
				}
				IHasXmlParserContext hasXmlParserContext = this.sourceReader as IHasXmlParserContext;
				return (hasXmlParserContext == null) ? null : hasXmlParserContext.ParserContext;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.IXmlNamespaceResolver.GetNamespacesInScope(System.Xml.XmlNamespaceScope)" />.</summary>
		/// <param name="scope"></param>
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return ((IHasXmlParserContext)this).ParserContext.NamespaceManager.GetNamespacesInScope(scope);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.IXmlNamespaceResolver.LookupPrefix(System.String)" />.</summary>
		/// <param name="namespaceName"></param>
		string IXmlNamespaceResolver.LookupPrefix(string ns)
		{
			IXmlNamespaceResolver xmlNamespaceResolver;
			if (this.validatingReader != null)
			{
				xmlNamespaceResolver = (this.sourceReader as IXmlNamespaceResolver);
			}
			else
			{
				xmlNamespaceResolver = (this.validatingReader as IXmlNamespaceResolver);
			}
			return (xmlNamespaceResolver == null) ? null : xmlNamespaceResolver.LookupNamespace(ns);
		}

		/// <summary>Gets the number of attributes on the current node.</summary>
		/// <returns>The number of attributes on the current node. This number includes default attributes.</returns>
		public override int AttributeCount
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.AttributeCount : 0;
			}
		}

		/// <summary>Gets the base URI of the current node.</summary>
		/// <returns>The base URI of the current node.</returns>
		public override string BaseURI
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.BaseURI : this.sourceReader.BaseURI;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Xml.XmlValidatingReader" /> implements the binary content read methods.</summary>
		/// <returns>true if the binary content read methods are implemented; otherwise false. The <see cref="T:System.Xml.XmlValidatingReader" /> class returns true.</returns>
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether this reader can parse and resolve entities.</summary>
		/// <returns>true if the reader can parse and resolve entities; otherwise, false. XmlValidatingReader always returns true.</returns>
		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the depth of the current node in the XML document.</summary>
		/// <returns>The depth of the current node in the XML document.</returns>
		public override int Depth
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.Depth : 0;
			}
		}

		/// <summary>Gets the encoding attribute for the document.</summary>
		/// <returns>The encoding value. If no encoding attribute exists, and there is not byte-order mark, this defaults to UTF-8.</returns>
		public Encoding Encoding
		{
			get
			{
				if (this.xmlTextReader != null)
				{
					return this.xmlTextReader.Encoding;
				}
				throw new NotSupportedException("Encoding is supported only for XmlTextReader.");
			}
		}

		/// <summary>Gets or sets a value that specifies how the reader handles entities.</summary>
		/// <returns>One of the <see cref="T:System.Xml.EntityHandling" /> values. If no EntityHandling is specified, it defaults to EntityHandling.ExpandEntities.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Invalid value was specified. </exception>
		public EntityHandling EntityHandling
		{
			get
			{
				return this.entityHandling;
			}
			set
			{
				this.entityHandling = value;
				if (this.dtdReader != null)
				{
					this.dtdReader.EntityHandling = value;
				}
			}
		}

		/// <summary>Gets a value indicating whether the reader is positioned at the end of the stream.</summary>
		/// <returns>true if the reader is positioned at the end of the stream; otherwise, false.</returns>
		public override bool EOF
		{
			get
			{
				return this.validatingReader != null && this.validatingReader.EOF;
			}
		}

		/// <summary>Gets a value indicating whether the current node can have a <see cref="P:System.Xml.XmlValidatingReader.Value" /> other than String.Empty.</summary>
		/// <returns>true if the node on which the reader is currently positioned can have a Value; otherwise, false.</returns>
		public override bool HasValue
		{
			get
			{
				return this.validatingReader != null && this.validatingReader.HasValue;
			}
		}

		/// <summary>Gets a value indicating whether the current node is an attribute that was generated from the default value defined in the document type definition (DTD) or schema.</summary>
		/// <returns>true if the current node is an attribute whose value was generated from the default value defined in the DTD or schema; false if the attribute value was explicitly set.</returns>
		public override bool IsDefault
		{
			get
			{
				return this.validatingReader != null && this.validatingReader.IsDefault;
			}
		}

		/// <summary>Gets a value indicating whether the current node is an empty element (for example, &lt;MyElement/&gt;).</summary>
		/// <returns>true if the current node is an element (<see cref="P:System.Xml.XmlValidatingReader.NodeType" /> equals XmlNodeType.Element) that ends with /&gt;; otherwise, false.</returns>
		public override bool IsEmptyElement
		{
			get
			{
				return this.validatingReader != null && this.validatingReader.IsEmptyElement;
			}
		}

		/// <summary>Gets the current line number.</summary>
		/// <returns>The current line number. The starting value for this property is 1.</returns>
		public int LineNumber
		{
			get
			{
				if (this.IsDefault)
				{
					return 0;
				}
				IXmlLineInfo xmlLineInfo = this.validatingReader as IXmlLineInfo;
				return (xmlLineInfo == null) ? 0 : xmlLineInfo.LineNumber;
			}
		}

		/// <summary>Gets the current line position.</summary>
		/// <returns>The current line position. The starting value for this property is 1.</returns>
		public int LinePosition
		{
			get
			{
				if (this.IsDefault)
				{
					return 0;
				}
				IXmlLineInfo xmlLineInfo = this.validatingReader as IXmlLineInfo;
				return (xmlLineInfo == null) ? 0 : xmlLineInfo.LinePosition;
			}
		}

		/// <summary>Gets the local name of the current node.</summary>
		/// <returns>The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.</returns>
		public override string LocalName
		{
			get
			{
				if (this.validatingReader == null)
				{
					return string.Empty;
				}
				if (this.Namespaces)
				{
					return this.validatingReader.LocalName;
				}
				return this.validatingReader.Name;
			}
		}

		/// <summary>Gets the qualified name of the current node.</summary>
		/// <returns>The qualified name of the current node. For example, Name is bk:book for the element &lt;bk:book&gt;.The name returned is dependent on the <see cref="P:System.Xml.XmlValidatingReader.NodeType" /> of the node. The following node types return the listed values. All other node types return an empty string.Node Type Name AttributeThe name of the attribute. DocumentTypeThe document type name. ElementThe tag name. EntityReferenceThe name of the entity referenced. ProcessingInstructionThe target of the processing instruction. XmlDeclarationThe literal string xml. </returns>
		public override string Name
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.Name : string.Empty;
			}
		}

		/// <summary>Gets or sets a value indicating whether to do namespace support.</summary>
		/// <returns>true to do namespace support; otherwise, false. The default is true.</returns>
		public bool Namespaces
		{
			get
			{
				return this.xmlTextReader == null || this.xmlTextReader.Namespaces;
			}
			set
			{
				if (this.ReadState != ReadState.Initial)
				{
					throw new InvalidOperationException("Namespaces have to be set before reading.");
				}
				if (this.xmlTextReader != null)
				{
					this.xmlTextReader.Namespaces = value;
					return;
				}
				throw new NotSupportedException("Property 'Namespaces' is supported only for XmlTextReader.");
			}
		}

		/// <summary>Gets the namespace Uniform Resource Identifier (URI) (as defined in the World Wide Web Consortium (W3C) Namespace specification) of the node on which the reader is positioned.</summary>
		/// <returns>The namespace URI of the current node; otherwise an empty string.</returns>
		public override string NamespaceURI
		{
			get
			{
				if (this.validatingReader == null)
				{
					return string.Empty;
				}
				if (this.Namespaces)
				{
					return this.validatingReader.NamespaceURI;
				}
				return string.Empty;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlNameTable" /> associated with this implementation.</summary>
		/// <returns>XmlNameTable that enables you to get the atomized version of a string within the node.</returns>
		public override XmlNameTable NameTable
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.NameTable : this.sourceReader.NameTable;
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlNodeType" /> values representing the type of the current node.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.NodeType : XmlNodeType.None;
			}
		}

		/// <summary>Gets the namespace prefix associated with the current node.</summary>
		/// <returns>The namespace prefix associated with the current node.</returns>
		public override string Prefix
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.Prefix : string.Empty;
			}
		}

		/// <summary>Gets the quotation mark character used to enclose the value of an attribute node.</summary>
		/// <returns>The quotation mark character (" or ') used to enclose the value of an attribute node.</returns>
		public override char QuoteChar
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.QuoteChar : this.sourceReader.QuoteChar;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlReader" /> used to construct this XmlValidatingReader.</summary>
		/// <returns>The XmlReader specified in the constructor.</returns>
		public XmlReader Reader
		{
			get
			{
				return this.sourceReader;
			}
		}

		/// <summary>Gets the state of the reader.</summary>
		/// <returns>One of the <see cref="T:System.Xml.ReadState" /> values.</returns>
		public override ReadState ReadState
		{
			get
			{
				if (this.validatingReader == null)
				{
					return ReadState.Initial;
				}
				return this.validatingReader.ReadState;
			}
		}

		internal XmlResolver Resolver
		{
			get
			{
				if (this.xmlTextReader != null)
				{
					return this.xmlTextReader.Resolver;
				}
				if (this.resolverSpecified)
				{
					return this.resolver;
				}
				return null;
			}
		}

		/// <summary>Gets a <see cref="T:System.Xml.Schema.XmlSchemaCollection" /> to use for validation.</summary>
		/// <returns>The XmlSchemaCollection to use for validation.</returns>
		public XmlSchemaCollection Schemas
		{
			get
			{
				if (this.schemas == null)
				{
					this.schemas = new XmlSchemaCollection(this.NameTable);
				}
				return this.schemas;
			}
		}

		/// <summary>Gets a schema type object.</summary>
		/// <returns>
		///   <see cref="T:System.Xml.Schema.XmlSchemaDatatype" />, <see cref="T:System.Xml.Schema.XmlSchemaSimpleType" />, or <see cref="T:System.Xml.Schema.XmlSchemaComplexType" /> depending whether the node value is a built in XML Schema definition language (XSD) type or a user defined simpleType or complexType; null if the current node has no schema type.</returns>
		public object SchemaType
		{
			get
			{
				return this.schemaInfo.SchemaType;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlReaderSettings" /> object that was used to create this <see cref="T:System.Xml.XmlValidatingReader" /> instance.</summary>
		/// <returns>null because <see cref="T:System.Xml.XmlValidatingReader" /> objects are not instantiated using the <see cref="T:System.Xml.XmlReaderSettings" /> class and the <see cref="Overload:System.Xml.XmlReader.Create" /> method.</returns>
		[MonoTODO]
		public override XmlReaderSettings Settings
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.Settings : this.sourceReader.Settings;
			}
		}

		/// <summary>Gets or sets a value indicating the type of validation to perform.</summary>
		/// <returns>One of the <see cref="T:System.Xml.ValidationType" /> values. If this property is not set, it defaults to ValidationType.Auto.</returns>
		/// <exception cref="T:System.InvalidOperationException">Setting the property after a Read has been called. </exception>
		[MonoTODO]
		public ValidationType ValidationType
		{
			get
			{
				return this.validationType;
			}
			set
			{
				if (this.ReadState != ReadState.Initial)
				{
					throw new InvalidOperationException("ValidationType cannot be set after the first call to Read method.");
				}
				switch (this.validationType)
				{
				case ValidationType.None:
				case ValidationType.Auto:
				case ValidationType.DTD:
				case ValidationType.Schema:
					this.validationType = value;
					break;
				case ValidationType.XDR:
					throw new NotSupportedException();
				}
			}
		}

		/// <summary>Gets the text value of the current node.</summary>
		/// <returns>The value returned depends on the <see cref="P:System.Xml.XmlValidatingReader.NodeType" /> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node Type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space between markup in a mixed content model. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
		public override string Value
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.Value : string.Empty;
			}
		}

		/// <summary>Gets the current xml:lang scope.</summary>
		/// <returns>The current xml:lang scope.</returns>
		public override string XmlLang
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.XmlLang : string.Empty;
			}
		}

		/// <summary>Sets the <see cref="T:System.Xml.XmlResolver" /> used for resolving external document type definition (DTD) and schema location references. The XmlResolver is also used to handle any import or include elements found in XML Schema definition language (XSD) schemas.</summary>
		/// <returns>The XmlResolver to use. If set to null, external resources are not resolved.In version 1.1 of the .NET Framework, the caller must be fully trusted to specify an XmlResolver.</returns>
		public XmlResolver XmlResolver
		{
			set
			{
				this.resolverSpecified = true;
				this.resolver = value;
				if (this.xmlTextReader != null)
				{
					this.xmlTextReader.XmlResolver = value;
				}
				XsdValidatingReader xsdValidatingReader = this.validatingReader as XsdValidatingReader;
				if (xsdValidatingReader != null)
				{
					xsdValidatingReader.XmlResolver = value;
				}
				DTDValidatingReader dtdvalidatingReader = this.validatingReader as DTDValidatingReader;
				if (dtdvalidatingReader != null)
				{
					dtdvalidatingReader.XmlResolver = value;
				}
			}
		}

		/// <summary>Gets the current xml:space scope.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlSpace" /> values. If no xml:space scope exists, this property defaults to XmlSpace.None.</returns>
		public override XmlSpace XmlSpace
		{
			get
			{
				return (this.validatingReader != null) ? this.validatingReader.XmlSpace : XmlSpace.None;
			}
		}

		/// <summary>Changes the <see cref="P:System.Xml.XmlReader.ReadState" /> to Closed.</summary>
		public override void Close()
		{
			if (this.validatingReader == null)
			{
				this.sourceReader.Close();
			}
			else
			{
				this.validatingReader.Close();
			}
		}

		/// <summary>Gets the value of the attribute with the specified index.</summary>
		/// <returns>The value of the specified attribute.</returns>
		/// <param name="i">The index of the attribute. The index is zero-based. (The first attribute has index 0.) </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="i" /> parameter is less than 0 or greater than or equal to <see cref="P:System.Xml.XmlValidatingReader.AttributeCount" />. </exception>
		public override string GetAttribute(int i)
		{
			if (this.validatingReader == null)
			{
				throw new IndexOutOfRangeException("Reader is not started.");
			}
			return this.validatingReader[i];
		}

		/// <summary>Gets the value of the attribute with the specified name.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found, null is returned.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public override string GetAttribute(string name)
		{
			return (this.validatingReader != null) ? this.validatingReader[name] : null;
		}

		/// <summary>Gets the value of the attribute with the specified local name and namespace Uniform Resource Identifier (URI).</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found, null is returned. This method does not move the reader.</returns>
		/// <param name="localName">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		public override string GetAttribute(string localName, string namespaceName)
		{
			return (this.validatingReader != null) ? this.validatingReader[localName, namespaceName] : null;
		}

		/// <summary>Gets a value indicating whether the class can return line information.</summary>
		/// <returns>true if the class can return line information; otherwise, false.</returns>
		public bool HasLineInfo()
		{
			IXmlLineInfo xmlLineInfo = this.validatingReader as IXmlLineInfo;
			return xmlLineInfo != null && xmlLineInfo.HasLineInfo();
		}

		/// <summary>Resolves a namespace prefix in the current element's scope.</summary>
		/// <returns>The namespace URI to which the prefix maps or null if no matching prefix is found.</returns>
		/// <param name="prefix">The prefix whose namespace Uniform Resource Identifier (URI) you want to resolve. To match the default namespace, pass an empty string. </param>
		public override string LookupNamespace(string prefix)
		{
			if (this.validatingReader != null)
			{
				return this.validatingReader.LookupNamespace(prefix);
			}
			return this.sourceReader.LookupNamespace(prefix);
		}

		/// <summary>Moves to the attribute with the specified index.</summary>
		/// <param name="i">The index of the attribute. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="i" /> parameter is less than 0 or greater than or equal to <see cref="P:System.Xml.XmlReader.AttributeCount" />. </exception>
		public override void MoveToAttribute(int i)
		{
			if (this.validatingReader == null)
			{
				throw new IndexOutOfRangeException("Reader is not started.");
			}
			this.validatingReader.MoveToAttribute(i);
		}

		/// <summary>Moves to the attribute with the specified name.</summary>
		/// <returns>true if the attribute is found; otherwise, false. If false, the position of the reader does not change.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public override bool MoveToAttribute(string name)
		{
			return this.validatingReader != null && this.validatingReader.MoveToAttribute(name);
		}

		/// <summary>Moves to the attribute with the specified local name and namespace Uniform Resource Identifier (URI).</summary>
		/// <returns>true if the attribute is found; otherwise, false. If false, the position of the reader does not change.</returns>
		/// <param name="localName">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		public override bool MoveToAttribute(string localName, string namespaceName)
		{
			return this.validatingReader != null && this.validatingReader.MoveToAttribute(localName, namespaceName);
		}

		/// <summary>Moves to the element that contains the current attribute node.</summary>
		/// <returns>true if the reader is positioned on an attribute (the reader moves to the element that owns the attribute); false if the reader is not positioned on an attribute (the position of the reader does not change).</returns>
		public override bool MoveToElement()
		{
			return this.validatingReader != null && this.validatingReader.MoveToElement();
		}

		/// <summary>Moves to the first attribute.</summary>
		/// <returns>true if an attribute exists (the reader moves to the first attribute); otherwise, false (the position of the reader does not change).</returns>
		public override bool MoveToFirstAttribute()
		{
			return this.validatingReader != null && this.validatingReader.MoveToFirstAttribute();
		}

		/// <summary>Moves to the next attribute.</summary>
		/// <returns>true if there is a next attribute; false if there are no more attributes.</returns>
		public override bool MoveToNextAttribute()
		{
			return this.validatingReader != null && this.validatingReader.MoveToNextAttribute();
		}

		/// <summary>Reads the next node from the stream.</summary>
		/// <returns>true if the next node was read successfully; false if there are no more nodes to read.</returns>
		[MonoTODO]
		public override bool Read()
		{
			if (this.validatingReader == null)
			{
				switch (this.ValidationType)
				{
				case ValidationType.None:
				case ValidationType.Auto:
					break;
				case ValidationType.DTD:
					this.validatingReader = (this.dtdReader = new DTDValidatingReader(this.sourceReader, this));
					this.dtdReader.XmlResolver = this.Resolver;
					goto IL_F3;
				case ValidationType.XDR:
					throw new NotSupportedException();
				case ValidationType.Schema:
					break;
				default:
					goto IL_F3;
				}
				this.dtdReader = new DTDValidatingReader(this.sourceReader, this);
				XsdValidatingReader xsdValidatingReader = new XsdValidatingReader(this.dtdReader);
				XsdValidatingReader xsdValidatingReader2 = xsdValidatingReader;
				xsdValidatingReader2.ValidationEventHandler = (ValidationEventHandler)Delegate.Combine(xsdValidatingReader2.ValidationEventHandler, new ValidationEventHandler(this.OnValidationEvent));
				xsdValidatingReader.ValidationType = this.ValidationType;
				xsdValidatingReader.Schemas = this.Schemas.SchemaSet;
				xsdValidatingReader.XmlResolver = this.Resolver;
				this.validatingReader = xsdValidatingReader;
				this.dtdReader.XmlResolver = this.Resolver;
				IL_F3:
				this.schemaInfo = (this.validatingReader as IHasXmlSchemaInfo);
			}
			return this.validatingReader.Read();
		}

		/// <summary>Parses the attribute value into one or more Text, EntityReference, or EndEntity nodes.</summary>
		/// <returns>true if there are nodes to return.false if the reader is not positioned on an attribute node when the initial call is made or if all the attribute values have been read.An empty attribute, such as, misc="", returns true with a single node with a value of String.Empty.</returns>
		public override bool ReadAttributeValue()
		{
			return this.validatingReader != null && this.validatingReader.ReadAttributeValue();
		}

		/// <summary>Reads the contents of an element or text node as a string.</summary>
		/// <returns>The contents of the element or text node. This can be an empty string if the reader is positioned on something other than an element or text node, or if there is no more text content to return in the current context.Note:The text node can be either an element or an attribute text node.</returns>
		public override string ReadString()
		{
			return base.ReadString();
		}

		/// <summary>Gets the common language runtime type for the specified XML Schema definition language (XSD) type.</summary>
		/// <returns>The common language runtime type for the specified XML Schema type.</returns>
		public object ReadTypedValue()
		{
			if (this.dtdReader == null)
			{
				return null;
			}
			XmlSchemaDatatype xmlSchemaDatatype = this.schemaInfo.SchemaType as XmlSchemaDatatype;
			if (xmlSchemaDatatype == null)
			{
				XmlSchemaType xmlSchemaType = this.schemaInfo.SchemaType as XmlSchemaType;
				if (xmlSchemaType != null)
				{
					xmlSchemaDatatype = xmlSchemaType.Datatype;
				}
			}
			if (xmlSchemaDatatype == null)
			{
				return null;
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					return null;
				}
				return xmlSchemaDatatype.ParseValue(this.Value, this.NameTable, this.dtdReader.ParserContext.NamespaceManager);
			}
			else
			{
				if (this.IsEmptyElement)
				{
					return null;
				}
				this.storedCharacters.Length = 0;
				bool flag = true;
				for (;;)
				{
					this.Read();
					XmlNodeType nodeType2 = this.NodeType;
					switch (nodeType2)
					{
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						goto IL_C6;
					default:
						if (nodeType2 == XmlNodeType.Whitespace || nodeType2 == XmlNodeType.SignificantWhitespace)
						{
							goto IL_C6;
						}
						flag = false;
						break;
					case XmlNodeType.Comment:
						break;
					}
					IL_E9:
					if (!flag || this.EOF)
					{
						break;
					}
					continue;
					IL_C6:
					this.storedCharacters.Append(this.Value);
					goto IL_E9;
				}
				return xmlSchemaDatatype.ParseValue(this.storedCharacters.ToString(), this.NameTable, this.dtdReader.ParserContext.NamespaceManager);
			}
		}

		/// <summary>Resolves the entity reference for EntityReference nodes.</summary>
		/// <exception cref="T:System.InvalidOperationException">The reader is not positioned on an EntityReference node. </exception>
		public override void ResolveEntity()
		{
			this.validatingReader.ResolveEntity();
		}

		internal void OnValidationEvent(object o, ValidationEventArgs e)
		{
			if (this.ValidationEventHandler != null)
			{
				this.ValidationEventHandler(o, e);
			}
			else if (this.ValidationType != ValidationType.None && e.Severity == XmlSeverityType.Error)
			{
				throw e.Exception;
			}
		}

		/// <summary>Reads the content and returns the Base64 decoded binary bytes.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Xml.XmlValidatingReader.ReadContentAsBase64(System.Byte[],System.Int32,System.Int32)" />  is not supported on the current node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		[MonoTODO]
		public override int ReadContentAsBase64(byte[] buffer, int offset, int length)
		{
			if (this.validatingReader != null)
			{
				return this.validatingReader.ReadContentAsBase64(buffer, offset, length);
			}
			return this.sourceReader.ReadContentAsBase64(buffer, offset, length);
		}

		/// <summary>Reads the content and returns the BinHex decoded binary bytes.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Xml.XmlValidatingReader.ReadContentAsBinHex(System.Byte[],System.Int32,System.Int32)" />  is not supported on the current node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlValidatingReader" /> implementation does not support this method.</exception>
		[MonoTODO]
		public override int ReadContentAsBinHex(byte[] buffer, int offset, int length)
		{
			if (this.validatingReader != null)
			{
				return this.validatingReader.ReadContentAsBinHex(buffer, offset, length);
			}
			return this.sourceReader.ReadContentAsBinHex(buffer, offset, length);
		}

		/// <summary>Reads the element and decodes the Base64 content.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not an element node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlValidatingReader" /> implementation does not support this method.</exception>
		/// <exception cref="T:System.Xml.XmlException">The element contains mixed-content.</exception>
		/// <exception cref="T:System.FormatException">The content cannot be converted to the requested type.</exception>
		[MonoTODO]
		public override int ReadElementContentAsBase64(byte[] buffer, int offset, int length)
		{
			if (this.validatingReader != null)
			{
				return this.validatingReader.ReadElementContentAsBase64(buffer, offset, length);
			}
			return this.sourceReader.ReadElementContentAsBase64(buffer, offset, length);
		}

		/// <summary>Reads the element and decodes the BinHex content.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not an element node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlValidatingReader" /> implementation does not support this method.</exception>
		/// <exception cref="T:System.Xml.XmlException">The element contains mixed-content.</exception>
		/// <exception cref="T:System.FormatException">The content cannot be converted to the requested type.</exception>
		[MonoTODO]
		public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int length)
		{
			if (this.validatingReader != null)
			{
				return this.validatingReader.ReadElementContentAsBinHex(buffer, offset, length);
			}
			return this.sourceReader.ReadElementContentAsBinHex(buffer, offset, length);
		}
	}
}
