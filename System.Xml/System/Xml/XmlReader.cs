using Mono.Xml;
using Mono.Xml.Schema;
using System;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	/// <summary>Represents a reader that provides fast, non-cached, forward-only access to XML data.</summary>
	public abstract class XmlReader : IDisposable
	{
		private StringBuilder readStringBuffer;

		private XmlReaderBinarySupport binary;

		private XmlReaderSettings settings;

		/// <summary>For a description of this member, see <see cref="M:System.IDisposable.Dispose" />.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(false);
		}

		/// <summary>When overridden in a derived class, gets the number of attributes on the current node.</summary>
		/// <returns>The number of attributes on the current node.</returns>
		public abstract int AttributeCount { get; }

		/// <summary>When overridden in a derived class, gets the base URI of the current node.</summary>
		/// <returns>The base URI of the current node.</returns>
		public abstract string BaseURI { get; }

		internal XmlReaderBinarySupport Binary
		{
			get
			{
				return this.binary;
			}
		}

		internal XmlReaderBinarySupport.CharGetter BinaryCharGetter
		{
			get
			{
				return (this.binary == null) ? null : this.binary.Getter;
			}
			set
			{
				if (this.binary == null)
				{
					this.binary = new XmlReaderBinarySupport(this);
				}
				this.binary.Getter = value;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Xml.XmlReader" /> implements the binary content read methods.</summary>
		/// <returns>true if the binary content read methods are implemented; otherwise false.</returns>
		public virtual bool CanReadBinaryContent
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Xml.XmlReader" /> implements the <see cref="M:System.Xml.XmlReader.ReadValueChunk(System.Char[],System.Int32,System.Int32)" /> method. </summary>
		/// <returns>true if the <see cref="T:System.Xml.XmlReader" /> implements the <see cref="M:System.Xml.XmlReader.ReadValueChunk(System.Char[],System.Int32,System.Int32)" /> method; otherwise false.</returns>
		public virtual bool CanReadValueChunk
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether this reader can parse and resolve entities.</summary>
		/// <returns>true if the reader can parse and resolve entities; otherwise, false.</returns>
		public virtual bool CanResolveEntity
		{
			get
			{
				return false;
			}
		}

		/// <summary>When overridden in a derived class, gets the depth of the current node in the XML document.</summary>
		/// <returns>The depth of the current node in the XML document.</returns>
		public abstract int Depth { get; }

		/// <summary>When overridden in a derived class, gets a value indicating whether the reader is positioned at the end of the stream.</summary>
		/// <returns>true if the reader is positioned at the end of the stream; otherwise, false.</returns>
		public abstract bool EOF { get; }

		/// <summary>Gets a value indicating whether the current node has any attributes.</summary>
		/// <returns>true if the current node has attributes; otherwise, false.</returns>
		public virtual bool HasAttributes
		{
			get
			{
				return this.AttributeCount > 0;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current node can have a <see cref="P:System.Xml.XmlReader.Value" />.</summary>
		/// <returns>true if the node on which the reader is currently positioned can have a Value; otherwise, false. If false, the node has a value of String.Empty.</returns>
		public abstract bool HasValue { get; }

		/// <summary>When overridden in a derived class, gets a value indicating whether the current node is an empty element (for example, &lt;MyElement/&gt;).</summary>
		/// <returns>true if the current node is an element (<see cref="P:System.Xml.XmlReader.NodeType" /> equals XmlNodeType.Element) that ends with /&gt;; otherwise, false.</returns>
		public abstract bool IsEmptyElement { get; }

		/// <summary>When overridden in a derived class, gets a value indicating whether the current node is an attribute that was generated from the default value defined in the DTD or schema.</summary>
		/// <returns>true if the current node is an attribute whose value was generated from the default value defined in the DTD or schema; false if the attribute value was explicitly set.</returns>
		public virtual bool IsDefault
		{
			get
			{
				return false;
			}
		}

		/// <summary>When overridden in a derived class, gets the value of the attribute with the specified index.</summary>
		/// <returns>The value of the specified attribute.</returns>
		/// <param name="i">The index of the attribute. </param>
		public virtual string this[int i]
		{
			get
			{
				return this.GetAttribute(i);
			}
		}

		/// <summary>When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.Name" />.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found, null is returned.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public virtual string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}

		/// <summary>When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" />.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found, null is returned.</returns>
		/// <param name="name">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		public virtual string this[string name, string namespaceURI]
		{
			get
			{
				return this.GetAttribute(name, namespaceURI);
			}
		}

		/// <summary>When overridden in a derived class, gets the local name of the current node.</summary>
		/// <returns>The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.</returns>
		public abstract string LocalName { get; }

		/// <summary>When overridden in a derived class, gets the qualified name of the current node.</summary>
		/// <returns>The qualified name of the current node. For example, Name is bk:book for the element &lt;bk:book&gt;.The name returned is dependent on the <see cref="P:System.Xml.XmlReader.NodeType" /> of the node. The following node types return the listed values. All other node types return an empty string.Node type Name AttributeThe name of the attribute. DocumentTypeThe document type name. ElementThe tag name. EntityReferenceThe name of the entity referenced. ProcessingInstructionThe target of the processing instruction. XmlDeclarationThe literal string xml. </returns>
		public virtual string Name
		{
			get
			{
				return (this.Prefix.Length <= 0) ? this.LocalName : (this.Prefix + ":" + this.LocalName);
			}
		}

		/// <summary>When overridden in a derived class, gets the namespace URI (as defined in the W3C Namespace specification) of the node on which the reader is positioned.</summary>
		/// <returns>The namespace URI of the current node; otherwise an empty string.</returns>
		public abstract string NamespaceURI { get; }

		/// <summary>When overridden in a derived class, gets the <see cref="T:System.Xml.XmlNameTable" /> associated with this implementation.</summary>
		/// <returns>The XmlNameTable enabling you to get the atomized version of a string within the node.</returns>
		public abstract XmlNameTable NameTable { get; }

		/// <summary>When overridden in a derived class, gets the type of the current node.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlNodeType" /> values representing the type of the current node.</returns>
		public abstract XmlNodeType NodeType { get; }

		/// <summary>When overridden in a derived class, gets the namespace prefix associated with the current node.</summary>
		/// <returns>The namespace prefix associated with the current node.</returns>
		public abstract string Prefix { get; }

		/// <summary>When overridden in a derived class, gets the quotation mark character used to enclose the value of an attribute node.</summary>
		/// <returns>The quotation mark character (" or ') used to enclose the value of an attribute node.</returns>
		public virtual char QuoteChar
		{
			get
			{
				return '"';
			}
		}

		/// <summary>When overridden in a derived class, gets the state of the reader.</summary>
		/// <returns>One of the <see cref="T:System.Xml.ReadState" /> values.</returns>
		public abstract ReadState ReadState { get; }

		/// <summary>Gets the schema information that has been assigned to the current node as a result of schema validation.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.IXmlSchemaInfo" /> object containing the schema information for the current node. Schema information can be set on elements, attributes, or on text nodes with a non-null <see cref="P:System.Xml.XmlReader.ValueType" /> (typed values). If the current node is not one of the above node types, or if the XmlReader instance does not report schema information, this property returns null.If this property is called from an <see cref="T:System.Xml.XmlTextReader" /> or an <see cref="T:System.Xml.XmlValidatingReader" /> object, this property always returns null. These XmlReader implementations do not expose schema information through the SchemaInfo property.Note:If you have to get the post-schema-validation information set (PSVI) for an element, position the reader on the end tag of the element, rather than the start tag. You get the PSVI through the SchemaInfo property of a reader. The validating reader that is created through <see cref="Overload:System.Xml.XmlReader.Create" /> with the <see cref="P:System.Xml.XmlReaderSettings.ValidationType" /> property set to <see cref="F:System.Xml.ValidationType.Schema" /> has complete PSVI for an element only when the reader is positioned on the end tag of an element.</returns>
		public virtual IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlReaderSettings" /> object used to create this <see cref="T:System.Xml.XmlReader" /> instance.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlReaderSettings" /> object used to create this reader instance. If this reader was not created using the <see cref="Overload:System.Xml.XmlReader.Create" /> method, this property returns null.</returns>
		public virtual XmlReaderSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		/// <summary>When overridden in a derived class, gets the text value of the current node.</summary>
		/// <returns>The value returned depends on the <see cref="P:System.Xml.XmlReader.NodeType" /> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space between markup in a mixed content model. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
		public abstract string Value { get; }

		/// <summary>When overridden in a derived class, gets the current xml:lang scope.</summary>
		/// <returns>The current xml:lang scope.</returns>
		public virtual string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>When overridden in a derived class, gets the current xml:space scope.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlSpace" /> values. If no xml:space scope exists, this property defaults to XmlSpace.None.</returns>
		public virtual XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.None;
			}
		}

		/// <summary>When overridden in a derived class, changes the <see cref="P:System.Xml.XmlReader.ReadState" /> to Closed.</summary>
		public abstract void Close();

		private static XmlNameTable PopulateNameTable(XmlReaderSettings settings)
		{
			XmlNameTable xmlNameTable = settings.NameTable;
			if (xmlNameTable == null)
			{
				xmlNameTable = new NameTable();
			}
			return xmlNameTable;
		}

		private static XmlParserContext PopulateParserContext(XmlReaderSettings settings, string baseUri)
		{
			XmlNameTable xmlNameTable = XmlReader.PopulateNameTable(settings);
			return new XmlParserContext(xmlNameTable, new XmlNamespaceManager(xmlNameTable), null, null, null, null, baseUri, null, XmlSpace.None, null);
		}

		private static XmlNodeType GetNodeType(XmlReaderSettings settings)
		{
			ConformanceLevel conformanceLevel = (settings == null) ? ConformanceLevel.Auto : settings.ConformanceLevel;
			return (conformanceLevel != ConformanceLevel.Fragment) ? XmlNodeType.Document : XmlNodeType.Element;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified stream.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object used to read the data contained in the stream. </returns>
		/// <param name="input">The stream containing the XML data. The <see cref="T:System.Xml.XmlReader" /> scans the first bytes of the stream looking for a byte order mark or other sign of encoding. When encoding is determined, the encoding is used to continue reading the stream, and processing continues parsing the input as a stream of (Unicode) characters.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">The <see cref="T:System.Xml.XmlReader" /> does not have sufficient permissions to access the location of the XML data.</exception>
		public static XmlReader Create(Stream stream)
		{
			return XmlReader.Create(stream, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance with specified URI.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read the XML data. </returns>
		/// <param name="inputUri">The URI for the file containing the XML data. The <see cref="T:System.Xml.XmlUrlResolver" /> class is used to convert the path to a canonical data representation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> value is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">The <see cref="T:System.Xml.XmlReader" /> does not have sufficient permissions to access the location of the XML data.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file identified by the URI does not exist.</exception>
		/// <exception cref="T:System.UriFormatException">The URI format is not correct.</exception>
		public static XmlReader Create(string url)
		{
			return XmlReader.Create(url, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance with the specified <see cref="T:System.IO.TextReader" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read the XML data.</returns>
		/// <param name="input">The <see cref="T:System.IO.TextReader" /> from which to read the XML data. Because a <see cref="T:System.IO.TextReader" /> returns a stream of Unicode characters, the encoding specified in the XML declaration is not used by the <see cref="T:System.Xml.XmlReader" /> to decode the data stream.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		public static XmlReader Create(TextReader reader)
		{
			return XmlReader.Create(reader, null);
		}

		/// <summary>Creates a new instance with the specified URI and <see cref="T:System.Xml.XmlReaderSettings" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="inputUri">The URI for the file containing the XML data. The <see cref="T:System.Xml.XmlResolver" /> object on the <see cref="T:System.Xml.XmlReaderSettings" /> object is used to convert the path to a canonical data representation. If <see cref="P:System.Xml.XmlReaderSettings.XmlResolver" /> is null, a new <see cref="T:System.Xml.XmlUrlResolver" /> object is used.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> value is null.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified by the URI cannot be found.</exception>
		/// <exception cref="T:System.UriFormatException">The URI format is not correct.</exception>
		public static XmlReader Create(string url, XmlReaderSettings settings)
		{
			return XmlReader.Create(url, settings, null);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance with the specified stream and <see cref="T:System.Xml.XmlReaderSettings" /> object.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read the XML data.</returns>
		/// <param name="input">The stream containing the XML data. The <see cref="T:System.Xml.XmlReader" /> scans the first bytes of the stream looking for a byte order mark or other sign of encoding. When encoding is determined, the encoding is used to continue reading the stream, and processing continues parsing the input as a stream of (Unicode) characters.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		public static XmlReader Create(Stream stream, XmlReaderSettings settings)
		{
			return XmlReader.Create(stream, settings, string.Empty);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified <see cref="T:System.IO.TextReader" /> and <see cref="T:System.Xml.XmlReaderSettings" /> objects.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="input">The <see cref="T:System.IO.TextReader" /> from which to read the XML data. Because a <see cref="T:System.IO.TextReader" /> returns a stream of Unicode characters, the encoding specified in the XML declaration is not used by the <see cref="T:System.Xml.XmlReader" /> to decode the data stream</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" />. This value can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		public static XmlReader Create(TextReader reader, XmlReaderSettings settings)
		{
			return XmlReader.Create(reader, settings, string.Empty);
		}

		private static XmlReaderSettings PopulateSettings(XmlReaderSettings src)
		{
			if (src == null)
			{
				return new XmlReaderSettings();
			}
			return src.Clone();
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified stream, base URI, and <see cref="T:System.Xml.XmlReaderSettings" /> object.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="input">The stream containing the XML data. The <see cref="T:System.Xml.XmlReader" /> scans the first bytes of the stream looking for a byte order mark or other sign of encoding. When encoding is determined, the encoding is used to continue reading the stream, and processing continues parsing the input as a stream of (Unicode) characters.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <param name="baseUri">The base URI for the entity or document being read. This value can be null.Security Note   The base URI is used to resolve the relative URI of the XML document. Do not use a base URI from an untrusted source.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		public static XmlReader Create(Stream stream, XmlReaderSettings settings, string baseUri)
		{
			settings = XmlReader.PopulateSettings(settings);
			return XmlReader.Create(stream, settings, XmlReader.PopulateParserContext(settings, baseUri));
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified <see cref="T:System.IO.TextReader" />, <see cref="T:System.Xml.XmlReaderSettings" />, and base URI.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="input">The <see cref="T:System.IO.TextReader" /> from which to read the XML data. Because a <see cref="T:System.IO.TextReader" /> returns a stream of Unicode characters, the encoding specified in the XML declaration is not used by the <see cref="T:System.Xml.XmlReader" /> to decode the data stream.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <param name="baseUri">The base URI for the entity or document being read. This value can be null.Security Note   The base URI is used to resolve the relative URI of the XML document. Do not use a base URI from an untrusted source.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		public static XmlReader Create(TextReader reader, XmlReaderSettings settings, string baseUri)
		{
			settings = XmlReader.PopulateSettings(settings);
			return XmlReader.Create(reader, settings, XmlReader.PopulateParserContext(settings, baseUri));
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance with the specified <see cref="T:System.Xml.XmlReader" /> and <see cref="T:System.Xml.XmlReaderSettings" /> objects.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object that is wrapped around the specified <see cref="T:System.Xml.XmlReader" /> object.</returns>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> object that you wish to use as the underlying reader.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance.The conformance level of the <see cref="T:System.Xml.XmlReaderSettings" /> object must either match the conformance level of the underlying reader, or it must be set to <see cref="F:System.Xml.ConformanceLevel.Auto" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="reader" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">If the <see cref="T:System.Xml.XmlReaderSettings" /> object specifies a conformance level that is not consistent with conformance level of the underlying reader.-or-The underlying <see cref="T:System.Xml.XmlReader" /> is in an <see cref="F:System.Xml.ReadState.Error" /> or <see cref="F:System.Xml.ReadState.Closed" /> state.</exception>
		public static XmlReader Create(XmlReader reader, XmlReaderSettings settings)
		{
			settings = XmlReader.PopulateSettings(settings);
			XmlReader xmlReader = XmlReader.CreateFilteredXmlReader(reader, settings);
			xmlReader.settings = settings;
			return xmlReader;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified URI, <see cref="T:System.Xml.XmlReaderSettings" />, and <see cref="T:System.Xml.XmlParserContext" /> objects.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="inputUri">The URI for the file containing the XML data. The <see cref="T:System.Xml.XmlResolver" /> object on the <see cref="T:System.Xml.XmlReaderSettings" /> object is used to convert the path to a canonical data representation. If <see cref="P:System.Xml.XmlReaderSettings.XmlResolver" /> is null, a new <see cref="T:System.Xml.XmlUrlResolver" /> object is used.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <param name="inputContext">The <see cref="T:System.Xml.XmlParserContext" /> object that provides the context information required to parse the XML fragment. The context information can include the <see cref="T:System.Xml.XmlNameTable" /> to use, encoding, namespace scope, the current xml:lang and xml:space scope, base URI, and document type definition. This value can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">The inputUri value is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">The <see cref="T:System.Xml.XmlReader" /> does not have sufficient permissions to access the location of the XML data.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Xml.XmlReaderSettings.NameTable" />  and <see cref="P:System.Xml.XmlParserContext.NameTable" /> properties both contain values. (Only one of these NameTable properties can be set and used).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified by the URI cannot be found.</exception>
		/// <exception cref="T:System.UriFormatException">The URI format is not correct.</exception>
		public static XmlReader Create(string url, XmlReaderSettings settings, XmlParserContext context)
		{
			settings = XmlReader.PopulateSettings(settings);
			bool closeInput = settings.CloseInput;
			XmlReader result;
			try
			{
				settings.CloseInput = true;
				if (context == null)
				{
					context = XmlReader.PopulateParserContext(settings, url);
				}
				XmlTextReader reader = new XmlTextReader(false, settings.XmlResolver, url, XmlReader.GetNodeType(settings), context);
				XmlReader xmlReader = XmlReader.CreateCustomizedTextReader(reader, settings);
				result = xmlReader;
			}
			finally
			{
				settings.CloseInput = closeInput;
			}
			return result;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified stream, <see cref="T:System.Xml.XmlReaderSettings" />, and <see cref="T:System.Xml.XmlParserContext" /> objects.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="input">The stream containing the XML data. The <see cref="T:System.Xml.XmlReader" /> scans the first bytes of the stream looking for a byte order mark or other sign of encoding. When encoding is determined, the encoding is used to continue reading the stream, and processing continues parsing the input as a stream of (Unicode) characters.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <param name="inputContext">The <see cref="T:System.Xml.XmlParserContext" /> object that provides the context information required to parse the XML fragment. The context information can include the <see cref="T:System.Xml.XmlNameTable" /> to use, encoding, namespace scope, the current xml:lang and xml:space scope, base URI, and document type definition. This value can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		public static XmlReader Create(Stream stream, XmlReaderSettings settings, XmlParserContext context)
		{
			settings = XmlReader.PopulateSettings(settings);
			if (context == null)
			{
				context = XmlReader.PopulateParserContext(settings, string.Empty);
			}
			return XmlReader.CreateCustomizedTextReader(new XmlTextReader(stream, XmlReader.GetNodeType(settings), context), settings);
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlReader" /> instance using the specified <see cref="T:System.IO.TextReader" />, <see cref="T:System.Xml.XmlReaderSettings" />, and <see cref="T:System.Xml.XmlParserContext" /> objects.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object to read XML data.</returns>
		/// <param name="input">The <see cref="T:System.IO.TextReader" /> from which to read the XML data. Because a <see cref="T:System.IO.TextReader" /> returns a stream of Unicode characters, the encoding specified in the XML declaration is not used by the <see cref="T:System.Xml.XmlReader" /> to decode the data stream.</param>
		/// <param name="settings">The <see cref="T:System.Xml.XmlReaderSettings" /> object used to configure the new <see cref="T:System.Xml.XmlReader" /> instance. This value can be null.</param>
		/// <param name="inputContext">The <see cref="T:System.Xml.XmlParserContext" /> object that provides the context information required to parse the XML fragment. The context information can include the <see cref="T:System.Xml.XmlNameTable" /> to use, encoding, namespace scope, the current xml:lang and xml:space scope, base URI, and document type definition. This value can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> value is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Xml.XmlReaderSettings.NameTable" />  and <see cref="P:System.Xml.XmlParserContext.NameTable" /> properties both contain values. (Only one of these NameTable properties can be set and used).</exception>
		public static XmlReader Create(TextReader reader, XmlReaderSettings settings, XmlParserContext context)
		{
			settings = XmlReader.PopulateSettings(settings);
			if (context == null)
			{
				context = XmlReader.PopulateParserContext(settings, string.Empty);
			}
			return XmlReader.CreateCustomizedTextReader(new XmlTextReader(context.BaseURI, reader, XmlReader.GetNodeType(settings), context), settings);
		}

		private static XmlReader CreateCustomizedTextReader(XmlTextReader reader, XmlReaderSettings settings)
		{
			reader.XmlResolver = settings.XmlResolver;
			reader.Normalization = true;
			reader.EntityHandling = EntityHandling.ExpandEntities;
			if (settings.ProhibitDtd)
			{
				reader.ProhibitDtd = true;
			}
			if (!settings.CheckCharacters)
			{
				reader.CharacterChecking = false;
			}
			reader.CloseInput = settings.CloseInput;
			reader.Conformance = settings.ConformanceLevel;
			reader.AdjustLineInfoOffset(settings.LineNumberOffset, settings.LinePositionOffset);
			if (settings.NameTable != null)
			{
				reader.SetNameTable(settings.NameTable);
			}
			XmlReader xmlReader = XmlReader.CreateFilteredXmlReader(reader, settings);
			xmlReader.settings = settings;
			return xmlReader;
		}

		private static XmlReader CreateFilteredXmlReader(XmlReader reader, XmlReaderSettings settings)
		{
			ConformanceLevel conformanceLevel;
			if (reader is XmlTextReader)
			{
				conformanceLevel = ((XmlTextReader)reader).Conformance;
			}
			else if (reader.Settings != null)
			{
				conformanceLevel = reader.Settings.ConformanceLevel;
			}
			else
			{
				conformanceLevel = settings.ConformanceLevel;
			}
			if (settings.ConformanceLevel != ConformanceLevel.Auto && conformanceLevel != settings.ConformanceLevel)
			{
				throw new InvalidOperationException(string.Format("ConformanceLevel cannot be overwritten by a wrapping XmlReader. The source reader has {0}, while {1} is specified.", conformanceLevel, settings.ConformanceLevel));
			}
			settings.ConformanceLevel = conformanceLevel;
			reader = XmlReader.CreateValidatingXmlReader(reader, settings);
			if (settings.IgnoreComments || settings.IgnoreProcessingInstructions || settings.IgnoreWhitespace)
			{
				return new XmlFilterReader(reader, settings);
			}
			reader.settings = settings;
			return reader;
		}

		private static XmlReader CreateValidatingXmlReader(XmlReader reader, XmlReaderSettings settings)
		{
			switch (settings.ValidationType)
			{
			case ValidationType.DTD:
			{
				XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(reader);
				xmlValidatingReader.XmlResolver = settings.XmlResolver;
				xmlValidatingReader.ValidationType = ValidationType.DTD;
				if ((settings.ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) == XmlSchemaValidationFlags.None)
				{
					throw new NotImplementedException();
				}
				return (xmlValidatingReader == null) ? reader : xmlValidatingReader;
			}
			default:
				return reader;
			case ValidationType.Schema:
				return new XmlSchemaValidatingReader(reader, settings);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Xml.XmlReader" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.ReadState != ReadState.Closed)
			{
				this.Close();
			}
		}

		/// <summary>When overridden in a derived class, gets the value of the attribute with the specified index.</summary>
		/// <returns>The value of the specified attribute. This method does not move the reader.</returns>
		/// <param name="i">The index of the attribute. The index is zero-based. (The first attribute has index 0.) </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="i" /> is out of range. It must be non-negative and less than the size of the attribute collection.</exception>
		public abstract string GetAttribute(int i);

		/// <summary>When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.Name" />.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found or the value is String.Empty, null is returned.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		public abstract string GetAttribute(string name);

		/// <summary>When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" />.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found or the value is String.Empty, null is returned. This method does not move the reader.</returns>
		/// <param name="name">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		public abstract string GetAttribute(string localName, string namespaceName);

		/// <summary>Gets a value indicating whether the string argument is a valid XML name.</summary>
		/// <returns>true if the name is valid; otherwise, false.</returns>
		/// <param name="str">The name to validate. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="str" /> value is null.</exception>
		public static bool IsName(string s)
		{
			return s != null && XmlChar.IsName(s);
		}

		/// <summary>Gets a value indicating whether or not the string argument is a valid XML name token.</summary>
		/// <returns>true if it is a valid name token; otherwise false.</returns>
		/// <param name="str">The name token to validate. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="str" /> value is null.</exception>
		public static bool IsNameToken(string s)
		{
			return s != null && XmlChar.IsNmToken(s);
		}

		/// <summary>Calls <see cref="M:System.Xml.XmlReader.MoveToContent" /> and tests if the current content node is a start tag or empty element tag.</summary>
		/// <returns>true if <see cref="M:System.Xml.XmlReader.MoveToContent" /> finds a start tag or empty element tag; false if a node type other than XmlNodeType.Element was found.</returns>
		/// <exception cref="T:System.Xml.XmlException">Incorrect XML is encountered in the input stream. </exception>
		public virtual bool IsStartElement()
		{
			return this.MoveToContent() == XmlNodeType.Element;
		}

		/// <summary>Calls <see cref="M:System.Xml.XmlReader.MoveToContent" /> and tests if the current content node is a start tag or empty element tag and if the <see cref="P:System.Xml.XmlReader.Name" /> property of the element found matches the given argument.</summary>
		/// <returns>true if the resulting node is an element and the Name property matches the specified string. false if a node type other than XmlNodeType.Element was found or if the element Name property does not match the specified string.</returns>
		/// <param name="name">The string matched against the Name property of the element found. </param>
		/// <exception cref="T:System.Xml.XmlException">Incorrect XML is encountered in the input stream. </exception>
		public virtual bool IsStartElement(string name)
		{
			return this.IsStartElement() && this.Name == name;
		}

		/// <summary>Calls <see cref="M:System.Xml.XmlReader.MoveToContent" /> and tests if the current content node is a start tag or empty element tag and if the <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" /> properties of the element found match the given strings.</summary>
		/// <returns>true if the resulting node is an element. false if a node type other than XmlNodeType.Element was found or if the LocalName and NamespaceURI properties of the element do not match the specified strings.</returns>
		/// <param name="localname">The string to match against the LocalName property of the element found. </param>
		/// <param name="ns">The string to match against the NamespaceURI property of the element found. </param>
		/// <exception cref="T:System.Xml.XmlException">Incorrect XML is encountered in the input stream. </exception>
		public virtual bool IsStartElement(string localName, string namespaceName)
		{
			return this.IsStartElement() && this.LocalName == localName && this.NamespaceURI == namespaceName;
		}

		/// <summary>When overridden in a derived class, resolves a namespace prefix in the current element's scope.</summary>
		/// <returns>The namespace URI to which the prefix maps or null if no matching prefix is found.</returns>
		/// <param name="prefix">The prefix whose namespace URI you want to resolve. To match the default namespace, pass an empty string. </param>
		public abstract string LookupNamespace(string prefix);

		/// <summary>When overridden in a derived class, moves to the attribute with the specified index.</summary>
		/// <param name="i">The index of the attribute. </param>
		public virtual void MoveToAttribute(int i)
		{
			if (i >= this.AttributeCount)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.MoveToFirstAttribute();
			for (int j = 0; j < i; j++)
			{
				this.MoveToNextAttribute();
			}
		}

		/// <summary>When overridden in a derived class, moves to the attribute with the specified <see cref="P:System.Xml.XmlReader.Name" />.</summary>
		/// <returns>true if the attribute is found; otherwise, false. If false, the reader's position does not change.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public abstract bool MoveToAttribute(string name);

		/// <summary>When overridden in a derived class, moves to the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" />.</summary>
		/// <returns>true if the attribute is found; otherwise, false. If false, the reader's position does not change.</returns>
		/// <param name="name">The local name of the attribute. </param>
		/// <param name="ns">The namespace URI of the attribute. </param>
		public abstract bool MoveToAttribute(string localName, string namespaceName);

		private bool IsContent(XmlNodeType nodeType)
		{
			switch (nodeType)
			{
			case XmlNodeType.Element:
				return true;
			default:
				return nodeType == XmlNodeType.EndElement || nodeType == XmlNodeType.EndEntity;
			case XmlNodeType.Text:
				return true;
			case XmlNodeType.CDATA:
				return true;
			case XmlNodeType.EntityReference:
				return true;
			}
		}

		/// <summary>Checks whether the current node is a content (non-white space text, CDATA, Element, EndElement, EntityReference, or EndEntity) node. If the node is not a content node, the reader skips ahead to the next content node or end of file. It skips over nodes of the following type: ProcessingInstruction, DocumentType, Comment, Whitespace, or SignificantWhitespace.</summary>
		/// <returns>The <see cref="P:System.Xml.XmlReader.NodeType" /> of the current node found by the method or XmlNodeType.None if the reader has reached the end of the input stream.</returns>
		/// <exception cref="T:System.Xml.XmlException">Incorrect XML encountered in the input stream. </exception>
		public virtual XmlNodeType MoveToContent()
		{
			ReadState readState = this.ReadState;
			if (readState != ReadState.Initial && readState != ReadState.Interactive)
			{
				return this.NodeType;
			}
			if (this.NodeType == XmlNodeType.Attribute)
			{
				this.MoveToElement();
			}
			while (!this.IsContent(this.NodeType))
			{
				this.Read();
				if (this.EOF)
				{
					return XmlNodeType.None;
				}
			}
			return this.NodeType;
		}

		/// <summary>When overridden in a derived class, moves to the element that contains the current attribute node.</summary>
		/// <returns>true if the reader is positioned on an attribute (the reader moves to the element that owns the attribute); false if the reader is not positioned on an attribute (the position of the reader does not change).</returns>
		public abstract bool MoveToElement();

		/// <summary>When overridden in a derived class, moves to the first attribute.</summary>
		/// <returns>true if an attribute exists (the reader moves to the first attribute); otherwise, false (the position of the reader does not change).</returns>
		public abstract bool MoveToFirstAttribute();

		/// <summary>When overridden in a derived class, moves to the next attribute.</summary>
		/// <returns>true if there is a next attribute; false if there are no more attributes.</returns>
		public abstract bool MoveToNextAttribute();

		/// <summary>When overridden in a derived class, reads the next node from the stream.</summary>
		/// <returns>true if the next node was read successfully; false if there are no more nodes to read.</returns>
		/// <exception cref="T:System.Xml.XmlException">An error occurred while parsing the XML. </exception>
		public abstract bool Read();

		/// <summary>When overridden in a derived class, parses the attribute value into one or more Text, EntityReference, or EndEntity nodes.</summary>
		/// <returns>true if there are nodes to return.false if the reader is not positioned on an attribute node when the initial call is made or if all the attribute values have been read.An empty attribute, such as, misc="", returns true with a single node with a value of String.Empty.</returns>
		public abstract bool ReadAttributeValue();

		/// <summary>Reads a text-only element.</summary>
		/// <returns>The text contained in the element that was read. An empty string if the element is empty (&lt;item&gt;&lt;/item&gt; or &lt;item/&gt;).</returns>
		/// <exception cref="T:System.Xml.XmlException">The next content node is not a start tag; or the element found does not contain a simple text value. </exception>
		public virtual string ReadElementString()
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			string result = string.Empty;
			if (!this.IsEmptyElement)
			{
				this.Read();
				result = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					string message2 = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
					throw this.XmlError(message2);
				}
			}
			this.Read();
			return result;
		}

		/// <summary>Checks that the <see cref="P:System.Xml.XmlReader.Name" /> property of the element found matches the given string before reading a text-only element.</summary>
		/// <returns>The text contained in the element that was read. An empty string if the element is empty (&lt;item&gt;&lt;/item&gt; or &lt;item/&gt;).</returns>
		/// <param name="name">The name to check. </param>
		/// <exception cref="T:System.Xml.XmlException">If the next content node is not a start tag; if the element Name does not match the given argument; or if the element found does not contain a simple text value. </exception>
		public virtual string ReadElementString(string name)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			if (name != this.Name)
			{
				string message2 = string.Format("The {0} tag from namespace {1} is expected.", this.Name, this.NamespaceURI);
				throw this.XmlError(message2);
			}
			string result = string.Empty;
			if (!this.IsEmptyElement)
			{
				this.Read();
				result = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					string message3 = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
					throw this.XmlError(message3);
				}
			}
			this.Read();
			return result;
		}

		/// <summary>Checks that the <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" /> properties of the element found matches the given strings before reading a text-only element.</summary>
		/// <returns>The text contained in the element that was read. An empty string if the element is empty (&lt;item&gt;&lt;/item&gt; or &lt;item/&gt;).</returns>
		/// <param name="localname">The local name to check. </param>
		/// <param name="ns">The namespace URI to check. </param>
		/// <exception cref="T:System.Xml.XmlException">If the next content node is not a start tag; if the element LocalName or NamespaceURI do not match the given arguments; or if the element found does not contain a simple text value. </exception>
		public virtual string ReadElementString(string localName, string namespaceName)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			if (localName != this.LocalName || this.NamespaceURI != namespaceName)
			{
				string message2 = string.Format("The {0} tag from namespace {1} is expected.", this.LocalName, this.NamespaceURI);
				throw this.XmlError(message2);
			}
			string result = string.Empty;
			if (!this.IsEmptyElement)
			{
				this.Read();
				result = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					string message3 = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
					throw this.XmlError(message3);
				}
			}
			this.Read();
			return result;
		}

		/// <summary>Checks that the current content node is an end tag and advances the reader to the next node.</summary>
		/// <exception cref="T:System.Xml.XmlException">The current node is not an end tag or if incorrect XML is encountered in the input stream. </exception>
		public virtual void ReadEndElement()
		{
			if (this.MoveToContent() != XmlNodeType.EndElement)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			this.Read();
		}

		/// <summary>When overridden in a derived class, reads all the content, including markup, as a string.</summary>
		/// <returns>All the XML content, including markup, in the current node. If the current node has no children, an empty string is returned.If the current node is neither an element nor attribute, an empty string is returned.</returns>
		/// <exception cref="T:System.Xml.XmlException">The XML was not well-formed, or an error occurred while parsing the XML. </exception>
		public virtual string ReadInnerXml()
		{
			if (this.ReadState != ReadState.Interactive || this.NodeType == XmlNodeType.EndElement)
			{
				return string.Empty;
			}
			if (this.IsEmptyElement)
			{
				this.Read();
				return string.Empty;
			}
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			if (this.NodeType == XmlNodeType.Element)
			{
				int i = this.Depth;
				this.Read();
				while (i < this.Depth)
				{
					if (this.ReadState != ReadState.Interactive)
					{
						throw this.XmlError("Unexpected end of the XML reader.");
					}
					xmlTextWriter.WriteNode(this, false);
				}
				this.Read();
			}
			else
			{
				xmlTextWriter.WriteNode(this, false);
			}
			return stringWriter.ToString();
		}

		/// <summary>When overridden in a derived class, reads the content, including markup, representing this node and all its children.</summary>
		/// <returns>If the reader is positioned on an element or an attribute node, this method returns all the XML content, including markup, of the current node and all its children; otherwise, it returns an empty string.</returns>
		/// <exception cref="T:System.Xml.XmlException">The XML was not well-formed, or an error occurred while parsing the XML. </exception>
		public virtual string ReadOuterXml()
		{
			if (this.ReadState != ReadState.Interactive || this.NodeType == XmlNodeType.EndElement)
			{
				return string.Empty;
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType != XmlNodeType.Element && nodeType != XmlNodeType.Attribute)
			{
				this.Skip();
				return string.Empty;
			}
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteNode(this, false);
			return stringWriter.ToString();
		}

		/// <summary>Checks that the current node is an element and advances the reader to the next node.</summary>
		/// <exception cref="T:System.Xml.XmlException">
		///   <see cref="M:System.Xml.XmlReader.IsStartElement" /> returns false. </exception>
		public virtual void ReadStartElement()
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			this.Read();
		}

		/// <summary>Checks that the current content node is an element with the given <see cref="P:System.Xml.XmlReader.Name" /> and advances the reader to the next node.</summary>
		/// <param name="name">The qualified name of the element. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <see cref="M:System.Xml.XmlReader.IsStartElement" /> returns false or if the <see cref="P:System.Xml.XmlReader.Name" /> of the element does not match the given <paramref name="name" />. </exception>
		public virtual void ReadStartElement(string name)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			if (name != this.Name)
			{
				string message2 = string.Format("The {0} tag from namespace {1} is expected.", this.Name, this.NamespaceURI);
				throw this.XmlError(message2);
			}
			this.Read();
		}

		/// <summary>Checks that the current content node is an element with the given <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" /> and advances the reader to the next node.</summary>
		/// <param name="localname">The local name of the element. </param>
		/// <param name="ns">The namespace URI of the element. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <see cref="M:System.Xml.XmlReader.IsStartElement" /> returns false, or the <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" /> properties of the element found do not match the given arguments. </exception>
		public virtual void ReadStartElement(string localName, string namespaceName)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				string message = string.Format("'{0}' is an invalid node type.", this.NodeType.ToString());
				throw this.XmlError(message);
			}
			if (localName != this.LocalName || this.NamespaceURI != namespaceName)
			{
				string message2 = string.Format("Expecting {0} tag from namespace {1}, got {2} and {3} instead", new object[]
				{
					localName,
					namespaceName,
					this.LocalName,
					this.NamespaceURI
				});
				throw this.XmlError(message2);
			}
			this.Read();
		}

		/// <summary>When overridden in a derived class, reads the contents of an element or text node as a string.</summary>
		/// <returns>The contents of the element or an empty string.</returns>
		/// <exception cref="T:System.Xml.XmlException">An error occurred while parsing the XML. </exception>
		public virtual string ReadString()
		{
			if (this.readStringBuffer == null)
			{
				this.readStringBuffer = new StringBuilder();
			}
			this.readStringBuffer.Length = 0;
			this.MoveToElement();
			XmlNodeType nodeType = this.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				if (this.IsEmptyElement)
				{
					return string.Empty;
				}
				for (;;)
				{
					this.Read();
					XmlNodeType nodeType2 = this.NodeType;
					if (nodeType2 != XmlNodeType.Text && nodeType2 != XmlNodeType.CDATA && nodeType2 != XmlNodeType.Whitespace && nodeType2 != XmlNodeType.SignificantWhitespace)
					{
						break;
					}
					this.readStringBuffer.Append(this.Value);
				}
				goto IL_122;
			default:
				if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
				{
					return string.Empty;
				}
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			}
			for (;;)
			{
				XmlNodeType nodeType2 = this.NodeType;
				if (nodeType2 != XmlNodeType.Text && nodeType2 != XmlNodeType.CDATA && nodeType2 != XmlNodeType.Whitespace && nodeType2 != XmlNodeType.SignificantWhitespace)
				{
					break;
				}
				this.readStringBuffer.Append(this.Value);
				this.Read();
			}
			IL_122:
			string result = this.readStringBuffer.ToString();
			this.readStringBuffer.Length = 0;
			return result;
		}

		/// <summary>Gets The Common Language Runtime (CLR) type for the current node.</summary>
		/// <returns>The CLR type that corresponds to the typed value of the node. The default is System.String.</returns>
		public virtual Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		/// <summary>Advances the <see cref="T:System.Xml.XmlReader" /> to the next descendant element with the specified qualified name.</summary>
		/// <returns>true if a matching descendant element is found; otherwise false. If a matching child element is not found, the <see cref="T:System.Xml.XmlReader" /> is positioned on the end tag (<see cref="P:System.Xml.XmlReader.NodeType" /> is XmlNodeType.EndElement) of the element.If the <see cref="T:System.Xml.XmlReader" /> is not positioned on an element when <see cref="M:System.Xml.XmlReader.ReadToDescendant(System.String)" /> was called, this method returns false and the position of the <see cref="T:System.Xml.XmlReader" /> is not changed.</returns>
		/// <param name="name">The qualified name of the element you wish to move to.</param>
		public virtual bool ReadToDescendant(string name)
		{
			if (this.ReadState == ReadState.Initial)
			{
				this.MoveToContent();
				if (this.IsStartElement(name))
				{
					return true;
				}
			}
			if (this.NodeType != XmlNodeType.Element || this.IsEmptyElement)
			{
				return false;
			}
			int i = this.Depth;
			this.Read();
			while (i < this.Depth)
			{
				if (this.NodeType == XmlNodeType.Element && name == this.Name)
				{
					return true;
				}
				this.Read();
			}
			return false;
		}

		/// <summary>Advances the <see cref="T:System.Xml.XmlReader" /> to the next descendant element with the specified local name and namespace URI.</summary>
		/// <returns>true if a matching descendant element is found; otherwise false. If a matching child element is not found, the <see cref="T:System.Xml.XmlReader" /> is positioned on the end tag (<see cref="P:System.Xml.XmlReader.NodeType" /> is XmlNodeType.EndElement) of the element.If the <see cref="T:System.Xml.XmlReader" /> is not positioned on an element when <see cref="M:System.Xml.XmlReader.ReadToDescendant(System.String,System.String)" /> was called, this method returns false and the position of the <see cref="T:System.Xml.XmlReader" /> is not changed.</returns>
		/// <param name="localName">The local name of the element you wish to move to.</param>
		/// <param name="namespaceURI">The namespace URI of the element you wish to move to.</param>
		public virtual bool ReadToDescendant(string localName, string namespaceURI)
		{
			if (this.ReadState == ReadState.Initial)
			{
				this.MoveToContent();
				if (this.IsStartElement(localName, namespaceURI))
				{
					return true;
				}
			}
			if (this.NodeType != XmlNodeType.Element || this.IsEmptyElement)
			{
				return false;
			}
			int i = this.Depth;
			this.Read();
			while (i < this.Depth)
			{
				if (this.NodeType == XmlNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
				{
					return true;
				}
				this.Read();
			}
			return false;
		}

		/// <summary>Reads until an element with the specified qualified name is found.</summary>
		/// <returns>true if a matching element is found; otherwise false and the <see cref="T:System.Xml.XmlReader" /> is in an end of file state.</returns>
		/// <param name="name">The qualified name of the element.</param>
		public virtual bool ReadToFollowing(string name)
		{
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element && name == this.Name)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Reads until an element with the specified local name and namespace URI is found.</summary>
		/// <returns>true if a matching element is found; otherwise false and the <see cref="T:System.Xml.XmlReader" /> is in an end of file state.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		public virtual bool ReadToFollowing(string localName, string namespaceURI)
		{
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Advances the XmlReader to the next sibling element with the specified qualified name.</summary>
		/// <returns>true if a matching sibling element is found; otherwise false. If a matching sibling element is not found, the XmlReader is positioned on the end tag (<see cref="P:System.Xml.XmlReader.NodeType" /> is XmlNodeType.EndElement) of the parent element.</returns>
		/// <param name="name">The qualified name of the sibling element you wish to move to.</param>
		public virtual bool ReadToNextSibling(string name)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return false;
			}
			int depth = this.Depth;
			this.Skip();
			while (!this.EOF && depth <= this.Depth)
			{
				if (this.NodeType == XmlNodeType.Element && name == this.Name)
				{
					return true;
				}
				this.Skip();
			}
			return false;
		}

		/// <summary>Advances the XmlReader to the next sibling element with the specified local name and namespace URI.</summary>
		/// <returns>true if a matching sibling element is found; otherwise false. If a matching sibling element is not found, the XmlReader is positioned on the end tag (<see cref="P:System.Xml.XmlReader.NodeType" /> is XmlNodeType.EndElement) of the parent element.</returns>
		/// <param name="localName">The local name of the sibling element you wish to move to.</param>
		/// <param name="namespaceURI">The namespace URI of the sibling element you wish to move to</param>
		public virtual bool ReadToNextSibling(string localName, string namespaceURI)
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return false;
			}
			int depth = this.Depth;
			this.Skip();
			while (!this.EOF && depth <= this.Depth)
			{
				if (this.NodeType == XmlNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
				{
					return true;
				}
				this.Skip();
			}
			return false;
		}

		/// <summary>Returns a new XmlReader instance that can be used to read the current node, and all its descendants.</summary>
		/// <returns>A new XmlReader instance set to ReadState.Initial. A call to the <see cref="M:System.Xml.XmlReader.Read" /> method positions the new XmlReader on the node that was current before the call to ReadSubtree method.</returns>
		/// <exception cref="T:System.InvalidOperationException">The XmlReader is not positioned on an element when this method is called.</exception>
		public virtual XmlReader ReadSubtree()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new InvalidOperationException(string.Format("ReadSubtree() can be invoked only when the reader is positioned on an element. Current node is {0}. {1}", this.NodeType, this.GetLocation()));
			}
			return new SubtreeXmlReader(this);
		}

		private string ReadContentString()
		{
			if (this.NodeType == XmlNodeType.Attribute || (this.NodeType != XmlNodeType.Element && this.HasAttributes))
			{
				return this.Value;
			}
			return this.ReadContentString(true);
		}

		private string ReadContentString(bool isText)
		{
			if (isText)
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					throw new InvalidOperationException(string.Format("Node type {0} is not supported in this operation.{1}", this.NodeType, this.GetLocation()));
				default:
					if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
					{
						return string.Empty;
					}
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					break;
				}
			}
			string text = string.Empty;
			for (;;)
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					goto IL_A5;
				default:
					switch (nodeType)
					{
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						goto IL_BB;
					case XmlNodeType.EndElement:
						return text;
					}
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					goto IL_BB;
				}
				IL_CD:
				if (!this.Read())
				{
					goto Block_6;
				}
				continue;
				IL_BB:
				text += this.Value;
				goto IL_CD;
			}
			IL_A5:
			if (isText)
			{
				return text;
			}
			throw this.XmlError("Child element is not expected in this operation.");
			Block_6:
			throw this.XmlError("Unexpected end of document.");
		}

		private string GetLocation()
		{
			IXmlLineInfo xmlLineInfo = this as IXmlLineInfo;
			return (xmlLineInfo == null || !xmlLineInfo.HasLineInfo()) ? string.Empty : string.Format(" {0} (line {1}, column {2})", this.BaseURI, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
		}

		/// <summary>Reads the current element and returns the contents as an <see cref="T:System.Object" />.</summary>
		/// <returns>A boxed common language runtime (CLR) object of the most appropriate type. The <see cref="P:System.Xml.XmlReader.ValueType" /> property determines the appropriate CLR type. If the content is typed as a list type, this method returns an array of boxed objects of the appropriate type.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		[MonoTODO]
		public virtual object ReadElementContentAsObject()
		{
			return this.ReadElementContentAs(this.ValueType, null);
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as an <see cref="T:System.Object" />.</summary>
		/// <returns>A boxed common language runtime (CLR) object of the most appropriate type. The <see cref="P:System.Xml.XmlReader.ValueType" /> property determines the appropriate CLR type. If the content is typed as a list type, this method returns an array of boxed objects of the appropriate type.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		[MonoTODO]
		public virtual object ReadElementContentAsObject(string localName, string namespaceURI)
		{
			return this.ReadElementContentAs(this.ValueType, null, localName, namespaceURI);
		}

		/// <summary>Reads the text content at the current position as an <see cref="T:System.Object" />.</summary>
		/// <returns>The text content as the most appropriate common language runtime (CLR) object.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		[MonoTODO]
		public virtual object ReadContentAsObject()
		{
			return this.ReadContentAs(this.ValueType, null);
		}

		/// <summary>Reads the element content as the requested type.</summary>
		/// <returns>The element content converted to the requested typed object.</returns>
		/// <param name="returnType">The type of the value to be returned.Note   With the release of the .NET Framework 3.5, the value of the <paramref name="returnType" /> parameter can now be the <see cref="T:System.DateTimeOffset" /> type.</param>
		/// <param name="namespaceResolver">An <see cref="T:System.Xml.IXmlNamespaceResolver" /> object that is used to resolve any namespace prefixes related to type conversion.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.OverflowException">Read Decimal.MaxValue.</exception>
		public virtual object ReadElementContentAs(Type type, IXmlNamespaceResolver resolver)
		{
			bool isEmptyElement = this.IsEmptyElement;
			this.ReadStartElement();
			object result = this.ValueAs((!isEmptyElement) ? this.ReadContentString(false) : string.Empty, type, resolver);
			if (!isEmptyElement)
			{
				this.ReadEndElement();
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the element content as the requested type.</summary>
		/// <returns>The element content converted to the requested typed object.</returns>
		/// <param name="returnType">The type of the value to be returned.Note   With the release of the .NET Framework 3.5, the value of the <paramref name="returnType" /> parameter can now be the <see cref="T:System.DateTimeOffset" /> type.</param>
		/// <param name="namespaceResolver">An <see cref="T:System.Xml.IXmlNamespaceResolver" /> object that is used to resolve any namespace prefixes related to type conversion.</param>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		/// <exception cref="T:System.OverflowException">Read Decimal.MaxValue.</exception>
		public virtual object ReadElementContentAs(Type type, IXmlNamespaceResolver resolver, string localName, string namespaceURI)
		{
			this.ReadStartElement(localName, namespaceURI);
			object result = this.ReadContentAs(type, resolver);
			this.ReadEndElement();
			return result;
		}

		/// <summary>Reads the content as an object of the type specified.</summary>
		/// <returns>The concatenated text content or attribute value converted to the requested type.</returns>
		/// <param name="returnType">The type of the value to be returned.Note   With the release of the .NET Framework 3.5, the value of the <paramref name="returnType" /> parameter can now be the <see cref="T:System.DateTimeOffset" /> type.</param>
		/// <param name="namespaceResolver">An <see cref="T:System.Xml.IXmlNamespaceResolver" /> object that is used to resolve any namespace prefixes related to type conversion. For example, this can be used when converting an <see cref="T:System.Xml.XmlQualifiedName" /> object to an xs:string.This value can be null.</param>
		/// <exception cref="T:System.FormatException">The content is not in the correct format for the target type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="returnType" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not a supported node type. See the table below for details.</exception>
		/// <exception cref="T:System.OverflowException">Read Decimal.MaxValue.</exception>
		public virtual object ReadContentAs(Type type, IXmlNamespaceResolver resolver)
		{
			return this.ValueAs(this.ReadContentString(), type, resolver);
		}

		private object ValueAs(string text, Type type, IXmlNamespaceResolver resolver)
		{
			try
			{
				if (type == typeof(object))
				{
					return text;
				}
				if (type == typeof(XmlQualifiedName))
				{
					if (resolver != null)
					{
						return XmlQualifiedName.Parse(text, resolver);
					}
					return XmlQualifiedName.Parse(text, this);
				}
				else
				{
					if (type == typeof(DateTimeOffset))
					{
						return XmlConvert.ToDateTimeOffset(text);
					}
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Boolean:
						return XQueryConvert.StringToBoolean(text);
					case TypeCode.Int32:
						return XQueryConvert.StringToInt(text);
					case TypeCode.Int64:
						return XQueryConvert.StringToInteger(text);
					case TypeCode.Single:
						return XQueryConvert.StringToFloat(text);
					case TypeCode.Double:
						return XQueryConvert.StringToDouble(text);
					case TypeCode.Decimal:
						return XQueryConvert.StringToDecimal(text);
					case TypeCode.DateTime:
						return XQueryConvert.StringToDateTime(text);
					case TypeCode.String:
						return text;
					}
				}
			}
			catch (Exception ex)
			{
				throw this.XmlError(string.Format("Current text value '{0}' is not acceptable for specified type '{1}'. {2}", text, type, (ex == null) ? string.Empty : ex.Message), ex);
			}
			throw new ArgumentException(string.Format("Specified type '{0}' is not supported.", type));
		}

		/// <summary>Reads the current element and returns the contents as a <see cref="T:System.Boolean" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.Boolean" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a <see cref="T:System.Boolean" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual bool ReadElementContentAsBoolean()
		{
			bool result;
			try
			{
				result = XQueryConvert.StringToBoolean(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as a <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.DateTime" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a <see cref="T:System.DateTime" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual DateTime ReadElementContentAsDateTime()
		{
			DateTime result;
			try
			{
				result = XQueryConvert.StringToDateTime(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as a <see cref="T:System.Decimal" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.Decimal" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a <see cref="T:System.Decimal" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual decimal ReadElementContentAsDecimal()
		{
			decimal result;
			try
			{
				result = XQueryConvert.StringToDecimal(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as a double-precision floating-point number.</summary>
		/// <returns>The element content as a double-precision floating-point number.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a double-precision floating-point number.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual double ReadElementContentAsDouble()
		{
			double result;
			try
			{
				result = XQueryConvert.StringToDouble(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as single-precision floating-point number.</summary>
		/// <returns>The element content as a single-precision floating point number.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a single-precision floating-point number.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual float ReadElementContentAsFloat()
		{
			float result;
			try
			{
				result = XQueryConvert.StringToFloat(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as a 32-bit signed integer.</summary>
		/// <returns>The element content as a 32-bit signed integer.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a 32-bit signed integer.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual int ReadElementContentAsInt()
		{
			int result;
			try
			{
				result = XQueryConvert.StringToInt(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as a 64-bit signed integer.</summary>
		/// <returns>The element content as a 64-bit signed integer.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a 64-bit signed integer.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual long ReadElementContentAsLong()
		{
			long result;
			try
			{
				result = XQueryConvert.StringToInteger(this.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the current element and returns the contents as a <see cref="T:System.String" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a <see cref="T:System.String" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		public virtual string ReadElementContentAsString()
		{
			bool isEmptyElement = this.IsEmptyElement;
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new InvalidOperationException(string.Format("'{0}' is an element node.", this.NodeType));
			}
			this.ReadStartElement();
			if (isEmptyElement)
			{
				return string.Empty;
			}
			string result = this.ReadContentString(false);
			this.ReadEndElement();
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a <see cref="T:System.Boolean" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.Boolean" /> object.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual bool ReadElementContentAsBoolean(string localName, string namespaceURI)
		{
			bool result;
			try
			{
				result = XQueryConvert.StringToBoolean(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>The element contents as a <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
		{
			DateTime result;
			try
			{
				result = XQueryConvert.StringToDateTime(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a <see cref="T:System.Decimal" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.Decimal" /> object.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a <see cref="T:System.Decimal" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
		{
			decimal result;
			try
			{
				result = XQueryConvert.StringToDecimal(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a double-precision floating-point number.</summary>
		/// <returns>The element content as a double-precision floating-point number.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to the requested type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual double ReadElementContentAsDouble(string localName, string namespaceURI)
		{
			double result;
			try
			{
				result = XQueryConvert.StringToDouble(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a single-precision floating-point number.</summary>
		/// <returns>The element content as a single-precision floating point number.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a single-precision floating-point number.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual float ReadElementContentAsFloat(string localName, string namespaceURI)
		{
			float result;
			try
			{
				result = XQueryConvert.StringToFloat(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a 32-bit signed integer.</summary>
		/// <returns>The element content as a 32-bit signed integer.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a 32-bit signed integer.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual int ReadElementContentAsInt(string localName, string namespaceURI)
		{
			int result;
			try
			{
				result = XQueryConvert.StringToInt(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a 64-bit signed integer.</summary>
		/// <returns>The element content as a 64-bit signed integer.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a 64-bit signed integer.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual long ReadElementContentAsLong(string localName, string namespaceURI)
		{
			long result;
			try
			{
				result = XQueryConvert.StringToInteger(this.ReadElementContentAsString(localName, namespaceURI));
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Checks that the specified local name and namespace URI matches that of the current element, then reads the current element and returns the contents as a <see cref="T:System.String" /> object.</summary>
		/// <returns>The element content as a <see cref="T:System.String" /> object.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on an element.</exception>
		/// <exception cref="T:System.Xml.XmlException">The current element contains child elements.-or-The element content cannot be converted to a <see cref="T:System.String" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">The method is called with null arguments.</exception>
		/// <exception cref="T:System.ArgumentException">The specified local name and namespace URI do not match that of the current element being read.</exception>
		public virtual string ReadElementContentAsString(string localName, string namespaceURI)
		{
			bool isEmptyElement = this.IsEmptyElement;
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new InvalidOperationException(string.Format("'{0}' is an element node.", this.NodeType));
			}
			this.ReadStartElement(localName, namespaceURI);
			if (isEmptyElement)
			{
				return string.Empty;
			}
			string result = this.ReadContentString(false);
			this.ReadEndElement();
			return result;
		}

		/// <summary>Reads the text content at the current position as a Boolean.</summary>
		/// <returns>The text content as a <see cref="T:System.Boolean" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual bool ReadContentAsBoolean()
		{
			bool result;
			try
			{
				result = XQueryConvert.StringToBoolean(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>The text content as a <see cref="T:System.DateTime" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual DateTime ReadContentAsDateTime()
		{
			DateTime result;
			try
			{
				result = XQueryConvert.StringToDateTime(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a <see cref="T:System.Decimal" /> object.</summary>
		/// <returns>The text content at the current position as a <see cref="T:System.Decimal" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual decimal ReadContentAsDecimal()
		{
			decimal result;
			try
			{
				result = XQueryConvert.StringToDecimal(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a double-precision floating-point number.</summary>
		/// <returns>The text content as a double-precision floating-point number.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual double ReadContentAsDouble()
		{
			double result;
			try
			{
				result = XQueryConvert.StringToDouble(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a single-precision floating point number.</summary>
		/// <returns>The text content at the current position as a single-precision floating point number.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual float ReadContentAsFloat()
		{
			float result;
			try
			{
				result = XQueryConvert.StringToFloat(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a 32-bit signed integer.</summary>
		/// <returns>The text content as a 32-bit signed integer.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual int ReadContentAsInt()
		{
			int result;
			try
			{
				result = XQueryConvert.StringToInt(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a 64-bit signed integer.</summary>
		/// <returns>The text content as a 64-bit signed integer.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual long ReadContentAsLong()
		{
			long result;
			try
			{
				result = XQueryConvert.StringToInteger(this.ReadContentString());
			}
			catch (FormatException innerException)
			{
				throw this.XmlError("Typed value is invalid.", innerException);
			}
			return result;
		}

		/// <summary>Reads the text content at the current position as a <see cref="T:System.String" /> object.</summary>
		/// <returns>The text content as a <see cref="T:System.String" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.FormatException">The string format is not valid.</exception>
		public virtual string ReadContentAsString()
		{
			return this.ReadContentString();
		}

		/// <summary>Reads the content and returns the Base64 decoded binary bytes.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Xml.XmlReader.ReadContentAsBase64(System.Byte[],System.Int32,System.Int32)" />  is not supported on the current node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlReader" /> implementation does not support this method.</exception>
		public virtual int ReadContentAsBase64(byte[] buffer, int offset, int length)
		{
			this.CheckSupport();
			return this.binary.ReadContentAsBase64(buffer, offset, length);
		}

		/// <summary>Reads the content and returns the BinHex decoded binary bytes.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Xml.XmlReader.ReadContentAsBinHex(System.Byte[],System.Int32,System.Int32)" /> is not supported on the current node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlReader" /> implementation does not support this method.</exception>
		public virtual int ReadContentAsBinHex(byte[] buffer, int offset, int length)
		{
			this.CheckSupport();
			return this.binary.ReadContentAsBinHex(buffer, offset, length);
		}

		/// <summary>Reads the element and decodes the Base64 content.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not an element node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlReader" /> implementation does not support this method.</exception>
		/// <exception cref="T:System.Xml.XmlException">The element contains mixed-content.</exception>
		/// <exception cref="T:System.FormatException">The content cannot be converted to the requested type.</exception>
		public virtual int ReadElementContentAsBase64(byte[] buffer, int offset, int length)
		{
			this.CheckSupport();
			return this.binary.ReadElementContentAsBase64(buffer, offset, length);
		}

		/// <summary>Reads the element and decodes the BinHex content.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not an element node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlReader" /> implementation does not support this method.</exception>
		/// <exception cref="T:System.Xml.XmlException">The element contains mixed-content.</exception>
		/// <exception cref="T:System.FormatException">The content cannot be converted to the requested type.</exception>
		public virtual int ReadElementContentAsBinHex(byte[] buffer, int offset, int length)
		{
			this.CheckSupport();
			return this.binary.ReadElementContentAsBinHex(buffer, offset, length);
		}

		private void CheckSupport()
		{
			if (!this.CanReadBinaryContent || !this.CanReadValueChunk)
			{
				throw new NotSupportedException();
			}
			if (this.binary == null)
			{
				this.binary = new XmlReaderBinarySupport(this);
			}
		}

		/// <summary>Reads large streams of text embedded in an XML document.</summary>
		/// <returns>The number of characters read into the buffer. The value zero is returned when there is no more text content.</returns>
		/// <param name="buffer">The array of characters that serves as the buffer to which the text contents are written. This value cannot be null.</param>
		/// <param name="index">The offset within the buffer where the <see cref="T:System.Xml.XmlReader" /> can start to copy the results.</param>
		/// <param name="count">The maximum number of characters to copy into the buffer. The actual number of characters copied is returned from this method.</param>
		/// <exception cref="T:System.InvalidOperationException">The current node does not have a value (<see cref="P:System.Xml.XmlReader.HasValue" /> is false).</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer, or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XmlReader" /> implementation does not support this method.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML data is not well-formed.</exception>
		public virtual int ReadValueChunk(char[] buffer, int offset, int length)
		{
			if (!this.CanReadValueChunk)
			{
				throw new NotSupportedException();
			}
			if (this.binary == null)
			{
				this.binary = new XmlReaderBinarySupport(this);
			}
			return this.binary.ReadValueChunk(buffer, offset, length);
		}

		/// <summary>When overridden in a derived class, resolves the entity reference for EntityReference nodes.</summary>
		/// <exception cref="T:System.InvalidOperationException">The reader is not positioned on an EntityReference node; this implementation of the reader cannot resolve entities (<see cref="P:System.Xml.XmlReader.CanResolveEntity" /> returns false). </exception>
		public abstract void ResolveEntity();

		/// <summary>Skips the children of the current node.</summary>
		public virtual void Skip()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return;
			}
			this.MoveToElement();
			if (this.NodeType != XmlNodeType.Element || this.IsEmptyElement)
			{
				this.Read();
				return;
			}
			int depth = this.Depth;
			while (this.Read() && depth < this.Depth)
			{
			}
			if (this.NodeType == XmlNodeType.EndElement)
			{
				this.Read();
			}
		}

		private XmlException XmlError(string message)
		{
			return new XmlException(this as IXmlLineInfo, this.BaseURI, message);
		}

		private XmlException XmlError(string message, Exception innerException)
		{
			return new XmlException(this as IXmlLineInfo, this.BaseURI, message);
		}
	}
}
