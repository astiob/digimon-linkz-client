using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionSubstringBefore : XPathFunction
	{
		private Expression arg0;

		private Expression arg1;

		public XPathFunctionSubstringBefore(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail == null || args.Tail.Tail != null)
			{
				throw new XPathException("substring-before takes 2 args");
			}
			this.arg0 = args.Arg;
			this.arg1 = args.Tail.Arg;
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
				return this.arg0.Peer && this.arg1.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			string text = this.arg0.EvaluateString(iter);
			string value = this.arg1.EvaluateString(iter);
			int num = text.IndexOf(value);
			if (num <= 0)
			{
				return string.Empty;
			}
			return text.Substring(0, num);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"substring-before(",
				this.arg0.ToString(),
				",",
				this.arg1.ToString(),
				")"
			});
		}
	}
}
