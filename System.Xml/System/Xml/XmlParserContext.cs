using Mono.Xml;
using Mono.Xml2;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Xml
{
	/// <summary>Provides all the context information required by the <see cref="T:System.Xml.XmlReader" /> to parse an XML fragment.</summary>
	public class XmlParserContext
	{
		private string baseURI = string.Empty;

		private string docTypeName = string.Empty;

		private Encoding encoding;

		private string internalSubset = string.Empty;

		private XmlNamespaceManager namespaceManager;

		private XmlNameTable nameTable;

		private string publicID = string.Empty;

		private string systemID = string.Empty;

		private string xmlLang = string.Empty;

		private XmlSpace xmlSpace;

		private ArrayList contextItems;

		private int contextItemCount;

		private DTDObjectModel dtd;

		/// <summary>Initializes a new instance of the XmlParserContext class with the specified <see cref="T:System.Xml.XmlNameTable" />, <see cref="T:System.Xml.XmlNamespaceManager" />, xml:lang, and xml:space values.</summary>
		/// <param name="nt">The <see cref="T:System.Xml.XmlNameTable" /> to use to atomize strings. If this is null, the name table used to construct the <paramref name="nsMgr" /> is used instead. For more information about atomized strings, see <see cref="T:System.Xml.XmlNameTable" />. </param>
		/// <param name="nsMgr">The <see cref="T:System.Xml.XmlNamespaceManager" /> to use for looking up namespace information, or null. </param>
		/// <param name="xmlLang">The xml:lang scope. </param>
		/// <param name="xmlSpace">An <see cref="T:System.Xml.XmlSpace" /> value indicating the xml:space scope. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="nt" /> is not the same XmlNameTable used to construct <paramref name="nsMgr" />. </exception>
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string xmlLang, XmlSpace xmlSpace) : this(nt, nsMgr, null, null, null, null, null, xmlLang, xmlSpace, null)
		{
		}

		/// <summary>Initializes a new instance of the XmlParserContext class with the specified <see cref="T:System.Xml.XmlNameTable" />, <see cref="T:System.Xml.XmlNamespaceManager" />, xml:lang, xml:space, and encoding.</summary>
		/// <param name="nt">The <see cref="T:System.Xml.XmlNameTable" /> to use to atomize strings. If this is null, the name table used to construct the <paramref name="nsMgr" /> is used instead. For more information on atomized strings, see <see cref="T:System.Xml.XmlNameTable" />. </param>
		/// <param name="nsMgr">The <see cref="T:System.Xml.XmlNamespaceManager" /> to use for looking up namespace information, or null. </param>
		/// <param name="xmlLang">The xml:lang scope. </param>
		/// <param name="xmlSpace">An <see cref="T:System.Xml.XmlSpace" /> value indicating the xml:space scope. </param>
		/// <param name="enc">An <see cref="T:System.Text.Encoding" /> object indicating the encoding setting. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="nt" /> is not the same XmlNameTable used to construct <paramref name="nsMgr" />. </exception>
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string xmlLang, XmlSpace xmlSpace, Encoding enc) : this(nt, nsMgr, null, null, null, null, null, xmlLang, xmlSpace, enc)
		{
		}

		/// <summary>Initializes a new instance of the XmlParserContext class with the specified <see cref="T:System.Xml.XmlNameTable" />, <see cref="T:System.Xml.XmlNamespaceManager" />, base URI, xml:lang, xml:space, and document type values.</summary>
		/// <param name="nt">The <see cref="T:System.Xml.XmlNameTable" /> to use to atomize strings. If this is null, the name table used to construct the <paramref name="nsMgr" /> is used instead. For more information about atomized strings, see <see cref="T:System.Xml.XmlNameTable" />. </param>
		/// <param name="nsMgr">The <see cref="T:System.Xml.XmlNamespaceManager" /> to use for looking up namespace information, or null. </param>
		/// <param name="docTypeName">The name of the document type declaration. </param>
		/// <param name="pubId">The public identifier. </param>
		/// <param name="sysId">The system identifier. </param>
		/// <param name="internalSubset">The internal DTD subset. The internal DTD subset is used for entity resolution, not document validation.</param>
		/// <param name="baseURI">The base URI for the XML fragment (the location from which the fragment was loaded). </param>
		/// <param name="xmlLang">The xml:lang scope. </param>
		/// <param name="xmlSpace">An <see cref="T:System.Xml.XmlSpace" /> value indicating the xml:space scope. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="nt" /> is not the same XmlNameTable used to construct <paramref name="nsMgr" />. </exception>
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, XmlSpace xmlSpace) : this(nt, nsMgr, docTypeName, pubId, sysId, internalSubset, baseURI, xmlLang, xmlSpace, null)
		{
		}

		/// <summary>Initializes a new instance of the XmlParserContext class with the specified <see cref="T:System.Xml.XmlNameTable" />, <see cref="T:System.Xml.XmlNamespaceManager" />, base URI, xml:lang, xml:space, encoding, and document type values.</summary>
		/// <param name="nt">The <see cref="T:System.Xml.XmlNameTable" /> to use to atomize strings. If this is null, the name table used to construct the <paramref name="nsMgr" /> is used instead. For more information about atomized strings, see <see cref="T:System.Xml.XmlNameTable" />. </param>
		/// <param name="nsMgr">The <see cref="T:System.Xml.XmlNamespaceManager" /> to use for looking up namespace information, or null. </param>
		/// <param name="docTypeName">The name of the document type declaration. </param>
		/// <param name="pubId">The public identifier. </param>
		/// <param name="sysId">The system identifier. </param>
		/// <param name="internalSubset">The internal DTD subset. The internal DTD subset is used for entity resolution, not document validation.</param>
		/// <param name="baseURI">The base URI for the XML fragment (the location from which the fragment was loaded). </param>
		/// <param name="xmlLang">The xml:lang scope. </param>
		/// <param name="xmlSpace">An <see cref="T:System.Xml.XmlSpace" /> value indicating the xml:space scope. </param>
		/// <param name="enc">An <see cref="T:System.Text.Encoding" /> object indicating the encoding setting. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="nt" /> is not the same XmlNameTable used to construct <paramref name="nsMgr" />. </exception>
		public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, XmlSpace xmlSpace, Encoding enc) : this(nt, nsMgr, (docTypeName == null || !(docTypeName != string.Empty)) ? null : new XmlTextReader(TextReader.Null, nt).GenerateDTDObjectModel(docTypeName, pubId, sysId, internalSubset), baseURI, xmlLang, xmlSpace, enc)
		{
		}

		internal XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, DTDObjectModel dtd, string baseURI, string xmlLang, XmlSpace xmlSpace, Encoding enc)
		{
			this.namespaceManager = nsMgr;
			this.nameTable = ((nt == null) ? ((nsMgr == null) ? null : nsMgr.NameTable) : nt);
			if (dtd != null)
			{
				this.DocTypeName = dtd.Name;
				this.PublicId = dtd.PublicId;
				this.SystemId = dtd.SystemId;
				this.InternalSubset = dtd.InternalSubset;
				this.dtd = dtd;
			}
			this.encoding = enc;
			this.BaseURI = baseURI;
			this.XmlLang = xmlLang;
			this.xmlSpace = xmlSpace;
			this.contextItems = new ArrayList();
		}

		/// <summary>Gets or sets the base URI.</summary>
		/// <returns>The base URI to use to resolve the DTD file.</returns>
		public string BaseURI
		{
			get
			{
				return this.baseURI;
			}
			set
			{
				this.baseURI = ((value == null) ? string.Empty : value);
			}
		}

		/// <summary>Gets or sets the name of the document type declaration.</summary>
		/// <returns>The name of the document type declaration.</returns>
		public string DocTypeName
		{
			get
			{
				return (this.docTypeName == null) ? ((this.dtd == null) ? null : this.dtd.Name) : this.docTypeName;
			}
			set
			{
				this.docTypeName = ((value == null) ? string.Empty : value);
			}
		}

		internal DTDObjectModel Dtd
		{
			get
			{
				return this.dtd;
			}
			set
			{
				this.dtd = value;
			}
		}

		/// <summary>Gets or sets the encoding type.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> object indicating the encoding type.</returns>
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = value;
			}
		}

		/// <summary>Gets or sets the internal DTD subset.</summary>
		/// <returns>The internal DTD subset. For example, this property returns everything between the square brackets &lt;!DOCTYPE doc [...]&gt;.The internal DTD subset is used for entity resolution, not document validation.</returns>
		public string InternalSubset
		{
			get
			{
				return (this.internalSubset == null) ? ((this.dtd == null) ? null : this.dtd.InternalSubset) : this.internalSubset;
			}
			set
			{
				this.internalSubset = ((value == null) ? string.Empty : value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.XmlNamespaceManager" />.</summary>
		/// <returns>The XmlNamespaceManager.</returns>
		public XmlNamespaceManager NamespaceManager
		{
			get
			{
				return this.namespaceManager;
			}
			set
			{
				this.namespaceManager = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlNameTable" /> used to atomize strings. For more information on atomized strings, see <see cref="T:System.Xml.XmlNameTable" />.</summary>
		/// <returns>The XmlNameTable.</returns>
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
			set
			{
				this.nameTable = value;
			}
		}

		/// <summary>Gets or sets the public identifier.</summary>
		/// <returns>The public identifier.</returns>
		public string PublicId
		{
			get
			{
				return (this.publicID == null) ? ((this.dtd == null) ? null : this.dtd.PublicId) : this.publicID;
			}
			set
			{
				this.publicID = ((value == null) ? string.Empty : value);
			}
		}

		/// <summary>Gets or sets the system identifier.</summary>
		/// <returns>The system identifier.</returns>
		public string SystemId
		{
			get
			{
				return (this.systemID == null) ? ((this.dtd == null) ? null : this.dtd.SystemId) : this.systemID;
			}
			set
			{
				this.systemID = ((value == null) ? string.Empty : value);
			}
		}

		/// <summary>Gets or sets the current xml:lang scope.</summary>
		/// <returns>The current xml:lang scope. If there is no xml:lang in scope, String.Empty is returned.</returns>
		public string XmlLang
		{
			get
			{
				return this.xmlLang;
			}
			set
			{
				this.xmlLang = ((value == null) ? string.Empty : value);
			}
		}

		/// <summary>Gets or sets the current xml:space scope.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlSpace" /> value indicating the xml:space scope.</returns>
		public XmlSpace XmlSpace
		{
			get
			{
				return this.xmlSpace;
			}
			set
			{
				this.xmlSpace = value;
			}
		}

		internal void PushScope()
		{
			XmlParserContext.ContextItem contextItem;
			if (this.contextItems.Count == this.contextItemCount)
			{
				contextItem = new XmlParserContext.ContextItem();
				this.contextItems.Add(contextItem);
			}
			else
			{
				contextItem = (XmlParserContext.ContextItem)this.contextItems[this.contextItemCount];
			}
			contextItem.BaseURI = this.BaseURI;
			contextItem.XmlLang = this.XmlLang;
			contextItem.XmlSpace = this.XmlSpace;
			this.contextItemCount++;
		}

		internal void PopScope()
		{
			if (this.contextItemCount == 0)
			{
				throw new XmlException("Unexpected end of element scope.");
			}
			this.contextItemCount--;
			XmlParserContext.ContextItem contextItem = (XmlParserContext.ContextItem)this.contextItems[this.contextItemCount];
			this.baseURI = contextItem.BaseURI;
			this.xmlLang = contextItem.XmlLang;
			this.xmlSpace = contextItem.XmlSpace;
		}

		private class ContextItem
		{
			public string BaseURI;

			public string XmlLang;

			public XmlSpace XmlSpace;
		}
	}
}
