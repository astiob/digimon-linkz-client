using System;
using System.Text;

namespace System.Xml.XPath
{
	internal class XPathFunctionNormalizeSpace : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionNormalizeSpace(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("normalize-space takes 1 or zero args");
				}
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.String;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0 == null || this.arg0.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			string text;
			if (this.arg0 != null)
			{
				text = this.arg0.EvaluateString(iter);
			}
			else
			{
				text = iter.Current.Value;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (char c in text)
			{
				if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
				{
					flag = true;
				}
				else
				{
					if (flag)
					{
						flag = false;
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(' ');
						}
					}
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"normalize-space(",
				(this.arg0 == null) ? string.Empty : this.arg0.ToString(),
				")"
			});
		}
	}
}
