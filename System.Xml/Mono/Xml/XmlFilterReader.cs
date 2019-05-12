using System;
using System.Xml;

namespace Mono.Xml
{
	internal class XmlFilterReader : XmlReader, IXmlLineInfo
	{
		private XmlReader reader;

		private XmlReaderSettings settings;

		private IXmlLineInfo lineInfo;

		public XmlFilterReader(XmlReader reader, XmlReaderSettings settings)
		{
			this.reader = reader;
			this.settings = settings.Clone();
			this.lineInfo = (reader as IXmlLineInfo);
		}

		public override bool CanReadBinaryContent
		{
			get
			{
				return this.reader.CanReadBinaryContent;
			}
		}

		public override bool CanReadValueChunk
		{
			get
			{
				return this.reader.CanReadValueChunk;
			}
		}

		public XmlReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public int LineNumber
		{
			get
			{
				return (this.lineInfo == null) ? 0 : this.lineInfo.LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return (this.lineInfo == null) ? 0 : this.lineInfo.LinePosition;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return this.reader.NodeType;
			}
		}

		public override string Name
		{
			get
			{
				return this.reader.Name;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.reader.LocalName;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.reader.NamespaceURI;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.reader.Prefix;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.reader.HasValue;
			}
		}

		public override int Depth
		{
			get
			{
				return this.reader.Depth;
			}
		}

		public override string Value
		{
			get
			{
				return this.reader.Value;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.reader.IsEmptyElement;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.reader.IsDefault;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this.reader.QuoteChar;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.reader.XmlLang;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				return this.reader.XmlSpace;
			}
		}

		public override int AttributeCount
		{
			get
			{
				return this.reader.AttributeCount;
			}
		}

		public override string this[int i]
		{
			get
			{
				return this.reader[i];
			}
		}

		public override string this[string name]
		{
			get
			{
				return this.reader[name];
			}
		}

		public override string this[string localName, string namespaceURI]
		{
			get
			{
				return this.reader[localName, namespaceURI];
			}
		}

		public override bool EOF
		{
			get
			{
				return this.reader.EOF;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.reader.ReadState;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		public override XmlReaderSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		public override string GetAttribute(string name)
		{
			return this.reader.GetAttribute(name);
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.reader.GetAttribute(localName, namespaceURI);
		}

		public override string GetAttribute(int i)
		{
			return this.reader.GetAttribute(i);
		}

		public bool HasLineInfo()
		{
			return this.lineInfo != null && this.lineInfo.HasLineInfo();
		}

		public override bool MoveToAttribute(string name)
		{
			return this.reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return this.reader.MoveToAttribute(localName, namespaceURI);
		}

		public override void MoveToAttribute(int i)
		{
			this.reader.MoveToAttribute(i);
		}

		public override bool MoveToFirstAttribute()
		{
			return this.reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return this.reader.MoveToNextAttribute();
		}

		public override bool MoveToElement()
		{
			return this.reader.MoveToElement();
		}

		public override void Close()
		{
			if (this.settings.CloseInput)
			{
				this.reader.Close();
			}
		}

		public override bool Read()
		{
			if (!this.reader.Read())
			{
				return false;
			}
			if (this.reader.NodeType == XmlNodeType.DocumentType && this.settings.ProhibitDtd)
			{
				throw new XmlException("Document Type Definition (DTD) is prohibited in this XML reader.");
			}
			if (this.reader.NodeType == XmlNodeType.Whitespace && this.settings.IgnoreWhitespace)
			{
				return this.Read();
			}
			if (this.reader.NodeType == XmlNodeType.ProcessingInstruction && this.settings.IgnoreProcessingInstructions)
			{
				return this.Read();
			}
			return this.reader.NodeType != XmlNodeType.Comment || !this.settings.IgnoreComments || this.Read();
		}

		public override string ReadString()
		{
			return this.reader.ReadString();
		}

		public override string LookupNamespace(string prefix)
		{
			return this.reader.LookupNamespace(prefix);
		}

		public override void ResolveEntity()
		{
			this.reader.ResolveEntity();
		}

		public override bool ReadAttributeValue()
		{
			return this.reader.ReadAttributeValue();
		}
	}
}
