using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Mono.Xml.Xsl
{
	internal class GenericOutputter : Outputter
	{
		private Hashtable _outputs;

		private XslOutput _currentOutput;

		private Emitter _emitter;

		private TextWriter pendingTextWriter;

		private StringBuilder pendingFirstSpaces;

		private WriteState _state;

		private Attribute[] pendingAttributes = new Attribute[10];

		private int pendingAttributesPos;

		private XmlNamespaceManager _nsManager;

		private ListDictionary _currentNamespaceDecls;

		private ArrayList newNamespaces = new ArrayList();

		private NameTable _nt;

		private Encoding _encoding;

		private bool _canProcessAttributes;

		private bool _insideCData;

		private bool _omitXmlDeclaration;

		private int _xpCount;

		private GenericOutputter(Hashtable outputs, Encoding encoding)
		{
			this._encoding = encoding;
			this._outputs = outputs;
			this._currentOutput = (XslOutput)outputs[string.Empty];
			this._state = WriteState.Prolog;
			this._nt = new NameTable();
			this._nsManager = new XmlNamespaceManager(this._nt);
			this._currentNamespaceDecls = new ListDictionary();
			this._omitXmlDeclaration = false;
		}

		public GenericOutputter(XmlWriter writer, Hashtable outputs, Encoding encoding) : this(writer, outputs, encoding, false)
		{
		}

		internal GenericOutputter(XmlWriter writer, Hashtable outputs, Encoding encoding, bool isVariable) : this(outputs, encoding)
		{
			this._emitter = new XmlWriterEmitter(writer);
			this._state = writer.WriteState;
			this._omitXmlDeclaration = true;
		}

		public GenericOutputter(TextWriter writer, Hashtable outputs, Encoding encoding) : this(outputs, encoding)
		{
			this.pendingTextWriter = writer;
		}

		internal GenericOutputter(TextWriter writer, Hashtable outputs) : this(writer, outputs, null)
		{
		}

		internal GenericOutputter(XmlWriter writer, Hashtable outputs) : this(writer, outputs, null)
		{
		}

		private Emitter Emitter
		{
			get
			{
				if (this._emitter == null)
				{
					this.DetermineOutputMethod(null, null);
				}
				return this._emitter;
			}
		}

		private void DetermineOutputMethod(string localName, string ns)
		{
			XslOutput xslOutput = (XslOutput)this._outputs[string.Empty];
			switch (xslOutput.Method)
			{
			case OutputMethod.XML:
				goto IL_89;
			case OutputMethod.HTML:
				break;
			case OutputMethod.Text:
				this._emitter = new TextEmitter(this.pendingTextWriter);
				goto IL_11B;
			default:
				if (localName == null || string.Compare(localName, "html", true, CultureInfo.InvariantCulture) != 0 || !(ns == string.Empty))
				{
					goto IL_89;
				}
				break;
			}
			this._emitter = new HtmlEmitter(this.pendingTextWriter, xslOutput);
			goto IL_11B;
			IL_89:
			XmlTextWriter xmlTextWriter = new XmlTextWriter(this.pendingTextWriter);
			if (xslOutput.Indent == "yes")
			{
				xmlTextWriter.Formatting = Formatting.Indented;
			}
			this._emitter = new XmlWriterEmitter(xmlTextWriter);
			if (!this._omitXmlDeclaration && !xslOutput.OmitXmlDeclaration)
			{
				this._emitter.WriteStartDocument((this._encoding == null) ? xslOutput.Encoding : this._encoding, xslOutput.Standalone);
			}
			IL_11B:
			this.pendingTextWriter = null;
		}

		private void CheckState()
		{
			if (this._state == WriteState.Element)
			{
				this._nsManager.PushScope();
				foreach (object obj in this._currentNamespaceDecls.Keys)
				{
					string text = (string)obj;
					string text2 = this._currentNamespaceDecls[text] as string;
					if (!(this._nsManager.LookupNamespace(text, false) == text2))
					{
						this.newNamespaces.Add(text);
						this._nsManager.AddNamespace(text, text2);
					}
				}
				for (int i = 0; i < this.pendingAttributesPos; i++)
				{
					Attribute attribute = this.pendingAttributes[i];
					string text3 = attribute.Prefix;
					if (text3 == "xml" && attribute.Namespace != "http://www.w3.org/XML/1998/namespace")
					{
						text3 = string.Empty;
					}
					string text4 = this._nsManager.LookupPrefix(attribute.Namespace, false);
					if (text3.Length == 0 && attribute.Namespace.Length > 0)
					{
						text3 = text4;
					}
					if (attribute.Namespace.Length > 0 && (text3 == null || text3 == string.Empty))
					{
						text3 = "xp_" + this._xpCount++;
						while (this._nsManager.LookupNamespace(text3) != null)
						{
							text3 = "xp_" + this._xpCount++;
						}
						this.newNamespaces.Add(text3);
						this._currentNamespaceDecls.Add(text3, attribute.Namespace);
						this._nsManager.AddNamespace(text3, attribute.Namespace);
					}
					this.Emitter.WriteAttributeString(text3, attribute.LocalName, attribute.Namespace, attribute.Value);
				}
				for (int j = 0; j < this.newNamespaces.Count; j++)
				{
					string text5 = (string)this.newNamespaces[j];
					string value = this._currentNamespaceDecls[text5] as string;
					if (text5 != string.Empty)
					{
						this.Emitter.WriteAttributeString("xmlns", text5, "http://www.w3.org/2000/xmlns/", value);
					}
					else
					{
						this.Emitter.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", value);
					}
				}
				this._currentNamespaceDecls.Clear();
				this._state = WriteState.Content;
				this.newNamespaces.Clear();
			}
			this._canProcessAttributes = false;
		}

		public override void WriteStartElement(string prefix, string localName, string nsURI)
		{
			if (this._emitter == null)
			{
				this.DetermineOutputMethod(localName, nsURI);
				if (this.pendingFirstSpaces != null)
				{
					this.WriteWhitespace(this.pendingFirstSpaces.ToString());
					this.pendingFirstSpaces = null;
				}
			}
			if (this._state == WriteState.Prolog && (this._currentOutput.DoctypePublic != null || this._currentOutput.DoctypeSystem != null))
			{
				this.Emitter.WriteDocType(prefix + ((prefix != null) ? string.Empty : ":") + localName, this._currentOutput.DoctypePublic, this._currentOutput.DoctypeSystem);
			}
			this.CheckState();
			if (nsURI == string.Empty)
			{
				prefix = string.Empty;
			}
			this.Emitter.WriteStartElement(prefix, localName, nsURI);
			this._state = WriteState.Element;
			if (this._nsManager.LookupNamespace(prefix, false) != nsURI)
			{
				this._currentNamespaceDecls[prefix] = nsURI;
			}
			this.pendingAttributesPos = 0;
			this._canProcessAttributes = true;
		}

		public override void WriteEndElement()
		{
			this.WriteEndElementInternal(false);
		}

		public override void WriteFullEndElement()
		{
			this.WriteEndElementInternal(true);
		}

		private void WriteEndElementInternal(bool fullEndElement)
		{
			this.CheckState();
			if (fullEndElement)
			{
				this.Emitter.WriteFullEndElement();
			}
			else
			{
				this.Emitter.WriteEndElement();
			}
			this._state = WriteState.Content;
			this._nsManager.PopScope();
		}

		public override void WriteAttributeString(string prefix, string localName, string nsURI, string value)
		{
			for (int i = 0; i < this.pendingAttributesPos; i++)
			{
				Attribute attribute = this.pendingAttributes[i];
				if (attribute.LocalName == localName && attribute.Namespace == nsURI)
				{
					this.pendingAttributes[i].Value = value;
					this.pendingAttributes[i].Prefix = prefix;
					return;
				}
			}
			if (this.pendingAttributesPos == this.pendingAttributes.Length)
			{
				Attribute[] sourceArray = this.pendingAttributes;
				this.pendingAttributes = new Attribute[this.pendingAttributesPos * 2 + 1];
				if (this.pendingAttributesPos > 0)
				{
					Array.Copy(sourceArray, 0, this.pendingAttributes, 0, this.pendingAttributesPos);
				}
			}
			this.pendingAttributes[this.pendingAttributesPos].Prefix = prefix;
			this.pendingAttributes[this.pendingAttributesPos].Namespace = nsURI;
			this.pendingAttributes[this.pendingAttributesPos].LocalName = localName;
			this.pendingAttributes[this.pendingAttributesPos].Value = value;
			this.pendingAttributesPos++;
		}

		public override void WriteNamespaceDecl(string prefix, string nsUri)
		{
			if (this._nsManager.LookupNamespace(prefix, false) == nsUri)
			{
				return;
			}
			for (int i = 0; i < this.pendingAttributesPos; i++)
			{
				Attribute attribute = this.pendingAttributes[i];
				if (attribute.Prefix == prefix || attribute.Namespace == nsUri)
				{
					return;
				}
			}
			if (this._currentNamespaceDecls[prefix] as string != nsUri)
			{
				this._currentNamespaceDecls[prefix] = nsUri;
			}
		}

		public override void WriteComment(string text)
		{
			this.CheckState();
			this.Emitter.WriteComment(text);
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			this.CheckState();
			this.Emitter.WriteProcessingInstruction(name, text);
		}

		public override void WriteString(string text)
		{
			this.CheckState();
			if (this._insideCData)
			{
				this.Emitter.WriteCDataSection(text);
			}
			else if (this._state != WriteState.Content && text.Length > 0 && XmlChar.IsWhitespace(text))
			{
				this.Emitter.WriteWhitespace(text);
			}
			else
			{
				this.Emitter.WriteString(text);
			}
		}

		public override void WriteRaw(string data)
		{
			this.CheckState();
			this.Emitter.WriteRaw(data);
		}

		public override void WriteWhitespace(string text)
		{
			if (this._emitter == null)
			{
				if (this.pendingFirstSpaces == null)
				{
					this.pendingFirstSpaces = new StringBuilder();
				}
				this.pendingFirstSpaces.Append(text);
				if (this._state == WriteState.Start)
				{
					this._state = WriteState.Prolog;
				}
			}
			else
			{
				this.CheckState();
				this.Emitter.WriteWhitespace(text);
			}
		}

		public override void Done()
		{
			this.Emitter.Done();
			this._state = WriteState.Closed;
		}

		public override bool CanProcessAttributes
		{
			get
			{
				return this._canProcessAttributes;
			}
		}

		public override bool InsideCDataSection
		{
			get
			{
				return this._insideCData;
			}
			set
			{
				this._insideCData = value;
			}
		}
	}
}
