using System;

namespace Mono.Xml.Xsl
{
	internal abstract class Outputter
	{
		public void WriteStartElement(string localName, string nsURI)
		{
			this.WriteStartElement(null, localName, nsURI);
		}

		public abstract void WriteStartElement(string prefix, string localName, string nsURI);

		public abstract void WriteEndElement();

		public virtual void WriteFullEndElement()
		{
			this.WriteEndElement();
		}

		public void WriteAttributeString(string localName, string value)
		{
			this.WriteAttributeString(string.Empty, localName, string.Empty, value);
		}

		public abstract void WriteAttributeString(string prefix, string localName, string nsURI, string value);

		public abstract void WriteNamespaceDecl(string prefix, string nsUri);

		public abstract void WriteComment(string text);

		public abstract void WriteProcessingInstruction(string name, string text);

		public abstract void WriteString(string text);

		public abstract void WriteRaw(string data);

		public abstract void WriteWhitespace(string text);

		public abstract void Done();

		public abstract bool CanProcessAttributes { get; }

		public abstract bool InsideCDataSection { get; set; }
	}
}
