using Mono.Xml.Xsl.yyParser;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class Tokenizer : yyInput
	{
		private const char EOL = '\0';

		private string m_rgchInput;

		private int m_ich;

		private int m_cch;

		private int m_iToken;

		private int m_iTokenPrev = 258;

		private object m_objToken;

		private bool m_fPrevWasOperator;

		private bool m_fThisIsOperator;

		private static readonly Hashtable s_mapTokens = new Hashtable();

		private static readonly object[] s_rgTokenMap = new object[]
		{
			274,
			"and",
			276,
			"or",
			278,
			"div",
			280,
			"mod",
			296,
			"ancestor",
			298,
			"ancestor-or-self",
			300,
			"attribute",
			302,
			"child",
			304,
			"descendant",
			306,
			"descendant-or-self",
			308,
			"following",
			310,
			"following-sibling",
			312,
			"namespace",
			314,
			"parent",
			316,
			"preceding",
			318,
			"preceding-sibling",
			320,
			"self",
			322,
			"comment",
			324,
			"text",
			326,
			"processing-instruction",
			328,
			"node"
		};

		public Tokenizer(string strInput)
		{
			this.m_rgchInput = strInput;
			this.m_ich = 0;
			this.m_cch = strInput.Length;
			this.SkipWhitespace();
		}

		static Tokenizer()
		{
			for (int i = 0; i < Tokenizer.s_rgTokenMap.Length; i += 2)
			{
				Tokenizer.s_mapTokens.Add(Tokenizer.s_rgTokenMap[i + 1], Tokenizer.s_rgTokenMap[i]);
			}
		}

		private char Peek(int iOffset)
		{
			if (this.m_ich + iOffset >= this.m_cch)
			{
				return '\0';
			}
			return this.m_rgchInput[this.m_ich + iOffset];
		}

		private char Peek()
		{
			return this.Peek(0);
		}

		private char GetChar()
		{
			if (this.m_ich >= this.m_cch)
			{
				return '\0';
			}
			return this.m_rgchInput[this.m_ich++];
		}

		private char PutBack()
		{
			if (this.m_ich == 0)
			{
				throw new XPathException("XPath parser returned an error status: invalid tokenizer state.");
			}
			return this.m_rgchInput[--this.m_ich];
		}

		private bool SkipWhitespace()
		{
			if (!Tokenizer.IsWhitespace(this.Peek()))
			{
				return false;
			}
			while (Tokenizer.IsWhitespace(this.Peek()))
			{
				this.GetChar();
			}
			return true;
		}

		private int ParseNumber()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (Tokenizer.IsDigit(this.Peek()))
			{
				stringBuilder.Append(this.GetChar());
			}
			if (this.Peek() == '.')
			{
				stringBuilder.Append(this.GetChar());
				while (Tokenizer.IsDigit(this.Peek()))
				{
					stringBuilder.Append(this.GetChar());
				}
			}
			this.m_objToken = double.Parse(stringBuilder.ToString(), NumberFormatInfo.InvariantInfo);
			return 331;
		}

		private int ParseLiteral()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char @char = this.GetChar();
			char c;
			while ((c = this.Peek()) != @char)
			{
				if (c == '\0')
				{
					throw new XPathException("unmatched " + @char + " in expression");
				}
				stringBuilder.Append(this.GetChar());
			}
			this.GetChar();
			this.m_objToken = stringBuilder.ToString();
			return 332;
		}

		private string ReadIdentifier()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char c = this.Peek();
			if (!char.IsLetter(c) && c != '_')
			{
				return null;
			}
			stringBuilder.Append(this.GetChar());
			while ((c = this.Peek()) == '_' || c == '-' || c == '.' || char.IsLetterOrDigit(c))
			{
				stringBuilder.Append(this.GetChar());
			}
			this.SkipWhitespace();
			return stringBuilder.ToString();
		}

		private int ParseIdentifier()
		{
			string text = this.ReadIdentifier();
			object obj = Tokenizer.s_mapTokens[text];
			int num = (obj == null) ? 333 : ((int)obj);
			this.m_objToken = text;
			char c = this.Peek();
			if (c == ':')
			{
				if (this.Peek(1) == ':')
				{
					if (obj == null || !this.IsAxisName(num))
					{
						throw new XPathException("invalid axis name: '" + text + "'");
					}
					return num;
				}
				else
				{
					this.GetChar();
					this.SkipWhitespace();
					c = this.Peek();
					if (c == '*')
					{
						this.GetChar();
						this.m_objToken = new XmlQualifiedName(string.Empty, text);
						return 333;
					}
					string text2 = this.ReadIdentifier();
					if (text2 == null)
					{
						throw new XPathException(string.Concat(new object[]
						{
							"invalid QName: ",
							text,
							":",
							c
						}));
					}
					c = this.Peek();
					this.m_objToken = new XmlQualifiedName(text2, text);
					if (c == '(')
					{
						return 269;
					}
					return 333;
				}
			}
			else if (!this.IsFirstToken && !this.m_fPrevWasOperator)
			{
				if (obj == null || !this.IsOperatorName(num))
				{
					throw new XPathException("invalid operator name: '" + text + "'");
				}
				return num;
			}
			else
			{
				if (c != '(')
				{
					this.m_objToken = new XmlQualifiedName(text, string.Empty);
					return 333;
				}
				if (obj == null)
				{
					this.m_objToken = new XmlQualifiedName(text, string.Empty);
					return 269;
				}
				if (this.IsNodeType(num))
				{
					return num;
				}
				throw new XPathException("invalid function name: '" + text + "'");
			}
		}

		private static bool IsWhitespace(char ch)
		{
			return ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r';
		}

		private static bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		private int ParseToken()
		{
			char c = this.Peek();
			char c2 = c;
			switch (c2)
			{
			case '!':
				this.GetChar();
				if (this.Peek() == '=')
				{
					this.m_fThisIsOperator = true;
					this.GetChar();
					return 288;
				}
				break;
			case '"':
				return this.ParseLiteral();
			default:
				switch (c2)
				{
				case '[':
					this.m_fThisIsOperator = true;
					this.GetChar();
					return 270;
				default:
					if (c2 == '\0')
					{
						return 258;
					}
					if (c2 == '|')
					{
						this.m_fThisIsOperator = true;
						this.GetChar();
						return 286;
					}
					if (Tokenizer.IsDigit(c))
					{
						return this.ParseNumber();
					}
					if (char.IsLetter(c) || c == '_')
					{
						int num = this.ParseIdentifier();
						if (this.IsOperatorName(num))
						{
							this.m_fThisIsOperator = true;
						}
						return num;
					}
					break;
				case ']':
					this.GetChar();
					return 271;
				}
				break;
			case '$':
				this.GetChar();
				this.m_fThisIsOperator = true;
				return 285;
			case '\'':
				return this.ParseLiteral();
			case '(':
				this.m_fThisIsOperator = true;
				this.GetChar();
				return 272;
			case ')':
				this.GetChar();
				return 273;
			case '*':
				this.GetChar();
				if (!this.IsFirstToken && !this.m_fPrevWasOperator)
				{
					this.m_fThisIsOperator = true;
					return 330;
				}
				return 284;
			case '+':
				this.m_fThisIsOperator = true;
				this.GetChar();
				return 282;
			case ',':
				this.m_fThisIsOperator = true;
				this.GetChar();
				return 267;
			case '-':
				this.m_fThisIsOperator = true;
				this.GetChar();
				return 283;
			case '.':
				this.GetChar();
				if (this.Peek() == '.')
				{
					this.GetChar();
					return 263;
				}
				if (Tokenizer.IsDigit(this.Peek()))
				{
					this.PutBack();
					return this.ParseNumber();
				}
				return 262;
			case '/':
				this.m_fThisIsOperator = true;
				this.GetChar();
				if (this.Peek() == '/')
				{
					this.GetChar();
					return 260;
				}
				return 259;
			case ':':
				this.GetChar();
				if (this.Peek() == ':')
				{
					this.m_fThisIsOperator = true;
					this.GetChar();
					return 265;
				}
				return 257;
			case '<':
				this.m_fThisIsOperator = true;
				this.GetChar();
				if (this.Peek() == '=')
				{
					this.GetChar();
					return 290;
				}
				return 294;
			case '=':
				this.m_fThisIsOperator = true;
				this.GetChar();
				return 287;
			case '>':
				this.m_fThisIsOperator = true;
				this.GetChar();
				if (this.Peek() == '=')
				{
					this.GetChar();
					return 292;
				}
				return 295;
			case '@':
				this.m_fThisIsOperator = true;
				this.GetChar();
				return 268;
			}
			throw new XPathException("invalid token: '" + c + "'");
		}

		public bool advance()
		{
			this.m_fThisIsOperator = false;
			this.m_objToken = null;
			this.m_iToken = this.ParseToken();
			this.SkipWhitespace();
			this.m_iTokenPrev = this.m_iToken;
			this.m_fPrevWasOperator = this.m_fThisIsOperator;
			return this.m_iToken != 258;
		}

		public int token()
		{
			return this.m_iToken;
		}

		public object value()
		{
			return this.m_objToken;
		}

		private bool IsFirstToken
		{
			get
			{
				return this.m_iTokenPrev == 258;
			}
		}

		private bool IsNodeType(int iToken)
		{
			switch (iToken)
			{
			case 322:
			case 324:
			case 326:
			case 328:
				return true;
			}
			return false;
		}

		private bool IsOperatorName(int iToken)
		{
			switch (iToken)
			{
			case 274:
			case 276:
			case 278:
			case 280:
				return true;
			}
			return false;
		}

		private bool IsAxisName(int iToken)
		{
			switch (iToken)
			{
			case 296:
			case 298:
			case 300:
			case 302:
			case 304:
			case 306:
			case 308:
			case 310:
			case 312:
			case 314:
			case 316:
			case 318:
			case 320:
				return true;
			}
			return false;
		}
	}
}
