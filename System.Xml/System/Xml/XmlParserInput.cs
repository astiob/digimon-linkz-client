using Mono.Xml;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Xml
{
	internal class XmlParserInput
	{
		private Stack sourceStack = new Stack();

		private XmlParserInput.XmlParserInputSource source;

		private bool has_peek;

		private int peek_char;

		private bool allowTextDecl = true;

		public XmlParserInput(TextReader reader, string baseURI) : this(reader, baseURI, 1, 0)
		{
		}

		public XmlParserInput(TextReader reader, string baseURI, int line, int column)
		{
			this.source = new XmlParserInput.XmlParserInputSource(reader, baseURI, false, line, column);
		}

		public void Close()
		{
			while (this.sourceStack.Count > 0)
			{
				((XmlParserInput.XmlParserInputSource)this.sourceStack.Pop()).Close();
			}
			this.source.Close();
		}

		public void Expect(int expected)
		{
			int num = this.ReadChar();
			if (num != expected)
			{
				throw this.ReaderError(string.Format(CultureInfo.InvariantCulture, "expected '{0}' ({1:X}) but found '{2}' ({3:X})", new object[]
				{
					(char)expected,
					expected,
					(char)num,
					num
				}));
			}
		}

		public void Expect(string expected)
		{
			int length = expected.Length;
			for (int i = 0; i < length; i++)
			{
				this.Expect((int)expected[i]);
			}
		}

		public void PushPEBuffer(DTDParameterEntityDeclaration pe)
		{
			this.sourceStack.Push(this.source);
			this.source = new XmlParserInput.XmlParserInputSource(new StringReader(pe.ReplacementText), pe.ActualUri, true, 1, 0);
		}

		private int ReadSourceChar()
		{
			int num = this.source.Read();
			while (num < 0 && this.sourceStack.Count > 0)
			{
				this.source = (this.sourceStack.Pop() as XmlParserInput.XmlParserInputSource);
				num = this.source.Read();
			}
			return num;
		}

		public int PeekChar()
		{
			if (this.has_peek)
			{
				return this.peek_char;
			}
			this.peek_char = this.ReadSourceChar();
			if (this.peek_char >= 55296 && this.peek_char <= 56319)
			{
				this.peek_char = 65536 + (this.peek_char - 55296 << 10);
				int num = this.ReadSourceChar();
				if (num >= 56320 && num <= 57343)
				{
					this.peek_char += num - 56320;
				}
			}
			this.has_peek = true;
			return this.peek_char;
		}

		public int ReadChar()
		{
			int result = this.PeekChar();
			this.has_peek = false;
			return result;
		}

		public string BaseURI
		{
			get
			{
				return this.source.BaseURI;
			}
		}

		public bool HasPEBuffer
		{
			get
			{
				return this.sourceStack.Count > 0;
			}
		}

		public int LineNumber
		{
			get
			{
				return this.source.LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return this.source.LinePosition;
			}
		}

		public bool AllowTextDecl
		{
			get
			{
				return this.allowTextDecl;
			}
			set
			{
				this.allowTextDecl = value;
			}
		}

		private XmlException ReaderError(string message)
		{
			return new XmlException(message, null, this.LineNumber, this.LinePosition);
		}

		private class XmlParserInputSource
		{
			public readonly string BaseURI;

			private readonly TextReader reader;

			public int state;

			public bool isPE;

			private int line;

			private int column;

			public XmlParserInputSource(TextReader reader, string baseUri, bool pe, int line, int column)
			{
				this.BaseURI = baseUri;
				this.reader = reader;
				this.isPE = pe;
				this.line = line;
				this.column = column;
			}

			public int LineNumber
			{
				get
				{
					return this.line;
				}
			}

			public int LinePosition
			{
				get
				{
					return this.column;
				}
			}

			public void Close()
			{
				this.reader.Close();
			}

			public int Read()
			{
				if (this.state == 2)
				{
					return -1;
				}
				if (this.isPE && this.state == 0)
				{
					this.state = 1;
					return 32;
				}
				int num = this.reader.Read();
				if (num == 10)
				{
					this.line++;
					this.column = 1;
				}
				else if (num >= 0)
				{
					this.column++;
				}
				if (num < 0 && this.state == 1)
				{
					this.state = 2;
					return 32;
				}
				return num;
			}
		}
	}
}
