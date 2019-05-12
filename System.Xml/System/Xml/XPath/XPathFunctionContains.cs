using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionContains : XPathFunction
	{
		private Expression arg0;

		private Expression arg1;

		public XPathFunctionContains(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail == null || args.Tail.Tail != null)
			{
				throw new XPathException("contains takes 2 args");
			}
			this.arg0 = args.Arg;
			this.arg1 = args.Tail.Arg;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Boolean;
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
			return this.arg0.EvaluateString(iter).IndexOf(this.arg1.EvaluateString(iter)) != -1;
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"contains(",
				this.arg0.ToString(),
				",",
				this.arg1.ToString(),
				")"
			});
		}
	}
}
