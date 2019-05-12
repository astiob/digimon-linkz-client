using System;
using System.IO;
using System.Text;

namespace Mono.Xml.Xsl
{
	internal class TextEmitter : Emitter
	{
		private TextWriter writer;

		public TextEmitter(TextWriter writer)
		{
			this.writer = writer;
		}

		public override void WriteStartDocument(Encoding encoding, StandaloneType standalone)
		{
		}

		public override void WriteEndDocument()
		{
		}

		public override void WriteDocType(string type, string publicId, string systemId)
		{
		}

		public override void WriteStartElement(string prefix, string localName, string nsURI)
		{
		}

		public override void WriteEndElement()
		{
		}

		public override void WriteAttributeString(string prefix, string localName, string nsURI, string value)
		{
		}

		public override void WriteComment(string text)
		{
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
		}

		public override void WriteString(string text)
		{
			this.writer.Write(text);
		}

		public override void WriteRaw(string data)
		{
			this.writer.Write(data);
		}

		public override void WriteCDataSection(string text)
		{
			this.writer.Write(text);
		}

		public override void Done()
		{
		}
	}
}
