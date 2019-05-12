using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Mono.Xml2
{
	internal class XmlTextReader : XmlReader, IHasXmlParserContext, IXmlLineInfo, IXmlNamespaceResolver
	{
		private const int peekCharCapacity = 1024;

		private XmlTextReader.XmlTokenInfo cursorToken;

		private XmlTextReader.XmlTokenInfo currentToken;

		private XmlTextReader.XmlAttributeTokenInfo currentAttributeToken;

		private XmlTextReader.XmlTokenInfo currentAttributeValueToken;

		private XmlTextReader.XmlAttributeTokenInfo[] attributeTokens = new XmlTextReader.XmlAttributeTokenInfo[10];

		private XmlTextReader.XmlTokenInfo[] attributeValueTokens = new XmlTextReader.XmlTokenInfo[10];

		private int currentAttribute;

		private int currentAttributeValue;

		private int attributeCount;

		private XmlParserContext parserContext;

		private XmlNameTable nameTable;

		private XmlNamespaceManager nsmgr;

		private ReadState readState;

		private bool disallowReset;

		private int depth;

		private int elementDepth;

		private bool depthUp;

		private bool popScope;

		private XmlTextReader.TagName[] elementNames;

		private int elementNameStackPos;

		private bool allowMultipleRoot;

		private bool isStandalone;

		private bool returnEntityReference;

		private string entityReferenceName;

		private StringBuilder valueBuffer;

		private TextReader reader;

		private char[] peekChars;

		private int peekCharsIndex;

		private int peekCharsLength;

		private int curNodePeekIndex;

		private bool preserveCurrentTag;

		private int line;

		private int column;

		private int currentLinkedNodeLineNumber;

		private int currentLinkedNodeLinePosition;

		private bool useProceedingLineInfo;

		private XmlNodeType startNodeType;

		private XmlNodeType currentState;

		private int nestLevel;

		private bool readCharsInProgress;

		private XmlReaderBinarySupport.CharGetter binaryCharGetter;

		private bool namespaces = true;

		private WhitespaceHandling whitespaceHandling;

		private XmlResolver resolver = new XmlUrlResolver();

		private bool normalization;

		private bool checkCharacters;

		private bool prohibitDtd;

		private bool closeInput = true;

		private EntityHandling entityHandling;

		private NameTable whitespacePool;

		private char[] whitespaceCache;

		private XmlTextReader.DtdInputStateStack stateStack = new XmlTextReader.DtdInputStateStack();

		protected XmlTextReader()
		{
		}

		public XmlTextReader(Stream input) : this(new XmlStreamReader(input))
		{
		}

		public XmlTextReader(string url) : this(url, new NameTable())
		{
		}

		public XmlTextReader(TextReader input) : this(input, new NameTable())
		{
		}

		protected XmlTextReader(XmlNameTable nt) : this(string.Empty, null, XmlNodeType.None, null)
		{
		}

		public XmlTextReader(Stream input, XmlNameTable nt) : this(new XmlStreamReader(input), nt)
		{
		}

		public XmlTextReader(string url, Stream input) : this(url, new XmlStreamReader(input))
		{
		}

		public XmlTextReader(string url, TextReader input) : this(url, input, new NameTable())
		{
		}

		public XmlTextReader(string url, XmlNameTable nt)
		{
			string url2;
			Stream streamFromUrl = this.GetStreamFromUrl(url, out url2);
			XmlParserContext context = new XmlParserContext(nt, new XmlNamespaceManager(nt), string.Empty, XmlSpace.None);
			this.InitializeContext(url2, context, new XmlStreamReader(streamFromUrl), XmlNodeType.Document);
		}

		public XmlTextReader(TextReader input, XmlNameTable nt) : this(string.Empty, input, nt)
		{
		}

		internal XmlTextReader(bool dummy, XmlResolver resolver, string url, XmlNodeType fragType, XmlParserContext context)
		{
			if (resolver == null)
			{
				resolver = new XmlUrlResolver();
			}
			this.XmlResolver = resolver;
			string url2;
			Stream streamFromUrl = this.GetStreamFromUrl(url, out url2);
			this.InitializeContext(url2, context, new XmlStreamReader(streamFromUrl), fragType);
		}

		public XmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context) : this((context == null) ? string.Empty : context.BaseURI, new XmlStreamReader(xmlFragment), fragType, context)
		{
			this.disallowReset = true;
		}

		internal XmlTextReader(string baseURI, TextReader xmlFragment, XmlNodeType fragType) : this(baseURI, xmlFragment, fragType, null)
		{
		}

		public XmlTextReader(string url, Stream input, XmlNameTable nt) : this(url, new XmlStreamReader(input), nt)
		{
		}

		public XmlTextReader(string url, TextReader input, XmlNameTable nt) : this(url, input, XmlNodeType.Document, null)
		{
		}

		public XmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context) : this((context == null) ? string.Empty : context.BaseURI, new StringReader(xmlFragment), fragType, context)
		{
			this.disallowReset = true;
		}

		internal XmlTextReader(string url, TextReader fragment, XmlNodeType fragType, XmlParserContext context)
		{
			this.InitializeContext(url, context, fragment, fragType);
		}

		XmlParserContext IHasXmlParserContext.ParserContext
		{
			get
			{
				return this.parserContext;
			}
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.GetNamespacesInScope(scope);
		}

		string IXmlNamespaceResolver.LookupPrefix(string ns)
		{
			return this.LookupPrefix(ns, false);
		}

		private Stream GetStreamFromUrl(string url, out string absoluteUriString)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.Length == 0)
			{
				throw new ArgumentException("url");
			}
			Uri uri = this.resolver.ResolveUri(null, url);
			absoluteUriString = ((!(uri != null)) ? string.Empty : uri.ToString());
			return this.resolver.GetEntity(uri, null, typeof(Stream)) as Stream;
		}

		public override int AttributeCount
		{
			get
			{
				return this.attributeCount;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.parserContext.BaseURI;
			}
		}

		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		public override bool CanReadValueChunk
		{
			get
			{
				return true;
			}
		}

		internal bool CharacterChecking
		{
			get
			{
				return this.checkCharacters;
			}
			set
			{
				this.checkCharacters = value;
			}
		}

		internal bool CloseInput
		{
			get
			{
				return this.closeInput;
			}
			set
			{
				this.closeInput = value;
			}
		}

		public override int Depth
		{
			get
			{
				int num = (this.currentToken.NodeType != XmlNodeType.Element) ? -1 : 0;
				if (this.currentAttributeValue >= 0)
				{
					return num + this.elementDepth + 2;
				}
				if (this.currentAttribute >= 0)
				{
					return num + this.elementDepth + 1;
				}
				return this.elementDepth;
			}
		}

		public Encoding Encoding
		{
			get
			{
				return this.parserContext.Encoding;
			}
		}

		public EntityHandling EntityHandling
		{
			get
			{
				return this.entityHandling;
			}
			set
			{
				this.entityHandling = value;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.readState == ReadState.EndOfFile;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.cursorToken.Value != null;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return false;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.cursorToken.IsEmptyElement;
			}
		}

		public int LineNumber
		{
			get
			{
				if (this.useProceedingLineInfo)
				{
					return this.line;
				}
				return this.cursorToken.LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				if (this.useProceedingLineInfo)
				{
					return this.column;
				}
				return this.cursorToken.LinePosition;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.cursorToken.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				return this.cursorToken.Name;
			}
		}

		public bool Namespaces
		{
			get
			{
				return this.namespaces;
			}
			set
			{
				if (this.readState != ReadState.Initial)
				{
					throw new InvalidOperationException("Namespaces have to be set before reading.");
				}
				this.namespaces = value;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.cursorToken.NamespaceURI;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return this.cursorToken.NodeType;
			}
		}

		public bool Normalization
		{
			get
			{
				return this.normalization;
			}
			set
			{
				this.normalization = value;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.cursorToken.Prefix;
			}
		}

		public bool ProhibitDtd
		{
			get
			{
				return this.prohibitDtd;
			}
			set
			{
				this.prohibitDtd = value;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this.cursorToken.QuoteChar;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.readState;
			}
		}

		public override XmlReaderSettings Settings
		{
			get
			{
				return base.Settings;
			}
		}

		public override string Value
		{
			get
			{
				return (this.cursorToken.Value == null) ? string.Empty : this.cursorToken.Value;
			}
		}

		public WhitespaceHandling WhitespaceHandling
		{
			get
			{
				return this.whitespaceHandling;
			}
			set
			{
				this.whitespaceHandling = value;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.parserContext.XmlLang;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				this.resolver = value;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				return this.parserContext.XmlSpace;
			}
		}

		public override void Close()
		{
			this.readState = ReadState.Closed;
			this.cursorToken.Clear();
			this.currentToken.Clear();
			this.attributeCount = 0;
			if (this.closeInput && this.reader != null)
			{
				this.reader.Close();
			}
		}

		public override string GetAttribute(int i)
		{
			if (i >= this.attributeCount)
			{
				throw new ArgumentOutOfRangeException("i is smaller than AttributeCount");
			}
			return this.attributeTokens[i].Value;
		}

		public override string GetAttribute(string name)
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				if (this.attributeTokens[i].Name == name)
				{
					return this.attributeTokens[i].Value;
				}
			}
			return null;
		}

		private int GetIndexOfQualifiedAttribute(string localName, string namespaceURI)
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				XmlTextReader.XmlAttributeTokenInfo xmlAttributeTokenInfo = this.attributeTokens[i];
				if (xmlAttributeTokenInfo.LocalName == localName && xmlAttributeTokenInfo.NamespaceURI == namespaceURI)
				{
					return i;
				}
			}
			return -1;
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			int indexOfQualifiedAttribute = this.GetIndexOfQualifiedAttribute(localName, namespaceURI);
			if (indexOfQualifiedAttribute < 0)
			{
				return null;
			}
			return this.attributeTokens[indexOfQualifiedAttribute].Value;
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.nsmgr.GetNamespacesInScope(scope);
		}

		public TextReader GetRemainder()
		{
			if (this.peekCharsLength < 0)
			{
				return this.reader;
			}
			return new StringReader(new string(this.peekChars, this.peekCharsIndex, this.peekCharsLength - this.peekCharsIndex) + this.reader.ReadToEnd());
		}

		public bool HasLineInfo()
		{
			return true;
		}

		public override string LookupNamespace(string prefix)
		{
			return this.LookupNamespace(prefix, false);
		}

		private string LookupNamespace(string prefix, bool atomizedNames)
		{
			string text = this.nsmgr.LookupNamespace(prefix, atomizedNames);
			return (!(text == string.Empty)) ? text : null;
		}

		public string LookupPrefix(string ns, bool atomizedName)
		{
			return this.nsmgr.LookupPrefix(ns, atomizedName);
		}

		public override void MoveToAttribute(int i)
		{
			if (i >= this.attributeCount)
			{
				throw new ArgumentOutOfRangeException("attribute index out of range.");
			}
			this.currentAttribute = i;
			this.currentAttributeValue = -1;
			this.cursorToken = this.attributeTokens[i];
		}

		public override bool MoveToAttribute(string name)
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				XmlTextReader.XmlAttributeTokenInfo xmlAttributeTokenInfo = this.attributeTokens[i];
				if (xmlAttributeTokenInfo.Name == name)
				{
					this.MoveToAttribute(i);
					return true;
				}
			}
			return false;
		}

		public override bool MoveToAttribute(string localName, string namespaceName)
		{
			int indexOfQualifiedAttribute = this.GetIndexOfQualifiedAttribute(localName, namespaceName);
			if (indexOfQualifiedAttribute < 0)
			{
				return false;
			}
			this.MoveToAttribute(indexOfQualifiedAttribute);
			return true;
		}

		public override bool MoveToElement()
		{
			if (this.currentToken == null)
			{
				return false;
			}
			if (this.cursorToken == this.currentToken)
			{
				return false;
			}
			if (this.currentAttribute >= 0)
			{
				this.currentAttribute = -1;
				this.currentAttributeValue = -1;
				this.cursorToken = this.currentToken;
				return true;
			}
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			if (this.attributeCount == 0)
			{
				return false;
			}
			this.MoveToElement();
			return this.MoveToNextAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			if (this.currentAttribute == 0 && this.attributeCount == 0)
			{
				return false;
			}
			if (this.currentAttribute + 1 < this.attributeCount)
			{
				this.currentAttribute++;
				this.currentAttributeValue = -1;
				this.cursorToken = this.attributeTokens[this.currentAttribute];
				return true;
			}
			return false;
		}

		public override bool Read()
		{
			if (this.readState == ReadState.Closed)
			{
				return false;
			}
			this.curNodePeekIndex = this.peekCharsIndex;
			this.preserveCurrentTag = true;
			this.nestLevel = 0;
			this.ClearValueBuffer();
			if (this.startNodeType == XmlNodeType.Attribute)
			{
				if (this.currentAttribute == 0)
				{
					return false;
				}
				this.SkipTextDeclaration();
				this.ClearAttributes();
				this.IncrementAttributeToken();
				this.ReadAttributeValueTokens(34);
				this.cursorToken = this.attributeTokens[0];
				this.currentAttributeValue = -1;
				this.readState = ReadState.Interactive;
				return true;
			}
			else
			{
				if (this.readState == ReadState.Initial && this.currentState == XmlNodeType.Element)
				{
					this.SkipTextDeclaration();
				}
				if (base.Binary != null)
				{
					base.Binary.Reset();
				}
				this.readState = ReadState.Interactive;
				this.currentLinkedNodeLineNumber = this.line;
				this.currentLinkedNodeLinePosition = this.column;
				this.useProceedingLineInfo = true;
				this.cursorToken = this.currentToken;
				this.attributeCount = 0;
				this.currentAttribute = (this.currentAttributeValue = -1);
				this.currentToken.Clear();
				if (this.depthUp)
				{
					this.depth++;
					this.depthUp = false;
				}
				if (this.readCharsInProgress)
				{
					this.readCharsInProgress = false;
					return this.ReadUntilEndTag();
				}
				bool flag = this.ReadContent();
				if (!flag && this.startNodeType == XmlNodeType.Document && this.currentState != XmlNodeType.EndElement)
				{
					throw this.NotWFError("Document element did not appear.");
				}
				this.useProceedingLineInfo = false;
				return flag;
			}
		}

		public override bool ReadAttributeValue()
		{
			if (this.readState == ReadState.Initial && this.startNodeType == XmlNodeType.Attribute)
			{
				this.Read();
			}
			if (this.currentAttribute < 0)
			{
				return false;
			}
			XmlTextReader.XmlAttributeTokenInfo xmlAttributeTokenInfo = this.attributeTokens[this.currentAttribute];
			if (this.currentAttributeValue < 0)
			{
				this.currentAttributeValue = xmlAttributeTokenInfo.ValueTokenStartIndex - 1;
			}
			if (this.currentAttributeValue < xmlAttributeTokenInfo.ValueTokenEndIndex)
			{
				this.currentAttributeValue++;
				this.cursorToken = this.attributeValueTokens[this.currentAttributeValue];
				return true;
			}
			return false;
		}

		public int ReadBase64(byte[] buffer, int offset, int length)
		{
			base.BinaryCharGetter = this.binaryCharGetter;
			int result;
			try
			{
				result = base.Binary.ReadBase64(buffer, offset, length);
			}
			finally
			{
				base.BinaryCharGetter = null;
			}
			return result;
		}

		public int ReadBinHex(byte[] buffer, int offset, int length)
		{
			base.BinaryCharGetter = this.binaryCharGetter;
			int result;
			try
			{
				result = base.Binary.ReadBinHex(buffer, offset, length);
			}
			finally
			{
				base.BinaryCharGetter = null;
			}
			return result;
		}

		public int ReadChars(char[] buffer, int offset, int length)
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("Offset must be non-negative integer.");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("Length must be non-negative integer.");
			}
			if (buffer.Length < offset + length)
			{
				throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
			}
			if (this.IsEmptyElement)
			{
				this.Read();
				return 0;
			}
			if (!this.readCharsInProgress && this.NodeType != XmlNodeType.Element)
			{
				return 0;
			}
			this.preserveCurrentTag = false;
			this.readCharsInProgress = true;
			this.useProceedingLineInfo = true;
			return this.ReadCharsInternal(buffer, offset, length);
		}

		public void ResetState()
		{
			if (this.disallowReset)
			{
				throw new InvalidOperationException("Cannot call ResetState when parsing an XML fragment.");
			}
			this.Clear();
		}

		public override void ResolveEntity()
		{
			throw new InvalidOperationException("XmlTextReader cannot resolve external entities.");
		}

		[MonoTODO]
		public override void Skip()
		{
			base.Skip();
		}

		internal DTDObjectModel DTD
		{
			get
			{
				return this.parserContext.Dtd;
			}
		}

		internal XmlResolver Resolver
		{
			get
			{
				return this.resolver;
			}
		}

		private XmlException NotWFError(string message)
		{
			return new XmlException(this, this.BaseURI, message);
		}

		private void Init()
		{
			this.allowMultipleRoot = false;
			this.elementNames = new XmlTextReader.TagName[10];
			this.valueBuffer = new StringBuilder();
			this.binaryCharGetter = new XmlReaderBinarySupport.CharGetter(this.ReadChars);
			this.checkCharacters = true;
			if (this.Settings != null)
			{
				this.checkCharacters = this.Settings.CheckCharacters;
			}
			this.prohibitDtd = false;
			this.closeInput = true;
			this.entityHandling = EntityHandling.ExpandCharEntities;
			this.peekCharsIndex = 0;
			if (this.peekChars == null)
			{
				this.peekChars = new char[1024];
			}
			this.peekCharsLength = -1;
			this.curNodePeekIndex = -1;
			this.line = 1;
			this.column = 1;
			this.currentLinkedNodeLineNumber = (this.currentLinkedNodeLinePosition = 0);
			this.Clear();
		}

		private void Clear()
		{
			this.currentToken = new XmlTextReader.XmlTokenInfo(this);
			this.cursorToken = this.currentToken;
			this.currentAttribute = -1;
			this.currentAttributeValue = -1;
			this.attributeCount = 0;
			this.readState = ReadState.Initial;
			this.depth = 0;
			this.elementDepth = 0;
			this.depthUp = false;
			this.popScope = (this.allowMultipleRoot = false);
			this.elementNameStackPos = 0;
			this.isStandalone = false;
			this.returnEntityReference = false;
			this.entityReferenceName = string.Empty;
			this.useProceedingLineInfo = false;
			this.currentState = XmlNodeType.None;
			this.readCharsInProgress = false;
		}

		private void InitializeContext(string url, XmlParserContext context, TextReader fragment, XmlNodeType fragType)
		{
			this.startNodeType = fragType;
			this.parserContext = context;
			if (context == null)
			{
				XmlNameTable nt = new NameTable();
				this.parserContext = new XmlParserContext(nt, new XmlNamespaceManager(nt), string.Empty, XmlSpace.None);
			}
			this.nameTable = this.parserContext.NameTable;
			this.nameTable = ((this.nameTable == null) ? new NameTable() : this.nameTable);
			this.nsmgr = this.parserContext.NamespaceManager;
			this.nsmgr = ((this.nsmgr == null) ? new XmlNamespaceManager(this.nameTable) : this.nsmgr);
			if (url != null && url.Length > 0)
			{
				Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);
				this.parserContext.BaseURI = uri.ToString();
			}
			this.Init();
			this.reader = fragment;
			if (fragType != XmlNodeType.Element)
			{
				if (fragType != XmlNodeType.Attribute)
				{
					if (fragType != XmlNodeType.Document)
					{
						throw new XmlException(string.Format("NodeType {0} is not allowed to create XmlTextReader.", fragType));
					}
				}
				else
				{
					this.reader = new StringReader(fragment.ReadToEnd().Replace("\"", "&quot;"));
				}
			}
			else
			{
				this.currentState = XmlNodeType.Element;
				this.allowMultipleRoot = true;
			}
		}

		internal ConformanceLevel Conformance
		{
			get
			{
				return (!this.allowMultipleRoot) ? ConformanceLevel.Document : ConformanceLevel.Fragment;
			}
			set
			{
				if (value == ConformanceLevel.Fragment)
				{
					this.currentState = XmlNodeType.Element;
					this.allowMultipleRoot = true;
				}
			}
		}

		internal void AdjustLineInfoOffset(int lineNumberOffset, int linePositionOffset)
		{
			this.line += lineNumberOffset;
			this.column += linePositionOffset;
		}

		internal void SetNameTable(XmlNameTable nameTable)
		{
			this.parserContext.NameTable = nameTable;
		}

		private void SetProperties(XmlNodeType nodeType, string name, string prefix, string localName, bool isEmptyElement, string value, bool clearAttributes)
		{
			this.SetTokenProperties(this.currentToken, nodeType, name, prefix, localName, isEmptyElement, value, clearAttributes);
			this.currentToken.LineNumber = this.currentLinkedNodeLineNumber;
			this.currentToken.LinePosition = this.currentLinkedNodeLinePosition;
		}

		private void SetTokenProperties(XmlTextReader.XmlTokenInfo token, XmlNodeType nodeType, string name, string prefix, string localName, bool isEmptyElement, string value, bool clearAttributes)
		{
			token.NodeType = nodeType;
			token.Name = name;
			token.Prefix = prefix;
			token.LocalName = localName;
			token.IsEmptyElement = isEmptyElement;
			token.Value = value;
			this.elementDepth = this.depth;
			if (clearAttributes)
			{
				this.ClearAttributes();
			}
		}

		private void ClearAttributes()
		{
			this.attributeCount = 0;
			this.currentAttribute = -1;
			this.currentAttributeValue = -1;
		}

		private int PeekSurrogate(int c)
		{
			if (this.peekCharsLength <= this.peekCharsIndex + 1 && !this.ReadTextReader(c))
			{
				return c;
			}
			int num = (int)this.peekChars[this.peekCharsIndex];
			int num2 = (int)this.peekChars[this.peekCharsIndex + 1];
			if ((num & 64512) != 55296 || (num2 & 64512) != 56320)
			{
				return num;
			}
			return 65536 + (num - 55296) * 1024 + (num2 - 56320);
		}

		private int PeekChar()
		{
			if (this.peekCharsIndex < this.peekCharsLength)
			{
				int num = (int)this.peekChars[this.peekCharsIndex];
				if (num == 0)
				{
					return -1;
				}
				if (num < 55296 || num >= 57343)
				{
					return num;
				}
				return this.PeekSurrogate(num);
			}
			else
			{
				if (!this.ReadTextReader(-1))
				{
					return -1;
				}
				return this.PeekChar();
			}
		}

		private int ReadChar()
		{
			int num = this.PeekChar();
			this.peekCharsIndex++;
			if (num >= 65536)
			{
				this.peekCharsIndex++;
			}
			if (num == 10)
			{
				this.line++;
				this.column = 1;
			}
			else if (num != -1)
			{
				this.column++;
			}
			return num;
		}

		private void Advance(int ch)
		{
			this.peekCharsIndex++;
			if (ch >= 65536)
			{
				this.peekCharsIndex++;
			}
			if (ch == 10)
			{
				this.line++;
				this.column = 1;
			}
			else if (ch != -1)
			{
				this.column++;
			}
		}

		private bool ReadTextReader(int remained)
		{
			if (this.peekCharsLength < 0)
			{
				this.peekCharsLength = this.reader.Read(this.peekChars, 0, this.peekChars.Length);
				return this.peekCharsLength > 0;
			}
			int num = (remained < 0) ? 0 : 1;
			int length = this.peekCharsLength - this.curNodePeekIndex;
			if (!this.preserveCurrentTag)
			{
				this.curNodePeekIndex = 0;
				this.peekCharsIndex = 0;
			}
			else if (this.peekCharsLength >= this.peekChars.Length)
			{
				if (this.curNodePeekIndex <= this.peekCharsLength >> 1)
				{
					char[] destinationArray = new char[this.peekChars.Length * 2];
					Array.Copy(this.peekChars, this.curNodePeekIndex, destinationArray, 0, length);
					this.peekChars = destinationArray;
					this.curNodePeekIndex = 0;
					this.peekCharsIndex = length;
				}
				else
				{
					Array.Copy(this.peekChars, this.curNodePeekIndex, this.peekChars, 0, length);
					this.curNodePeekIndex = 0;
					this.peekCharsIndex = length;
				}
			}
			if (remained >= 0)
			{
				this.peekChars[this.peekCharsIndex] = (char)remained;
			}
			int num2 = this.peekChars.Length - this.peekCharsIndex - num;
			if (num2 > 1024)
			{
				num2 = 1024;
			}
			int num3 = this.reader.Read(this.peekChars, this.peekCharsIndex + num, num2);
			int num4 = num + num3;
			this.peekCharsLength = this.peekCharsIndex + num4;
			return num4 != 0;
		}

		private bool ReadContent()
		{
			if (this.popScope)
			{
				this.nsmgr.PopScope();
				this.parserContext.PopScope();
				this.popScope = false;
			}
			if (this.returnEntityReference)
			{
				this.SetEntityReferenceProperties();
			}
			else
			{
				int num = this.PeekChar();
				if (num == -1)
				{
					this.readState = ReadState.EndOfFile;
					this.ClearValueBuffer();
					this.SetProperties(XmlNodeType.None, string.Empty, string.Empty, string.Empty, false, null, true);
					if (this.depth > 0)
					{
						throw this.NotWFError("unexpected end of file. Current depth is " + this.depth);
					}
					return false;
				}
				else
				{
					int num2 = num;
					switch (num2)
					{
					case 9:
					case 10:
					case 13:
						break;
					default:
						if (num2 != 32)
						{
							if (num2 != 60)
							{
								this.ReadText(true);
								goto IL_168;
							}
							this.Advance(num);
							int num3 = this.PeekChar();
							if (num3 != 33)
							{
								if (num3 != 47)
								{
									if (num3 != 63)
									{
										this.ReadStartTag();
									}
									else
									{
										this.Advance(63);
										this.ReadProcessingInstruction();
									}
								}
								else
								{
									this.Advance(47);
									this.ReadEndTag();
								}
							}
							else
							{
								this.Advance(33);
								this.ReadDeclaration();
							}
							goto IL_168;
						}
						break;
					}
					if (!this.ReadWhitespace())
					{
						return this.ReadContent();
					}
				}
			}
			IL_168:
			return this.ReadState != ReadState.EndOfFile;
		}

		private void SetEntityReferenceProperties()
		{
			DTDEntityDeclaration dtdentityDeclaration = (this.DTD == null) ? null : this.DTD.EntityDecls[this.entityReferenceName];
			if (this.isStandalone && (this.DTD == null || dtdentityDeclaration == null || !dtdentityDeclaration.IsInternalSubset))
			{
				throw this.NotWFError("Standalone document must not contain any references to an non-internally declared entity.");
			}
			if (dtdentityDeclaration != null && dtdentityDeclaration.NotationName != null)
			{
				throw this.NotWFError("Reference to any unparsed entities is not allowed here.");
			}
			this.ClearValueBuffer();
			this.SetProperties(XmlNodeType.EntityReference, this.entityReferenceName, string.Empty, this.entityReferenceName, false, null, true);
			this.returnEntityReference = false;
			this.entityReferenceName = string.Empty;
		}

		private void ReadStartTag()
		{
			if (this.currentState == XmlNodeType.EndElement)
			{
				throw this.NotWFError("Multiple document element was detected.");
			}
			this.currentState = XmlNodeType.Element;
			this.nsmgr.PushScope();
			this.currentLinkedNodeLineNumber = this.line;
			this.currentLinkedNodeLinePosition = this.column;
			string text;
			string text2;
			string name = this.ReadName(out text, out text2);
			if (this.currentState == XmlNodeType.EndElement)
			{
				throw this.NotWFError("document has terminated, cannot open new element");
			}
			bool isEmptyElement = false;
			this.ClearAttributes();
			this.SkipWhitespace();
			if (XmlChar.IsFirstNameChar(this.PeekChar()))
			{
				this.ReadAttributes(false);
			}
			this.cursorToken = this.currentToken;
			for (int i = 0; i < this.attributeCount; i++)
			{
				this.attributeTokens[i].FillXmlns();
			}
			for (int j = 0; j < this.attributeCount; j++)
			{
				this.attributeTokens[j].FillNamespace();
			}
			if (this.namespaces)
			{
				for (int k = 0; k < this.attributeCount; k++)
				{
					if (this.attributeTokens[k].Prefix == "xmlns" && this.attributeTokens[k].Value == string.Empty)
					{
						throw this.NotWFError("Empty namespace URI cannot be mapped to non-empty prefix.");
					}
				}
			}
			for (int l = 0; l < this.attributeCount; l++)
			{
				for (int m = l + 1; m < this.attributeCount; m++)
				{
					if (object.ReferenceEquals(this.attributeTokens[l].Name, this.attributeTokens[m].Name) || (object.ReferenceEquals(this.attributeTokens[l].LocalName, this.attributeTokens[m].LocalName) && object.ReferenceEquals(this.attributeTokens[l].NamespaceURI, this.attributeTokens[m].NamespaceURI)))
					{
						throw this.NotWFError("Attribute name and qualified name must be identical.");
					}
				}
			}
			if (this.PeekChar() == 47)
			{
				this.Advance(47);
				isEmptyElement = true;
				this.popScope = true;
			}
			else
			{
				this.depthUp = true;
				this.PushElementName(name, text2, text);
			}
			this.parserContext.PushScope();
			this.Expect(62);
			this.SetProperties(XmlNodeType.Element, name, text, text2, isEmptyElement, null, false);
			if (text.Length > 0)
			{
				this.currentToken.NamespaceURI = this.LookupNamespace(text, true);
			}
			else if (this.namespaces)
			{
				this.currentToken.NamespaceURI = this.nsmgr.DefaultNamespace;
			}
			if (this.namespaces)
			{
				if (this.NamespaceURI == null)
				{
					throw this.NotWFError(string.Format("'{0}' is undeclared namespace.", this.Prefix));
				}
				try
				{
					for (int n = 0; n < this.attributeCount; n++)
					{
						this.MoveToAttribute(n);
						if (this.NamespaceURI == null)
						{
							throw this.NotWFError(string.Format("'{0}' is undeclared namespace.", this.Prefix));
						}
					}
				}
				finally
				{
					this.MoveToElement();
				}
			}
			for (int num = 0; num < this.attributeCount; num++)
			{
				if (object.ReferenceEquals(this.attributeTokens[num].Prefix, "xml"))
				{
					string localName = this.attributeTokens[num].LocalName;
					string value = this.attributeTokens[num].Value;
					string text3 = localName;
					switch (text3)
					{
					case "base":
						if (this.resolver != null)
						{
							Uri baseUri = (!(this.BaseURI != string.Empty)) ? null : new Uri(this.BaseURI);
							Uri uri = this.resolver.ResolveUri(baseUri, value);
							this.parserContext.BaseURI = ((!(uri != null)) ? string.Empty : uri.ToString());
						}
						else
						{
							this.parserContext.BaseURI = value;
						}
						break;
					case "lang":
						this.parserContext.XmlLang = value;
						break;
					case "space":
					{
						string text4 = value;
						if (text4 != null)
						{
							if (XmlTextReader.<>f__switch$map51 == null)
							{
								XmlTextReader.<>f__switch$map51 = new Dictionary<string, int>(2)
								{
									{
										"preserve",
										0
									},
									{
										"default",
										1
									}
								};
							}
							int num3;
							if (XmlTextReader.<>f__switch$map51.TryGetValue(text4, out num3))
							{
								if (num3 != 0)
								{
									if (num3 != 1)
									{
										goto IL_502;
									}
									this.parserContext.XmlSpace = XmlSpace.Default;
								}
								else
								{
									this.parserContext.XmlSpace = XmlSpace.Preserve;
								}
								break;
							}
						}
						IL_502:
						throw this.NotWFError(string.Format("Invalid xml:space value: {0}", value));
					}
					}
				}
			}
			if (this.IsEmptyElement)
			{
				this.CheckCurrentStateUpdate();
			}
		}

		private void PushElementName(string name, string local, string prefix)
		{
			if (this.elementNames.Length == this.elementNameStackPos)
			{
				XmlTextReader.TagName[] destinationArray = new XmlTextReader.TagName[this.elementNames.Length * 2];
				Array.Copy(this.elementNames, 0, destinationArray, 0, this.elementNameStackPos);
				this.elementNames = destinationArray;
			}
			this.elementNames[this.elementNameStackPos++] = new XmlTextReader.TagName(name, local, prefix);
		}

		private void ReadEndTag()
		{
			if (this.currentState != XmlNodeType.Element)
			{
				throw this.NotWFError("End tag cannot appear in this state.");
			}
			this.currentLinkedNodeLineNumber = this.line;
			this.currentLinkedNodeLinePosition = this.column;
			if (this.elementNameStackPos == 0)
			{
				throw this.NotWFError("closing element without matching opening element");
			}
			XmlTextReader.TagName tagName = this.elementNames[--this.elementNameStackPos];
			this.Expect(tagName.Name);
			this.ExpectAfterWhitespace('>');
			this.depth--;
			this.SetProperties(XmlNodeType.EndElement, tagName.Name, tagName.Prefix, tagName.LocalName, false, null, true);
			if (tagName.Prefix.Length > 0)
			{
				this.currentToken.NamespaceURI = this.LookupNamespace(tagName.Prefix, true);
			}
			else if (this.namespaces)
			{
				this.currentToken.NamespaceURI = this.nsmgr.DefaultNamespace;
			}
			this.popScope = true;
			this.CheckCurrentStateUpdate();
		}

		private void CheckCurrentStateUpdate()
		{
			if (this.depth == 0 && !this.allowMultipleRoot && (this.IsEmptyElement || this.NodeType == XmlNodeType.EndElement))
			{
				this.currentState = XmlNodeType.EndElement;
			}
		}

		private void AppendValueChar(int ch)
		{
			if (ch < 65535)
			{
				this.valueBuffer.Append((char)ch);
			}
			else
			{
				this.AppendSurrogatePairValueChar(ch);
			}
		}

		private void AppendSurrogatePairValueChar(int ch)
		{
			this.valueBuffer.Append((char)((ch - 65536) / 1024 + 55296));
			this.valueBuffer.Append((char)((ch - 65536) % 1024 + 56320));
		}

		private string CreateValueString()
		{
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
			{
				int length = this.valueBuffer.Length;
				if (this.whitespaceCache == null)
				{
					this.whitespaceCache = new char[32];
				}
				if (length < this.whitespaceCache.Length)
				{
					if (this.whitespacePool == null)
					{
						this.whitespacePool = new NameTable();
					}
					for (int i = 0; i < length; i++)
					{
						this.whitespaceCache[i] = this.valueBuffer[i];
					}
					return this.whitespacePool.Add(this.whitespaceCache, 0, this.valueBuffer.Length);
				}
			}
			return (this.valueBuffer.Capacity >= 100) ? this.valueBuffer.ToString() : this.valueBuffer.ToString(0, this.valueBuffer.Length);
		}

		private void ClearValueBuffer()
		{
			this.valueBuffer.Length = 0;
		}

		private void ReadText(bool notWhitespace)
		{
			if (this.currentState != XmlNodeType.Element)
			{
				throw this.NotWFError("Text node cannot appear in this state.");
			}
			this.preserveCurrentTag = false;
			if (notWhitespace)
			{
				this.ClearValueBuffer();
			}
			int num = this.PeekChar();
			bool flag = false;
			while (num != 60 && num != -1)
			{
				if (num == 38)
				{
					this.ReadChar();
					num = this.ReadReference(false);
					if (this.returnEntityReference)
					{
						break;
					}
				}
				else
				{
					if (this.normalization && num == 13)
					{
						this.ReadChar();
						num = this.PeekChar();
						if (num != 10)
						{
							this.AppendValueChar(10);
						}
						continue;
					}
					if (this.CharacterChecking && XmlChar.IsInvalid(num))
					{
						throw this.NotWFError("Not allowed character was found.");
					}
					num = this.ReadChar();
				}
				if (num < 65535)
				{
					this.valueBuffer.Append((char)num);
				}
				else
				{
					this.AppendSurrogatePairValueChar(num);
				}
				if (num == 93)
				{
					if (flag && this.PeekChar() == 62)
					{
						throw this.NotWFError("Inside text content, character sequence ']]>' is not allowed.");
					}
					flag = true;
				}
				else if (flag)
				{
					flag = false;
				}
				num = this.PeekChar();
				notWhitespace = true;
			}
			if (this.returnEntityReference && this.valueBuffer.Length == 0)
			{
				this.SetEntityReferenceProperties();
			}
			else
			{
				XmlNodeType nodeType = (!notWhitespace) ? ((this.XmlSpace != XmlSpace.Preserve) ? XmlNodeType.Whitespace : XmlNodeType.SignificantWhitespace) : XmlNodeType.Text;
				this.SetProperties(nodeType, string.Empty, string.Empty, string.Empty, false, null, true);
			}
		}

		private int ReadReference(bool ignoreEntityReferences)
		{
			if (this.PeekChar() == 35)
			{
				this.Advance(35);
				return this.ReadCharacterReference();
			}
			return this.ReadEntityReference(ignoreEntityReferences);
		}

		private int ReadCharacterReference()
		{
			int num = 0;
			if (this.PeekChar() == 120)
			{
				this.Advance(120);
				int num2;
				while ((num2 = this.PeekChar()) != 59 && num2 != -1)
				{
					this.Advance(num2);
					if (num2 >= 48 && num2 <= 57)
					{
						num = (num << 4) + num2 - 48;
					}
					else if (num2 >= 65 && num2 <= 70)
					{
						num = (num << 4) + num2 - 65 + 10;
					}
					else
					{
						if (num2 < 97 || num2 > 102)
						{
							throw this.NotWFError(string.Format(CultureInfo.InvariantCulture, "invalid hexadecimal digit: {0} (#x{1:X})", new object[]
							{
								(char)num2,
								num2
							}));
						}
						num = (num << 4) + num2 - 97 + 10;
					}
				}
			}
			else
			{
				int num2;
				while ((num2 = this.PeekChar()) != 59 && num2 != -1)
				{
					this.Advance(num2);
					if (num2 < 48 || num2 > 57)
					{
						throw this.NotWFError(string.Format(CultureInfo.InvariantCulture, "invalid decimal digit: {0} (#x{1:X})", new object[]
						{
							(char)num2,
							num2
						}));
					}
					num = num * 10 + num2 - 48;
				}
			}
			this.ReadChar();
			if (this.CharacterChecking && this.Normalization && XmlChar.IsInvalid(num))
			{
				throw this.NotWFError(string.Concat(new object[]
				{
					"Referenced character was not allowed in XML. Normalization is ",
					this.normalization,
					", checkCharacters = ",
					this.checkCharacters
				}));
			}
			return num;
		}

		private int ReadEntityReference(bool ignoreEntityReferences)
		{
			string text = this.ReadName();
			this.Expect(59);
			int predefinedEntity = XmlChar.GetPredefinedEntity(text);
			if (predefinedEntity >= 0)
			{
				return predefinedEntity;
			}
			if (ignoreEntityReferences)
			{
				this.AppendValueChar(38);
				for (int i = 0; i < text.Length; i++)
				{
					this.AppendValueChar((int)text[i]);
				}
				this.AppendValueChar(59);
			}
			else
			{
				this.returnEntityReference = true;
				this.entityReferenceName = text;
			}
			return -1;
		}

		private void ReadAttributes(bool isXmlDecl)
		{
			bool flag = false;
			this.currentAttribute = -1;
			this.currentAttributeValue = -1;
			while (this.SkipWhitespace() || !flag)
			{
				this.IncrementAttributeToken();
				this.currentAttributeToken.LineNumber = this.line;
				this.currentAttributeToken.LinePosition = this.column;
				string prefix;
				string localName;
				this.currentAttributeToken.Name = this.ReadName(out prefix, out localName);
				this.currentAttributeToken.Prefix = prefix;
				this.currentAttributeToken.LocalName = localName;
				this.ExpectAfterWhitespace('=');
				this.SkipWhitespace();
				this.ReadAttributeValueTokens(-1);
				if (isXmlDecl)
				{
					string value = this.currentAttributeToken.Value;
				}
				this.attributeCount++;
				if (!this.SkipWhitespace())
				{
					flag = true;
				}
				int num = this.PeekChar();
				if (isXmlDecl)
				{
					if (num == 63)
					{
						goto IL_103;
					}
				}
				else if (num == 47 || num == 62)
				{
					goto IL_103;
				}
				if (num != -1)
				{
					continue;
				}
				IL_103:
				this.currentAttribute = -1;
				this.currentAttributeValue = -1;
				return;
			}
			throw this.NotWFError("Unexpected token. Name is required here.");
		}

		private void AddAttributeWithValue(string name, string value)
		{
			this.IncrementAttributeToken();
			XmlTextReader.XmlAttributeTokenInfo xmlAttributeTokenInfo = this.attributeTokens[this.currentAttribute];
			xmlAttributeTokenInfo.Name = this.NameTable.Add(name);
			xmlAttributeTokenInfo.Prefix = string.Empty;
			xmlAttributeTokenInfo.NamespaceURI = string.Empty;
			this.IncrementAttributeValueToken();
			XmlTextReader.XmlTokenInfo token = this.attributeValueTokens[this.currentAttributeValue];
			this.SetTokenProperties(token, XmlNodeType.Text, string.Empty, string.Empty, string.Empty, false, value, false);
			xmlAttributeTokenInfo.Value = value;
			this.attributeCount++;
		}

		private void IncrementAttributeToken()
		{
			this.currentAttribute++;
			if (this.attributeTokens.Length == this.currentAttribute)
			{
				XmlTextReader.XmlAttributeTokenInfo[] array = new XmlTextReader.XmlAttributeTokenInfo[this.attributeTokens.Length * 2];
				this.attributeTokens.CopyTo(array, 0);
				this.attributeTokens = array;
			}
			if (this.attributeTokens[this.currentAttribute] == null)
			{
				this.attributeTokens[this.currentAttribute] = new XmlTextReader.XmlAttributeTokenInfo(this);
			}
			this.currentAttributeToken = this.attributeTokens[this.currentAttribute];
			this.currentAttributeToken.Clear();
		}

		private void IncrementAttributeValueToken()
		{
			this.currentAttributeValue++;
			if (this.attributeValueTokens.Length == this.currentAttributeValue)
			{
				XmlTextReader.XmlTokenInfo[] array = new XmlTextReader.XmlTokenInfo[this.attributeValueTokens.Length * 2];
				this.attributeValueTokens.CopyTo(array, 0);
				this.attributeValueTokens = array;
			}
			if (this.attributeValueTokens[this.currentAttributeValue] == null)
			{
				this.attributeValueTokens[this.currentAttributeValue] = new XmlTextReader.XmlTokenInfo(this);
			}
			this.currentAttributeValueToken = this.attributeValueTokens[this.currentAttributeValue];
			this.currentAttributeValueToken.Clear();
		}

		private void ReadAttributeValueTokens(int dummyQuoteChar)
		{
			int num = (dummyQuoteChar >= 0) ? dummyQuoteChar : this.ReadChar();
			if (num != 39 && num != 34)
			{
				throw this.NotWFError("an attribute value was not quoted");
			}
			this.currentAttributeToken.QuoteChar = (char)num;
			this.IncrementAttributeValueToken();
			this.currentAttributeToken.ValueTokenStartIndex = this.currentAttributeValue;
			this.currentAttributeValueToken.LineNumber = this.line;
			this.currentAttributeValueToken.LinePosition = this.column;
			bool flag = false;
			bool flag2 = true;
			bool flag3 = true;
			this.currentAttributeValueToken.ValueBufferStart = this.valueBuffer.Length;
			while (flag3)
			{
				int num2 = this.ReadChar();
				if (num2 == num)
				{
					break;
				}
				if (flag)
				{
					this.IncrementAttributeValueToken();
					this.currentAttributeValueToken.ValueBufferStart = this.valueBuffer.Length;
					this.currentAttributeValueToken.LineNumber = this.line;
					this.currentAttributeValueToken.LinePosition = this.column;
					flag = false;
					flag2 = true;
				}
				int num3 = num2;
				switch (num3)
				{
				case 9:
				case 10:
					if (!this.normalization)
					{
						goto IL_2CD;
					}
					num2 = 32;
					goto IL_2CD;
				default:
					if (num3 != -1)
					{
						if (num3 != 38)
						{
							if (num3 != 60)
							{
								goto IL_2CD;
							}
							throw this.NotWFError("attribute values cannot contain '<'");
						}
						else if (this.PeekChar() == 35)
						{
							this.Advance(35);
							num2 = this.ReadCharacterReference();
							this.AppendValueChar(num2);
						}
						else
						{
							string text = this.ReadName();
							this.Expect(59);
							int predefinedEntity = XmlChar.GetPredefinedEntity(text);
							if (predefinedEntity < 0)
							{
								this.CheckAttributeEntityReferenceWFC(text);
								if (this.entityHandling == EntityHandling.ExpandEntities)
								{
									string text2 = this.DTD.GenerateEntityAttributeText(text);
									foreach (char ch in ((IEnumerable<char>)text2))
									{
										this.AppendValueChar((int)ch);
									}
								}
								else
								{
									this.currentAttributeValueToken.ValueBufferEnd = this.valueBuffer.Length;
									this.currentAttributeValueToken.NodeType = XmlNodeType.Text;
									if (!flag2)
									{
										this.IncrementAttributeValueToken();
									}
									this.currentAttributeValueToken.Name = text;
									this.currentAttributeValueToken.Value = string.Empty;
									this.currentAttributeValueToken.NodeType = XmlNodeType.EntityReference;
									flag = true;
								}
							}
							else
							{
								this.AppendValueChar(predefinedEntity);
							}
						}
					}
					else
					{
						if (dummyQuoteChar < 0)
						{
							throw this.NotWFError("unexpected end of file in an attribute value");
						}
						flag3 = false;
					}
					break;
				case 13:
					if (!this.normalization)
					{
						goto IL_2CD;
					}
					if (this.PeekChar() == 10)
					{
						continue;
					}
					if (!this.normalization)
					{
						goto IL_2CD;
					}
					num2 = 32;
					goto IL_2CD;
				}
				IL_31D:
				flag2 = false;
				continue;
				IL_2CD:
				if (this.CharacterChecking && XmlChar.IsInvalid(num2))
				{
					throw this.NotWFError("Invalid character was found.");
				}
				if (num2 < 65535)
				{
					this.valueBuffer.Append((char)num2);
				}
				else
				{
					this.AppendSurrogatePairValueChar(num2);
				}
				goto IL_31D;
			}
			if (!flag)
			{
				this.currentAttributeValueToken.ValueBufferEnd = this.valueBuffer.Length;
				this.currentAttributeValueToken.NodeType = XmlNodeType.Text;
			}
			this.currentAttributeToken.ValueTokenEndIndex = this.currentAttributeValue;
		}

		private void CheckAttributeEntityReferenceWFC(string entName)
		{
			DTDEntityDeclaration dtdentityDeclaration = (this.DTD != null) ? this.DTD.EntityDecls[entName] : null;
			if (dtdentityDeclaration == null)
			{
				if (this.entityHandling == EntityHandling.ExpandEntities || (this.DTD != null && this.resolver != null && dtdentityDeclaration == null))
				{
					throw this.NotWFError(string.Format("Referenced entity '{0}' does not exist.", entName));
				}
				return;
			}
			else
			{
				if (dtdentityDeclaration.HasExternalReference)
				{
					throw this.NotWFError("Reference to external entities is not allowed in the value of an attribute.");
				}
				if (this.isStandalone && !dtdentityDeclaration.IsInternalSubset)
				{
					throw this.NotWFError("Reference to external entities is not allowed in the internal subset.");
				}
				if (dtdentityDeclaration.EntityValue.IndexOf('<') >= 0)
				{
					throw this.NotWFError("Attribute must not contain character '<' either directly or indirectly by way of entity references.");
				}
				return;
			}
		}

		private void ReadProcessingInstruction()
		{
			string text = this.ReadName();
			if (text != "xml" && text.ToLower(CultureInfo.InvariantCulture) == "xml")
			{
				throw this.NotWFError("Not allowed processing instruction name which starts with 'X', 'M', 'L' was found.");
			}
			if (!this.SkipWhitespace() && this.PeekChar() != 63)
			{
				throw this.NotWFError("Invalid processing instruction name was found.");
			}
			this.ClearValueBuffer();
			int num;
			while ((num = this.PeekChar()) != -1)
			{
				this.Advance(num);
				if (num == 63 && this.PeekChar() == 62)
				{
					this.Advance(62);
					break;
				}
				if (this.CharacterChecking && XmlChar.IsInvalid(num))
				{
					throw this.NotWFError("Invalid character was found.");
				}
				this.AppendValueChar(num);
			}
			if (object.ReferenceEquals(text, "xml"))
			{
				this.VerifyXmlDeclaration();
			}
			else
			{
				if (this.currentState == XmlNodeType.None)
				{
					this.currentState = XmlNodeType.XmlDeclaration;
				}
				this.SetProperties(XmlNodeType.ProcessingInstruction, text, string.Empty, text, false, null, true);
			}
		}

		private void VerifyXmlDeclaration()
		{
			if (!this.allowMultipleRoot && this.currentState != XmlNodeType.None)
			{
				throw this.NotWFError("XML declaration cannot appear in this state.");
			}
			this.currentState = XmlNodeType.XmlDeclaration;
			string text = this.CreateValueString();
			this.ClearAttributes();
			int num = 0;
			string text2 = null;
			string text3 = null;
			string empty;
			string text4;
			this.ParseAttributeFromString(text, ref num, out empty, out text4);
			if (empty != "version" || text4 != "1.0")
			{
				throw this.NotWFError("'version' is expected.");
			}
			empty = string.Empty;
			if (this.SkipWhitespaceInString(text, ref num) && num < text.Length)
			{
				this.ParseAttributeFromString(text, ref num, out empty, out text4);
			}
			if (empty == "encoding")
			{
				if (!XmlChar.IsValidIANAEncoding(text4))
				{
					throw this.NotWFError("'encoding' must be a valid IANA encoding name.");
				}
				if (this.reader is XmlStreamReader)
				{
					this.parserContext.Encoding = ((XmlStreamReader)this.reader).Encoding;
				}
				else
				{
					this.parserContext.Encoding = Encoding.Unicode;
				}
				text2 = text4;
				empty = string.Empty;
				if (this.SkipWhitespaceInString(text, ref num) && num < text.Length)
				{
					this.ParseAttributeFromString(text, ref num, out empty, out text4);
				}
			}
			if (empty == "standalone")
			{
				this.isStandalone = (text4 == "yes");
				if (text4 != "yes" && text4 != "no")
				{
					throw this.NotWFError("Only 'yes' or 'no' is allow for 'standalone'");
				}
				text3 = text4;
				this.SkipWhitespaceInString(text, ref num);
			}
			else if (empty.Length != 0)
			{
				throw this.NotWFError(string.Format("Unexpected token: '{0}'", empty));
			}
			if (num < text.Length)
			{
				throw this.NotWFError("'?' is expected.");
			}
			this.AddAttributeWithValue("version", "1.0");
			if (text2 != null)
			{
				this.AddAttributeWithValue("encoding", text2);
			}
			if (text3 != null)
			{
				this.AddAttributeWithValue("standalone", text3);
			}
			this.currentAttribute = (this.currentAttributeValue = -1);
			this.SetProperties(XmlNodeType.XmlDeclaration, "xml", string.Empty, "xml", false, text, false);
		}

		private bool SkipWhitespaceInString(string text, ref int idx)
		{
			int num = idx;
			while (idx < text.Length && XmlChar.IsWhitespace((int)text[idx]))
			{
				idx++;
			}
			return idx - num > 0;
		}

		private void ParseAttributeFromString(string src, ref int idx, out string name, out string value)
		{
			while (idx < src.Length && XmlChar.IsWhitespace((int)src[idx]))
			{
				idx++;
			}
			int num = idx;
			while (idx < src.Length && XmlChar.IsNameChar((int)src[idx]))
			{
				idx++;
			}
			name = src.Substring(num, idx - num);
			while (idx < src.Length && XmlChar.IsWhitespace((int)src[idx]))
			{
				idx++;
			}
			if (idx == src.Length || src[idx] != '=')
			{
				throw this.NotWFError(string.Format("'=' is expected after {0}", name));
			}
			idx++;
			while (idx < src.Length && XmlChar.IsWhitespace((int)src[idx]))
			{
				idx++;
			}
			if (idx == src.Length || (src[idx] != '"' && src[idx] != '\''))
			{
				throw this.NotWFError("'\"' or ''' is expected.");
			}
			char c = src[idx];
			idx++;
			num = idx;
			while (idx < src.Length && src[idx] != c)
			{
				idx++;
			}
			idx++;
			value = src.Substring(num, idx - num - 1);
		}

		internal void SkipTextDeclaration()
		{
			if (this.PeekChar() != 60)
			{
				return;
			}
			this.ReadChar();
			if (this.PeekChar() != 63)
			{
				this.peekCharsIndex = 0;
				return;
			}
			this.ReadChar();
			while (this.peekCharsIndex < 6)
			{
				if (this.PeekChar() < 0)
				{
					break;
				}
				this.ReadChar();
			}
			if (!(new string(this.peekChars, 2, 4) != "xml "))
			{
				this.SkipWhitespace();
				if (this.PeekChar() == 118)
				{
					this.Expect("version");
					this.ExpectAfterWhitespace('=');
					this.SkipWhitespace();
					int num = this.ReadChar();
					char[] array = new char[3];
					int num2 = 0;
					int num3 = num;
					if (num3 != 34 && num3 != 39)
					{
						throw this.NotWFError("Invalid version declaration inside text declaration.");
					}
					while (this.PeekChar() != num)
					{
						if (this.PeekChar() == -1)
						{
							throw this.NotWFError("Invalid version declaration inside text declaration.");
						}
						if (num2 == 3)
						{
							throw this.NotWFError("Invalid version number inside text declaration.");
						}
						array[num2] = (char)this.ReadChar();
						num2++;
						if (num2 == 3 && new string(array) != "1.0")
						{
							throw this.NotWFError("Invalid version number inside text declaration.");
						}
					}
					this.ReadChar();
					this.SkipWhitespace();
				}
				if (this.PeekChar() == 101)
				{
					this.Expect("encoding");
					this.ExpectAfterWhitespace('=');
					this.SkipWhitespace();
					int num4 = this.ReadChar();
					int num3 = num4;
					if (num3 != 34 && num3 != 39)
					{
						throw this.NotWFError("Invalid encoding declaration inside text declaration.");
					}
					while (this.PeekChar() != num4)
					{
						if (this.ReadChar() == -1)
						{
							throw this.NotWFError("Invalid encoding declaration inside text declaration.");
						}
					}
					this.ReadChar();
					this.SkipWhitespace();
				}
				else if (this.Conformance == ConformanceLevel.Auto)
				{
					throw this.NotWFError("Encoding declaration is mandatory in text declaration.");
				}
				this.Expect("?>");
				this.curNodePeekIndex = this.peekCharsIndex;
				return;
			}
			if (new string(this.peekChars, 2, 4).ToLower(CultureInfo.InvariantCulture) == "xml ")
			{
				throw this.NotWFError("Processing instruction name must not be character sequence 'X' 'M' 'L' with case insensitivity.");
			}
			this.peekCharsIndex = 0;
		}

		private void ReadDeclaration()
		{
			int num = this.PeekChar();
			int num2 = num;
			if (num2 != 45)
			{
				if (num2 != 68)
				{
					if (num2 != 91)
					{
						throw this.NotWFError("Unexpected declaration markup was found.");
					}
					this.ReadChar();
					this.Expect("CDATA[");
					this.ReadCDATA();
				}
				else
				{
					this.Expect("DOCTYPE");
					this.ReadDoctypeDecl();
				}
			}
			else
			{
				this.Expect("--");
				this.ReadComment();
			}
		}

		private void ReadComment()
		{
			if (this.currentState == XmlNodeType.None)
			{
				this.currentState = XmlNodeType.XmlDeclaration;
			}
			this.preserveCurrentTag = false;
			this.ClearValueBuffer();
			int num;
			while ((num = this.PeekChar()) != -1)
			{
				this.Advance(num);
				if (num == 45 && this.PeekChar() == 45)
				{
					this.Advance(45);
					if (this.PeekChar() != 62)
					{
						throw this.NotWFError("comments cannot contain '--'");
					}
					this.Advance(62);
					break;
				}
				else
				{
					if (XmlChar.IsInvalid(num))
					{
						throw this.NotWFError("Not allowed character was found.");
					}
					this.AppendValueChar(num);
				}
			}
			this.SetProperties(XmlNodeType.Comment, string.Empty, string.Empty, string.Empty, false, null, true);
		}

		private void ReadCDATA()
		{
			if (this.currentState != XmlNodeType.Element)
			{
				throw this.NotWFError("CDATA section cannot appear in this state.");
			}
			this.preserveCurrentTag = false;
			this.ClearValueBuffer();
			bool flag = false;
			int num = 0;
			while (this.PeekChar() != -1)
			{
				if (!flag)
				{
					num = this.ReadChar();
				}
				flag = false;
				if (num == 93 && this.PeekChar() == 93)
				{
					num = this.ReadChar();
					if (this.PeekChar() == 62)
					{
						this.ReadChar();
						break;
					}
					flag = true;
				}
				if (this.normalization && num == 13)
				{
					num = this.PeekChar();
					if (num != 10)
					{
						this.AppendValueChar(10);
					}
				}
				else
				{
					if (this.CharacterChecking && XmlChar.IsInvalid(num))
					{
						throw this.NotWFError("Invalid character was found.");
					}
					if (num < 65535)
					{
						this.valueBuffer.Append((char)num);
					}
					else
					{
						this.AppendSurrogatePairValueChar(num);
					}
				}
			}
			this.SetProperties(XmlNodeType.CDATA, string.Empty, string.Empty, string.Empty, false, null, true);
		}

		private void ReadDoctypeDecl()
		{
			if (this.prohibitDtd)
			{
				throw this.NotWFError("Document Type Declaration (DTD) is prohibited in this XML.");
			}
			XmlNodeType xmlNodeType = this.currentState;
			if (xmlNodeType != XmlNodeType.Element && xmlNodeType != XmlNodeType.DocumentType && xmlNodeType != XmlNodeType.EndElement)
			{
				this.currentState = XmlNodeType.DocumentType;
				string text = null;
				string text2 = null;
				int intSubsetStartLine = 0;
				int intSubsetStartColumn = 0;
				this.SkipWhitespace();
				string text3 = this.ReadName();
				this.SkipWhitespace();
				switch (this.PeekChar())
				{
				case 80:
					text = this.ReadPubidLiteral();
					if (!this.SkipWhitespace())
					{
						throw this.NotWFError("Whitespace is required between PUBLIC id and SYSTEM id.");
					}
					text2 = this.ReadSystemLiteral(false);
					break;
				case 83:
					text2 = this.ReadSystemLiteral(true);
					break;
				}
				this.SkipWhitespace();
				if (this.PeekChar() == 91)
				{
					this.ReadChar();
					intSubsetStartLine = this.LineNumber;
					intSubsetStartColumn = this.LinePosition;
					this.ClearValueBuffer();
					this.ReadInternalSubset();
					this.parserContext.InternalSubset = this.CreateValueString();
				}
				this.ExpectAfterWhitespace('>');
				this.GenerateDTDObjectModel(text3, text, text2, this.parserContext.InternalSubset, intSubsetStartLine, intSubsetStartColumn);
				this.SetProperties(XmlNodeType.DocumentType, text3, string.Empty, text3, false, this.parserContext.InternalSubset, true);
				if (text != null)
				{
					this.AddAttributeWithValue("PUBLIC", text);
				}
				if (text2 != null)
				{
					this.AddAttributeWithValue("SYSTEM", text2);
				}
				this.currentAttribute = (this.currentAttributeValue = -1);
				return;
			}
			throw this.NotWFError("Document type cannot appear in this state.");
		}

		internal DTDObjectModel GenerateDTDObjectModel(string name, string publicId, string systemId, string internalSubset)
		{
			return this.GenerateDTDObjectModel(name, publicId, systemId, internalSubset, 0, 0);
		}

		internal DTDObjectModel GenerateDTDObjectModel(string name, string publicId, string systemId, string internalSubset, int intSubsetStartLine, int intSubsetStartColumn)
		{
			this.parserContext.Dtd = new DTDObjectModel(this.NameTable);
			this.DTD.BaseURI = this.BaseURI;
			this.DTD.Name = name;
			this.DTD.PublicId = publicId;
			this.DTD.SystemId = systemId;
			this.DTD.InternalSubset = internalSubset;
			this.DTD.XmlResolver = this.resolver;
			this.DTD.IsStandalone = this.isStandalone;
			this.DTD.LineNumber = this.line;
			this.DTD.LinePosition = this.column;
			return new DTDReader(this.DTD, intSubsetStartLine, intSubsetStartColumn)
			{
				Normalization = this.normalization
			}.GenerateDTDObjectModel();
		}

		private XmlTextReader.DtdInputState State
		{
			get
			{
				return this.stateStack.Peek();
			}
		}

		private int ReadValueChar()
		{
			int num = this.ReadChar();
			this.AppendValueChar(num);
			return num;
		}

		private void ExpectAndAppend(string s)
		{
			this.Expect(s);
			this.valueBuffer.Append(s);
		}

		private void ReadInternalSubset()
		{
			bool flag = true;
			while (flag)
			{
				int num = this.ReadValueChar();
				switch (num)
				{
				case 34:
					if (this.State == XmlTextReader.DtdInputState.InsideDoubleQuoted)
					{
						this.stateStack.Pop();
					}
					else if (this.State != XmlTextReader.DtdInputState.InsideSingleQuoted && this.State != XmlTextReader.DtdInputState.Comment)
					{
						this.stateStack.Push(XmlTextReader.DtdInputState.InsideDoubleQuoted);
					}
					break;
				default:
					switch (num)
					{
					case 60:
						switch (this.State)
						{
						case XmlTextReader.DtdInputState.Comment:
						case XmlTextReader.DtdInputState.InsideSingleQuoted:
						case XmlTextReader.DtdInputState.InsideDoubleQuoted:
							break;
						default:
						{
							int num2 = this.ReadValueChar();
							int num3 = num2;
							if (num3 != 33)
							{
								if (num3 != 63)
								{
									throw this.NotWFError(string.Format("unexpected '<{0}'.", (char)num2));
								}
								this.stateStack.Push(XmlTextReader.DtdInputState.PI);
							}
							else
							{
								int num4 = this.ReadValueChar();
								if (num4 != 45)
								{
									if (num4 != 65)
									{
										if (num4 == 69)
										{
											switch (this.ReadValueChar())
											{
											case 76:
												this.ExpectAndAppend("EMENT");
												this.stateStack.Push(XmlTextReader.DtdInputState.ElementDecl);
												break;
											case 77:
												goto IL_1B1;
											case 78:
												this.ExpectAndAppend("TITY");
												this.stateStack.Push(XmlTextReader.DtdInputState.EntityDecl);
												break;
											default:
												goto IL_1B1;
											}
											break;
											IL_1B1:
											throw this.NotWFError("unexpected token '<!E'.");
										}
										if (num4 == 78)
										{
											this.ExpectAndAppend("OTATION");
											this.stateStack.Push(XmlTextReader.DtdInputState.NotationDecl);
										}
									}
									else
									{
										this.ExpectAndAppend("TTLIST");
										this.stateStack.Push(XmlTextReader.DtdInputState.AttlistDecl);
									}
								}
								else
								{
									this.ExpectAndAppend("-");
									this.stateStack.Push(XmlTextReader.DtdInputState.Comment);
								}
							}
							break;
						}
						}
						break;
					default:
						if (num == -1)
						{
							throw this.NotWFError("unexpected end of file at DTD.");
						}
						if (num != 45)
						{
							if (num == 93)
							{
								XmlTextReader.DtdInputState state = this.State;
								switch (state)
								{
								case XmlTextReader.DtdInputState.Comment:
								case XmlTextReader.DtdInputState.InsideSingleQuoted:
								case XmlTextReader.DtdInputState.InsideDoubleQuoted:
									break;
								default:
									if (state != XmlTextReader.DtdInputState.Free)
									{
										throw this.NotWFError("unexpected end of file at DTD.");
									}
									this.valueBuffer.Remove(this.valueBuffer.Length - 1, 1);
									flag = false;
									break;
								}
							}
						}
						else if (this.State == XmlTextReader.DtdInputState.Comment && this.PeekChar() == 45)
						{
							this.ReadValueChar();
							this.ExpectAndAppend(">");
							this.stateStack.Pop();
						}
						break;
					case 62:
						switch (this.State)
						{
						case XmlTextReader.DtdInputState.ElementDecl:
							break;
						case XmlTextReader.DtdInputState.AttlistDecl:
							break;
						case XmlTextReader.DtdInputState.EntityDecl:
							break;
						case XmlTextReader.DtdInputState.NotationDecl:
							break;
						case XmlTextReader.DtdInputState.PI:
							goto IL_320;
						case XmlTextReader.DtdInputState.Comment:
						case XmlTextReader.DtdInputState.InsideSingleQuoted:
						case XmlTextReader.DtdInputState.InsideDoubleQuoted:
							continue;
						default:
							goto IL_320;
						}
						this.stateStack.Pop();
						break;
						IL_320:
						throw this.NotWFError("unexpected token '>'");
					case 63:
						if (this.State == XmlTextReader.DtdInputState.PI && this.ReadValueChar() == 62)
						{
							this.stateStack.Pop();
						}
						break;
					}
					break;
				case 37:
					if (this.State != XmlTextReader.DtdInputState.Free && this.State != XmlTextReader.DtdInputState.EntityDecl && this.State != XmlTextReader.DtdInputState.Comment && this.State != XmlTextReader.DtdInputState.InsideDoubleQuoted && this.State != XmlTextReader.DtdInputState.InsideSingleQuoted)
					{
						throw this.NotWFError("Parameter Entity Reference cannot appear as a part of markupdecl (see XML spec 2.8).");
					}
					break;
				case 39:
					if (this.State == XmlTextReader.DtdInputState.InsideSingleQuoted)
					{
						this.stateStack.Pop();
					}
					else if (this.State != XmlTextReader.DtdInputState.InsideDoubleQuoted && this.State != XmlTextReader.DtdInputState.Comment)
					{
						this.stateStack.Push(XmlTextReader.DtdInputState.InsideSingleQuoted);
					}
					break;
				}
			}
		}

		private string ReadSystemLiteral(bool expectSYSTEM)
		{
			if (expectSYSTEM)
			{
				this.Expect("SYSTEM");
				if (!this.SkipWhitespace())
				{
					throw this.NotWFError("Whitespace is required after 'SYSTEM'.");
				}
			}
			else
			{
				this.SkipWhitespace();
			}
			int num = this.ReadChar();
			int num2 = 0;
			this.ClearValueBuffer();
			while (num2 != num)
			{
				num2 = this.ReadChar();
				if (num2 < 0)
				{
					throw this.NotWFError("Unexpected end of stream in ExternalID.");
				}
				if (num2 != num)
				{
					this.AppendValueChar(num2);
				}
			}
			return this.CreateValueString();
		}

		private string ReadPubidLiteral()
		{
			this.Expect("PUBLIC");
			if (!this.SkipWhitespace())
			{
				throw this.NotWFError("Whitespace is required after 'PUBLIC'.");
			}
			int num = this.ReadChar();
			int num2 = 0;
			this.ClearValueBuffer();
			while (num2 != num)
			{
				num2 = this.ReadChar();
				if (num2 < 0)
				{
					throw this.NotWFError("Unexpected end of stream in ExternalID.");
				}
				if (num2 != num && !XmlChar.IsPubidChar(num2))
				{
					throw this.NotWFError(string.Format("character '{0}' not allowed for PUBLIC ID", (char)num2));
				}
				if (num2 != num)
				{
					this.AppendValueChar(num2);
				}
			}
			return this.CreateValueString();
		}

		private string ReadName()
		{
			string text;
			string text2;
			return this.ReadName(out text, out text2);
		}

		private string ReadName(out string prefix, out string localName)
		{
			bool flag = this.preserveCurrentTag;
			this.preserveCurrentTag = true;
			int num = this.peekCharsIndex - this.curNodePeekIndex;
			int num2 = this.PeekChar();
			if (!XmlChar.IsFirstNameChar(num2))
			{
				throw this.NotWFError(string.Format(CultureInfo.InvariantCulture, "a name did not start with a legal character {0} ({1})", new object[]
				{
					num2,
					(char)num2
				}));
			}
			this.Advance(num2);
			int num3 = 1;
			int num4 = -1;
			while (XmlChar.IsNameChar(num2 = this.PeekChar()))
			{
				this.Advance(num2);
				if (num2 == 58 && this.namespaces && num4 < 0)
				{
					num4 = num3;
				}
				num3++;
			}
			int num5 = this.curNodePeekIndex + num;
			string text = this.NameTable.Add(this.peekChars, num5, num3);
			if (num4 > 0)
			{
				prefix = this.NameTable.Add(this.peekChars, num5, num4);
				localName = this.NameTable.Add(this.peekChars, num5 + num4 + 1, num3 - num4 - 1);
			}
			else
			{
				prefix = string.Empty;
				localName = text;
			}
			this.preserveCurrentTag = flag;
			return text;
		}

		private void Expect(int expected)
		{
			int num = this.ReadChar();
			if (num != expected)
			{
				throw this.NotWFError(string.Format(CultureInfo.InvariantCulture, "expected '{0}' ({1:X}) but found '{2}' ({3:X})", new object[]
				{
					(char)expected,
					expected,
					(num >= 0) ? ((char)num) : "EOF",
					num
				}));
			}
		}

		private void Expect(string expected)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				if (this.ReadChar() != (int)expected[i])
				{
					throw this.NotWFError(string.Format(CultureInfo.InvariantCulture, "'{0}' is expected", new object[]
					{
						expected
					}));
				}
			}
		}

		private void ExpectAfterWhitespace(char c)
		{
			int num;
			do
			{
				num = this.ReadChar();
			}
			while (num < 33 && XmlChar.IsWhitespace(num));
			if ((int)c != num)
			{
				throw this.NotWFError(string.Format(CultureInfo.InvariantCulture, "Expected {0}, but found {1} [{2}]", new object[]
				{
					c,
					(num >= 0) ? ((char)num) : "EOF",
					num
				}));
			}
		}

		private bool SkipWhitespace()
		{
			int num = this.PeekChar();
			bool flag = num == 32 || num == 9 || num == 10 || num == 13;
			if (!flag)
			{
				return false;
			}
			this.Advance(num);
			while ((num = this.PeekChar()) == 32 || num == 9 || num == 10 || num == 13)
			{
				this.Advance(num);
			}
			return flag;
		}

		private bool ReadWhitespace()
		{
			if (this.currentState == XmlNodeType.None)
			{
				this.currentState = XmlNodeType.XmlDeclaration;
			}
			bool flag = this.preserveCurrentTag;
			this.preserveCurrentTag = true;
			int num = this.peekCharsIndex - this.curNodePeekIndex;
			int num2 = this.PeekChar();
			do
			{
				this.Advance(num2);
				num2 = this.PeekChar();
			}
			while (num2 == 32 || num2 == 9 || num2 == 10 || num2 == 13);
			bool flag2 = this.currentState == XmlNodeType.Element && num2 != -1 && num2 != 60;
			if (!flag2 && (this.whitespaceHandling == WhitespaceHandling.None || (this.whitespaceHandling == WhitespaceHandling.Significant && this.XmlSpace != XmlSpace.Preserve)))
			{
				return false;
			}
			this.ClearValueBuffer();
			this.valueBuffer.Append(this.peekChars, this.curNodePeekIndex, this.peekCharsIndex - this.curNodePeekIndex - num);
			this.preserveCurrentTag = flag;
			if (flag2)
			{
				this.ReadText(false);
			}
			else
			{
				XmlNodeType nodeType = (this.XmlSpace != XmlSpace.Preserve) ? XmlNodeType.Whitespace : XmlNodeType.SignificantWhitespace;
				this.SetProperties(nodeType, string.Empty, string.Empty, string.Empty, false, null, true);
			}
			return true;
		}

		private int ReadCharsInternal(char[] buffer, int offset, int length)
		{
			int num = offset;
			for (int i = 0; i < length; i++)
			{
				int num2 = this.PeekChar();
				int num3 = num2;
				if (num3 == -1)
				{
					throw this.NotWFError("Unexpected end of xml.");
				}
				if (num3 != 60)
				{
					this.Advance(num2);
					if (num2 < 65535)
					{
						buffer[num++] = (char)num2;
					}
					else
					{
						buffer[num++] = (char)((num2 - 65536) / 1024 + 55296);
						buffer[num++] = (char)((num2 - 65536) % 1024 + 56320);
					}
				}
				else
				{
					if (i + 1 == length)
					{
						return i;
					}
					this.Advance(num2);
					if (this.PeekChar() != 47)
					{
						this.nestLevel++;
						buffer[num++] = '<';
					}
					else
					{
						if (this.nestLevel-- <= 0)
						{
							this.Expect(47);
							if (this.depthUp)
							{
								this.depth++;
								this.depthUp = false;
							}
							this.ReadEndTag();
							this.readCharsInProgress = false;
							this.Read();
							return i;
						}
						buffer[num++] = '<';
					}
				}
			}
			return length;
		}

		private bool ReadUntilEndTag()
		{
			if (this.Depth == 0)
			{
				this.currentState = XmlNodeType.EndElement;
			}
			for (;;)
			{
				int num = this.ReadChar();
				int num2 = num;
				if (num2 == -1)
				{
					break;
				}
				if (num2 == 60)
				{
					if (this.PeekChar() != 47)
					{
						this.nestLevel++;
					}
					else if (--this.nestLevel <= 0)
					{
						this.ReadChar();
						string a = this.ReadName();
						if (!(a != this.elementNames[this.elementNameStackPos - 1].Name))
						{
							goto IL_AE;
						}
					}
				}
			}
			throw this.NotWFError("Unexpected end of xml.");
			IL_AE:
			this.Expect(62);
			this.depth--;
			return this.Read();
		}

		internal class XmlTokenInfo
		{
			private string valueCache;

			protected XmlTextReader Reader;

			public string Name;

			public string LocalName;

			public string Prefix;

			public string NamespaceURI;

			public bool IsEmptyElement;

			public char QuoteChar;

			public int LineNumber;

			public int LinePosition;

			public int ValueBufferStart;

			public int ValueBufferEnd;

			public XmlNodeType NodeType;

			public XmlTokenInfo(XmlTextReader xtr)
			{
				this.Reader = xtr;
				this.Clear();
			}

			public virtual string Value
			{
				get
				{
					if (this.valueCache != null)
					{
						return this.valueCache;
					}
					if (this.ValueBufferStart >= 0)
					{
						this.valueCache = this.Reader.valueBuffer.ToString(this.ValueBufferStart, this.ValueBufferEnd - this.ValueBufferStart);
						return this.valueCache;
					}
					switch (this.NodeType)
					{
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.Comment:
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						this.valueCache = this.Reader.CreateValueString();
						return this.valueCache;
					}
					return null;
				}
				set
				{
					this.valueCache = value;
				}
			}

			public virtual void Clear()
			{
				this.ValueBufferStart = -1;
				this.valueCache = null;
				this.NodeType = XmlNodeType.None;
				this.Name = (this.LocalName = (this.Prefix = (this.NamespaceURI = string.Empty)));
				this.IsEmptyElement = false;
				this.QuoteChar = '"';
				this.LineNumber = (this.LinePosition = 0);
			}
		}

		internal class XmlAttributeTokenInfo : XmlTextReader.XmlTokenInfo
		{
			public int ValueTokenStartIndex;

			public int ValueTokenEndIndex;

			private string valueCache;

			private StringBuilder tmpBuilder = new StringBuilder();

			public XmlAttributeTokenInfo(XmlTextReader reader) : base(reader)
			{
				this.NodeType = XmlNodeType.Attribute;
			}

			public override string Value
			{
				get
				{
					if (this.valueCache != null)
					{
						return this.valueCache;
					}
					if (this.ValueTokenStartIndex == this.ValueTokenEndIndex)
					{
						XmlTextReader.XmlTokenInfo xmlTokenInfo = this.Reader.attributeValueTokens[this.ValueTokenStartIndex];
						if (xmlTokenInfo.NodeType == XmlNodeType.EntityReference)
						{
							this.valueCache = "&" + xmlTokenInfo.Name + ";";
						}
						else
						{
							this.valueCache = xmlTokenInfo.Value;
						}
						return this.valueCache;
					}
					this.tmpBuilder.Length = 0;
					for (int i = this.ValueTokenStartIndex; i <= this.ValueTokenEndIndex; i++)
					{
						XmlTextReader.XmlTokenInfo xmlTokenInfo2 = this.Reader.attributeValueTokens[i];
						if (xmlTokenInfo2.NodeType == XmlNodeType.Text)
						{
							this.tmpBuilder.Append(xmlTokenInfo2.Value);
						}
						else
						{
							this.tmpBuilder.Append('&');
							this.tmpBuilder.Append(xmlTokenInfo2.Name);
							this.tmpBuilder.Append(';');
						}
					}
					this.valueCache = this.tmpBuilder.ToString(0, this.tmpBuilder.Length);
					return this.valueCache;
				}
				set
				{
					this.valueCache = value;
				}
			}

			public override void Clear()
			{
				base.Clear();
				this.valueCache = null;
				this.NodeType = XmlNodeType.Attribute;
				this.ValueTokenStartIndex = (this.ValueTokenEndIndex = 0);
			}

			internal void FillXmlns()
			{
				if (object.ReferenceEquals(this.Prefix, "xmlns"))
				{
					this.Reader.nsmgr.AddNamespace(this.LocalName, this.Value);
				}
				else if (object.ReferenceEquals(this.Name, "xmlns"))
				{
					this.Reader.nsmgr.AddNamespace(string.Empty, this.Value);
				}
			}

			internal void FillNamespace()
			{
				if (object.ReferenceEquals(this.Prefix, "xmlns") || object.ReferenceEquals(this.Name, "xmlns"))
				{
					this.NamespaceURI = "http://www.w3.org/2000/xmlns/";
				}
				else if (this.Prefix.Length == 0)
				{
					this.NamespaceURI = string.Empty;
				}
				else
				{
					this.NamespaceURI = this.Reader.LookupNamespace(this.Prefix, true);
				}
			}
		}

		private struct TagName
		{
			public readonly string Name;

			public readonly string LocalName;

			public readonly string Prefix;

			public TagName(string n, string l, string p)
			{
				this.Name = n;
				this.LocalName = l;
				this.Prefix = p;
			}
		}

		private enum DtdInputState
		{
			Free = 1,
			ElementDecl,
			AttlistDecl,
			EntityDecl,
			NotationDecl,
			PI,
			Comment,
			InsideSingleQuoted,
			InsideDoubleQuoted
		}

		private class DtdInputStateStack
		{
			private Stack intern = new Stack();

			public DtdInputStateStack()
			{
				this.Push(XmlTextReader.DtdInputState.Free);
			}

			public XmlTextReader.DtdInputState Peek()
			{
				return (XmlTextReader.DtdInputState)((int)this.intern.Peek());
			}

			public XmlTextReader.DtdInputState Pop()
			{
				return (XmlTextReader.DtdInputState)((int)this.intern.Pop());
			}

			public void Push(XmlTextReader.DtdInputState val)
			{
				this.intern.Push(val);
			}
		}
	}
}
