using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionStringLength : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionStringLength(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("string-length takes 1 or zero args");
				}
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
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
			return (double)text.Length;
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"string-length(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
