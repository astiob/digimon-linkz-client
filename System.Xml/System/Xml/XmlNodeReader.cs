using Mono.Xml;
using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml
{
	/// <summary>Represents a reader that provides fast, non-cached forward only access to XML data in an <see cref="T:System.Xml.XmlNode" />.</summary>
	public class XmlNodeReader : XmlReader, IHasXmlParserContext, IXmlNamespaceResolver
	{
		private XmlReader entity;

		private XmlNodeReaderImpl source;

		private bool entityInsideAttribute;

		private bool insideAttribute;

		/// <summary>Creates an instance of the XmlNodeReader class using the specified <see cref="T:System.Xml.XmlNode" />.</summary>
		/// <param name="node">The XmlNode you want to read. </param>
		public XmlNodeReader(XmlNode node)
		{
			this.source = new XmlNodeReaderImpl(node);
		}

		private XmlNodeReader(XmlNodeReaderImpl entityContainer, bool insideAttribute)
		{
			this.source = new XmlNodeReaderImpl(entityContainer);
			this.entityInsideAttribute = insideAttribute;
		}

		XmlParserContext IHasXmlParserContext.ParserContext
		{
			get
			{
				return ((IHasXmlParserContext)this.Current).ParserContext;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.IXmlNamespaceResolver.GetNamespacesInScope(System.Xml.XmlNamespaceScope)" />.</summary>
		/// <param name="scope"></param>
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return ((IXmlNamespaceResolver)this.Current).GetNamespacesInScope(scope);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.IXmlNamespaceResolver.LookupPrefix(System.String)" />.</summary>
		/// <param name="namespaceName"></param>
		string IXmlNamespaceResolver.LookupPrefix(string ns)
		{
			return ((IXmlNamespaceResolver)this.Current).LookupPrefix(ns);
		}

		private XmlReader Current
		{
			get
			{
				return (this.entity == null || this.entity.ReadState == ReadState.Initial) ? this.source : this.entity;
			}
		}

		/// <summary>Gets the number of attributes on the current node.</summary>
		/// <returns>The number of attributes on the current node. This number includes default attributes.</returns>
		public override int AttributeCount
		{
			get
			{
				return this.Current.AttributeCount;
			}
		}

		/// <summary>Gets the base URI of the current node.</summary>
		/// <returns>The base URI of the current node.</returns>
		public override string BaseURI
		{
			get
			{
				return this.Current.BaseURI;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Xml.XmlNodeReader" /> implements the binary content read methods.</summary>
		/// <returns>true if the binary content read methods are implemented; otherwise false. The <see cref="T:System.Xml.XmlNodeReader" /> class always returns true.</returns>
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether this reader can parse and resolve entities.</summary>
		/// <returns>true if the reader can parse and resolve entities; otherwise, false. XmlNodeReader always returns true.</returns>
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
				if (this.entity != null && this.entity.ReadState == ReadState.Interactive)
				{
					return this.source.Depth + this.entity.Depth + 1;
				}
				return this.source.Depth;
			}
		}

		/// <summary>Gets a value indicating whether the reader is positioned at the end of the stream.</summary>
		/// <returns>true if the reader is positioned at the end of the stream; otherwise, false.</returns>
		public override bool EOF
		{
			get
			{
				return this.source.EOF;
			}
		}

		/// <summary>Gets a value indicating whether the current node has any attributes.</summary>
		/// <returns>true if the current node has attributes; otherwise, false.</returns>
		public override bool HasAttributes
		{
			get
			{
				return this.Current.HasAttributes;
			}
		}

		/// <summary>Gets a value indicating whether the current node can have a <see cref="P:System.Xml.XmlNodeReader.Value" />.</summary>
		/// <returns>true if the node on which the reader is currently positioned can have a Value; otherwise, false.</returns>
		public override bool HasValue
		{
			get
			{
				return this.Current.HasValue;
			}
		}

		/// <summary>Gets a value indicating whether the current node is an attribute that was generated from the default value defined in the document type definition (DTD) or schema.</summary>
		/// <returns>true if the current node is an attribute whose value was generated from the default value defined in the DTD or schema; false if the attribute value was explicitly set.</returns>
		public override bool IsDefault
		{
			get
			{
				return this.Current.IsDefault;
			}
		}

		/// <summary>Gets a value indicating whether the current node is an empty element (for example, &lt;MyElement/&gt;).</summary>
		/// <returns>true if the current node is an element (<see cref="P:System.Xml.XmlNodeReader.NodeType" /> equals XmlNodeType.Element) and it ends with /&gt;; otherwise, false.</returns>
		public override bool IsEmptyElement
		{
			get
			{
				return this.Current.IsEmptyElement;
			}
		}

		/// <summary>Gets the local name of the current node.</summary>
		/// <returns>The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.</returns>
		public override string LocalName
		{
			get
			{
				return this.Current.LocalName;
			}
		}

		/// <summary>Gets the qualified name of the current node.</summary>
		/// <returns>The qualified name of the current node. For example, Name is bk:book for the element &lt;bk:book&gt;.The name returned is dependent on the <see cref="P:System.Xml.XmlNodeReader.NodeType" /> of the node. The following node types return the listed values. All other node types return an empty string.Node Type Name AttributeThe name of the attribute. DocumentTypeThe document type name. ElementThe tag name. EntityReferenceThe name of the entity referenced. ProcessingInstructionThe target of the processing instruction. XmlDeclarationThe literal string xml. </returns>
		public override string Name
		{
			get
			{
				return this.Current.Name;
			}
		}

		/// <summary>Gets the namespace URI (as defined in the W3C Namespace specification) of the node on which the reader is positioned.</summary>
		/// <returns>The namespace URI of the current node; otherwise an empty string.</returns>
		public override string NamespaceURI
		{
			get
			{
				return this.Current.NamespaceURI;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlNameTable" /> associated with this implementation.</summary>
		/// <returns>The XmlNameTable enabling you to get the atomized version of a string within the node.</returns>
		public override XmlNameTable NameTable
		{
			get
			{
				return this.Current.NameTable;
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlNodeType" /> values representing the type of the current node.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				if (this.entity != null)
				{
					return (this.entity.ReadState != ReadState.Initial) ? ((!this.entity.EOF) ? this.entity.NodeType : XmlNodeType.EndEntity) : this.source.NodeType;
				}
				return this.source.NodeType;
			}
		}

		/// <summary>Gets the namespace prefix associated with the current node.</summary>
		/// <returns>The namespace prefix associated with the current node.</returns>
		public override string Prefix
		{
			get
			{
				return this.Current.Prefix;
			}
		}

		/// <summary>Gets the state of the reader.</summary>
		/// <returns>One of the <see cref="T:System.Xml.ReadState" /> values.</returns>
		public override ReadState ReadState
		{
			get
			{
				return (this.entity == null) ? this.source.ReadState : ReadState.Interactive;
			}
		}

		/// <summary>Gets the schema information that has been assigned to the current node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.IXmlSchemaInfo" /> object containing the schema information for the current node.</returns>
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				IXmlSchemaInfo result;
				if (this.entity != null)
				{
					IXmlSchemaInfo schemaInfo = this.entity.SchemaInfo;
					result = schemaInfo;
				}
				else
				{
					result = this.source.SchemaInfo;
				}
				return result;
			}
		}

		/// <summary>Gets the text value of the current node.</summary>
		/// <returns>The value returned depends on the <see cref="P:System.Xml.XmlNodeReader.NodeType" /> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node Type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space between markup in a mixed content model. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
		public override string Value
		{
			get
			{
				return this.Current.Value;
			}
		}

		/// <summary>Gets the current xml:lang scope.</summary>
		/// <returns>The current xml:lang scope.</returns>
		public override string XmlLang
		{
			get
			{
				return this.Current.XmlLang;
			}
		}

		/// <summary>Gets the current xml:space scope.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlSpace" /> values. If no xml:space scope exists, this property defaults to XmlSpace.None.</returns>
		public override XmlSpace XmlSpace
		{
			get
			{
				return this.Current.XmlSpace;
			}
		}

		/// <summary>Changes the <see cref="P:System.Xml.XmlNodeReader.ReadState" /> to Closed.</summary>
		public override void Close()
		{
			if (this.entity != null)
			{
				this.entity.Close();
			}
			this.source.Close();
		}

		/// <summary>Gets the value of the attribute with the specified index.</summary>
		/// <returns>The value of the specified attribute.</returns>
		/// <param name="attributeIndex">The index of the attribute. The index is zero-based. (The first attribute has index 0.) </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="i" /> parameter is less than 0 or greater than or equal to <see cref="P:System.Xml.XmlNodeReader.AttributeCount" />. </exception>
		public override string GetAttribute(int attributeIndex)
		{
			return this.Current.GetAttribute(attributeIndex);
		}

		/// <summary>Gets the value of the attribute with the specified name.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found, null is returned.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public override string GetAttribute(string name)
		{
			return this.Current.GetAttribute(name);
		}

		/// <summary>Gets the value of the attribute with the specified local name and namespace URI.</summary>
		/// <returns>The value of the specified attribute. If the attribute is not found, null is returned.</returns>
		/// <param name="name">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		public override string GetAttribute(string name, string namespaceURI)
		{
			return this.Current.GetAttribute(name, namespaceURI);
		}

		/// <summary>Resolves a namespace prefix in the current element's scope.</summary>
		/// <returns>The namespace URI to which the prefix maps or null if no matching prefix is found.</returns>
		/// <param name="prefix">The prefix whose namespace URI you want to resolve. To match the default namespace, pass an empty string. This string does not have to be atomized. </param>
		public override string LookupNamespace(string prefix)
		{
			return this.Current.LookupNamespace(prefix);
		}

		/// <summary>Moves to the attribute with the specified index.</summary>
		/// <param name="attributeIndex">The index of the attribute. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="i" /> parameter is less than 0 or greater than or equal to <see cref="P:System.Xml.XmlReader.AttributeCount" />. </exception>
		public override void MoveToAttribute(int i)
		{
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.Current.MoveToAttribute(i);
			this.insideAttribute = true;
		}

		/// <summary>Moves to the attribute with the specified name.</summary>
		/// <returns>true if the attribute is found; otherwise, false. If false, the reader's position does not change.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public override bool MoveToAttribute(string name)
		{
			if (this.entity != null && !this.entityInsideAttribute)
			{
				return this.entity.MoveToAttribute(name);
			}
			if (!this.source.MoveToAttribute(name))
			{
				return false;
			}
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.insideAttribute = true;
			return true;
		}

		/// <summary>Moves to the attribute with the specified local name and namespace URI.</summary>
		/// <returns>true if the attribute is found; otherwise, false. If false, the reader's position does not change.</returns>
		/// <param name="name">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			if (this.entity != null && !this.entityInsideAttribute)
			{
				return this.entity.MoveToAttribute(localName, namespaceURI);
			}
			if (!this.source.MoveToAttribute(localName, namespaceURI))
			{
				return false;
			}
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.insideAttribute = true;
			return true;
		}

		/// <summary>Moves to the element that contains the current attribute node.</summary>
		/// <returns>true if the reader is positioned on an attribute (the reader moves to the element that owns the attribute); false if the reader is not positioned on an attribute (the position of the reader does not change).</returns>
		public override bool MoveToElement()
		{
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity = null;
			}
			if (!this.Current.MoveToElement())
			{
				return false;
			}
			this.insideAttribute = false;
			return true;
		}

		/// <summary>Moves to the first attribute.</summary>
		/// <returns>true if an attribute exists (the reader moves to the first attribute); otherwise, false (the position of the reader does not change).</returns>
		public override bool MoveToFirstAttribute()
		{
			if (this.entity != null && !this.entityInsideAttribute)
			{
				return this.entity.MoveToFirstAttribute();
			}
			if (!this.source.MoveToFirstAttribute())
			{
				return false;
			}
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.insideAttribute = true;
			return true;
		}

		/// <summary>Moves to the next attribute.</summary>
		/// <returns>true if there is a next attribute; false if there are no more attributes.</returns>
		public override bool MoveToNextAttribute()
		{
			if (this.entity != null && !this.entityInsideAttribute)
			{
				return this.entity.MoveToNextAttribute();
			}
			if (!this.source.MoveToNextAttribute())
			{
				return false;
			}
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity.Close();
				this.entity = null;
			}
			this.insideAttribute = true;
			return true;
		}

		/// <summary>Reads the next node from the stream.</summary>
		/// <returns>true if the next node was read successfully; false if there are no more nodes to read.</returns>
		public override bool Read()
		{
			this.insideAttribute = false;
			if (this.entity != null && (this.entityInsideAttribute || this.entity.EOF))
			{
				this.entity = null;
			}
			if (this.entity != null)
			{
				this.entity.Read();
				return true;
			}
			return this.source.Read();
		}

		/// <summary>Parses the attribute value into one or more Text, EntityReference, or EndEntity nodes.</summary>
		/// <returns>true if there are nodes to return.false if the reader is not positioned on an attribute node when the initial call is made or if all the attribute values have been read.An empty attribute, such as, misc="", returns true with a single node with a value of String.Empty.</returns>
		public override bool ReadAttributeValue()
		{
			if (this.entity != null && this.entityInsideAttribute)
			{
				if (!this.entity.EOF)
				{
					this.entity.Read();
					return true;
				}
				this.entity = null;
			}
			return this.Current.ReadAttributeValue();
		}

		/// <summary>Reads the content and returns the Base64 decoded binary bytes.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Xml.XmlNodeReader.ReadContentAsBase64(System.Byte[],System.Int32,System.Int32)" /> is not supported on the current node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		public override int ReadContentAsBase64(byte[] buffer, int offset, int length)
		{
			if (this.entity != null)
			{
				return this.entity.ReadContentAsBase64(buffer, offset, length);
			}
			return this.source.ReadContentAsBase64(buffer, offset, length);
		}

		/// <summary>Reads the content and returns the BinHex decoded binary bytes.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Xml.XmlNodeReader.ReadContentAsBinHex(System.Byte[],System.Int32,System.Int32)" />  is not supported on the current node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		public override int ReadContentAsBinHex(byte[] buffer, int offset, int length)
		{
			if (this.entity != null)
			{
				return this.entity.ReadContentAsBinHex(buffer, offset, length);
			}
			return this.source.ReadContentAsBinHex(buffer, offset, length);
		}

		/// <summary>Reads the element and decodes the Base64 content.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not an element node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.Xml.XmlException">The element contains mixed content.</exception>
		/// <exception cref="T:System.FormatException">The content cannot be converted to the requested type.</exception>
		public override int ReadElementContentAsBase64(byte[] buffer, int offset, int length)
		{
			if (this.entity != null)
			{
				return this.entity.ReadElementContentAsBase64(buffer, offset, length);
			}
			return this.source.ReadElementContentAsBase64(buffer, offset, length);
		}

		/// <summary>Reads the element and decodes the BinHex content.</summary>
		/// <returns>The number of bytes written to the buffer.</returns>
		/// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
		/// <param name="index">The offset into the buffer where to start copying the result.</param>
		/// <param name="count">The maximum number of bytes to copy into the buffer. The actual number of bytes copied is returned from this method.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node is not an element node.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index into the buffer or index + count is larger than the allocated buffer size.</exception>
		/// <exception cref="T:System.Xml.XmlException">The element contains mixed content.</exception>
		/// <exception cref="T:System.FormatException">The content cannot be converted to the requested type.</exception>
		public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int length)
		{
			if (this.entity != null)
			{
				return this.entity.ReadElementContentAsBinHex(buffer, offset, length);
			}
			return this.source.ReadElementContentAsBinHex(buffer, offset, length);
		}

		/// <summary>Reads the contents of an element or text node as a string.</summary>
		/// <returns>The contents of the element or text-like node (This can include CDATA, Text nodes, and so on). This can be an empty string if the reader is positioned on something other than an element or text node, or if there is no more text content to return in the current context.Note: The text node can be either an element or an attribute text node.</returns>
		public override string ReadString()
		{
			return base.ReadString();
		}

		/// <summary>Resolves the entity reference for EntityReference nodes.</summary>
		/// <exception cref="T:System.InvalidOperationException">The reader is not positioned on an EntityReference node. </exception>
		public override void ResolveEntity()
		{
			if (this.entity != null)
			{
				this.entity.ResolveEntity();
			}
			else
			{
				if (this.source.NodeType != XmlNodeType.EntityReference)
				{
					throw new InvalidOperationException("The current node is not an Entity Reference");
				}
				this.entity = new XmlNodeReader(this.source, this.insideAttribute);
			}
		}

		/// <summary>Skips the children of the current node.</summary>
		public override void Skip()
		{
			if (this.entity != null && this.entityInsideAttribute)
			{
				this.entity = null;
			}
			this.Current.Skip();
		}
	}
}
