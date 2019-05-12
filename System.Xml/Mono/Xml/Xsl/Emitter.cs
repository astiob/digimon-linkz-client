using System;
using System.Text;

namespace Mono.Xml.Xsl
{
	internal abstract class Emitter
	{
		public abstract void WriteStartDocument(Encoding encoding, StandaloneType standalone);

		public abstract void WriteEndDocument();

		public abstract void WriteDocType(string type, string publicId, string systemId);

		public abstract void WriteStartElement(string prefix, string localName, string nsURI);

		public abstract void WriteEndElement();

		public virtual void WriteFullEndElement()
		{
			this.WriteEndElement();
		}

		public abstract void WriteAttributeString(string prefix, string localName, string nsURI, string value);

		public abstract void WriteComment(string text);

		public abstract void WriteProcessingInstruction(string name, string text);

		public abstract void WriteString(string text);

		public abstract void WriteCDataSection(string text);

		public abstract void WriteRaw(string data);

		public abstract void Done();

		public virtual void WriteWhitespace(string text)
		{
			this.WriteString(text);
		}
	}
}
