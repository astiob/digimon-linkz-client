using System;
using System.Collections.Generic;
using System.Xml;

namespace Mono.Xml
{
	internal class SubtreeXmlReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
	{
		private int startDepth;

		private bool eof;

		private bool initial;

		private bool read;

		private XmlReader Reader;

		private IXmlLineInfo li;

		private IXmlNamespaceResolver nsResolver;

		public SubtreeXmlReader(XmlReader reader)
		{
			this.Reader = reader;
			this.li = (reader as IXmlLineInfo);
			this.nsResolver = (reader as IXmlNamespaceResolver);
			this.initial = true;
			this.startDepth = reader.Depth;
			if (reader.ReadState == ReadState.Initial)
			{
				this.startDepth = -1;
			}
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			IDictionary<string, string> result;
			if (this.nsResolver != null)
			{
				IDictionary<string, string> namespacesInScope = this.nsResolver.GetNamespacesInScope(scope);
				result = namespacesInScope;
			}
			else
			{
				result = new Dictionary<string, string>();
			}
			return result;
		}

		string IXmlNamespaceResolver.LookupPrefix(string ns)
		{
			return (this.nsResolver == null) ? string.Empty : this.nsResolver.LookupPrefix(ns);
		}

		public override int AttributeCount
		{
			get
			{
				return (!this.initial) ? this.Reader.AttributeCount : 0;
			}
		}

		public override bool CanReadBinaryContent
		{
			get
			{
				return this.Reader.CanReadBinaryContent;
			}
		}

		public override bool CanReadValueChunk
		{
			get
			{
				return this.Reader.CanReadValueChunk;
			}
		}

		public override int Depth
		{
			get
			{
				return this.Reader.Depth - this.startDepth;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.Reader.BaseURI;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.eof || this.Reader.EOF;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.Reader.IsEmptyElement;
			}
		}

		public int LineNumber
		{
			get
			{
				return (!this.initial) ? ((this.li == null) ? 0 : this.li.LineNumber) : 0;
			}
		}

		public int LinePosition
		{
			get
			{
				return (!this.initial) ? ((this.li == null) ? 0 : this.li.LinePosition) : 0;
			}
		}

		public override bool HasValue
		{
			get
			{
				return !this.initial && !this.eof && this.Reader.HasValue;
			}
		}

		public override string LocalName
		{
			get
			{
				return (!this.initial && !this.eof) ? this.Reader.LocalName : string.Empty;
			}
		}

		public override string Name
		{
			get
			{
				return (!this.initial && !this.eof) ? this.Reader.Name : string.Empty;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.Reader.NameTable;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return (!this.initial && !this.eof) ? this.Reader.NamespaceURI : string.Empty;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return (!this.initial && !this.eof) ? this.Reader.NodeType : XmlNodeType.None;
			}
		}

		public override string Prefix
		{
			get
			{
				return (!this.initial && !this.eof) ? this.Reader.Prefix : string.Empty;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return (!this.initial) ? ((!this.eof) ? this.Reader.ReadState : ReadState.EndOfFile) : ReadState.Initial;
			}
		}

		public override XmlReaderSettings Settings
		{
			get
			{
				return this.Reader.Settings;
			}
		}

		public override string Value
		{
			get
			{
				return (!this.initial) ? this.Reader.Value : string.Empty;
			}
		}

		public override void Close()
		{
			while (this.Read())
			{
			}
		}

		public override string GetAttribute(int i)
		{
			return (!this.initial) ? this.Reader.GetAttribute(i) : null;
		}

		public override string GetAttribute(string name)
		{
			return (!this.initial) ? this.Reader.GetAttribute(name) : null;
		}

		public override string GetAttribute(string local, string ns)
		{
			return (!this.initial) ? this.Reader.GetAttribute(local, ns) : null;
		}

		public bool HasLineInfo()
		{
			return this.li != null && this.li.HasLineInfo();
		}

		public override string LookupNamespace(string prefix)
		{
			return this.Reader.LookupNamespace(prefix);
		}

		public override bool MoveToFirstAttribute()
		{
			return !this.initial && this.Reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return !this.initial && this.Reader.MoveToNextAttribute();
		}

		public override void MoveToAttribute(int i)
		{
			if (!this.initial)
			{
				this.Reader.MoveToAttribute(i);
			}
		}

		public override bool MoveToAttribute(string name)
		{
			return !this.initial && this.Reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string local, string ns)
		{
			return !this.initial && this.Reader.MoveToAttribute(local, ns);
		}

		public override bool MoveToElement()
		{
			return !this.initial && this.Reader.MoveToElement();
		}

		public override bool Read()
		{
			if (this.initial)
			{
				this.initial = false;
				return true;
			}
			if (!this.read)
			{
				this.read = true;
				this.Reader.MoveToElement();
				bool flag = !this.Reader.IsEmptyElement && this.Reader.Read();
				if (!flag)
				{
					this.eof = true;
				}
				return flag;
			}
			this.Reader.MoveToElement();
			if (this.Reader.Depth > this.startDepth && this.Reader.Read())
			{
				return true;
			}
			this.eof = true;
			return false;
		}

		public override bool ReadAttributeValue()
		{
			return !this.initial && !this.eof && this.Reader.ReadAttributeValue();
		}

		public override void ResolveEntity()
		{
			if (this.initial || this.eof)
			{
				return;
			}
			this.Reader.ResolveEntity();
		}
	}
}
