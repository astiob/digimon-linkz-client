using System;
using System.Text;
using System.Xml;

namespace Mono.Xml.Xsl
{
	internal class XmlWriterEmitter : Emitter
	{
		private XmlWriter writer;

		public XmlWriterEmitter(XmlWriter writer)
		{
			this.writer = writer;
		}

		public override void WriteStartDocument(Encoding encoding, StandaloneType standalone)
		{
			string text = string.Empty;
			if (standalone != StandaloneType.YES)
			{
				if (standalone == StandaloneType.NO)
				{
					text = " standalone=\"no\"";
				}
			}
			else
			{
				text = " standalone=\"yes\"";
			}
			if (encoding == null)
			{
				this.writer.WriteProcessingInstruction("xml", "version=\"1.0\"" + text);
			}
			else
			{
				this.writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + encoding.WebName + "\"" + text);
			}
		}

		public override void WriteEndDocument()
		{
		}

		public override void WriteDocType(string type, string publicId, string systemId)
		{
			if (systemId == null)
			{
				return;
			}
			this.writer.WriteDocType(type, publicId, systemId, null);
		}

		public override void WriteStartElement(string prefix, string localName, string nsURI)
		{
			this.writer.WriteStartElement(prefix, localName, nsURI);
		}

		public override void WriteEndElement()
		{
			this.writer.WriteEndElement();
		}

		public override void WriteFullEndElement()
		{
			this.writer.WriteFullEndElement();
		}

		public override void WriteAttributeString(string prefix, string localName, string nsURI, string value)
		{
			this.writer.WriteAttributeString(prefix, localName, nsURI, value);
		}

		public override void WriteComment(string text)
		{
			while (text.IndexOf("--") >= 0)
			{
				text = text.Replace("--", "- -");
			}
			if (text.EndsWith("-"))
			{
				text += ' ';
			}
			this.writer.WriteComment(text);
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			while (text.IndexOf("?>") >= 0)
			{
				text = text.Replace("?>", "? >");
			}
			this.writer.WriteProcessingInstruction(name, text);
		}

		public override void WriteString(string text)
		{
			this.writer.WriteString(text);
		}

		public override void WriteRaw(string data)
		{
			this.writer.WriteRaw(data);
		}

		public override void WriteCDataSection(string text)
		{
			int num = text.IndexOf("]]>");
			if (num >= 0)
			{
				this.writer.WriteCData(text.Substring(0, num + 2));
				this.WriteCDataSection(text.Substring(num + 2));
			}
			else
			{
				this.writer.WriteCData(text);
			}
		}

		public override void WriteWhitespace(string value)
		{
			this.writer.WriteWhitespace(value);
		}

		public override void Done()
		{
			this.writer.Flush();
		}
	}
}
