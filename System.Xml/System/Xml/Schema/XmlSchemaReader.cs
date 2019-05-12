using System;

namespace System.Xml.Schema
{
	internal class XmlSchemaReader : XmlReader, IXmlLineInfo
	{
		private XmlReader reader;

		private ValidationEventHandler handler;

		private bool hasLineInfo;

		public XmlSchemaReader(XmlReader reader, ValidationEventHandler handler)
		{
			this.reader = reader;
			this.handler = handler;
			if (reader is IXmlLineInfo)
			{
				IXmlLineInfo xmlLineInfo = (IXmlLineInfo)reader;
				this.hasLineInfo = xmlLineInfo.HasLineInfo();
			}
		}

		public string FullName
		{
			get
			{
				return this.NamespaceURI + ":" + this.LocalName;
			}
		}

		public XmlReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public void RaiseInvalidElementError()
		{
			string text = "Element " + this.FullName + " is invalid in this context.\n";
			if (this.hasLineInfo)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"The error occured on (",
					((IXmlLineInfo)this.reader).LineNumber,
					",",
					((IXmlLineInfo)this.reader).LinePosition,
					")"
				});
			}
			XmlSchemaObject.error(this.handler, text, null);
			this.SkipToEnd();
		}

		public bool ReadNextElement()
		{
			this.MoveToElement();
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element || this.NodeType == XmlNodeType.EndElement)
				{
					if (!(this.reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema"))
					{
						return true;
					}
					this.RaiseInvalidElementError();
				}
			}
			return false;
		}

		public void SkipToEnd()
		{
			this.MoveToElement();
			if (this.IsEmptyElement || this.NodeType != XmlNodeType.Element)
			{
				return;
			}
			if (this.NodeType == XmlNodeType.Element)
			{
				int depth = this.Depth;
				while (this.Read())
				{
					if (this.Depth == depth)
					{
						break;
					}
				}
			}
		}

		public bool HasLineInfo()
		{
			return this.hasLineInfo;
		}

		public int LineNumber
		{
			get
			{
				return (!this.hasLineInfo) ? 0 : ((IXmlLineInfo)this.reader).LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return (!this.hasLineInfo) ? 0 : ((IXmlLineInfo)this.reader).LinePosition;
			}
		}

		public override int AttributeCount
		{
			get
			{
				return this.reader.AttributeCount;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		public override bool CanResolveEntity
		{
			get
			{
				return this.reader.CanResolveEntity;
			}
		}

		public override int Depth
		{
			get
			{
				return this.reader.Depth;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.reader.EOF;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				return this.reader.HasAttributes;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.reader.HasValue;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.reader.IsDefault;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.reader.IsEmptyElement;
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

		public override string this[string name, string namespaceURI]
		{
			get
			{
				return this.reader[name, namespaceURI];
			}
		}

		public override string LocalName
		{
			get
			{
				return this.reader.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				return this.reader.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.reader.NamespaceURI;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return this.reader.NodeType;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.reader.Prefix;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this.reader.QuoteChar;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.reader.ReadState;
			}
		}

		public override string Value
		{
			get
			{
				return this.reader.Value;
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

		public override void Close()
		{
			this.reader.Close();
		}

		public override bool Equals(object obj)
		{
			return this.reader.Equals(obj);
		}

		public override string GetAttribute(int i)
		{
			return this.reader.GetAttribute(i);
		}

		public override string GetAttribute(string name)
		{
			return this.reader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			return this.reader.GetAttribute(name, namespaceURI);
		}

		public override int GetHashCode()
		{
			return this.reader.GetHashCode();
		}

		public override bool IsStartElement()
		{
			return this.reader.IsStartElement();
		}

		public override bool IsStartElement(string localname, string ns)
		{
			return this.reader.IsStartElement(localname, ns);
		}

		public override bool IsStartElement(string name)
		{
			return this.reader.IsStartElement(name);
		}

		public override string LookupNamespace(string prefix)
		{
			return this.reader.LookupNamespace(prefix);
		}

		public override void MoveToAttribute(int i)
		{
			this.reader.MoveToAttribute(i);
		}

		public override bool MoveToAttribute(string name)
		{
			return this.reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return this.reader.MoveToAttribute(name, ns);
		}

		public override XmlNodeType MoveToContent()
		{
			return this.reader.MoveToContent();
		}

		public override bool MoveToElement()
		{
			return this.reader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return this.reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return this.reader.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return this.reader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return this.reader.ReadAttributeValue();
		}

		public override string ReadElementString()
		{
			return this.reader.ReadElementString();
		}

		public override string ReadElementString(string localname, string ns)
		{
			return this.reader.ReadElementString(localname, ns);
		}

		public override string ReadElementString(string name)
		{
			return this.reader.ReadElementString(name);
		}

		public override void ReadEndElement()
		{
			this.reader.ReadEndElement();
		}

		public override string ReadInnerXml()
		{
			return this.reader.ReadInnerXml();
		}

		public override string ReadOuterXml()
		{
			return this.reader.ReadOuterXml();
		}

		public override void ReadStartElement()
		{
			this.reader.ReadStartElement();
		}

		public override void ReadStartElement(string localname, string ns)
		{
			this.reader.ReadStartElement(localname, ns);
		}

		public override void ReadStartElement(string name)
		{
			this.reader.ReadStartElement(name);
		}

		public override string ReadString()
		{
			return this.reader.ReadString();
		}

		public override void ResolveEntity()
		{
			this.reader.ResolveEntity();
		}

		public override void Skip()
		{
			this.reader.Skip();
		}

		public override string ToString()
		{
			return this.reader.ToString();
		}
	}
}
