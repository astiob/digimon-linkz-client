using System;
using System.Xml;

namespace Mono.Xml.XPath
{
	internal class XmlDocumentInsertionWriter : XmlWriter
	{
		private XmlNode parent;

		private XmlNode current;

		private XmlNode nextSibling;

		private WriteState state;

		private XmlAttribute attribute;

		public XmlDocumentInsertionWriter(XmlNode owner, XmlNode nextSibling)
		{
			this.parent = owner;
			if (this.parent == null)
			{
				throw new InvalidOperationException();
			}
			XmlNodeType nodeType = this.parent.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Document:
				this.current = ((XmlDocument)this.parent).CreateDocumentFragment();
				goto IL_A1;
			default:
				if (nodeType != XmlNodeType.Element)
				{
					throw new InvalidOperationException(string.Format("Insertion into {0} node is not allowed.", this.parent.NodeType));
				}
				break;
			case XmlNodeType.DocumentFragment:
				break;
			}
			this.current = this.parent.OwnerDocument.CreateDocumentFragment();
			IL_A1:
			this.nextSibling = nextSibling;
			this.state = WriteState.Content;
		}

		internal event XmlWriterClosedEventHandler Closed;

		public override WriteState WriteState
		{
			get
			{
				return this.state;
			}
		}

		public override void Close()
		{
			while (this.current.ParentNode != null)
			{
				this.current = this.current.ParentNode;
			}
			this.parent.InsertBefore((XmlDocumentFragment)this.current, this.nextSibling);
			if (this.Closed != null)
			{
				this.Closed(this);
			}
		}

		public override void Flush()
		{
		}

		public override string LookupPrefix(string ns)
		{
			return this.current.GetPrefixOfNamespace(ns);
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
			this.attribute = this.current.OwnerDocument.CreateAttribute(prefix, name, ns);
			this.state = WriteState.Attribute;
		}

		public override void WriteProcessingInstruction(string name, string value)
		{
			XmlProcessingInstruction newChild = this.current.OwnerDocument.CreateProcessingInstruction(name, value);
			this.current.AppendChild(newChild);
		}

		public override void WriteComment(string text)
		{
			XmlComment newChild = this.current.OwnerDocument.CreateComment(text);
			this.current.AppendChild(newChild);
		}

		public override void WriteCData(string text)
		{
			XmlCDataSection newChild = this.current.OwnerDocument.CreateCDataSection(text);
			this.current.AppendChild(newChild);
		}

		public override void WriteStartElement(string prefix, string name, string ns)
		{
			if (prefix == null && ns != null && ns.Length > 0)
			{
				prefix = this.LookupPrefix(ns);
			}
			XmlElement newChild = this.current.OwnerDocument.CreateElement(prefix, name, ns);
			this.current.AppendChild(newChild);
			this.current = newChild;
		}

		public override void WriteEndElement()
		{
			this.current = this.current.ParentNode;
			if (this.current == null)
			{
				throw new InvalidOperationException("No element is opened.");
			}
		}

		public override void WriteFullEndElement()
		{
			XmlElement xmlElement = this.current as XmlElement;
			if (xmlElement != null)
			{
				xmlElement.IsEmpty = false;
			}
			this.WriteEndElement();
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
			this.WriteString(Convert.ToBase64String(data, start, length));
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
			if (this.attribute != null)
			{
				XmlAttribute xmlAttribute = this.attribute;
				xmlAttribute.Value += text;
			}
			else
			{
				XmlText newChild = this.current.OwnerDocument.CreateTextNode(text);
				this.current.AppendChild(newChild);
			}
		}

		public override void WriteWhitespace(string text)
		{
			if (this.state != WriteState.Attribute)
			{
				this.current.AppendChild(this.current.OwnerDocument.CreateTextNode(text));
			}
			else if (this.attribute.ChildNodes.Count == 0)
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
			XmlElement xmlElement = (this.current as XmlElement) ?? ((this.nextSibling != null) ? null : (this.parent as XmlElement));
			if (this.state != WriteState.Attribute || xmlElement == null)
			{
				throw new InvalidOperationException("Current state is not inside attribute. Cannot close attribute.");
			}
			xmlElement.SetAttributeNode(this.attribute);
			this.attribute = null;
			this.state = WriteState.Content;
		}
	}
}
