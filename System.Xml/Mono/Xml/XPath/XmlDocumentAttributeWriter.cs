using System;
using System.Xml;

namespace Mono.Xml.XPath
{
	internal class XmlDocumentAttributeWriter : XmlWriter
	{
		private XmlElement element;

		private WriteState state;

		private XmlAttribute attribute;

		public XmlDocumentAttributeWriter(XmlNode owner)
		{
			this.element = (owner as XmlElement);
			if (this.element == null)
			{
				throw new ArgumentException("To write attributes, current node must be an element.");
			}
			this.state = WriteState.Content;
		}

		public override WriteState WriteState
		{
			get
			{
				return this.state;
			}
		}

		public override void Close()
		{
		}

		public override void Flush()
		{
		}

		public override string LookupPrefix(string ns)
		{
			return this.element.GetPrefixOfNamespace(ns);
		}

		public override void WriteStartAttribute(string prefix, string name, string ns)
		{
			if (this.state != WriteState.Content)
			{
				throw new InvalidOperationException("Current state is not inside element. Cannot start attribute.");
			}
			if (prefix == null && ns != null && ns.Length > 0)
			{
				prefix = this.LookupPrefix(ns);
			}
			this.attribute = this.element.OwnerDocument.CreateAttribute(prefix, name, ns);
			this.state = WriteState.Attribute;
		}

		public override void WriteProcessingInstruction(string name, string value)
		{
			throw new NotSupportedException();
		}

		public override void WriteComment(string text)
		{
			throw new NotSupportedException();
		}

		public override void WriteCData(string text)
		{
			throw new NotSupportedException();
		}

		public override void WriteStartElement(string prefix, string name, string ns)
		{
			throw new NotSupportedException();
		}

		public override void WriteEndElement()
		{
			throw new NotSupportedException();
		}

		public override void WriteFullEndElement()
		{
			throw new NotSupportedException();
		}

		public override void WriteDocType(string name, string pubid, string systemId, string intsubset)
		{
			throw new NotSupportedException();
		}

		public override void WriteStartDocument()
		{
			throw new NotSupportedException();
		}

		public override void WriteStartDocument(bool standalone)
		{
			throw new NotSupportedException();
		}

		public override void WriteEndDocument()
		{
			throw new NotSupportedException();
		}

		public override void WriteBase64(byte[] data, int start, int length)
		{
			throw new NotSupportedException();
		}

		public override void WriteRaw(char[] raw, int start, int length)
		{
			throw new NotSupportedException();
		}

		public override void WriteRaw(string raw)
		{
			throw new NotSupportedException();
		}

		public override void WriteSurrogateCharEntity(char msb, char lsb)
		{
			throw new NotSupportedException();
		}

		public override void WriteCharEntity(char c)
		{
			throw new NotSupportedException();
		}

		public override void WriteEntityRef(string entname)
		{
			if (this.state != WriteState.Attribute)
			{
				throw new InvalidOperationException("Current state is not inside attribute. Cannot write attribute value.");
			}
			this.attribute.AppendChild(this.attribute.OwnerDocument.CreateEntityReference(entname));
		}

		public override void WriteChars(char[] data, int start, int length)
		{
			this.WriteString(new string(data, start, length));
		}

		public override void WriteString(string text)
		{
			if (this.state != WriteState.Attribute)
			{
				throw new InvalidOperationException("Current state is not inside attribute. Cannot write attribute value.");
			}
			XmlAttribute xmlAttribute = this.attribute;
			xmlAttribute.Value += text;
		}

		public override void WriteWhitespace(string text)
		{
			if (this.state != WriteState.Attribute)
			{
				throw new InvalidOperationException("Current state is not inside attribute. Cannot write attribute value.");
			}
			if (this.attribute.ChildNodes.Count == 0)
			{
				this.attribute.AppendChild(this.attribute.OwnerDocument.CreateWhitespace(text));
			}
			else
			{
				XmlAttribute xmlAttribute = this.attribute;
				xmlAttribute.Value += text;
			}
		}

		public override void WriteEndAttribute()
		{
			if (this.state != WriteState.Attribute)
			{
				throw new InvalidOperationException("Current state is not inside attribute. Cannot close attribute.");
			}
			this.element.SetAttributeNode(this.attribute);
			this.attribute = null;
			this.state = WriteState.Content;
		}
	}
}
