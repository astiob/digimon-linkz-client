using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslAvt
	{
		private string simpleString;

		private ArrayList avtParts;

		public XslAvt(string str, Compiler comp)
		{
			if (str.IndexOf("{") == -1 && str.IndexOf("}") == -1)
			{
				this.simpleString = str;
				return;
			}
			this.avtParts = new ArrayList();
			StringBuilder stringBuilder = new StringBuilder();
			StringReader stringReader = new StringReader(str);
			while (stringReader.Peek() != -1)
			{
				char c = (char)stringReader.Read();
				switch (c)
				{
				case '{':
					if ((ushort)stringReader.Peek() == 123)
					{
						stringBuilder.Append((char)stringReader.Read());
						continue;
					}
					if (stringBuilder.Length != 0)
					{
						this.avtParts.Add(new XslAvt.SimpleAvtPart(stringBuilder.ToString()));
						stringBuilder.Length = 0;
					}
					while ((c = (char)stringReader.Read()) != '}')
					{
						char c2 = c;
						if (c2 != '"' && c2 != '\'')
						{
							stringBuilder.Append(c);
						}
						else
						{
							char c3 = c;
							stringBuilder.Append(c);
							while ((c = (char)stringReader.Read()) != c3)
							{
								stringBuilder.Append(c);
								if (stringReader.Peek() == -1)
								{
									throw new XsltCompileException("Unexpected end of AVT", null, comp.Input);
								}
							}
							stringBuilder.Append(c);
						}
						if (stringReader.Peek() == -1)
						{
							throw new XsltCompileException("Unexpected end of AVT", null, comp.Input);
						}
					}
					this.avtParts.Add(new XslAvt.XPathAvtPart(comp.CompileExpression(stringBuilder.ToString())));
					stringBuilder.Length = 0;
					continue;
				case '}':
					c = (char)stringReader.Read();
					if (c != '}')
					{
						throw new XsltCompileException("Braces must be escaped", null, comp.Input);
					}
					break;
				}
				stringBuilder.Append(c);
			}
			if (stringBuilder.Length != 0)
			{
				this.avtParts.Add(new XslAvt.SimpleAvtPart(stringBuilder.ToString()));
				stringBuilder.Length = 0;
			}
		}

		public static string AttemptPreCalc(ref XslAvt avt)
		{
			if (avt == null)
			{
				return null;
			}
			if (avt.simpleString != null)
			{
				string result = avt.simpleString;
				avt = null;
				return result;
			}
			return null;
		}

		public string Evaluate(XslTransformProcessor p)
		{
			if (this.simpleString != null)
			{
				return this.simpleString;
			}
			if (this.avtParts.Count == 1)
			{
				return ((XslAvt.AvtPart)this.avtParts[0]).Evaluate(p);
			}
			StringBuilder avtStringBuilder = p.GetAvtStringBuilder();
			int count = this.avtParts.Count;
			for (int i = 0; i < count; i++)
			{
				avtStringBuilder.Append(((XslAvt.AvtPart)this.avtParts[i]).Evaluate(p));
			}
			return p.ReleaseAvtStringBuilder();
		}

		private abstract class AvtPart
		{
			public abstract string Evaluate(XslTransformProcessor p);
		}

		private sealed class SimpleAvtPart : XslAvt.AvtPart
		{
			private string val;

			public SimpleAvtPart(string val)
			{
				this.val = val;
			}

			public override string Evaluate(XslTransformProcessor p)
			{
				return this.val;
			}
		}

		private sealed class XPathAvtPart : XslAvt.AvtPart
		{
			private XPathExpression expr;

			public XPathAvtPart(XPathExpression expr)
			{
				this.expr = expr;
			}

			public override string Evaluate(XslTransformProcessor p)
			{
				return p.EvaluateString(this.expr);
			}
		}
	}
}
