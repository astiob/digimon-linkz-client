using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class XPathNavigatorReader : XmlReader
	{
		private XPathNavigator root;

		private XPathNavigator current;

		private bool started;

		private bool closed;

		private bool endElement;

		private bool attributeValueConsumed;

		private StringBuilder readStringBuffer = new StringBuilder();

		private StringBuilder innerXmlBuilder = new StringBuilder();

		private int depth;

		private int attributeCount;

		private bool eof;

		public XPathNavigatorReader(XPathNavigator nav)
		{
			this.current = nav.Clone();
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

		public override XmlNodeType NodeType
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return XmlNodeType.None;
				}
				if (this.endElement)
				{
					return XmlNodeType.EndElement;
				}
				if (this.attributeValueConsumed)
				{
					return XmlNodeType.Text;
				}
				switch (this.current.NodeType)
				{
				case XPathNodeType.Root:
					return XmlNodeType.None;
				case XPathNodeType.Element:
					return XmlNodeType.Element;
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					return XmlNodeType.Attribute;
				case XPathNodeType.Text:
					return XmlNodeType.Text;
				case XPathNodeType.SignificantWhitespace:
					return XmlNodeType.SignificantWhitespace;
				case XPathNodeType.Whitespace:
					return XmlNodeType.Whitespace;
				case XPathNodeType.ProcessingInstruction:
					return XmlNodeType.ProcessingInstruction;
				case XPathNodeType.Comment:
					return XmlNodeType.Comment;
				default:
					throw new InvalidOperationException(string.Format("Current XPathNavigator status is {0} which is not acceptable to XmlReader.", this.current.NodeType));
				}
			}
		}

		public override string Name
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return string.Empty;
				}
				if (this.attributeValueConsumed)
				{
					return string.Empty;
				}
				if (this.current.NodeType == XPathNodeType.Namespace)
				{
					return (!(this.current.Name == string.Empty)) ? ("xmlns:" + this.current.Name) : "xmlns";
				}
				return this.current.Name;
			}
		}

		public override string LocalName
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return string.Empty;
				}
				if (this.attributeValueConsumed)
				{
					return string.Empty;
				}
				if (this.current.NodeType == XPathNodeType.Namespace && this.current.LocalName == string.Empty)
				{
					return "xmlns";
				}
				return this.current.LocalName;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return string.Empty;
				}
				if (this.attributeValueConsumed)
				{
					return string.Empty;
				}
				if (this.current.NodeType == XPathNodeType.Namespace)
				{
					return "http://www.w3.org/2000/xmlns/";
				}
				return this.current.NamespaceURI;
			}
		}

		public override string Prefix
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return string.Empty;
				}
				if (this.attributeValueConsumed)
				{
					return string.Empty;
				}
				if (this.current.NodeType == XPathNodeType.Namespace && this.current.LocalName != string.Empty)
				{
					return "xmlns";
				}
				return this.current.Prefix;
			}
		}

		public override bool HasValue
		{
			get
			{
				switch (this.current.NodeType)
				{
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return true;
				default:
					return false;
				}
			}
		}

		public override int Depth
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return 0;
				}
				if (this.NodeType == XmlNodeType.Attribute)
				{
					return this.depth + 1;
				}
				if (this.attributeValueConsumed)
				{
					return this.depth + 2;
				}
				return this.depth;
			}
		}

		public override string Value
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return string.Empty;
				}
				switch (this.current.NodeType)
				{
				case XPathNodeType.Root:
				case XPathNodeType.Element:
					return string.Empty;
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return this.current.Value;
				default:
					throw new InvalidOperationException("Current XPathNavigator status is {0} which is not acceptable to XmlReader.");
				}
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.current.BaseURI;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.ReadState == ReadState.Interactive && this.current.IsEmptyElement;
			}
		}

		public override bool IsDefault
		{
			get
			{
				IXmlSchemaInfo xmlSchemaInfo = this.current as IXmlSchemaInfo;
				return xmlSchemaInfo != null && xmlSchemaInfo.IsDefault;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return '"';
			}
		}

		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.current.SchemaInfo;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.current.XmlLang;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.None;
			}
		}

		public override int AttributeCount
		{
			get
			{
				return this.attributeCount;
			}
		}

		private int GetAttributeCount()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return 0;
			}
			int num = 0;
			if (this.current.MoveToFirstAttribute())
			{
				do
				{
					num++;
				}
				while (this.current.MoveToNextAttribute());
				this.current.MoveToParent();
			}
			if (this.current.MoveToFirstNamespace(XPathNamespaceScope.Local))
			{
				do
				{
					num++;
				}
				while (this.current.MoveToNextNamespace(XPathNamespaceScope.Local));
				this.current.MoveToParent();
			}
			return num;
		}

		private void SplitName(string name, out string localName, out string ns)
		{
			if (name == "xmlns")
			{
				localName = "xmlns";
				ns = "http://www.w3.org/2000/xmlns/";
				return;
			}
			localName = name;
			ns = string.Empty;
			int num = name.IndexOf(':');
			if (num > 0)
			{
				localName = name.Substring(num + 1, name.Length - num - 1);
				ns = this.LookupNamespace(name.Substring(0, num));
				if (name.Substring(0, num) == "xmlns")
				{
					ns = "http://www.w3.org/2000/xmlns/";
				}
			}
		}

		public override string this[string name]
		{
			get
			{
				string localName;
				string namespaceURI;
				this.SplitName(name, out localName, out namespaceURI);
				return this[localName, namespaceURI];
			}
		}

		public override string this[string localName, string namespaceURI]
		{
			get
			{
				string attribute = this.current.GetAttribute(localName, namespaceURI);
				if (attribute != string.Empty)
				{
					return attribute;
				}
				XPathNavigator xpathNavigator = this.current.Clone();
				return (!xpathNavigator.MoveToAttribute(localName, namespaceURI)) ? null : string.Empty;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.ReadState == ReadState.EndOfFile;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				if (this.eof)
				{
					return ReadState.EndOfFile;
				}
				if (this.closed)
				{
					return ReadState.Closed;
				}
				if (!this.started)
				{
					return ReadState.Initial;
				}
				return ReadState.Interactive;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.current.NameTable;
			}
		}

		public override string GetAttribute(string name)
		{
			string localName;
			string namespaceURI;
			this.SplitName(name, out localName, out namespaceURI);
			return this[localName, namespaceURI];
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this[localName, namespaceURI];
		}

		public override string GetAttribute(int i)
		{
			return this[i];
		}

		private bool CheckAttributeMove(bool b)
		{
			if (b)
			{
				this.attributeValueConsumed = false;
			}
			return b;
		}

		public override bool MoveToAttribute(string name)
		{
			string localName;
			string namespaceURI;
			this.SplitName(name, out localName, out namespaceURI);
			return this.MoveToAttribute(localName, namespaceURI);
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			bool flag = namespaceURI == "http://www.w3.org/2000/xmlns/";
			XPathNavigator xpathNavigator = null;
			switch (this.current.NodeType)
			{
			case XPathNodeType.Element:
				break;
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				xpathNavigator = this.current.Clone();
				this.MoveToElement();
				break;
			default:
				goto IL_F1;
			}
			if (this.MoveToFirstAttribute())
			{
				for (;;)
				{
					bool flag2;
					if (flag)
					{
						if (localName == "xmlns")
						{
							flag2 = (this.current.Name == string.Empty);
						}
						else
						{
							flag2 = (localName == this.current.Name);
						}
					}
					else
					{
						flag2 = (this.current.LocalName == localName && this.current.NamespaceURI == namespaceURI);
					}
					if (flag2)
					{
						break;
					}
					if (!this.MoveToNextAttribute())
					{
						goto Block_6;
					}
				}
				this.attributeValueConsumed = false;
				return true;
				Block_6:
				this.MoveToElement();
			}
			IL_F1:
			if (xpathNavigator != null)
			{
				this.current = xpathNavigator;
			}
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			switch (this.current.NodeType)
			{
			case XPathNodeType.Element:
				break;
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				this.current.MoveToParent();
				break;
			default:
				return false;
			}
			return this.CheckAttributeMove(this.current.MoveToFirstNamespace(XPathNamespaceScope.Local)) || this.CheckAttributeMove(this.current.MoveToFirstAttribute());
		}

		public override bool MoveToNextAttribute()
		{
			switch (this.current.NodeType)
			{
			case XPathNodeType.Element:
				return this.MoveToFirstAttribute();
			case XPathNodeType.Attribute:
				return this.CheckAttributeMove(this.current.MoveToNextAttribute());
			case XPathNodeType.Namespace:
			{
				if (this.CheckAttributeMove(this.current.MoveToNextNamespace(XPathNamespaceScope.Local)))
				{
					return true;
				}
				XPathNavigator other = this.current.Clone();
				this.current.MoveToParent();
				if (this.CheckAttributeMove(this.current.MoveToFirstAttribute()))
				{
					return true;
				}
				this.current.MoveTo(other);
				return false;
			}
			default:
				return false;
			}
		}

		public override bool MoveToElement()
		{
			if (this.current.NodeType == XPathNodeType.Attribute || this.current.NodeType == XPathNodeType.Namespace)
			{
				this.attributeValueConsumed = false;
				return this.current.MoveToParent();
			}
			return false;
		}

		public override void Close()
		{
			this.closed = true;
			this.eof = true;
		}

		public override bool Read()
		{
			if (this.eof)
			{
				return false;
			}
			if (base.Binary != null)
			{
				base.Binary.Reset();
			}
			switch (this.ReadState)
			{
			case ReadState.Initial:
				this.started = true;
				this.root = this.current.Clone();
				if (this.current.NodeType == XPathNodeType.Root && !this.current.MoveToFirstChild())
				{
					this.endElement = false;
					this.eof = true;
					return false;
				}
				this.attributeCount = this.GetAttributeCount();
				return true;
			case ReadState.Interactive:
				if ((this.IsEmptyElement || this.endElement) && this.root.IsSamePosition(this.current))
				{
					this.eof = true;
					return false;
				}
				break;
			case ReadState.Error:
			case ReadState.EndOfFile:
			case ReadState.Closed:
				return false;
			}
			this.MoveToElement();
			if (this.endElement || !this.current.MoveToFirstChild())
			{
				if (!this.endElement && !this.current.IsEmptyElement && this.current.NodeType == XPathNodeType.Element)
				{
					this.endElement = true;
				}
				else if (!this.current.MoveToNext())
				{
					this.current.MoveToParent();
					if (this.current.NodeType == XPathNodeType.Root)
					{
						this.endElement = false;
						this.eof = true;
						return false;
					}
					this.endElement = (this.current.NodeType == XPathNodeType.Element);
					if (this.endElement)
					{
						this.depth--;
					}
				}
				else
				{
					this.endElement = false;
				}
			}
			else
			{
				this.depth++;
			}
			if (!this.endElement && this.current.NodeType == XPathNodeType.Element)
			{
				this.attributeCount = this.GetAttributeCount();
			}
			else
			{
				this.attributeCount = 0;
			}
			return true;
		}

		public override string ReadString()
		{
			this.readStringBuffer.Length = 0;
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
				goto IL_105;
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
			IL_105:
			string result = this.readStringBuffer.ToString();
			this.readStringBuffer.Length = 0;
			return result;
		}

		public override string LookupNamespace(string prefix)
		{
			XPathNavigator xpathNavigator = this.current.Clone();
			string result;
			try
			{
				this.MoveToElement();
				if (this.current.NodeType != XPathNodeType.Element)
				{
					this.current.MoveToParent();
				}
				if (this.current.MoveToFirstNamespace())
				{
					while (!(this.current.LocalName == prefix))
					{
						if (!this.current.MoveToNextNamespace())
						{
							goto IL_77;
						}
					}
					return this.current.Value;
				}
				IL_77:
				result = null;
			}
			finally
			{
				this.current = xpathNavigator;
			}
			return result;
		}

		public override void ResolveEntity()
		{
			throw new InvalidOperationException();
		}

		public override bool ReadAttributeValue()
		{
			if (this.NodeType != XmlNodeType.Attribute)
			{
				return false;
			}
			if (this.attributeValueConsumed)
			{
				return false;
			}
			this.attributeValueConsumed = true;
			return true;
		}
	}
}
