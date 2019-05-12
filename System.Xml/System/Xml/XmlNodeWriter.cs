using System;

namespace System.Xml
{
	internal class XmlNodeWriter : XmlWriter
	{
		private XmlDocument doc;

		private bool isClosed;

		private XmlNode current;

		private XmlAttribute attribute;

		private bool isDocumentEntity;

		private XmlDocumentFragment fragment;

		private XmlNodeType state;

		public XmlNodeWriter() : this(true)
		{
		}

		public XmlNodeWriter(bool isDocumentEntity)
		{
			this.doc = new XmlDocument();
			this.state = XmlNodeType.None;
			this.isDocumentEntity = isDocumentEntity;
			if (!isDocumentEntity)
			{
				this.current = (this.fragment = this.doc.CreateDocumentFragment());
			}
		}

		public XmlNode Document
		{
			get
			{
				return (!this.isDocumentEntity) ? this.fragment : this.doc;
			}
		}

		public override WriteState WriteState
		{
			get
			{
				if (this.isClosed)
				{
					return WriteState.Closed;
				}
				if (this.attribute != null)
				{
					return WriteState.Attribute;
				}
				XmlNodeType xmlNodeType = this.state;
				if (xmlNodeType == XmlNodeType.None)
				{
					return WriteState.Start;
				}
				if (xmlNodeType == XmlNodeType.DocumentType)
				{
					return WriteState.Element;
				}
				if (xmlNodeType != XmlNodeType.XmlDeclaration)
				{
					return WriteState.Content;
				}
				return WriteState.Prolog;
			}
		}

		public override string XmlLang
		{
			get
			{
				for (XmlElement xmlElement = this.current as XmlElement; xmlElement != null; xmlElement = (xmlElement.ParentNode as XmlElement))
				{
					if (xmlElement.HasAttribute("xml:lang"))
					{
						return xmlElement.GetAttribute("xml:lang");
					}
				}
				return string.Empty;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				XmlElement xmlElement = this.current as XmlElement;
				while (xmlElement != null)
				{
					string text = xmlElement.GetAttribute("xml:space");
					string text2 = text;
					switch (text2)
					{
					case "preserve":
						return XmlSpace.Preserve;
					case "default":
						return XmlSpace.Default;

						xmlElement = (xmlElement.ParentNode as XmlElement);
						continue;
					}
					throw new InvalidOperationException(string.Format("Invalid xml:space {0}.", text));
				}
				return XmlSpace.None;
			}
		}

		private void CheckState()
		{
			if (this.isClosed)
			{
				throw new InvalidOperationException();
			}
		}

		private void WritePossiblyTopLevelNode(XmlNode n, bool possiblyAttribute)
		{
			this.CheckState();
			if (!possiblyAttribute && this.attribute != null)
			{
				throw new InvalidOperationException(string.Format("Current state is not acceptable for {0}.", n.NodeType));
			}
			if (this.state != XmlNodeType.Element)
			{
				this.Document.AppendChild(n);
			}
			else if (this.attribute != null)
			{
				this.attribute.AppendChild(n);
			}
			else
			{
				this.current.AppendChild(n);
			}
			if (this.state == XmlNodeType.None)
			{
				this.state = XmlNodeType.XmlDeclaration;
			}
		}

		public override void Close()
		{
			this.CheckState();
			this.isClosed = true;
		}

		public override void Flush()
		{
		}

		public override string LookupPrefix(string ns)
		{
			this.CheckState();
			if (this.current == null)
			{
				throw new InvalidOperationException();
			}
			return this.current.GetPrefixOfNamespace(ns);
		}

		public override void WriteStartDocument()
		{
			this.WriteStartDocument(null);
		}

		public override void WriteStartDocument(bool standalone)
		{
			this.WriteStartDocument((!standalone) ? "no" : "yes");
		}

		private void WriteStartDocument(string sddecl)
		{
			this.CheckState();
			if (this.state != XmlNodeType.None)
			{
				throw new InvalidOperationException("Current state is not acceptable for xmldecl.");
			}
			this.doc.AppendChild(this.doc.CreateXmlDeclaration("1.0", null, sddecl));
			this.state = XmlNodeType.XmlDeclaration;
		}

		public override void WriteEndDocument()
		{
			this.CheckState();
			this.isClosed = true;
		}

		public override void WriteDocType(string name, string publicId, string systemId, string internalSubset)
		{
			this.CheckState();
			XmlNodeType xmlNodeType = this.state;
			if (xmlNodeType != XmlNodeType.None && xmlNodeType != XmlNodeType.XmlDeclaration)
			{
				throw new InvalidOperationException("Current state is not acceptable for doctype.");
			}
			this.doc.AppendChild(this.doc.CreateDocumentType(name, publicId, systemId, internalSubset));
			this.state = XmlNodeType.DocumentType;
		}

		public override void WriteStartElement(string prefix, string name, string ns)
		{
			this.CheckState();
			if (this.isDocumentEntity && this.state == XmlNodeType.EndElement && this.doc.DocumentElement != null)
			{
				throw new InvalidOperationException("Current state is not acceptable for startElement.");
			}
			XmlElement newChild = this.doc.CreateElement(prefix, name, ns);
			if (this.current == null)
			{
				this.Document.AppendChild(newChild);
				this.state = XmlNodeType.Element;
			}
			else
			{
				this.current.AppendChild(newChild);
				this.state = XmlNodeType.Element;
			}
			this.current = newChild;
		}

		public override void WriteEndElement()
		{
			this.WriteEndElementInternal(false);
		}

		public override void WriteFullEndElement()
		{
			this.WriteEndElementInternal(true);
		}

		private void WriteEndElementInternal(bool forceFull)
		{
			this.CheckState();
			if (this.current == null)
			{
				throw new InvalidOperationException("Current state is not acceptable for endElement.");
			}
			if (!forceFull && this.current.FirstChild == null)
			{
				((XmlElement)this.current).IsEmpty = true;
			}
			if (this.isDocumentEntity && this.current.ParentNode == this.doc)
			{
				this.state = XmlNodeType.EndElement;
			}
			else
			{
				this.current = this.current.ParentNode;
			}
		}

		public override void WriteStartAttribute(string prefix, string name, string ns)
		{
			this.CheckState();
			if (this.attribute != null)
			{
				throw new InvalidOperationException("There is an open attribute.");
			}
			if (!(this.current is XmlElement))
			{
				throw new InvalidOperationException("Current state is not acceptable for startAttribute.");
			}
			this.attribute = this.doc.CreateAttribute(prefix, name, ns);
			((XmlElement)this.current).SetAttributeNode(this.attribute);
		}

		public override void WriteEndAttribute()
		{
			this.CheckState();
			if (this.attribute == null)
			{
				throw new InvalidOperationException("Current state is not acceptable for startAttribute.");
			}
			this.attribute = null;
		}

		public override void WriteCData(string data)
		{
			this.CheckState();
			if (this.current == null)
			{
				throw new InvalidOperationException("Current state is not acceptable for CDATAsection.");
			}
			this.current.AppendChild(this.doc.CreateCDataSection(data));
		}

		public override void WriteComment(string comment)
		{
			this.WritePossiblyTopLevelNode(this.doc.CreateComment(comment), false);
		}

		public override void WriteProcessingInstruction(string name, string value)
		{
			this.WritePossiblyTopLevelNode(this.doc.CreateProcessingInstruction(name, value), false);
		}

		public override void WriteEntityRef(string name)
		{
			this.WritePossiblyTopLevelNode(this.doc.CreateEntityReference(name), true);
		}

		public override void WriteCharEntity(char c)
		{
			this.WritePossiblyTopLevelNode(this.doc.CreateTextNode(new string(new char[]
			{
				c
			}, 0, 1)), true);
		}

		public override void WriteWhitespace(string ws)
		{
			this.WritePossiblyTopLevelNode(this.doc.CreateWhitespace(ws), true);
		}

		public override void WriteString(string data)
		{
			this.CheckState();
			if (this.current == null)
			{
				throw new InvalidOperationException("Current state is not acceptable for Text.");
			}
			if (this.attribute != null)
			{
				this.attribute.AppendChild(this.doc.CreateTextNode(data));
			}
			else
			{
				XmlText xmlText = this.current.LastChild as XmlText;
				if (xmlText == null)
				{
					this.current.AppendChild(this.doc.CreateTextNode(data));
				}
				else
				{
					xmlText.AppendData(data);
				}
			}
		}

		public override void WriteName(string name)
		{
			this.WriteString(name);
		}

		public override void WriteNmToken(string nmtoken)
		{
			this.WriteString(nmtoken);
		}

		public override void WriteQualifiedName(string name, string ns)
		{
			string text = this.LookupPrefix(ns);
			if (text == null)
			{
				throw new ArgumentException(string.Format("Invalid namespace {0}", ns));
			}
			if (text != string.Empty)
			{
				this.WriteString(name);
			}
			else
			{
				this.WriteString(text + ":" + name);
			}
		}

		public override void WriteChars(char[] chars, int start, int len)
		{
			this.WriteString(new string(chars, start, len));
		}

		public override void WriteRaw(string data)
		{
			this.WriteString(data);
		}

		public override void WriteRaw(char[] chars, int start, int len)
		{
			this.WriteChars(chars, start, len);
		}

		public override void WriteBase64(byte[] data, int start, int len)
		{
			this.WriteString(Convert.ToBase64String(data, start, len));
		}

		public override void WriteBinHex(byte[] data, int start, int len)
		{
			throw new NotImplementedException();
		}

		public override void WriteSurrogateCharEntity(char c1, char c2)
		{
			throw new NotImplementedException();
		}
	}
}
