using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XslDecimalFormat
	{
		private NumberFormatInfo info = new NumberFormatInfo();

		private char digit = '#';

		private char zeroDigit = '0';

		private char patternSeparator = ';';

		private string baseUri;

		private int lineNumber;

		private int linePosition;

		public static readonly XslDecimalFormat Default = new XslDecimalFormat();

		private XslDecimalFormat()
		{
		}

		public XslDecimalFormat(Compiler c)
		{
			XPathNavigator input = c.Input;
			IXmlLineInfo xmlLineInfo = input as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				this.lineNumber = xmlLineInfo.LineNumber;
				this.linePosition = xmlLineInfo.LinePosition;
			}
			this.baseUri = input.BaseURI;
			if (input.MoveToFirstAttribute())
			{
				for (;;)
				{
					if (!(input.NamespaceURI != string.Empty))
					{
						string localName = input.LocalName;
						switch (localName)
						{
						case "decimal-separator":
							if (input.Value.Length != 1)
							{
								goto Block_7;
							}
							this.info.NumberDecimalSeparator = input.Value;
							break;
						case "grouping-separator":
							if (input.Value.Length != 1)
							{
								goto Block_8;
							}
							this.info.NumberGroupSeparator = input.Value;
							break;
						case "infinity":
							this.info.PositiveInfinitySymbol = input.Value;
							break;
						case "minus-sign":
							if (input.Value.Length != 1)
							{
								goto Block_9;
							}
							this.info.NegativeSign = input.Value;
							break;
						case "NaN":
							this.info.NaNSymbol = input.Value;
							break;
						case "percent":
							if (input.Value.Length != 1)
							{
								goto Block_10;
							}
							this.info.PercentSymbol = input.Value;
							break;
						case "per-mille":
							if (input.Value.Length != 1)
							{
								goto Block_11;
							}
							this.info.PerMilleSymbol = input.Value;
							break;
						case "digit":
							if (input.Value.Length != 1)
							{
								goto Block_12;
							}
							this.digit = input.Value[0];
							break;
						case "zero-digit":
							if (input.Value.Length != 1)
							{
								goto Block_13;
							}
							this.zeroDigit = input.Value[0];
							break;
						case "pattern-separator":
							if (input.Value.Length != 1)
							{
								goto Block_14;
							}
							this.patternSeparator = input.Value[0];
							break;
						}
					}
					if (!input.MoveToNextAttribute())
					{
						goto Block_15;
					}
				}
				Block_7:
				throw new XsltCompileException("XSLT decimal-separator value must be exact one character", null, input);
				Block_8:
				throw new XsltCompileException("XSLT grouping-separator value must be exact one character", null, input);
				Block_9:
				throw new XsltCompileException("XSLT minus-sign value must be exact one character", null, input);
				Block_10:
				throw new XsltCompileException("XSLT percent value must be exact one character", null, input);
				Block_11:
				throw new XsltCompileException("XSLT per-mille value must be exact one character", null, input);
				Block_12:
				throw new XsltCompileException("XSLT digit value must be exact one character", null, input);
				Block_13:
				throw new XsltCompileException("XSLT zero-digit value must be exact one character", null, input);
				Block_14:
				throw new XsltCompileException("XSLT pattern-separator value must be exact one character", null, input);
				Block_15:
				input.MoveToParent();
				this.info.NegativeInfinitySymbol = this.info.NegativeSign + this.info.PositiveInfinitySymbol;
			}
		}

		public char Digit
		{
			get
			{
				return this.digit;
			}
		}

		public char ZeroDigit
		{
			get
			{
				return this.zeroDigit;
			}
		}

		public NumberFormatInfo Info
		{
			get
			{
				return this.info;
			}
		}

		public char PatternSeparator
		{
			get
			{
				return this.patternSeparator;
			}
		}

		public void CheckSameAs(XslDecimalFormat other)
		{
			if (this.digit != other.digit || this.patternSeparator != other.patternSeparator || this.zeroDigit != other.zeroDigit || this.info.NumberDecimalSeparator != other.info.NumberDecimalSeparator || this.info.NumberGroupSeparator != other.info.NumberGroupSeparator || this.info.PositiveInfinitySymbol != other.info.PositiveInfinitySymbol || this.info.NegativeSign != other.info.NegativeSign || this.info.NaNSymbol != other.info.NaNSymbol || this.info.PercentSymbol != other.info.PercentSymbol || this.info.PerMilleSymbol != other.info.PerMilleSymbol)
			{
				throw new XsltCompileException(null, other.baseUri, other.lineNumber, other.linePosition);
			}
		}

		public string FormatNumber(double number, string pattern)
		{
			return this.ParsePatternSet(pattern).FormatNumber(number);
		}

		private DecimalFormatPatternSet ParsePatternSet(string pattern)
		{
			return new DecimalFormatPatternSet(pattern, this);
		}
	}
}
