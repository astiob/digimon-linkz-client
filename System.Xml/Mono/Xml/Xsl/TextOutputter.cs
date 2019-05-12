using System;
using System.IO;

namespace Mono.Xml.Xsl
{
	internal class TextOutputter : Outputter
	{
		private TextWriter _writer;

		private int _depth;

		private bool _ignoreNestedText;

		public TextOutputter(TextWriter w, bool ignoreNestedText)
		{
			this._writer = w;
			this._ignoreNestedText = ignoreNestedText;
		}

		public override void WriteStartElement(string prefix, string localName, string nsURI)
		{
			if (this._ignoreNestedText)
			{
				this._depth++;
			}
		}

		public override void WriteEndElement()
		{
			if (this._ignoreNestedText)
			{
				this._depth--;
			}
		}

		public override void WriteAttributeString(string prefix, string localName, string nsURI, string value)
		{
		}

		public override void WriteNamespaceDecl(string prefix, string nsUri)
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
			this.WriteImpl(text);
		}

		public override void WriteRaw(string data)
		{
			this.WriteImpl(data);
		}

		public override void WriteWhitespace(string text)
		{
			this.WriteImpl(text);
		}

		private void WriteImpl(string text)
		{
			if (!this._ignoreNestedText || this._depth == 0)
			{
				this._writer.Write(text);
			}
		}

		public override void Done()
		{
			this._writer.Flush();
		}

		public override bool CanProcessAttributes
		{
			get
			{
				return false;
			}
		}

		public override bool InsideCDataSection
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
