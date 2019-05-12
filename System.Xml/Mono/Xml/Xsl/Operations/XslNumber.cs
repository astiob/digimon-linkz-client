using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslNumber : XslCompiledElement
	{
		private XslNumberingLevel level;

		private Pattern count;

		private Pattern from;

		private XPathExpression value;

		private XslAvt format;

		private XslAvt lang;

		private XslAvt letterValue;

		private XslAvt groupingSeparator;

		private XslAvt groupingSize;

		public XslNumber(Compiler c) : base(c)
		{
		}

		public static double Round(double n)
		{
			double num = Math.Floor(n);
			return (n - num < 0.5) ? num : (num + 1.0);
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(base.DebugInput);
			}
			c.CheckExtraAttributes("number", new string[]
			{
				"level",
				"count",
				"from",
				"value",
				"format",
				"lang",
				"letter-value",
				"grouping-separator",
				"grouping-size"
			});
			string attribute = c.GetAttribute("level");
			switch (attribute)
			{
			case "single":
				this.level = XslNumberingLevel.Single;
				goto IL_12C;
			case "multiple":
				this.level = XslNumberingLevel.Multiple;
				goto IL_12C;
			case "any":
				this.level = XslNumberingLevel.Any;
				goto IL_12C;
			}
			this.level = XslNumberingLevel.Single;
			IL_12C:
			this.count = c.CompilePattern(c.GetAttribute("count"), c.Input);
			this.from = c.CompilePattern(c.GetAttribute("from"), c.Input);
			this.value = c.CompileExpression(c.GetAttribute("value"));
			this.format = c.ParseAvtAttribute("format");
			this.lang = c.ParseAvtAttribute("lang");
			this.letterValue = c.ParseAvtAttribute("letter-value");
			this.groupingSeparator = c.ParseAvtAttribute("grouping-separator");
			this.groupingSize = c.ParseAvtAttribute("grouping-size");
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			string text = this.GetFormat(p);
			if (text != string.Empty)
			{
				p.Out.WriteString(text);
			}
		}

		private XslNumber.XslNumberFormatter GetNumberFormatter(XslTransformProcessor p)
		{
			string text = "1";
			string text2 = null;
			string text3 = null;
			char c = '\0';
			decimal d = 0m;
			if (this.format != null)
			{
				text = this.format.Evaluate(p);
			}
			if (this.lang != null)
			{
				text2 = this.lang.Evaluate(p);
			}
			if (this.letterValue != null)
			{
				text3 = this.letterValue.Evaluate(p);
			}
			if (this.groupingSeparator != null)
			{
				c = this.groupingSeparator.Evaluate(p)[0];
			}
			if (this.groupingSize != null)
			{
				d = decimal.Parse(this.groupingSize.Evaluate(p), CultureInfo.InvariantCulture);
			}
			if (d > 2147483647m || d < 1m)
			{
				d = 0m;
			}
			return new XslNumber.XslNumberFormatter(text, text2, text3, c, (int)d);
		}

		private string GetFormat(XslTransformProcessor p)
		{
			XslNumber.XslNumberFormatter numberFormatter = this.GetNumberFormatter(p);
			if (this.value != null)
			{
				double num = p.EvaluateNumber(this.value);
				return numberFormatter.Format(num);
			}
			switch (this.level)
			{
			case XslNumberingLevel.Single:
			{
				int num2 = this.NumberSingle(p);
				return numberFormatter.Format((double)num2, num2 != 0);
			}
			case XslNumberingLevel.Multiple:
				return numberFormatter.Format(this.NumberMultiple(p));
			case XslNumberingLevel.Any:
			{
				int num2 = this.NumberAny(p);
				return numberFormatter.Format((double)num2, num2 != 0);
			}
			default:
				throw new XsltException("Should not get here", null, p.CurrentNode);
			}
		}

		private int[] NumberMultiple(XslTransformProcessor p)
		{
			ArrayList arrayList = new ArrayList();
			XPathNavigator xpathNavigator = p.CurrentNode.Clone();
			bool flag = false;
			while (!this.MatchesFrom(xpathNavigator, p))
			{
				if (this.MatchesCount(xpathNavigator, p))
				{
					int num = 1;
					while (xpathNavigator.MoveToPrevious())
					{
						if (this.MatchesCount(xpathNavigator, p))
						{
							num++;
						}
					}
					arrayList.Add(num);
				}
				if (!xpathNavigator.MoveToParent())
				{
					IL_70:
					if (!flag)
					{
						return new int[0];
					}
					int[] array = new int[arrayList.Count];
					int num2 = arrayList.Count;
					for (int i = 0; i < arrayList.Count; i++)
					{
						array[--num2] = (int)arrayList[i];
					}
					return array;
				}
			}
			flag = true;
			goto IL_70;
		}

		private int NumberAny(XslTransformProcessor p)
		{
			int num = 0;
			XPathNavigator xpathNavigator = p.CurrentNode.Clone();
			xpathNavigator.MoveToRoot();
			bool flag = this.from == null;
			for (;;)
			{
				if (this.from != null && this.MatchesFrom(xpathNavigator, p))
				{
					flag = true;
					num = 0;
				}
				else if (flag && this.MatchesCount(xpathNavigator, p))
				{
					num++;
				}
				if (xpathNavigator.IsSamePosition(p.CurrentNode))
				{
					break;
				}
				if (!xpathNavigator.MoveToFirstChild())
				{
					while (!xpathNavigator.MoveToNext())
					{
						if (!xpathNavigator.MoveToParent())
						{
							return 0;
						}
					}
				}
			}
			return num;
		}

		private int NumberSingle(XslTransformProcessor p)
		{
			XPathNavigator xpathNavigator = p.CurrentNode.Clone();
			while (!this.MatchesCount(xpathNavigator, p))
			{
				if (this.from != null && this.MatchesFrom(xpathNavigator, p))
				{
					return 0;
				}
				if (!xpathNavigator.MoveToParent())
				{
					return 0;
				}
			}
			if (this.from != null)
			{
				XPathNavigator xpathNavigator2 = xpathNavigator.Clone();
				if (this.MatchesFrom(xpathNavigator2, p))
				{
					return 0;
				}
				bool flag = false;
				while (xpathNavigator2.MoveToParent())
				{
					if (this.MatchesFrom(xpathNavigator2, p))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return 0;
				}
			}
			int num = 1;
			while (xpathNavigator.MoveToPrevious())
			{
				if (this.MatchesCount(xpathNavigator, p))
				{
					num++;
				}
			}
			return num;
		}

		private bool MatchesCount(XPathNavigator item, XslTransformProcessor p)
		{
			if (this.count == null)
			{
				return item.NodeType == p.CurrentNode.NodeType && item.LocalName == p.CurrentNode.LocalName && item.NamespaceURI == p.CurrentNode.NamespaceURI;
			}
			return p.Matches(this.count, item);
		}

		private bool MatchesFrom(XPathNavigator item, XslTransformProcessor p)
		{
			if (this.from == null)
			{
				return item.NodeType == XPathNodeType.Root;
			}
			return p.Matches(this.from, item);
		}

		private class XslNumberFormatter
		{
			private string firstSep;

			private string lastSep;

			private ArrayList fmtList = new ArrayList();

			public XslNumberFormatter(string format, string lang, string letterValue, char groupingSeparator, int groupingSize)
			{
				if (format == null || format == string.Empty)
				{
					this.fmtList.Add(XslNumber.XslNumberFormatter.FormatItem.GetItem(null, "1", groupingSeparator, groupingSize));
				}
				else
				{
					XslNumber.XslNumberFormatter.NumberFormatterScanner numberFormatterScanner = new XslNumber.XslNumberFormatter.NumberFormatterScanner(format);
					this.firstSep = numberFormatterScanner.Advance(false);
					string text = numberFormatterScanner.Advance(true);
					if (text == null)
					{
						string sep = this.firstSep;
						this.firstSep = null;
						this.fmtList.Add(XslNumber.XslNumberFormatter.FormatItem.GetItem(sep, "1", groupingSeparator, groupingSize));
					}
					else
					{
						this.fmtList.Add(XslNumber.XslNumberFormatter.FormatItem.GetItem(".", text, groupingSeparator, groupingSize));
						string sep;
						for (;;)
						{
							sep = numberFormatterScanner.Advance(false);
							text = numberFormatterScanner.Advance(true);
							if (text == null)
							{
								break;
							}
							this.fmtList.Add(XslNumber.XslNumberFormatter.FormatItem.GetItem(sep, text, groupingSeparator, groupingSize));
							if (text == null)
							{
								return;
							}
						}
						this.lastSep = sep;
					}
				}
			}

			public string Format(double value)
			{
				return this.Format(value, true);
			}

			public string Format(double value, bool formatContent)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.firstSep != null)
				{
					stringBuilder.Append(this.firstSep);
				}
				if (formatContent)
				{
					((XslNumber.XslNumberFormatter.FormatItem)this.fmtList[0]).Format(stringBuilder, value);
				}
				if (this.lastSep != null)
				{
					stringBuilder.Append(this.lastSep);
				}
				return stringBuilder.ToString();
			}

			public string Format(int[] values)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.firstSep != null)
				{
					stringBuilder.Append(this.firstSep);
				}
				int num = 0;
				int num2 = this.fmtList.Count - 1;
				if (values.Length > 0)
				{
					if (this.fmtList.Count > 0)
					{
						XslNumber.XslNumberFormatter.FormatItem formatItem = (XslNumber.XslNumberFormatter.FormatItem)this.fmtList[num];
						formatItem.Format(stringBuilder, (double)values[0]);
					}
					if (num < num2)
					{
						num++;
					}
				}
				for (int i = 1; i < values.Length; i++)
				{
					XslNumber.XslNumberFormatter.FormatItem formatItem2 = (XslNumber.XslNumberFormatter.FormatItem)this.fmtList[num];
					stringBuilder.Append(formatItem2.sep);
					int num3 = values[i];
					formatItem2.Format(stringBuilder, (double)num3);
					if (num < num2)
					{
						num++;
					}
				}
				if (this.lastSep != null)
				{
					stringBuilder.Append(this.lastSep);
				}
				return stringBuilder.ToString();
			}

			private class NumberFormatterScanner
			{
				private int pos;

				private int len;

				private string fmt;

				public NumberFormatterScanner(string fmt)
				{
					this.fmt = fmt;
					this.len = fmt.Length;
				}

				public string Advance(bool alphaNum)
				{
					int num = this.pos;
					while (this.pos < this.len && char.IsLetterOrDigit(this.fmt, this.pos) == alphaNum)
					{
						this.pos++;
					}
					if (this.pos == num)
					{
						return null;
					}
					return this.fmt.Substring(num, this.pos - num);
				}
			}

			private abstract class FormatItem
			{
				public readonly string sep;

				public FormatItem(string sep)
				{
					this.sep = sep;
				}

				public abstract void Format(StringBuilder b, double num);

				public static XslNumber.XslNumberFormatter.FormatItem GetItem(string sep, string item, char gpSep, int gpSize)
				{
					char c = item[0];
					if (c == '0' || c == '1')
					{
						int i;
						for (i = 1; i < item.Length; i++)
						{
							if (!char.IsDigit(item, i))
							{
								break;
							}
						}
						return new XslNumber.XslNumberFormatter.DigitItem(sep, i, gpSep, gpSize);
					}
					if (c == 'A')
					{
						return new XslNumber.XslNumberFormatter.AlphaItem(sep, true);
					}
					if (c == 'I')
					{
						return new XslNumber.XslNumberFormatter.RomanItem(sep, true);
					}
					if (c == 'a')
					{
						return new XslNumber.XslNumberFormatter.AlphaItem(sep, false);
					}
					if (c != 'i')
					{
						return new XslNumber.XslNumberFormatter.DigitItem(sep, 1, gpSep, gpSize);
					}
					return new XslNumber.XslNumberFormatter.RomanItem(sep, false);
				}
			}

			private class AlphaItem : XslNumber.XslNumberFormatter.FormatItem
			{
				private bool uc;

				private static readonly char[] ucl = new char[]
				{
					'A',
					'B',
					'C',
					'D',
					'E',
					'F',
					'G',
					'H',
					'I',
					'J',
					'K',
					'L',
					'M',
					'N',
					'O',
					'P',
					'Q',
					'R',
					'S',
					'T',
					'U',
					'V',
					'W',
					'X',
					'Y',
					'Z'
				};

				private static readonly char[] lcl = new char[]
				{
					'a',
					'b',
					'c',
					'd',
					'e',
					'f',
					'g',
					'h',
					'i',
					'j',
					'k',
					'l',
					'm',
					'n',
					'o',
					'p',
					'q',
					'r',
					's',
					't',
					'u',
					'v',
					'w',
					'x',
					'y',
					'z'
				};

				public AlphaItem(string sep, bool uc) : base(sep)
				{
					this.uc = uc;
				}

				public override void Format(StringBuilder b, double num)
				{
					XslNumber.XslNumberFormatter.AlphaItem.alphaSeq(b, num, (!this.uc) ? XslNumber.XslNumberFormatter.AlphaItem.lcl : XslNumber.XslNumberFormatter.AlphaItem.ucl);
				}

				private static void alphaSeq(StringBuilder b, double n, char[] alphabet)
				{
					n = XslNumber.Round(n);
					if (n == 0.0)
					{
						return;
					}
					if (n > (double)alphabet.Length)
					{
						XslNumber.XslNumberFormatter.AlphaItem.alphaSeq(b, Math.Floor((n - 1.0) / (double)alphabet.Length), alphabet);
					}
					b.Append(alphabet[((int)n - 1) % alphabet.Length]);
				}
			}

			private class RomanItem : XslNumber.XslNumberFormatter.FormatItem
			{
				private bool uc;

				private static readonly string[] ucrDigits = new string[]
				{
					"M",
					"CM",
					"D",
					"CD",
					"C",
					"XC",
					"L",
					"XL",
					"X",
					"IX",
					"V",
					"IV",
					"I"
				};

				private static readonly string[] lcrDigits = new string[]
				{
					"m",
					"cm",
					"d",
					"cd",
					"c",
					"xc",
					"l",
					"xl",
					"x",
					"ix",
					"v",
					"iv",
					"i"
				};

				private static readonly int[] decValues = new int[]
				{
					1000,
					900,
					500,
					400,
					100,
					90,
					50,
					40,
					10,
					9,
					5,
					4,
					1
				};

				public RomanItem(string sep, bool uc) : base(sep)
				{
					this.uc = uc;
				}

				public override void Format(StringBuilder b, double num)
				{
					if (num < 1.0 || num > 4999.0)
					{
						b.Append(num);
						return;
					}
					num = XslNumber.Round(num);
					for (int i = 0; i < XslNumber.XslNumberFormatter.RomanItem.decValues.Length; i++)
					{
						while ((double)XslNumber.XslNumberFormatter.RomanItem.decValues[i] <= num)
						{
							if (this.uc)
							{
								b.Append(XslNumber.XslNumberFormatter.RomanItem.ucrDigits[i]);
							}
							else
							{
								b.Append(XslNumber.XslNumberFormatter.RomanItem.lcrDigits[i]);
							}
							num -= (double)XslNumber.XslNumberFormatter.RomanItem.decValues[i];
						}
						if (num == 0.0)
						{
							break;
						}
					}
				}
			}

			private class DigitItem : XslNumber.XslNumberFormatter.FormatItem
			{
				private NumberFormatInfo nfi;

				private int decimalSectionLength;

				private StringBuilder numberBuilder;

				public DigitItem(string sep, int len, char gpSep, int gpSize) : base(sep)
				{
					this.nfi = new NumberFormatInfo();
					this.nfi.NumberDecimalDigits = 0;
					this.nfi.NumberGroupSizes = new int[1];
					if (gpSep != '\0' && gpSize > 0)
					{
						this.nfi.NumberGroupSeparator = gpSep.ToString();
						this.nfi.NumberGroupSizes = new int[]
						{
							gpSize
						};
					}
					this.decimalSectionLength = len;
				}

				public override void Format(StringBuilder b, double num)
				{
					string text = num.ToString("N", this.nfi);
					int num2 = this.decimalSectionLength;
					if (num2 > 1)
					{
						if (this.numberBuilder == null)
						{
							this.numberBuilder = new StringBuilder();
						}
						for (int i = num2; i > text.Length; i--)
						{
							this.numberBuilder.Append('0');
						}
						this.numberBuilder.Append((text.Length > num2) ? text.Substring(text.Length - num2, num2) : text);
						text = this.numberBuilder.ToString();
						this.numberBuilder.Length = 0;
					}
					b.Append(text);
				}
			}
		}
	}
}
