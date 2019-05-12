using System;

namespace System.Xml
{
	/// <summary>Specifies the type of node.</summary>
	public enum XmlNodeType
	{
		/// <summary>This is returned by the <see cref="T:System.Xml.XmlReader" /> if a Read method has not been called.</summary>
		None,
		/// <summary>An element (for example, &lt;item&gt; ).</summary>
		Element,
		/// <summary>An attribute (for example, id='123' ).</summary>
		Attribute,
		/// <summary>The text content of a node.</summary>
		Text,
		/// <summary>A CDATA section (for example, &lt;![CDATA[my escaped text]]&gt; ).</summary>
		CDATA,
		/// <summary>A reference to an entity (for example, &amp;num; ).</summary>
		EntityReference,
		/// <summary>An entity declaration (for example, &lt;!ENTITY...&gt; ).</summary>
		Entity,
		/// <summary>A processing instruction (for example, &lt;?pi test?&gt; ).</summary>
		ProcessingInstruction,
		/// <summary>A comment (for example, &lt;!-- my comment --&gt; ).</summary>
		Comment,
		/// <summary>A document object that, as the root of the document tree, provides access to the entire XML document.</summary>
		Document,
		/// <summary>The document type declaration, indicated by the following tag (for example, &lt;!DOCTYPE...&gt; ).</summary>
		DocumentType,
		/// <summary>A document fragment.</summary>
		DocumentFragment,
		/// <summary>A notation in the document type declaration (for example, &lt;!NOTATION...&gt; ).</summary>
		Notation,
		/// <summary>White space between markup.</summary>
		Whitespace,
		/// <summary>White space between markup in a mixed content model or white space within the xml:space="preserve" scope.</summary>
		SignificantWhitespace,
		/// <summary>An end element tag (for example, &lt;/item&gt; ).</summary>
		EndElement,
		/// <summary>Returned when XmlReader gets to the end of the entity replacement as a result of a call to <see cref="M:System.Xml.XmlReader.ResolveEntity" />.</summary>
		EndEntity,
		/// <summary>The XML declaration (for example, &lt;?xml version='1.0'?&gt; ).</summary>
		XmlDeclaration
	}
}
